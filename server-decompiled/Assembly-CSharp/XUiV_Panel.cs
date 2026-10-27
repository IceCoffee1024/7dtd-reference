using System;
using UnityEngine;

// Token: 0x02001184 RID: 4484
public class XUiV_Panel : XUiView
{
	// Token: 0x17001108 RID: 4360
	// (get) Token: 0x06008E69 RID: 36457 RVA: 0x0035E191 File Offset: 0x0035C391
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.panel;
		}
	}

	// Token: 0x17001109 RID: 4361
	// (get) Token: 0x06008E6A RID: 36458 RVA: 0x0035E199 File Offset: 0x0035C399
	// (set) Token: 0x06008E6B RID: 36459 RVA: 0x0035E1A1 File Offset: 0x0035C3A1
	[XuiXmlAttribute("clipping", false)]
	public UIDrawCall.Clipping Clipping
	{
		get
		{
			return this.clipping;
		}
		set
		{
			if (value == this.clipping)
			{
				return;
			}
			this.clipping = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700110A RID: 4362
	// (get) Token: 0x06008E6C RID: 36460 RVA: 0x0035E1BA File Offset: 0x0035C3BA
	// (set) Token: 0x06008E6D RID: 36461 RVA: 0x0035E1C2 File Offset: 0x0035C3C2
	[XuiXmlAttribute("clippingsize", false)]
	public Vector2 ClippingSize
	{
		get
		{
			return this.clippingSize;
		}
		set
		{
			if (value == this.clippingSize)
			{
				return;
			}
			this.clippingSize = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700110B RID: 4363
	// (get) Token: 0x06008E6E RID: 36462 RVA: 0x0035E1E0 File Offset: 0x0035C3E0
	// (set) Token: 0x06008E6F RID: 36463 RVA: 0x0035E1E8 File Offset: 0x0035C3E8
	[XuiXmlAttribute("clippingcenter", false)]
	public Vector2 ClippingCenter
	{
		get
		{
			return this.clippingCenter;
		}
		set
		{
			if (value == this.clippingCenter)
			{
				return;
			}
			this.clippingCenter = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700110C RID: 4364
	// (get) Token: 0x06008E70 RID: 36464 RVA: 0x0035E206 File Offset: 0x0035C406
	// (set) Token: 0x06008E71 RID: 36465 RVA: 0x0035E20E File Offset: 0x0035C40E
	[XuiXmlAttribute("clippingsoftness", false)]
	public Vector2 ClippingSoftness
	{
		get
		{
			return this.clippingSoftness;
		}
		set
		{
			if (value == this.clippingSoftness)
			{
				return;
			}
			this.clippingSoftness = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700110D RID: 4365
	// (get) Token: 0x06008E72 RID: 36466 RVA: 0x0035E22C File Offset: 0x0035C42C
	// (set) Token: 0x06008E73 RID: 36467 RVA: 0x0035E234 File Offset: 0x0035C434
	[XuiXmlAttribute("clippingoffset", false)]
	public Vector2 ClipOffset
	{
		get
		{
			return this.clippingOffset;
		}
		set
		{
			if (value == this.clippingOffset)
			{
				return;
			}
			this.clippingOffset = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700110E RID: 4366
	// (get) Token: 0x06008E74 RID: 36468 RVA: 0x0035E252 File Offset: 0x0035C452
	// (set) Token: 0x06008E75 RID: 36469 RVA: 0x0035E25F File Offset: 0x0035C45F
	public bool StaticWidgets
	{
		get
		{
			return this.panel.widgetsAreStatic;
		}
		set
		{
			this.panel.widgetsAreStatic = value;
		}
	}

	// Token: 0x1700110F RID: 4367
	// (get) Token: 0x06008E76 RID: 36470 RVA: 0x0035E26D File Offset: 0x0035C46D
	public override Vector3[] WorldCorners
	{
		get
		{
			return this.panel.worldCorners;
		}
	}

	// Token: 0x06008E77 RID: 36471 RVA: 0x0035E27C File Offset: 0x0035C47C
	public XUiV_Panel(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E78 RID: 36472 RVA: 0x0035E2D0 File Offset: 0x0035C4D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIPanel>();
	}

	// Token: 0x06008E79 RID: 36473 RVA: 0x0035E2D9 File Offset: 0x0035C4D9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.panel = this.uiTransform.gameObject.GetComponent<UIPanel>();
	}

	// Token: 0x06008E7A RID: 36474 RVA: 0x0035E2F7 File Offset: 0x0035C4F7
	public override void InitView()
	{
		base.InitView();
		this.panel.depth = this.depth;
		this.updateClipping();
		base.SetDirty();
	}

	// Token: 0x06008E7B RID: 36475 RVA: 0x0035E31C File Offset: 0x0035C51C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		base.updateData();
		this.updateClipping();
		this.refreshBoxCollider();
	}

	// Token: 0x17001110 RID: 4368
	// (get) Token: 0x06008E7C RID: 36476 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool updateClippingOffset
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06008E7D RID: 36477 RVA: 0x0035E330 File Offset: 0x0035C530
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateClipping()
	{
		if (this.clipping == UIDrawCall.Clipping.None)
		{
			return;
		}
		this.panel.clipping = this.clipping;
		Vector2 zero = this.clippingOffset;
		Vector2 vector = this.clippingCenter;
		Vector2 vector2 = this.clippingSize;
		Vector2 vector3 = this.clippingSoftness;
		if (vector == new Vector2(-10000f, -10000f))
		{
			vector = new Vector2((float)this.size.x / 2f, (float)(-(float)this.size.y) / 2f);
		}
		if (vector2 == new Vector2(-10000f, -10000f))
		{
			vector2 = new Vector2((float)this.size.x, (float)this.size.y);
		}
		if (vector2.x < 0f)
		{
			vector2.x = 0f;
		}
		if (vector2.y < 0f)
		{
			vector2.y = 0f;
		}
		this.panel.clipSoftness = vector3;
		this.panel.baseClipRegion = new Vector4(vector.x, vector.y, vector2.x, vector2.y);
		if (this.updateClippingOffset)
		{
			if (zero == new Vector2(-10000f, -10000f))
			{
				zero = Vector2.zero;
			}
			this.panel.clipOffset = zero;
		}
	}

	// Token: 0x040068BA RID: 26810
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIPanel panel;

	// Token: 0x040068BB RID: 26811
	[PublicizedFrom(EAccessModifier.Private)]
	public UIDrawCall.Clipping clipping;

	// Token: 0x040068BC RID: 26812
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingOffset = new Vector2(-10000f, -10000f);

	// Token: 0x040068BD RID: 26813
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingSize = new Vector2(-10000f, -10000f);

	// Token: 0x040068BE RID: 26814
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingCenter = new Vector2(-10000f, -10000f);

	// Token: 0x040068BF RID: 26815
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingSoftness;
}
