using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example2.Model
{
    public enum Shape
    {
        Line,
        Circle,
        Rectangle,
        Pencil,
        Eraser,
        Triangle
    }
    public enum DrawTool
    {
        Pen,
        
        //Fill
    }

    public class DrawerState
    {
         
        public Pen pen = new Pen(Color.Red);
        public Bitmap bmp;
        Graphics g;
        GraphicsPath path;
        private PictureBox pictureBox1;
        public Point[] rec = new Point[4];
    
        PointF[] points = new PointF[3];


        public Point prevPoint;

        public DrawTool DrawTool { get; set; }
        public Shape Shape { get; set; }

        //int cnt = 0;
        //public void Fill(int x0, int y0)
        //{
        //    cnt++;
        //    if (cnt >= 1000)
        //    {
        //        return;
        //    }
        //    if (bmp.GetPixel(x0 + 1, y0) != pen.Color && x0 + 1 <= bmp.Width && bmp.GetPixel(x0, y0) == bmp.GetPixel(x0 + 1, y0))
        //    {
        //        bmp.SetPixel(x0, y0, pen.Color);
        //        Fill(x0 + 1, y0);
        //        return;
        //    }

        //    if (bmp.GetPixel(x0 - 1, y0) != pen.Color && x0 - 1 != -1 && bmp.GetPixel(x0, y0) == bmp.GetPixel(x0 - 1, y0))
        //    {
        //        bmp.SetPixel(x0, y0, pen.Color);
        //        Fill(x0 - 1, y0);
        //        return;
        //    }

        //    if (bmp.GetPixel(x0, y0 - 1) != pen.Color && y0 - 1 != -1 && bmp.GetPixel(x0, y0) == bmp.GetPixel(x0, y0 - 1))
        //    {
        //        bmp.SetPixel(x0, y0, pen.Color);
        //        Fill(x0, y0 - 1);
        //        return;
        //    }

        //    if (bmp.GetPixel(x0, y0 + 1) != pen.Color && y0 + 1 < bmp.Height && bmp.GetPixel(x0, y0) == bmp.GetPixel(x0, y0 + 1))
        //    {
        //        bmp.SetPixel(x0, y0, pen.Color);
        //        Fill(x0, y0 + 1);
        //        return;
        //    }
        //}

        public void FixPath()
        {
            if (path != null)
            {
                g.DrawPath(pen, path);
                path = null;
                
            }
        }

        public DrawerState(PictureBox pictureBox1)
        {
            this.pictureBox1 = pictureBox1;
            
            Load("");

            DrawTool = DrawTool.Pen;
            Shape = Shape.Pencil;
            //Fill(12, 12);

            pictureBox1.Paint += PictureBox1_Paint;
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (path != null)
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        public void Draw(Point currentPoint)
        {
            switch (Shape)
            {
                case Shape.Line:
                    path = new GraphicsPath();
                    path.AddLine(prevPoint, currentPoint);
                    break;
                case Shape.Circle:
                    Rectangle rect = new Rectangle(prevPoint.X, prevPoint.Y, 
                        Math.Min(currentPoint.X - prevPoint.X, 
                        currentPoint.Y - prevPoint.Y),
                        Math.Min(currentPoint.X - prevPoint.X, 
                        currentPoint.Y - prevPoint.Y));

                    path = new GraphicsPath();
                    path.AddEllipse(rect);
                    break;
                case Shape.Rectangle:
                    rec[0] = new Point(prevPoint.X, prevPoint.Y);
                    rec[2] = new Point(currentPoint.X, currentPoint.Y);
                    rec[3] = new Point(prevPoint.X, currentPoint.Y);
                    rec[1] = new Point(currentPoint.X, prevPoint.Y);
                    path = new GraphicsPath();
                    path.AddPolygon(rec);
                    break;
                case Shape.Triangle:
                    path = new GraphicsPath();
                    points[0].X = (prevPoint.X + currentPoint.X) / 2;
                    points[0].Y = prevPoint.Y;
                    points[1].X = prevPoint.X;
                    points[1].Y = currentPoint.Y;
                    points[2].X = currentPoint.X;
                    points[2].Y = currentPoint.Y;
                    path.AddPolygon(points);
                    break;
                case Shape.Pencil:
                    g.DrawLine(pen, prevPoint, currentPoint);
                    prevPoint = currentPoint;
                    break;
                case Shape.Eraser:
                    g.DrawLine(new Pen(Color.White,pen.Width), prevPoint, currentPoint);
                    break;
                default:
                    break;
            }

            pictureBox1.Refresh();
        }

        public void Save(string fileName)
        {
            bmp.Save(fileName);
        }

        public void Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                bmp = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            }
            else {
                bmp = new Bitmap(fileName);
            }

            g = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
        }
    }
}
