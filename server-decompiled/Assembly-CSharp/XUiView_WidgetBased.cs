using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02001137 RID: 4407
public abstract class XUiView_WidgetBased : XUiView
{
	// Token: 0x17001044 RID: 4164
	// (get) Token: 0x06008BDC RID: 35804 RVA: 0x0035143E File Offset: 0x0034F63E
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.widget;
		}
	}

	// Token: 0x17001045 RID: 4165
	// (get) Token: 0x06008BDD RID: 35805 RVA: 0x00351446 File Offset: 0x0034F646
	public override Vector3 LocalCenter
	{
		get
		{
			return this.widget.localCenter;
		}
	}

	// Token: 0x17001046 RID: 4166
	// (get) Token: 0x06008BDE RID: 35806 RVA: 0x00351453 File Offset: 0x0034F653
	public override Vector3[] WorldCorners
	{
		get
		{
			return this.widget.worldCorners;
		}
	}

	// Token: 0x17001047 RID: 4167
	// (get) Token: 0x06008BDF RID: 35807 RVA: 0x00351460 File Offset: 0x0034F660
	// (set) Token: 0x06008BE0 RID: 35808 RVA: 0x00351468 File Offset: 0x0034F668
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

	// Token: 0x17001048 RID: 4168
	// (get) Token: 0x06008BE1 RID: 35809 RVA: 0x00351481 File Offset: 0x0034F681
	// (set) Token: 0x06008BE2 RID: 35810 RVA: 0x00351489 File Offset: 0x0034F689
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

	// Token: 0x17001049 RID: 4169
	// (get) Token: 0x06008BE3 RID: 35811 RVA: 0x003514A7 File Offset: 0x0034F6A7
	// (set) Token: 0x06008BE4 RID: 35812 RVA: 0x003514AF File Offset: 0x0034F6AF
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

	// Token: 0x1700104A RID: 4170
	// (get) Token: 0x06008BE5 RID: 35813 RVA: 0x003514C8 File Offset: 0x0034F6C8
	// (set) Token: 0x06008BE6 RID: 35814 RVA: 0x003514D0 File Offset: 0x0034F6D0
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

	// Token: 0x06008BE7 RID: 35815 RVA: 0x003514E9 File Offset: 0x0034F6E9
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView_WidgetBased(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008BE8 RID: 35816 RVA: 0x003514F4 File Offset: 0x0034F6F4
	public override void InitView()
	{
		base.InitView();
		this.widget.pivot = this.pivot;
		this.widget.depth = this.depth;
		this.uiTransform.localScale = Vector3.one;
		this.uiTransform.localPosition = new Vector3((float)base.PaddedPosition.x, (float)base.PaddedPosition.y, 0f);
	}

	// Token: 0x06008BE9 RID: 35817 RVA: 0x00351566 File Offset: 0x0034F766
	public override void SetDefaults(XUiController _parent)
	{
		this.Pivot = UIWidget.Pivot.TopLeft;
		base.SetDefaults(_parent);
	}

	// Token: 0x06008BEA RID: 35818 RVA: 0x00351578 File Offset: 0x0034F778
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

	// Token: 0x06008BEB RID: 35819 RVA: 0x00351620 File Offset: 0x0034F820
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

	// Token: 0x06008BEC RID: 35820 RVA: 0x00351684 File Offset: 0x0034F884
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		this.widget.keepAspectRatio = this.keepAspectRatio;
		this.widget.aspectRatio = this.aspectRatio;
		base.updateData();
		this.refreshBoxCollider();
	}

	// Token: 0x1700104B RID: 4171
	// (get) Token: 0x06008BED RID: 35821 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool anchorsKeepSize
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06008BEE RID: 35822 RVA: 0x003516B4 File Offset: 0x0034F8B4
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

	// Token: 0x06008BEF RID: 35823 RVA: 0x00351747 File Offset: 0x0034F947
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator <anchorsParsed>g__MarkAsChangedLater|31_0()
	{
		yield return new WaitForEndOfFrame();
		this.widget.MarkAsChanged();
		yield break;
	}

	// Token: 0x040066FF RID: 26367
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget widget;

	// Token: 0x04006700 RID: 26368
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.AspectRatioSource keepAspectRatio;

	// Token: 0x04006701 RID: 26369
	[PublicizedFrom(EAccessModifier.Protected)]
	public float aspectRatio;

	// Token: 0x04006702 RID: 26370
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool autoResizeCollider;

	// Token: 0x04006703 RID: 26371
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIWidget.Pivot pivot;
}
