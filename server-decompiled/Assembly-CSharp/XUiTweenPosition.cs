using System;
using UnityEngine;

// Token: 0x02001162 RID: 4450
public class XUiTweenPosition : XUiTweenAbs
{
	// Token: 0x17001064 RID: 4196
	// (get) Token: 0x06008C6E RID: 35950 RVA: 0x00357B07 File Offset: 0x00355D07
	// (set) Token: 0x06008C6F RID: 35951 RVA: 0x00357B0F File Offset: 0x00355D0F
	[XuiXmlAttribute("start", false)]
	public Vector3 Start
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

	// Token: 0x17001065 RID: 4197
	// (get) Token: 0x06008C70 RID: 35952 RVA: 0x00357B37 File Offset: 0x00355D37
	// (set) Token: 0x06008C71 RID: 35953 RVA: 0x00357B3F File Offset: 0x00355D3F
	[XuiXmlAttribute("end", false)]
	public Vector3 End
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

	// Token: 0x06008C72 RID: 35954 RVA: 0x00357B68 File Offset: 0x00355D68
	[XuiXmlAttribute("play_to", false)]
	public void AttributePlayToEnd(Vector3 _value)
	{
		if (this.tween == null)
		{
			return;
		}
		if ((double)(this.tween.to - _value).sqrMagnitude < 0.1)
		{
			return;
		}
		base.Enabled = true;
		this.tween.tweenFactor = 0f;
		this.tween.SetStartToCurrentValue();
		this.End = _value;
		this.tween.PlayForward();
	}

	// Token: 0x06008C73 RID: 35955 RVA: 0x00357BDE File Offset: 0x00355DDE
	public XUiTweenPosition(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008C74 RID: 35956 RVA: 0x00357C0C File Offset: 0x00355E0C
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenPosition>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x040067C2 RID: 26562
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenPosition tween;

	// Token: 0x040067C3 RID: 26563
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 start = Vector3.zero;

	// Token: 0x040067C4 RID: 26564
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 end = new Vector3(0f, 8f, 0f);
}
