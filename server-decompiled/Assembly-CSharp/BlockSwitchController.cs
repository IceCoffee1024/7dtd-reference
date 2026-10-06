using System;
using UnityEngine;

// Token: 0x020000EE RID: 238
public class BlockSwitchController : MonoBehaviour
{
	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000613 RID: 1555 RVA: 0x0002B8E5 File Offset: 0x00029AE5
	// (set) Token: 0x06000614 RID: 1556 RVA: 0x0002B8ED File Offset: 0x00029AED
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
	// (get) Token: 0x06000615 RID: 1557 RVA: 0x0002B8FC File Offset: 0x00029AFC
	// (set) Token: 0x06000616 RID: 1558 RVA: 0x0002B904 File Offset: 0x00029B04
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

	// Token: 0x06000617 RID: 1559 RVA: 0x0002B913 File Offset: 0x00029B13
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.UpdateLights();
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0002B91B File Offset: 0x00029B1B
	public void SetState(bool _powered, bool _activated)
	{
		this.powered = _powered;
		this.activated = _activated;
		this.UpdateLights();
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0002B934 File Offset: 0x00029B34
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

	// Token: 0x040006C5 RID: 1733
	public GameObject RedLight;

	// Token: 0x040006C6 RID: 1734
	public GameObject GreenLight;

	// Token: 0x040006C7 RID: 1735
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool powered;

	// Token: 0x040006C8 RID: 1736
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool activated;
}
