namespace ProxyAndroid
{
    partial class InitWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitWindow));
            this.tbInitInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbInitInfo
            // 
            this.tbInitInfo.Location = new System.Drawing.Point(56, 28);
            this.tbInitInfo.Multiline = true;
            this.tbInitInfo.Name = "tbInitInfo";
            this.tbInitInfo.ReadOnly = true;
            this.tbInitInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbInitInfo.Size = new System.Drawing.Size(299, 86);
            this.tbInitInfo.TabIndex = 0;
            // 
            // InitWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 141);
            this.Controls.Add(this.tbInitInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InitWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "初始化";
            this.Load += new System.EventHandler(this.InitWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbInitInfo;
    }
}