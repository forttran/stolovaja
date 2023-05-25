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
		//функция для подключения обработчика нажатия клавиш
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (keyData == (Keys.Insert)) {
				user = new Users();
				user.Show();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}	
		//Функция загразки главной формы
		private void Ctolov_Load(object sender, EventArgs e) {
			try {
				Data data = Data.GetInstance();
				data.OrderElementListGV = this.listOrderElements;
				data.OrderElementListGV.DataSource = data.OrderElementListView;
				data.ListOrderElementsCaptions();
				DataXML xml = new();
				xml.GetXML();
			}catch(Exception ex) {
				MessageBox.Show("При загрузке формы возникли проблемы.");
				currentClassLogger.Error("Stolov.Ctolov_Load.При загрузке формы возникли проблемы.");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		//Отображение формы настроек
		private void Settings_Click(object sender, EventArgs e) {
			SettingsProg settings = new();
			settings.Show();
		}
		//о программе
		private void Program_Click(object sender, EventArgs e) {
			AboutProgram about = new();
			about.ShowDialog();
		}
		//выходим
		private void Exit_Click(object sender, EventArgs e) {
			Application.Exit();
		}
		//Функция начала создания нового заказа
		private void CreateMenu_Click(object sender, EventArgs e) {
			ShowUser();
		}
		//Функция начала создания нового заказа
		private void CreateMenuPanel_Click(object sender, EventArgs e) {
			ShowUser();
		}
		//Функция начала создания нового заказа
		private void NewOrder_Click(object sender, EventArgs e) {
			ShowUser();
		}
		//Отображаем форму
		public void ShowUser() {
			user = new Users();
			user.Show();
		}
		//Выгрузка данных в ис-про
		private void DownloadIsPRO_Click(object sender, EventArgs e) {
			try {
				Data data = Data.GetInstance();
				if (data.OrderElementList.Count > 0) {//если есть что выгружать
					DataXML xml = new();
					xml.CloneXML();//Клонируем данные на всякий случай
					OrdersToISPRO OrdersToISPRO = new();
					OrdersToISPRO.TMPtoPrdZkg();//запуск главной функции выгрузки
					MessageBox.Show("Данные выгружены");
					currentClassLogger.Debug("Данные выгружены");
				} else {
					MessageBox.Show("Данных для выгрузки нет");
					currentClassLogger.Debug("Данных для выгрузки нет");
				}
			}catch(Exception ex) {
				MessageBox.Show("При выгрузке в ИС-ПРО возникли проблемы.");
				currentClassLogger.Error("Stolov.DownloadIsPRO_Click.При выгрузке в ИС-ПРО возникли проблемы.");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		//Функция нужна для отображения контекстного меню на главной
		private void ListOrderElements_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e) {
			if (e.ColumnIndex != -1 && e.RowIndex != -1) {
				contextMenu.Show(Cursor.Position);
			}
		}
		//Функция нужна для перемещения выделения по датагриду при клике левой кнопки мыши
		private void ListOrderElements_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				var hit = listOrderElements.HitTest(e.X, e.Y);
				if (hit.RowIndex >= 0) {
					listOrderElements.ClearSelection();
					listOrderElements.Rows[hit.RowIndex].Selected = true;
					listOrderElements.CurrentCell = listOrderElements.Rows[hit.RowIndex].Cells[0]; 
				}
			}
		}
		//Функция редактирования
		private void ChangeMenuFunc() {
			try {
				if (listOrderElements.Rows.Count > 0) {
					int row = listOrderElements.CurrentCell.RowIndex;//получаем номер заказа
					Data data = Data.GetInstance();
					order = new Order();//создаем форму 

					data.OrderElementList[row].inAction = row;//подготовка и инициализация структур для редактирования
					data.orderElement = data.OrderElementList[row];
					data.order = data.orderElement.order.Copy();

					if (data.orderElement.user == null) {
						order.Lebeltext = "наличный расчет";
					} else {
						order.Lebeltext = data.orderElement.user[1].ToString();
					}

					data.orderGV = order.dataGridOrder;
					data.orderGV.DataSource = data.orderElement.order;
					data.item = row;

					if (data.orderElement.order.Rows.Count > 0) {//Если есть что отображать по позициям в заказе
						data.DataOrderCaption();//рисуем шапку
					}
					order.Show();
				} else {
					MessageBox.Show("Не создан ни один заказ. Открывать нечего");
				}
			}catch(Exception ex) {
				MessageBox.Show("При редактировании заказа возникли проблемы.");
				currentClassLogger.Error("Stolov.ChangeMenuFunc.При редактировании заказа возникли проблемы.");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		//Функция для копирования заказов
		private void CopyMenuFunc() {
			try {
				if (listOrderElements.Rows.Count > 0) {
					int row = listOrderElements.CurrentCell.RowIndex;//получаем номер заказа
					Data data = Data.GetInstance();

					data.OrderElementList.Add(data.OrderElementList[row].Copy());//Добавляем копию заказа в главную струтуру
					if (data.OrderElementList[row].user != null) {
						data.OrderElementListView.Add(new OrderElementView(data.OrderElementListView.Count + 1, data.OrderElementList[row].user[1].ToString(), data.OrderElementList[row].GetSum()));
					} else {
						data.OrderElementListView.Add(new OrderElementView(data.OrderElementListView.Count + 1, "", data.OrderElementList[row].GetSum()));
					}
					DataXML xml = new();//Добавляем данные в XML
					xml.SetXML(data.OrderElementList[row].Copy());
				} else {
					MessageBox.Show("Не создан ни один заказ. Копировать нечего");
				}
			} catch (Exception ex) {
				MessageBox.Show("При копировании заказа возникли проблемы.");
				currentClassLogger.Error("Stolov.CopyMenuFunc.При копировании заказа возникли проблемы.");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		//функция удаления заказа
		private void DeleteMenuFunc() {
			try {
				if (listOrderElements.Rows.Count > 0) {
					DialogResult dialogResult = MessageBox.Show("Вы точно хотите удалить выбранную запись?", "Столовая", MessageBoxButtons.YesNo);
					if (dialogResult == DialogResult.Yes) {
						Data data = Data.GetInstance();
						int row = listOrderElements.CurrentCell.RowIndex;
						data.ElementRemove(row);//собственно удаление
					}
				} else {
					MessageBox.Show("Не создан ни один заказ. Удалять нечего");
				}
			} catch (Exception ex) {
				MessageBox.Show("При удалении заказа возникли проблемы.");
				currentClassLogger.Error("Stolov.DeleteMenuFunc.При удалении заказа возникли проблемы.");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		//Функция реализации возврата
		private void ReturnMenu_Click(object sender, EventArgs e) {
			try {
				int row = listOrderElements.CurrentCell.RowIndex;
				Data data = Data.GetInstance();
				data.OrderElementList[row].ret = true;
				MessageBox.Show("Возврат для заказа оформлен");
			} catch (Exception ex) {
				MessageBox.Show("При воврате заказа возникли проблемы.");
				currentClassLogger.Error("Stolov.ReturnMenu_Click.При воврате заказа возникли проблемы.");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		private void ChangeMenu_Click(object sender, EventArgs e) {
			ChangeMenuFunc();
		}
		private void DeleteMenu_Click(object sender, EventArgs e) {
			DeleteMenuFunc();
		}
		private void OpenMenu_Click(object sender, EventArgs e) {
			ChangeMenuFunc();
		}
		private void CopyMenu_Click(object sender, EventArgs e) {
			CopyMenuFunc();
		}
		private void OpenMenuPanel_Click(object sender, EventArgs e) {
			ChangeMenuFunc();
		}
		private void ChangeMenuPanel_Click(object sender, EventArgs e) {
			ChangeMenuFunc();
		}
		private void DeleteMenuPanel_Click(object sender, EventArgs e) {
			DeleteMenuFunc();
		}
		private void CopyMenuPanel_Click(object sender, EventArgs e) {
			CopyMenuFunc();
		}
	}
}
