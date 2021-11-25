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
                b = true;
            }
            else 
            {
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
                graph.drawGraph(panel1, i, g);
            }
            label1.Text = graph.VCount.ToString();
            label2.Text = graph.ECount.ToString();           
        }

        

        private void button2_Click(object sender, EventArgs e)
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
            Refresh();            
            Thread.Sleep(1000);
            for(int i = 0; i < graph.VCount; i++) 
            {
                graph.V[i].Info = 0;
            }
            graph.FalseColor();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
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
    }
}
