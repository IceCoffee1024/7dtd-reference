using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001992 RID: 6546
	[Preserve]
	public class RequirementInQuestZone : BaseRequirement
	{
		// Token: 0x0600C988 RID: 51592 RVA: 0x0049938C File Offset: 0x0049758C
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

		// Token: 0x0600C989 RID: 51593 RVA: 0x004993DA File Offset: 0x004975DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInQuestZone();
		}
	}
}
