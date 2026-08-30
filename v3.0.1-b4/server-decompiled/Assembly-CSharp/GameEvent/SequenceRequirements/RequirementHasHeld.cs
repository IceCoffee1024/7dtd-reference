using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001909 RID: 6409
	[Preserve]
	public class RequirementHasHeld : BaseRequirement
	{
		// Token: 0x0600C579 RID: 50553 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C57A RID: 50554 RVA: 0x0048AD78 File Offset: 0x00488F78
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			return entityPlayer != null && !entityPlayer.inventory.holdingItemStack.IsEmpty();
		}

		// Token: 0x0600C57B RID: 50555 RVA: 0x0048ADA4 File Offset: 0x00488FA4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
		}

		// Token: 0x0600C57C RID: 50556 RVA: 0x0048ADAD File Offset: 0x00488FAD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasHeld();
		}
	}
}
