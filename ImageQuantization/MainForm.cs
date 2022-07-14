using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;
        public int clusters;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
            clusters = Convert.ToInt32(K.Text);

        }

        private void quantizebtn_Click(object sender, EventArgs e)
        {
            var execution_watch = System.Diagnostics.Stopwatch.StartNew();//O(1)
            int h = ImageOperations.GetHeight( ImageMatrix);
            int w = ImageOperations.GetWidth( ImageMatrix);
            var my = ImageOperations.Distinct_colors(ImageMatrix, h, w);//O(n^2) = O(height*width)           
            mst_graph grap = new mst_graph(my);//O(d^2)
            var nodes = grap.clustering(clusters);//O(d log(d))
            Dictionary<int,RGBPixel> avg = grap.average(nodes, clusters, my);//O(d)
            ImageMatrix = grap.mapping(avg, nodes, h, w, ImageMatrix, my);//O(n^2) = O(height*width)
            ImageOperations.DisplayImage(  ImageMatrix, pictureBox2);
            execution_watch.Stop();//O(1)
            Console.WriteLine("Execution time = " + execution_watch.ElapsedMilliseconds/1000 + "s " + execution_watch.ElapsedMilliseconds % 1000 + "ms");//O(1)

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void txtWidth_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}