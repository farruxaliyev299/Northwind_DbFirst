using Northwind_DbFirst.Helpers;

namespace Northwind_DbFirst
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (var tableD in DbHelper.GetTableNamesWithPropertyColumns("NorthwindDB"))
            {
                Directory.CreateDirectory(@$"C:\C#\Console\Northwind_DbFirst\Northwind_DbFirst\Models\");
                FileStream createModels = new(@$"C:\C#\Console\Northwind_DbFirst\Northwind_DbFirst\Models\{tableD.Key.Replace(" ", String.Empty)}.cs", FileMode.Create);
                createModels.Close();
                StreamWriter sw = new(@$"C:\C#\Console\Northwind_DbFirst\Northwind_DbFirst\Models\{tableD.Key.Replace(" ", String.Empty)}.cs");
                sw.WriteLine($"namespace DB_Experiments.Models;\n\npublic class {tableD.Key.Replace(" ", String.Empty)}\n" + "{");
                foreach (var prop in tableD.Value)
                {
                    sw.WriteLine($"\n   {prop}");
                }
                sw.WriteLine("\n}");
                sw.Close();
            }
        }
    }
}