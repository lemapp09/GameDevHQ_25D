using TMPro;
using UnityEngine;

namespace GameDevHQ_25dCert
{
    public class UIManager_cert : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _livesText;
        [SerializeField] private GameObject _gameOver;
        private static UIManager_cert _instance;

        public static UIManager_cert Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void DisplayCoinTotal(int displayCoinTotal)
        {
            _coinText.text = displayCoinTotal.ToString();
        }

        public void DisplayLivesTotal(int lives)
        {
            _livesText.text = lives.ToString();
        }

        public void DisplayGameOver()
        {
            _gameOver.SetActive(true);
        }

    }
}