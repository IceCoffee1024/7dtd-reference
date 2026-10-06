using System;
using UnityEngine;

// Token: 0x0200162F RID: 5679
public class vp_Weapon : vp_Component
{
	// Token: 0x17001503 RID: 5379
	// (get) Token: 0x0600B20A RID: 45578 RVA: 0x00427CA2 File Offset: 0x00425EA2
	// (set) Token: 0x0600B20B RID: 45579 RVA: 0x00427CB4 File Offset: 0x00425EB4
	public bool Wielded
	{
		get
		{
			return this.m_Wielded && base.Rendering;
		}
		set
		{
			this.m_Wielded = value;
		}
	}

	// Token: 0x17001504 RID: 5380
	// (get) Token: 0x0600B20C RID: 45580 RVA: 0x00427CBD File Offset: 0x00425EBD
	public vp_PlayerEventHandler Player
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Player == null && base.EventHandler != null)
			{
				this.m_Player = (vp_PlayerEventHandler)base.EventHandler;
			}
			return this.m_Player;
		}
	}

	// Token: 0x0600B20D RID: 45581 RVA: 0x00427CF4 File Offset: 0x00425EF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.RotationOffset = base.transform.localEulerAngles;
		this.PositionOffset = base.transform.position;
		base.Transform.localEulerAngles = this.RotationOffset;
		if (base.transform.parent == null)
		{
			Debug.LogError("Error (" + ((this != null) ? this.ToString() : null) + ") Must not be placed in scene root. Disabling self.");
			vp_Utility.Activate(base.gameObject, false);
			return;
		}
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = false;
		}
	}

	// Token: 0x0600B20E RID: 45582 RVA: 0x00427D95 File Offset: 0x00425F95
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		this.RefreshWeaponModel();
		base.OnEnable();
	}

	// Token: 0x0600B20F RID: 45583 RVA: 0x00427DA3 File Offset: 0x00425FA3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		this.RefreshWeaponModel();
		this.Activate3rdPersonModel(false);
		base.OnDisable();
	}

	// Token: 0x17001505 RID: 5381
	// (get) Token: 0x0600B210 RID: 45584 RVA: 0x00427DB8 File Offset: 0x00425FB8
	// (set) Token: 0x0600B211 RID: 45585 RVA: 0x00427DC0 File Offset: 0x00425FC0
	public Vector3 RotationSpring2DefaultRotation
	{
		get
		{
			return this.m_RotationSpring2DefaultRotation;
		}
		set
		{
			this.m_RotationSpring2DefaultRotation = value;
		}
	}

	// Token: 0x0600B212 RID: 45586 RVA: 0x00427DCC File Offset: 0x00425FCC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Start()
	{
		base.Start();
		this.m_PositionSpring2 = new vp_Spring(base.transform, vp_Spring.UpdateMode.PositionAdditiveSelf, true);
		this.m_PositionSpring2.MinVelocity = 1E-05f;
		this.m_RotationSpring2 = new vp_Spring(base.transform, vp_Spring.UpdateMode.RotationAdditiveGlobal, true);
		this.m_RotationSpring2.MinVelocity = 1E-05f;
		this.SnapSprings();
		this.Refresh();
		base.CacheRenderers();
	}

	// Token: 0x17001506 RID: 5382
	// (get) Token: 0x0600B213 RID: 45587 RVA: 0x00427E37 File Offset: 0x00426037
	public Vector3 Recoil
	{
		get
		{
			return this.m_RotationSpring2.State;
		}
	}

	// Token: 0x0600B214 RID: 45588 RVA: 0x00427E44 File Offset: 0x00426044
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (Time.timeScale == 0f)
		{
			return;
		}
		this.UpdateSprings();
	}

	// Token: 0x0600B215 RID: 45589 RVA: 0x00427E5F File Offset: 0x0042605F
	public virtual void AddForce2(Vector3 positional, Vector3 angular)
	{
		this.m_PositionSpring2.AddForce(positional);
		this.m_RotationSpring2.AddForce(angular);
	}

	// Token: 0x0600B216 RID: 45590 RVA: 0x00427E79 File Offset: 0x00426079
	public virtual void AddForce2(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
	{
		this.AddForce2(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot));
	}

	// Token: 0x0600B217 RID: 45591 RVA: 0x00427E94 File Offset: 0x00426094
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSprings()
	{
		base.Transform.localPosition = Vector3.up;
		base.Transform.localRotation = Quaternion.identity;
		this.m_PositionSpring2.FixedUpdate();
		this.m_RotationSpring2.FixedUpdate();
	}

	// Token: 0x0600B218 RID: 45592 RVA: 0x00427ECC File Offset: 0x004260CC
	public override void Refresh()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.Stiffness = new Vector3(this.PositionSpring2Stiffness, this.PositionSpring2Stiffness, this.PositionSpring2Stiffness);
			this.m_PositionSpring2.Damping = Vector3.one - new Vector3(this.PositionSpring2Damping, this.PositionSpring2Damping, this.PositionSpring2Damping);
			this.m_PositionSpring2.RestState = Vector3.zero;
		}
		if (this.m_RotationSpring2 != null)
		{
			this.m_RotationSpring2.Stiffness = new Vector3(this.RotationSpring2Stiffness, this.RotationSpring2Stiffness, this.RotationSpring2Stiffness);
			this.m_RotationSpring2.Damping = Vector3.one - new Vector3(this.RotationSpring2Damping, this.RotationSpring2Damping, this.RotationSpring2Damping);
			this.m_RotationSpring2.RestState = this.m_RotationSpring2DefaultRotation;
		}
	}

	// Token: 0x0600B219 RID: 45593 RVA: 0x00427FAE File Offset: 0x004261AE
	public override void Activate()
	{
		base.Activate();
		this.m_Wielded = true;
		base.Rendering = true;
	}

	// Token: 0x0600B21A RID: 45594 RVA: 0x00427FC4 File Offset: 0x004261C4
	public virtual void SnapSprings()
	{
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.RestState = Vector3.zero;
			this.m_PositionSpring2.State = Vector3.zero;
			this.m_PositionSpring2.Stop(true);
		}
		if (this.m_RotationSpring2 != null)
		{
			this.m_RotationSpring2.RestState = this.m_RotationSpring2DefaultRotation;
			this.m_RotationSpring2.State = this.m_RotationSpring2DefaultRotation;
			this.m_RotationSpring2.Stop(true);
		}
	}

	// Token: 0x0600B21B RID: 45595 RVA: 0x0042803B File Offset: 0x0042623B
	public virtual void StopSprings()
	{
		if (this.m_PositionSpring2 != null)
		{
			this.m_PositionSpring2.Stop(true);
		}
		if (this.m_RotationSpring2 != null)
		{
			this.m_RotationSpring2.Stop(true);
		}
	}

	// Token: 0x0600B21C RID: 45596 RVA: 0x00428065 File Offset: 0x00426265
	public virtual void Wield(bool isWielding = true)
	{
		this.m_Wielded = isWielding;
		this.Refresh();
		base.StateManager.CombineStates();
	}

	// Token: 0x0600B21D RID: 45597 RVA: 0x0042807F File Offset: 0x0042627F
	public virtual void RefreshWeaponModel()
	{
		if (this.Player == null)
		{
			return;
		}
		vp_Value<bool> isFirstPerson = this.Player.IsFirstPerson;
	}

	// Token: 0x0600B21E RID: 45598 RVA: 0x0042809C File Offset: 0x0042629C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Activate3rdPersonModel(bool active = true)
	{
		if (this.Weapon3rdPersonModel == null)
		{
			return;
		}
		if (active)
		{
			this.Weapon3rdPersonModel.GetComponent<Renderer>().enabled = true;
			vp_Utility.Activate(this.Weapon3rdPersonModel, true);
			return;
		}
		this.Weapon3rdPersonModel.GetComponent<Renderer>().enabled = false;
		vp_Timer.In(0.1f, delegate
		{
			if (this.Weapon3rdPersonModel != null)
			{
				vp_Utility.Activate(this.Weapon3rdPersonModel, false);
			}
		}, this.m_Weapon3rdPersonModelWakeUpTimer);
	}

	// Token: 0x0600B21F RID: 45599 RVA: 0x00428106 File Offset: 0x00426306
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		base.Rendering = false;
	}

	// Token: 0x0600B220 RID: 45600 RVA: 0x00428127 File Offset: 0x00426327
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		base.Rendering = true;
	}

	// Token: 0x17001507 RID: 5383
	// (get) Token: 0x0600B221 RID: 45601 RVA: 0x00428148 File Offset: 0x00426348
	public virtual Vector3 OnValue_AimDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Weapon3rdPersonModel.transform.position - this.Player.LookPoint.Get()).normalized;
		}
	}

	// Token: 0x040085B4 RID: 34228
	public GameObject Weapon3rdPersonModel;

	// Token: 0x040085B5 RID: 34229
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_WeaponModel;

	// Token: 0x040085B6 RID: 34230
	public Vector3 PositionOffset = new Vector3(0.15f, -0.15f, -0.15f);

	// Token: 0x040085B7 RID: 34231
	public Vector3 AimingPositionOffset = new Vector3(0f, 0f, 0f);

	// Token: 0x040085B8 RID: 34232
	public float PositionSpring2Stiffness = 0.95f;

	// Token: 0x040085B9 RID: 34233
	public float PositionSpring2Damping = 0.25f;

	// Token: 0x040085BA RID: 34234
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring2;

	// Token: 0x040085BB RID: 34235
	public Vector3 RotationOffset = Vector3.zero;

	// Token: 0x040085BC RID: 34236
	public float RotationSpring2Stiffness = 0.95f;

	// Token: 0x040085BD RID: 34237
	public float RotationSpring2Damping = 0.25f;

	// Token: 0x040085BE RID: 34238
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationSpring2;

	// Token: 0x040085BF RID: 34239
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Wielded = true;

	// Token: 0x040085C0 RID: 34240
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Weapon3rdPersonModelWakeUpTimer = new vp_Timer.Handle();

	// Token: 0x040085C1 RID: 34241
	public int AnimationType = 1;

	// Token: 0x040085C2 RID: 34242
	public int AnimationGrip = 1;

	// Token: 0x040085C3 RID: 34243
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040085C4 RID: 34244
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_RotationSpring2DefaultRotation = Vector3.zero;

	// Token: 0x02001630 RID: 5680
	public new enum Type
	{
		// Token: 0x040085C6 RID: 34246
		Custom,
		// Token: 0x040085C7 RID: 34247
		Firearm,
		// Token: 0x040085C8 RID: 34248
		Melee,
		// Token: 0x040085C9 RID: 34249
		Thrown
	}

	// Token: 0x02001631 RID: 5681
	public enum Grip
	{
		// Token: 0x040085CB RID: 34251
		Custom,
		// Token: 0x040085CC RID: 34252
		OneHanded,
		// Token: 0x040085CD RID: 34253
		TwoHanded
	}
}
