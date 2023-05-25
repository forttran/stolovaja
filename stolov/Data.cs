using NLog.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using NLog.Filters;
using NLog.Fluent;
using NLog.Web;

namespace stolov {
	internal class Data {

		public static Logger currentClassLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

		private static Data instance;
		public int idXML = 0;
		//Пользователи
		public DataTable users;
		public DataGridView usersGV;
		//Прайс
		public DataTable prise;
		public DataGridView priseGV;

		//Заказ
		public DataTable order;
		public DataGridView orderGV;

		//текущий заказ
		public int item;

		//Список заказов(главная структура)
		public OrderElement orderElement;
		public List<OrderElement> OrderElementList = new();

		//Вьюшки для отобращения списка заказов
		public BindingList<OrderElementView> OrderElementListView = new();
		public DataGridView OrderElementListGV;

		//Реализация паттерна singleton
		public static Data GetInstance() {
			instance ??= new Data();
			return instance;
		}
		//минимальная инициализация в конструкторе
		private Data() {
			order = new DataTable();
			prise = new DataTable();
			item = -1;
		}
		//Генерация шапки главной формы
		public void ListOrderElementsCaptions() {
			OrderElementListGV.Columns[0].HeaderText = "Номер";
			OrderElementListGV.Columns[0].Width = 50;
			OrderElementListGV.Columns[1].HeaderText = "Дата";
			OrderElementListGV.Columns[1].Width = 80;
			OrderElementListGV.Columns[2].HeaderText = "ФИО";
			OrderElementListGV.Columns[2].Width = 270;
			OrderElementListGV.Columns[3].HeaderText = "Сумма";
			OrderElementListGV.Columns[3].Width = 130;
		}
		//Генерация шапки Order
		public void DataOrderCaption() {
			orderGV.Columns[0].HeaderText = "id";
			orderGV.Columns[0].Width = 70;
			orderGV.Columns[1].HeaderText = "Артикул";
			orderGV.Columns[1].Width = 140;
			orderGV.Columns[2].HeaderText = "Название";
			orderGV.Columns[2].Width = 440;
			orderGV.Columns[3].HeaderText = "Тип";
			orderGV.Columns[3].Width = 100;
			orderGV.Columns[4].HeaderText = "Единицы";
			orderGV.Columns[4].Width = 60;
			orderGV.Columns[5].HeaderText = "Количество";
			orderGV.Columns[5].Width = 130;
			orderGV.Columns[6].HeaderText = "Стоимость";
			orderGV.Columns[6].Width = 130;
			orderGV.Columns[7].Visible = false;
			orderGV.Columns[8].Visible = false;
			orderGV.Columns[9].Visible = false;
			orderGV.Columns[0].Visible = false;
			orderGV.Columns[1].Visible = false;
			orderGV.Columns[3].Visible = false;
			orderGV.Columns[4].Visible = false;
			for (int i = 0; i < 7; i++) {
				orderGV.Columns[i].ReadOnly = i != 5;
			}
		}
		//Генерация шапки Prise
		public void DataPriseCaption(DataTable dt, DataGridView dgPriceList) {
			prise = dt;
			priseGV = dgPriceList;
			priseGV.DataSource = dt;
			priseGV.Columns[0].HeaderText = "id";
			priseGV.Columns[0].Width = 70;
			priseGV.Columns[1].HeaderText = "Артикул";
			priseGV.Columns[1].Width = 140;
			priseGV.Columns[2].HeaderText = "Название";
			priseGV.Columns[2].Width = 440;
			priseGV.Columns[3].HeaderText = "Тип";
			priseGV.Columns[3].Width = 100;
			priseGV.Columns[4].HeaderText = "Единицы";
			priseGV.Columns[4].Width = 60;
			priseGV.Columns[5].HeaderText = "Количество";
			priseGV.Columns[5].Width = 130;
			priseGV.Columns[6].HeaderText = "Стоимость";
			priseGV.Columns[6].Width = 130;
			priseGV.Columns[7].Visible = false;
			priseGV.Columns[8].Visible = false;
			priseGV.Columns[9].Visible = false;
			priseGV.Columns[0].Visible = false;
			priseGV.Columns[1].Visible = false;
			priseGV.Columns[3].Visible = false;
			priseGV.Columns[4].Visible = false;
			//this.dgPriceList.ReadOnly = true;
			priseGV.EditMode = DataGridViewEditMode.EditOnEnter;
			for (int i = 0; i < 7; i++) {
				priseGV.Columns[i].ReadOnly = i != 5;
			}
			dgPriceList.CurrentCell = dgPriceList.Rows[0].Cells[5];
		}

		public void DataUserCaption(DataTable dt, DataGridView dgUserList) {
			users = dt;
			usersGV = dgUserList;
			usersGV.DataSource = dt;
			usersGV.Columns[0].HeaderText = "Табельный";
			usersGV.Columns[0].Width = 70;
			usersGV.Columns[1].HeaderText = "ФИО";
			usersGV.Columns[1].Width = 240;
			usersGV.Columns[2].HeaderText = "Подразделение";
			usersGV.Columns[2].Width = 180;
			usersGV.Columns[3].HeaderText = "Дата устройства";
			usersGV.Columns[3].Width = 100;
			usersGV.Columns[4].HeaderText = "Дата рождения";
			usersGV.Columns[4].Width = 100;
			usersGV.Columns[5].HeaderText = "Должность";
			usersGV.Columns[5].Width = 200;
		}

			//функция удаления заказа
			public void ElementRemove(int row) {
			try {
				OrderElementList.RemoveAt(row);
				OrderElementListView.RemoveAt(row);
				DataXML xml = new();
				xml.DeleteXML(row);
			}catch(Exception ex) {
				MessageBox.Show("При удалении заказа возникли проблемы.");
				currentClassLogger.Error("Data.ElementRemove.При удалении заказа возникли проблемы.");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
	}
}
