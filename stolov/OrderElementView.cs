using System;

namespace stolov {
	public class OrderElementView {
		public int num { get; set; }
		public string date { get; set; }
		public string FIO { get; set; }
		public double sum { get; set; }
		public OrderElementView(int num, string FIO, double sum) {
			this.num = num;
			this.FIO = FIO;
			this.sum = sum;
			this.date = DateTime.Now.ToString("dd/MM/yyyy");
		}
	}
}