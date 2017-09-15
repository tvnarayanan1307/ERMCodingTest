using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERMCodingTest.CSVProcessorInterface
{
    public interface ICSVProcessor
    {
        void GetCSVFiles();
        void ProcessCSVFile(string csvFileName);
        void ProcessCSVData(string csvFileName, DataTable datatable);
    }
}
