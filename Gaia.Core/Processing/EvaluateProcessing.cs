using Gaia.DataStreams;
using Gaia.GaiaSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.Processing
{
    /// <summary>
    /// Evaulating processing or expression on data streams
    /// </summary>
    public class EvaluateProcessing : Algorithm
    {

        public EvaluateProcessing(Project project, IMessanger messanger) : base(project, messanger)
        {

        }

        /// <summary>
        /// Calculate an expression on datastreams.
        /// If the linenum is different than -1, the intermediate calculations are reported through the messanger object.
        /// If the linenum is -1, the intermediate calculations are NOT reported.
        /// </summary>
        /// <param name="dataStream">Data stream</param>
        /// <param name="expression">Expression to evaulate</param>
        /// <param name="lineNum">Number of line to be processed.</param>
        /// <returns></returns>
        public AlgorithmResult Calculate(DataStream dataStream, String expression, long lineNum = -1)
        {
            WriteMessage("Calculating...");

            int num = 0;

            string[] sstr = expression.Split(new string[] { ":=" }, StringSplitOptions.None);

            string leftExpr = "";
            string rightExprProto = "";

            if (sstr.Length > 1)
            {
                leftExpr = sstr[0];
                rightExprProto = sstr[1];
            }
            else
            {
                rightExprProto = expression;
            }

            // Extract left side field
            PropertyInfo resultField = null;
            if (leftExpr != "")
            {
                foreach (PropertyInfo prop in dataStream.CreateDataLine().GetType().GetProperties())
                {
                    String subStr = "[" + prop.Name + "]";
                    if (leftExpr.Contains(subStr))
                    {
                        resultField = prop;
                        break;
                    }
                }

                if (resultField == null)
                {
                    WriteMessage("Left side field does not exist with the given name!");
                    return AlgorithmResult.Failure;
                }

                if(!resultField.CanWrite)
                {
                    WriteMessage("Left side field is not writeable!");
                    return AlgorithmResult.Failure;

                }
            }

            // Solve the expressions left side field
            dataStream.Open();
            dataStream.Begin();

            if (resultField == null)
            {
                WriteMessage("Results: " + Environment.NewLine);
            }

            while (!dataStream.IsEOF())
            {
                DataLine dataLine = dataStream.ReadLine();
                String rightExpr = rightExprProto;

                if (IsCanceled())
                {
                    dataStream.Close();
                    WriteMessage("Processing canceled!", null, null, ConsoleMessageType.Warning);
                    return AlgorithmResult.Partial;
                }

                if ((lineNum!=-1) && (num >= lineNum)) break;
                num++;

                WriteProgress((double)dataStream.GetPosition() / (double)dataStream.DataNumber * 100.0);

                foreach (PropertyInfo prop in dataStream.CreateDataLine().GetType().GetProperties())
                {
                    object value = prop.GetValue(dataLine);
                    String subStr = "[" + prop.Name + "]";
                    rightExpr = rightExpr.Replace(subStr, value.ToString());

                }

                try
                {
                    double value = evaluateExpresstion(rightExpr);

                    if ((resultField == null) || (lineNum != -1))
                    {
                        WriteMessage(num + ". line: " + Convert.ToString(value));
                    }

                    if ((resultField != null) && (lineNum == -1))
                    {
                        resultField.SetValue(dataLine, value);
                        dataStream.ReplaceDataLine(dataLine, dataStream.GetPosition() - 1);
                    }

                }
                catch
                {
                    WriteMessage("Cannot evaluate " + num + ". line.", null, null, ConsoleMessageType.Error);
                    return AlgorithmResult.Failure;
                }

            }

            if ((resultField != null) && (lineNum == -1))
            {
                WriteProgress(95);
                WriteMessage("Check order flag...");
                dataStream.UpdateOrderFlag();
                WriteProgress(100);
            }

            dataStream.Close();

            return AlgorithmResult.Sucess;
        }

        private double evaluateExpresstion(String expression)
        {
            return Convert.ToDouble((new DataTable().Compute(expression, null)));
        }
    }
}
