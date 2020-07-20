using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentForBoat.Models;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace RentForBoat.Controllers
{
    public class BoatRequestController : Controller
    {
        // GET: BoatRequest

        //Boat objBoat=new Boat();

        string connStr = ConfigurationManager.ConnectionStrings["dbConn"].ConnectionString;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BoatRequestForm()
        {
            return View("BoatRequestPage");
        }

        [HttpPost]
        public ActionResult BoatRequestForm(Boat objBoat)
        {
            string message = string.Empty;
            string boatNumber = objBoat.BoatNumber;
            decimal rate = Convert.ToDecimal(objBoat.BoatRate);

            string FileName = string.Empty;
            string FileExtension = string.Empty;
            string UploadPath = string.Empty;
            byte[] bytes;

            if (objBoat.ImageFile.ContentLength > 0 && objBoat.ImageFile != null)
            {
                using (BinaryReader br = new BinaryReader(objBoat.ImageFile.InputStream))
                {
                    bytes = br.ReadBytes(objBoat.ImageFile.ContentLength);
                }

                FileName = Path.GetFileNameWithoutExtension(objBoat.ImageFile.FileName);
                FileExtension = Path.GetExtension(objBoat.ImageFile.FileName);

                //FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;
                //UploadPath = ConfigurationManager.AppSettings["UserImagePath"].ToString();
                //objBoat.ImagePath = UploadPath + FileName;
                //objBoat.ImageFile.SaveAs(objBoat.ImagePath);

                // This part also do the WebApi Rest Or Webservice Soap Or WCF 
               
                SqlConnection con = new SqlConnection(connStr);
                SqlCommand cmd = new SqlCommand("BOAT.BOAT_REGISTRATION_REQUEST", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cmd.Parameters.Add("@BOATNUMBER", SqlDbType.VarChar).Value = boatNumber;
                cmd.Parameters.Add("@BOATRATE", SqlDbType.Decimal).Value = rate;
                cmd.Parameters.Add("@IMAGENAME", SqlDbType.VarChar).Value = FileName;
                cmd.Parameters.Add("@IMAGEDATA", SqlDbType.VarBinary).Value = bytes;
                cmd.Parameters.Add("@RESULT", SqlDbType.VarChar, 500);
                cmd.Parameters["@RESULT"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                message = (string)cmd.Parameters["@RESULT"].Value;
                ViewBag.Message = message;
                con.Close();                
            }
            ModelState.Clear();
            return View("BoatRequestPage");
        }

        public ActionResult BoatRequestList()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand sqlComm = new SqlCommand("BOAT.BOAT_REGISTRATION_REQUEST_LIST", conn);
                sqlComm.Parameters.AddWithValue("@BOATNUMBER", "");                
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                da.Fill(ds);
            }
            return View(ds);

        }

        [HttpPost]
        public ActionResult BoatRequestList( string boatNumber)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand sqlComm = new SqlCommand("BOAT.BOAT_REQUEST_AVAILABILTY", conn);
                sqlComm.Parameters.AddWithValue("@BOATNUMBER", boatNumber);
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    ViewBag.Message = " Boat Request is available";
                }
                else
                {
                    ViewBag.Message = "Boat Request is  not available";
                }

            }
            return View(ds);

        }
    }
}