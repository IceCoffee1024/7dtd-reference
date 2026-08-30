using System;
using System.Runtime.CompilerServices;

// Token: 0x0200046D RID: 1133
public static class EnumBodyPartHitExtensions
{
	// Token: 0x0600221D RID: 8733 RVA: 0x000CE0D5 File Offset: 0x000CC2D5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNone(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Arms) > EnumBodyPartHit.None;
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x000CE0D5 File Offset: 0x000CC2D5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsArm(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Arms) > EnumBodyPartHit.None;
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x000CE0E1 File Offset: 0x000CC2E1
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Legs) > EnumBodyPartHit.None;
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x000CE0ED File Offset: 0x000CC2ED
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLeftLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & (EnumBodyPartHit.LeftUpperLeg | EnumBodyPartHit.LeftLowerLeg)) > EnumBodyPartHit.None;
	}

	// Token: 0x06002221 RID: 8737 RVA: 0x000CE0F9 File Offset: 0x000CC2F9
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsRightLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & (EnumBodyPartHit.RightUpperLeg | EnumBodyPartHit.RightLowerLeg)) > EnumBodyPartHit.None;
	}

	// Token: 0x06002222 RID: 8738 RVA: 0x000CE105 File Offset: 0x000CC305
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

	// Token: 0x06002223 RID: 8739 RVA: 0x000CE12A File Offset: 0x000CC32A
	public static EnumBodyPartHit ToFlag(this BodyPrimaryHit _parts)
	{
		return (EnumBodyPartHit)(1 << _parts - BodyPrimaryHit.Torso);
	}

	// Token: 0x06002224 RID: 8740 RVA: 0x000CE134 File Offset: 0x000CC334
	public static bool IsMultiHit(this EnumBodyPartHit _parts)
	{
		EnumBodyPartHit enumBodyPartHit = _parts.ToPrimary().ToFlag();
		return (_parts & ~enumBodyPartHit) > EnumBodyPartHit.None;
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x000CE154 File Offset: 0x000CC354
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
