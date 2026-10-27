using System;
using UnityEngine;

// Token: 0x02001174 RID: 4468
public class XUiTweenScale : XUiTweenAbs
{
	// Token: 0x17001073 RID: 4211
	// (get) Token: 0x06008CD0 RID: 36048 RVA: 0x00359E34 File Offset: 0x00358034
	// (set) Token: 0x06008CD1 RID: 36049 RVA: 0x00359E3C File Offset: 0x0035803C
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

	// Token: 0x17001074 RID: 4212
	// (get) Token: 0x06008CD2 RID: 36050 RVA: 0x00359E64 File Offset: 0x00358064
	// (set) Token: 0x06008CD3 RID: 36051 RVA: 0x00359E6C File Offset: 0x0035806C
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

	// Token: 0x06008CD4 RID: 36052 RVA: 0x00359E94 File Offset: 0x00358094
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

	// Token: 0x06008CD5 RID: 36053 RVA: 0x00359F0A File Offset: 0x0035810A
	public XUiTweenScale(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008CD6 RID: 36054 RVA: 0x00359F38 File Offset: 0x00358138
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenScale>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x04006808 RID: 26632
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenScale tween;

	// Token: 0x04006809 RID: 26633
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 start = Vector3.one;

	// Token: 0x0400680A RID: 26634
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 end = new Vector3(1.25f, 1.25f, 1f);
}
