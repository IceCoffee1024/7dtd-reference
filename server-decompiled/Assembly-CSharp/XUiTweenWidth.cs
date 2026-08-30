using System;
using UnityEngine;

// Token: 0x0200114D RID: 4429
public class XUiTweenWidth : XUiTweenAbs
{
	// Token: 0x17001035 RID: 4149
	// (get) Token: 0x06008C18 RID: 35864 RVA: 0x00353AA8 File Offset: 0x00351CA8
	// (set) Token: 0x06008C19 RID: 35865 RVA: 0x00353AB0 File Offset: 0x00351CB0
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

	// Token: 0x17001036 RID: 4150
	// (get) Token: 0x06008C1A RID: 35866 RVA: 0x00353AD8 File Offset: 0x00351CD8
	// (set) Token: 0x06008C1B RID: 35867 RVA: 0x00353AE0 File Offset: 0x00351CE0
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

	// Token: 0x06008C1C RID: 35868 RVA: 0x00353B08 File Offset: 0x00351D08
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

	// Token: 0x06008C1D RID: 35869 RVA: 0x00353B6E File Offset: 0x00351D6E
	public XUiTweenWidth(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008C1E RID: 35870 RVA: 0x00353B87 File Offset: 0x00351D87
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenWidth>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x04006776 RID: 26486
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenWidth tween;

	// Token: 0x04006777 RID: 26487
	[PublicizedFrom(EAccessModifier.Private)]
	public int start = 20;

	// Token: 0x04006778 RID: 26488
	[PublicizedFrom(EAccessModifier.Private)]
	public int end = 40;
}
