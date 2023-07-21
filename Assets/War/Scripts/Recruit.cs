using System.Collections.Generic;

namespace War
{
    public class Recruit : Piece
    {
        #region Public Methods

        public override List<Square> GetMoves(Board board)
        {
            var moves = new List<Square>();
            var pos = _square.BoardPosition;

            // get the squares the recruit can move to.
            foreach (var direction in Point.GetDiagonals())
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

            // get the squares the recruit can attack.
            foreach (var direction in Point.GetPerpendiculars())
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
            OccupySquare(move);
        }

        #endregion

        #region Private Methods

        protected override string GetSpriteName() { return "Recruit"; }

        #endregion
    }
}
