using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseManageModels;
using CourseManageDAL;

namespace CourseManageBLL
{
    public class CourseManage
    {
        CourseService courseService = new CourseService();
        public int AddCourse(Course course)
        {
            return courseService.AddCourse(course);
        }
        public List<Course> QueryCourse(int categoryId, string courseName)
        {
            return courseService.QueryCourse(categoryId, courseName);
        }
        public int UpdateCourseInfo(Course course) {
            return courseService.UpdateCourseInfo(course);
        }
        public int DeleteCourse(Course course)
        {
            return courseService.DelecteCourse(course);
        }
    }
}
