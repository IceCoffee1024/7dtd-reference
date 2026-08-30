using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015F8 RID: 5624
public class vp_FPInteractManager : MonoBehaviour
{
	// Token: 0x17001477 RID: 5239
	// (get) Token: 0x0600B021 RID: 45089 RVA: 0x0041AFCD File Offset: 0x004191CD
	// (set) Token: 0x0600B022 RID: 45090 RVA: 0x0041AFD5 File Offset: 0x004191D5
	public float CrosshairTimeoutTimer { get; set; }

	// Token: 0x0600B023 RID: 45091 RVA: 0x0041AFDE File Offset: 0x004191DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.GetComponent<vp_FPPlayerEventHandler>();
		this.m_Camera = base.GetComponentInChildren<vp_FPCamera>();
	}

	// Token: 0x0600B024 RID: 45092 RVA: 0x0041AFF8 File Offset: 0x004191F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B025 RID: 45093 RVA: 0x0041B014 File Offset: 0x00419214
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B026 RID: 45094 RVA: 0x0041B030 File Offset: 0x00419230
	public virtual void OnStart_Dead()
	{
		this.ShouldFinishInteraction();
	}

	// Token: 0x0600B027 RID: 45095 RVA: 0x0041B03C File Offset: 0x0041923C
	public virtual void LateUpdate()
	{
		if (this.m_Player.Dead.Active)
		{
			return;
		}
		if (this.m_OriginalCrosshair == null && this.m_Player.Crosshair.Get() != null)
		{
			this.m_OriginalCrosshair = this.m_Player.Crosshair.Get();
		}
		this.InteractCrosshair();
	}

	// Token: 0x0600B028 RID: 45096 RVA: 0x0041B0A8 File Offset: 0x004192A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool CanStart_Interact()
	{
		if (this.ShouldFinishInteraction())
		{
			return false;
		}
		if (this.m_Player.SetWeapon.Active)
		{
			return false;
		}
		vp_Interactable vp_Interactable = null;
		if (!this.FindInteractable(out vp_Interactable))
		{
			return false;
		}
		if (vp_Interactable.InteractType != vp_Interactable.vp_InteractType.Normal)
		{
			return false;
		}
		if (!vp_Interactable.TryInteract(this.m_Player))
		{
			return false;
		}
		this.ResetCrosshair(false);
		this.m_LastInteractable = vp_Interactable;
		return true;
	}

	// Token: 0x0600B029 RID: 45097 RVA: 0x0041B10C File Offset: 0x0041930C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool ShouldFinishInteraction()
	{
		if (this.m_Player.Interactable.Get() != null)
		{
			this.m_CurrentCrosshairInteractable = null;
			this.ResetCrosshair(true);
			this.m_Player.Interactable.Get().FinishInteraction();
			this.m_Player.Interactable.Set(null);
			return true;
		}
		return false;
	}

	// Token: 0x0600B02A RID: 45098 RVA: 0x0041B178 File Offset: 0x00419378
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InteractCrosshair()
	{
		if (this.m_Player.Crosshair.Get() == null)
		{
			return;
		}
		if (this.m_Player.Interactable.Get() != null)
		{
			return;
		}
		vp_Interactable interactable = null;
		if (this.FindInteractable(out interactable))
		{
			if (interactable != this.m_CurrentCrosshairInteractable)
			{
				if (this.CrosshairTimeoutTimer > Time.time && this.m_LastInteractable != null && interactable.GetType() == this.m_LastInteractable.GetType())
				{
					return;
				}
				this.m_CanInteract = true;
				this.m_CurrentCrosshairInteractable = interactable;
				if (interactable.InteractText != "" && !this.m_ShowTextTimer.Active)
				{
					vp_Timer.In(interactable.DelayShowingText, delegate
					{
						this.m_Player.HUDText.Send(interactable.InteractText);
					}, this.m_ShowTextTimer);
				}
				if (interactable.m_InteractCrosshair == null)
				{
					return;
				}
				this.m_Player.Crosshair.Set(interactable.m_InteractCrosshair);
				return;
			}
		}
		else
		{
			this.m_CanInteract = false;
			this.ResetCrosshair(true);
		}
	}

	// Token: 0x0600B02B RID: 45099 RVA: 0x0041B2D4 File Offset: 0x004194D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool FindInteractable(out vp_Interactable interactable)
	{
		interactable = null;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.m_Camera.Transform.position, this.m_Camera.Transform.forward, out raycastHit, this.MaxInteractDistance, -538750989))
		{
			if (!this.m_Interactables.TryGetValue(raycastHit.collider, out interactable))
			{
				Dictionary<Collider, vp_Interactable> interactables = this.m_Interactables;
				Collider collider = raycastHit.collider;
				vp_Interactable component;
				interactable = (component = raycastHit.collider.GetComponent<vp_Interactable>());
				interactables.Add(collider, component);
			}
			return !(interactable == null) && (interactable.InteractDistance != 0f || raycastHit.distance < (this.m_Player.IsFirstPerson.Get() ? this.InteractDistance : this.InteractDistance3rdPerson)) && (interactable.InteractDistance <= 0f || raycastHit.distance < interactable.InteractDistance);
		}
		return false;
	}

	// Token: 0x0600B02C RID: 45100 RVA: 0x0041B3C0 File Offset: 0x004195C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void ResetCrosshair(bool reset = true)
	{
		if (this.m_OriginalCrosshair == null || this.m_Player.Crosshair.Get() == this.m_OriginalCrosshair)
		{
			return;
		}
		this.m_ShowTextTimer.Cancel();
		if (reset)
		{
			this.m_Player.Crosshair.Set(this.m_OriginalCrosshair);
		}
		this.m_CurrentCrosshairInteractable = null;
	}

	// Token: 0x0600B02D RID: 45101 RVA: 0x0041B430 File Offset: 0x00419630
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody attachedRigidbody = hit.collider.attachedRigidbody;
		if (attachedRigidbody == null || attachedRigidbody.isKinematic)
		{
			return;
		}
		vp_Interactable vp_Interactable = null;
		if (!this.m_Interactables.TryGetValue(hit.collider, out vp_Interactable))
		{
			this.m_Interactables.Add(hit.collider, vp_Interactable = hit.collider.GetComponent<vp_Interactable>());
		}
		if (vp_Interactable == null)
		{
			return;
		}
		if (vp_Interactable.InteractType != vp_Interactable.vp_InteractType.CollisionTrigger)
		{
			return;
		}
		hit.gameObject.SendMessage("TryInteract", this.m_Player, SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x17001478 RID: 5240
	// (get) Token: 0x0600B02E RID: 45102 RVA: 0x0041B4BB File Offset: 0x004196BB
	// (set) Token: 0x0600B02F RID: 45103 RVA: 0x0041B4C3 File Offset: 0x004196C3
	public virtual vp_Interactable OnValue_Interactable
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CurrentInteractable;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_CurrentInteractable = value;
		}
	}

	// Token: 0x17001479 RID: 5241
	// (get) Token: 0x0600B030 RID: 45104 RVA: 0x0041B4CC File Offset: 0x004196CC
	// (set) Token: 0x0600B031 RID: 45105 RVA: 0x0041B4D4 File Offset: 0x004196D4
	public virtual bool OnValue_CanInteract
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_CanInteract;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_CanInteract = value;
		}
	}

	// Token: 0x040083B0 RID: 33712
	public float InteractDistance = 2f;

	// Token: 0x040083B1 RID: 33713
	public float InteractDistance3rdPerson = 3f;

	// Token: 0x040083B2 RID: 33714
	public float MaxInteractDistance = 25f;

	// Token: 0x040083B4 RID: 33716
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x040083B5 RID: 33717
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x040083B6 RID: 33718
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_CurrentInteractable;

	// Token: 0x040083B7 RID: 33719
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Texture m_OriginalCrosshair;

	// Token: 0x040083B8 RID: 33720
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_LastInteractable;

	// Token: 0x040083B9 RID: 33721
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, vp_Interactable> m_Interactables = new Dictionary<Collider, vp_Interactable>();

	// Token: 0x040083BA RID: 33722
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_CurrentCrosshairInteractable;

	// Token: 0x040083BB RID: 33723
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_ShowTextTimer = new vp_Timer.Handle();

	// Token: 0x040083BC RID: 33724
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_CanInteract;
}
