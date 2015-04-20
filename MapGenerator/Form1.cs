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
            Program.GenerateNew();
        }

        private void button2_Click(object sender, EventArgs e)//Increase width
        {
            Program.Changesize(imgOriginal.Width+1, imgOriginal.Height);
        }

        private void button3_Click(object sender, EventArgs e)//Decrease width
        {
            Program.Changesize(imgOriginal.Width-1, imgOriginal.Height);
        }

        private void button4_Click(object sender, EventArgs e)//Increase height
        {
            Program.Changesize(imgOriginal.Width, imgOriginal.Height+1);
        }

        private void button5_Click(object sender, EventArgs e)//Decrease height
        {
            Program.Changesize(imgOriginal.Width, imgOriginal.Height-1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Program.DeColor();
        }
    }
}
