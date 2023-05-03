using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_tier_Project
{
    public partial class Form1 : Form
    {
        internal enum Grids
        {
            Stud,
            Cours,
            Enroll,
            Prog
        }

        internal static Form1 current;

        private Grids grid;

        public Form1()
        {
            current = this;
            InitializeComponent();
        }
        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Students.UpdateStudents();
        }

        private void bindingSource2_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Enrollments.UpdateEnrollments();
        }
        private void bindingSource3_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Courses.UpdateCourses();
        }

        private void bindingSource4_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Programs.UpdatePrograms();
        }
        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Stud;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource1.DataSource = Data.Students.GetStudents();
            bindingSource1.Sort = "StId";
            dataGridView1.DataSource = bindingSource1;


        }
        private void enrollmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (grid != Grids.Enroll)
            {
                grid = Grids.Enroll;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                bindingSource2.DataSource = Data.Enrollments.GetDisplayEnrollments();
                bindingSource2.Sort = "StId, CId";
                dataGridView1.DataSource = bindingSource2;

                dataGridView1.Columns["StId"].DisplayIndex = 0;
                dataGridView1.Columns["StId"].HeaderText = "Student Id";
                dataGridView1.Columns["StName"].DisplayIndex = 1;
                dataGridView1.Columns["StName"].HeaderText = "Student Name";
                dataGridView1.Columns["CId"].DisplayIndex = 2;
                dataGridView1.Columns["CId"].HeaderText = "Course Id";
                dataGridView1.Columns["CName"].DisplayIndex = 3;
                dataGridView1.Columns["CName"].HeaderText = "Course Name";
                dataGridView1.Columns["ProgId"].DisplayIndex = 4;
                dataGridView1.Columns["ProgId"].HeaderText = "Program Id";
                dataGridView1.Columns["ProgName"].DisplayIndex = 5;
                dataGridView1.Columns["ProgName"].HeaderText = "Program Name";
                dataGridView1.Columns["FinalNote"].DisplayIndex = 6;
                dataGridView1.Columns["FinalNote"].HeaderText = "Final Grade";
            }
        }
        private void coursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Cours;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource3.DataSource = Data.Courses.GetCourses();
            bindingSource3.Sort = "CId";
            dataGridView1.DataSource = bindingSource3;
        }
        private void programsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = Grids.Prog;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingSource4.DataSource = Data.Programs.GetPrograms();
            bindingSource4.Sort = "ProgId";
            dataGridView1.DataSource = bindingSource4;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Impossible to insert / update / delete");
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            BusinessLayer.Students.UpdateStudents();
            BusinessLayer.Enrollments.UpdateEnrollments();
            BusinessLayer.Courses.UpdateCourses();
            BusinessLayer.Programs.UpdatePrograms();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new Form2();
            Form2.current.Visible = false;
            new Form3();
            Form3.current.Visible = false;
            dataGridView1.Dock = DockStyle.Fill;
        }

        internal static void BLLMessage(string s)
        {
            MessageBox.Show("Business Layer: " + s);
        }

        internal static void DALMessage(string s)
        {
            MessageBox.Show("Data Layer: " + s);
        }

        private void addToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form2.current.Start(Form2.Modes.INSERT,null);
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {
                string grade = c[0].Cells["FinalNote"].Value.ToString();
                if (grade != "")
                {
                    MessageBox.Show("Only if the finalGrade is remove you can modify the row");
                }
                else
                {
                Form2.current.Start(Form2.Modes.UPDATE, c);
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("At least one line must be selected for deletion");
            }
            else // (c.Count > 1)
            {
                string grade = c[0].Cells["FinalNote"].Value.ToString();
                if (BusinessLayer.Enrollments.FinalGradeIsEmpty(grade)) {
                    List<string[]> lId = new List<string[]>();
                    for (int i = 0; i < c.Count; i++)
                    {
                        lId.Add(new string[] { ("" + c[i].Cells["StId"].Value),
                                               ("" + c[i].Cells["CId"].Value) });


                        //string.("" + c[i].Cells["StId"].Value),
                        //               string.Parse("" + c[i].Cells["CId"].Value) });
                    }
                    Data.Enrollments.DeleteData(lId);
                }
                else
                {
                    MessageBox.Show("You need to delete first the final note to delete the row");
                }
            }
        }
        private void manageFinalGradeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection c = dataGridView1.SelectedRows;
            if (c.Count == 0)
            {
                MessageBox.Show("A line must be selected for update");
            }
            else if (c.Count > 1)
            {
                MessageBox.Show("Only one line must be selected for update");
            }
            else
            {
                Form3.current.Start(Form3.Modes.UPDATE, c);
            }
        }
    }
}
