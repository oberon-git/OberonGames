using Photon.Pun;
using UnityEngine;

namespace War
{
    public class GameManager : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField]
        private GameObject _gameStateObject;
        private GameState _gameState;

        #endregion

        #region Unity Messages

        private void Start()
        {
            _gameState = _gameStateObject.GetComponent<GameState>();

            var player = Player.GetOwnedPlayer();
            if (player != null && player.Team == Team.Light)
            {
                _gameState.Board.GetComponent<Board>().SetLightPieces();
            }
            else if (player != null && player.Team == Team.Dark)
            {
                _gameState.Board.GetComponent<Board>().SetDarkPieces();
                _gameState.StartGame();
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        #endregion

        #region Public Methods

        public void PlayAgain()
        {
            gameObject.GetPhotonView().RPC(nameof(PlayAgainRpc), RpcTarget.All);
        }

        [PunRPC]
        private void PlayAgainRpc()
        {
            var player = Player.GetOwnedPlayer();
            player.Team = player.Team == Team.Light ? Team.Dark : Team.Light;
            PhotonNetwork.LoadLevel("War");
        }

        #endregion

        #region Static Methods

        public static void StartGame(string username)
        {
            var data = new object[3];
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                data[0] = Team.Light;
                data[1] = "PlayerOne";
                data[2] = username;
            }
            else
            {
                data[0] = Team.Dark;
                data[1] = "PlayerTwo";
                data[2] = username;
            }

            PhotonNetwork.Instantiate("WarPlayer", Vector2.zero, Quaternion.identity, 0, data);
            PhotonNetwork.LoadLevel("War");
        }

        #endregion
    }
}