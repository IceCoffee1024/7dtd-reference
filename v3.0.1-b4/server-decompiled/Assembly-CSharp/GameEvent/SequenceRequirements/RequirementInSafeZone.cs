using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001910 RID: 6416
	[Preserve]
	public class RequirementInSafeZone : BaseRequirement
	{
		// Token: 0x0600C598 RID: 50584 RVA: 0x0048B1B8 File Offset: 0x004893B8
		public override bool CanPerform(Entity target)
		{
			if (!GameManager.Instance.World.CanPlaceBlockAt(new Vector3i((target == null) ? this.Owner.TargetPosition : target.position), null, false))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C599 RID: 50585 RVA: 0x0048B209 File Offset: 0x00489409
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInSafeZone();
		}
	}
}
