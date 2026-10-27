using System;
using System.IO;

// Token: 0x02000A5A RID: 2650
public class PlayerProfile
{
	// Token: 0x170008A7 RID: 2215
	// (get) Token: 0x06004F78 RID: 20344 RVA: 0x001E488B File Offset: 0x001E2A8B
	// (set) Token: 0x06004F79 RID: 20345 RVA: 0x001E4893 File Offset: 0x001E2A93
	public bool IsMale
	{
		get
		{
			return this.isMale;
		}
		set
		{
			this.isMale = value;
		}
	}

	// Token: 0x170008A8 RID: 2216
	// (get) Token: 0x06004F7A RID: 20346 RVA: 0x001E489C File Offset: 0x001E2A9C
	// (set) Token: 0x06004F7B RID: 20347 RVA: 0x001E48A4 File Offset: 0x001E2AA4
	public string RaceName
	{
		get
		{
			return this.raceName;
		}
		set
		{
			this.raceName = value;
		}
	}

	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x06004F7C RID: 20348 RVA: 0x001E48AD File Offset: 0x001E2AAD
	// (set) Token: 0x06004F7D RID: 20349 RVA: 0x001E48B5 File Offset: 0x001E2AB5
	public string EyeColor
	{
		get
		{
			return this.eyeColor;
		}
		set
		{
			this.eyeColor = value;
		}
	}

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x06004F7E RID: 20350 RVA: 0x001E48BE File Offset: 0x001E2ABE
	// (set) Token: 0x06004F7F RID: 20351 RVA: 0x001E48C6 File Offset: 0x001E2AC6
	public string HairName
	{
		get
		{
			return this.hairName;
		}
		set
		{
			this.hairName = value;
		}
	}

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x06004F80 RID: 20352 RVA: 0x001E48CF File Offset: 0x001E2ACF
	// (set) Token: 0x06004F81 RID: 20353 RVA: 0x001E48D7 File Offset: 0x001E2AD7
	public string HairColor
	{
		get
		{
			return this.hairColor;
		}
		set
		{
			this.hairColor = value;
		}
	}

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x06004F82 RID: 20354 RVA: 0x001E48E0 File Offset: 0x001E2AE0
	// (set) Token: 0x06004F83 RID: 20355 RVA: 0x001E48E8 File Offset: 0x001E2AE8
	public string MustacheName
	{
		get
		{
			return this.mustacheName;
		}
		set
		{
			this.mustacheName = value;
		}
	}

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x06004F84 RID: 20356 RVA: 0x001E48F1 File Offset: 0x001E2AF1
	// (set) Token: 0x06004F85 RID: 20357 RVA: 0x001E48F9 File Offset: 0x001E2AF9
	public string ChopsName
	{
		get
		{
			return this.chopsName;
		}
		set
		{
			this.chopsName = value;
		}
	}

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x06004F86 RID: 20358 RVA: 0x001E4902 File Offset: 0x001E2B02
	// (set) Token: 0x06004F87 RID: 20359 RVA: 0x001E490A File Offset: 0x001E2B0A
	public string BeardName
	{
		get
		{
			return this.beardName;
		}
		set
		{
			this.beardName = value;
		}
	}

	// Token: 0x170008AF RID: 2223
	// (get) Token: 0x06004F88 RID: 20360 RVA: 0x001E4913 File Offset: 0x001E2B13
	// (set) Token: 0x06004F89 RID: 20361 RVA: 0x001E491B File Offset: 0x001E2B1B
	public int VariantNumber
	{
		get
		{
			return this.variantNumber;
		}
		set
		{
			this.variantNumber = value;
		}
	}

	// Token: 0x170008B0 RID: 2224
	// (get) Token: 0x06004F8A RID: 20362 RVA: 0x001E4924 File Offset: 0x001E2B24
	// (set) Token: 0x06004F8B RID: 20363 RVA: 0x001E4971 File Offset: 0x001E2B71
	public string ProfileArchetype
	{
		get
		{
			if (this.archetype == null || this.archetype == string.Empty)
			{
				if (this.isMale)
				{
					this.archetype = "BaseMale";
				}
				else
				{
					this.archetype = "BaseFemale";
				}
			}
			return this.archetype;
		}
		set
		{
			this.archetype = value;
		}
	}

	// Token: 0x06004F8C RID: 20364 RVA: 0x001E497C File Offset: 0x001E2B7C
	public Archetype CreateTempArchetype()
	{
		if (this.archetype != "BaseMale" && this.archetype != "BaseFemale")
		{
			return Archetype.GetArchetype(this.archetype);
		}
		return new Archetype(this.archetype, this.isMale, true)
		{
			Race = this.raceName,
			Variant = this.variantNumber,
			Hair = this.hairName,
			HairColor = this.hairColor,
			MustacheName = this.mustacheName,
			ChopsName = this.chopsName,
			BeardName = this.beardName,
			EyeColorName = this.EyeColor
		};
	}

	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x06004F8D RID: 20365 RVA: 0x001E4A2B File Offset: 0x001E2C2B
	public string EntityClassName
	{
		get
		{
			if (!this.isMale)
			{
				return "playerFemale";
			}
			return "playerMale";
		}
	}

