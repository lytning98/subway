using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subway.Graph
{
    /// <summary>
    /// GraphSolver 类用于在由地铁地图建立的图论模型上求解最短路等图论问题
    /// </summary>
    static partial class GraphSolver
    {
        const int INF = 0x3fffffff;
        static private List<List<Edge>> adjList;
        static private List<Dictionary<int, int>> graphNodeDic;
        static private List<Tuple<int, int>> nodeDic;
        static private int nodeCount;

        static GraphSolver()
        {
            adjList = new List<List<Edge>>();
            graphNodeDic = new List<Dictionary<int, int>>();
            nodeDic = new List<Tuple<int, int>>();
        }

        private static void AddEdge(int from, int to, int dis)
        {
            Edge e = new Edge(from, to, dis);
            adjList[from].Add(e);
            e = new Edge(to, from, dis);
            adjList[to].Add(e);
        }

        private static void AllocateNodeId()
        {
            int tot = 0;
            for (int i = 0; i < Map.StationCount; i++)
            {
                graphNodeDic.Add(new Dictionary<int, int>());
                foreach (int lineId in Map.StationLines[i])
                {
                    nodeDic.Add(new Tuple<int, int>(i, lineId));
                    graphNodeDic[i].Add(lineId, tot++);
                }
            }
            for(int i = 0; i < tot; i++)
                adjList.Add(new List<Edge>());
            nodeCount = tot;
        }

        private static void BuildEdges()
        {
            for(int i = 0; i < Map.StationCount; i++)
            { 
                foreach(int lid1 in Map.StationLines[i])
                    foreach(int lid2 in Map.StationLines[i])
                        if(lid1 < lid2)
                            AddEdge(graphNodeDic[i][lid1], graphNodeDic[i][lid2], 3);
            }
            for (int i = 0; i < Map.LineCount; i++)
            {
                for (int j = 1; j < Map.LineStations[i].Count; j++)
                {
                    AddEdge(graphNodeDic[Map.LineStations[i][j-1]][i],
                        graphNodeDic[Map.LineStations[i][j]][i], 1);
                }
            }
        }

        public static void BuildGraph()
        {
            AllocateNodeId();
            BuildEdges();
        }

        /// <summary>
        /// 计算两点间最短路径
        /// </summary>
        /// <param name="from">起点</param>
        /// <param name="to">重点</param>
        /// <returns>返回最短路径</returns>
        public static Path CalcPath(int from, int to)
        {
            InitSPFA();
            SPFA(from);
            return CollectPath(to);
        }

        /// <summary>
        /// 计算遍历路径可行解
        /// </summary>
        /// <param name="from">起点</param>
        /// <returns>遍历路径</returns>
        public static Path TravelAroundPath(int from)
        {
            int[] Depatures = new int[Map.LineCount];
            int[] Terminals = new int[Map.LineCount];
            bool[] Traveled = new bool[Map.LineCount];

            Path res = new Path();
            int current = from;
            for (int lid = 0; lid < Map.LineCount; lid++)
            {
                int lineDepature = Map.LineStations[lid][0];
                int lineTerminal = Map.LineStations[lid][Map.LineStations[lid].Count - 1];
                if (lineTerminal == lineDepature)
                    lineTerminal = Map.LineStations[lid][Map.LineStations[lid].Count - 2];
                Depatures[lid] = lineDepature;
                Terminals[lid] = lineTerminal;
                Traveled[lid] = false;
            }
            for(int iter = 0; iter < Map.LineCount; iter++)
            {
                int minStep = INF, minId = 0, minInvId = 0, minLine = 0;
                Path through = null;
                for(int lid = 0; lid < Map.LineCount; lid++)
                {
                    if (Traveled[lid]) continue;
                    Path move = CalcPath(current, Depatures[lid]);
                    if(move.Count < minStep)
                    {
                        minStep = move.Count;
                        minId = Depatures[lid];
                        minInvId = Terminals[lid];
                        minLine = lid;
                        through = new Path();
                        for (int i = 0; i < Map.LineStations[lid].Count; i++)
                            through.Add(new Tuple<int, int>(Map.LineStations[lid][i], lid));
                    }
                    move = CalcPath(current, Terminals[lid]);
                    if(move.Count < minStep)
                    {
                        minStep = move.Count;
                        minId = Terminals[lid];
                        minInvId = Depatures[lid];
                        minLine = lid;
                        through = new Path();
                        for (int i = Map.LineStations[lid].Count - 1; i >= 0; i--)
                            through.Add(new Tuple<int, int>(Map.LineStations[lid][i], lid));
                    }
                }
                //Console.WriteLine("from {0} to {1} to {2} for {3}", Map.subwayMap.Stations[current].Name,
                    //Map.subwayMap.Stations[minId].Name, Map.subwayMap.Stations[minInvId].Name, Map.subwayMap.Lines[minLine].Name);
                Traveled[minLine] = true;
                res.Merge(CalcPath(current, minId));
                res.Merge(through);
                current = minInvId;
            }
            res.Trim();
            return res;
        }
    }
}
