using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200198C RID: 6540
	[Preserve]
	public class RequirementHasHeld : BaseRequirement
	{
		// Token: 0x0600C96C RID: 51564 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C96D RID: 51565 RVA: 0x00498FA4 File Offset: 0x004971A4
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return entityPlayer != null && !entityPlayer.inventory.holdingItemStack.IsEmpty();
		}

		// Token: 0x0600C96E RID: 51566 RVA: 0x00498FD0 File Offset: 0x004971D0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
		}

		// Token: 0x0600C96F RID: 51567 RVA: 0x00498FD9 File Offset: 0x004971D9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasHeld();
		}
	}
}
