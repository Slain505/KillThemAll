using System.Collections.Generic;
using Code.Model;
using Code.Shared;
using Code.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Model.Popups
{
    /// <summary>
    /// Represents the end of the game popup, displaying scores and providing an option to replay.
    /// It manages the leaderboard and player's final score.
    /// </summary>
    public class EndPopup : PopupStateModel<EndPopup, PlayerModel>
    {
        [SerializeField] private Button playButton;
        [SerializeField] private RectTransform leaderboardContent;
        [SerializeField] private TextMeshProUGUI scoreText;

        private List<GameObject> leaderboardEntries = new List<GameObject>();
        
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
            
            var leaderboardModel = Code.Game.Game.Instance.LeaderboardModel;
            leaderboardModel.AddItem(new LeaderboardEntryModel(Code.Game.Game.Instance.PlayerName, model.Score, true));
            scoreText.text = $"Your score: {model.Score}";
            PopulateLeaderboard(Code.Game.Game.Instance.LeaderboardModel);
            Debug.Log("EndPopup opened. Leaderboard populated.");
        }

        protected override void OnClose()
        {
            base.OnClose();
            foreach (var entry in leaderboardEntries)
            {
                Code.Game.Game.Get<ObjectPoolsController>().LeaderboardPool.ReturnObjectToPool(entry);
            }
            
            leaderboardEntries.Clear();
            Debug.Log("EndPopup closed. Leaderboard entries cleared.");
        }

        /// <summary>
        /// Shows Top-10 players in the leaderboard.
        /// </summary>
        private void PopulateLeaderboard(LeaderboardModel leaderboardModel)
        {
            for (int i = 0; i < leaderboardModel.NumItems && i < 10; i++)
            {
                var data = leaderboardModel.GetItem(i);
                var item = Code.Game.Game.Get<ObjectPoolsController>().LeaderboardPool.GetObject();
                item.GetComponent<LeaderboardEntry>().SetData(i + 1, data);
                item.transform.SetParent(leaderboardContent, false);
                
                if (data.IsNew)
                {
                    data.IsNew = false;
                }

                leaderboardEntries.Add(item);
            }

            leaderboardContent.sizeDelta = new Vector2(leaderboardContent.sizeDelta.x,leaderboardModel.NumItems * 60);
            leaderboardContent.anchoredPosition = new Vector2(leaderboardContent.anchoredPosition.x, -leaderboardContent.sizeDelta.y / 2);
        }

        private void OnPlayButtonClicked()
        {
            SceneManager.LoadScene(1);
            Close();
        }
    }
}