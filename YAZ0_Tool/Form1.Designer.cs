namespace YAZ0_Tool
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.YAZ0_CompressLevel_TXT = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.YAZ0_SearchRangeValue_TXT = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "DecompressYAZ0";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.DecompYAZ0_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(166, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "CompressYAZ0";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.CompYAZ0_Click);
            // 
            // YAZ0_CompressLevel_TXT
            // 
            this.YAZ0_CompressLevel_TXT.Location = new System.Drawing.Point(96, 75);
            this.YAZ0_CompressLevel_TXT.Name = "YAZ0_CompressLevel_TXT";
            this.YAZ0_CompressLevel_TXT.Size = new System.Drawing.Size(82, 19);
            this.YAZ0_CompressLevel_TXT.TabIndex = 2;
            this.YAZ0_CompressLevel_TXT.Text = "0";
            this.YAZ0_CompressLevel_TXT.Leave += new System.EventHandler(this.YAZ0_CompressLevelTXT_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Level :";
            // 
            // YAZ0_SearchRangeValue_TXT
            // 
            this.YAZ0_SearchRangeValue_TXT.Location = new System.Drawing.Point(96, 100);
            this.YAZ0_SearchRangeValue_TXT.Name = "YAZ0_SearchRangeValue_TXT";
            this.YAZ0_SearchRangeValue_TXT.Size = new System.Drawing.Size(82, 19);
            this.YAZ0_SearchRangeValue_TXT.TabIndex = 4;
            this.YAZ0_SearchRangeValue_TXT.Text = "0x400";
            this.YAZ0_SearchRangeValue_TXT.Leave += new System.EventHandler(this.YAZ0_SearchRangeValue_TXT_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Search range :";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(96, 125);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(82, 20);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Endian :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(189, 154);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.YAZ0_SearchRangeValue_TXT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.YAZ0_CompressLevel_TXT);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "YAZ0 Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox YAZ0_CompressLevel_TXT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox YAZ0_SearchRangeValue_TXT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
    }
}

