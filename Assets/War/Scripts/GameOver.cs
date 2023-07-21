using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    private TextMeshProUGUI _gameOverText;

    #endregion

    #region Public Methods

    public void SetGameOverText(string text)
    {
        _gameOverText.text = text;
    }

    #endregion
}
