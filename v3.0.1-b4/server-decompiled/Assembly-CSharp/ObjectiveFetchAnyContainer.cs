using System;
using UnityEngine.Scripting;

// Token: 0x02000983 RID: 2435
[Preserve]
public class ObjectiveFetchAnyContainer : ObjectiveBaseFetchContainer
{
	// Token: 0x17000788 RID: 1928
	// (get) Token: 0x06004767 RID: 18279 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x06004768 RID: 18280 RVA: 0x001BFD7B File Offset: 0x001BDF7B
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetch_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			base.SetupExpectedItem();
		}
		this.itemCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x001BFDB0 File Offset: 0x001BDFB0
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", this.currentCount, this.itemCount);
	}

	// Token: 0x0600476A RID: 18282 RVA: 0x001BDD79 File Offset: 0x001BBF79
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.fetchTag);
	}

	// Token: 0x0600476B RID: 18283 RVA: 0x001BFDFF File Offset: 0x001BDFFF
	public override void HandleFailed()
	{
		base.HandleFailed();
	}

	// Token: 0x0600476C RID: 18284 RVA: 0x001BFE08 File Offset: 0x001BE008
	public override void AddHooks()
	{
		base.CurrentValue = 0;
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
		LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		playerInventory.Backpack.OnBackpackItemsChangedInternal += this.Backpack_OnBackpackItemsChangedInternal;
		playerInventory.Toolbelt.OnToolbeltItemsChangedInternal += this.Toolbelt_OnToolbeltItemsChangedInternal;
		QuestEventManager.Current.ContainerOpened += this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed += this.Current_ContainerClosed;
	}

	// Token: 0x0600476D RID: 18285 RVA: 0x001BFEB5 File Offset: 0x001BE0B5
	public override void RemoveObjectives()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
	}

	// Token: 0x0600476E RID: 18286 RVA: 0x001BFEE4 File Offset: 0x001BE0E4
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		if (playerInventory != null)
		{
			playerInventory.Backpack.OnBackpackItemsChangedInternal -= this.Backpack_OnBackpackItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal -= this.Toolbelt_OnToolbeltItemsChangedInternal;
		}
	}

	// Token: 0x0600476F RID: 18287 RVA: 0x001BFF50 File Offset: 0x001BE150
	[PublicizedFrom(EAccessModifier.Private)]
	public void Backpack_OnBackpackItemsChangedInternal()
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		if (base.Complete || uiforPlayer.xui.PlayerInventory == null)
		{
			return;
		}
		this.Refresh();
	}

	// Token: 0x06004770 RID: 18288 RVA: 0x001BFF90 File Offset: 0x001BE190
	[PublicizedFrom(EAccessModifier.Private)]
	public void Toolbelt_OnToolbeltItemsChangedInternal()
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		if (base.Complete || uiforPlayer.xui.PlayerInventory == null)
		{
			return;
		}
		this.Refresh();
	}

	// Token: 0x06004771 RID: 18289 RVA: 0x001BFFD0 File Offset: 0x001BE1D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerOpened(Vector3i containerLocation, ITileEntityLootable tileEntity)
	{
		if (base.GetItemCount(-2) >= this.itemCount)
		{
			return;
		}
		if (tileEntity.blockValue.Block.GetBlockName() == this.defaultContainer && !tileEntity.HasItem(this.expectedItem))
		{
			tileEntity.AddItem(new ItemStack(this.expectedItem, this.itemCount));
		}
	}

	// Token: 0x06004772 RID: 18290 RVA: 0x001C0034 File Offset: 0x001BE234
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, ITileEntityLootable tileEntity)
	{
		if (tileEntity.blockValue.Block.GetBlockName() == this.defaultContainer)
		{
			tileEntity.RemoveItem(this.expectedItem);
			tileEntity.SetModified();
		}
	}

	// Token: 0x06004773 RID: 18291 RVA: 0x001C0074 File Offset: 0x001BE274
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		this.currentCount = base.GetItemCount(-2);
		if (this.currentCount == 0)
		{
			return;
		}
		this.SetupDisplay();
		base.CurrentValue = 3;
		base.Complete = base.OwnerQuest.CheckRequirements();
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
			this.RemoveHooks();
		}
	}

	// Token: 0x06004774 RID: 18292 RVA: 0x001C00DC File Offset: 0x001BE2DC
	public override BaseObjective Clone()
	{
		ObjectiveFetchAnyContainer objectiveFetchAnyContainer = new ObjectiveFetchAnyContainer();
		this.CopyValues(objectiveFetchAnyContainer);
		return objectiveFetchAnyContainer;
	}

	// Token: 0x06004775 RID: 18293 RVA: 0x001C00F7 File Offset: 0x001BE2F7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		((ObjectiveFetchAnyContainer)objective).defaultContainer = this.defaultContainer;
	}

	// Token: 0x040038FA RID: 14586
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;
}
