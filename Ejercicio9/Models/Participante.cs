using System;

namespace BreakLineEvents.Models
{
    /// <summary>
    /// Representa a un participante del evento.
    ///
    /// ESTRATEGIA DE IGUALDAD:
    /// Dos participantes se consideran el mismo si comparten el mismo Documento
    /// o el mismo Email normalizado (sin espacios, en minúsculas).
    /// Esto permite detectar duplicados provenientes de distintas fuentes.
    ///
    /// GetHashCode usa solo el Documento para mantener consistencia con Equals
    /// en HashSet (un único hash por objeto, incluso cuando el Email coincide).
    /// </summary>
    public class Participante : IEquatable<Participante>
    {
        public Guid   Id             { get; init; } = Guid.NewGuid();
        public string Documento      { get; init; } = "";
        public string NombreCompleto { get; init; } = "";
        public string Email          { get; init; } = "";
        public bool   EsVip          { get; init; }

        // Email normalizado: sin espacios extremos y en minúsculas
        public string EmailNormalizado => Email.Trim().ToLowerInvariant();

        public bool Equals(Participante? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Documento == other.Documento
                || EmailNormalizado == other.EmailNormalizado;
        }

        public override bool Equals(object? obj) => Equals(obj as Participante);

        // Hash basado en Documento para ser consistente con Equals
        public override int GetHashCode() => Documento.GetHashCode();

        public override string ToString() =>
            $"{NombreCompleto,-25} | DOC: {Documento,-6} | EMAIL: {Email}{(EsVip ? " [VIP]" : "")}";
    }
}