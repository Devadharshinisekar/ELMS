using System;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using ELMSApplication.Models;
namespace ELMSApplication.Models{
    public class Database{
        static SqlConnection sqlconnection=new SqlConnection("DefaultConnection");
        static  public string Login(Employee employee){
            sqlconnection.Open();
            SqlCommand command=new SqlCommand();
            return "success";
        }
    }
}