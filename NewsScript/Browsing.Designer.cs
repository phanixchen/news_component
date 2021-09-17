namespace NewsScript
{
    partial class Browsing
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgnews = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lNewsid = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tbUserAdded = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPOSKeyword = new System.Windows.Forms.TextBox();
            this.tbPOS = new System.Windows.Forms.TextBox();
            this.tbNews = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgnews)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Font = new System.Drawing.Font("新細明體", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(882, 481);
            this.tabControl1.TabIndex = 8;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgnews);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(874, 453);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "檢視新聞";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgnews
            // 
            this.dgnews.AllowUserToAddRows = false;
            this.dgnews.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgnews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgnews.Location = new System.Drawing.Point(3, 3);
            this.dgnews.Name = "dgnews";
            this.dgnews.ReadOnly = true;
            this.dgnews.RowTemplate.Height = 24;
            this.dgnews.Size = new System.Drawing.Size(868, 447);
            this.dgnews.TabIndex = 0;
            this.dgnews.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgnews_CellMouseClick);
            this.dgnews.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgnews_CellMouseDoubleClick);
            this.dgnews.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgnews_KeyDown);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lNewsid);
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Controls.Add(this.tbUserAdded);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.tbPOSKeyword);
            this.tabPage3.Controls.Add(this.tbPOS);
            this.tabPage3.Controls.Add(this.tbNews);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(874, 453);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "關鍵字";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lNewsid
            // 
            this.lNewsid.AutoSize = true;
            this.lNewsid.Location = new System.Drawing.Point(76, 6);
            this.lNewsid.Name = "lNewsid";
            this.lNewsid.Size = new System.Drawing.Size(0, 15);
            this.lNewsid.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(523, 423);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 27);
            this.button1.TabIndex = 6;
            this.button1.Text = "儲存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbUserAdded
            // 
            this.tbUserAdded.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tbUserAdded.Location = new System.Drawing.Point(684, 28);
            this.tbUserAdded.Multiline = true;
            this.tbUserAdded.Name = "tbUserAdded";
            this.tbUserAdded.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbUserAdded.Size = new System.Drawing.Size(187, 389);
            this.tbUserAdded.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(681, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "新增關鍵字";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(436, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "切詞關鍵字";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(271, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 3;
            this.label6.Text = "切詞結果";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "新聞原文";
            // 
            // tbPOSKeyword
            // 
            this.tbPOSKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tbPOSKeyword.Location = new System.Drawing.Point(439, 28);
            this.tbPOSKeyword.Multiline = true;
            this.tbPOSKeyword.Name = "tbPOSKeyword";
            this.tbPOSKeyword.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbPOSKeyword.Size = new System.Drawing.Size(239, 389);
            this.tbPOSKeyword.TabIndex = 1;
            // 
            // tbPOS
            // 
            this.tbPOS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tbPOS.Location = new System.Drawing.Point(274, 28);
            this.tbPOS.Multiline = true;
            this.tbPOS.Name = "tbPOS";
            this.tbPOS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbPOS.Size = new System.Drawing.Size(159, 389);
            this.tbPOS.TabIndex = 1;
            // 
            // tbNews
            // 
            this.tbNews.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.tbNews.Location = new System.Drawing.Point(3, 28);
            this.tbNews.Multiline = true;
            this.tbNews.Name = "tbNews";
            this.tbNews.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbNews.Size = new System.Drawing.Size(265, 422);
            this.tbNews.TabIndex = 0;
            // 
            // Browsing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 505);
            this.Controls.Add(this.tabControl1);
            this.Name = "Browsing";
            this.Text = "Browsing";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgnews)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgnews;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lNewsid;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbUserAdded;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPOS;
        private System.Windows.Forms.TextBox tbNews;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPOSKeyword;
    }
}