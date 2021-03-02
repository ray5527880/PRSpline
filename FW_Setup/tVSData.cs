using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace BF_FW
{

    public class tVSData
    {
        public struct VSData
        {
            public int Year;
            public int MD;
            public int HM;
            public int SS;
            public decimal DUR;
            public decimal V1;
            public decimal V2;
            public decimal V3;
            public decimal Down;
            public decimal Cycle;
            public int Type;
        }
        string _SqlConnectionAlarms;
        //public VSData VSDatas = new VSData();
        public tVSData( string SqlConnectionAlarms)
        {
            _SqlConnectionAlarms = SqlConnectionAlarms;
        }
        
        public void AddData(VSData vSData,int IED_ID)
        {
            string strSQL = "Insert into [dbo].[VSData]"
                         + "([YEAR] ,[MD] ,[HM] ,[SS] ,[DUR],[V1],[V2],[V3],[DOWN],[CYCLE],[TYPE],[IEDID])"
                         + " values(@V1,@V2,@V3,@V4,@V5,@V6,@V7,@V8,@V9,@V10,@V11,@V12)";
            using (var sqlConn = new SqlConnection(_SqlConnectionAlarms))
            {
                sqlConn.Open();
                using (SqlCommand sqlCmd = new SqlCommand(strSQL, sqlConn))
                {
                    sqlCmd.Parameters.AddWithValue("@V1", vSData.Year);
                    sqlCmd.Parameters.AddWithValue("@V2",vSData.MD);
                    sqlCmd.Parameters.AddWithValue("@V3", vSData.HM);
                    sqlCmd.Parameters.AddWithValue("@V4", vSData.SS);
                    sqlCmd.Parameters.AddWithValue("@V5", vSData.DUR);
                    sqlCmd.Parameters.AddWithValue("@V6", vSData.V1);
                    sqlCmd.Parameters.AddWithValue("@V7", vSData.V2);
                    sqlCmd.Parameters.AddWithValue("@V8", vSData.V3);
                    sqlCmd.Parameters.AddWithValue("@V9", vSData.Down);
                    sqlCmd.Parameters.AddWithValue("@V10", vSData.Cycle);
                    sqlCmd.Parameters.AddWithValue("@V11", vSData.Type);
                    sqlCmd.Parameters.AddWithValue("@V12", IED_ID);

                    sqlCmd.ExecuteNonQuery();
                }
            }
        }

    }
}
