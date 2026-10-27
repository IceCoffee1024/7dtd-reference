using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A56 RID: 6742
	[Preserve]
	public class ActionTwitchAddActionCooldown : ActionBaseClientAction
	{
		// Token: 0x0600CD18 RID: 52504 RVA: 0x004AC438 File Offset: 0x004AA638
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null && entityPlayer.TwitchEnabled && entityPlayer.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Disabled)
			{
				return this.searchType != ActionTwitchAddActionCooldown.SearchTypes.Name || !(this.twitchActions == "");
			}
			return base.CanPerform(target);
		}

		// Token: 0x0600CD19 RID: 52505 RVA: 0x004AC484 File Offset: 0x004AA684
		public override void OnClientPerform(Entity target)
		{
			TwitchManager twitchManager = TwitchManager.Current;
			if (!twitchManager.TwitchActive)
			{
				return;
			}
			float currentUnityTime = twitchManager.CurrentUnityTime;
			switch (this.searchType)
			{
			case ActionTwitchAddActionCooldown.SearchTypes.Name:
			{
				string[] array = this.twitchActions.Split(',', StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					if (TwitchActionManager.TwitchActions.ContainsKey(array[i]))
					{
						TwitchAction twitchAction = TwitchActionManager.TwitchActions[array[i]];
						twitchAction.tempCooldown = this.cooldownTime;
						twitchAction.tempCooldownSet = currentUnityTime;
					}
				}
				return;
			}
			case ActionTwitchAddActionCooldown.SearchTypes.Positive:
			{
				using (Dictionary<string, TwitchAction>.ValueCollection.Enumerator enumerator = twitchManager.AvailableCommands.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TwitchAction twitchAction2 = enumerator.Current;
						if (twitchAction2.IsPositive)
						{
							twitchAction2.tempCooldown = this.cooldownTime;
							twitchAction2.tempCooldownSet = currentUnityTime;
						}
					}
					return;
				}
				break;
			}
			case ActionTwitchAddActionCooldown.SearchTypes.Negative:
				break;
			default:
				return;
			}
			foreach (TwitchAction twitchAction3 in twitchManager.AvailableCommands.Values)
			{
				if (!twitchAction3.IsPositive)
				{
					twitchAction3.tempCooldown = this.cooldownTime;
					twitchAction3.tempCooldownSet = currentUnityTime;
				}
			}
		}

		// Token: 0x0600CD1A RID: 52506 RVA: 0x004AC5D8 File Offset: 0x004AA7D8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionTwitchAddActionCooldown.PropTime, ref this.cooldownTime);
			properties.ParseString(ActionTwitchAddActionCooldown.PropTwitchActions, ref this.twitchActions);
			properties.ParseEnum<ActionTwitchAddActionCooldown.SearchTypes>(ActionTwitchAddActionCooldown.PropSearchType, ref this.searchType);
		}

		// Token: 0x0600CD1B RID: 52507 RVA: 0x004AC614 File Offset: 0x004AA814
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTwitchAddActionCooldown
			{
				twitchActions = this.twitchActions,
				cooldownTime = this.cooldownTime,
				searchType = this.searchType
			};
		}

		// Token: 0x04009B8E RID: 39822
		[PublicizedFrom(EAccessModifier.Protected)]
		public float cooldownTime = 5f;

		// Token: 0x04009B8F RID: 39823
		[PublicizedFrom(EAccessModifier.Protected)]
		public string twitchActions = "";

		// Token: 0x04009B90 RID: 39824
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTwitchAddActionCooldown.SearchTypes searchType;

		// Token: 0x04009B91 RID: 39825
		public static string PropTime = "time";

		// Token: 0x04009B92 RID: 39826
		public static string PropTwitchActions = "action_name";

		// Token: 0x04009B93 RID: 39827
		public static string PropSearchType = "search_type";

		// Token: 0x02001A57 RID: 6743
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum SearchTypes
		{
			// Token: 0x04009B95 RID: 39829
			Name,
			// Token: 0x04009B96 RID: 39830
			Positive,
			// Token: 0x04009B97 RID: 39831
			Negative
		}
	}
}
