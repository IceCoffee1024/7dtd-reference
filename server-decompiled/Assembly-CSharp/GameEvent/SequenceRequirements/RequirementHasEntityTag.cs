using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200192E RID: 6446
	[Preserve]
	public class RequirementHasEntityTag : BaseRequirement
	{
		// Token: 0x0600C6FB RID: 50939 RVA: 0x00491014 File Offset: 0x0048F214
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

		// Token: 0x0600C6FC RID: 50940 RVA: 0x00491069 File Offset: 0x0048F269
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementHasEntityTag.PropTag, ref this.tag);
		}

		// Token: 0x0600C6FD RID: 50941 RVA: 0x00491083 File Offset: 0x0048F283
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasEntityTag
			{
				tag = this.tag,
				Invert = this.Invert
			};
		}

		// Token: 0x040095E8 RID: 38376
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag = "";

		// Token: 0x040095E9 RID: 38377
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";
	}
}
