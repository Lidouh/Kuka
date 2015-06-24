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
using System.Drawing.Printing;

namespace Mapping
{
    public partial class Form1 : Form
    {
        List<System.Drawing.Point> listPix = new List<System.Drawing.Point>();
        List<System.Drawing.Point> listPixTop = new List<System.Drawing.Point>();
        List<System.Drawing.Point> listPixBack = new List<System.Drawing.Point>();
        List<System.Drawing.Point> listPixRight = new List<System.Drawing.Point>();
        string file;
        string project_directory = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
        private PrintDocument pd;
        Image imgToPrint;


        public Form1()
        {
            InitializeComponent();
            this.Text = "Virtual Mapping";
            //pictureBox1.Image = ResizeImage(pictureBox1.Image, new Size(200, 200));
            //Image<Bgr, Byte> image = new Image<Bgr, byte>(100, 100);
            this.pd = new PrintDocument();
            this.pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            //e.Graphics.DrawImage(pictureBox1.Image, pictureBox1.Bounds);
            //e.Graphics.DrawImage(pictureBox1.Image, e.MarginBounds);
            //e.Graphics.DrawImage(pictureBox1.Image, 0,0, pictureBox1.Image.Width, pictureBox1.Image.Height);

            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Inch;

            //Image img = pictureBox1.Image;
            Image img = imgToPrint;
            Graphics gg = Graphics.FromImage(img);


            RectangleF marginBounds = e.MarginBounds;
            if (!pd.PrintController.IsPreview)
                marginBounds.Offset(-e.PageSettings.HardMarginX,
                                -e.PageSettings.HardMarginY);

            float x = marginBounds.X / 100f +
                            (marginBounds.Width / 100f -
                                (float)img.Width / gg.DpiX) / 2f;
            float y = marginBounds.Y / 100f +
                            (marginBounds.Height / 100f -
                                (float)img.Height / gg.DpiY) / 2f;



            g.DrawImage(img, x, y);

            //Don't call g.Dispose(). Operating System will do this job.
            gg.Dispose();//You should call it to release graphics object immediately.

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
                    //pictureBox4.Image = new Bitmap(file);
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

                    System.Drawing.Point[] pts = currentContour.ToArray();
                    foreach (System.Drawing.Point p in pts)
                    {
                        //add points to listbox
                        listBox1.Items.Add(p);
                        listPix.Add(p);
                    }
                }
            }

