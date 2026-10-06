using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001601 RID: 5633
[RequireComponent(typeof(Collider))]
public abstract class vp_Interactable : MonoBehaviour
{
	// Token: 0x0600AF82 RID: 44930 RVA: 0x004172AC File Offset: 0x004154AC
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

	// Token: 0x0600AF83 RID: 44931 RVA: 0x00417305 File Offset: 0x00415505
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600AF84 RID: 44932 RVA: 0x00417321 File Offset: 0x00415521
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600AF85 RID: 44933 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool TryInteract(vp_FPPlayerEventHandler player)
	{
		return false;
	}

	// Token: 0x0600AF86 RID: 44934 RVA: 0x00417340 File Offset: 0x00415540
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

	// Token: 0x0600AF87 RID: 44935 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void FinishInteraction()
	{
	}

	// Token: 0x0600AF88 RID: 44936 RVA: 0x004173DC File Offset: 0x004155DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Interactable()
	{
	}

	// Token: 0x040082F6 RID: 33526
	public vp_Interactable.vp_InteractType InteractType;

	// Token: 0x040082F7 RID: 33527
	public List<string> RecipientTags = new List<string>();

	// Token: 0x040082F8 RID: 33528
	public float InteractDistance;

	// Token: 0x040082F9 RID: 33529
	public Texture m_InteractCrosshair;

	// Token: 0x040082FA RID: 33530
	public string InteractText = "";

	// Token: 0x040082FB RID: 33531
	public float DelayShowingText = 2f;

	// Token: 0x040082FC RID: 33532
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Transform m_Transform;

	// Token: 0x040082FD RID: 33533
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPController m_Controller;

	// Token: 0x040082FE RID: 33534
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPCamera m_Camera;

	// Token: 0x040082FF RID: 33535
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_WeaponHandler m_WeaponHandler;

	// Token: 0x04008300 RID: 33536
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;

	// Token: 0x02001602 RID: 5634
	public enum vp_InteractType
	{
		// Token: 0x04008302 RID: 33538
		Normal,
		// Token: 0x04008303 RID: 33539
		Trigger,
		// Token: 0x04008304 RID: 33540
		CollisionTrigger
	}
}
