using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200060B RID: 1547
[Preserve]
public class Blinking : LightState
{
	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06003267 RID: 12903 RVA: 0x00148A54 File Offset: 0x00146C54
	public override float LODThreshold
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06003268 RID: 12904 RVA: 0x00148A5B File Offset: 0x00146C5B
	public override bool CanBeOn
	{
		get
		{
			return this.switchedOn;
		}
	}

	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x06003269 RID: 12905 RVA: 0x00148A63 File Offset: 0x00146C63
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

	// Token: 0x0600326A RID: 12906 RVA: 0x00148A78 File Offset: 0x00146C78
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

	// Token: 0x0600326B RID: 12907 RVA: 0x00148A87 File Offset: 0x00146C87
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		base.StartCoroutine(this.blink());
	}

	// Token: 0x0600326C RID: 12908 RVA: 0x00148A96 File Offset: 0x00146C96
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x04002807 RID: 10247
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool switchedOn = true;
}
