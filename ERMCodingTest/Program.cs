using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ERMCodingTest.CSVProcessorLib;
using ERMCodingTest.CSVProcessorInterface;

namespace ERMCodingTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            string csvDirectoryPath = ConfigurationManager.AppSettings["csvFilePath"].ToString(); ;
            ICSVProcessor iCSVProcessor = new LPCSVProcessor(csvDirectoryPath);
            iCSVProcessor.GetCSVFiles();
            Console.ReadKey();
            iCSVProcessor = new TOUCSVProcessor(csvDirectoryPath);
            iCSVProcessor.GetCSVFiles();
            Console.ReadKey();

        }
    }
}
