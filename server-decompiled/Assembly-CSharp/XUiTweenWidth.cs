using System;
using UnityEngine;

// Token: 0x02001134 RID: 4404
public class XUiTweenWidth : XUiTweenAbs
{
	// Token: 0x17000FFC RID: 4092
	// (get) Token: 0x06008B26 RID: 35622 RVA: 0x0034F274 File Offset: 0x0034D474
	// (set) Token: 0x06008B27 RID: 35623 RVA: 0x0034F27C File Offset: 0x0034D47C
	[XuiXmlAttribute("start", false)]
	public int Start
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

	// Token: 0x17000FFD RID: 4093
	// (get) Token: 0x06008B28 RID: 35624 RVA: 0x0034F2A4 File Offset: 0x0034D4A4
	// (set) Token: 0x06008B29 RID: 35625 RVA: 0x0034F2AC File Offset: 0x0034D4AC
	[XuiXmlAttribute("end", false)]
	public int End
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

	// Token: 0x06008B2A RID: 35626 RVA: 0x0034F2D4 File Offset: 0x0034D4D4
	[XuiXmlAttribute("play_to", false)]
	public void AttributePlayToEnd(int _value)
	{
		if (this.tween == null)
		{
			return;
		}
		if (Mathf.Abs(this.tween.to - _value) < 1)
		{
			return;
		}
		base.Enabled = true;
		this.tween.tweenFactor = 0f;
		this.tween.SetStartToCurrentValue();
		this.End = _value;
		this.tween.PlayForward();
	}

	// Token: 0x06008B2B RID: 35627 RVA: 0x0034F33A File Offset: 0x0034D53A
	public XUiTweenWidth(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008B2C RID: 35628 RVA: 0x0034F353 File Offset: 0x0034D553
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenWidth>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040066AC RID: 26284
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenWidth tween;

	// Token: 0x040066AD RID: 26285
	[PublicizedFrom(EAccessModifier.Private)]
	public int start = 20;

	// Token: 0x040066AE RID: 26286
	[PublicizedFrom(EAccessModifier.Private)]
	public int end = 40;
}
