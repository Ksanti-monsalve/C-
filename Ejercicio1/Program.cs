using System;
using System.Collections.Generic;
using System.Linq;

namespace Ejercicio1
{
    class Program
    {
        private static readonly List<string> tableRows = new()
        {
            "Juan Perez, 25 años, Mexico",
            "Maria Lopez, 28 años, Argentina",
            "Julia Gomez, 30 años, España",
            "Carlos Ruiz, 35 años, Colombia",
            "Ana Martin, 22 años, Peru",
            "Pedro Sanchez, 40 años, Chile"
        };

        static void FilterTable(List<string> rows, string searchText)
        {
            Console.WriteLine("\n=== RESULTADOS DEL FILTRO ===");

            IEnumerable<string> filtered;
            if (string.IsNullOrWhiteSpace(searchText))
            {
                filtered = rows;
            }
            else
            {
                filtered = rows.Where(row => row.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!filtered.Any())
            {
                Console.WriteLine("No se encontraron coincidencias.");
            }
            else
            {
                foreach (var row in filtered)
                {
                    Console.WriteLine($" - {row}");
                }
            }
            Console.WriteLine();
        }

        static void Main()
        {
            Console.WriteLine("=== FILTRO DE TABLA EN CONSOLA ===");
            Console.WriteLine("Ingrese texto para filtrar (vacío para mostrar todo, 'exit' para salir):\n");

            // Mostrar todas las filas inicialmente
            Console.WriteLine("TABLA COMPLETA:");
            for (int i = 0; i < tableRows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tableRows[i]}");
            }
            Console.WriteLine();

            while (true)
            {
                Console.Write("Texto de búsqueda: ");
                string? searchText = Console.ReadLine();

                if (string.Equals(searchText, "exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\n¡Hasta luego!");
                    break;
                }

                Console.Clear();
                Console.WriteLine("=== FILTRO DE TABLA EN CONSOLA ===");
                Console.WriteLine("Ingrese texto para filtrar (vacío para mostrar todo, 'exit' para salir):\n");

                // Reimprimir tabla completa
                Console.WriteLine("TABLA COMPLETA:");
                for (int i = 0; i < tableRows.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {tableRows[i]}");
                }

                FilterTable(tableRows, searchText ?? string.Empty);
            }
        }
    }
}