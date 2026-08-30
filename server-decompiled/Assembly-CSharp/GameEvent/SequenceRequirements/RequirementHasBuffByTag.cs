using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200192D RID: 6445
	[Preserve]
	public class RequirementHasBuffByTag : BaseRequirement
	{
		// Token: 0x0600C6F6 RID: 50934 RVA: 0x00490F64 File Offset: 0x0048F164
		public override bool CanPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null && entityAlive.Buffs.HasBuffByTag(this.buffTags))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C6F7 RID: 50935 RVA: 0x00490F9E File Offset: 0x0048F19E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(RequirementHasBuffByTag.PropBuffTags))
			{
				this.buffTags = FastTags<TagGroup.Global>.Parse(properties.Values[RequirementHasBuffByTag.PropBuffTags]);
			}
		}

		// Token: 0x0600C6F8 RID: 50936 RVA: 0x00490FD4 File Offset: 0x0048F1D4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasBuffByTag
			{
				buffTags = this.buffTags,
				Invert = this.Invert
			};
		}

		// Token: 0x040095E6 RID: 38374
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> buffTags = FastTags<TagGroup.Global>.none;

		// Token: 0x040095E7 RID: 38375
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffTags = "buff_tags";
	}
}
