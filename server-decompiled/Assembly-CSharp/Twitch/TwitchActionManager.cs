using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017F2 RID: 6130
	public class TwitchActionManager
	{
		// Token: 0x17001712 RID: 5906
		// (get) Token: 0x0600BDF0 RID: 48624 RVA: 0x00464012 File Offset: 0x00462212
		public static TwitchActionManager Current
		{
			get
			{
				if (TwitchActionManager.instance == null)
				{
					TwitchActionManager.instance = new TwitchActionManager();
				}
				return TwitchActionManager.instance;
			}
		}

		// Token: 0x17001713 RID: 5907
		// (get) Token: 0x0600BDF1 RID: 48625 RVA: 0x0046402A File Offset: 0x0046222A
		public static bool HasInstance
		{
			get
			{
				return TwitchActionManager.instance != null;
			}
		}

		// Token: 0x0600BDF2 RID: 48626 RVA: 0x00464034 File Offset: 0x00462234
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchActionManager()
		{
		}

		// Token: 0x0600BDF3 RID: 48627 RVA: 0x00464047 File Offset: 0x00462247
		public void Cleanup()
		{
			if (TwitchActionManager.TwitchActions != null)
			{
				TwitchActionManager.TwitchActions.Clear();
			}
			if (TwitchActionManager.TwitchVotes != null)
			{
				TwitchActionManager.TwitchVotes.Clear();
			}
			this.CategoryList.Clear();
			if (TwitchManager.HasInstance)
			{
				TwitchManager.Current.CleanupData();
			}
		}

		// Token: 0x0600BDF4 RID: 48628 RVA: 0x00464087 File Offset: 0x00462287
		public void AddAction(TwitchAction action)
		{
			if (!TwitchActionManager.TwitchActions.ContainsKey(action.Name))
			{
				TwitchActionManager.TwitchActions.Add(action.Name, action);
			}
		}

		// Token: 0x0600BDF5 RID: 48629 RVA: 0x004640AC File Offset: 0x004622AC
		public void AddVoteClass(TwitchVote vote)
		{
			TwitchActionManager.TwitchVotes.Add(vote.VoteName, vote);
		}

		// Token: 0x0600BDF6 RID: 48630 RVA: 0x004640C0 File Offset: 0x004622C0
		public int GetCategoryIndex(string categoryName)
		{
			for (int i = 0; i < this.CategoryList.Count; i++)
			{
				if (categoryName.StartsWith(this.CategoryList[i].Name))
				{
					return i;
				}
			}
			return 9999;
		}

		// Token: 0x0600BDF7 RID: 48631 RVA: 0x00464104 File Offset: 0x00462304
		public TwitchActionManager.ActionCategory GetCategory(string categoryName)
		{
			for (int i = 0; i < this.CategoryList.Count; i++)
			{
				if (this.CategoryList[i].Name == categoryName)
				{
					return this.CategoryList[i];
				}
			}
			return null;
		}

		// Token: 0x04008E65 RID: 36453
		[PublicizedFrom(EAccessModifier.Private)]
		public static TwitchActionManager instance = null;

		// Token: 0x04008E66 RID: 36454
		public List<TwitchActionManager.ActionCategory> CategoryList = new List<TwitchActionManager.ActionCategory>();

		// Token: 0x04008E67 RID: 36455
		public static Dictionary<string, TwitchAction> TwitchActions = new Dictionary<string, TwitchAction>();

		// Token: 0x04008E68 RID: 36456
		public static Dictionary<string, TwitchVote> TwitchVotes = new Dictionary<string, TwitchVote>();

		// Token: 0x020017F3 RID: 6131
		public class ActionCategory
		{
			// Token: 0x04008E69 RID: 36457
			public string Name;

			// Token: 0x04008E6A RID: 36458
			public string DisplayName;

			// Token: 0x04008E6B RID: 36459
			public string Icon;

			// Token: 0x04008E6C RID: 36460
			public bool ShowInCommandList = true;

			// Token: 0x04008E6D RID: 36461
			public bool AlwaysShowInMenu;
		}
	}
}