	// Token: 0x06004F8E RID: 20366 RVA: 0x001E4A40 File Offset: 0x001E2C40
	public PlayerProfile()
	{
		this.raceName = "white";
		this.isMale = true;
		this.variantNumber = 1;
	}

	// Token: 0x06004F8F RID: 20367 RVA: 0x001E4AD4 File Offset: 0x001E2CD4
	public PlayerProfile Clone()
	{
		return new PlayerProfile
		{
			raceName = this.raceName,
			isMale = this.isMale,
			variantNumber = this.variantNumber,
			archetype = this.archetype,
			hairName = this.hairName,
			hairColor = this.hairColor,
			mustacheName = this.mustacheName,
			chopsName = this.chopsName,
			beardName = this.beardName,
			EyeColor = this.EyeColor
		};
	}

	// Token: 0x06004F90 RID: 20368 RVA: 0x001E4B60 File Offset: 0x001E2D60
	public void Write(BinaryWriter writer)
	{
		writer.Write(5);
		writer.Write(this.archetype);
		writer.Write(this.isMale);
		writer.Write(this.raceName);
		writer.Write((byte)this.variantNumber);
		writer.Write(this.hairName ?? "");
		writer.Write(this.hairColor ?? "");
		writer.Write(this.mustacheName ?? "");
		writer.Write(this.chopsName ?? "");
		writer.Write(this.beardName ?? "");
		writer.Write(this.eyeColor ?? "Blue01");
	}

	// Token: 0x06004F91 RID: 20369 RVA: 0x001E4C24 File Offset: 0x001E2E24
	public static PlayerProfile Read(BinaryReader reader)
	{
		PlayerProfile playerProfile = new PlayerProfile();
		int num = reader.ReadInt32();
		playerProfile.archetype = reader.ReadString();
		playerProfile.IsMale = reader.ReadBoolean();
		playerProfile.RaceName = reader.ReadString();
		playerProfile.VariantNumber = (int)reader.ReadByte();
		if (num > 1)
		{
			playerProfile.HairName = reader.ReadString();
		}
		if (num > 2)
		{
			playerProfile.HairColor = reader.ReadString();
		}
		if (num > 3)
		{
			playerProfile.MustacheName = reader.ReadString();
			playerProfile.ChopsName = reader.ReadString();
			playerProfile.BeardName = reader.ReadString();
		}
		if (num > 4)
		{
			playerProfile.EyeColor = reader.ReadString();
		}
		return playerProfile;
	}

	// Token: 0x06004F92 RID: 20370 RVA: 0x001E4CC7 File Offset: 0x001E2EC7
	public static PlayerProfile LoadLocalProfile()
	{
		return PlayerProfile.LoadProfile(ProfileSDF.CurrentProfileName());
	}

	// Token: 0x06004F93 RID: 20371 RVA: 0x001E4CD4 File Offset: 0x001E2ED4
	public static PlayerProfile LoadProfile(string _profileName)
	{
		return new PlayerProfile
		{
			IsMale = ProfileSDF.GetIsMale(_profileName),
			RaceName = ProfileSDF.GetRaceName(_profileName),
			VariantNumber = ProfileSDF.GetVariantNumber(_profileName),
			ProfileArchetype = ProfileSDF.GetArchetype(_profileName),
			HairName = ProfileSDF.GetHairName(_profileName),
			HairColor = ProfileSDF.GetHairColorName(_profileName),
			MustacheName = ProfileSDF.GetMustacheName(_profileName),
			ChopsName = ProfileSDF.GetChopsName(_profileName),
			BeardName = ProfileSDF.GetBeardName(_profileName),
			EyeColor = ProfileSDF.GetEyeColorName(_profileName)
		};
	}

	// Token: 0x04003D57 RID: 15703
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMale = true;

	// Token: 0x04003D58 RID: 15704
	[PublicizedFrom(EAccessModifier.Private)]
	public string raceName = "";

	// Token: 0x04003D59 RID: 15705
	[PublicizedFrom(EAccessModifier.Private)]
	public int variantNumber = 1;

	// Token: 0x04003D5A RID: 15706
	[PublicizedFrom(EAccessModifier.Private)]
	public string archetype = "";

	// Token: 0x04003D5B RID: 15707
	[PublicizedFrom(EAccessModifier.Private)]
	public string eyeColor = "";

	// Token: 0x04003D5C RID: 15708
	[PublicizedFrom(EAccessModifier.Private)]
	public string hairName = "";

	// Token: 0x04003D5D RID: 15709
	[PublicizedFrom(EAccessModifier.Private)]
	public string hairColor = "";

	// Token: 0x04003D5E RID: 15710
	[PublicizedFrom(EAccessModifier.Private)]
	public string mustacheName = "";

	// Token: 0x04003D5F RID: 15711
	[PublicizedFrom(EAccessModifier.Private)]
	public string chopsName = "";

	// Token: 0x04003D60 RID: 15712
	[PublicizedFrom(EAccessModifier.Private)]
	public string beardName = "";

	// Token: 0x04003D61 RID: 15713
	[PublicizedFrom(EAccessModifier.Private)]
	public const int version = 5;
}
