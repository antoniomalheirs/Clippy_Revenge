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

        // --- COMPONENTES PARA MESSAGE TRAP ---
        private Label trollLabel;
        private System.Timers.Timer prankIntervalTimer;
        private System.Windows.Forms.Timer messageDisplayTimer;
        private Random random = new Random();
        private List<string> trollMessages = new List<string> {
            "O seu ficheiro foi eliminado com sucesso.",
            "A formatar a unidade C:... 2%", 
            "Deteção de atividade de teclado suspeita.",
            "A sua subscrição de pôneis expirou.", 
            "A comprar Bitcoin com o seu cartão de crédito...",
            "Atualização do Windows falhou. A reverter alterações...", 
            "O rato moveu-se. Tem a certeza que foi você?", 
            "A enviar o seu histórico de navegação para a sua mãe."
        };

        // --- COMPONENTES PARA A TRAP DO CLIPPY ---
        private ClippyControl clippy;                        // A nossa instância do Clippy
        private System.Timers.Timer clippyAppearanceTimer;  // Timer para decidir quando o Clippy aparece
        private List<string> clippyMessages = new List<string> {
            "Parece que você está a tentar trabalhar. Quer ajuda com isso?",
            "Uma dica: mover o rato aleatoriamente não resolve problemas.",
            "Detetei uma falha na cadeira. Por favor, contacte o suporte.",
            "Posso sugerir uma pausa? Você parece cansado.",
            "Não se esqueça de piscar os olhos. É importante."
        };

        // --- COMPONENTES PARA O MOUSE TRAP ---
        private System.Windows.Forms.Timer mouseMonitorTimer;  // Timer que vigia a posição do rato
        private System.Windows.Forms.Timer mouseJiggleTimer;  // Timer que mexe o rato
        private Rectangle trollArea;                         // A área "proibida" do ecrã
        private int jiggleCounter;                          // Contador para limitar a duração do movimento
        private bool drawTrapBorder = false;               // Controla se o contorno deve ser desenhado
        
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
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const int LWA_COLORKEY = 0x1;

        public Form1()
        {
            //File.WriteAllText(logPath, ""); 
            //Log("Construtor - Início");

            this.Opacity = 0;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.Bounds.Size;
            this.TopMost = true;
            this.BackColor = Color.Magenta;
            this.Paint += Form1_Paint;

            InitializeComponent();
            InitializeTrollComponents();
            InitializeMouseTrap(); // Adicionado
            InitializeClippyTrap();

            SetLayeredWindowAttributes(this.Handle, (uint)ColorTranslator.ToWin32(this.BackColor), 0, LWA_COLORKEY);
            //Log("Construtor - SetLayeredWindowAttributes chamado.");

            int initialStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, initialStyle | WS_EX_TRANSPARENT);
            //Log("Construtor - WS_EX_TRANSPARENT inicial definido.");

            RegisterHotKey(this.Handle, HOTKEY_ID, MOD_CTRL_ALT_SHIFT, (uint)Keys.K);
            this.FormClosing += Form1_FormClosing;

            this.Opacity = 100;
            //Log("Construtor - Opacidade restaurada. Fim do Construtor.");
        }

        private void InitializeClippyTrap()
        {
            // Cria a instância do nosso controlo e adiciona-a ao Form1
            clippy = new ClippyControl { Visible = false };
            this.Controls.Add(clippy);

            // Configura o timer que fará o Clippy aparecer
            clippyAppearanceTimer = new System.Timers.Timer();
            clippyAppearanceTimer.Interval = random.Next(30000, 90000); // Aparece entre 30s e 1.5min
            clippyAppearanceTimer.Elapsed += OnClippyAppearanceElapsed;
            clippyAppearanceTimer.Start();
        }

        private void OnClippyAppearanceElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Usa Invoke para chamar o método que mostra o Clippy na thread da UI
            if (this.IsDisposed || !this.IsHandleCreated) return;
            this.Invoke((MethodInvoker)ShowClippy);

            // Define um novo intervalo aleatório para a próxima aparição
            clippyAppearanceTimer.Interval = random.Next(30000, 90000);
        }

        private void ShowClippy()
        {
            string message = clippyMessages[random.Next(clippyMessages.Count)];
            clippy.StartAnimation(message);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Verifica se a flag para desenhar o contorno está ativa
            if (drawTrapBorder)
            {
                // Cria uma caneta amarela com 4 pixels de espessura
                using (Pen borderPen = new Pen(Color.Yellow, 4))
                {
                    // Desenha o retângulo na área da armadilha
                    e.Graphics.DrawRectangle(borderPen, trollArea);
                }
            }
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

        private void InitializeMouseTrap()
        {
            trollArea = new Rectangle(Screen.PrimaryScreen.Bounds.Width - 200, 0, 200, 200);

            mouseMonitorTimer = new System.Windows.Forms.Timer();
            mouseMonitorTimer.Interval = 100;
            mouseMonitorTimer.Tick += OnMouseMonitorTick;
            mouseMonitorTimer.Start();

            mouseJiggleTimer = new System.Windows.Forms.Timer();
            mouseJiggleTimer.Interval = 30;
            mouseJiggleTimer.Tick += OnMouseJiggleTick;
        }

        private void OnMouseMonitorTick(object sender, EventArgs e)
        {
            if (trollArea.Contains(Cursor.Position))
            {
                drawTrapBorder = true;    // <-- ADICIONE ESTA LINHA: Ativa o desenho
                this.Invalidate();        // <-- ADICIONE ESTA LINHA: Força a janela a redesenhar-se agora

                //Log("Rato entrou na área proibida! A ativar o jiggle.");
                mouseMonitorTimer.Stop();
                jiggleCounter = 0;
                mouseJiggleTimer.Start();
            }
        }

        private void OnMouseJiggleTick(object sender, EventArgs e)
        {
            jiggleCounter++;
            if (jiggleCounter > 100)
            {
                drawTrapBorder = false;    // <-- ADICIONE ESTA LINHA: Ativa o desenho
                this.Invalidate();        // <-- ADICIONE ESTA LINHA: Força a janela a redesenhar-se agora


                //Log("Fim do jiggle. A voltar ao normal.");
                mouseJiggleTimer.Stop();
                mouseMonitorTimer.Start();
                return;
            }

            int offsetX = random.Next(-25, 26);
            int offsetY = random.Next(-25, 26);

            Point newPosition = new Point(
                Math.Max(0, Math.Min(Screen.PrimaryScreen.Bounds.Width - 1, Cursor.Position.X + offsetX)),
                Math.Max(0, Math.Min(Screen.PrimaryScreen.Bounds.Height - 1, Cursor.Position.Y + offsetY))
            );

            Cursor.Position = newPosition;
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
                cp.ExStyle |= WS_EX_LAYERED | WS_EX_TOOLWINDOW;
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