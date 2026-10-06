using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001960 RID: 6496
	[Preserve]
	public class RequirementInPOI : BaseRequirement
	{
		// Token: 0x0600C86F RID: 51311 RVA: 0x00495814 File Offset: 0x00493A14
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

		// Token: 0x0600C870 RID: 51312 RVA: 0x004958DF File Offset: 0x00493ADF
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(RequirementInPOI.PropPOITier, ref this.poiTier);
			properties.ParseString(RequirementInPOI.PropPOITags, ref this.poiTags);
			properties.ParseString(RequirementInPOI.PropPOINames, ref this.poiName);
		}

		// Token: 0x0600C871 RID: 51313 RVA: 0x0049591B File Offset: 0x00493B1B
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

		// Token: 0x040096B7 RID: 38583
		[PublicizedFrom(EAccessModifier.Private)]
		public string poiName = "";

		// Token: 0x040096B8 RID: 38584
		[PublicizedFrom(EAccessModifier.Private)]
		public string poiTags = "";

		// Token: 0x040096B9 RID: 38585
		[PublicizedFrom(EAccessModifier.Private)]
		public int poiTier = -1;

		// Token: 0x040096BA RID: 38586
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOITier = "tier";

		// Token: 0x040096BB RID: 38587
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOITags = "tags";

		// Token: 0x040096BC RID: 38588
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOINames = "name";
	}
}
