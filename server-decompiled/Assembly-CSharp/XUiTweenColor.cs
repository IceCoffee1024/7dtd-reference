using System;
using UnityEngine;

// Token: 0x0200115F RID: 4447
public class XUiTweenColor : XUiTweenAbs
{
	// Token: 0x1700105E RID: 4190
	// (get) Token: 0x06008C59 RID: 35929 RVA: 0x0035774F File Offset: 0x0035594F
	// (set) Token: 0x06008C5A RID: 35930 RVA: 0x00357757 File Offset: 0x00355957
	[XuiXmlAttribute("start", false)]
	public Color Start
	{
		get
		{
			return this.start;
		}
		set
		{
			this.start = value;
			if (this.tween != null)
			{
				this.tween.from = this.start;
			}
		}
	}

	// Token: 0x1700105F RID: 4191
	// (get) Token: 0x06008C5B RID: 35931 RVA: 0x0035777F File Offset: 0x0035597F
	// (set) Token: 0x06008C5C RID: 35932 RVA: 0x00357787 File Offset: 0x00355987
	[XuiXmlAttribute("end", false)]
	public Color End
	{
		get
		{
			return this.end;
		}
		set
		{
			this.end = value;
			if (this.tween != null)
			{
				this.tween.to = this.end;
			}
		}
	}

	// Token: 0x06008C5D RID: 35933 RVA: 0x003577B0 File Offset: 0x003559B0
	[XuiXmlAttribute("play_to", false)]
	public void AttributePlayToEnd(Color _value)
	{
		if (this.tween == null)
		{
			return;
		}
		Color to = this.tween.to;
		if ((double)(Mathf.Abs(to.r - _value.r) + Mathf.Abs(to.g - _value.g) + Mathf.Abs(to.b - _value.b) + Mathf.Abs(to.a - _value.a)) < 0.002)
		{
			return;
		}
		base.Enabled = true;
		this.tween.tweenFactor = 0f;
		this.tween.SetStartToCurrentValue();
		this.End = _value;
		this.tween.PlayForward();
	}

	// Token: 0x06008C5E RID: 35934 RVA: 0x00357864 File Offset: 0x00355A64
	public XUiTweenColor(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008C5F RID: 35935 RVA: 0x00357883 File Offset: 0x00355A83
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenColor>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067B9 RID: 26553
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenColor tween;

	// Token: 0x040067BA RID: 26554
	[PublicizedFrom(EAccessModifier.Private)]
	public Color start = Color.black;

	// Token: 0x040067BB RID: 26555
	[PublicizedFrom(EAccessModifier.Private)]
	public Color end = Color.white;
}
