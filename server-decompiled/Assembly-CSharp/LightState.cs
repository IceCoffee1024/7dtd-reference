using System;
using UnityEngine;

// Token: 0x0200060F RID: 1551
public abstract class LightState : MonoBehaviour
{
	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x0600327E RID: 12926 RVA: 0x00040F84 File Offset: 0x0003F184
	public virtual float LODThreshold
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x0600327F RID: 12927 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool CanBeOn
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06003280 RID: 12928 RVA: 0x00040F84 File Offset: 0x0003F184
	public virtual float Intensity
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06003281 RID: 12929 RVA: 0x00040F84 File Offset: 0x0003F184
	public virtual float Emissive
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06003282 RID: 12930 RVA: 0x00148E91 File Offset: 0x00147091
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

	// Token: 0x06003283 RID: 12931 RVA: 0x00148EB1 File Offset: 0x001470B1
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.lightLOD = base.gameObject.GetComponent<LightLOD>();
	}

	// Token: 0x06003284 RID: 12932 RVA: 0x00148EC4 File Offset: 0x001470C4
	public virtual void Kill()
	{
		global::UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06003285 RID: 12933 RVA: 0x00002C47 File Offset: 0x00000E47
	[PublicizedFrom(EAccessModifier.Protected)]
	public LightState()
	{
	}

	// Token: 0x04002821 RID: 10273
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public LightLOD lightLOD;
}
