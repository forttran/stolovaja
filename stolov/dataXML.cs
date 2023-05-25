using NLog.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using NLog;
using NLog.Filters;
using NLog.Fluent;
using System.IO;
namespace stolov {
	internal class DataXML {
		public static Logger currentClassLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		public DataXML() { }
		private XElement GenerateElement(OrderElement orderElement) {
			XElement order;
			XElement user;
			Data data = Data.GetInstance();
			try {
				if (orderElement.user != null) {
					XAttribute type = new("Type", 1);
					XAttribute Kpu_Tn = new("Kpu_Tn", orderElement.user[0]);
					XAttribute Fio = new("Fio", orderElement.user[1]);
					XAttribute SprPdr_NmFull = new("SprPdr_NmFull", orderElement.user[2]);
					XAttribute Kpu_DtNpSt = new("Kpu_DtNpSt", orderElement.user[3]);
					XAttribute Kpu_DtRoj = new("Kpu_DtRoj", orderElement.user[4]);
					XAttribute Dol = new("Dol", orderElement.user[5]);
					user = new("user", type, Kpu_Tn, Fio, SprPdr_NmFull, Kpu_DtNpSt, Kpu_DtRoj, Dol);
				} else {
					XAttribute type = new("Type", 0);
					user = new("user", type);
				}

				XElement lists = new("lists");
				for (int i = 0; i <= orderElement.order.Rows.Count - 1; i++) {
					XAttribute prcSRcdNom = new("prcSRcdNom", orderElement.order.Rows[i][0]);
					XAttribute sPrArt = new("sPrArt", orderElement.order.Rows[i][1]);
					XAttribute sPrArtNm = new("sPrArtNm", orderElement.order.Rows[i][2]);
					XAttribute sPrGrpNm = new("sPrGrpNm", orderElement.order.Rows[i][3]);
					XAttribute sPrEiSh = new("sPrEiSh", orderElement.order.Rows[i][4]);
					XAttribute SklN_NoTrd = new("SklN_NoTrd", orderElement.order.Rows[i][5]);
					XAttribute sPrCn = new("sPrCn", orderElement.order.Rows[i][6]);
					XAttribute SklOpa_Rcd = new("SklOpa_Rcd", orderElement.order.Rows[i][7]);
					XAttribute SklPrc_Rcd = new("SklPrc_Rcd", orderElement.order.Rows[i][8]);
					XAttribute EI_Rcd = new("EI_Rcd", orderElement.order.Rows[i][9]);
					XElement list = new("list", prcSRcdNom, sPrArt, sPrArtNm, sPrGrpNm, sPrEiSh, SklN_NoTrd, sPrCn, SklOpa_Rcd, SklPrc_Rcd, EI_Rcd);
					lists.Add(list);
				}
				int id;
				if(data.item == -1) {
					data.idXML++;
					id = data.idXML;
				} else {
					id = orderElement.id;
				}
				order = new XElement("order",
						new XAttribute("sum", orderElement.GetSum()),
						new XAttribute("id", id),
						new XAttribute("ret", orderElement.ret),
						user, lists);
			}catch(Exception ex) {
				MessageBox.Show("Ошибка генерации элемента");
				currentClassLogger.Error("DataXML.GenerateElement.Ошибка генерации элемента");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
				order = new XElement("order");
			}
			return order;
		}
		public void GetXML() {//Получаем данные из XML файла
			try {
				Data data = Data.GetInstance();
				//считываем данные из файла
				XDocument xdoc;
				try {
					xdoc = XDocument.Load("orders.xml");
				} catch {
					xdoc = new XDocument(new XElement("orders"));
				}
				XElement orders = xdoc.Element("orders");

				if (orders is not null) {
					// проходим по всем элементам orders
					int index = 1;
					foreach (XElement order in orders.Elements("order")) {
						//получаем максимальный айдишник
						data.idXML = Math.Max(data.idXML, Int32.Parse(order.Attribute("id").Value));

						//Создаем пустой заказ
						OrderElement el = new(Int32.Parse(order.Attribute("id").Value)) {
							ret = Convert.ToBoolean(order.Attribute("ret").Value)
						};
						//Создаем объект юзера
						XElement user = order.Element("user");
						if(Int32.Parse(user.Attribute("Type").Value) == 1) {
							String[] namesUser = { "Kpu_Tn", "Fio", "SprPdr_NmFull", "Kpu_DtNpSt", "Kpu_DtRoj", "Dol" };
							DataTable dt = new();
							DataRow drow = dt.NewRow();
							int i = 0;
							foreach (string item in namesUser) {
								dt.Columns.Add(item);
								drow[i] = user.Attribute(item).Value;
								i++;
							}
							el.user = drow;
						} else {
							el.user = null;
						}

						//Создаем объект позиций
						XElement lists = order.Element("lists");		
						String[] namesOrder = { "prcSRcdNom", "sPrArt", "sPrArtNm", "sPrGrpNm", "sPrEiSh", "SklN_NoTrd", "sPrCn", "SklOpa_Rcd", "SklPrc_Rcd", "EI_Rcd" };
						el.order = new DataTable();
						foreach (string item in namesOrder) {
							el.order.Columns.Add(item);
						}
						foreach (XElement list in lists.Elements("list")) {
							DataRow row = el.order.NewRow();
							int i = 0;
							foreach (string item in namesOrder) {
								row[i] = list.Attribute(item).Value;
								i++;
							}
							el.order.Rows.Add(row);
						}

						//Добавляем созданный элемент в список
						data.OrderElementList.Add(el);
						if(el.user != null) {
							data.OrderElementListView.Add(new OrderElementView(index, el.user[1].ToString(), el.GetSum()));
						} else {
							data.OrderElementListView.Add(new OrderElementView(index, "", el.GetSum()));
						}
						index++;
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("Ошибка получения данных из XML");
				currentClassLogger.Error("DataXML.GetXML.Ошибка получения данных из XML");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		public void SetXML(OrderElement orderElement) {//Запись в XML фаил
			try {
				Data data = Data.GetInstance();
				//считываем данные из файла
				XDocument xdoc;
				try {
					xdoc = XDocument.Load("orders.xml");
				} catch {
					xdoc = new XDocument(new XElement("orders"));
				}
				XElement orders = xdoc.Element("orders");

				//Добавляем новый элемент и сохраняем
				orders.Add(GenerateElement(orderElement));
				xdoc.Save("orders.xml");
			}catch(Exception ex) {
				MessageBox.Show("Ошибка записи данных в XML");
				currentClassLogger.Error("DataXML.SetXML.Ошибка записи данных в XML");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}

		public void ChangeXML(int index) {//Изменение XML файла
			try {
				Data data = Data.GetInstance();

				//считываем данные из файла
				XDocument xdoc;
				try {
					xdoc = XDocument.Load("orders.xml");
				} catch {
					xdoc = new XDocument(new XElement("orders"));
				}
				XElement orders = xdoc.Element("orders");

				if (orders is not null) {
					var order = xdoc.Element("orders")?
						.Elements("order")
						.FirstOrDefault(p => p.Attribute("id")?.Value == (data.OrderElementList[index].id).ToString());
					XElement ChangeOrder = GenerateElement(data.OrderElementList[index]);
					order.ReplaceWith(ChangeOrder);
				}

				xdoc.Save("orders.xml");
			}catch(Exception ex) {
				MessageBox.Show("Ошибка изменения данных в XML");
				currentClassLogger.Error("DataXML.ChangeXML.Ошибка изменения данных в XML");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}

		public void DeleteXML(int index) {//Удаление XML файла
			try {
				Data data = Data.GetInstance();

				//считываем данные из файла
				XDocument xdoc;
				try {
					xdoc = XDocument.Load("orders.xml");
				} catch {
					xdoc = new XDocument(new XElement("orders"));
				}
				XElement orders = xdoc.Element("orders");

				if (orders is not null) {
					// проходим по всем элементам person
					int i = 1;
					foreach (XElement order in orders.Elements("order")) {
						if (index == i - 1) {
							order.Remove();
						}
						i++;
					}
				}
				xdoc.Save("orders.xml");
			}catch	(Exception ex) {
				MessageBox.Show("Ошибка удаления данных в XML");
				currentClassLogger.Error("DataXML.DeleteXML.Ошибка удаления данных в XML");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		public void CloneXML() {//Клонирование XML файла
			try {
				File.Copy(@"orders.xml", @"orders_old.xml", true);
			}catch (Exception ex) {
				MessageBox.Show("Ошибка копирования файла данных XML");
				currentClassLogger.Error("DataXML.CloneXML.Ошибка копирования файла данных XML");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
	}
}
