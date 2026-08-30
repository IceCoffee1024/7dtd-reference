using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001936 RID: 6454
	[Preserve]
	public class RequirementInSafeZone : BaseRequirement
	{
		// Token: 0x0600C71F RID: 50975 RVA: 0x00491504 File Offset: 0x0048F704
		public override bool CanPerform(Entity target)
		{
			if (!GameManager.Instance.World.CanPlaceBlockAt(new Vector3i((target == null) ? this.Owner.TargetPosition : target.position), null, false))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C720 RID: 50976 RVA: 0x00491555 File Offset: 0x0048F755
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInSafeZone();
		}
	}
}
