using System;
using UnityEngine.Scripting;

// Token: 0x0200099A RID: 2458
[Preserve]
public class ObjectiveFetchAnyContainer : ObjectiveBaseFetchContainer
{
	// Token: 0x170007BA RID: 1978
	// (get) Token: 0x0600479C RID: 18332 RVA: 0x00020119 File Offset: 0x0001E319
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Number;
		}
	}

	// Token: 0x0600479D RID: 18333 RVA: 0x001C2D1B File Offset: 0x001C0F1B
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetch_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			base.SetupExpectedItem();
		}
		this.itemCount = Convert.ToInt32(this.Value);
	}

	// Token: 0x0600479E RID: 18334 RVA: 0x001C2D50 File Offset: 0x001C0F50
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = string.Format("{0}/{1}", this.currentCount, this.itemCount);
	}

	// Token: 0x0600479F RID: 18335 RVA: 0x001C0B15 File Offset: 0x001BED15
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag(QuestEventManager.fetchTag);
	}

	// Token: 0x060047A0 RID: 18336 RVA: 0x001C2D9F File Offset: 0x001C0F9F
	public override void HandleFailed()
	{
		base.HandleFailed();
	}

	// Token: 0x060047A1 RID: 18337 RVA: 0x001C2DA8 File Offset: 0x001C0FA8
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

	// Token: 0x060047A2 RID: 18338 RVA: 0x001C2E55 File Offset: 0x001C1055
	public override void RemoveObjectives()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
	}

	// Token: 0x060047A3 RID: 18339 RVA: 0x001C2E84 File Offset: 0x001C1084
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

	// Token: 0x060047A4 RID: 18340 RVA: 0x001C2EF0 File Offset: 0x001C10F0
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

	// Token: 0x060047A5 RID: 18341 RVA: 0x001C2F30 File Offset: 0x001C1130
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

	// Token: 0x060047A6 RID: 18342 RVA: 0x001C2F70 File Offset: 0x001C1170
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerOpened(Vector3i containerLocation, TEFeatureStorage tileEntity, bool firstTimeTouched)
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

	// Token: 0x060047A7 RID: 18343 RVA: 0x001C2FD4 File Offset: 0x001C11D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, TEFeatureStorage tileEntity)
	{
		if (tileEntity.blockValue.Block.GetBlockName() == this.defaultContainer)
		{
			tileEntity.RemoveItem(this.expectedItem);
			tileEntity.SetModified();
		}
	}

	// Token: 0x060047A8 RID: 18344 RVA: 0x001C3014 File Offset: 0x001C1214
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

	// Token: 0x060047A9 RID: 18345 RVA: 0x001C307C File Offset: 0x001C127C
	public override BaseObjective Clone()
	{
		ObjectiveFetchAnyContainer objectiveFetchAnyContainer = new ObjectiveFetchAnyContainer();
		this.CopyValues(objectiveFetchAnyContainer);
		return objectiveFetchAnyContainer;
	}

	// Token: 0x060047AA RID: 18346 RVA: 0x001C3097 File Offset: 0x001C1297
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		((ObjectiveFetchAnyContainer)objective).defaultContainer = this.defaultContainer;
	}

	// Token: 0x04003987 RID: 14727
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;
}
