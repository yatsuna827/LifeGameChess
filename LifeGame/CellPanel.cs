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
    public partial class CellPanel : Panel
    {
        private readonly static Color AliveColor = Color.Black;
        private readonly static Color DeadColor = Color.White;
        public static bool AllowClick = true;
        public CellPanel()
        {
            InitializeComponent();

            Size = new Size(20, 20);
            BackColor = DeadColor;
            BorderStyle = BorderStyle.FixedSingle;
            MouseDown += (sender, e) => { if (AllowClick) { IsAlive ^= true; OnPanelClicked(EventArgs.Empty); } };
        }

        private bool isAlive;
        public bool IsAlive { get => isAlive; set { isAlive = value; BackColor = value ? AliveColor : DeadColor; } }

        public event EventHandler PanelClicked;
        protected virtual void OnPanelClicked(EventArgs e)
        {
            PanelClicked?.Invoke(this, e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
