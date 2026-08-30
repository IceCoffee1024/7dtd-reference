using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015C7 RID: 5575
[RequireComponent(typeof(Collider))]
public abstract class vp_Interactable : MonoBehaviour
{
	// Token: 0x0600ADF3 RID: 44531 RVA: 0x0040E290 File Offset: 0x0040C490
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Start()
	{
		this.m_Transform = base.transform;
		if (this.RecipientTags.Count == 0)
		{
			this.RecipientTags.Add("Player");
		}
		if (this.InteractType == vp_Interactable.vp_InteractType.Trigger && base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().isTrigger = true;
		}
	}

	// Token: 0x0600ADF4 RID: 44532 RVA: 0x0040E2E9 File Offset: 0x0040C4E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600ADF5 RID: 44533 RVA: 0x0040E305 File Offset: 0x0040C505
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600ADF6 RID: 44534 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool TryInteract(vp_FPPlayerEventHandler player)
	{
		return false;
	}

	// Token: 0x0600ADF7 RID: 44535 RVA: 0x0040E324 File Offset: 0x0040C524
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnTriggerEnter(Collider col)
	{
		if (this.InteractType != vp_Interactable.vp_InteractType.Trigger)
		{
			return;
		}
		foreach (string text in this.RecipientTags)
		{
			if (col.gameObject.tag == text)
			{
				goto IL_004F;
			}
		}
		return;
		IL_004F:
		this.m_Player = col.gameObject.GetComponent<vp_FPPlayerEventHandler>();
		if (this.m_Player == null)
		{
			return;
		}
		this.TryInteract(this.m_Player);
	}

	// Token: 0x0600ADF8 RID: 44536 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void FinishInteraction()
	{
	}

	// Token: 0x0600ADF9 RID: 44537 RVA: 0x0040E3C0 File Offset: 0x0040C5C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Interactable()
	{
	}

	// Token: 0x040081C7 RID: 33223
	public vp_Interactable.vp_InteractType InteractType;

	// Token: 0x040081C8 RID: 33224
	public List<string> RecipientTags = new List<string>();

	// Token: 0x040081C9 RID: 33225
	public float InteractDistance;

	// Token: 0x040081CA RID: 33226
	public Texture m_InteractCrosshair;

	// Token: 0x040081CB RID: 33227
	public string InteractText = "";

	// Token: 0x040081CC RID: 33228
	public float DelayShowingText = 2f;

	// Token: 0x040081CD RID: 33229
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040081CE RID: 33230
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x040081CF RID: 33231
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x040081D0 RID: 33232
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x040081D1 RID: 33233
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x020015C8 RID: 5576
	public enum vp_InteractType
	{
		// Token: 0x040081D3 RID: 33235
		Normal,
		// Token: 0x040081D4 RID: 33236
		Trigger,
		// Token: 0x040081D5 RID: 33237
		CollisionTrigger
	}
}
