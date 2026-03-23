using System;
using System.Collections.Generic;

class Program
{
    /// <summary>
    /// Verifica que los signos de agrupamiento de la expresión estén
    /// correctamente emparejados y anidados.
    /// Retorna -1 si todo es correcto, o la posición del primer error.
    /// </summary>
    static int SimbEquilibrados(string expresion)
    {
        // Mapa: cada cierre conoce cuál es su apertura esperada
        var pares = new Dictionary<char, char>
        {
            { ')', '(' },
            { ']', '[' },
            { '}', '{' }
        };

        var aperturas = new HashSet<char> { '(', '[', '{' };
        var pila      = new Stack<(char simbolo, int posicion)>();

        for (int i = 0; i < expresion.Length; i++)
        {
            char c = expresion[i];

            if (aperturas.Contains(c))
            {
                // Apilamos el símbolo de apertura junto con su posición
                pila.Push((c, i));
            }
            else if (pares.ContainsKey(c))
            {
                // Es un símbolo de cierre
                if (pila.Count == 0)
                {
                    // Cierre sin apertura previa → error aquí
                    return i;
                }

                var (ultimo, _) = pila.Pop();

                if (ultimo != pares[c])
                {
                    // El cierre no coincide con la última apertura → error aquí
                    return i;
                }
            }
            // Cualquier otro carácter se ignora
        }

        // Si quedan aperturas sin cerrar → error en la posición de la primera
        if (pila.Count > 0)
            return pila.Peek().posicion;

        return -1; // Todo correcto
    }

    static void MostrarCaso(string expresion)
    {
        int resultado = SimbEquilibrados(expresion);

        Console.WriteLine($"  Expresión : \"{expresion}\"");

        if (resultado == -1)
        {
            Console.WriteLine("  Resultado : -1  ✓  (expresión correcta)");
        }
        else
        {
            Console.WriteLine($"  Resultado : {resultado}  ✗  (error en posición {resultado})");

            // Mostrar flecha indicando la posición del error
            string margen = new string(' ', resultado + 14); // 14 = longitud de '  Detalle : "'
            Console.WriteLine($"  Detalle   : \"{expresion}\"");
            Console.WriteLine($"               {new string(' ', resultado)}↑ posición {resultado} → '{expresion[resultado]}'");
        }

        Console.WriteLine();
    }

    static void Main()
    {
        Console.WriteLine("══════════════════════════════════════════════════════");
        Console.WriteLine("       Verificador de Signos de Agrupamiento          ");
        Console.WriteLine("══════════════════════════════════════════════════════\n");

        // ── Casos del enunciado ──────────────────────────────────────────
        Console.WriteLine("── Casos del enunciado ────────────────────────────────");
        MostrarCaso("[1+x+3*(y-5)]");
        MostrarCaso("[1+x)");
        MostrarCaso("}1+x");

        // ── Casos extra ──────────────────────────────────────────────────
        Console.WriteLine("── Casos extra ────────────────────────────────────────");
        MostrarCaso("({[]})");           // correctamente anidado
        MostrarCaso("(a+b]");            // cierre incorrecto
        MostrarCaso("((x+y)");           // apertura sin cierre
        MostrarCaso("{1+[2*(3+4)]}");    // correcto complejo
        MostrarCaso("");                 // cadena vacía → correcto

        // ── Prueba interactiva ───────────────────────────────────────────
        Console.WriteLine("══════════════════════════════════════════════════════");
        Console.WriteLine("Ingresa tu propia expresión:");
        Console.Write("> ");

        string? entrada = Console.ReadLine();

        if (entrada != null)
        {
            Console.WriteLine();
            Console.WriteLine("── Tu expresión ───────────────────────────────────────");
            MostrarCaso(entrada);
        }

        Console.WriteLine("Presiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}