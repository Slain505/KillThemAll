using Code.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Model.Popups
{
	public class SplashPopup : PopupState<SplashPopup>
	{
		[SerializeField] private Button playButton;
		[SerializeField] private TMP_InputField nameInputField;

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

		private void OnPlayButtonClicked()
		{
			SceneManager.LoadScene(1);

			Code.Game.Game.Instance.PlayerName = !string.IsNullOrEmpty(nameInputField.text) ? nameInputField.text : "Player";

			Close();
		}
	}
}