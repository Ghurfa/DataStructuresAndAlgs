using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CheckersMCTS
{
    internal class CheckersState : IGameState<CheckersState>
    {
        public readonly int BoardSize;

        // State data
        private readonly Checker[,] Board;
        private ImmutableArray<CheckersState>? Children;
        public readonly ImmutableArray<Checker> Checkers;
        public readonly bool IsWhiteTurn;

        // Accessors
        public Checker this[int x, int y] => Board[x, y];

        // Interface implementation
        public bool IsWin => throw new NotImplementedException();
        public bool IsTie => throw new NotImplementedException();
        public bool IsLoss => throw new NotImplementedException();
        public bool IsTerminal => throw new NotImplementedException();

        private CheckersState(Checker[,] board, bool isWhiteTurn)
        {
            BoardSize = board.GetLength(0);
            Board = board;
            Checkers = GenerateCheckersArray();
            IsWhiteTurn = isWhiteTurn;
        }

        private ImmutableArray<Checker> GenerateCheckersArray()
        {
            ImmutableArray<Checker>.Builder checkersList = ImmutableArray.CreateBuilder<Checker>();
            for (int x = 0; x < Board.GetLength(0); x++)
            {
                for (int y = 0; y < Board.GetLength(1); y++)
                {
                    if (Board[x, y] != null)
                    {
                        checkersList.Add(Board[x, y]);
                    }
                }
            }
            return checkersList.ToImmutable();
        }

        public IReadOnlyList<CheckersState> GetChildren()
        {
            if (Children != null) return Children;

            ImmutableArray<CheckersState>.Builder childStates = ImmutableArray.CreateBuilder<CheckersState>();

            // Check for captures
            foreach (Checker checker in Checkers)
            {
                bool isWhite = !checker.Type.HasFlag(CheckerType.Red);

                if (IsWhiteTurn ^ !isWhite)
                {
                    Point coordinates = checker.Coordinates;
                    bool movesUp = checker.Type.HasFlag(CheckerType.MovesUp);
                    bool movesDown = checker.Type.HasFlag(CheckerType.MovesDown);

                    if (movesUp && coordinates.X >= 2 && coordinates.Y >= 2)
                    {
                        Checker captured = Board[coordinates.X - 1, coordinates.Y - 1];
                        bool isEmpty = Board[coordinates.X - 2, coordinates.Y - 2] == null;
                        if (isEmpty && captured != null && captured.Type.HasFlag(CheckerType.Red) == isWhite)
                        {
                            childStates.Add(WithCapture(checker, -1, -1));
                        }
                    }
                }
            }

            Children = childStates.ToImmutable();
            return Children;
        }

        private CheckersState WithCapture(Checker capturer, int xDir, int yDir)
        {
            Checker[,] newBoard = new Checker[BoardSize, BoardSize];
            Board.CopyTo(newBoard, 0);

            newBoard[capturer.Coordinates.X, capturer.Coordinates.Y] = null;
            newBoard[capturer.Coordinates.X + xDir, capturer.Coordinates.Y + yDir] = null;

            int endX = capturer.Coordinates.X + 2 * xDir;
            int endY = capturer.Coordinates.Y + 2 * yDir;
            bool shouldKing = (yDir < 0 && capturer.Coordinates.Y == 2 * yDir) || (yDir > 0 && capturer.Coordinates.Y + 2 * yDir == BoardSize - 1);

            Checker newChecker = capturer with { Type = shouldKing ? capturer.Type | CheckerType.King : capturer.Type };

            newBoard[endX, endY] = newChecker;

            throw new NotImplementedException();
            //return new CheckersState(newBoard, );
        }

        private static bool HasCaptures(IEnumerable<Checker> checkers, Checker[,] board, bool isWhiteTurn)
        {
            foreach (Checker checker in checkers)
            {
                bool isWhite = !checker.Type.HasFlag(CheckerType.Red);

                if (isWhiteTurn ^ !isWhite)
                {
                    Point coordinates = checker.Coordinates;
                    bool movesUp = checker.Type.HasFlag(CheckerType.MovesUp);
                    bool movesDown = checker.Type.HasFlag(CheckerType.MovesDown);

                    if (movesUp && coordinates.X >= 2 && coordinates.Y >= 2)
                    {
                        Checker captured = board[coordinates.X - 1, coordinates.Y - 1];
                        bool isEmpty = board[coordinates.X - 2, coordinates.Y - 2] == null;
                        if (isEmpty && captured != null && captured.Type.HasFlag(CheckerType.Red) == isWhite)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static CheckersState StartingState { get; } = GenerateStartingState();

        private static CheckersState GenerateStartingState(int boardSize = 10)
        {
            Checker[,] board = new Checker[boardSize, boardSize];

            for (int y = 0; y < 3; y++)
            {
                for (int x = y % 2 == 0 ? 1 : 0; x < boardSize; x += 2)
                {
                    Checker newChecker = new(new Point(x, y), CheckerType.WhitePawn);
                    board[x, y] = newChecker;
                }
            }

            for (int y = boardSize - 3; y < boardSize; y++)
            {
                for (int x = y % 2 == 0 ? 1 : 0; x < boardSize; x += 2)
                {
                    Checker newChecker = new(new Point(x, y), CheckerType.RedPawn);
                    board[x, y] = newChecker;
                }
            }

            return new CheckersState(board, true);
        }
    }
}
