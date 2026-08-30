using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001934 RID: 6452
	[Preserve]
	public class RequirementInPOI : BaseRequirement
	{
		// Token: 0x0600C717 RID: 50967 RVA: 0x00491328 File Offset: 0x0048F528
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

		// Token: 0x0600C718 RID: 50968 RVA: 0x004913F3 File Offset: 0x0048F5F3
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(RequirementInPOI.PropPOITier, ref this.poiTier);
			properties.ParseString(RequirementInPOI.PropPOITags, ref this.poiTags);
			properties.ParseString(RequirementInPOI.PropPOINames, ref this.poiName);
		}

		// Token: 0x0600C719 RID: 50969 RVA: 0x0049142F File Offset: 0x0048F62F
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

		// Token: 0x040095F1 RID: 38385
		[PublicizedFrom(EAccessModifier.Private)]
		public string poiName = "";

		// Token: 0x040095F2 RID: 38386
		[PublicizedFrom(EAccessModifier.Private)]
		public string poiTags = "";

		// Token: 0x040095F3 RID: 38387
		[PublicizedFrom(EAccessModifier.Private)]
		public int poiTier = -1;

		// Token: 0x040095F4 RID: 38388
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOITier = "tier";

		// Token: 0x040095F5 RID: 38389
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOITags = "tags";

		// Token: 0x040095F6 RID: 38390
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOINames = "name";
	}
}
