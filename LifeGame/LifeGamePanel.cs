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
    public partial class LifeGamePanel : Control
    {
        public LifeGamePanel()
        {
            InitializeComponent();

            _rowCount = 32;
            _columnCount = 32;
            RenderPanels();
        }
        public LifeGamePanel(int h, int w)
        {
            InitializeComponent();

            _rowCount = h;
            _columnCount = w;
            RenderPanels();
        }

        private int _rowCount = 16, _columnCount = 16;
        public int RowCount 
        {
            get => _rowCount; 
            set 
            {
                if (_rowCount == value || value < 0 || 32 < value) return;

                _rowCount = value;
                RenderPanels();
            } 
        }
        public int ColumnCount
        {
            get => _columnCount;
            set
            {
                if (_columnCount == value || value < 0 || 32 < value) return;

                _columnCount = value;
                RenderPanels();
            }
        }

        private CellPanel[][] cells;

        private void RenderPanels()
        {
            if(cells != null) foreach (var row in cells) foreach (var cell in row) if (!cell.IsDisposed) cell.Dispose();

            this.Size = new Size(19 * RowCount + 1, 19 * ColumnCount + 1);
            cells = new CellPanel[RowCount][];
            for (int i = 0; i < RowCount; i++)
            {
                cells[i] = new CellPanel[ColumnCount];
                for (int k = 0; k < ColumnCount; k++)
                {
                    cells[i][k] = new CellPanel() { Parent = this, Location = new Point(19 * i, 19 * k) };
                    cells[i][k].PanelClicked += new EventHandler(OnPanelStateChanged);
                }
            }
        }
        public bool AllWhite { get => cells.All(_ => _.All(__ => !__.IsAlive)); }
        public bool this[int rowIndex, int columnIndex]
        {
            get => cells[rowIndex][columnIndex].IsAlive;
            set { cells[rowIndex][columnIndex].IsAlive = value; }
        }

        public event EventHandler PanelStateChanged;
        public void OnPanelStateChanged(object sender, EventArgs e) => PanelStateChanged.Invoke(sender, e);
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
