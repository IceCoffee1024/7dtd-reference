using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001627 RID: 5671
public class vp_PlayerDamageHandler : vp_DamageHandler
{
	// Token: 0x170014EB RID: 5355
	// (get) Token: 0x0600B1AB RID: 45483 RVA: 0x00425EB7 File Offset: 0x004240B7
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

	// Token: 0x170014EC RID: 5356
	// (get) Token: 0x0600B1AC RID: 45484 RVA: 0x00425EDE File Offset: 0x004240DE
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

	// Token: 0x170014ED RID: 5357
	// (get) Token: 0x0600B1AD RID: 45485 RVA: 0x00425F0C File Offset: 0x0042410C
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

	// Token: 0x0600B1AE RID: 45486 RVA: 0x00425F66 File Offset: 0x00424166
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnEnable()
	{
		if (this.Player != null)
		{
			this.Player.Register(this);
		}
	}

	// Token: 0x0600B1AF RID: 45487 RVA: 0x00425F82 File Offset: 0x00424182
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnDisable()
	{
		if (this.Player != null)
		{
			this.Player.Unregister(this);
		}
	}

	// Token: 0x0600B1B0 RID: 45488 RVA: 0x00425F9E File Offset: 0x0042419E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		if (this.Inventory != null)
		{
			this.m_InventoryWasEnabledAtStart = this.Inventory.enabled;
		}
	}

	// Token: 0x0600B1B1 RID: 45489 RVA: 0x00425FC0 File Offset: 0x004241C0
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

	// Token: 0x0600B1B2 RID: 45490 RVA: 0x004261C8 File Offset: 0x004243C8
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

	// Token: 0x170014EE RID: 5358
	// (get) Token: 0x0600B1B3 RID: 45491 RVA: 0x004262AC File Offset: 0x004244AC
	// (set) Token: 0x0600B1B4 RID: 45492 RVA: 0x004262B4 File Offset: 0x004244B4
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

	// Token: 0x170014EF RID: 5359
	// (get) Token: 0x0600B1B5 RID: 45493 RVA: 0x004262C8 File Offset: 0x004244C8
	public virtual float OnValue_MaxHealth
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.MaxHealth;
		}
	}

	// Token: 0x0600B1B6 RID: 45494 RVA: 0x004262D0 File Offset: 0x004244D0
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

	// Token: 0x0400853D RID: 34109
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerEventHandler m_Player;

	// Token: 0x0400853E RID: 34110
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_PlayerInventory m_Inventory;

	// Token: 0x0400853F RID: 34111
	public bool AllowFallDamage = true;

	// Token: 0x04008540 RID: 34112
	public float FallImpactThreshold = 0.15f;

	// Token: 0x04008541 RID: 34113
	public bool DeathOnFallImpactThreshold;

	// Token: 0x04008542 RID: 34114
	public Vector2 FallImpactPitch = new Vector2(1f, 1.5f);

	// Token: 0x04008543 RID: 34115
	public List<AudioClip> FallImpactSounds = new List<AudioClip>();

	// Token: 0x04008544 RID: 34116
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float m_FallImpactMultiplier = 2f;

	// Token: 0x04008545 RID: 34117
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_InventoryWasEnabledAtStart = true;

	// Token: 0x04008546 RID: 34118
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Collider> m_Colliders;
}
