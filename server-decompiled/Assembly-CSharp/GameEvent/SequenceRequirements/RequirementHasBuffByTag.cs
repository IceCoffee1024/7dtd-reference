using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200198A RID: 6538
	[Preserve]
	public class RequirementHasBuffByTag : BaseRequirement
	{
		// Token: 0x0600C962 RID: 51554 RVA: 0x00498E44 File Offset: 0x00497044
		public override bool CanPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null && entityAlive.Buffs.HasBuffByTag(this.buffTags))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C963 RID: 51555 RVA: 0x00498E7E File Offset: 0x0049707E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(RequirementHasBuffByTag.PropBuffTags))
			{
				this.buffTags = FastTags<TagGroup.Global>.Parse(properties.Values[RequirementHasBuffByTag.PropBuffTags]);
			}
		}

		// Token: 0x0600C964 RID: 51556 RVA: 0x00498EB4 File Offset: 0x004970B4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasBuffByTag
			{
				buffTags = this.buffTags,
				Invert = this.Invert
			};
		}

		// Token: 0x04009795 RID: 38805
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> buffTags = FastTags<TagGroup.Global>.none;

		// Token: 0x04009796 RID: 38806
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffTags = "buff_tags";
	}
}
