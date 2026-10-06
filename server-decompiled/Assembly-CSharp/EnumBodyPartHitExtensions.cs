using System;
using System.Runtime.CompilerServices;

// Token: 0x0200046F RID: 1135
public static class EnumBodyPartHitExtensions
{
	// Token: 0x0600220C RID: 8716 RVA: 0x000CE189 File Offset: 0x000CC389
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNone(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Arms) > EnumBodyPartHit.None;
	}

	// Token: 0x0600220D RID: 8717 RVA: 0x000CE189 File Offset: 0x000CC389
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsArm(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Arms) > EnumBodyPartHit.None;
	}

	// Token: 0x0600220E RID: 8718 RVA: 0x000CE195 File Offset: 0x000CC395
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Legs) > EnumBodyPartHit.None;
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x000CE1A1 File Offset: 0x000CC3A1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLeftLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & (EnumBodyPartHit.LeftUpperLeg | EnumBodyPartHit.LeftLowerLeg)) > EnumBodyPartHit.None;
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x000CE1AD File Offset: 0x000CC3AD
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsRightLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & (EnumBodyPartHit.RightUpperLeg | EnumBodyPartHit.RightLowerLeg)) > EnumBodyPartHit.None;
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x000CE1B9 File Offset: 0x000CC3B9
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

	// Token: 0x06002212 RID: 8722 RVA: 0x000CE1DE File Offset: 0x000CC3DE
	public static EnumBodyPartHit ToFlag(this BodyPrimaryHit _parts)
	{
		return (EnumBodyPartHit)(1 << _parts - BodyPrimaryHit.Torso);
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x000CE1E8 File Offset: 0x000CC3E8
	public static bool IsMultiHit(this EnumBodyPartHit _parts)
	{
		EnumBodyPartHit enumBodyPartHit = _parts.ToPrimary().ToFlag();
		return (_parts & ~enumBodyPartHit) > EnumBodyPartHit.None;
	}

	// Token: 0x06002214 RID: 8724 RVA: 0x000CE208 File Offset: 0x000CC408
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
