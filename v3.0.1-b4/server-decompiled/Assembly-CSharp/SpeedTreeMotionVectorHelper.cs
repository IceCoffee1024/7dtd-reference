using System;
using UnityEngine;

// Token: 0x02001358 RID: 4952
public class SpeedTreeMotionVectorHelper : MonoBehaviour
{
	// Token: 0x06009C01 RID: 39937 RVA: 0x003AF98B File Offset: 0x003ADB8B
	public void Init(Renderer renderer)
	{
		this.renderer = renderer;
	}

	// Token: 0x06009C02 RID: 39938 RVA: 0x003AF994 File Offset: 0x003ADB94
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnBecameVisible()
	{
		if (!SpeedTreeWindHistoryBufferManager.Instance.TryRegisterActiveRenderer(this.renderer))
		{
			Debug.LogError("Failed to register tree renderer.");
			base.enabled = false;
		}
	}

	// Token: 0x06009C03 RID: 39939 RVA: 0x003AF9B9 File Offset: 0x003ADBB9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnBecameInvisible()
	{
		SpeedTreeWindHistoryBufferManager.Instance.DeregisterActiveRenderer(this.renderer);
	}

	// Token: 0x040075F9 RID: 30201
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Renderer renderer;
}
