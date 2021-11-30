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
            
        Bitmap bmp = new Bitmap(300, 300); //создание места для рисования    
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
                    panel1.Controls.Add(graph.V[i].lb);                    
                }
                graph.AddEdges();
                for (int i = 0; i < graph.ECount; i++)
                {
                    panel1.Controls.Add(graph.E[i].lb);
                    graph.E[i].lb.Click += new System.EventHandler(label_Click);
                }
                b = true;
            }
            else 
            {
                for (int i = graph.ECount - 1; i >= 0; i--)
                {
                    panel1.Controls.Remove(graph.E[i].lb);
                }
                for (int i = graph.VCount - 1; i >= 0; i--)
                {
                    panel1.Controls.Remove(graph.V[i].lb);
                }
                label3.Text = "";
                graph.Remove();
                b = false;
            }
            this.Refresh();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            for (int i = 0; i < graph.ECount; i++)
            {
                graph.E[i].draw(panel1, g);
            }
            for (int i = 0; i < graph.VCount; i++)
            {
                graph.V[i].draw(panel1, g);
            }
            label1.Text = graph.VCount.ToString();
            label2.Text = graph.ECount.ToString();           
        }
       
        private void button3_Click(object sender, EventArgs e) //алгоритм Дейкстры
        {
            int min, minindex, temp;
            //int[] steps = new int[graph.VCount];
            string steps = "";
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
                            steps = steps + "n "; 
                        }
                        else
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

            bool flag = false;
            for (int i = 0;i < graph.VCount; i++) //проверка на связность
            {
                int sum = 0;
                for(int j = 0;j< graph.VCount; j++) 
                {
                    sum = sum + mas[i, j];
                }
                if (sum == 0) 
                {
                    flag = true;
                    break;
                }
            }

            if (flag == false) //если граф связный
            {
                //Восстановление пути
                int[] ver = new int[graph.VCount]; // массив посещенных вершин
                int end = Convert.ToInt32(nud3.Value) - 1; //индекс вершины до которой нужно провести путь
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
            
            Thread.Sleep(5000);
            
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
                                panel1.Controls.Add(graph.E[graph.GetEdgeIndex(temp, graph.V[i])].lb);
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
                panel1.Controls.Add(graph.V[graph.VCount - 1].lb);
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
        }     
    }
}
