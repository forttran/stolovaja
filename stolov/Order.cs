using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using NLog;
using NLog.Filters;
using NLog.Fluent;
using NLog.Web;
using System.Web.UI.WebControls;

namespace stolov {
	public partial class Order : Form {
		public static Logger currentClassLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		public Prise prise;
		public Order() {
			InitializeComponent();
		}
		//функция для подключения обработчика нажатия клавиш
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (keyData == (Keys.F3)) {//По F3 отображать прайс
				prise = new Prise();
				prise.Show();
				return true;
			}
			if (keyData == (Keys.Enter)) {//По enter печатать
				EnterEndPrint();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		public string Lebeltext {
			get { return FIO.Text; }
			set { FIO.Text = value; }
		}
		//Отображение прайса
		private void Button1_Click(object sender, EventArgs e) {
			try {
				Data data = Data.GetInstance();
				prise = new Prise();
				prise.ShowDialog();
				if (data.order.Rows.Count > 0) {
					data.orderGV.Visible = true;
					data.DataOrderCaption();
				} else {
					data.orderGV.Visible = false;
				}
			} catch (Exception ex) {
				MessageBox.Show("Проблемы с отображением прайса");
				currentClassLogger.Error("Orders.button1_Click.Ошибка подключения к серверу: ");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		//Логирование для печати
		private void Log(Data data) {
			try {
				currentClassLogger.Debug("Новый заказ");
				if (data.orderElement.user != null) {
					currentClassLogger.Debug("Табельный: " + data.orderElement.user[0].ToString());
					currentClassLogger.Debug("ФИО: " + data.orderElement.user[1].ToString());
				} else {
					currentClassLogger.Debug("Табельный: наличный расчет");
					currentClassLogger.Debug("ФИО: наличный расчет");
				}
				DataRow[] result = data.order.Select();
				foreach (DataRow row in result) {
					currentClassLogger.Debug("товар: " + row[2] + " " + row[5] + " " + row[6]);
				}
			} catch (Exception ex) {
				MessageBox.Show("Проблемы с отображением логов");
				currentClassLogger.Error("Orders.Log.Проблемы с отображением логов: ");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		//Сохранение и печать
		private void EnterEndPrint() {
			try {
				Data data = Data.GetInstance();
				String FIO = "";
				String tab = "";
				data.orderElement.order = data.order.Copy();
				if (data.orderElement.inAction == -1) {//Если элемент новый
					data.orderElement.inAction = data.OrderElementList.Count;
					data.OrderElementList.Add(data.orderElement);
					if (data.orderElement.user != null) {
						FIO = data.orderElement.user[1].ToString();
						tab = data.orderElement.user[0].ToString();
					}
					data.OrderElementListView.Add(new OrderElementView(data.OrderElementListView.Count + 1, FIO, data.orderElement.GetSum()));
					DataXML xml = new();
					xml.SetXML(data.orderElement);
				} else {//если элемент редактируется
					data.OrderElementList[data.orderElement.inAction] = data.orderElement;
					data.OrderElementListView[data.orderElement.inAction].sum = data.orderElement.GetSum();
					DataXML xml = new();
					xml.ChangeXML(data.orderElement.inAction);
				}
				data.orderGV.Refresh();
				this.Close();
				Print print = new(data.orderElement.order, data.OrderElementListView.Count, FIO, tab);

				Log(data);
				data.order.Clear();
				data.orderElement = null;
				data.item = -1;
			} catch (Exception ex) {
				MessageBox.Show("Проблемы с сохранением и печатью");
				currentClassLogger.Error("Orders.EnterEndPrint.Проблемы с сохранением и печатью");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		private void Button2_Click(object sender, EventArgs e) {
			EnterEndPrint();
		}
		//Функция загрузки формы
		private void Order_Load(object sender, EventArgs e) {
			try {
				Data data = Data.GetInstance();
				data.orderGV = this.dataGridOrder;
				data.orderGV.DataSource = data.order;
				if (data.order.Rows.Count > 0) {
					data.orderGV.Visible = true;
					data.DataOrderCaption();
				} else {
					data.orderGV.Visible = false;
				}
			} catch (Exception ex) {
				MessageBox.Show("Проблемы с загрузкой формы");
				currentClassLogger.Error("Orders.Order_Load.Проблемы с загрузкой формы");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}

		private void Order_FormClosing(object sender, FormClosingEventArgs e) {
			Data data = Data.GetInstance();
			data.order.Clear();
			data.item = -1;
		}
		//смена пользователя
		private void FIO_Click(object sender, EventArgs e) {
			try {
				Data data = Data.GetInstance();
				Users user = new();
				if (data.item == -1) {
					data.item = -2;
					currentClassLogger.Debug("Редактирование нового");
				}
				user.ShowDialog();
				if (data.item == -2) {
					if (data.orderElement.user != null) {
						this.FIO.Text = data.orderElement.user[1].ToString();
					} else {
						this.FIO.Text = "Безналичный расчет";
					}
				} else {
					if (data.OrderElementList[data.item].user != null) {
						this.FIO.Text = data.OrderElementList[data.item].user[1].ToString();
						data.OrderElementListView[data.item].FIO = data.OrderElementList[data.item].user[1].ToString();
					} else {
						this.FIO.Text = "Безналичный расчет";
						data.OrderElementListView[data.item].FIO = "";
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("Проблемы с редактированием заказчика");
				currentClassLogger.Error("Orders.FIO_Click.Проблемы с редактированием заказчика");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
	}
}
