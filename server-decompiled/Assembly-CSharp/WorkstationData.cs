using System;

// Token: 0x020002E2 RID: 738
public class WorkstationData
{
	// Token: 0x06001540 RID: 5440 RVA: 0x000801A4 File Offset: 0x0007E3A4
	public WorkstationData(string blockName, DynamicProperties properties)
	{
		if (properties.Values.ContainsKey("WorkstationName"))
		{
			this.WorkstationName = properties.Values["WorkstationName"];
		}
		else
		{
			this.WorkstationName = blockName;
		}
		if (properties.Values.ContainsKey("WorkstationIcon"))
		{
			this.WorkstationIcon = properties.Values["WorkstationIcon"];
		}
		else
		{
			this.WorkstationIcon = "ui_game_symbol_hammer";
		}
		if (properties.Values.ContainsKey("CraftActionName"))
		{
			this.CraftActionName = Localization.Get(properties.Values["CraftActionName"], false, null);
		}
		else
		{
			this.CraftActionName = Localization.Get("lblContextActionCraft", false, null);
		}
		if (properties.Values.ContainsKey("CraftIcon"))
		{
			this.CraftIcon = properties.Values["CraftIcon"];
		}
		else
		{
			this.CraftIcon = "ui_game_symbol_hammer";
		}
		if (properties.Values.ContainsKey("OpenSound"))
		{
			this.OpenSound = properties.Values["OpenSound"];
		}
		else
		{
			this.OpenSound = "open_workbench";
		}
		if (properties.Values.ContainsKey("CloseSound"))
		{
			this.CloseSound = properties.Values["CloseSound"];
		}
		else
		{
			this.CloseSound = "close_workbench";
		}
		if (properties.Values.ContainsKey("CraftSound"))
		{
			this.CraftSound = properties.Values["CraftSound"];
		}
		else
		{
			this.CraftSound = "craft_click_craft";
		}
		if (properties.Values.ContainsKey("CraftCompleteSound"))
		{
			this.CraftCompleteSound = properties.Values["CraftCompleteSound"];
		}
		else
		{
			this.CraftCompleteSound = "craft_complete_item";
		}
		if (properties.Values.ContainsKey("WorkstationWindow"))
		{
			this.WorkstationWindow = properties.Values["WorkstationWindow"];
			return;
		}
		this.WorkstationWindow = "";
	}

	// Token: 0x04000E3A RID: 3642
	public string WorkstationName;

	// Token: 0x04000E3B RID: 3643
	public string WorkstationIcon;

	// Token: 0x04000E3C RID: 3644
	public string CraftIcon;

	// Token: 0x04000E3D RID: 3645
	public string CraftActionName = "";

	// Token: 0x04000E3E RID: 3646
	public string WorkstationWindow = "";

	// Token: 0x04000E3F RID: 3647
	public string OpenSound;

	// Token: 0x04000E40 RID: 3648
	public string CloseSound;

	// Token: 0x04000E41 RID: 3649
	public string CraftSound;

	// Token: 0x04000E42 RID: 3650
	public string CraftCompleteSound;
}
