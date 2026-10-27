using System;
using UnityEngine;

// Token: 0x020012C5 RID: 4805
public class NGuiPanelFade : MonoBehaviour
{
	// Token: 0x06009812 RID: 38930 RVA: 0x00393CAC File Offset: 0x00391EAC
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

	// Token: 0x06009813 RID: 38931 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
	}

	// Token: 0x06009814 RID: 38932 RVA: 0x00393D05 File Offset: 0x00391F05
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
		this.bFadeOut = false;
		this.init();
	}

	// Token: 0x06009815 RID: 38933 RVA: 0x00393D14 File Offset: 0x00391F14
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDisable()
	{
		this.reset();
	}

	// Token: 0x06009816 RID: 38934 RVA: 0x00393D1C File Offset: 0x00391F1C
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

	// Token: 0x06009817 RID: 38935 RVA: 0x00393D68 File Offset: 0x00391F68
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

	// Token: 0x06009818 RID: 38936 RVA: 0x00393E1C File Offset: 0x0039201C
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

	// Token: 0x06009819 RID: 38937 RVA: 0x00393F24 File Offset: 0x00392124
	public void StartFadeOut()
	{
		this.bFadeOut = true;
		this.mStart = Time.time;
	}

	// Token: 0x040071D7 RID: 29143
	public float duration = 0.3f;

	// Token: 0x040071D8 RID: 29144
	public bool bFadeInWhenEnabled = true;

	// Token: 0x040071D9 RID: 29145
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mStart;

	// Token: 0x040071DA RID: 29146
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIWidget[] mWidgets;

	// Token: 0x040071DB RID: 29147
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] alpha;

	// Token: 0x040071DC RID: 29148
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFadeIn;

	// Token: 0x040071DD RID: 29149
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFadeOut;
}
