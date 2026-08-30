using System;
using UnityEngine.Scripting;

// Token: 0x02000994 RID: 2452
[Preserve]
public class ObjectiveFetchAnyContainer : ObjectiveBaseFetchContainer
{
	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x060047ED RID: 18413 RVA: 0x00020021 File Offset: 0x0001E221
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x060047EE RID: 18414 RVA: 0x001C256F File Offset: 0x001C076F
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetch_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			base.SetupExpectedItem();
		}
		this.itemCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x060047EF RID: 18415 RVA: 0x001C25A4 File Offset: 0x001C07A4
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", this.currentCount, this.itemCount);
	}

	// Token: 0x060047F0 RID: 18416 RVA: 0x001C056D File Offset: 0x001BE76D
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.fetchTag);
	}

	// Token: 0x060047F1 RID: 18417 RVA: 0x001C25F3 File Offset: 0x001C07F3
	public override void HandleFailed()
	{
		base.HandleFailed();
	}

	// Token: 0x060047F2 RID: 18418 RVA: 0x001C25FC File Offset: 0x001C07FC
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

	// Token: 0x060047F3 RID: 18419 RVA: 0x001C26A9 File Offset: 0x001C08A9
	public override void RemoveObjectives()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
	}

	// Token: 0x060047F4 RID: 18420 RVA: 0x001C26D8 File Offset: 0x001C08D8
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

	// Token: 0x060047F5 RID: 18421 RVA: 0x001C2744 File Offset: 0x001C0944
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

	// Token: 0x060047F6 RID: 18422 RVA: 0x001C2784 File Offset: 0x001C0984
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

	// Token: 0x060047F7 RID: 18423 RVA: 0x001C27C4 File Offset: 0x001C09C4
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

	// Token: 0x060047F8 RID: 18424 RVA: 0x001C2828 File Offset: 0x001C0A28
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, ITileEntityLootable tileEntity)
	{
		if (tileEntity.blockValue.Block.GetBlockName() == this.defaultContainer)
		{
			tileEntity.RemoveItem(this.expectedItem);
			tileEntity.SetModified();
		}
	}

	// Token: 0x060047F9 RID: 18425 RVA: 0x001C2868 File Offset: 0x001C0A68
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

	// Token: 0x060047FA RID: 18426 RVA: 0x001C28D0 File Offset: 0x001C0AD0
	public override BaseObjective Clone()
	{
		ObjectiveFetchAnyContainer objectiveFetchAnyContainer = new ObjectiveFetchAnyContainer();
		this.CopyValues(objectiveFetchAnyContainer);
		return objectiveFetchAnyContainer;
	}

	// Token: 0x060047FB RID: 18427 RVA: 0x001C28EB File Offset: 0x001C0AEB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		((ObjectiveFetchAnyContainer)objective).defaultContainer = this.defaultContainer;
	}

	// Token: 0x0400398B RID: 14731
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;
}
