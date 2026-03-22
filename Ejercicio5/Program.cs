using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    /// <summary>
    /// Determina recursivamente si existe algún subconjunto de 'numeros'
    /// (a partir del índice 'indice') cuyos elementos sumen exactamente 'objetivo'.
    /// </summary>
    static bool ExisteSuma(int[] numeros, int objetivo, int indice = 0)
    {
        // Caso base 1: la suma objetivo se alcanzó exactamente
        if (objetivo == 0) return true;

        // Caso base 2: no quedan elementos o la suma se pasó
        if (indice == numeros.Length || objetivo < 0) return false;

        // Rama 1: INCLUIR el elemento actual en el subconjunto
        bool incluyendo = ExisteSuma(numeros, objetivo - numeros[indice], indice + 1);

        // Rama 2: EXCLUIR el elemento actual del subconjunto
        bool excluyendo = ExisteSuma(numeros, objetivo, indice + 1);

        return incluyendo || excluyendo;
    }

    /// <summary>
    /// Encuentra y devuelve UN subconjunto que suma el objetivo (para mostrar en consola).
    /// Retorna null si no existe.
    /// </summary>
    static List<int>? EncontrarSubconjunto(int[] numeros, int objetivo, int indice = 0)
    {
        if (objetivo == 0) return new List<int>();
        if (indice == numeros.Length || objetivo < 0) return null;

        // Intentar incluyendo el elemento actual
        var conElemento = EncontrarSubconjunto(numeros, objetivo - numeros[indice], indice + 1);
        if (conElemento != null)
        {
            conElemento.Insert(0, numeros[indice]);
            return conElemento;
        }

        // Intentar excluyendo el elemento actual
        return EncontrarSubconjunto(numeros, objetivo, indice + 1);
    }

    static void EjecutarCaso(int[] numeros, int objetivo)
    {
        bool resultado  = ExisteSuma(numeros, objetivo);
        var  subconjunto = EncontrarSubconjunto(numeros, objetivo);

        string arreglo = $"[{string.Join(", ", numeros)}]";

        Console.WriteLine($"  ExisteSuma({arreglo}, {objetivo})");
        Console.WriteLine($"  Resultado : {(resultado ? "true ✓" : "false ✗")}");

        if (resultado && subconjunto != null)
        {
            string sub  = $"[{string.Join(", ", subconjunto)}]";
            string suma = string.Join(" + ", subconjunto);
            Console.WriteLine($"  Ejemplo   : {sub}  →  {suma} = {objetivo}");
        }

        Console.WriteLine();
    }

    static void Main()
    {
        Console.WriteLine("══════════════════════════════════════════════════════");
        Console.WriteLine("    Verificar Subconjunto con Suma Dada (Recursivo)   ");
        Console.WriteLine("══════════════════════════════════════════════════════\n");

        // ── Casos del enunciado ──────────────────────────────────────────
        Console.WriteLine("── Casos del enunciado ────────────────────────────────");
        EjecutarCaso(new[] { 3, 4, 2, 8, 7 }, 6);
        EjecutarCaso(new[] { 3, 4, 2, 8, 7 }, 26);
        EjecutarCaso(new[] { 4 },              4);

        // ── Casos extra ──────────────────────────────────────────────────
        Console.WriteLine("── Casos extra ────────────────────────────────────────");
        EjecutarCaso(new[] { 1, 2, 3, 4, 5 }, 9);   // 4+5 o 2+3+4
        EjecutarCaso(new[] { 10, 20, 30 },    15);   // imposible
        EjecutarCaso(new[] { 5, 5, 5, 5 },   10);   // 5+5

        // ── Prueba interactiva ───────────────────────────────────────────
        Console.WriteLine("══════════════════════════════════════════════════════");
        Console.WriteLine("Prueba interactiva");
        Console.WriteLine("──────────────────────────────────────────────────────");

        Console.Write("Ingresa los números separados por comas: ");
        string? entradaNumeros = Console.ReadLine();

        Console.Write("Ingresa la suma objetivo : ");
        string? entradaObjetivo = Console.ReadLine();

        try
        {
            int[] numeros = entradaNumeros!
                .Split(',')
                .Select(s => int.Parse(s.Trim()))
                .ToArray();

            int objetivo = int.Parse(entradaObjetivo!.Trim());

            Console.WriteLine();
            EjecutarCaso(numeros, objetivo);
        }
        catch
        {
            Console.WriteLine("\nEntrada inválida. Asegúrate de ingresar números enteros correctamente.");
        }

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}