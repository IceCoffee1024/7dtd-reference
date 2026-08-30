using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001608 RID: 5640
public class vp_PlayerDamageHandler : vp_DamageHandler
{
	// Token: 0x170014A8 RID: 5288
	// (get) Token: 0x0600B119 RID: 45337 RVA: 0x004221DB File Offset: 0x004203DB
	public vp_PlayerEventHandler Player
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Player == null)
			{
				this.m_Player = base.transform.GetComponent<vp_PlayerEventHandler>();
			}
			return this.m_Player;
		}
	}

	// Token: 0x170014A9 RID: 5289
	// (get) Token: 0x0600B11A RID: 45338 RVA: 0x00422202 File Offset: 0x00420402
	public vp_PlayerInventory Inventory
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Inventory == null)
			{
				this.m_Inventory = base.transform.root.GetComponentInChildren<vp_PlayerInventory>();
			}
			return this.m_Inventory;
		}
	}

	// Token: 0x170014AA RID: 5290
	// (get) Token: 0x0600B11B RID: 45339 RVA: 0x00422230 File Offset: 0x00420430
	public List<Collider> Colliders
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_Colliders == null)
			{
				this.m_Colliders = new List<Collider>();
				foreach (Collider collider in base.GetComponentsInChildren<Collider>())
				{
					if (collider.gameObject.layer == 23)
					{
						this.m_Colliders.Add(collider);
					}
				}
			}
			return this.m_Colliders;
		}
	}

	// Token: 0x0600B11C RID: 45340 RVA: 0x0042228A File Offset: 0x0042048A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B11D RID: 45341 RVA: 0x004222A6 File Offset: 0x004204A6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B11E RID: 45342 RVA: 0x004222C2 File Offset: 0x004204C2
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		if (this.Inventory != null)
		{
			this.m_InventoryWasEnabledAtStart = this.Inventory.enabled;
		}
	}

	// Token: 0x0600B11F RID: 45343 RVA: 0x004222E4 File Offset: 0x004204E4
	public override void Die()
	{
		if (!base.enabled || !vp_Utility.IsActive(base.gameObject))
		{
			return;
		}
		if (this.m_Audio != null)
		{
			this.m_Audio.pitch = Time.timeScale;
			this.m_Audio.PlayOneShot(this.DeathSound);
		}
		foreach (GameObject gameObject in this.DeathSpawnObjects)
		{
			if (gameObject != null)
			{
				vp_Utility.Instantiate(gameObject, base.transform.position, base.transform.rotation);
			}
		}
		foreach (Collider collider in this.Colliders)
		{
			collider.enabled = false;
		}
		if (this.Inventory != null && this.Inventory.enabled)
		{
			this.Inventory.enabled = false;
		}
		this.Player.SetWeapon.Argument = 0;
		this.Player.SetWeapon.Start(0f);
		this.Player.Dead.Start(0f);
		this.Player.Run.Stop(0f);
		this.Player.Jump.Stop(0f);
		this.Player.Crouch.Stop(0f);
		this.Player.Zoom.Stop(0f);
		this.Player.Attack.Stop(0f);
		this.Player.Reload.Stop(0f);
		this.Player.Climb.Stop(0f);
		this.Player.Interact.Stop(0f);
		if (vp_Gameplay.isMultiplayer && vp_Gameplay.isMaster)
		{
			vp_GlobalEvent<Transform>.Send("Kill", base.transform.root);
		}
	}

	// Token: 0x0600B120 RID: 45344 RVA: 0x004224EC File Offset: 0x004206EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Reset()
	{
		base.Reset();
		if (!Application.isPlaying)
		{
			return;
		}
		this.Player.Dead.Stop(0f);
		this.Player.Stop.Send();
		foreach (Collider collider in this.Colliders)
		{
			collider.enabled = true;
		}
		if (this.Inventory != null && !this.Inventory.enabled)
		{
			this.Inventory.enabled = this.m_InventoryWasEnabledAtStart;
		}
		if (this.m_Audio != null)
		{
			this.m_Audio.pitch = Time.timeScale;
			this.m_Audio.PlayOneShot(this.RespawnSound);
		}
	}

	// Token: 0x170014AB RID: 5291
	// (get) Token: 0x0600B121 RID: 45345 RVA: 0x004225D0 File Offset: 0x004207D0
	// (set) Token: 0x0600B122 RID: 45346 RVA: 0x004225D8 File Offset: 0x004207D8
	public virtual float OnValue_Health
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.CurrentHealth;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.CurrentHealth = Mathf.Min(value, this.MaxHealth);
		}
	}

	// Token: 0x170014AC RID: 5292
	// (get) Token: 0x0600B123 RID: 45347 RVA: 0x004225EC File Offset: 0x004207EC
	public virtual float OnValue_MaxHealth
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.MaxHealth;
		}
	}

	// Token: 0x0600B124 RID: 45348 RVA: 0x004225F4 File Offset: 0x004207F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnMessage_FallImpact(float impact)
	{
		if (this.Player.Dead.Active || !this.AllowFallDamage || impact <= this.FallImpactThreshold)
		{
			return;
		}
		vp_AudioUtility.PlayRandomSound(this.m_Audio, this.FallImpactSounds, this.FallImpactPitch);
		float num = Mathf.Abs(this.DeathOnFallImpactThreshold ? this.MaxHealth : (this.MaxHealth * impact));
		this.Damage(num);
	}

	// Token: 0x040084E7 RID: 34023
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x040084E8 RID: 34024
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerInventory m_Inventory;

	// Token: 0x040084E9 RID: 34025
	public bool AllowFallDamage = true;

	// Token: 0x040084EA RID: 34026
	public float FallImpactThreshold = 0.15f;

	// Token: 0x040084EB RID: 34027
	public bool DeathOnFallImpactThreshold;

	// Token: 0x040084EC RID: 34028
	public Vector2 FallImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x040084ED RID: 34029
	public List<AudioClip> FallImpactSounds = new List<AudioClip>();

	// Token: 0x040084EE RID: 34030
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallImpactMultiplier = 2f;

	// Token: 0x040084EF RID: 34031
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_InventoryWasEnabledAtStart = true;

	// Token: 0x040084F0 RID: 34032
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Collider> m_Colliders;
}
