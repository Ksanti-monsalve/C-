using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    /// <summary>
    /// Recibe un arreglo de enteros positivos y retorna los valores que aparecen más de una vez.
    /// </summary>
    static int[] EncontrarRepetidos(int[] arreglo)
    {
        Dictionary<int, int> frecuencia = new Dictionary<int, int>();

        foreach (int numero in arreglo)
        {
            if (frecuencia.ContainsKey(numero))
                frecuencia[numero]++;
            else
                frecuencia[numero] = 1;
        }

        return frecuencia
            .Where(par => par.Value > 1)
            .Select(par => par.Key)
            .OrderBy(n => n)
            .ToArray();
    }

    static void Main()
    {
        // ── Casos de prueba ──────────────────────────────────────────────
        int[][] casos =
        {
            new[] { 10, 3, 5, 3, 10 },          // Ejemplo del enunciado
            new[] { 1, 2, 3, 4, 5 },             // Sin repetidos
            new[] { 7, 7, 7, 7 },                // Todos iguales
            new[] { 4, 1, 2, 1, 2, 3, 4, 5, 3 } // Múltiples repetidos
        };

        Console.WriteLine("══════════════════════════════════════════");
        Console.WriteLine("   Detector de Números Repetidos en C#    ");
        Console.WriteLine("══════════════════════════════════════════\n");

        foreach (int[] caso in casos)
        {
            int[] repetidos = EncontrarRepetidos(caso);

            Console.WriteLine($"  Entrada  : [{string.Join(", ", caso)}]");

            if (repetidos.Length == 0)
                Console.WriteLine("  Repetidos: (ninguno)");
            else
                Console.WriteLine($"  Repetidos: [{string.Join(", ", repetidos)}]");

            Console.WriteLine();
        }

        // ── Prueba interactiva ───────────────────────────────────────────
        Console.WriteLine("──────────────────────────────────────────");
        Console.WriteLine("Ingresa tu propio arreglo (números separados por comas):");
        Console.Write("> ");

        string? entrada = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(entrada))
        {
            try
            {
                int[] personalizado = entrada
                    .Split(',')
                    .Select(s => int.Parse(s.Trim()))
                    .ToArray();

                int[] resultado = EncontrarRepetidos(personalizado);

                Console.WriteLine($"\n  Entrada  : [{string.Join(", ", personalizado)}]");

                if (resultado.Length == 0)
                    Console.WriteLine("  Repetidos: (ninguno)");
                else
                    Console.WriteLine($"  Repetidos: [{string.Join(", ", resultado)}]");
            }
            catch
            {
                Console.WriteLine("Entrada inválida. Asegúrate de ingresar solo números enteros separados por comas.");
            }
        }

        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}