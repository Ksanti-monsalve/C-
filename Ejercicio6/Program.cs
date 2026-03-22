using System;
using System.Collections.Generic;

class Program
{
    /// <summary>
    /// Genera todas las permutaciones posibles de la cadena recibida.
    /// Usa backtracking: fija un carácter al inicio y permuta el resto recursivamente.
    /// </summary>
    static List<string> Permutaciones(string cadena)
    {
        var resultado = new List<string>();
        Permutar(cadena.ToCharArray(), 0, resultado);
        return resultado;
    }

    static void Permutar(char[] chars, int inicio, List<string> resultado)
    {
        // Caso base: todos los caracteres están fijados → permutación completa
        if (inicio == chars.Length)
        {
            resultado.Add(new string(chars));
            return;
        }

        for (int i = inicio; i < chars.Length; i++)
        {
            // Colocar chars[i] en la posición 'inicio'
            Intercambiar(chars, inicio, i);

            // Permutar el resto de la cadena (desde inicio+1)
            Permutar(chars, inicio + 1, resultado);

            // Deshacer el intercambio (backtracking)
            Intercambiar(chars, inicio, i);
        }
    }

    static void Intercambiar(char[] chars, int i, int j)
    {
        (chars[i], chars[j]) = (chars[j], chars[i]);
    }

    static void MostrarResultado(string cadena, List<string> permutaciones)
    {
        long factorial = 1;
        for (int i = 1; i <= cadena.Length; i++) factorial *= i;

        Console.WriteLine($"  Permutaciones(\"{cadena}\")");
        Console.WriteLine($"  Caracteres : {cadena.Length}  →  {cadena.Length}! = {factorial} permutaciones\n");

        int num = 1;
        foreach (string p in permutaciones)
            Console.WriteLine($"    {num++,3}. {p}");

        Console.WriteLine();
    }

    static void Main()
    {
        Console.WriteLine("══════════════════════════════════════════════");
        Console.WriteLine("       Generador de Permutaciones             ");
        Console.WriteLine("══════════════════════════════════════════════\n");

        // ── Casos de prueba ──────────────────────────────────────────────
        string[] casos = { "abc", "ab", "d", "1234" };

        foreach (string caso in casos)
        {
            Console.WriteLine("──────────────────────────────────────────────");
            MostrarResultado(caso, Permutaciones(caso));
        }

        // ── Prueba interactiva ───────────────────────────────────────────
        Console.WriteLine("══════════════════════════════════════════════");
        Console.WriteLine("Ingresa tu propia cadena (máx. 8 caracteres recomendado):");
        Console.Write("> ");

        string? entrada = Console.ReadLine()?.Trim();

        if (!string.IsNullOrEmpty(entrada))
        {
            if (entrada.Length > 8)
            {
                Console.WriteLine($"\n  ⚠ Con {entrada.Length} caracteres se generarían {Factorial(entrada.Length):N0} permutaciones.");
                Console.Write("  ¿Deseas continuar de todas formas? (s/n): ");
                if (Console.ReadLine()?.Trim().ToLower() != "s")
                {
                    Console.WriteLine("\nOperación cancelada.");
                    Console.WriteLine("Presiona cualquier tecla para salir...");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine();
            Console.WriteLine("──────────────────────────────────────────────");
            MostrarResultado(entrada, Permutaciones(entrada));
        }

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }

    static long Factorial(int n)
    {
        long f = 1;
        for (int i = 1; i <= n; i++) f *= i;
        return f;
    }
}