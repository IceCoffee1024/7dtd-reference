using System;
using UnityEngine;

// Token: 0x02000502 RID: 1282
public class CharacterControllerUnity : CharacterControllerAbstract
{
	// Token: 0x06002A08 RID: 10760 RVA: 0x00107AA3 File Offset: 0x00105CA3
	public CharacterControllerUnity(CharacterController _cc)
	{
		this.cc = _cc;
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x00107AB2 File Offset: 0x00105CB2
	public override void Enable(bool isEnabled)
	{
		this.cc.enabled = isEnabled;
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x00107AC0 File Offset: 0x00105CC0
	public override void SetStepOffset(float _stepOffset)
	{
		this.cc.stepOffset = _stepOffset;
	}

	// Token: 0x06002A0B RID: 10763 RVA: 0x00107ACE File Offset: 0x00105CCE
	public override float GetStepOffset()
	{
		return this.cc.stepOffset;
	}

	// Token: 0x06002A0C RID: 10764 RVA: 0x00107ADB File Offset: 0x00105CDB
	public override void SetSize(Vector3 _center, float _height, float _radius)
	{
		this.cc.center = _center;
		this.cc.height = _height;
		this.cc.radius = _radius;
	}

	// Token: 0x06002A0D RID: 10765 RVA: 0x00107B01 File Offset: 0x00105D01
	public override void SetCenter(Vector3 _center)
	{
		this.cc.center = _center;
	}

	// Token: 0x06002A0E RID: 10766 RVA: 0x00107B0F File Offset: 0x00105D0F
	public override Vector3 GetCenter()
	{
		return this.cc.center;
	}

	// Token: 0x06002A0F RID: 10767 RVA: 0x00107B1C File Offset: 0x00105D1C
	public override void SetRadius(float _radius)
	{
		this.cc.radius = _radius;
	}

	// Token: 0x06002A10 RID: 10768 RVA: 0x00107B2A File Offset: 0x00105D2A
	public override float GetRadius()
	{
		return this.cc.radius;
	}

	// Token: 0x06002A11 RID: 10769 RVA: 0x00107B37 File Offset: 0x00105D37
	public override void SetSkinWidth(float _width)
	{
		this.cc.skinWidth = _width;
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x00107B45 File Offset: 0x00105D45
	public override float GetSkinWidth()
	{
		return this.cc.skinWidth;
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x00107B52 File Offset: 0x00105D52
	public override void SetHeight(float _height)
	{
		this.cc.height = _height;
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x00107B60 File Offset: 0x00105D60
	public override float GetHeight()
	{
		return this.cc.height;
	}

	// Token: 0x06002A15 RID: 10773 RVA: 0x00107B6D File Offset: 0x00105D6D
	public override bool IsGrounded()
	{
		return this.cc.isGrounded;
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x00107B7A File Offset: 0x00105D7A
	public override CollisionFlags Move(Vector3 _dir)
	{
		return this.cc.Move(_dir);
	}

	// Token: 0x06002A17 RID: 10775 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Rotate(Quaternion _dir)
	{
	}

	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x06002A18 RID: 10776 RVA: 0x00107B88 File Offset: 0x00105D88
	// (set) Token: 0x06002A19 RID: 10777 RVA: 0x00107B95 File Offset: 0x00105D95
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

	// Token: 0x04001FEE RID: 8174
	[PublicizedFrom(EAccessModifier.Private)]
	public CharacterController cc;
}
