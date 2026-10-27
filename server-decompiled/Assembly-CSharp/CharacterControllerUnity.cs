using System;
using UnityEngine;

// Token: 0x0200050A RID: 1290
public class CharacterControllerUnity : CharacterControllerAbstract
{
	// Token: 0x06002A04 RID: 10756 RVA: 0x0010892F File Offset: 0x00106B2F
	public CharacterControllerUnity(CharacterController _cc)
	{
		this.cc = _cc;
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x0010893E File Offset: 0x00106B3E
	public override void Enable(bool isEnabled)
	{
		this.cc.enabled = isEnabled;
	}

	// Token: 0x06002A06 RID: 10758 RVA: 0x0010894C File Offset: 0x00106B4C
	public override void SetStepOffset(float _stepOffset)
	{
		this.cc.stepOffset = _stepOffset;
	}

	// Token: 0x06002A07 RID: 10759 RVA: 0x0010895A File Offset: 0x00106B5A
	public override float GetStepOffset()
	{
		return this.cc.stepOffset;
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x00108967 File Offset: 0x00106B67
	public override void SetSize(Vector3 _center, float _height, float _radius)
	{
		this.cc.center = _center;
		this.cc.height = _height;
		this.cc.radius = _radius;
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x0010898D File Offset: 0x00106B8D
	public override void SetCenter(Vector3 _center)
	{
		this.cc.center = _center;
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x0010899B File Offset: 0x00106B9B
	public override Vector3 GetCenter()
	{
		return this.cc.center;
	}

	// Token: 0x06002A0B RID: 10763 RVA: 0x001089A8 File Offset: 0x00106BA8
	public override void SetRadius(float _radius)
	{
		this.cc.radius = _radius;
	}

	// Token: 0x06002A0C RID: 10764 RVA: 0x001089B6 File Offset: 0x00106BB6
	public override float GetRadius()
	{
		return this.cc.radius;
	}

	// Token: 0x06002A0D RID: 10765 RVA: 0x001089C3 File Offset: 0x00106BC3
	public override void SetSkinWidth(float _width)
	{
		this.cc.skinWidth = _width;
	}

	// Token: 0x06002A0E RID: 10766 RVA: 0x001089D1 File Offset: 0x00106BD1
	public override float GetSkinWidth()
	{
		return this.cc.skinWidth;
	}

	// Token: 0x06002A0F RID: 10767 RVA: 0x001089DE File Offset: 0x00106BDE
	public override void SetHeight(float _height)
	{
		this.cc.height = _height;
	}

	// Token: 0x06002A10 RID: 10768 RVA: 0x001089EC File Offset: 0x00106BEC
	public override float GetHeight()
	{
		return this.cc.height;
	}

	// Token: 0x06002A11 RID: 10769 RVA: 0x001089F9 File Offset: 0x00106BF9
	public override bool IsGrounded()
	{
		return this.cc.isGrounded;
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x00108A06 File Offset: 0x00106C06
	public override CollisionFlags Move(Vector3 _dir)
	{
		return this.cc.Move(_dir);
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Rotate(Quaternion _dir)
	{
	}

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06002A14 RID: 10772 RVA: 0x00108A14 File Offset: 0x00106C14
	// (set) Token: 0x06002A15 RID: 10773 RVA: 0x00108A21 File Offset: 0x00106C21
	public override bool enableOverlapRecovery
	{
		get
		{
			return this.cc.enableOverlapRecovery;
		}
		set
		{
			this.cc.enableOverlapRecovery = value;
		}
	}

	// Token: 0x04002006 RID: 8198
	[PublicizedFrom(EAccessModifier.Private)]
	public CharacterController cc;
}
