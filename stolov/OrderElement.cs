using NLog.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using NLog;
using NLog.Filters;
using NLog.Fluent;

namespace stolov {
	public class OrderElement {
		public DataTable order;
		public DataRow user;
		public bool ret;
		public int inAction;
		public int id;
		public static Logger currentClassLogger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
		public double getSum() {
			double sum = 0;
			if (this.order == null) {
				this.order = new DataTable();
			}
			if (this.order.Rows.Count > 0) {
				DataRow[] result = this.order.Select();
				foreach (DataRow row in result) {
					double sm = 0;
					//Боремся с проклятой проблемой разделителей точка или запятая
					try {
						sm = Convert.ToDouble(row[5]) * Convert.ToDouble(row[6]);
					} catch {
						sm = Convert.ToDouble(row[5].ToString().Replace(".", ",")) * Convert.ToDouble(row[6].ToString().Replace(".", ","));
					}
					sum += sm;
				}
			}
			return sum;
		}
		public double getTax() {
			double sum = 0;
			double nalog = 0.83333;
			if (this.order == null) {
				this.order = new DataTable();
			}
			if (this.order.Rows.Count > 0) {
				DataRow[] result = this.order.Select();
				foreach (DataRow row in result) {
					sum += (Convert.ToDouble(row[6]) * nalog )* Convert.ToDouble(row[5]);
				}
			}
			return sum;
		}
		public OrderElement(DataRow user) {
			Data data = Data.getInstance();
			this.user = user;
			this.order = new DataTable();
			this.inAction = -1;
			this.ret = false;
			this.id = data.idXML + 1;
			//this.id = data.OrderElementList.Count + 1;
		}
		public OrderElement(int id) {
			this.id = id;
		}
		public OrderElement Copy() {
			OrderElement newlist = new OrderElement(this.user);
			newlist.order = this.order.Copy();
			return newlist;
		}
	}
}
