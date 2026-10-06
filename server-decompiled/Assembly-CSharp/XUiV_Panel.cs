using System;
using UnityEngine;

// Token: 0x02001174 RID: 4468
public class XUiV_Panel : XUiView
{
	// Token: 0x170010FD RID: 4349
	// (get) Token: 0x06008E15 RID: 36373 RVA: 0x0035C0C1 File Offset: 0x0035A2C1
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.panel;
		}
	}

	// Token: 0x170010FE RID: 4350
	// (get) Token: 0x06008E16 RID: 36374 RVA: 0x0035C0C9 File Offset: 0x0035A2C9
	// (set) Token: 0x06008E17 RID: 36375 RVA: 0x0035C0D1 File Offset: 0x0035A2D1
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

	// Token: 0x170010FF RID: 4351
	// (get) Token: 0x06008E18 RID: 36376 RVA: 0x0035C0EA File Offset: 0x0035A2EA
	// (set) Token: 0x06008E19 RID: 36377 RVA: 0x0035C0F2 File Offset: 0x0035A2F2
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

	// Token: 0x17001100 RID: 4352
	// (get) Token: 0x06008E1A RID: 36378 RVA: 0x0035C110 File Offset: 0x0035A310
	// (set) Token: 0x06008E1B RID: 36379 RVA: 0x0035C118 File Offset: 0x0035A318
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

	// Token: 0x17001101 RID: 4353
	// (get) Token: 0x06008E1C RID: 36380 RVA: 0x0035C136 File Offset: 0x0035A336
	// (set) Token: 0x06008E1D RID: 36381 RVA: 0x0035C13E File Offset: 0x0035A33E
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

	// Token: 0x17001102 RID: 4354
	// (get) Token: 0x06008E1E RID: 36382 RVA: 0x0035C15C File Offset: 0x0035A35C
	// (set) Token: 0x06008E1F RID: 36383 RVA: 0x0035C164 File Offset: 0x0035A364
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

	// Token: 0x17001103 RID: 4355
	// (get) Token: 0x06008E20 RID: 36384 RVA: 0x0035C182 File Offset: 0x0035A382
	// (set) Token: 0x06008E21 RID: 36385 RVA: 0x0035C18F File Offset: 0x0035A38F
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

	// Token: 0x17001104 RID: 4356
	// (get) Token: 0x06008E22 RID: 36386 RVA: 0x0035C19D File Offset: 0x0035A39D
	public override Vector3[] WorldCorners
	{
		get
		{
			return this.panel.worldCorners;
		}
	}

	// Token: 0x06008E23 RID: 36387 RVA: 0x0035C1AC File Offset: 0x0035A3AC
	public XUiV_Panel(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E24 RID: 36388 RVA: 0x0035C200 File Offset: 0x0035A400
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIPanel>();
	}

	// Token: 0x06008E25 RID: 36389 RVA: 0x0035C209 File Offset: 0x0035A409
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.panel = this.uiTransform.gameObject.GetComponent<UIPanel>();
	}

	// Token: 0x06008E26 RID: 36390 RVA: 0x0035C227 File Offset: 0x0035A427
	public override void InitView()
	{
		base.InitView();
		this.panel.depth = this.depth;
		this.updateClipping();
		base.SetDirty();
	}

	// Token: 0x06008E27 RID: 36391 RVA: 0x0035C24C File Offset: 0x0035A44C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		base.updateData();
		this.updateClipping();
		this.refreshBoxCollider();
	}

	// Token: 0x17001105 RID: 4357
	// (get) Token: 0x06008E28 RID: 36392 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool updateClippingOffset
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x06008E29 RID: 36393 RVA: 0x0035C260 File Offset: 0x0035A460
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

	// Token: 0x0400687A RID: 26746
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIPanel panel;

	// Token: 0x0400687B RID: 26747
	[PublicizedFrom(EAccessModifier.Private)]
	public UIDrawCall.Clipping clipping;

	// Token: 0x0400687C RID: 26748
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingOffset = new Vector2(-10000f, -10000f);

	// Token: 0x0400687D RID: 26749
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingSize = new Vector2(-10000f, -10000f);

	// Token: 0x0400687E RID: 26750
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingCenter = new Vector2(-10000f, -10000f);

	// Token: 0x0400687F RID: 26751
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingSoftness;
}
