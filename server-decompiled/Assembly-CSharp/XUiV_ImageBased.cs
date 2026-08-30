using System;
using UnityEngine;

// Token: 0x0200113F RID: 4415
public abstract class XUiV_ImageBased : XUiView_WidgetBased
{
	// Token: 0x1700106C RID: 4204
	// (get) Token: 0x06008C53 RID: 35923 RVA: 0x003528FD File Offset: 0x00350AFD
	// (set) Token: 0x06008C54 RID: 35924 RVA: 0x00352905 File Offset: 0x00350B05
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

	// Token: 0x1700106D RID: 4205
	// (get) Token: 0x06008C55 RID: 35925 RVA: 0x00351D51 File Offset: 0x0034FF51
	// (set) Token: 0x06008C56 RID: 35926 RVA: 0x0035291E File Offset: 0x00350B1E
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

	// Token: 0x1700106E RID: 4206
	// (get) Token: 0x06008C57 RID: 35927 RVA: 0x00352937 File Offset: 0x00350B37
	// (set) Token: 0x06008C58 RID: 35928 RVA: 0x0035293F File Offset: 0x00350B3F
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

	// Token: 0x1700106F RID: 4207
	// (get) Token: 0x06008C59 RID: 35929 RVA: 0x00352958 File Offset: 0x00350B58
	// (set) Token: 0x06008C5A RID: 35930 RVA: 0x00352960 File Offset: 0x00350B60
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

	// Token: 0x17001070 RID: 4208
	// (get) Token: 0x06008C5B RID: 35931 RVA: 0x00352979 File Offset: 0x00350B79
	// (set) Token: 0x06008C5C RID: 35932 RVA: 0x00352981 File Offset: 0x00350B81
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

	// Token: 0x17001071 RID: 4209
	// (get) Token: 0x06008C5D RID: 35933 RVA: 0x0035299F File Offset: 0x00350B9F
	// (set) Token: 0x06008C5E RID: 35934 RVA: 0x003529A7 File Offset: 0x00350BA7
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

	// Token: 0x17001072 RID: 4210
	// (get) Token: 0x06008C5F RID: 35935 RVA: 0x003529C0 File Offset: 0x00350BC0
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

	// Token: 0x06008C60 RID: 35936 RVA: 0x003529E4 File Offset: 0x00350BE4
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

	// Token: 0x06008C61 RID: 35937 RVA: 0x00352A1C File Offset: 0x00350C1C
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

	// Token: 0x06008C62 RID: 35938 RVA: 0x00352A76 File Offset: 0x00350C76
	public XUiV_ImageBased(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008C63 RID: 35939 RVA: 0x00352A8B File Offset: 0x00350C8B
	public override void SetDefaults(XUiController _parent)
	{
		base.SetDefaults(_parent);
		this.FillCenter = true;
		this.Type = UIBasicSprite.Type.Simple;
	}

	// Token: 0x04006731 RID: 26417
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool keepSourceAspectRatio;

	// Token: 0x04006732 RID: 26418
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIBasicSprite.Type type;

	// Token: 0x04006733 RID: 26419
	[PublicizedFrom(EAccessModifier.Protected)]
	public UIBasicSprite.Flip flip;

	// Token: 0x04006734 RID: 26420
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool fillCenter;

	// Token: 0x04006735 RID: 26421
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool foregroundLayer;

	// Token: 0x04006736 RID: 26422
	[PublicizedFrom(EAccessModifier.Private)]
	public float globalOpacityModifier = 1f;

	// Token: 0x04006737 RID: 26423
	[PublicizedFrom(EAccessModifier.Private)]
	public float previousGlobalOpacity;
}
