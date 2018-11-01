using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sp_kullanim_example
{
    public partial class Form1 : Form
    {
        public string _err = "";
        public Form1 ()
        {
            InitializeComponent ();

        }
        private void button1_Click (object sender, EventArgs e)
        {
            dataGridView1.DataSource=db.sp_select ("sp_select_uye", out _err);
        }
        private void button2_Click (object sender, EventArgs e)
        {
            if (db.sp_insert ("sp_insert", textBox1.Text, textBox2.Text, textBox4.Text, Convert.ToInt32 (textBox3.Text), textBox5.Text, out _err))
            {
                dataGridView1.DataSource=db.sp_select ("sp_select_uye", out _err);
                MessageBox.Show ("INSERT SUCCESSFUL !!");

            }
            else
            {
                MessageBox.Show ("INSERT FAIL ! ");
            }
        }
        private void button3_Click (object sender, EventArgs e)
        {
            if (db.sp_update ("sp_update", textBox1.Text, textBox2.Text, textBox4.Text, textBox5.Text, textBox6.Text, out _err))
            {

                MessageBox.Show ("UPDATE SUCCESSFUL");
                dataGridView1.DataSource=db.sp_select ("sp_select_uye", out _err);


            }
            else
            {
                MessageBox.Show ("UPDATE FAIL !");

            }
        }
        private void button4_Click (object sender, EventArgs e)
        {
            if (db.sp_delete ("sp_delete", textBox1.Text, textBox2.Text, textBox5.Text, out _err))
            {

                MessageBox.Show ("DELETE SUCCESSFUL");
                dataGridView1.DataSource=db.sp_select ("sp_select_uye", out _err);


            }
            else
            {
                MessageBox.Show ("DELETE FAIL !");

            }

        }
    }
}
