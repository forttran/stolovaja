using NLog.Web;
using stolov.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using NLog;
using NLog.Filters;
using NLog.Fluent;
using System.Runtime.Remoting.Messaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace stolov {
	internal class Print {
		public static Logger currentClassLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		// объект для печати
		public PrintDocument printDocument = new PrintDocument();

		private int num;
		private string tab;
		private List<OrderItem> Orders;
		private string FIO;

		public Print(List<OrderItem>  Orders, int num, string FIO, string tab) {
			this.Orders = Orders;
			this.num = num;
			this.FIO = FIO;
			this.tab = tab;
			print();
		}
		public Print(DataTable Orders, int num, string FIO, string tab) {
			this.num = num;
			this.FIO = FIO;
			this.tab = tab;
			this.Orders = new List<OrderItem>();
			DataRow[] result = Orders.Select();
			foreach (DataRow row in result) {
				OrderItem Item = new(row[2].ToString().Remove(0, 2), Convert.ToDouble(row[5]), Convert.ToDouble(row[6]));
				this.Orders.Add(Item);
			}
			print();
		}
		private PrintDocument PD = new PrintDocument();
		public void print() {
			/*
			// обработчик события печати
			printDocument.PrintPage += PrintPageHandler;
			// диалог настройки печати
			PrintDialog printDialog = new PrintDialog();
			// установка объекта печати для его настройки
			printDialog.Document = printDocument;
			// если в диалоге было нажато ОК
			if (printDialog.ShowDialog() == DialogResult.OK)
				printDialog.Document.Print(); // печатаем
			*/
			try {
				PrintDocument pd = new PrintDocument();
				pd.PrintPage += new PrintPageEventHandler(PrintPageHandler);
				pd.PrinterSettings.PrinterName = Settings.Default.Prints;
				pd.PrinterSettings.PrintFileName = @"C:\1.pdf";
				pd.DocumentName = "Чек столовой №" + this.num.ToString();
				pd.Print();
			} catch {
				MessageBox.Show("Зайдите в настройки и выберете принтер на котором будут печататься чеки");
			}

		}
		public void PrintPageHandler(object sender, PrintPageEventArgs e) {
			// печать строки result
			e.Graphics.DrawString(getStringPrint(), new Font("Lucida Console", Settings.Default.sizePrint, FontStyle.Bold), Brushes.Black, 0, 0);
		}
		public string ToCenter(string str) {
			int widthСheck = 54;
			string result = "";
			int c = (widthСheck - str.Count()) / 2;
			result = "".PadLeft(c, ' ') + str + "\n";
			return result;
		}
		public string getStringPrint() {
			string caption = "";
			string[] header = new string[6];
			string[] footer = new string[6];
			string result = "";
			double sum = 0;

			try {
				string line;
				StreamReader streamWriter = new StreamReader(Settings.Default.holidayMsg);
				while ((line = streamWriter.ReadLine()) != null) {
					caption += ToCenter(line);
				}
				streamWriter.Close();

			} catch (Exception e) {
				currentClassLogger.Error("Ошибка чтения файла заголовка чека");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
			}

			result += caption;

			header[0] = ToCenter("Чек №" + this.num.ToString().PadRight(3) + " от " + DateTime.Now.ToString("dd/MM/yyyy"));
			header[1] = "Покупатель: ПОКУПАТЕЛЬ СТОЛОВОЙ\n";
			header[2] = "Таб № " + this.tab + " " + this.FIO + "\n";
			header[3] = "----------------------------------+----+------+-------\n";
			header[4] = "Наименование товара               |К-во| Цена | Сумма \n";
			header[5] = "----------------------------------+----+------+-------\n";

			foreach (string st in header) {
				result += st;
			}

			foreach (OrderItem Item in this.Orders) {
				int cnt = 34;//размер поля
				int start = 0;
				int i = 0;
				string[] mn = new string[Item.name.Length / cnt + 1];

				Item.sum = Item.count * Item.price;

				if (Item.name.Length < cnt) {
					result += Item.name.PadRight(34) + "|" + Item.count.ToString("F").PadLeft(4) + "|" + Item.price.ToString("F").PadLeft(6) + "|" + Item.sum.ToString("F").PadLeft(7) + "\n";
				} else {
					while (start + cnt < Item.name.Length) {
						mn[i] = Item.name.Substring(start, cnt);
						start = start + cnt;
						i++;
					}
					if (Item.name.Length > start) {
						mn[i] = Item.name.Substring(start);
					}
					for (int j = 0; j < mn.Length; j++) {
						if (j == 0)
							result += mn[j].PadRight(34) + "|" + Item.count.ToString("F").PadLeft(4) + "|" + Item.price.ToString("F").PadLeft(6) + "|" + Item.sum.ToString("F").PadLeft(7) + "\n";
						else
							result += mn[j].PadRight(34) + "|" + "".PadLeft(4) + "|" + "".PadLeft(6) + "|" + "".ToString().PadLeft(7) + "\n";
					}
				}
				sum += Item.sum;
			}
			footer[0] = "----------------------------------+----+------+-------\n";
			footer[1] = "                                         ИТОГ  " + sum.ToString("F").PadLeft(7) + "\n";

			foreach (string st in footer) {
				result += st;
			}

			return result;
		}
	}
}
