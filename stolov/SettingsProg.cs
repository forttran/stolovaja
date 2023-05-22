using Microsoft.AspNetCore.Hosting.Server;
using stolov.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stolov {
	public partial class SettingsProg : Form {
		public SettingsProg() {
			InitializeComponent();
		}

		private void tabControl1_DrawItem(object sender, DrawItemEventArgs e) {
			Graphics g;
			string sText;
			int iX;
			float iY;

			SizeF sizeText;
			TabControl ctlTab;

			ctlTab = (TabControl)sender;

			g = e.Graphics;

			sText = ctlTab.TabPages[e.Index].Text;
			sizeText = g.MeasureString(sText, ctlTab.Font);
			iX = e.Bounds.Left + 6;
			iY = e.Bounds.Top + (e.Bounds.Height - sizeText.Height) / 2;
			g.DrawString(sText, ctlTab.Font, Brushes.Black, iX, iY);
		}

		private void SettingsProg_Load(object sender, EventArgs e) {
			textBox6.Text = Settings.Default.GeneratePath;
			textBox7.Text = Settings.Default.holidayMsg;
			string[] elementConnect = Settings.Default.ConnectionString.Split(';');
			textBox1.Text = elementConnect[0].Split('=')[1];
			textBox2.Text = elementConnect[1].Split('=')[1];
			textBox3.Text = elementConnect[2].Split('=')[1];
			textBox4.Text = elementConnect[3].Split('=')[1];
			textBox5.Text = elementConnect[4].Split('=')[1];

			String[] printers = PrinterSettings.InstalledPrinters.Cast<string>().ToArray();
			for (int i = 0; i < printers.Length; i++) {
				listPrinters.Items.Add(printers[i].ToString());
			}
			for (int i = 0; i < printers.Length; i++) {
				if (Settings.Default.Prints == printers[i].ToString()) {
					listPrinters.SetSelected(i, true);
				}
			}

		}

		private void button1_Click(object sender, EventArgs e) {
			Settings.Default.GeneratePath = textBox6.Text;
			Settings.Default.holidayMsg = textBox7.Text;
			string ConnectionString = "Data Source=" + textBox1.Text + ";";
			ConnectionString += "Initial Catalog=" + textBox2.Text + ";";
			ConnectionString += "Persist Security Info=" + textBox3.Text + ";";
			ConnectionString += "User ID=" + textBox4.Text + ";";
			ConnectionString += "Password=" + textBox5.Text;
			Settings.Default.ConnectionString = ConnectionString;
			Settings.Default.Prints = listPrinters.Items[listPrinters.SelectedIndex].ToString();
			Settings.Default.Save();
			this.Close();
		}

		private void button3_Click(object sender, EventArgs e) {
			FolderBrowserDialog FBD = new FolderBrowserDialog();
			if (FBD.ShowDialog() == DialogResult.OK) {
				textBox6.Text = FBD.SelectedPath;
			}
		}

		private void button4_Click(object sender, EventArgs e) {
			OpenFileDialog OPF = new OpenFileDialog();
			OPF.Filter = "Файлы txt|*.txt";
			if (OPF.ShowDialog() == DialogResult.OK) {
				textBox7.Text = OPF.FileName;
			}
		}

		private void button2_Click(object sender, EventArgs e) {
			this.Close();
		}
	}
}
