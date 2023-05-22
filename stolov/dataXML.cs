using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace stolov {
	internal class dataXML {
		public dataXML() { }
		public void Get(OrderElement orderElement) {
			Data data = Data.getInstance();
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load("people.xml");
			// получим корневой элемент
			XmlElement? xRoot = xDoc.DocumentElement;
			if (xRoot != null) {
				XmlElement orderElem = xDoc.CreateElement("order");
				XmlElement fioElem = xDoc.CreateElement("fio");
				XmlElement tabElem = xDoc.CreateElement("tab");

				XmlText fioText = xDoc.CreateTextNode(orderElement.user[1].ToString());
				XmlText tabText = xDoc.CreateTextNode(orderElement.user[0].ToString());

				fioElem.AppendChild(fioText);
				tabElem.AppendChild(tabText);

				orderElem.AppendChild(fioElem);
				orderElem.AppendChild(tabElem);

				xRoot?.AppendChild(orderElem);
				// сохраняем изменения xml-документа в файл
				xDoc.Save("people.xml");
			}
		}
	}
}
