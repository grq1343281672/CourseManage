using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseManageDAL;
using CourseManageModels;

namespace CourseManageBLL
{
    public class TeacherManger
    {
        private TeacherServicecs teacherServicecs = new TeacherServicecs();
        public Teacher TeacherLogin(Teacher teacher)
        {
            teacher = teacherServicecs.TeacherLogin(teacher);
            return teacher;
        }
        public int ModifyPwd(Teacher teacher)
        {
            return teacherServicecs.ModifyPwd(teacher);
        }
    }
}
