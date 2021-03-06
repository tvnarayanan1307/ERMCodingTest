﻿using System;
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
    public class TOUCSVProcessor : ICSVProcessor
    {
        private string csvDirectoryPath = string.Empty;


        /// <summary>
        /// Default constructor
        /// </summary>
        public TOUCSVProcessor()
        {

        }

        /// <summary>
        /// Parameterized constructor by passing the CSV Directory path
        /// </summary>
        /// <param name="csvDirectoryPath"></param>
        public TOUCSVProcessor(string csvDirectoryPath)
        {
            this.csvDirectoryPath = csvDirectoryPath;
        }

        /// <summary>
        ///  Method to get the TOU CSV Files implemented from the Interface ICSVProcessor
        /// </summary>
        public void GetCSVFiles()
        {
            DirectoryInfo diCSVFiles = new DirectoryInfo(csvDirectoryPath);
            var touCSVFiles = diCSVFiles.GetFiles("TOU_*.csv");
            Console.WriteLine("\nProcessing TOU Files Data:\n");
            foreach (var touFile in touCSVFiles)
            {
                ProcessCSVFile(touFile.FullName.ToString());
            }
        }

        /// <summary>
        /// Method to process each LP CSV Files implemented from the Interface ICSVProcessor
        /// </summary>
        /// <param name="touFileName"></param>
        public void ProcessCSVFile(string touFileName)
        {
            //Code to get the Data Table by invoking the static method
            //GetDataTableFromCSV of the CSVHelper class
            DataTable TOUCSVData = CSVHelper.GetDataTableFromCSV(touFileName);

            //Invoke ProcessCSVData by passing filename and the datatable
            ProcessCSVData(touFileName, TOUCSVData);
        }

        /// <summary>
        /// Method implemented from ICSVProcessor interface by passing the CSV Filename and DataTable
        /// </summary>
        /// <param name="touFileName"></param>
        /// <param name="dtTOUCSVData"></param>
        public void ProcessCSVData(string touFileName, DataTable dtTOUCSVData)
        {
            int totalRowCount = dtTOUCSVData.Rows.Count;
            decimal medianValue = 0.0M;
            decimal medianValue1 = 0.0M;
            decimal medianValue2 = 0.0M;
            dtTOUCSVData.DefaultView.Sort = "Energy asc";
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

            dtTOUCSVData.DefaultView.Sort = "Energy asc";
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
