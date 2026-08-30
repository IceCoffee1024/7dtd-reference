using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001935 RID: 6453
	[Preserve]
	public class RequirementInQuestZone : BaseRequirement
	{
		// Token: 0x0600C71C RID: 50972 RVA: 0x004914AC File Offset: 0x0048F6AC
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

		// Token: 0x0600C71D RID: 50973 RVA: 0x004914FA File Offset: 0x0048F6FA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInQuestZone();
		}
	}
}
