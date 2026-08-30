using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015ED RID: 5613
public class vp_PlayerDamageHandler : vp_DamageHandler
{
	// Token: 0x1700146F RID: 5231
	// (get) Token: 0x0600B01C RID: 45084 RVA: 0x0041CE9B File Offset: 0x0041B09B
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

	// Token: 0x17001470 RID: 5232
	// (get) Token: 0x0600B01D RID: 45085 RVA: 0x0041CEC2 File Offset: 0x0041B0C2
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

	// Token: 0x17001471 RID: 5233
	// (get) Token: 0x0600B01E RID: 45086 RVA: 0x0041CEF0 File Offset: 0x0041B0F0
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

	// Token: 0x0600B01F RID: 45087 RVA: 0x0041CF4A File Offset: 0x0041B14A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B020 RID: 45088 RVA: 0x0041CF66 File Offset: 0x0041B166
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B021 RID: 45089 RVA: 0x0041CF82 File Offset: 0x0041B182
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		if (this.Inventory != null)
		{
			this.m_InventoryWasEnabledAtStart = this.Inventory.enabled;
		}
	}

	// Token: 0x0600B022 RID: 45090 RVA: 0x0041CFA4 File Offset: 0x0041B1A4
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

	// Token: 0x0600B023 RID: 45091 RVA: 0x0041D1AC File Offset: 0x0041B3AC
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

	// Token: 0x17001472 RID: 5234
	// (get) Token: 0x0600B024 RID: 45092 RVA: 0x0041D290 File Offset: 0x0041B490
	// (set) Token: 0x0600B025 RID: 45093 RVA: 0x0041D298 File Offset: 0x0041B498
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

	// Token: 0x17001473 RID: 5235
	// (get) Token: 0x0600B026 RID: 45094 RVA: 0x0041D2AC File Offset: 0x0041B4AC
	public virtual float OnValue_MaxHealth
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.MaxHealth;
		}
	}

	// Token: 0x0600B027 RID: 45095 RVA: 0x0041D2B4 File Offset: 0x0041B4B4
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

	// Token: 0x0400840E RID: 33806
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x0400840F RID: 33807
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerInventory m_Inventory;

	// Token: 0x04008410 RID: 33808
	public bool AllowFallDamage = true;

	// Token: 0x04008411 RID: 33809
	public float FallImpactThreshold = 0.15f;

	// Token: 0x04008412 RID: 33810
	public bool DeathOnFallImpactThreshold;

	// Token: 0x04008413 RID: 33811
	public Vector2 FallImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x04008414 RID: 33812
	public List<AudioClip> FallImpactSounds = new List<AudioClip>();

	// Token: 0x04008415 RID: 33813
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallImpactMultiplier = 2f;

	// Token: 0x04008416 RID: 33814
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_InventoryWasEnabledAtStart = true;

	// Token: 0x04008417 RID: 33815
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Collider> m_Colliders;
}
