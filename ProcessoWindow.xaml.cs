using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ChecklistInstaller.Models;
using ChecklistInstaller.Services;

namespace ChecklistInstaller
{
    // Tela de acompanhamento: aqui ficam as ações das etapas de um processo.

    public partial class ProcessoWindow : Window
    {
        private readonly Processo processo;
        private readonly JsonService jsonService;

        // Ao abrir, mostra os dados do processo escolhido, suas etapas e o progresso atual.
        public ProcessoWindow(Processo processo)
        {
            InitializeComponent();

            this.processo = processo;

            jsonService =
                new JsonService();

            txtTituloProcesso.Text =
                processo.Titulo;

            txtInformacoes.Text =
                $"Usuário: {processo.Usuario}  |  Sistema: {processo.Sistema}";

            CarregarEtapasNaTela();

            AtualizarProgresso();
        }

        // Refaz a lista na tela após uma mudança, removendo a versão anterior para não duplicar etapas.
        private void CarregarEtapasNaTela()
        {
            painelEtapas.Children.Clear();

            foreach (Etapa etapa in processo.Etapas)
            {
                CriarCardEtapa(etapa);
            }
        }

        // Monta o cartão de uma etapa. Para mudar cores, tamanhos e botões da etapa, preciso começar aqui.
        private void CriarCardEtapa(Etapa etapa)
        {
            Border card =
                new Border();

            card.Background =
                Brushes.White;

            card.BorderBrush =
                new SolidColorBrush(
                    Color.FromRgb(217, 226, 234)
                );

            card.BorderThickness =
                new Thickness(1);

            card.CornerRadius =
                new CornerRadius(10);

            card.Height =
                58;

            card.Margin =
                new Thickness(0, 0, 0, 8);


            // Divide o cartão em três espaços: descrição, botão Editar e botão de exclusão.
            Grid grid =
                new Grid();

            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width =
                        new GridLength(
                            1,
                            GridUnitType.Star
                        )
                });

            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = new GridLength(80)
                });

            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = new GridLength(55)
                });

            // A caixinha mostra se a etapa foi concluída. Marcar ou desmarcar chama EtapaAlterada.

            CheckBox checkBox =
                new CheckBox();

            checkBox.Content =
                etapa.Descricao;

            checkBox.IsChecked =
                etapa.Concluida;

            // Guarda a etapa junto da caixinha para sabermos qual atualizar quando houver um clique.
            checkBox.Tag =
                etapa;

            checkBox.FontSize =
                15;

            checkBox.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(25, 54, 82)
                );

            checkBox.VerticalAlignment =
                VerticalAlignment.Center;

            checkBox.Margin =
                new Thickness(15, 0, 5, 0);

            checkBox.Checked +=
                EtapaAlterada;

            checkBox.Unchecked +=
                EtapaAlterada;

            Grid.SetColumn(
                checkBox,
                0
            );

            grid.Children.Add(
                checkBox
            );

            // BOTÃO EDITAR

            Button botaoEditar =
                new Button();

            botaoEditar.Content =
                "Editar";

            botaoEditar.Width =
                65;

            botaoEditar.Height =
                32;

            botaoEditar.VerticalAlignment =
                VerticalAlignment.Center;

            botaoEditar.Tag =
                etapa;

            botaoEditar.Background =
                Brushes.White;

            botaoEditar.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(25, 54, 82)
                );

            botaoEditar.BorderBrush =
                new SolidColorBrush(
                    Color.FromRgb(217, 226, 234)
                );

            botaoEditar.BorderThickness =
                new Thickness(1);

            botaoEditar.Click +=
                BtnEditarEtapa_Click;

            Grid.SetColumn(
                botaoEditar,
                1
            );

            grid.Children.Add(
                botaoEditar
            );

            // BOTÃO EXCLUIR

            Button botaoExcluir =
                new Button();

            botaoExcluir.Content =
                "X";

            botaoExcluir.Width =
                35;

            botaoExcluir.Height =
                32;

            botaoExcluir.VerticalAlignment =
                VerticalAlignment.Center;

            botaoExcluir.Tag =
                etapa;

            botaoExcluir.Background =
                Brushes.White;

            botaoExcluir.Foreground =
                new SolidColorBrush(
                    Color.FromRgb(217, 43, 36)
                );

            botaoExcluir.BorderBrush =
                new SolidColorBrush(
                    Color.FromRgb(235, 200, 197)
                );

            botaoExcluir.BorderThickness =
                new Thickness(1);

            botaoExcluir.Click +=
                BtnExcluirEtapa_Click;

            Grid.SetColumn(
                botaoExcluir,
                2
            );

            grid.Children.Add(
                botaoExcluir
            );


            card.Child =
                grid;

            painelEtapas.Children.Add(
                card
            );
        }

        // Botão Adicionar: exige uma descrição, cria uma etapa pendente e salva o processo.
        private void BtnAdicionarEtapa_Click(
            object sender,
            RoutedEventArgs e)
        {
            string descricao =
                txtNovaEtapa.Text.Trim();

            if (string.IsNullOrWhiteSpace(descricao))
            {
                MessageBox.Show(
                    "Digite uma etapa.",
                    "Atenção",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            Etapa etapa =
                new Etapa
                {
                    Descricao = descricao,
                    Concluida = false
                };

            processo.Etapas.Add(
                etapa
            );

            txtNovaEtapa.Clear();

            CarregarEtapasNaTela();

            AtualizarProgresso();

            SalvarProcesso();
        }

        // Ao marcar ou desmarcar uma etapa, atualiza o progresso e salva a mudança na hora.
        private void EtapaAlterada(
            object sender,
            RoutedEventArgs e)
        {
            CheckBox checkBox =
                (CheckBox)sender;

            Etapa etapa =
                (Etapa)checkBox.Tag;

            etapa.Concluida =
                checkBox.IsChecked == true;

            AtualizarProgresso();

            SalvarProcesso();
        }

        // Abre a edição da descrição. Só atualiza a tela e salva se a edição for confirmada.
        // Os campos e a conferência do texto vao ficar em EditarEtapaWindow.
        private void BtnEditarEtapa_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button botao =
                (Button)sender;

            Etapa etapa =
                (Etapa)botao.Tag;

            EditarEtapaWindow janela =
                new EditarEtapaWindow(
                    etapa
                );

            bool? resultado =
                janela.ShowDialog();

            if (resultado == true)
            {
                CarregarEtapasNaTela();

                AtualizarProgresso();

                SalvarProcesso();
            }
        }

        // Pede confirmação antes de remover a etapa; depois atualiza o progresso e salva.
        private void BtnExcluirEtapa_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button botao =
                (Button)sender;

            Etapa etapa =
                (Etapa)botao.Tag;

            MessageBoxResult resposta =
                MessageBox.Show(
                    $"Deseja realmente excluir a etapa \"{etapa.Descricao}\"?",
                    "Excluir etapa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

            if (resposta ==
                MessageBoxResult.Yes)
            {
                processo.Etapas.Remove(
                    etapa
                );

                CarregarEtapasNaTela();

                AtualizarProgresso();

                SalvarProcesso();
            }
        }

        // Conta as etapas concluídas e atualiza o texto e a barra de progresso.
        // Se quiser mudar como o avanço é calculado nesta tela, é aqui. (Tomo bastante cuidado aqui!)
        private void AtualizarProgresso()
        {
            int totalEtapas =
                processo.Etapas.Count;

            int etapasConcluidas =
                0;

            foreach (Etapa etapa in processo.Etapas)
            {
                if (etapa.Concluida)
                {
                    etapasConcluidas++;
                }
            }

            txtProgresso.Text =
                $"Progresso: {etapasConcluidas} / {totalEtapas}";

            if (totalEtapas > 0)
            {
                double porcentagem =
                    (double)etapasConcluidas /
                    totalEtapas *
                    100;

                barraProgresso.Value =
                    porcentagem;
            }
            else
            {
                // Sem etapas, a barra fica vazia e não há divisão por zero.
                barraProgresso.Value = 0;
            }
        }

        // Salva o processo inteiro, incluindo as etapas. A pasta e o arquivo são definidos em Services/JsonService.cs.
        private void SalvarProcesso()
        {
            jsonService.Salvar(
                processo
            );
        }
    }
}
