using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200190E RID: 6414
	[Preserve]
	public class RequirementInPOI : BaseRequirement
	{
		// Token: 0x0600C590 RID: 50576 RVA: 0x0048AFDC File Offset: 0x004891DC
		public override bool CanPerform(Entity target)
		{
			bool flag = true;
			EntityPlayer entityPlayer = target as EntityPlayer;
			if (entityPlayer != null)
			{
				if (entityPlayer.prefab != null)
				{
					Prefab prefab = entityPlayer.prefab.prefab;
					if (this.poiTags != "" && !prefab.Tags.Test_AnySet(FastTags<TagGroup.Poi>.Parse(this.poiTags)))
					{
						flag = false;
					}
					if (this.poiName != "" && !this.poiName.ContainsCaseInsensitive(prefab.PrefabName) && !this.poiName.ContainsCaseInsensitive(prefab.LocalizedName))
					{
						flag = false;
					}
					if (this.poiTier != -1 && (int)prefab.DifficultyTier != this.poiTier)
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
			}
			if (!this.Invert)
			{
				return flag;
			}
			return !flag;
		}

		// Token: 0x0600C591 RID: 50577 RVA: 0x0048B0A7 File Offset: 0x004892A7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(RequirementInPOI.PropPOITier, ref this.poiTier);
			properties.ParseString(RequirementInPOI.PropPOITags, ref this.poiTags);
			properties.ParseString(RequirementInPOI.PropPOINames, ref this.poiName);
		}

		// Token: 0x0600C592 RID: 50578 RVA: 0x0048B0E3 File Offset: 0x004892E3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInPOI
			{
				Invert = this.Invert,
				poiTier = this.poiTier,
				poiTags = this.poiTags,
				poiName = this.poiName
			};
		}

		// Token: 0x040094C8 RID: 38088
		[PublicizedFrom(EAccessModifier.Private)]
		public string poiName = "";

		// Token: 0x040094C9 RID: 38089
		[PublicizedFrom(EAccessModifier.Private)]
		public string poiTags = "";

		// Token: 0x040094CA RID: 38090
		[PublicizedFrom(EAccessModifier.Private)]
		public int poiTier = -1;

		// Token: 0x040094CB RID: 38091
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOITier = "tier";

		// Token: 0x040094CC RID: 38092
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOITags = "tags";

		// Token: 0x040094CD RID: 38093
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOINames = "name";
	}
}