            processedColor = color.ToBitmap();
            processedGray = grayImage.ToBitmap();

        }


        public void IdentifyContoursList
            (Bitmap colorImage, int thresholdValue, bool invert,
            out Bitmap processedGray, out Bitmap processedColor,
            ListBox listB, List<System.Drawing.Point> listP)
        {
            Image<Gray, byte> grayImage = new Image<Gray, byte>(colorImage);
            Image<Bgr, byte> color = new Image<Bgr, byte>(colorImage);
            grayImage = grayImage.ThresholdBinary(new Gray(thresholdValue), new Gray(255));

            if (invert){ grayImage._Not(); }

            using (MemStorage storage = new MemStorage())
            {
                for (Contour<System.Drawing.Point> contours = grayImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST, storage); contours != null; contours = contours.HNext)
                {
                    Contour<System.Drawing.Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.015, storage);
                    //Contour<System.Drawing.Point> currentContour = contours;
                    if (currentContour.BoundingRectangle.Width > 20)
                    {
                        CvInvoke.cvDrawContours(color, contours, new MCvScalar(255), new MCvScalar(255), -1, 1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, new System.Drawing.Point(0, 0));
                        color.Draw(currentContour.BoundingRectangle, new Bgr(0, 255, 0), 1);
                    }

                    System.Drawing.Point [] pts = currentContour.ToArray();
                    foreach (System.Drawing.Point p in pts)
                    {
                        //add points to listbox and list
                        listB.Items.Add(p);
                        listP.Add(p);
                    }
                }
            }

            processedColor = color.ToBitmap();
            processedGray = grayImage.ToBitmap();
        }


        public Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            int hypotenuse = (int) Math.Sqrt(img.Width*img.Width + img.Height*img.Height);
            Bitmap bmp = new Bitmap(hypotenuse, hypotenuse);
            //Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            gfx.FillRectangle(new SolidBrush(Color.White),
                new Rectangle(0, 0, bmp.Width, bmp.Height));

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2 , -(float)bmp.Height / 2 );

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new System.Drawing.Point((hypotenuse - img.Width) / 2, (hypotenuse - img.Height) / 2));
            //gfx.DrawImage(img, new System.Drawing.Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }

        //not used
        public static System.Drawing.Point RotatePoint(float angle, System.Drawing.Point pt)
        {
            var a = angle * System.Math.PI / 180.0;
            float cosa = (float)Math.Cos(a);
            float sina = (float)Math.Sin(a);
            System.Drawing.Point newPoint = new System.Drawing.Point(
                                                (int) (pt.X * cosa - pt.Y * sina),
                                                (int) (pt.X * sina + pt.Y * cosa));
            return newPoint;
        }

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
            if (listPix[listBox1.SelectedIndex].X < bmp.Width - 2 && listPix[listBox1.SelectedIndex].X > 2
                && listPix[listBox1.SelectedIndex].Y < bmp.Height - 2 && listPix[listBox1.SelectedIndex].Y > 22)
            {
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X, listPix[listBox1.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X, listPix[listBox1.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X, listPix[listBox1.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X, listPix[listBox1.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X, listPix[listBox1.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(listPix[listBox1.SelectedIndex].X + 1, listPix[listBox1.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X + 1, listPix[listBox1.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X + 1, listPix[listBox1.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X + 1, listPix[listBox1.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X + 1, listPix[listBox1.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(listPix[listBox1.SelectedIndex].X - 1, listPix[listBox1.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X - 1, listPix[listBox1.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X - 1, listPix[listBox1.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X - 1, listPix[listBox1.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X - 1, listPix[listBox1.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(listPix[listBox1.SelectedIndex].X + 2, listPix[listBox1.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X + 2, listPix[listBox1.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X + 2, listPix[listBox1.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X + 2, listPix[listBox1.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X + 2, listPix[listBox1.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(listPix[listBox1.SelectedIndex].X - 2, listPix[listBox1.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X - 2, listPix[listBox1.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X - 2, listPix[listBox1.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X - 2, listPix[listBox1.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(listPix[listBox1.SelectedIndex].X - 2, listPix[listBox1.SelectedIndex].Y - 2, Color.Black);

                pictureBox1.Image = bmp;
                Console.WriteLine(listPix[listBox1.SelectedIndex].X + " , " + listPix[listBox1.SelectedIndex].Y);
            }
        }

        private void buttonCoordinates_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                listPix.Clear();
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
                /*************** Déforme l'image de pictureBox1 en carré ****************/
                List<IntPoint> cornersImage = new List<IntPoint>();
                cornersImage.Add(new IntPoint(0, 0));
                cornersImage.Add(new IntPoint(pictureBox1.Image.Width, 0));
                cornersImage.Add(new IntPoint(pictureBox1.Image.Width, pictureBox1.Image.Height));
                cornersImage.Add(new IntPoint(0, pictureBox1.Image.Height));
                // create filter
                SimpleQuadrilateralTransformation filterImage =
                    new SimpleQuadrilateralTransformation(cornersImage, 400, 400);
                // apply the filter
                Bitmap newImage = filterImage.Apply(new Bitmap(pictureBox1.Image));
                pictureBox1.Image = newImage;

                /*************** Changement de texture ****************/
                //renderControl1.AlterTexture(file);
                //pictureBox1.Image = RotateImage(pictureBox1.Image, -45);

                /*************** Coupe la partie gauche de l'image pour la face RIGHT du cube ****************/
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
                newImageRight.Save(project_directory + "\\newRight.jpg");
                pictureBoxRight.Image = newImageRight;

                /*************** Coupe la partie droite de l'image pour la face BACK du cube ****************/
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
                newImageBack.Save(project_directory + "\\newBack.jpg");
                pictureBoxBack.Image = newImageBack;

                /**************** Coupe la partie haute de l'image pour la face TOP du cube ***************/
                //CropTriangle();
                pictureBoxTop.Image = ResizeImage(pictureBox1.Image, new Size(200, 200));
                pictureBoxTop.Image = RotateImage(pictureBoxTop.Image, -45);

                Bitmap source = (Bitmap) pictureBoxTop.Image;
                Rectangle section = new Rectangle(new System.Drawing.Point(0, 0), new Size(source.Width/2, source.Height/2));
                Bitmap CroppedImage = CropImage(source, section);
                CroppedImage.Save(project_directory + "\\newTop.jpg");
                pictureBoxTop.Image = CroppedImage;

                /*************** Changement de texture ****************/
                renderControl1.AlterTextureTop(project_directory + "\\newTop.jpg");
                renderControl1.AlterTextureBack(project_directory + "\\newBack.jpg");
                renderControl1.AlterTextureRight(project_directory + "\\newRight.jpg");

            }
        }

        public Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }

        //not used
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
            Bitmap bmp = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height / 2);
            //Graphics graph = pictureBox4.CreateGraphics();
            Graphics graph = Graphics.FromImage(bmp);
            //the white image
            graph.FillRectangle(new SolidBrush(Color.White),
                new Rectangle(0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height));
            // set the clop region of the forms graphic object to be the new shape
            graph.Clip = new Region(path);

            //graph.RotateTransform(-45);
            //graph.TranslateTransform(-pictureBox1.Image.Width * 1/2, pictureBox1.Image.Height );
            // draw the image cliped to the custom shape
            graph.DrawImage(pictureBox1.Image, new System.Drawing.Point(0, 0));
            //pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxTop.Image = bmp;
            //pictureBox4.Image = RotateImage(pictureBox4.Image, -45);

        }

        private void buttonReinit_Click(object sender, EventArgs e)
        {
            renderControl1.ReinitTexture();
        }

        private void buttonCoordBack_Click(object sender, EventArgs e)
        {
            if (pictureBoxBack.Image != null)
            {
                listPixBack.Clear();
                listBoxBack.Items.Clear();
                Bitmap bmp = new Bitmap(pictureBoxBack.Image);
                Image<Bgr, Byte> img = new Image<Bgr, byte>(bmp);

                Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();

                Gray cannyThreshold = new Gray(80);
                Gray cannyThresholdLinking = new Gray(120);
                Gray circleAccumulatorThreshold = new Gray(120);

                Image<Gray, Byte> cannyEdges = gray.Canny(cannyThreshold, cannyThresholdLinking).Not();

                Bitmap color;
                Bitmap bgray;
                IdentifyContoursList(cannyEdges.Bitmap, 50, true, out bgray, out color, listBoxBack, listPixBack);

                pictureBoxBack.Image = color;
            }

        }

        private void buttonCoordRight_Click(object sender, EventArgs e)
        {
            if (pictureBoxRight.Image != null)
            {
                listPixRight.Clear();
                listBoxRight.Items.Clear();
                Bitmap bmp = new Bitmap(pictureBoxRight.Image);
                Image<Bgr, Byte> img = new Image<Bgr, byte>(bmp);

                Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();

                Gray cannyThreshold = new Gray(80);
                Gray cannyThresholdLinking = new Gray(120);
                Gray circleAccumulatorThreshold = new Gray(120);

                Image<Gray, Byte> cannyEdges = gray.Canny(cannyThreshold, cannyThresholdLinking).Not();

                Bitmap color;
                Bitmap bgray;
                IdentifyContoursList(cannyEdges.Bitmap, 50, true, out bgray, out color, listBoxRight, listPixRight);

                pictureBoxRight.Image = color;
            }

        }

        private void buttonCoordTop_Click(object sender, EventArgs e)
        {
            if (pictureBoxTop.Image != null)
            {
                listPixTop.Clear();
                listBoxTop.Items.Clear();
                Bitmap bmp = new Bitmap(pictureBoxTop.Image);
                Image<Bgr, Byte> img = new Image<Bgr, byte>(bmp);

                Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();

                Gray cannyThreshold = new Gray(80);
                Gray cannyThresholdLinking = new Gray(120);
                Gray circleAccumulatorThreshold = new Gray(120);

                Image<Gray, Byte> cannyEdges = gray.Canny(cannyThreshold, cannyThresholdLinking).Not();

                Bitmap color;
                Bitmap bgray;
                IdentifyContoursList(cannyEdges.Bitmap, 50, true, out bgray, out color, listBoxTop, listPixTop);

                pictureBoxTop.Image = color;
            }

        }

        private void verifySetPixels(PictureBox pictureB, int x, int y)
        {
            Bitmap bmp = new Bitmap(pictureB.Image);
            if (x < bmp.Width && x > 0 && y < bmp.Height && y > 0)
            {
                bmp.SetPixel(x, y, Color.Black);
            }

        }

        private void settingPixels(PictureBox pictureB, ListBox listB, List<System.Drawing.Point> listP)
        {
            Bitmap bmp = new Bitmap(pictureB.Image);
            if (listP[listB.SelectedIndex].X < bmp.Width - 2 && listP[listB.SelectedIndex].X > 2
                && listP[listB.SelectedIndex].Y < bmp.Height - 2 && listP[listB.SelectedIndex].Y > 2)
            {
                bmp.SetPixel(listP[listB.SelectedIndex].X, listP[listB.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X, listP[listB.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X, listP[listB.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X, listP[listB.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X, listP[listB.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(listP[listB.SelectedIndex].X + 1, listP[listB.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X + 1, listP[listB.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X + 1, listP[listB.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X + 1, listP[listB.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X + 1, listP[listB.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(listP[listB.SelectedIndex].X - 1, listP[listB.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X - 1, listP[listB.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X - 1, listP[listB.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X - 1, listP[listB.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X - 1, listP[listB.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(listP[listB.SelectedIndex].X + 2, listP[listB.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X + 2, listP[listB.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X + 2, listP[listB.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X + 2, listP[listB.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X + 2, listP[listB.SelectedIndex].Y - 2, Color.Black);

                bmp.SetPixel(listP[listB.SelectedIndex].X - 2, listP[listB.SelectedIndex].Y, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X - 2, listP[listB.SelectedIndex].Y + 1, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X - 2, listP[listB.SelectedIndex].Y - 1, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X - 2, listP[listB.SelectedIndex].Y + 2, Color.Black);
                bmp.SetPixel(listP[listB.SelectedIndex].X - 2, listP[listB.SelectedIndex].Y - 2, Color.Black);

                pictureB.Image = bmp;
            }
        
        }

        private void listBoxBack_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingPixels(pictureBoxBack, listBoxBack, listPixBack);
        }

        private void listBoxRight_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingPixels(pictureBoxRight, listBoxRight, listPixRight);
        }

        private void listBoxTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingPixels(pictureBoxTop, listBoxTop, listPixTop);
        }

        public static void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, Bitmap destBitmap, Rectangle destRegion)
        {
            using (Graphics grD = Graphics.FromImage(destBitmap))
            {
                grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
            }
        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {
            imgToPrint = Image.FromFile( project_directory+ "\\cross.png") ;
            Image imgToPastTop = Image.FromFile(project_directory + "\\newTop.jpg");
            Image imgToPastRight = Image.FromFile(project_directory + "\\newRight.jpg");
            Image imgToPastBack = Image.FromFile(project_directory + "\\newBack.jpg");

            Rectangle regionSrcTop = new Rectangle(new System.Drawing.Point(0, 0), new Size(imgToPastTop.Width, imgToPastTop.Height));
            Rectangle regionSrcRight = new Rectangle(new System.Drawing.Point(0, 0), new Size(imgToPastRight.Width, imgToPastRight.Height));
            Rectangle regionSrcBack = new Rectangle(new System.Drawing.Point(0, 0), new Size(imgToPastBack.Width, imgToPastBack.Height));
            
            Rectangle regionDestTop = new Rectangle(new System.Drawing.Point(202, 1), new Size(200, 200));
            Rectangle regionDestRight = new Rectangle(new System.Drawing.Point(202, 202), new Size(200, 200));
            Rectangle regionDestBack = new Rectangle(new System.Drawing.Point(403, 202), new Size(200, 200));

            CopyRegionIntoImage((Bitmap)imgToPastTop, regionSrcTop, (Bitmap)imgToPrint, regionDestTop);
            CopyRegionIntoImage((Bitmap)imgToPastRight, regionSrcRight, (Bitmap)imgToPrint, regionDestRight);
            CopyRegionIntoImage((Bitmap)imgToPastBack, regionSrcBack, (Bitmap)imgToPrint, regionDestBack);


            PrintPreviewDialog print1 = new PrintPreviewDialog();
            //PrintDialog print1 = new PrintDialog();
            print1.Document = this.pd;
            if (print1.ShowDialog() == DialogResult.OK)
            { pd.Print(); }
        }


    }
}
