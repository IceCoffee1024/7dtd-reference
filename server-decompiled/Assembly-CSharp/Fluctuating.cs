using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000616 RID: 1558
[Preserve]
public class Fluctuating : LightState
{
	// Token: 0x1700054F RID: 1359
	// (get) Token: 0x060032C5 RID: 12997 RVA: 0x0014A0C3 File Offset: 0x001482C3
	public override float LODThreshold
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x17000550 RID: 1360
	// (get) Token: 0x060032C6 RID: 12998 RVA: 0x0014A0CA File Offset: 0x001482CA
	public override float Intensity
	{
		get
		{
			return this.currentIntensity;
		}
	}

	// Token: 0x17000551 RID: 1361
	// (get) Token: 0x060032C7 RID: 12999 RVA: 0x0014A0D2 File Offset: 0x001482D2
	public override float Emissive
	{
		get
		{
			return this.currentEmissive;
		}
	}

	// Token: 0x060032C8 RID: 13000 RVA: 0x0014A0DA File Offset: 0x001482DA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.startIntensity = 1f;
		this.fixedFrameRate = 1f / Time.fixedDeltaTime;
		this.currentIntensity = this.startIntensity;
		this.currentEmissive = this.startIntensity;
	}

	// Token: 0x060032C9 RID: 13001 RVA: 0x0014A118 File Offset: 0x00148318
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

	// Token: 0x060032CA RID: 13002 RVA: 0x0014A29C File Offset: 0x0014849C
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

	// Token: 0x060032CB RID: 13003 RVA: 0x0014A338 File Offset: 0x00148538
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

	// Token: 0x060032CC RID: 13004 RVA: 0x0014A3BC File Offset: 0x001485BC
	[PublicizedFrom(EAccessModifier.Private)]
	public float slideProbability(float intensity)
	{
		return (intensity - 0.2f) / 0.8f;
	}

	// Token: 0x060032CD RID: 13005 RVA: 0x0014A3CC File Offset: 0x001485CC
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

	// Token: 0x0400281F RID: 10271
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float unityLightIntensityMax = 8f;

	// Token: 0x04002820 RID: 10272
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float hiRange = 1f;

	// Token: 0x04002821 RID: 10273
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float loRange = 0.2f;

	// Token: 0x04002822 RID: 10274
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float flutterVariance = 0.0625f;

	// Token: 0x04002823 RID: 10275
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float increment;

	// Token: 0x04002824 RID: 10276
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float startIntensity;

	// Token: 0x04002825 RID: 10277
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fixedFrameRate;

	// Token: 0x04002826 RID: 10278
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentIntensity;

	// Token: 0x04002827 RID: 10279
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentEmissive;

	// Token: 0x04002828 RID: 10280
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool canSwitchProcess = true;

	// Token: 0x04002829 RID: 10281
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int process;

	// Token: 0x0400282A RID: 10282
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numOfFrames;

	// Token: 0x0400282B RID: 10283
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentFrame;

	// Token: 0x0400282C RID: 10284
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float t;

	// Token: 0x0400282D RID: 10285
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float preSlideIntenisty;

	// Token: 0x0400282E RID: 10286
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float slideTo;

	// Token: 0x0400282F RID: 10287
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool up;
}
