using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stolov {
	internal class User {
		public string Kpu_Tn { get; set; }
		public string Fio { get; set; }
		public string SprPdr_NmFull { get; set; }
		public string Kpu_DtNpSt { get; set; }
		public string Kpu_DtRoj { get; set; }
		public string Dol { get; set; }

		public User(string Kpu_Tn, string Fio, string SprPdr_NmFull, string Kpu_DtNpSt, string Kpu_DtRoj, string Dol) {
			this.Kpu_Tn = Kpu_Tn;
			this.Fio = Fio;
			this.SprPdr_NmFull = SprPdr_NmFull;
			this.Kpu_DtNpSt = Kpu_DtNpSt;
			this.Kpu_DtRoj = Kpu_DtRoj;
			this.Dol = Dol;
		}
	}
}
