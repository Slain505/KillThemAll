using System.Collections.Generic;
using UnityEngine;

namespace Code.Model
{
	public class LeaderboardModel : IListModel<LeaderboardEntryModel>
	{
		public int NumItems => entries.Count;

		public LeaderboardModel()
		{
			//mock data
			for (int i = 0; i < 10; ++i)
			{
				var r = Random.Range(0, NameList.Names.Length);
				var p = new LeaderboardEntryModel($"{NameList.Names[r]}{(i * 317) % 100}", Random.Range(1, 50));
				entries.Add(p);
			}
			entries.Sort();
			entries.Reverse();
		}

		public LeaderboardEntryModel GetItem(int index) => entries[index];

		public void AddItem(LeaderboardEntryModel model)
		{
			entries.Add(model);
			entries.Sort();
			entries.Reverse();
		}

		private readonly List<LeaderboardEntryModel> entries = new List<LeaderboardEntryModel>();
	}
}