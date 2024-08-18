using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CourseManageModels;

namespace CourseManageDAL
{
    public class CourseService
    {
        #region 添加课程方法
        public int AddCourse(Course course)
        {
            //定义SQL语句
            //string sql = $"insert into Course(CourseName,CourseContent,ClassHourse,Credit,CategoryId,TeacherId) ";
            //sql += $"values('{course.CouseName},'{course.CourseContent}',{course.ClassHour},{course.Credit},{course.CategoryId},{course.TeacherId})";
            //return SQLHelper.Update(sql);

            //带参数的SQL语句
            string sql = $"insert into Course(CourseName,CourseContent,ClassHour,Credit,CategoryId,TeacherId) ";
            sql += $"values(@CourseName,@CourseContent,@ClassHour,@Credit,@CategoryId,@TeacherId)";
            //封装SQL语句中的参数
            SqlParameter[] sqlParameter = new SqlParameter[]{
                new SqlParameter("@CourseName",course.CourseName),
                new SqlParameter("@CourseContent",course.CourseContent),
                new SqlParameter("@ClassHour",course.ClassHour),
                new SqlParameter("@Credit",course.Credit),
                new SqlParameter("@CategoryId",course.CategoryId),
                new SqlParameter("@TeacherId",course.TeacherId)
            };
            //执行带参数的SQL语句
            return SQLHelper.Update(sql, sqlParameter);
        }
        #endregion

        #region 查询课程方法
        public List<Course> QueryCourse(int categoryId, string courseName)
        {
            string sql = "select CourseId,CourseName,ClassHour,Credit,CourseContent,CategoryId,Teacher.TeacherName,Teacher.TeacherId from Course";
            sql += " join Teacher on Teacher.TeacherId = Course.TeacherId";

            bool hasWhereClause = false;

            if (categoryId != -1)
            {
                sql += " where CategoryId = " + categoryId;
                hasWhereClause = true;
            }
            if (courseName != "")
            {
                if (hasWhereClause)
                {
                    sql += $" and CourseName like '%{courseName}%'";
                }
                else
                {
                    sql += $" where CourseName like '%{courseName}%'";
                }

            }
            SqlDataReader result = SQLHelper.GetReader(sql);
            List<Course> list = new List<Course>();
            while (result.Read())
            {
                list.Add(new Course
                {
                    CourseId = (int)result["CourseId"],
                    CourseName = result["CourseName"].ToString(),
                    ClassHour = (int)result["ClassHour"],
                    Credit = (int)result["Credit"],
                    CourseContent = result["CourseContent"].ToString(),
                    CategoryId = (int)result["CategoryId"],
                    TeacherId = (int)result["TeacherId"],
                    TeacherName = result["TeacherName"].ToString()
                });
            }
            result.Close();
            return list;

        }
        #endregion

        #region 修改课程信息
        public int UpdateCourseInfo(Course course)
        {
            //定义sql语句
            string sql = "update Course set CourseName = @CourseName,CourseContent = @CourseContent,Credit = @Credit,ClassHour = @ClassHour,CategoryId = @CategoryId where CourseId = @CourseId";
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("CourseName",course.CourseName),
                new SqlParameter("CourseContent",course.CourseContent),
                new SqlParameter("Credit",course.Credit),
                new SqlParameter("ClassHour",course.ClassHour),
                new SqlParameter("CategoryId",course.CategoryId),
                new SqlParameter("CourseId",course.CourseId),
            };
            return SQLHelper.Update(sql,sqlParameters);
        }
        #endregion

        #region 删除课程方法
        public int DelecteCourse(Course course)
        {
            string sql = $"delete from Course where CourseId = {course.CourseId} ";
            return SQLHelper.Update(sql);
        }
        #endregion
    }
}
