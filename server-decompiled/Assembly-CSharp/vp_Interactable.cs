using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015E2 RID: 5602
[RequireComponent(typeof(Collider))]
public abstract class vp_Interactable : MonoBehaviour
{
	// Token: 0x0600AEF0 RID: 44784 RVA: 0x004135D0 File Offset: 0x004117D0
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

	// Token: 0x0600AEF1 RID: 44785 RVA: 0x00413629 File Offset: 0x00411829
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600AEF2 RID: 44786 RVA: 0x00413645 File Offset: 0x00411845
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600AEF3 RID: 44787 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool TryInteract(vp_FPPlayerEventHandler player)
	{
		return false;
	}

	// Token: 0x0600AEF4 RID: 44788 RVA: 0x00413664 File Offset: 0x00411864
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

	// Token: 0x0600AEF5 RID: 44789 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void FinishInteraction()
	{
	}

	// Token: 0x0600AEF6 RID: 44790 RVA: 0x00413700 File Offset: 0x00411900
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Interactable()
	{
	}

	// Token: 0x040082A0 RID: 33440
	public vp_Interactable.vp_InteractType InteractType;

	// Token: 0x040082A1 RID: 33441
	public List<string> RecipientTags = new List<string>();

	// Token: 0x040082A2 RID: 33442
	public float InteractDistance;

	// Token: 0x040082A3 RID: 33443
	public Texture m_InteractCrosshair;

	// Token: 0x040082A4 RID: 33444
	public string InteractText = "";

	// Token: 0x040082A5 RID: 33445
	public float DelayShowingText = 2f;

	// Token: 0x040082A6 RID: 33446
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040082A7 RID: 33447
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x040082A8 RID: 33448
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x040082A9 RID: 33449
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x040082AA RID: 33450
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x020015E3 RID: 5603
	public enum vp_InteractType
	{
		// Token: 0x040082AC RID: 33452
		Normal,
		// Token: 0x040082AD RID: 33453
		Trigger,
		// Token: 0x040082AE RID: 33454
		CollisionTrigger
	}
}
