namespace ReportMiniApp
{
    using System;
    using System.Reflection;

    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine();
                Console.WriteLine("\tUsage:");
                Console.WriteLine($"\t{AppDomain.CurrentDomain.FriendlyName}.exe <ReportFileName>");
                Console.WriteLine();
                Console.WriteLine("\tE.g.:");
                Console.WriteLine($"\t{AppDomain.CurrentDomain.FriendlyName}.exe MyReport.xml");
                Console.WriteLine();
                return;
            }

            ReportDefinitionLoader loader = new ReportDefinitionLoader();

            Console.WriteLine();
            Console.WriteLine("REPORT:");
            Console.WriteLine();

            foreach (IReportWidget widget in loader.LoadFromXml(args[0]))
            {
                widget.Render();
            }

            Console.WriteLine();
        }
    }
}
