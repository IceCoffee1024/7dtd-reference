using System;
using UnityEngine;

// Token: 0x02001170 RID: 4464
public class XUiTweenFill : XUiTweenAbs
{
	// Token: 0x1700106B RID: 4203
	// (get) Token: 0x06008CB4 RID: 36020 RVA: 0x0035996B File Offset: 0x00357B6B
	// (set) Token: 0x06008CB5 RID: 36021 RVA: 0x00359973 File Offset: 0x00357B73
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

	// Token: 0x1700106C RID: 4204
	// (get) Token: 0x06008CB6 RID: 36022 RVA: 0x003599A0 File Offset: 0x00357BA0
	// (set) Token: 0x06008CB7 RID: 36023 RVA: 0x003599A8 File Offset: 0x00357BA8
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

	// Token: 0x06008CB8 RID: 36024 RVA: 0x003599D8 File Offset: 0x00357BD8
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

	// Token: 0x06008CB9 RID: 36025 RVA: 0x00359A47 File Offset: 0x00357C47
	public XUiTweenFill(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008CBA RID: 36026 RVA: 0x00359A5B File Offset: 0x00357C5B
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenFill>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067FC RID: 26620
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenFill tween;

	// Token: 0x040067FD RID: 26621
	[PublicizedFrom(EAccessModifier.Private)]
	public float start;

	// Token: 0x040067FE RID: 26622
	[PublicizedFrom(EAccessModifier.Private)]
	public float end = 1f;
}
