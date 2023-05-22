using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NLog;
using NLog.Filters;
using NLog.Fluent;
using NLog.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml.Serialization;

namespace stolov {
    public partial class Ctolov : Form {
		public static Logger currentClassLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		public Users user;
		public Order order;
		public Ctolov() {
            InitializeComponent();
        }

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (keyData == (Keys.Insert)) {
				user = new Users();
				user.Show();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void toolStripMenuItem8_Click(object sender, EventArgs e) {
			user = new Users();
			user.Show();
		}

		private void Ctolov_Load(object sender, EventArgs e) {
			Data data = Data.getInstance();
            data.OrderElementListGV = this.listOrderElements;
			data.OrderElementListGV.DataSource = data.OrderElementListView;
			this.listOrderElements.Columns[0].HeaderText = "Номер";
			this.listOrderElements.Columns[0].Width = 50;
			this.listOrderElements.Columns[1].HeaderText = "Дата";
			this.listOrderElements.Columns[1].Width = 80;
			this.listOrderElements.Columns[2].HeaderText = "ФИО";
			this.listOrderElements.Columns[2].Width = 270;
			this.listOrderElements.Columns[3].HeaderText = "Сумма";
			this.listOrderElements.Columns[3].Width = 130;
		}

		private void toolStripButton5_Click(object sender, EventArgs e) {
			Data data = Data.getInstance();

			// объект для сериализации
			Person person = new Person("Tom", 37);

			// передаем в конструктор тип класса Person
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Person));

			// получаем поток, куда будем записывать сериализованный объект
			using (FileStream fs = new FileStream("person.xml", FileMode.OpenOrCreate)) {
				xmlSerializer.Serialize(fs, person);

				Console.WriteLine("Object has been serialized");
			}
		}

		private void settings_Click(object sender, EventArgs e) {
			SettingsProg settings = new SettingsProg();
			settings.Show();
		}

		private void DownloadIsPRO_Click(object sender, EventArgs e) {
			Data data = Data.getInstance();
			if (data.OrderElementList.Count > 0) {
				OrdersToISPRO OrdersToISPRO = new OrdersToISPRO();
				OrdersToISPRO.TMPtoPrdZkg();
				MessageBox.Show("Данные выгружены");
				currentClassLogger.Debug("Данные выгружены");
			} else {
				MessageBox.Show("Данных для выгрузки нет");
				currentClassLogger.Debug("Данных для выгрузки нет");
			}
		}

		private void exit_Click(object sender, EventArgs e) {
			Application.Exit();
		}


		private void listOrderElements_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e) {
			if (e.ColumnIndex != -1 && e.RowIndex != -1) {
				contextMenu.Show(Cursor.Position);
			}
		}

		private void listOrderElements_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				var hit = listOrderElements.HitTest(e.X, e.Y);
				if (hit.RowIndex >= 0) {
					listOrderElements.ClearSelection();
					listOrderElements.Rows[hit.RowIndex].Selected = true;
				}
			}
		}

		private void changeMenuFunc() {
			if (listOrderElements.Rows.Count > 0) {
				int row = listOrderElements.CurrentCell.RowIndex;
				Data data = Data.getInstance();

				order = new Order();
				data.orderElement = data.OrderElementList[row];
				data.order = data.orderElement.order.Copy();
				if (data.orderElement.user == null) {
					order.lebeltext = "наличный расчет";
				} else {
					order.lebeltext = data.orderElement.user[1].ToString();
				}
				data.orderGV = order.dataGridOrder;
				data.orderGV.DataSource = data.orderElement.order;
				data.item = row;
				data.dataOrderCaption();
				order.Show();
			} else {
				MessageBox.Show("Не создан ни один заказ. Открывать нечего");
			}
		}
		private void CopyMenuFunc() {
			if (listOrderElements.Rows.Count > 0) {
				int row = listOrderElements.CurrentCell.RowIndex;
				Data data = Data.getInstance();

				data.OrderElementList.Add(data.OrderElementList[row].Copy());
				data.OrderElementListView.Add(new OrderElementView(data.OrderElementListView.Count + 1, data.OrderElementList[row].user[1].ToString(), data.OrderElementList[row].getSum()));
			} else {
				MessageBox.Show("Не создан ни один заказ. Копировать нечего");
			}
		}
		private void deleteMenuFunc() {
			if (listOrderElements.Rows.Count > 0) {
				DialogResult dialogResult = MessageBox.Show("Вы точно хотите удалить выбранную запись?", "Столовая", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes) {
					int row = listOrderElements.CurrentCell.RowIndex;
					Data data = Data.getInstance();
					data.OrderElementList.RemoveAt(row);
					data.OrderElementListView.RemoveAt(row);
				}
			} else {
				MessageBox.Show("Не создан ни один заказ. Удалять нечего");
			}
		}
		private void changeMenu_Click(object sender, EventArgs e) {
			changeMenuFunc();
		}
		private void ReturnMenu_Click(object sender, EventArgs e) {
			int row = listOrderElements.CurrentCell.RowIndex;
			Data data = Data.getInstance();
			data.OrderElementList[row].ret = true;
			MessageBox.Show("Возврат для заказа оформлен");
		}
		private void deleteMenu_Click(object sender, EventArgs e) {
			deleteMenuFunc();
		}

		private void OpenMenu_Click(object sender, EventArgs e) {
			changeMenuFunc();
		}

		private void CreateMenu_Click(object sender, EventArgs e) {
			user = new Users();
			user.Show();
		}

		private void CopyMenu_Click(object sender, EventArgs e) {
			CopyMenuFunc();
		}

		private void program_Click(object sender, EventArgs e) {
			AboutProgram about = new AboutProgram();
			about.ShowDialog();
		}

		private void OpenMenuPanel_Click(object sender, EventArgs e) {
			changeMenuFunc();
		}

		private void CreateMenuPanel_Click(object sender, EventArgs e) {
			user = new Users();
			user.Show();
		}

		private void changeMenuPanel_Click(object sender, EventArgs e) {
			changeMenuFunc();
		}

		private void deleteMenuPanel_Click(object sender, EventArgs e) {
			deleteMenuFunc();
		}

		private void CopyMenuPanel_Click(object sender, EventArgs e) {
			CopyMenuFunc();
		}
	}
}
