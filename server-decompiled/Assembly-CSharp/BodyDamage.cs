using System;
using System.IO;
using System.Runtime.CompilerServices;

// Token: 0x0200046B RID: 1131
public struct BodyDamage
{
	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x0600220B RID: 8715 RVA: 0x000CDB05 File Offset: 0x000CBD05
	public bool HasLeftLeg
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 96U) == 0U;
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x0600220C RID: 8716 RVA: 0x000CDB13 File Offset: 0x000CBD13
	public bool HasRightLeg
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 384U) == 0U;
		}
	}

	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x0600220D RID: 8717 RVA: 0x000CDB24 File Offset: 0x000CBD24
	public bool HasLimbs
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 330U) != 330U;
		}
	}

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x0600220E RID: 8718 RVA: 0x000CDB3C File Offset: 0x000CBD3C
	public bool IsAnyLegMissing
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 480U) > 0U;
		}
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x0600220F RID: 8719 RVA: 0x000CDB4D File Offset: 0x000CBD4D
	public bool IsAnyArmOrLegMissing
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 510U) > 0U;
		}
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06002210 RID: 8720 RVA: 0x000CDB5E File Offset: 0x000CBD5E
	public bool IsCrippled
	{
		get
		{
			return (this.Flags & 12288U) > 0U;
		}
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x000CDB70 File Offset: 0x000CBD70
	public static BodyDamage Read(BinaryReader _br, int _version)
	{
		if (_version > 21)
		{
			return BodyDamage.ReadData(_br, _br.ReadInt32());
		}
		if (_version > 20)
		{
			return BodyDamage.ReadData(_br, 0);
		}
		if (_version > 19)
		{
			_br.ReadInt32();
		}
		return default(BodyDamage);
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x000CDBB4 File Offset: 0x000CBDB4
	[PublicizedFrom(EAccessModifier.Private)]
	public static BodyDamage ReadData(BinaryReader br, int version)
	{
		BodyDamage bodyDamage = default(BodyDamage);
		if (version >= 4)
		{
			bodyDamage.damageType = (EnumDamageTypes)br.ReadInt32();
		}
		if (version >= 3)
		{
			bodyDamage.Flags = br.ReadUInt32();
		}
		else
		{
			br.ReadInt16();
			br.ReadInt16();
			br.ReadInt16();
			br.ReadInt16();
			br.ReadInt16();
			br.ReadInt16();
			if (br.ReadBoolean())
			{
				bodyDamage.Flags |= 2U;
			}
			if (br.ReadBoolean())
			{
				bodyDamage.Flags |= 8U;
			}
			if (br.ReadBoolean())
			{
				bodyDamage.Flags |= 1U;
			}
			if (br.ReadBoolean())
			{
				bodyDamage.Flags |= 128U;
			}
			if (br.ReadBoolean())
			{
				bodyDamage.Flags |= 8192U;
			}
			if (version >= 1)
			{
				br.ReadInt16();
				br.ReadInt16();
				br.ReadInt16();
				br.ReadInt16();
				if (br.ReadBoolean())
				{
					bodyDamage.Flags |= 4U;
				}
				if (br.ReadBoolean())
				{
					bodyDamage.Flags |= 16U;
				}
				if (br.ReadBoolean())
				{
					bodyDamage.Flags |= 64U;
				}
				if (br.ReadBoolean())
				{
					bodyDamage.Flags |= 256U;
				}
				if (version >= 2 && br.ReadBoolean())
				{
					bodyDamage.Flags |= 32U;
				}
				if (br.ReadBoolean())
				{
					bodyDamage.Flags |= 4096U;
				}
			}
		}
		bodyDamage.ShouldBeCrawler = !bodyDamage.HasLeftLeg || !bodyDamage.HasRightLeg;
		return bodyDamage;
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x000CDD4E File Offset: 0x000CBF4E
	public void Write(BinaryWriter bw)
	{
		bw.Write(BodyDamage.cBinaryVersion);
		bw.Write((int)this.damageType);
		bw.Write(this.Flags);
	}

	// Token: 0x040017CA RID: 6090
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cBinaryVersion = 4;

	// Token: 0x040017CB RID: 6091
	public int StunKnee;

	// Token: 0x040017CC RID: 6092
	public int StunProne;

	// Token: 0x040017CD RID: 6093
	public float StunDuration;

	// Token: 0x040017CE RID: 6094
	public EnumEntityStunType CurrentStun;

	// Token: 0x040017CF RID: 6095
	public bool ShouldBeCrawler;

	// Token: 0x040017D0 RID: 6096
	public const uint cNoHead = 1U;

	// Token: 0x040017D1 RID: 6097
	public const uint cNoArmLUpper = 2U;

	// Token: 0x040017D2 RID: 6098
	public const uint cNoArmLLower = 4U;

	// Token: 0x040017D3 RID: 6099
	public const uint cNoArmRUpper = 8U;

	// Token: 0x040017D4 RID: 6100
	public const uint cNoArmRLower = 16U;

	// Token: 0x040017D5 RID: 6101
	public const uint cNoArm = 30U;

	// Token: 0x040017D6 RID: 6102
	public const uint cNoLegLUpper = 32U;

	// Token: 0x040017D7 RID: 6103
	public const uint cNoLegLLower = 64U;

	// Token: 0x040017D8 RID: 6104
	public const uint cNoLegRUpper = 128U;

	// Token: 0x040017D9 RID: 6105
	public const uint cNoLegRLower = 256U;

	// Token: 0x040017DA RID: 6106
	public const uint cNoLeg = 480U;

	// Token: 0x040017DB RID: 6107
	public const uint cCrippledLegL = 4096U;

	// Token: 0x040017DC RID: 6108
	public const uint cCrippledLegR = 8192U;

	// Token: 0x040017DD RID: 6109
	public uint Flags;

	// Token: 0x040017DE RID: 6110
	public EnumDamageTypes damageType;

	// Token: 0x040017DF RID: 6111
	public EnumBodyPartHit bodyPartHit;
}
