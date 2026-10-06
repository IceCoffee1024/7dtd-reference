using System;
using UnityEngine.Scripting;

// Token: 0x02000D61 RID: 3425
[Preserve]
public class BaseItemActionEntry
{
	// Token: 0x17000B28 RID: 2856
	// (get) Token: 0x060067DF RID: 26591 RVA: 0x00292690 File Offset: 0x00290890
	// (set) Token: 0x060067E0 RID: 26592 RVA: 0x00292698 File Offset: 0x00290898
	public string ActionName { get; set; }

	// Token: 0x17000B29 RID: 2857
	// (get) Token: 0x060067E1 RID: 26593 RVA: 0x002926A1 File Offset: 0x002908A1
	// (set) Token: 0x060067E2 RID: 26594 RVA: 0x002926A9 File Offset: 0x002908A9
	public string IconName { get; set; }

	// Token: 0x17000B2A RID: 2858
	// (get) Token: 0x060067E3 RID: 26595 RVA: 0x002926B2 File Offset: 0x002908B2
	// (set) Token: 0x060067E4 RID: 26596 RVA: 0x002926BA File Offset: 0x002908BA
	public bool Enabled { get; set; }

	// Token: 0x17000B2B RID: 2859
	// (get) Token: 0x060067E5 RID: 26597 RVA: 0x002926C3 File Offset: 0x002908C3
	// (set) Token: 0x060067E6 RID: 26598 RVA: 0x002926CB File Offset: 0x002908CB
	public string SoundName { get; set; }

	// Token: 0x17000B2C RID: 2860
	// (get) Token: 0x060067E7 RID: 26599 RVA: 0x002926D4 File Offset: 0x002908D4
	// (set) Token: 0x060067E8 RID: 26600 RVA: 0x002926DC File Offset: 0x002908DC
	public string DisabledSound { get; set; }

	// Token: 0x17000B2D RID: 2861
	// (get) Token: 0x060067E9 RID: 26601 RVA: 0x002926E5 File Offset: 0x002908E5
	// (set) Token: 0x060067EA RID: 26602 RVA: 0x002926ED File Offset: 0x002908ED
	public XUiController ItemController { get; set; }

	// Token: 0x17000B2E RID: 2862
	// (get) Token: 0x060067EB RID: 26603 RVA: 0x002926F6 File Offset: 0x002908F6
	// (set) Token: 0x060067EC RID: 26604 RVA: 0x002926FE File Offset: 0x002908FE
	public XUiC_ItemActionEntry ParentItem { get; set; }

	// Token: 0x17000B2F RID: 2863
	// (get) Token: 0x060067ED RID: 26605 RVA: 0x00292707 File Offset: 0x00290907
	// (set) Token: 0x060067EE RID: 26606 RVA: 0x0029270F File Offset: 0x0029090F
	public XUiC_ItemActionList ParentActionList { get; set; }

	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x060067EF RID: 26607 RVA: 0x00292718 File Offset: 0x00290918
	// (set) Token: 0x060067F0 RID: 26608 RVA: 0x00292720 File Offset: 0x00290920
	public BaseItemActionEntry.GamepadShortCut ShortCut { get; set; }

	// Token: 0x060067F1 RID: 26609 RVA: 0x0029272C File Offset: 0x0029092C
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

	// Token: 0x060067F2 RID: 26610 RVA: 0x0029277A File Offset: 0x0029097A
	public virtual void RefreshEnabled()
	{
		if (this.ItemController is XUiC_ItemStack)
		{
			this.Enabled = !((XUiC_ItemStack)this.ItemController).StackLock;
		}
	}

	// Token: 0x060067F3 RID: 26611 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnActivated()
	{
	}

	// Token: 0x060067F4 RID: 26612 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnDisabledActivate()
	{
	}

	// Token: 0x060067F5 RID: 26613 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnTimerCompleted()
	{
	}

	// Token: 0x060067F6 RID: 26614 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void DisableEvents()
	{
	}

	// Token: 0x02000D62 RID: 3426
	public enum GamepadShortCut
	{
		// Token: 0x0400501D RID: 20509
		DPadUp,
		// Token: 0x0400501E RID: 20510
		DPadLeft,
		// Token: 0x0400501F RID: 20511
		DPadRight,
		// Token: 0x04005020 RID: 20512
		DPadDown,
		// Token: 0x04005021 RID: 20513
		None,
		// Token: 0x04005022 RID: 20514
		Max
	}
}
