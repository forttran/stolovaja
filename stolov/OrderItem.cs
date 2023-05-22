using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stolov {
    internal class OrderItem {
        public OrderItem(string name, double count, double price) {
            this.name = name;
            this.count = count;
            this.price = price;
            this.sum = count * price;
        }
        public string name { get; set; }
        public double count { get; set; }
        public double price { get; set; }
        public double sum { get; set; }
    }
}
