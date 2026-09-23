using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ChecklistInstaller.Models;

namespace ChecklistInstaller.Services
{
    // Cuida dos arquivos de processos e da lista de sistemas. Para mudar onde ou como salvar, comece aqui.
    public class JsonService
    {
        private readonly string pastaData;
        private readonly string caminhoSistemas;

        // Prepara a pasta CheckQ dentro de AppData/Local do usuário do Windows.
        // Os processos ficam em arquivos separados; a lista de sistemas fica em sistemas.json.
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


        // Salva os dados e as etapas no arquivo do processo, substituindo a versão anterior.
        // O Id liga cada processo ao seu arquivo: preserve esse valor ao editar os dados.

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

        // Busca um processo pelo identificador. Se o arquivo não existir, retorna sem um processo.
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

        // Lê os arquivos de processos para montar a lista da tela inicial.
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

        // Apaga o arquivo do processo. A confirmação da pessoa deve acontecer na tela antes desta chamada.
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

        // Carrega as opções dos formulários. Se ainda não houver arquivo, começa com uma lista vazia.
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

        // Guarda um sistema novo para os próximos cadastros, sem repetir nomes que só mudam maiúsculas e minúsculas.
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

        // Grava a lista completa de sistemas no arquivo sistemas.json.
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
