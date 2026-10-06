using System;
using System.Collections.Generic;

namespace Twitch
{
	// Token: 0x0200182A RID: 6186
	public class TwitchActionPreset
	{
		// Token: 0x0600BF7C RID: 49020 RVA: 0x00469D66 File Offset: 0x00467F66
		public void AddCooldownModifier(TwitchActionCooldownModifier modifier)
		{
			if (this.ActionCooldownModifiers == null)
			{
				this.ActionCooldownModifiers = new List<TwitchActionCooldownModifier>();
			}
			this.ActionCooldownModifiers.Add(modifier);
		}

		// Token: 0x0600BF7D RID: 49021 RVA: 0x00469D88 File Offset: 0x00467F88
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

		// Token: 0x04008FD4 RID: 36820
		public string Name;

		// Token: 0x04008FD5 RID: 36821
		public bool IsEnabled = true;

		// Token: 0x04008FD6 RID: 36822
		public bool IsDefault;

		// Token: 0x04008FD7 RID: 36823
		public bool IsEmpty;

		// Token: 0x04008FD8 RID: 36824
		public string Title;

		// Token: 0x04008FD9 RID: 36825
		public string Description;

		// Token: 0x04008FDA RID: 36826
		public bool AllowPointGeneration = true;

		// Token: 0x04008FDB RID: 36827
		public bool UseHelperReward = true;

		// Token: 0x04008FDC RID: 36828
		public bool ShowNewCommands = true;

		// Token: 0x04008FDD RID: 36829
		public List<TwitchActionCooldownModifier> ActionCooldownModifiers;

		// Token: 0x04008FDE RID: 36830
		public List<string> AddedActions = new List<string>();

		// Token: 0x04008FDF RID: 36831
		public List<string> RemovedActions = new List<string>();
	}
}
