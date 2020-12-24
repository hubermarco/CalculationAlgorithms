﻿using CurveChartImageCreator;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CurveChartImageCreatorTests
{
    [TestFixture]
    public class CurveChartImageApiTests
    {
        [Test]
        public void When_curve_chart_image_is_created_then_corresponding_file_is_stored()
        {
            var f = new List<double> { 100, 105, 110, 115, 120, 125, 130, 135, 145, 150, 160, 165, 175, 180, 190, 200, 210, 220, 230, 240, 250, 260, 275, 290, 300, 315, 330, 345, 360, 380, 400, 420, 440, 460, 480, 500, 525, 550, 575, 600, 630, 660, 700, 730, 760, 800, 830, 870, 900, 950, 1000, 1050, 1100, 1150, 1200, 1250, 1312.5, 1375, 1437.5, 1500, 1562.5, 1625, 1687.5, 1750, 1812.5, 1875, 1937.5, 2000, 2062.5, 2125, 2187.5, 2250, 2312.5, 2375, 2437.5, 2500, 2562.5, 2625, 2687.5, 2750, 2812.5, 2875, 2937.5, 3000, 3062.5, 3125, 3187.5, 3250, 3312.5, 3375, 3437.5, 3500, 3562.5, 3625, 3687.5, 3750, 3812.5, 3875, 3937.5, 4000, 4062.5, 4125, 4187.5, 4250, 4312.5, 4375, 4437.5, 4500, 4562.5, 4625, 4687.5, 4750, 4812.5, 4875, 4937.5, 5000, 5062.5, 5125, 5187.5, 5250, 5312.5, 5375, 5437.5, 5500, 5562.5, 5625, 5687.5, 5750, 5812.5, 5875, 5937.5, 6000, 6062.5, 6125, 6187.5, 6250, 6312.5, 6375, 6437.5, 6500, 6562.5, 6625, 6687.5, 6750, 6812.5, 6875, 6937.5, 7000, 7062.5, 7125, 7187.5, 7250, 7312.5, 7375, 7437.5, 7500, 7562.5, 7625, 7687.5, 7750, 7812.5, 7875, 7937.5, 8000, 8062.5, 8125, 8187.5, 8250, 8312.5, 8375, 8437.5, 8500, 8562.5, 8625, 8687.5, 8750, 8812.5, 8875, 8937.5, 9000, 9062.5, 9125, 9187.5, 9250, 9312.5, 9375, 9437.5, 9500, 9562.5, 9625, 9687.5, 9750, 9812.5, 9875, 9937.5, 10000, 10062.5, 10125, 10187.5, 10250, 10312.5, 10375, 10437.5, 10500, 10562.5, 10625, 10687.5, 10750, 10812.5, 10875, 10937.5, 11000, 11062.5, 11125, 11187.5, 11250, 11312.5, 11375, 11437.5, 11500, 11562.5, 11625, 11687.5, 11750, 11812.5, 11875, 11937.5, 12000 };
            var curve = new List<double> { 1.01225914568586, 1.01312669389332, 1.01510998749382, 1.01776707056865, 1.0206952587105, 1.0219896083306, 1.0219488119469, 1.01866325927172, 1.00453960565027, 0.990959417804932, 0.956769141023688, 0.938473580873142, 0.90092814026715, 0.881204275862698, 0.834390634865054, 0.783231251230187, 0.703953008483941, 0.62423004180947, 0.543519757074016, 0.472748115238241, 0.420177768345911, 0.374859470571852, 0.333556334377277, 0.30780105435082, 0.301292803323442, 0.286493539731841, 0.272731703711746, 0.258962369458824, 0.252836050533659, 0.264790946853794, 0.318226748351119, 0.402345178082109, 0.486927680356988, 0.55040470619443, 0.584643184593637, 0.589300484305054, 0.617121833209745, 0.675516445831797, 0.752357659901709, 0.82871575247118, 0.91385493535449, 0.997900654693403, 1.14654975436009, 1.29549106909078, 1.48049325768423, 1.76308847804103, 2.01223004458969, 2.40907016302058, 2.81269165111046, 3.54001145625575, 4.32301724757485, 5.15875702729167, 5.84739335575142, 6.51904044519092, 7.19390008264455, 7.82442456591315, 8.92090165136494, 10.0343524557515, 11.1413158643746, 12.1433286309967, 12.9618136677003, 13.6012688943514, 14.0149967669386, 14.2335075680566, 14.295572106683, 14.1999119552601, 14.0009278390438, 13.7066224509578, 13.3216153777246, 12.8720651656454, 12.4087740112342, 11.903845707839, 11.469104314604, 11.1051144727383, 10.8092382534152, 10.6491714468869, 10.5065710049135, 10.428582297655, 10.3714884067165, 10.3208589741648, 10.2980438567484, 10.1822687190271, 10.0618524727375, 9.89921818730432, 9.6948154764433, 9.44124503604148, 9.11537151905731, 8.78829596633738, 8.43119086740576, 8.04144039666744, 7.66421443128432, 7.20020658537971, 6.77083682291573, 6.33467857251614, 5.89241807095717, 5.50339059392001, 5.05509181670553, 4.64957292159707, 4.27732777563014, 3.92279327847987, 3.67151574091647, 3.43710525287471, 3.2453483456586, 3.10745840781099, 3.02411324491695, 3.01678918968619, 3.03562463424794, 3.07141926077625, 3.14884935328413, 3.24862118130254, 3.39005346716243, 3.53760542510195, 3.68404647177608, 3.83624129224554, 3.98924073026772, 4.14339803844854, 4.30705540916826, 4.45024020836729, 4.59119879816311, 4.71746728382999, 4.81818788631171, 4.93337576944691, 5.0378824379336, 5.13863135014033, 5.2444913415638, 5.32383515333109, 5.47221677105109, 5.60420051481252, 5.75739444549645, 5.92338684506013, 6.09180501911683, 6.35844214460027, 6.60030383760662, 6.86130803041722, 7.14176062619548, 7.44768631891732, 7.83179354172801, 8.18266057382619, 8.54233838861849, 8.93276013302087, 9.31450885911491, 9.74713607954703, 10.154654356664, 10.561202070211, 10.9552877323928, 11.3068447776111, 11.6780727069072, 12.0243314519638, 12.3453175135064, 12.6367925120181, 12.8718844550762, 13.1050925381165, 13.31018181913, 13.4813487997464, 13.6238696414021, 13.6978903914709, 13.7559108956338, 13.7999926755352, 13.8176689138508, 13.8160248575562, 13.7477342652224, 13.6695438611925, 13.5958975790106, 13.4979585966022, 13.3841549752885, 13.2181818847114, 13.0520681050731, 12.8897309278023, 12.7157498116477, 12.5329063592453, 12.3236597125384, 12.112853650184, 11.9066127118103, 11.6977519350714, 11.484247626736, 11.2746919490777, 11.0064332063516, 10.7453781813205, 10.4649404537679, 10.1705203614462, 9.89471138751187, 9.52144533627685, 9.20089518390589, 8.86903872405432, 8.53069962491045, 8.21421160121823, 7.79518436481729, 7.42076736157795, 7.04224834320191, 6.65355936061303, 6.2901122558954, 5.84045838350315, 5.42186572326536, 4.99563986793946, 4.56239785004675, 4.16147455970299, 3.67909021969091, 3.23052553397324, 2.78303855169877, 2.33441775875981, 1.92232216506714, 1.4412005718991, 0.993734839834901, 0.550745554215528, 0.111813700733606, -0.279501707795039, -0.727717782955668, -1.14119093930804, -1.55082816245888, -1.94898126123396, -2.29326061605982, -2.68276902481941, -3.03082816283257, -3.35055232293607, -3.64441071186451, -3.86798627411791, -4.07432180441758, -4.29054181079086, -4.48208441645061, -4.65204460213148, -4.76023307107702, -4.84851057181423, -4.950370063304, -5.02521311450991, -5.0791345589966, -5.07178928353576, -5.04491551292036, -5.0282101167891 };
            var curve2 = curve.Select(x => x + 10).ToList();
            var outPutDir  = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\Test";

            CurveChartImageApi.Create(
                 fileNameWithoutExtention: "test",
                 headerCaption: "Header",
                 xGrid1: f,
                 xGrid2: f,
                 curveList1: new List<List<double>> { curve },
                 curveList2: new List<List<double>> { curve2 },
                 outputDir: outPutDir,
                 linearFreqAxis: false,
                 imageWidth: 900,
                 imageHeight: 600);
        }
    }
}
