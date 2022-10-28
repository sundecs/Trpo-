using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FL
{
    public partial class Form1 : Form
    {
        private string dict_path;

        public Form1()
        {
            InitializeComponent();

            dict_path = Application.StartupPath + "\\dict\\";

            if(!Directory.Exists(dict_path))
            {
                Directory.CreateDirectory(dict_path);
            }
            
            listBox1.Sorted = true;

            if (!IsDirectoryEmpty(dict_path))
            {
                // populate list of dictionaries
                string[] dicts = Directory.GetFiles(dict_path, "*.txt");
                foreach (string dict in dicts)
                {
                    listBox1.Items.Add(Path.GetFileNameWithoutExtension(dict));
                }
            }

            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            textBox1.Enabled = false;
        }

        // check if dic folder is empty
        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        // add new dictionary
        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "|*.txt";
            saveFileDialog1.InitialDirectory = dict_path;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.Title = "Create new dictionaty";
            saveFileDialog1.ShowDialog();

            // if the file name is not an empty string open it for saving
            if (saveFileDialog1.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();
                listBox1.Items.Add(Path.GetFileNameWithoutExtension(fs.Name));
                fs.Close();
                
            }
        }

        // select dictionary
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(dict_path);
            if(listBox1.SelectedIndex >= 0)
            {
                FileInfo[] fi = di.GetFiles(listBox1.Items[listBox1.SelectedIndex].ToString() + ".txt");
                button2.Enabled = true;
                if (fi[0].Length > 0)
                {
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    textBox1.Enabled = true;
                }
                else
                {
                    button3.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    textBox1.Enabled = false;
                }
            }
        }

        // add new word
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 word = new Form2(dict_path + listBox1.SelectedItem.ToString() + ".txt");
            word.ShowDialog();
            word.Dispose();

            DirectoryInfo di = new DirectoryInfo(dict_path);
            FileInfo[] fi = di.GetFiles(listBox1.Items[listBox1.SelectedIndex].ToString() + ".txt");
            if (fi[0].Length > 0)
            {
                button3.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                textBox1.Enabled = true;
            }
        }

        // provide translation
        private void Translate()
        {
            bool entry = false;

            if (textBox1.Text != "")
            {
                label3.Text = "";
                textBox2.Text = "";

                using (StreamReader sr = new StreamReader(
                    dict_path + listBox1.SelectedItem.ToString() + ".txt", Encoding.Unicode))
                {
                    while (sr.Peek() != -1)
                    {
                        string line = sr.ReadLine();
                        string left = line.Substring(0, line.IndexOf('='));
                        string right = line.Substring(line.IndexOf('=') + 1);
                        if (textBox1.Text == left)
                        {
                            entry = true;
                            label3.Text = " " + left;
                            textBox2.Text = textBox2.Text + "\u25AA " + right + Environment.NewLine;
                        }
                        if (textBox1.Text == right)
                        {
                            entry = true;
                            label3.Text = " " + right;
                            textBox2.Text = textBox2.Text + "\u25AA " + left + Environment.NewLine;
                        }
                    }
                }

                if (!entry)
                {
                    label3.Text = " " + textBox1.Text;
                    textBox2.Text = " No translation in \"" + listBox1.SelectedItem.ToString() + "\"!";
                }

                textBox1.Text = "";
                textBox1.Select();
            }
        }

        // translate button
        private void button5_Click(object sender, EventArgs e)
        {
            Translate();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && button5.Enabled == true)
            {
                Translate();
            }
        }

        // learn mode
        private void button3_Click(object sender, EventArgs e)
        {
            Form3 learn = new Form3(dict_path + listBox1.SelectedItem.ToString() + ".txt",
                                                          listBox1.SelectedItem.ToString());
            learn.ShowDialog();
            learn.Dispose();
        }

        // exam mode
        private void button4_Click(object sender, EventArgs e)
        {
            Form4 exam = new Form4(dict_path + listBox1.SelectedItem.ToString() + ".txt",
                                                        listBox1.SelectedItem.ToString());
            exam.ShowDialog();
            exam.Dispose();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
