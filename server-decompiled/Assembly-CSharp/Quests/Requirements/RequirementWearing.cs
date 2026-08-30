using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x020018AA RID: 6314
	[Preserve]
	public class RequirementWearing : BaseRequirement
	{
		// Token: 0x0600C2DA RID: 49882 RVA: 0x00480D20 File Offset: 0x0047EF20
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementWearing_keyword", false, null);
			this.expectedItem = ItemClass.GetItem(base.ID, false);
			this.expectedItemClass = ItemClass.GetItemClass(base.ID, false);
			base.Description = string.Format("{0} {1}", text, this.expectedItemClass.GetLocalizedItemName());
		}

		// Token: 0x0600C2DB RID: 49883 RVA: 0x00480D7A File Offset: 0x0047EF7A
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerEquipment.IsWearing(this.expectedItem);
		}

		// Token: 0x0600C2DC RID: 49884 RVA: 0x00480DB5 File Offset: 0x0047EFB5
		public override BaseRequirement Clone()
		{
			return new RequirementWearing
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x040093CE RID: 37838
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x040093CF RID: 37839
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;
	}
}
