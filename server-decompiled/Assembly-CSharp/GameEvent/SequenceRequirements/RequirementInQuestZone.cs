using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200190F RID: 6415
	[Preserve]
	public class RequirementInQuestZone : BaseRequirement
	{
		// Token: 0x0600C595 RID: 50581 RVA: 0x0048B160 File Offset: 0x00489360
		public override bool CanPerform(Entity target)
		{
			World world = GameManager.Instance.World;
			Vector3 position = target.position;
			position.y = position.z;
			if (QuestEventManager.Current.QuestBounds.Contains(position))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C596 RID: 50582 RVA: 0x0048B1AE File Offset: 0x004893AE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInQuestZone();
		}
	}
}
