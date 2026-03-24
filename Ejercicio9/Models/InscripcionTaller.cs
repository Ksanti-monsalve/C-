using System;

namespace BreakLineEvents.Models
{
    /// <summary>
    /// Relaciona un participante con un taller.
    /// Dos inscripciones son iguales si tienen el mismo participante Y el mismo taller.
    /// </summary>
    public class InscripcionTaller : IEquatable<InscripcionTaller>
    {
        public Participante Participante { get; init; } = null!;
        public Taller       Taller       { get; init; } = null!;

        public bool Equals(InscripcionTaller? other)
        {
            if (other is null) return false;
            return Participante.Equals(other.Participante)
                && Taller.Id == other.Taller.Id;
        }

        public override bool Equals(object? obj) => Equals(obj as InscripcionTaller);

        public override int GetHashCode() =>
            HashCode.Combine(Participante.GetHashCode(), Taller.Id);

        public override string ToString() =>
            $"{Participante.NombreCompleto} → {Taller.Nombre}";
    }
}