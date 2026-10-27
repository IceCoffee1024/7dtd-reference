using System;
using UnityEngine;

// Token: 0x02001641 RID: 5697
public class vp_Weapon : vp_Component
{
	// Token: 0x1700150F RID: 5391
	// (get) Token: 0x0600B262 RID: 45666 RVA: 0x0042A0A6 File Offset: 0x004282A6
	// (set) Token: 0x0600B263 RID: 45667 RVA: 0x0042A0B8 File Offset: 0x004282B8
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

	// Token: 0x17001510 RID: 5392
	// (get) Token: 0x0600B264 RID: 45668 RVA: 0x0042A0C1 File Offset: 0x004282C1
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

	// Token: 0x0600B265 RID: 45669 RVA: 0x0042A0F8 File Offset: 0x004282F8
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

	// Token: 0x0600B266 RID: 45670 RVA: 0x0042A199 File Offset: 0x00428399
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		this.RefreshWeaponModel();
		base.OnEnable();
	}

	// Token: 0x0600B267 RID: 45671 RVA: 0x0042A1A7 File Offset: 0x004283A7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		this.RefreshWeaponModel();
		this.Activate3rdPersonModel(false);
		base.OnDisable();
	}

	// Token: 0x17001511 RID: 5393
	// (get) Token: 0x0600B268 RID: 45672 RVA: 0x0042A1BC File Offset: 0x004283BC
	// (set) Token: 0x0600B269 RID: 45673 RVA: 0x0042A1C4 File Offset: 0x004283C4
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

	// Token: 0x0600B26A RID: 45674 RVA: 0x0042A1D0 File Offset: 0x004283D0
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

	// Token: 0x17001512 RID: 5394
	// (get) Token: 0x0600B26B RID: 45675 RVA: 0x0042A23B File Offset: 0x0042843B
	public Vector3 Recoil
	{
		get
		{
			return this.m_RotationSpring2.State;
		}
	}

	// Token: 0x0600B26C RID: 45676 RVA: 0x0042A248 File Offset: 0x00428448
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

	// Token: 0x0600B26D RID: 45677 RVA: 0x0042A263 File Offset: 0x00428463
	public virtual void AddForce2(Vector3 positional, Vector3 angular)
	{
		this.m_PositionSpring2.AddForce(positional);
		this.m_RotationSpring2.AddForce(angular);
	}

	// Token: 0x0600B26E RID: 45678 RVA: 0x0042A27D File Offset: 0x0042847D
	public virtual void AddForce2(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
	{
		this.AddForce2(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot));
	}

	// Token: 0x0600B26F RID: 45679 RVA: 0x0042A298 File Offset: 0x00428498
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSprings()
	{
		base.Transform.localPosition = Vector3.up;
		base.Transform.localRotation = Quaternion.identity;
		this.m_PositionSpring2.FixedUpdate();
		this.m_RotationSpring2.FixedUpdate();
	}

	// Token: 0x0600B270 RID: 45680 RVA: 0x0042A2D0 File Offset: 0x004284D0
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

	// Token: 0x0600B271 RID: 45681 RVA: 0x0042A3B2 File Offset: 0x004285B2
	public override void Activate()
	{
		base.Activate();
		this.m_Wielded = true;
		base.Rendering = true;
	}

	// Token: 0x0600B272 RID: 45682 RVA: 0x0042A3C8 File Offset: 0x004285C8
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

	// Token: 0x0600B273 RID: 45683 RVA: 0x0042A43F File Offset: 0x0042863F
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

	// Token: 0x0600B274 RID: 45684 RVA: 0x0042A469 File Offset: 0x00428669
	public virtual void Wield(bool isWielding = true)
	{
		this.m_Wielded = isWielding;
		this.Refresh();
		base.StateManager.CombineStates();
	}

	// Token: 0x0600B275 RID: 45685 RVA: 0x0042A483 File Offset: 0x00428683
	public virtual void RefreshWeaponModel()
	{
		if (this.Player == null)
		{
			return;
		}
		vp_Value<bool> isFirstPerson = this.Player.IsFirstPerson;
	}

	// Token: 0x0600B276 RID: 45686 RVA: 0x0042A4A0 File Offset: 0x004286A0
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

	// Token: 0x0600B277 RID: 45687 RVA: 0x0042A50A File Offset: 0x0042870A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		base.Rendering = false;
	}

	// Token: 0x0600B278 RID: 45688 RVA: 0x0042A52B File Offset: 0x0042872B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		base.Rendering = true;
	}

	// Token: 0x17001513 RID: 5395
	// (get) Token: 0x0600B279 RID: 45689 RVA: 0x0042A54C File Offset: 0x0042874C
	public virtual Vector3 OnValue_AimDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Weapon3rdPersonModel.transform.position - this.Player.LookPoint.Get()).normalized;
		}
	}

	// Token: 0x040085F5 RID: 34293
	public GameObject Weapon3rdPersonModel;

	// Token: 0x040085F6 RID: 34294
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_WeaponModel;

	// Token: 0x040085F7 RID: 34295
	public Vector3 PositionOffset = new Vector3(0.15f, -0.15f, -0.15f);

	// Token: 0x040085F8 RID: 34296
	public Vector3 AimingPositionOffset = new Vector3(0f, 0f, 0f);

	// Token: 0x040085F9 RID: 34297
	public float PositionSpring2Stiffness = 0.95f;

	// Token: 0x040085FA RID: 34298
	public float PositionSpring2Damping = 0.25f;

	// Token: 0x040085FB RID: 34299
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring2;

	// Token: 0x040085FC RID: 34300
	public Vector3 RotationOffset = Vector3.zero;

	// Token: 0x040085FD RID: 34301
	public float RotationSpring2Stiffness = 0.95f;

	// Token: 0x040085FE RID: 34302
	public float RotationSpring2Damping = 0.25f;

	// Token: 0x040085FF RID: 34303
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationSpring2;

	// Token: 0x04008600 RID: 34304
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Wielded = true;

	// Token: 0x04008601 RID: 34305
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Weapon3rdPersonModelWakeUpTimer = new vp_Timer.Handle();

	// Token: 0x04008602 RID: 34306
	public int AnimationType = 1;

	// Token: 0x04008603 RID: 34307
	public int AnimationGrip = 1;

	// Token: 0x04008604 RID: 34308
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008605 RID: 34309
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_RotationSpring2DefaultRotation = Vector3.zero;

	// Token: 0x02001642 RID: 5698
	public new enum Type
	{
		// Token: 0x04008607 RID: 34311
		Custom,
		// Token: 0x04008608 RID: 34312
		Firearm,
		// Token: 0x04008609 RID: 34313
		Melee,
		// Token: 0x0400860A RID: 34314
		Thrown
	}

	// Token: 0x02001643 RID: 5699
	public enum Grip
	{
		// Token: 0x0400860C RID: 34316
		Custom,
		// Token: 0x0400860D RID: 34317
		OneHanded,
		// Token: 0x0400860E RID: 34318
		TwoHanded
	}
}
