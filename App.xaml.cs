using System.Configuration;
using System.Data;
using System.Windows;

namespace ChecklistInstaller;

/// <summary>
/// Interaçao logica com App.xaml
/// </summary>
public partial class App : Application
{
    public static Services.TemaService Tema { get; } = new();

    // Recupera o modo escolhido antes de mostrar a primeira tela.
    protected override void OnStartup(StartupEventArgs e)
    {
        Tema.Carregar();
        base.OnStartup(e);
    }
}

