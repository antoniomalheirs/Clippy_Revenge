using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinSystemHelperF
{
    public partial class ClippyControl : UserControl
    {
        // Estados da animação
        private enum ClippyState { Hidden, Entering, Wandering, Exiting }
        private ClippyState currentState = ClippyState.Hidden;

        // Componentes da animação
        private Timer animationTimer = new Timer();
        private Random random = new Random();
        private PointF currentPosition;
        private Point targetPosition;
        private Point entryPoint;
        private float speed = 5f;
        private int wanderCount = 0;

        // --- LÓGICA DE CAPTURA DO CURSOR ---
        private bool isCursorTrapped = false; // Flag que controla a armadilha

        // Imagens para a inversão do sprite
        private Image imageRight;
        private Image imageLeft;
        private bool isFacingRight = true;

        public ClippyControl()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.SupportsTransparentBackColor,
                          true);

            this.BackColor = Color.Magenta;

            pnlBubble.BackColor = Color.Transparent;
            picClippy.BackColor = Color.Transparent;
            lblMessage.BackColor = Color.Transparent;

            imageRight = this.pnlBubble.BackgroundImage;
            if (imageRight != null)
            {
                imageLeft = (Image)imageRight.Clone();
                imageLeft.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }

            animationTimer.Interval = 30;
            animationTimer.Tick += OnAnimationTick;
        }

        // Método principal para iniciar a animação
        public void StartAnimation(string message)
        {
            if (currentState != ClippyState.Hidden) return;

            lblMessage.Text = message;
            PlanNewPath();
            this.Location = new Point((int)currentPosition.X, (int)currentPosition.Y);
            this.Visible = true;
            this.BringToFront();
            currentState = ClippyState.Entering;
            animationTimer.Start();
        }

        // O "game loop" que move o personagem
        private void OnAnimationTick(object sender, EventArgs e)
        {
            // --- NOVA LÓGICA DE CAPTURA POR PROXIMIDADE ---
            // Verifica se o rato passou por cima, mas apenas se estiver a passear
            Rectangle controlBoundsOnScreen = this.RectangleToScreen(this.ClientRectangle);
            if (currentState == ClippyState.Wandering && controlBoundsOnScreen.Contains(Cursor.Position))
            {
                // Ativa a armadilha e inicia a fuga!
                isCursorTrapped = true;
                currentState = ClippyState.Exiting;
                targetPosition = entryPoint;
            }

            // Se a armadilha estiver ativa, prende o cursor
            if (isCursorTrapped)
            {
                ProcessCursorTrap();
            }

            // Lógica de Movimento
            float dx = targetPosition.X - currentPosition.X;
            float dy = targetPosition.Y - currentPosition.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            UpdateSpriteDirection(dx);

            if (distance < speed)
            {
                ProcessTargetReached();
            }
            else
            {
                currentPosition.X += (dx / distance) * speed;
                currentPosition.Y += (dy / distance) * speed;
            }

            this.Location = new Point((int)currentPosition.X, (int)currentPosition.Y);
        }

        // Decide o que fazer quando chega a um ponto de destino
        private void ProcessTargetReached()
        {
            var screen = Screen.FromControl(this.Parent).Bounds;
            currentPosition = targetPosition;

            switch (currentState)
            {
                case ClippyState.Entering:
                    currentState = ClippyState.Wandering;
                    wanderCount = 0;
                    targetPosition = new Point(random.Next(50, screen.Width - this.Width - 50), random.Next(50, screen.Height - this.Height - 50));
                    break;

                case ClippyState.Wandering:
                    wanderCount++;
                    if (wanderCount >= 3)
                    {
                        currentState = ClippyState.Exiting;
                        targetPosition = entryPoint;
                    }
                    else
                    {
                        targetPosition = new Point(random.Next(50, screen.Width - this.Width - 50), random.Next(50, screen.Height - this.Height - 50));
                    }
                    break;

                    // Ao chegar ao destino de saída, desliga tudo e liberta o rato
                case ClippyState.Exiting:
                    isCursorTrapped = false; // <-- LIBERTA O CURSOR
                    currentState = ClippyState.Hidden;
                    this.Visible = false;
                    animationTimer.Stop();
                    break;
            }
        }

        // --- NOVO MÉTODO PARA PRENDER O CURSOR ---
        private void ProcessCursorTrap()
        {
            // Prende o cursor ao centro do pnlBubble (o personagem)
            Cursor.Position = new Point(this.Left + pnlBubble.Left + pnlBubble.Width / 2, this.Top + pnlBubble.Top + pnlBubble.Height / 2);
        }

        // Atualiza a imagem (esquerda/direita) e a posição do balão
        private void UpdateSpriteDirection(float dx)
        {
            if (imageRight == null || imageLeft == null) return;

            if (dx > 0.1 && !isFacingRight)
            {
                isFacingRight = true;
                pnlBubble.BackgroundImage = imageRight;
                picClippy.Left = pnlBubble.Left - picClippy.Width + 40;
            }
            else if (dx < -0.1 && isFacingRight)
            {
                isFacingRight = false;
                pnlBubble.BackgroundImage = imageLeft;
                picClippy.Left = pnlBubble.Right - 40;
            }
        }

        // Escolhe um ponto de entrada/saída aleatório
        private void PlanNewPath()
        {
            int edge = random.Next(4);
            var screen = Screen.FromControl(this.Parent).Bounds;
            switch (edge)
            {
                case 0: // Cima
                    entryPoint = new Point(random.Next(0, screen.Width - this.Width), -this.Height);
                    targetPosition = new Point(entryPoint.X, 10);
                    break;
                case 1: // Direita
                    entryPoint = new Point(screen.Width, random.Next(0, screen.Height - this.Height));
                    targetPosition = new Point(screen.Width - this.Width - 10, entryPoint.Y);
                    break;
                case 2: // Baixo
                    entryPoint = new Point(random.Next(0, screen.Width - this.Width), screen.Height);
                    targetPosition = new Point(entryPoint.X, screen.Height - this.Height - 10);
                    break;
                case 3: // Esquerda
                    entryPoint = new Point(-this.Width, random.Next(0, screen.Height - this.Height));
                    targetPosition = new Point(10, entryPoint.Y);
                    break;
            }
            currentPosition = entryPoint;
        }
    }
}