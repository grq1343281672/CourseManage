using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CourseManageModels;
using CourseManageBLL;

namespace CourseManageUI
{
    public partial class FrmAdminLogin : Form
    {
        public FrmAdminLogin()
        {
            InitializeComponent();
        }

        #region 窗体移动代码
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoginSys_Click(object sender, EventArgs e)
        {
            if (this.txtLoginAccount.Text.Trim().Length == 0 || this.txtLoginPwd.Text.Trim().Length == 0)
            {
                MessageBox.Show("账号或密码不能为空,请重新输入");
            }
            else
            {
                Teacher teacher = new Teacher();
                teacher.LoginAccount = this.txtLoginAccount.Text.Trim();
                teacher.LoginPwd = this.txtLoginPwd.Text.Trim();
                teacher = new TeacherManger().TeacherLogin(teacher);
                if (teacher != null)
                {
                    Program.currentTeacher = teacher;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("账号或密码错误,重新输入", "提示信息");
                }
            }

        }
    }
}
