using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subway.Graph
{
    /// <summary>
    /// GraphSolver 类中使用 SPFA 算法求解最短路的部分
    /// </summary>
    static partial class GraphSolver
    {
        private static List<int> dist, prev;
        private static List<bool> inQueue;
        private static Queue<int> Q;

        private static void InitSPFA()
        {
            dist = new List<int>();
            prev = new List<int>();
            inQueue = new List<bool>();
            Q = new Queue<int>();

            for (int i = 0; i < nodeCount; i++)
            {
                dist.Add(INF);
                prev.Add(-1);
                inQueue.Add(false);
            }
            Q.Clear();
        }

        private static void SPFA(int source)
        {
            foreach (var kv in graphNodeDic[source])
            {
                dist[kv.Value] = 0;
                Q.Enqueue(kv.Value);
                inQueue[kv.Value] = true;
            }
            while (Q.Count != 0)
            {
                int x = Q.Dequeue();
                inQueue[x] = false;
                foreach (Edge e in adjList[x])
                {
                    if (dist[e.To] > dist[e.From] + e.Dis)
                    {
                        dist[e.To] = dist[e.From] + e.Dis;
                        prev[e.To] = e.From;
                        if (!inQueue[e.To])
                        {
                            inQueue[e.To] = true;
                            Q.Enqueue(e.To);
                        }
                    }
                }
            }
        }
        
        private static Path CollectPath(int terminal)
        {
            Path res = new Path();
            int current = 0, curMin = 0x3fffffff;
            foreach (var kv in graphNodeDic[terminal])
                if (dist[kv.Value] < curMin)
                {
                    curMin = dist[kv.Value];
                    current = kv.Value;
                }
            while (current != -1)
            {
                res.Add(nodeDic[current]);
                current = prev[current];
            }
            res.Reverse();
            return res;
        }

    }
}
