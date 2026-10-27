using System;
using UnityEngine;

// Token: 0x02000507 RID: 1287
public abstract class CharacterControllerAbstract
{
	// Token: 0x060029CE RID: 10702
	public abstract void Enable(bool isEnabled);

	// Token: 0x060029CF RID: 10703
	public abstract void SetStepOffset(float _stepOffset);

	// Token: 0x060029D0 RID: 10704
	public abstract float GetStepOffset();

	// Token: 0x060029D1 RID: 10705
	public abstract void SetSize(Vector3 _center, float _height, float _radius);

	// Token: 0x060029D2 RID: 10706
	public abstract void SetCenter(Vector3 _center);

	// Token: 0x060029D3 RID: 10707
	public abstract Vector3 GetCenter();

	// Token: 0x060029D4 RID: 10708
	public abstract void SetRadius(float _radius);

	// Token: 0x060029D5 RID: 10709
	public abstract float GetRadius();

	// Token: 0x060029D6 RID: 10710
	public abstract void SetSkinWidth(float _width);

	// Token: 0x060029D7 RID: 10711
	public abstract float GetSkinWidth();

	// Token: 0x060029D8 RID: 10712
	public abstract void SetHeight(float _height);

	// Token: 0x060029D9 RID: 10713
	public abstract float GetHeight();

	// Token: 0x060029DA RID: 10714
	public abstract bool IsGrounded();

	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x060029DB RID: 10715 RVA: 0x00108406 File Offset: 0x00106606
	public virtual Vector3 GroundNormal
	{
		get
		{
			return Vector3.up;
		}
	}

	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x060029DC RID: 10716
	// (set) Token: 0x060029DD RID: 10717
	public abstract bool enableOverlapRecovery { get; set; }

	// Token: 0x060029DE RID: 10718
	public abstract CollisionFlags Move(Vector3 _dir);

	// Token: 0x060029DF RID: 10719 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual CollisionFlags Update()
	{
		return CollisionFlags.None;
	}

	// Token: 0x060029E0 RID: 10720
	public abstract void Rotate(Quaternion _dir);

	// Token: 0x060029E1 RID: 10721 RVA: 0x0000640C File Offset: 0x0000460C
	[PublicizedFrom(EAccessModifier.Protected)]
	public CharacterControllerAbstract()
	{
	}
}
