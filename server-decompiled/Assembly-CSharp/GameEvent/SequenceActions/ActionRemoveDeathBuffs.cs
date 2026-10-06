using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CC RID: 6604
	[Preserve]
	public class ActionRemoveDeathBuffs : ActionBaseTargetAction
	{
		// Token: 0x0600CA4E RID: 51790 RVA: 0x004A0C32 File Offset: 0x0049EE32
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.tags = FastTags<TagGroup.Global>.Parse(this.excludeTags);
		}

		// Token: 0x0600CA4F RID: 51791 RVA: 0x004A0C48 File Offset: 0x0049EE48
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.Buffs.RemoveDeathBuffs(this.tags);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CA50 RID: 51792 RVA: 0x004A0C77 File Offset: 0x0049EE77
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveDeathBuffs.PropExcludeTags))
			{
				this.excludeTags = properties.Values[ActionRemoveDeathBuffs.PropExcludeTags];
			}
		}

		// Token: 0x0600CA51 RID: 51793 RVA: 0x004A0CA8 File Offset: 0x0049EEA8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveDeathBuffs
			{
				excludeTags = this.excludeTags,
				tags = this.tags,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x0400990D RID: 39181
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeTags = "";

		// Token: 0x0400990E RID: 39182
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> tags;

		// Token: 0x0400990F RID: 39183
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTags = "exclude_tags";
	}
}
