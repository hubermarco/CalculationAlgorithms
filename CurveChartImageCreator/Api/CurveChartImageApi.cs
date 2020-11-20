using System;
using System.Collections.Generic;
using System.Linq;

namespace CurveChartImageCreator
{
    public class CurveChartImageApi
    {
        public static void Create(
            string fileNameWithoutExtention,
            string headerCaption,
            IList<double> xGrid,
            IEnumerable<IList<double>> curveList1,
            IEnumerable<IList<double>> curveList2,
            string outputDir,
            bool linearFreqAxis = false,
            uint imageWidth = 600,
            uint imageHeight = 400)
        {
            var freqCurveList1 = (curveList1 != null) ? new List<FreqCrv>() : null;

            if(curveList1 != null)
            {
                foreach (var curve in curveList1)
                {
                    var freqCrv = new FreqCrv(TCurveType.None);
                    xGrid.Select((x, index) => new FreqPt(x, curve[index])).ToList().ForEach(freqPt => freqCrv.Add(freqPt));
                    freqCurveList1.Add(freqCrv);
                }
            }

            var freqCurveList2 = (curveList2 != null) ? new List<FreqCrv>() : null;

            if(curveList2 != null)
            {
                foreach (var curve in curveList2)
                {
                    var freqCrv = new FreqCrv(TCurveType.None);
                    xGrid.Select((x, index) => new FreqPt(x, curve[index])).ToList().ForEach(freqPt => freqCrv.Add(freqPt));
                    freqCurveList2.Add(freqCrv);
                }
            }

            TestCurveChartImage.Create(
                fileNameWithoutExtention: fileNameWithoutExtention,
                headerCaption: headerCaption,
                targetCurves: freqCurveList1,
                simCurves: freqCurveList2,
                outputDir,
                linearFreqAxis,
                imageWidth,
                imageHeight);

        }
    }
}
