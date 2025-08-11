using System.Windows.Forms;

namespace WinSystemHelperF
{
    partial class ClippyControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlBubble;
        private System.Windows.Forms.PictureBox picClippy;
        private System.Windows.Forms.Label lblMessage;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClippyControl));
            this.pnlBubble = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.picClippy = new System.Windows.Forms.PictureBox();
            this.pnlBubble.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClippy)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBubble
            // 
            this.pnlBubble.BackColor = System.Drawing.Color.LightYellow;
            this.pnlBubble.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlBubble.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBubble.Controls.Add(this.lblMessage);
            this.pnlBubble.Location = new System.Drawing.Point(162, 3);
            this.pnlBubble.Name = "pnlBubble";
            this.pnlBubble.Size = new System.Drawing.Size(250, 88);
            this.pnlBubble.TabIndex = 0;
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.Black;
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(248, 86);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "Mensagem do Clippy";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMessage.Click += new System.EventHandler(this.lblMessage_Click);
            // 
            // picClippy
            // 
            this.picClippy.BackColor = System.Drawing.Color.Transparent;
            this.picClippy.Image = ((System.Drawing.Image)(resources.GetObject("picClippy.Image")));
            this.picClippy.Location = new System.Drawing.Point(3, 3);
            this.picClippy.Name = "picClippy";
            this.picClippy.Size = new System.Drawing.Size(153, 87);
            this.picClippy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picClippy.TabIndex = 2;
            this.picClippy.TabStop = false;
            this.picClippy.Click += new System.EventHandler(this.picClippy_Click);
            // 
            // ClippyControl
            // 
            this.BackColor = System.Drawing.Color.Magenta;
            this.Controls.Add(this.pnlBubble);
            this.Controls.Add(this.picClippy);
            this.Name = "ClippyControl";
            this.Size = new System.Drawing.Size(417, 95);
            this.pnlBubble.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picClippy)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
