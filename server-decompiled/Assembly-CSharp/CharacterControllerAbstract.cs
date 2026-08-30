using System;
using UnityEngine;

// Token: 0x020004FA RID: 1274
public abstract class CharacterControllerAbstract
{
	// Token: 0x060029A8 RID: 10664
	public abstract void Enable(bool isEnabled);

	// Token: 0x060029A9 RID: 10665
	public abstract void SetStepOffset(float _stepOffset);

	// Token: 0x060029AA RID: 10666
	public abstract float GetStepOffset();

	// Token: 0x060029AB RID: 10667
	public abstract void SetSize(Vector3 _center, float _height, float _radius);

	// Token: 0x060029AC RID: 10668
	public abstract void SetCenter(Vector3 _center);

	// Token: 0x060029AD RID: 10669
	public abstract Vector3 GetCenter();

	// Token: 0x060029AE RID: 10670
	public abstract void SetRadius(float _radius);

	// Token: 0x060029AF RID: 10671
	public abstract float GetRadius();

	// Token: 0x060029B0 RID: 10672
	public abstract void SetSkinWidth(float _width);

	// Token: 0x060029B1 RID: 10673
	public abstract float GetSkinWidth();

	// Token: 0x060029B2 RID: 10674
	public abstract void SetHeight(float _height);

	// Token: 0x060029B3 RID: 10675
	public abstract float GetHeight();

	// Token: 0x060029B4 RID: 10676
	public abstract bool IsGrounded();

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x060029B5 RID: 10677 RVA: 0x0010653E File Offset: 0x0010473E
	public virtual Vector3 GroundNormal
	{
		get
		{
			return Vector3.up;
		}
	}

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x060029B6 RID: 10678
	// (set) Token: 0x060029B7 RID: 10679
	public abstract bool enableOverlapRecovery { get; set; }

	// Token: 0x060029B8 RID: 10680
	public abstract CollisionFlags Move(Vector3 _dir);

	// Token: 0x060029B9 RID: 10681 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual CollisionFlags Update()
	{
		return CollisionFlags.None;
	}

	// Token: 0x060029BA RID: 10682
	public abstract void Rotate(Quaternion _dir);

	// Token: 0x060029BB RID: 10683 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public CharacterControllerAbstract()
	{
	}
}
