using System;
using UnityEngine.Scripting;

// Token: 0x02000F32 RID: 3890
[Preserve]
public class XUiC_DialogResponseEntry : XUiController
{
	// Token: 0x17000E53 RID: 3667
	// (get) Token: 0x06007997 RID: 31127 RVA: 0x002E08D0 File Offset: 0x002DEAD0
	// (set) Token: 0x06007998 RID: 31128 RVA: 0x002E08D8 File Offset: 0x002DEAD8
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

	// Token: 0x06007999 RID: 31129 RVA: 0x002E09A4 File Offset: 0x002DEBA4
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

	// Token: 0x0600799A RID: 31130 RVA: 0x002E0AD8 File Offset: 0x002DECD8
	public override void Init()
	{
		base.Init();
		this.IsDirty = true;
	}

	// Token: 0x0600799B RID: 31131 RVA: 0x002E0AE7 File Offset: 0x002DECE7
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

	// Token: 0x0600799C RID: 31132 RVA: 0x002C75E5 File Offset: 0x002C57E5
	public override void Update(float _dt)
	{
		base.RefreshBindings();
		this.IsDirty = false;
		base.Update(_dt);
	}

	// Token: 0x0600799D RID: 31133 RVA: 0x0008A995 File Offset: 0x00088B95
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x0600799E RID: 31134 RVA: 0x002E0B18 File Offset: 0x002DED18
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

	// Token: 0x04005A23 RID: 23075
	[PublicizedFrom(EAccessModifier.Private)]
	public string rowColor;

	// Token: 0x04005A24 RID: 23076
	[PublicizedFrom(EAccessModifier.Private)]
	public string hoverColor;

	// Token: 0x04005A25 RID: 23077
	public new bool Selected;

	// Token: 0x04005A26 RID: 23078
	public bool IsHovered;

	// Token: 0x04005A27 RID: 23079
	public bool HasRequirement = true;

	// Token: 0x04005A28 RID: 23080
	[PublicizedFrom(EAccessModifier.Private)]
	public static string enabledColor = "255,255,255,255";

	// Token: 0x04005A29 RID: 23081
	[PublicizedFrom(EAccessModifier.Private)]
	public static string disabledColor = "200,200,200,255";

	// Token: 0x04005A2A RID: 23082
	[PublicizedFrom(EAccessModifier.Private)]
	public DialogResponse currentResponse;
}
