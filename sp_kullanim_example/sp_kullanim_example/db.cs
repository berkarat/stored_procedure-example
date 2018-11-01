using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sp_kullanim_example
{
    class db
    {

        public static SqlConnection sqldbsconnect ()
        {
            var hklm = RegistryKey.OpenBaseKey (RegistryHive.LocalMachine, RegistryView.Registry64);
            // WİN32 çalışıyor RECYCLE        !!!!!!!!!!!!!!!!!!! DİİKAT ET !!!!!!!!!!
            var key = hklm.OpenSubKey (@"SOFTWARE\berkarat\sql");


            //  string constring = Settings.Default.dbconnection.ToString ();

            string constring = key.GetValue ("dbconnection").ToString ();
            SqlConnection con = new SqlConnection (constring);

            try
            {


                con.Open ();

            }
            catch (Exception)
            {

                //  MessageBox.Show(" Server Bağlantı Sorunu ", "UYARI", MessageBoxButtons.OK);
                //EventLogaYaz.HataMesajiYaz(ex, 0);

            }

            return con;
        }
        public static DataTable sp_select (string sp_name, out string _error)
        {
            DataTable ds = new DataTable ();
            _error=null;
            try
            {
                using (SqlConnection con = sqldbsconnect ())
                {
                    if (con.State.ToString ()=="Open")
                    {
                        SqlCommand sqlComm = new SqlCommand (sp_name, con);

                        sqlComm.CommandType=CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter ();
                        da.SelectCommand=sqlComm;

                        da.Fill (ds);
                    }
                    else
                    {
                        _error="DB SERVER CONNECTION = "+con.State.ToString ();
                    }
                }

            }
            catch (Exception ex)
            {

                _error=ex.ToString ();
            }
            return ds;
        }
        public static bool sp_insert (string sp_name, string ad, string soyad, string parola, int yas, string email, out string _error)
        {

            DataSet ds = new DataSet ();
            _error=null;
            bool xs = true;

            ds=check ();
            List<string> l = new List<string> ();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                //     l.Add (item["ad"].ToString ());
                if (item["email"].ToString ()==email)
                {
                    xs=false;
                }
            }




            try
            {
                using (SqlConnection con = sqldbsconnect ())
                {
                    if (con.State.ToString ()=="Open"&&xs==true)
                    {


                        SqlCommand sqlComm = new SqlCommand (sp_name, con);
                        sqlComm.Parameters.AddWithValue ("ad", ad);
                        sqlComm.Parameters.AddWithValue ("soyad", soyad);
                        sqlComm.Parameters.AddWithValue ("yas", yas);
                        sqlComm.Parameters.AddWithValue ("password", parola);
                        sqlComm.Parameters.AddWithValue ("email", email);
                        sqlComm.CommandType=CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter ();
                        da.SelectCommand=sqlComm;

                        da.Fill (ds);
                        return true;
                    }
                    else
                    {
                        if (xs==false)
                        {
                            _error="KULLANICI ZATEN KAYITLI ";
                            return false;
                        }
                        _error="DB SERVER CONNECTION = "+con.State.ToString ();
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                if (xs==false)
                {
                    _error="KULLANICI ZATEN KAYITLI ";
                }
                _error=ex.ToString ();
                return false;

            }

        }
        public static bool sp_update (string sp_name, string ad, string soyad, string parola, string email, string newpassw, out string _error)
        {

            DataSet ds = new DataSet ();
            _error=null;
            bool xs = true;

            ds=check ();
            List<string> l = new List<string> ();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                //     l.Add (item["ad"].ToString ());
                if (item["email"].ToString ()==email)
                {
                    xs=false;
                }
            }


            if (!xs)
            {
                try
                {
                    using (SqlConnection con = sqldbsconnect ())
                    {
                        if (con.State.ToString ()=="Open"&&xs==false)
                        {


                            SqlCommand sqlComm = new SqlCommand (sp_name, con);
                            sqlComm.Parameters.AddWithValue ("ad", ad);
                            sqlComm.Parameters.AddWithValue ("soyad", soyad);

                            sqlComm.Parameters.AddWithValue ("password", parola);
                            sqlComm.Parameters.AddWithValue ("email", email);
                            sqlComm.Parameters.AddWithValue ("newpassword", newpassw);

                            sqlComm.CommandType=CommandType.StoredProcedure;
                            SqlDataAdapter da = new SqlDataAdapter ();
                            da.SelectCommand=sqlComm;

                            da.Fill (ds);
                            return true;
                        }
                        else
                        {
                            _error="DB SERVER CONNECTION = "+con.State.ToString ();
                            return false;
                        }
                    }

                }
                catch (Exception ex)
                {

                    return false;
                }

            }
            else
            {

                _error="KULLANICI  YOK !  ";

                return false;
            }


        }
        public static bool sp_delete (string sp_name, string ad, string soyad, string email, out string _error)
        {

            DataSet ds = new DataSet ();
            _error=null;
            bool xs = true;

            ds=check ();
            List<string> l = new List<string> ();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                //     l.Add (item["ad"].ToString ());
                if (item["email"].ToString ()==email)
                {
                    xs=false;
                }
            }


            if (!xs)
            {
                try
                {
                    using (SqlConnection con = sqldbsconnect ())
                    {
                        if (con.State.ToString ()=="Open"&&xs==false)
                        {


                            SqlCommand sqlComm = new SqlCommand (sp_name, con);
                            sqlComm.Parameters.AddWithValue ("ad", ad);
                            sqlComm.Parameters.AddWithValue ("soyad", soyad);
                            sqlComm.Parameters.AddWithValue ("email", email);
                            sqlComm.CommandType=CommandType.StoredProcedure;
                            SqlDataAdapter da = new SqlDataAdapter ();
                            da.SelectCommand=sqlComm;

                            da.Fill (ds);
                            return true;
                        }
                        else
                        {
                            _error="DB SERVER CONNECTION = "+con.State.ToString ();
                            return false;
                        }
                    }

                }
                catch (Exception ex)
                {

                    return false;
                }

            }
            else
            {

                _error="KULLANICI  YOK !  ";

                return false;
            }


        }
        public static DataSet check ()
        {
            DataSet ds = new DataSet ();
            SqlConnection con = sqldbsconnect ();
            string command = @"SELECT * FROM tbl_uyebilgi ";
            SqlCommand sqlComm = new SqlCommand (command, con);
            SqlDataAdapter da = new SqlDataAdapter (sqlComm);
            da.SelectCommand=sqlComm;

            da.Fill (ds);
            //  con.Close ();
            return ds;


        }

    }
}
