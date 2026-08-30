using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A5 RID: 6565
	[Preserve]
	public class ActionRemoveBuffsByTag : ActionBaseTargetAction
	{
		// Token: 0x0600C911 RID: 51473 RVA: 0x0049CBB3 File Offset: 0x0049ADB3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.tags = FastTags<TagGroup.Global>.Parse(this.buffTag);
		}

		// Token: 0x0600C912 RID: 51474 RVA: 0x0049CBC8 File Offset: 0x0049ADC8
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.Buffs.RemoveBuffsByTag(this.tags);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C913 RID: 51475 RVA: 0x0049CBF7 File Offset: 0x0049ADF7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveBuffsByTag.PropBuffTags))
			{
				this.buffTag = properties.Values[ActionRemoveBuffsByTag.PropBuffTags];
			}
		}

		// Token: 0x0600C914 RID: 51476 RVA: 0x0049CC28 File Offset: 0x0049AE28
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

		// Token: 0x0400984F RID: 38991
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffTag = "";

		// Token: 0x04009850 RID: 38992
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> tags;

		// Token: 0x04009851 RID: 38993
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffTags = "buff_tag";
	}
}
