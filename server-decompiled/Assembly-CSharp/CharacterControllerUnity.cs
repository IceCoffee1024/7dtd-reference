using System;
using UnityEngine;

// Token: 0x02000508 RID: 1288
public class CharacterControllerUnity : CharacterControllerAbstract
{
	// Token: 0x060029FD RID: 10749 RVA: 0x001083DB File Offset: 0x001065DB
	public CharacterControllerUnity(CharacterController _cc)
	{
		this.cc = _cc;
	}

	// Token: 0x060029FE RID: 10750 RVA: 0x001083EA File Offset: 0x001065EA
	public override void Enable(bool isEnabled)
	{
		this.cc.enabled = isEnabled;
	}

	// Token: 0x060029FF RID: 10751 RVA: 0x001083F8 File Offset: 0x001065F8
	public override void SetStepOffset(float _stepOffset)
	{
		this.cc.stepOffset = _stepOffset;
	}

	// Token: 0x06002A00 RID: 10752 RVA: 0x00108406 File Offset: 0x00106606
	public override float GetStepOffset()
	{
		return this.cc.stepOffset;
	}

	// Token: 0x06002A01 RID: 10753 RVA: 0x00108413 File Offset: 0x00106613
	public override void SetSize(Vector3 _center, float _height, float _radius)
	{
		this.cc.center = _center;
		this.cc.height = _height;
		this.cc.radius = _radius;
	}

	// Token: 0x06002A02 RID: 10754 RVA: 0x00108439 File Offset: 0x00106639
	public override void SetCenter(Vector3 _center)
	{
		this.cc.center = _center;
	}

	// Token: 0x06002A03 RID: 10755 RVA: 0x00108447 File Offset: 0x00106647
	public override Vector3 GetCenter()
	{
		return this.cc.center;
	}

	// Token: 0x06002A04 RID: 10756 RVA: 0x00108454 File Offset: 0x00106654
	public override void SetRadius(float _radius)
	{
		this.cc.radius = _radius;
	}

	// Token: 0x06002A05 RID: 10757 RVA: 0x00108462 File Offset: 0x00106662
	public override float GetRadius()
	{
		return this.cc.radius;
	}

	// Token: 0x06002A06 RID: 10758 RVA: 0x0010846F File Offset: 0x0010666F
	public override void SetSkinWidth(float _width)
	{
		this.cc.skinWidth = _width;
	}

	// Token: 0x06002A07 RID: 10759 RVA: 0x0010847D File Offset: 0x0010667D
	public override float GetSkinWidth()
	{
		return this.cc.skinWidth;
	}

	// Token: 0x06002A08 RID: 10760 RVA: 0x0010848A File Offset: 0x0010668A
	public override void SetHeight(float _height)
	{
		this.cc.height = _height;
	}

	// Token: 0x06002A09 RID: 10761 RVA: 0x00108498 File Offset: 0x00106698
	public override float GetHeight()
	{
		return this.cc.height;
	}

	// Token: 0x06002A0A RID: 10762 RVA: 0x001084A5 File Offset: 0x001066A5
	public override bool IsGrounded()
	{
		return this.cc.isGrounded;
	}

	// Token: 0x06002A0B RID: 10763 RVA: 0x001084B2 File Offset: 0x001066B2
	public override CollisionFlags Move(Vector3 _dir)
	{
		return this.cc.Move(_dir);
	}

	// Token: 0x06002A0C RID: 10764 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Rotate(Quaternion _dir)
	{
	}

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x06002A0D RID: 10765 RVA: 0x001084C0 File Offset: 0x001066C0
	// (set) Token: 0x06002A0E RID: 10766 RVA: 0x001084CD File Offset: 0x001066CD
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

	// Token: 0x04001FF6 RID: 8182
	[PublicizedFrom(EAccessModifier.Private)]
	public CharacterController cc;
}
