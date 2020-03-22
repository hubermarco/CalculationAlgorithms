using System.Linq;

namespace CalculationAlgorithmWrapper
{
    public class CurveConverter
    {
         public static void ConvertDebuggerString(
            string debuggerString, 
            ref string matlabCurveString,
            ref string cSharpCurveString)
        {
            var outputString = "curve = [";

            var debuggerLines = debuggerString.Split('\n');

            var debuggerLinesFiltered = debuggerLines.Where(x => (x != "\r") && (x != "")).ToArray();

            foreach (var debuggerLine in debuggerLinesFiltered)
            {
                var columns = debuggerLine.Split('\t');

                var number = (columns.Length >= 3) ? columns[3] : "";
                var num = 0.0;

                if (double.TryParse(number, out num))
                {
                    outputString += number + " ";
                }
            }

            outputString += "];";

            matlabCurveString = outputString;
            cSharpCurveString = outputString;
        }
    }
}
