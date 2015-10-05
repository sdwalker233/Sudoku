using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        DLX a=new DLX();
        Point Now;
        int[,] pMap = new int[10, 10];
        int[,] pAns = new int[10, 10];
        public int x, y;

        public Form1()
        {
            InitializeComponent();
            this.Width = this.Width - this.ClientRectangle.Width + 352;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            for(int i = 1; i <= 10; i++)
            {
                if (i % 3 == 1)
                {
                    g.DrawLine(new Pen(Brushes.Black, 3), 32, i * 32, 320, i * 32);
                }
                else
                {
                    g.DrawLine(new Pen(Brushes.Black, 1), 32, i * 32, 320, i * 32);
                }
            }
            for(int i = 1; i <= 10; i++)
            {
                if(i % 3 == 1)
                {
                    g.DrawLine(new Pen(Brushes.Black, 3), i * 32, 32, i * 32, 320);
                }
                else
                {
                    g.DrawLine(new Pen(Brushes.Black, 1), i * 32, 32, i * 32, 320);
                }
            }
            g.FillRectangle(Brushes.SkyBlue, 32, 416, 130, 32);
            SizeF s1 = g.MeasureString("Solve", new Font("Consolas", 16));
            g.DrawString("Solve", new Font("Consolas", 16), Brushes.White, 32 + (130 - s1.Width) / 2, 416 + (32 - s1.Height) / 2);

            g.FillRectangle(Brushes.LightPink, 190, 416, 130, 32);
            SizeF s2 = g.MeasureString("Reset", new Font("Consolas", 16));
            g.DrawString("Reset", new Font("Consolas", 16), Brushes.White, 190 + (130 - s2.Width) / 2, 416 + (32 - s2.Height) / 2);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            x = (this.PointToClient(MousePosition).X - 32) / 32 + 1;
            y = (this.PointToClient(MousePosition).Y - 32) / 32 + 1;
            if(e.Button == MouseButtons.Left)
            {
                if (x >= 1 && x <= 9 && y >= 1 && y <= 9)
                {
                    Now.X = x; Now.Y = y;
                    Graphics g = this.CreateGraphics();
                    g.FillRectangle(Brushes.White, 32 , 352 , 300, 32);
                    g.DrawLine(new Pen(Brushes.Black, 3), 32, 352, 320, 352);
                    g.DrawLine(new Pen(Brushes.Black, 3), 32, 384, 320, 384);
                    g.DrawLine(new Pen(Brushes.Black, 3), 32, 352, 32, 384);
                    g.DrawLine(new Pen(Brushes.Black, 3), 320, 352, 320, 384);
                    for (int i = 2; i <= 9; i++)
                    {
                        g.DrawLine(new Pen(Brushes.Black, 1), 32 * i, 352, 32 * i, 384);
                    }
                    for (int i = 1; i <= 9; i++)
                    {
                        
                        SizeF s = g.MeasureString(Convert.ToString(i), new Font("Consolas", 16));
                        g.DrawString(Convert.ToString(i), new Font("Consolas", 16), Brushes.Black, i * 32 + (32 - s.Width) / 2, 352 + (32 - s.Height) / 2);
                    }
                }
                else if (x >= 1 && x <= 9 && y == 11)
                {
                    Graphics g = this.CreateGraphics();
                    g.FillRectangle(Brushes.White, Now.X * 32 + 2, Now.Y * 32 + 2, 28, 28);
                    SizeF s = g.MeasureString(Convert.ToString(x), new Font("Consolas", 16));
                    g.DrawString(Convert.ToString(x), new Font("Consolas", 16), Brushes.Black, Now.X * 32 + (32 - s.Width) / 2, Now.Y * 32 + (32 - s.Height) / 2);
                    pMap[Now.Y, Now.X] = x;
                    g.FillRectangle(Brushes.White, 30, 350, 322, 36);
                }
                else if (x >= 1 && x <= 4 && y == 13)
                {
                    solve();
                }
                else if (x >= 6 && x <= 9 && y == 13)
                {
                    Graphics g = this.CreateGraphics();
                    for (int i = 1; i <= 9; i++)
                    {
                        for (int j = 1; j <= 9; j++)
                        {
                            g.FillRectangle(Brushes.White, i * 32 + 2, j * 32 + 2, 28, 28);
                            pMap[i, j] = 0;
                        }
                    }
                        
                }
            }
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            x = (this.PointToClient(MousePosition).X - 32) / 32 + 1;
            y = (this.PointToClient(MousePosition).Y - 32) / 32 + 1;
            Graphics g = this.CreateGraphics();
            if (x >= 1 && x <= 9 && y >= 1 && y <= 9)
            {
                g.FillRectangle(Brushes.White, x * 32 + 2, y * 32 + 2, 28, 28);
                pMap[y, x] = 0;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode == 13) solve();
            char ch = (char)e.KeyCode;
            if (ch >= '1' && ch <= '9' && x >= 1 && x <= 9 && y >= 1 && y <= 9)
            {
                Graphics g = this.CreateGraphics();
                g.FillRectangle(Brushes.White, x * 32 + 2, y * 32 + 2, 28, 28);
                SizeF s = g.MeasureString(ch.ToString() , new Font("Consolas", 16));
                g.DrawString(ch.ToString() , new Font("Consolas", 16), Brushes.Black, x * 32 + (32 - s.Width) / 2, y * 32 + (32 - s.Height) / 2);
                pMap[y, x] = (int)ch - '0';
                //MessageBox.Show(pMap[x, y].ToString());
                g.FillRectangle(Brushes.White, 30, 350, 322, 36);
                //
            }
        }

        private void insert(int x, int y, int v)
        {
            int l=((x-1)*9+y-1)*9+v;
	        a.Link(l,(x-1)*9+y);
	        a.Link(l,81+(x-1)*9+v);
	        a.Link(l,162+(y-1)*9+v);
	        a.Link(l,243+((x-1)/3*3+(y-1)/3)*9+v);
        }

        private void solve()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            a.init(729, 324);
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    pAns[i, j] = 0;
                }
            }
            
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    if (pMap[i, j] >= 1&&pMap[i , j] <= 9)
                    {
                        insert(i, j, pMap[i,j]);
                    }
                    else
                    {
                        for (int k = 1; k <= 9; k++)
                        {
                            insert(i, j, k);
                        }
                    }
                }
            }
            
            if (a.Dance(0))
            {
                for (int i = 0; i < a.ansd; i++)
                {
                    if (pMap[(a.ans[i] - 1) / 9 / 9 + 1, ((a.ans[i] - 1) / 9) % 9 + 1]==0)
                        pAns[(a.ans[i] - 1) / 9 / 9 + 1, ((a.ans[i] - 1) / 9) % 9 + 1] = (a.ans[i] - 1) % 9 +1;
                }
                Graphics g = this.CreateGraphics();
                for(int i = 1; i <= 9; i++)
                {
                    for(int j = 1; j <= 9; j++)
                    {
                        if(pAns[i, j] != 0)
                        {
                            g.FillRectangle(Brushes.White, j * 32 + 2, i * 32 + 2, 28, 28);
                            SizeF s = g.MeasureString(Convert.ToString(pAns[i, j]), new Font("Consolas", 16));
                            g.DrawString(Convert.ToString(pAns[i, j]), new Font("Consolas", 16), Brushes.Red, j * 32 + (32 - s.Width) / 2, i * 32 + (32 - s.Height) / 2);
                        }
                    }
                }
                watch.Stop();
                MessageBox.Show("用时"+watch.ElapsedMilliseconds.ToString()+"毫秒!","Congratulations!");
            }
            else
            {
                MessageBox.Show("Unsolvable!","Sorry");
            }
        }
    }

    class DLX
    {
        public const int maxnode = 4000;
        public const int MaxN = 800;
        public const int MaxM = 400;
        int n, m, size;
        int[] U = new int[maxnode];
        int[] D = new int[maxnode];
        int[] R = new int[maxnode];
        int[] L = new int[maxnode];
        int[] Row = new int[maxnode], Col = new int[maxnode];
        int[] H = new int[MaxN], S = new int[MaxM];

        public int[] ans = new int[MaxN];
        public int ansd;
        public void init(int _n, int _m)
        {
            n = _n;
            m = _m;
            ansd = 0;
            for (int i = 0; i <= m; i++)
            {
                S[i] = 0;
                U[i] = D[i] = i;
                L[i] = i - 1;
                R[i] = i + 1;
            }
            R[m] = 0; L[0] = m;
            size = m;
            for (int i = 1; i <= n; i++) H[i] = -1;
        }
        public void Link(int r, int c)
        {
            ++S[Col[++size] = c];
            Row[size] = r;
            D[size] = D[c];
            U[D[c]] = size;
            U[size] = c;
            D[c] = size;
            if (H[r] < 0) H[r] = L[size] = R[size] = size;
            else
            {
                R[size] = R[H[r]];
                L[R[H[r]]] = size;
                L[size] = H[r];
                R[H[r]] = size;
            }
        }
        void remove(int c)
        {
            L[R[c]] = L[c]; R[L[c]] = R[c];
            for (int i = D[c]; i != c; i = D[i])
                for (int j = R[i]; j != i; j = R[j])
                {
                    U[D[j]] = U[j];
                    D[U[j]] = D[j];
                    --S[Col[j]];
                }
        }
        void resume(int c)
        {
            for (int i = U[c]; i != c; i = U[i])
                for (int j = L[i]; j != i; j = L[j])
                    ++S[Col[U[D[j]] = D[U[j]] = j]];
            L[R[c]] = R[L[c]] = c;
        }
        public bool Dance(int d)
        {
            if (R[0] == 0)
            {
                return true;
            }
            int c = R[0];
            for (int i = R[0]; i != 0; i = R[i])
                if (S[i] < S[c])
                    c = i;
            remove(c);
            for (int i = D[c]; i != c; i = D[i])
            {
                ans[d] = Row[i]; ansd = d + 1;
                for (int j = R[i]; j != i; j = R[j]) remove(Col[j]);
                if (Dance(d + 1)) return true;
                for (int j = L[i]; j != i; j = L[j]) resume(Col[j]);
            }
            resume(c);
            return false;
        }
    };
}
