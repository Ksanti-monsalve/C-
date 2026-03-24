using BreakLineEvents.Models;

namespace BreakLineEvents.Models
{
    public class InscripcionRechazada
    {
        public Participante Participante { get; init; } = null!;
        public Taller       Taller       { get; init; } = null!;
        public string       Motivo       { get; init; } = "";

        public override string ToString() =>
            $"  - {Participante.NombreCompleto} → Taller {Taller.Nombre} | Motivo: {Motivo}";
    }

    public class DuplicadoDetectado
    {
        public string Tipo    { get; init; } = ""; // "Documento" o "Email"
        public string Valor   { get; init; } = "";
        public string Fuente  { get; init; } = "";

        public override string ToString() =>
            $"  - {Tipo} repetido: {Valor}  (detectado en {Fuente})";
    }
}