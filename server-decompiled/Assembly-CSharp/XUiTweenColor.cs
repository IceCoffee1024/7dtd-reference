using System;
using UnityEngine;

// Token: 0x0200112E RID: 4398
public class XUiTweenColor : XUiTweenAbs
{
	// Token: 0x17000FF0 RID: 4080
	// (get) Token: 0x06008AFC RID: 35580 RVA: 0x0034EAFB File Offset: 0x0034CCFB
	// (set) Token: 0x06008AFD RID: 35581 RVA: 0x0034EB03 File Offset: 0x0034CD03
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

	// Token: 0x17000FF1 RID: 4081
	// (get) Token: 0x06008AFE RID: 35582 RVA: 0x0034EB2B File Offset: 0x0034CD2B
	// (set) Token: 0x06008AFF RID: 35583 RVA: 0x0034EB33 File Offset: 0x0034CD33
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

	// Token: 0x06008B00 RID: 35584 RVA: 0x0034EB5C File Offset: 0x0034CD5C
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

	// Token: 0x06008B01 RID: 35585 RVA: 0x0034EC10 File Offset: 0x0034CE10
	public XUiTweenColor(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008B02 RID: 35586 RVA: 0x0034EC2F File Offset: 0x0034CE2F
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenColor>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x0400669A RID: 26266
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenColor tween;

	// Token: 0x0400669B RID: 26267
	[PublicizedFrom(EAccessModifier.Private)]
	public Color start = Color.black;

	// Token: 0x0400669C RID: 26268
	[PublicizedFrom(EAccessModifier.Private)]
	public Color end = Color.white;
}
