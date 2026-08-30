using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000602 RID: 1538
[Preserve]
public class Blinking : LightState
{
	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x06003214 RID: 12820 RVA: 0x00146930 File Offset: 0x00144B30
	public override float LODThreshold
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700051C RID: 1308
	// (get) Token: 0x06003215 RID: 12821 RVA: 0x00146937 File Offset: 0x00144B37
	public override bool CanBeOn
	{
		get
		{
			return this.switchedOn;
		}
	}

	// Token: 0x1700051D RID: 1309
	// (get) Token: 0x06003216 RID: 12822 RVA: 0x0014693F File Offset: 0x00144B3F
	public override float Emissive
	{
		get
		{
			if (!this.switchedOn)
			{
				return 0f;
			}
			return 1f;
		}
	}

	// Token: 0x06003217 RID: 12823 RVA: 0x00146954 File Offset: 0x00144B54
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator blink()
	{
		for (;;)
		{
			this.switchedOn = !this.switchedOn;
			yield return new WaitForSeconds(1f / this.lightLOD.StateRate);
		}
		yield break;
	}

	// Token: 0x06003218 RID: 12824 RVA: 0x00146963 File Offset: 0x00144B63
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		base.StartCoroutine(this.blink());
	}

	// Token: 0x06003219 RID: 12825 RVA: 0x00146972 File Offset: 0x00144B72
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x04002794 RID: 10132
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool switchedOn = true;
}
