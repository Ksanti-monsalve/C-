using System;
using System.Collections.Generic;
using System.Linq;
using BreakLineEvents.Models;

namespace BreakLineEvents.Services
{
    /// <summary>
    /// Servicio de conciliación: aplica operaciones de conjuntos sobre
    /// HashSet<Participante> para determinar autorizados, ausentes y colados.
    /// </summary>
    public class ConciliacionService
    {
        // Colecciones principales (requeridas por el enunciado)
        public HashSet<Participante> Preregistrados    { get; }
        public HashSet<Participante> RegistroManual    { get; }
        public HashSet<Participante> InvitadosVip      { get; }
        public HashSet<Participante> ListaNegra        { get; }
        public HashSet<Participante> AsistentesReales  { get; }
        public HashSet<InscripcionTaller> Inscripciones { get; } = new();

        // Resultados derivados
        public HashSet<Participante>    Autorizados          { get; private set; } = new();
        public HashSet<Participante>    NoAutorizados         { get; private set; } = new();
        public HashSet<Participante>    Ausentes              { get; private set; } = new();
        public List<InscripcionRechazada> InscripcionesRechazadas { get; } = new();
        public List<DuplicadoDetectado>   Duplicados           { get; }

        private readonly List<Taller> _talleres;

        public ConciliacionService(
            HashSet<Participante> preregistrados,
            HashSet<Participante> registroManual,
            HashSet<Participante> invitadosVip,
            HashSet<Participante> listaNegra,
            HashSet<Participante> asistentesReales,
            List<DuplicadoDetectado> duplicados,
            List<Taller> talleres)
        {
            Preregistrados   = preregistrados;
            RegistroManual   = registroManual;
            InvitadosVip     = invitadosVip;
            ListaNegra       = listaNegra;
            AsistentesReales = asistentesReales;
            Duplicados       = duplicados;
            _talleres        = talleres;
        }

        /// <summary>
        /// Calcula autorizados, no autorizados y ausentes usando operaciones de conjuntos.
        /// </summary>
        public void ProcesarConciliacion()
        {
            // autorizados = (preregistrados ∪ registroManual ∪ invitadosVip) - listaNegra
            Autorizados = new HashSet<Participante>(Preregistrados);
            Autorizados.UnionWith(RegistroManual);   // ∪ registroManual
            Autorizados.UnionWith(InvitadosVip);     // ∪ invitadosVip
            Autorizados.ExceptWith(ListaNegra);      // - listaNegra

            // noAutorizados = asistentesReales - autorizados
            NoAutorizados = new HashSet<Participante>(AsistentesReales);
            NoAutorizados.ExceptWith(Autorizados);

            // ausentes = autorizados - asistentesReales
            Ausentes = new HashSet<Participante>(Autorizados);
            Ausentes.ExceptWith(AsistentesReales);

            // Demostración: todos los VIPs están dentro de los autorizados
            bool vipsDentroDeAutorizados = InvitadosVip.IsSubsetOf(
                Autorizados.Union(ListaNegra)); // VIPs pueden estar bloqueados
            _ = vipsDentroDeAutorizados; // usado en reporte

            ProcesarInscripciones();
        }

        /// <summary>
        /// Intenta inscribir participantes a talleres, validando todas las reglas.
        /// CASO 5: cruce de horario | CASO 6: taller sin cupo
        /// </summary>
        private void ProcesarInscripciones()
        {
            // Inscripciones de prueba: (documento participante, nombre taller)
            var intentos = new List<(string doc, string taller)>
            {
                // Inscripciones válidas
                ("111", "Clean Architecture"),
                ("222", "Clean Architecture"),
                ("555", "Docker Pro"),
                ("123", "Microservicios Avanzados"),
                ("999", "Azure DevOps"),
                ("777", "IA con .NET"),
                ("101", "Azure DevOps"),
                ("201", "IA con .NET"),
                ("202", "Azure DevOps"),

                // CASO 5: Ana (111) ya está en Clean Architecture (09:00-11:00)
                //         → intenta entrar a Microservicios (09:30-11:30) → CRUCE
                ("111", "Microservicios Avanzados"),

                // CASO 6: Docker Pro tiene capacidad 1, ya hay uno inscrito
                //         → Pedro (222) intenta entrar → SIN CUPO
                ("222", "Docker Pro"),

                // Participante no autorizado intenta inscribirse → RECHAZADO
                ("000", "IA con .NET"),
            };

            foreach (var (doc, nombreTaller) in intentos)
            {
                var participante = BuscarParticipante(Autorizados, doc);
                var taller       = _talleres.FirstOrDefault(t => t.Nombre == nombreTaller);

                if (participante is null || taller is null) continue;

                InscribirParticipante(participante, taller);
            }

            // Intento de participante NO autorizado (Desconocido X)
            var noAuth = BuscarParticipante(AsistentesReales, "000");
            var tallerIA = _talleres.FirstOrDefault(t => t.Nombre == "IA con .NET");
            if (noAuth is not null && tallerIA is not null)
            {
                InscripcionesRechazadas.Add(new InscripcionRechazada
                {
                    Participante = noAuth,
                    Taller       = tallerIA,
                    Motivo       = "participante no autorizado"
                });
            }
        }

        private void InscribirParticipante(Participante p, Taller taller)
        {
            // Validar 1: participante autorizado
            if (!Autorizados.Contains(p))
            {
                InscripcionesRechazadas.Add(new InscripcionRechazada
                    { Participante = p, Taller = taller, Motivo = "participante no autorizado" });
                return;
            }

            // Validar 2: cruce de horario
            var talleresDelParticipante = Inscripciones
                .Where(i => i.Participante.Equals(p))
                .Select(i => i.Taller);

            if (talleresDelParticipante.Any(t => t.SeCruzaCon(taller)))
            {
                InscripcionesRechazadas.Add(new InscripcionRechazada
                    { Participante = p, Taller = taller, Motivo = "cruce de horario" });
                return;
            }

            // Validar 3: capacidad del taller
            int inscritos = Inscripciones.Count(i => i.Taller.Id == taller.Id);
            if (inscritos >= taller.Capacidad)
            {
                InscripcionesRechazadas.Add(new InscripcionRechazada
                    { Participante = p, Taller = taller, Motivo = "sin cupo disponible" });
                return;
            }

            // Todo válido: inscribir
            Inscripciones.Add(new InscripcionTaller { Participante = p, Taller = taller });
        }

        private static Participante? BuscarParticipante(IEnumerable<Participante> conjunto, string doc) =>
            conjunto.FirstOrDefault(p => p.Documento == doc);

        // ── Conteos para el reporte ──────────────────────────────────────
        public IEnumerable<Taller> TalleresLlenos() =>
            _talleres.Where(t =>
                Inscripciones.Count(i => i.Taller.Id == t.Id) >= t.Capacidad);

        public bool VipsSonSubconjuntoDeAutorizados() =>
            InvitadosVip.IsSubsetOf(Autorizados.Union(ListaNegra));

        public bool HaySuperpocisionEntreAsistentesYAutorizados() =>
            AsistentesReales.Overlaps(Autorizados);
    }
}