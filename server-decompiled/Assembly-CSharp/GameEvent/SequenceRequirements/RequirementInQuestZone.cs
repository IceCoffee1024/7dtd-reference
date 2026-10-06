using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001961 RID: 6497
	[Preserve]
	public class RequirementInQuestZone : BaseRequirement
	{
		// Token: 0x0600C874 RID: 51316 RVA: 0x00495998 File Offset: 0x00493B98
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

		// Token: 0x0600C875 RID: 51317 RVA: 0x004959E6 File Offset: 0x00493BE6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInQuestZone();
		}
	}
}
