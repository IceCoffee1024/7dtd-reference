using System;
using UnityEngine;

// Token: 0x02001637 RID: 5687
public class vp_3rdPersonWeaponAim : MonoBehaviour
{
	// Token: 0x170014E3 RID: 5347
	// (get) Token: 0x0600B1C6 RID: 45510 RVA: 0x004263E0 File Offset: 0x004245E0
	public Transform Transform
	{
		get
		{
			if (this.m_Transform == null)
			{
				this.m_Transform = base.transform;
			}
			return this.m_Transform;
		}
	}

	// Token: 0x170014E4 RID: 5348
	// (get) Token: 0x0600B1C7 RID: 45511 RVA: 0x00426402 File Offset: 0x00424602
	public vp_PlayerEventHandler Player
	{
		get
		{
			if (this.m_Player == null)
			{
				this.m_Player = (vp_PlayerEventHandler)this.Root.GetComponentInChildren(typeof(vp_PlayerEventHandler));
			}
			return this.m_Player;
		}
	}

	// Token: 0x170014E5 RID: 5349
	// (get) Token: 0x0600B1C8 RID: 45512 RVA: 0x00426438 File Offset: 0x00424638
	public vp_WeaponHandler WeaponHandler
	{
		get
		{
			if (this.m_WeaponHandler == null)
			{
				this.m_WeaponHandler = (vp_WeaponHandler)this.Root.GetComponentInChildren(typeof(vp_WeaponHandler));
			}
			return this.m_WeaponHandler;
		}
	}

