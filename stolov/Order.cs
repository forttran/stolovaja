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
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (keyData == (Keys.F3)) {
				prise = new Prise();
				prise.Show();
				return true;
			}
			if (keyData == (Keys.Enter)) {
				EnterEndPrint();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		public string lebeltext {
			get { return FIO.Text; }
			set { FIO.Text = value; }
		}
		private void label1_Click(object sender, EventArgs e) {
		}

		private void button1_Click(object sender, EventArgs e) {
			prise = new Prise();
			prise.Show();
		}
		private void Log(Data data) {
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
		}
		private void EnterEndPrint() {
			Data data = Data.getInstance();
			String FIO = "";
			String tab = "";
			data.orderElement.order = data.order.Copy();
			this.Close();
			if (data.orderElement.inAction == -1) {
				data.orderElement.inAction = data.OrderElementList.Count;
				data.OrderElementList.Add(data.orderElement);
				if(data.orderElement.user != null) {
					FIO = data.orderElement.user[1].ToString();
					tab = data.orderElement.user[0].ToString();
				}
				data.OrderElementListView.Add(new OrderElementView(data.OrderElementListView.Count + 1, FIO, data.orderElement.getSum()));
			} else {
				data.OrderElementList[data.orderElement.inAction] = data.orderElement;
				data.OrderElementListView[data.orderElement.inAction].sum = data.orderElement.getSum();
			}
			data.orderGV.Refresh();
			//dataXML xml = new dataXML();
			//xml.Get(data.orderElement);
			Print print = new Print(data.orderElement.order, data.OrderElementListView.Count, FIO, tab);

			Log(data);
			data.order.Clear();
			data.item = -1;
		}
		private void button2_Click(object sender, EventArgs e) {
			EnterEndPrint();
		}

		private void Order_Load(object sender, EventArgs e) {
			Data data = Data.getInstance();
			data.orderGV = this.dataGridOrder;
			data.orderGV.DataSource = data.order;
			if (data.order.Rows.Count > 0) {
				data.dataOrderCaption();
			}
		}

		private void Order_FormClosing(object sender, FormClosingEventArgs e) {
			Data data = Data.getInstance();
			data.order.Clear();
			data.item = -1;
		}

		private void FIO_Click(object sender, EventArgs e) {
			Data data = Data.getInstance();
			Users user = new Users();
			user.ShowDialog();
			if(data.OrderElementList[data.item].user != null) {
				this.FIO.Text = data.OrderElementList[data.item].user[1].ToString();
				data.OrderElementListView[data.item].FIO = data.OrderElementList[data.item].user[1].ToString();
			} else {
				this.FIO.Text = "Безналичный расчет";
				data.OrderElementListView[data.item].FIO = "";
			}
		}
	}
}
