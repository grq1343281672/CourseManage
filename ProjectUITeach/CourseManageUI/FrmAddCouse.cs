using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CourseManageBLL;
using CourseManageModels;

namespace CourseManageUI
{
    public partial class FrmAddCouse : Form
    {
        private CourseCategoryManage categoryManage = new CourseCategoryManage();
        private List<Course> coursList = new List<Course>();
        private int resultSum = 0;
        public FrmAddCouse()
        {
            InitializeComponent();
            //动态填充 课程分类下拉框
            this.cbbCategory.DataSource = categoryManage.GetCourseCategory();
            this.cbbCategory.DisplayMember = "CategoryName";
            this.cbbCategory.ValueMember = "CategoryId";
            //禁止自动生成列
            this.dgvCourseList.AutoGenerateColumns = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveToDB_Click(object sender, EventArgs e)
        {
            bool nullTextBox = false;
            //判断所有textbox用户是否输入
            foreach (Control item in groupBoxCourseInfo.Controls)
            {
                if (item is TextBox && item.Text.Trim().Length == 0)
                {      
                        if(item.Tag == null || item.Tag.ToString() == string.Empty)
                        {
                            item.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MyTextBox_MouseClick);
                            item.Tag = "True";
                        }
                        //对用户没有输入的文本框进行提示
                        nullTextBox = true;
                        item.Text = "内容不能为空!";
                        item.ForeColor = Color.Red;
                }
            }
            if (nullTextBox)
            {
                return;
            }
            //封装用户数据
            Course course = new Course
            {
                CourseName = this.txtCouseName.Text,
                CourseContent = this.txtCourseContent.Text,
                ClassHour = Convert.ToInt32(this.txtClassHour.Text),
                Credit = Convert.ToInt32(this.txtCredit.Text),
                CategoryId = (int)this.cbbCategory.SelectedValue,
                TeacherId = Program.currentTeacher.TeacherId,
                CategoryName = this.cbbCategory.Text
            };
            int result = new CourseManage().AddCourse(course);
            this.resultSum += result;
            this.coursList.Add(course);
            this.dgvCourseList.DataSource = null;
            this.dgvCourseList.DataSource = this.coursList;
            this.lblCount.Text = resultSum.ToString();
            if (ckbAutoClear.Checked == true)
            {
                foreach (Control item in groupBoxCourseInfo.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text = "";
                    }
                }
            }

        }
        /// <summary>
        /// 清除文本框提示内容 并恢复字体颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "内容不能为空!")
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // 获取当前行的序号
            string rowIndex = (e.RowIndex + 1).ToString();

            // 定义要绘制的矩形区域
            Rectangle headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, this.dgvCourseList.RowHeadersWidth, e.RowBounds.Height);

            // 绘制序号
            TextRenderer.DrawText(e.Graphics, rowIndex, this.dgvCourseList.RowHeadersDefaultCellStyle.Font, headerBounds, this.dgvCourseList.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
