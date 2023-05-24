using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stolov {
	internal class Data {
		private static Data instance;
		public int idXML = 0;
		//Пользователи
		public DataTable users;
		public DataGridView usersGV;

		//Прайс
		public DataTable prise;

		//Заказ
		public DataTable order;
		public DataGridView orderGV;

		public int item;

		//Список заказов
		public OrderElement orderElement;
		public List<OrderElement> OrderElementList = new List<OrderElement>();
		public BindingList<OrderElementView> OrderElementListView = new BindingList<OrderElementView>();
		public DataGridView OrderElementListGV;
		public static Data getInstance() {
			if (instance == null)
				instance = new Data();
			return instance;
		}
		private Data() {
			order = new DataTable();
			prise = new DataTable();
			item = -1;
		}
		public void dataOrderCaption() {
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

		public void elementRemove(int row) {
			OrderElementList.RemoveAt(row);
			OrderElementListView.RemoveAt(row);
			dataXML xml = new dataXML();
			xml.deleteXML(row);
		}
	}
}
