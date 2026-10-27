using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000614 RID: 1556
[Preserve]
public class Blinking : LightState
{
	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x060032B8 RID: 12984 RVA: 0x00149FEC File Offset: 0x001481EC
	public override float LODThreshold
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x060032B9 RID: 12985 RVA: 0x00149FF3 File Offset: 0x001481F3
	public override bool CanBeOn
	{
		get
		{
			return this.switchedOn;
		}
	}

	// Token: 0x1700054C RID: 1356
	// (get) Token: 0x060032BA RID: 12986 RVA: 0x00149FFB File Offset: 0x001481FB
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

	// Token: 0x060032BB RID: 12987 RVA: 0x0014A010 File Offset: 0x00148210
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

	// Token: 0x060032BC RID: 12988 RVA: 0x0014A01F File Offset: 0x0014821F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		base.StartCoroutine(this.blink());
	}

	// Token: 0x060032BD RID: 12989 RVA: 0x0014A02E File Offset: 0x0014822E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x0400281B RID: 10267
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool switchedOn = true;
}
