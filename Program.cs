using System;
using System.Windows.Forms;

namespace WinSystemHelperF
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Inicia a aplicação com o nosso contexto, e não diretamente com o Form1
            Application.Run(new StealthAppContext());
        }
    }

    // O gestor de ciclo de vida da nossa aplicação
    public class StealthAppContext : ApplicationContext
    {
        private Form1 overlayForm;

        public StealthAppContext()
        {
            ShowOverlayForm();
        }

        private void ShowOverlayForm()
        {
            overlayForm = new Form1();
            // Dizemos ao contexto para "ouvir" quando o formulário for fechado
            overlayForm.FormClosed += OnFormClosed;
            overlayForm.Show();
        }

        // Este método é chamado quando a janela Form1 é fechada
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            // Em vez de fechar a aplicação, esperamos 10 segundos e recriamos o formulário
            var respawnTimer = new Timer();
            respawnTimer.Interval = 10000;
            respawnTimer.Tick += (s, args) =>
            {
                ShowOverlayForm();
                respawnTimer.Stop();
                respawnTimer.Dispose();
            };
            respawnTimer.Start();
        }
    }
}