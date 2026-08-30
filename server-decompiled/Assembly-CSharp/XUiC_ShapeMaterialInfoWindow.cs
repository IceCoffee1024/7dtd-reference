using System;
using UnityEngine.Scripting;

// Token: 0x0200100B RID: 4107
[Preserve]
public class XUiC_ShapeMaterialInfoWindow : XUiController
{
	// Token: 0x060081A8 RID: 33192 RVA: 0x003144B4 File Offset: 0x003126B4
	public override void Init()
	{
		base.Init();
		XUiController childById = base.GetChildById("backgroundTexture");
		if (childById != null)
		{
			this.backgroundTexture = (XUiV_Texture)childById.ViewComponent;
			this.backgroundTexture.CreateMaterial("Unlit/Transparent Colored Emissive TextureArray");
		}
		XUiController childById2 = base.GetChildById("btnDowngrade");
		XUiController childById3 = base.GetChildById("btnUpgrade");
		if (childById2 != null)
		{
			childById2.OnPress += this.BtnDowngrade_OnPress;
		}
		if (childById3 != null)
		{
			childById3.OnPress += this.BtnUpgrade_OnPress;
		}
	}

	// Token: 0x060081A9 RID: 33193 RVA: 0x00314539 File Offset: 0x00312739
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnUpgrade_OnPress(XUiController _sender, int _mouseButton)
	{
		this.windowGroup.Controller.GetChildByType<XUiC_ShapesWindow>().UpgradeDowngradeShapes(this.blockData.UpgradeBlock);
	}

	// Token: 0x060081AA RID: 33194 RVA: 0x0031455B File Offset: 0x0031275B
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDowngrade_OnPress(XUiController _sender, int _mouseButton)
	{
		this.windowGroup.Controller.GetChildByType<XUiC_ShapesWindow>().UpgradeDowngradeShapes(this.blockData.DowngradeBlock);
	}

	// Token: 0x060081AB RID: 33195 RVA: 0x002F7098 File Offset: 0x002F5298
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x060081AC RID: 33196 RVA: 0x00314580 File Offset: 0x00312780
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "hasmaterial")
		{
			_value = (!string.IsNullOrEmpty(this.materialName)).ToString();
			return true;
		}
		if (_bindingName == "materialname")
		{
			_value = this.materialName ?? "";
			return true;
		}
		if (_bindingName == "has_upgrade")
		{
			_value = (!string.IsNullOrEmpty(this.upgradeMaterial)).ToString();
			return true;
		}
		if (_bindingName == "upgrade_material")
		{
			_value = this.upgradeMaterial ?? "";
			return true;
		}
		if (_bindingName == "has_downgrade")
		{
			_value = (!string.IsNullOrEmpty(this.downgradeMaterial)).ToString();
			return true;
		}
		if (!(_bindingName == "downgrade_material"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = this.downgradeMaterial ?? "";
		return true;
	}

	// Token: 0x060081AD RID: 33197 RVA: 0x00314670 File Offset: 0x00312870
	public void SetShape(Block _newBlockData)
	{
		this.backgroundTexture.IsVisible = false;
		this.blockData = _newBlockData;
		this.materialName = null;
		this.upgradeMaterial = null;
		this.downgradeMaterial = null;
		if (_newBlockData != null && _newBlockData.GetAutoShapeType() != EAutoShapeType.None)
		{
			this.materialName = _newBlockData.blockMaterial.GetLocalizedMaterialName();
			if (!_newBlockData.UpgradeBlock.isair)
			{
				this.upgradeMaterial = _newBlockData.UpgradeBlock.Block.blockMaterial.GetLocalizedMaterialName();
			}
			if (!_newBlockData.DowngradeBlock.isair)
			{
				this.downgradeMaterial = _newBlockData.DowngradeBlock.Block.blockMaterial.GetLocalizedMaterialName();
			}
			if (this.backgroundTexture != null)
			{
				int sideTextureId = _newBlockData.GetSideTextureId(new BlockValue((uint)_newBlockData.blockID), BlockFace.Top, 0);
				if (sideTextureId != 0)
				{
					MeshDescription meshDescription = MeshDescription.meshes[0];
					UVRectTiling uvrectTiling = meshDescription.textureAtlas.uvMapping[sideTextureId];
					this.backgroundTexture.Texture = meshDescription.textureAtlas.diffuseTexture;
					if (meshDescription.bTextureArray)
					{
						this.backgroundTexture.Material.SetTexture("_BumpMap", meshDescription.textureAtlas.normalTexture);
						this.backgroundTexture.Material.SetFloat("_Index", (float)uvrectTiling.index);
						this.backgroundTexture.Material.SetFloat("_Size", (float)uvrectTiling.blockW);
					}
					else
					{
						this.backgroundTexture.UVRect = uvrectTiling.uv;
					}
					this.backgroundTexture.IsVisible = true;
				}
			}
		}
		base.RefreshBindings();
	}

	// Token: 0x04005FF1 RID: 24561
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture backgroundTexture;

	// Token: 0x04005FF2 RID: 24562
	[PublicizedFrom(EAccessModifier.Private)]
	public Block blockData;

	// Token: 0x04005FF3 RID: 24563
	[PublicizedFrom(EAccessModifier.Private)]
	public string materialName;

	// Token: 0x04005FF4 RID: 24564
	[PublicizedFrom(EAccessModifier.Private)]
	public string upgradeMaterial;

	// Token: 0x04005FF5 RID: 24565
	[PublicizedFrom(EAccessModifier.Private)]
	public string downgradeMaterial;
}
