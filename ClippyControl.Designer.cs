namespace WinSystemHelperF
{
    partial class ClippyControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClippyControl));
            this.pnlBubble = new System.Windows.Forms.Panel();
            this.picClippy = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.picClippy.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBubble
            // 
            this.pnlBubble.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlBubble.BackgroundImage")));
            this.pnlBubble.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnlBubble.Location = new System.Drawing.Point(115, 94);
            this.pnlBubble.Name = "pnlBubble";
            this.pnlBubble.Size = new System.Drawing.Size(150, 150);
            this.pnlBubble.TabIndex = 0;
            this.pnlBubble.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnClippyMouseDown);
            // 
            // picClippy
            // 
            this.picClippy.Controls.Add(this.lblMessage);
            this.picClippy.Location = new System.Drawing.Point(5, 13);
            this.picClippy.Name = "picClippy";
            this.picClippy.Size = new System.Drawing.Size(260, 120);
            this.picClippy.TabIndex = 1;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(32, 27);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Padding = new System.Windows.Forms.Padding(5);
            this.lblMessage.Size = new System.Drawing.Size(196, 66);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Mensagem de teste.";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMessage.Click += new System.EventHandler(this.lblMessage_Click);
            this.lblMessage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnClippyMouseDown);
            // 
            // ClippyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picClippy);
            this.Controls.Add(this.pnlBubble);
            this.Name = "ClippyControl";
            this.Size = new System.Drawing.Size(271, 250);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnClippyMouseDown);
            this.picClippy.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBubble;
        private System.Windows.Forms.Panel picClippy;
        private System.Windows.Forms.Label lblMessage;
    }
}