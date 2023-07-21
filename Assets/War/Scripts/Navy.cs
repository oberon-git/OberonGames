using System.Collections.Generic;

namespace War
{
    public class Navy : Piece
    {
        #region Public Methods

        public override List<Square> GetMoves(Board board)
        {
            var moves = new List<Square>();
            var pos = _square.BoardPosition;

            foreach (var direction in Point.GetAllDirections())
            {
                var middle = pos + direction;
                var curr = middle + direction;
                if (board.TryGetSquare(curr, out var potentialMove))
                {
                    if (!potentialMove.Occupied)
                    {
                        moves.Add(potentialMove);
                    }
                }
            }

            foreach (var direction in Point.GetPerpendiculars())
            {
                var middle = pos + direction;
                var curr = middle + direction;
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
            var pos = _square.BoardPosition;
            foreach (var direction in Point.GetAllDirections())
            {
                var curr = pos + direction;
                if (board.TryGetSquare(curr, out var square))
                {
                    square.RestoreFog(Team);
                }
            }

            OccupySquare(move);

            pos = _square.BoardPosition;
            foreach (var direction in Point.GetAllDirections())
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

        protected override string GetSpriteName() { return "Ship"; }

        #endregion
    }
}