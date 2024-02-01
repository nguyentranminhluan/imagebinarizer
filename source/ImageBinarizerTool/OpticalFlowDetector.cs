using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.ML;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Daenet.ImageBinarizerApp
{
    public class OpticalFlowDetector
    {




        public void Process(string input, string output)
        {
            List<(double Timestamp, float MeanX, float peak)> lowestMeanXList = new List<(double, float, float)>();

            // The video feed is read in as a VideoCapture object
            using (var cap = new VideoCapture(input))
            {
                Mat firstFrame = cap.QueryFrame();
                Mat prevGray = new Mat();
                CvInvoke.CvtColor(firstFrame, prevGray, ColorConversion.Bgr2Gray);
                Mat frame = new Mat();
                Mat grayFrame = new Mat();
                Mat flow = new Mat();

                Mat mask = new Mat(firstFrame.Size, firstFrame.Depth, firstFrame.NumberOfChannels);

                mask.SetTo(new MCvScalar(0, 255, 0));

                int frameWidth = (int)cap.Get(CapProp.FrameWidth);
                int frameHeight = (int)cap.Get(CapProp.FrameHeight);
                int fps = (int)cap.Get(CapProp.Fps);

                VideoWriter videoWriter = new VideoWriter(output, fps, new System.Drawing.Size(frameWidth, frameHeight), true);
                int i = 0;

                int leftToRightChanges = 0;
                int rightToLeftChanges = 0;

                while (true)
                {
                    frame = cap.QueryFrame();
                    if (frame == null)
                    {
                        Console.WriteLine("Done");

                        break;
                    }

                    CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);





                    CvInvoke.CalcOpticalFlowFarneback(prevGray, grayFrame, flow, 0.5, 3, 15, 3, 5, 1.1, 0);

                    //magnitude, angle = cv.cartToPolar(flow[..., 0], flow[..., 1])
                    Mat[] polarComponents = new Mat[2];
                    polarComponents[0] = new Mat();
                    polarComponents[1] = new Mat();


                    CvInvoke.CartToPolar(flow.Split()[0], flow.Split()[1], polarComponents[0], polarComponents[1], true);



                    VectorOfMat splitted = new VectorOfMat();
                    CvInvoke.Split(mask, splitted);


                    var multiplyArray = new ScalarArray(0.5);



                    CvInvoke.Multiply(polarComponents[1], multiplyArray, splitted[0]);


                    CvInvoke.Normalize(polarComponents[0], splitted[2], 0, 255, NormType.MinMax);

                    splitted[1].ConvertTo(splitted[1], DepthType.Cv32F);
                    ;

                    CvInvoke.Merge(splitted, mask);

                    int gridRows = 25;
                    int gridCols = 25;
                    int squareWidth = frame.Width / gridCols;
                    int squareHeight = frame.Height / gridRows;
                    float totalMeanX = 0;
                    int count = 0;
                    float peakValue = 0;

                    // Draw arrow vectors for each square
                    for (int row = gridRows / 6; row < gridRows / 2; row++)
                    {
                        for (int col = gridCols / 5; col < gridCols; col++)
                        {
                            int startX = col * squareWidth;
                            int startY = row * squareHeight;
                            int endX = startX + squareWidth;
                            int endY = startY + squareHeight;

                            // Extract the optical flow for the current square
                            Rectangle rect = new Rectangle(startX, startY, squareWidth, squareHeight);
                            Mat squareFlow = new Mat(flow, rect);

                            MCvScalar meanScalar = CvInvoke.Mean(squareFlow);


                            // Calculate mean direction for the square
                            float meanX = (float)meanScalar.V0;  // Access the mean value in X direction
                            float meanY = (float)meanScalar.V1;  // Access the mean value in Y direction

                            if (Math.Abs(meanX) > peakValue)
                            {
                                peakValue = Math.Abs(meanX);
                            }
                            totalMeanX += Math.Abs(meanX);
                            count++;

                            //
                            //Get the times of meanX lowest

                            PointF startPoint = new PointF(startX + squareWidth / 2, startY + squareHeight / 2);
                            PointF endPoint = new PointF(startPoint.X + meanX * 10, startPoint.Y);

                            CvInvoke.ArrowedLine(frame, Point.Round(startPoint), Point.Round(endPoint), new MCvScalar(255, 255, 255), 1);
                        }
                    }

                    float frameMeanX = totalMeanX / count;
                    lowestMeanXList.Add((cap.Get(CapProp.PosMsec) / 1000.0, frameMeanX, peakValue));

                    i++;
                    Mat result = new Mat();
                    videoWriter.Write(frame);
                    lowestMeanXList = lowestMeanXList.Where(item => item.Timestamp >= 1).ToList();

                    lowestMeanXList = lowestMeanXList.Where(peak => peak.peak < 0.45).ToList();

                    lowestMeanXList = lowestMeanXList.GroupBy(entry => (int)entry.Timestamp)
                                       .Select(group => group.OrderBy(entry => entry.MeanX).First())
                                       .ToList();
                    lowestMeanXList = lowestMeanXList.OrderBy(item => item.MeanX).ToList();

                    lowestMeanXList = lowestMeanXList.OrderBy(entry => (int)entry.Timestamp)
                                      .GroupByConsecutive(entry => (int)entry.Timestamp + 1)
                                      .Select(group => group.OrderBy(entry => entry.MeanX).First())
                                      .ToList();
                    grayFrame.CopyTo(prevGray);



                } // The using stat
                cap.Dispose();
                videoWriter.Dispose();
                using (StreamWriter writer = new StreamWriter(Path.ChangeExtension(output, "txt")))
                {
                    foreach (var item in lowestMeanXList)
                    {
                        writer.WriteLine($"Timestamp: {item.Timestamp}, Min MeanX: {item.MeanX}, PeakValue: {item.peak}");

                    }
                    writer.Dispose();

                }
            }

        }

    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> GroupByConsecutive<T>(
            this IEnumerable<T> source, Func<T, double> keySelector)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                var consecutiveGroup = new List<T> { enumerator.Current };
                var previousKey = keySelector(enumerator.Current);

                while (enumerator.MoveNext())
                {
                    var currentKey = keySelector(enumerator.Current);

                    if (Math.Abs(currentKey - previousKey) > 5)
                    {
                        yield return consecutiveGroup;
                        consecutiveGroup = new List<T>();
                    }

                    consecutiveGroup.Add(enumerator.Current);
                    previousKey = currentKey;
                }

                yield return consecutiveGroup;
            }
        }
    }
}
