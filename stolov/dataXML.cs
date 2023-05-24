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
	internal class dataXML {
		public static Logger currentClassLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		public dataXML() { }
		private XElement generateElement(OrderElement orderElement) {
			XElement order;
			Data data = Data.getInstance();
			try {
				XAttribute Kpu_Tn = new XAttribute("Kpu_Tn", orderElement.user[0]);
				XAttribute Fio = new XAttribute("Fio", orderElement.user[1]);
				XAttribute SprPdr_NmFull = new XAttribute("SprPdr_NmFull", orderElement.user[2]);
				XAttribute Kpu_DtNpSt = new XAttribute("Kpu_DtNpSt", orderElement.user[3]);
				XAttribute Kpu_DtRoj = new XAttribute("Kpu_DtRoj", orderElement.user[4]);
				XAttribute Dol = new XAttribute("Dol", orderElement.user[5]);
				XElement user = new XElement("user", Kpu_Tn, Fio, SprPdr_NmFull, Kpu_DtNpSt, Kpu_DtRoj, Dol);

				XElement lists = new XElement("lists");
				for (int i = 0; i <= orderElement.order.Rows.Count - 1; i++) {
					XAttribute prcSRcdNom = new XAttribute("prcSRcdNom", orderElement.order.Rows[i][0]);
					XAttribute sPrArt = new XAttribute("sPrArt", orderElement.order.Rows[i][1]);
					XAttribute sPrArtNm = new XAttribute("sPrArtNm", orderElement.order.Rows[i][2]);
					XAttribute sPrGrpNm = new XAttribute("sPrGrpNm", orderElement.order.Rows[i][3]);
					XAttribute sPrEiSh = new XAttribute("sPrEiSh", orderElement.order.Rows[i][4]);
					XAttribute SklN_NoTrd = new XAttribute("SklN_NoTrd", orderElement.order.Rows[i][5]);
					XAttribute sPrCn = new XAttribute("sPrCn", orderElement.order.Rows[i][6]);
					XAttribute SklOpa_Rcd = new XAttribute("SklOpa_Rcd", orderElement.order.Rows[i][7]);
					XAttribute SklPrc_Rcd = new XAttribute("SklPrc_Rcd", orderElement.order.Rows[i][8]);
					XAttribute EI_Rcd = new XAttribute("EI_Rcd", orderElement.order.Rows[i][9]);
					XElement list = new XElement("list", prcSRcdNom, sPrArt, sPrArtNm, sPrGrpNm, sPrEiSh, SklN_NoTrd, sPrCn, SklOpa_Rcd, SklPrc_Rcd, EI_Rcd);
					lists.Add(list);
				}
				int id;
				if(data.item == -1) {
					currentClassLogger.Debug("Новый");
					data.idXML = data.idXML + 1;
					id = data.idXML;
				} else {
					currentClassLogger.Debug("Редактирование="+ data.item);
					id = orderElement.id;
				}
				order = new XElement("order",
						new XAttribute("sum", orderElement.getSum()),
						new XAttribute("id", id),
						user, lists);
			}catch(Exception ex) {
				MessageBox.Show("Ошибка генерации элемента");
				currentClassLogger.Error("Ошибка генерации элемента");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
				order = new XElement("order");
			}
			return order;
		}
		public void GetXML() {
			try {
				Data data = Data.getInstance();
				XDocument xdoc;
				try {
					xdoc = XDocument.Load("orders.xml");
				} catch (Exception e) {
					xdoc = new XDocument(new XElement("orders"));
				}
				// получаем корневой узел
				XElement? orders = xdoc.Element("orders");
				if (orders is not null) {
					// проходим по всем элементам person
					int index = 1;
					foreach (XElement order in orders.Elements("order")) {
						data.idXML = Math.Max(data.idXML, Int32.Parse(order.Attribute("id").Value));
						OrderElement el = new OrderElement(Int32.Parse(order.Attribute("id").Value));
						XElement? user = order.Element("user");
						String[] namesUser = { "Kpu_Tn", "Fio", "SprPdr_NmFull", "Kpu_DtNpSt", "Kpu_DtRoj", "Dol" };
						DataTable dt = new DataTable();
						DataRow drow = dt.NewRow();
						int i = 0;
						foreach (string item in namesUser) {
							dt.Columns.Add(item);
							drow[i] = user.Attribute(item).Value;
							i++;
						}
						el.user = drow;

						XElement? lists = order.Element("lists");
						XAttribute[] ls = new XAttribute[11];
						String[] namesOrder = { "prcSRcdNom", "sPrArt", "sPrArtNm", "sPrGrpNm", "sPrEiSh", "SklN_NoTrd", "sPrCn", "SklOpa_Rcd", "SklPrc_Rcd", "EI_Rcd" };
						el.order = new DataTable();
						foreach (string item in namesOrder) {
							el.order.Columns.Add(item);
						}
						foreach (XElement list in lists.Elements("list")) {
							DataRow row = el.order.NewRow();
							i = 0;
							foreach (string item in namesOrder) {
								row[i] = list.Attribute(item).Value;
								i++;
							}
							el.order.Rows.Add(row);
						}
						data.OrderElementList.Add(el);
						data.OrderElementListView.Add(new OrderElementView(index, el.user[1].ToString(), el.getSum()));
						index++;
					}
				}
			} catch (Exception ex) {
				MessageBox.Show("Ошибка получения данных из XML");
				currentClassLogger.Error("Ошибка получения данных из XML");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		public void SetXML(OrderElement orderElement) {
			try {
				Data data = Data.getInstance();
				XDocument xdoc;
				try {
					xdoc = XDocument.Load("orders.xml");
				} catch (Exception ex) {
					xdoc = new XDocument(new XElement("orders"));
				}
				XElement? root = xdoc.Element("orders");
				root.Add(generateElement(orderElement));
				xdoc.Save("orders.xml");
			}catch(Exception ex) {
				MessageBox.Show("Ошибка записи данных в XML");
				currentClassLogger.Error("Ошибка записи данных в XML");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}

		public void changeXML(int index) {
			try {
				Data data = Data.getInstance();
				XDocument xdoc;
				try {
					xdoc = XDocument.Load("orders.xml");
				} catch (Exception ex) {
					xdoc = new XDocument(new XElement("orders"));
				}
				XElement? orders = xdoc.Element("orders");
				if (orders is not null) {
					var order = xdoc.Element("orders")?
						.Elements("order")
						.FirstOrDefault(p => p.Attribute("id")?.Value == (data.OrderElementList[index].id).ToString());
					currentClassLogger.Debug("+++"+data.OrderElementList[index].id);
					XElement ChangeOrder = generateElement(data.OrderElementList[index]);
					order.ReplaceWith(ChangeOrder);
				}
				xdoc.Save("orders.xml");
			}catch(Exception ex) {
				MessageBox.Show("Ошибка изменения данных в XML");
				currentClassLogger.Error("Ошибка изменения данных в XML");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}

		public void deleteXML(int index) {
			try {
				Data data = Data.getInstance();
				XDocument xdoc;
				try {
					xdoc = XDocument.Load("orders.xml");
				} catch (Exception ex) {
					xdoc = new XDocument(new XElement("orders"));
				}
				XElement? orders = xdoc.Element("orders");
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
				currentClassLogger.Error("Ошибка удаления данных в XML");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
		public void cloneXML() {
			try {
				File.Copy(@"orders.xml", @"orders_old.xml");
			}catch (Exception ex) {
				MessageBox.Show("Ошибка копирования файла данных XML");
				currentClassLogger.Error("Ошибка копирования файла данных XML");
				currentClassLogger.Error("Ошибка: " + ex.Message + "; Source: " + ex.Source);
			}
		}
	}
}
