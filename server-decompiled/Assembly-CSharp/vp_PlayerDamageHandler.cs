using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001639 RID: 5689
public class vp_PlayerDamageHandler : vp_DamageHandler
{
	// Token: 0x170014F7 RID: 5367
	// (get) Token: 0x0600B203 RID: 45571 RVA: 0x004282BB File Offset: 0x004264BB
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

	// Token: 0x170014F8 RID: 5368
	// (get) Token: 0x0600B204 RID: 45572 RVA: 0x004282E2 File Offset: 0x004264E2
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

	// Token: 0x170014F9 RID: 5369
	// (get) Token: 0x0600B205 RID: 45573 RVA: 0x00428310 File Offset: 0x00426510
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

	// Token: 0x0600B206 RID: 45574 RVA: 0x0042836A File Offset: 0x0042656A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B207 RID: 45575 RVA: 0x00428386 File Offset: 0x00426586
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B208 RID: 45576 RVA: 0x004283A2 File Offset: 0x004265A2
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		if (this.Inventory != null)
		{
			this.m_InventoryWasEnabledAtStart = this.Inventory.enabled;
		}
	}

	// Token: 0x0600B209 RID: 45577 RVA: 0x004283C4 File Offset: 0x004265C4
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

	// Token: 0x0600B20A RID: 45578 RVA: 0x004285CC File Offset: 0x004267CC
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

	// Token: 0x170014FA RID: 5370
	// (get) Token: 0x0600B20B RID: 45579 RVA: 0x004286B0 File Offset: 0x004268B0
	// (set) Token: 0x0600B20C RID: 45580 RVA: 0x004286B8 File Offset: 0x004268B8
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

	// Token: 0x170014FB RID: 5371
	// (get) Token: 0x0600B20D RID: 45581 RVA: 0x004286CC File Offset: 0x004268CC
	public virtual float OnValue_MaxHealth
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.MaxHealth;
		}
	}

	// Token: 0x0600B20E RID: 45582 RVA: 0x004286D4 File Offset: 0x004268D4
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

	// Token: 0x0400857E RID: 34174
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x0400857F RID: 34175
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerInventory m_Inventory;

	// Token: 0x04008580 RID: 34176
	public bool AllowFallDamage = true;

	// Token: 0x04008581 RID: 34177
	public float FallImpactThreshold = 0.15f;

	// Token: 0x04008582 RID: 34178
	public bool DeathOnFallImpactThreshold;

	// Token: 0x04008583 RID: 34179
	public Vector2 FallImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x04008584 RID: 34180
	public List<AudioClip> FallImpactSounds = new List<AudioClip>();

	// Token: 0x04008585 RID: 34181
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallImpactMultiplier = 2f;

	// Token: 0x04008586 RID: 34182
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_InventoryWasEnabledAtStart = true;

	// Token: 0x04008587 RID: 34183
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Collider> m_Colliders;
}
