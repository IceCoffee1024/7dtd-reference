using System;
using UnityEngine;

// Token: 0x0200114C RID: 4428
public class XUiV_TextureBased : XUiV_ImageBased
{
	// Token: 0x170010BD RID: 4285
	// (get) Token: 0x06008D51 RID: 36177 RVA: 0x00354DEA File Offset: 0x00352FEA
	// (set) Token: 0x06008D52 RID: 36178 RVA: 0x00354DF2 File Offset: 0x00352FF2
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

	// Token: 0x170010BE RID: 4286
	// (get) Token: 0x06008D53 RID: 36179 RVA: 0x00354E01 File Offset: 0x00353001
	// (set) Token: 0x06008D54 RID: 36180 RVA: 0x00354E09 File Offset: 0x00353009
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

	// Token: 0x170010BF RID: 4287
	// (get) Token: 0x06008D55 RID: 36181 RVA: 0x00354E18 File Offset: 0x00353018
	// (set) Token: 0x06008D56 RID: 36182 RVA: 0x00354E25 File Offset: 0x00353025
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

	// Token: 0x170010C0 RID: 4288
	// (get) Token: 0x06008D57 RID: 36183 RVA: 0x00354E33 File Offset: 0x00353033
	// (set) Token: 0x06008D58 RID: 36184 RVA: 0x00354E3B File Offset: 0x0035303B
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

	// Token: 0x170010C1 RID: 4289
	// (get) Token: 0x06008D59 RID: 36185 RVA: 0x00354E4A File Offset: 0x0035304A
	// (set) Token: 0x06008D5A RID: 36186 RVA: 0x00354E52 File Offset: 0x00353052
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

	// Token: 0x170010C2 RID: 4290
	// (get) Token: 0x06008D5B RID: 36187 RVA: 0x00354E61 File Offset: 0x00353061
	// (set) Token: 0x06008D5C RID: 36188 RVA: 0x00354E69 File Offset: 0x00353069
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

	// Token: 0x170010C3 RID: 4291
	// (get) Token: 0x06008D5D RID: 36189 RVA: 0x00354E78 File Offset: 0x00353078
	// (set) Token: 0x06008D5E RID: 36190 RVA: 0x00354E80 File Offset: 0x00353080
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

	// Token: 0x170010C4 RID: 4292
	// (get) Token: 0x06008D5F RID: 36191 RVA: 0x00354E8F File Offset: 0x0035308F
	// (set) Token: 0x06008D60 RID: 36192 RVA: 0x00354E97 File Offset: 0x00353097
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

	// Token: 0x06008D61 RID: 36193 RVA: 0x00354EB0 File Offset: 0x003530B0
	public XUiV_TextureBased(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008D62 RID: 36194 RVA: 0x00354EEF File Offset: 0x003530EF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UITexture>();
	}

	// Token: 0x06008D63 RID: 36195 RVA: 0x00354F00 File Offset: 0x00353100
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.uiTexture = this.uiTransform.gameObject.GetComponent<UITexture>());
	}

	// Token: 0x06008D64 RID: 36196 RVA: 0x00354F34 File Offset: 0x00353134
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

	// Token: 0x06008D65 RID: 36197 RVA: 0x00354F75 File Offset: 0x00353175
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

	// Token: 0x06008D66 RID: 36198 RVA: 0x003543A1 File Offset: 0x003525A1
	public override void InitView()
	{
		base.InitView();
		this.updateData();
	}

	// Token: 0x06008D67 RID: 36199 RVA: 0x00354FA0 File Offset: 0x003531A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateData()
	{
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

	// Token: 0x06008D68 RID: 36200 RVA: 0x003550D4 File Offset: 0x003532D4
	public void SetTextureDirty()
	{
		this.uiTexture.mainTexture = null;
		base.SetDirty();
	}

	// Token: 0x06008D69 RID: 36201 RVA: 0x003550E8 File Offset: 0x003532E8
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

	// Token: 0x06008D6A RID: 36202 RVA: 0x00355112 File Offset: 0x00353312
	[XuiXmlAttribute("material", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeMaterial(string _value)
	{
		this.xui.LoadData<Material>(_value, delegate(Material _o)
		{
			this.material = new Material(_o);
		});
	}

	// Token: 0x06008D6B RID: 36203 RVA: 0x0035512C File Offset: 0x0035332C
	[XuiXmlAttribute("rect_offset", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeRectOffset(Vector2 _value)
	{
		Rect uvrect = this.UVRect;
		uvrect.x = _value.x;
		uvrect.y = _value.y;
		this.UVRect = uvrect;
	}

	// Token: 0x06008D6C RID: 36204 RVA: 0x00355164 File Offset: 0x00353364
	[XuiXmlAttribute("rect_size", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeRectSize(Vector2 _value)
	{
		Rect uvrect = this.UVRect;
		uvrect.width = _value.x;
		uvrect.height = _value.y;
		this.UVRect = uvrect;
	}

	// Token: 0x04006790 RID: 26512
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITexture uiTexture;

	// Token: 0x04006791 RID: 26513
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture texture;

	// Token: 0x04006792 RID: 26514
	[PublicizedFrom(EAccessModifier.Private)]
	public Material material;

	// Token: 0x04006793 RID: 26515
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect uvRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x04006794 RID: 26516
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector4 border = Vector4.zero;

	// Token: 0x04006795 RID: 26517
	[PublicizedFrom(EAccessModifier.Private)]
	public Color color = Color.white;

	// Token: 0x04006796 RID: 26518
	[PublicizedFrom(EAccessModifier.Private)]
	public UIBasicSprite.FillDirection fillDirection;

	// Token: 0x04006797 RID: 26519
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isCreatedMaterial;

	// Token: 0x04006798 RID: 26520
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sourceAspectRatioRespectPivot;
}
