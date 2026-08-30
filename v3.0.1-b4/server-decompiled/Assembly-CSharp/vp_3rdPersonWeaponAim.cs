using System;
using UnityEngine;

// Token: 0x020015EB RID: 5611
public class vp_3rdPersonWeaponAim : MonoBehaviour
{
	// Token: 0x1700145B RID: 5211
	// (get) Token: 0x0600AFDF RID: 45023 RVA: 0x0041AFC0 File Offset: 0x004191C0
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

	// Token: 0x1700145C RID: 5212
	// (get) Token: 0x0600AFE0 RID: 45024 RVA: 0x0041AFE2 File Offset: 0x004191E2
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

	// Token: 0x1700145D RID: 5213
	// (get) Token: 0x0600AFE1 RID: 45025 RVA: 0x0041B018 File Offset: 0x00419218
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

	// Token: 0x1700145E RID: 5214
	// (get) Token: 0x0600AFE2 RID: 45026 RVA: 0x0041B04E File Offset: 0x0041924E
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

	// Token: 0x1700145F RID: 5215
	// (get) Token: 0x0600AFE3 RID: 45027 RVA: 0x0041B075 File Offset: 0x00419275
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

	// Token: 0x17001460 RID: 5216
	// (get) Token: 0x0600AFE4 RID: 45028 RVA: 0x0041B09C File Offset: 0x0041929C
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

	// Token: 0x17001461 RID: 5217
	// (get) Token: 0x0600AFE5 RID: 45029 RVA: 0x0041B0C4 File Offset: 0x004192C4
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

	// Token: 0x0600AFE6 RID: 45030 RVA: 0x0041B172 File Offset: 0x00419372
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600AFE7 RID: 45031 RVA: 0x0041B18E File Offset: 0x0041938E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600AFE8 RID: 45032 RVA: 0x0041B1AC File Offset: 0x004193AC
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

	// Token: 0x0600AFE9 RID: 45033 RVA: 0x0041B2AE File Offset: 0x004194AE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void LateUpdate()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.UpdateAiming();
	}

	// Token: 0x0600AFEA RID: 45034 RVA: 0x0041B2C4 File Offset: 0x004194C4
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

	// Token: 0x040083B1 RID: 33713
	public GameObject Hand;

	// Token: 0x040083B2 RID: 33714
	[Range(0f, 360f)]
	public float AngleAdjustX;

	// Token: 0x040083B3 RID: 33715
	[Range(0f, 360f)]
	public float AngleAdjustY;

	// Token: 0x040083B4 RID: 33716
	[Range(0f, 360f)]
	public float AngleAdjustZ;

	// Token: 0x040083B5 RID: 33717
	[Range(0f, 5f)]
	public float RecoilFactorX = 1f;

	// Token: 0x040083B6 RID: 33718
	[Range(0f, 5f)]
	public float RecoilFactorY = 1f;

	// Token: 0x040083B7 RID: 33719
	[Range(0f, 5f)]
	public float RecoilFactorZ = 1f;

	// Token: 0x040083B8 RID: 33720
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_DefaultRotation;

	// Token: 0x040083B9 RID: 33721
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ReferenceUpDir;

	// Token: 0x040083BA RID: 33722
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_ReferenceLookDir;

	// Token: 0x040083BB RID: 33723
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Quaternion m_HandBoneRotDif;

	// Token: 0x040083BC RID: 33724
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_WorldDir = Vector3.zero;

	// Token: 0x040083BD RID: 33725
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040083BE RID: 33726
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040083BF RID: 33727
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x040083C0 RID: 33728
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Animator m_Animator;

	// Token: 0x040083C1 RID: 33729
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_Root;

	// Token: 0x040083C2 RID: 33730
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_LowerArmObj;

	// Token: 0x040083C3 RID: 33731
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Transform m_HandObj;
}
