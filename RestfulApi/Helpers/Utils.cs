using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace WebApiDemo.Helpers
{
    public class Utils {


        public const string motif = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
        public static string CStr(object objVariable, string objDefault)
        {
            if (objVariable == null)
                return objDefault;
            else
            {
                try
                {
                    string sTemp = Convert.ToString(objVariable);
                    return sTemp.Trim();
                }
                catch (Exception ex)
                {
                    return objDefault;
                }
            }
        }

        public static string CStr(object objVariable)
        {
            return CStr(objVariable, String.Empty);
        }
        public static bool isDataSetAvailable(DataSet ObjDataSet)
        {
            if (ObjDataSet != null && ObjDataSet.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool IsIntZeroOrNull(int? value)
        {
            if (value == null) return true;

            if (value == 0) return true;

            return false;
        }
        public static bool IsStringEmptyOrNull(string value)
        {
            bool hasValue = false;

            if (String.IsNullOrEmpty(value)) { hasValue = true; }

            return hasValue;

        }


        public static int CInt(object obj)
        {
            try
            {
                if (int.TryParse(obj.ToString(), out int iRet))
                {
                    return iRet;
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return 0;
        }


        public static bool IsBlank(object objVariable)
        {
            if (IsNull(objVariable))
                return true;
            else
                return (objVariable.ToString().Trim().Length == 0);
        }

        public static bool IsNull(object objVariable)
        {
            if (objVariable == null || objVariable == DBNull.Value)
                return true;
            else
                return (objVariable is DBNull);
        }

        public static string CProductAvibility(object objVariable)
        {
            string value = CStr(objVariable, String.Empty);

            if(value == "1")
            {
                return "Yes";
            }
            return "No";
        }

        public static bool IsEmail(string value)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(value);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }

        public static bool IsPhoneNo(string value)
        {
            if (value != null) return Regex.IsMatch(value, motif);
            else return false;
        }
    }

    

}