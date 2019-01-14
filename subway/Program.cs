using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace subway
{

    using Graph;

    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            if (!Map.InitMap("subway_map.json"))
                return;
            GraphSolver.BuildGraph();
            if (args.Length == 3 && args[0] == "/b")
            {
                if(!Map.StationId.ContainsKey(args[1]) ||
                    !Map.StationId.ContainsKey(args[2]))
                {
                    Console.WriteLine("站名有误或数据文件未收录此站！");
                    return;
                }
                int id1 = Map.StationId[args[1]], id2 = Map.StationId[args[2]];
                var res = GraphSolver.CalcPath(id1, id2);
                Console.WriteLine(res.ToString());
                return;
            }
            if(args.Length == 2 && args[0] == "/a")
            {
                if(!Map.StationId.ContainsKey(args[1]))
                {
                    Console.WriteLine("站名有误或数据文件未收录此站！");
                    return;
                }
                int id = Map.StationId[args[1]];
                var res = GraphSolver.TravelAroundPath(id);
                Console.WriteLine(res.ToPathString(false));
                return;
            }
            if(args.Length == 2 && args[0] == "/z")
            {
                LineQuery.TestAnswer(args[1]);
                return;
            }
            if(args.Length == 1 && args[0] == "/g")
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new GUIForm());
                return;
            }
            if(args.Length == 0)
            {
                Console.WriteLine("当前以无参数方式启动.");
                LineQuery.LineStations();
                return;
            }
            Console.WriteLine("参数有误, 请仔细检查.");
            Console.WriteLine(" - 路径查询: subway.exe /b 起始站 终到站"); ;
            Console.WriteLine(" - 线路查询: subway.exe");
            Console.WriteLine(" - 环游路径: subway.exe /a 起始站");
            Console.WriteLine(" - 检测答案: subway.exe /z 答案txt路径");
            Console.WriteLine(" - 启动GUI: subway.exe /g");
        }
    }
}
