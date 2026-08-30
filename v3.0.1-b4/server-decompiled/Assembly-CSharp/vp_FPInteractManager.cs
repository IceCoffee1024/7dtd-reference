using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015DD RID: 5597
public class vp_FPInteractManager : MonoBehaviour
{
	// Token: 0x1700143E RID: 5182
	// (get) Token: 0x0600AF24 RID: 44836 RVA: 0x00415C8D File Offset: 0x00413E8D
	// (set) Token: 0x0600AF25 RID: 44837 RVA: 0x00415C95 File Offset: 0x00413E95
	public float CrosshairTimeoutTimer { get; set; }

	// Token: 0x0600AF26 RID: 44838 RVA: 0x00415C9E File Offset: 0x00413E9E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = base.GetComponent<vp_FPPlayerEventHandler>();
		this.m_Camera = base.GetComponentInChildren<vp_FPCamera>();
	}

	// Token: 0x0600AF27 RID: 44839 RVA: 0x00415CB8 File Offset: 0x00413EB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600AF28 RID: 44840 RVA: 0x00415CD4 File Offset: 0x00413ED4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600AF29 RID: 44841 RVA: 0x00415CF0 File Offset: 0x00413EF0
	public virtual void OnStart_Dead()
	{
		this.ShouldFinishInteraction();
	}

	// Token: 0x0600AF2A RID: 44842 RVA: 0x00415CFC File Offset: 0x00413EFC
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

	// Token: 0x0600AF2B RID: 44843 RVA: 0x00415D68 File Offset: 0x00413F68
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

	// Token: 0x0600AF2C RID: 44844 RVA: 0x00415DCC File Offset: 0x00413FCC
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

	// Token: 0x0600AF2D RID: 44845 RVA: 0x00415E38 File Offset: 0x00414038
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

	// Token: 0x0600AF2E RID: 44846 RVA: 0x00415F94 File Offset: 0x00414194
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

	// Token: 0x0600AF2F RID: 44847 RVA: 0x00416080 File Offset: 0x00414280
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

	// Token: 0x0600AF30 RID: 44848 RVA: 0x004160F0 File Offset: 0x004142F0
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

	// Token: 0x1700143F RID: 5183
	// (get) Token: 0x0600AF31 RID: 44849 RVA: 0x0041617B File Offset: 0x0041437B
	// (set) Token: 0x0600AF32 RID: 44850 RVA: 0x00416183 File Offset: 0x00414383
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

	// Token: 0x17001440 RID: 5184
	// (get) Token: 0x0600AF33 RID: 44851 RVA: 0x0041618C File Offset: 0x0041438C
	// (set) Token: 0x0600AF34 RID: 44852 RVA: 0x00416194 File Offset: 0x00414394
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

	// Token: 0x040082D7 RID: 33495
	public float InteractDistance = 2f;

	// Token: 0x040082D8 RID: 33496
	public float InteractDistance3rdPerson = 3f;

	// Token: 0x040082D9 RID: 33497
	public float MaxInteractDistance = 25f;

	// Token: 0x040082DB RID: 33499
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x040082DC RID: 33500
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x040082DD RID: 33501
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_CurrentInteractable;

	// Token: 0x040082DE RID: 33502
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Texture m_OriginalCrosshair;

	// Token: 0x040082DF RID: 33503
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_LastInteractable;

	// Token: 0x040082E0 RID: 33504
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<Collider, vp_Interactable> m_Interactables = new Dictionary<Collider, vp_Interactable>();

	// Token: 0x040082E1 RID: 33505
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Interactable m_CurrentCrosshairInteractable;

	// Token: 0x040082E2 RID: 33506
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_Timer.Handle m_ShowTextTimer = new vp_Timer.Handle();

	// Token: 0x040082E3 RID: 33507
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_CanInteract;
}
