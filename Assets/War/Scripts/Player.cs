using Photon.Pun;
using UnityEngine;

namespace War
{
    public class Player : MonoBehaviour, IPunInstantiateMagicCallback
    {

        #region Public Properties

        public Team Team { get; set; }

        public string Name { get; set; }

        #endregion

        #region PUN Callbacks

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            var data = info.photonView.InstantiationData;
            Team = (Team)data[0];
            gameObject.name = (string)data[1];
            Name = (string)data[2];

            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region Static Methods

        public static Player GetOwnedPlayer()
        {
            var p1 = GameObject.Find("PlayerOne");
            if (p1 != null && p1.GetPhotonView().IsMine)
            {
                return p1.GetComponent<Player>();
            }

            var p2 = GameObject.Find("PlayerTwo");
            if (p2 != null && p2.GetPhotonView().IsMine)
            {
                return p2.GetComponent<Player>();
            }

            return null;
        }

        public static Player GetOpponentsPlayer()
        {
            var p1 = GameObject.Find("PlayerOne");
            if (p1 != null && !p1.GetPhotonView().IsMine)
            {
                return p1.GetComponent<Player>();
            }

            var p2 = GameObject.Find("PlayerTwo");
            if (p2 != null && !p2.GetPhotonView().IsMine)
            {
                return p2.GetComponent<Player>();
            }

            return null;
        }

        #endregion
    }
}
