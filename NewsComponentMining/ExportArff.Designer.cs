namespace NewsComponentMining
{
    partial class ExportArff
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
            this.tbFrom = new System.Windows.Forms.TextBox();
            this.tbTo = new System.Windows.Forms.TextBox();
            this.tbType = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bExport = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbComponentType = new System.Windows.Forms.TextBox();
            this.bExport2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbFrom
            // 
            this.tbFrom.Location = new System.Drawing.Point(102, 46);
            this.tbFrom.Name = "tbFrom";
            this.tbFrom.Size = new System.Drawing.Size(100, 22);
            this.tbFrom.TabIndex = 0;
            // 
            // tbTo
            // 
            this.tbTo.Location = new System.Drawing.Point(102, 85);
            this.tbTo.Name = "tbTo";
            this.tbTo.Size = new System.Drawing.Size(100, 22);
            this.tbTo.TabIndex = 1;
            // 
            // tbType
            // 
            this.tbType.Location = new System.Drawing.Point(102, 130);
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(100, 22);
            this.tbType.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "From";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "To";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "NewsType";
            // 
            // bExport
            // 
            this.bExport.Location = new System.Drawing.Point(112, 213);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(75, 23);
            this.bExport.TabIndex = 8;
            this.bExport.Text = "Export";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "ComponentType";
            // 
            // tbComponentType
            // 
            this.tbComponentType.Location = new System.Drawing.Point(102, 173);
            this.tbComponentType.Name = "tbComponentType";
            this.tbComponentType.Size = new System.Drawing.Size(100, 22);
            this.tbComponentType.TabIndex = 7;
            // 
            // bExport2
            // 
            this.bExport2.Location = new System.Drawing.Point(223, 213);
            this.bExport2.Name = "bExport2";
            this.bExport2.Size = new System.Drawing.Size(75, 23);
            this.bExport2.TabIndex = 9;
            this.bExport2.Text = "Export 2";
            this.bExport2.UseVisualStyleBackColor = true;
            this.bExport2.Click += new System.EventHandler(this.bExport2_Click);
            // 
            // ExportArff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 376);
            this.Controls.Add(this.bExport2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbComponentType);
            this.Controls.Add(this.bExport);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbType);
            this.Controls.Add(this.tbTo);
            this.Controls.Add(this.tbFrom);
            this.Name = "ExportArff";
            this.Text = "ExportArff";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbFrom;
        private System.Windows.Forms.TextBox tbTo;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbComponentType;
        private System.Windows.Forms.Button bExport2;
    }
}