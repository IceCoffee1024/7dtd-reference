using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x02001884 RID: 6276
	[Preserve]
	public class RequirementWearing : BaseRequirement
	{
		// Token: 0x0600C153 RID: 49491 RVA: 0x0047ABF4 File Offset: 0x00478DF4
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementWearing_keyword", false, null);
			this.expectedItem = ItemClass.GetItem(base.ID, false);
			this.expectedItemClass = ItemClass.GetItemClass(base.ID, false);
			base.Description = string.Format("{0} {1}", text, this.expectedItemClass.GetLocalizedItemName());
		}

		// Token: 0x0600C154 RID: 49492 RVA: 0x0047AC4E File Offset: 0x00478E4E
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerEquipment.IsWearing(this.expectedItem);
		}

		// Token: 0x0600C155 RID: 49493 RVA: 0x0047AC89 File Offset: 0x00478E89
		public override BaseRequirement Clone()
		{
			return new RequirementWearing
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x040092A5 RID: 37541
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x040092A6 RID: 37542
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;
	}
}
