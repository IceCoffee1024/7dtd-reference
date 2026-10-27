using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200198B RID: 6539
	[Preserve]
	public class RequirementHasEntityTag : BaseRequirement
	{
		// Token: 0x0600C967 RID: 51559 RVA: 0x00498EF4 File Offset: 0x004970F4
		public override bool CanPerform(Entity target)
		{
			FastTags<TagGroup.Global> fastTags = ((this.tag == "") ? FastTags<TagGroup.Global>.none : FastTags<TagGroup.Global>.Parse(this.tag));
			if (!(target is EntityAlive))
			{
				return false;
			}
			if (target.HasAnyTags(fastTags))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C968 RID: 51560 RVA: 0x00498F49 File Offset: 0x00497149
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementHasEntityTag.PropTag, ref this.tag);
		}

		// Token: 0x0600C969 RID: 51561 RVA: 0x00498F63 File Offset: 0x00497163
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasEntityTag
			{
				tag = this.tag,
				Invert = this.Invert
			};
		}

		// Token: 0x04009797 RID: 38807
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag = "";

		// Token: 0x04009798 RID: 38808
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";
	}
}
