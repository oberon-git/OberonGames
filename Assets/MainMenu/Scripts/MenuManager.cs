using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

public class MenuManager : MonoBehaviourPunCallbacks
{
    #region Serialized Fields

    [SerializeField]
    private GameObject _usernamePanel;

    [SerializeField]
    private TMP_InputField _usernameInput;

    [SerializeField]
    private Button _submitButton;

    [SerializeField]
    private TMP_Dropdown _gameSelect;

    [SerializeField]
    private Button _joinButton;

    [SerializeField]
    private TMP_InputField _roomNameInput;

    #endregion

    #region Private Fields

    private string _username;
    private string _roomName;
    private int _gameSelection;

    #endregion

    #region Unity Messages

    private void Start()
    {
        _usernameInput.onValueChanged.AddListener(ValidateUsername);

        _gameSelect.AddOptions(new List<string>() 
        {
            "War",
            //"Ski Race",
            //"Cats"
        });

        _gameSelect.onValueChanged.AddListener(selection => _gameSelection = selection);

        _submitButton.onClick.AddListener(Submit);
        _joinButton.onClick.AddListener(Join);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    #endregion

    #region PUN Callbacks

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        _usernamePanel.SetActive(false);
        _gameSelect.gameObject.SetActive(true);
        _roomNameInput.gameObject.SetActive(true);
        _roomNameInput.onValueChanged.AddListener(ValidateRoomName);
        ValidateRoomName(_roomNameInput.text);
    }

    public override void OnJoinedRoom()
    {
        switch (_gameSelection)
        {
            case 0:
                War.GameManager.StartGame(_username);
                break;
            default:
                Debug.Log("Unimplemented");
                break;
        }
    }

    #endregion

    #region Private Methods

    private void Submit()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Join()
    {
        PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions() { MaxPlayers = 2 }, TypedLobby.Default);
    }

    private void ValidateUsername(string username)
    {
        if (username != null && username.Length >= 3)
        {
            _username = username;
            _submitButton.gameObject.SetActive(true);
        }
        else
        {
            _submitButton.gameObject.SetActive(false);
        }
    }

    private void ValidateRoomName(string roomName)
    {
        if (roomName != null && roomName.Length >= 3)
        {
            _roomName = roomName;
            _joinButton.gameObject.SetActive(true);
        }
        else
        {
            _joinButton.gameObject.SetActive(false);
        }
    }

    #endregion
}
