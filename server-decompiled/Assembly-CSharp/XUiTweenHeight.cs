using System;
using UnityEngine;

// Token: 0x02001130 RID: 4400
public class XUiTweenHeight : XUiTweenAbs
{
	// Token: 0x17000FF4 RID: 4084
	// (get) Token: 0x06008B0A RID: 35594 RVA: 0x0034ED97 File Offset: 0x0034CF97
	// (set) Token: 0x06008B0B RID: 35595 RVA: 0x0034ED9F File Offset: 0x0034CF9F
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

	// Token: 0x17000FF5 RID: 4085
	// (get) Token: 0x06008B0C RID: 35596 RVA: 0x0034EDC7 File Offset: 0x0034CFC7
	// (set) Token: 0x06008B0D RID: 35597 RVA: 0x0034EDCF File Offset: 0x0034CFCF
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

	// Token: 0x06008B0E RID: 35598 RVA: 0x0034EDF8 File Offset: 0x0034CFF8
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

	// Token: 0x06008B0F RID: 35599 RVA: 0x0034EE5E File Offset: 0x0034D05E
	public XUiTweenHeight(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008B10 RID: 35600 RVA: 0x0034EE77 File Offset: 0x0034D077
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenHeight>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040066A0 RID: 26272
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenHeight tween;

	// Token: 0x040066A1 RID: 26273
	[PublicizedFrom(EAccessModifier.Private)]
	public int start = 20;

	// Token: 0x040066A2 RID: 26274
	[PublicizedFrom(EAccessModifier.Private)]
	public int end = 40;
}
