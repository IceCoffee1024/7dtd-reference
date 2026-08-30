using System;
using UnityEngine;

// Token: 0x02001143 RID: 4419
public class XUiV_Panel : XUiView
{
	// Token: 0x1700108F RID: 4239
	// (get) Token: 0x06008CB8 RID: 36024 RVA: 0x00353465 File Offset: 0x00351665
	public override UIRect UiRect
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.panel;
		}
	}

	// Token: 0x17001090 RID: 4240
	// (get) Token: 0x06008CB9 RID: 36025 RVA: 0x0035346D File Offset: 0x0035166D
	// (set) Token: 0x06008CBA RID: 36026 RVA: 0x00353475 File Offset: 0x00351675
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

	// Token: 0x17001091 RID: 4241
	// (get) Token: 0x06008CBB RID: 36027 RVA: 0x0035348E File Offset: 0x0035168E
	// (set) Token: 0x06008CBC RID: 36028 RVA: 0x00353496 File Offset: 0x00351696
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

	// Token: 0x17001092 RID: 4242
	// (get) Token: 0x06008CBD RID: 36029 RVA: 0x003534B4 File Offset: 0x003516B4
	// (set) Token: 0x06008CBE RID: 36030 RVA: 0x003534BC File Offset: 0x003516BC
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

	// Token: 0x17001093 RID: 4243
	// (get) Token: 0x06008CBF RID: 36031 RVA: 0x003534DA File Offset: 0x003516DA
	// (set) Token: 0x06008CC0 RID: 36032 RVA: 0x003534E2 File Offset: 0x003516E2
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

	// Token: 0x17001094 RID: 4244
	// (get) Token: 0x06008CC1 RID: 36033 RVA: 0x00353500 File Offset: 0x00351700
	// (set) Token: 0x06008CC2 RID: 36034 RVA: 0x00353508 File Offset: 0x00351708
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

	// Token: 0x17001095 RID: 4245
	// (get) Token: 0x06008CC3 RID: 36035 RVA: 0x00353526 File Offset: 0x00351726
	// (set) Token: 0x06008CC4 RID: 36036 RVA: 0x00353533 File Offset: 0x00351733
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

	// Token: 0x17001096 RID: 4246
	// (get) Token: 0x06008CC5 RID: 36037 RVA: 0x00353541 File Offset: 0x00351741
	public override Vector3[] WorldCorners
	{
		get
		{
			return this.panel.worldCorners;
		}
	}

	// Token: 0x06008CC6 RID: 36038 RVA: 0x00353550 File Offset: 0x00351750
	public XUiV_Panel(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008CC7 RID: 36039 RVA: 0x003535A4 File Offset: 0x003517A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		_go.AddComponent<UIPanel>();
	}

	// Token: 0x06008CC8 RID: 36040 RVA: 0x003535AD File Offset: 0x003517AD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.panel = this.uiTransform.gameObject.GetComponent<UIPanel>();
	}

	// Token: 0x06008CC9 RID: 36041 RVA: 0x003535CB File Offset: 0x003517CB
	public override void InitView()
	{
		base.InitView();
		this.panel.depth = this.depth;
		this.updateClipping();
		base.SetDirty();
	}

	// Token: 0x06008CCA RID: 36042 RVA: 0x003535F0 File Offset: 0x003517F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		base.updateData();
		this.updateClipping();
		this.refreshBoxCollider();
	}

	// Token: 0x06008CCB RID: 36043 RVA: 0x00353604 File Offset: 0x00351804
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
		if (zero == new Vector2(-10000f, -10000f))
		{
			zero = Vector2.zero;
		}
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
		this.panel.clipOffset = zero;
	}

	// Token: 0x0400675B RID: 26459
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIPanel panel;

	// Token: 0x0400675C RID: 26460
	[PublicizedFrom(EAccessModifier.Private)]
	public UIDrawCall.Clipping clipping;

	// Token: 0x0400675D RID: 26461
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingOffset = new Vector2(-10000f, -10000f);

	// Token: 0x0400675E RID: 26462
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingSize = new Vector2(-10000f, -10000f);

	// Token: 0x0400675F RID: 26463
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingCenter = new Vector2(-10000f, -10000f);

	// Token: 0x04006760 RID: 26464
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 clippingSoftness;
}
