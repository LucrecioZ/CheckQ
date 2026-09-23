using System;
using System.Collections.Generic;
using System.Windows;
using ChecklistInstaller.Models;
using ChecklistInstaller.Services;

namespace ChecklistInstaller
{
    // Cadastro de um processo: reúne os dados e entrega o resultado para a tela principal salvar.
    public partial class NovoProcessoWindow : Window
    {
        private JsonService jsonService;

        // A tela principal consulta este resultado quando a pessoa confirma a criação.
        public Processo ProcessoCriado { get; private set; }
            = new Processo();

        // Prepara o formulário e carrega os sistemas que já foram cadastrados.
        public NovoProcessoWindow()
        {
            InitializeComponent();

            jsonService = new JsonService();

            CarregarSistemas();
        }

        // Preenche as sugestões de sistema. O campo também permite digitar um nome novo.
        private void CarregarSistemas()
        {
            List<string> sistemas =
                jsonService.ListarSistemas();

            cmbSistema.ItemsSource =
                sistemas;
        }

        // Botão Criar: confere os campos obrigatórios e monta o novo processo.
        // Para mudar as regras de preenchimento ou os avisos, procure os testes abaixo.
        private void BtnCriar_Click(
            object sender,
            RoutedEventArgs e)
        {
            string usuario =
                txtUsuario.Text.Trim();

            string sistema =
                cmbSistema.Text.Trim();

            string titulo =
                txtTitulo.Text.Trim();

            // VALIDAÇÕES

            if (string.IsNullOrWhiteSpace(usuario))
            {
                MessageBox.Show(
                    "Digite o usuário responsável.",
                    "Atenção",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(sistema))
            {
                MessageBox.Show(
                    "Digite ou selecione um sistema.",
                    "Atenção",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            if (string.IsNullOrWhiteSpace(titulo))
            {
                MessageBox.Show(
                    "Digite um título para o processo.",
                    "Atenção",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            // SALVAR SISTEMA

            jsonService.AdicionarSistema(
                sistema
            );

            // CRIAR PROCESSO

            Processo processo =
                new Processo();

            // Cada processo recebe um identificador próprio, usado também no nome do arquivo salvo.
            processo.Id =
                Guid.NewGuid().ToString();

            processo.Usuario =
                usuario;

            processo.Sistema =
                sistema;

            processo.Titulo =
                titulo;


            ProcessoCriado =
                processo;


            // Avisa que deu certo. MainWindow.BtnNovoProcesso_Click salva o processo e abre suas etapas.
            DialogResult = true;

            Close();
        }

        // Fecha o formulário sem criar o processo.
        private void BtnCancelar_Click(
            object sender,
            RoutedEventArgs e)
        {
            DialogResult = false;

            Close();
        }
    }
}
