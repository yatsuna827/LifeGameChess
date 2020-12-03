using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeGame
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var size = radioButton1.Checked ? 8 : 16;

            var rand = new Random();
            var p = (double)numericUpDown3.Value / 100.0;
            var code = 0ul;
            if(check_UseCode.Checked)
            {
                try { code = Convert.ToUInt64(textBox1.Text, 16); }
                catch
                {
                    MessageBox.Show("入力されたコードが不正です. ランダムな盤面を生成します.");
                    for (int bit = 0; bit < 64; bit++) if (rand.NextDouble() < p) code |= (1ul << bit);
                }
            }
            else { for (int bit = 0; bit < 64; bit++) if (rand.NextDouble() < p) code |= (1ul << bit); }

            var board = new ChessBoard(size, (int)AtInitialization.Value, code, (int)PerTurn.Value, IsLoopBox.Checked);

            board.ShowDialog();
        }
    }
}
