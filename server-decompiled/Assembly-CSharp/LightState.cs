using System;
using UnityEngine;

// Token: 0x02000618 RID: 1560
public abstract class LightState : MonoBehaviour
{
	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x060032CF RID: 13007 RVA: 0x00041024 File Offset: 0x0003F224
	public virtual float LODThreshold
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x060032D0 RID: 13008 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CanBeOn
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x060032D1 RID: 13009 RVA: 0x00041024 File Offset: 0x0003F224
	public virtual float Intensity
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x060032D2 RID: 13010 RVA: 0x00041024 File Offset: 0x0003F224
	public virtual float Emissive
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x060032D3 RID: 13011 RVA: 0x0014A429 File Offset: 0x00148629
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

	// Token: 0x060032D4 RID: 13012 RVA: 0x0014A449 File Offset: 0x00148649
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.lightLOD = base.gameObject.GetComponent<LightLOD>();
	}

	// Token: 0x060032D5 RID: 13013 RVA: 0x0014A45C File Offset: 0x0014865C
	public virtual void Kill()
	{
		global::UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060032D6 RID: 13014 RVA: 0x00002C47 File Offset: 0x00000E47
	[PublicizedFrom(EAccessModifier.Protected)]
	public LightState()
	{
	}

	// Token: 0x04002835 RID: 10293
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public LightLOD lightLOD;
}
