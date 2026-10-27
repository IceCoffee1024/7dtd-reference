using System;
using UnityEngine;

// Token: 0x0200118D RID: 4493
public class XUiV_TextureBased : XUiV_ImageBased
{
	// Token: 0x17001145 RID: 4421
	// (get) Token: 0x06008F22 RID: 36642 RVA: 0x0035FEBA File Offset: 0x0035E0BA
	// (set) Token: 0x06008F23 RID: 36643 RVA: 0x0035FEC2 File Offset: 0x0035E0C2
	public virtual Texture Texture
	{
		get
		{
			return this.texture;
		}
		set
		{
			this.texture = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001146 RID: 4422
	// (get) Token: 0x06008F24 RID: 36644 RVA: 0x0035FED1 File Offset: 0x0035E0D1
	// (set) Token: 0x06008F25 RID: 36645 RVA: 0x0035FED9 File Offset: 0x0035E0D9
	public Material Material
	{
		get
		{
			return this.material;
		}
		set
		{
			this.material = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001147 RID: 4423
	// (get) Token: 0x06008F26 RID: 36646 RVA: 0x0035FEE8 File Offset: 0x0035E0E8
	// (set) Token: 0x06008F27 RID: 36647 RVA: 0x0035FEF5 File Offset: 0x0035E0F5
	public UIDrawCall.OnRenderCallback OnRenderTexture
	{
		get
		{
			return this.uiTexture.onRender;
		}
		set
		{
			this.uiTexture.onRender = value;
		}
	}

	// Token: 0x17001148 RID: 4424
	// (get) Token: 0x06008F28 RID: 36648 RVA: 0x0035FF03 File Offset: 0x0035E103
	// (set) Token: 0x06008F29 RID: 36649 RVA: 0x0035FF0B File Offset: 0x0035E10B
	public Rect UVRect
	{
		get
		{
			return this.uvRect;
		}
		set
		{
			this.uvRect = value;
			base.SetDirty();
		}
	}

	// Token: 0x17001149 RID: 4425
	// (get) Token: 0x06008F2A RID: 36650 RVA: 0x0035FF1A File Offset: 0x0035E11A
	// (set) Token: 0x06008F2B RID: 36651 RVA: 0x0035FF22 File Offset: 0x0035E122
	public Vector4 Border
	{
		get
		{
			return this.border;
		}
		set
		{
			this.border = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700114A RID: 4426
	// (get) Token: 0x06008F2C RID: 36652 RVA: 0x0035FF31 File Offset: 0x0035E131
	// (set) Token: 0x06008F2D RID: 36653 RVA: 0x0035FF39 File Offset: 0x0035E139
	[XuiXmlAttribute("color", false)]
	public Color Color
	{
		get
		{
			return this.color;
		}
		set
		{
			this.color = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700114B RID: 4427
	// (get) Token: 0x06008F2E RID: 36654 RVA: 0x0035FF48 File Offset: 0x0035E148
	// (set) Token: 0x06008F2F RID: 36655 RVA: 0x0035FF50 File Offset: 0x0035E150
	public UIBasicSprite.FillDirection FillDirection
	{
		get
		{
			return this.fillDirection;
		}
		set
		{
			this.fillDirection = value;
			base.SetDirty();
		}
	}

	// Token: 0x1700114C RID: 4428
	// (get) Token: 0x06008F30 RID: 36656 RVA: 0x0035FF5F File Offset: 0x0035E15F
	// (set) Token: 0x06008F31 RID: 36657 RVA: 0x0035FF67 File Offset: 0x0035E167
	[XuiXmlAttribute("sourceaspectratiorespectpivot", false)]
	public bool SourceAspectRatioRespectPivot
	{
		get
		{
			return this.sourceAspectRatioRespectPivot;
		}
		set
		{
			if (this.sourceAspectRatioRespectPivot == value)
			{
				return;
			}
			this.sourceAspectRatioRespectPivot = value;
			base.SetDirty();
		}
	}

	// Token: 0x06008F32 RID: 36658 RVA: 0x0035FF80 File Offset: 0x0035E180
	public XUiV_TextureBased(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008F33 RID: 36659 RVA: 0x0035FFBF File Offset: 0x0035E1BF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UITexture>();
	}

	// Token: 0x06008F34 RID: 36660 RVA: 0x0035FFD0 File Offset: 0x0035E1D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.uiTexture = this.uiTransform.gameObject.GetComponent<UITexture>());
	}

	// Token: 0x06008F35 RID: 36661 RVA: 0x00360004 File Offset: 0x0035E204
	public void CreateMaterial(string _shaderName = "Unlit/Transparent Colored Emissive TextureArray")
	{
		Shader shader = GlobalAssets.FindShader(_shaderName);
		if (shader == null)
		{
			Log.Error("Could not find shader " + _shaderName);
			return;
		}
		this.Material = new Material(shader);
		this.isCreatedMaterial = true;
	}

	// Token: 0x06008F36 RID: 36662 RVA: 0x00360045 File Offset: 0x0035E245
	public override void Cleanup()
	{
		base.Cleanup();
		if (!this.isCreatedMaterial)
		{
			return;
		}
		global::UnityEngine.Object.Destroy(this.material);
		this.material = null;
		this.isCreatedMaterial = false;
	}

	// Token: 0x06008F37 RID: 36663 RVA: 0x0035F471 File Offset: 0x0035D671
	public override void InitView()
	{
		base.InitView();
		this.updateData();
	}

	// Token: 0x06008F38 RID: 36664 RVA: 0x00360070 File Offset: 0x0035E270
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
		if (this.forceTextureUpdate)
		{
			this.uiTexture.mainTexture = null;
			this.forceTextureUpdate = false;
		}
		this.uiTexture.enabled = this.texture != null;
		this.uiTexture.mainTexture = this.texture;
		this.uiTexture.color = base.opacityModColor(this.color);
		this.uiTexture.keepAspectRatio = this.keepAspectRatio;
		this.uiTexture.aspectRatio = this.aspectRatio;
		this.uiTexture.fixedAspect = this.keepSourceAspectRatio;
		this.uiTexture.fixedAspectRespectPivot = this.sourceAspectRatioRespectPivot;
		this.uiTexture.SetDimensions(this.size.x, this.size.y);
		this.uiTexture.type = this.type;
		this.uiTexture.border = this.border;
		this.uiTexture.uvRect = this.uvRect;
		this.uiTexture.flip = this.flip;
		this.uiTexture.centerType = (this.fillCenter ? UIBasicSprite.AdvancedType.Sliced : UIBasicSprite.AdvancedType.Invisible);
		this.uiTexture.fillDirection = this.fillDirection;
		this.uiTexture.material = this.material;
		base.updateData();
	}

	// Token: 0x06008F39 RID: 36665 RVA: 0x003601BF File Offset: 0x0035E3BF
	public void ForceTextureUpdate()
	{
		this.forceTextureUpdate = true;
		base.SetDirty();
	}

	// Token: 0x06008F3A RID: 36666 RVA: 0x003601CE File Offset: 0x0035E3CE
	public virtual void UnloadTexture()
	{
		if (this.Texture == null)
		{
			return;
		}
		this.uiTexture.mainTexture = null;
		this.texture = null;
		base.SetDirty();
	}

	// Token: 0x06008F3B RID: 36667 RVA: 0x003601F8 File Offset: 0x0035E3F8
	[XuiXmlAttribute("material", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeMaterial(string _value)
	{
		this.xui.LoadData<Material>(_value, delegate(Material _o)
		{
			this.material = new Material(_o);
		});
	}

	// Token: 0x06008F3C RID: 36668 RVA: 0x00360214 File Offset: 0x0035E414
	[XuiXmlAttribute("rect_offset", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeRectOffset(Vector2 _value)
	{
		Rect uvrect = this.UVRect;
		uvrect.x = _value.x;
		uvrect.y = _value.y;
		this.UVRect = uvrect;
	}

	// Token: 0x06008F3D RID: 36669 RVA: 0x0036024C File Offset: 0x0035E44C
	[XuiXmlAttribute("rect_size", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeRectSize(Vector2 _value)
	{
		Rect uvrect = this.UVRect;
		uvrect.width = _value.x;
		uvrect.height = _value.y;
		this.UVRect = uvrect;
	}

	// Token: 0x040068FF RID: 26879
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITexture uiTexture;

	// Token: 0x04006900 RID: 26880
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture texture;

	// Token: 0x04006901 RID: 26881
	[PublicizedFrom(EAccessModifier.Private)]
	public Material material;

	// Token: 0x04006902 RID: 26882
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect uvRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x04006903 RID: 26883
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector4 border = Vector4.zero;

	// Token: 0x04006904 RID: 26884
	[PublicizedFrom(EAccessModifier.Private)]
	public Color color = Color.white;

	// Token: 0x04006905 RID: 26885
	[PublicizedFrom(EAccessModifier.Private)]
	public UIBasicSprite.FillDirection fillDirection;

	// Token: 0x04006906 RID: 26886
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isCreatedMaterial;

	// Token: 0x04006907 RID: 26887
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceTextureUpdate;

	// Token: 0x04006908 RID: 26888
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sourceAspectRatioRespectPivot;
}
