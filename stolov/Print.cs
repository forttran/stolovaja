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
		public PrintDocument printDocument = new();

		private readonly int num;
		private readonly string tab;
		private readonly List<OrderItem> Orders;
		private readonly string FIO;
		//Инициализация объекта печати
		public Print(DataTable Orders, int num, string FIO, string tab) {
			try {
				this.num = num;
				this.FIO = FIO;
				this.tab = tab;
				this.Orders = new List<OrderItem>();
				DataRow[] result = Orders.Select();
				foreach (DataRow row in result) {
					OrderItem Item = new(row[2].ToString().Remove(0, 2), Convert.ToDouble(row[5]), Convert.ToDouble(row[6]));
					this.Orders.Add(Item);
				}
				PrintOrder();
			} catch (Exception e) {
				MessageBox.Show("При инициализации объекта печати возникли проблемы");
				currentClassLogger.Error("Print.Print.При инициализации объекта печати возникли проблемы");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
			};
		}
		public void PrintOrder() {
			try {
				PrintDocument pd = new();
				pd.PrintPage += new PrintPageEventHandler(PrintPageHandler);
				pd.PrinterSettings.PrinterName = Settings.Default.Prints;
				pd.PrinterSettings.PrintFileName = @"C:\1.pdf";
				pd.DocumentName = "Чек столовой №" + this.num.ToString();
				pd.Print();
			} catch (Exception e) {
				MessageBox.Show("Зайдите в настройки и выберете принтер на котором будут печататься чеки");
				currentClassLogger.Error("Print.PrintOrder.Зайдите в настройки и выберете принтер на котором будут печататься чеки");
				currentClassLogger.Error("Ошибка: " + e.Message + "; Source: " + e.Source);
			}

		}
		//Настройка печати
		public void PrintPageHandler(object sender, PrintPageEventArgs e) {
			// печать строки result
			try {
				e.Graphics.DrawString(GetStringPrint(), new Font("Lucida Console", Settings.Default.sizePrint, FontStyle.Bold), Brushes.Black, 0, 0);
			} catch (Exception ex) {
				MessageBox.Show("При настройке печати возникли проблемы");
				currentClassLogger.Error("Print.PrintPageHandler.При настройке печати возникли проблемы");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		//Центрирование записей в чеке
		public string ToCenter(string str) {
			try {
				int widthСheck = 54;
				int c = (widthСheck - str.Count()) / 2;
				string result = "".PadLeft(c, ' ') + str + "\n";
				return result;
			} catch (Exception ex) {
				currentClassLogger.Error("Print.ToCenter.Ошибка");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
				return str;
			}
		}
		//Функция генерирования чека для печати
		public string GetStringPrint() {
			string caption = "";
			string[] header = new string[6];
			string[] footer = new string[6];
			string result = "";
			double sum = 0;
			try {
				try {
					string line;
					StreamReader streamWriter = new(Settings.Default.holidayMsg);
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
							start += cnt;
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
			} catch (Exception ex) {
				MessageBox.Show("При генерации чека возникли проблемы");
				currentClassLogger.Error("Print.GetStringPrint.При генерации чека возникли проблемы");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
				return "";
			}
			
		}
	}
}
