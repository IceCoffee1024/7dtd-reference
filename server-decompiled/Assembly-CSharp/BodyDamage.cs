using System;
using System.Runtime.CompilerServices;

// Token: 0x02000473 RID: 1139
public struct BodyDamage
{
	// Token: 0x170003DE RID: 990
	// (get) Token: 0x0600221C RID: 8732 RVA: 0x000CE701 File Offset: 0x000CC901
	public bool HasLeftLeg
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 96U) == 0U;
		}
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x0600221D RID: 8733 RVA: 0x000CE70F File Offset: 0x000CC90F
	public bool HasRightLeg
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 384U) == 0U;
		}
	}

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x0600221E RID: 8734 RVA: 0x000CE720 File Offset: 0x000CC920
	public bool HasLimbs
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 330U) != 330U;
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x0600221F RID: 8735 RVA: 0x000CE738 File Offset: 0x000CC938
	public bool IsAnyLegMissing
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 480U) > 0U;
		}
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x06002220 RID: 8736 RVA: 0x000CE749 File Offset: 0x000CC949
	public bool IsAnyArmOrLegMissing
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 510U) > 0U;
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x06002221 RID: 8737 RVA: 0x000CE75A File Offset: 0x000CC95A
	public bool IsCrippled
	{
		get
		{
			return (this.Flags & 12288U) > 0U;
		}
	}

	// Token: 0x06002222 RID: 8738 RVA: 0x000CE76C File Offset: 0x000CC96C
	public static BodyDamage Read(PooledBinaryReader _br, int _version)
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

	// Token: 0x06002223 RID: 8739 RVA: 0x000CE7B0 File Offset: 0x000CC9B0
	[PublicizedFrom(EAccessModifier.Private)]
	public static BodyDamage ReadData(PooledBinaryReader br, int version)
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

	// Token: 0x06002224 RID: 8740 RVA: 0x000CE94A File Offset: 0x000CCB4A
	public void Write(PooledBinaryWriter bw)
	{
		bw.Write(BodyDamage.cBinaryVersion);
		bw.Write((int)this.damageType);
		bw.Write(this.Flags);
	}

	// Token: 0x040017E7 RID: 6119
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cBinaryVersion = 4;

	// Token: 0x040017E8 RID: 6120
	public int StunKnee;

	// Token: 0x040017E9 RID: 6121
	public int StunProne;

	// Token: 0x040017EA RID: 6122
	public float StunDuration;

	// Token: 0x040017EB RID: 6123
	public EnumEntityStunType CurrentStun;

	// Token: 0x040017EC RID: 6124
	public bool ShouldBeCrawler;

	// Token: 0x040017ED RID: 6125
	public const uint cNoHead = 1U;

	// Token: 0x040017EE RID: 6126
	public const uint cNoArmLUpper = 2U;

	// Token: 0x040017EF RID: 6127
	public const uint cNoArmLLower = 4U;

	// Token: 0x040017F0 RID: 6128
	public const uint cNoArmRUpper = 8U;

	// Token: 0x040017F1 RID: 6129
	public const uint cNoArmRLower = 16U;

	// Token: 0x040017F2 RID: 6130
	public const uint cNoArm = 30U;

	// Token: 0x040017F3 RID: 6131
	public const uint cNoLegLUpper = 32U;

	// Token: 0x040017F4 RID: 6132
	public const uint cNoLegLLower = 64U;

	// Token: 0x040017F5 RID: 6133
	public const uint cNoLegRUpper = 128U;

	// Token: 0x040017F6 RID: 6134
	public const uint cNoLegRLower = 256U;

	// Token: 0x040017F7 RID: 6135
	public const uint cNoLeg = 480U;

	// Token: 0x040017F8 RID: 6136
	public const uint cCrippledLegL = 4096U;

	// Token: 0x040017F9 RID: 6137
	public const uint cCrippledLegR = 8192U;

	// Token: 0x040017FA RID: 6138
	public uint Flags;

	// Token: 0x040017FB RID: 6139
	public EnumDamageTypes damageType;

	// Token: 0x040017FC RID: 6140
	public EnumBodyPartHit bodyPartHit;
}
