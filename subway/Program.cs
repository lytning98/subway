using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace subway
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (!Map.InitMap("subway_map.json"))
                return;

            if(args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            Console.WriteLine("started in console mode");
        }
    }
}
