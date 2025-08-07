using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinSystemHelperF
{
    public partial class Form1 : Form
    {
        // --- LOGGING ---
        private static readonly string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "troll_app_log.txt");
        private void Log(string message)
        {
            try { File.AppendAllText(logPath, $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}"); }
            catch { /* Ignora erros de log */ }
        }

        // --- COMPONENTES ---
        private Label trollLabel;
        private System.Timers.Timer prankIntervalTimer;
        private System.Windows.Forms.Timer messageDisplayTimer;
        private Random random = new Random();
        private List<string> trollMessages = new List<string> {
            "O seu ficheiro foi eliminado com sucesso.", "A formatar a unidade C:... 2%", "Deteção de atividade de teclado suspeita.", "A sua subscrição de pôneis expirou.", "A comprar Bitcoin com o seu cartão de crédito...", "Atualização do Windows falhou. A reverter alterações...", "O rato moveu-se. Tem a certeza que foi você?", "A enviar o seu histórico de navegação para a sua mãe."
        };

        // --- API DO WINDOWS ---
        [DllImport("user32.dll")] private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")] private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll", SetLastError = true)] private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        [DllImport("user32.dll", SetLastError = true)] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)] private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const uint MOD_CTRL_ALT_SHIFT = 1 | 2 | 4;
        private const int HOTKEY_ID = 1;
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int LWA_COLORKEY = 0x1;

        public Form1()
        {
            File.WriteAllText(logPath, ""); // Limpa o log antigo
            Log("Construtor - Início");

            // PASSO 1: Definir propriedades que não precisam de um "handle" de janela
            this.Opacity = 0; // Começa invisível para evitar o "piscar"
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.Bounds.Size;
            this.TopMost = true;
            this.BackColor = Color.Magenta; // A nossa cor chave para a transparência

            InitializeComponent();
            InitializeTrollComponents();

            // PASSO 2: Fazer a magia que PRECISA de um "handle" de janela
            // O "handle" (this.Handle) já foi criado por InitializeComponent()

            // Torna o fundo Magenta transparente
            SetLayeredWindowAttributes(this.Handle, (uint)ColorTranslator.ToWin32(this.BackColor), 0, LWA_COLORKEY);
            Log("Construtor - SetLayeredWindowAttributes chamado.");

            // Define o estado inicial como "clicável através"
            int initialStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, initialStyle | WS_EX_TRANSPARENT);
            Log("Construtor - WS_EX_TRANSPARENT inicial definido.");

            // Regista a hotkey para fechar
            RegisterHotKey(this.Handle, HOTKEY_ID, MOD_CTRL_ALT_SHIFT, (uint)Keys.K);
            this.FormClosing += Form1_FormClosing;

            // PASSO 3: Agora que tudo está configurado, podemos restaurar a opacidade
            // A janela vai aparecer, mas já totalmente transparente
            this.Opacity = 100;
            Log("Construtor - Opacidade restaurada. Fim do Construtor.");
        }

        private void InitializeTrollComponents()
        {
            trollLabel = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.AntiqueWhite,
                Font = new Font("Arial", 36, FontStyle.Bold),
                Visible = false
            };
            this.Controls.Add(trollLabel);

            prankIntervalTimer = new System.Timers.Timer { Interval = 10000 };
            prankIntervalTimer.Elapsed += OnPrankIntervalElapsed;
            prankIntervalTimer.Start();

            messageDisplayTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            messageDisplayTimer.Tick += OnMessageDisplayTick;
        }

        private void OnPrankIntervalElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            this.Invoke((MethodInvoker)ShowRandomMessage);
        }

        private void ShowRandomMessage()
        {
            if (messageDisplayTimer.Enabled) return;

            int currentStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, currentStyle & ~WS_EX_TRANSPARENT);

            trollLabel.Text = trollMessages[random.Next(trollMessages.Count)];
            trollLabel.Location = new Point(random.Next(0, this.Width - trollLabel.Width), random.Next(0, this.Height - trollLabel.Height));
            trollLabel.Visible = true;

            messageDisplayTimer.Start();
        }

        private void OnMessageDisplayTick(object sender, EventArgs e)
        {
            trollLabel.Visible = false;
            messageDisplayTimer.Stop();

            int currentStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, currentStyle | WS_EX_TRANSPARENT);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_LAYERED;
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == HOTKEY_ID)
            {
                Application.Exit();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, HOTKEY_ID);
        }
    }
}