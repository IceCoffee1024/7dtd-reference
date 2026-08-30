using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200193A RID: 6458
	[Preserve]
	public class RequirementIsHomerunActive : BaseRequirement
	{
		// Token: 0x0600C730 RID: 50992 RVA: 0x00491714 File Offset: 0x0048F914
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

		// Token: 0x0600C731 RID: 50993 RVA: 0x0049174C File Offset: 0x0048F94C
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
