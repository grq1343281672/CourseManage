using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using CourseManageModels;

namespace CourseManageDAL
{
     public class CourseCategoryService
    {
        /// <summary>
        /// 查询全部课程分类对象
        /// </summary>
        /// <returns></returns>
        public List<CourseCategory> GetCourseCategory()
        {
            List<CourseCategory> list = new List<CourseCategory>();
            string sql = "select CategoryName,CategoryId from CourseCategory";
            SqlDataReader result =  SQLHelper.GetReader(sql);
            while (result.Read())
            {
                list.Add(new CourseCategory
                {
                    CategoryId = (int)result["CategoryId"],
                    CategoryName = result["CategoryName"].ToString(),
                });
            }
            result.Close();
            return list;
        }
    }
}
