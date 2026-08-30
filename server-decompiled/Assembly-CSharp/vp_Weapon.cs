using System;
using UnityEngine;

// Token: 0x020015F5 RID: 5621
public class vp_Weapon : vp_Component
{
	// Token: 0x17001487 RID: 5255
	// (get) Token: 0x0600B07B RID: 45179 RVA: 0x0041EC86 File Offset: 0x0041CE86
	// (set) Token: 0x0600B07C RID: 45180 RVA: 0x0041EC98 File Offset: 0x0041CE98
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

	// Token: 0x17001488 RID: 5256
	// (get) Token: 0x0600B07D RID: 45181 RVA: 0x0041ECA1 File Offset: 0x0041CEA1
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

	// Token: 0x0600B07E RID: 45182 RVA: 0x0041ECD8 File Offset: 0x0041CED8
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

	// Token: 0x0600B07F RID: 45183 RVA: 0x0041ED79 File Offset: 0x0041CF79
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		this.RefreshWeaponModel();
		base.OnEnable();
	}

	// Token: 0x0600B080 RID: 45184 RVA: 0x0041ED87 File Offset: 0x0041CF87
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		this.RefreshWeaponModel();
		this.Activate3rdPersonModel(false);
		base.OnDisable();
	}

	// Token: 0x17001489 RID: 5257
	// (get) Token: 0x0600B081 RID: 45185 RVA: 0x0041ED9C File Offset: 0x0041CF9C
	// (set) Token: 0x0600B082 RID: 45186 RVA: 0x0041EDA4 File Offset: 0x0041CFA4
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

	// Token: 0x0600B083 RID: 45187 RVA: 0x0041EDB0 File Offset: 0x0041CFB0
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

	// Token: 0x1700148A RID: 5258
	// (get) Token: 0x0600B084 RID: 45188 RVA: 0x0041EE1B File Offset: 0x0041D01B
	public Vector3 Recoil
	{
		get
		{
			return this.m_RotationSpring2.State;
		}
	}

	// Token: 0x0600B085 RID: 45189 RVA: 0x0041EE28 File Offset: 0x0041D028
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

	// Token: 0x0600B086 RID: 45190 RVA: 0x0041EE43 File Offset: 0x0041D043
	public virtual void AddForce2(Vector3 positional, Vector3 angular)
	{
		this.m_PositionSpring2.AddForce(positional);
		this.m_RotationSpring2.AddForce(angular);
	}

	// Token: 0x0600B087 RID: 45191 RVA: 0x0041EE5D File Offset: 0x0041D05D
	public virtual void AddForce2(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot)
	{
		this.AddForce2(new Vector3(xPos, yPos, zPos), new Vector3(xRot, yRot, zRot));
	}

	// Token: 0x0600B088 RID: 45192 RVA: 0x0041EE78 File Offset: 0x0041D078
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateSprings()
	{
		base.Transform.localPosition = Vector3.up;
		base.Transform.localRotation = Quaternion.identity;
		this.m_PositionSpring2.FixedUpdate();
		this.m_RotationSpring2.FixedUpdate();
	}

	// Token: 0x0600B089 RID: 45193 RVA: 0x0041EEB0 File Offset: 0x0041D0B0
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

	// Token: 0x0600B08A RID: 45194 RVA: 0x0041EF92 File Offset: 0x0041D192
	public override void Activate()
	{
		base.Activate();
		this.m_Wielded = true;
		base.Rendering = true;
	}

	// Token: 0x0600B08B RID: 45195 RVA: 0x0041EFA8 File Offset: 0x0041D1A8
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

	// Token: 0x0600B08C RID: 45196 RVA: 0x0041F01F File Offset: 0x0041D21F
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

	// Token: 0x0600B08D RID: 45197 RVA: 0x0041F049 File Offset: 0x0041D249
	public virtual void Wield(bool isWielding = true)
	{
		this.m_Wielded = isWielding;
		this.Refresh();
		base.StateManager.CombineStates();
	}

	// Token: 0x0600B08E RID: 45198 RVA: 0x0041F063 File Offset: 0x0041D263
	public virtual void RefreshWeaponModel()
	{
		if (this.Player == null)
		{
			return;
		}
		vp_Value<bool> isFirstPerson = this.Player.IsFirstPerson;
	}

	// Token: 0x0600B08F RID: 45199 RVA: 0x0041F080 File Offset: 0x0041D280
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

	// Token: 0x0600B090 RID: 45200 RVA: 0x0041F0EA File Offset: 0x0041D2EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStart_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		base.Rendering = false;
	}

	// Token: 0x0600B091 RID: 45201 RVA: 0x0041F10B File Offset: 0x0041D30B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnStop_Dead()
	{
		if (this.Player.IsFirstPerson.Get())
		{
			return;
		}
		base.Rendering = true;
	}

	// Token: 0x1700148B RID: 5259
	// (get) Token: 0x0600B092 RID: 45202 RVA: 0x0041F12C File Offset: 0x0041D32C
	public virtual Vector3 OnValue_AimDirection
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return (this.Weapon3rdPersonModel.transform.position - this.Player.LookPoint.Get()).normalized;
		}
	}

	// Token: 0x04008485 RID: 33925
	public GameObject Weapon3rdPersonModel;

	// Token: 0x04008486 RID: 33926
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public GameObject m_WeaponModel;

	// Token: 0x04008487 RID: 33927
	public Vector3 PositionOffset = new Vector3(0.15f, -0.15f, -0.15f);

	// Token: 0x04008488 RID: 33928
	public Vector3 AimingPositionOffset = new Vector3(0f, 0f, 0f);

	// Token: 0x04008489 RID: 33929
	public float PositionSpring2Stiffness = 0.95f;

	// Token: 0x0400848A RID: 33930
	public float PositionSpring2Damping = 0.25f;

	// Token: 0x0400848B RID: 33931
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_PositionSpring2;

	// Token: 0x0400848C RID: 33932
	public Vector3 RotationOffset = Vector3.zero;

	// Token: 0x0400848D RID: 33933
	public float RotationSpring2Stiffness = 0.95f;

	// Token: 0x0400848E RID: 33934
	public float RotationSpring2Damping = 0.25f;

	// Token: 0x0400848F RID: 33935
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Spring m_RotationSpring2;

	// Token: 0x04008490 RID: 33936
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Wielded = true;

	// Token: 0x04008491 RID: 33937
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_Weapon3rdPersonModelWakeUpTimer = new vp_Timer.Handle();

	// Token: 0x04008492 RID: 33938
	public int AnimationType = 1;

	// Token: 0x04008493 RID: 33939
	public int AnimationGrip = 1;

	// Token: 0x04008494 RID: 33940
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x04008495 RID: 33941
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 m_RotationSpring2DefaultRotation = Vector3.zero;

	// Token: 0x020015F6 RID: 5622
	public new enum Type
	{
		// Token: 0x04008497 RID: 33943
		Custom,
		// Token: 0x04008498 RID: 33944
		Firearm,
		// Token: 0x04008499 RID: 33945
		Melee,
		// Token: 0x0400849A RID: 33946
		Thrown
	}

	// Token: 0x020015F7 RID: 5623
	public enum Grip
	{
		// Token: 0x0400849C RID: 33948
		Custom,
		// Token: 0x0400849D RID: 33949
		OneHanded,
		// Token: 0x0400849E RID: 33950
		TwoHanded
	}
}
