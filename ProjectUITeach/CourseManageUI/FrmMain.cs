using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseManageUI
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            this.lblCurrentUser.Text = Program.currentTeacher.TeacherName;
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

        /// <summary>
        /// 窗体嵌入通用方法
        /// </summary>
        /// <param name="childForm"></param>
        private void OpenChildFrm(Form childForm)
        {
            foreach (Control item in this.panel_Right.Controls)
            {
                if (item is Form)
                {
                    ((Form)item).Close();
                }
            }
            //将子窗体设置为非顶级控件
            childForm.TopLevel = false;
            childForm.Parent = this.panel_Right;//设置窗体父容器
            childForm.Dock = DockStyle.Fill;
            childForm.Show();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //添加课程
        private void btnAddCourse_Click(object sender, EventArgs e)
        {
            OpenChildFrm(new FrmAddCouse());
        }
        //课程信息管理
        private void btnCourseManage_Click(object sender, EventArgs e)
        {
            OpenChildFrm(new FrmCourseManage());
        }

        private void btnTeacherManage_Click(object sender, EventArgs e)
        {

        }

        private void btnModifyPwd_Click(object sender, EventArgs e)
        {
            OpenChildFrm(new FrmModifyPwd());
        }
    }
}
