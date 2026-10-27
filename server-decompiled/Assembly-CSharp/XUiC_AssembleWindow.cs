using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000EDA RID: 3802
[Preserve]
public class XUiC_AssembleWindow : XUiController
{
	// Token: 0x0600761B RID: 30235 RVA: 0x002C90B3 File Offset: 0x002C72B3
	public override void Init()
	{
		base.Init();
		this.btnComplete = base.GetChildById("btnComplete");
		if (this.btnComplete != null)
		{
			this.btnComplete.OnPress += this.BtnComplete_OnPress;
		}
	}

	// Token: 0x17000E05 RID: 3589
	// (get) Token: 0x0600761C RID: 30236 RVA: 0x002C90EB File Offset: 0x002C72EB
	// (set) Token: 0x0600761D RID: 30237 RVA: 0x002C90F4 File Offset: 0x002C72F4
	public virtual ItemStack ItemStack
	{
		get
		{
			return this.itemStack;
		}
		set
		{
			this.itemStack = value;
			if (!this.itemStack.IsEmpty())
			{
				this.itemClass = this.itemStack.itemValue.ItemClass;
				this.itemDisplayEntry = UIDisplayInfoManager.Current.GetDisplayStatsForTag(this.itemClass.DisplayType);
			}
			else
			{
				this.itemClass = null;
			}
			base.RefreshBindings();
		}
	}

	// Token: 0x0600761E RID: 30238 RVA: 0x002C9155 File Offset: 0x002C7355
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnComplete_OnPress(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
	}

