using System;
using UnityEngine;

// Token: 0x02001372 RID: 4978
public class SpeedTreeMotionVectorHelper : MonoBehaviour
{
	// Token: 0x06009CFC RID: 40188 RVA: 0x003B4C33 File Offset: 0x003B2E33
	public void Init(Renderer renderer)
	{
		this.renderer = renderer;
	}

	// Token: 0x06009CFD RID: 40189 RVA: 0x003B4C3C File Offset: 0x003B2E3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnBecameVisible()
	{
		if (!SpeedTreeWindHistoryBufferManager.Instance.TryRegisterActiveRenderer(this.renderer))
		{
			Debug.LogError("Failed to register tree renderer.");
			base.enabled = false;
		}
	}

	// Token: 0x06009CFE RID: 40190 RVA: 0x003B4C61 File Offset: 0x003B2E61
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnBecameInvisible()
	{
		SpeedTreeWindHistoryBufferManager.Instance.DeregisterActiveRenderer(this.renderer);
	}

	// Token: 0x040076D1 RID: 30417
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Renderer renderer;
}
