using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001613 RID: 5651
[RequireComponent(typeof(Collider))]
public abstract class vp_Interactable : MonoBehaviour
{
	// Token: 0x0600AFDA RID: 45018 RVA: 0x004196B0 File Offset: 0x004178B0
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

	// Token: 0x0600AFDB RID: 45019 RVA: 0x00419709 File Offset: 0x00417909
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600AFDC RID: 45020 RVA: 0x00419725 File Offset: 0x00417925
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600AFDD RID: 45021 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool TryInteract(vp_FPPlayerEventHandler player)
	{
		return false;
	}

	// Token: 0x0600AFDE RID: 45022 RVA: 0x00419744 File Offset: 0x00417944
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

	// Token: 0x0600AFDF RID: 45023 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void FinishInteraction()
	{
	}

	// Token: 0x0600AFE0 RID: 45024 RVA: 0x004197E0 File Offset: 0x004179E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Interactable()
	{
	}

	// Token: 0x04008337 RID: 33591
	public vp_Interactable.vp_InteractType InteractType;

	// Token: 0x04008338 RID: 33592
	public List<string> RecipientTags = new List<string>();

	// Token: 0x04008339 RID: 33593
	public float InteractDistance;

	// Token: 0x0400833A RID: 33594
	public Texture m_InteractCrosshair;

	// Token: 0x0400833B RID: 33595
	public string InteractText = "";

	// Token: 0x0400833C RID: 33596
	public float DelayShowingText = 2f;

	// Token: 0x0400833D RID: 33597
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x0400833E RID: 33598
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x0400833F RID: 33599
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x04008340 RID: 33600
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x04008341 RID: 33601
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x02001614 RID: 5652
	public enum vp_InteractType
	{
		// Token: 0x04008343 RID: 33603
		Normal,
		// Token: 0x04008344 RID: 33604
		Trigger,
		// Token: 0x04008345 RID: 33605
		CollisionTrigger
	}
}
