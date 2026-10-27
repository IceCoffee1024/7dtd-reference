using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001993 RID: 6547
	[Preserve]
	public class RequirementInSafeZone : BaseRequirement
	{
		// Token: 0x0600C98B RID: 51595 RVA: 0x004993E4 File Offset: 0x004975E4
		public override bool CanPerform(Entity target)
		{
			if (!GameManager.Instance.World.CanPlaceBlockAt(new Vector3i((target == null) ? this.Owner.TargetPosition : target.position), null, false))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C98C RID: 51596 RVA: 0x00499435 File Offset: 0x00497635
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInSafeZone();
		}
	}
}
