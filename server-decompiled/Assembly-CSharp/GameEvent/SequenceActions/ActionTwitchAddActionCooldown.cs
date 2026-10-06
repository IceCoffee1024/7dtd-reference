using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A25 RID: 6693
	[Preserve]
	public class ActionTwitchAddActionCooldown : ActionBaseClientAction
	{
		// Token: 0x0600CC04 RID: 52228 RVA: 0x004A893C File Offset: 0x004A6B3C
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null && entityPlayer.TwitchEnabled && entityPlayer.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Disabled)
			{
				return this.searchType != ActionTwitchAddActionCooldown.SearchTypes.Name || !(this.twitchActions == "");
			}
			return base.CanPerform(target);
		}

		// Token: 0x0600CC05 RID: 52229 RVA: 0x004A8988 File Offset: 0x004A6B88
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

		// Token: 0x0600CC06 RID: 52230 RVA: 0x004A8ADC File Offset: 0x004A6CDC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionTwitchAddActionCooldown.PropTime, ref this.cooldownTime);
			properties.ParseString(ActionTwitchAddActionCooldown.PropTwitchActions, ref this.twitchActions);
			properties.ParseEnum<ActionTwitchAddActionCooldown.SearchTypes>(ActionTwitchAddActionCooldown.PropSearchType, ref this.searchType);
		}

		// Token: 0x0600CC07 RID: 52231 RVA: 0x004A8B18 File Offset: 0x004A6D18
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

		// Token: 0x04009AA5 RID: 39589
		[PublicizedFrom(EAccessModifier.Protected)]
		public float cooldownTime = 5f;

		// Token: 0x04009AA6 RID: 39590
		[PublicizedFrom(EAccessModifier.Protected)]
		public string twitchActions = "";

		// Token: 0x04009AA7 RID: 39591
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTwitchAddActionCooldown.SearchTypes searchType;

		// Token: 0x04009AA8 RID: 39592
		public static string PropTime = "time";

		// Token: 0x04009AA9 RID: 39593
		public static string PropTwitchActions = "action_name";

		// Token: 0x04009AAA RID: 39594
		public static string PropSearchType = "search_type";

		// Token: 0x02001A26 RID: 6694
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum SearchTypes
		{
			// Token: 0x04009AAC RID: 39596
			Name,
			// Token: 0x04009AAD RID: 39597
			Positive,
			// Token: 0x04009AAE RID: 39598
			Negative
		}
	}
}
