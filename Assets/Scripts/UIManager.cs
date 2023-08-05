using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI _CoinText;
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
    
    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }


    public void DisplayCoinTotal(int displayCoinTotal) {
        _CoinText.text = displayCoinTotal.ToString();
    }
}
