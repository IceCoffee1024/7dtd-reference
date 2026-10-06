using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200060F RID: 1551
[Preserve]
public class Fluctuating : LightState
{
	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x060032B7 RID: 12983 RVA: 0x001494CF File Offset: 0x001476CF
	public override float LODThreshold
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x060032B8 RID: 12984 RVA: 0x001494D6 File Offset: 0x001476D6
	public override float Intensity
	{
		get
		{
			return this.currentIntensity;
		}
	}

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x060032B9 RID: 12985 RVA: 0x001494DE File Offset: 0x001476DE
	public override float Emissive
	{
		get
		{
			return this.currentEmissive;
		}
	}

	// Token: 0x060032BA RID: 12986 RVA: 0x001494E6 File Offset: 0x001476E6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.startIntensity = 1f;
		this.fixedFrameRate = 1f / Time.fixedDeltaTime;
		this.currentIntensity = this.startIntensity;
		this.currentEmissive = this.startIntensity;
	}

	// Token: 0x060032BB RID: 12987 RVA: 0x00149524 File Offset: 0x00147724
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (GameManager.Instance.IsPaused())
		{
			return;
		}
		if (this.canSwitchProcess)
		{
			this.ChangeProcess();
			this.currentFrame = 0;
			int num = (int)(this.lightLOD.FluxDelay * this.fixedFrameRate);
			if (this.process == 0)
			{
				this.numOfFrames = global::UnityEngine.Random.Range(num / 2, num);
				this.t = 0f;
				this.preSlideIntenisty = this.currentIntensity;
				this.up = global::UnityEngine.Random.Range(0f, 1f) > this.slideProbability(this.preSlideIntenisty);
				if (this.up)
				{
					this.slideTo = global::UnityEngine.Random.Range(this.preSlideIntenisty, 1f);
				}
				else
				{
					this.slideTo = global::UnityEngine.Random.Range(0.2f, this.preSlideIntenisty);
				}
				this.increment = (this.slideTo - this.preSlideIntenisty) / (float)this.numOfFrames;
			}
			else if (this.process == 1)
			{
				this.numOfFrames = global::UnityEngine.Random.Range(90, 181);
			}
			else
			{
				this.numOfFrames = global::UnityEngine.Random.Range(num / 2, num);
			}
		}
		if (this.process == 0)
		{
			this.Slide();
		}
		else if (this.process == 1)
		{
			this.Flutter();
		}
		else if (this.canSwitchProcess)
		{
			this.currentIntensity = this.startIntensity;
			this.currentEmissive = this.startIntensity / 1f;
		}
		int num2 = this.currentFrame + 1;
		this.currentFrame = num2;
		this.canSwitchProcess = num2 >= this.numOfFrames;
	}

	// Token: 0x060032BC RID: 12988 RVA: 0x001496A8 File Offset: 0x001478A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Slide()
	{
		if (this.up)
		{
			this.currentIntensity = Mathf.Lerp(this.preSlideIntenisty, this.slideTo, this.t);
			this.currentEmissive = this.currentIntensity / 1f;
			this.t += this.increment;
			return;
		}
		this.currentIntensity = Mathf.Lerp(this.slideTo, this.preSlideIntenisty, this.t);
		this.currentEmissive = this.currentIntensity / 1f;
		this.t -= this.increment;
	}

	// Token: 0x060032BD RID: 12989 RVA: 0x00149744 File Offset: 0x00147944
	[PublicizedFrom(EAccessModifier.Private)]
	public void Flutter()
	{
		int num = global::UnityEngine.Random.Range(0, 3);
		if (num == 0)
		{
			this.currentIntensity = Mathf.Clamp(this.currentIntensity + 0.0625f, 0.2f, 1f);
			this.currentEmissive = this.currentIntensity / 1f;
			return;
		}
		if (num != 1)
		{
			return;
		}
		this.currentIntensity = Mathf.Clamp(this.currentIntensity - 0.0625f, 0.2f, 1f);
		this.currentEmissive = this.currentIntensity / 1f;
	}

	// Token: 0x060032BE RID: 12990 RVA: 0x001497C8 File Offset: 0x001479C8
	[PublicizedFrom(EAccessModifier.Private)]
	public float slideProbability(float intensity)
	{
		return (intensity - 0.2f) / 0.8f;
	}

	// Token: 0x060032BF RID: 12991 RVA: 0x001497D8 File Offset: 0x001479D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void ChangeProcess()
	{
		int num = global::UnityEngine.Random.Range(1, 3);
		if (num != this.process)
		{
			this.process = num;
			return;
		}
		if (this.process > 0)
		{
			this.process = (this.process + 1) % 3;
			return;
		}
		this.process++;
	}

	// Token: 0x04002804 RID: 10244
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float unityLightIntensityMax = 8f;

	// Token: 0x04002805 RID: 10245
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float hiRange = 1f;

	// Token: 0x04002806 RID: 10246
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float loRange = 0.2f;

	// Token: 0x04002807 RID: 10247
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float flutterVariance = 0.0625f;

	// Token: 0x04002808 RID: 10248
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float increment;

	// Token: 0x04002809 RID: 10249
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float startIntensity;

	// Token: 0x0400280A RID: 10250
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fixedFrameRate;

	// Token: 0x0400280B RID: 10251
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentIntensity;

	// Token: 0x0400280C RID: 10252
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentEmissive;

	// Token: 0x0400280D RID: 10253
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool canSwitchProcess = true;

	// Token: 0x0400280E RID: 10254
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int process;

	// Token: 0x0400280F RID: 10255
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numOfFrames;

	// Token: 0x04002810 RID: 10256
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentFrame;

	// Token: 0x04002811 RID: 10257
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float t;

	// Token: 0x04002812 RID: 10258
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float preSlideIntenisty;

	// Token: 0x04002813 RID: 10259
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float slideTo;

	// Token: 0x04002814 RID: 10260
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool up;
}
