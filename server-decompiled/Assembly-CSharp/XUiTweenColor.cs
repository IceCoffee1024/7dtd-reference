using System;
using UnityEngine;

// Token: 0x0200116F RID: 4463
public class XUiTweenColor : XUiTweenAbs
{
	// Token: 0x17001069 RID: 4201
	// (get) Token: 0x06008CAD RID: 36013 RVA: 0x003597FB File Offset: 0x003579FB
	// (set) Token: 0x06008CAE RID: 36014 RVA: 0x00359803 File Offset: 0x00357A03
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

	// Token: 0x1700106A RID: 4202
	// (get) Token: 0x06008CAF RID: 36015 RVA: 0x0035982B File Offset: 0x00357A2B
	// (set) Token: 0x06008CB0 RID: 36016 RVA: 0x00359833 File Offset: 0x00357A33
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

	// Token: 0x06008CB1 RID: 36017 RVA: 0x0035985C File Offset: 0x00357A5C
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

	// Token: 0x06008CB2 RID: 36018 RVA: 0x00359910 File Offset: 0x00357B10
	public XUiTweenColor(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008CB3 RID: 36019 RVA: 0x0035992F File Offset: 0x00357B2F
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenColor>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067F9 RID: 26617
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenColor tween;

	// Token: 0x040067FA RID: 26618
	[PublicizedFrom(EAccessModifier.Private)]
	public Color start = Color.black;

	// Token: 0x040067FB RID: 26619
	[PublicizedFrom(EAccessModifier.Private)]
	public Color end = Color.white;
}
