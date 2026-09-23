using System.Windows;
using ChecklistInstaller.Models;

namespace ChecklistInstaller
{
    public partial class EditarEtapaWindow : Window
    {
        private readonly Etapa etapa;

        // Abre o formulário com a descrição atual da etapa escolhida.
        public EditarEtapaWindow(Etapa etapa)
        {
            InitializeComponent();

            this.etapa = etapa;

            txtDescricao.Text =
                etapa.Descricao;
        }

        // Exige uma descrição e confirma a alteração. ProcessoWindow salva o resultado no arquivo.
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

        // Fecha sem aplicar o texto digitado à etapa.
        private void BtnCancelar_Click(
            object sender,
            RoutedEventArgs e)
        {
            DialogResult = false;

            Close();
        }
    }
}
