using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeGame
{
    public class MooreLifeGameCell
    {
        private readonly MooreLifeGameCell[] cells;

        public virtual MooreLifeGameCell UpperLeft { get { return cells[0]; } set { cells[0] = value; } }
        public virtual MooreLifeGameCell Upper { get { return cells[1]; } set { cells[1] = value; } }
        public virtual MooreLifeGameCell UpperRight { get { return cells[2]; } set { cells[2] = value; } }
        public virtual MooreLifeGameCell Left { get { return cells[3]; } set { cells[3] = value; } }
        public virtual MooreLifeGameCell Right { get { return cells[4]; } set { cells[4] = value; } }
        public virtual MooreLifeGameCell LowerLeft { get { return cells[5]; } set { cells[5] = value; } }
        public virtual MooreLifeGameCell Lower { get { return cells[6]; } set { cells[6] = value; } }
        public virtual MooreLifeGameCell LowerRight { get { return cells[7]; } set { cells[7] = value; } }

        public virtual bool IsAlive { get { return currentState.IsAlive; } }

        private CellState currentState = CellState.GetEmptyCell();
        private CellState nextState = CellState.GetEmptyCell();

        protected int CountAliveNeighborhoods() => cells.Count(_ => _.IsAlive);

        public void CalcNextState() { nextState = currentState.GetNextState(CountAliveNeighborhoods()); }
        public void GetNext() { currentState = nextState; }

        public static MooreLifeGameCell Create(bool isAlive = false)
        {
            // 近傍を全てEmptyCellで埋めたセルを生成する.
            return new MooreLifeGameCell(isAlive, Enumerable.Repeat<MooreLifeGameCell>(EmptyMooreLifeGameCell.Instance, 9).ToArray());
        }

        protected MooreLifeGameCell() { }
        private MooreLifeGameCell(bool isAlive, MooreLifeGameCell[] cells)
        {
            this.cells = cells;
            currentState = isAlive ? CellState.GetAliveCell() : CellState.GetDeadCell();
            nextState = currentState;
        }
        public static MooreLifeGameCell EmptyCell => EmptyMooreLifeGameCell.Instance;
        class EmptyMooreLifeGameCell : MooreLifeGameCell
        {
            public override bool IsAlive => false;
            public static readonly EmptyMooreLifeGameCell Instance = new EmptyMooreLifeGameCell();
            
            // 近傍は全てEmpty. Setは無効.
            public override MooreLifeGameCell UpperLeft { get => this; set { } }
            public override MooreLifeGameCell Upper { get => this; set { } }
            public override MooreLifeGameCell UpperRight { get => this; set { } }

            public override MooreLifeGameCell Left { get => this; set { } }
            public override MooreLifeGameCell Right { get => this; set { } }

            public override MooreLifeGameCell LowerLeft { get => this; set { } }
            public override MooreLifeGameCell Lower { get => this; set { } }
            public override MooreLifeGameCell LowerRight { get => this; set { } }

            private EmptyMooreLifeGameCell() { }
        }
    }

    abstract class CellState
    {
        public abstract bool IsAlive { get; }
        public abstract CellState GetNextState(int neighborhoods);

        public static CellState GetAliveCell() => Alive.GetInstance();
        public static CellState GetDeadCell() => Dead.GetInstance();
        public static CellState GetEmptyCell() => Empty.GetInstance();

        class Alive : CellState
        {
            public override bool IsAlive => true;
            public override CellState GetNextState(int neighborhoods)
            {
                if (neighborhoods == 2 || neighborhoods == 3) 
                    return this;
                else
                    return Dead.GetInstance();
            }
            private static readonly Alive instance = new Alive();
            public static Alive GetInstance() => instance;
            private Alive() { }
        }
        class Dead : CellState
        {
            public override bool IsAlive => false;
            public override CellState GetNextState(int neighborhoods)
            {
                if (neighborhoods == 3) 
                    return Alive.GetInstance(); 
                else
                    return this;
            }
            private static readonly Dead instance = new Dead();
            public static Dead GetInstance() => instance;
            private Dead() { }
        }
        class Empty : CellState
        {
            public override bool IsAlive => false;
            public override CellState GetNextState(int _) => this;
            private static readonly Empty instance = new Empty();
            private Empty() { }
            public static Empty GetInstance() => instance;
        }
    }

    public static class LifeGameExtensions
    {
        public static void JointRight(this MooreLifeGameCell leftCell, MooreLifeGameCell rightCell)
        {
            leftCell.Right = rightCell;
            rightCell.Left = leftCell;
        }
        public static void JointLowerRight(this MooreLifeGameCell upperLeftCell, MooreLifeGameCell lowerRightCell)
        {
            upperLeftCell.LowerRight = lowerRightCell;
            lowerRightCell.UpperLeft = upperLeftCell;
        }
        public static void JointLower(this MooreLifeGameCell upperCell, MooreLifeGameCell lowerCell)
        {
            upperCell.Lower = lowerCell;
            lowerCell.Upper = upperCell;
        }
        public static void JointLowerLeft(this MooreLifeGameCell upperRightCell, MooreLifeGameCell lowerLeftCell)
        {
            upperRightCell.LowerLeft = lowerLeftCell;
            lowerLeftCell.UpperRight = upperRightCell;
        }

        public static void SetAroundCells(this MooreLifeGameCell centerCell,
            MooreLifeGameCell rightCell, MooreLifeGameCell lowerRightCell, MooreLifeGameCell lowerCell, MooreLifeGameCell lowerLeftCell)
        {
            centerCell.JointRight(rightCell);
            centerCell.JointLowerRight(lowerRightCell);
            centerCell.JointLower(lowerCell);
            centerCell.JointLowerLeft(lowerLeftCell);
        }
    }
}

