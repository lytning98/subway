using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace subway
{
    using JsonClass;

    static class Map
    {
        public static Subway subwayMap;
        public static Dictionary<string, int> StationId { get; }
        public static Dictionary<string, int> LineId { get; }
        public static List<List<int>> LineStations { get; }
        public static List<List<int>> StationLines { get; }
        public static HashSet<Tuple<int, int>> RouteSet { get; }
        
        static Map()
        {
            StationId = new Dictionary<string, int>();
            LineId = new Dictionary<string, int>();
            LineStations = new List<List<int>>();
            StationLines = new List<List<int>>();
            RouteSet = new HashSet<Tuple<int, int>>();
        }

        public static string MapName {
            get{
                return subwayMap.Title; 
            }
        }
        public static int StationCount {
            get {
                return subwayMap.Stations.Count;
            }
        }
        public static int LineCount
        {
            get{
                return subwayMap.Lines.Count;
            }
        }

        private static bool LoadJson(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                subwayMap = JsonConvert.DeserializeObject<Subway>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine("地铁数据读取异常, 请确认数据文件 [{0}] 存在且完整!", path);
                return false;
            }
            return true;
        }

        private static void CollectStations()
        {
            int id = 0;
            foreach (SubwayStation ss in subwayMap.Stations)
            {
                StationId[ss.Name] = id++;
                StationLines.Add(new List<int>());
            }
        }

        private static void CollectLines()
        {
            int lid = 0;
            foreach (SubwayLine sl in subwayMap.Lines)
            {
                LineStations.Add(new List<int>());
                LineId[sl.Name] = lid;
                int lastStationId = -1;
                foreach (string stationName in sl.Path)
                {
                    int sid = StationId[stationName];
                    LineStations[lid].Add(sid);
                    if(!StationLines[sid].Contains(lid))
                        StationLines[sid].Add(lid);

                    if (lastStationId != -1)
                    {
                        RouteSet.Add(new Tuple<int, int>(lastStationId, sid));
                        RouteSet.Add(new Tuple<int, int>(sid, lastStationId));
                    }
                    lastStationId = sid;
                }
                lid++;
            }
        }

        public static bool InitMap(string path)
        {
            if (!LoadJson(path))
                return false;
            CollectStations();
            CollectLines();
            Console.WriteLine("已加载 " + subwayMap.Title);
            return true;
        }
    }
}
