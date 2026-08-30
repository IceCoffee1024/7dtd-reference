using System;

// Token: 0x020002DF RID: 735
public class WorkstationData
{
	// Token: 0x06001543 RID: 5443 RVA: 0x0007FEF0 File Offset: 0x0007E0F0
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

	// Token: 0x04000E41 RID: 3649
	public string WorkstationName;

	// Token: 0x04000E42 RID: 3650
	public string WorkstationIcon;

	// Token: 0x04000E43 RID: 3651
	public string CraftIcon;

	// Token: 0x04000E44 RID: 3652
	public string CraftActionName = "";

	// Token: 0x04000E45 RID: 3653
	public string WorkstationWindow = "";

	// Token: 0x04000E46 RID: 3654
	public string OpenSound;

	// Token: 0x04000E47 RID: 3655
	public string CloseSound;

	// Token: 0x04000E48 RID: 3656
	public string CraftSound;

	// Token: 0x04000E49 RID: 3657
	public string CraftCompleteSound;
}
