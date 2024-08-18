using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace CourseManageDAL
{
    public class SQLHelper
    {
        //从配置文件读取连接信息
        private static string connString = ConfigurationManager.ConnectionStrings["connString"].ToString(); 
        /// <summary>
        /// 执行增、删、改操作
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>受影响行数</returns>
        public static int Update(string sql,SqlParameter[] parameters = null)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand com = new SqlCommand(sql, conn);
            if (parameters!=null)
            {
                com.Parameters.AddRange(parameters);
            }
            try
            {
                conn.Open();
                return com.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw new Exception("执行方法public static int Update(string sql)发生异常:" + ex);
            }
            finally
            {
                conn.Close();
            }
            
        }
        /// <summary>
        /// 查询课程总数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>课程总数</returns>
        public static object GetSingleResult(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand com = new SqlCommand(sql,conn);
            try
            {
                conn.Open();
                return com.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw new Exception("执行方法public static object GetSingleResult(string sql)发生异常:" + ex);
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 查询指定结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回结果集</returns>
        public static SqlDataReader GetReader(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand com = new SqlCommand(sql,conn);
            try
            {
                conn.Open();
                return com.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception("执行方法public static SqlDataReader GetReader(string sql)出现异常：" + ex);
            }
        }
    }
}
