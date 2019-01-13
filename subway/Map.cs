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
        public static bool InitMap(string path)
        {
            try { 
                string json = File.ReadAllText(path);
                Subway subway = JsonConvert.DeserializeObject<Subway>(json);
            }
            catch(Exception e)
            {
                Console.WriteLine("地铁数据读取异常, 请确认数据文件 [{0}] 存在且完整!", path);
                return false;
            }
            Console.WriteLine("load success");
            return true;
        }
    }
}
