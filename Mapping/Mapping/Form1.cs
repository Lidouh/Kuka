using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Collections;
using System.IO;
using AForge.Imaging.Filters;
using AForge;

namespace Mapping
{
    public partial class Form1 : Form
    {
        System.Drawing.Point [] pts;
        List<System.Drawing.Point> pixels = new List<System.Drawing.Point>();
        string file;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Virtual Mapping";
            //pictureBox1.Image = ResizeImage(pictureBox1.Image, new Size(200, 200));
            //Image<Bgr, Byte> image = new Image<Bgr, byte>(100, 100);
        }

        public void Render()
        {
            renderControl1.Render();
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = "Open Image File";
            //fDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            fDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            //fDialog.InitialDirectory = @"C:\";
            DialogResult result = fDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                file = fDialog.FileName;
                try
                {
                    pictureBox1.Image = new Bitmap(file);
                    pictureBox4.Image = new Bitmap(file);
                    Console.WriteLine(file);
                }
                catch (IOException){}
            }
            
            Console.WriteLine(result); // <-- For debugging use.
        }

        public void IdentifyContours(Bitmap colorImage, int thresholdValue, bool invert, out Bitmap processedGray, out Bitmap processedColor)
        {
            Image<Gray, byte> grayImage = new Image<Gray, byte>(colorImage);
            Image<Bgr, byte> color = new Image<Bgr, byte>(colorImage);

            grayImage = grayImage.ThresholdBinary(new Gray(thresholdValue), new Gray(255));

            if (invert)
            {
                grayImage._Not();
            }

            using (MemStorage storage = new MemStorage())
            {

                for (Contour<System.Drawing.Point> contours = grayImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage); contours != null; contours = contours.HNext)
                {
                    
                    Contour<System.Drawing.Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.015, storage);
                    //Contour<Point> currentContour = contours;
                    if (currentContour.BoundingRectangle.Width > 20)
                    {
                        CvInvoke.cvDrawContours(color, contours, new MCvScalar(255), new MCvScalar(255), -1, 1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, new System.Drawing.Point(0, 0));
                        color.Draw(currentContour.BoundingRectangle, new Bgr(0, 255, 0), 1);
                    }

                    pts = currentContour.ToArray();
                    foreach (System.Drawing.Point p in pts)
                    {
                        //add points to listbox
                        listBox1.Items.Add(p);
                        pixels.Add(p);
                    }
                }
            }

            processedColor = color.ToBitmap();
            processedGray = grayImage.ToBitmap();

        }

        //not used
        public Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new System.Drawing.Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }

        //not used
        public Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            if (pixels[listBox1.SelectedIndex].X < bmp.Width - 2 && pixels[listBox1.SelectedIndex].X > 2
                && pixels[listBox1.SelectedIndex].Y < bmp.Height - 2 && pixels[listBox1.SelectedIndex].Y > 22)
            {
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X, pixels[listBox1.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X, pixels[listBox1.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X, pixels[listBox1.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X, pixels[listBox1.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X, pixels[listBox1.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(pixels[listBox1.SelectedIndex].X + 1, pixels[listBox1.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X + 1, pixels[listBox1.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X + 1, pixels[listBox1.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X + 1, pixels[listBox1.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X + 1, pixels[listBox1.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(pixels[listBox1.SelectedIndex].X - 1, pixels[listBox1.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X - 1, pixels[listBox1.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X - 1, pixels[listBox1.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X - 1, pixels[listBox1.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X - 1, pixels[listBox1.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(pixels[listBox1.SelectedIndex].X + 2, pixels[listBox1.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X + 2, pixels[listBox1.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X + 2, pixels[listBox1.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X + 2, pixels[listBox1.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X + 2, pixels[listBox1.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(pixels[listBox1.SelectedIndex].X - 2, pixels[listBox1.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X - 2, pixels[listBox1.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X - 2, pixels[listBox1.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X - 2, pixels[listBox1.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(pixels[listBox1.SelectedIndex].X - 2, pixels[listBox1.SelectedIndex].Y - 2, Color.Black);

                pictureBox1.Image = bmp;
                Console.WriteLine(pixels[listBox1.SelectedIndex].X + " , " + pixels[listBox1.SelectedIndex].Y);
            }
        }

        private void buttonCoordinates_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pixels.Clear();
                listBox1.Items.Clear();
                Bitmap bmp = new Bitmap(pictureBox1.Image);
                Image<Bgr, Byte> img = new Image<Bgr, byte>(bmp);

                Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();

                Gray cannyThreshold = new Gray(80);
                Gray cannyThresholdLinking = new Gray(120);
                Gray circleAccumulatorThreshold = new Gray(120);

                Image<Gray, Byte> cannyEdges = gray.Canny(cannyThreshold, cannyThresholdLinking).Not();

                Bitmap color;
                Bitmap bgray;
                IdentifyContours(cannyEdges.Bitmap, 50, true, out bgray, out color);

                pictureBox1.Image = color;
                //pictureBox1.Image = bgray;
            }

        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            if (file != null)
            {
                renderControl1.AlterTexture(file);
                //pictureBox1.Image = RotateImage(pictureBox1.Image, -45);

                List<IntPoint> cornersRight = new List<IntPoint>();
                cornersRight.Add(new IntPoint(0, 0));
                cornersRight.Add(new IntPoint(pictureBox1.Image.Width / 2, pictureBox1.Image.Height / 2));
                cornersRight.Add(new IntPoint(pictureBox1.Image.Width / 2, pictureBox1.Image.Height));
                cornersRight.Add(new IntPoint(0, pictureBox1.Image.Height));
                // create filter
                SimpleQuadrilateralTransformation filterRight =
                    new SimpleQuadrilateralTransformation(cornersRight, 200, 200);
                // apply the filter
                Bitmap newImageRight = filterRight.Apply(new Bitmap(pictureBox1.Image));
                pictureBox2.Image = newImageRight;

                List<IntPoint> cornersBack = new List<IntPoint>();
                cornersBack.Add(new IntPoint(pictureBox1.Image.Width / 2, pictureBox1.Image.Height / 2));
                cornersBack.Add(new IntPoint(pictureBox1.Image.Width, 0));
                cornersBack.Add(new IntPoint(pictureBox1.Image.Width, pictureBox1.Image.Height));
                cornersBack.Add(new IntPoint(pictureBox1.Image.Width /2, pictureBox1.Image.Height));
                // create filter
                SimpleQuadrilateralTransformation filterBack =
                    new SimpleQuadrilateralTransformation(cornersBack, 200, 200);
                // apply the filter
                Bitmap newImageBack = filterBack.Apply(new Bitmap(pictureBox1.Image));
                pictureBox3.Image = newImageBack;

                CropTriangle();
            }
        }

        private void CropTriangle()
        {
            // create a graphic path to hold the shape data
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            // add a set of points that define the shape
            path.AddLines(new System.Drawing.Point[]
                              { new System.Drawing.Point(0, 0),
                                new System.Drawing.Point(pictureBox1.Image.Width, 0),
                                new System.Drawing.Point(pictureBox1.Image.Width /2, pictureBox1.Image.Height /2)
                              });
            // close the shape
            path.CloseAllFigures();
            // create graphics object
            Graphics graph = pictureBox1.CreateGraphics();
            //the white image
            graph.FillRectangle(new SolidBrush(Color.White),
                new Rectangle(0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height));
            // set the clop region of the forms graphic object to be the new shape
            graph.Clip = new Region(path);
            //graph.RotateTransform(-45);
            // draw the image cliped to the custom shape
            graph.DrawImage(pictureBox1.Image, new System.Drawing.Point(0, 0));
        }

        private void buttonReinit_Click(object sender, EventArgs e)
        {
            renderControl1.ReinitTexture();
        }


    }
}
