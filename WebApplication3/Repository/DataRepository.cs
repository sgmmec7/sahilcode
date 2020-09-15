using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication3.Interfaces;
using WebApplication3.Models;

namespace WebApplication3.Repository
{
    public class DataRepository : IDataRepository
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        SqlDataAdapter dad;
        SqlCommand cmd;
        DataTable dtbl;

        public int getReq()
        {
            try
            {
                string strQry = "Select ISNULL(Max(RequestId),0) as reqId from tblRequest";
                cmd = new SqlCommand(strQry, con);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                int number = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                if (number == 0)
                {
                    return 1;
                }
                else
                {
                    int value = number + 1;
                    return value;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public string LoginUser(string userId, string password)
        {
            try
            {
                string uid = string.Empty;
                string pwd = string.Empty;
                string strQry = "Select UserId,Password from tblLogin where UserId = @UserId";
                dad = new SqlDataAdapter(strQry, con);
                dad.SelectCommand.Parameters.Add("@UserId", SqlDbType.VarChar, 50).Value = userId;
                dtbl = new DataTable();
                dad.Fill(dtbl);
                if (dtbl.Rows.Count > 0)
                {
                    uid = Convert.ToString(dtbl.Rows[0]["UserId"]);
                }
                if (uid == userId)
                {
                    pwd = Convert.ToString(dtbl.Rows[0]["Password"]);
                    if (password == pwd)
                    {
                        return "login";
                    }
                    else
                    {
                        return "not login";
                    }
                }
                else
                {
                    return "not login";
                }
            }
            catch (Exception ex)
            {

                return "error";
            }
        }

        public string InsertBoatInformation(clsBoat objBoat)
        {
            try
            {
                cmd = new SqlCommand();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spInsertData";
                cmd.Parameters.Add("@reqId", SqlDbType.Int).Value = objBoat.reqId;
                cmd.Parameters.Add("@cName", SqlDbType.VarChar, 50).Value = Convert.ToString(HttpContext.Current.Session["UserId"]);
                cmd.Parameters.Add("@hRate", SqlDbType.Int).Value = objBoat.hRate;
                cmd.Parameters.Add("@BoatName", SqlDbType.VarChar, 50).Value = objBoat.BName;

                SqlParameter param = new SqlParameter("@BoatId", SqlDbType.Int);
                //SqlParameter param = new SqlParameter("@BoatId", SqlDbType.VarChar, -1);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
                int boatId = Convert.ToInt32(param.Value);
                //string boatId = Convert.ToString(param.Value);
                con.Close();

                if (boatId == -1)
                {
                    return "error";
                }
                else
                {
                    return Convert.ToString(boatId);
                }
                //return "";
            }
            catch (Exception ex)
            {
                return "error";

            }
        }

        public string retrievBoatStatus(clsBoat objBoat)
        {
            try
            {
                cmd = new SqlCommand();
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spGetData";
                cmd.Parameters.Add("@reqId", SqlDbType.Int).Value = objBoat.reqId;
                cmd.Parameters.Add("@cName", SqlDbType.VarChar, 50).Value = Convert.ToString(HttpContext.Current.Session["UserId"]);
                cmd.Parameters.Add("@BoatNo", SqlDbType.VarChar, 50).Value = objBoat.BNo;

                dad = new SqlDataAdapter();
                dad.SelectCommand = cmd;
                dtbl = new DataTable();
                dad.Fill(dtbl);
                con.Close();
                if (dtbl.Rows.Count > 0)
                {
                    string status = Convert.ToString(dtbl.Rows[0]["Status"]);
                    if(status == "NC")
                    {
                        return "boat is not booked";
                    }
                    else if (status == "CN")
                    {
                        return "boat is booked";
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "no record found";
                }
            }
            catch (Exception ex)
            {
                return "error";

            }
        }
    }
}