using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D1 RID: 6609
	[Preserve]
	public class ActionRemoveBuffsByTag : ActionBaseTargetAction
	{
		// Token: 0x0600CA69 RID: 51817 RVA: 0x004A1077 File Offset: 0x0049F277
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.tags = FastTags<TagGroup.Global>.Parse(this.buffTag);
		}

		// Token: 0x0600CA6A RID: 51818 RVA: 0x004A108C File Offset: 0x0049F28C
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.Buffs.RemoveBuffsByTag(this.tags);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CA6B RID: 51819 RVA: 0x004A10BB File Offset: 0x0049F2BB
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveBuffsByTag.PropBuffTags))
			{
				this.buffTag = properties.Values[ActionRemoveBuffsByTag.PropBuffTags];
			}
		}

		// Token: 0x0600CA6C RID: 51820 RVA: 0x004A10EC File Offset: 0x0049F2EC
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

		// Token: 0x04009915 RID: 39189
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffTag = "";

		// Token: 0x04009916 RID: 39190
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> tags;

		// Token: 0x04009917 RID: 39191
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffTags = "buff_tag";
	}
}
