using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace War
{
    public class Airforce : Piece
    {
        #region Private Fields

        private bool _forward = true;

        #endregion

        #region Public Methods

        public override List<Square> GetMoves(Board board)
        {
            var moves = new List<Square>();
            var pos = _square.BoardPosition;

            var movementDirections = GetMovementDirections();

            foreach (var direction in movementDirections)
            {
                var curr = pos + direction;
                if (board.TryGetSquare(curr, out var potentialMove))
                {
                    if (!potentialMove.Occupied)
                    {
                        moves.Add(potentialMove);
                    }
                    else
                    {
                        curr += direction;
                        if (board.TryGetSquare(curr, out var nextPotentialMove))
                        {
                            if (!nextPotentialMove.Occupied)
                            {
                                moves.Add(nextPotentialMove);
                            }
                        }
                    }
                }
            }

            return moves;
        }

        public override void Move(Board board, Square move)
        {
            var pos = _square.BoardPosition;
            var next = move.BoardPosition;
            var difference = next - pos;
            if (Math.Abs(difference.X) == 2)
            {
                var middle = pos + (difference / 2);
                if (board.TryGetSquare(middle, out var square))
                {
                    if (square.Piece.Team != Team)
                    {
                        square.Piece.DestroySelf();
                    }
                }
            }

            foreach (var direction in GetMovementDirections())
            {
                var curr = pos + direction;
                if (board.TryGetSquare(curr, out var square))
                {
                    square.RestoreFog(Team);
                }
            }

            OccupySquare(move);

            if (board.IsEdge(next))
            {
                _image.transform.Rotate(new(0, 180, 0));
                _forward = !_forward;
            }

            pos = _square.BoardPosition;
            foreach (var direction in GetMovementDirections())
            {
                var curr = pos + direction;
                if (board.TryGetSquare(curr, out var square))
                {
                    square.Defog(Team);
                }
            }
        }

        #endregion

        #region Private Methods

        protected override string GetSpriteName() { return "Plane"; }

        private List<Point> GetMovementDirections()
        {
            if ((Team == Team.Light && _forward) || (Team == Team.Dark && !_forward))
            {
                return new()
            {
                new Point(1, -1),
                new Point(1, 0),
                new Point(1, 1)
            };
            }
            else
            {
                return new()
            {
                new Point(-1, -1),
                new Point(-1, 0),
                new Point(-1, 1)
            };
            }
        }

        #endregion
    }
}
