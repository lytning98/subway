using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subway
{
    using JsonClass;
    using System.IO;

    /// <summary>
    ///  用于处理线路详情等简单查询的类
    /// </summary>
    static class LineQuery
    {
        /// <summary>
        /// 查询线路详情
        /// </summary>
        public static void LineStations()
        {
            Console.WriteLine("输入地铁线路名（如“4号线”“房山线”，不含引号）查询该线路所有站点，输入“exit”退出");
            Console.WriteLine("可选线路 : ");
            foreach (SubwayLine sl in Map.subwayMap.Lines)
                Console.WriteLine(" -> " + sl.Name);
            while (true)
            {

                Console.Write(">>>> ");
                string str = Console.ReadLine();
                if (!Map.LineId.ContainsKey(str))
                {
                    if (str == "exit")
                        return;
                    Console.WriteLine("线路名有误!");
                }
                else
                {
                    int lid = Map.LineId[str];
                    foreach (string name in Map.subwayMap.Lines[lid].Path)
                        Console.WriteLine(" - " + name);
                }
            }
        }

        /// <summary>
        /// 检查遍历路径答案
        /// </summary>
        /// <param name="path">答案文件路径/param>
        public static void TestAnswer(string path)
        {
            List<string> lines = null;
            try
            { 
                lines = File.ReadLines(path).ToList();
            }catch(Exception e)
            {
                Console.WriteLine("指定的文件 {0} 有误!", path);
            }
            lines.RemoveAt(0);
            int test;
            if (int.TryParse(lines[0], out test) == true)
                lines.RemoveAt(0);
            foreach(string line in lines)
                if(!Map.StationId.ContainsKey(line))
                {
                    Console.WriteLine("error");
                    Console.WriteLine("站点 {0} 有误或未收录", line);
                    return;
                }
            bool[] traveled = new bool[Map.StationCount];
            int current = Map.StationId[lines[0]];
            traveled[current] = true;
            for(int i = 1; i < lines.Count; i++)
            {
                int next = Map.StationId[lines[i]];
                traveled[next] = true;
                if(!Map.RouteSet.Contains(new Tuple<int, int>(current, next)))
                {
                    Console.WriteLine("error");
                    Console.WriteLine("路线有误：{0} -> {1}", lines[i - 1], lines[i]);
                    return;
                }
                current = next;
            }

            for(int i = 0; i < traveled.Length; i++)
                if(!traveled[i])
                {
                    Console.WriteLine("false");
                    Console.WriteLine("遗漏了站点：{0}", Map.subwayMap.Stations[i].Name);
                    return;
                }

            Console.WriteLine("true");
            return;
        }
    }
}
