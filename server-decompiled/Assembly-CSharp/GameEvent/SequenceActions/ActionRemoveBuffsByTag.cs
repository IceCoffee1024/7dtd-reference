using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A02 RID: 6658
	[Preserve]
	public class ActionRemoveBuffsByTag : ActionBaseTargetAction
	{
		// Token: 0x0600CB7D RID: 52093 RVA: 0x004A4B77 File Offset: 0x004A2D77
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.tags = FastTags<TagGroup.Global>.Parse(this.buffTag);
		}

		// Token: 0x0600CB7E RID: 52094 RVA: 0x004A4B8C File Offset: 0x004A2D8C
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.Buffs.RemoveBuffsByTag(this.tags);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CB7F RID: 52095 RVA: 0x004A4BBB File Offset: 0x004A2DBB
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveBuffsByTag.PropBuffTags))
			{
				this.buffTag = properties.Values[ActionRemoveBuffsByTag.PropBuffTags];
			}
		}

		// Token: 0x0600CB80 RID: 52096 RVA: 0x004A4BEC File Offset: 0x004A2DEC
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

		// Token: 0x040099FE RID: 39422
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffTag = "";

		// Token: 0x040099FF RID: 39423
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> tags;

		// Token: 0x04009A00 RID: 39424
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffTags = "buff_tag";
	}
}
