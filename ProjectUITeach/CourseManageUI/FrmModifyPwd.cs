using CourseManageModels;
using CourseManageBLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseManageUI
{
    public partial class FrmModifyPwd : Form
    {
        public FrmModifyPwd()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveToDB_Click(object sender, EventArgs e)
        {
            //判断与原始密码是否相等
            if (!this.txtOldPwd.Text.Trim().Equals(Program.currentTeacher.LoginPwd))
            {
                MessageBox.Show("与原密码不相同！请重新输入!","修改信息");
                return;
            }
            //判断新密码格式是否正确
            if (TxtDetection(this.txtNewPwd) == 0)
            {
                return;
            }
            //判断两次新密码 是否一致
            if (!this.txtNewPwd.Text.Trim().Equals(this.txtConfirmNewPwd.Text.Trim()))
            {
                MessageBox.Show("两次输入密码不一致!请重新输入!","修改信息");
            }
            //封装新密码信息
            Teacher teacher = new Teacher {
                TeacherId = Program.currentTeacher.TeacherId,
                LoginPwd = this.txtNewPwd.Text
            };
            //提交后台
            int result = new TeacherManger().ModifyPwd(teacher);
            //提交是否成功
            if (result == -1)
            {
                MessageBox.Show("修改密码失败","修改信息");
            }
            MessageBox.Show("修改成功 请重新登录！", "修改信息");
            
            

        }

        /// <summary>
        /// 检测当前文本框格式是否正确
        /// </summary>
        /// <param name="textInfo"></param>
        /// <returns>0:格式不正确 1:格式正确</returns>
        private int TxtDetection(TextBox textInfo)
        {
            string pattern = "^[a-zA-Z0-9.@_]*$";
            if (Regex.IsMatch(textInfo.Text, pattern))
            {
                if (textInfo.Text.Trim().Length >= 6 && textInfo.Text.Trim().Length <= 18)
                {
                    return 1;
                }
                else
                {
                    MessageBox.Show("新密码长度不匹配！请重新输入!", "修改信息");
                    return 0;
                }
            }
            else
            {
                MessageBox.Show("新密码格式不正确！请重新输入!", "修改信息");
                return 0;
            }
        }
    }
}
