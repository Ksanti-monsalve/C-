using System;
using System.Linq;
using System.Text;

class Program
{
    const string Mayusculas  = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const string Minusculas  = "abcdefghijklmnopqrstuvwxyz";
    const string Digitos     = "0123456789";
    const string Especiales  = "!@#$%&*?+=-/";

    static string GenerarContrasena(Random rng)
    {
        // Longitud aleatoria entre 15 y 87
        int longitud = rng.Next(15, 88);

        // Pool completo de caracteres
        string pool = Mayusculas + Minusculas + Digitos + Especiales;

        // Garantizar al menos un carácter de cada categoría obligatoria
        char[] contrasena = new char[longitud];
        contrasena[0] = Mayusculas[rng.Next(Mayusculas.Length)];
        contrasena[1] = Minusculas[rng.Next(Minusculas.Length)];
        contrasena[2] = Digitos   [rng.Next(Digitos.Length)];
        contrasena[3] = Especiales[rng.Next(Especiales.Length)];

        // Rellenar el resto con caracteres aleatorios del pool
        for (int i = 4; i < longitud; i++)
            contrasena[i] = pool[rng.Next(pool.Length)];

        // Mezclar para que los obligatorios no queden siempre al inicio
        for (int i = longitud - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (contrasena[i], contrasena[j]) = (contrasena[j], contrasena[i]);
        }

        return new string(contrasena);
    }

    static void MostrarValidacion(string pwd)
    {
        bool tieneMayus    = pwd.Any(c => Mayusculas.Contains(c));
        bool tieneMinus    = pwd.Any(c => Minusculas.Contains(c));
        bool tieneDigito   = pwd.Any(c => Digitos.Contains(c));
        bool tieneEspecial = pwd.Any(c => Especiales.Contains(c));
        bool longitudOk    = pwd.Length >= 15 && pwd.Length <= 87;

        Console.WriteLine("\n  Validación:");
        Console.WriteLine($"    {"Longitud (15-87)",-22}: {pwd.Length,2} caracteres  {(longitudOk    ? "✓" : "✗")}");
        Console.WriteLine($"    {"Mayúscula",-22}: {(tieneMayus    ? "✓" : "✗")}");
        Console.WriteLine($"    {"Minúscula",-22}: {(tieneMinus    ? "✓" : "✗")}");
        Console.WriteLine($"    {"Dígito",-22}: {(tieneDigito   ? "✓" : "✗")}");
        Console.WriteLine($"    {"Carácter especial",-22}: {(tieneEspecial ? "✓" : "✗")}");
    }

    static void Main()
    {
        Random rng = new Random();

        Console.WriteLine("══════════════════════════════════════════════");
        Console.WriteLine("       Generador de Contraseñas Seguras       ");
        Console.WriteLine("══════════════════════════════════════════════\n");

        bool continuar = true;

        while (continuar)
        {
            string pwd = GenerarContrasena(rng);

            Console.WriteLine("  Contraseña generada:");
            Console.WriteLine($"\n    {pwd}\n");

            MostrarValidacion(pwd);

            Console.WriteLine("\n──────────────────────────────────────────────");
            Console.Write("  ¿Generar otra contraseña? (s/n): ");
            string respuesta = Console.ReadLine()?.Trim().ToLower() ?? "n";
            continuar = respuesta == "s";
            Console.WriteLine();
        }

        Console.WriteLine("  ¡Hasta luego! Recuerda guardar tu contraseña en un lugar seguro.");
        Console.WriteLine("\nPresiona cualquier tecla para salir...");
        Console.ReadKey();
    }
}