using System;
using UnityEngine.Scripting;

namespace Quests.Requirements
{
	// Token: 0x020018D6 RID: 6358
	[Preserve]
	public class RequirementWearing : BaseRequirement
	{
		// Token: 0x0600C432 RID: 50226 RVA: 0x00485268 File Offset: 0x00483468
		public override void SetupRequirement()
		{
			string text = Localization.Get("RequirementWearing_keyword", false, null);
			this.expectedItem = ItemClass.GetItem(base.ID, false);
			this.expectedItemClass = ItemClass.GetItemClass(base.ID, false);
			base.Description = string.Format("{0} {1}", text, this.expectedItemClass.GetLocalizedItemName());
		}

		// Token: 0x0600C433 RID: 50227 RVA: 0x004852C2 File Offset: 0x004834C2
		public override bool CheckRequirement()
		{
			return !base.OwnerQuest.Active || LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerEquipment.IsWearing(this.expectedItem);
		}

		// Token: 0x0600C434 RID: 50228 RVA: 0x004852FD File Offset: 0x004834FD
		public override BaseRequirement Clone()
		{
			return new RequirementWearing
			{
				ID = base.ID,
				Value = base.Value,
				Phase = base.Phase
			};
		}

		// Token: 0x04009494 RID: 38036
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x04009495 RID: 38037
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;
	}
}
