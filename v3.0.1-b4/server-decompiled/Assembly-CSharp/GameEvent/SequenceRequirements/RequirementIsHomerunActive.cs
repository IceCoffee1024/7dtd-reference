using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001914 RID: 6420
	[Preserve]
	public class RequirementIsHomerunActive : BaseRequirement
	{
		// Token: 0x0600C5A9 RID: 50601 RVA: 0x0048B3C8 File Offset: 0x004895C8
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

		// Token: 0x0600C5AA RID: 50602 RVA: 0x0048B400 File Offset: 0x00489600
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
