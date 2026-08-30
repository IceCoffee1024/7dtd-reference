using System;
using UnityEngine;

// Token: 0x0200114A RID: 4426
public class XUiTweenPosition : XUiTweenAbs
{
	// Token: 0x1700102F RID: 4143
	// (get) Token: 0x06008C03 RID: 35843 RVA: 0x003536E7 File Offset: 0x003518E7
	// (set) Token: 0x06008C04 RID: 35844 RVA: 0x003536EF File Offset: 0x003518EF
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

	// Token: 0x17001030 RID: 4144
	// (get) Token: 0x06008C05 RID: 35845 RVA: 0x00353717 File Offset: 0x00351917
	// (set) Token: 0x06008C06 RID: 35846 RVA: 0x0035371F File Offset: 0x0035191F
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

	// Token: 0x06008C07 RID: 35847 RVA: 0x00353748 File Offset: 0x00351948
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

	// Token: 0x06008C08 RID: 35848 RVA: 0x003537BE File Offset: 0x003519BE
	public XUiTweenPosition(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008C09 RID: 35849 RVA: 0x003537EC File Offset: 0x003519EC
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenPosition>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x0400676D RID: 26477
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenPosition tween;

	// Token: 0x0400676E RID: 26478
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 start = Vector3.zero;

	// Token: 0x0400676F RID: 26479
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 end = new Vector3(0f, 8f, 0f);
}
