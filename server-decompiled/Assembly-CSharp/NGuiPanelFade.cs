using System;
using UnityEngine;

// Token: 0x02001281 RID: 4737
public class NGuiPanelFade : MonoBehaviour
{
	// Token: 0x06009632 RID: 38450 RVA: 0x00387F00 File Offset: 0x00386100
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.mWidgets = base.GetComponentsInChildren<UIWidget>();
		this.alpha = new float[this.mWidgets.Length];
		for (int i = 0; i < this.mWidgets.Length; i++)
		{
			this.alpha[i] = this.mWidgets[i].color.a;
		}
	}

	// Token: 0x06009633 RID: 38451 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
	}

	// Token: 0x06009634 RID: 38452 RVA: 0x00387F59 File Offset: 0x00386159
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
		this.bFadeOut = false;
		this.init();
	}

	// Token: 0x06009635 RID: 38453 RVA: 0x00387F68 File Offset: 0x00386168
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDisable()
	{
		this.reset();
	}

	// Token: 0x06009636 RID: 38454 RVA: 0x00387F70 File Offset: 0x00386170
	[PublicizedFrom(EAccessModifier.Private)]
	public void reset()
	{
		for (int i = 0; i < this.mWidgets.Length; i++)
		{
			Color color = this.mWidgets[i].color;
			color.a = this.alpha[i];
			this.mWidgets[i].color = color;
		}
	}

	// Token: 0x06009637 RID: 38455 RVA: 0x00387FBC File Offset: 0x003861BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void init()
	{
		this.mStart = Time.time;
		this.bFadeIn = this.bFadeInWhenEnabled;
		this.mWidgets = base.GetComponentsInChildren<UIWidget>();
		if (this.alpha.Length != this.mWidgets.Length)
		{
			this.alpha = new float[this.mWidgets.Length];
		}
		for (int i = 0; i < this.mWidgets.Length; i++)
		{
			Color color = this.mWidgets[i].color;
			if (color.a != 0f)
			{
				this.alpha[i] = color.a;
			}
			if (this.bFadeIn)
			{
				color.a = 0f;
				this.mWidgets[i].color = color;
			}
		}
	}

	// Token: 0x06009638 RID: 38456 RVA: 0x00388070 File Offset: 0x00386270
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		float num;
		if (this.bFadeIn)
		{
			num = ((this.duration > 0f) ? Mathf.Clamp01((Time.time - this.mStart) / this.duration) : 1f);
		}
		else
		{
			if (!this.bFadeOut)
			{
				return;
			}
			num = ((this.duration > 0f) ? (1f - Mathf.Clamp01((Time.realtimeSinceStartup - this.mStart) / this.duration)) : 0f);
		}
		for (int i = 0; i < this.mWidgets.Length; i++)
		{
			Color color = this.mWidgets[i].color;
			color.a = num * this.alpha[i];
			this.mWidgets[i].color = color;
		}
		if (this.bFadeOut && num <= 0.001f)
		{
			this.reset();
			base.gameObject.SetActive(false);
			this.bFadeOut = false;
		}
		if (this.bFadeIn && num >= 1f)
		{
			this.bFadeIn = false;
		}
	}

	// Token: 0x06009639 RID: 38457 RVA: 0x00388178 File Offset: 0x00386378
	public void StartFadeOut()
	{
		this.bFadeOut = true;
		this.mStart = Time.time;
	}

	// Token: 0x04007057 RID: 28759
	public float duration = 0.3f;

	// Token: 0x04007058 RID: 28760
	public bool bFadeInWhenEnabled = true;

	// Token: 0x04007059 RID: 28761
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mStart;

	// Token: 0x0400705A RID: 28762
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIWidget[] mWidgets;

	// Token: 0x0400705B RID: 28763
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] alpha;

	// Token: 0x0400705C RID: 28764
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFadeIn;

	// Token: 0x0400705D RID: 28765
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFadeOut;
}
