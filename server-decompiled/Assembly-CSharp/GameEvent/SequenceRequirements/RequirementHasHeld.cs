using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200192F RID: 6447
	[Preserve]
	public class RequirementHasHeld : BaseRequirement
	{
		// Token: 0x0600C700 RID: 50944 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C701 RID: 50945 RVA: 0x004910C4 File Offset: 0x0048F2C4
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return entityPlayer != null && !entityPlayer.inventory.holdingItemStack.IsEmpty();
		}

		// Token: 0x0600C702 RID: 50946 RVA: 0x004910F0 File Offset: 0x0048F2F0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
		}

		// Token: 0x0600C703 RID: 50947 RVA: 0x004910F9 File Offset: 0x0048F2F9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasHeld();
		}
	}
}
