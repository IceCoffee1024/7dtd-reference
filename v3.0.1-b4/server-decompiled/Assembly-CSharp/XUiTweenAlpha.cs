using System;
using UnityEngine;

// Token: 0x0200112D RID: 4397
public class XUiTweenAlpha : XUiTweenAbs
{
	// Token: 0x17000FEE RID: 4078
	// (get) Token: 0x06008AF5 RID: 35573 RVA: 0x0034E9D1 File Offset: 0x0034CBD1
	// (set) Token: 0x06008AF6 RID: 35574 RVA: 0x0034E9D9 File Offset: 0x0034CBD9
	[XuiXmlAttribute("start", false)]
	public float Start
	{
		get
		{
			return this.start;
		}
		set
		{
			this.start = Mathf.Clamp01(value);
			if (this.tween != null)
			{
				this.tween.from = this.start;
			}
		}
	}

	// Token: 0x17000FEF RID: 4079
	// (get) Token: 0x06008AF7 RID: 35575 RVA: 0x0034EA06 File Offset: 0x0034CC06
	// (set) Token: 0x06008AF8 RID: 35576 RVA: 0x0034EA0E File Offset: 0x0034CC0E
	[XuiXmlAttribute("end", false)]
	public float End
	{
		get
		{
			return this.end;
		}
		set
		{
			this.end = Mathf.Clamp01(value);
			if (this.tween != null)
			{
				this.tween.to = this.end;
			}
		}
	}

	// Token: 0x06008AF9 RID: 35577 RVA: 0x0034EA3C File Offset: 0x0034CC3C
	[XuiXmlAttribute("play_to", false)]
	public void AttributePlayToEnd(float _value)
	{
		if (this.tween == null)
		{
			return;
		}
		if ((double)Mathf.Abs(this.tween.to - _value) < 0.0001)
		{
			return;
		}
		base.Enabled = true;
		this.tween.tweenFactor = 0f;
		this.tween.SetStartToCurrentValue();
		this.End = _value;
		this.tween.PlayForward();
	}

	// Token: 0x06008AFA RID: 35578 RVA: 0x0034EAAB File Offset: 0x0034CCAB
	public XUiTweenAlpha(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008AFB RID: 35579 RVA: 0x0034EABF File Offset: 0x0034CCBF
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenAlpha>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x04006697 RID: 26263
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenAlpha tween;

	// Token: 0x04006698 RID: 26264
	[PublicizedFrom(EAccessModifier.Private)]
	public float start;

	// Token: 0x04006699 RID: 26265
	[PublicizedFrom(EAccessModifier.Private)]
	public float end = 1f;
}
