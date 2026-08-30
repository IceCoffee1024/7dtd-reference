using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A0 RID: 6560
	[Preserve]
	public class ActionRemoveDeathBuffs : ActionBaseTargetAction
	{
		// Token: 0x0600C8F6 RID: 51446 RVA: 0x0049C76E File Offset: 0x0049A96E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.tags = FastTags<TagGroup.Global>.Parse(this.excludeTags);
		}

		// Token: 0x0600C8F7 RID: 51447 RVA: 0x0049C784 File Offset: 0x0049A984
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.Buffs.RemoveDeathBuffs(this.tags);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C8F8 RID: 51448 RVA: 0x0049C7B3 File Offset: 0x0049A9B3
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveDeathBuffs.PropExcludeTags))
			{
				this.excludeTags = properties.Values[ActionRemoveDeathBuffs.PropExcludeTags];
			}
		}

		// Token: 0x0600C8F9 RID: 51449 RVA: 0x0049C7E4 File Offset: 0x0049A9E4
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

		// Token: 0x04009847 RID: 38983
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeTags = "";

		// Token: 0x04009848 RID: 38984
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> tags;

		// Token: 0x04009849 RID: 38985
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTags = "exclude_tags";
	}
}
