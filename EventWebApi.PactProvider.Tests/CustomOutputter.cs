using PactNet.Reporters.Outputters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventWebApi.PactProvider.Tests
{
    public class CustomOutputter : IReportOutputter
    {
        private readonly StringBuilder StringBuilder = new StringBuilder();

        public string Output { get
            {
                return StringBuilder.ToString();
            } }
        public void Write(string report)
        {
            StringBuilder.Append(report);
        }
    }
}
