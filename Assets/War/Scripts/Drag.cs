using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace War
{
    public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Public Properties

        public bool CanDrag { get => _gameState.Ready && _gameState.Turn == _piece.Team && gameObject.GetPhotonView().IsMine; }

        public Transform ParentBeforeDrag { get; set; }

        public Transform ParentAfterDrag { get; set; }

        #endregion

        #region Private Fields

        private Image _image;

        private Piece _piece;

        private GameState _gameState;

        private List<Square> _moves = new();

        #endregion

        #region Unity Messages

        private void Start()
        {
            _image = GetComponentInChildren<Image>();
            _piece = GetComponent<Piece>();

            var gameState = transform.root.GetChild(0).gameObject;
            _gameState = gameState.GetComponent<GameState>();
        }

        #endregion

        #region Drag Handlers

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!CanDrag) return;

            ParentBeforeDrag = transform.parent;
            ParentAfterDrag = transform.parent;

            transform.SetParent(transform.root);
            transform.SetAsLastSibling();

            _moves = _piece.GetMoves(_gameState.Board);

            _image.raycastTarget = false;

            foreach (var square in _moves)
            {
                square.Highlight(true);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!CanDrag) return;
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!CanDrag) return;

            var drop = ParentAfterDrag.gameObject;
            if (drop.TryGetComponent(out Square square) && _moves.Contains(square))
            {
                gameObject.GetPhotonView().RPC(nameof(ReparentAndMoveRpc), RpcTarget.All, square.gameObject.GetPhotonView().ViewID);
            }
            else
            {
                transform.SetParent(ParentBeforeDrag);
                transform.localPosition = Vector2.zero;
            }

            _image.raycastTarget = true;

            foreach (var move in _moves)
            {
                move.Highlight(false);
            }

            _moves.Clear();
        }

        #endregion

        #region Private Methods

        [PunRPC]
        private void ReparentAndMoveRpc(int viewID)
        {
            Square square = PhotonNetwork.GetPhotonView(viewID).gameObject.GetComponent<Square>();
            _piece.Move(_gameState.Board, square);
            _gameState.ChangeTurn();
        }

        #endregion
    }
}