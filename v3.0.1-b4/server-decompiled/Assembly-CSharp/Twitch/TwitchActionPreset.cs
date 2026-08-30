using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x020017D8 RID: 6104
	public class TwitchActionPreset
	{
		// Token: 0x0600BC9F RID: 48287 RVA: 0x0045FA8E File Offset: 0x0045DC8E
		public void AddCooldownModifier(TwitchActionCooldownModifier modifier)
		{
			if (this.ActionCooldownModifiers == null)
			{
				this.ActionCooldownModifiers = new List<TwitchActionCooldownModifier>();
			}
			this.ActionCooldownModifiers.Add(modifier);
		}

		// Token: 0x0600BCA0 RID: 48288 RVA: 0x0045FAB0 File Offset: 0x0045DCB0
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

		// Token: 0x04008DF2 RID: 36338
		public string Name;

		// Token: 0x04008DF3 RID: 36339
		public bool IsEnabled = true;

		// Token: 0x04008DF4 RID: 36340
		public bool IsDefault;

		// Token: 0x04008DF5 RID: 36341
		public bool IsEmpty;

		// Token: 0x04008DF6 RID: 36342
		public string Title;

		// Token: 0x04008DF7 RID: 36343
		public string Description;

		// Token: 0x04008DF8 RID: 36344
		public bool AllowPointGeneration = true;

		// Token: 0x04008DF9 RID: 36345
		public bool UseHelperReward = true;

		// Token: 0x04008DFA RID: 36346
		public bool ShowNewCommands = true;

		// Token: 0x04008DFB RID: 36347
		public List<TwitchActionCooldownModifier> ActionCooldownModifiers;

		// Token: 0x04008DFC RID: 36348
		public List<string> AddedActions = new List<string>();

		// Token: 0x04008DFD RID: 36349
		public List<string> RemovedActions = new List<string>();
	}
}
