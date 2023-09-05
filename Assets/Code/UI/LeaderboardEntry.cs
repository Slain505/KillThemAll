using Code.Model;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    /// <summary>
    /// Represents an individual entry in the game's leaderboard.
    /// It manages the display of player's rank, name, and score.
    /// </summary>
    public class LeaderboardEntry : MonoBehaviour 
    {
        [SerializeField] private TextMeshProUGUI positionText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI scoreText;

        private LeaderboardEntryModel entryModel;

        /// <summary>
        /// Sets the leaderboard entry data and updates the visual representation.
        /// </summary>
        /// <param name="position">The rank or position of the player.</param>
        /// <param name="model">The data model representing the leaderboard entry.</param>
        public void SetData(int position, LeaderboardEntryModel model)
        {
            entryModel = model;
            positionText.text = position.ToString();
            nameText.text = entryModel.Name;
            scoreText.text = entryModel.Score.ToString();
            scoreText.color = entryModel.IsNew ? Color.red : Color.white;
        }
    }
}