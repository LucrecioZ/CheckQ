using System.Collections.Generic;
using System.Windows;
using ChecklistInstaller.Models;
using ChecklistInstaller.Services;

namespace ChecklistInstaller
{
    public partial class EditarProcessoWindow : Window
    {
        private readonly Processo processo;
        private readonly JsonService jsonService;

        public EditarProcessoWindow(Processo processo)
        {
            InitializeComponent();

            this.processo = processo;
            jsonService = new JsonService();

            CarregarSistemas();

            txtUsuario.Text = processo.Usuario;
            cmbSistema.Text = processo.Sistema;
            txtTitulo.Text = processo.Titulo;
        }

        private void CarregarSistemas()
        {
            List<string> sistemas =
                jsonService.ListarSistemas();

            cmbSistema.ItemsSource = sistemas;
        }

        private void BtnSalvar_Click(
            object sender,
            RoutedEventArgs e)
        {
            string usuario =
                txtUsuario.Text.Trim();

            string sistema =
                cmbSistema.Text.Trim();

            string titulo =
                txtTitulo.Text.Trim();

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

            processo.Usuario = usuario;
            processo.Sistema = sistema;
            processo.Titulo = titulo;

            jsonService.AdicionarSistema(sistema);
            jsonService.Salvar(processo);

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