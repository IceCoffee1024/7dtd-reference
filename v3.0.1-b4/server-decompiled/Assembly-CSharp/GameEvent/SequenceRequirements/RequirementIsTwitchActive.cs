using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001916 RID: 6422
	[Preserve]
	public class RequirementIsTwitchActive : BaseRequirement
	{
		// Token: 0x0600C5AF RID: 50607 RVA: 0x0048B470 File Offset: 0x00489670
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

		// Token: 0x0600C5B0 RID: 50608 RVA: 0x0048B4A1 File Offset: 0x004896A1
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
