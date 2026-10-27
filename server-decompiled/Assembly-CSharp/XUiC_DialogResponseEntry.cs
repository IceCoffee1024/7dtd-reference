using System;
using UnityEngine.Scripting;

// Token: 0x02000F41 RID: 3905
[Preserve]
public class XUiC_DialogResponseEntry : XUiController
{
	// Token: 0x17000E5D RID: 3677
	// (get) Token: 0x060079DA RID: 31194 RVA: 0x002E21C0 File Offset: 0x002E03C0
	// (set) Token: 0x060079DB RID: 31195 RVA: 0x002E21C8 File Offset: 0x002E03C8
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

	// Token: 0x060079DC RID: 31196 RVA: 0x002E2294 File Offset: 0x002E0494
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

	// Token: 0x060079DD RID: 31197 RVA: 0x002E23C8 File Offset: 0x002E05C8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x060079DE RID: 31198 RVA: 0x002E23D7 File Offset: 0x002E05D7
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

	// Token: 0x060079DF RID: 31199 RVA: 0x002C8B51 File Offset: 0x002C6D51
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x060079E0 RID: 31200 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x060079E1 RID: 31201 RVA: 0x002E2408 File Offset: 0x002E0608
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

	// Token: 0x04005A56 RID: 23126
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04005A57 RID: 23127
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04005A58 RID: 23128
	public new bool Selected;

	// Token: 0x04005A59 RID: 23129
	public bool IsHovered;

	// Token: 0x04005A5A RID: 23130
	public bool HasRequirement = true;

	// Token: 0x04005A5B RID: 23131
	[PublicizedFrom(EAccessModifier.Private)]
	public static string enabledColor = "255,255,255,255";

	// Token: 0x04005A5C RID: 23132
	[PublicizedFrom(EAccessModifier.Private)]
	public static string disabledColor = "200,200,200,255";

	// Token: 0x04005A5D RID: 23133
	[PublicizedFrom(EAccessModifier.Private)]
	public DialogResponse currentResponse;
}
