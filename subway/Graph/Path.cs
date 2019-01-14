using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace subway.Graph
{
    /// <summary>
    /// 用于记录行程路径的类, 基于泛型 List, Tuple item 分别为站名、路线名
    /// </summary>
    class Path : List<Tuple<int, int>>
    {
        /// <summary>
        /// 根据地铁站名生成行程描述字符串
        /// </summary>
        /// <returns>描述字符串</returns>
        public string ToPathString(bool transition = true)
        {
            string res = this.Count.ToString();
            int lastStationId = -1;
            foreach (Tuple<int, int> t in this)
            {
                if (t.Item1 == lastStationId)
                {
                    if(transition)
                        res += " 换乘" + Map.subwayMap.Lines[t.Item2].Name;
                }
                else
                {
                    res += "\n" + Map.subwayMap.Stations[t.Item1].Name;
                }
                lastStationId = t.Item1;
            }
            return res;
        }

        public override string ToString()
        {
            return ToPathString();
        }

        /// <summary>
        /// 合并另一条路径至当前路径
        /// </summary>
        /// <param name="another">另一条路径</param>
        public void Merge(Path another)
        {
            foreach (var node in another)
                this.Add(node);
        }

        /// <summary>
        /// 去除重复站点
        /// </summary>
        public void Trim()
        {
            for (int i = 0; i < this.Count - 1; i++)
                if (this[i] == this[i + 1])
                    this.RemoveAt(i);
        }
    }
}
