using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class BlockSwitchSingleController : MonoBehaviour
{
	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600061B RID: 1563 RVA: 0x0002B987 File Offset: 0x00029B87
	// (set) Token: 0x0600061C RID: 1564 RVA: 0x0002B98F File Offset: 0x00029B8F
	public bool Activated
	{
		get
		{
			return this.activated;
		}
		set
		{
			this.activated = value;
			this.SetState();
		}
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x0002B99E File Offset: 0x00029B9E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.SetState();
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0002B98F File Offset: 0x00029B8F
	public void SetState(bool _activated)
	{
		this.activated = _activated;
		this.SetState();
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0002B9A6 File Offset: 0x00029BA6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetState()
	{
		if (this.ItemPrefab != null)
		{
			this.ItemPrefab.SetActive(!this.activated);
		}
	}

	// Token: 0x040006C9 RID: 1737
	public GameObject ItemPrefab;

	// Token: 0x040006CA RID: 1738
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool activated;
}
