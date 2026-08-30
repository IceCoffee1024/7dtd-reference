using System;
using System.IO;

// Token: 0x02000A46 RID: 2630
public class PlayerProfile
{
	// Token: 0x17000881 RID: 2177
	// (get) Token: 0x06004F85 RID: 20357 RVA: 0x001E24F3 File Offset: 0x001E06F3
	// (set) Token: 0x06004F86 RID: 20358 RVA: 0x001E24FB File Offset: 0x001E06FB
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

	// Token: 0x17000882 RID: 2178
	// (get) Token: 0x06004F87 RID: 20359 RVA: 0x001E2504 File Offset: 0x001E0704
	// (set) Token: 0x06004F88 RID: 20360 RVA: 0x001E250C File Offset: 0x001E070C
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

	// Token: 0x17000883 RID: 2179
	// (get) Token: 0x06004F89 RID: 20361 RVA: 0x001E2515 File Offset: 0x001E0715
	// (set) Token: 0x06004F8A RID: 20362 RVA: 0x001E251D File Offset: 0x001E071D
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

	// Token: 0x17000884 RID: 2180
	// (get) Token: 0x06004F8B RID: 20363 RVA: 0x001E2526 File Offset: 0x001E0726
	// (set) Token: 0x06004F8C RID: 20364 RVA: 0x001E252E File Offset: 0x001E072E
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

	// Token: 0x17000885 RID: 2181
	// (get) Token: 0x06004F8D RID: 20365 RVA: 0x001E2537 File Offset: 0x001E0737
	// (set) Token: 0x06004F8E RID: 20366 RVA: 0x001E253F File Offset: 0x001E073F
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

	// Token: 0x17000886 RID: 2182
	// (get) Token: 0x06004F8F RID: 20367 RVA: 0x001E2548 File Offset: 0x001E0748
	// (set) Token: 0x06004F90 RID: 20368 RVA: 0x001E2550 File Offset: 0x001E0750
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

	// Token: 0x17000887 RID: 2183
	// (get) Token: 0x06004F91 RID: 20369 RVA: 0x001E2559 File Offset: 0x001E0759
	// (set) Token: 0x06004F92 RID: 20370 RVA: 0x001E2561 File Offset: 0x001E0761
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

	// Token: 0x17000888 RID: 2184
	// (get) Token: 0x06004F93 RID: 20371 RVA: 0x001E256A File Offset: 0x001E076A
	// (set) Token: 0x06004F94 RID: 20372 RVA: 0x001E2572 File Offset: 0x001E0772
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

	// Token: 0x17000889 RID: 2185
	// (get) Token: 0x06004F95 RID: 20373 RVA: 0x001E257B File Offset: 0x001E077B
	// (set) Token: 0x06004F96 RID: 20374 RVA: 0x001E2583 File Offset: 0x001E0783
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

	// Token: 0x1700088A RID: 2186
	// (get) Token: 0x06004F97 RID: 20375 RVA: 0x001E258C File Offset: 0x001E078C
	// (set) Token: 0x06004F98 RID: 20376 RVA: 0x001E25D9 File Offset: 0x001E07D9
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

	// Token: 0x06004F99 RID: 20377 RVA: 0x001E25E4 File Offset: 0x001E07E4
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

	// Token: 0x1700088B RID: 2187
	// (get) Token: 0x06004F9A RID: 20378 RVA: 0x001E2693 File Offset: 0x001E0893
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

	// Token: 0x06004F9B RID: 20379 RVA: 0x001E26A8 File Offset: 0x001E08A8
	public PlayerProfile()
	{
		this.raceName = "white";
		this.isMale = true;
		this.variantNumber = 1;
	}

	// Token: 0x06004F9C RID: 20380 RVA: 0x001E273C File Offset: 0x001E093C
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

	// Token: 0x06004F9D RID: 20381 RVA: 0x001E27C8 File Offset: 0x001E09C8
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

	// Token: 0x06004F9E RID: 20382 RVA: 0x001E288C File Offset: 0x001E0A8C
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

	// Token: 0x06004F9F RID: 20383 RVA: 0x001E292F File Offset: 0x001E0B2F
	public static PlayerProfile LoadLocalProfile()
	{
		return PlayerProfile.LoadProfile(ProfileSDF.CurrentProfileName());
	}

	// Token: 0x06004FA0 RID: 20384 RVA: 0x001E293C File Offset: 0x001E0B3C
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

	// Token: 0x04003D29 RID: 15657
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMale = true;

	// Token: 0x04003D2A RID: 15658
	[PublicizedFrom(EAccessModifier.Private)]
	public string raceName = "";

	// Token: 0x04003D2B RID: 15659
	[PublicizedFrom(EAccessModifier.Private)]
	public int variantNumber = 1;

	// Token: 0x04003D2C RID: 15660
	[PublicizedFrom(EAccessModifier.Private)]
	public string archetype = "";

	// Token: 0x04003D2D RID: 15661
	[PublicizedFrom(EAccessModifier.Private)]
	public string eyeColor = "";

	// Token: 0x04003D2E RID: 15662
	[PublicizedFrom(EAccessModifier.Private)]
	public string hairName = "";

	// Token: 0x04003D2F RID: 15663
	[PublicizedFrom(EAccessModifier.Private)]
	public string hairColor = "";

	// Token: 0x04003D30 RID: 15664
	[PublicizedFrom(EAccessModifier.Private)]
	public string mustacheName = "";

	// Token: 0x04003D31 RID: 15665
	[PublicizedFrom(EAccessModifier.Private)]
	public string chopsName = "";

	// Token: 0x04003D32 RID: 15666
	[PublicizedFrom(EAccessModifier.Private)]
	public string beardName = "";

	// Token: 0x04003D33 RID: 15667
	[PublicizedFrom(EAccessModifier.Private)]
	public const int version = 5;
}
