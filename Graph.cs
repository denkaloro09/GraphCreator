using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace AlgosGraph
{
    
    class Vertex //вершина
    {
        public int Number { get; set; } //у вершины есть число, котоое там хранится
        public Label lb;
        public bool Clicked;
        public int Info { get; set; }
    
        public int x, y, r; //координаты вершины на экране и радиус
        public Vertex(int _number, int _x, int _y) //создание вершины
        {
            Clicked = false;
            Info = 0;
            r = 12;
            x = _x;
            y = _y;
            lb = new Label();
            Number = _number;            
            lb.AutoSize = true;
            lb.Size = new Size(13, 20);
        }
        public override string ToString()
        {
            return Number.ToString();
        }
        public bool isSpaceTaken(int a,int b) 
        {
            if (((a - x) * (a - x) + (b - y) * (b - y)) <= (36*36))
            {
                
                return true;               
            }
            else
            {
                return false;
            }
        }
        public bool isSpaceTM(MouseEventArgs e)
        {
            if (((e.X - x) * (e.X - x) + (e.Y - y) * (e.Y - y)) <= (36 * 36))
            {
                Clicked = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void draw(Panel sender, Graphics g) //метод для рисования 
        {
            Rectangle rect = new Rectangle(x - r, y - r, r * 2, r * 2);
            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.WhiteSmoke); ;
            lb.Text = Number.ToString();
            lb.Location = new Point(x - r/2 -lb.Width/5, y - r/2);
            switch (Info)
            {
                case 0:
                    brush.Color = Color.WhiteSmoke;
                    lb.BackColor = Color.WhiteSmoke;
                    break;
                case 1:
                    brush.Color = Color.Red;
                    lb.BackColor = Color.Red;
                    break;
                case 2:
                    brush.Color = Color.Gray;
                    lb.BackColor = Color.Gray;
                    break;
            }
            g.FillEllipse(brush, rect);
            g.DrawEllipse(pen, rect);
        }
    }
    class Edge //ребро
    {
        public Vertex From { get; set; } //откуда
        public Vertex To { get; set; } //куда
        public int Weight { get; set; } //вес, по умолчанию всегда равно 1
        public int Info { get; set; }
        
        public Edge(Vertex F,Vertex T, int w = 1) 
        {
            Info = 0;
            From = F;
            To = T;
            Weight = w;
        }
        public void draw(Panel sender,Graphics g) //рисование ребра
        {
            
            Pen pen = new Pen(Color.Black);
            switch (Info)
            {
                case 0:
                    pen.Color = Color.Black;
                    
                    break;
                case 1:
                    pen.Color = Color.Red;
                    
                    break;
                case 2:
                    pen.Color = Color.Red;
                   
                    break;
            }
            g.DrawLine(pen, From.x,From.y,To.x,To.y);
        }

    }
    class Graph //собственно граф
    {
        public List<Vertex> V = new List<Vertex>(); //список вершин
        public List<Edge> E = new List<Edge>(); //список ребер
        public int VCount => V.Count; //количество вершин
        public int ECount => E.Count; //количество ребер
        public void AddV(Vertex vertex) //функция добавления вершины в граф
        {
            V.Add(vertex);
        }
        public void AddE(Vertex from,Vertex to) //функция добавления ребра в граф
        {
            Edge edge = new Edge(from, to);
            E.Add(edge);        
        }
        public void drawGraph(Panel sender, int index, Graphics g) //рисование вершин графа
        {          
                V[index].draw(sender, g);           
        }
        public bool isVertexClicked(MouseEventArgs e) //нажали ли на вершину
        {
            for (int i = 0; i < VCount; i++)
            {                              
                    if (V[i].isSpaceTM(e) == true)
                    {                    
                        return true;
                        break;
                    }                
            }
            return false;
        }
        public bool isVertexClicked(int a,int b) //тоже самое, что и сверху, только с двумя случайнми числами
        {
            for (int i = 0; i < VCount; i++)
            {
                if (V[i].isSpaceTaken(a,b) == true)
                {
                    return true;
                    break;
                }
            }
            return false;
        }
        
        public void CreateV(int index) //создание вершин графа
        {
            Random rnd = new Random();
            //добавляем первую вершину
            if (index > VCount)
            {
                AddV(new Vertex(VCount + 1, rnd.Next(20, 350), rnd.Next(20, 350)));
                for (int i = VCount + 1; i < index + 1; i++)
                {
                    int a = rnd.Next(20, 350);
                    int b = rnd.Next(20, 350);
                    //если новые координаты не совпади с координатами других вершин
                    if (isVertexClicked(a, b) == false)
                    {
                        AddV(new Vertex(i, a, b)); //то добавляем вершину
                    }
                }
            }       
        }      
        public void AddEdges() //добавление ребер
        {
            Random rnd = new Random();
            for (int i = 0;i<VCount-1;i++) //сначала добавлем от первого до последнего
            {
                int a;               
                a = rnd.Next(i + 1, VCount);
                              
                bool b = true;
                foreach(var v in GetVList(V[i])) 
                {
                    if (v == V[a]) 
                    {
                        b = false;
                    }
                }
                if (b == true) 
                {
                    AddE(V[i], V[a]);
                }                                          
            }
            for(int i = VCount-1;i > 0; i--) //потом наоборот
            {
                int a;
                a = rnd.Next(0, i-1);
                bool b = true;
                foreach (var v in GetVList(V[i]))
                {
                    if (v == V[a])
                    {
                        b = false;
                    }
                }
                if (b == true)
                {
                    AddE(V[i], V[a]);
                }
            }
        }
        public void FalseColor() 
        {
            for(int i = 0; i < ECount; i++) 
            {
                E[i].Info = 0;
            }
        }
        public void IsEdge(Vertex from,Vertex to) 
        {
            foreach (var edge in E)
            {
                if (edge.From == from)
                {
                    if (edge.To == to)
                    {
                        edge.Info = 2;
                        break;
                    }
                }
                else if (edge.From == to) 
                {
                    if (edge.To == from)
                    {
                        edge.Info = 2;
                        break;
                    }
                }            
            }          
        }
        public void Remove() //удаление графа
        {
            for(int i = ECount - 1; i >= 0; i--) 
            {
                E.RemoveAt(i);
            }
            for(int i = VCount -1;i >= 0; i--) 
            {               
                V.RemoveAt(i);
            }
        }
        public int[,] getMatrix()  //получение матрицы смежности
        {
            var matrix = new int[V.Count, V.Count];

            foreach(var edge in E) 
            {
                var row = edge.From.Number - 1;
                var column = edge.To.Number - 1;
                matrix[row, column] = edge.Weight; 
            }
            return matrix;
        }
        public List<Vertex> GetVList(Vertex vertex) //получение списка смежных вершин у данной вершины
        {
            var result = new List<Vertex>();
            foreach(var edge in E) 
            {
                if(edge.From == vertex) 
                {
                    result.Add(edge.To);
                }
                if(edge.To == vertex) 
                {
                    result.Add(edge.From);
                }
            }
            NameComparer vn = new NameComparer();
            result.Sort(vn);
            return result;
        }
        
        public bool Wave(Vertex start,Vertex finish) 
        {          
            var list = new List<Vertex>();

            list.Add(start);

            for(int i = 0;i < list.Count; i++) 
            {
                var vertex = list[i];
                foreach (var v in GetVList(vertex))
                {
                    if (!list.Contains(v))
                    {
                        list.Add(v);
                    }
                }
            }
            return list.Contains(finish);
        }
        public void Find(Vertex start, Vertex goal)
        {
            for (int i = 0; i < VCount; i++)
            {
                Queue<Vertex> list = new Queue<Vertex>();
                list.Enqueue(start);
                while (list.Count != 0)
                {
                    Vertex ver = list.Peek();
                    list.Dequeue();
                    V[i].Info = 2;
                    foreach (var v in GetVList(V[i])) //если вершина смежная
                    {
                        if(v.Info == 0) //если вершина не обнаружена
                        {
                            list.Enqueue(v);
                            v.Info = 1;                           
                        }
                    }
                }
            }
        }
    }
    class NameComparer : IComparer<Vertex>
    {
        public int Compare(Vertex o1, Vertex o2)
        {
            if (o1.Number > o2.Number)
            {
                return 1;
            }
            else if (o1.Number < o2.Number)
            {
                return -1;
            }
            return 0;
        }
    }

}
