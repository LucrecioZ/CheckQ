using System.IO;
using System.Windows;

namespace ChecklistInstaller.Services;

// Centraliza a troca de cores e lembra a escolha feita neste computador.
public class TemaService
{
    private readonly string caminhoPreferencia;
    private ResourceDictionary? coresAtuais;

    public bool ModoEscuro { get; private set; }

    public TemaService(string? caminhoPreferencia = null)
    {
        this.caminhoPreferencia = caminhoPreferencia ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CheckQ", "tema.txt");
    }

    // Sem uma preferência válida, o aplicativo começa no modo claro.
    public void Carregar()
    {
        bool escuro = false;
        try
        {
            escuro = File.Exists(caminhoPreferencia)
                && File.ReadAllText(caminhoPreferencia).Trim() == "Escuro";
        }
        catch (Exception erro) when (erro is IOException or UnauthorizedAccessException)
        {
            // Uma falha na leitura da preferência não impede a abertura do aplicativo.
        }

        Aplicar(escuro);
    }

    // Atualiza as janelas abertas. As paletas ficam em Themes/Claro.xaml e Themes/Escuro.xaml.
    private void Aplicar(bool escuro)
    {
        string nome = escuro ? "Escuro" : "Claro";
        var novasCores = new ResourceDictionary
        {
            Source = new Uri($"pack://application:,,,/CheckQ;component/Themes/{nome}.xaml")
        };

        var dicionarios = Application.Current.Resources.MergedDictionaries;
        if (coresAtuais != null)
            dicionarios.Remove(coresAtuais);

        dicionarios.Add(novasCores);
        coresAtuais = novasCores;
        ModoEscuro = escuro;
    }

    // A troca funciona mesmo se não for possível guardar a preferência; a tela avisa nesse caso.
    public bool Alternar()
    {
        Aplicar(!ModoEscuro);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(caminhoPreferencia))!);
            File.WriteAllText(caminhoPreferencia, ModoEscuro ? "Escuro" : "Claro");
            return true;
        }
        catch (Exception erro) when (erro is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }
}
