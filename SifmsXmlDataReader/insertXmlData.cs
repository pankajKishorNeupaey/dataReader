/*
 *Author:Pankaj Kishor Neupaney
 * Created date:07-jul-2020
 * Desc:insert xml files in database
*/
using SifmsXmlDataReader.DbContext;
using SifmsXmlDataReader.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using XmlConversion;

namespace SifmsXmlDataReader
{
    public class insertXmlData: MyDbContext
    {
         public int StoreExpenditureXmlToDB( string path,EmailSettings emailSettings)
        {

            MySql.Data.MySqlClient.MySqlTransaction myTransaction = null;
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(path);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmldoc.NameTable);
            DataSet ds = new DataSet();
            XmlReader xmlFile;
            xmlFile = XmlReader.Create(path, new XmlReaderSettings());
            ds.ReadXml(xmlFile);
            XmlAttributeCollection attrColl = xmldoc.DocumentElement.Attributes;
            var xmlnsattribute = attrColl.GetNamedItem("xmlns").Value;
            var MsgDtTmattribute = attrColl.GetNamedItem("MsgDtTm").Value;
            var MessageIdattribute = attrColl.GetNamedItem("MessageId").Value;
            var Sourceattribute = attrColl.GetNamedItem("Source").Value;
            var Destinationattribute = attrColl.GetNamedItem("Destination").Value;
            var StateNameattribute = attrColl.GetNamedItem("StateName").Value;
            var RecordsCountattribute = attrColl.GetNamedItem("RecordsCount").Value;
            var NetAmountSumattribute = attrColl.GetNamedItem("NetAmountSum").Value;
            List<Expenditure> strli = new List<Expenditure>();

            foreach (DataRow dr in ds.Tables[1].Rows)
            {

                strli.Add(new Expenditure
                {
                    fin_year1 = Convert.ToInt32(dr[0].ToString()),
                    fin_year2 = Convert.ToInt32(dr[1].ToString()),
                    statecode = Convert.ToInt32(dr[2].ToString()),
                    budgetcode = Convert.ToInt64(dr[3].ToString()),
                    objectheadcode = dr[4].ToString(),
                    ddocode = dr[5].ToString(),
                    vouchernumber = Convert.ToInt32(dr[6].ToString()),
                    date = DateTime.ParseExact(dr[7].ToString(), "dd/MM/yyyy",
                    System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat),
                    grossamount = Convert.ToDecimal(dr[8].ToString()),
                    deductionamount = Convert.ToDecimal(dr[9].ToString()),
                    netamount = Convert.ToDecimal(dr[10].ToString()),
                    stateexpuniqueid = Convert.ToInt32(dr[11].ToString()),
                    payeecount = Convert.ToInt32(dr[12].ToString()),
                    paydetail = dr[13].ToString(),
                    payeedetails = dr[14].ToString(),
                    xmlns = Convert.ToString(xmlnsattribute),
                    MsgDtTm = Convert.ToString(MsgDtTmattribute),
                    MessageId = Convert.ToString(MessageIdattribute),
                    Source = Convert.ToString(Sourceattribute),
                    Destination = Convert.ToString(Destinationattribute),
                    StateName = Convert.ToString(StateNameattribute),
                    RecordsCount = Convert.ToString(RecordsCountattribute),
                    NetAmountSum = Convert.ToString(NetAmountSumattribute),
                });

            }
            myTransaction = _MyConnection._MyConnection.BeginTransaction(IsolationLevel.ReadUncommitted);
            _MyCommand.Add_Transaction(myTransaction);
            foreach (var expenditure in strli)
            {
                _MyCommand.Clear_CommandParameter();
                _MyCommand.Add_Parameter_WithValue("prm_fin_year1", expenditure.fin_year1);
                _MyCommand.Add_Parameter_WithValue("prm_fin_year2", expenditure.fin_year2);
                _MyCommand.Add_Parameter_WithValue("prm_statecode", expenditure.statecode);
                _MyCommand.Add_Parameter_WithValue("prm_budgetcode", expenditure.budgetcode);
                _MyCommand.Add_Parameter_WithValue("prm_objectheadcode", expenditure.objectheadcode);
                _MyCommand.Add_Parameter_WithValue("prm_ddocode", expenditure.ddocode);
                _MyCommand.Add_Parameter_WithValue("prm_vouchernumber", expenditure.vouchernumber);
                _MyCommand.Add_Parameter_WithValue("prm_date", expenditure.date);
                _MyCommand.Add_Parameter_WithValue("prm_grossamount", expenditure.grossamount);
                _MyCommand.Add_Parameter_WithValue("prm_deductionamount", expenditure.deductionamount);
                _MyCommand.Add_Parameter_WithValue("prm_netamount", expenditure.netamount);
                _MyCommand.Add_Parameter_WithValue("prm_stateexpuniqueid", expenditure.stateexpuniqueid);
                _MyCommand.Add_Parameter_WithValue("prm_payeecount", expenditure.payeecount);
                _MyCommand.Add_Parameter_WithValue("prm_paydetail", expenditure.paydetail);
                _MyCommand.Add_Parameter_WithValue("prm_payeedetails", expenditure.payeedetails);
                //_MyCommand.Add_Parameter_WithValue("", expenditure.xmlns);
                _MyCommand.Add_Parameter_WithValue("prm_msgdttm", expenditure.MsgDtTm);
                _MyCommand.Add_Parameter_WithValue("prm_messageid", expenditure.MessageId);
                _MyCommand.Add_Parameter_WithValue("prm_source", expenditure.Source);
                _MyCommand.Add_Parameter_WithValue("prm_destination", expenditure.Destination);
                _MyCommand.Add_Parameter_WithValue("prm_statename", expenditure.StateName);
                _MyCommand.Add_Parameter_WithValue("prm_recordscount", expenditure.RecordsCount);
                _MyCommand.Add_Parameter_WithValue("prm_netamountsum", expenditure.NetAmountSum);
                _MyCommand.Select_Scalar("insertsifmsexpendituredetails", System.Data.CommandType.StoredProcedure);

            }
            myTransaction.Commit();
            int ab = 1;
            SendEmails mail = new SendEmails();
            mail.mailer(emailSettings);
            return ab;
        }
    }
}
