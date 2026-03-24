using System;

namespace BreakLineEvents.Models
{
    /// <summary>
    /// Representa un taller del evento con su horario y capacidad.
    /// Dos talleres se solapan si sus franjas horarias se cruzan.
    /// </summary>
    public class Taller
    {
        public Guid     Id         { get; init; } = Guid.NewGuid();
        public string   Nombre     { get; init; } = "";
        public TimeOnly HoraInicio { get; init; }
        public TimeOnly HoraFin    { get; init; }
        public int      Capacidad  { get; init; }

        /// <summary>
        /// Determina si este taller se cruza en horario con otro.
        /// Hay cruce si uno empieza antes de que el otro termine.
        /// </summary>
        public bool SeCruzaCon(Taller otro) =>
            HoraInicio < otro.HoraFin && otro.HoraInicio < HoraFin;

        public override string ToString() =>
            $"\"{Nombre}\" ({HoraInicio:HH:mm}–{HoraFin:HH:mm}, cap. {Capacidad})";
    }
}