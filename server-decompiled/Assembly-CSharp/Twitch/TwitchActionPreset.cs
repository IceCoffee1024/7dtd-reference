using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200185B RID: 6235
	public class TwitchActionPreset
	{
		// Token: 0x0600C090 RID: 49296 RVA: 0x0046D712 File Offset: 0x0046B912
		public void AddCooldownModifier(TwitchActionCooldownModifier modifier)
		{
			if (this.ActionCooldownModifiers == null)
			{
				this.ActionCooldownModifiers = new List<TwitchActionCooldownModifier>();
			}
			this.ActionCooldownModifiers.Add(modifier);
		}

		// Token: 0x0600C091 RID: 49297 RVA: 0x0046D734 File Offset: 0x0046B934
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

		// Token: 0x040090BD RID: 37053
		public string Name;

		// Token: 0x040090BE RID: 37054
		public bool IsEnabled = true;

		// Token: 0x040090BF RID: 37055
		public bool IsDefault;

		// Token: 0x040090C0 RID: 37056
		public bool IsEmpty;

		// Token: 0x040090C1 RID: 37057
		public string Title;

		// Token: 0x040090C2 RID: 37058
		public string Description;

		// Token: 0x040090C3 RID: 37059
		public bool AllowPointGeneration = true;

		// Token: 0x040090C4 RID: 37060
		public bool UseHelperReward = true;

		// Token: 0x040090C5 RID: 37061
		public bool ShowNewCommands = true;

		// Token: 0x040090C6 RID: 37062
		public List<TwitchActionCooldownModifier> ActionCooldownModifiers;

		// Token: 0x040090C7 RID: 37063
		public List<string> AddedActions = new List<string>();

		// Token: 0x040090C8 RID: 37064
		public List<string> RemovedActions = new List<string>();
	}
}
