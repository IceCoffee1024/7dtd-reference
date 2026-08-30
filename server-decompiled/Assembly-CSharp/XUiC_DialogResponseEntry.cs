using System;
using UnityEngine.Scripting;

// Token: 0x02000F23 RID: 3875
[Preserve]
public class XUiC_DialogResponseEntry : XUiController
{
	// Token: 0x17000E36 RID: 3638
	// (get) Token: 0x0600799C RID: 31132 RVA: 0x002DD534 File Offset: 0x002DB734
	// (set) Token: 0x0600799D RID: 31133 RVA: 0x002DD53C File Offset: 0x002DB73C
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

	// Token: 0x0600799E RID: 31134 RVA: 0x002DD608 File Offset: 0x002DB808
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

	// Token: 0x0600799F RID: 31135 RVA: 0x002DD73C File Offset: 0x002DB93C
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x060079A0 RID: 31136 RVA: 0x002DD74B File Offset: 0x002DB94B
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

	// Token: 0x060079A1 RID: 31137 RVA: 0x002C422D File Offset: 0x002C242D
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060079A2 RID: 31138 RVA: 0x0008A715 File Offset: 0x00088915
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x060079A3 RID: 31139 RVA: 0x002DD77C File Offset: 0x002DB97C
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

	// Token: 0x040059EB RID: 23019
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x040059EC RID: 23020
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x040059ED RID: 23021
	public new bool Selected;

	// Token: 0x040059EE RID: 23022
	public bool IsHovered;

	// Token: 0x040059EF RID: 23023
	public bool HasRequirement = true;

	// Token: 0x040059F0 RID: 23024
	[PublicizedFrom(EAccessModifier.Private)]
	public static string enabledColor = "255,255,255,255";

	// Token: 0x040059F1 RID: 23025
	[PublicizedFrom(EAccessModifier.Private)]
	public static string disabledColor = "200,200,200,255";

	// Token: 0x040059F2 RID: 23026
	[PublicizedFrom(EAccessModifier.Private)]
	public DialogResponse currentResponse;
}
