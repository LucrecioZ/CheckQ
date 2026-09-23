using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using ChecklistInstaller.Models;
using ChecklistInstaller.Services;

namespace ChecklistInstaller
{
    // Tela inicial: lista os processos e reúne os caminhos para criar, abrir, editar e excluir.
    // A estrutura geral fica em MainWindow.xaml; os cartões são desenhados aqui em CriarCardProcesso.
    public partial class MainWindow : Window
    {
        private readonly JsonService jsonService;

        // Ao iniciar, prepara a tela e busca os processos já salvos no computador.
        public MainWindow()
        {
            InitializeComponent();

            AtualizarBotaoTema();

            jsonService = new JsonService();

            CarregarProcessos();
        }

        // Recarrega a lista e a quantidade de processos, substituindo os cartões antigos.
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

        // O botão mostra o modo para o qual podemos mudar com o próximo clique.
        private void AtualizarBotaoTema()
        {
            btnAlternarTema.Content = App.Tema.ModoEscuro ? "Modo claro" : "Modo escuro";
            btnAlternarTema.ToolTip = App.Tema.ModoEscuro
                ? "Trocar para o modo claro" : "Trocar para o modo escuro";
        }

        private void BtnAlternarTema_Click(object sender, RoutedEventArgs e)
        {
            bool salvo = App.Tema.Alternar();
            AtualizarBotaoTema();

            if (!salvo)
                MessageBox.Show(this,
                    "O modo foi alterado, mas não foi possível guardar sua escolha para a próxima abertura.",
                    "Preferência de aparência", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Monta o resumo de um processo, com progresso e botões. Ajuste o visual dos cartões aqui.
        private void CriarCardProcesso(Processo processo)
        {
            // Conta as tarefas concluídas para mostrar o avanço na tela inicial.
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

            // Aparência do cartão: fundo, borda, cantos, espaço entre cartões e sombra.

            Border card = new Border
            {
                Background = (Brush)FindResource("Superficie"),

                BorderBrush = (Brush)FindResource("Borda"),

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

            // Distribui o espaço entre o resumo clicável e os botões Editar e Excluir.

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

            // Clicar no resumo abre as etapas. O estilo BotaoCard define a aparência dessa área.

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

            // Textos do resumo: título, sistema, responsável, tarefas e barra de progresso.

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
                        (Brush)FindResource("AzulEscuro"),

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
                        (Brush)FindResource("TextoSecundario")
                };

            TextBlock usuario =
                new TextBlock
                {
                    Text =
                        $"Responsável: {processo.Usuario}",

                    FontSize = 13,

                    Foreground =
                        (Brush)FindResource("TextoSecundario"),

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
                        (Brush)FindResource("DestaqueTexto"),

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

            // Mantém as cores do cartão ligadas ao modo escolhido.
            card.SetResourceReference(Border.BackgroundProperty, "Superficie");
            card.SetResourceReference(Border.BorderBrushProperty, "Borda");
            titulo.SetResourceReference(TextBlock.ForegroundProperty, "AzulEscuro");
            sistema.SetResourceReference(TextBlock.ForegroundProperty, "TextoSecundario");
            usuario.SetResourceReference(TextBlock.ForegroundProperty, "TextoSecundario");
            tarefas.SetResourceReference(TextBlock.ForegroundProperty, "DestaqueTexto");

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

            // Aparência do botão Editar; a abertura do formulário fica em BtnEditarProcesso_Click.

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

            // Aparência do botão Excluir; a confirmação e a exclusão ficam em BtnExcluirProcesso_Click.

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

            // Junta as partes do cartão e coloca o resultado na lista da tela inicial.

            card.Child =
                grid;

            painelProcessos.Children.Add(
                card
            );
        }

        // Abre o cadastro. Se a criação for confirmada, salva o processo e abre a tela de etapas.
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

        // Descobre qual processo foi clicado e encaminha para AbrirProcesso.
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

        // Abre a edição dos dados e renova a lista após a confirmação.
        // Quem confere os campos e salva essa edição é EditarProcessoWindow.
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

        // Pede confirmação e apaga o arquivo do processo, incluindo todas as suas etapas.
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

        // Mostra as etapas e esconde a tela inicial enquanto o processo estiver aberto.
        private void AbrirProcesso(
            Processo processo)
        {
            ProcessoWindow janela =
                new ProcessoWindow(
                    processo
                );

            janela.Show();

            Hide();

            // Ao fechar as etapas, volta à tela inicial e busca o progresso atualizado nos arquivos.
            janela.Closed += (s, e) =>
            {
                Show();

                CarregarProcessos();
            };
        }
    }
}
