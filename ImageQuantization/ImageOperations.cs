using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Linq;
///Algorithms Project



namespace ImageQuantization
{
    /// <summary>
    /// Holds the pixel color in 3 byte values: red, green and blue
    /// </summary>
    public struct RGBPixel
    {
        public byte red, green, blue;
 
    }

    public struct RGBPixelD
    {
        public double red, green, blue;
    }


    /// <summary>
    /// Library of static functions that deal with images
    /// </summary>
    public class ImageOperations
    {
        /// <summary>
        /// Open an image and load it into 2D array of colors (size: Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of colors</returns>
        public static RGBPixel[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            RGBPixel[,] Buffer = new RGBPixel[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x].red = Buffer[y, x].green = Buffer[y, x].blue = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x].red = p[2];
                            Buffer[y, x].green = p[1];
                            Buffer[y, x].blue = p[0];
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }

        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage( RGBPixel[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[2] = ImageMatrix[i, j].red;
                        p[1] = ImageMatrix[i, j].green;
                        p[0] = ImageMatrix[i, j].blue;
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }


      
        /// <param name="ImageMatrix">Colored image matrix</param>
        

        public static int dis_counter = 0;//O(1)
        public static RGBPixel[]  Distinct_colors( RGBPixel[,] ImageMatrix, int hight, int width)
        {
            bool[,,] exist = new bool[256, 256, 256];//O(1)

            for (int i = 0; i < hight; i++)//O(n^2) = //O(height*width)
            {
                for (int j = 0; j < width; j++)//O(n) = O(width)
                {
                    if (!exist[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue])//O(1)
                    {
                        exist[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue] = true;//O(1)                                                                         
                        dis_counter++;//O(1)                   
                    }           
                }
            }
            RGBPixel[] dis_color = new RGBPixel[dis_counter];//O(1)
            Array.Clear(exist, 0, exist.Length);//O(1)
            int cnt = 0;//O(1)
            for (int i = 0; i < hight; i++)//O(n^2) = //O(height*width)
            {
                for (int j = 0; j < width; j++)//O(n) = O(width)
                {
                    if (!exist[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue])//O(1)
                    {
                        exist[ImageMatrix[i, j].red, ImageMatrix[i, j].green, ImageMatrix[i, j].blue] = true;//O(1)                                                                         
                        dis_color[cnt++] = ImageMatrix[i, j];//O(1)            
                    }
                }
            }
            return dis_color;//O(1)
        }
    }

    public class mst_graph
    {
        public struct node
        {
            public int parent, rank;
        };
        class arc : IComparable<arc>
        {
            public int s, d;
            public double w;
            public int CompareTo(arc compareEdge)
            {
                return compareEdge.w.CompareTo(this.w) * -1;
            }

        }

