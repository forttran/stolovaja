namespace stolov {
    partial class Ctolov {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ctolov));
			this.listOrderElements = new System.Windows.Forms.DataGridView();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.OpenMenuPanel = new System.Windows.Forms.ToolStripButton();
			this.CreateMenuPanel = new System.Windows.Forms.ToolStripButton();
			this.changeMenuPanel = new System.Windows.Forms.ToolStripButton();
			this.deleteMenuPanel = new System.Windows.Forms.ToolStripButton();
			this.CopyMenuPanel = new System.Windows.Forms.ToolStripButton();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
			this.DownloadIsPRO = new System.Windows.Forms.ToolStripMenuItem();
			this.settings = new System.Windows.Forms.ToolStripMenuItem();
			this.program = new System.Windows.Forms.ToolStripMenuItem();
			this.exit = new System.Windows.Forms.ToolStripMenuItem();
			this.newOrder = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.OpenMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.changeMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.CreateMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.CopyMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.ReturnMenu = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.listOrderElements)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// listOrderElements
			// 
			this.listOrderElements.AllowUserToDeleteRows = false;
			this.listOrderElements.BackgroundColor = System.Drawing.Color.White;
			this.listOrderElements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.listOrderElements.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listOrderElements.Location = new System.Drawing.Point(0, 0);
			this.listOrderElements.Name = "listOrderElements";
			this.listOrderElements.ReadOnly = true;
			this.listOrderElements.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.listOrderElements.Size = new System.Drawing.Size(574, 346);
			this.listOrderElements.TabIndex = 1;
			this.listOrderElements.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.listOrderElements_CellContextMenuStripNeeded);
			this.listOrderElements.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listOrderElements_MouseDown);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenMenuPanel,
            this.CreateMenuPanel,
            this.changeMenuPanel,
            this.deleteMenuPanel,
            this.CopyMenuPanel});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(574, 25);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// OpenMenuPanel
			// 
			this.OpenMenuPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.OpenMenuPanel.Image = global::stolov.Properties.Resources.open;
			this.OpenMenuPanel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.OpenMenuPanel.Name = "OpenMenuPanel";
			this.OpenMenuPanel.Size = new System.Drawing.Size(23, 22);
			this.OpenMenuPanel.Text = "Открыть";
			this.OpenMenuPanel.Click += new System.EventHandler(this.OpenMenuPanel_Click);
			// 
			// CreateMenuPanel
			// 
			this.CreateMenuPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.CreateMenuPanel.Image = global::stolov.Properties.Resources.create;
			this.CreateMenuPanel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.CreateMenuPanel.Name = "CreateMenuPanel";
			this.CreateMenuPanel.Size = new System.Drawing.Size(23, 22);
			this.CreateMenuPanel.Text = "Новый заказ";
			this.CreateMenuPanel.Click += new System.EventHandler(this.CreateMenuPanel_Click);
			// 
			// changeMenuPanel
			// 
			this.changeMenuPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.changeMenuPanel.Image = global::stolov.Properties.Resources.edit;
			this.changeMenuPanel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.changeMenuPanel.Name = "changeMenuPanel";
			this.changeMenuPanel.Size = new System.Drawing.Size(23, 22);
			this.changeMenuPanel.Text = "Изменить";
			this.changeMenuPanel.Click += new System.EventHandler(this.changeMenuPanel_Click);
			// 
			// deleteMenuPanel
			// 
			this.deleteMenuPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.deleteMenuPanel.Image = global::stolov.Properties.Resources.delete;
			this.deleteMenuPanel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.deleteMenuPanel.Name = "deleteMenuPanel";
			this.deleteMenuPanel.Size = new System.Drawing.Size(23, 22);
			this.deleteMenuPanel.Text = "Удалить";
			this.deleteMenuPanel.Click += new System.EventHandler(this.deleteMenuPanel_Click);
			// 
			// CopyMenuPanel
			// 
			this.CopyMenuPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.CopyMenuPanel.Image = global::stolov.Properties.Resources.copy;
			this.CopyMenuPanel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.CopyMenuPanel.Name = "CopyMenuPanel";
			this.CopyMenuPanel.Size = new System.Drawing.Size(23, 22);
			this.CopyMenuPanel.Text = "Копировать";
			this.CopyMenuPanel.Click += new System.EventHandler(this.CopyMenuPanel_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem7,
            this.newOrder});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(574, 24);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DownloadIsPRO,
            this.settings,
            this.program,
            this.exit});
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(53, 20);
			this.toolStripMenuItem7.Text = "Меню";
			// 
			// DownloadIsPRO
			// 
			this.DownloadIsPRO.Name = "DownloadIsPRO";
			this.DownloadIsPRO.Size = new System.Drawing.Size(190, 22);
			this.DownloadIsPRO.Text = "Выгрузить в ИС-ПРО";
			this.DownloadIsPRO.Click += new System.EventHandler(this.DownloadIsPRO_Click);
			// 
			// settings
			// 
			this.settings.Name = "settings";
			this.settings.Size = new System.Drawing.Size(190, 22);
			this.settings.Text = "Настройки";
			this.settings.Click += new System.EventHandler(this.settings_Click);
			// 
			// program
			// 
			this.program.Name = "program";
			this.program.Size = new System.Drawing.Size(190, 22);
			this.program.Text = "О программе";
			this.program.Click += new System.EventHandler(this.program_Click);
			// 
			// exit
			// 
			this.exit.Name = "exit";
			this.exit.Size = new System.Drawing.Size(190, 22);
			this.exit.Text = "Выход";
			this.exit.Click += new System.EventHandler(this.exit_Click);
			// 
			// newOrder
			// 
			this.newOrder.Name = "newOrder";
			this.newOrder.Size = new System.Drawing.Size(88, 20);
			this.newOrder.Text = "Новый заказ";
			this.newOrder.Click += new System.EventHandler(this.newOrder_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.listOrderElements);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 49);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(574, 346);
			this.panel1.TabIndex = 6;
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenMenu,
            this.changeMenu,
            this.CreateMenu,
            this.deleteMenu,
            this.CopyMenu,
            this.ReturnMenu});
			this.contextMenu.Name = "contextMenuStrip2";
			this.contextMenu.Size = new System.Drawing.Size(168, 136);
			// 
			// OpenMenu
			// 
			this.OpenMenu.Image = global::stolov.Properties.Resources.open;
			this.OpenMenu.Name = "OpenMenu";
			this.OpenMenu.Size = new System.Drawing.Size(167, 22);
			this.OpenMenu.Text = "Открыть";
			this.OpenMenu.Click += new System.EventHandler(this.OpenMenu_Click);
			// 
			// changeMenu
			// 
			this.changeMenu.Image = global::stolov.Properties.Resources.edit;
			this.changeMenu.Name = "changeMenu";
			this.changeMenu.Size = new System.Drawing.Size(167, 22);
			this.changeMenu.Text = "Изменить";
			this.changeMenu.Click += new System.EventHandler(this.changeMenu_Click);
			// 
			// CreateMenu
			// 
			this.CreateMenu.Image = global::stolov.Properties.Resources.create;
			this.CreateMenu.Name = "CreateMenu";
			this.CreateMenu.Size = new System.Drawing.Size(167, 22);
			this.CreateMenu.Text = "Создать";
			this.CreateMenu.Click += new System.EventHandler(this.CreateMenu_Click);
			// 
			// deleteMenu
			// 
			this.deleteMenu.Image = global::stolov.Properties.Resources.delete;
			this.deleteMenu.Name = "deleteMenu";
			this.deleteMenu.Size = new System.Drawing.Size(167, 22);
			this.deleteMenu.Text = "Удалить";
			this.deleteMenu.Click += new System.EventHandler(this.deleteMenu_Click);
			// 
			// CopyMenu
			// 
			this.CopyMenu.Image = global::stolov.Properties.Resources.copy;
			this.CopyMenu.Name = "CopyMenu";
			this.CopyMenu.Size = new System.Drawing.Size(167, 22);
			this.CopyMenu.Text = "Копировать";
			this.CopyMenu.Click += new System.EventHandler(this.CopyMenu_Click);
			// 
			// ReturnMenu
			// 
			this.ReturnMenu.Image = global::stolov.Properties.Resources.ret;
			this.ReturnMenu.Name = "ReturnMenu";
			this.ReturnMenu.Size = new System.Drawing.Size(167, 22);
			this.ReturnMenu.Text = "Возврат по кассе";
			this.ReturnMenu.Click += new System.EventHandler(this.ReturnMenu_Click);
			// 
			// Ctolov
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(574, 395);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Ctolov";
			this.Text = "Столовая";
			this.Load += new System.EventHandler(this.Ctolov_Load);
			((System.ComponentModel.ISupportInitialize)(this.listOrderElements)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
		private System.Windows.Forms.DataGridView listOrderElements;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton OpenMenuPanel;
		private System.Windows.Forms.ToolStripButton CreateMenuPanel;
		private System.Windows.Forms.ToolStripButton changeMenuPanel;
		private System.Windows.Forms.ToolStripButton deleteMenuPanel;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem DownloadIsPRO;
		private System.Windows.Forms.ToolStripMenuItem exit;
		private System.Windows.Forms.ToolStripMenuItem newOrder;
		private System.Windows.Forms.ToolStripButton CopyMenuPanel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStripMenuItem settings;
		private System.Windows.Forms.ToolStripMenuItem changeMenu;
		private System.Windows.Forms.ToolStripMenuItem deleteMenu;
		private System.Windows.Forms.ToolStripMenuItem ReturnMenu;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem OpenMenu;
		private System.Windows.Forms.ToolStripMenuItem CreateMenu;
		private System.Windows.Forms.ToolStripMenuItem CopyMenu;
		private System.Windows.Forms.ToolStripMenuItem program;
	}
}

