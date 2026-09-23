using System.Collections.Generic;

namespace ChecklistInstaller.Models
{
    public class Processo
    {
        public string Id { get; set; } = string.Empty;

        public string Usuario { get; set; } = string.Empty;

        public string Sistema { get; set; } = string.Empty;

        public string Titulo { get; set; } = string.Empty;

        public List<Etapa> Etapas { get; set; }

        public Processo()
        {
            Etapas = new List<Etapa>();
        }
    }
}