	// Token: 0x0600761F RID: 30239 RVA: 0x002C9170 File Offset: 0x002C7370
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = !this.itemStack.IsEmpty();
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 3191456325U)
		{
			if (num <= 1607128273U)
			{
				if (num <= 619741203U)
				{
					if (num != 546263858U)
					{
						if (num == 619741203U)
						{
							if (bindingName == "itemqualitytitle")
							{
								value = Localization.Get("xuiQuality", false, null);
								return true;
							}
						}
					}
					else if (bindingName == "itemqualityfill")
					{
						value = (flag ? this.qualityfillFormatter.Format(((float)this.itemStack.itemValue.MaxUseTimes - this.itemStack.itemValue.UseTimes) / (float)this.itemStack.itemValue.MaxUseTimes) : "1");
						return true;
					}
				}
				else if (num != 1556795416U)
				{
					if (num != 1573573035U)
					{
						if (num == 1607128273U)
						{
							if (bindingName == "itemstattitle1")
							{
								value = (flag ? this.GetStatTitle(0) : "");
								return true;
							}
						}
					}
					else if (bindingName == "itemstattitle3")
					{
						value = (flag ? this.GetStatTitle(2) : "");
						return true;
					}
				}
				else if (bindingName == "itemstattitle2")
				{
					value = (flag ? this.GetStatTitle(1) : "");
					return true;
				}
			}
			else if (num <= 1640683511U)
			{
				if (num != 1623905892U)
				{
					if (num == 1640683511U)
					{
						if (bindingName == "itemstattitle7")
						{
							value = (flag ? this.GetStatTitle(6) : "");
							return true;
						}
					}
				}
				else if (bindingName == "itemstattitle6")
				{
					value = (flag ? this.GetStatTitle(5) : "");
					return true;
				}
			}
			else if (num != 1657461130U)
			{
				if (num != 1674238749U)
				{
					if (num == 3191456325U)
					{
						if (bindingName == "itemname")
						{
							value = (flag ? this.itemClass.GetLocalizedItemName() : "");
							return true;
						}
					}
				}
				else if (bindingName == "itemstattitle5")
				{
					value = (flag ? this.GetStatTitle(4) : "");
					return true;
				}
			}
			else if (bindingName == "itemstattitle4")
			{
				value = (flag ? this.GetStatTitle(3) : "");
				return true;
			}
		}
		else if (num <= 4140647608U)
		{
			if (num <= 3994216002U)
			{
				if (num != 3708628627U)
				{
					if (num == 3994216002U)
					{
						if (bindingName == "itemqualitycolor")
						{
							value = "255,255,255,255";
							if (flag)
							{
								Color32 color = QualityInfo.GetQualityColor((int)this.itemStack.itemValue.Quality);
								value = this.qualitycolorFormatter.Format(color);
							}
							return true;
						}
					}
				}
				else if (bindingName == "itemicon")
				{
					value = "";
					if (flag)
					{
						value = this.itemStack.itemValue.GetPropertyOverride("CustomIcon", (this.itemClass.CustomIcon != null) ? this.itemClass.CustomIcon.Value : this.itemClass.GetIconName());
					}
					return true;
				}
			}
			else if (num != 4053908414U)
			{
				if (num != 4113438435U)
				{
					if (num == 4140647608U)
					{
						if (bindingName == "itemstat4")
						{
							value = (flag ? this.GetStatValue(3) : "");
							return true;
						}
					}
				}
				else if (bindingName == "itemquality")
				{
					value = (flag ? this.qualityFormatter.Format((int)this.itemStack.itemValue.Quality) : "0");
					return true;
				}
			}
			else if (bindingName == "itemicontint")
			{
				Color32 color2 = Color.white;
				if (this.itemClass != null)
				{
					color2 = this.itemStack.itemValue.ItemClass.GetIconTint(this.itemStack.itemValue);
				}
				value = this.itemicontintcolorFormatter.Format(color2);
				return true;
			}
		}
		else if (num <= 4190980465U)
		{
			if (num != 4157425227U)
			{
				if (num != 4174202846U)
				{
					if (num == 4190980465U)
					{
						if (bindingName == "itemstat7")
						{
							value = (flag ? this.GetStatValue(6) : "");
							return true;
						}
					}
				}
				else if (bindingName == "itemstat6")
				{
					value = (flag ? this.GetStatValue(5) : "");
					return true;
				}
			}
			else if (bindingName == "itemstat5")
			{
				value = (flag ? this.GetStatValue(4) : "");
				return true;
			}
		}
		else if (num != 4224535703U)
		{
			if (num != 4241313322U)
			{
				if (num == 4258090941U)
				{
					if (bindingName == "itemstat3")
					{
						value = (flag ? this.GetStatValue(2) : "");
						return true;
					}
				}
			}
			else if (bindingName == "itemstat2")
			{
				value = (flag ? this.GetStatValue(1) : "");
				return true;
			}
		}
		else if (bindingName == "itemstat1")
		{
			value = (flag ? this.GetStatValue(0) : "");
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x06007620 RID: 30240 RVA: 0x002C9738 File Offset: 0x002C7938
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

	// Token: 0x06007621 RID: 30241 RVA: 0x002C97B8 File Offset: 0x002C79B8
	[PublicizedFrom(EAccessModifier.Private)]
	public string GetStatValue(int index)
	{
		if (this.itemDisplayEntry == null || this.itemDisplayEntry.DisplayStats.Count <= index)
		{
			return "";
		}
		DisplayInfoEntry displayInfoEntry = this.itemDisplayEntry.DisplayStats[index];
		return XUiM_ItemStack.GetStatItemValueTextWithModInfo(this.itemStack, this.xui.playerUI.entityPlayer, displayInfoEntry);
	}

	// Token: 0x06007622 RID: 30242 RVA: 0x002C9814 File Offset: 0x002C7A14
	public override void Update(float _dt)
	{
		if (GameManager.Instance == null && GameManager.Instance.World == null)
		{
			return;
		}
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x06007623 RID: 30243 RVA: 0x002C984C File Offset: 0x002C7A4C
	public virtual void OnChanged()
	{
		XUiC_AssembleWindowGroup.GetWindowGroup(this.xui).ItemStack = this.ItemStack;
	}

	// Token: 0x0400579B RID: 22427
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack itemStack = ItemStack.Empty;

	// Token: 0x0400579C RID: 22428
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass itemClass;

	// Token: 0x0400579D RID: 22429
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController btnComplete;

	// Token: 0x0400579E RID: 22430
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor qualitycolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x0400579F RID: 22431
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterFloat qualityfillFormatter = new CachedStringFormatterFloat(null);

	// Token: 0x040057A0 RID: 22432
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterInt qualityFormatter = new CachedStringFormatterInt();

	// Token: 0x040057A1 RID: 22433
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatterXuiRgbaColor itemicontintcolorFormatter = new CachedStringFormatterXuiRgbaColor();

	// Token: 0x040057A2 RID: 22434
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemDisplayEntry itemDisplayEntry;
}
