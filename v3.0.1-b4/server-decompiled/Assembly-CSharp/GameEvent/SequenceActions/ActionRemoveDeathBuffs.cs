using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200197A RID: 6522
	[Preserve]
	public class ActionRemoveDeathBuffs : ActionBaseTargetAction
	{
		// Token: 0x0600C76F RID: 51055 RVA: 0x0049642A File Offset: 0x0049462A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.tags = FastTags<TagGroup.Global>.Parse(this.excludeTags);
		}

		// Token: 0x0600C770 RID: 51056 RVA: 0x00496440 File Offset: 0x00494640
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.Buffs.RemoveDeathBuffs(this.tags);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C771 RID: 51057 RVA: 0x0049646F File Offset: 0x0049466F
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveDeathBuffs.PropExcludeTags))
			{
				this.excludeTags = properties.Values[ActionRemoveDeathBuffs.PropExcludeTags];
			}
		}

		// Token: 0x0600C772 RID: 51058 RVA: 0x004964A0 File Offset: 0x004946A0
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

		// Token: 0x0400971E RID: 38686
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeTags = "";

		// Token: 0x0400971F RID: 38687
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> tags;

		// Token: 0x04009720 RID: 38688
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTags = "exclude_tags";
	}
}
