using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class BlockSwitchController : MonoBehaviour
{
	// Token: 0x17000065 RID: 101
	// (get) Token: 0x0600060C RID: 1548 RVA: 0x0002B729 File Offset: 0x00029929
	// (set) Token: 0x0600060D RID: 1549 RVA: 0x0002B731 File Offset: 0x00029931
	public bool Powered
	{
		get
		{
			return this.powered;
		}
		set
		{
			this.powered = value;
			this.UpdateLights();
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600060E RID: 1550 RVA: 0x0002B740 File Offset: 0x00029940
	// (set) Token: 0x0600060F RID: 1551 RVA: 0x0002B748 File Offset: 0x00029948
	public bool Activated
	{
		get
		{
			return this.activated;
		}
		set
		{
			this.activated = value;
			this.UpdateLights();
		}
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0002B757 File Offset: 0x00029957
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.UpdateLights();
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0002B75F File Offset: 0x0002995F
	public void SetState(bool _powered, bool _activated)
	{
		this.powered = _powered;
		this.activated = _activated;
		this.UpdateLights();
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0002B778 File Offset: 0x00029978
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateLights()
	{
		if (!this.Powered)
		{
			this.GreenLight.SetActive(false);
			this.RedLight.SetActive(false);
			return;
		}
		this.GreenLight.SetActive(this.activated);
		this.RedLight.SetActive(!this.activated);
	}

	// Token: 0x040006C2 RID: 1730
	public GameObject RedLight;

	// Token: 0x040006C3 RID: 1731
	public GameObject GreenLight;

	// Token: 0x040006C4 RID: 1732
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool powered;

	// Token: 0x040006C5 RID: 1733
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool activated;
}
