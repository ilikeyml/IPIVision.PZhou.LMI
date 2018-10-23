using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.Util;



namespace Calibrition
{
    public class SurfaceCalibration
    {

        public static Matrix<double> Solve(Matrix<double> src1, Matrix<double> src2)
        {
            Matrix<double> output = new Matrix<double>(4, 3);
            CvInvoke.Solve(src1, src2, output, Emgu.CV.CvEnum.DecompMethod.Svd);
            return output;
        }

        public static unsafe Matrix<double> CreateMatrixFromFile(string[] fileData, bool isHomogeneous, int pointCount)
        {
            List<double> rawdata = new List<double>();

            for (int i = 0; i < fileData.Length; i++)
            {
                string[] subData = fileData[i].Split(',');
                for (int j = 0; j < subData.Length; j++)
                {
                    rawdata.Add(Convert.ToDouble(subData[j]));
                }
                if (isHomogeneous)
                {
                    rawdata.Add(1);
                }
            }

            double[] dataArray = rawdata.ToArray();
            fixed (double* dataPtr = dataArray)
            {
                if (isHomogeneous)
                {
                    return new Matrix<double>(pointCount, 4, (IntPtr)dataPtr);
                }
                return new Matrix<double>(pointCount, 3, (IntPtr)dataPtr);
            }

        }

        public static bool SerializeMatrixToXml<T>(T matrixData, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            (new XmlSerializer(typeof(T))).Serialize(new StringWriter(sb), matrixData);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(sb.ToString());

            xDoc.Save(filePath);

            return false;
        }

        public static T DeserializeXmlToMatrix<T>(string filePath)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(filePath);
            T matrix = (T)
            (new XmlSerializer(typeof(T))).Deserialize(new XmlNodeReader(xDoc));
            return matrix;
        }

        public static string MatrixToString(Matrix<double> matrix)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Cols; j++)
                {
                    sb.Append(matrix[i, j].ToString());
                    if (j < matrix.Cols-1)
                    {
                        sb.Append(",");
                    }
    
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

    }
}
