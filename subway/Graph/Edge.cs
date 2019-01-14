using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subway.Graph
{
    class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Dis { get; set; }

        public Edge(int from, int to, int dis)
        {
            From = from;
            To = to;
            Dis = dis;
        }
    }
}
