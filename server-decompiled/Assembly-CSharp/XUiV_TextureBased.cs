using System;
using UnityEngine;

// Token: 0x02001165 RID: 4453
public class XUiV_TextureBased : XUiV_ImageBased
{
	// Token: 0x170010F6 RID: 4342
	// (get) Token: 0x06008E43 RID: 36419 RVA: 0x00359626 File Offset: 0x00357826
	// (set) Token: 0x06008E44 RID: 36420 RVA: 0x0035962E File Offset: 0x0035782E
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

	// Token: 0x170010F7 RID: 4343
	// (get) Token: 0x06008E45 RID: 36421 RVA: 0x0035963D File Offset: 0x0035783D
	// (set) Token: 0x06008E46 RID: 36422 RVA: 0x00359645 File Offset: 0x00357845
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

	// Token: 0x170010F8 RID: 4344
	// (get) Token: 0x06008E47 RID: 36423 RVA: 0x00359654 File Offset: 0x00357854
	// (set) Token: 0x06008E48 RID: 36424 RVA: 0x00359661 File Offset: 0x00357861
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

	// Token: 0x170010F9 RID: 4345
	// (get) Token: 0x06008E49 RID: 36425 RVA: 0x0035966F File Offset: 0x0035786F
	// (set) Token: 0x06008E4A RID: 36426 RVA: 0x00359677 File Offset: 0x00357877
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

	// Token: 0x170010FA RID: 4346
	// (get) Token: 0x06008E4B RID: 36427 RVA: 0x00359686 File Offset: 0x00357886
	// (set) Token: 0x06008E4C RID: 36428 RVA: 0x0035968E File Offset: 0x0035788E
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

	// Token: 0x170010FB RID: 4347
	// (get) Token: 0x06008E4D RID: 36429 RVA: 0x0035969D File Offset: 0x0035789D
	// (set) Token: 0x06008E4E RID: 36430 RVA: 0x003596A5 File Offset: 0x003578A5
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

	// Token: 0x170010FC RID: 4348
	// (get) Token: 0x06008E4F RID: 36431 RVA: 0x003596B4 File Offset: 0x003578B4
	// (set) Token: 0x06008E50 RID: 36432 RVA: 0x003596BC File Offset: 0x003578BC
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

	// Token: 0x170010FD RID: 4349
	// (get) Token: 0x06008E51 RID: 36433 RVA: 0x003596CB File Offset: 0x003578CB
	// (set) Token: 0x06008E52 RID: 36434 RVA: 0x003596D3 File Offset: 0x003578D3
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

	// Token: 0x06008E53 RID: 36435 RVA: 0x003596EC File Offset: 0x003578EC
	public XUiV_TextureBased(XUi _xui, string _id)
		: base(_xui, _id)
	{
	}

	// Token: 0x06008E54 RID: 36436 RVA: 0x0035972B File Offset: 0x0035792B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void createComponents(GameObject _go)
	{
		base.createComponents(_go);
		_go.AddComponent<UITexture>();
	}

	// Token: 0x06008E55 RID: 36437 RVA: 0x0035973C File Offset: 0x0035793C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void captureComponents()
	{
		base.captureComponents();
		this.widget = (this.uiTexture = this.uiTransform.gameObject.GetComponent<UITexture>());
	}

	// Token: 0x06008E56 RID: 36438 RVA: 0x00359770 File Offset: 0x00357970
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

	// Token: 0x06008E57 RID: 36439 RVA: 0x003597B1 File Offset: 0x003579B1
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

	// Token: 0x06008E58 RID: 36440 RVA: 0x00358BDD File Offset: 0x00356DDD
	public override void InitView()
	{
		base.InitView();
		this.updateData();
	}

	// Token: 0x06008E59 RID: 36441 RVA: 0x003597DC File Offset: 0x003579DC
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

	// Token: 0x06008E5A RID: 36442 RVA: 0x00359910 File Offset: 0x00357B10
	public void SetTextureDirty()
	{
		this.uiTexture.mainTexture = null;
		base.SetDirty();
	}

	// Token: 0x06008E5B RID: 36443 RVA: 0x00359924 File Offset: 0x00357B24
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

	// Token: 0x06008E5C RID: 36444 RVA: 0x0035994E File Offset: 0x00357B4E
	[XuiXmlAttribute("material", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeMaterial(string _value)
	{
		this.xui.LoadData<Material>(_value, delegate(Material _o)
		{
			this.material = new Material(_o);
		});
	}

	// Token: 0x06008E5D RID: 36445 RVA: 0x00359968 File Offset: 0x00357B68
	[XuiXmlAttribute("rect_offset", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeRectOffset(Vector2 _value)
	{
		Rect uvrect = this.UVRect;
		uvrect.x = _value.x;
		uvrect.y = _value.y;
		this.UVRect = uvrect;
	}

	// Token: 0x06008E5E RID: 36446 RVA: 0x003599A0 File Offset: 0x00357BA0
	[XuiXmlAttribute("rect_size", false)]
	[PublicizedFrom(EAccessModifier.Private)]
	public void attributeRectSize(Vector2 _value)
	{
		Rect uvrect = this.UVRect;
		uvrect.width = _value.x;
		uvrect.height = _value.y;
		this.UVRect = uvrect;
	}

	// Token: 0x0400685A RID: 26714
	[PublicizedFrom(EAccessModifier.Protected)]
	public UITexture uiTexture;

	// Token: 0x0400685B RID: 26715
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture texture;

	// Token: 0x0400685C RID: 26716
	[PublicizedFrom(EAccessModifier.Private)]
	public Material material;

	// Token: 0x0400685D RID: 26717
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect uvRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x0400685E RID: 26718
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector4 border = Vector4.zero;

	// Token: 0x0400685F RID: 26719
	[PublicizedFrom(EAccessModifier.Private)]
	public Color color = Color.white;

	// Token: 0x04006860 RID: 26720
	[PublicizedFrom(EAccessModifier.Private)]
	public UIBasicSprite.FillDirection fillDirection;

	// Token: 0x04006861 RID: 26721
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isCreatedMaterial;

	// Token: 0x04006862 RID: 26722
	[PublicizedFrom(EAccessModifier.Private)]
	public bool sourceAspectRatioRespectPivot;
}