        int a;
        arc[] res;
        public mst_graph(RGBPixel[] arr)
        {
            //initialization
            a = ImageOperations.dis_counter;//O(1)
            bool[] visited = new bool[a + 1];//O(1)
            int idx = 1, source = -1;//O(1)
            res = new arc[a];//O(1)
            for (int i = 0; i < a; ++i) //O(d)
                res[i] = new arc();//O(1)
            double min = double.MaxValue;//O(1)
            /*calculated the distance between the first distinct color and all other colors 
             * and we store these edges and mark the first distinct color as visited.*/
           visited[0] = true;//O(1)
            int v = 0;//O(1)
            for (int j = 1; j < ImageOperations.dis_counter; j++)//O(d)
            {
                double db = arr[0].blue - arr[j].blue;//O(1)
                //db = Math.Pow(db, 2);//O(1)
                db = db * db;
                double dr = arr[0].red - arr[j].red;//O(1)
                //dr = Math.Pow(dr, 2);//O(1)
                dr = dr * dr;
                double dg = arr[0].green - arr[j].green;//O(1)
                //dg = Math.Pow(dg, 2);//O(1)
                dg = dg * dg;
                double ww = Math.Sqrt(db + dr + dg);//O(1)
                res[idx].s = 0;//O(1)
                res[idx].d = j;//O(1)
                res[idx].w = ww;//O(1)
                idx++;//O(1)
                if (min > ww)//O(1)
                {
                    min = ww;//O(1)
                    v = j;//O(1)
                }
            }
            visited[v] = true;//O(1)
            //Calculate distances from the new source and update the edges till the mst is constructed
            for (int e = 0; e < a - 2; e++)//O(d^2)
            {
                min = double.MaxValue;//O(1)
                source = v;//O(1)
                for (int j = 0; j < ImageOperations.dis_counter; j++)//O(d)
                {
                    if (visited[j])//O(1)
                    { continue; }//O(1)
                    double db = arr[source].blue - arr[j].blue;//O(1)
                    //db = Math.Pow(db, 2);//O(1)
                    db = db * db;
                    double dr = arr[source].red - arr[j].red;//O(1)
                    //dr = Math.Pow(dr, 2);//O(1)
                    dr = dr * dr;
                    double dg = arr[source].green - arr[j].green;//O(1)
                    //dg = Math.Pow(dg, 2);//O(1)
                    dg = dg * dg;
                    double ww = Math.Sqrt(db + dr + dg);//O(1)
                    if (ww < res[j].w)//O(1)
                    {
                        res[j].s = source;//O(1)
                        res[j].w = ww;//O(1)
                        res[j].d = j;//O(1)
                    }
                    if (min > res[j].w)//O(1)
                    {
                        min = res[j].w;//O(1)
                        v = j;//O(1)
                    }
                }
                visited[v] = true;//O(1)

            }
            Array.Sort(res);//dlog(d)
            //Calculate the minimum spanning tree cost
            double minimumCost = 0;//O(1)
            for (int i = 1; i < ImageOperations.dis_counter; ++i)//O(d)
            {
                //Console.WriteLine(res[i].s + " -- "
                //                  + res[i].d
                //                  + " == " + res[i].w);
                minimumCost += res[i].w;//O(1)
            }
            minimumCost= (double)Math.Round((decimal)minimumCost, 2, MidpointRounding.ToEven);//O(1)
            Console.WriteLine("Cost of MST = "+ minimumCost);//O(1)
            Console.WriteLine("Number of distinct colors = " + ImageOperations.dis_counter);//O(1)
        }

