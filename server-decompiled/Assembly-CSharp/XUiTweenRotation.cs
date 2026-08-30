using System;
using UnityEngine;

// Token: 0x0200114B RID: 4427
public class XUiTweenRotation : XUiTweenAbs
{
	// Token: 0x17001031 RID: 4145
	// (get) Token: 0x06008C0A RID: 35850 RVA: 0x00353828 File Offset: 0x00351A28
	// (set) Token: 0x06008C0B RID: 35851 RVA: 0x00353830 File Offset: 0x00351A30
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

	// Token: 0x17001032 RID: 4146
	// (get) Token: 0x06008C0C RID: 35852 RVA: 0x00353858 File Offset: 0x00351A58
	// (set) Token: 0x06008C0D RID: 35853 RVA: 0x00353860 File Offset: 0x00351A60
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

	// Token: 0x06008C0E RID: 35854 RVA: 0x00353888 File Offset: 0x00351A88
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

	// Token: 0x06008C0F RID: 35855 RVA: 0x003538FE File Offset: 0x00351AFE
	public XUiTweenRotation(XUiView _targetView)
		: base(_targetView)
	{
	}

	// Token: 0x06008C10 RID: 35856 RVA: 0x0035392C File Offset: 0x00351B2C
	public override void CreateTween(GameObject _uiGameObject)
	{
		this.tween = _uiGameObject.AddMissingComponent<TweenRotation>();
		this.tween.from = this.start;
		this.tween.to = this.end;
		base.setCommonTweenValues(this.tween);
	}

	// Token: 0x04006770 RID: 26480
	[PublicizedFrom(EAccessModifier.Private)]
	public TweenRotation tween;

	// Token: 0x04006771 RID: 26481
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 start = Vector3.zero;

	// Token: 0x04006772 RID: 26482
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 end = new Vector3(0f, 0f, 360f);
}
