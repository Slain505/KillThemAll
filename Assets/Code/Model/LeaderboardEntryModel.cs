using System;

namespace Code.Model
{
	public class LeaderboardEntryModel : IComparable<LeaderboardEntryModel>
	{
		public string Name { get; }
		public int Score { get; }
		public bool IsNew { get; set; }

		public LeaderboardEntryModel(string name, int score)
		{
			Name = name;
			Score = score;
		}
		
		public LeaderboardEntryModel(string name, int score, bool isNew)
		{
			Name = name;
			Score = score;
			IsNew = isNew;
		}

		public int CompareTo(LeaderboardEntryModel other)
		{
			return Score.CompareTo(other.Score);
		}
	}
}