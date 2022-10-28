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
    public partial class Form4 : Form
    {
        private string path;
        private List<String> entry_left;
        private List<String> entry_right;
        private Random rnd;
        private int entry_index;
        private int correct;
        private int wrong;
        private int count;

        public Form4(string path, string dict_name)
        {
            InitializeComponent();

            this.path = path;
            this.Text = "Exam: \'" + dict_name + "\'.";

            label1.Text = "Translate the words. Type your answer in the text field below.";

            entry_left = new List<string>();
            entry_right = new List<string>();
            rnd = new Random();

            ExamSession();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Check();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                Check();
            }
        }

        // initiate new exam session
        private void ExamSession()
        {
            label3.Text = "";
            label4.Text = "Correct: ";
            label5.Text = "Wrong: ";

            entry_left.Clear();
            entry_left.Clear();

            correct = 0;
            wrong = 0;

            using (StreamReader sr = new StreamReader(path, Encoding.Unicode))
            {
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine();
                    string left = line.Substring(0, line.IndexOf('='));
                    string right = line.Substring(line.IndexOf('=') + 1);

                    entry_left.Add(left);
                    entry_right.Add(right);
                }
            }

            count = entry_left.Count;

            NextWord();

        }

        // pick the next word to translate
        private void NextWord()
        {
            if (count > 0)
            {
                textBox1.Select();
                entry_index = rnd.Next(entry_left.Count);
                label2.Text = entry_right.ElementAt(entry_index);
            }
            else
            {
                int result = (correct * 100) / (correct + wrong);
                string grade = "";

                if (result < 60)
                {
                    grade = "\'Very poor\'";
                }

                if (result >= 60 && result < 70)
                {
                    grade = "\'Poor\'";
                }

                if (result >= 70 && result < 80)
                {
                    grade = "\'Satisfactory\'";
                }

                if (result >= 80 && result < 90)
                {
                    grade = "\'Good\'";
                }

                if (result >= 90 && result < 100)
                {
                    grade = "\'Very good\'";
                }

                if(result == 100)
                {
                    grade = "\'Excellent\'";
                }

                if (MessageBox.Show("Your grade is " + grade + " with " + result.ToString() + 
                    "% of correct answers. Would you like to start over?", "Exam session complete",
                              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ExamSession();
                }
                else
                {
                    this.Close();
                }
            }
        }

        // ckeck a word-translation pair for a match
        private void Check()
        {
            if(textBox1.Text == "")
            {
                return;
            }

            if (textBox1.Text == entry_left.ElementAt(entry_index))
            {
                label3.ForeColor = Color.Green;
                label3.Text = entry_right .ElementAt(entry_index) + " = " + 
                                            entry_left.ElementAt(entry_index);
                correct++;
                label4.Text = "Correct: " + correct.ToString();
            }
            else
            {
                label3.ForeColor = Color.Red;
                label3.Text = entry_right .ElementAt(entry_index) + " \u2260 " + textBox1.Text +
                    " (" + entry_left.ElementAt(entry_index) + ")";
                wrong++;
                label5.Text = "Wrong:  " + wrong.ToString();
            }

            textBox1.Text = "";

            entry_left.RemoveAt(entry_index);
            entry_right.RemoveAt(entry_index);
            count--;

            NextWord();
        }
    }
}
