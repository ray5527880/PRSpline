using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace BF_FW
{
    public class tVSIEDName
    {
        public struct IEDName
        {
            public int ID;
            public string Name;
        }
        private string _SqlConnectionAlarms;
        public tVSIEDName(string SqlConnectionAlarms)
        {
            _SqlConnectionAlarms = SqlConnectionAlarms;
        }
        public IEDName GetData(int id)
        {
            IEDName reData = new IEDName();

            using (var sqlConn = new SqlConnection(_SqlConnectionAlarms))
            {
                sqlConn.Open();
                string str = string.Format("SELECT TOP(1)[ID],[Name] FROM [dbo].[tVSIEDName] WHERE [ID]={0}", id.ToString());
                using (var sqlCmd=new SqlCommand(str, sqlConn))
                {
                    using (var sqlRdr = sqlCmd.ExecuteReader())
                    {
                        while (sqlRdr.Read())
                        {
                            reData.ID = sqlRdr.IsDBNull(0) ? 0 : sqlRdr.GetInt32(0);
                            reData.Name = sqlRdr.IsDBNull(1) ? "" : sqlRdr.GetString(1);
                        }
                    }
                }
            }
            return reData;
        }
        public IEDName GetData(string name)
        {
            IEDName reData = new IEDName();

            using (var sqlConn = new SqlConnection(_SqlConnectionAlarms))
            {
                sqlConn.Open();
                string str = string.Format("SELECT TOP(1)[ID],[Name] FROM [dbo].[tVSIEDName] WHERE [Name]='{0}'", name);
                using (var sqlCmd = new SqlCommand(str, sqlConn))
                {
                    using (var sqlRdr = sqlCmd.ExecuteReader())
                    {
                        while (sqlRdr.Read())
                        {
                            reData.ID = sqlRdr.IsDBNull(0) ? 0 : sqlRdr.GetInt32(0);
                            reData.Name = sqlRdr.IsDBNull(1) ? "" : sqlRdr.GetString(1);
                        }
                    }
                }
            }
            return reData;
        }
        public bool AddData(string name)
        {
            bool reValue = false;
            using (var sqlConn = new SqlConnection(_SqlConnectionAlarms))
            {
                sqlConn.Open();
                string str = string.Format("Insert into [dbo].[tVSIEDName] ([Name]) values(@V1)");
                using (var sqlCmd=new SqlCommand(str, sqlConn))
                {                    
                    sqlCmd.Parameters.AddWithValue("@V1", name);
                    sqlCmd.ExecuteNonQuery();
                    reValue = true;
                }
            }
            return reValue;
        }
        public bool DeleteData(string name)
        {
            bool reValue = false;
            using (var sqlConn=new SqlConnection(_SqlConnectionAlarms))
            {
                sqlConn.Open();
                string str = string.Format("DELETE [dbo].[tVSIEDName] WHERE [Name]='{0}'", name);
                using (var sqlCmd=new SqlCommand(str, sqlConn))
                {
                    sqlCmd.ExecuteNonQuery();
                    reValue = true;
                }
            }
            return reValue;
        }
        public bool UpDateData(IEDName data, string name)
        {
            bool reValue = false;
            using(var sqlConn=new SqlConnection(_SqlConnectionAlarms))
            {
                sqlConn.Open();
                string str = string.Format("UPDATE [dbo].[tVSIEDName] SET [Name]='{0}' WHERE [Name]='{1}'", name, data.Name);
                using(var sqlCmd=new SqlCommand(str, sqlConn))
                {
                    sqlCmd.ExecuteNonQuery();
                    reValue = true;
                }
            }
            return reValue;
        }
    }
}
