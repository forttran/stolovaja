using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace stolov {
	public partial class Prise : Form {
		public DataTable dt;
		public Prise() {
			InitializeComponent();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (keyData == (Keys.Escape)) {
				SavePrice();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
		private void Prise_Load(object sender, EventArgs e) {
			DB.getInstance();
			dt = DB.Query("select CONVERT(varchar,SklN_Rcd) prcSRcdNom, SklN_Cd sPrArt, SklN_Nm sPrArtNm, SklGr_Nm sPrGrpNm, EI_ShNm sPrEiSh, SklN_NoTrd SklN_NoTrd, Arc.ArcCn sPrCn, SklOpa_Rcd, SklPrc_Rcd, EI_Rcd FROM TrdPrm, SklPrc INNER JOIN SklOpa ON SklPrc.SklPrc_RcdOpa = SklOpa_Rcd INNER JOIN SklN ON SklN_Rcd = SklOpa_RcdNom and SklN_Arc<> 1 INNER JOIN SklGr ON SklN_RcdGrp = SklGr_Rcd Left JOIN Ei ON SklPrc.SklPrc_Ei = Ei_Rcd LEFT JOIN(select CA.SklPrcArc_Prc as SklPrcArc_Prc, CA.sklprcarc_cn1b as ArcCn, CA.sklprcarc_dat as sklprcarc_dat from sklprcarc CA where CA.SklPrcArc_PrcR = 267 AND CA.SklPrcArc_Dat = (SELECT MAX(A1.SklPrcArc_Dat) FROM SklPrcArc A1 WHERE A1.SklPrcArc_PrcR = 267 AND A1.SklPrcArc_Prc = CA.SklPrcArc_Prc GROUP BY A1.SklPrcArc_Prc,A1.SklPrcArc_PrcR) ) Arc ON Arc.SklPrcArc_Prc = SklPrc.SklPrc_Rcd where SklPrc.SklPrc_Cd = 71 order by SklN_NmAlt");
			Data data = Data.getInstance();

			if(data.order.Rows.Count > 0){
				DataRow[] result = data.order.Select("");
				foreach (DataRow row in result) {			
					string arg = "prcSRcdNom='" + row[0].ToString()+"'";
					DataRow[] res = dt.Select(arg);
					foreach (DataRow item in res) {
						item[5] = row[5];
					}
				}
			}

			data.prise = dt;
			data.usersGV = this.dgPriceList;
			this.dgPriceList.DataSource = dt;
			this.dgPriceList.Columns[0].HeaderText = "id";
			this.dgPriceList.Columns[0].Width = 70;
			this.dgPriceList.Columns[1].HeaderText = "Артикул";
			this.dgPriceList.Columns[1].Width = 140;
			this.dgPriceList.Columns[2].HeaderText = "Название";
			this.dgPriceList.Columns[2].Width = 440;
			this.dgPriceList.Columns[3].HeaderText = "Тип";
			this.dgPriceList.Columns[3].Width = 100;
			this.dgPriceList.Columns[4].HeaderText = "Единицы";
			this.dgPriceList.Columns[4].Width = 60;
			this.dgPriceList.Columns[5].HeaderText = "Количество";
			this.dgPriceList.Columns[5].Width = 130;
			this.dgPriceList.Columns[6].HeaderText = "Стоимость";
			this.dgPriceList.Columns[6].Width = 130;	
			this.dgPriceList.Columns[7].Visible = false;
			this.dgPriceList.Columns[8].Visible = false;
			this.dgPriceList.Columns[9].Visible = false;
			this.dgPriceList.Columns[0].Visible = false;
			this.dgPriceList.Columns[1].Visible = false;
			this.dgPriceList.Columns[3].Visible = false;
			this.dgPriceList.Columns[4].Visible = false;
			//this.dgPriceList.ReadOnly = true;
			this.dgPriceList.EditMode = DataGridViewEditMode.EditOnEnter;
			for(int i=0; i<7; i++) {
				this.dgPriceList.Columns[i].ReadOnly = i!=5;
			}
		}
		private void SavePrice() {
			DataRow[] result = dt.Select("SklN_NoTrd <>0");
			if (result.Length > 0) {
				Data data = Data.getInstance();
				data.order = result.CopyToDataTable();
				data.orderGV.DataSource = result.CopyToDataTable();
				data.dataOrderCaption();
			} else {
				MessageBox.Show("Отсутствуют данные для формирования");
			}
			this.Hide();
		}
		private void button1_Click(object sender, EventArgs e) {
			SavePrice();
		}


	}
}
