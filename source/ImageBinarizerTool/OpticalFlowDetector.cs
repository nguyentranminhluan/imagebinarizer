using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Daenet.ImageBinarizerApp
{
    public class OpticalFlowDetector
    {




        public void Process()
        {
            // The video feed is read in as a VideoCapture object
            using (var cap = new VideoCapture($@"D:\test for heckler and koch\videotest1.wmv"))
            {
                Mat firstFrame = cap.QueryFrame();
                Mat prevGray = new Mat();
                CvInvoke.CvtColor(firstFrame, prevGray, ColorConversion.Bgr2Gray);
                Mat frame = new Mat();
                Mat grayFrame = new Mat();
                Mat flow = new Mat();

                Mat mask = new Mat(firstFrame.Size, firstFrame.Depth, firstFrame.NumberOfChannels);
                mask.SetTo(new MCvScalar(0, 0, 0));

                mask.SetTo(new MCvScalar(0, 255, 0), null);

                int frameWidth = (int)cap.Get(CapProp.FrameWidth);
                int frameHeight = (int)cap.Get(CapProp.FrameHeight);
                int fps = (int)cap.Get(CapProp.Fps);
                VideoWriter videoWriter = new VideoWriter($@"D:\test for heckler and koch/result1.wmv", fps, new System.Drawing.Size(frameWidth, frameHeight), true);




                while (true)
                {
                    frame = cap.QueryFrame();
                    if (frame == null)
                        break;

                    CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);
                    CvInvoke.CalcOpticalFlowFarneback(prevGray, grayFrame, flow, 0.5, 3, 15, 3, 5, 1.2, 0);

                    //magnitude, angle = cv.cartToPolar(flow[..., 0], flow[..., 1])
                    Mat[] polarComponents = new Mat[2];
                    polarComponents[0] = new Mat();
                    polarComponents[1] = new Mat();


                    CvInvoke.CartToPolar(flow.Split()[0], flow.Split()[1], polarComponents[0], polarComponents[1], true);

                    //VectorOfMat splitted = new VectorOfMat(3);

                    int dim1 = 288;
                    int dim2 = 788;
                    int dim3 = 3;
                    VectorOfVectorOfByte splitted = new VectorOfVectorOfByte(
                        byte[][][] matrix = new byte[dim1][][]);
                    CvInvoke.Split(mask,  splitted);





                    //CvInvoke.Multiply(polarComponents[1], new UMat(polarComponents[1].Size, polarComponents[1].Depth, 1), splitted[0], 180.0 / Math.PI / 2.0); ;

                    CvInvoke.Multiply(polarComponents[1], new ScalarArray(180.0 / Math.PI / 2.0), splitted[0]);


                    CvInvoke.Normalize(polarComponents[0], splitted[2], 0, 255, NormType.MinMax);
;
                    CvInvoke.Merge(splitted, mask);

                    var k = mask.NumberOfChannels;






                    Mat result = new Mat();
                    CvInvoke.CvtColor(mask, result, ColorConversion.Hsv2Bgr);

                    videoWriter.Write(result);





                    prevGray = grayFrame;



                } // The using stat

                cap.Dispose();
            }

        }

    }
}
