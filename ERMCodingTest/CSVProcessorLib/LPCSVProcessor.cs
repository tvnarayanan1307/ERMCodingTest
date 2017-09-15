using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERMCodingTest.CSVProcessorInterface;
using ERMCodingTest.Utilities;
using System.Data;
using System.IO;

namespace ERMCodingTest.CSVProcessorLib
{
    public class LPCSVProcessor : ICSVProcessor
    {
        public string csvDirectoryPath = "";

        /// <summary>
        /// Default constructor
        /// </summary>
        public LPCSVProcessor()
        {

        }

        /// <summary>
        /// Parameterized constructor by passing the CSV Directory path
        /// </summary>
        /// <param name="csvDirectoryPath"></param>
        public LPCSVProcessor(string csvDirectoryPath)
        {
            this.csvDirectoryPath = csvDirectoryPath;
        }

        /// <summary>
        /// Method to get the LP CSV Files implemented from the Interface ICSVProcessor
        /// </summary>
        public void GetCSVFiles()
        {
            DirectoryInfo diCSVFiles = new DirectoryInfo(csvDirectoryPath);

            var lpCSVFiles = diCSVFiles.GetFiles("LP_*.csv");

            Console.WriteLine("Processing LP Files Data:\n");
            foreach (var lpFile in lpCSVFiles)
            {
                ProcessCSVFile(lpFile.FullName.ToString());
            }
        }

        /// <summary>
        /// Method to process each LP CSV Files implemented from the Interface ICSVProcessor
        /// </summary>
        /// <param name="lpFileName"></param>
        public void ProcessCSVFile(string lpFileName)
        {
            //Code to get the Data Table by invoking the static method
            //GetDataTableFromCSV of the CSVHelper class
            DataTable LPCSVData = CSVHelper.GetDataTableFromCSV(lpFileName);

            //Invoke ProcessCSVData by passing filename and the datatable
            ProcessCSVData(lpFileName, LPCSVData);
        }

        /// <summary>
        /// Method implemented from ICSVProcessor interface by passing the CSV Filename and DataTable
        /// </summary>
        /// <param name="lpFileName"></param>
        /// <param name="dtLPCSVData"></param>
        public void ProcessCSVData(string lpFileName, DataTable dtLPCSVData)
        {
            int totalRowCount = dtLPCSVData.Rows.Count;
            decimal medianValue = 0.0M;
            dtLPCSVData.DefaultView.Sort = "Data Value asc";
            if (totalRowCount == 2)
            {
                decimal medianValue1 = Convert.ToDecimal(dtLPCSVData.Rows[0]["Data Value"].ToString());
                decimal medianValue2 = Convert.ToDecimal(dtLPCSVData.Rows[1]["Data Value"].ToString());
                medianValue = Convert.ToDecimal((medianValue1 + medianValue2) / 2);
            }
            else
            {
                if (totalRowCount % 2 == 0)
                {
                    int medianValueIndex = Convert.ToInt32(((totalRowCount + 1) / 2));
                    decimal medianValue1 = Convert.ToDecimal(dtLPCSVData.Rows[medianValueIndex]["Data Value"].ToString());
                    decimal medianValue2 = Convert.ToDecimal(dtLPCSVData.Rows[medianValueIndex + 1]["Data Value"].ToString());
                    medianValue = Convert.ToDecimal((medianValue1 + medianValue2) / 2);
                }
                else
                {
                    int medianValueIndex = (totalRowCount + 1) / 2;
                    medianValue = Convert.ToDecimal(dtLPCSVData.Rows[medianValueIndex]["DataValue"].ToString());
                }
            }
            dtLPCSVData.DefaultView.Sort = "Data Value asc";
            foreach (DataRow drLPData in dtLPCSVData.Rows)
            {
                double lpDataValue = Convert.ToDouble(drLPData["Data Value"].ToString());
                if ((lpDataValue > Convert.ToDouble((Convert.ToDecimal(0.2) * medianValue))) || (lpDataValue < Convert.ToDouble((Convert.ToDecimal(0.2) * medianValue))))
                {
                    FileInfo lpFileInfo = new FileInfo(lpFileName);
                    string fileName = lpFileInfo.Name;
                    DateTime lpDate = Convert.ToDateTime(drLPData["Date/Time"].ToString());
                    Console.Write(String.Format("LP Filename: \"{0}\" Date: \"{1}\" Value: \"{2}\" Median Value: \"{3}\" \n", fileName, lpDate.ToShortDateString(), lpDataValue.ToString(), medianValue.ToString()));
                }
            }
        }
    }
}
