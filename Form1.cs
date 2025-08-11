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
        private System.Windows.Forms.Timer prankIntervalTimer;
        private System.Windows.Forms.Timer messageDisplayTimer;
        private static readonly Random random = new Random();
        private readonly List<string> trollMessages = new List<string> {
            "Alguns arquivos foram deletados.!. CUIDADO AO BAIXAR ARQUIVOS DA INTERNET",
            "Limpando Cache e Arquivos Temporarios",
            "Detectei teclas sendo precionadas foi você ?",
            "Sua inscrição no Xvideos expirou.!.",
            "Procurando cartões para efetuar compras.!.",
            "O Mouse se mexeu ? Foi você ?",
            "A enviar o seus dados para PRF.!."
        };

        // --- COMPONENTES PARA A TRAP DO CLIPPY ---
        private ClippyControl clippy;
        private System.Windows.Forms.Timer clippyAppearanceTimer;
        private readonly List<string> clippyMessages = new List<string> {
            "Parece que você está a tentar trabalhar. Quer ajuda com isso?",
            "Uma dica: mover o mouse aleatoriamente não resolve seus problemas.",
            "Detetei uma falha na cadeira. Por favor, contacte o suporte.",
            "Posso sugerir uma pausa? Você parece cansado.",
            "Não se esqueça de piscar os olhos. É importante."
        };

        // --- COMPONENTES PARA O MOUSE TRAP ---
        private System.Windows.Forms.Timer mouseMonitorTimer;
        private System.Windows.Forms.Timer mouseTrapTimer;
        private System.Windows.Forms.Timer randomMouseTrapTimer; // Novo timer para ativação aleatória
        private Rectangle trollArea;
        private int trapCounter;
        private bool drawTrapBorder = false;
        private Point mouseTargetPosition;
        private Queue<Point> mousePath = new Queue<Point>();
        private bool isJiggling = true;

        // --- API DO WINDOWS ---
        [DllImport("user32.dll")] private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")] private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll", SetLastError = true)] private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        [DllImport("user32.dll", SetLastError = true)] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)] private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        // Modifiers
        private const uint MOD_CTRL_ALT_SHIFT = 0x0001 | 0x0002 | 0x0004;
        private const int HOTKEY_ID = 1;
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const int LWA_COLORKEY = 0x1;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.Bounds.Size;
            this.TopMost = true;
            this.BackColor = Color.Magenta;
            this.Paint += Form1_Paint;
            this.Opacity = 0.0;

            InitializeTrollComponents();
            InitializeMouseTrap();
            InitializeClippyTrap();

            try
            {
                SetLayeredWindowAttributes(this.Handle, (uint)ColorTranslator.ToWin32(this.BackColor), 0, LWA_COLORKEY);
            }
            catch (Exception ex)
            {
                Log("SetLayeredWindowAttributes falhou: " + ex.Message);
            }

            try
            {
                int initialStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
                SetWindowLong(this.Handle, GWL_EXSTYLE, initialStyle | WS_EX_TRANSPARENT);
            }
            catch (Exception ex)
            {
                Log("SetWindowLong inicial falhou: " + ex.Message);
            }

            bool hotOk = RegisterHotKey(this.Handle, HOTKEY_ID, MOD_CTRL_ALT_SHIFT, (uint)Keys.K);
            if (!hotOk) Log("Falha ao registrar hotkey.");

            this.FormClosing += Form1_FormClosing;
            this.Opacity = 1.0;
        }

        private void InitializeClippyTrap()
        {
            clippy = new ClippyControl { Visible = false };
            clippy.Location = new Point(10, 10);
            this.Controls.Add(clippy);
            clippy.VisibleChanged += OnClippyVisibilityChanged;

            clippyAppearanceTimer = new System.Windows.Forms.Timer();
            clippyAppearanceTimer.Tick += (s, e) =>
            {
                clippyAppearanceTimer.Interval = random.Next(15000, 30000);
                TryShowClippy();
            };
            clippyAppearanceTimer.Interval = random.Next(15000, 30000);
            clippyAppearanceTimer.Start();
        }

        private void TryShowClippy()
        {
            if (this.IsDisposed || !this.IsHandleCreated || clippy == null) return;
            string message = clippyMessages[random.Next(clippyMessages.Count)];
            if (!clippy.Visible) clippy.StartAnimation(message);
        }

        private void OnClippyVisibilityChanged(object sender, EventArgs e)
        {
            try
            {
                int currentStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
                if (clippy.Visible)
                {
                    SetWindowLong(this.Handle, GWL_EXSTYLE, currentStyle & ~WS_EX_TRANSPARENT);
                }
                else
                {
                    SetWindowLong(this.Handle, GWL_EXSTYLE, currentStyle | WS_EX_TRANSPARENT);
                }
            }
            catch (Exception ex)
            {
                Log("OnClippyVisibilityChanged falhou: " + ex.Message);
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

            prankIntervalTimer = new System.Windows.Forms.Timer();
            prankIntervalTimer.Interval = 10000;
            prankIntervalTimer.Tick += (s, e) => ShowRandomMessage();
            prankIntervalTimer.Start();

            messageDisplayTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            messageDisplayTimer.Tick += OnMessageDisplayTick;
        }

        private void InitializeMouseTrap()
        {
            Rectangle primary = Screen.PrimaryScreen.Bounds;
            trollArea = new Rectangle(primary.Width - 200, 0, 200, 200);

            mouseMonitorTimer = new System.Windows.Forms.Timer();
            mouseMonitorTimer.Interval = 100;
            mouseMonitorTimer.Tick += OnMouseMonitorTick;
            mouseMonitorTimer.Start();

            mouseTrapTimer = new System.Windows.Forms.Timer();
            mouseTrapTimer.Interval = 30;
            mouseTrapTimer.Tick += OnMouseTrapTick;

            // Configuração do novo timer para ativação aleatória
            randomMouseTrapTimer = new System.Windows.Forms.Timer();
            randomMouseTrapTimer.Tick += OnRandomMouseTrapTick;
            randomMouseTrapTimer.Interval = random.Next(20000, 40000); // Entre 20 e 40 segundos
            randomMouseTrapTimer.Start();
        }

        private void ActivateMouseTrap(bool isRandomEvent)
        {
            // Só ativa se não houver outra trap do mouse em andamento
            if (mouseTrapTimer.Enabled) return;

            if (isRandomEvent)
            {
                drawTrapBorder = false; // Sem borda para eventos aleatórios
            }
            else
            {
                drawTrapBorder = true;
                this.Invalidate();
            }

            mouseMonitorTimer.Stop();
            randomMouseTrapTimer.Stop(); // Pausa o timer aleatório enquanto a trap está ativa

            isJiggling = random.Next(0, 2) == 0;
            trapCounter = 0;

            if (!isJiggling)
            {
                mousePath.Clear();
                Rectangle virtualBounds = SystemInformation.VirtualScreen;
                for (int i = 0; i < random.Next(3, 6); i++)
                {
                    mousePath.Enqueue(new Point(
                        random.Next(virtualBounds.Left, virtualBounds.Right),
                        random.Next(virtualBounds.Top, virtualBounds.Bottom)
                    ));
                }
                if (mousePath.Count > 0)
                {
                    mouseTargetPosition = mousePath.Dequeue();
                }
            }

            mouseTrapTimer.Start();
        }

        private void OnRandomMouseTrapTick(object sender, EventArgs e)
        {
            // Define o próximo intervalo aleatório
            randomMouseTrapTimer.Interval = random.Next(20000, 40000);
            ActivateMouseTrap(true); // Ativa a trap como um evento aleatório
        }

        private void OnMouseMonitorTick(object sender, EventArgs e)
        {
            Point cursorPos = Cursor.Position;
            if (trollArea.Contains(cursorPos))
            {
                ActivateMouseTrap(false); // Ativa a trap pela trollArea
            }
        }

        private void OnMouseTrapTick(object sender, EventArgs e)
        {
            trapCounter++;
            if (isJiggling)
            {
                if (trapCounter > 100)
                {
                    drawTrapBorder = false;
                    this.Invalidate();
                    mouseTrapTimer.Stop();
                    mouseMonitorTimer.Start();
                    randomMouseTrapTimer.Start(); // Reativa o timer aleatório
                    return;
                }

                int offsetX = random.Next(-25, 26);
                int offsetY = random.Next(-25, 26);
                Rectangle virtualBounds = SystemInformation.VirtualScreen;
                Point current = Cursor.Position;
                Point newPosition = new Point(
                    Math.Max(virtualBounds.Left, Math.Min(virtualBounds.Right - 1, current.X + offsetX)),
                    Math.Max(virtualBounds.Top, Math.Min(virtualBounds.Bottom - 1, current.Y + offsetY))
                );
                Cursor.Position = newPosition;
            }
            else
            {
                PointF currentPos = Cursor.Position;
                float dx = mouseTargetPosition.X - currentPos.X;
                float dy = mouseTargetPosition.Y - currentPos.Y;
                float distance = (float)Math.Sqrt(dx * dx + dy * dy);
                float speed = 25f;

                if (distance < speed)
                {
                    Cursor.Position = mouseTargetPosition;
                    if (mousePath.Count > 0)
                    {
                        mouseTargetPosition = mousePath.Dequeue();
                    }
                    else
                    {
                        drawTrapBorder = false;
                        this.Invalidate();
                        mouseTrapTimer.Stop();
                        mouseMonitorTimer.Start();
                        randomMouseTrapTimer.Start(); // Reativa o timer aleatório
                        return;
                    }
                }
                else
                {
                    Point newPosition = new Point(
                        (int)(currentPos.X + (dx / distance) * speed),
                        (int)(currentPos.Y + (dy / distance) * speed)
                    );
                    Cursor.Position = newPosition;
                }
            }
        }

        private void ShowRandomMessage()
        {
            if (messageDisplayTimer.Enabled) return;

            try
            {
                int currentStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
                SetWindowLong(this.Handle, GWL_EXSTYLE, currentStyle & ~WS_EX_TRANSPARENT);
            }
            catch (Exception ex)
            {
                Log("ShowRandomMessage - SetWindowLong falhou: " + ex.Message);
            }

            trollLabel.Text = trollMessages[random.Next(trollMessages.Count)];
            trollLabel.Visible = true;
            trollLabel.PerformLayout();

            int maxX = Math.Max(0, this.Width - trollLabel.Width);
            int maxY = Math.Max(0, this.Height - trollLabel.Height);
            trollLabel.Location = new Point(random.Next(0, Math.Max(1, maxX)), random.Next(0, Math.Max(1, maxY)));

            messageDisplayTimer.Start();
        }

        private void OnMessageDisplayTick(object sender, EventArgs e)
        {
            trollLabel.Visible = false;
            messageDisplayTimer.Stop();

            try
            {
                int currentStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
                SetWindowLong(this.Handle, GWL_EXSTYLE, currentStyle | WS_EX_TRANSPARENT);
            }
            catch (Exception ex)
            {
                Log("OnMessageDisplayTick - SetWindowLong falhou: " + ex.Message);
            }
        }

        public void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (drawTrapBorder)
            {
                using (Pen borderPen = new Pen(Color.Yellow, 4))
                {
                    e.Graphics.DrawRectangle(borderPen, trollArea);
                }
            }
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
            const int WM_HOTKEY = 0x0312;
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID)
            {
                Application.Exit();
                return;
            }

            base.WndProc(ref m);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { UnregisterHotKey(this.Handle, HOTKEY_ID); } catch { }
            StopAndDisposeTimers();
        }

        private void StopAndDisposeTimers()
        {
            try
            {
                prankIntervalTimer?.Stop();
                prankIntervalTimer?.Dispose();

                messageDisplayTimer?.Stop();
                messageDisplayTimer?.Dispose();

                mouseMonitorTimer?.Stop();
                mouseMonitorTimer?.Dispose();

                mouseTrapTimer?.Stop();
                mouseTrapTimer?.Dispose();

                clippyAppearanceTimer?.Stop();
                clippyAppearanceTimer?.Dispose();

                randomMouseTrapTimer?.Stop(); // Para o novo timer
                randomMouseTrapTimer?.Dispose();
            }
            catch (Exception ex)
            {
                Log("StopAndDisposeTimers falhou: " + ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopAndDisposeTimers();
                clippy?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}