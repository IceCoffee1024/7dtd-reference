using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001966 RID: 6502
	[Preserve]
	public class RequirementIsHomerunActive : BaseRequirement
	{
		// Token: 0x0600C888 RID: 51336 RVA: 0x00495C00 File Offset: 0x00493E00
		public override bool CanPerform(Entity target)
		{
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer == null)
			{
				return false;
			}
			bool flag = GameEventManager.Current.HomerunManager.HasHomerunActive(entityPlayer);
			if (!this.Invert)
			{
				return flag;
			}
			return !flag;
		}

		// Token: 0x0600C889 RID: 51337 RVA: 0x00495C38 File Offset: 0x00493E38
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementIsHomerunActive
			{
				Invert = this.Invert
			};
		}
	}
}
