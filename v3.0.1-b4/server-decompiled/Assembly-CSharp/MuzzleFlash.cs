using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
public class MuzzleFlash : MonoBehaviour
{
	// Token: 0x06000019 RID: 25 RVA: 0x00002818 File Offset: 0x00000A18
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		base.GetComponent<Light>().intensity = 0f;
		this.targetIntensity = this.highIntensity;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002838 File Offset: 0x00000A38
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		if (this.alarmOn)
		{
			base.GetComponent<Light>().intensity = Mathf.Lerp(base.GetComponent<Light>().intensity, this.targetIntensity, this.fadeSpeed * Time.deltaTime);
			this.CheckTargetIntensity();
			return;
		}
		base.GetComponent<Light>().intensity = Mathf.Lerp(base.GetComponent<Light>().intensity, 0f, this.fadeSpeed * Time.deltaTime);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000028B0 File Offset: 0x00000AB0
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckTargetIntensity()
	{
		if (Mathf.Abs(this.targetIntensity - base.GetComponent<Light>().intensity) < this.changeMargin)
		{
			if (this.targetIntensity == this.highIntensity)
			{
				this.targetIntensity = this.lowIntensity;
				return;
			}
			this.targetIntensity = this.highIntensity;
		}
	}

	// Token: 0x04000020 RID: 32
	public float fadeSpeed = 2f;

	// Token: 0x04000021 RID: 33
	public float highIntensity = 2f;

	// Token: 0x04000022 RID: 34
	public float lowIntensity = 0.5f;

	// Token: 0x04000023 RID: 35
	public float changeMargin = 0.2f;

	// Token: 0x04000024 RID: 36
	public bool alarmOn;

	// Token: 0x04000025 RID: 37
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float targetIntensity;
}
