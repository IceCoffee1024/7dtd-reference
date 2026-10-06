using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001617 RID: 5655
public class vp_FPInteractManager : MonoBehaviour
{
	// Token: 0x170014BA RID: 5306
	// (get) Token: 0x0600B0B3 RID: 45235 RVA: 0x0041ECA9 File Offset: 0x0041CEA9
	// (set) Token: 0x0600B0B4 RID: 45236 RVA: 0x0041ECB1 File Offset: 0x0041CEB1
	public float CrosshairTimeoutTimer { get; set; }

	// Token: 0x0600B0B5 RID: 45237 RVA: 0x0041ECBA File Offset: 0x0041CEBA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.GetComponent<vp_FPPlayerEventHandler>();
		this.m_Camera = base.GetComponentInChildren<vp_FPCamera>();
	}

	// Token: 0x0600B0B6 RID: 45238 RVA: 0x0041ECD4 File Offset: 0x0041CED4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B0B7 RID: 45239 RVA: 0x0041ECF0 File Offset: 0x0041CEF0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B0B8 RID: 45240 RVA: 0x0041ED0C File Offset: 0x0041CF0C
	public virtual void OnStart_Dead()
	{
		this.ShouldFinishInteraction();
	}

	// Token: 0x0600B0B9 RID: 45241 RVA: 0x0041ED18 File Offset: 0x0041CF18
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

	// Token: 0x0600B0BA RID: 45242 RVA: 0x0041ED84 File Offset: 0x0041CF84
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

	// Token: 0x0600B0BB RID: 45243 RVA: 0x0041EDE8 File Offset: 0x0041CFE8
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

	// Token: 0x0600B0BC RID: 45244 RVA: 0x0041EE54 File Offset: 0x0041D054
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

	// Token: 0x0600B0BD RID: 45245 RVA: 0x0041EFB0 File Offset: 0x0041D1B0
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

	// Token: 0x0600B0BE RID: 45246 RVA: 0x0041F09C File Offset: 0x0041D29C
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

	// Token: 0x0600B0BF RID: 45247 RVA: 0x0041F10C File Offset: 0x0041D30C
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

	// Token: 0x170014BB RID: 5307
	// (get) Token: 0x0600B0C0 RID: 45248 RVA: 0x0041F197 File Offset: 0x0041D397
	// (set) Token: 0x0600B0C1 RID: 45249 RVA: 0x0041F19F File Offset: 0x0041D39F
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

	// Token: 0x170014BC RID: 5308
	// (get) Token: 0x0600B0C2 RID: 45250 RVA: 0x0041F1A8 File Offset: 0x0041D3A8
	// (set) Token: 0x0600B0C3 RID: 45251 RVA: 0x0041F1B0 File Offset: 0x0041D3B0
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

	// Token: 0x04008406 RID: 33798
	public float InteractDistance = 2f;

	// Token: 0x04008407 RID: 33799
	public float InteractDistance3rdPerson = 3f;

	// Token: 0x04008408 RID: 33800
	public float MaxInteractDistance = 25f;

	// Token: 0x0400840A RID: 33802
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x0400840B RID: 33803
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x0400840C RID: 33804
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_CurrentInteractable;

	// Token: 0x0400840D RID: 33805
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Texture m_OriginalCrosshair;

	// Token: 0x0400840E RID: 33806
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_LastInteractable;

	// Token: 0x0400840F RID: 33807
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, vp_Interactable> m_Interactables = new Dictionary<Collider, vp_Interactable>();

	// Token: 0x04008410 RID: 33808
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_CurrentCrosshairInteractable;

	// Token: 0x04008411 RID: 33809
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_ShowTextTimer = new vp_Timer.Handle();

	// Token: 0x04008412 RID: 33810
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_CanInteract;
}
