using System;
using System.Collections.Generic;
using BreakLineEvents.Models;

namespace BreakLineEvents.Services
{
    /// <summary>
    /// Carga los datos de prueba que cubren todos los casos del enunciado.
    /// </summary>
    public static class CargaDatos
    {
        // ── Talleres ─────────────────────────────────────────────────────
        public static List<Taller> CrearTalleres() => new()
        {
            new Taller { Nombre = "Clean Architecture",       HoraInicio = new TimeOnly(9,0),  HoraFin = new TimeOnly(11,0), Capacidad = 3  },
            new Taller { Nombre = "Microservicios Avanzados", HoraInicio = new TimeOnly(9,30), HoraFin = new TimeOnly(11,30),Capacidad = 2  },
            new Taller { Nombre = "Docker Pro",               HoraInicio = new TimeOnly(12,0), HoraFin = new TimeOnly(14,0), Capacidad = 1  },
            new Taller { Nombre = "Azure DevOps",             HoraInicio = new TimeOnly(14,0), HoraFin = new TimeOnly(16,0), Capacidad = 5  },
            new Taller { Nombre = "IA con .NET",              HoraInicio = new TimeOnly(16,0), HoraFin = new TimeOnly(18,0), Capacidad = 4  },
        };

        // ── Participantes individuales (con casos del enunciado) ──────────
        public static (
            HashSet<Participante> preregistrados,
            HashSet<Participante> registroManual,
            HashSet<Participante> invitadosVip,
            HashSet<Participante> listaNegra,
            HashSet<Participante> asistentesReales,
            List<DuplicadoDetectado> duplicados
        ) CargarParticipantes()
        {
            var duplicados = new List<DuplicadoDetectado>();

            // ── Preregistrados ────────────────────────────────────────────
            var preregistrados = new HashSet<Participante>();

            AgregarConControl(preregistrados, duplicados, "preregistrados",
                new Participante { Documento = "111", NombreCompleto = "Ana Torres",    Email = "ana@gmail.com"     });
            AgregarConControl(preregistrados, duplicados, "preregistrados",
                new Participante { Documento = "222", NombreCompleto = "Pedro Gómez",   Email = "pedro@mail.com"    });
            AgregarConControl(preregistrados, duplicados, "preregistrados",
                new Participante { Documento = "333", NombreCompleto = "Laura Pérez",   Email = "laura@correo.com"  });
            AgregarConControl(preregistrados, duplicados, "preregistrados",
                new Participante { Documento = "444", NombreCompleto = "Carlos Ruiz",   Email = "carlos@correo.com" });
            AgregarConControl(preregistrados, duplicados, "preregistrados",
                new Participante { Documento = "555", NombreCompleto = "Sofía Vargas",  Email = "sofia@correo.com"  });

            // CASO 1: Duplicado por documento (mismo 123, distinto nombre y email)
            AgregarConControl(preregistrados, duplicados, "preregistrados",
                new Participante { Documento = "123", NombreCompleto = "Ana Torres",    Email = "ana@gmail.com"     });
            AgregarConControl(preregistrados, duplicados, "preregistrados",
                new Participante { Documento = "123", NombreCompleto = "Ana T.",        Email = "anatorres@gmail.com" }); // DUPLICADO DOC

            // CASO 2: Duplicado por email normalizado (mayúsculas + espacio)
            AgregarConControl(preregistrados, duplicados, "preregistrados",
                new Participante { Documento = "999", NombreCompleto = "Luis Díaz",     Email = "LDiaz@correo.com"  });
            AgregarConControl(preregistrados, duplicados, "preregistrados",
                new Participante { Documento = "888", NombreCompleto = "Luis D.",       Email = "ldiaz@correo.com " }); // DUPLICADO EMAIL

            // ── Registro manual ───────────────────────────────────────────
            var registroManual = new HashSet<Participante>();

            AgregarConControl(registroManual, duplicados, "registroManual",
                new Participante { Documento = "666", NombreCompleto = "Mario Lara",    Email = "mario@web.com"     });
            AgregarConControl(registroManual, duplicados, "registroManual",
                new Participante { Documento = "777", NombreCompleto = "Elena Ríos",    Email = "elena@web.com"     });
            AgregarConControl(registroManual, duplicados, "registroManual",
                new Participante { Documento = "333", NombreCompleto = "Laura Pérez",   Email = "laura@correo.com"  }); // ya en preregistrados
            AgregarConControl(registroManual, duplicados, "registroManual",
                new Participante { Documento = "101", NombreCompleto = "Jorge Medina",  Email = "jorge@web.com"     });

            // ── Invitados VIP ─────────────────────────────────────────────
            var invitadosVip = new HashSet<Participante>();

            AgregarConControl(invitadosVip, duplicados, "invitadosVip",
                new Participante { Documento = "201", NombreCompleto = "Diana Castillo", Email = "diana@vip.com", EsVip = true });
            AgregarConControl(invitadosVip, duplicados, "invitadosVip",
                new Participante { Documento = "202", NombreCompleto = "Roberto Suárez", Email = "roberto@vip.com", EsVip = true });
            AgregarConControl(invitadosVip, duplicados, "invitadosVip",
                new Participante { Documento = "203", NombreCompleto = "Valeria Mora",   Email = "valeria@vip.com", EsVip = true });

            // ── Lista negra ───────────────────────────────────────────────
            var listaNegra = new HashSet<Participante>();

            // CASO 3: Carlos Ruiz está preregistrado PERO también en lista negra
            listaNegra.Add(new Participante { Documento = "444", NombreCompleto = "Carlos Ruiz", Email = "carlos@correo.com" });
            listaNegra.Add(new Participante { Documento = "666", NombreCompleto = "Mario Lara",  Email = "mario@web.com"    });

            // ── Asistentes reales ─────────────────────────────────────────
            var asistentesReales = new HashSet<Participante>();

            // Autorizados que sí asistieron
            asistentesReales.Add(new Participante { Documento = "111", NombreCompleto = "Ana Torres",    Email = "ana@gmail.com"    });
            asistentesReales.Add(new Participante { Documento = "222", NombreCompleto = "Pedro Gómez",   Email = "pedro@mail.com"   });
            asistentesReales.Add(new Participante { Documento = "555", NombreCompleto = "Sofía Vargas",  Email = "sofia@correo.com" });
            asistentesReales.Add(new Participante { Documento = "123", NombreCompleto = "Ana Torres",    Email = "ana@gmail.com"    });
            asistentesReales.Add(new Participante { Documento = "999", NombreCompleto = "Luis Díaz",     Email = "LDiaz@correo.com" });
            asistentesReales.Add(new Participante { Documento = "777", NombreCompleto = "Elena Ríos",    Email = "elena@web.com"    });
            asistentesReales.Add(new Participante { Documento = "101", NombreCompleto = "Jorge Medina",  Email = "jorge@web.com"    });
            asistentesReales.Add(new Participante { Documento = "201", NombreCompleto = "Diana Castillo",Email = "diana@vip.com"    });
            asistentesReales.Add(new Participante { Documento = "202", NombreCompleto = "Roberto Suárez",Email = "roberto@vip.com"  });
            asistentesReales.Add(new Participante { Documento = "203", NombreCompleto = "Valeria Mora",  Email = "valeria@vip.com"  });

            // CASO 4: Asistente real NO registrado en ninguna fuente
            asistentesReales.Add(new Participante { Documento = "000", NombreCompleto = "Desconocido X", Email = "unknown@x.com"   });

            // Laura (333) y Carlos (444 - lista negra) NO asistieron → aparecerán en ausentes / bloqueados

            return (preregistrados, registroManual, invitadosVip, listaNegra, asistentesReales, duplicados);
        }

        // ── Helper: agrega y registra si es duplicado ─────────────────────
        private static void AgregarConControl(
            HashSet<Participante> conjunto,
            List<DuplicadoDetectado> duplicados,
            string fuente,
            Participante p)
        {
            if (!conjunto.Add(p))
            {
                // HashSet.Add devuelve false si ya existe (por Equals/GetHashCode)
                var existente = ObtenerExistente(conjunto, p);
                if (existente?.Documento == p.Documento)
                    duplicados.Add(new DuplicadoDetectado { Tipo = "Documento", Valor = p.Documento, Fuente = fuente });
                else
                    duplicados.Add(new DuplicadoDetectado { Tipo = "Email",     Valor = p.EmailNormalizado, Fuente = fuente });
            }
        }

        private static Participante? ObtenerExistente(HashSet<Participante> conjunto, Participante p)
        {
            foreach (var item in conjunto)
                if (item.Equals(p)) return item;
            return null;
        }
    }
}