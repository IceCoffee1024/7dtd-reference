using System;
using System.Runtime.CompilerServices;

// Token: 0x02000471 RID: 1137
public struct BodyDamage
{
	// Token: 0x170003DE RID: 990
	// (get) Token: 0x06002217 RID: 8727 RVA: 0x000CE355 File Offset: 0x000CC555
	public bool HasLeftLeg
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 96U) == 0U;
		}
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x06002218 RID: 8728 RVA: 0x000CE363 File Offset: 0x000CC563
	public bool HasRightLeg
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 384U) == 0U;
		}
	}

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x06002219 RID: 8729 RVA: 0x000CE374 File Offset: 0x000CC574
	public bool HasLimbs
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 330U) != 330U;
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x0600221A RID: 8730 RVA: 0x000CE38C File Offset: 0x000CC58C
	public bool IsAnyLegMissing
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 480U) > 0U;
		}
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x0600221B RID: 8731 RVA: 0x000CE39D File Offset: 0x000CC59D
	public bool IsAnyArmOrLegMissing
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (this.Flags & 510U) > 0U;
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x0600221C RID: 8732 RVA: 0x000CE3AE File Offset: 0x000CC5AE
	public bool IsCrippled
	{
		get
		{
			return (this.Flags & 12288U) > 0U;
		}
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x000CE3C0 File Offset: 0x000CC5C0
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

	// Token: 0x0600221E RID: 8734 RVA: 0x000CE404 File Offset: 0x000CC604
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

	// Token: 0x0600221F RID: 8735 RVA: 0x000CE59E File Offset: 0x000CC79E
	public void Write(PooledBinaryWriter bw)
	{
		bw.Write(BodyDamage.cBinaryVersion);
		bw.Write((int)this.damageType);
		bw.Write(this.Flags);
	}

	// Token: 0x040017D7 RID: 6103
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cBinaryVersion = 4;

	// Token: 0x040017D8 RID: 6104
	public int StunKnee;

	// Token: 0x040017D9 RID: 6105
	public int StunProne;

	// Token: 0x040017DA RID: 6106
	public float StunDuration;

	// Token: 0x040017DB RID: 6107
	public EnumEntityStunType CurrentStun;

	// Token: 0x040017DC RID: 6108
	public bool ShouldBeCrawler;

	// Token: 0x040017DD RID: 6109
	public const uint cNoHead = 1U;

	// Token: 0x040017DE RID: 6110
	public const uint cNoArmLUpper = 2U;

	// Token: 0x040017DF RID: 6111
	public const uint cNoArmLLower = 4U;

	// Token: 0x040017E0 RID: 6112
	public const uint cNoArmRUpper = 8U;

	// Token: 0x040017E1 RID: 6113
	public const uint cNoArmRLower = 16U;

	// Token: 0x040017E2 RID: 6114
	public const uint cNoArm = 30U;

	// Token: 0x040017E3 RID: 6115
	public const uint cNoLegLUpper = 32U;

	// Token: 0x040017E4 RID: 6116
	public const uint cNoLegLLower = 64U;

	// Token: 0x040017E5 RID: 6117
	public const uint cNoLegRUpper = 128U;

	// Token: 0x040017E6 RID: 6118
	public const uint cNoLegRLower = 256U;

	// Token: 0x040017E7 RID: 6119
	public const uint cNoLeg = 480U;

	// Token: 0x040017E8 RID: 6120
	public const uint cCrippledLegL = 4096U;

	// Token: 0x040017E9 RID: 6121
	public const uint cCrippledLegR = 8192U;

	// Token: 0x040017EA RID: 6122
	public uint Flags;

	// Token: 0x040017EB RID: 6123
	public EnumDamageTypes damageType;

	// Token: 0x040017EC RID: 6124
	public EnumBodyPartHit bodyPartHit;
}
