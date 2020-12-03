using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeGame
{
    public partial class Form1 : Form
    {
        private int height = 32, width = 32;
        private uint generation;
        private LifeGamePanel boardPanel;
        private uint[] codes;

        public Form1()
        {
            InitializeComponent();

            boardPanel = new LifeGamePanel(32, 32)
            {
                Parent = this,
                Location = new Point(118, 12)
            };

            codes = new uint[height];
        }

        private uint[] GetCodes()
        {
            if (boardPanel == null) return new uint[0];

            var codes = new uint[height];
            for(int i=0; i<height; i++)
                for (int k = 0; k < width; k++)
                    if (boardPanel[i, k]) codes[i] |= (1u << k);

            textBox1.Text = string.Join("," + Environment.NewLine, codes.Select(_ => $"{_:X8}"));
            return codes;
        }

        private void SetBoardWithCode(uint[] codes)
        {
            if (boardPanel == null) return;

            for (int i=0; i<32; i++)
            {
                int bit = 1;
                for (int k = 0; k < 32; k++, bit <<= 1)
                {
                    boardPanel[i, k] = (codes[i] & bit) != 0;
                }
            }
        }

        bool working;
        private void button1_Click(object sender, EventArgs e)
        {
            if (working) { working = false; return; }

            button1.Text = "Stop";
            CellPanel.AllowClick = false;
            var board = IsLoopBox.Checked ? LifeGameBoard.CreateLoopBoard(height, width, (_h, _w) => boardPanel[_h, _w]) : LifeGameBoard.CreateBoard(height, width, (_h, _w) => boardPanel[_h, _w]);
            int h = height, w = width;

            working = true;
            Task.Run(() =>
            {
                var nextFrame = Environment.TickCount;
                while (true)
                {
                    if (!working) break;
                    if(nextFrame <= Environment.TickCount)
                    {
                        nextFrame += 100;
                        board.AlternateGeneration();

                        Invoke((MethodInvoker)(() =>
                        {
                            numericUpDown1.Value = ++generation;
                            for (int i = 0; i < h; i++)
                                for (int k = 0; k < w; k++)
                                    boardPanel[i, k] = board[i, k];
                        }));
                    }
                }
                Invoke((MethodInvoker)(() => button1.Text = "Start"));
                CellPanel.AllowClick = true;
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (working) return;

            var p = (float)numericUpDown3.Value;
            var rand = new Random();
            for (int i = 0; i < height; i++)
                for (int k = 0; k < width; k++)
                    boardPanel[i, k] = rand.NextDouble() < p;

            generation = 0;
            numericUpDown1.Value = 0;
            codes = GetCodes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (working) return;

            generation = 0;
            numericUpDown1.Value = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (codes.Length != 32) return;

            SetBoardWithCode(codes);
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (working)
            {
                e.Cancel = true;

                working = false;
                await Task.Run(() => { while (working) { } });

                Close();
            }
        }

    }
}
