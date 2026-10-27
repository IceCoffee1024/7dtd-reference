using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001991 RID: 6545
	[Preserve]
	public class RequirementInPOI : BaseRequirement
	{
		// Token: 0x0600C983 RID: 51587 RVA: 0x00499208 File Offset: 0x00497408
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

		// Token: 0x0600C984 RID: 51588 RVA: 0x004992D3 File Offset: 0x004974D3
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(RequirementInPOI.PropPOITier, ref this.poiTier);
			properties.ParseString(RequirementInPOI.PropPOITags, ref this.poiTags);
			properties.ParseString(RequirementInPOI.PropPOINames, ref this.poiName);
		}

		// Token: 0x0600C985 RID: 51589 RVA: 0x0049930F File Offset: 0x0049750F
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

		// Token: 0x040097A0 RID: 38816
		[PublicizedFrom(EAccessModifier.Private)]
		public string poiName = "";

		// Token: 0x040097A1 RID: 38817
		[PublicizedFrom(EAccessModifier.Private)]
		public string poiTags = "";

		// Token: 0x040097A2 RID: 38818
		[PublicizedFrom(EAccessModifier.Private)]
		public int poiTier = -1;

		// Token: 0x040097A3 RID: 38819
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOITier = "tier";

		// Token: 0x040097A4 RID: 38820
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOITags = "tags";

		// Token: 0x040097A5 RID: 38821
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropPOINames = "name";
	}
}
