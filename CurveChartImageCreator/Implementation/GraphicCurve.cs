using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace CurveChartImageCreator
{
    public class GraphicCurve
    {
        public static bool LinearFreqAxis;
        public static string HeaderCaption;
        public static string LastError;
        public static double YMin4Graph = 1.0;
        public static double YMax4Graph = -1.0;

        //draw curves to bitmap file

        internal static void WriteFile(IList<FreqCrv> targetCurves, IList<FreqCrv> simCurves, Stream stream, uint width = 300, uint height = 300)
        {
            try
            {
                var dWidth = width; //px
                var dHeight = height; // px
                const double dBorder = 30.0; // px
                const double dFontSize = 10.0; // px
                const float fLineWidth = 1.6f; // px

                if (!(targetCurves.Count > 0 && targetCurves[0].Count >= 1 && simCurves.Count > 0 && simCurves[0].Count > 1))
                {
                    return;
                }

                var dXMin = targetCurves[0][0].X;
                var dXMax = dXMin;
                var dYMin = targetCurves[0][0].Y;
                var dYMax = dYMin;

                foreach (var crv in targetCurves)
                {
                    foreach (var pt in crv)
                    {
                        if (pt.X < dXMin)
                        {
                            dXMin = pt.X;
                        }
                        if (pt.X > dXMax)
                        {
                            dXMax = pt.X;
                        }
                        if (pt.Y < dYMin)
                        {
                            dYMin = pt.Y;
                        }
                        if (pt.Y > dYMax)
                        {
                            dYMax = pt.Y;
                        }
                    }
                }

                foreach (var crv in simCurves)
                {
                    foreach (var pt in crv)
                    {
                        if (pt.X < dXMin)
                        {
                            dXMin = pt.X;
                        }
                        if (pt.X > dXMax)
                        {
                            dXMax = pt.X;
                        }
                        if (pt.Y < dYMin)
                        {
                            dYMin = pt.Y;
                        }
                        if (pt.Y > dYMax)
                        {
                            dYMax = pt.Y;
                        }
                    }
                }

#pragma warning disable 612

                if (YMax4Graph <= YMin4Graph) //autoscale
                {
                    dYMin = 10.0*Math.Floor( dYMin/10.0 );
                    dYMax = 10.0*Math.Ceiling( dYMax/10.0 );
                }
                else
                {
                    dYMin = YMin4Graph;
                    dYMax = YMax4Graph;
                }
#pragma warning restore 612

                var dXScale = (dWidth - 2.0 * dBorder) / Math.Log10(dXMax / dXMin);   //logarithmic x axis
                if (LinearFreqAxis)
                {
                    dXScale = (dWidth - 2.0 * dBorder) / (dXMax - dXMin);   //linear x axis
                }
                var dYScale = (dHeight - 2.0 * dBorder) / (dYMax - dYMin);

                //drawing resources
                var mGraph = new Bitmap((int)dWidth, (int)dHeight, PixelFormat.Format32bppArgb);
                var font = new Font("Lucida Sans Unicode", (int)dFontSize);
                var graph = Graphics.FromImage(mGraph);
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                var brushBlk = new SolidBrush(Color.Black);
                var brushRed = new SolidBrush(Color.Red);
                var brushLGrn = new SolidBrush(Color.DarkGreen);
                var brushGry = new SolidBrush(Color.DarkGray);
                var brushYellow = new SolidBrush(Color.FromArgb(255, 255, 230, 0));
                var brushOrange = new SolidBrush(Color.Orange);
                var brushBlue = new SolidBrush(Color.Blue);
                var brushDarkBlue = new SolidBrush(Color.DarkBlue);
                var penBlk = new Pen(brushBlk, fLineWidth);
                var penRed = new Pen(brushRed, fLineWidth);
                var penLGrn = new Pen(brushLGrn, fLineWidth);
                var penGry = new Pen(brushGry, 1f);
                var penYellow = new Pen(brushYellow, fLineWidth);
                var penOrange = new Pen(brushOrange, fLineWidth);
                var penBlue = new Pen(brushBlue, fLineWidth);
                var penDarkBlue = new Pen(brushDarkBlue, fLineWidth);

                graph.FillRectangle(Brushes.White, 0.0f, 0.0f, dWidth, dHeight);

                //axis frame
                var dX1 = dBorder;
                var dY1 = dBorder;
                var dX2 = dWidth - dBorder;
                var dY2 = dBorder;
                DrawLine(dX1, dY1, dX2, dY2, penGry, graph, dHeight);
                dX1 = dX2;
                dY1 = dY2;
                dX2 = dWidth - dBorder;
                dY2 = dHeight - dBorder;
                DrawLine(dX1, dY1, dX2, dY2, penGry, graph, dHeight);
                dX1 = dX2;
                dY1 = dY2;
                dX2 = dBorder;
                dY2 = dHeight - dBorder;
                DrawLine(dX1, dY1, dX2, dY2, penGry, graph, dHeight);
                dX1 = dX2;
                dY1 = dY2;
                dX2 = dBorder;
                dY2 = dBorder;
                DrawLine(dX1, dY1, dX2, dY2, penGry, graph, dHeight);
                dX1 = 1.5 * dBorder;
                dY1 = dHeight - 0.5 * dBorder;
                DrawStringY( HeaderCaption, dX1, dY1, font, brushGry, graph, dHeight );

                //min y
                dX1 = 0.02 * dBorder;
                dY1 = 1.0 * dBorder;
                DrawStringY(dYMin.ToString( CultureInfo.InvariantCulture ), dX1, dY1, font, brushGry, graph, dHeight);

                //max y
                dX1 = 0.02 * dBorder;
                dY1 = dHeight - 1.0 * dBorder;
                DrawStringY(dYMax.ToString( CultureInfo.InvariantCulture ), dX1, dY1, font, brushGry, graph, dHeight);

                //x-axis:  125Hz  250Hz  500Hz  1k  2k  4k  8k
                dY1 = dBorder;
                dY2 = dHeight - dBorder;
                dX1 = Value2PixelX(125.0, dBorder, dXScale, dXMin);
                DrawStringX("125", dX1, dY1, font, brushGry, graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);
                dX1 = Value2PixelX(250.0, dBorder, dXScale, dXMin);
                DrawStringX("250", dX1, dY1, font, brushGry, graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);
                dX1 = Value2PixelX(500.0, dBorder, dXScale, dXMin);
                DrawStringX("500", dX1, dY1, font, brushGry, graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);
                dX1 = Value2PixelX(1000.0, dBorder, dXScale, dXMin);
                DrawStringX("1k", dX1, dY1, font, brushGry, graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);
                dX1 = Value2PixelX(2000.0, dBorder, dXScale, dXMin);
                DrawStringX("2k", dX1, dY1, font, brushGry, graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);
                dX1 = Value2PixelX(4000.0, dBorder, dXScale, dXMin);
                DrawStringX("4k", dX1, dY1, font, brushGry, graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);
                dX1 = Value2PixelX(8000.0, dBorder, dXScale, dXMin);
                DrawStringX("8k", dX1, dY1, font, brushGry, graph, dHeight);
                DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);

                //linear x-axis only:  5k 6k 7k 10k 11k
                if (LinearFreqAxis)
                {
                    dX1 = Value2PixelX(5000.0, dBorder, dXScale, dXMin);
                    DrawStringX("5k", dX1, dY1, font, brushGry, graph, dHeight);
                    DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);
                    dX1 = Value2PixelX(6000.0, dBorder, dXScale, dXMin);
                    DrawStringX("6k", dX1, dY1, font, brushGry, graph, dHeight);
                    DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);
                    dX1 = Value2PixelX(7000.0, dBorder, dXScale, dXMin);
                    DrawStringX("7k", dX1, dY1, font, brushGry, graph, dHeight);
                    DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);
                    dX1 = Value2PixelX(10000.0, dBorder, dXScale, dXMin);
                    DrawStringX("10k", dX1, dY1, font, brushGry, graph, dHeight);
                    DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);
                    dX1 = Value2PixelX(11000.0, dBorder, dXScale, dXMin);
                    DrawStringX("11k", dX1, dY1, font, brushGry, graph, dHeight);
                    DrawLine(dX1, dY1, dX1, dY2, penGry, graph, dHeight);
                }


                //y-axis:  10db steps
                dX1 = dBorder;
                dX2 = dWidth - dBorder;
                for (var i = (int)Math.Ceiling(0.1 + dYMin / 10.0); 1.0 + 10.0 * i < dYMax; i++)
                {
                    double dValue = 10.0 * i;
                    dY1 = dBorder + dYScale * (dValue - dYMin);
                    dY2 = dY1;
                    DrawLine(dX1, dY1, dX2, dY2, penGry, graph, dHeight);
                    DrawStringY(dValue.ToString( CultureInfo.InvariantCulture ), 0.02 * dBorder, dY1, font, brushGry, graph, dHeight);
                }

                //targets
                foreach (var crv in targetCurves)
                {
                    if ( (crv.CurveType != null) && (crv.CurveType == TCurveType.MPO_Target) )
                       DrawCurve( dHeight, dXMin, dYMin, penYellow, graph, crv, dYScale, dXScale, dBorder );
                    else
                        DrawCurve(dHeight, dXMin, dYMin, penRed, graph, crv, dYScale, dXScale, dBorder);
                }
                //sim curves
                foreach (var crv in simCurves)
                {
                    if (crv.CurveType == TCurveType.Level_Low || crv.CurveType == TCurveType.Level_Medium ||
                        crv.CurveType == TCurveType.Level_High)
                    {
                        DrawCurve(dHeight, dXMin, dYMin, penBlk, graph, crv, dYScale, dXScale, dBorder);
                    }
                    else if (crv.CurveType == TCurveType.Fog)
                    {
                        DrawCurve(dHeight, dXMin, dYMin, penOrange, graph, crv, dYScale, dXScale, dBorder);
                    } 
                    else if (crv.CurveType == TCurveType.TinnitusNoiserSimulation ||
                             crv.CurveType == TCurveType.TinnitusNoiserBroadbandLevel)
                    {
                        DrawCurve(dHeight, dXMin, dYMin, penLGrn, graph, crv, dYScale, dXScale, dBorder);
                    }
                    else if (crv.CurveType == TCurveType.CgmNoiseBroadbandLevel ||
                             crv.CurveType == TCurveType.CgmReferenceCurve ||
                             crv.CurveType == TCurveType.CriticalGain ||
                             crv.CurveType == TCurveType.CriticalGainFollowUp ||
                             crv.CurveType == TCurveType.CriticalGainMeasured ||
                             crv.CurveType == TCurveType.CriticalGainStatistical)
                    {
                        DrawCurve(dHeight, dXMin, dYMin, penBlue, graph, crv, dYScale, dXScale, dBorder);
                    }
                    else if (crv.CurveType == TCurveType.Effective_MPO)
                    {
                        DrawCurve(dHeight, dXMin, dYMin, penDarkBlue, graph, crv, dYScale, dXScale, dBorder);
                    }
                    else
                    {
                        DrawCurve(dHeight, dXMin, dYMin, penGry, graph, crv, dYScale, dXScale, dBorder);
                    }
                }

                if (stream != null)
                    mGraph.Save(stream, ImageFormat.Png);
            }
            catch (Exception exc)
            {
                if (LastError != null) LastError += "\n";
                LastError += exc.Message;
                if (exc.InnerException != null) LastError += exc.InnerException.Message;
            }
        }

        internal static void WriteLegend(Stream stream, uint width = 300, uint height = 300)
        {
            try
            {
                var dWidth = width; //px
                var dHeight = height; // px
                const double dBorder = 30.0; // px
                const double dFontSize = 10.0; // px
                const float fLineWidth = 1.6f; // px
          
                //drawing resources
                var mGraph = new Bitmap((int)dWidth, (int)dHeight, PixelFormat.Format32bppArgb);
                var font = new Font("Lucida Sans Unicode", (int)dFontSize);
                var graph = Graphics.FromImage(mGraph);
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                var brushBlk = new SolidBrush(Color.Black);
                var brushRed = new SolidBrush(Color.Red);
                var brushLGrn = new SolidBrush(Color.DarkGreen);
                var brushGry = new SolidBrush(Color.DarkGray);
                var brushYellow = new SolidBrush(Color.FromArgb( 255, 255, 230, 0 ));
                var brushOrange = new SolidBrush(Color.Orange);
                var penBlk = new Pen(brushBlk, fLineWidth);
                var penRed = new Pen(brushRed, fLineWidth);
                var penLGrn = new Pen(brushLGrn, fLineWidth);
                var penGry = new Pen(brushGry, fLineWidth);
                var penYellow = new Pen(brushYellow, fLineWidth);
                var penOrange = new Pen(brushOrange, fLineWidth);

                graph.FillRectangle(Brushes.White, 0.0f, 0.0f, dWidth, dHeight);

                double dX = dBorder;
                double dX2 = dX + 3*dBorder;
                double dY = dHeight - dBorder;

                DrawLine(dX, dY, dX2, dY, penYellow, graph, dHeight);  
                DrawStringY("MPO Target", dX2 + dBorder, dY, font, brushYellow, graph, dHeight);

                dY -= dBorder;
                DrawLine(dX, dY, dX2, dY, penRed, graph, dHeight);
                DrawStringY("Target", dX2 + dBorder, dY, font, brushRed, graph, dHeight);

                dY -= dBorder;
                DrawLine(dX, dY, dX2, dY, penBlk, graph, dHeight);
                DrawStringY("Level Curve", dX2 + dBorder, dY, font, brushBlk, graph, dHeight);

                dY -= dBorder;
                DrawLine(dX, dY, dX2, dY, penOrange, graph, dHeight);
                DrawStringY("Fog Curve", dX2 + dBorder, dY, font, brushOrange, graph, dHeight);

                dY -= dBorder;
                DrawLine(dX, dY, dX2, dY, penLGrn, graph, dHeight);
                DrawStringY("Tinn. Curve", dX2 + dBorder, dY, font, brushLGrn, graph, dHeight);

                dY -= dBorder;
                DrawLine(dX, dY, dX2, dY, penGry, graph, dHeight);
                DrawStringY("Other Curve", dX2 + dBorder, dY, font, brushGry, graph, dHeight);

                if (stream != null)
                    mGraph.Save(stream, ImageFormat.Png);
            }
            catch (Exception exc)
            {
                if (LastError != null) LastError += "\n";
                LastError += exc.Message;
                if (exc.InnerException != null) LastError += exc.InnerException.Message;
            }
        }

        private static void DrawCurve( double dHeight, double dXMin, double dYMin, Pen penRed, Graphics graph,
                                         FreqCrv crv, double dYScale, double dXScale, double dBorder )
        {
            double dX1 = 0;
            double dY1 = 0;
            for (var ii = 0; ii < crv.Count; ii++)
            {
                if (ii == 0)
                {
                    dX1 = Value2PixelX( crv[ii].X, dBorder, dXScale, dXMin );
                    dY1 = dBorder + dYScale*( crv[ii].Y - dYMin );
                }
                else
                {
                    var dX2 = Value2PixelX( crv[ii].X, dBorder, dXScale, dXMin );
                    var dY2 = dBorder + dYScale*( crv[ii].Y - dYMin );
                    DrawLine( dX1, dY1, dX2, dY2, penRed, graph, dHeight );
                    dX1 = dX2;
                    dY1 = dY2;
                }
            }
        }

        private static void DrawLine(double dX1, double dY1, double dX2, double dY2,
                              Pen pen, Graphics graph, double dHeight)
        {
            graph.DrawLine(pen, (float)dX1, (float)(dHeight - dY1), (float)dX2, (float)(dHeight - dY2));
        }

        private static void DrawStringX(string sString, double dX, double dY,
                                 Font font, Brush brush, Graphics graph, double dHeight)
        {
            //draw the string centered along x-axis
            var fX = (float)dX;
            var fY = (float)dY;
            var pt = new PointF(fX, (float)dHeight - fY);
            var fmt = new StringFormat {Alignment = StringAlignment.Center};
            graph.DrawString(sString, font, brush, pt, fmt);
        }

        private static void DrawStringY(string sString, double dX, double dY,
                                 Font font, Brush brush, Graphics graph, double dHeight)
        {
            //draw the string centered along y-axis
            var fX = (float)dX;
            var fY = (float)dY;
            var pt = new PointF(fX, (float)dHeight - fY);
            var fmt = new StringFormat {LineAlignment = StringAlignment.Center};
            graph.DrawString(sString, font, brush, pt, fmt);
        }

        //helper functions for graphic
        private static double Value2PixelX(double dValue, double dBorder, double dXScale, double dXMin)
        {
            double dPixelPos;

            if (LinearFreqAxis)
            {
                dPixelPos = dBorder + dXScale * (dValue - dXMin);   //linear x axis
            }
            else
            {
                dPixelPos = dBorder + dXScale * Math.Log10(dValue / dXMin);   //logarithmic x axis
            }
            return dPixelPos;
        }

    }
}
