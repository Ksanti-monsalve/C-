using System;
using BreakLineEvents.Services;
using BreakLineEvents.Reports;

namespace BreakLineEvents
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("══════════════════════════════════════════════════════════");
            Console.WriteLine("  BreakLine Events — Sistema de Conciliación de Asistentes");
            Console.WriteLine("══════════════════════════════════════════════════════════");

            // 1. Cargar datos de prueba
            Console.WriteLine("\n[1/3] Cargando datos...");
            var talleres = CargaDatos.CrearTalleres();
            var (preregistrados, registroManual, invitadosVip,
                 listaNegra, asistentesReales, duplicados) = CargaDatos.CargarParticipantes();

            // 2. Procesar conciliación con operaciones de conjuntos
            Console.WriteLine("[2/3] Procesando conciliación (HashSet + operaciones de conjuntos)...");
            var servicio = new ConciliacionService(
                preregistrados, registroManual, invitadosVip,
                listaNegra, asistentesReales, duplicados, talleres);

            servicio.ProcesarConciliacion();

            // 3. Mostrar reporte final
            Console.WriteLine("[3/3] Generando reporte...");
            ReporteService.MostrarReporte(servicio);

            Console.WriteLine("\nPresiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
