using System;
using UnityEngine.Scripting;

// Token: 0x02001024 RID: 4132
[Preserve]
public class XUiC_ShapeMaterialInfoWindow : XUiController
{
	// Token: 0x0600829A RID: 33434 RVA: 0x00318CDC File Offset: 0x00316EDC
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

	// Token: 0x0600829B RID: 33435 RVA: 0x00318D61 File Offset: 0x00316F61
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnUpgrade_OnPress(XUiController _sender, int _mouseButton)
	{
		this.windowGroup.Controller.GetChildByType<XUiC_ShapesWindow>().UpgradeDowngradeShapes(this.blockData.UpgradeBlock);
	}

	// Token: 0x0600829C RID: 33436 RVA: 0x00318D83 File Offset: 0x00316F83
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnDowngrade_OnPress(XUiController _sender, int _mouseButton)
	{
		this.windowGroup.Controller.GetChildByType<XUiC_ShapesWindow>().UpgradeDowngradeShapes(this.blockData.DowngradeBlock);
	}

	// Token: 0x0600829D RID: 33437 RVA: 0x002FB86C File Offset: 0x002F9A6C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x0600829E RID: 33438 RVA: 0x00318DA8 File Offset: 0x00316FA8
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

	// Token: 0x0600829F RID: 33439 RVA: 0x00318E98 File Offset: 0x00317098
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

	// Token: 0x040060B8 RID: 24760
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture backgroundTexture;

	// Token: 0x040060B9 RID: 24761
	[PublicizedFrom(EAccessModifier.Private)]
	public Block blockData;

	// Token: 0x040060BA RID: 24762
	[PublicizedFrom(EAccessModifier.Private)]
	public string materialName;

	// Token: 0x040060BB RID: 24763
	[PublicizedFrom(EAccessModifier.Private)]
	public string upgradeMaterial;

	// Token: 0x040060BC RID: 24764
	[PublicizedFrom(EAccessModifier.Private)]
	public string downgradeMaterial;
}
