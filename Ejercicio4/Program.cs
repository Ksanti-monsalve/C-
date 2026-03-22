using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

class CharFrecuencia
{
    public char Car   { get; init; }
    public int  Veces { get; init; }
}

class Program
{
    /// <summary>
    /// Normaliza un carácter: minúscula y sin tilde.
    /// Ej: 'Á' → 'a', 'É' → 'e', 'Ñ' → 'n'
    /// </summary>
    static char Normalizar(char c)
    {
        string s = c.ToString().ToLowerInvariant();

        // Descompone el carácter en base + diacrítico y queda solo con la base
        string decomposed = s.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (char ch in decomposed)
            if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                sb.Append(ch);

        return sb.ToString().Normalize(NormalizationForm.FormC)[0];
    }

    /// <summary>
    /// Recibe una cadena y devuelve la frecuencia de cada carácter
    /// alfanumérico, sin distinguir mayúsculas ni tildes, ordenada
    /// alfabéticamente. El texto original no se modifica.
    /// </summary>
    static List<CharFrecuencia> ContarFrecuencia(string texto)
    {
        var frecuencia = new Dictionary<char, int>();

        foreach (char c in texto)
        {
            if (!char.IsLetterOrDigit(c)) continue;

            char clave = char.IsDigit(c) ? c : Normalizar(c);

            if (frecuencia.ContainsKey(clave))
                frecuencia[clave]++;
            else
                frecuencia[clave] = 1;
        }

        return frecuencia
            .OrderBy(p => p.Key)
            .Select(p => new CharFrecuencia { Car = p.Key, Veces = p.Value })
            .ToList();
    }

    static void Mostrar(string etiqueta, string texto, List<CharFrecuencia> resultado)
    {
        Console.WriteLine($"  Entrada  : \"{texto}\"");
        Console.WriteLine($"  Original : \"{texto}\"  ← sin modificar\n");
        Console.WriteLine("  Resultado:");

        foreach (var item in resultado)
            Console.WriteLine($"    {{ Car = '{item.Car}', Veces = {item.Veces} }}");

        Console.WriteLine();
    }

    static void Main()
    {
        Console.WriteLine("══════════════════════════════════════════════════");
        Console.WriteLine("      Frecuencia de Caracteres en una Cadena      ");
        Console.WriteLine("══════════════════════════════════════════════════\n");

        // ── Casos de prueba ──────────────────────────────────────────────
        string[] casos =
        {
            "Hoy ya es día 10",
            "Murciélago",          // todas las vocales con y sin tilde
            "AaBbCc 123 AaBb",     // mayúsculas y repetidos
            "¡Hola, mundo! 2025"   // signos ignorados, números contados
        };

        int num = 1;
        foreach (string caso in casos)
        {
            Console.WriteLine($"── Caso {num++} ───────────────────────────────────────");
            Mostrar("", caso, ContarFrecuencia(caso));
        }

        // ── Prueba interactiva ───────────────────────────────────────────
        Console.WriteLine("══════════════════════════════════════════════════");
        Console.WriteLine("Ingresa tu propia cadena de texto:");
        Console.Write("> ");

        string? entrada = Console.ReadLine();

        if (!string.IsNullOrEmpty(entrada))
        {
            Console.WriteLine();
            Console.WriteLine("── Tu cadena ────────────────────────────────────");
            Mostrar("", entrada, ContarFrecuencia(entrada));
        }

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}