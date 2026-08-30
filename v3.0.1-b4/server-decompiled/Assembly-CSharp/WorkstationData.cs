using System;

// Token: 0x020002DB RID: 731
public class WorkstationData
{
	// Token: 0x06001530 RID: 5424 RVA: 0x0007F834 File Offset: 0x0007DA34
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

	// Token: 0x04000E30 RID: 3632
	public string WorkstationName;

	// Token: 0x04000E31 RID: 3633
	public string WorkstationIcon;

	// Token: 0x04000E32 RID: 3634
	public string CraftIcon;

	// Token: 0x04000E33 RID: 3635
	public string CraftActionName = "";

	// Token: 0x04000E34 RID: 3636
	public string WorkstationWindow = "";

	// Token: 0x04000E35 RID: 3637
	public string OpenSound;

	// Token: 0x04000E36 RID: 3638
	public string CloseSound;

	// Token: 0x04000E37 RID: 3639
	public string CraftSound;

	// Token: 0x04000E38 RID: 3640
	public string CraftCompleteSound;
}
