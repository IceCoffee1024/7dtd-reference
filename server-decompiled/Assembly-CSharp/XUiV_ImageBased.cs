using System;
using UnityEngine;

// Token: 0x02001180 RID: 4480
public abstract class XUiV_ImageBased : XUiView_WidgetBased
{
	// Token: 0x170010E5 RID: 4325
	// (get) Token: 0x06008E04 RID: 36356 RVA: 0x0035D629 File Offset: 0x0035B829
	// (set) Token: 0x06008E05 RID: 36357 RVA: 0x0035D631 File Offset: 0x0035B831
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

	// Token: 0x170010E6 RID: 4326
	// (get) Token: 0x06008E06 RID: 36358 RVA: 0x0035CA51 File Offset: 0x0035AC51
	// (set) Token: 0x06008E07 RID: 36359 RVA: 0x0035D64A File Offset: 0x0035B84A
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

	// Token: 0x170010E7 RID: 4327
	// (get) Token: 0x06008E08 RID: 36360 RVA: 0x0035D663 File Offset: 0x0035B863
	// (set) Token: 0x06008E09 RID: 36361 RVA: 0x0035D66B File Offset: 0x0035B86B
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

	// Token: 0x170010E8 RID: 4328
	// (get) Token: 0x06008E0A RID: 36362 RVA: 0x0035D684 File Offset: 0x0035B884
	// (set) Token: 0x06008E0B RID: 36363 RVA: 0x0035D68C File Offset: 0x0035B88C
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

	// Token: 0x170010E9 RID: 4329
	// (get) Token: 0x06008E0C RID: 36364 RVA: 0x0035D6A5 File Offset: 0x0035B8A5
	// (set) Token: 0x06008E0D RID: 36365 RVA: 0x0035D6AD File Offset: 0x0035B8AD
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

	// Token: 0x170010EA RID: 4330
	// (get) Token: 0x06008E0E RID: 36366 RVA: 0x0035D6CB File Offset: 0x0035B8CB
	// (set) Token: 0x06008E0F RID: 36367 RVA: 0x0035D6D3 File Offset: 0x0035B8D3
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

	// Token: 0x170010EB RID: 4331
	// (get) Token: 0x06008E10 RID: 36368 RVA: 0x0035D6EC File Offset: 0x0035B8EC
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

	// Token: 0x06008E11 RID: 36369 RVA: 0x0035D710 File Offset: 0x0035B910
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

	// Token: 0x06008E12 RID: 36370 RVA: 0x0035D748 File Offset: 0x0035B948
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

	// Token: 0x06008E13 RID: 36371 RVA: 0x0035D7A2 File Offset: 0x0035B9A2
	public XUiV_ImageBased(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E14 RID: 36372 RVA: 0x0035D7B7 File Offset: 0x0035B9B7
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.FillCenter = true;
		this.Type = UIBasicSprite.Type.Simple;
	}

	// Token: 0x04006890 RID: 26768
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool keepSourceAspectRatio;

	// Token: 0x04006891 RID: 26769
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIBasicSprite.Type type;

	// Token: 0x04006892 RID: 26770
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIBasicSprite.Flip flip;

	// Token: 0x04006893 RID: 26771
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fillCenter;

	// Token: 0x04006894 RID: 26772
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool foregroundLayer;

	// Token: 0x04006895 RID: 26773
	[PublicizedFrom(EAccessModifier.Private)]
	public float globalOpacityModifier = 1f;

	// Token: 0x04006896 RID: 26774
	[PublicizedFrom(EAccessModifier.Private)]
	public float previousGlobalOpacity;
}
