using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001962 RID: 6498
	[Preserve]
	public class RequirementInSafeZone : BaseRequirement
	{
		// Token: 0x0600C877 RID: 51319 RVA: 0x004959F0 File Offset: 0x00493BF0
		public override bool CanPerform(Entity target)
		{
			if (!GameManager.Instance.World.CanPlaceBlockAt(new Vector3i((target == null) ? this.Owner.TargetPosition : target.position), null, false))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C878 RID: 51320 RVA: 0x00495A41 File Offset: 0x00493C41
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInSafeZone();
		}
	}
}
