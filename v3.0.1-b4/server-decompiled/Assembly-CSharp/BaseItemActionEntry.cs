using System;
using UnityEngine.Scripting;

// Token: 0x02000D3E RID: 3390
[Preserve]
public class BaseItemActionEntry
{
	// Token: 0x17000AF5 RID: 2805
	// (get) Token: 0x0600674C RID: 26444 RVA: 0x0028C244 File Offset: 0x0028A444
	// (set) Token: 0x0600674D RID: 26445 RVA: 0x0028C24C File Offset: 0x0028A44C
	public string ActionName { get; set; }

	// Token: 0x17000AF6 RID: 2806
	// (get) Token: 0x0600674E RID: 26446 RVA: 0x0028C255 File Offset: 0x0028A455
	// (set) Token: 0x0600674F RID: 26447 RVA: 0x0028C25D File Offset: 0x0028A45D
	public string IconName { get; set; }

	// Token: 0x17000AF7 RID: 2807
	// (get) Token: 0x06006750 RID: 26448 RVA: 0x0028C266 File Offset: 0x0028A466
	// (set) Token: 0x06006751 RID: 26449 RVA: 0x0028C26E File Offset: 0x0028A46E
	public bool Enabled { get; set; }

	// Token: 0x17000AF8 RID: 2808
	// (get) Token: 0x06006752 RID: 26450 RVA: 0x0028C277 File Offset: 0x0028A477
	// (set) Token: 0x06006753 RID: 26451 RVA: 0x0028C27F File Offset: 0x0028A47F
	public string SoundName { get; set; }

	// Token: 0x17000AF9 RID: 2809
	// (get) Token: 0x06006754 RID: 26452 RVA: 0x0028C288 File Offset: 0x0028A488
	// (set) Token: 0x06006755 RID: 26453 RVA: 0x0028C290 File Offset: 0x0028A490
	public string DisabledSound { get; set; }

	// Token: 0x17000AFA RID: 2810
	// (get) Token: 0x06006756 RID: 26454 RVA: 0x0028C299 File Offset: 0x0028A499
	// (set) Token: 0x06006757 RID: 26455 RVA: 0x0028C2A1 File Offset: 0x0028A4A1
	public XUiController ItemController { get; set; }

	// Token: 0x17000AFB RID: 2811
	// (get) Token: 0x06006758 RID: 26456 RVA: 0x0028C2AA File Offset: 0x0028A4AA
	// (set) Token: 0x06006759 RID: 26457 RVA: 0x0028C2B2 File Offset: 0x0028A4B2
	public XUiC_ItemActionEntry ParentItem { get; set; }

	// Token: 0x17000AFC RID: 2812
	// (get) Token: 0x0600675A RID: 26458 RVA: 0x0028C2BB File Offset: 0x0028A4BB
	// (set) Token: 0x0600675B RID: 26459 RVA: 0x0028C2C3 File Offset: 0x0028A4C3
	public XUiC_ItemActionList ParentActionList { get; set; }

	// Token: 0x17000AFD RID: 2813
	// (get) Token: 0x0600675C RID: 26460 RVA: 0x0028C2CC File Offset: 0x0028A4CC
	// (set) Token: 0x0600675D RID: 26461 RVA: 0x0028C2D4 File Offset: 0x0028A4D4
	public BaseItemActionEntry.GamepadShortCut ShortCut { get; set; }

	// Token: 0x0600675E RID: 26462 RVA: 0x0028C2E0 File Offset: 0x0028A4E0
	public BaseItemActionEntry(XUiController itemController, string actionName, string spriteName, BaseItemActionEntry.GamepadShortCut shortcut = BaseItemActionEntry.GamepadShortCut.None, string soundName = "crafting/craft_click_craft", string disabledSoundName = "ui/ui_denied")
	{
		this.ItemController = itemController;
		this.ActionName = Localization.Get(actionName, false, null);
		this.IconName = spriteName;
		this.SoundName = soundName;
		this.DisabledSound = disabledSoundName;
		this.Enabled = true;
		this.ShortCut = shortcut;
	}

	// Token: 0x0600675F RID: 26463 RVA: 0x0028C32E File Offset: 0x0028A52E
	public virtual void RefreshEnabled()
	{
		if (this.ItemController is XUiC_ItemStack)
		{
			this.Enabled = !((XUiC_ItemStack)this.ItemController).StackLock;
		}
	}

	// Token: 0x06006760 RID: 26464 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnActivated()
	{
	}

	// Token: 0x06006761 RID: 26465 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnDisabledActivate()
	{
	}

	// Token: 0x06006762 RID: 26466 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnTimerCompleted()
	{
	}

	// Token: 0x06006763 RID: 26467 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void DisableEvents()
	{
	}

	// Token: 0x02000D3F RID: 3391
	public enum GamepadShortCut
	{
		// Token: 0x04004F40 RID: 20288
		DPadUp,
		// Token: 0x04004F41 RID: 20289
		DPadLeft,
		// Token: 0x04004F42 RID: 20290
		DPadRight,
		// Token: 0x04004F43 RID: 20291
		DPadDown,
		// Token: 0x04004F44 RID: 20292
		None,
		// Token: 0x04004F45 RID: 20293
		Max
	}
}
