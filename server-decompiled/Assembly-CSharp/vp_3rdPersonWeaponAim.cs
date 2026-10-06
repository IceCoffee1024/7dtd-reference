using System;
using UnityEngine;

// Token: 0x02001625 RID: 5669
public class vp_3rdPersonWeaponAim : MonoBehaviour
{
	// Token: 0x170014D7 RID: 5335
	// (get) Token: 0x0600B16E RID: 45422 RVA: 0x00423FDC File Offset: 0x004221DC
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

	// Token: 0x170014D8 RID: 5336
	// (get) Token: 0x0600B16F RID: 45423 RVA: 0x00423FFE File Offset: 0x004221FE
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

	// Token: 0x170014D9 RID: 5337
	// (get) Token: 0x0600B170 RID: 45424 RVA: 0x00424034 File Offset: 0x00422234
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

	// Token: 0x170014DA RID: 5338
	// (get) Token: 0x0600B171 RID: 45425 RVA: 0x0042406A File Offset: 0x0042226A
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

	// Token: 0x170014DB RID: 5339
	// (get) Token: 0x0600B172 RID: 45426 RVA: 0x00424091 File Offset: 0x00422291
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

	// Token: 0x170014DC RID: 5340
	// (get) Token: 0x0600B173 RID: 45427 RVA: 0x004240B8 File Offset: 0x004222B8
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

	// Token: 0x170014DD RID: 5341
	// (get) Token: 0x0600B174 RID: 45428 RVA: 0x004240E0 File Offset: 0x004222E0
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

	// Token: 0x0600B175 RID: 45429 RVA: 0x0042418E File Offset: 0x0042238E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B176 RID: 45430 RVA: 0x004241AA File Offset: 0x004223AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B177 RID: 45431 RVA: 0x004241C8 File Offset: 0x004223C8
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

	// Token: 0x0600B178 RID: 45432 RVA: 0x004242CA File Offset: 0x004224CA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.UpdateAiming();
	}

	// Token: 0x0600B179 RID: 45433 RVA: 0x004242E0 File Offset: 0x004224E0
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

	// Token: 0x040084E0 RID: 34016
	public GameObject Hand;

	// Token: 0x040084E1 RID: 34017
	[Range(0f, 360f)]
	public float AngleAdjustX;

	// Token: 0x040084E2 RID: 34018
	[Range(0f, 360f)]
	public float AngleAdjustY;

	// Token: 0x040084E3 RID: 34019
	[Range(0f, 360f)]
	public float AngleAdjustZ;

	// Token: 0x040084E4 RID: 34020
	[Range(0f, 5f)]
	public float RecoilFactorX = 1f;

	// Token: 0x040084E5 RID: 34021
	[Range(0f, 5f)]
	public float RecoilFactorY = 1f;

	// Token: 0x040084E6 RID: 34022
	[Range(0f, 5f)]
	public float RecoilFactorZ = 1f;

	// Token: 0x040084E7 RID: 34023
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_DefaultRotation;

	// Token: 0x040084E8 RID: 34024
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ReferenceUpDir;

	// Token: 0x040084E9 RID: 34025
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ReferenceLookDir;

	// Token: 0x040084EA RID: 34026
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_HandBoneRotDif;

	// Token: 0x040084EB RID: 34027
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_WorldDir = Vector3.zero;

	// Token: 0x040084EC RID: 34028
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040084ED RID: 34029
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040084EE RID: 34030
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x040084EF RID: 34031
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x040084F0 RID: 34032
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Root;

	// Token: 0x040084F1 RID: 34033
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_LowerArmObj;

	// Token: 0x040084F2 RID: 34034
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_HandObj;
}
