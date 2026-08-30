using System;
using UnityEngine;

// Token: 0x0200112F RID: 4399
public class XUiTweenFill : XUiTweenAbs
{
	// Token: 0x17000FF2 RID: 4082
	// (get) Token: 0x06008B03 RID: 35587 RVA: 0x0034EC6B File Offset: 0x0034CE6B
	// (set) Token: 0x06008B04 RID: 35588 RVA: 0x0034EC73 File Offset: 0x0034CE73
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

	// Token: 0x17000FF3 RID: 4083
	// (get) Token: 0x06008B05 RID: 35589 RVA: 0x0034ECA0 File Offset: 0x0034CEA0
	// (set) Token: 0x06008B06 RID: 35590 RVA: 0x0034ECA8 File Offset: 0x0034CEA8
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

	// Token: 0x06008B07 RID: 35591 RVA: 0x0034ECD8 File Offset: 0x0034CED8
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

	// Token: 0x06008B08 RID: 35592 RVA: 0x0034ED47 File Offset: 0x0034CF47
	public XUiTweenFill(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008B09 RID: 35593 RVA: 0x0034ED5B File Offset: 0x0034CF5B
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenFill>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x0400669D RID: 26269
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenFill tween;

	// Token: 0x0400669E RID: 26270
	[PublicizedFrom(EAccessModifier.Private)]
	public float start;

	// Token: 0x0400669F RID: 26271
	[PublicizedFrom(EAccessModifier.Private)]
	public float end = 1f;
}
