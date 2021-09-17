namespace NewsComponentMining
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
            this.bProcessNewsData = new System.Windows.Forms.Button();
            this.bExportArff = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bProcessNewsData
            // 
            this.bProcessNewsData.Location = new System.Drawing.Point(35, 25);
            this.bProcessNewsData.Name = "bProcessNewsData";
            this.bProcessNewsData.Size = new System.Drawing.Size(123, 31);
            this.bProcessNewsData.TabIndex = 0;
            this.bProcessNewsData.Text = "Process News Data";
            this.bProcessNewsData.UseVisualStyleBackColor = true;
            this.bProcessNewsData.Click += new System.EventHandler(this.bProcessNewsData_Click);
            // 
            // bExportArff
            // 
            this.bExportArff.Location = new System.Drawing.Point(368, 183);
            this.bExportArff.Name = "bExportArff";
            this.bExportArff.Size = new System.Drawing.Size(75, 23);
            this.bExportArff.TabIndex = 1;
            this.bExportArff.Text = "ExportArff";
            this.bExportArff.UseVisualStyleBackColor = true;
            this.bExportArff.Click += new System.EventHandler(this.bExportArff_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 463);
            this.Controls.Add(this.bExportArff);
            this.Controls.Add(this.bProcessNewsData);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bProcessNewsData;
        private System.Windows.Forms.Button bExportArff;
    }
}

