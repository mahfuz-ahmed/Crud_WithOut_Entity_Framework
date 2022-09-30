using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRUD_Complete_WithOutEntity.Models;

namespace CRUD_Complete_WithOutEntity.Controllers
{
    public class CrudConController : Controller
    {

        private SqlConnection con;

        public void connection()
        {
            string conStr = ConfigurationManager.ConnectionStrings["connection"].ToString();
            con = new SqlConnection(conStr);
        }

        // GET: CrudCon
        public ActionResult Index()
        {        
            return View();
        }

        [HttpPost]

        public ActionResult InsertUser(CrudModel inModel)
        {
            connection();
            string query = "INSERT INTO UserInformation (userName, contactNumber, countryName, cityName) VALUES (@userName, @contactNumber, @countryName, @cityName)";
            using(SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@userName", inModel.userName);
                cmd.Parameters.AddWithValue("@contactNumber", inModel.contactNumber);
                cmd.Parameters.AddWithValue("@countryName", inModel.countryName);
                cmd.Parameters.AddWithValue("@cityName", inModel.cityName);
                cmd.ExecuteNonQuery();
            }
            return View("Index");
        }



        [HttpGet]
        public ActionResult selectUser()
        {
            connection();
            List<CrudModel> allList = new List<CrudModel>();
            string query = "select * from UserInformation";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                SqlDataReader dtRd = cmd.ExecuteReader();
                while (dtRd.Read())
                {
                    var modelDt = new CrudModel();
                    modelDt.UserId = Convert.ToInt32(dtRd["UserId"]);
                    modelDt.userName = dtRd["userName"].ToString();
                    modelDt.contactNumber = dtRd["contactNumber"].ToString();
                    modelDt.countryName = dtRd["countryName"].ToString();
                    modelDt.cityName = dtRd["cityName"].ToString();
                    allList.Add(modelDt);
                }
            }
            return View(allList);
        }

        //GET : UpdateUser/
        [HttpGet]
        public ActionResult userUpdate(int id)
        {
            connection();
            CrudModel crdModel = new CrudModel();
            DataTable dtTbl = new DataTable();
            string quary = "select * from UserInformation where UserId = @UserId";
            using(SqlDataAdapter sda = new SqlDataAdapter(quary, con))
            {
                sda.SelectCommand.Parameters.AddWithValue("@UserId", id);
                sda.Fill(dtTbl);
            }
            if(dtTbl.Rows.Count >0)
            {
                crdModel.UserId = Convert.ToInt32(dtTbl.Rows[0][0].ToString());
                crdModel.userName = dtTbl.Rows[0][1].ToString();
                crdModel.contactNumber = dtTbl.Rows[0][2].ToString();
                crdModel.countryName = dtTbl.Rows[0][3].ToString();
                crdModel.cityName = dtTbl.Rows[0][4].ToString();
                return View(crdModel);
            }
            else
            return RedirectToAction("selectUser");
        }
        [HttpPost]
        public ActionResult userUpdate(CrudModel rcvUpdt)
        {
            connection();
            string query = @"UPDATE UserInformation
                        SET userName = @userName
                        , contactNumber = @contactNumber
                         , countryName = @countryName
                        , cityName = @cityName
                         WHERE UserId = @UserId";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@UserId",rcvUpdt.UserId);
                cmd.Parameters.AddWithValue("@userName", rcvUpdt.userName);
                cmd.Parameters.AddWithValue("@contactNumber", rcvUpdt.contactNumber);
                cmd.Parameters.AddWithValue("@countryName", rcvUpdt.countryName);
                cmd.Parameters.AddWithValue("@cityName", rcvUpdt.cityName);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("selectUser");
        }

        public ActionResult userDelete(int id)
        {
            connection();
            string query = "DELETE FROM UserInformation WHERE UserId = @UserId";
            using(SqlCommand cmd = new SqlCommand(query,con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@UserId", id);
                cmd.ExecuteNonQuery();
                con.Close();
            }

            return RedirectToAction("selectUser");            
        }    
    }
}