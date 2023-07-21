using System.Collections.Generic;

namespace War
{
    public class Spy : Piece
    {
        #region Public Methods

        public override List<Square> GetMoves(Board board)
        {
            var moves = new List<Square>();
            var pos = _square.BoardPosition;

            // get the squares the spy can move to.
            foreach (var direction in Point.GetAllDirections())
            {
                var curr = pos + direction;
                if (board.TryGetSquare(curr, out var potentialMove))
                {
                    if (!potentialMove.Occupied)
                    {
                        moves.Add(potentialMove);
                    }
                }
            }

            var attackDirections = new List<Point>() { new Point(-1, 0), new Point(1, 0) };
            foreach (var direction in attackDirections)
            {
                var curr = pos + direction;
                if (board.TryGetSquare(curr, out var potentialMove))
                {
                    if (potentialMove.Occupied && potentialMove.Piece.Team != Team)
                    {
                        moves.Add(potentialMove);
                    }
                }
            }

            return moves;
        }

        public override void Move(Board board, Square move)
        {
            OccupySquare(move, false);

            var pos = _square.BoardPosition;
            foreach (var direction in Point.GetPerpendiculars())
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

        protected override string GetSpriteName() { return "Spy"; }

        #endregion
    }
}