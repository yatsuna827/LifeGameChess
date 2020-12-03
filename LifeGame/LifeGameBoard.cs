using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGame
{
    class LifeGameBoard
    {
        private readonly MooreLifeGameCell[][] board;
        public int Height { get; private set; }
        public int Width { get; private set; }

        public void AlternateGeneration()
        {
            foreach (var row in board)
                foreach (var cell in row)
                    cell.CalcNextState();

            foreach (var row in board)
                foreach (var cell in row)
                    cell.GetNext();
        }

        public bool this[int rowIndex, int columnIndex] => board[rowIndex][columnIndex].IsAlive;

        private LifeGameBoard(MooreLifeGameCell[][] board) => this.board = board;

        public static LifeGameBoard CreateBoard(int h, int w)
        {
            var board = new MooreLifeGameCell[h][];
            foreach (var i in Enumerable.Range(0, h))
            {
                board[i] = new MooreLifeGameCell[w];
                foreach (var k in Enumerable.Range(0, w))
                {
                    board[i][k] = MooreLifeGameCell.Create(false);
                }
            }

            var empty = MooreLifeGameCell.EmptyCell;
            foreach (var i in Enumerable.Range(0, h))
            {
                foreach (var k in Enumerable.Range(0, w))
                {
                    var right = k == board[i].Length - 1 ? empty : board[i][k + 1];
                    var lowerRight = i == board.Length - 1 || k == board[i].Length - 1 ? empty : board[i + 1][k + 1];
                    var lower = i == board.Length - 1 ? empty : board[i + 1][k];
                    var lowerLeft = i == board.Length - 1 || k == 0 ? empty : board[i + 1][k - 1];

                    board[i][k].SetAroundCells(right, lowerRight, lower, lowerLeft);
                }
            }

            return new LifeGameBoard(board);
        }

        public static LifeGameBoard CreateBoard(int h, int w, Func<int,int,bool> initialStateSelector)
        {
            var board = new MooreLifeGameCell[h][];
            foreach (var i in Enumerable.Range(0, h))
            {
                board[i] = new MooreLifeGameCell[w];
                foreach (var k in Enumerable.Range(0, w))
                {
                    board[i][k] = MooreLifeGameCell.Create(initialStateSelector?.Invoke(i, k) ?? false);
                }
            }

            var empty = MooreLifeGameCell.EmptyCell;
            foreach (var i in Enumerable.Range(0, h))
            {
                foreach (var k in Enumerable.Range(0, w))
                {
                    var right = k == board[i].Length - 1 ? empty : board[i][k + 1];
                    var lowerRight = i == board.Length - 1 || k == board[i].Length - 1 ? empty : board[i + 1][k + 1];
                    var lower = i == board.Length - 1 ? empty : board[i + 1][k];
                    var lowerLeft = i == board.Length - 1 || k == 0 ? empty : board[i + 1][k - 1];

                    board[i][k].SetAroundCells(right, lowerRight, lower, lowerLeft);
                }
            }

            return new LifeGameBoard(board);
        }


        public static LifeGameBoard CreateLoopBoard(int h, int w)
        {

            var board = new MooreLifeGameCell[h][];
            foreach (var i in Enumerable.Range(0, h))
            {
                board[i] = new MooreLifeGameCell[w];
                foreach (var k in Enumerable.Range(0, w))
                {
                    board[i][k] = MooreLifeGameCell.Create(false);
                }
            }

            var empty = MooreLifeGameCell.EmptyCell;
            foreach (var i in Enumerable.Range(0, h))
            {
                foreach (var k in Enumerable.Range(0, w))
                {
                    var right = board[i][(k + 1) % w];
                    var lowerRight = board[(i + 1) % h][(k + 1) % w];
                    var lower = board[(i + 1) % h][k];
                    var lowerLeft = board[(i + 1) % h][(k - 1 + w) % w];

                    board[i][k].SetAroundCells(right, lowerRight, lower, lowerLeft);
                }
            }

            return new LifeGameBoard(board);
        }

        public static LifeGameBoard CreateLoopBoard(int h, int w, Func<int, int, bool> initialStateSelector)
        {

            var board = new MooreLifeGameCell[h][];
            foreach (var i in Enumerable.Range(0, h))
            {
                board[i] = new MooreLifeGameCell[w];
                foreach (var k in Enumerable.Range(0, w))
                {
                    board[i][k] = MooreLifeGameCell.Create(initialStateSelector?.Invoke(i, k) ?? false);
                }
            }

            var empty = MooreLifeGameCell.EmptyCell;
            foreach (var i in Enumerable.Range(0, h))
            {
                foreach (var k in Enumerable.Range(0, w))
                {
                    var right = board[i][(k + 1) % w];
                    var lowerRight = board[(i + 1) % h][(k + 1) % w];
                    var lower = board[(i + 1) % h][k];
                    var lowerLeft = board[(i + 1) % h][(k - 1 + w) % w];

                    board[i][k].SetAroundCells(right, lowerRight, lower, lowerLeft);
                }
            }

            return new LifeGameBoard(board);
        }
    }
}
