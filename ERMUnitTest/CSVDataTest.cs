using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ERMCodingTest.CSVProcessorLib;
using ERMCodingTest.CSVProcessorInterface;


namespace ERMUnitTest
{
    [TestClass]
    public class CSVDataTest
    {
        /// <summary>
        /// Test method to test LP CSV Data
        /// </summary>
        [TestMethod]
        public void LPCSVDataTest()
        {
            ICSVProcessor lpCSVDataProcessor = new LPCSVProcessor();
            lpCSVDataProcessor.ProcessCSVFile(@"D:\CSVFiles\LP_214612653_20150907T220027915.csv");
        }

        /// <summary>
        /// Test method to test TOU CSV Data
        /// </summary>
        [TestMethod]
        public void TOUCSVDataTest()
        {
            ICSVProcessor touCSVDataProcessor = new TOUCSVProcessor();
            touCSVDataProcessor.ProcessCSVFile(@"D:\CSVFiles\TOU_212621145_20150911T022358.csv");
        }
    }
}
