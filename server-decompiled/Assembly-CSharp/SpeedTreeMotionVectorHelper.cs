using System;
using UnityEngine;

// Token: 0x02001392 RID: 5010
public class SpeedTreeMotionVectorHelper : MonoBehaviour
{
	// Token: 0x06009DA8 RID: 40360 RVA: 0x003B97AF File Offset: 0x003B79AF
	public void Init(Renderer renderer)
	{
		this.renderer = renderer;
	}

	// Token: 0x06009DA9 RID: 40361 RVA: 0x003B97B8 File Offset: 0x003B79B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnBecameVisible()
	{
		if (!SpeedTreeWindHistoryBufferManager.Instance.TryRegisterActiveRenderer(this.renderer))
		{
			Debug.LogError("Failed to register tree renderer.");
			base.enabled = false;
		}
	}

	// Token: 0x06009DAA RID: 40362 RVA: 0x003B97DD File Offset: 0x003B79DD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnBecameInvisible()
	{
		SpeedTreeWindHistoryBufferManager.Instance.DeregisterActiveRenderer(this.renderer);
	}

	// Token: 0x0400774C RID: 30540
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Renderer renderer;
}
