using System;
using UnityEngine;

// Token: 0x02000606 RID: 1542
public abstract class LightState : MonoBehaviour
{
	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x0600322B RID: 12843 RVA: 0x00040D24 File Offset: 0x0003EF24
	public virtual float LODThreshold
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x0600322C RID: 12844 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CanBeOn
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x0600322D RID: 12845 RVA: 0x00040D24 File Offset: 0x0003EF24
	public virtual float Intensity
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x0600322E RID: 12846 RVA: 0x00040D24 File Offset: 0x0003EF24
	public virtual float Emissive
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000527 RID: 1319
	// (get) Token: 0x0600322F RID: 12847 RVA: 0x00146D6D File Offset: 0x00144F6D
	public virtual float AudioFrequency
	{
		get
		{
			if (!this.lightLOD)
			{
				return -1f;
			}
			return this.lightLOD.StateRate;
		}
	}

	// Token: 0x06003230 RID: 12848 RVA: 0x00146D8D File Offset: 0x00144F8D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.lightLOD = base.gameObject.GetComponent<LightLOD>();
	}

	// Token: 0x06003231 RID: 12849 RVA: 0x00146DA0 File Offset: 0x00144FA0
	public virtual void Kill()
	{
		global::UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06003232 RID: 12850 RVA: 0x00002C47 File Offset: 0x00000E47
	[PublicizedFrom(EAccessModifier.Protected)]
	public LightState()
	{
	}

	// Token: 0x040027AE RID: 10158
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public LightLOD lightLOD;
}
