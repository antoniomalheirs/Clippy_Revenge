namespace WinSystemHelperF
{
    partial class ClippyControl
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClippyControl));
            this.pnlBubble = new System.Windows.Forms.PictureBox();
            this.picClippy = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBubble)).BeginInit();
            this.picClippy.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBubble
            // 
            this.pnlBubble.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlBubble.BackgroundImage")));
            this.pnlBubble.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlBubble.Location = new System.Drawing.Point(37, 78);
            this.pnlBubble.Name = "pnlBubble";
            this.pnlBubble.Size = new System.Drawing.Size(150, 87);
            this.pnlBubble.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pnlBubble.TabIndex = 0;
            this.pnlBubble.TabStop = false;
            // 
            // picClippy
            // 
            this.picClippy.Controls.Add(this.lblMessage);
            this.picClippy.Location = new System.Drawing.Point(1, 2);
            this.picClippy.Name = "picClippy";
            this.picClippy.Size = new System.Drawing.Size(186, 70);
            this.picClippy.TabIndex = 1;
            // 
            // lblMessage
            // 
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(186, 70);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "label1";
            // 
            // ClippyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Magenta;
            this.Controls.Add(this.picClippy);
            this.Controls.Add(this.pnlBubble);
            this.Name = "ClippyControl";
            this.Size = new System.Drawing.Size(189, 167);
            ((System.ComponentModel.ISupportInitialize)(this.pnlBubble)).EndInit();
            this.picClippy.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pnlBubble;
        private System.Windows.Forms.Panel picClippy;
        private System.Windows.Forms.Label lblMessage;
    }
}
