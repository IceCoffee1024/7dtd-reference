using System;
using UnityEngine;

// Token: 0x02001170 RID: 4464
public abstract class XUiV_ImageBased : XUiView_WidgetBased
{
	// Token: 0x170010DA RID: 4314
	// (get) Token: 0x06008DB0 RID: 36272 RVA: 0x0035B559 File Offset: 0x00359759
	// (set) Token: 0x06008DB1 RID: 36273 RVA: 0x0035B561 File Offset: 0x00359761
	[XuiXmlAttribute("keepsourceaspectratio", false)]
	public bool KeepSourceAspectRatio
	{
		get
		{
			return this.keepSourceAspectRatio;
		}
		set
		{
			if (value == this.keepSourceAspectRatio)
			{
				return;
			}
			this.keepSourceAspectRatio = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010DB RID: 4315
	// (get) Token: 0x06008DB2 RID: 36274 RVA: 0x0035A9A5 File Offset: 0x00358BA5
	// (set) Token: 0x06008DB3 RID: 36275 RVA: 0x0035B57A File Offset: 0x0035977A
	[XuiXmlAttribute("type", false)]
	public virtual UIBasicSprite.Type Type
	{
		get
		{
			return this.type;
		}
		set
		{
			if (this.type == value)
			{
				return;
			}
			this.type = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010DC RID: 4316
	// (get) Token: 0x06008DB4 RID: 36276 RVA: 0x0035B593 File Offset: 0x00359793
	// (set) Token: 0x06008DB5 RID: 36277 RVA: 0x0035B59B File Offset: 0x0035979B
	[XuiXmlAttribute("fillcenter", false)]
	public bool FillCenter
	{
		get
		{
			return this.fillCenter;
		}
		set
		{
			if (this.fillCenter == value)
			{
				return;
			}
			this.fillCenter = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010DD RID: 4317
	// (get) Token: 0x06008DB6 RID: 36278 RVA: 0x0035B5B4 File Offset: 0x003597B4
	// (set) Token: 0x06008DB7 RID: 36279 RVA: 0x0035B5BC File Offset: 0x003597BC
	[XuiXmlAttribute("flip", false)]
	public UIBasicSprite.Flip Flip
	{
		get
		{
			return this.flip;
		}
		set
		{
			if (this.flip == value)
			{
				return;
			}
			this.flip = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010DE RID: 4318
	// (get) Token: 0x06008DB8 RID: 36280 RVA: 0x0035B5D5 File Offset: 0x003597D5
	// (set) Token: 0x06008DB9 RID: 36281 RVA: 0x0035B5DD File Offset: 0x003597DD
	[XuiXmlAttribute("globalopacitymod", false)]
	public float GlobalOpacityModifier
	{
		get
		{
			return this.globalOpacityModifier;
		}
		set
		{
			if (Mathf.Approximately(this.globalOpacityModifier, value))
			{
				return;
			}
			this.globalOpacityModifier = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010DF RID: 4319
	// (get) Token: 0x06008DBA RID: 36282 RVA: 0x0035B5FB File Offset: 0x003597FB
	// (set) Token: 0x06008DBB RID: 36283 RVA: 0x0035B603 File Offset: 0x00359803
	[XuiXmlAttribute("foregroundlayer", false)]
	public bool ForegroundLayer
	{
		get
		{
			return this.foregroundLayer;
		}
		set
		{
			if (this.foregroundLayer == value)
			{
				return;
			}
			this.foregroundLayer = value;
			base.SetDirty();
		}
	}

	// Token: 0x170010E0 RID: 4320
	// (get) Token: 0x06008DBC RID: 36284 RVA: 0x0035B61C File Offset: 0x0035981C
	public float GlobalOpacitySetting
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!this.foregroundLayer)
			{
				return this.xui.BackgroundGlobalOpacity;
			}
			return this.xui.ForegroundGlobalOpacity;
		}
	}

	// Token: 0x06008DBD RID: 36285 RVA: 0x0035B640 File Offset: 0x00359840
	public override void Update(float _dt)
	{
		float globalOpacitySetting = this.GlobalOpacitySetting;
		if (!Mathf.Approximately(this.previousGlobalOpacity, globalOpacitySetting))
		{
			this.previousGlobalOpacity = globalOpacitySetting;
			base.SetDirty();
		}
		base.Update(_dt);
	}

	// Token: 0x06008DBE RID: 36286 RVA: 0x0035B678 File Offset: 0x00359878
	[PublicizedFrom(EAccessModifier.Protected)]
	public Color opacityModColor(Color _origColor)
	{
		if (this.globalOpacityModifier == 0f)
		{
			return _origColor;
		}
		float globalOpacitySetting = this.GlobalOpacitySetting;
		if (globalOpacitySetting >= 0.999f)
		{
			return _origColor;
		}
		float num = Mathf.Clamp01(_origColor.a * this.globalOpacityModifier * globalOpacitySetting);
		return new Color(_origColor.r, _origColor.g, _origColor.b, num);
	}

	// Token: 0x06008DBF RID: 36287 RVA: 0x0035B6D2 File Offset: 0x003598D2
	public XUiV_ImageBased(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008DC0 RID: 36288 RVA: 0x0035B6E7 File Offset: 0x003598E7
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.FillCenter = true;
		this.Type = UIBasicSprite.Type.Simple;
	}

	// Token: 0x04006850 RID: 26704
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool keepSourceAspectRatio;

	// Token: 0x04006851 RID: 26705
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIBasicSprite.Type type;

	// Token: 0x04006852 RID: 26706
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIBasicSprite.Flip flip;

	// Token: 0x04006853 RID: 26707
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fillCenter;

	// Token: 0x04006854 RID: 26708
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool foregroundLayer;

	// Token: 0x04006855 RID: 26709
	[PublicizedFrom(EAccessModifier.Private)]
	public float globalOpacityModifier = 1f;

	// Token: 0x04006856 RID: 26710
	[PublicizedFrom(EAccessModifier.Private)]
	public float previousGlobalOpacity;
}
