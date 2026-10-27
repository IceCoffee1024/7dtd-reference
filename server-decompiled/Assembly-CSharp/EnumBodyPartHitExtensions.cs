using System;
using System.Runtime.CompilerServices;

// Token: 0x02000471 RID: 1137
public static class EnumBodyPartHitExtensions
{
	// Token: 0x06002211 RID: 8721 RVA: 0x000CE535 File Offset: 0x000CC735
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNone(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Arms) > EnumBodyPartHit.None;
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x000CE535 File Offset: 0x000CC735
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsArm(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Arms) > EnumBodyPartHit.None;
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x000CE541 File Offset: 0x000CC741
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & EnumBodyPartHit.Legs) > EnumBodyPartHit.None;
	}

	// Token: 0x06002214 RID: 8724 RVA: 0x000CE54D File Offset: 0x000CC74D
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLeftLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & (EnumBodyPartHit.LeftUpperLeg | EnumBodyPartHit.LeftLowerLeg)) > EnumBodyPartHit.None;
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x000CE559 File Offset: 0x000CC759
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsRightLeg(this EnumBodyPartHit bodyPart)
	{
		return (bodyPart & (EnumBodyPartHit.RightUpperLeg | EnumBodyPartHit.RightLowerLeg)) > EnumBodyPartHit.None;
	}

	// Token: 0x06002216 RID: 8726 RVA: 0x000CE565 File Offset: 0x000CC765
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

	// Token: 0x06002217 RID: 8727 RVA: 0x000CE58A File Offset: 0x000CC78A
	public static EnumBodyPartHit ToFlag(this BodyPrimaryHit _parts)
	{
		return (EnumBodyPartHit)(1 << _parts - BodyPrimaryHit.Torso);
	}

	// Token: 0x06002218 RID: 8728 RVA: 0x000CE594 File Offset: 0x000CC794
	public static bool IsMultiHit(this EnumBodyPartHit _parts)
	{
		EnumBodyPartHit enumBodyPartHit = _parts.ToPrimary().ToFlag();
		return (_parts & ~enumBodyPartHit) > EnumBodyPartHit.None;
	}

	// Token: 0x06002219 RID: 8729 RVA: 0x000CE5B4 File Offset: 0x000CC7B4
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
