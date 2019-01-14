using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace subway
{
    using Graph;

    public partial class GUIForm : Form
    {
        private float ratioX = 1.0f, ratioY = 1.0f;
        private Path path;
        private int current = 0;
        private Graphics G;
        private Font font = new Font("微软雅黑", 20);

        public GUIForm()
        {
            InitializeComponent();
        }

        private void DrawDot(int id, Brush brush)
        {
            float x = Map.subwayMap.Stations[id].x, y = Map.subwayMap.Stations[id].y;
            x *= ratioX; y *= ratioY;
            G.FillEllipse(brush, x, y, 10, 10);
        }

        private void ShowPath()
        {
            MapBox.Refresh();
            current = 0;
            DrawDot(path[0].Item1, Brushes.Red);
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!Map.StationId.ContainsKey(textBox1.Text))
            {
                MessageBox.Show("站名有误或未收录该站点!");
                return;
            }
            timer1.Enabled = false;
            timer1.Interval = 100;
            int id = Map.StationId[textBox1.Text];
            path = GraphSolver.TravelAroundPath(id);
            ShowPath();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawDot(path[current].Item1, Brushes.LightGreen);
            current++;
            if (current == path.Count)
            {
                timer1.Enabled = false;
                InfoText.Text = string.Format("已完成，共{0}站", current);
                return;
            }
            DrawDot(path[current].Item1, Brushes.Red);
            InfoText.Text = string.Format("{0}站", current);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(!Map.StationId.ContainsKey(DepatureText.Text) || !Map.StationId.ContainsKey(TerminalText.Text))
            {
                MessageBox.Show("站名有误或未收录该站点!");
                return;
            }
            timer1.Enabled = false;
            timer1.Interval = 300;
            int id1 = Map.StationId[DepatureText.Text], id2 = Map.StationId[TerminalText.Text];
            path = GraphSolver.CalcPath(id1, id2);
            ShowPath();
        }


        private void GUIForm_Load(object sender, EventArgs e)
        {
            ratioX = MapBox.Width / 1035f;
            ratioY = MapBox.Height / 680f;
            G = MapBox.CreateGraphics();
        }
    }
}
