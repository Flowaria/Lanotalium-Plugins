using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConfig
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Console.Write("huh ");
            Console.Read();

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
