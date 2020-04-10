using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CurveChartImageCreater.Implementation
{
    internal class Resources
    {
        internal Resources(double dWidth, double dHeight, double dFontSize, float fLineWidth)
        {
            MGraph = new Bitmap((int)dWidth, (int)dHeight, PixelFormat.Format32bppArgb);
            Font = new Font("Lucida Sans Unicode", (int)dFontSize);
            Graph = Graphics.FromImage(MGraph);
            Graph.SmoothingMode = SmoothingMode.AntiAlias;
            BrushBlk = new SolidBrush(Color.Black);
            BrushRed = new SolidBrush(Color.Red);
            BrushLGrn = new SolidBrush(Color.DarkGreen);
            BrushGry = new SolidBrush(Color.DarkGray);
            BrushYellow = new SolidBrush(Color.FromArgb(255, 255, 230, 0));
            BrushOrange = new SolidBrush(Color.Orange);
            BrushBlue = new SolidBrush(Color.Blue);
            BrushDarkBlue = new SolidBrush(Color.DarkBlue);
            PenBlk = new Pen(BrushBlk, fLineWidth);
            PenRed = new Pen(BrushRed, fLineWidth);
            PenLGrn = new Pen(BrushLGrn, fLineWidth);
            PenGry = new Pen(BrushGry, 1f);
            PenYellow = new Pen(BrushYellow, fLineWidth);
            PenOrange = new Pen(BrushOrange, fLineWidth);
            PenBlue = new Pen(BrushBlue, fLineWidth);
            PenDarkBlue = new Pen(BrushDarkBlue, fLineWidth);
        }

        public Bitmap MGraph { get; }
        Font Font { get; }
        Graphics Graph { get; }
        SolidBrush BrushBlk { get; }
        SolidBrush BrushRed { get; }
        SolidBrush BrushLGrn { get; }
        SolidBrush BrushGry { get; }
        SolidBrush BrushYellow { get; }
        SolidBrush BrushOrange { get; }
        SolidBrush BrushBlue { get; }
        SolidBrush BrushDarkBlue { get; }
        Pen PenBlk { get; }
        Pen PenRed { get; }
        Pen PenLGrn { get; }
        Pen PenGry { get; }
        Pen PenYellow { get; }
        Pen PenOrange { get; }
        Pen PenBlue { get; }
        Pen PenDarkBlue { get; }
    }
}
