using System;
using UnityEngine;

// Token: 0x0200117D RID: 4477
public class XUiV_TextureBased : XUiV_ImageBased
{
	// Token: 0x1700113A RID: 4410
	// (get) Token: 0x06008ECE RID: 36558 RVA: 0x0035DDCA File Offset: 0x0035BFCA
	// (set) Token: 0x06008ECF RID: 36559 RVA: 0x0035DDD2 File Offset: 0x0035BFD2
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

	// Token: 0x1700113B RID: 4411
	// (get) Token: 0x06008ED0 RID: 36560 RVA: 0x0035DDE1 File Offset: 0x0035BFE1
	// (set) Token: 0x06008ED1 RID: 36561 RVA: 0x0035DDE9 File Offset: 0x0035BFE9
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

	// Token: 0x1700113C RID: 4412
	// (get) Token: 0x06008ED2 RID: 36562 RVA: 0x0035DDF8 File Offset: 0x0035BFF8
	// (set) Token: 0x06008ED3 RID: 36563 RVA: 0x0035DE05 File Offset: 0x0035C005
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

	// Token: 0x1700113D RID: 4413
	// (get) Token: 0x06008ED4 RID: 36564 RVA: 0x0035DE13 File Offset: 0x0035C013
	// (set) Token: 0x06008ED5 RID: 36565 RVA: 0x0035DE1B File Offset: 0x0035C01B
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

	// Token: 0x1700113E RID: 4414
	// (get) Token: 0x06008ED6 RID: 36566 RVA: 0x0035DE2A File Offset: 0x0035C02A
	// (set) Token: 0x06008ED7 RID: 36567 RVA: 0x0035DE32 File Offset: 0x0035C032
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

	// Token: 0x1700113F RID: 4415
	// (get) Token: 0x06008ED8 RID: 36568 RVA: 0x0035DE41 File Offset: 0x0035C041
	// (set) Token: 0x06008ED9 RID: 36569 RVA: 0x0035DE49 File Offset: 0x0035C049
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

	// Token: 0x17001140 RID: 4416
	// (get) Token: 0x06008EDA RID: 36570 RVA: 0x0035DE58 File Offset: 0x0035C058
	// (set) Token: 0x06008EDB RID: 36571 RVA: 0x0035DE60 File Offset: 0x0035C060
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

	// Token: 0x17001141 RID: 4417
	// (get) Token: 0x06008EDC RID: 36572 RVA: 0x0035DE6F File Offset: 0x0035C06F
	// (set) Token: 0x06008EDD RID: 36573 RVA: 0x0035DE77 File Offset: 0x0035C077
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

	// Token: 0x06008EDE RID: 36574 RVA: 0x0035DE90 File Offset: 0x0035C090
	public XUiV_TextureBased(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008EDF RID: 36575 RVA: 0x0035DECF File Offset: 0x0035C0CF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UITexture>();
	}

	// Token: 0x06008EE0 RID: 36576 RVA: 0x0035DEE0 File Offset: 0x0035C0E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.uiTexture = this.uiTransform.gameObject.GetComponent<UITexture>());
	}

	// Token: 0x06008EE1 RID: 36577 RVA: 0x0035DF14 File Offset: 0x0035C114
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

	// Token: 0x06008EE2 RID: 36578 RVA: 0x0035DF55 File Offset: 0x0035C155
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

	// Token: 0x06008EE3 RID: 36579 RVA: 0x0035D381 File Offset: 0x0035B581
	public override void InitView()
	{
		base.InitView();
		this.updateData();
	}

	// Token: 0x06008EE4 RID: 36580 RVA: 0x0035DF80 File Offset: 0x0035C180
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

	// Token: 0x06008EE5 RID: 36581 RVA: 0x0035E0CF File Offset: 0x0035C2CF
	public void ForceTextureUpdate()
	{
		this.forceTextureUpdate = true;
		base.SetDirty();
	}

	// Token: 0x06008EE6 RID: 36582 RVA: 0x0035E0DE File Offset: 0x0035C2DE
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

	// Token: 0x06008EE7 RID: 36583 RVA: 0x0035E108 File Offset: 0x0035C308
	[XuiXmlAttribute("material", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeMaterial(string _value)
	{
		this.xui.LoadData<Material>(_value, delegate(Material _o)
		{
			this.material = new Material(_o);
		});
	}

	// Token: 0x06008EE8 RID: 36584 RVA: 0x0035E124 File Offset: 0x0035C324
	[XuiXmlAttribute("rect_offset", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeRectOffset(Vector2 _value)
	{
		Rect uvrect = this.UVRect;
		uvrect.x = _value.x;
		uvrect.y = _value.y;
		this.UVRect = uvrect;
	}

	// Token: 0x06008EE9 RID: 36585 RVA: 0x0035E15C File Offset: 0x0035C35C
	[XuiXmlAttribute("rect_size", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeRectSize(Vector2 _value)
	{
		Rect uvrect = this.UVRect;
		uvrect.width = _value.x;
		uvrect.height = _value.y;
		this.UVRect = uvrect;
	}

	// Token: 0x040068BF RID: 26815
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITexture uiTexture;

	// Token: 0x040068C0 RID: 26816
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture texture;

	// Token: 0x040068C1 RID: 26817
	[PublicizedFrom(EAccessModifier.Private)]
	public Material material;

	// Token: 0x040068C2 RID: 26818
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect uvRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x040068C3 RID: 26819
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector4 border = Vector4.zero;

	// Token: 0x040068C4 RID: 26820
	[PublicizedFrom(EAccessModifier.Private)]
	public Color color = Color.white;

	// Token: 0x040068C5 RID: 26821
	[PublicizedFrom(EAccessModifier.Private)]
	public UIBasicSprite.FillDirection fillDirection;

	// Token: 0x040068C6 RID: 26822
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isCreatedMaterial;

	// Token: 0x040068C7 RID: 26823
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceTextureUpdate;

	// Token: 0x040068C8 RID: 26824
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sourceAspectRatioRespectPivot;
}
