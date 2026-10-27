using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001042 RID: 4162
[Preserve]
public class XUiC_ShapeInfoWindow : XUiController
{
	// Token: 0x060082F5 RID: 33525 RVA: 0x0030042C File Offset: 0x002FE62C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x060082F6 RID: 33526 RVA: 0x0031D5E8 File Offset: 0x0031B7E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 1674238749U)
		{
			if (num <= 1573573035U)
			{
				if (num != 325231145U)
				{
					if (num != 1556795416U)
					{
						if (num == 1573573035U)
						{
							if (_bindingName == "itemstattitle3")
							{
								_value = this.GetStatTitle(2);
								return true;
							}
						}
					}
					else if (_bindingName == "itemstattitle2")
					{
						_value = this.GetStatTitle(1);
						return true;
					}
				}
				else if (_bindingName == "blockname")
				{
					_value = this.shapeName;
					return true;
				}
			}
			else if (num != 1607128273U)
			{
				if (num != 1657461130U)
				{
					if (num == 1674238749U)
					{
						if (_bindingName == "itemstattitle5")
						{
							_value = this.GetStatTitle(4);
							return true;
						}
					}
				}
				else if (_bindingName == "itemstattitle4")
				{
					_value = this.GetStatTitle(3);
					return true;
				}
			}
			else if (_bindingName == "itemstattitle1")
			{
				_value = this.GetStatTitle(0);
				return true;
			}
		}
		else if (num <= 4140647608U)
		{
			if (num != 2594821807U)
			{
				if (num != 4048624866U)
				{
					if (num == 4140647608U)
					{
						if (_bindingName == "itemstat4")
						{
							_value = this.GetStatValue(3);
							return true;
						}
					}
				}
				else if (_bindingName == "blockicontint")
				{
					Color32 color = Color.white;
					if (this.blockData != null)
					{
						color = this.blockData.CustomIconTint;
					}
					_value = this.itemicontintcolorFormatter.Format(color);
					return true;
				}
			}
			else if (_bindingName == "blockicon")
			{
				_value = ((this.blockData == null) ? "" : this.blockData.GetIconName());
				return true;
			}
		}
		else if (num <= 4224535703U)
		{
			if (num != 4157425227U)
			{
				if (num == 4224535703U)
				{
					if (_bindingName == "itemstat1")
					{
						_value = this.GetStatValue(0);
						return true;
					}
				}
			}
			else if (_bindingName == "itemstat5")
			{
				_value = this.GetStatValue(4);
				return true;
			}
		}
		else if (num != 4241313322U)
		{
			if (num == 4258090941U)
			{
				if (_bindingName == "itemstat3")
				{
					_value = this.GetStatValue(2);
					return true;
				}
			}
		}
		else if (_bindingName == "itemstat2")
		{
			_value = this.GetStatValue(1);
			return true;
		}
		return base.GetBindingValueInternal(ref _value, _bindingName);
	}

	// Token: 0x060082F7 RID: 33527 RVA: 0x0031D8A0 File Offset: 0x0031BAA0
	public void SetShape(Block _newBlockData)
	{
		this.blockData = _newBlockData;
		if (_newBlockData != null)
		{
			if (_newBlockData.GetAutoShapeType() == EAutoShapeType.None)
			{
				this.shapeName = this.blockData.GetLocalizedBlockName();
			}
			else
			{
				this.shapeName = this.blockData.GetLocalizedAutoShapeShapeName();
			}
		}
		else
		{
			this.shapeName = "";
		}
		this.itemValue = _newBlockData.ToBlockValue().ToItemValue();
		this.itemDisplayEntry = UIDisplayInfoManager.Current.GetDisplayStatsForTag(_newBlockData.DisplayType);
		base.RefreshBindings();
	}

	// Token: 0x060082F8 RID: 33528 RVA: 0x0031D920 File Offset: 0x0031BB20
	[PublicizedFrom(EAccessModifier.Private)]
	public string GetStatTitle(int index)
	{
		if (this.itemDisplayEntry == null || this.itemDisplayEntry.DisplayStats.Count <= index)
		{
			return "";
		}
		if (this.itemDisplayEntry.DisplayStats[index].TitleOverride != null)
		{
			return this.itemDisplayEntry.DisplayStats[index].TitleOverride;
		}
		return UIDisplayInfoManager.Current.GetLocalizedName(this.itemDisplayEntry.DisplayStats[index].StatType);
	}

	// Token: 0x060082F9 RID: 33529 RVA: 0x0031D9A0 File Offset: 0x0031BBA0
	[PublicizedFrom(EAccessModifier.Private)]
	public string GetStatValue(int index)
	{
		if (this.itemDisplayEntry == null || this.itemDisplayEntry.DisplayStats.Count <= index)
		{
			return "";
		}
		DisplayInfoEntry displayInfoEntry = this.itemDisplayEntry.DisplayStats[index];
		return XUiM_ItemStack.GetStatItemValueTextWithCompareInfo(this.itemValue, ItemValue.None, this.xui.playerUI.entityPlayer, displayInfoEntry, false, true);
	}

	// Token: 0x0400611A RID: 24858
	[PublicizedFrom(EAccessModifier.Private)]
	public Block blockData;

	// Token: 0x0400611B RID: 24859
	[PublicizedFrom(EAccessModifier.Private)]
	public string shapeName = "";

	// Token: 0x0400611C RID: 24860
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x0400611D RID: 24861
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemDisplayEntry itemDisplayEntry;

	// Token: 0x0400611E RID: 24862
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
