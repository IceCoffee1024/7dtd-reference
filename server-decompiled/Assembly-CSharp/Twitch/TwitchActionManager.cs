using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200181E RID: 6174
	public class TwitchActionManager
	{
		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x0600BF48 RID: 48968 RVA: 0x0046854E File Offset: 0x0046674E
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

		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x0600BF49 RID: 48969 RVA: 0x00468566 File Offset: 0x00466766
		public static bool HasInstance
		{
			get
			{
				return TwitchActionManager.instance != null;
			}
		}

		// Token: 0x0600BF4A RID: 48970 RVA: 0x00468570 File Offset: 0x00466770
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchActionManager()
		{
		}

		// Token: 0x0600BF4B RID: 48971 RVA: 0x00468583 File Offset: 0x00466783
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

		// Token: 0x0600BF4C RID: 48972 RVA: 0x004685C3 File Offset: 0x004667C3
		public void AddAction(TwitchAction action)
		{
			if (!TwitchActionManager.TwitchActions.ContainsKey(action.Name))
			{
				TwitchActionManager.TwitchActions.Add(action.Name, action);
			}
		}

		// Token: 0x0600BF4D RID: 48973 RVA: 0x004685E8 File Offset: 0x004667E8
		public void AddVoteClass(TwitchVote vote)
		{
			TwitchActionManager.TwitchVotes.Add(vote.VoteName, vote);
		}

		// Token: 0x0600BF4E RID: 48974 RVA: 0x004685FC File Offset: 0x004667FC
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

		// Token: 0x0600BF4F RID: 48975 RVA: 0x00468640 File Offset: 0x00466840
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

		// Token: 0x04008F2B RID: 36651
		[PublicizedFrom(EAccessModifier.Private)]
		public static TwitchActionManager instance = null;

		// Token: 0x04008F2C RID: 36652
		public List<TwitchActionManager.ActionCategory> CategoryList = new List<TwitchActionManager.ActionCategory>();

		// Token: 0x04008F2D RID: 36653
		public static Dictionary<string, TwitchAction> TwitchActions = new Dictionary<string, TwitchAction>();

		// Token: 0x04008F2E RID: 36654
		public static Dictionary<string, TwitchVote> TwitchVotes = new Dictionary<string, TwitchVote>();

		// Token: 0x0200181F RID: 6175
		public class ActionCategory
		{
			// Token: 0x04008F2F RID: 36655
			public string Name;

			// Token: 0x04008F30 RID: 36656
			public string DisplayName;

			// Token: 0x04008F31 RID: 36657
			public string Icon;

			// Token: 0x04008F32 RID: 36658
			public bool ShowInCommandList = true;

			// Token: 0x04008F33 RID: 36659
			public bool AlwaysShowInMenu;
		}
	}
}
