using System;
using System.Linq;
using BreakLineEvents.Services;

namespace BreakLineEvents.Reports
{
    /// <summary>
    /// Genera el reporte final en consola a partir de los resultados de la conciliación.
    /// </summary>
    public static class ReporteService
    {
        public static void MostrarReporte(ConciliacionService svc)
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         BREAKLINE EVENTS — REPORTE FINAL                ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");

            // ── Resumen numérico ─────────────────────────────────────────
            Console.WriteLine();
            Console.WriteLine("┌─── RESUMEN DE FUENTES ─────────────────────────────────┐");
            Console.WriteLine($"  Preregistrados   : {svc.Preregistrados.Count}");
            Console.WriteLine($"  Registro manual  : {svc.RegistroManual.Count}");
            Console.WriteLine($"  Invitados VIP    : {svc.InvitadosVip.Count}");
            Console.WriteLine($"  Lista negra      : {svc.ListaNegra.Count}");
            Console.WriteLine($"  Autorizados (∪-✗): {svc.Autorizados.Count}");
            Console.WriteLine($"  Asistentes reales: {svc.AsistentesReales.Count}");
            Console.WriteLine($"  Inscripciones OK : {svc.Inscripciones.Count}");

            // ── Operaciones de conjuntos ─────────────────────────────────
            Console.WriteLine();
            Console.WriteLine("┌─── OPERACIONES DE CONJUNTOS ───────────────────────────┐");
            Console.WriteLine($"  ¿VIPs ⊆ (autorizados ∪ lista negra)? : {(svc.VipsSonSubconjuntoDeAutorizados() ? "Sí ✓" : "No ✗")}");
            Console.WriteLine($"  ¿Asistentes ∩ Autorizados ≠ ∅?       : {(svc.HaySuperpocisionEntreAsistentesYAutorizados() ? "Sí ✓" : "No ✗")}");

            // ── Asistentes NO autorizados ────────────────────────────────
            Console.WriteLine();
            Console.WriteLine("┌─── ASISTENTES NO AUTORIZADOS ──────────────────────────┐");
            if (!svc.NoAutorizados.Any())
            {
                Console.WriteLine("  (ninguno)");
            }
            else
            {
                foreach (var p in svc.NoAutorizados.OrderBy(p => p.NombreCompleto))
                    Console.WriteLine($"  - {p}");
            }

            // ── Autorizados ausentes ─────────────────────────────────────
            Console.WriteLine();
            Console.WriteLine("┌─── AUTORIZADOS QUE NO ASISTIERON ──────────────────────┐");
            if (!svc.Ausentes.Any())
            {
                Console.WriteLine("  (ninguno)");
            }
            else
            {
                foreach (var p in svc.Ausentes.OrderBy(p => p.NombreCompleto))
                    Console.WriteLine($"  - {p}");
            }

            // ── Talleres llenos ──────────────────────────────────────────
            Console.WriteLine();
            Console.WriteLine("┌─── TALLERES LLENOS ────────────────────────────────────┐");
            var llenos = svc.TalleresLlenos().ToList();
            if (!llenos.Any())
            {
                Console.WriteLine("  (ninguno)");
            }
            else
            {
                foreach (var t in llenos)
                    Console.WriteLine($"  - {t}");
            }

            // ── Inscripciones rechazadas ─────────────────────────────────
            Console.WriteLine();
            Console.WriteLine("┌─── INSCRIPCIONES RECHAZADAS ───────────────────────────┐");
            if (!svc.InscripcionesRechazadas.Any())
            {
                Console.WriteLine("  (ninguna)");
            }
            else
            {
                foreach (var r in svc.InscripcionesRechazadas)
                    Console.WriteLine(r);
            }

            // ── Duplicados detectados ────────────────────────────────────
            Console.WriteLine();
            Console.WriteLine("┌─── DUPLICADOS DETECTADOS EN CARGA ─────────────────────┐");
            if (!svc.Duplicados.Any())
            {
                Console.WriteLine("  (ninguno)");
            }
            else
            {
                foreach (var d in svc.Duplicados)
                    Console.WriteLine(d);
            }

            // ── Inscripciones válidas ────────────────────────────────────
            Console.WriteLine();
            Console.WriteLine("┌─── INSCRIPCIONES VÁLIDAS ──────────────────────────────┐");
            foreach (var ins in svc.Inscripciones
                         .OrderBy(i => i.Taller.HoraInicio)
                         .ThenBy(i => i.Participante.NombreCompleto))
            {
                Console.WriteLine($"  ✓ {ins.Participante.NombreCompleto,-25} → {ins.Taller.Nombre} ({ins.Taller.HoraInicio:HH:mm}–{ins.Taller.HoraFin:HH:mm})");
            }

            Console.WriteLine();
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
        }
    }
}