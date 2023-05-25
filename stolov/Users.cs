using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using NLog.Filters;
using NLog.Fluent;
using NLog.Web;

namespace stolov {
	public partial class Users : Form {
		public static Logger currentClassLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		public Order order;
		public Users() {
			InitializeComponent();
		}
		//функция для подключения обработчика нажатия клавиш
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (keyData == (Keys.Down)) {//При нажатии кнопки вниз переходим на датагрид
				if (dgUserList.Focus() == false) {
					return true;
				} else {
					return base.ProcessCmdKey(ref msg, keyData);
				}
			}
			if (keyData == (Keys.Enter)) {//При нажатии на enter происходит выбор пользователя
				if (dgUserList.Focused) {
					CellClick(true);
				} else {
					if((textBox1.Text == "") && (textBox2.Text == "")){
						CellClick(false);
					}
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
		//Функция загразки формы юзеров
		private void Users_Load(object sender, EventArgs e) {
			try {
				DataTable dt;
				DB.getInstance();
				dt = DB.Query("SELECT CONVERT(varchar,Kpu_Tn) Kpu_Tn, c1.Kpu_Fio Fio, pdr.SprPdr_NmFull,  CONVERT(varchar,c1.Kpu_DtNpSt,105), CONVERT(varchar,Kpu_DtRoj,105),dol.SprD_NmIm Dol FROM KPUC1 c1 LEFT JOIN KPUX x ON x.Kpu_Rcd = c1.Kpu_Rcd LEFT JOIN SPRDOL dol ON dol.SprD_Cd = x.Kpu_CdDol LEFT JOIN SPRPDR pdr on pdr.SprPdr_Pd=x.Kpu_CdPd WHERE c1.Kpu_DtUvl < '19500101' and Kpu_Tn<4000000000 and kpu_Fam is not null order by Kpu_Tn");
				Data data = Data.GetInstance();
				data.DataUserCaption(dt, this.dgUserList);
			} catch (Exception ex) {
				MessageBox.Show("При загрузке формы возникли проблемы.");
				currentClassLogger.Error("Users.Users_Load.При загрузке формы возникли проблемы.");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}

		}
		//Поиск по табельному
		private void textBox1_TextChanged(object sender, EventArgs e) {
			textBox2.Text = "";
		   (dgUserList.DataSource as DataTable).DefaultView.RowFilter =
	   String.Format("Kpu_Tn like '{0}%'", textBox1.Text);
		}
		//Поиск по фамилии
		private void textBox2_TextChanged(object sender, EventArgs e) {
			textBox1.Text = "";
			(dgUserList.DataSource as DataTable).DefaultView.RowFilter =
	   String.Format("Fio like '{0}%'", textBox2.Text);
		}
		//функция выбора юзера
		private void CellClick(bool key) {
			try {
				Data data = Data.GetInstance();
				int row;
				DataRow Us;
				String LabelText;
				if (key) {
					row = dgUserList.CurrentRow.Index;
					Us = ((DataRowView)dgUserList.Rows[row].DataBoundItem).Row;
					LabelText = dgUserList.Rows[row].Cells[1].Value.ToString(); 
				} else {
					Us = null;
					LabelText = "Наличный расчет";
				}
				if (data.item == -1) {//Если это новый юзер
					data.orderElement = new OrderElement(Us);
					order = new Order();
					order.Lebeltext = LabelText;
					data.orderGV = order.dataGridOrder;
					data.orderGV.DataSource = data.order;
					data.orderGV.RowHeadersVisible = false;
					order.Show();
					this.Close();
				} else {
					if (data.item == -2) {
						data.orderElement.user = Us;
					} else {
						data.OrderElementList[data.item].user = Us;
					}
					this.Close();//Если редактируем
				}
			} catch (Exception ex) {
				MessageBox.Show("При выборе пользователя возникли проблемы");
				currentClassLogger.Error("Users.CellClick.При выборе пользователя возникли проблемы.");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		private void dgUserList_CellClick(object sender, DataGridViewCellEventArgs e) {
			CellClick(true);
		}
	}
}
