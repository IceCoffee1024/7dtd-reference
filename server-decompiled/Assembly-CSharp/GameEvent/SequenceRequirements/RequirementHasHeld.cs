using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200195B RID: 6491
	[Preserve]
	public class RequirementHasHeld : BaseRequirement
	{
		// Token: 0x0600C858 RID: 51288 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C859 RID: 51289 RVA: 0x004955B0 File Offset: 0x004937B0
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return entityPlayer != null && !entityPlayer.inventory.holdingItemStack.IsEmpty();
		}

		// Token: 0x0600C85A RID: 51290 RVA: 0x004955DC File Offset: 0x004937DC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
		}

		// Token: 0x0600C85B RID: 51291 RVA: 0x004955E5 File Offset: 0x004937E5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasHeld();
		}
	}
}
