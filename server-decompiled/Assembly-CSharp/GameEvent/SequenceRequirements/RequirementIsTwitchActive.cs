using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001968 RID: 6504
	[Preserve]
	public class RequirementIsTwitchActive : BaseRequirement
	{
		// Token: 0x0600C88E RID: 51342 RVA: 0x00495CA8 File Offset: 0x00493EA8
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

		// Token: 0x0600C88F RID: 51343 RVA: 0x00495CD9 File Offset: 0x00493ED9
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
