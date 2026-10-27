using System;
using UnityEngine;

// Token: 0x02001171 RID: 4465
public class XUiTweenHeight : XUiTweenAbs
{
	// Token: 0x1700106D RID: 4205
	// (get) Token: 0x06008CBB RID: 36027 RVA: 0x00359A97 File Offset: 0x00357C97
	// (set) Token: 0x06008CBC RID: 36028 RVA: 0x00359A9F File Offset: 0x00357C9F
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

	// Token: 0x1700106E RID: 4206
	// (get) Token: 0x06008CBD RID: 36029 RVA: 0x00359AC7 File Offset: 0x00357CC7
	// (set) Token: 0x06008CBE RID: 36030 RVA: 0x00359ACF File Offset: 0x00357CCF
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

	// Token: 0x06008CBF RID: 36031 RVA: 0x00359AF8 File Offset: 0x00357CF8
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

	// Token: 0x06008CC0 RID: 36032 RVA: 0x00359B5E File Offset: 0x00357D5E
	public XUiTweenHeight(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008CC1 RID: 36033 RVA: 0x00359B77 File Offset: 0x00357D77
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenHeight>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067FF RID: 26623
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenHeight tween;

	// Token: 0x04006800 RID: 26624
	[PublicizedFrom(EAccessModifier.Private)]
	public int start = 20;

	// Token: 0x04006801 RID: 26625
	[PublicizedFrom(EAccessModifier.Private)]
	public int end = 40;
}
