using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001178 RID: 4472
public abstract class XUiView_WidgetBased : XUiView
{
	// Token: 0x170010BD RID: 4285
	// (get) Token: 0x06008D8D RID: 36237 RVA: 0x0035C13E File Offset: 0x0035A33E
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.widget;
		}
	}

	// Token: 0x170010BE RID: 4286
	// (get) Token: 0x06008D8E RID: 36238 RVA: 0x0035C146 File Offset: 0x0035A346
	public override Vector3 LocalCenter
	{
		get
		{
			return this.widget.localCenter;
		}
	}

	// Token: 0x170010BF RID: 4287
	// (get) Token: 0x06008D8F RID: 36239 RVA: 0x0035C153 File Offset: 0x0035A353
	public override Vector3[] WorldCorners
	{
		get
		{
			return this.widget.worldCorners;
		}
	}

	// Token: 0x170010C0 RID: 4288
	// (get) Token: 0x06008D90 RID: 36240 RVA: 0x0035C160 File Offset: 0x0035A360
	// (set) Token: 0x06008D91 RID: 36241 RVA: 0x0035C168 File Offset: 0x0035A368
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

	// Token: 0x170010C1 RID: 4289
	// (get) Token: 0x06008D92 RID: 36242 RVA: 0x0035C181 File Offset: 0x0035A381
	// (set) Token: 0x06008D93 RID: 36243 RVA: 0x0035C189 File Offset: 0x0035A389
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

	// Token: 0x170010C2 RID: 4290
	// (get) Token: 0x06008D94 RID: 36244 RVA: 0x0035C1A7 File Offset: 0x0035A3A7
	// (set) Token: 0x06008D95 RID: 36245 RVA: 0x0035C1AF File Offset: 0x0035A3AF
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

	// Token: 0x170010C3 RID: 4291
	// (get) Token: 0x06008D96 RID: 36246 RVA: 0x0035C1C8 File Offset: 0x0035A3C8
	// (set) Token: 0x06008D97 RID: 36247 RVA: 0x0035C1D0 File Offset: 0x0035A3D0
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

	// Token: 0x06008D98 RID: 36248 RVA: 0x0035C1E9 File Offset: 0x0035A3E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView_WidgetBased(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008D99 RID: 36249 RVA: 0x0035C1F4 File Offset: 0x0035A3F4
	public override void InitView()
	{
		base.InitView();
		this.widget.pivot = this.pivot;
		this.widget.depth = this.depth;
		this.uiTransform.localScale = Vector3.one;
		this.uiTransform.localPosition = new Vector3((float)base.PaddedPosition.x, (float)base.PaddedPosition.y, 0f);
	}

	// Token: 0x06008D9A RID: 36250 RVA: 0x0035C266 File Offset: 0x0035A466
	public override void SetDefaults(XUiController _parent)
	{
		this.Pivot = UIWidget.Pivot.TopLeft;
		base.SetDefaults(_parent);
	}

	// Token: 0x06008D9B RID: 36251 RVA: 0x0035C278 File Offset: 0x0035A478
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

	// Token: 0x06008D9C RID: 36252 RVA: 0x0035C320 File Offset: 0x0035A520
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

	// Token: 0x06008D9D RID: 36253 RVA: 0x0035C384 File Offset: 0x0035A584
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.widget.keepAspectRatio = this.keepAspectRatio;
		this.widget.aspectRatio = this.aspectRatio;
		base.updateData();
		this.refreshBoxCollider();
	}

	// Token: 0x170010C4 RID: 4292
	// (get) Token: 0x06008D9E RID: 36254 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool anchorsKeepSize
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06008D9F RID: 36255 RVA: 0x0035C3B4 File Offset: 0x0035A5B4
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

	// Token: 0x06008DA0 RID: 36256 RVA: 0x0035C447 File Offset: 0x0035A647
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator <anchorsParsed>g__MarkAsChangedLater|31_0()
	{
		yield return new WaitForEndOfFrame();
		this.widget.MarkAsChanged();
		yield break;
	}

	// Token: 0x0400685E RID: 26718
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget widget;

	// Token: 0x0400685F RID: 26719
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.AspectRatioSource keepAspectRatio;

	// Token: 0x04006860 RID: 26720
	[PublicizedFrom(EAccessModifier.Protected)]
	public float aspectRatio;

	// Token: 0x04006861 RID: 26721
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool autoResizeCollider;

	// Token: 0x04006862 RID: 26722
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.Pivot pivot;
}
