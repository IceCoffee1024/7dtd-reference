using System;
using System.Runtime.CompilerServices;

// Token: 0x02000469 RID: 1129
public static class EnumBodyPartHitExtensions
{
	// Token: 0x06002200 RID: 8704 RVA: 0x000CD939 File Offset: 0x000CBB39
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNone(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Arms) > EnumBodyPartHit.None;
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x000CD939 File Offset: 0x000CBB39
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsArm(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Arms) > EnumBodyPartHit.None;
	}

	// Token: 0x06002202 RID: 8706 RVA: 0x000CD945 File Offset: 0x000CBB45
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Legs) > EnumBodyPartHit.None;
	}

	// Token: 0x06002203 RID: 8707 RVA: 0x000CD951 File Offset: 0x000CBB51
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLeftLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & (EnumBodyPartHit.LeftUpperLeg | EnumBodyPartHit.LeftLowerLeg)) > EnumBodyPartHit.None;
	}

	// Token: 0x06002204 RID: 8708 RVA: 0x000CD95D File Offset: 0x000CBB5D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsRightLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & (EnumBodyPartHit.RightUpperLeg | EnumBodyPartHit.RightLowerLeg)) > EnumBodyPartHit.None;
	}

	// Token: 0x06002205 RID: 8709 RVA: 0x000CD969 File Offset: 0x000CBB69
	public static BodyPrimaryHit LowerToUpperLimb(this BodyPrimaryHit _primary)
	{
		switch (_primary)
		{
		case BodyPrimaryHit.LeftLowerArm:
			return BodyPrimaryHit.LeftUpperArm;
		case BodyPrimaryHit.RightLowerArm:
			return BodyPrimaryHit.RightUpperArm;
		case BodyPrimaryHit.LeftLowerLeg:
			return BodyPrimaryHit.LeftUpperLeg;
		case BodyPrimaryHit.RightLowerLeg:
			return BodyPrimaryHit.RightUpperLeg;
		default:
			return _primary;
		}
	}

	// Token: 0x06002206 RID: 8710 RVA: 0x000CD98E File Offset: 0x000CBB8E
	public static EnumBodyPartHit ToFlag(this BodyPrimaryHit _parts)
	{
		return (EnumBodyPartHit)(1 << _parts - BodyPrimaryHit.Torso);
	}

	// Token: 0x06002207 RID: 8711 RVA: 0x000CD998 File Offset: 0x000CBB98
	public static bool IsMultiHit(this EnumBodyPartHit _parts)
	{
		EnumBodyPartHit enumBodyPartHit = _parts.ToPrimary().ToFlag();
		return (_parts & ~enumBodyPartHit) > EnumBodyPartHit.None;
	}

	// Token: 0x06002208 RID: 8712 RVA: 0x000CD9B8 File Offset: 0x000CBBB8
	public static BodyPrimaryHit ToPrimary(this EnumBodyPartHit _part)
	{
		if ((_part & EnumBodyPartHit.Head) > EnumBodyPartHit.None)
		{
			return BodyPrimaryHit.Head;
		}
		if ((_part & EnumBodyPartHit.LeftUpperLeg) > EnumBodyPartHit.None)
		{
			return BodyPrimaryHit.LeftUpperLeg;
		}
		if ((_part & EnumBodyPartHit.RightUpperLeg) > EnumBodyPartHit.None)
		{
			return BodyPrimaryHit.RightUpperLeg;
		}
		if ((_part & EnumBodyPartHit.LeftLowerLeg) > EnumBodyPartHit.None)
		{
			return BodyPrimaryHit.LeftLowerLeg;
		}
		if ((_part & EnumBodyPartHit.RightLowerLeg) > EnumBodyPartHit.None)
		{
			return BodyPrimaryHit.RightLowerLeg;
		}
		if ((_part & EnumBodyPartHit.LeftUpperArm) > EnumBodyPartHit.None)
		{
			return BodyPrimaryHit.LeftUpperArm;
		}
		if ((_part & EnumBodyPartHit.RightUpperArm) > EnumBodyPartHit.None)
		{
			return BodyPrimaryHit.RightUpperArm;
		}
		if ((_part & EnumBodyPartHit.LeftLowerArm) > EnumBodyPartHit.None)
		{
			return BodyPrimaryHit.LeftLowerLeg;
		}
		if ((_part & EnumBodyPartHit.RightLowerArm) > EnumBodyPartHit.None)
		{
			return BodyPrimaryHit.RightLowerLeg;
		}
		if (_part != EnumBodyPartHit.None)
		{
			return BodyPrimaryHit.Torso;
		}
		return BodyPrimaryHit.None;
	}
}
