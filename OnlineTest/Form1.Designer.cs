namespace OnlineTest
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.buttonTest01 = new System.Windows.Forms.Button();
            this.buttonTest02 = new System.Windows.Forms.Button();
            this.buttonRe = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStart.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(66)))), ((int)(((byte)(240)))));
            this.buttonStart.Location = new System.Drawing.Point(659, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(173, 63);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(546, 425);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // buttonTest01
            // 
            this.buttonTest01.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonTest01.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonTest01.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonTest01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTest01.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTest01.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(66)))), ((int)(((byte)(240)))));
            this.buttonTest01.Location = new System.Drawing.Point(659, 102);
            this.buttonTest01.Name = "buttonTest01";
            this.buttonTest01.Size = new System.Drawing.Size(173, 63);
            this.buttonTest01.TabIndex = 2;
            this.buttonTest01.Text = "Test01";
            this.buttonTest01.UseVisualStyleBackColor = true;
            this.buttonTest01.Click += new System.EventHandler(this.buttonTest01_Click);
            // 
            // buttonTest02
            // 
            this.buttonTest02.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonTest02.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonTest02.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonTest02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTest02.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTest02.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(66)))), ((int)(((byte)(240)))));
            this.buttonTest02.Location = new System.Drawing.Point(659, 189);
            this.buttonTest02.Name = "buttonTest02";
            this.buttonTest02.Size = new System.Drawing.Size(173, 63);
            this.buttonTest02.TabIndex = 3;
            this.buttonTest02.Text = "Test02";
            this.buttonTest02.UseVisualStyleBackColor = true;
            this.buttonTest02.Click += new System.EventHandler(this.buttonTest02_Click);
            // 
            // buttonRe
            // 
            this.buttonRe.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.buttonRe.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonRe.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonRe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRe.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(66)))), ((int)(((byte)(240)))));
            this.buttonRe.Location = new System.Drawing.Point(659, 292);
            this.buttonRe.Name = "buttonRe";
            this.buttonRe.Size = new System.Drawing.Size(173, 63);
            this.buttonRe.TabIndex = 4;
            this.buttonRe.Text = "Recheck";
            this.buttonRe.UseVisualStyleBackColor = true;
            this.buttonRe.Click += new System.EventHandler(this.buttonRe_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(933, 449);
            this.Controls.Add(this.buttonRe);
            this.Controls.Add(this.buttonTest02);
            this.Controls.Add(this.buttonTest01);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.buttonStart);
            this.Name = "Form1";
            this.Text = "Calibration";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonTest01;
        private System.Windows.Forms.Button buttonTest02;
        private System.Windows.Forms.Button buttonRe;
    }
}

