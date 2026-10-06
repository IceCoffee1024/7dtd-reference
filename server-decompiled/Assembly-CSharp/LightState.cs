using System;
using UnityEngine;

// Token: 0x02000611 RID: 1553
public abstract class LightState : MonoBehaviour
{
	// Token: 0x17000552 RID: 1362
	// (get) Token: 0x060032C1 RID: 12993 RVA: 0x00041064 File Offset: 0x0003F264
	public virtual float LODThreshold
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000553 RID: 1363
	// (get) Token: 0x060032C2 RID: 12994 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CanBeOn
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000554 RID: 1364
	// (get) Token: 0x060032C3 RID: 12995 RVA: 0x00041064 File Offset: 0x0003F264
	public virtual float Intensity
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000555 RID: 1365
	// (get) Token: 0x060032C4 RID: 12996 RVA: 0x00041064 File Offset: 0x0003F264
	public virtual float Emissive
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000556 RID: 1366
	// (get) Token: 0x060032C5 RID: 12997 RVA: 0x00149835 File Offset: 0x00147A35
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

	// Token: 0x060032C6 RID: 12998 RVA: 0x00149855 File Offset: 0x00147A55
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.lightLOD = base.gameObject.GetComponent<LightLOD>();
	}

	// Token: 0x060032C7 RID: 12999 RVA: 0x00149868 File Offset: 0x00147A68
	public virtual void Kill()
	{
		global::UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060032C8 RID: 13000 RVA: 0x00002C47 File Offset: 0x00000E47
	[PublicizedFrom(EAccessModifier.Protected)]
	public LightState()
	{
	}

	// Token: 0x0400281A RID: 10266
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public LightLOD lightLOD;
}
