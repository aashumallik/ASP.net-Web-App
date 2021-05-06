using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace FirestoneWebTemplate.Classes 
{

    public class FireStoneDBO
    {
        private SqlCommand _cmd = null;
        

        public FireStoneDBO()
        {
            _cmd = new SqlCommand("", new SqlConnection());
        }
        

        public SqlCommand QueryCommand
        {
            get
            {
                return _cmd;
            }           
            set               
            {
               _cmd = value;
            }           
        }
        
        
        public string Connection
        {
            get
            {
                return _cmd.Connection.ConnectionString;
            }           
            set               
            {
               _cmd.Connection.ConnectionString = value;
            }           
        }


        public string QueryText
        {
            get
            {
                return _cmd.CommandText;                
            }
            set
            {
                _cmd.CommandText = value;
            }   
        }


        public CommandType QueryType
        {
            get
            {
                return _cmd.CommandType;
            }
            set
            {
                _cmd.CommandType = value;
            }
        }


        public SqlParameterCollection Parameters
        {
            get
            {
                return _cmd.Parameters;
            }
            //set
            //{
            //    _cmd.Parameters = value;
            //}
        }


        public void OpenConnection()
        {

            try
            {
                if (_cmd.Connection.State != ConnectionState.Open)
                {
                    _cmd.Connection.Open();
                }                
            }
            catch (Exception ex) { }
            
        }

        
        public void CloseConnection()
        {

            try
            {
                if (_cmd.Connection.State == ConnectionState.Open)
                {
                    _cmd.Connection.Close();
                    _cmd.Connection.Close();
                    _cmd.Dispose();
                }
            }
            catch (Exception ex) { }

        }



        public void ExecNonQuery()
        {

            try
            {
                if (_cmd.Connection.State != ConnectionState.Open)
                {
                    _cmd.Connection.Open();
                }

                _cmd.ExecuteNonQuery();
            }
            catch (Exception ex) {  }
            finally 
            {
                _cmd.Connection.Close();
                _cmd.Dispose();           
            }

        }


        public SqlDataReader ExecReader()
        {
            SqlDataReader dr = null;            
            try
            {
                if (_cmd.Connection.State != ConnectionState.Open)
                {
                    _cmd.Connection.Open();
                }
                                
                dr = _cmd.ExecuteReader();
            }
            catch (Exception ex) { }
            finally
            {
                
            }

            return dr;
        }


    }
}