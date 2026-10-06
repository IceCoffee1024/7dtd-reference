using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200060D RID: 1549
[Preserve]
public class Blinking : LightState
{
	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x060032AA RID: 12970 RVA: 0x001493F8 File Offset: 0x001475F8
	public override float LODThreshold
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x060032AB RID: 12971 RVA: 0x001493FF File Offset: 0x001475FF
	public override bool CanBeOn
	{
		get
		{
			return this.switchedOn;
		}
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x060032AC RID: 12972 RVA: 0x00149407 File Offset: 0x00147607
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

	// Token: 0x060032AD RID: 12973 RVA: 0x0014941C File Offset: 0x0014761C
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

	// Token: 0x060032AE RID: 12974 RVA: 0x0014942B File Offset: 0x0014762B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		base.StartCoroutine(this.blink());
	}

	// Token: 0x060032AF RID: 12975 RVA: 0x0014943A File Offset: 0x0014763A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x04002800 RID: 10240
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool switchedOn = true;
}
