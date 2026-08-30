using System;
using UnityEngine.Scripting;

// Token: 0x02000F0C RID: 3852
[Preserve]
public class XUiC_DialogResponseEntry : XUiController
{
	// Token: 0x17000E1C RID: 3612
	// (get) Token: 0x060078E4 RID: 30948 RVA: 0x002D95A8 File Offset: 0x002D77A8
	// (set) Token: 0x060078E5 RID: 30949 RVA: 0x002D95B0 File Offset: 0x002D77B0
	public DialogResponse CurrentResponse
	{
		get
		{
			return this.currentResponse;
		}
		set
		{
			this.currentResponse = value;
			this.HasRequirement = true;
			base.ViewComponent.Enabled = value != null;
			if (this.currentResponse != null && this.currentResponse.RequirementList.Count > 0)
			{
				int i = 0;
				while (i < this.currentResponse.RequirementList.Count)
				{
					if (!this.currentResponse.RequirementList[i].CheckRequirement(this.xui.playerUI.entityPlayer, this.xui.Dialog.Respondent))
					{
						this.HasRequirement = false;
						if (this.currentResponse.RequirementList[i].RequirementVisibilityType == BaseDialogRequirement.RequirementVisibilityTypes.Hide)
						{
							this.currentResponse = null;
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			base.RefreshBindings();
		}
	}

	// Token: 0x060078E6 RID: 30950 RVA: 0x002D967C File Offset: 0x002D787C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.currentResponse != null;
		if (bindingName == "response")
		{
			value = "";
			if (flag)
			{
				value = (this.HasRequirement ? this.currentResponse.Text : this.currentResponse.GetRequiredDescription(this.xui.playerUI.entityPlayer));
			}
			return true;
		}
		if (bindingName == "textstatecolor")
		{
			value = "255,255,255,255";
			if (flag)
			{
				value = (this.HasRequirement ? XUiC_DialogResponseEntry.enabledColor : XUiC_DialogResponseEntry.disabledColor);
			}
			return true;
		}
		if (bindingName == "rowstatecolor")
		{
			value = "255,255,255,255";
			if (flag)
			{
				if (this.HasRequirement)
				{
					value = (this.Selected ? "255,255,255,255" : (this.IsHovered ? this.hoverColor : XUiC_DialogResponseEntry.enabledColor));
				}
				else
				{
					value = XUiC_DialogResponseEntry.disabledColor;
				}
			}
			return true;
		}
		if (bindingName == "rowstatesprite")
		{
			value = (this.Selected ? "ui_game_select_row" : "menu_empty");
			return true;
		}
		if (!(bindingName == "showresponse"))
		{
			return base.GetBindingValueInternal(ref value, bindingName);
		}
		value = flag.ToString();
		return true;
	}

	// Token: 0x060078E7 RID: 30951 RVA: 0x002D97B0 File Offset: 0x002D79B0
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x060078E8 RID: 30952 RVA: 0x002D97BF File Offset: 0x002D79BF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHovered(bool _isOver)
	{
		base.OnHovered(_isOver);
		if (this.currentResponse == null)
		{
			this.IsHovered = false;
			return;
		}
		if (this.IsHovered != _isOver)
		{
			this.IsHovered = _isOver;
			base.RefreshBindings();
		}
	}

	// Token: 0x060078E9 RID: 30953 RVA: 0x002C0D09 File Offset: 0x002BEF09
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060078EA RID: 30954 RVA: 0x0008A069 File Offset: 0x00088269
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x060078EB RID: 30955 RVA: 0x002D97F0 File Offset: 0x002D79F0
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "enabled_color")
		{
			XUiC_DialogResponseEntry.enabledColor = value;
			return true;
		}
		if (name == "disabled_color")
		{
			XUiC_DialogResponseEntry.disabledColor = value;
			return true;
		}
		if (name == "row_color")
		{
			this.rowColor = value;
			return true;
		}
		if (!(name == "hover_color"))
		{
			return base.ParseAttribute(name, value);
		}
		this.hoverColor = value;
		return true;
	}

	// Token: 0x04005939 RID: 22841
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x0400593A RID: 22842
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x0400593B RID: 22843
	public new bool Selected;

	// Token: 0x0400593C RID: 22844
	public bool IsHovered;

	// Token: 0x0400593D RID: 22845
	public bool HasRequirement = true;

	// Token: 0x0400593E RID: 22846
	[PublicizedFrom(EAccessModifier.Private)]
	public static string enabledColor = "255,255,255,255";

	// Token: 0x0400593F RID: 22847
	[PublicizedFrom(EAccessModifier.Private)]
	public static string disabledColor = "200,200,200,255";

	// Token: 0x04005940 RID: 22848
	[PublicizedFrom(EAccessModifier.Private)]
	public DialogResponse currentResponse;
}
