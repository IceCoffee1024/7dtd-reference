using System;
using UnityEngine;

// Token: 0x02001610 RID: 5648
public class vp_Weapon : vp_Component
{
	// Token: 0x170014C0 RID: 5312
	// (get) Token: 0x0600B178 RID: 45432 RVA: 0x00423FC6 File Offset: 0x004221C6
	// (set) Token: 0x0600B179 RID: 45433 RVA: 0x00423FD8 File Offset: 0x004221D8
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

	// Token: 0x170014C1 RID: 5313
	// (get) Token: 0x0600B17A RID: 45434 RVA: 0x00423FE1 File Offset: 0x004221E1
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

	// Token: 0x0600B17B RID: 45435 RVA: 0x00424018 File Offset: 0x00422218
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

	// Token: 0x0600B17C RID: 45436 RVA: 0x004240B9 File Offset: 0x004222B9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		this.RefreshWeaponModel();
		base.OnEnable();
	}

	// Token: 0x0600B17D RID: 45437 RVA: 0x004240C7 File Offset: 0x004222C7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		this.RefreshWeaponModel();
		this.Activate3rdPersonModel(false);
		base.OnDisable();
	}

	// Token: 0x170014C2 RID: 5314
	// (get) Token: 0x0600B17E RID: 45438 RVA: 0x004240DC File Offset: 0x004222DC
	// (set) Token: 0x0600B17F RID: 45439 RVA: 0x004240E4 File Offset: 0x004222E4
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

	// Token: 0x0600B180 RID: 45440 RVA: 0x004240F0 File Offset: 0x004222F0
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

	// Token: 0x170014C3 RID: 5315
	// (get) Token: 0x0600B181 RID: 45441 RVA: 0x0042415B File Offset: 0x0042235B
	public Vector3 Recoil
	{
		get
		{
			return this.m_RotationSpring2.State;
		}
	}

	// Token: 0x0600B182 RID: 45442 RVA: 0x00424168 File Offset: 0x00422368
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

	// Token: 0x0600B183 RID: 45443 RVA: 0x00424183 File Offset: 0x00422383
	public virtual void AddForce2(Vector3 positional, Vector3 angular)
	{
		this.m_PositionSpring2.AddForce(positional);
		this.m_RotationSpring2.AddForce(angular);
	}

	// Token: 0x0600B184 RID: 45444 RVA: 0x0042419D File Offset: 0x0042239D
	public virtual void AddForce2(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
	{
		this.AddForce2(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot));
	}

	// Token: 0x0600B185 RID: 45445 RVA: 0x004241B8 File Offset: 0x004223B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSprings()
	{
		base.Transform.localPosition = Vector3.up;
		base.Transform.localRotation = Quaternion.identity;
		this.m_PositionSpring2.FixedUpdate();
		this.m_RotationSpring2.FixedUpdate();
	}

	// Token: 0x0600B186 RID: 45446 RVA: 0x004241F0 File Offset: 0x004223F0
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

	// Token: 0x0600B187 RID: 45447 RVA: 0x004242D2 File Offset: 0x004224D2
	public override void Activate()
	{
		base.Activate();
		this.m_Wielded = true;
		base.Rendering = true;
	}

	// Token: 0x0600B188 RID: 45448 RVA: 0x004242E8 File Offset: 0x004224E8
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

	// Token: 0x0600B189 RID: 45449 RVA: 0x0042435F File Offset: 0x0042255F
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

	// Token: 0x0600B18A RID: 45450 RVA: 0x00424389 File Offset: 0x00422589
	public virtual void Wield(bool isWielding = true)
	{
		this.m_Wielded = isWielding;
		this.Refresh();
		base.StateManager.CombineStates();
	}

	// Token: 0x0600B18B RID: 45451 RVA: 0x004243A3 File Offset: 0x004225A3
	public virtual void RefreshWeaponModel()
	{
		if (this.Player == null)
		{
			return;
		}
		vp_Value<bool> isFirstPerson = this.Player.IsFirstPerson;
	}

	// Token: 0x0600B18C RID: 45452 RVA: 0x004243C0 File Offset: 0x004225C0
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

	// Token: 0x0600B18D RID: 45453 RVA: 0x0042442A File Offset: 0x0042262A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		base.Rendering = false;
	}

	// Token: 0x0600B18E RID: 45454 RVA: 0x0042444B File Offset: 0x0042264B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		base.Rendering = true;
	}

	// Token: 0x170014C4 RID: 5316
	// (get) Token: 0x0600B18F RID: 45455 RVA: 0x0042446C File Offset: 0x0042266C
	public virtual Vector3 OnValue_AimDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Weapon3rdPersonModel.transform.position - this.Player.LookPoint.Get()).normalized;
		}
	}

	// Token: 0x0400855E RID: 34142
	public GameObject Weapon3rdPersonModel;

	// Token: 0x0400855F RID: 34143
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_WeaponModel;

	// Token: 0x04008560 RID: 34144
	public Vector3 PositionOffset = new Vector3(0.15f, -0.15f, -0.15f);

	// Token: 0x04008561 RID: 34145
	public Vector3 AimingPositionOffset = new Vector3(0f, 0f, 0f);

	// Token: 0x04008562 RID: 34146
	public float PositionSpring2Stiffness = 0.95f;

	// Token: 0x04008563 RID: 34147
	public float PositionSpring2Damping = 0.25f;

	// Token: 0x04008564 RID: 34148
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring2;

	// Token: 0x04008565 RID: 34149
	public Vector3 RotationOffset = Vector3.zero;

	// Token: 0x04008566 RID: 34150
	public float RotationSpring2Stiffness = 0.95f;

	// Token: 0x04008567 RID: 34151
	public float RotationSpring2Damping = 0.25f;

	// Token: 0x04008568 RID: 34152
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationSpring2;

	// Token: 0x04008569 RID: 34153
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Wielded = true;

	// Token: 0x0400856A RID: 34154
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Weapon3rdPersonModelWakeUpTimer = new vp_Timer.Handle();

	// Token: 0x0400856B RID: 34155
	public int AnimationType = 1;

	// Token: 0x0400856C RID: 34156
	public int AnimationGrip = 1;

	// Token: 0x0400856D RID: 34157
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x0400856E RID: 34158
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_RotationSpring2DefaultRotation = Vector3.zero;

	// Token: 0x02001611 RID: 5649
	public new enum Type
	{
		// Token: 0x04008570 RID: 34160
		Custom,
		// Token: 0x04008571 RID: 34161
		Firearm,
		// Token: 0x04008572 RID: 34162
		Melee,
		// Token: 0x04008573 RID: 34163
		Thrown
	}

	// Token: 0x02001612 RID: 5650
	public enum Grip
	{
		// Token: 0x04008575 RID: 34165
		Custom,
		// Token: 0x04008576 RID: 34166
		OneHanded,
		// Token: 0x04008577 RID: 34167
		TwoHanded
	}
}
