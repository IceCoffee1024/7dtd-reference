using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001023 RID: 4131
[Preserve]
public class XUiC_ShapeInfoWindow : XUiController
{
	// Token: 0x06008294 RID: 33428 RVA: 0x002FB86C File Offset: 0x002F9A6C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.IsDirty && base.ViewComponent.IsVisible)
		{
			this.IsDirty = false;
		}
	}

	// Token: 0x06008295 RID: 33429 RVA: 0x003188A0 File Offset: 0x00316AA0
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

	// Token: 0x06008296 RID: 33430 RVA: 0x00318B58 File Offset: 0x00316D58
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

	// Token: 0x06008297 RID: 33431 RVA: 0x00318BD8 File Offset: 0x00316DD8
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

	// Token: 0x06008298 RID: 33432 RVA: 0x00318C58 File Offset: 0x00316E58
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

	// Token: 0x040060B3 RID: 24755
	[PublicizedFrom(EAccessModifier.Private)]
	public Block blockData;

	// Token: 0x040060B4 RID: 24756
	[PublicizedFrom(EAccessModifier.Private)]
	public string shapeName = "";

	// Token: 0x040060B5 RID: 24757
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue itemValue;

	// Token: 0x040060B6 RID: 24758
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemDisplayEntry itemDisplayEntry;

	// Token: 0x040060B7 RID: 24759
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();
}
