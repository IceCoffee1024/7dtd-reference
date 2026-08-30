using System;
using UnityEngine;

// Token: 0x02001148 RID: 4424
public class XUiTweenFill : XUiTweenAbs
{
	// Token: 0x1700102B RID: 4139
	// (get) Token: 0x06008BF5 RID: 35829 RVA: 0x0035349F File Offset: 0x0035169F
	// (set) Token: 0x06008BF6 RID: 35830 RVA: 0x003534A7 File Offset: 0x003516A7
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

	// Token: 0x1700102C RID: 4140
	// (get) Token: 0x06008BF7 RID: 35831 RVA: 0x003534D4 File Offset: 0x003516D4
	// (set) Token: 0x06008BF8 RID: 35832 RVA: 0x003534DC File Offset: 0x003516DC
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

	// Token: 0x06008BF9 RID: 35833 RVA: 0x0035350C File Offset: 0x0035170C
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

	// Token: 0x06008BFA RID: 35834 RVA: 0x0035357B File Offset: 0x0035177B
	public XUiTweenFill(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008BFB RID: 35835 RVA: 0x0035358F File Offset: 0x0035178F
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenFill>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x04006767 RID: 26471
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenFill tween;

	// Token: 0x04006768 RID: 26472
	[PublicizedFrom(EAccessModifier.Private)]
	public float start;

	// Token: 0x04006769 RID: 26473
	[PublicizedFrom(EAccessModifier.Private)]
	public float end = 1f;
}
