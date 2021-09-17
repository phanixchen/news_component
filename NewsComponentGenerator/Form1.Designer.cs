namespace NewsComponentGenerator
{
    partial class Form1
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
            this.bAnalyse = new System.Windows.Forms.Button();
            this.tbNews = new System.Windows.Forms.TextBox();
            this.tbP = new System.Windows.Forms.TextBox();
            this.tbT = new System.Windows.Forms.TextBox();
            this.tbS = new System.Windows.Forms.TextBox();
            this.tbI = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbAdvSearch = new System.Windows.Forms.CheckBox();
            this.bAnalyse_2 = new System.Windows.Forms.Button();
            this.bFeedback = new System.Windows.Forms.Button();
            this.tbNewsNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bAnalyse
            // 
            this.bAnalyse.Location = new System.Drawing.Point(449, 9);
            this.bAnalyse.Name = "bAnalyse";
            this.bAnalyse.Size = new System.Drawing.Size(75, 23);
            this.bAnalyse.TabIndex = 0;
            this.bAnalyse.Text = "分析";
            this.bAnalyse.UseVisualStyleBackColor = true;
            this.bAnalyse.Visible = false;
            this.bAnalyse.Click += new System.EventHandler(this.bAnalyse_Click);
            // 
            // tbNews
            // 
            this.tbNews.Location = new System.Drawing.Point(12, 71);
            this.tbNews.Multiline = true;
            this.tbNews.Name = "tbNews";
            this.tbNews.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbNews.Size = new System.Drawing.Size(253, 414);
            this.tbNews.TabIndex = 1;
            // 
            // tbP
            // 
            this.tbP.Location = new System.Drawing.Point(271, 41);
            this.tbP.Multiline = true;
            this.tbP.Name = "tbP";
            this.tbP.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbP.Size = new System.Drawing.Size(253, 203);
            this.tbP.TabIndex = 1;
            // 
            // tbT
            // 
            this.tbT.Location = new System.Drawing.Point(530, 41);
            this.tbT.Multiline = true;
            this.tbT.Name = "tbT";
            this.tbT.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbT.Size = new System.Drawing.Size(253, 203);
            this.tbT.TabIndex = 1;
            // 
            // tbS
            // 
            this.tbS.Location = new System.Drawing.Point(271, 282);
            this.tbS.Multiline = true;
            this.tbS.Name = "tbS";
            this.tbS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbS.Size = new System.Drawing.Size(253, 203);
            this.tbS.TabIndex = 1;
            // 
            // tbI
            // 
            this.tbI.Location = new System.Drawing.Point(530, 282);
            this.tbI.Multiline = true;
            this.tbI.Name = "tbI";
            this.tbI.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbI.Size = new System.Drawing.Size(253, 203);
            this.tbI.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(12, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "新聞原文";
            this.label1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(268, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "人物";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(527, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "交通工具";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(271, 264);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "場景";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(527, 264);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "物件";
            // 
            // cbAdvSearch
            // 
            this.cbAdvSearch.AutoSize = true;
            this.cbAdvSearch.Checked = true;
            this.cbAdvSearch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAdvSearch.Location = new System.Drawing.Point(185, 16);
            this.cbAdvSearch.Name = "cbAdvSearch";
            this.cbAdvSearch.Size = new System.Drawing.Size(81, 16);
            this.cbAdvSearch.TabIndex = 3;
            this.cbAdvSearch.Text = "Adv. Search";
            this.cbAdvSearch.UseVisualStyleBackColor = true;
            // 
            // bAnalyse_2
            // 
            this.bAnalyse_2.Location = new System.Drawing.Point(104, 12);
            this.bAnalyse_2.Name = "bAnalyse_2";
            this.bAnalyse_2.Size = new System.Drawing.Size(75, 23);
            this.bAnalyse_2.TabIndex = 4;
            this.bAnalyse_2.Text = "分析";
            this.bAnalyse_2.UseVisualStyleBackColor = true;
            this.bAnalyse_2.Click += new System.EventHandler(this.bAnalyse_2_Click);
            // 
            // bFeedback
            // 
            this.bFeedback.Location = new System.Drawing.Point(708, 12);
            this.bFeedback.Name = "bFeedback";
            this.bFeedback.Size = new System.Drawing.Size(75, 23);
            this.bFeedback.TabIndex = 5;
            this.bFeedback.Text = "FeedBack";
            this.bFeedback.UseVisualStyleBackColor = true;
            this.bFeedback.Click += new System.EventHandler(this.bFeedback_Click);
            // 
            // tbNewsNo
            // 
            this.tbNewsNo.Location = new System.Drawing.Point(165, 41);
            this.tbNewsNo.Name = "tbNewsNo";
            this.tbNewsNo.Size = new System.Drawing.Size(100, 22);
            this.tbNewsNo.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(104, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "新聞編號:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 497);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbNewsNo);
            this.Controls.Add(this.bFeedback);
            this.Controls.Add(this.bAnalyse_2);
            this.Controls.Add(this.cbAdvSearch);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbI);
            this.Controls.Add(this.tbS);
            this.Controls.Add(this.tbT);
            this.Controls.Add(this.tbP);
            this.Controls.Add(this.tbNews);
            this.Controls.Add(this.bAnalyse);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "新聞物件分析";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bAnalyse;
        private System.Windows.Forms.TextBox tbNews;
        private System.Windows.Forms.TextBox tbP;
        private System.Windows.Forms.TextBox tbT;
        private System.Windows.Forms.TextBox tbS;
        private System.Windows.Forms.TextBox tbI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbAdvSearch;
        private System.Windows.Forms.Button bAnalyse_2;
        private System.Windows.Forms.Button bFeedback;
        private System.Windows.Forms.TextBox tbNewsNo;
        private System.Windows.Forms.Label label6;
    }
}

