using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace War
{
    public class Board : MonoBehaviour
    {
        #region Public Properties

        public List<List<Square>> Squares { get; private set; } = new();

        #endregion

        #region Unity Messages

        void Start()
        {
            var squaresGameObject = transform.GetChild(0);

            var col = new List<Square>();
            for (int i = 0; i < 98; i++)
            {
                var square = squaresGameObject.GetChild(i).GetComponent<Square>();
                square.BoardPosition = new Point(i / 7, i % 7);
                col.Add(square);
                if (i % 7 == 6)
                {
                    Squares.Add(col);
                    col = new List<Square>();
                }
            }
        }

        #endregion

        #region Public Methods

        public void SetLightPieces()
        {
            PhotonNetwork.Instantiate("Airforce", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[0][0].name });
            PhotonNetwork.Instantiate("Navy", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[0][1].name });
            PhotonNetwork.Instantiate("General", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[0][3].name });
            PhotonNetwork.Instantiate("Navy", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[0][5].name });
            PhotonNetwork.Instantiate("Airforce", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[0][6].name });
            PhotonNetwork.Instantiate("Recruit", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[1][0].name });
            PhotonNetwork.Instantiate("Spy", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[1][1].name });
            PhotonNetwork.Instantiate("Recruit", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[1][2].name });
            PhotonNetwork.Instantiate("Recruit", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[1][3].name });
            PhotonNetwork.Instantiate("Recruit", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[1][4].name });
            PhotonNetwork.Instantiate("Spy", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[1][5].name });
            PhotonNetwork.Instantiate("Recruit", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Light, Squares[1][6].name });

            foreach (var col in Squares)
            {
                foreach (var square in col)
                {
                    square.AddFog(Team.Light);
                }
            }
        }

        public void SetDarkPieces()
        {
            PhotonNetwork.Instantiate("Airforce", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[13][0].name });
            PhotonNetwork.Instantiate("Navy", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[13][1].name });
            PhotonNetwork.Instantiate("General", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[13][3].name });
            PhotonNetwork.Instantiate("Navy", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[13][5].name });
            PhotonNetwork.Instantiate("Airforce", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[13][6].name });
            PhotonNetwork.Instantiate("Recruit", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[12][0].name });
            PhotonNetwork.Instantiate("Spy", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[12][1].name });
            PhotonNetwork.Instantiate("Recruit", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[12][2].name });
            PhotonNetwork.Instantiate("Recruit", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[12][3].name });
            PhotonNetwork.Instantiate("Recruit", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[12][4].name });
            PhotonNetwork.Instantiate("Spy", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[12][5].name });
            PhotonNetwork.Instantiate("Recruit", Vector2.zero, Quaternion.identity, 0, new object[] { Team.Dark, Squares[12][6].name });

            foreach (var col in Squares)
            {
                foreach (var square in col)
                {
                    square.AddFog(Team.Dark);
                }
            }
        }

        public bool TryGetSquare(Point p, out Square square)
        {
            if (p.X >= 0 && p.X < Squares.Count && p.Y >= 0 && p.Y < Squares[0].Count)
            {
                square = Squares[p.X][p.Y];
                return true;
            }
            square = default;
            return false;
        }

        public bool IsEdge(Point p)
        {
            return p.X == 0 || p.X == 13;
        }

        public bool IsOpponentsEdge(Point p, Team team)
        {
            if (team == Team.Light)
            {
                return p.X == 13;
            }
            else
            {
                return p.X == 0;
            }
        }

        #endregion
    }
}
