using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace AprioriAlg
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
            ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            textBox4.Text = "3";
            textBox5.Text = "0,6";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        String textbox4;
        String textbox5;

        private void button2_Click(object sender, EventArgs e)
        {
            string min_sup, min_conf;
            min_sup = textbox4;
            min_conf = textbox5;
            Apriori apriori = new Apriori(int.Parse(min_sup), double.Parse(min_conf));
            Dictionary<String, int> frequentCollection = apriori.get();
            String[] dataSet = apriori.AprioriShow().ToArray();
            for (int i= 0;i< dataSet.Length;i++ )
            {
                textBox3.Text += dataSet[i].ToString()+"\r\n";
            }
            textBox1.Text = "           Частый набор\r\n";
            foreach (KeyValuePair<string, int> kvp in frequentCollection)// KeyValuePair содержит только одну пару ключ-значение для обхода
            {
                textBox1.Text += kvp.Key + ":" + kvp.Value + "\r\n";
            }
            Dictionary<String, Double> relationRules = apriori.Rules(frequentCollection);
            
            textBox2.Text = "           Правила ассоциации" + "\r\n";
            foreach (KeyValuePair<string, double> kvp in relationRules)
            {
                textBox2.Text += kvp.Key + ":" + kvp.Value + "\r\n";
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textbox4 = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textbox5 = textBox5.Text;
        }
    }
}
