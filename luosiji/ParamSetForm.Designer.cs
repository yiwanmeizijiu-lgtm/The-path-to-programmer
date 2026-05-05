namespace luosiji
{
    partial class ParamSetForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbx_ip = new System.Windows.Forms.TextBox();
            this.tbx_startPos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbx_port = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbx_endPos = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbx_UpLimit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "PLC IP:";
            // 
            // tbx_ip
            // 
            this.tbx_ip.Location = new System.Drawing.Point(179, 36);
            this.tbx_ip.Name = "tbx_ip";
            this.tbx_ip.Size = new System.Drawing.Size(310, 25);
            this.tbx_ip.TabIndex = 1;
            // 
            // tbx_startPos
            // 
            this.tbx_startPos.Location = new System.Drawing.Point(179, 141);
            this.tbx_startPos.Name = "tbx_startPos";
            this.tbx_startPos.Size = new System.Drawing.Size(310, 25);
            this.tbx_startPos.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "锁付起始位置:";
            // 
            // tbx_port
            // 
            this.tbx_port.Location = new System.Drawing.Point(179, 89);
            this.tbx_port.Name = "tbx_port";
            this.tbx_port.Size = new System.Drawing.Size(310, 25);
            this.tbx_port.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "PLC 端口:";
            // 
            // tbx_endPos
            // 
            this.tbx_endPos.Location = new System.Drawing.Point(179, 198);
            this.tbx_endPos.Name = "tbx_endPos";
            this.tbx_endPos.Size = new System.Drawing.Size(310, 25);
            this.tbx_endPos.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "锁附结束位置:";
            // 
            // tbx_UpLimit
            // 
            this.tbx_UpLimit.Location = new System.Drawing.Point(179, 250);
            this.tbx_UpLimit.Name = "tbx_UpLimit";
            this.tbx_UpLimit.Size = new System.Drawing.Size(310, 25);
            this.tbx_UpLimit.TabIndex = 9;
           
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 253);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "扭力上限:";
            
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(229, 339);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(149, 43);
            this.btn_save.TabIndex = 10;
            this.btn_save.Text = "保存参数";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // ParamSetForm
            // 
            this.ClientSize = new System.Drawing.Size(624, 493);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.tbx_UpLimit);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbx_endPos);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbx_port);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbx_startPos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbx_ip);
            this.Controls.Add(this.label1);
            this.Name = "ParamSetForm";
            this.Text = "参数设置页面";
           
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbx_ip;
        private System.Windows.Forms.TextBox tbx_startPos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbx_port;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbx_endPos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbx_UpLimit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_save;
    }
}