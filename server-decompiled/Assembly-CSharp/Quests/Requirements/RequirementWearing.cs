using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x02001907 RID: 6407
	[Preserve]
	public class RequirementWearing : BaseRequirement
	{
		// Token: 0x0600C546 RID: 50502 RVA: 0x00488C14 File Offset: 0x00486E14
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementWearing_keyword", false, null);
			this.expectedItem = ItemClass.GetItem(base.ID, false);
			this.expectedItemClass = ItemClass.GetItemClass(base.ID, false);
			base.Description = string.Format("{0} {1}", text, this.expectedItemClass.GetLocalizedItemName());
		}

		// Token: 0x0600C547 RID: 50503 RVA: 0x00488C6E File Offset: 0x00486E6E
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerEquipment.IsWearing(this.expectedItem);
		}

		// Token: 0x0600C548 RID: 50504 RVA: 0x00488CA9 File Offset: 0x00486EA9
		public override BaseRequirement Clone()
		{
			return new RequirementWearing
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x0400957D RID: 38269
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x0400957E RID: 38270
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;
	}
}
