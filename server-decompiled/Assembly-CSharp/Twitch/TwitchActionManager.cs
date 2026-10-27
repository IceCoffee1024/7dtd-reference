using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200184F RID: 6223
	public class TwitchActionManager
	{
		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x0600C05C RID: 49244 RVA: 0x0046BEFA File Offset: 0x0046A0FA
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

		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x0600C05D RID: 49245 RVA: 0x0046BF12 File Offset: 0x0046A112
		public static bool HasInstance
		{
			get
			{
				return TwitchActionManager.instance != null;
			}
		}

		// Token: 0x0600C05E RID: 49246 RVA: 0x0046BF1C File Offset: 0x0046A11C
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchActionManager()
		{
		}

		// Token: 0x0600C05F RID: 49247 RVA: 0x0046BF2F File Offset: 0x0046A12F
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

		// Token: 0x0600C060 RID: 49248 RVA: 0x0046BF6F File Offset: 0x0046A16F
		public void AddAction(TwitchAction action)
		{
			if (!TwitchActionManager.TwitchActions.ContainsKey(action.Name))
			{
				TwitchActionManager.TwitchActions.Add(action.Name, action);
			}
		}

		// Token: 0x0600C061 RID: 49249 RVA: 0x0046BF94 File Offset: 0x0046A194
		public void AddVoteClass(TwitchVote vote)
		{
			TwitchActionManager.TwitchVotes.Add(vote.VoteName, vote);
		}

		// Token: 0x0600C062 RID: 49250 RVA: 0x0046BFA8 File Offset: 0x0046A1A8
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

		// Token: 0x0600C063 RID: 49251 RVA: 0x0046BFEC File Offset: 0x0046A1EC
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

		// Token: 0x04009014 RID: 36884
		[PublicizedFrom(EAccessModifier.Private)]
		public static TwitchActionManager instance = null;

		// Token: 0x04009015 RID: 36885
		public List<TwitchActionManager.ActionCategory> CategoryList = new List<TwitchActionManager.ActionCategory>();

		// Token: 0x04009016 RID: 36886
		public static Dictionary<string, TwitchAction> TwitchActions = new Dictionary<string, TwitchAction>();

		// Token: 0x04009017 RID: 36887
		public static Dictionary<string, TwitchVote> TwitchVotes = new Dictionary<string, TwitchVote>();

		// Token: 0x02001850 RID: 6224
		public class ActionCategory
		{
			// Token: 0x04009018 RID: 36888
			public string Name;

			// Token: 0x04009019 RID: 36889
			public string DisplayName;

			// Token: 0x0400901A RID: 36890
			public string Icon;

			// Token: 0x0400901B RID: 36891
			public bool ShowInCommandList = true;

			// Token: 0x0400901C RID: 36892
			public bool AlwaysShowInMenu;
		}
	}
}
