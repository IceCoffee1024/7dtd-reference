using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001629 RID: 5673
public class vp_FPInteractManager : MonoBehaviour
{
	// Token: 0x170014C6 RID: 5318
	// (get) Token: 0x0600B10B RID: 45323 RVA: 0x004210AD File Offset: 0x0041F2AD
	// (set) Token: 0x0600B10C RID: 45324 RVA: 0x004210B5 File Offset: 0x0041F2B5
	public float CrosshairTimeoutTimer { get; set; }

	// Token: 0x0600B10D RID: 45325 RVA: 0x004210BE File Offset: 0x0041F2BE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.GetComponent<vp_FPPlayerEventHandler>();
		this.m_Camera = base.GetComponentInChildren<vp_FPCamera>();
	}

	// Token: 0x0600B10E RID: 45326 RVA: 0x004210D8 File Offset: 0x0041F2D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B10F RID: 45327 RVA: 0x004210F4 File Offset: 0x0041F2F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B110 RID: 45328 RVA: 0x00421110 File Offset: 0x0041F310
	public virtual void OnStart_Dead()
	{
		this.ShouldFinishInteraction();
	}

	// Token: 0x0600B111 RID: 45329 RVA: 0x0042111C File Offset: 0x0041F31C
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

	// Token: 0x0600B112 RID: 45330 RVA: 0x00421188 File Offset: 0x0041F388
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

	// Token: 0x0600B113 RID: 45331 RVA: 0x004211EC File Offset: 0x0041F3EC
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

	// Token: 0x0600B114 RID: 45332 RVA: 0x00421258 File Offset: 0x0041F458
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

	// Token: 0x0600B115 RID: 45333 RVA: 0x004213B4 File Offset: 0x0041F5B4
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

	// Token: 0x0600B116 RID: 45334 RVA: 0x004214A0 File Offset: 0x0041F6A0
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

	// Token: 0x0600B117 RID: 45335 RVA: 0x00421510 File Offset: 0x0041F710
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

	// Token: 0x170014C7 RID: 5319
	// (get) Token: 0x0600B118 RID: 45336 RVA: 0x0042159B File Offset: 0x0041F79B
	// (set) Token: 0x0600B119 RID: 45337 RVA: 0x004215A3 File Offset: 0x0041F7A3
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

	// Token: 0x170014C8 RID: 5320
	// (get) Token: 0x0600B11A RID: 45338 RVA: 0x004215AC File Offset: 0x0041F7AC
	// (set) Token: 0x0600B11B RID: 45339 RVA: 0x004215B4 File Offset: 0x0041F7B4
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

	// Token: 0x04008447 RID: 33863
	public float InteractDistance = 2f;

	// Token: 0x04008448 RID: 33864
	public float InteractDistance3rdPerson = 3f;

	// Token: 0x04008449 RID: 33865
	public float MaxInteractDistance = 25f;

	// Token: 0x0400844B RID: 33867
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x0400844C RID: 33868
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x0400844D RID: 33869
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_CurrentInteractable;

	// Token: 0x0400844E RID: 33870
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Texture m_OriginalCrosshair;

	// Token: 0x0400844F RID: 33871
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_LastInteractable;

	// Token: 0x04008450 RID: 33872
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, vp_Interactable> m_Interactables = new Dictionary<Collider, vp_Interactable>();

	// Token: 0x04008451 RID: 33873
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_CurrentCrosshairInteractable;

	// Token: 0x04008452 RID: 33874
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_ShowTextTimer = new vp_Timer.Handle();

	// Token: 0x04008453 RID: 33875
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_CanInteract;
}
