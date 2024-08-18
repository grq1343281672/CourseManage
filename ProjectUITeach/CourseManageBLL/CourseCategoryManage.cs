using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseManageModels;
using CourseManageDAL;

namespace CourseManageBLL
{
    public class CourseCategoryManage
    {
        private CourseCategoryService categoryService = new CourseCategoryService();
        public List<CourseCategory> GetCourseCategory() {
            return categoryService.GetCourseCategory();
        }
    }
}
