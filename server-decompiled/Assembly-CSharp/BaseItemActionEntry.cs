using System;
using UnityEngine.Scripting;

// Token: 0x02000D6C RID: 3436
[Preserve]
public class BaseItemActionEntry
{
	// Token: 0x17000B2E RID: 2862
	// (get) Token: 0x06006804 RID: 26628 RVA: 0x00293BB8 File Offset: 0x00291DB8
	// (set) Token: 0x06006805 RID: 26629 RVA: 0x00293BC0 File Offset: 0x00291DC0
	public string ActionName { get; set; }

	// Token: 0x17000B2F RID: 2863
	// (get) Token: 0x06006806 RID: 26630 RVA: 0x00293BC9 File Offset: 0x00291DC9
	// (set) Token: 0x06006807 RID: 26631 RVA: 0x00293BD1 File Offset: 0x00291DD1
	public string IconName { get; set; }

	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x06006808 RID: 26632 RVA: 0x00293BDA File Offset: 0x00291DDA
	// (set) Token: 0x06006809 RID: 26633 RVA: 0x00293BE2 File Offset: 0x00291DE2
	public bool Enabled { get; set; }

	// Token: 0x17000B31 RID: 2865
	// (get) Token: 0x0600680A RID: 26634 RVA: 0x00293BEB File Offset: 0x00291DEB
	// (set) Token: 0x0600680B RID: 26635 RVA: 0x00293BF3 File Offset: 0x00291DF3
	public string SoundName { get; set; }

	// Token: 0x17000B32 RID: 2866
	// (get) Token: 0x0600680C RID: 26636 RVA: 0x00293BFC File Offset: 0x00291DFC
	// (set) Token: 0x0600680D RID: 26637 RVA: 0x00293C04 File Offset: 0x00291E04
	public string DisabledSound { get; set; }

	// Token: 0x17000B33 RID: 2867
	// (get) Token: 0x0600680E RID: 26638 RVA: 0x00293C0D File Offset: 0x00291E0D
	// (set) Token: 0x0600680F RID: 26639 RVA: 0x00293C15 File Offset: 0x00291E15
	public XUiController ItemController { get; set; }

	// Token: 0x17000B34 RID: 2868
	// (get) Token: 0x06006810 RID: 26640 RVA: 0x00293C1E File Offset: 0x00291E1E
	// (set) Token: 0x06006811 RID: 26641 RVA: 0x00293C26 File Offset: 0x00291E26
	public XUiC_ItemActionEntry ParentItem { get; set; }

	// Token: 0x17000B35 RID: 2869
	// (get) Token: 0x06006812 RID: 26642 RVA: 0x00293C2F File Offset: 0x00291E2F
	// (set) Token: 0x06006813 RID: 26643 RVA: 0x00293C37 File Offset: 0x00291E37
	public XUiC_ItemActionList ParentActionList { get; set; }

	// Token: 0x17000B36 RID: 2870
	// (get) Token: 0x06006814 RID: 26644 RVA: 0x00293C40 File Offset: 0x00291E40
	// (set) Token: 0x06006815 RID: 26645 RVA: 0x00293C48 File Offset: 0x00291E48
	public BaseItemActionEntry.GamepadShortCut ShortCut { get; set; }

	// Token: 0x06006816 RID: 26646 RVA: 0x00293C54 File Offset: 0x00291E54
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

	// Token: 0x06006817 RID: 26647 RVA: 0x00293CA2 File Offset: 0x00291EA2
	public virtual void RefreshEnabled()
	{
		if (this.ItemController is XUiC_ItemStack)
		{
			this.Enabled = !((XUiC_ItemStack)this.ItemController).StackLock;
		}
	}

	// Token: 0x06006818 RID: 26648 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnActivated()
	{
	}

	// Token: 0x06006819 RID: 26649 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnDisabledActivate()
	{
	}

	// Token: 0x0600681A RID: 26650 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnTimerCompleted()
	{
	}

	// Token: 0x0600681B RID: 26651 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void DisableEvents()
	{
	}

	// Token: 0x02000D6D RID: 3437
	public enum GamepadShortCut
	{
		// Token: 0x04005042 RID: 20546
		DPadUp,
		// Token: 0x04005043 RID: 20547
		DPadLeft,
		// Token: 0x04005044 RID: 20548
		DPadRight,
		// Token: 0x04005045 RID: 20549
		DPadDown,
		// Token: 0x04005046 RID: 20550
		None,
		// Token: 0x04005047 RID: 20551
		Max
	}
}
