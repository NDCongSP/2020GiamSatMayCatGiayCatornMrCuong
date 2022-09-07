namespace QuanLyGiay
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonPalette1 = new ComponentFactory.Krypton.Toolkit.KryptonPalette(this.components);
            this.kryptonDataGridView1 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.colTram = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTocDo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDoiDon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonManager1
            // 
            this.kryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Silver;
            // 
            // kryptonPalette1
            // 
            this.kryptonPalette1.BasePaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Silver;
            // 
            // kryptonDataGridView1
            // 
            this.kryptonDataGridView1.AllowUserToAddRows = false;
            this.kryptonDataGridView1.AllowUserToDeleteRows = false;
            this.kryptonDataGridView1.AllowUserToResizeRows = false;
            this.kryptonDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.kryptonDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.kryptonDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTram,
            this.colTocDo,
            this.colDoiDon,
            this.colDan});
            this.kryptonDataGridView1.Location = new System.Drawing.Point(1041, 12);
            this.kryptonDataGridView1.Name = "kryptonDataGridView1";
            this.kryptonDataGridView1.ReadOnly = true;
            this.kryptonDataGridView1.RowHeadersVisible = false;
            this.kryptonDataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.kryptonDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.kryptonDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryptonDataGridView1.Size = new System.Drawing.Size(297, 269);
            this.kryptonDataGridView1.TabIndex = 0;
            // 
            // colTram
            // 
            this.colTram.HeaderText = "TRẠM";
            this.colTram.Name = "colTram";
            this.colTram.ReadOnly = true;
            this.colTram.Width = 68;
            // 
            // colTocDo
            // 
            this.colTocDo.HeaderText = "TỐC ĐỘ";
            this.colTocDo.Name = "colTocDo";
            this.colTocDo.ReadOnly = true;
            this.colTocDo.Width = 79;
            // 
            // colDoiDon
            // 
            this.colDoiDon.HeaderText = "ĐỔI ĐƠN";
            this.colDoiDon.Name = "colDoiDon";
            this.colDoiDon.ReadOnly = true;
            this.colDoiDon.Width = 85;
            // 
            // colDan
            // 
            this.colDan.HeaderText = "DÀN";
            this.colDan.Name = "colDan";
            this.colDan.ReadOnly = true;
            this.colDan.Width = 61;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.kryptonDataGridView1);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CUTTER PC (SERVER)";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
        private ComponentFactory.Krypton.Toolkit.KryptonPalette kryptonPalette1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView kryptonDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTram;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTocDo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDoiDon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDan;
    }
}