        int find_parent(node[] nodes, int i)
        {
            if (nodes[i].parent != i)//O(1)
                return nodes[i].parent = find_parent(nodes, nodes[i].parent);//O(log(d))

            return nodes[i].parent;//O(1)
        }
        void union(node[] nodes, int x, int y)
        {
            int x_parent = find_parent(nodes, x);//O(log(d))
            int y_parent = find_parent(nodes, y);//O(log(d))

            if (nodes[x_parent].rank < nodes[y_parent].rank)//O(1)
            {
                nodes[x_parent].parent = y_parent;//O(1)
            }
            else if (nodes[x_parent].rank > nodes[y_parent].rank)//O(1)
            {
                nodes[y_parent].parent = x_parent;//O(1)
            }
            else
            {
                nodes[x_parent].parent = y_parent;//O(1)
                nodes[y_parent].rank++;//O(1)
            }
        }
        public node[] clustering(int clusters)
        {
            int K = ImageOperations.dis_counter;//O(1)
            node[] nodes = new node[ImageOperations.dis_counter];//O(1)
            for (int i = 0; i < ImageOperations.dis_counter; i++)//O(d)
                nodes[i] = new node();//O(1)

            for (int i = 0; i < ImageOperations.dis_counter; i++)//O(d)
            {
                nodes[i].parent = i;//O(1)
                nodes[i].rank = 0;//O(1)
            }
            int e = 0;//O(1)
            int idx = 1;//O(1)
            while (e < ImageOperations.dis_counter - 1)//O(dlog(d))
            {
                arc current_arc = new arc();//O(1)
                current_arc = res[idx++];//O(1)
                int x = find_parent(nodes, current_arc.s);//O(log(d))
                int y = find_parent(nodes, current_arc.d);//O(log(d))
                if (x != y)//O(1)
                {
                    e++;//O(1)
                    if (K == clusters)//O(1)
                    {
                        for (int j = 0; j < ImageOperations.dis_counter; j++)//O(d log(d))
                        {
                            nodes[j].parent = find_parent(nodes, nodes[j].parent);//O(log(d))
                            //Console.WriteLine(j + " " + nodes[j].parent);
                        }
                        return nodes;//O(1)
                    }
                    union(nodes, x, y);//O(log(d))
                    K--;//O(1)
                }
            }
            if (K == clusters)//O(1)
            {
                for (int j = 0; j < ImageOperations.dis_counter; j++)//O(d log(d))
                {
                    nodes[j].parent = find_parent(nodes, nodes[j].parent);//O(log(d))
                    //Console.WriteLine(j + " " + nodes[j].parent);
                }

                return nodes;//O(1)
            }

            return null;//O(1)
        }
        public Dictionary<int, RGBPixel> average(node[] nodes, int k, RGBPixel[] arr)
        {

            RGBPixelD[] sum = new RGBPixelD[ImageOperations.dis_counter];//O(1)
            bool[] visited = new bool[ImageOperations.dis_counter];//O(1)
            int[] cnt = new int[ImageOperations.dis_counter];//O(1)
            Dictionary<int, RGBPixel> avg = new Dictionary<int, RGBPixel>(k);//O(k)
            for (int i = 0; i < ImageOperations.dis_counter; i++)//(d)
            {
                sum[nodes[i].parent].red += arr[i].red;//O(1)
                sum[nodes[i].parent].blue += arr[i].blue;//O(1)
                sum[nodes[i].parent].green += arr[i].green;//O(1)
                cnt[nodes[i].parent]++;//O(1)
            }
            for (int i = 0; i < ImageOperations.dis_counter; i++)//O(d)
            {
                if (!visited[nodes[i].parent])//O(1)
                {
                    RGBPixel y = new RGBPixel();//O(1)
                    double red = sum[nodes[i].parent].red / (double)cnt[nodes[i].parent];//O(1)
                    double green = sum[nodes[i].parent].green / (double)cnt[nodes[i].parent];//O(1)
                    double blue = sum[nodes[i].parent].blue / (double)cnt[nodes[i].parent];//O(1)
                    sum[nodes[i].parent].red = (byte)Math.Round((decimal)red, 0);//O(1)
                    sum[nodes[i].parent].blue = (byte)Math.Round((decimal)blue, 0);//O(1)
                    sum[nodes[i].parent].green = (byte)Math.Round((decimal)green, 0);//O(1)
                    visited[nodes[i].parent] = true;//O(1)
                    y.red = (byte)sum[nodes[i].parent].red;//O(1)
                    y.green = (byte)sum[nodes[i].parent].green;//O(1)
                    y.blue = (byte)sum[nodes[i].parent].blue;//O(1)
                    avg[nodes[i].parent] = y;//O(1)
                }

            }
            //Console.WriteLine("The color palette:");//O(1)
            //foreach (var x in avg)//O(k)
            //{
               
            //    Console.WriteLine(x.Value.red + ", " + x.Value.green + ", " + x.Value.blue);//O(1)
            //}
            return avg;//O(1)
        }

        public RGBPixel[,] mapping(Dictionary<int, RGBPixel> avg, node[] nodes, int height, int width, RGBPixel[,] image, RGBPixel[] my)
        {
            int[,,] map = new int[256, 256, 256];//O(1)
            for (int i = 0; i < ImageOperations.dis_counter; i++)//O(d)
            {
                map[my[i].red, my[i].green, my[i].blue] = i;//O(1)
            }
            for (int i = 0; i < height; i++)//O(n^2) = O(height*width)
            {
                for (int j = 0; j < width; j++)//O(n) = O(width)
                {
                    image[i, j] = avg[nodes[map[image[i, j].red, image[i, j].green, image[i, j].blue]].parent];//O(1)

                    // Console.WriteLine(nodes[image[i, j].id].parent);
                    //Console.WriteLine(image[i, j].id);
                }
            }

            return image;//O(1)
        }
    }
}
