using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200197F RID: 6527
	[Preserve]
	public class ActionRemoveBuffsByTag : ActionBaseTargetAction
	{
		// Token: 0x0600C78A RID: 51082 RVA: 0x0049686F File Offset: 0x00494A6F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.tags = FastTags<TagGroup.Global>.Parse(this.buffTag);
		}

		// Token: 0x0600C78B RID: 51083 RVA: 0x00496884 File Offset: 0x00494A84
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.Buffs.RemoveBuffsByTag(this.tags);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C78C RID: 51084 RVA: 0x004968B3 File Offset: 0x00494AB3
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveBuffsByTag.PropBuffTags))
			{
				this.buffTag = properties.Values[ActionRemoveBuffsByTag.PropBuffTags];
			}
		}

		// Token: 0x0600C78D RID: 51085 RVA: 0x004968E4 File Offset: 0x00494AE4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveBuffsByTag
			{
				buffTag = this.buffTag,
				tags = this.tags,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x04009726 RID: 38694
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffTag = "";

		// Token: 0x04009727 RID: 38695
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> tags;

		// Token: 0x04009728 RID: 38696
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffTags = "buff_tag";
	}
}
