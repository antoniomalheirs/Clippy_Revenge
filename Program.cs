// Ficheiro: Program.cs
using System;
using System.Windows.Forms;
using WinSystemHelperF;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Em vez de Application.Run(new Form1()),
        // usamos um ApplicationContext personalizado.
        Application.Run(new StealthAppContext());
    }
}

// Classe ApplicationContext personalizada
public class StealthAppContext : ApplicationContext
{
    private Form1 overlayForm;

    public StealthAppContext()
    {
        // Cria o nosso formulário de sobreposição.
        // O formulário irá configurar-se no seu evento Load.
        overlayForm = new Form1();
        overlayForm.Show(); // Mostra o formulário, que será invisível e sem foco
    }
}