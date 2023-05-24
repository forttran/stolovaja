using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Filters;
using NLog.Fluent;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Data;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using stolov.Properties;
using System.IO;

namespace stolov {
	internal class OrdersToISPRO {

		public static Logger currentClassLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		//функция резервирования rcd
		public static string Reserved_Rcd(string TableName, string ColumnName) {
			string verification = "declare @RetRcd int  "
								+ "DECLARE @DB VARCHAR(40) = DB_NAME() "
								+ "DECLARE @FirmRcd BIGINT = (SELECT TOP 1 TrdPrm_CdFirm FROM TRDPRM) "
								+ " exec spSysGetRcd @DB,'" + TableName + "','" + ColumnName + "',@FirmRcd,4294967295,@RetRcd output  "
								+ " select @RetRcd";
			string RCD = string.Empty;

			try { 
				QueryManager.ExecuteQuery(verification); 
			} catch (Exception excp) {
				MessageBox.Show("Ошибка генератора Rcd");
				currentClassLogger.Error("Ошибка генератора Rcd: " + excp.Message + "; Source: " + excp.Source); return "-1"; 
			}
			object temp = QueryManager.GetResultObject();
			if (temp != null) RCD = temp.ToString();
			return RCD;
		}
		public string getRcdNmr() {
			string date = DateTime.Today.ToString("yyyyMMdd");
			string sql = "SELECT ISNULL(MAX(PrdZkg_RcdNmr),0)  FROM PRDZKG WHERE PrdZkg_RcdNmr < 9999999 and (PRDZKG_Dt = CONVERT(date, '" + date + "') ) ";
			try {
				DB.getInstance();
				DataTable dt = DB.Query(sql);
				DataRow[] result = dt.Select();
				int res = Convert.ToInt32(result[0][0]) + 1;
				return res.ToString();
			} catch (Exception e) {
				MessageBox.Show("Ошибка получения RcdNmr");
				currentClassLogger.Error(sql);
				currentClassLogger.Error("Ошибка получения RcdNmr");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
				return "-1";
			};
		}
		public void insertOrder(string PrdZkg_Rcd, string RcdNmr, string date, double sum, double nalog, double usr, int tab) {
			currentClassLogger.Debug("date=" + date);
			string sql = "";
			sql = "BEGIN TRY "
				+ "BEGIN TRAN "

				+ "declare @dat VARCHAR(8) "
				+ "set @dat = '"+date+"' "
				+ "DECLARE @RcdNmr BIGINT "
				+ "DECLARE @StrNmr VARCHAR(20) "
				+ "SET @RcdNmr = " + RcdNmr
				+ "IF @RcdNmr > 10000000 SET @RcdNmr = 10000000 "
				+ "SET @StrNmr = RIGHT('00000000'+CONVERT(VARCHAR(20),@RcdNmr),8) "

				+ "DECLARE @sum FLOAT "
				+ "DECLARE @nalog FLOAT "
				+ "DECLARE @usr BIGINT "
				+ "set @sum = REPLACE('" + sum + "',',','.')" 
				+ " set @nalog = REPLACE('" + nalog + "',',','.')"
				+ " set @usr = " + usr
				+ " INSERT INTO PrdZkg("
				+ " PrdZkg_Rcd,  PrdZkg_JrnRcd, PrdZkg_Nmr,  PrdZkg_Dt, PrdZkg_DtOtg, PrdZkg_UslD, PrdZkg_KAgID, PrdZkg_RcvrID, PrdZkg_RcvrNm,  PrdZkg_RcdNmr,  PrdZkg_DtOpl, PrdZkg_DtRes, PrdZkg_Sm,  PrdZkg_ValID, PrdZkg_ValKot, PrdZkg_ValKur, PrdZkg_AvSum, PrdZkg_SmNds, PrdZkg_SmTax, PrdZkg_SmWoNds, PrdZkg_TaxMdl, PrdZkg_Prior, PrdZkg_CdUsr, PrdZkg_DocTim"
				+ " )values("
				+ PrdZkg_Rcd+ ",  75, @StrNmr,  @dat, @dat, 1, 4869, 4869, 'ПОКУПАТЕЛЬ СТОЛОВОЙ',  @RcdNmr, @dat, @dat, @sum, 1, 0, 1.0, 0, @nalog, @nalog, @sum-@nalog, 13, 1, @usr, GETDATE())"
				//+ "--select * from PRDZKG where PrdZkg_JrnRcd=75"
				+ " COMMIT TRAN"
				+ " END TRY"
				+ " BEGIN CATCH"
				+ " ROLLBACK TRAN"
				+ " END CATCH";
			try {
				//SqlDataReader dr = command.ExecuteReader();
				QueryManager.ExecuteNonQuery(sql);
			} catch (Exception e) {
				MessageBox.Show("Ошибка вставки в таблицу PrdZkg");
				currentClassLogger.Error(sql);
				currentClassLogger.Error("Ошибка вставки в таблицу PrdZkg");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
			};
			if(tab != -1) {
				sql = "insert into UFPRV (UF_TblId,UF_TblRcd,UF_RkRcd,UF_RkValS )values (391, " + PrdZkg_Rcd + "  ,286," + tab + ")";
				try {
					//SqlDataReader dr = command.ExecuteReader();
					QueryManager.ExecuteNonQuery(sql);
				} catch (Exception e) {
					MessageBox.Show("Ошибка вставки в таблицу UFPRV");
					currentClassLogger.Error(sql);
					currentClassLogger.Error("Ошибка вставки в таблицу UFPRV");
					currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
				};
			}

			sql = "insert into UFPRV(UF_TblId, UF_TblRcd, UF_RkRcd, UF_RkValS)values(391, " + PrdZkg_Rcd + ", 287, (select SklPrcRst_Sh from SKLPRCRST where SklPrcRst_Rcd = 226))";

			try {
				//SqlDataReader dr = command.ExecuteReader();
				QueryManager.ExecuteNonQuery(sql);
			} catch (Exception e) {
				MessageBox.Show("Ошибка вставки в таблицу UFPRV");
				currentClassLogger.Error(sql);
				currentClassLogger.Error("Ошибка вставки в таблицу UFPRV");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
			};
		}
		public void insertPosition(string trds_rcd, string zkg_rcd, string date, string RcdOra, string RcdNom, string RcdPrc, string EiQt, string cnt, string sum, string tax) {
			string sql = "";
			sql = "declare @dt VARCHAR(8) "
				+ "set @dt = '"+date+"' "
				+ "DECLARE @cnt FLOAT "
				+ "DECLARE @sum FLOAT "
				+ "DECLARE @tax FLOAT "
				+ "set @cnt = REPLACE('" + cnt + "',',','.')" 
				+ " set @sum = REPLACE('" + sum + "',',','.')" 
				+ " set @tax = REPLACE('" + tax + "',',','.')" 
				+ " INSERT INTO TRDS(TrdS_Rcd, TrdS_TypHdr, TrdS_RcdHdr, TrdS_Dat, TrdS_KAgRcd, TrdS_RcdOpa, TrdS_RcdNom, TrdS_Dest, TrdS_RcdPrc, TrdS_EiQt, TrdS_EiCn, TrdS_Qt, TrdS_QtOsn, TrdS_QtCn, TrdS_CnFul, TrdS_Cn, TrdS_CnSkl, TrdS_Sum, TrdS_SumOpl, TrdS_SumSkl, TrdS_SumTax, TrdS_SumTSk, TrdS_CnExt, TrdS_SumExt, TrdS_ValCd, TrdS_ValKur, TrdS_TpCn, TrdS_CdPrc, TrdS_Dat1, TrdS_Dat2, TrdS_NewFlg"
				+ " )values("
				+ trds_rcd + ", 17, " + zkg_rcd + ", @dt, 4869, " + RcdOra + ", " + RcdNom + ", 2, " + RcdPrc + ", " + EiQt + ", " + EiQt + ", @cnt, @cnt, @cnt, @sum, @sum, @sum, @sum*@cnt, @sum*@cnt, @sum*@cnt, @tax, -@tax, @sum, @sum*@cnt, 1, 1.0, 1, 267, @dt, @dt, 1)";
			try {
				//SqlDataReader dr = command.ExecuteReader();
				QueryManager.ExecuteNonQuery(sql);
			} catch (Exception e) {
				MessageBox.Show("Ошибка вставки в таблицу TRDS");
				currentClassLogger.Error(sql);
				currentClassLogger.Error("Ошибка вставки в таблицу TRDS");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
			};

			sql = "INSERT INTO ATrdsTax " +
				  "( ATrdsTax_RcdS,   " +
				  "    ATrdsTax_RcdAs,  " +
				  "    TrdsTax_cd,  " +
				  "    TrdsTax_RateCd,   " +
				  "    TrdsTax_Sum,   " +
				  "    TrdsTax_SumBas,   " +
				  "   TrdsTax_flg   " +
				  "  ) " +
				  "SELECT  " + trds_rcd + " AS ATrdsTax_RcdS,  " +
				  " 0 AS ATrdsTax_RcdAs,   " +
				  " TAX.NmTax_Cd AS TrdsTax_cd,   " +
				  " TAX.NmTax_CdRate AS TrdsTaxe_RateCd,   " +
				  " ROUND(h.Trds_SumTax,2) AS TrdsTax_Sum,   " +
				  " ROUND(h.Trds_Sum,2) AS TrdsTax_SumBas," +
				  " 4 AS TrdsTax_flg   " +
				  " FROM Trds AS h  " +
				  " LEFT JOIN SKLNOMTAX AS TAX ON TAX.NmTax_RcdPar = H.TrdS_RcdNom  and TAX.NmTax_Type = 2   " +
				  " LEFT JOIN TAXRATE AS NDS ON NDS.TaxRate_Rcd = TAX.NmTax_CdRate   " +
				  " WHERE h.Trds_RcdHdr = " + zkg_rcd +
				  "  AND h.Trds_Rcd =  " + trds_rcd +
				  "  AND Trds_TypHdr = 17; ";

			try {
				//SqlDataReader dr = command.ExecuteReader();
				QueryManager.ExecuteNonQuery(sql);
			} catch (Exception e) {
				MessageBox.Show("Ошибка вставки в таблицу ATrdsTax");
				currentClassLogger.Error(sql);
				currentClassLogger.Error("Ошибка вставки в таблицу ATrdsTax");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
			};
		}

