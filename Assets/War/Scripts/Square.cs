using Photon.Pun;
using UnityEngine;

namespace War
{
    public class Square : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField]
        private bool _lightFog;

        [SerializeField]
        private bool _darkFog;

        #endregion

        #region Public Properties
        public Piece Piece { get; set; } = null;

        public bool Occupied { get => Piece != null; }

        public Point BoardPosition { get; set; }

        #endregion

        #region Private Fields

        private GameObject _squareHighlightPrefab;
        private GameObject _highlight;

        private GameObject _fogPrefab;
        private GameObject _fog;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            _squareHighlightPrefab = Resources.Load<GameObject>("SquareHighlight");
            _fogPrefab = Resources.Load<GameObject>("Fog");
        }

        private void Update()
        {
            if (_fog != null)
            {
                _fog.transform.SetAsLastSibling();
            }

            if (_highlight != null)
            {
                _highlight.transform.SetAsLastSibling();
            }
        }

        #endregion

        #region Public Methods

        public void RemovePiece()
        {
            gameObject.GetPhotonView().RPC(nameof(RemovePieceRpc), RpcTarget.All);
        }

        [PunRPC]
        private void RemovePieceRpc()
        {
            Piece = null;
        }

        public void AddFog(Team team)
        {
            if ((team == Team.Light && _lightFog) || (team == Team.Dark && _darkFog))
            {
                _fog = Instantiate(_fogPrefab, transform);
                _fog.transform.SetAsLastSibling();
            }
        }

        public void Defog(Team team)
        {
            var player = Player.GetOwnedPlayer();
            if (player != null && player.Team == team)
            {
                if (team == Team.Light && _lightFog)
                {
                    Destroy(_fog);
                    _fog = null;
                    _lightFog = false;
                }
                else if (team == Team.Dark && _darkFog)
                {
                    Destroy(_fog);
                    _fog = null;
                    _darkFog = false;
                }
            }
        }

        public void RestoreFog(Team team)
        {
            var player = Player.GetOwnedPlayer();
            if (player != null && player.Team == team)
            {
                if (_fog != null)
                {
                    Destroy(_fog);
                }

                _fog = Instantiate(_fogPrefab, transform);
                _fog.transform.SetAsLastSibling();
                if (team == Team.Light)
                {
                    _lightFog = true;
                }
                else
                {
                    _darkFog = true;
                }
            }
        }

        public bool IsFoggy(Team team)
        {
            if (team == Team.Light && _lightFog)
            {
                return true;
            }
            else if (team == Team.Dark && _darkFog)
            {
                return true;
            }

            return false;
        }

        public void DeactivateFog()
        {
            if (_fog != null)
            {
                _fog.SetActive(false);
            }
        }

        public void Highlight(bool on)
        {
            if (on)
            {
                _highlight = Instantiate(_squareHighlightPrefab, transform);
                _highlight.transform.SetAsLastSibling();
            }
            else
            {
                Destroy(_highlight);
            }
        }

        #endregion
    }
}