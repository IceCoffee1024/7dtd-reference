using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000604 RID: 1540
[Preserve]
public class Fluctuating : LightState
{
	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06003221 RID: 12833 RVA: 0x00146A07 File Offset: 0x00144C07
	public override float LODThreshold
	{
		get
		{
			return 0.2f;
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06003222 RID: 12834 RVA: 0x00146A0E File Offset: 0x00144C0E
	public override float Intensity
	{
		get
		{
			return this.currentIntensity;
		}
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x06003223 RID: 12835 RVA: 0x00146A16 File Offset: 0x00144C16
	public override float Emissive
	{
		get
		{
			return this.currentEmissive;
		}
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x00146A1E File Offset: 0x00144C1E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.startIntensity = 1f;
		this.fixedFrameRate = 1f / Time.fixedDeltaTime;
		this.currentIntensity = this.startIntensity;
		this.currentEmissive = this.startIntensity;
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x00146A5C File Offset: 0x00144C5C
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

	// Token: 0x06003226 RID: 12838 RVA: 0x00146BE0 File Offset: 0x00144DE0
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

	// Token: 0x06003227 RID: 12839 RVA: 0x00146C7C File Offset: 0x00144E7C
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

	// Token: 0x06003228 RID: 12840 RVA: 0x00146D00 File Offset: 0x00144F00
	[PublicizedFrom(EAccessModifier.Private)]
	public float slideProbability(float intensity)
	{
		return (intensity - 0.2f) / 0.8f;
	}

	// Token: 0x06003229 RID: 12841 RVA: 0x00146D10 File Offset: 0x00144F10
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

	// Token: 0x04002798 RID: 10136
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float unityLightIntensityMax = 8f;

	// Token: 0x04002799 RID: 10137
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float hiRange = 1f;

	// Token: 0x0400279A RID: 10138
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float loRange = 0.2f;

	// Token: 0x0400279B RID: 10139
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float flutterVariance = 0.0625f;

	// Token: 0x0400279C RID: 10140
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float increment;

	// Token: 0x0400279D RID: 10141
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float startIntensity;

	// Token: 0x0400279E RID: 10142
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fixedFrameRate;

	// Token: 0x0400279F RID: 10143
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentIntensity;

	// Token: 0x040027A0 RID: 10144
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentEmissive;

	// Token: 0x040027A1 RID: 10145
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool canSwitchProcess = true;

	// Token: 0x040027A2 RID: 10146
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int process;

	// Token: 0x040027A3 RID: 10147
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int numOfFrames;

	// Token: 0x040027A4 RID: 10148
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int currentFrame;

	// Token: 0x040027A5 RID: 10149
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float t;

	// Token: 0x040027A6 RID: 10150
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float preSlideIntenisty;

	// Token: 0x040027A7 RID: 10151
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float slideTo;

	// Token: 0x040027A8 RID: 10152
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool up;
}
