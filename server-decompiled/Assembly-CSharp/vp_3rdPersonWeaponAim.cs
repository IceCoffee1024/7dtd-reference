using System;
using UnityEngine;

// Token: 0x02001606 RID: 5638
public class vp_3rdPersonWeaponAim : MonoBehaviour
{
	// Token: 0x17001494 RID: 5268
	// (get) Token: 0x0600B0DC RID: 45276 RVA: 0x00420300 File Offset: 0x0041E500
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

	// Token: 0x17001495 RID: 5269
	// (get) Token: 0x0600B0DD RID: 45277 RVA: 0x00420322 File Offset: 0x0041E522
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

	// Token: 0x17001496 RID: 5270
	// (get) Token: 0x0600B0DE RID: 45278 RVA: 0x00420358 File Offset: 0x0041E558
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

	// Token: 0x17001497 RID: 5271
	// (get) Token: 0x0600B0DF RID: 45279 RVA: 0x0042038E File Offset: 0x0041E58E
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

	// Token: 0x17001498 RID: 5272
	// (get) Token: 0x0600B0E0 RID: 45280 RVA: 0x004203B5 File Offset: 0x0041E5B5
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

	// Token: 0x17001499 RID: 5273
	// (get) Token: 0x0600B0E1 RID: 45281 RVA: 0x004203DC File Offset: 0x0041E5DC
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

	// Token: 0x1700149A RID: 5274
	// (get) Token: 0x0600B0E2 RID: 45282 RVA: 0x00420404 File Offset: 0x0041E604
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

	// Token: 0x0600B0E3 RID: 45283 RVA: 0x004204B2 File Offset: 0x0041E6B2
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B0E4 RID: 45284 RVA: 0x004204CE File Offset: 0x0041E6CE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B0E5 RID: 45285 RVA: 0x004204EC File Offset: 0x0041E6EC
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

	// Token: 0x0600B0E6 RID: 45286 RVA: 0x004205EE File Offset: 0x0041E7EE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.UpdateAiming();
	}

	// Token: 0x0600B0E7 RID: 45287 RVA: 0x00420604 File Offset: 0x0041E804
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

	// Token: 0x0400848A RID: 33930
	public GameObject Hand;

	// Token: 0x0400848B RID: 33931
	[Range(0f, 360f)]
	public float AngleAdjustX;

	// Token: 0x0400848C RID: 33932
	[Range(0f, 360f)]
	public float AngleAdjustY;

	// Token: 0x0400848D RID: 33933
	[Range(0f, 360f)]
	public float AngleAdjustZ;

	// Token: 0x0400848E RID: 33934
	[Range(0f, 5f)]
	public float RecoilFactorX = 1f;

	// Token: 0x0400848F RID: 33935
	[Range(0f, 5f)]
	public float RecoilFactorY = 1f;

	// Token: 0x04008490 RID: 33936
	[Range(0f, 5f)]
	public float RecoilFactorZ = 1f;

	// Token: 0x04008491 RID: 33937
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_DefaultRotation;

	// Token: 0x04008492 RID: 33938
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ReferenceUpDir;

	// Token: 0x04008493 RID: 33939
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ReferenceLookDir;

	// Token: 0x04008494 RID: 33940
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_HandBoneRotDif;

	// Token: 0x04008495 RID: 33941
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_WorldDir = Vector3.zero;

	// Token: 0x04008496 RID: 33942
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x04008497 RID: 33943
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008498 RID: 33944
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x04008499 RID: 33945
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x0400849A RID: 33946
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Root;

	// Token: 0x0400849B RID: 33947
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_LowerArmObj;

	// Token: 0x0400849C RID: 33948
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_HandObj;
}
