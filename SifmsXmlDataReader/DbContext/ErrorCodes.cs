using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace SifmsXmlDataReader.DbContext
{
    class ErrorCodes
    {
        static string GetErrorCode(string ElementType, string CodeClass, string CodeMethod, string CustomMsg, Exception ex)
        {
            //var dd = Path.Combine(Directory.GetCurrentDirectory());
            XDocument document = XDocument.Load(Path.Combine(Directory.GetCurrentDirectory(), "ErrorCode.xml"));
            var values = document.Descendants("Codes").Where(i => i.Element("Type").Value == ElementType.Trim() && i.Element("Class").Value == CodeClass.Trim() && i.Element("Method").Value == CodeMethod.Trim()).Select(i => i.Element("Value").Value).Distinct();

            if (CustomMsg != "")
                // return "Source#" + values.ToList()[0].ToString() + ": " + CustomMsg + ">" + ex.Message; 
                return CustomMsg + ": " + "Source#" + values.ToList()[0].ToString() + ">" + ex.Message;
            else
                return "Source#" + values.ToList()[0].ToString() + ": We are experiencing server error while processing your request. Inconvenience is regretted." + ">" + ex.Message; ;
        }
        public static string ProcessException(Exception ex, string ElementType, string CodeClass, string CodeMethod, string CustomMsg = "")
        {
            if (ex.Message.Contains("Source#DAL("))
                return ex.Message;
            else if (ex.Message.Contains("Source#BAL("))
                return ex.Message;
            else if (ex.Message.Contains("Source#API("))
                return ex.Message;
            //else if (ex.Message.Contains("Source#UI("))
            //    return ex.Message;
            else
                return GetErrorCode(ElementType, CodeClass, CodeMethod, CustomMsg, ex);
        }
        public static string MySqlExceptionMsg(MySqlException ex)
        {
            if (ex.Number == 1045)
                return "Database does not exist";
            else if (ex.Number == 1048)
                // error: "Column value can not be null";
                return "Please fill in the mandatory fields. Column value cannot be null";
            else if (ex.Number == 1054)
                // return "unknown column in sql query";
                return "The specified method for the request is invalid";
            else if (ex.Number == 1062)
                return "The value was already entered.All entries must be unique";
            else if (ex.Number == 1064)
                //return "unknown command in mysql query";
                return "The specified method for the request is invalid";
            else if (ex.Number == 1109)
                //return "unknown table in mysql query";
                return "Table does not exists.";
            else if (ex.Number == 1146)
                return "any table, column name does not exist";
            else if (ex.Number == 1215)
                //return "Cannot add foreign key constraints";
                return "The specified method for the request is invalid.Cannot add foreign key constraints";
            else if (ex.Number == 1241)
                return "operand should contain 1 column";
            else if (ex.Number == 1318)
                //  return "incorrect number of argument in store procedure";
                return "Please provide all the required fields.";
            else if (ex.Number == 1451)
                //return "Cannot delete or update a parent row: a foreign key constraint fails";
                return "Please provide all the required fields.";
            else if (ex.Number == 1452)
                //return "Cannot add or update a child row: a foreign key constraint fails";
                return "Please provide all the required fields.";
            else
                return "";
        }
    }
}
