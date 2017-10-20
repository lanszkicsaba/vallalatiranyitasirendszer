namespace VIR
{
    partial class HomeForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Keszletezes = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.welcome_label = new System.Windows.Forms.Label();
            this.logout_btn = new System.Windows.Forms.Button();
            this.tableexport_btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.kibocsajtoneve_textBox = new System.Windows.Forms.TextBox();
            this.kibocsajtocime_textBox = new System.Windows.Forms.TextBox();
            this.kibocsajtoadoszam_textBox = new System.Windows.Forms.TextBox();
            this.kibocsajtoszamlaszam_textBox = new System.Windows.Forms.TextBox();
            this.kibocsajtokozossegiadoszam_textBox = new System.Windows.Forms.TextBox();
            this.vevoneve_textBox = new System.Windows.Forms.TextBox();
            this.vevocime_textBox = new System.Windows.Forms.TextBox();
            this.vevoadoszam_textBox = new System.Windows.Forms.TextBox();
            this.vevoszamlaszam_textBox = new System.Windows.Forms.TextBox();
            this.vevokozossegiadoszam_textBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.megrendelesek_comboBox = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.Keszletezes.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Keszletezes);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 62);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(860, 488);
            this.tabControl1.TabIndex = 0;
            // 
            // Keszletezes
            // 
            this.Keszletezes.Controls.Add(this.tableexport_btn);
            this.Keszletezes.Location = new System.Drawing.Point(4, 22);
            this.Keszletezes.Name = "Keszletezes";
            this.Keszletezes.Padding = new System.Windows.Forms.Padding(3);
            this.Keszletezes.Size = new System.Drawing.Size(852, 462);
            this.Keszletezes.TabIndex = 0;
            this.Keszletezes.Text = "Készletezés";
            this.Keszletezes.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.megrendelesek_comboBox);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(852, 462);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Számlázás";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // welcome_label
            // 
            this.welcome_label.AutoSize = true;
            this.welcome_label.Location = new System.Drawing.Point(619, 36);
            this.welcome_label.Name = "welcome_label";
            this.welcome_label.Size = new System.Drawing.Size(153, 13);
            this.welcome_label.TabIndex = 1;
            this.welcome_label.Text = "Üdvözöllek, \"Felhasználó név\"";
            // 
            // logout_btn
            // 
            this.logout_btn.Location = new System.Drawing.Point(778, 31);
            this.logout_btn.Name = "logout_btn";
            this.logout_btn.Size = new System.Drawing.Size(89, 23);
            this.logout_btn.TabIndex = 2;
            this.logout_btn.Text = "Kijelentkezés";
            this.logout_btn.UseVisualStyleBackColor = true;
            // 
            // tableexport_btn
            // 
            this.tableexport_btn.Location = new System.Drawing.Point(262, 35);
            this.tableexport_btn.Name = "tableexport_btn";
            this.tableexport_btn.Size = new System.Drawing.Size(312, 23);
            this.tableexport_btn.TabIndex = 3;
            this.tableexport_btn.Text = "Tábla exportálás";
            this.tableexport_btn.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.kibocsajtokozossegiadoszam_textBox);
            this.groupBox1.Controls.Add(this.kibocsajtoszamlaszam_textBox);
            this.groupBox1.Controls.Add(this.kibocsajtoadoszam_textBox);
            this.groupBox1.Controls.Add(this.kibocsajtocime_textBox);
            this.groupBox1.Controls.Add(this.kibocsajtoneve_textBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(36, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(318, 294);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Számlakibocsátó adatai:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.vevokozossegiadoszam_textBox);
            this.groupBox2.Controls.Add(this.vevoszamlaszam_textBox);
            this.groupBox2.Controls.Add(this.vevoadoszam_textBox);
            this.groupBox2.Controls.Add(this.vevocime_textBox);
            this.groupBox2.Controls.Add(this.vevoneve_textBox);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(457, 38);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 294);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vevő adatai:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Név:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Cím:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Adóazonosító szám:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Számlaszám:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 200);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Közösségi adószám:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Közösségi adószám:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(47, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Számlaszám:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(47, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Adóazonosító szám:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(50, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Cím:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(47, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Név:";
            // 
            // kibocsajtoneve_textBox
            // 
            this.kibocsajtoneve_textBox.Location = new System.Drawing.Point(56, 41);
            this.kibocsajtoneve_textBox.Name = "kibocsajtoneve_textBox";
            this.kibocsajtoneve_textBox.Size = new System.Drawing.Size(218, 20);
            this.kibocsajtoneve_textBox.TabIndex = 5;
            // 
            // kibocsajtocime_textBox
            // 
            this.kibocsajtocime_textBox.Location = new System.Drawing.Point(56, 84);
            this.kibocsajtocime_textBox.Name = "kibocsajtocime_textBox";
            this.kibocsajtocime_textBox.Size = new System.Drawing.Size(218, 20);
            this.kibocsajtocime_textBox.TabIndex = 6;
            // 
            // kibocsajtoadoszam_textBox
            // 
            this.kibocsajtoadoszam_textBox.Location = new System.Drawing.Point(129, 117);
            this.kibocsajtoadoszam_textBox.Name = "kibocsajtoadoszam_textBox";
            this.kibocsajtoadoszam_textBox.Size = new System.Drawing.Size(145, 20);
            this.kibocsajtoadoszam_textBox.TabIndex = 7;
            // 
            // kibocsajtoszamlaszam_textBox
            // 
            this.kibocsajtoszamlaszam_textBox.Location = new System.Drawing.Point(95, 152);
            this.kibocsajtoszamlaszam_textBox.Name = "kibocsajtoszamlaszam_textBox";
            this.kibocsajtoszamlaszam_textBox.Size = new System.Drawing.Size(179, 20);
            this.kibocsajtoszamlaszam_textBox.TabIndex = 8;
            // 
            // kibocsajtokozossegiadoszam_textBox
            // 
            this.kibocsajtokozossegiadoszam_textBox.Location = new System.Drawing.Point(130, 200);
            this.kibocsajtokozossegiadoszam_textBox.Name = "kibocsajtokozossegiadoszam_textBox";
            this.kibocsajtokozossegiadoszam_textBox.Size = new System.Drawing.Size(144, 20);
            this.kibocsajtokozossegiadoszam_textBox.TabIndex = 9;
            // 
            // vevoneve_textBox
            // 
            this.vevoneve_textBox.Location = new System.Drawing.Point(81, 41);
            this.vevoneve_textBox.Name = "vevoneve_textBox";
            this.vevoneve_textBox.Size = new System.Drawing.Size(218, 20);
            this.vevoneve_textBox.TabIndex = 10;
            // 
            // vevocime_textBox
            // 
            this.vevocime_textBox.Location = new System.Drawing.Point(82, 81);
            this.vevocime_textBox.Name = "vevocime_textBox";
            this.vevocime_textBox.Size = new System.Drawing.Size(218, 20);
            this.vevocime_textBox.TabIndex = 11;
            // 
            // vevoadoszam_textBox
            // 
            this.vevoadoszam_textBox.Location = new System.Drawing.Point(156, 115);
            this.vevoadoszam_textBox.Name = "vevoadoszam_textBox";
            this.vevoadoszam_textBox.Size = new System.Drawing.Size(145, 20);
            this.vevoadoszam_textBox.TabIndex = 12;
            // 
            // vevoszamlaszam_textBox
            // 
            this.vevoszamlaszam_textBox.Location = new System.Drawing.Point(121, 152);
            this.vevoszamlaszam_textBox.Name = "vevoszamlaszam_textBox";
            this.vevoszamlaszam_textBox.Size = new System.Drawing.Size(179, 20);
            this.vevoszamlaszam_textBox.TabIndex = 13;
            // 
            // vevokozossegiadoszam_textBox
            // 
            this.vevokozossegiadoszam_textBox.Location = new System.Drawing.Point(157, 193);
            this.vevokozossegiadoszam_textBox.Name = "vevokozossegiadoszam_textBox";
            this.vevokozossegiadoszam_textBox.Size = new System.Drawing.Size(144, 20);
            this.vevokozossegiadoszam_textBox.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(345, 423);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Számla kiállítása";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // megrendelesek_comboBox
            // 
            this.megrendelesek_comboBox.FormattingEnabled = true;
            this.megrendelesek_comboBox.Location = new System.Drawing.Point(92, 375);
            this.megrendelesek_comboBox.Name = "megrendelesek_comboBox";
            this.megrendelesek_comboBox.Size = new System.Drawing.Size(644, 21);
            this.megrendelesek_comboBox.TabIndex = 3;
            this.megrendelesek_comboBox.Text = "Válaszon a megrendelések közül";
            // 
            // HomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.logout_btn);
            this.Controls.Add(this.welcome_label);
            this.Controls.Add(this.tabControl1);
            this.Name = "HomeForm";
            this.Text = "VIR Rendszer";
            this.tabControl1.ResumeLayout(false);
            this.Keszletezes.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Keszletezes;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label welcome_label;
        private System.Windows.Forms.Button logout_btn;
        private System.Windows.Forms.Button tableexport_btn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox vevokozossegiadoszam_textBox;
        private System.Windows.Forms.TextBox vevoszamlaszam_textBox;
        private System.Windows.Forms.TextBox vevoadoszam_textBox;
        private System.Windows.Forms.TextBox vevocime_textBox;
        private System.Windows.Forms.TextBox vevoneve_textBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox kibocsajtokozossegiadoszam_textBox;
        private System.Windows.Forms.TextBox kibocsajtoszamlaszam_textBox;
        private System.Windows.Forms.TextBox kibocsajtoadoszam_textBox;
        private System.Windows.Forms.TextBox kibocsajtocime_textBox;
        private System.Windows.Forms.TextBox kibocsajtoneve_textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox megrendelesek_comboBox;
    }
}