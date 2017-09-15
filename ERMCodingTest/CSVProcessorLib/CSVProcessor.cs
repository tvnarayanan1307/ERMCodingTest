using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using ERMCodingTest.Utilities;
using System.Data;

namespace ERMCodingTest.CSVProcessorLib
{
    public class CSVProcessor
    {
        public string csvFilePath;
        public void GetCSVFiles()
        {

            DirectoryInfo diCSVFiles = new DirectoryInfo(csvFilePath);

            var lpCSVFiles = diCSVFiles.GetFiles("LP_*.csv");

            var touCSVFiles = diCSVFiles.GetFiles("TOU_*.csv");

            Console.WriteLine("Processing LP Files Data:\n");
            foreach (var lpFile in lpCSVFiles)
            {
                ProcessLPCSVFile(lpFile.FullName.ToString());
            }
            Console.ReadKey();
            Console.WriteLine("\nProcessing TOU Files Data:\n");
            foreach (var touFile in touCSVFiles)
            {
                ProcessTOUCSVFile(touFile.FullName.ToString());
            }

        }

        private void ProcessLPCSVFile(string lpFileName)
        {
            
            DataTable LPCSVData = CSVHelper.GetDataTableFromCSV(lpFileName);
            ProcessLPData(lpFileName,LPCSVData);
        }

        private void ProcessTOUCSVFile(string touFileName)
        {
            DataTable TOUCSVData = CSVHelper.GetDataTableFromCSV(touFileName);
            ProcessTOUData(touFileName, TOUCSVData);
        }

        private void ProcessLPData(string lpFileName, DataTable dtLPCSVData)
        {
            int totalRowCount = dtLPCSVData.Rows.Count;
            decimal medianValue = 0.0M;
            dtLPCSVData.DefaultView.Sort = "DataValue asc";
            if(totalRowCount == 2)
            {
                decimal medianValue1 = Convert.ToDecimal(dtLPCSVData.Rows[0]["DataValue"].ToString());
                decimal medianValue2 = Convert.ToDecimal(dtLPCSVData.Rows[1]["DataValue"].ToString());
                medianValue = Convert.ToDecimal((medianValue1 + medianValue2) / 2);
            }
            else
            {
                if (totalRowCount % 2 == 0)
                {
                    int medianValueIndex = Convert.ToInt32(((totalRowCount + 1) / 2));
                    decimal medianValue1 = Convert.ToDecimal(dtLPCSVData.Rows[medianValueIndex]["DataValue"].ToString());
                    decimal medianValue2 = Convert.ToDecimal(dtLPCSVData.Rows[medianValueIndex + 1]["DataValue"].ToString());
                    medianValue = Convert.ToDecimal((medianValue1 + medianValue2) / 2);
                }
                else
                {
                    int medianValueIndex = (totalRowCount + 1) / 2;
                    medianValue = Convert.ToDecimal(dtLPCSVData.Rows[medianValueIndex]["DataValue"].ToString());
                }
            }

            foreach(DataRow drLPData in dtLPCSVData.Rows)
            {
                double lpDataValue = Convert.ToDouble(drLPData["DataValue"].ToString());
                if((lpDataValue > Convert.ToDouble((Convert.ToDecimal(0.2) * medianValue))) || (lpDataValue < Convert.ToDouble((Convert.ToDecimal(0.2) * medianValue))))
                {
                    FileInfo lpFileInfo = new FileInfo(lpFileName);
                    string fileName = lpFileInfo.Name;
                    DateTime lpDate = Convert.ToDateTime(drLPData["Date/Time"].ToString());
                    Console.Write(String.Format("LP Filename: \"{0}\" Date: \"{1}\" Value: \"{2}\" Median Value: \"{3}\" \n", fileName, lpDate.ToShortDateString(), lpDataValue.ToString(), medianValue.ToString()));
                }
            }
        }

        private void ProcessTOUData(string touFileName, DataTable dtTOUCSVData)
        {
            
            int totalRowCount = dtTOUCSVData.Rows.Count;
            decimal medianValue = 0.0M;
            decimal medianValue1 = 0.0M;
            decimal medianValue2 = 0.0M;
            if (totalRowCount == 2)
            {
                medianValue1 = Convert.ToDecimal(dtTOUCSVData.Rows[0]["Energy"].ToString());
                medianValue2 = Convert.ToDecimal(dtTOUCSVData.Rows[1]["Energy"].ToString());
                medianValue = Convert.ToDecimal((medianValue1 + medianValue2) / 2);
            }
            else
            {
                if (totalRowCount % 2 == 0)
                {
                    int medianValueIndex = Convert.ToInt32(((totalRowCount + 1) / 2));
                    medianValue1 = Convert.ToDecimal(dtTOUCSVData.Rows[medianValueIndex]["Energy"].ToString());
                    medianValue2 = Convert.ToDecimal(dtTOUCSVData.Rows[medianValueIndex + 1]["Energy"].ToString());
                    medianValue = Convert.ToDecimal((medianValue1 + medianValue2) / 2);
                }
                else
                {
                    int medianValueIndex = (totalRowCount + 1) / 2;
                    medianValue = Convert.ToDecimal(dtTOUCSVData.Rows[medianValueIndex]["Energy"].ToString());
                }
            }
            

            foreach (DataRow drLPData in dtTOUCSVData.Rows)
            {
                double touEnergyValue = Convert.ToDouble(drLPData["Energy"].ToString());
                if ((touEnergyValue > Convert.ToDouble((Convert.ToDecimal(0.2) * medianValue))) || (touEnergyValue < Convert.ToDouble((Convert.ToDecimal(0.2) * medianValue))))
                {
                    FileInfo touFileInfo = new FileInfo(touFileName);
                    string fileName = touFileInfo.Name;
                    DateTime lpDate = Convert.ToDateTime(drLPData["Date/Time"].ToString());
                    Console.Write(String.Format("TOU Filename: \"{0}\" Date: \"{1}\" Energy Value: \"{2}\" Median Value: \"{3}\" \n", fileName, lpDate.ToShortDateString(), touEnergyValue.ToString(), medianValue.ToString()));
                }
            }
        }



    }
}
