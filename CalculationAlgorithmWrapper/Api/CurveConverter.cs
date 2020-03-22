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
            var outputStringMatlab = ConvertDebuggerStringToMatlabCurveString(debuggerString);
            var outputStringCsharp = ConvertMatlabCurveStringToCSharpCurveString(outputStringMatlab);

            matlabCurveString = outputStringMatlab;
            cSharpCurveString = outputStringCsharp;
        }

        private static string ConvertDebuggerStringToMatlabCurveString(string debuggerString)
        {
            var outputStringMatlab = "curve = [";

            var debuggerLines = debuggerString.Split('\n');

            var debuggerLinesFiltered = debuggerLines.Where(x => (x != "\r") && (x != "")).ToArray();

            foreach (var debuggerLine in debuggerLinesFiltered)
            {
                var columns = debuggerLine.Split('\t');

                var number = (columns.Length >= 3) ? columns[3] : "";
                var num = 0.0;

                if (double.TryParse(number, out num))
                {
                    outputStringMatlab += number + " ";
                }
            }

            // remove last blank
            if(outputStringMatlab[outputStringMatlab.Length - 1] == ' ')
            {
                outputStringMatlab = outputStringMatlab.Remove(outputStringMatlab.Length - 1);
            }
            
            outputStringMatlab += "];";

            return outputStringMatlab;
        }

        private static string ConvertMatlabCurveStringToCSharpCurveString(string matlabCurveString)
        {
            var outputStringCsharp = matlabCurveString.Replace(" ", ", ").Replace("curve,", "var curve").Replace("=,", "=")
                .Replace("[", "new List<double> {").Replace("]", "}").Replace(", }", "}");

            return outputStringCsharp;
        }
    }
}
