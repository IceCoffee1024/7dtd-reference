using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017CC RID: 6092
	public class TwitchActionManager
	{
		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x0600BC6B RID: 48235 RVA: 0x0045E276 File Offset: 0x0045C476
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

		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x0600BC6C RID: 48236 RVA: 0x0045E28E File Offset: 0x0045C48E
		public static bool HasInstance
		{
			get
			{
				return TwitchActionManager.instance != null;
			}
		}

		// Token: 0x0600BC6D RID: 48237 RVA: 0x0045E298 File Offset: 0x0045C498
		[PublicizedFrom(EAccessModifier.Private)]
		public TwitchActionManager()
		{
		}

		// Token: 0x0600BC6E RID: 48238 RVA: 0x0045E2AB File Offset: 0x0045C4AB
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

		// Token: 0x0600BC6F RID: 48239 RVA: 0x0045E2EB File Offset: 0x0045C4EB
		public void AddAction(TwitchAction action)
		{
			if (!TwitchActionManager.TwitchActions.ContainsKey(action.Name))
			{
				TwitchActionManager.TwitchActions.Add(action.Name, action);
			}
		}

		// Token: 0x0600BC70 RID: 48240 RVA: 0x0045E310 File Offset: 0x0045C510
		public void AddVoteClass(TwitchVote vote)
		{
			TwitchActionManager.TwitchVotes.Add(vote.VoteName, vote);
		}

		// Token: 0x0600BC71 RID: 48241 RVA: 0x0045E324 File Offset: 0x0045C524
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

		// Token: 0x0600BC72 RID: 48242 RVA: 0x0045E368 File Offset: 0x0045C568
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

		// Token: 0x04008D49 RID: 36169
		[PublicizedFrom(EAccessModifier.Private)]
		public static TwitchActionManager instance = null;

		// Token: 0x04008D4A RID: 36170
		public List<TwitchActionManager.ActionCategory> CategoryList = new List<TwitchActionManager.ActionCategory>();

		// Token: 0x04008D4B RID: 36171
		public static Dictionary<string, TwitchAction> TwitchActions = new Dictionary<string, TwitchAction>();

		// Token: 0x04008D4C RID: 36172
		public static Dictionary<string, TwitchVote> TwitchVotes = new Dictionary<string, TwitchVote>();

		// Token: 0x020017CD RID: 6093
		public class ActionCategory
		{
			// Token: 0x04008D4D RID: 36173
			public string Name;

			// Token: 0x04008D4E RID: 36174
			public string DisplayName;

			// Token: 0x04008D4F RID: 36175
			public string Icon;

			// Token: 0x04008D50 RID: 36176
			public bool ShowInCommandList = true;

			// Token: 0x04008D51 RID: 36177
			public bool AlwaysShowInMenu;
		}
	}
}
