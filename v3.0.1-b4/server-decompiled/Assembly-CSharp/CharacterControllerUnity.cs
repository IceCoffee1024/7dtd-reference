using System;
using UnityEngine;

// Token: 0x020004FD RID: 1277
public class CharacterControllerUnity : CharacterControllerAbstract
{
	// Token: 0x060029DE RID: 10718 RVA: 0x00106A67 File Offset: 0x00104C67
	public CharacterControllerUnity(CharacterController _cc)
	{
		this.cc = _cc;
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x00106A76 File Offset: 0x00104C76
	public override void Enable(bool isEnabled)
	{
		this.cc.enabled = isEnabled;
	}

	// Token: 0x060029E0 RID: 10720 RVA: 0x00106A84 File Offset: 0x00104C84
	public override void SetStepOffset(float _stepOffset)
	{
		this.cc.stepOffset = _stepOffset;
	}

	// Token: 0x060029E1 RID: 10721 RVA: 0x00106A92 File Offset: 0x00104C92
	public override float GetStepOffset()
	{
		return this.cc.stepOffset;
	}

	// Token: 0x060029E2 RID: 10722 RVA: 0x00106A9F File Offset: 0x00104C9F
	public override void SetSize(Vector3 _center, float _height, float _radius)
	{
		this.cc.center = _center;
		this.cc.height = _height;
		this.cc.radius = _radius;
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x00106AC5 File Offset: 0x00104CC5
	public override void SetCenter(Vector3 _center)
	{
		this.cc.center = _center;
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x00106AD3 File Offset: 0x00104CD3
	public override Vector3 GetCenter()
	{
		return this.cc.center;
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x00106AE0 File Offset: 0x00104CE0
	public override void SetRadius(float _radius)
	{
		this.cc.radius = _radius;
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x00106AEE File Offset: 0x00104CEE
	public override float GetRadius()
	{
		return this.cc.radius;
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x00106AFB File Offset: 0x00104CFB
	public override void SetSkinWidth(float _width)
	{
		this.cc.skinWidth = _width;
	}

	// Token: 0x060029E8 RID: 10728 RVA: 0x00106B09 File Offset: 0x00104D09
	public override float GetSkinWidth()
	{
		return this.cc.skinWidth;
	}

	// Token: 0x060029E9 RID: 10729 RVA: 0x00106B16 File Offset: 0x00104D16
	public override void SetHeight(float _height)
	{
		this.cc.height = _height;
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x00106B24 File Offset: 0x00104D24
	public override float GetHeight()
	{
		return this.cc.height;
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x00106B31 File Offset: 0x00104D31
	public override bool IsGrounded()
	{
		return this.cc.isGrounded;
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x00106B3E File Offset: 0x00104D3E
	public override CollisionFlags Move(Vector3 _dir)
	{
		return this.cc.Move(_dir);
	}

	// Token: 0x060029ED RID: 10733 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Rotate(Quaternion _dir)
	{
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x060029EE RID: 10734 RVA: 0x00106B4C File Offset: 0x00104D4C
	// (set) Token: 0x060029EF RID: 10735 RVA: 0x00106B59 File Offset: 0x00104D59
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

	// Token: 0x04001FBF RID: 8127
	[PublicizedFrom(EAccessModifier.Private)]
	public CharacterController cc;
}
