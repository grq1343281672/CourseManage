using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CourseManageModels;

namespace CourseManageDAL
{
    public class TeacherServicecs
    {
        /// <summary>
        /// 教师登录
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns>教师数据</returns>
        public Teacher TeacherLogin(Teacher teacher)
        {
            //封装SQL语句
            string sql = $"select TeacherName,TeacherId from Teacher where LoginAccount ='{teacher.LoginAccount}' and LoginPwd = '{teacher.LoginPwd}' ";
            //提交查询
            SqlDataReader reader = SQLHelper.GetReader(sql);
            //判断登录是否正确 如正确则封装数据
            if (reader.Read())
            {
                teacher.TeacherName = reader["TeacherName"].ToString();
                teacher.TeacherId = (int)reader["TeacherId"];
            }
            else
            {
                teacher = null;//表示账号密码不正确
            }
            reader.Close();
            return teacher;
        }
        public int ModifyPwd(Teacher teacher)
        {
            string sql = $"update Teacher set LoginPwd = '{teacher.LoginPwd}' where TeacherId = {teacher.TeacherId}";
            return SQLHelper.Update(sql);
        }
    }
}
