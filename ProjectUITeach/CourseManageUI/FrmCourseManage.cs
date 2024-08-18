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
using CourseManageBLL;
using CourseManageModels;

namespace CourseManageUI
{
    public partial class FrmCourseManage : Form
    {
        private CourseCategoryManage categoryManage = new CourseCategoryManage();
        private CourseManage courseManage = new CourseManage();
        private List<Course> result = new List<Course>();
        //构造函数 初始化信息
        public FrmCourseManage()
        {
            InitializeComponent();
            List<CourseCategory> list = categoryManage.GetCourseCategory();
            list.Insert(0, new CourseCategory { CategoryId = -1, CategoryName = "--请选择--" });
            this.cbbCategory.DataSource = list;
            this.cbbCategory.DisplayMember = "CategoryName";
            this.cbbCategory.ValueMember = "CategoryId";
            //禁止自动生成列
            this.dgvCourseList.AutoGenerateColumns = false;

            //禁止点击
            this.btnModifyCourse.Enabled = false;
            this.btnDelCourse.Enabled = false;

            this.cbbCategory_Modify.DataSource = categoryManage.GetCourseCategory();//创建新数据源或者复制前面数据源 否则会产生两个下拉框联动
            this.cbbCategory_Modify.DisplayMember = "CategoryName";
            this.cbbCategory_Modify.ValueMember = "CategoryId";
        }
        //关闭窗体
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //显示修改信息窗体
        private void btnModifyCourse_Click(object sender, EventArgs e)
        {
            //判断用户是否有选择
            if (this.dgvCourseList.CurrentRow == null)
            {
                MessageBox.Show("请选择要修改的列","提示信息");
                return;
            }
            //禁用dgv控件 防止用户修改
            this.dgvCourseList.Enabled = false;

            //获得当前选中的课程ID
            int courseId = (int)this.dgvCourseList.CurrentRow.Cells["CourseId"].Value;
            //更改修改课程面板中 课程编号的值
            this.lblCourseId.Text = courseId.ToString();
            Course currentCourse = null;
            //找到当前选中的course对象
            //foreach (var item in this.result)
            //{
            //    if (item.CourseId.Equals(courseId))
            //    {
            //        currentCourse = item;
            //        break;
            //    }
            //}
            //LinQ查询
            currentCourse = (from c in this.result where c.CourseId.Equals(courseId) select c).First();
            //展示当前选中课程数据
            this.txtCouseName_Modify.Text = currentCourse.CourseName;
            this.txtCourseContent.Text = currentCourse.CourseContent;
            this.txtClassHour.Text = currentCourse.ClassHour.ToString();
            this.txtCredit.Text = currentCourse.Credit.ToString();
            //将课程信息下拉框与当前选择课程信息做同步
            this.cbbCategory_Modify.SelectedValue = currentCourse.CategoryId;

            //显示修改课程面板
            this.panel1.Visible = true;
        }
        //退出修改信息窗体
        private void btnCloseModify_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = false;
            this.dgvCourseList.Enabled = true;
            foreach (Control item in groupBox1.Controls) {
                if (item is TextBox)
                {
                    item.ForeColor = Color.Black;
                }
            }
        }
        //查询按钮
        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.dgvCourseList.Enabled = true;
            this.panel1.Visible = false;
            if ((int)this.cbbCategory.SelectedValue == -1 && txtCourseName.Text.Trim().Length == 0)
            {
                MessageBox.Show("请选择至少一个查询条件", "提示信息");
                return;
            }
            result = courseManage.QueryCourse((int)this.cbbCategory.SelectedValue, this.txtCourseName.Text.Trim());
            if (result.Count == 0)
            {
                MessageBox.Show("没有查询到相关数据", "查询信息");
                this.dgvCourseList.DataSource = null;
                this.lblCount.Text = "0";
                this.btnModifyCourse.Enabled = this.btnDelCourse.Enabled = false;
            }
            else
            {
                this.dgvCourseList.DataSource = result;
                this.lblCount.Text = result.Count.ToString();
                this.btnModifyCourse.Enabled = true;
                this.btnDelCourse.Enabled = true;
            }
            

        }
        //给dataGridView1增加行号显示
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // 获取当前行的序号
            string rowIndex = (e.RowIndex + 1).ToString();

            // 定义要绘制的矩形区域
            Rectangle headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, this.dgvCourseList.RowHeadersWidth, e.RowBounds.Height);

            // 绘制序号
            TextRenderer.DrawText(e.Graphics, rowIndex, this.dgvCourseList.RowHeadersDefaultCellStyle.Font, headerBounds, this.dgvCourseList.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        //修改数据按钮
        private void btnSaveToDB_Click(object sender, EventArgs e)
        {
            bool nullTextBox = false;
            string pattern = @"\d+";
            //判断所有textbox用户是否输入
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox && item.Text.Trim().Length == 0)
                {
                    if (item.Tag == null || item.Tag.ToString() ==string.Empty)
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
            //判断用户输入课时是否为数字 且 大于0
            if (!Regex.IsMatch(this.txtClassHour.Text,pattern))
            {
                MessageBox.Show("请输入数字！！");
                return;
            }
            if (!(Convert.ToInt32(this.txtClassHour.Text) > 0))
            {
                MessageBox.Show("课时不能为负数！");
                return;
            }
            //判断用户输入学分是否为数字 且 大于0 小于30
            if (!Regex.IsMatch(this.txtCredit.Text, pattern))
            {
                MessageBox.Show("请输入数字！！");
                return;
            }
            if (!(Convert.ToInt32(this.txtCredit.Text) > 0 && Convert.ToInt32(this.txtCredit.Text) <= 30))
            {
                MessageBox.Show("学分不能为负数且小于30！");
                return;
            }
            //封装修改数据
            Course course = new Course
            {
                CourseName = this.txtCouseName_Modify.Text.Trim(),
                CourseContent = this.txtCourseContent.Text.Trim(),
                ClassHour = Convert.ToInt32(this.txtClassHour.Text),
                Credit = Convert.ToInt32(this.txtCredit.Text),
                CategoryName = this.cbbCategory_Modify.Text,
                CategoryId = (int)this.cbbCategory_Modify.SelectedValue,
                CourseId = Convert.ToInt32(this.lblCourseId.Text),
                TeacherId = Program.currentTeacher.TeacherId,
            };
            //调用后台
            courseManage.UpdateCourseInfo(course);
            //从查询缓存里找到当前选中对象
            Course currentCourse = (from c in this.result where c.CourseId == course.CourseId select c).First();
            //重新赋值
            currentCourse.CourseName = course.CourseName;
            currentCourse.CategoryName = course.CategoryName;
            currentCourse.CategoryId = course.CategoryId;
            currentCourse.CourseContent = course.CourseContent;
            currentCourse.Credit = course.Credit;
            currentCourse.ClassHour = course.ClassHour;
            //刷新视图
            this.dgvCourseList.Refresh();
            this.panel1.Visible = false;
            this.dgvCourseList.Enabled = true;
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
        //删除课程
        private void BtnDelCourse_Click(object sender, EventArgs e)
        {
            
            int courseId = (int)this.dgvCourseList.CurrentRow.Cells["CourseId"].Value;//获取当前选中课程ID
            DialogResult dialog =  MessageBox.Show($"确定要删除编号{courseId}的课程么","删除课程",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
            if (dialog == DialogResult.Cancel)
            {
                return;
            }
            Course course = new Course
            {
                CourseId = courseId
            };
            this.courseManage.DeleteCourse(course);
            foreach (Course item in this.result)
            {
                if (item.CourseId == courseId)
                {
                    result.Remove(item);
                    break;
                }
            }
            this.dgvCourseList.Enabled = true;  //启用dataGridView控件
            this.lblCount.Text = this.result.Count.ToString();  //更改查询总数文本
            this.dgvCourseList.DataSource = null; //更新dataGridView1数据源
            this.dgvCourseList.DataSource = result;
            this.dgvCourseList.Refresh();//刷新dataGridView显示
            this.panel1.Visible = false; //隐藏修改信息面板
        }
    }
}
