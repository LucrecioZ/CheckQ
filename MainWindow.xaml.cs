using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using ChecklistInstaller.Models;
using ChecklistInstaller.Services;

namespace ChecklistInstaller
{
    public partial class MainWindow : Window
    {
        private readonly JsonService jsonService;

        public MainWindow()
        {
            InitializeComponent();

            jsonService = new JsonService();

            CarregarProcessos();
        }

        private void CarregarProcessos()
        {
            painelProcessos.Children.Clear();

            List<Processo> processos =
                jsonService.ListarProcessos();

            txtQuantidadeProcessos.Text =
                processos.Count == 1
                    ? "1 processo"
                    : $"{processos.Count} processos";

            foreach (Processo processo in processos)
            {
                CriarCardProcesso(processo);
            }
        }

        private void CriarCardProcesso(Processo processo)
        {
            int concluidas = 0;

            foreach (Etapa etapa in processo.Etapas)
            {
                if (etapa.Concluida)
                {
                    concluidas++;
                }
            }

            int total =
                processo.Etapas.Count;
                

            double porcentagem = 0;

            if (total > 0)
            {
                porcentagem =
                    (double)concluidas / total * 100;
            }

            string resumoTarefas =
                $"{total} tarefas • {concluidas} concluídas";

            // ==========================================
            // CARD
            // ==========================================

            Border card = new Border
            {
                Background = Brushes.White,

                BorderBrush = new SolidColorBrush(
                    Color.FromRgb(220, 228, 235)
                ),

                BorderThickness = new Thickness(1),

                CornerRadius = new CornerRadius(14),

                Margin = new Thickness(0, 0, 0, 12),

                Effect = new DropShadowEffect
                {
                    BlurRadius = 15,
                    ShadowDepth = 1,
                    Opacity = 0.08
                }
            };

            // ==========================================
            // GRID DO CARD
            // ==========================================

            Grid grid = new Grid
            {
                Height = 145
            };

            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = new GridLength(
                        1,
                        GridUnitType.Star
                    )
                }
            );

            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = new GridLength(90)
                }
            );

            grid.ColumnDefinitions.Add(
                new ColumnDefinition
                {
                    Width = new GridLength(95)
                }
            );

            // ==========================================
            // ÁREA PARA ABRIR O PROCESSO
            // ==========================================

            Button botaoAbrir = new Button();

            botaoAbrir.Style =
                (Style)FindResource("BotaoCard");

            botaoAbrir.HorizontalContentAlignment =
                HorizontalAlignment.Left;

            botaoAbrir.VerticalContentAlignment =
                VerticalAlignment.Center;

            botaoAbrir.Tag = processo;

            botaoAbrir.Click +=
                BtnAbrirProcesso_Click;

            // ==========================================
            // INFORMAÇÕES
            // ==========================================

            StackPanel informacoes =
                new StackPanel();

            TextBlock titulo =
                new TextBlock
                {
                    Text = processo.Titulo,

                    FontSize = 18,

                    FontWeight =
                        FontWeights.SemiBold,

                    Foreground =
                        new SolidColorBrush(
                            Color.FromRgb(25, 54, 82)
                        ),

                    Margin =
                        new Thickness(0, 0, 0, 7)
                };

            TextBlock sistema =
                new TextBlock
                {
                    Text =
                        $"Sistema: {processo.Sistema}",

                    FontSize = 13,

                    Foreground =
                        new SolidColorBrush(
                            Color.FromRgb(104, 119, 135)
                        )
                };

            TextBlock usuario =
                new TextBlock
                {
                    Text =
                        $"Responsável: {processo.Usuario}",

                    FontSize = 13,

                    Foreground =
                        new SolidColorBrush(
                            Color.FromRgb(104, 119, 135)
                        ),

                    Margin =
                        new Thickness(0, 3, 0, 10)
                };

            TextBlock tarefas =
                new TextBlock
                {
                    Text = resumoTarefas,

                    FontSize = 12,

                    FontWeight =
                        FontWeights.SemiBold,

                    Foreground =
                        new SolidColorBrush(
                            Color.FromRgb(8, 127, 193)
                        ),

                    Margin =
                        new Thickness(0, 0, 0, 7)
                };    

            ProgressBar progresso =
                new ProgressBar
                {
                    Minimum = 0,

                    Maximum = 100,

                    Value = porcentagem,

                    Height = 7,

                    Width = 300,

                    HorizontalAlignment =
                        HorizontalAlignment.Left
                };

            informacoes.Children.Add(titulo);
            informacoes.Children.Add(sistema);
            informacoes.Children.Add(usuario);
            informacoes.Children.Add(tarefas);
            informacoes.Children.Add(progresso);

            botaoAbrir.Content =
                informacoes;

            Grid.SetColumn(
                botaoAbrir,
                0
            );

            grid.Children.Add(
                botaoAbrir
            );

            // ==========================================
            // BOTÃO EDITAR
            // ==========================================

            Button botaoEditar =
                new Button();

            botaoEditar.Content =
                "Editar";

            botaoEditar.Width =
                78;

            botaoEditar.Height =
                34;

            botaoEditar.Padding =
                new Thickness(5, 0, 5, 0);

            botaoEditar.VerticalAlignment =
                VerticalAlignment.Center;

            botaoEditar.Tag =
                processo;

            botaoEditar.Style =
                (Style)FindResource(
                    "BotaoSecundario"
                );

            botaoEditar.Click +=
                BtnEditarProcesso_Click;

            Grid.SetColumn(
                botaoEditar,
                1
            );

            grid.Children.Add(
                botaoEditar
            );

            // ==========================================
            // BOTÃO EXCLUIR
            // ==========================================

            Button botaoExcluir =
                new Button();

            botaoExcluir.Content =
                "Excluir";

            botaoExcluir.Width =
                82;

            botaoExcluir.Height =
                34;

            botaoExcluir.Padding =
                new Thickness(5, 0, 5, 0);

            botaoExcluir.Margin =
                new Thickness(0, 0, 12, 0);

            botaoExcluir.VerticalAlignment =
                VerticalAlignment.Center;

            botaoExcluir.Tag =
                processo;

            botaoExcluir.Style =
                (Style)FindResource(
                    "BotaoPerigo"
                );

            botaoExcluir.Click +=
                BtnExcluirProcesso_Click;

            Grid.SetColumn(
                botaoExcluir,
                2
            );

            grid.Children.Add(
                botaoExcluir
            );

            // ==========================================
            // FINAL DO CARD
            // ==========================================

            card.Child =
                grid;

            painelProcessos.Children.Add(
                card
            );
        }

        private void BtnNovoProcesso_Click(
            object sender,
            RoutedEventArgs e)
        {
            NovoProcessoWindow janela =
                new NovoProcessoWindow();

            bool? resultado =
                janela.ShowDialog();

            if (resultado == true)
            {
                Processo processo =
                    janela.ProcessoCriado;

                jsonService.Salvar(
                    processo
                );

                AbrirProcesso(
                    processo
                );

                CarregarProcessos();
            }
        }

        private void BtnAbrirProcesso_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button botao =
                (Button)sender;

            Processo processo =
                (Processo)botao.Tag;

            AbrirProcesso(
                processo
            );
        }

        private void BtnEditarProcesso_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button botao =
                (Button)sender;

            Processo processo =
                (Processo)botao.Tag;

            EditarProcessoWindow janela =
                new EditarProcessoWindow(
                    processo
                );

            bool? resultado =
                janela.ShowDialog();

            if (resultado == true)
            {
                CarregarProcessos();
            }
        }

        private void BtnExcluirProcesso_Click(
            object sender,
            RoutedEventArgs e)
        {
            Button botao =
                (Button)sender;

            Processo processo =
                (Processo)botao.Tag;

            MessageBoxResult resposta =
                MessageBox.Show(
                    $"Tem certeza que deseja excluir o processo \"{processo.Titulo}\"?\n\nTodas as etapas e o progresso desse processo serão apagados.",
                    "Excluir processo",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

            if (resposta ==
                MessageBoxResult.Yes)
            {
                jsonService.Excluir(
                    processo.Id
                );

                CarregarProcessos();
            }
        }

        private void AbrirProcesso(
            Processo processo)
        {
            ProcessoWindow janela =
                new ProcessoWindow(
                    processo
                );

            janela.Show();

            Hide();

            janela.Closed += (s, e) =>
            {
                Show();

                CarregarProcessos();
            };
        }
    }
}