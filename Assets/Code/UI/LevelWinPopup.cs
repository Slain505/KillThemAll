using Code.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI
{
    public class LevelWinPopup : PopupStateModel<LevelWinPopup>
    {
        [SerializeField] private Button playButton;
        [SerializeField] private TextMeshProUGUI levelWinText;
        
        public override void Init()
        {
            base.Init();
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        
        public override void Dispose()
        {
            base.Dispose();
            playButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
        
        protected override void OnOpen()
        {
            base.OnOpen();
            
            levelWinText.text = "You win!";
            Debug.Log("LevelWinPopup opened.");
        }

        protected override void OnClose()
        {
            base.OnClose();
            Debug.Log("LevelWinPopup closed.");
        }

        private void OnPlayButtonClicked()
        {
            SceneManager.LoadScene(1);
            Close();
        }
    }
}