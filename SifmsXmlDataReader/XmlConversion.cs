/*
 *Author:Pankaj Kishor Neupaney
 * Created date:07-jul-2020
 * Desc:class to replicate xmldata
 */
using System;
namespace XmlConversion
{
    public class XmlConversion
    {
    }

        public class Expenditure
        {
            public int fin_year1 { get; set; }
            public int fin_year2 { get; set; }
            public int statecode { get; set; }
            public long budgetcode { get; set; }
            public string objectheadcode { get; set; }
            public string ddocode { get; set; }
            public int vouchernumber { get; set; }
            public DateTime date { get; set; }
            public decimal grossamount { get; set; }
            public decimal deductionamount { get; set; }
            public decimal netamount { get; set; }
            public int stateexpuniqueid { get; set; }
            public int payeecount { get; set; }
            public string paydetail { get; set; }
            public string payeedetails { get; set; }
            public string xmlns { get; set; }
            public string MsgDtTm { get; set; }
            public string MessageId { get; set; }
            public string Source { get; set; }
            public string Destination { get; set; }
            public string StateName { get; set; }
            public string RecordsCount { get; set; }
            public string NetAmountSum { get; set; }

        }
    public class Tracker
    {
       public string executionTime { get; set; }
        public string executionDate { get; set; }
        public string dataType { get; set; }
        public bool status { get; set; }

    }

}
