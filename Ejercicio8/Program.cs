using System;
using System.Linq;

class Program
{
    /// <summary>
    /// Determina si dos palabras son anagramas entre sí.
    /// - Ignora espacios y mayúsculas.
    /// - Dos palabras idénticas NO son anagramas.
    /// </summary>
    static bool EsAnagrama(string palabra1, string palabra2)
    {
        // Normalizar: minúsculas y sin espacios
        string p1 = palabra1.ToLowerInvariant().Replace(" ", "");
        string p2 = palabra2.ToLowerInvariant().Replace(" ", "");

        // Palabras idénticas no son anagramas
        if (p1 == p2) return false;

        // Deben tener la misma longitud
        if (p1.Length != p2.Length) return false;

        // Ordenar letras y comparar → si son iguales, son anagramas
        return string.Concat(p1.OrderBy(c => c))
            == string.Concat(p2.OrderBy(c => c));
    }

    static void MostrarCaso(string p1, string p2)
    {
        bool resultado = EsAnagrama(p1, p2);

        string sorted1 = string.Concat(p1.ToLowerInvariant().Replace(" ", "").OrderBy(c => c));
        string sorted2 = string.Concat(p2.ToLowerInvariant().Replace(" ", "").OrderBy(c => c));

        Console.WriteLine($"  EsAnagrama(\"{p1}\", \"{p2}\")");
        Console.WriteLine($"  Ordenadas : \"{sorted1}\" vs \"{sorted2}\"");
        Console.WriteLine($"  Resultado : {(resultado ? "true  ✓ Son anagramas" : "false ✗ No son anagramas")}");
        Console.WriteLine();
    }

    static void Main()
    {
        Console.WriteLine("══════════════════════════════════════════════");
        Console.WriteLine("         Verificador de Anagramas             ");
        Console.WriteLine("══════════════════════════════════════════════\n");

        // ── Casos del enunciado ──────────────────────────────────────────
        Console.WriteLine("── Casos del enunciado ────────────────────────────");
        MostrarCaso("amor", "roma");
        MostrarCaso("rota", "otra");
        MostrarCaso("otra", "otra");

        // ── Casos extra ──────────────────────────────────────────────────
        Console.WriteLine("── Casos extra ─────────────────────────────────────");
        MostrarCaso("listen",  "silent");   // clásico en inglés
        MostrarCaso("res",     "ser");      // español corto
        MostrarCaso("lado",    "dola");     // con letras distintas en orden
        MostrarCaso("hola",    "mundo");    // diferente longitud
        MostrarCaso("el amor", "la rome");  // con espacios

        // ── Prueba interactiva ───────────────────────────────────────────
        Console.WriteLine("══════════════════════════════════════════════");
        Console.WriteLine("Prueba interactiva");
        Console.WriteLine("──────────────────────────────────────────────");

        Console.Write("Primera palabra : ");
        string? w1 = Console.ReadLine();

        Console.Write("Segunda palabra : ");
        string? w2 = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(w1) && !string.IsNullOrWhiteSpace(w2))
        {
            Console.WriteLine();
            Console.WriteLine("── Tu resultado ───────────────────────────────────");
            MostrarCaso(w1, w2);
        }

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}