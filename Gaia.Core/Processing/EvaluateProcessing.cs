using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core;
using Gaia.Core.DataStreams;
using Gaia.Exceptions;

namespace Gaia.Core.Processing
{
    /// <summary>
    /// Evaulating processing or expression on data streams
    /// </summary>
    public class EvaluateProcessing : Algorithm
    {
        public DataStream SourceDataStream;
        public String Expression;
        public long ProcessingLineNum = -1;

        public static EvaluateProcessingFactory Factory
        {
            get
            {
                return new EvaluateProcessingFactory();
            }
        }

        public class EvaluateProcessingFactory : AlgorithmFactory
        {
            public String Name { get { return "Evaulate an expression in data streams"; } }
            public String Description { get { return "Evaulate an expression on data lines in stream."; } }

            public EvaluateProcessing Create(Project project, IMessanger messanger, DataStream sourceDataStream, String expression)
            {
                EvaluateProcessing algorithm = new EvaluateProcessing(project, messanger, Name, Description);
                algorithm.SourceDataStream = sourceDataStream;
                algorithm.Expression = expression;
                return algorithm;
            }
        }

        private EvaluateProcessing(Project project, IMessanger messanger, String name, String description) : base(project, messanger, name, description)
        {

        }
      
        /// <summary>
        /// Calculate an expression on datastreams.
        /// If the linenum is different than -1, the intermediate calculations are reported through the messanger object.
        /// If the linenum is -1, the intermediate calculations are NOT reported.
        /// </summary>
        /// <returns></returns>
        public override AlgorithmResult Run()
        {
            if (SourceDataStream == null)
            {
                new GaiaAssertException("Data stream is null!");
            }

            WriteMessage("Calculating...");

            int num = 0;

            string[] sstr = Expression.Split(new string[] { ":=" }, StringSplitOptions.None);

            string leftExpr = "";
            string rightExprProto = "";

            if (sstr.Length > 1)
            {
                leftExpr = sstr[0];
                rightExprProto = sstr[1];
            }
            else
            {
                rightExprProto = Expression;
            }

            // Extract left side field
            PropertyInfo resultField = null;
            if (leftExpr != "")
            {
                foreach (PropertyInfo prop in SourceDataStream.CreateDataLine().GetType().GetProperties())
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
            SourceDataStream.Open();
            SourceDataStream.Begin();

            if (resultField == null)
            {
                WriteMessage("Results: " + Environment.NewLine);
            }

            while (!SourceDataStream.IsEOF())
            {
                DataLine dataLine = SourceDataStream.ReadLine();
                String rightExpr = rightExprProto;

                if (IsCanceled())
                {
                    SourceDataStream.Close();
                    WriteMessage("Processing canceled!", null, null, ConsoleMessageType.Warning);
                    return AlgorithmResult.Partial;
                }

                if ((ProcessingLineNum!=-1) && (num >= ProcessingLineNum)) break;
                num++;

                WriteProgress((double)SourceDataStream.GetPosition() / (double)SourceDataStream.DataNumber * 100.0);

                foreach (PropertyInfo prop in SourceDataStream.CreateDataLine().GetType().GetProperties())
                {
                    object value = prop.GetValue(dataLine);
                    String subStr = "[" + prop.Name + "]";
                    rightExpr = rightExpr.Replace(subStr, value.ToString());

                }

                try
                {
                    double value = evaluateExpresstion(rightExpr);

                    if ((resultField == null) || (ProcessingLineNum != -1))
                    {
                        WriteMessage(num + ". line: " + Convert.ToString(value));
                    }

                    if ((resultField != null) && (ProcessingLineNum == -1))
                    {
                        resultField.SetValue(dataLine, value);
                        SourceDataStream.ReplaceDataLine(dataLine, SourceDataStream.GetPosition() - 1);
                    }

                }
                catch
                {
                    WriteMessage("Cannot evaluate " + num + ". line.", null, null, ConsoleMessageType.Error);
                    return AlgorithmResult.Failure;
                }

            }

            if ((resultField != null) && (ProcessingLineNum == -1))
            {
                WriteProgress(95);
                WriteMessage("Check order flag...");
                SourceDataStream.UpdateOrderFlag();
                WriteProgress(100);
            }

            SourceDataStream.Close();

            return AlgorithmResult.Sucess;
        }

        private double evaluateExpresstion(String expression)
        {
            return Convert.ToDouble((new DataTable().Compute(expression, null)));
        }
    }
}
