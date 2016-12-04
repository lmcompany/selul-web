using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Net.Mail;

namespace RecordGame_SeloulManager.Services
{
    /// <summary>
    /// Summary description for Manager
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Manager : System.Web.Services.WebService
    {
        [WebMethod]
        public void AddRequest(string Fname,string Lname,string Subject,string Request,string Email,string Phone,
            string companySubject ,string token)
        {
            if (token != "AB==1234fcvghjhk56534rfdgvkljbvn38937510405jgnjdfs=-84j!$%^kgjshcufk")
            {
                return ;
            }
            SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SConnection"].ToString());
            SqlCommand cmm = new SqlCommand();
            try
            {

                cmm.Connection = cnn;
                cnn.Open();
                cmm.CommandText = "INSERT INTO Selul_Request (name,subject,request,companySubject,email,phone) VALUES " +
                    "(@name,@subject,@request,@companySubject,@email,@phone)";
                cmm.Parameters.AddWithValue("name", Fname+" "+Lname);
                cmm.Parameters.AddWithValue("subject", Subject);
                cmm.Parameters.AddWithValue("request", Request);
                cmm.Parameters.AddWithValue("companySubject", companySubject);
                cmm.Parameters.AddWithValue("email", Email);
                cmm.Parameters.AddWithValue("phone", Phone);
                cmm.ExecuteNonQuery();
                cnn.Close();
            }
            catch { }
            /*SmtpClient smtpClient = new SmtpClient("mail.lmco.ir",25);

            smtpClient.Credentials = new System.Net.NetworkCredential("admin@lmco.ir", "Lm123!@#");
            smtpClient.UseDefaultCredentials = false;
            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.;
            
            //smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("admin@lmco.ir");
            mail.To.Add(new MailAddress("info@lmco.ir"));
            //mail.CC.Add(new MailAddress("mahdisaidi86@gmail.com"));
            //mail.CC.Add(new MailAddress("ebrahimsepehr@gmail.com"));
            mail.Body = "یک درخواست جدید از طریق بازی سلول برای شما ارسال شده است"+
                "\n\nنام ارسال کننده : "+ Fname+" "+Lname+
                "\nعنوان : "+Subject+"\nعنوان شرکت : "+companySubject+"\nپست الکترونیکی : "+Email+"\nشماره تلفن : "+Phone+
                "\nمتن درخواست : \n"+Request;
            smtpClient.Send(mail);//*/
        }
        [WebMethod]
        public string BestWeeklyRecords(string token)
        {
            if (token != "AB==1234fcvghjhk56534rfdgvkljbvn38937510405jgnjdfs=-84j!$%^kgjshcufk")
            {
                return "Don't do this kid!!";
            }
            SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SConnection"].ToString());
            SqlCommand cmm = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                cmm.Connection = cnn;
                cmm.CommandText = "select top 20 b.Score,b.Row,a.userName ShownName from"+
                                " (select FKUserId,max(record) Score,Row= Rank() over (order by max(Record) desc),max(AddTime) at"+
                                 " from selul_record group by FKUserID) b inner join selul_user a on a.id=b.FKUserId"+
                                 " WHERE b.at>= DATEADD(day,-7, GETDATE())"+
                                 " order by b.Row,b.at";
                cnn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmm);
                da.Fill(ds);
                cnn.Close();
                return Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
            }
            catch (Exception e)
            { return "error"; }
        }

        [WebMethod]
        public string BestRecords(string token)
        {
            if (token != "AB==1234fcvghjhk56534rfdgvkljbvn38937510405jgnjdfs=-84j!$%^kgjshcufk")
            {
                return "Don't do this kid!!";
            }
            SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SConnection"].ToString());
            SqlCommand cmm = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                cmm.Connection = cnn;
                cmm.CommandText =  " select top 20 b.Score,b.Row,a.userName ShownName from"+
                                    " (select FKUserId,max(record) Score,Row= Rank() over (order by max(Record) desc),max(AddTime) at"+
                                     " from selul_record group by FKUserID) b inner join selul_user a on a.id=b.FKUserId"+
                                     " order by b.Row,b.at";
                cnn.Open();
                
                SqlDataAdapter da = new SqlDataAdapter(cmm);
                da.Fill(ds);
                cnn.Close();
                return Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
            }
            catch(Exception e)
            { return "error"; }

           
        }

        [WebMethod]
        public string AddRecord(string id, string record, string userName, string email, string token)
        {
            if (token != "AB==1234fcvghjhk56534rfdgvkljbvn38937510405jgnjdfs=-84j!$%^kgjshcufk")
            {
                return "Don't do this kid!!";
            }
            SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SConnection"].ToString());
            SqlCommand cmm = new SqlCommand();
            try
            {
                
                cmm.Connection = cnn;
                cnn.Open();
                cmm.CommandText = "UPDATE Selul_User set userName = @userName,email=@email WHERE id=@id";
                cmm.Parameters.AddWithValue("userName", userName);
                cmm.Parameters.AddWithValue("email", email);
                cmm.Parameters.AddWithValue("id", id);
                cmm.ExecuteNonQuery();
                cmm = new SqlCommand();
                cmm.Connection = cnn;
                cmm.CommandText = "INSERT INTO Selul_Record (FKuserId,record,ShownName) VALUES " +
                    "(@FKuserId,@record,@ShownName)";
                cmm.Parameters.AddWithValue("FKuserId", id);
                cmm.Parameters.AddWithValue("record", record);
                cmm.Parameters.AddWithValue("ShownName", userName);
                cmm.ExecuteNonQuery();

                cmm = new SqlCommand();
                cmm.Connection = cnn;
                cmm.CommandText = "SELECT a.row FROM (SELECT b.FKuserId,row = ROW_NUMBER() over (order by b.d desc))" +
                    " FROM (SELECT FKuserId,max(record) d FROM Selul_Record GROUP BY FKuserId) b) a WHERE a.FKuserId = @FKuserId";
                cmm.Parameters.AddWithValue("FKuserId", id);
                object rank = cmm.ExecuteScalar();

                cnn.Close();
                return rank.ToString();
            }
            catch
            {
                return "";
            }
        }

        [WebMethod]
        public string GetNotification(string token,string id)
        {
            if (token != "AB==1234fcvghjhk56534rfdgvkljbvn38937510405jgnjdfs=-84j!$%^kgjshcufk")
            {
                return "Don't do this kid!!";
            }
            SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SConnection"].ToString());
            SqlCommand cmm = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                cmm.Connection = cnn;
                cmm.CommandText = "SELECT top 1 noti_content From Selul_Notification WHERE isSent=0 and FKUserId=@FKUserId "+
                    "update top(1) Selul_Notification set isSent=1 where  FKUserId=@FKUserId and isSent = 0";
                    
                cmm.Parameters.AddWithValue("FKUserId", id);
                cnn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmm);
                da.Fill(ds);
                cnn.Close();
                if( ds.Tables[0].Rows.Count == 0)
                {
                    return "";
                }
                else
                {
                    return ds.Tables[0].Rows[0].ItemArray[0].ToString();
                }                
            }
            catch (Exception e)
            { return "error"; }


        }

        [WebMethod]
        public string AddUser(string UserName, string Record, string Email, string PhoneNumber, string token)
        {
            if (token != "AB==1234fcvghjhk56534rfdgvkljbvn38937510405jgnjdfs=-84j!$%^kgjshcufk")
            {
                return "Don't do this kid!!";
            }
            SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SConnection"].ToString());
            SqlCommand cmm = new SqlCommand();
            cmm.Connection = cnn;
            cmm.CommandText = "INSERT INTO Selul_User (userName,email,phone) output inserted.id VALUES " +
                "(@userName,@email,@phone)";
            cmm.Parameters.AddWithValue("userName", UserName);
            cmm.Parameters.AddWithValue("email", Email);
            cmm.Parameters.AddWithValue("phone", PhoneNumber);
            try
            {
                cnn.Open();
                object id = cmm.ExecuteScalar();
                cmm = new SqlCommand();
                cmm.Connection = cnn;
                cmm.CommandText = "INSERT INTO Selul_Record (FKuserId,record,ShownName) VALUES "+
                    "(@FKuserId,@record,@ShownName)";
                cmm.Parameters.AddWithValue("FKuserId", id);
                cmm.Parameters.AddWithValue("record", Record);
                cmm.Parameters.AddWithValue("ShownName", UserName);
                cmm.ExecuteNonQuery();

                cnn.Close();
                return id.ToString();
            }
            catch
            {
                return "";
            }
            

        }
    }
}
