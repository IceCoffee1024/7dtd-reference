using System;
using UnityEngine;

// Token: 0x0200129B RID: 4763
public class NGuiPanelFade : MonoBehaviour
{
	// Token: 0x0600972D RID: 38701 RVA: 0x0038D05C File Offset: 0x0038B25C
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

	// Token: 0x0600972E RID: 38702 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
	}

	// Token: 0x0600972F RID: 38703 RVA: 0x0038D0B5 File Offset: 0x0038B2B5
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
		this.bFadeOut = false;
		this.init();
	}

	// Token: 0x06009730 RID: 38704 RVA: 0x0038D0C4 File Offset: 0x0038B2C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDisable()
	{
		this.reset();
	}

	// Token: 0x06009731 RID: 38705 RVA: 0x0038D0CC File Offset: 0x0038B2CC
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

	// Token: 0x06009732 RID: 38706 RVA: 0x0038D118 File Offset: 0x0038B318
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

	// Token: 0x06009733 RID: 38707 RVA: 0x0038D1CC File Offset: 0x0038B3CC
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

	// Token: 0x06009734 RID: 38708 RVA: 0x0038D2D4 File Offset: 0x0038B4D4
	public void StartFadeOut()
	{
		this.bFadeOut = true;
		this.mStart = Time.time;
	}

	// Token: 0x0400712C RID: 28972
	public float duration = 0.3f;

	// Token: 0x0400712D RID: 28973
	public bool bFadeInWhenEnabled = true;

	// Token: 0x0400712E RID: 28974
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mStart;

	// Token: 0x0400712F RID: 28975
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIWidget[] mWidgets;

	// Token: 0x04007130 RID: 28976
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] alpha;

	// Token: 0x04007131 RID: 28977
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFadeIn;

	// Token: 0x04007132 RID: 28978
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFadeOut;
}
