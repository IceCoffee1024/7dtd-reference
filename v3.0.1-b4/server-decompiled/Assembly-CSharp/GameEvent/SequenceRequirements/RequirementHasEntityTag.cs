using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001908 RID: 6408
	[Preserve]
	public class RequirementHasEntityTag : BaseRequirement
	{
		// Token: 0x0600C574 RID: 50548 RVA: 0x0048ACC8 File Offset: 0x00488EC8
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

		// Token: 0x0600C575 RID: 50549 RVA: 0x0048AD1D File Offset: 0x00488F1D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementHasEntityTag.PropTag, ref this.tag);
		}

		// Token: 0x0600C576 RID: 50550 RVA: 0x0048AD37 File Offset: 0x00488F37
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasEntityTag
			{
				tag = this.tag,
				Invert = this.Invert
			};
		}

		// Token: 0x040094BF RID: 38079
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag = "";

		// Token: 0x040094C0 RID: 38080
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";
	}
}
