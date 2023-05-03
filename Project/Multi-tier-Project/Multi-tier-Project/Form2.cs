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
    public partial class Form2 : Form
    {
        internal enum Modes
        {
            INSERT,
            UPDATE

        }

        internal static Form2 current;

        private Modes mode = Modes.INSERT;

        private string[] assignInitial;

        public Form2()
        {
            current = this;
            InitializeComponent();
        }

        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            mode = m;

            comboBox1.DisplayMember = "StId";
            comboBox1.ValueMember = "StId";
            comboBox1.DataSource = Data.Students.GetStudents();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.SelectedIndex = 0;

            comboBox2.DisplayMember = "CId";
            comboBox2.ValueMember = "CId"; // filter by ProgId
            comboBox2.DataSource = Data.Courses.GetCourses();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndex = 0;

            //comboBox1.
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            comboBox1.Enabled = true;


            if ((mode == Modes.UPDATE) && (c!= null))
            {
                comboBox1.Enabled= false;
                comboBox1.SelectedValue = c[0].Cells["StId"].Value;
                comboBox2.SelectedValue = c[0].Cells["CId"].Value;
                assignInitial = new string[] { (string)c[0].Cells["StId"].Value, (string)c[0].Cells["CId"].Value };
            }

            ShowDialog();
        }
        
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                var a = from r in Data.Students.GetStudents().AsEnumerable()
                        where r.Field<String>("StId") == (String)comboBox1.SelectedValue
                        select new { Name = r.Field<string>("StName") };
                textBox1.Text = a.Single().Name;
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                var a = from r in Data.Courses.GetCourses().AsEnumerable()
                        where r.Field<String>("CId") == (String)comboBox2.SelectedValue
                        select new { Name = r.Field<string>("CName") };
                textBox2.Text = a.Single().Name;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (BusinessLayer.Enrollments.SameProgramStudent((string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue))
            {
                int r = -1;
                if (mode == Modes.INSERT)
                {
                    r = Data.Enrollments.InsertData(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });
                }
                if (mode == Modes.UPDATE)
                {
                    List<String[]> lId = new List<String[]>();
                    lId.Add(assignInitial);

                    r = Data.Enrollments.DeleteData(lId);

                    if (r == 0)
                    {
                        r = Data.Enrollments.InsertData(new string[] { (string)comboBox1.SelectedValue, (string)comboBox2.SelectedValue });

                    }


                }

                if (r == 0) { Close(); }

            }
            else
            {
                MessageBox.Show("Student can only enroll from the courses avalable from his program.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
