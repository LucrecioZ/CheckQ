using System;
using System.Collections.Generic;
using System.Windows;
using ChecklistInstaller.Models;
using ChecklistInstaller.Services;

namespace ChecklistInstaller
{
    public partial class NovoProcessoWindow : Window
    {
        private JsonService jsonService;

        public Processo ProcessoCriado { get; private set; }
            = new Processo();

        public NovoProcessoWindow()
        {
            InitializeComponent();

            jsonService = new JsonService();

            CarregarSistemas();
        }

        private void CarregarSistemas()
        {
            List<string> sistemas =
                jsonService.ListarSistemas();

            cmbSistema.ItemsSource =
                sistemas;
        }

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


            // ==========================================
            // VALIDAÇÕES
            // ==========================================

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


            // ==========================================
            // SALVAR SISTEMA
            // ==========================================

            jsonService.AdicionarSistema(
                sistema
            );


            // ==========================================
            // CRIAR PROCESSO
            // ==========================================

            Processo processo =
                new Processo();

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


            DialogResult = true;

            Close();
        }

        private void BtnCancelar_Click(
            object sender,
            RoutedEventArgs e)
        {
            DialogResult = false;

            Close();
        }
    }
}