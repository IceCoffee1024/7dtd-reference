using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200193C RID: 6460
	[Preserve]
	public class RequirementIsTwitchActive : BaseRequirement
	{
		// Token: 0x0600C736 RID: 50998 RVA: 0x004917BC File Offset: 0x0048F9BC
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

		// Token: 0x0600C737 RID: 50999 RVA: 0x004917ED File Offset: 0x0048F9ED
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