		public void generateFile(string RcdNmr, OrderElement elements) {
			//генерируем первый файл
			Data data = Data.getInstance();
			try {
				string fileName = "R" + DateTime.Today.ToString("yyyy") + RcdNmr.PadLeft(8, '0');
				string path = Settings.Default.GeneratePath + "\\" + fileName + ".TXT";
				using (FileStream fileStream = File.Open(path, FileMode.Create)) {
					using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream, Encoding.UTF8)) {
						DataRow[] result = elements.order.Select();
						foreach (DataRow row in result) {
							string str12 = row[2].ToString().Remove(0, 2) + ";1;" + row[5].ToString() + ";" + Convert.ToDouble(row[6]).ToString("0.##") + ";" + row[4].ToString() + ";";
							str12 += "1;";//секция
							str12 += "0;";//сумма скидки
							str12 += "1;";//код налога

							//Если нет табельного, то нал, иначе безнал
							if(elements.user == null || elements.ret == true) {
								str12 += "0;";//0-нал;1-безнал
								str12 += "4;";//способ расчета по данному чеку 4-безнал 6-нал
							} else {
								str12 += "1;";//0-нал;1-безнал
								str12 += "6;";//способ расчета по данному чеку 4-безнал 6-нал
							}

							str12 += "1;";//код системы налогооблажения
							str12 += "1;";//код предмета расчета
							//MessageBox.Show("RcdNmr = " + RcdNmr + " elements.ret = " + elements.ret);
							if(elements.ret == true) {
								str12 += "1;";
							} else {
								str12 += "0;";
							}//код операции 0 - покупка 1 - возврат
							str12 += "secretary_mgn @rossmol.ru;";//email
							str12 += "3519393343";//телефон
							streamWriter.WriteLine(str12);
						}
					}
				}
			} catch (Exception e) {
				MessageBox.Show("Ошибка записи первого файла");
				currentClassLogger.Error("Ошибка записи первого файла");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
			}
			//генерируем второй фаил
			try {
				string fileName = "P" + DateTime.Today.ToString("yyyy") + RcdNmr.PadLeft(8, '0');
				string path = Settings.Default.GeneratePath + "\\" + fileName + ".TXT";
				using (FileStream fileStream = File.Open(path, FileMode.Create)) {
					using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream, Encoding.UTF8)) {
						string str12;
						if (elements.user == null || elements.ret == true) {
							str12 = "0;" + elements.getSum().ToString("0.##");
						} else {
							str12 = "3;" + elements.getSum().ToString("0.##");
						}
						streamWriter.WriteLine(str12);
					}
				}
			} catch (Exception e) {
				MessageBox.Show("Ошибка записи второго файла");
				currentClassLogger.Error("Ошибка записи второго файла");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
			}
		}
		public void TMPtoPrdZkg() {
			Data data = Data.getInstance();
			DateTime today = DateTime.Today;
			prdzkgt(today.ToString("yyyyMMdd"));
			int index = 0;
			foreach (OrderElement item in data.OrderElementList.ToArray()) {
				try {
					string PrdZkg_Rcd = Reserved_Rcd("PrdZkg", "PrdZkg_Rcd");//резервируем rcd в таблице PrdZkg
					string RcdNmr = getRcdNmr();
					int tab = -1;
					if(item.user != null) {
						tab = Convert.ToInt32(item.user[0]);
					}
					insertOrder(PrdZkg_Rcd, RcdNmr, today.ToString("yyyyMMdd"), item.getSum(), item.getTax(), 34, tab);
					currentClassLogger.Debug("zkg=" + PrdZkg_Rcd + " sum=" + item.getSum());
					DataRow[] result = item.order.Select();
					foreach (DataRow row in result) {
						string TrdS_Rcd = Reserved_Rcd("TrdS", "TrdS_Rcd");//резервируем rcd в таблице TrdS
						currentClassLogger.Debug("|" + row[0] + "|" + row[1] + "|" + row[2] + "|" + row[3] + "|" + row[4] + "|" + row[5] + "|" + row[6] + "|" + row[7] + "|" + row[8] + "|" + row[9]);
						insertPosition(TrdS_Rcd, PrdZkg_Rcd, today.ToString("yyyyMMdd"), row[7].ToString(), row[0].ToString(), row[8].ToString(), row[9].ToString(), row[5].ToString(), row[6].ToString(), (Convert.ToDouble(row[6]) * 0.13).ToString());
					}
					generateFile(RcdNmr, item);
				} catch (Exception e) {
					MessageBox.Show("Ошибка итерации");
					currentClassLogger.Error("Ошибка итерации");
					currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
				}
				data.OrderElementListView.RemoveAt(0);
				DataXML xml = new DataXML();
				xml.DeleteXML(0);
				index++;
			}
			data.OrderElementList.Clear();
		}

		public void prdzkgt(string date) {
			string sql = "select count(*) from PRDZKDT where PrdZkDt_JrRcd='75' and PrdZkDt_Dt='" + date + "' ";
			try {
				DB.getInstance();
				DataTable dt = DB.Query(sql);
				DataRow[] result = dt.Select();
				if (Convert.ToInt32(result[0][0]) == 0) {
					sql = "insert into PRDZKDT (PrdZkDt_JrRcd, PrdZkDt_Dt)values(75,'" + date + "')";
					DB.Query(sql);
				}
			} catch (Exception e) {
				MessageBox.Show("Ошибка работы с таблицей PRDZKDT");
				currentClassLogger.Error(sql);
				currentClassLogger.Error("Ошибка работы с таблицей PRDZKDT");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
			};
		}
	}
}
