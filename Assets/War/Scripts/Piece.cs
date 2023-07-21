using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace War
{
    public abstract class Piece : MonoBehaviour, IPunInstantiateMagicCallback
    {
        #region Public Properties

        public Team Team { get; set; }

        #endregion

        #region Private Fields

        protected Square _square;
        protected Image _image;

        #endregion

        #region PUN Callbacks

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var data = info.photonView.InstantiationData;
            Team = (Team)data[0];
            string parentName = (string)data[1];
            var parent = GameObject.Find(parentName);
            transform.SetParent(parent.transform);
            transform.localPosition = Vector2.zero;

            _square = parent.GetComponent<Square>();
            _square.Piece = this;

            var _image = GetComponentInChildren<Image>();
            if (Team == Team.Dark)
            {
                _image.color = Color.black;
                transform.Rotate(new(0, 180, 0));
            }

            if (gameObject.GetPhotonView().IsMine)
            {
                _image.raycastTarget = true;
            }
        }

        #endregion

        #region Public Methods

        public void DestroySelf()
        {
            _square.RemovePiece();
            PhotonNetwork.Destroy(gameObject.GetPhotonView());
        }

        public abstract List<Square> GetMoves(Board board);

        public abstract void Move(Board board, Square move);

        #endregion

        #region Private Methods

        protected void OccupySquare(Square square, bool restoreFog = true)
        {
            if (restoreFog) _square.RestoreFog(Team);
            _square.RemovePiece();
            _square = square;
            if (_square.Occupied && _square.Piece.gameObject.GetPhotonView().IsMine)
            {
                PhotonNetwork.Destroy(_square.Piece.gameObject.GetPhotonView());
            }

            _square.Piece = this;
            _square.Defog(Team);

            transform.SetParent(_square.transform);
            transform.localPosition = Vector2.zero;
        }

        protected abstract string GetSpriteName();

        #endregion
    }
}
