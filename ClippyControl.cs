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
        private float speed = 5f;
        private int wanderCount = 0;
        private bool isCursorTrapped = false;

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
            //this.TransparencyKey = Color.Magenta;

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

            this.MouseDown += OnClippyMouseDown;
            pnlBubble.MouseDown += OnClippyMouseDown;
            picClippy.MouseDown += OnClippyMouseDown;
            lblMessage.MouseDown += OnClippyMouseDown;
        }

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

        private void OnClippyMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks >= 1 && currentState == ClippyState.Wandering)
            {
                isCursorTrapped = true;
                currentState = ClippyState.Exiting;
                targetPosition = entryPoint;
            }
        }

        private void OnAnimationTick(object sender, EventArgs e)
        {
            if (isCursorTrapped)
            {
                ProcessCursorTrap();
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
            }

            this.Location = new Point((int)currentPosition.X, (int)currentPosition.Y);
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
                    targetPosition = new Point(random.Next(50, screen.Width - this.Width - 50), random.Next(50, screen.Height - this.Height - 50));
                    break;
                case ClippyState.Wandering:
                    wanderCount++;
                    if (wanderCount >= 6)
                    {
                        currentState = ClippyState.Exiting;
                        targetPosition = entryPoint;
                        currentState = ClippyState.Hidden;
                    }
                    else
                    {
                        targetPosition = new Point(random.Next(50, screen.Width - this.Width - 50), random.Next(50, screen.Height - this.Height - 50));
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

        private void ProcessCursorTrap()
        {
            Cursor.Position = new Point(this.Left + pnlBubble.Left + pnlBubble.Width / 2, this.Top + pnlBubble.Top + pnlBubble.Height / 2);
        }

        private void UpdateSpriteDirection(float dx)
        {
            if (imageRight == null || imageLeft == null) return;
            if (dx > 0.1 && !isFacingRight)
            {
                isFacingRight = true;
                pnlBubble.BackgroundImage = imageRight;
                picClippy.Left = pnlBubble.Left - picClippy.Width + 215;
            }
            else if (dx < -0.1 && isFacingRight)
            {
                isFacingRight = false;
                pnlBubble.BackgroundImage = imageLeft;
                picClippy.Left = pnlBubble.Right - 305;
            }
        }

        private void PlanNewPath()
        {
            int edge = random.Next(4);
            var screen = Screen.FromControl(this.Parent).Bounds;
            switch (edge)
            {
                case 0:
                    entryPoint = new Point(random.Next(0, screen.Width - this.Width), -this.Height);
                    targetPosition = new Point(entryPoint.X, 10);
                    break;
                case 1:
                    entryPoint = new Point(screen.Width, random.Next(0, screen.Height - this.Height));
                    targetPosition = new Point(screen.Width - this.Width - 10, entryPoint.Y);
                    break;
                case 2:
                    entryPoint = new Point(random.Next(0, screen.Width - this.Width), screen.Height);
                    targetPosition = new Point(entryPoint.X, screen.Height - this.Height - 10);
                    break;
                case 3:
                    entryPoint = new Point(-this.Width, random.Next(0, screen.Height - this.Height));
                    targetPosition = new Point(10, entryPoint.Y);
                    break;
            }
            currentPosition = entryPoint;
        }

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }
    }
}