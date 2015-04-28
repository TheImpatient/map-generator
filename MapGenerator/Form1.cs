using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void SetImage(Bitmap bm)
        {
            pictureBox1.Image = bm;
            if (bm.Width <= 100)
            {
                pictureBox1.Size = new Size(bm.Width * 4, bm.Height * 4);
            }
            else if (bm.Width > 100 && bm.Width <= 250)
            {
                pictureBox1.Size = new Size(bm.Width * 2, bm.Height * 2);
            }
            else if (bm.Width > 250)
            {
                pictureBox1.Size = new Size(bm.Width, bm.Height);
            }
            
            this.imgOriginal = bm;
            this.Refresh();
            Application.DoEvents();
        }

        public void setText(string newText)
        {
            label1.Text = newText;
            this.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)//Generate new
        {
            if (numericUpDown1.Value != 0 || numericUpDown2.Value != 0)
            {
                Program.Changesize((int)numericUpDown1.Value, (int)numericUpDown2.Value);

                Program.GenerateNew();
            }
            else
            {
                Program.GenerateNew();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
