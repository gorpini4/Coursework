namespace KursovaKomiv_3kurs
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.butGen = new System.Windows.Forms.Button();
            this.butPerebor = new System.Windows.Forms.Button();
            this.butSimulated = new System.Windows.Forms.Button();
            this.butGreedy = new System.Windows.Forms.Button();
            this.butPereborMasok = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.butTestGO = new System.Windows.Forms.Button();
            this.richTBshortStory = new System.Windows.Forms.RichTextBox();
            this.butGen1xNxN = new System.Windows.Forms.Button();
            this.timerDO = new System.Windows.Forms.Timer(this.components);
            this.butBranchBound = new System.Windows.Forms.Button();
            this.pB = new System.Windows.Forms.PictureBox();
            this.numericN = new System.Windows.Forms.NumericUpDown();
            this.buttonConstMas = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericN)).BeginInit();
            this.SuspendLayout();
            // 
            // butGen
            // 
            this.butGen.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.butGen.Location = new System.Drawing.Point(12, 262);
            this.butGen.Name = "butGen";
            this.butGen.Size = new System.Drawing.Size(135, 35);
            this.butGen.TabIndex = 0;
            this.butGen.Text = "Gen count*(N*N)";
            this.butGen.UseVisualStyleBackColor = false;
            this.butGen.Click += new System.EventHandler(this.butGen_Click);
            // 
            // butPerebor
            // 
            this.butPerebor.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.butPerebor.Location = new System.Drawing.Point(12, 39);
            this.butPerebor.Name = "butPerebor";
            this.butPerebor.Size = new System.Drawing.Size(135, 35);
            this.butPerebor.TabIndex = 1;
            this.butPerebor.Text = "perebor";
            this.butPerebor.UseVisualStyleBackColor = false;
            this.butPerebor.Click += new System.EventHandler(this.butPerebor_Click);
            // 
            // butSimulated
            // 
            this.butSimulated.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.butSimulated.Location = new System.Drawing.Point(12, 162);
            this.butSimulated.Name = "butSimulated";
            this.butSimulated.Size = new System.Drawing.Size(135, 35);
            this.butSimulated.TabIndex = 2;
            this.butSimulated.Text = "Simul....";
            this.butSimulated.UseVisualStyleBackColor = false;
            this.butSimulated.Click += new System.EventHandler(this.butSimulated_Click);
            // 
            // butGreedy
            // 
            this.butGreedy.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.butGreedy.Location = new System.Drawing.Point(12, 121);
            this.butGreedy.Name = "butGreedy";
            this.butGreedy.Size = new System.Drawing.Size(135, 35);
            this.butGreedy.TabIndex = 3;
            this.butGreedy.Text = "GreedyAlgo";
            this.butGreedy.UseVisualStyleBackColor = false;
            this.butGreedy.Click += new System.EventHandler(this.butGreedy_Click);
            // 
            // butPereborMasok
            // 
            this.butPereborMasok.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.butPereborMasok.Location = new System.Drawing.Point(12, 80);
            this.butPereborMasok.Name = "butPereborMasok";
            this.butPereborMasok.Size = new System.Drawing.Size(135, 35);
            this.butPereborMasok.TabIndex = 4;
            this.butPereborMasok.Text = "PereborMasok";
            this.butPereborMasok.UseVisualStyleBackColor = false;
            this.butPereborMasok.Click += new System.EventHandler(this.butPereborMasok_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 511);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1148, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Status
            // 
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(60, 17);
            this.Status.Text = "StatusText";
            // 
            // butTestGO
            // 
            this.butTestGO.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.butTestGO.Location = new System.Drawing.Point(12, 447);
            this.butTestGO.Name = "butTestGO";
            this.butTestGO.Size = new System.Drawing.Size(135, 61);
            this.butTestGO.TabIndex = 6;
            this.butTestGO.Text = "get statistics";
            this.butTestGO.UseVisualStyleBackColor = false;
            this.butTestGO.Click += new System.EventHandler(this.butTestGO_Click);
            // 
            // richTBshortStory
            // 
            this.richTBshortStory.BackColor = System.Drawing.SystemColors.Window;
            this.richTBshortStory.Location = new System.Drawing.Point(153, 12);
            this.richTBshortStory.Name = "richTBshortStory";
            this.richTBshortStory.Size = new System.Drawing.Size(478, 496);
            this.richTBshortStory.TabIndex = 7;
            this.richTBshortStory.Text = "";
            // 
            // butGen1xNxN
            // 
            this.butGen1xNxN.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.butGen1xNxN.Location = new System.Drawing.Point(12, 303);
            this.butGen1xNxN.Name = "butGen1xNxN";
            this.butGen1xNxN.Size = new System.Drawing.Size(135, 35);
            this.butGen1xNxN.TabIndex = 8;
            this.butGen1xNxN.Text = "Gen 1*(N*N)";
            this.butGen1xNxN.UseVisualStyleBackColor = false;
            this.butGen1xNxN.Click += new System.EventHandler(this.butGen1xNxN_Click);
            // 
            // timerDO
            // 
            this.timerDO.Tick += new System.EventHandler(this.timerDO_Tick);
            // 
            // butBranchBound
            // 
            this.butBranchBound.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.butBranchBound.Location = new System.Drawing.Point(12, 201);
            this.butBranchBound.Name = "butBranchBound";
            this.butBranchBound.Size = new System.Drawing.Size(135, 35);
            this.butBranchBound.TabIndex = 9;
            this.butBranchBound.Text = "Віток та границь";
            this.butBranchBound.UseVisualStyleBackColor = false;
            this.butBranchBound.Click += new System.EventHandler(this.butBranchBound_Click);
            // 
            // pB
            // 
            this.pB.BackColor = System.Drawing.SystemColors.Desktop;
            this.pB.Location = new System.Drawing.Point(637, 13);
            this.pB.Name = "pB";
            this.pB.Size = new System.Drawing.Size(500, 500);
            this.pB.TabIndex = 10;
            this.pB.TabStop = false;
            this.pB.Click += new System.EventHandler(this.pB_Click);
            // 
            // numericN
            // 
            this.numericN.Location = new System.Drawing.Point(12, 13);
            this.numericN.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericN.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericN.Name = "numericN";
            this.numericN.Size = new System.Drawing.Size(135, 20);
            this.numericN.TabIndex = 11;
            this.numericN.Value = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.numericN.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // buttonConstMas
            // 
            this.buttonConstMas.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.buttonConstMas.Location = new System.Drawing.Point(12, 380);
            this.buttonConstMas.Name = "buttonConstMas";
            this.buttonConstMas.Size = new System.Drawing.Size(135, 61);
            this.buttonConstMas.TabIndex = 12;
            this.buttonConstMas.Text = "Запустити алгоритми на константному масиві";
            this.buttonConstMas.UseVisualStyleBackColor = false;
            this.buttonConstMas.Click += new System.EventHandler(this.buttonConstMas_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1148, 533);
            this.Controls.Add(this.buttonConstMas);
            this.Controls.Add(this.numericN);
            this.Controls.Add(this.pB);
            this.Controls.Add(this.butBranchBound);
            this.Controls.Add(this.butGen1xNxN);
            this.Controls.Add(this.richTBshortStory);
            this.Controls.Add(this.butTestGO);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.butPereborMasok);
            this.Controls.Add(this.butGreedy);
            this.Controls.Add(this.butSimulated);
            this.Controls.Add(this.butPerebor);
            this.Controls.Add(this.butGen);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butGen;
        private System.Windows.Forms.Button butPerebor;
        private System.Windows.Forms.Button butSimulated;
        private System.Windows.Forms.Button butGreedy;
        private System.Windows.Forms.Button butPereborMasok;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel Status;
        private System.Windows.Forms.Button butTestGO;
        private System.Windows.Forms.RichTextBox richTBshortStory;
        private System.Windows.Forms.Button butGen1xNxN;
        private System.Windows.Forms.Timer timerDO;
        private System.Windows.Forms.Button butBranchBound;
        private System.Windows.Forms.PictureBox pB;
        private System.Windows.Forms.NumericUpDown numericN;
        private System.Windows.Forms.Button buttonConstMas;
    }
}

