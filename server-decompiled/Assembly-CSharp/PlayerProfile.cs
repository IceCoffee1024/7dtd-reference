using System;
using System.IO;

// Token: 0x02000A50 RID: 2640
public class PlayerProfile
{
	// Token: 0x170008A1 RID: 2209
	// (get) Token: 0x06004F54 RID: 20308 RVA: 0x001E37A3 File Offset: 0x001E19A3
	// (set) Token: 0x06004F55 RID: 20309 RVA: 0x001E37AB File Offset: 0x001E19AB
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

	// Token: 0x170008A2 RID: 2210
	// (get) Token: 0x06004F56 RID: 20310 RVA: 0x001E37B4 File Offset: 0x001E19B4
	// (set) Token: 0x06004F57 RID: 20311 RVA: 0x001E37BC File Offset: 0x001E19BC
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

	// Token: 0x170008A3 RID: 2211
	// (get) Token: 0x06004F58 RID: 20312 RVA: 0x001E37C5 File Offset: 0x001E19C5
	// (set) Token: 0x06004F59 RID: 20313 RVA: 0x001E37CD File Offset: 0x001E19CD
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

	// Token: 0x170008A4 RID: 2212
	// (get) Token: 0x06004F5A RID: 20314 RVA: 0x001E37D6 File Offset: 0x001E19D6
	// (set) Token: 0x06004F5B RID: 20315 RVA: 0x001E37DE File Offset: 0x001E19DE
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

	// Token: 0x170008A5 RID: 2213
	// (get) Token: 0x06004F5C RID: 20316 RVA: 0x001E37E7 File Offset: 0x001E19E7
	// (set) Token: 0x06004F5D RID: 20317 RVA: 0x001E37EF File Offset: 0x001E19EF
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

	// Token: 0x170008A6 RID: 2214
	// (get) Token: 0x06004F5E RID: 20318 RVA: 0x001E37F8 File Offset: 0x001E19F8
	// (set) Token: 0x06004F5F RID: 20319 RVA: 0x001E3800 File Offset: 0x001E1A00
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

	// Token: 0x170008A7 RID: 2215
	// (get) Token: 0x06004F60 RID: 20320 RVA: 0x001E3809 File Offset: 0x001E1A09
	// (set) Token: 0x06004F61 RID: 20321 RVA: 0x001E3811 File Offset: 0x001E1A11
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

	// Token: 0x170008A8 RID: 2216
	// (get) Token: 0x06004F62 RID: 20322 RVA: 0x001E381A File Offset: 0x001E1A1A
	// (set) Token: 0x06004F63 RID: 20323 RVA: 0x001E3822 File Offset: 0x001E1A22
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

	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x06004F64 RID: 20324 RVA: 0x001E382B File Offset: 0x001E1A2B
	// (set) Token: 0x06004F65 RID: 20325 RVA: 0x001E3833 File Offset: 0x001E1A33
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

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x06004F66 RID: 20326 RVA: 0x001E383C File Offset: 0x001E1A3C
	// (set) Token: 0x06004F67 RID: 20327 RVA: 0x001E3889 File Offset: 0x001E1A89
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

	// Token: 0x06004F68 RID: 20328 RVA: 0x001E3894 File Offset: 0x001E1A94
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

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x06004F69 RID: 20329 RVA: 0x001E3943 File Offset: 0x001E1B43
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

	// Token: 0x06004F6A RID: 20330 RVA: 0x001E3958 File Offset: 0x001E1B58
	public PlayerProfile()
	{
		this.raceName = "white";
		this.isMale = true;
		this.variantNumber = 1;
	}

	// Token: 0x06004F6B RID: 20331 RVA: 0x001E39EC File Offset: 0x001E1BEC
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

	// Token: 0x06004F6C RID: 20332 RVA: 0x001E3A78 File Offset: 0x001E1C78
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

	// Token: 0x06004F6D RID: 20333 RVA: 0x001E3B3C File Offset: 0x001E1D3C
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

	// Token: 0x06004F6E RID: 20334 RVA: 0x001E3BDF File Offset: 0x001E1DDF
	public static PlayerProfile LoadLocalProfile()
	{
		return PlayerProfile.LoadProfile(ProfileSDF.CurrentProfileName());
	}

	// Token: 0x06004F6F RID: 20335 RVA: 0x001E3BEC File Offset: 0x001E1DEC
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

	// Token: 0x04003D36 RID: 15670
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMale = true;

	// Token: 0x04003D37 RID: 15671
	[PublicizedFrom(EAccessModifier.Private)]
	public string raceName = "";

	// Token: 0x04003D38 RID: 15672
	[PublicizedFrom(EAccessModifier.Private)]
	public int variantNumber = 1;

	// Token: 0x04003D39 RID: 15673
	[PublicizedFrom(EAccessModifier.Private)]
	public string archetype = "";

	// Token: 0x04003D3A RID: 15674
	[PublicizedFrom(EAccessModifier.Private)]
	public string eyeColor = "";

	// Token: 0x04003D3B RID: 15675
	[PublicizedFrom(EAccessModifier.Private)]
	public string hairName = "";

	// Token: 0x04003D3C RID: 15676
	[PublicizedFrom(EAccessModifier.Private)]
	public string hairColor = "";

	// Token: 0x04003D3D RID: 15677
	[PublicizedFrom(EAccessModifier.Private)]
	public string mustacheName = "";

	// Token: 0x04003D3E RID: 15678
	[PublicizedFrom(EAccessModifier.Private)]
	public string chopsName = "";

	// Token: 0x04003D3F RID: 15679
	[PublicizedFrom(EAccessModifier.Private)]
	public string beardName = "";

	// Token: 0x04003D40 RID: 15680
	[PublicizedFrom(EAccessModifier.Private)]
	public const int version = 5;
}
