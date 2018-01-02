using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace Search_Engine
{
    public partial class Form1 : Form
    {

        public string operation = "0";
        public string[] toFind;
        public int[] result;

        public Form1()
        {
            InitializeComponent();
        }


        private void cmb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsLetter(e.KeyChar) || Char.IsPunctuation(e.KeyChar) || Char.IsSymbol(e.KeyChar) || Char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int[] res1;
            int[] res2;
            Queue<int> Qres1 = new Queue<int>();
            Queue<int> Qres2 = new Queue<int>();
            string[] temp = searchBox.Text.Split(' ','.',',');
            string[] files = System.IO.Directory.GetFiles("C:\\Users\\" + Environment.UserName + "\\Desktop\\Samples", "*.txt");
            Queue<string> TSearch = new Queue<string>();
            Queue<string> SQueue = new Queue<string>();
            int[] contain = new int[files.Length];
            int x = 0;


            while (cmb.Items.Count != 0) {
                cmb.Items.Clear();
            }

            while (x != temp.Length)
            {
                TSearch.Enqueue(temp[x]);
                x++;
            }


            while (TSearch.Count > 0)
            {
                if (TSearch.Peek() == "and" || TSearch.Peek() == "or" || TSearch.Peek() == "OR" || TSearch.Peek() == "AND")
                {
                    operation = TSearch.Dequeue();
                }

                else
                {
                    SQueue.Enqueue(TSearch.Dequeue());
                }
            }





            toFind = SQueue.ToArray();
            int[,] TResult = new int[files.Length, toFind.Length];
            SQueue.Clear();
            
            
            

            
            for (int i = 0; i < files.Length; i++)
            {
                using (StreamReader sr = new StreamReader(files[i]))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] TFLine = line.Split(' ','\'','.',',','\t','\v','\n','\r');

                        for (int j = 0; j < toFind.Length; j++)
                        {
                            foreach (string word in TFLine)
                            {
                                if (word.Equals(toFind[j], StringComparison.InvariantCultureIgnoreCase))
                                {
                                    TResult[i, j]++;

                                }
                            }

                        }

                    }
                }
            }



            for (x = 0; x < files.Length; x++)
            {
                Qres1.Enqueue(TResult[x, 0]);
                if (toFind.Length >= 2)
                    Qres2.Enqueue(TResult[x, 1]);
            }

            int[] srval1 = Qres1.ToArray();
            int[] srval2 = Qres2.ToArray();

            for (int i = 0; i < files.Length; i++)
            {
                if (operation.Equals("and", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (toFind.Length >= 2)
                        contain[i] = srval1[i] * srval2[i];
                    else if (toFind.Length == 1)
                        contain[i] = srval1[i];
                }
                else
                {
                    for (int j = 0; j < toFind.Length; j++)
                    {

                        if (TResult[i, j] > 0)
                        {
                            contain[i]++;
                        }
                    }
                }
            }

            for (int i = 0; i < files.Length; i++)
            {
                if (contain[i] > 0)
                {
                    cmb.Items.Add(files[i]);
                    x++;
                }

            }

            if (x == 0) {
                MessageBox.Show("Not Found");
            }

        }
    }
}
