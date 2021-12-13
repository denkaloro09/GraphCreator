using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AlgosGraph
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();           
        }
            
        Bitmap bmp = new Bitmap(1000, 1000); //создание места для рисования    
        Graph graph = new Graph();
        bool b = false;
        Vertex temp;
        Label lbtemp;
                   
        private void button1_Click(object sender, EventArgs e)
        {
            if (b == false)
            {
                graph.CreateV(Convert.ToInt32(nud1.Value));
                for (int i = 0; i < graph.VCount; i++)
                {
                    pictureBox1.Controls.Add(graph.V[i].lb);                    
                }
                graph.AddEdges();
                for (int i = 0; i < graph.ECount; i++)
                {
                    pictureBox1.Controls.Add(graph.E[i].lb);
                    graph.E[i].lb.Click += new System.EventHandler(label_Click);
                }
                b = true;
            }
            else 
            {
                for (int i = graph.ECount - 1; i >= 0; i--)
                {
                    pictureBox1.Controls.Remove(graph.E[i].lb);
                }
                for (int i = graph.VCount - 1; i >= 0; i--)
                {
                    pictureBox1.Controls.Remove(graph.V[i].lb);
                }
                label3.Text = "";
                graph.Remove();
                b = false;
            }
            Refresh();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.WhiteSmoke);
            for (int i = 0; i < graph.ECount; i++)
            {
                graph.E[i].draw(pictureBox1, g,bmp);
            }
            for (int i = 0; i < graph.VCount; i++)
            {
                graph.V[i].draw(pictureBox1, g,bmp);
            }
            label1.Text = graph.VCount.ToString();
            label2.Text = graph.ECount.ToString();           
        }
        private void Form_Paint(object sender, PaintEventArgs e) 
        {
            
        }
        private void butttonRGR_Click(object sender, EventArgs e)  //алгоритм для РГР
        {
            string s = "";
            int[,] mas = graph.getMatrixOriented(); //получаем массив весов у основого графа
            int ourVertex = Convert.ToInt32(nud2.Value) - 1; // наша начальная вершина

            int max = 0;
            
            for (int i = 0; i < graph.VCount; i++) //макс из того что есть
            {
                if (mas[ourVertex, i] > max)
                {
                    max = mas[ourVertex, i];
                    s = s + max.ToString();
                    label15.Text = s;
                    Thread.Sleep(100);
                }
            }
            
            bool flag1 = true;
            //bool flag2 = true;
            List<int> mtemp = new List<int>(); //список достижимых вершин
            while (flag1 == true)          
            {
                //поиск в ширину
                string stg = "";
                
                Queue<Vertex> list = new Queue<Vertex>();
                list.Enqueue(graph.V[Convert.ToInt32(nud2.Value) - 1]);
                while (list.Count != 0)
                {
                    Vertex ver = list.Peek(); //извлекаем вершину
                    list.Dequeue();
                    ver.Info = 2; //отмечаем ее как посещенную                                  
                    mtemp.Add(ver.Number); //если вершина посещена, то добавляем ее в список достижимых вершин
                    stg = stg + " " + ver.Number.ToString();
                    label16.Text = stg;
                    foreach (var v in graph.GetVListOrineted(ver,max)) //если вершина смежная
                    {
                        if (v.Info == 0) //если вершина не обнаружена
                        {
                            graph.IsEdgeOrineted(ver, v);
                            list.Enqueue(v);
                            v.Info = 1; //обнаружена
                            Refresh();
                            Thread.Sleep(700);
                        }
                    }
                }

                for (int i = 0; i < graph.VCount; i++) //обнуляем цвета и все такое
                {
                    graph.V[i].Info = 0;
                }
                graph.FalseColor();
                Refresh();

                if (mtemp.Contains(Convert.ToInt32(nud2.Value)) && mtemp.Contains(Convert.ToInt32(nud3.Value)))
                {
                    flag1 = false;
                    s = s + "n";
                    label15.Text = s;
                }
               /* if (mtemp.Contains(Convert.ToInt32(nud3.Value)))
                {
                    flag2 = false;
                    s = s + "s";
                    label15.Text = s;
                }*/
                mtemp.Clear();
                max = max - 1;
                s = s + " " + max.ToString();
                label15.Text = s;
                Thread.Sleep(100);
                if (max < 0)
                    break;

            }
            max = max + 1;
            label14.Text = max.ToString();

        }

        



        private void button3_Click(object sender, EventArgs e) //алгоритм Дейкстры
        {
            int min, minindex, temp;
            string steps = "";
            for(int i = 0; i < graph.VCount; i++) 
            {
                steps = steps + graph.V[i].Number.ToString() + "  ";
            }
            steps = steps + "\n";
            for(int i = 0;i < graph.VCount; i++) 
            {
                graph.V[i].metka = 10000;
            }
            graph.V[Convert.ToInt32(nud2.Value) - 1].metka = 0; //вершина с которой начинается поиск пути
            int [,] mas = graph.getMatrix(); //матрица связей
            do
            {
                minindex = 10000; 
                min = 1000;
                for (int i = 0; i < graph.VCount; i++)
                {
                    if (graph.V[i].Info == 0 && graph.V[i].metka < min)
                    {
                        min = graph.V[i].metka;
                        minindex = i;
                       
                    }
                }
                if (minindex != 10000)
                {
                    for (int i = 0; i < graph.VCount; i++)
                    {
                        if (mas[minindex,i] > 0) 
                        {
                            temp = min + mas[minindex, i];
                            if(temp < graph.V[i].metka) 
                            {
                                graph.V[i].metka = temp;
                            }

                        }
                        if (graph.V[i].metka == 10000)
                        {
                            steps = steps + "n  "; 
                        }
                        else if (graph.V[i].metka < 10)
                        {
                            steps = steps + graph.V[i].metka.ToString() + "  ";
                        }
                        else  if(graph.V[i].metka >= 10)
                        {
                            steps = steps + graph.V[i].metka.ToString() + " ";
                        }
                        
                    }
                    steps = steps + "\n";
                    graph.V[minindex].Info = 1;                 
                    Refresh();
                    Thread.Sleep(700);
                }
            } while (minindex < 10000);
            label11.Text = steps;
            string s = "";
            for (int i = 0; i < graph.VCount; i++)
                s = s + graph.V[i].metka.ToString() + " ";
            label12.Text = s;
            for (int i = 0; i < graph.VCount; i++)
            {
                graph.V[i].Info = 0;
            }
            Refresh();

            int end = Convert.ToInt32(nud3.Value) - 1; //индекс вершины до которой нужно провести путь

            bool flag = false;
            for (int i = 0;i < graph.VCount; i++) //проверка на связность
            {
                int sum = 0;
                for(int j = 0;j< graph.VCount; j++) 
                {
                    sum = sum + mas[i, j];
                }
                if (i == end && sum == 0) 
                {
                    flag = true;
                    break;
                }
            }

            if (flag == false) //если граф связный
            {
                //Восстановление пути
                int[] ver = new int[graph.VCount]; // массив посещенных вершин
                //int end = Convert.ToInt32(nud3.Value) - 1; //индекс вершины до которой нужно провести путь
                ver[0] = end + 1; // начальный элемент - конечная вершина
                int k = 1; // индекс предыдущей вершины (для массива пути)

                int weight = graph.V[end].metka; // вес конечной вершины
                
                while (end != Convert.ToInt32(nud2.Value) - 1) // пока не дошли до начальной вершины
                {
                    for (int i = 0; i < graph.VCount; i++) // просматриваем все вершины
                        if (mas[i, end] != 0)   // если связь между вершинами есть
                        {
                            int tmp = weight - mas[i, end]; // определяем вес пути из предыдущей вершины
                            if (tmp == graph.V[i].metka) // если вес совпал с рассчитанным
                            {                            // значит из этой вершины и был переход
                                graph.isEdgeDeik(i, end); //подсвечиваем ребро между этими вершинами
                                weight = tmp; // сохраняем новый вес
                                end = i;       // сохраняем предыдущую вершину
                                ver[k] = i + 1; // и записываем ее в массив
                                k++;
                            }
                        }
                }
                string str = "";
                for (int i = k - 1; i >= 0; i--)
                    str = str + " " + ver[i].ToString();
                label13.Text = str;
                Refresh();
            }
            
            Thread.Sleep(4000);
            
            graph.FalseColor();
            Refresh();
        }
        private void DFS() 
        {          
               
            

        }
        private void button5_Click(object sender, EventArgs e) //поиск в глубину
        {
            List<Vertex> L = new List<Vertex>();
            Stack<Vertex> S = new Stack<Vertex>();
            int[,] matrix = graph.getMatrix();
            S.Push(graph.V[Convert.ToInt32(nud2.Value)-1]);
            while(S.Count != 0) 
            {
                Vertex StackVer = S.Pop();
                if(L.Contains(StackVer) == false) //если нет в списке посещенных
                {
                    L.Add(StackVer);
                    StackVer.Info = 2;
                    foreach (var v in graph.GetVListDFS(StackVer)) 
                    {
                        graph.IsEdge(StackVer, v);
                        S.Push(v);
                        Refresh();
                        Thread.Sleep(500);
                    }
                }
            }
            Thread.Sleep(1000);
            for (int i = 0; i < graph.VCount; i++)
            {
                graph.V[i].Info = 0;
            }
            graph.FalseColor();
            Refresh();

        }
        private void button2_Click(object sender, EventArgs e) //поиск в ширину
        {
            string s = "";
            Queue<Vertex> list = new Queue<Vertex>();
            list.Enqueue(graph.V[Convert.ToInt32(nud2.Value)-1]);
            while (list.Count != 0)
            {
                Vertex ver = list.Peek(); //извлекаем вершину
                list.Dequeue();
                ver.Info = 2; //отмечаем ее как посещенную               
                s = s + " " + ver.Number.ToString();
                label3.Text = s;
                Refresh();
                Thread.Sleep(700);
                
                foreach (var v in graph.GetVList(ver)) //если вершина смежная
                {
                    if (v.Info == 0) //если вершина не обнаружена
                    {
                        graph.IsEdge(ver, v);
                        list.Enqueue(v);                        
                        v.Info = 1; //обнаружена                       
                        Refresh();
                        //graph.FalseColor();
                        Thread.Sleep(700);                       
                    }
                }
            }
                    
            Thread.Sleep(1000);
            for(int i = 0; i < graph.VCount; i++) 
            {
                graph.V[i].Info = 0;
            }
            graph.FalseColor();
            Refresh();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            lbtemp = null;     
            if (Control.ModifierKeys == Keys.Control) 
            {
                if (graph.isVertexClicked(e) == true) {
                    if (temp == null)
                    {                      
                        for (int i = 0; i < graph.VCount; i++)
                        {
                            if (graph.V[i].Clicked == true)
                            {
                                temp = graph.V[i];
                                graph.V[i].Clicked = false;
                                break;
                            }
                        }
                    }
                    else 
                    {
                        for (int i = 0; i < graph.VCount; i++)
                        {
                            if (graph.V[i].Clicked == true && temp != graph.V[i])
                            {
                                graph.AddE(temp, graph.V[i]);
                                //добавление label у ребра на panel
                                pictureBox1.Controls.Add(graph.E[graph.GetEdgeIndex(temp, graph.V[i])].lb);
                                graph.E[graph.GetEdgeIndex(temp, graph.V[i])].lb.Click += new System.EventHandler(label_Click);
                                temp = null;
                                graph.V[i].Clicked = false;
                                Refresh();
                                break;

                            }
                        }
                    } 
                }
            } 
            else if(graph.isVertexClicked(e) == false)
            {
                graph.AddV(new Vertex(graph.VCount + 1, e.X, e.Y));
                pictureBox1.Controls.Add(graph.V[graph.VCount - 1].lb);
                Refresh();
            }
            else 
            {
                for (int i = 0; i < graph.VCount; i++)
                {
                    if (graph.V[i].Clicked == true)
                    {                       
                        graph.V[i].Clicked = false;
                        break;
                    }
                }
            }                     
        }

        private void label_Click(object sender, EventArgs e)
        {
            Label lab = (Label)sender;

            for (int i = 0; i < graph.ECount; i++)
            {
                if (lab == graph.E[i].lb)
                {
                    lbtemp = graph.E[i].lb;
                    nudForLabel.Value = Convert.ToDecimal(graph.E[i].Weight);
                    break;
                }
            }
            Refresh();
        }

        private void nudForLabel_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < graph.ECount; i++)
            {
                if (lbtemp == graph.E[i].lb)
                {
                    graph.E[i].Weight = Convert.ToInt32(nudForLabel.Value);
                    graph.E[i].lb.Text = graph.E[i].Weight.ToString();
                }
            }

        }

        private void label11_Click(object sender, EventArgs e)
        {
            int[,] mas = graph.getMatrix();
            string s = "";
            for(int i = 0; i < graph.VCount; i++) 
            {
                for(int j = 0;j < graph.VCount; j++) 
                {
                    s = s + mas[i, j].ToString();
                }
                s = s + "\n";
            }
            label11.Text = s;
            Refresh();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Под пропускной способностью пути будем понимать наименьший вес дуги этого пути.\nНаписать алгоритм, определяющий наибольшие пропускные способности путей между: 1) фиксированной парой вершин.\nРазработать алгоритм решения этой задачи и написать программу.","Задача");
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Расчетно-графическую рабоут выполнил:\nКадыров Денис Назирович\nПРО-228 ", "Об Авторе");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string s = "";
            List<int> paths = graph.FindWay(Convert.ToInt32(nud2.Value) - 1, Convert.ToInt32(nud3.Value) - 1,pictureBox1);
            foreach (var i in paths) 
            {
                if (i != paths.Count - 1)
                {
                    s = s + " " + i;
                }
                else
                {
                    s = s + " - " + i;
                }
            }
            label14.Text = s;
            Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
