using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ChecklistInstaller.Models;

namespace ChecklistInstaller.Services
{
    public class JsonService
    {
        private readonly string pastaData;
        private readonly string caminhoSistemas;

        public JsonService()
        {
            pastaData = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData
                ),
                "CheckQ"
            );

            Directory.CreateDirectory(pastaData);

            caminhoSistemas = Path.Combine(
                pastaData,
                "sistemas.json"
            );
        }


        // PROCESSOS, NÃO POSSO ESQUECER DE SALVAR O ID, SENÃO NÃO CONSIGO CARREGAR O PROCESSO NOVAMENTE

        public void Salvar(Processo processo)
        {
            string caminhoArquivo = Path.Combine(
                pastaData,
                $"processo_{processo.Id}.json"
            );

            string json = JsonSerializer.Serialize(
                processo,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }
            );

            File.WriteAllText(
                caminhoArquivo,
                json
            );
        }

        public Processo? Carregar(string id)
        {
            string caminhoArquivo = Path.Combine(
                pastaData,
                $"processo_{id}.json"
            );

            if (!File.Exists(caminhoArquivo))
            {
                return null;
            }

            string json =
                File.ReadAllText(caminhoArquivo);

            return JsonSerializer.Deserialize<Processo>(
                json
            );
        }

        public List<Processo> ListarProcessos()
        {
            List<Processo> processos =
                new List<Processo>();

            string[] arquivos =
                Directory.GetFiles(
                    pastaData,
                    "processo_*.json"
                );

            foreach (string arquivo in arquivos)
            {
                string json =
                    File.ReadAllText(arquivo);

                Processo? processo =
                    JsonSerializer.Deserialize<Processo>(
                        json
                    );

                if (processo != null)
                {
                    processos.Add(processo);
                }
            }

            return processos;
        }

        public void Excluir(string id)
        {
            string caminhoArquivo = Path.Combine(
                pastaData,
                $"processo_{id}.json"
            );

            if (File.Exists(caminhoArquivo))
            {
                File.Delete(caminhoArquivo);
            }
        }

        // ==========================================
        // SISTEMAS
        // ==========================================

        public List<string> ListarSistemas()
{
            if (!File.Exists(caminhoSistemas))
            {
                List<string> sistemasPadrao =
                    new List<string>();

                SalvarSistemas(sistemasPadrao);

                return sistemasPadrao;
            }

            string json =
                File.ReadAllText(caminhoSistemas);

            List<string>? sistemas =
                JsonSerializer.Deserialize<List<string>>(json);

            if (sistemas == null ||
                sistemas.Count == 0)
            {
                sistemas =
                    new List<string>();

                SalvarSistemas(sistemas);
            }

    return sistemas;
}

        public void AdicionarSistema(string nomeSistema)
        {
            nomeSistema =
                nomeSistema.Trim();

            if (string.IsNullOrWhiteSpace(nomeSistema))
            {
                return;
            }

            List<string> sistemas =
                ListarSistemas();

            bool jaExiste =
                sistemas.Exists(
                    sistema =>
                        string.Equals(
                            sistema,
                            nomeSistema,
                            StringComparison.OrdinalIgnoreCase
                        )
                );

            if (!jaExiste)
            {
                sistemas.Add(nomeSistema);

                SalvarSistemas(sistemas);
            }
        }

        private void SalvarSistemas(
            List<string> sistemas)
        {
            string json =
                JsonSerializer.Serialize(
                    sistemas,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }
                );

            File.WriteAllText(
                caminhoSistemas,
                json
            );
        }
    }
}