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
    public partial class Form3 : Form
    {
        internal enum Modes
        {
            INSERT,
            UPDATE
        }

        internal static Form3 current;

        private Modes mode = Modes.INSERT;

        private string[] assignInitial;

        public Form3()
        {
            current = this;
            InitializeComponent();
        }

        internal void Start(Modes m, DataGridViewSelectedRowCollection c)
        {
            mode = m;


 //           if (mode == Modes.UPDATE)
 //           {
                textBox1.ReadOnly = true;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = false;

                if(c != null) { 
                    textBox1.Text = "" + c[0].Cells["StId"].Value;
                    textBox2.Text = "" + c[0].Cells["StName"].Value;
                    textBox3.Text = "" + c[0].Cells["CId"].Value;
                    textBox4.Text = "" + c[0].Cells["CName"].Value;
                    textBox5.Text = "" + c[0].Cells["FinalNote"].Value;
                    assignInitial = new string[] { (string)c[0].Cells["StId"].Value, (string)c[0].Cells["CId"].Value };
                }
//          }

            ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int r = -1;

            List<string[]> lId = new List<string[]>();
            lId.Add(assignInitial);

            r = Data.Enrollments.DeleteData(lId);

            if (r == 0)
            {
                string finalGrade = textBox5.Text;
                if (string.IsNullOrEmpty(textBox5.Text))
                {
                    r = Data.Enrollments.InsertData(new string[] { assignInitial[0], assignInitial[1], null });
                }
                else
                {
                    if (int.TryParse(finalGrade, out int n))
                    {
                        
                        int grade = int.Parse(finalGrade);
                        if (grade < 0 || grade > 100)
                        {
                            Data.Enrollments.InsertData(new string[] { assignInitial[0], assignInitial[1], null });
                            MessageBox.Show("Grade must be between 0 and 100.");
                            r = -1;
                        }
                        else
                        {
                            r = Data.Enrollments.InsertData(new string[] { assignInitial[0], assignInitial[1], finalGrade });
                        }
                    } else {
                        Data.Enrollments.InsertData(new string[] { assignInitial[0], assignInitial[1], null });
                        MessageBox.Show("Grade must be an integer."); r = -1;
                    }
                } 
            }
            
            if (r == 0) { Close(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
