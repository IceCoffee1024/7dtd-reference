using System;
using System.IO;

// Token: 0x02000A35 RID: 2613
public class PlayerProfile
{
	// Token: 0x1700086F RID: 2159
	// (get) Token: 0x06004EFF RID: 20223 RVA: 0x001DFA5B File Offset: 0x001DDC5B
	// (set) Token: 0x06004F00 RID: 20224 RVA: 0x001DFA63 File Offset: 0x001DDC63
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

	// Token: 0x17000870 RID: 2160
	// (get) Token: 0x06004F01 RID: 20225 RVA: 0x001DFA6C File Offset: 0x001DDC6C
	// (set) Token: 0x06004F02 RID: 20226 RVA: 0x001DFA74 File Offset: 0x001DDC74
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

	// Token: 0x17000871 RID: 2161
	// (get) Token: 0x06004F03 RID: 20227 RVA: 0x001DFA7D File Offset: 0x001DDC7D
	// (set) Token: 0x06004F04 RID: 20228 RVA: 0x001DFA85 File Offset: 0x001DDC85
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

	// Token: 0x17000872 RID: 2162
	// (get) Token: 0x06004F05 RID: 20229 RVA: 0x001DFA8E File Offset: 0x001DDC8E
	// (set) Token: 0x06004F06 RID: 20230 RVA: 0x001DFA96 File Offset: 0x001DDC96
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

	// Token: 0x17000873 RID: 2163
	// (get) Token: 0x06004F07 RID: 20231 RVA: 0x001DFA9F File Offset: 0x001DDC9F
	// (set) Token: 0x06004F08 RID: 20232 RVA: 0x001DFAA7 File Offset: 0x001DDCA7
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

	// Token: 0x17000874 RID: 2164
	// (get) Token: 0x06004F09 RID: 20233 RVA: 0x001DFAB0 File Offset: 0x001DDCB0
	// (set) Token: 0x06004F0A RID: 20234 RVA: 0x001DFAB8 File Offset: 0x001DDCB8
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

	// Token: 0x17000875 RID: 2165
	// (get) Token: 0x06004F0B RID: 20235 RVA: 0x001DFAC1 File Offset: 0x001DDCC1
	// (set) Token: 0x06004F0C RID: 20236 RVA: 0x001DFAC9 File Offset: 0x001DDCC9
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

	// Token: 0x17000876 RID: 2166
	// (get) Token: 0x06004F0D RID: 20237 RVA: 0x001DFAD2 File Offset: 0x001DDCD2
	// (set) Token: 0x06004F0E RID: 20238 RVA: 0x001DFADA File Offset: 0x001DDCDA
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

	// Token: 0x17000877 RID: 2167
	// (get) Token: 0x06004F0F RID: 20239 RVA: 0x001DFAE3 File Offset: 0x001DDCE3
	// (set) Token: 0x06004F10 RID: 20240 RVA: 0x001DFAEB File Offset: 0x001DDCEB
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

	// Token: 0x17000878 RID: 2168
	// (get) Token: 0x06004F11 RID: 20241 RVA: 0x001DFAF4 File Offset: 0x001DDCF4
	// (set) Token: 0x06004F12 RID: 20242 RVA: 0x001DFB41 File Offset: 0x001DDD41
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

	// Token: 0x06004F13 RID: 20243 RVA: 0x001DFB4C File Offset: 0x001DDD4C
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

	// Token: 0x17000879 RID: 2169
	// (get) Token: 0x06004F14 RID: 20244 RVA: 0x001DFBFB File Offset: 0x001DDDFB
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

	// Token: 0x06004F15 RID: 20245 RVA: 0x001DFC10 File Offset: 0x001DDE10
	public PlayerProfile()
	{
		this.raceName = "white";
		this.isMale = true;
		this.variantNumber = 1;
	}

	// Token: 0x06004F16 RID: 20246 RVA: 0x001DFCA4 File Offset: 0x001DDEA4
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

	// Token: 0x06004F17 RID: 20247 RVA: 0x001DFD30 File Offset: 0x001DDF30
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

	// Token: 0x06004F18 RID: 20248 RVA: 0x001DFDF4 File Offset: 0x001DDFF4
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

	// Token: 0x06004F19 RID: 20249 RVA: 0x001DFE97 File Offset: 0x001DE097
	public static PlayerProfile LoadLocalProfile()
	{
		return PlayerProfile.LoadProfile(ProfileSDF.CurrentProfileName());
	}

	// Token: 0x06004F1A RID: 20250 RVA: 0x001DFEA4 File Offset: 0x001DE0A4
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

	// Token: 0x04003C98 RID: 15512
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isMale = true;

	// Token: 0x04003C99 RID: 15513
	[PublicizedFrom(EAccessModifier.Private)]
	public string raceName = "";

	// Token: 0x04003C9A RID: 15514
	[PublicizedFrom(EAccessModifier.Private)]
	public int variantNumber = 1;

	// Token: 0x04003C9B RID: 15515
	[PublicizedFrom(EAccessModifier.Private)]
	public string archetype = "";

	// Token: 0x04003C9C RID: 15516
	[PublicizedFrom(EAccessModifier.Private)]
	public string eyeColor = "";

	// Token: 0x04003C9D RID: 15517
	[PublicizedFrom(EAccessModifier.Private)]
	public string hairName = "";

	// Token: 0x04003C9E RID: 15518
	[PublicizedFrom(EAccessModifier.Private)]
	public string hairColor = "";

	// Token: 0x04003C9F RID: 15519
	[PublicizedFrom(EAccessModifier.Private)]
	public string mustacheName = "";

	// Token: 0x04003CA0 RID: 15520
	[PublicizedFrom(EAccessModifier.Private)]
	public string chopsName = "";

	// Token: 0x04003CA1 RID: 15521
	[PublicizedFrom(EAccessModifier.Private)]
	public string beardName = "";

	// Token: 0x04003CA2 RID: 15522
	[PublicizedFrom(EAccessModifier.Private)]
	public const int version = 5;
}
