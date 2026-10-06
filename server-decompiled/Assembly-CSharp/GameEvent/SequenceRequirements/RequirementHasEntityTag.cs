using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200195A RID: 6490
	[Preserve]
	public class RequirementHasEntityTag : BaseRequirement
	{
		// Token: 0x0600C853 RID: 51283 RVA: 0x00495500 File Offset: 0x00493700
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

		// Token: 0x0600C854 RID: 51284 RVA: 0x00495555 File Offset: 0x00493755
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementHasEntityTag.PropTag, ref this.tag);
		}

		// Token: 0x0600C855 RID: 51285 RVA: 0x0049556F File Offset: 0x0049376F
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementHasEntityTag
			{
				tag = this.tag,
				Invert = this.Invert
			};
		}

		// Token: 0x040096AE RID: 38574
		[PublicizedFrom(EAccessModifier.Protected)]
		public string tag = "";

		// Token: 0x040096AF RID: 38575
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTag = "entity_tags";
	}
}
