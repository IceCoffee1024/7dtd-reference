using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001168 RID: 4456
public abstract class XUiView_WidgetBased : XUiView
{
	// Token: 0x170010B2 RID: 4274
	// (get) Token: 0x06008D39 RID: 36153 RVA: 0x0035A092 File Offset: 0x00358292
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.widget;
		}
	}

	// Token: 0x170010B3 RID: 4275
	// (get) Token: 0x06008D3A RID: 36154 RVA: 0x0035A09A File Offset: 0x0035829A
	public override Vector3 LocalCenter
	{
		get
		{
			return this.widget.localCenter;
		}
	}

	// Token: 0x170010B4 RID: 4276
	// (get) Token: 0x06008D3B RID: 36155 RVA: 0x0035A0A7 File Offset: 0x003582A7
	public override Vector3[] WorldCorners
	{
		get
		{
			return this.widget.worldCorners;
		}
	}

	// Token: 0x170010B5 RID: 4277
	// (get) Token: 0x06008D3C RID: 36156 RVA: 0x0035A0B4 File Offset: 0x003582B4
	// (set) Token: 0x06008D3D RID: 36157 RVA: 0x0035A0BC File Offset: 0x003582BC
	[XuiXmlAttribute("keep_aspect_ratio", false)]
	public UIWidget.AspectRatioSource KeepAspectRatio
	{
		get
		{
			return this.keepAspectRatio;
		}
		set
		{
			if (this.keepAspectRatio == value)
			{
				return;
			}
			this.keepAspectRatio = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010B6 RID: 4278
	// (get) Token: 0x06008D3E RID: 36158 RVA: 0x0035A0D5 File Offset: 0x003582D5
	// (set) Token: 0x06008D3F RID: 36159 RVA: 0x0035A0DD File Offset: 0x003582DD
	[XuiXmlAttribute("aspect_ratio", false)]
	public float AspectRatio
	{
		get
		{
			return this.aspectRatio;
		}
		set
		{
			if (Mathf.Approximately(this.aspectRatio, value))
			{
				return;
			}
			this.aspectRatio = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010B7 RID: 4279
	// (get) Token: 0x06008D40 RID: 36160 RVA: 0x0035A0FB File Offset: 0x003582FB
	// (set) Token: 0x06008D41 RID: 36161 RVA: 0x0035A103 File Offset: 0x00358303
	[XuiXmlAttribute("pivot", false)]
	public UIWidget.Pivot Pivot
	{
		get
		{
			return this.pivot;
		}
		set
		{
			if (this.pivot == value)
			{
				return;
			}
			this.pivot = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010B8 RID: 4280
	// (get) Token: 0x06008D42 RID: 36162 RVA: 0x0035A11C File Offset: 0x0035831C
	// (set) Token: 0x06008D43 RID: 36163 RVA: 0x0035A124 File Offset: 0x00358324
	[XuiXmlAttribute("auto_resize_collider", false)]
	public bool AutoResizeCollider
	{
		get
		{
			return this.autoResizeCollider;
		}
		set
		{
			if (this.autoResizeCollider == value)
			{
				return;
			}
			this.autoResizeCollider = value;
			base.SetDirty();
		}
	}

	// Token: 0x06008D44 RID: 36164 RVA: 0x0035A13D File Offset: 0x0035833D
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView_WidgetBased(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008D45 RID: 36165 RVA: 0x0035A148 File Offset: 0x00358348
	public override void InitView()
	{
		base.InitView();
		this.widget.pivot = this.pivot;
		this.widget.depth = this.depth;
		this.uiTransform.localScale = Vector3.one;
		this.uiTransform.localPosition = new Vector3((float)base.PaddedPosition.x, (float)base.PaddedPosition.y, 0f);
	}

	// Token: 0x06008D46 RID: 36166 RVA: 0x0035A1BA File Offset: 0x003583BA
	public override void SetDefaults(XUiController _parent)
	{
		this.Pivot = UIWidget.Pivot.TopLeft;
		base.SetDefaults(_parent);
	}

	// Token: 0x06008D47 RID: 36167 RVA: 0x0035A1CC File Offset: 0x003583CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void refreshBoxCollider()
	{
		if (this.AutoResizeCollider || this.widget.isAnchored)
		{
			this.widget.autoResizeBoxCollider = true;
			return;
		}
		this.collider.center = this.widget.localCenter;
		this.collider.size = new Vector3(this.widget.localSize.x * base.ColliderScale + (float)(2 * base.ColliderPadding.x), this.widget.localSize.y * base.ColliderScale + (float)(2 * base.ColliderPadding.y), 0f);
	}

	// Token: 0x06008D48 RID: 36168 RVA: 0x0035A274 File Offset: 0x00358474
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.anchoredLeftAndRight)
		{
			this.size.x = Mathf.RoundToInt(this.widget.localSize.x);
		}
		if (this.anchoredTopAndBottom)
		{
			this.size.y = Mathf.RoundToInt(this.widget.localSize.y);
		}
	}

	// Token: 0x06008D49 RID: 36169 RVA: 0x0035A2D8 File Offset: 0x003584D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.widget.keepAspectRatio = this.keepAspectRatio;
		this.widget.aspectRatio = this.aspectRatio;
		base.updateData();
		this.refreshBoxCollider();
	}

	// Token: 0x170010B9 RID: 4281
	// (get) Token: 0x06008D4A RID: 36170 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool anchorsKeepSize
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06008D4B RID: 36171 RVA: 0x0035A308 File Offset: 0x00358508
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void anchorsParsed()
	{
		base.anchorsParsed();
		if (this.anchorsKeepSize)
		{
			if (!this.anchoredLeftAndRight && this.widget.width != this.size.x)
			{
				this.widget.width = this.size.x;
			}
			if (!this.anchoredTopAndBottom && this.widget.height != this.size.y)
			{
				this.widget.height = this.size.y;
			}
		}
		ThreadManager.StartCoroutine(this.<anchorsParsed>g__MarkAsChangedLater|31_0());
	}

	// Token: 0x06008D4C RID: 36172 RVA: 0x0035A39B File Offset: 0x0035859B
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator <anchorsParsed>g__MarkAsChangedLater|31_0()
	{
		yield return new WaitForEndOfFrame();
		this.widget.MarkAsChanged();
		yield break;
	}

	// Token: 0x0400681E RID: 26654
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget widget;

	// Token: 0x0400681F RID: 26655
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.AspectRatioSource keepAspectRatio;

	// Token: 0x04006820 RID: 26656
	[PublicizedFrom(EAccessModifier.Protected)]
	public float aspectRatio;

	// Token: 0x04006821 RID: 26657
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool autoResizeCollider;

	// Token: 0x04006822 RID: 26658
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.Pivot pivot;
}
