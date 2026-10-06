using System;
using UnityEngine;

// Token: 0x02001165 RID: 4453
public class XUiTweenWidth : XUiTweenAbs
{
	// Token: 0x1700106A RID: 4202
	// (get) Token: 0x06008C83 RID: 35971 RVA: 0x00357EC8 File Offset: 0x003560C8
	// (set) Token: 0x06008C84 RID: 35972 RVA: 0x00357ED0 File Offset: 0x003560D0
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

	// Token: 0x1700106B RID: 4203
	// (get) Token: 0x06008C85 RID: 35973 RVA: 0x00357EF8 File Offset: 0x003560F8
	// (set) Token: 0x06008C86 RID: 35974 RVA: 0x00357F00 File Offset: 0x00356100
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

	// Token: 0x06008C87 RID: 35975 RVA: 0x00357F28 File Offset: 0x00356128
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

	// Token: 0x06008C88 RID: 35976 RVA: 0x00357F8E File Offset: 0x0035618E
	public XUiTweenWidth(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008C89 RID: 35977 RVA: 0x00357FA7 File Offset: 0x003561A7
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenWidth>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067CB RID: 26571
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenWidth tween;

	// Token: 0x040067CC RID: 26572
	[PublicizedFrom(EAccessModifier.Private)]
	public int start = 20;

	// Token: 0x040067CD RID: 26573
	[PublicizedFrom(EAccessModifier.Private)]
	public int end = 40;
}
