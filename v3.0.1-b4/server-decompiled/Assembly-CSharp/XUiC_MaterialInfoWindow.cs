using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F7C RID: 3964
[Preserve]
public class XUiC_MaterialInfoWindow : XUiController
{
	// Token: 0x06007D01 RID: 32001 RVA: 0x002F7065 File Offset: 0x002F5265
	public override void Init()
	{
		base.Init();
		this.textMaterial = base.GetChildById("textMaterial").ViewComponent as XUiV_Texture;
		this.textMaterial.CreateMaterial("Unlit/Transparent Colored Emissive TextureArray");
	}

	// Token: 0x06007D02 RID: 32002 RVA: 0x002F7098 File Offset: 0x002F5298
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x06007D03 RID: 32003 RVA: 0x002F70C0 File Offset: 0x002F52C0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1111481258U)
		{
			if (num <= 107290595U)
			{
				if (num != 74828773U)
				{
					if (num == 107290595U)
					{
						if (bindingName == "materialname")
						{
							value = ((this.TextureData != null) ? Localization.Get(this.TextureData.Name, false, null) : "");
							return true;
						}
					}
				}
				else if (bindingName == "perklevel")
				{
					value = ((this.TextureData != null) ? this.perklevelFormatter.Format(this.TextureData.RequiredLevel) : "");
					return true;
				}
			}
			else if (num != 461391004U)
			{
				if (num != 1095359625U)
				{
					if (num == 1111481258U)
					{
						if (bindingName == "paintcost")
						{
							value = ((this.TextureData != null) ? this.paintcostFormatter.Format(this.TextureData.PaintCost) : "");
							return true;
						}
					}
				}
				else if (bindingName == "paintunit")
				{
					value = ((this.TextureData != null) ? Localization.Get("xuiPaintUnit", false, null) : "");
					return true;
				}
			}
			else if (bindingName == "paintcosttitle")
			{
				value = ((this.TextureData != null) ? Localization.Get("xuiPaintCost", false, null) : "");
				return true;
			}
		}
		else if (num <= 2509437782U)
		{
			if (num != 1605967500U)
			{
				if (num == 2509437782U)
				{
					if (bindingName == "requiredtitle")
					{
						value = ((this.TextureData != null) ? Localization.Get("xuiRequired", false, null) : "");
						return true;
					}
				}
			}
			else if (bindingName == "group")
			{
				value = ((this.TextureData != null) ? Localization.Get(this.TextureData.Group, false, null) : "");
				return true;
			}
		}
		else if (num != 2728359946U)
		{
			if (num != 3066960388U)
			{
				if (num == 3069197533U)
				{
					if (bindingName == "perk")
					{
						value = "";
						if (this.TextureData != null && this.TextureData.LockedByPerk != "")
						{
							ProgressionValue progressionValue = this.xui.playerUI.entityPlayer.Progression.GetProgressionValue(this.TextureData.LockedByPerk);
							value = Localization.Get(progressionValue.ProgressionClass.NameKey, false, null);
						}
						return true;
					}
				}
			}
			else if (bindingName == "hasperklock")
			{
				value = ((this.TextureData != null) ? (this.TextureData.LockedByPerk != "").ToString() : "false");
				return true;
			}
		}
		else if (bindingName == "grouptitle")
		{
			value = ((this.TextureData != null) ? Localization.Get("xuiMaterialGroup", false, null) : "");
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06007D04 RID: 32004 RVA: 0x002F73EC File Offset: 0x002F55EC
	public void SetMaterial(BlockTextureData newTexture)
	{
		this.TextureData = newTexture;
		this.textMaterial.IsVisible = false;
		if (this.TextureData != null)
		{
			this.textMaterial.IsVisible = true;
			MeshDescription meshDescription = MeshDescription.meshes[0];
			int textureID = (int)this.TextureData.TextureID;
			Rect rect;
			if (textureID == 0)
			{
				rect = WorldConstants.uvRectZero;
			}
			else
			{
				rect = meshDescription.textureAtlas.uvMapping[textureID].uv;
			}
			this.textMaterial.Texture = meshDescription.textureAtlas.diffuseTexture;
			if (meshDescription.bTextureArray)
			{
				this.textMaterial.Material.SetTexture("_BumpMap", meshDescription.textureAtlas.normalTexture);
				this.textMaterial.Material.SetFloat("_Index", (float)meshDescription.textureAtlas.uvMapping[textureID].index);
				this.textMaterial.Material.SetFloat("_Size", (float)meshDescription.textureAtlas.uvMapping[textureID].blockW);
			}
			else
			{
				this.textMaterial.UVRect = rect;
			}
		}
		base.RefreshBindings();
	}

	// Token: 0x04005C89 RID: 23689
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockTextureData TextureData;

	// Token: 0x04005C8A RID: 23690
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture textMaterial;

	// Token: 0x04005C8B RID: 23691
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<ushort> paintcostFormatter = new CachedStringFormatter<ushort>((ushort _i) => _i.ToString());

	// Token: 0x04005C8C RID: 23692
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<ushort> perklevelFormatter = new CachedStringFormatter<ushort>((ushort _i) => _i.ToString());
}
