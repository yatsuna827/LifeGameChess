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
    public partial class ChessBoard : Form
    {
        private readonly LifeGamePanel boardPanel;
        private readonly int BoardSize;
        private readonly int GenerationPerTurn;
        private readonly int InitializationTurn;
        private readonly bool IsTorus;
        private bool isFirstTurn;
        private bool gamePlaying;
        private ulong gameCode;

        public ChessBoard(int size, int initTurn, ulong initialBoard, int gpt, bool isTorus)
        {
            InitializeComponent();

            CellPanel.AllowClick = false;
            this.Size = new Size(148 + 19 * size, 63 + 19 * size);
            boardPanel = new LifeGamePanel(size, size)
            {
                Parent = this,
                Location = new Point(118, 12)
            };
            boardPanel.PanelStateChanged += new EventHandler(Board1_PanelStateChanged);

            BoardSize = size;
            InitializationTurn = initTurn;
            gameCode = initialBoard;
            GenerationPerTurn = gpt;
            IsTorus = isTorus;

            InitializeBoardRandom();
        }

        public async void Board1_PanelStateChanged(object sender, EventArgs e)
        {
            if(gamePlaying)
            {
                await AdvanceGame(GenerationPerTurn);
                if (boardPanel.AllWhite)
                {
                    MessageBox.Show((isFirstTurn ? "First Player " : "Second Player ") + "Win !!");

                    Close();
                    return;
                }
                Invoke((MethodInvoker)(() => ToggleTurn()));
            }
        }

        private Task AdvanceGame(int advance = 5)
        {
            var board = IsTorus ? 
                LifeGameBoard.CreateLoopBoard(BoardSize, BoardSize, (_h, _w) => boardPanel[_h, _w]) : 
                LifeGameBoard.CreateBoard(BoardSize, BoardSize, (_h, _w) => boardPanel[_h, _w]);

            CellPanel.AllowClick = false;
            return Task.Run(() =>
            {
                var nextFrame = Environment.TickCount + 100;
                int count = 0;
                while (true)
                {
                    if (nextFrame <= Environment.TickCount)
                    {
                        nextFrame += 100;
                        board.AlternateGeneration();

                        Invoke((MethodInvoker)(() =>
                        {
                            numericUpDown1.Value += 1;
                            for (int i = 0; i < BoardSize; i++)
                                for (int k = 0; k < BoardSize; k++)
                                    boardPanel[i, k] = board[i, k];
                        }));
                        if (++count == advance) break;
                    }
                }

                CellPanel.AllowClick = true;
            });
        }
        private void ToggleTurn()
        {
            isFirstTurn ^= true;
            Text = isFirstTurn ? "First Player's Turn" : "Second Player's Turn";
        }

        private void InitializeBoardRandom()
        {
            var rand = new Random();
            for (int i = 0; i < BoardSize; i++)
                for (int k = 0; k < BoardSize; k++)
                    boardPanel[i, k] = ((gameCode >> (i * 8 + k)) & 1) == 1;
        }
        private ulong GetGameCode()
        {
            var code = 0ul;
            for (int bit = 0; bit < 64; bit++)
            {
                var row = bit % 8;
                var column = bit / 8;
                if (boardPanel[row, column]) code |= (1ul << bit);
            }
            return code;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            gamePlaying = true;
            isFirstTurn = false;

            CellPanel.AllowClick = true;

            MessageBox.Show($"{InitializationTurn}世代進めます.");

            await AdvanceGame(InitializationTurn);
            MessageBox.Show("ゲーム開始");
            ToggleTurn();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText($"{gameCode:X16}");
        }
    }
}
