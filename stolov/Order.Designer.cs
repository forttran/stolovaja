namespace stolov {
	partial class Order {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.panel3 = new System.Windows.Forms.Panel();
			this.FIO = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.dataGridOrder = new System.Windows.Forms.DataGridView();
			this.panel1.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridOrder)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.panel4);
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(762, 29);
			this.panel1.TabIndex = 0;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.button2);
			this.panel4.Controls.Add(this.button1);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(479, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(283, 29);
			this.panel4.TabIndex = 1;
			// 
			// button2
			// 
			this.button2.Dock = System.Windows.Forms.DockStyle.Right;
			this.button2.Location = new System.Drawing.Point(6, 0);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(148, 29);
			this.button2.TabIndex = 1;
			this.button2.Text = "Завершить и распечатать";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2_Click);
			// 
			// button1
			// 
			this.button1.Dock = System.Windows.Forms.DockStyle.Right;
			this.button1.Location = new System.Drawing.Point(154, 0);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(129, 29);
			this.button1.TabIndex = 0;
			this.button1.Text = "Выбрать из прайса";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1_Click);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.FIO);
			this.panel3.Controls.Add(this.label1);
			this.panel3.Cursor = System.Windows.Forms.Cursors.Default;
			this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(479, 29);
			this.panel3.TabIndex = 0;
			// 
			// FIO
			// 
			this.FIO.Dock = System.Windows.Forms.DockStyle.Left;
			this.FIO.Location = new System.Drawing.Point(64, 0);
			this.FIO.Name = "FIO";
			this.FIO.Size = new System.Drawing.Size(409, 29);
			this.FIO.TabIndex = 1;
			this.FIO.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.FIO.Click += new System.EventHandler(this.FIO_Click);
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Left;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 29);
			this.label1.TabIndex = 0;
			this.label1.Text = "Заказчик:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.dataGridOrder);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 29);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(762, 421);
			this.panel2.TabIndex = 1;
			// 
			// dataGridOrder
			// 
			this.dataGridOrder.AllowUserToAddRows = false;
			this.dataGridOrder.AllowUserToDeleteRows = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle1.Format = "N2";
			dataGridViewCellStyle1.NullValue = null;
			this.dataGridOrder.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridOrder.BackgroundColor = System.Drawing.Color.White;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.Format = "N2";
			dataGridViewCellStyle2.NullValue = null;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridOrder.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.dataGridOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle3.Format = "N2";
			dataGridViewCellStyle3.NullValue = null;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridOrder.DefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridOrder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridOrder.Location = new System.Drawing.Point(0, 0);
			this.dataGridOrder.Name = "dataGridOrder";
			this.dataGridOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridOrder.Size = new System.Drawing.Size(762, 421);
			this.dataGridOrder.TabIndex = 0;
			// 
			// Order
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(762, 450);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "Order";
			this.Text = "Заказ";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Order_FormClosing);
			this.Load += new System.EventHandler(this.Order_Load);
			this.panel1.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridOrder)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		public System.Windows.Forms.DataGridView dataGridOrder;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label FIO;
	}
}