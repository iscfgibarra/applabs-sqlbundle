using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SqlBundle
{
    class Program
    {
        static void Main(string[] args)
        {
            const string nombreArchivoSalida = "SqlBundled.sql";
            const string principal = "PRINCIPAL.sql";
            string rutaEjecucion =
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            rutaEjecucion = rutaEjecucion.Replace("file:\\", "");

            Console.WriteLine("Proporciona el nombre de la base de datos:");
            var nombreBd = Console.ReadLine();

            if (string.IsNullOrEmpty(nombreBd))
            {
                Console.WriteLine("Por favor proporcione el nombre de la base de datos: SqlBundle.exe NombreBd");
                Console.Read();
                return;
            }

            var pathBundle = $"{rutaEjecucion}\\{nombreArchivoSalida}";
            var pathPrincipal = $"{rutaEjecucion}\\{principal}";


            var dir = new System.IO.DirectoryInfo(rutaEjecucion);
            var files = dir.GetFiles();
            string sqlbundle = string.Empty;
            string sqlprincipal =
                $"USE [{nombreBd}]\nGO\nDECLARE @RFC NVARCHAR(13) = 'CÑT040504890',\n@UserId BIGINT = 1,\n@Limit INT = 1000\n";
            string sqldrop = string.Empty;


            foreach (var fileInfo in files)
            {
                if (fileInfo.Extension == ".sql" 
                    && !fileInfo.Name.Contains(principal)
                    && !fileInfo.Name.Contains(nombreArchivoSalida))
                {
                    Console.WriteLine($"Leyendo: {fileInfo.Name}");
                    sqlbundle += File.ReadAllText(fileInfo.FullName, Encoding.UTF8) + "\n\n ------------------------------------------- \n";
                    sqlprincipal += $"EXEC {fileInfo.Name.Replace(".sql", "")} @Limit, @Rfc, @UserId \n";
                }                
            }

            sqlbundle = sqlbundle.Replace("DBFACTSYSTEM_PRUEBA322", nombreBd);
            sqlbundle = sqlbundle.Replace("DBFACTSYSTEM_PRUEBA", nombreBd);
            
            
            File.WriteAllText(pathBundle, sqlbundle, Encoding.UTF8);
            File.WriteAllText(pathPrincipal, sqlprincipal, Encoding.UTF8);


            Console.WriteLine($"Se ha terminado de escribir el conjunto de scripts en {nombreArchivoSalida} y {principal}");
            Console.Read();
        }
    }
}
