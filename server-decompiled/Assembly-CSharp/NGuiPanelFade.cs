using System;
using UnityEngine;

// Token: 0x020012B3 RID: 4787
public class NGuiPanelFade : MonoBehaviour
{
	// Token: 0x060097BA RID: 38842 RVA: 0x003918A4 File Offset: 0x0038FAA4
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

	// Token: 0x060097BB RID: 38843 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
	}

	// Token: 0x060097BC RID: 38844 RVA: 0x003918FD File Offset: 0x0038FAFD
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
		this.bFadeOut = false;
		this.init();
	}

	// Token: 0x060097BD RID: 38845 RVA: 0x0039190C File Offset: 0x0038FB0C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDisable()
	{
		this.reset();
	}

	// Token: 0x060097BE RID: 38846 RVA: 0x00391914 File Offset: 0x0038FB14
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

	// Token: 0x060097BF RID: 38847 RVA: 0x00391960 File Offset: 0x0038FB60
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

	// Token: 0x060097C0 RID: 38848 RVA: 0x00391A14 File Offset: 0x0038FC14
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

	// Token: 0x060097C1 RID: 38849 RVA: 0x00391B1C File Offset: 0x0038FD1C
	public void StartFadeOut()
	{
		this.bFadeOut = true;
		this.mStart = Time.time;
	}

	// Token: 0x04007196 RID: 29078
	public float duration = 0.3f;

	// Token: 0x04007197 RID: 29079
	public bool bFadeInWhenEnabled = true;

	// Token: 0x04007198 RID: 29080
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float mStart;

	// Token: 0x04007199 RID: 29081
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UIWidget[] mWidgets;

	// Token: 0x0400719A RID: 29082
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float[] alpha;

	// Token: 0x0400719B RID: 29083
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFadeIn;

	// Token: 0x0400719C RID: 29084
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bFadeOut;
}
