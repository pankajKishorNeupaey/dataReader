/*
 *Author:Pankaj Kishor Neupaney
 * Created date:07-jul-2020
 * Desc:this files deals with all the function 
 * which is use to track xml insertd records
*/
using SifmsXmlDataReader.DbContext;
using System;
using System.Data;
using XmlConversion;

namespace SifmsXmlDataReader
{
    public class XmlRecordTracker : MyDbContext
    {
        public int GetXmlInsertRecordForCurrentDate()
        {
            int status = 0;
            string date = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            _MyCommand.Clear_CommandParameter();
            _MyCommand.Add_Parameter_WithValue("prm_executiondate", date);
            DataTable dt =_MyCommand.Select_Table("getXmlInsertRecordForCurrentDate",
                CommandType.StoredProcedure);
            if (dt.Rows.Count !=0)
            {
                status = Convert.ToInt32(dt.Rows[0][4]);
            }
            return status;
        }

        public int InsertXmlInsertedRecords( Tracker tracker)
        {
            _MyCommand.Clear_CommandParameter();
            _MyCommand.Add_Parameter_WithValue("prm_execution_time",tracker.executionTime );
            _MyCommand.Add_Parameter_WithValue("prm_execution_date", tracker.executionDate);
            _MyCommand.Add_Parameter_WithValue("prm_data_type", tracker.dataType);
            _MyCommand.Add_Parameter_WithValue("prm_status", tracker.status);
            _MyCommand.Select_Scalar("insertservicesruntimerecords", CommandType.StoredProcedure);
            return 1;
        }
    }
}
