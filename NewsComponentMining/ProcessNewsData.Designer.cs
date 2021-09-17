namespace NewsComponentMining
{
    partial class ProcessNewsData
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該公開 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.bBrowse = new System.Windows.Forms.Button();
            this.fChooseNewsFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.tbFolder = new System.Windows.Forms.TextBox();
            this.lFiles = new System.Windows.Forms.ListBox();
            this.tbContent = new System.Windows.Forms.TextBox();
            this.tbP = new System.Windows.Forms.TextBox();
            this.tbI = new System.Windows.Forms.TextBox();
            this.tbT = new System.Windows.Forms.TextBox();
            this.tbS = new System.Windows.Forms.TextBox();
            this.bSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bBrowse
            // 
            this.bBrowse.Location = new System.Drawing.Point(25, 12);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(96, 30);
            this.bBrowse.TabIndex = 0;
            this.bBrowse.Text = "Browse Folder";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // tbFolder
            // 
            this.tbFolder.Location = new System.Drawing.Point(127, 18);
            this.tbFolder.Name = "tbFolder";
            this.tbFolder.ReadOnly = true;
            this.tbFolder.Size = new System.Drawing.Size(522, 22);
            this.tbFolder.TabIndex = 1;
            // 
            // lFiles
            // 
            this.lFiles.FormattingEnabled = true;
            this.lFiles.ItemHeight = 12;
            this.lFiles.Location = new System.Drawing.Point(12, 48);
            this.lFiles.Name = "lFiles";
            this.lFiles.Size = new System.Drawing.Size(120, 196);
            this.lFiles.TabIndex = 2;
            this.lFiles.SelectedIndexChanged += new System.EventHandler(this.lFiles_SelectedIndexChanged);
            // 
            // tbContent
            // 
            this.tbContent.Location = new System.Drawing.Point(465, 48);
            this.tbContent.Multiline = true;
            this.tbContent.Name = "tbContent";
            this.tbContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbContent.Size = new System.Drawing.Size(248, 398);
            this.tbContent.TabIndex = 3;
            this.tbContent.WordWrap = false;
            // 
            // tbP
            // 
            this.tbP.Location = new System.Drawing.Point(138, 48);
            this.tbP.Multiline = true;
            this.tbP.Name = "tbP";
            this.tbP.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbP.Size = new System.Drawing.Size(154, 196);
            this.tbP.TabIndex = 4;
            this.tbP.WordWrap = false;
            // 
            // tbI
            // 
            this.tbI.Location = new System.Drawing.Point(298, 48);
            this.tbI.Multiline = true;
            this.tbI.Name = "tbI";
            this.tbI.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbI.Size = new System.Drawing.Size(154, 196);
            this.tbI.TabIndex = 5;
            this.tbI.WordWrap = false;
            // 
            // tbT
            // 
            this.tbT.Location = new System.Drawing.Point(138, 250);
            this.tbT.Multiline = true;
            this.tbT.Name = "tbT";
            this.tbT.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbT.Size = new System.Drawing.Size(154, 196);
            this.tbT.TabIndex = 6;
            this.tbT.WordWrap = false;
            // 
            // tbS
            // 
            this.tbS.Location = new System.Drawing.Point(298, 250);
            this.tbS.Multiline = true;
            this.tbS.Name = "tbS";
            this.tbS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbS.Size = new System.Drawing.Size(154, 196);
            this.tbS.TabIndex = 6;
            this.tbS.WordWrap = false;
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(25, 416);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(96, 30);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "Save";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // ProcessNewsData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 459);
            this.Controls.Add(this.tbS);
            this.Controls.Add(this.tbT);
            this.Controls.Add(this.tbI);
            this.Controls.Add(this.tbP);
            this.Controls.Add(this.tbContent);
            this.Controls.Add(this.lFiles);
            this.Controls.Add(this.tbFolder);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.bBrowse);
            this.Name = "ProcessNewsData";
            this.Text = "ProcessNewsData";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.FolderBrowserDialog fChooseNewsFolder;
        private System.Windows.Forms.TextBox tbFolder;
        private System.Windows.Forms.ListBox lFiles;
        private System.Windows.Forms.TextBox tbContent;
        private System.Windows.Forms.TextBox tbP;
        private System.Windows.Forms.TextBox tbI;
        private System.Windows.Forms.TextBox tbT;
        private System.Windows.Forms.TextBox tbS;
        private System.Windows.Forms.Button bSave;
    }
}