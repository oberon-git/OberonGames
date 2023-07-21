using Photon.Pun;
using TMPro;
using UnityEngine;

namespace War
{
    public class GameState : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField]
        private Board _board;

        [SerializeField]
        private TextMeshProUGUI _turnLabel;

        [SerializeField]
        private GameOver _gameOver;

        #endregion

        #region Public Properties

        public bool Ready { get; set; } = false;

        public Board Board { get => _board; }

        public Team Turn { get; set; } = Team.Light;

        #endregion

        #region Private Fields

        private Player _player;
        private Player _otherPlayer;

        #endregion

        #region Public Methods

        public void StartGame()
        {
            gameObject.GetPhotonView().RPC(nameof(StartGameRpc), RpcTarget.All);
        }

        [PunRPC]
        private void StartGameRpc()
        {
            Ready = true;
            _player = Player.GetOwnedPlayer();
            _otherPlayer = Player.GetOpponentsPlayer();
            _turnLabel.gameObject.SetActive(true);
            SetTurnLabelText();
        }

        public void ChangeTurn()
        {
            if (Won())
            {
                _turnLabel.gameObject.SetActive(false);

                foreach (var col in Board.Squares)
                {
                    foreach (var square in col)
                    {
                        square.DeactivateFog();
                    }
                }

                var player = Player.GetOwnedPlayer();
                if (player.Team == Turn)
                {
                    _gameOver.SetGameOverText("You Won! :)");
                }
                else
                {
                    _gameOver.SetGameOverText("You Lost! :(");
                }

                _gameOver.gameObject.SetActive(true);
            }
            else
            {
                if (Turn == Team.Light)
                {
                    Turn = Team.Dark;
                }
                else
                {
                    Turn = Team.Light;
                }

                SetTurnLabelText();
            }
        }

        #endregion

        #region Private Methods

        private void SetTurnLabelText(int retries = 0)
        {

            if (_player != null && _player.Team == Turn)
            {
                _turnLabel.text = _player.Name + "'s Turn";
            }
            else if (_otherPlayer != null && _otherPlayer.Team == Turn)
            {
                _turnLabel.text = _otherPlayer.Name + "'s Turn";
            }
            else
            {

                if (retries <= 1)
                {
                    _player = Player.GetOwnedPlayer();
                    _otherPlayer = Player.GetOpponentsPlayer();
                    SetTurnLabelText(retries + 1);
                }
                else
                {
                    _turnLabel.text = "Error: player not set";
                }
            }
        }

        private bool Won()
        {
            var generalAlive = false;
            var generalOnEdge = false;
            foreach (var col in Board.Squares)
            {
                foreach (var square in col)
                {
                    if (square.Occupied && square.Piece.Team != Turn && square.Piece.GetType() == typeof(General))
                    {
                        generalAlive = true;
                    }
                    if (Board.IsOpponentsEdge(square.BoardPosition, Turn))
                    {
                        if (square.Occupied && square.Piece.Team == Turn && square.Piece.GetType() == typeof(General))
                        {
                            generalOnEdge = true;
                        }
                    }
                }
            }

            return !generalAlive || generalOnEdge;
        }

        #endregion
    }
}