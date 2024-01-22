using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Daenet.ImageBinarizerApp
{
    public class ObjectDetector
    {
        private VideoCapture _capture;
        private BackgroundSubtractorMOG2 _bgndSub;
        private string _outputPath;
        private string _inputPath;

        public ObjectDetector(string inputPath, string outputPath)
        {
            _inputPath = inputPath;
            _outputPath = outputPath;
            _capture = new VideoCapture(_inputPath);

            if (!_capture.IsOpened)
            {
                return;
            }

            _bgndSub = new BackgroundSubtractorMOG2();
        }

        public void ProcessVideo()
        {
            int frameWidth = _capture.Width;
            int frameHeight = _capture.Height;
            VideoWriter videoWriter = new VideoWriter(_outputPath, (int)_capture.Get(CapProp.Fps), new System.Drawing.Size(frameWidth, frameHeight), true);


            Mat grayPrev = new Mat();
            Mat grayCurrent = new Mat();
            VectorOfPointF prevPts = new VectorOfPointF();
            VectorOfPointF currentPts = new VectorOfPointF();
            VectorOfByte status = new VectorOfByte();
            VectorOfFloat err = new VectorOfFloat();
            PointF prevFlowVector = PointF.Empty;

            while (true)
            {

                // Read a frame from the video
                Mat frame = _capture.QueryFrame();

                if (frame == null)
                    break; // Break the loop if the video is finished


                    // Apply the background subtraction
                    Mat fgMask = new Mat();
                _bgndSub.Apply(frame, fgMask);

                // Find contours in the foreground mask
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(fgMask, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

                //define region of interest
                Rectangle roi = new Rectangle(0, 0, frame.Width, frame.Height / 2);


                // Iterate through each contour and draw a rectangle around it
                for (int i = 0; i < contours.Size; i++)
                {
                    //
                    //add area filter
                    double minContourArea = 100.0;
                    double contourArea = CvInvoke.ContourArea(contours[i]);

                    if (contourArea > minContourArea && roi.Contains(CvInvoke.BoundingRectangle(contours[i])))
                    {
                    Rectangle boundingBox = CvInvoke.BoundingRectangle(contours[i]);
                    CvInvoke.Rectangle(frame, boundingBox, new MCvScalar(0, 255, 0), 2); // Draw a green rectangle
                    }
                }

                videoWriter.Write(frame);


                // Display the original frame and the foreground mask
                //pictureBoxOriginal.Image = frame.Bitmap;
                //pictureBoxForeground.Image = fgMask.Bitmap;

                //Application.DoEvents(); // Allow UI events to be processed

                //// Break the loop if the form is closed
                //if (IsDisposed)
            }
            videoWriter.Dispose();

        }

        public void CutVideo(double startTime, double endTime)
        {
            using (VideoCapture videoCapture = new VideoCapture(_inputPath))
            {
                if (!videoCapture.IsOpened)
                {
                    Console.WriteLine("Error: Unable to open input video file.");
                    return;
                }

                // Get video properties
                int frameWidth = (int)videoCapture.Get(CapProp.FrameWidth);
                int frameHeight = (int)videoCapture.Get(CapProp.FrameHeight);
                int fps = (int)videoCapture.Get(CapProp.Fps);

                // Create VideoWriter for the output video
                VideoWriter videoWriter = new VideoWriter(_outputPath, fps, new System.Drawing.Size(frameWidth, frameHeight), true);

                // Set the video capture position to the start time
                videoCapture.Set(CapProp.PosFrames, (int)(startTime * fps));

                while (true)
                {
                    Mat frame = videoCapture.QueryFrame();

                    if (frame == null || videoCapture.Get(CapProp.PosFrames) > endTime*fps)
                        break;

                    // Write the frame to the output video
                    videoWriter.Write(frame);
                }

                // Release the VideoWriter
                videoWriter.Dispose();
            }

            Console.WriteLine("Video cutting completed.");
        }

    }

}



