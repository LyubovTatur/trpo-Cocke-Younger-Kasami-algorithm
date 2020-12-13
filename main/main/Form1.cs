using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace main
{
    public partial class Form1 : Form
    {
        CYK cyk = new CYK();
        // Regex reg = new Regex(@"(\w+)\s+->\s+(\w+)\|*(\w+)*");
        Regex reg = new Regex("(\\w+)\\s*->\\s*(\\w+)\\s*\\|*\\s*(\\w+)*");
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.RowCount = 1;
            dataGridView1.ColumnCount = 1;
            dataGridView1.Rows.Clear();
            cyk.Clear();
            cyk.word = textBox1.Text;
            foreach (var line in richTextBox1.Lines)
            {
                if (reg.IsMatch(line))
                {
                    cyk.AddNonterm(reg.Match(line).Groups[1].Value, new List<string> { reg.Match(line).Groups[2].Value, reg.Match(line).Groups[3].Value, reg.Match(line).Groups[4].Value, reg.Match(line).Groups[5].Value });
                    // MessageBox.Show(reg.Match(line).Groups[1].Value + " " + reg.Match(line).Groups[2].Value + " " + reg.Match(line).Groups[3].Value + " " + reg.Match(line).Groups[4].Value + " " + reg.Match(line).Groups[5].Value + "\\\\ " + line);
                }
            }
          

            dataGridView1.ColumnCount = cyk.word.Length;
            dataGridView1.RowCount = cyk.word.Length;
            

            int J = dataGridView1.RowCount - 1;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = dataGridView1.RowCount - 1; j > J; j--)
                {

                    dataGridView1[j, i].Style.BackColor = Color.Gray;
                }
                J--;
            }
            //for (int j = 0; j > dataGridView1.RowCount; j++)
            //    dataGridView1.Cells[j,0].va = cyk.word[j].ToString();
            trackBar1.Maximum = cyk.word.Length;
            var table = cyk.algorythm2try();
            int n = dataGridView1.RowCount;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < table[i][j].Count; k++)
                    {
                        dataGridView1[j, i].Value+= table[i][j][k];
                    }
                    
                }
                n--;
            }

        }
    }

    public class CYK 
    {
        public string word;

        List<Nonterminal> nonterminals = new List<Nonterminal>();
        public void AddNonterm(string nonterm, List<string> terms)
        {
            nonterminals.Add(new Nonterminal(nonterm, terms));
        }

        public void Clear()
        {
            nonterminals = new List<Nonterminal>();
        }
        
        public void algorithm1try()
        {
            int n = word.Length, r = nonterminals.Count;
            bool[,,] P = new bool[n,n,r];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int t = 0; t < r; t++)
                    {
                        P[i, j, t] = false;
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    if (nonterminals[j].terminals.Contains(word[i].ToString()))
                    {
                        P[1, i, j] = true;
                    }
                }
            }
            for (int i = 1; i < n; i++)
            {
                for (int j = 0; j < n-i+1; j++)
                {
                    for (int k = 0;  k < i-1;  k++)
                    {

                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                if (P[n-1,1,i])
                {
                    MessageBox.Show("d");
                }
                
            }

        }
        
        public List<List<List<string>>> algorythm2try()
        {
            int n = word.Length, r = nonterminals.Count;
            List<List<List<string>>> table = new List<List<List<string>>>();
            for (int i = 0; i < n; i++)
            {
                table.Add(new List<List<string>>());
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    table[i].Add(new List<string>());
                }
                    
                
            }
            for (int i = 0; i < n; i++)
            {
                foreach (var nonterm in nonterminals)
                {
                    foreach (var term in nonterm.terminals)
                    {
                        if (word[i].ToString() == term)
                        {
                            
                                table[0][i].Add( nonterm.nonterminal);
                            
                        }
                    }
                }
            }
            int n1 = n-1;
            
            for (int i = 1; i < n; i++) //иду по стпрочкам
            {
                for (int j = 0; j < n1; j++)// столбцы
                { 
                    int stolbec1 = j, stolbec2 = j+1;//начальные
                    int w = i - 1;
                   // MessageBox.Show(w.ToString());
                    // мы должны брать построчно что то
                    for (int k = 0; k < i; k++)//строчки от нуля до позиции
                    {
                        //MessageBox.Show(table[0][0].Count.ToString() + " " + table[0][1].Count.ToString());
                        // MessageBox.Show(w.ToString());
                        //MessageBox.Show(table[k][stolbec1].Count.ToString() + " " + table[w][stolbec2].Count.ToString());
                        if (table[k][stolbec1].Count>0  && table[w][stolbec2].Count > 0)
                        {
                            

                            List<string>znach1 = table[k][stolbec1];
                                List<string>znach2 = table[w][stolbec2];
                            
                                


                           
                            List<string> rescombines = new List<string>();
                            for (int combs1 = 0; combs1 < znach1.Count; combs1++)
                            {
                                for (int combs2 = 0; combs2 < znach2.Count; combs2++)
                                {
                                    rescombines.Add(znach1[combs1] + znach2[combs2]);
                                    
                                }
                            }
                            foreach (var non in nonterminals)
                            {

                                foreach (var term in non.terminals)
                                {
                                    foreach (var combs in rescombines)
                                    {
                                        if (term == combs)
                                        {

                                            if (table[i][j].Count >0)
                                            {
                                                
                                                
                                                
                                                bool g = false;
                                                for (int h = 0; h < table[i][j].Count; h++)
                                                {
                                                    if (table[i][j][h] == non.nonterminal)
                                                    {
                                                        g = true;
                                                    }
                                                }
                                                if (!g)
                                                {
                                                    table[i][j].Add(non.nonterminal);
                                                }
                                                
                                            }
                                            else
                                            {
                                                table[i][j].Add(non.nonterminal);
                                            }
                                            
                                        }
                                    }
                                }
                            }
                        }

                        stolbec2++;
                        w--;
                        
                    }
                }
                n1--;//дальше они уменьшгаются
            }
            return table;
        }
       
    }

    public class Nonterminal
    {
        public string nonterminal;
        public List<string> terminals = new List<string>();

        public Nonterminal(string nonterminal, List<string> terminals)
        {
            this.nonterminal = nonterminal;
            this.terminals = terminals;
        }
    }
}
