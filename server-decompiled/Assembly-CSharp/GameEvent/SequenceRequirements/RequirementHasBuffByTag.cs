using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001959 RID: 6489
	[Preserve]
	public class RequirementHasBuffByTag : BaseRequirement
	{
		// Token: 0x0600C84E RID: 51278 RVA: 0x00495450 File Offset: 0x00493650
		public override bool CanPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null && entityAlive.Buffs.HasBuffByTag(this.buffTags))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C84F RID: 51279 RVA: 0x0049548A File Offset: 0x0049368A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(RequirementHasBuffByTag.PropBuffTags))
			{
				this.buffTags = FastTags<TagGroup.Global>.Parse(properties.Values[RequirementHasBuffByTag.PropBuffTags]);
			}
		}

		// Token: 0x0600C850 RID: 51280 RVA: 0x004954C0 File Offset: 0x004936C0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasBuffByTag
			{
				buffTags = this.buffTags,
				Invert = this.Invert
			};
		}

		// Token: 0x040096AC RID: 38572
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> buffTags = FastTags<TagGroup.Global>.none;

		// Token: 0x040096AD RID: 38573
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffTags = "buff_tags";
	}
}
