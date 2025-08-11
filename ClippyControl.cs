using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinSystemHelperF
{
    public partial class ClippyControl : UserControl
    {
        private enum ClippyState { Hidden, Entering, Wandering, Exiting }
        private ClippyState currentState = ClippyState.Hidden;

        private Timer animationTimer = new Timer();
        private Random random = new Random();
        private PointF currentPosition;
        private Point targetPosition;
        private Point entryPoint;
        private float speed = 2.5f;
        private int wanderCount = 0;
        private bool isCursorTrapped = false;

        private Image imageRight;
        private Image imageLeft;
        private bool isFacingRight = true;

        public ClippyControl()
        {
            InitializeComponent();

            // Configura para melhor desenho e transparência
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.SupportsTransparentBackColor,
                          true);

            this.BackColor = Color.Magenta; // Cor-chave para transparência

            // Ajusta cores dos controles internos para evitar problema no clique
            pnlBubble.BackColor = Color.LightYellow;
            picClippy.BackColor = Color.Magenta;
            lblMessage.BackColor = Color.LightYellow;

            // Clona imagem para inversão horizontal, usando as imagens já definidas no designer
            imageRight = picClippy.Image;
            if (imageRight != null)
            {
                imageLeft = (Image)imageRight.Clone();
                imageLeft.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }


            animationTimer.Interval = 30;
            animationTimer.Tick += OnAnimationTick;

            // Inscreve clique para toda área (inclusive filhos)
            AddClickHandlers(this);
        }
        private void AddClickHandlers(Control control)
        {
            control.Click += OnClippyClick;
            foreach (Control child in control.Controls)
            {
                AddClickHandlers(child);
            }
        }
        private void OnClippyClick(object sender, EventArgs e)
        {
            if (currentState == ClippyState.Wandering)
            {
                isCursorTrapped = true;
                currentState = ClippyState.Exiting;
                targetPosition = entryPoint;
            }
        }
        public void StartAnimation(string message)
        {
            if (currentState != ClippyState.Hidden) return;

            lblMessage.Text = message;
            PlanNewPath();
            this.Location = Point.Round(currentPosition);
            this.Visible = true;
            this.BringToFront();
            currentState = ClippyState.Entering;
            animationTimer.Start();
        }
        private void OnAnimationTick(object sender, EventArgs e)
        {
            if (isCursorTrapped)
            {
                // Trava o cursor na bolha durante saída
                Point trapPosition = new Point(
                    this.Left + pnlBubble.Left + pnlBubble.Width / 2,
                    this.Top + pnlBubble.Top + pnlBubble.Height / 2);
                Cursor.Position = trapPosition;
            }

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
                this.Location = Point.Round(currentPosition);
            }
        }
        private void ProcessTargetReached()
        {
            var screen = Screen.FromControl(this.Parent).Bounds;
            currentPosition = targetPosition;

            switch (currentState)
            {
                case ClippyState.Entering:
                    currentState = ClippyState.Wandering;
                    wanderCount = 0;
                    SetRandomWanderTarget(screen);
                    break;

                case ClippyState.Wandering:
                    wanderCount++;
                    if (wanderCount >= 6)
                    {
                        currentState = ClippyState.Exiting;
                        targetPosition = entryPoint;
                    }
                    else
                    {
                        SetRandomWanderTarget(screen);
                    }
                    break;

                case ClippyState.Exiting:
                    isCursorTrapped = false;
                    currentState = ClippyState.Hidden;
                    this.Visible = false;
                    animationTimer.Stop();
                    break;
            }
        }
        private void SetRandomWanderTarget(Rectangle screen)
        {
            targetPosition = new Point(
                random.Next(50, screen.Width - this.Width - 50),
                random.Next(50, screen.Height - this.Height - 50));
        }
        private void UpdateSpriteDirection(float dx)
        {
            if (picClippy.Image == null) return;

            if (dx > 0.1f && !isFacingRight)
            {
                isFacingRight = true;
                picClippy.Image = imageRight;
            }
            else if (dx < -0.1f && isFacingRight)
            {
                isFacingRight = false;
                picClippy.Image = imageLeft;
            }
        }
        private void PlanNewPath()
        {
            int edge = random.Next(4);
            var screen = Screen.FromControl(this.Parent).Bounds;

            switch (edge)
            {
                case 0: // Entrando de cima
                    entryPoint = new Point(random.Next(0, screen.Width - this.Width), -this.Height);
                    targetPosition = new Point(entryPoint.X, 10);
                    break;
                case 1: // Entrando pela direita
                    entryPoint = new Point(screen.Width, random.Next(0, screen.Height - this.Height));
                    targetPosition = new Point(screen.Width - this.Width - 10, entryPoint.Y);
                    break;
                case 2: // Entrando por baixo
                    entryPoint = new Point(random.Next(0, screen.Width - this.Width), screen.Height);
                    targetPosition = new Point(entryPoint.X, screen.Height - this.Height - 10);
                    break;
                case 3: // Entrando pela esquerda
                    entryPoint = new Point(-this.Width, random.Next(0, screen.Height - this.Height));
                    targetPosition = new Point(10, entryPoint.Y);
                    break;
            }
            currentPosition = entryPoint;
        }
        private void lblMessage_Click(object sender, EventArgs e)
        {
            if (currentState == ClippyState.Wandering)
            {
                isCursorTrapped = true;
                currentState = ClippyState.Exiting;
                targetPosition = entryPoint;
            }
        }
        private void picClippy_Click(object sender, EventArgs e)
        {
            if (currentState == ClippyState.Wandering)
            {
                isCursorTrapped = true;
                currentState = ClippyState.Exiting;
                targetPosition = entryPoint;
            }
        }
    }
}
