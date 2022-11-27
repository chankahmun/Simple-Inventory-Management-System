using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApiDemo.Helpers
{
    public enum ExecuteType
    {
        Text = 1,
        StoreProcedure = 2,
        TableDirect = 3
    }

    public class DbHelper
    {
        protected SqlConnection conn;
        protected SqlCommand cmd;
        protected SqlDataAdapter da;
        protected string sConnectionString;

        
        #region public constructor

        public DbHelper()
        {
            try
            {
                sConnectionString = ConfigurationManager.ConnectionStrings["SqlServerConnectionString"].ToString();
                
                conn = new SqlConnection(sConnectionString);
            }
            catch (Exception ex)
            {
                
            }
        }

        #endregion

        #region public Method   
        public bool Select(ExecuteType exeType, string sSQL, Hashtable htParams, out DataSet dsOutput, out string sMessage)
        {
            dsOutput = null;
            sMessage = string.Empty;
            bool bReturnValue = false;

            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandTimeout = 60;
                cmd.CommandType = getCommandType(exeType);
                cmd.CommandText = sSQL;
                if (htParams != null)
                {
                    cmd.Parameters.AddRange(getParameterCollection(htParams));
                }

                da = new SqlDataAdapter(cmd);
                dsOutput = new DataSet();
                da.Fill(dsOutput);

                bReturnValue = true;
            }
            catch (Exception ex)
            {
               
                sMessage = ex.Message;
            }
            finally
            {
                if (da != null) da.Dispose();
                if (cmd != null) cmd.Dispose();
                if (conn != null) conn.Close();
            }

            return bReturnValue;
        }

        public int Update(ExecuteType exeType, string sSQL, Hashtable htParams, out string sMessage)
        {
            int iRetVal = 1;
            int iRowEffected = 0;
            sMessage = string.Empty;

            try
            {
                cmd = new SqlCommand();
                cmd.Connection = conn;
                conn.Open();
                cmd.CommandType = getCommandType(exeType);
                cmd.CommandText = sSQL;
                if (htParams != null)
                {
                    cmd.Parameters.AddRange(getParameterCollection(htParams));
                }

                iRowEffected = cmd.ExecuteNonQuery();

                if (iRowEffected > 0)
                {
                    iRetVal = 0;
                    sMessage = "Success";
                }

            }
            catch (Exception ex)
            {
                
                sMessage = ex.Message;
                iRetVal = -1;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (conn != null) conn.Close();
            }

            return iRetVal;
        }
        #endregion

        #region Private Method

        private SqlParameter[] getParameterCollection(Hashtable htParams)
        {
            SqlParameter[] sqlParams = new SqlParameter[htParams.Count];
            int i = 0;
            //IDictionaryEnumerator enumerator = htParams.GetEnumerator();
            string sKey = string.Empty;

            foreach (DictionaryEntry de in htParams)
            {
                sKey = de.Key.ToString();
                mtdCheckKey(ref sKey);

                if (Utils.IsNull(de.Value) || Utils.IsBlank(de.Value))
                {
                    sqlParams[i] = new SqlParameter(sKey, DBNull.Value);
                }
                else
                {
                    sqlParams[i] = new SqlParameter(sKey, de.Value);
                }
                i++;
            }

            return sqlParams;
        }

        private void mtdCheckKey(ref string sKey)
        {
            char[] aryKey;

            if (!sKey.StartsWith("@"))
            {
                aryKey = sKey.ToCharArray();
                sKey = "@";
                for (int i = 0; i < aryKey.Length; i++)
                {
                    sKey += aryKey[i];
                }
            }
        }

        private void BindReturnValue(SqlDataReader reader, ref Hashtable htReturnObj)
        {
            int iFieldCount = 0;
            iFieldCount = reader.FieldCount;

            for (int i = 0; i < iFieldCount; i++)
            {
                htReturnObj.Add(reader.GetName(i), reader.GetValue(i));
            }

        }


        private CommandType getCommandType(ExecuteType exeType)
        {
            switch (exeType)
            {
                case ExecuteType.Text:
                    return CommandType.Text;
                case ExecuteType.StoreProcedure:
                    return CommandType.StoredProcedure;
                case ExecuteType.TableDirect:
                    return CommandType.TableDirect;
                default: return CommandType.Text;
            }
        }

        private string getConnectionString()
        {
            string sConn = string.Empty;

            return sConn;
        }

        #endregion
    }
}

