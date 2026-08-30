using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017FE RID: 6142
	public class TwitchActionPreset
	{
		// Token: 0x0600BE24 RID: 48676 RVA: 0x0046582A File Offset: 0x00463A2A
		public void AddCooldownModifier(TwitchActionCooldownModifier modifier)
		{
			if (this.ActionCooldownModifiers == null)
			{
				this.ActionCooldownModifiers = new List<TwitchActionCooldownModifier>();
			}
			this.ActionCooldownModifiers.Add(modifier);
		}

		// Token: 0x0600BE25 RID: 48677 RVA: 0x0046584C File Offset: 0x00463A4C
		public void HandleCooldowns()
		{
			foreach (TwitchAction twitchAction in TwitchActionManager.TwitchActions.Values)
			{
				twitchAction.Cooldown = twitchAction.OriginalCooldown;
				if (twitchAction.IsInPreset(this) && this.ActionCooldownModifiers != null)
				{
					for (int i = 0; i < this.ActionCooldownModifiers.Count; i++)
					{
						TwitchActionCooldownModifier twitchActionCooldownModifier = this.ActionCooldownModifiers[i];
						if (twitchActionCooldownModifier.ActionName == twitchAction.Name || twitchActionCooldownModifier.CategoryName == twitchAction.MainCategory.Name)
						{
							switch (twitchActionCooldownModifier.Modifier)
							{
							case PassiveEffect.ValueModifierTypes.base_set:
								twitchAction.Cooldown = twitchActionCooldownModifier.Value;
								break;
							case PassiveEffect.ValueModifierTypes.base_add:
								twitchAction.Cooldown += twitchActionCooldownModifier.Value;
								break;
							case PassiveEffect.ValueModifierTypes.base_subtract:
								twitchAction.Cooldown -= twitchActionCooldownModifier.Value;
								break;
							case PassiveEffect.ValueModifierTypes.perc_set:
								twitchAction.Cooldown *= twitchActionCooldownModifier.Value;
								break;
							case PassiveEffect.ValueModifierTypes.perc_add:
								twitchAction.Cooldown += twitchAction.Cooldown * twitchActionCooldownModifier.Value;
								break;
							case PassiveEffect.ValueModifierTypes.perc_subtract:
								twitchAction.Cooldown -= twitchAction.Cooldown * twitchActionCooldownModifier.Value;
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x04008F0E RID: 36622
		public string Name;

		// Token: 0x04008F0F RID: 36623
		public bool IsEnabled = true;

		// Token: 0x04008F10 RID: 36624
		public bool IsDefault;

		// Token: 0x04008F11 RID: 36625
		public bool IsEmpty;

		// Token: 0x04008F12 RID: 36626
		public string Title;

		// Token: 0x04008F13 RID: 36627
		public string Description;

		// Token: 0x04008F14 RID: 36628
		public bool AllowPointGeneration = true;

		// Token: 0x04008F15 RID: 36629
		public bool UseHelperReward = true;

		// Token: 0x04008F16 RID: 36630
		public bool ShowNewCommands = true;

		// Token: 0x04008F17 RID: 36631
		public List<TwitchActionCooldownModifier> ActionCooldownModifiers;

		// Token: 0x04008F18 RID: 36632
		public List<string> AddedActions = new List<string>();

		// Token: 0x04008F19 RID: 36633
		public List<string> RemovedActions = new List<string>();
	}
}
