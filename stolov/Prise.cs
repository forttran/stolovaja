using NLog.Web;
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
using NLog;
using NLog.Filters;
using NLog.Fluent;
using NLog.Web;

namespace stolov {
	public partial class Prise : Form {
		public static Logger currentClassLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		public DataTable dt;
		public Prise() {
			InitializeComponent();
		}
		//функция для подключения обработчика нажатия клавиш
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			if (keyData == (Keys.Escape)) {//При нажатии esc закрыть окно и сохранить прайс
				SavePrice();
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}
		//Функция загразки прайса
		private void Prise_Load(object sender, EventArgs e) {
			try {
				DB.getInstance();
				dt = DB.Query("select CONVERT(varchar,SklN_Rcd) prcSRcdNom, SklN_Cd sPrArt, SklN_Nm sPrArtNm, SklGr_Nm sPrGrpNm, EI_ShNm sPrEiSh, SklN_NoTrd SklN_NoTrd, Arc.ArcCn sPrCn, SklOpa_Rcd, SklPrc_Rcd, EI_Rcd FROM TrdPrm, SklPrc INNER JOIN SklOpa ON SklPrc.SklPrc_RcdOpa = SklOpa_Rcd INNER JOIN SklN ON SklN_Rcd = SklOpa_RcdNom and SklN_Arc<> 1 INNER JOIN SklGr ON SklN_RcdGrp = SklGr_Rcd Left JOIN Ei ON SklPrc.SklPrc_Ei = Ei_Rcd LEFT JOIN(select CA.SklPrcArc_Prc as SklPrcArc_Prc, CA.sklprcarc_cn1b as ArcCn, CA.sklprcarc_dat as sklprcarc_dat from sklprcarc CA where CA.SklPrcArc_PrcR = 267 AND CA.SklPrcArc_Dat = (SELECT MAX(A1.SklPrcArc_Dat) FROM SklPrcArc A1 WHERE A1.SklPrcArc_PrcR = 267 AND A1.SklPrcArc_Prc = CA.SklPrcArc_Prc GROUP BY A1.SklPrcArc_Prc,A1.SklPrcArc_PrcR) ) Arc ON Arc.SklPrcArc_Prc = SklPrc.SklPrc_Rcd where SklPrc.SklPrc_Cd = 71 order by SklN_NmAlt");
				Data data = Data.GetInstance();
				//Если в заказе уже что-то есть, то отображаем это в прайсе
				if (data.order.Rows.Count > 0) {
					DataRow[] result = data.order.Select("");
					foreach (DataRow row in result) {
						string arg = "prcSRcdNom='" + row[0].ToString() + "'";
						DataRow[] res = dt.Select(arg);
						foreach (DataRow item in res) {
							item[5] = row[5];
						}
					}
				}
				data.DataPriseCaption(dt, this.dgPriceList);
			} catch (Exception ex) {
				MessageBox.Show("При загрузке формы возникли проблемы.");
				currentClassLogger.Error("Prise.Prise_Load.При загрузке формы возникли проблемы.");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		//Сохраняем результаты выбора из прайса в заказ
		private void SavePrice() {
			try {
				DataRow[] result = dt.Select("SklN_NoTrd <>0");
				if (result.Length > 0) {
					Data data = Data.GetInstance();
					data.order = result.CopyToDataTable();
					data.orderGV.DataSource = result.CopyToDataTable();
					data.orderGV.Visible = true;
					data.DataOrderCaption();
				} else {
					MessageBox.Show("Отсутствуют данные для формирования");
				}
			} catch (Exception ex) {
				MessageBox.Show("При сохранении прайса возникли проблемы.");
				currentClassLogger.Error("Prise.SavePrice.При сохранении прайса возникли проблемы.");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
			//this.Close();
			this.Hide();
		}
		private void Button1_Click(object sender, EventArgs e) {
			 SavePrice();
		}
	}
}
