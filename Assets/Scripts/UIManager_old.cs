using TMPro;
using UnityEngine;

public class UIManager_old : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _livesText;
    [SerializeField] private GameObject _gameOver;
    private static UIManager_old _instance;
    public static UIManager_old Instance { get { return _instance; } }
    
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    
    public void DisplayCoinTotal(int displayCoinTotal) {
        _coinText.text = displayCoinTotal.ToString();
    }
    public void DisplayLivesTotal(int lives) {
        _livesText.text = lives.ToString();
    }

    public void DisplayGameOver() {
        _gameOver.SetActive(true);
    }
    
}
