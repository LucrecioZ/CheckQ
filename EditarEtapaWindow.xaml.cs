using System.Windows;
using ChecklistInstaller.Models;

namespace ChecklistInstaller
{
    public partial class EditarEtapaWindow : Window
    {
        private readonly Etapa etapa;

        public EditarEtapaWindow(Etapa etapa)
        {
            InitializeComponent();

            this.etapa = etapa;

            txtDescricao.Text =
                etapa.Descricao;
        }

        private void BtnSalvar_Click(
            object sender,
            RoutedEventArgs e)
        {
            string descricao =
                txtDescricao.Text.Trim();

            if (string.IsNullOrWhiteSpace(descricao))
            {
                MessageBox.Show(
                    "Digite uma descrição.",
                    "Atenção",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            etapa.Descricao =
                descricao;

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