	// Token: 0x170014E6 RID: 5350
	// (get) Token: 0x0600B1C9 RID: 45513 RVA: 0x0042646E File Offset: 0x0042466E
	public Animator Animator
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Animator == null)
			{
				this.m_Animator = this.Root.GetComponentInChildren<Animator>();
			}
			return this.m_Animator;
		}
	}

	// Token: 0x170014E7 RID: 5351
	// (get) Token: 0x0600B1CA RID: 45514 RVA: 0x00426495 File Offset: 0x00424695
	public Transform Root
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.m_Root == null)
			{
				this.m_Root = this.Transform.root;
			}
			return this.m_Root;
		}
	}

	// Token: 0x170014E8 RID: 5352
	// (get) Token: 0x0600B1CB RID: 45515 RVA: 0x004264BC File Offset: 0x004246BC
	public Transform LowerArmObj
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.m_LowerArmObj == null)
			{
				this.m_LowerArmObj = this.HandObj.parent;
			}
			return this.m_LowerArmObj;
		}
	}

	// Token: 0x170014E9 RID: 5353
	// (get) Token: 0x0600B1CC RID: 45516 RVA: 0x004264E4 File Offset: 0x004246E4
	public Transform HandObj
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (this.m_HandObj == null)
			{
				if (this.Hand != null)
				{
					this.m_HandObj = this.Hand.transform;
				}
				else
				{
					this.m_HandObj = vp_Utility.GetTransformByNameInAncestors(this.Transform, "hand", true, true);
					if (this.m_HandObj == null && this.Transform.parent != null)
					{
						this.m_HandObj = this.Transform.parent;
					}
					if (this.m_HandObj != null)
					{
						this.Hand = this.m_HandObj.gameObject;
					}
				}
			}
			return this.m_HandObj;
		}
	}

	// Token: 0x0600B1CD RID: 45517 RVA: 0x00426592 File Offset: 0x00424792
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B1CE RID: 45518 RVA: 0x004265AE File Offset: 0x004247AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B1CF RID: 45519 RVA: 0x004265CC File Offset: 0x004247CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_DefaultRotation = this.Transform.localRotation;
		if (this.LowerArmObj == null || this.HandObj == null)
		{
			Debug.LogError("Hierarchy Error (" + ((this != null) ? this.ToString() : null) + ") This script should be placed on a 3rd person weapon gameobject childed to a hand bone in a rigged character.");
			base.enabled = false;
			return;
		}
		Quaternion quaternion = Quaternion.Inverse(this.LowerArmObj.rotation);
		this.m_ReferenceLookDir = quaternion * this.Root.rotation * Vector3.forward;
		this.m_ReferenceUpDir = quaternion * this.Root.rotation * Vector3.up;
		Quaternion rotation = this.HandObj.rotation;
		this.HandObj.rotation = this.Root.rotation;
		Quaternion rotation2 = this.HandObj.rotation;
		this.HandObj.rotation = rotation;
		this.m_HandBoneRotDif = Quaternion.Inverse(rotation2) * rotation;
	}

	// Token: 0x0600B1D0 RID: 45520 RVA: 0x004266CE File Offset: 0x004248CE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.UpdateAiming();
	}

	// Token: 0x0600B1D1 RID: 45521 RVA: 0x004266E4 File Offset: 0x004248E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateAiming()
	{
		if (this.Animator == null)
		{
			return;
		}
		if ((!this.Animator.GetBool("IsAttacking") && !this.Animator.GetBool("IsZooming")) || this.Animator.GetBool("IsReloading") || this.Animator.GetBool("IsOutOfControl") || this.Player.CurrentWeaponIndex.Get() == 0)
		{
			this.Transform.localRotation = this.m_DefaultRotation;
			return;
		}
		Quaternion rotation = this.Transform.rotation;
		this.Transform.rotation = Quaternion.LookRotation(this.Player.AimDirection.Get());
		this.m_WorldDir = this.Transform.forward;
		this.Transform.rotation = rotation;
		this.HandObj.rotation = vp_3DUtility.GetBoneLookRotationInWorldSpace(this.HandObj.rotation, this.LowerArmObj.rotation, this.m_WorldDir, 1f, this.m_ReferenceUpDir, this.m_ReferenceLookDir, this.m_HandBoneRotDif);
		this.HandObj.Rotate(this.Transform.forward, this.AngleAdjustZ + this.WeaponHandler.CurrentWeapon.Recoil.z * this.RecoilFactorZ, Space.World);
		this.HandObj.Rotate(this.Transform.up, this.AngleAdjustY + this.WeaponHandler.CurrentWeapon.Recoil.y * this.RecoilFactorY, Space.World);
		this.HandObj.Rotate(this.Transform.right, this.AngleAdjustX + this.WeaponHandler.CurrentWeapon.Recoil.x * this.RecoilFactorX, Space.World);
	}

	// Token: 0x04008521 RID: 34081
	public GameObject Hand;

	// Token: 0x04008522 RID: 34082
	[Range(0f, 360f)]
	public float AngleAdjustX;

	// Token: 0x04008523 RID: 34083
	[Range(0f, 360f)]
	public float AngleAdjustY;

	// Token: 0x04008524 RID: 34084
	[Range(0f, 360f)]
	public float AngleAdjustZ;

	// Token: 0x04008525 RID: 34085
	[Range(0f, 5f)]
	public float RecoilFactorX = 1f;

	// Token: 0x04008526 RID: 34086
	[Range(0f, 5f)]
	public float RecoilFactorY = 1f;

	// Token: 0x04008527 RID: 34087
	[Range(0f, 5f)]
	public float RecoilFactorZ = 1f;

	// Token: 0x04008528 RID: 34088
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_DefaultRotation;

	// Token: 0x04008529 RID: 34089
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ReferenceUpDir;

	// Token: 0x0400852A RID: 34090
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ReferenceLookDir;

	// Token: 0x0400852B RID: 34091
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_HandBoneRotDif;

	// Token: 0x0400852C RID: 34092
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_WorldDir = Vector3.zero;

	// Token: 0x0400852D RID: 34093
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x0400852E RID: 34094
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x0400852F RID: 34095
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x04008530 RID: 34096
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x04008531 RID: 34097
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Root;

	// Token: 0x04008532 RID: 34098
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_LowerArmObj;

	// Token: 0x04008533 RID: 34099
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_HandObj;
}
