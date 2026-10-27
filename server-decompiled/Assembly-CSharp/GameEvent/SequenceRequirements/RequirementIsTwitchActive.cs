using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001999 RID: 6553
	[Preserve]
	public class RequirementIsTwitchActive : BaseRequirement
	{
		// Token: 0x0600C9A2 RID: 51618 RVA: 0x0049969C File Offset: 0x0049789C
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer == null)
			{
				return false;
			}
			if (!this.Invert)
			{
				return entityPlayer.TwitchEnabled;
			}
			return !entityPlayer.TwitchEnabled;
		}

		// Token: 0x0600C9A3 RID: 51619 RVA: 0x004996CD File Offset: 0x004978CD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementIsTwitchActive
			{
				Invert = this.Invert
			};
		}
	}
}
