using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019FD RID: 6653
	[Preserve]
	public class ActionRemoveDeathBuffs : ActionBaseTargetAction
	{
		// Token: 0x0600CB62 RID: 52066 RVA: 0x004A4732 File Offset: 0x004A2932
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.tags = FastTags<TagGroup.Global>.Parse(this.excludeTags);
		}

		// Token: 0x0600CB63 RID: 52067 RVA: 0x004A4748 File Offset: 0x004A2948
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.Buffs.RemoveDeathBuffs(this.tags);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CB64 RID: 52068 RVA: 0x004A4777 File Offset: 0x004A2977
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveDeathBuffs.PropExcludeTags))
			{
				this.excludeTags = properties.Values[ActionRemoveDeathBuffs.PropExcludeTags];
			}
		}

		// Token: 0x0600CB65 RID: 52069 RVA: 0x004A47A8 File Offset: 0x004A29A8
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

		// Token: 0x040099F6 RID: 39414
		[PublicizedFrom(EAccessModifier.Protected)]
		public string excludeTags = "";

		// Token: 0x040099F7 RID: 39415
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> tags;

		// Token: 0x040099F8 RID: 39416
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropExcludeTags = "exclude_tags";
	}
}
