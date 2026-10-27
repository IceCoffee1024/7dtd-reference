using System;
using UnityEngine;

// Token: 0x020013A4 RID: 5028
public class SpeedTreeMotionVectorHelper : MonoBehaviour
{
	// Token: 0x06009E00 RID: 40448 RVA: 0x003BBBB3 File Offset: 0x003B9DB3
	public void Init(Renderer renderer)
	{
		this.renderer = renderer;
	}

	// Token: 0x06009E01 RID: 40449 RVA: 0x003BBBBC File Offset: 0x003B9DBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnBecameVisible()
	{
		if (!SpeedTreeWindHistoryBufferManager.Instance.TryRegisterActiveRenderer(this.renderer))
		{
			Debug.LogError("Failed to register tree renderer.");
			base.enabled = false;
		}
	}

	// Token: 0x06009E02 RID: 40450 RVA: 0x003BBBE1 File Offset: 0x003B9DE1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnBecameInvisible()
	{
		SpeedTreeWindHistoryBufferManager.Instance.DeregisterActiveRenderer(this.renderer);
	}

	// Token: 0x0400778D RID: 30605
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Renderer renderer;
}
