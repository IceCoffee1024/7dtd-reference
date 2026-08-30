using System;
using System.Collections.Generic;
using Twitch;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D2 RID: 6610
	[Preserve]
	public class ActionTwitchAddActionCooldown : ActionBaseClientAction
	{
		// Token: 0x0600C923 RID: 51491 RVA: 0x0049E07C File Offset: 0x0049C27C
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null && entityPlayer.TwitchEnabled && entityPlayer.TwitchActionsEnabled != EntityPlayer.TwitchActionsStates.Disabled)
			{
				return this.searchType != ActionTwitchAddActionCooldown.SearchTypes.Name || !(this.twitchActions == "");
			}
			return base.CanPerform(target);
		}

		// Token: 0x0600C924 RID: 51492 RVA: 0x0049E0C8 File Offset: 0x0049C2C8
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

		// Token: 0x0600C925 RID: 51493 RVA: 0x0049E21C File Offset: 0x0049C41C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseFloat(ActionTwitchAddActionCooldown.PropTime, ref this.cooldownTime);
			properties.ParseString(ActionTwitchAddActionCooldown.PropTwitchActions, ref this.twitchActions);
			properties.ParseEnum<ActionTwitchAddActionCooldown.SearchTypes>(ActionTwitchAddActionCooldown.PropSearchType, ref this.searchType);
		}

		// Token: 0x0600C926 RID: 51494 RVA: 0x0049E258 File Offset: 0x0049C458
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

		// Token: 0x040098AE RID: 39086
		[PublicizedFrom(EAccessModifier.Protected)]
		public float cooldownTime = 5f;

		// Token: 0x040098AF RID: 39087
		[PublicizedFrom(EAccessModifier.Protected)]
		public string twitchActions = "";

		// Token: 0x040098B0 RID: 39088
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTwitchAddActionCooldown.SearchTypes searchType;

		// Token: 0x040098B1 RID: 39089
		public static string PropTime = "time";

		// Token: 0x040098B2 RID: 39090
		public static string PropTwitchActions = "action_name";

		// Token: 0x040098B3 RID: 39091
		public static string PropSearchType = "search_type";

		// Token: 0x020019D3 RID: 6611
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum SearchTypes
		{
			// Token: 0x040098B5 RID: 39093
			Name,
			// Token: 0x040098B6 RID: 39094
			Positive,
			// Token: 0x040098B7 RID: 39095
			Negative
		}
	}
}
