using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009A7 RID: 2471
[Preserve]
public class ObjectiveFetchFromTreasure : BaseObjective
{
	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x060047E9 RID: 18409 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x060047EA RID: 18410 RVA: 0x001C48AC File Offset: 0x001C2AAC
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemoveFetchItems()
	{
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
		XUi xui = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui;
		XUiM_PlayerInventory playerInventory = xui.PlayerInventory;
		int num = 1;
		int num2 = 1;
		num2 -= playerInventory.Backpack.DecItem(this.expectedItem, num2, false, null);
		if (num2 > 0)
		{
			playerInventory.Toolbelt.DecItem(this.expectedItem, num2, false, null);
		}
		if (num != num2)
		{
			xui.CollectedItemList.RemoveItemStack(new ItemStack(this.expectedItem.Clone(), num - num2));
		}
	}

	// Token: 0x060047EB RID: 18411 RVA: 0x001C493F File Offset: 0x001C2B3F
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
	}

	// Token: 0x060047EC RID: 18412 RVA: 0x001C4964 File Offset: 0x001C2B64
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupExpectedItem()
	{
		if (base.OwnerQuest.QuestCode == 0)
		{
			base.OwnerQuest.SetupQuestCode();
		}
		this.expectedItemClass = ItemClass.GetItemClass(ObjectiveFetchFromTreasure.questItemClassID, false);
		int id = this.expectedItemClass.Id;
		ushort num = StringParsers.ParseUInt16(this.ID, 0, -1, NumberStyles.Integer);
		this.expectedItemClass = ItemClassQuest.GetItemQuestById(num);
		this.expectedItem = new ItemValue(id, false);
		this.expectedItem.Seed = num;
		this.expectedItem.Meta = base.OwnerQuest.QuestCode;
		this.itemCount = 1;
	}

	// Token: 0x060047ED RID: 18413 RVA: 0x001C49F7 File Offset: 0x001C2BF7
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x060047EE RID: 18414 RVA: 0x001C4A20 File Offset: 0x001C2C20
	public override void HandleCompleted()
	{
		base.HandleCompleted();
		this.RemoveFetchItems();
	}

	// Token: 0x060047EF RID: 18415 RVA: 0x001C4A2E File Offset: 0x001C2C2E
	public override void HandlePhaseCompleted()
	{
		base.HandlePhaseCompleted();
	}

	// Token: 0x060047F0 RID: 18416 RVA: 0x001C4A36 File Offset: 0x001C2C36
	public override void HandleFailed()
	{
		base.HandleFailed();
		this.RemoveFetchItems();
	}

	// Token: 0x060047F1 RID: 18417 RVA: 0x001C4A44 File Offset: 0x001C2C44
	public override void AddHooks()
	{
		LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer);
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		playerInventory.Backpack.OnBackpackItemsChangedInternal += this.Backpack_OnBackpackItemsChangedInternal;
		playerInventory.Toolbelt.OnToolbeltItemsChangedInternal += this.Toolbelt_OnToolbeltItemsChangedInternal;
		QuestEventManager.Current.ContainerOpened += this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed += this.Current_ContainerClosed;
		QuestEventManager.Current.BlockChange += this.Current_BlockChange;
		this.Refresh();
	}

	// Token: 0x060047F2 RID: 18418 RVA: 0x001C4AFC File Offset: 0x001C2CFC
	public override void RemoveObjectives()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
		QuestEventManager.Current.BlockChange -= this.Current_BlockChange;
	}

	// Token: 0x060047F3 RID: 18419 RVA: 0x001C4B4C File Offset: 0x001C2D4C
	public override void RemoveHooks()
	{
		base.OwnerQuest.RemoveMapObject();
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		if (playerInventory != null)
		{
			playerInventory.Backpack.OnBackpackItemsChangedInternal -= this.Backpack_OnBackpackItemsChangedInternal;
			playerInventory.Toolbelt.OnToolbeltItemsChangedInternal -= this.Toolbelt_OnToolbeltItemsChangedInternal;
		}
	}

	// Token: 0x060047F4 RID: 18420 RVA: 0x001C4BB8 File Offset: 0x001C2DB8
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

	// Token: 0x060047F5 RID: 18421 RVA: 0x001C4BF8 File Offset: 0x001C2DF8
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

	// Token: 0x060047F6 RID: 18422 RVA: 0x001C4C38 File Offset: 0x001C2E38
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerOpened(Vector3i containerLocation, TEFeatureStorage lootTE, bool firstTimeTouched)
	{
		Vector3 zero = Vector3.zero;
		base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.TreasurePoint);
		if ((float)containerLocation.x != zero.x || (float)containerLocation.z != zero.z)
		{
			return;
		}
		if (this.GetItemCount() >= 1)
		{
			return;
		}
		if (GameManager.Instance.World.GetBlock(containerLocation).Block.GetBlockName() == this.containerName && lootTE != null && !lootTE.HasItem(this.expectedItem))
		{
			this.hasOpened = true;
			lootTE.AddItem(new ItemStack(this.expectedItem, 1));
		}
	}

	// Token: 0x060047F7 RID: 18423 RVA: 0x001C4CD8 File Offset: 0x001C2ED8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, TEFeatureStorage lootTE)
	{
		if (GameManager.Instance.World.GetBlock(containerLocation).Block.GetBlockName() == this.containerName && lootTE != null)
		{
			lootTE.RemoveItem(this.expectedItem);
			lootTE.SetModified();
			if (base.Complete)
			{
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					QuestEventManager.Current.FinishTreasureQuest(base.OwnerQuest.QuestCode, base.OwnerQuest.OwnerJournal.OwnerPlayer);
					return;
				}
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestObjectiveUpdate>().Setup(NetPackageQuestObjectiveUpdate.QuestObjectiveEventTypes.TreasureComplete, base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, base.OwnerQuest.QuestCode), false);
			}
		}
	}

	// Token: 0x060047F8 RID: 18424 RVA: 0x001C4D9C File Offset: 0x001C2F9C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_BlockChange(Block blockOld, Block blockNew, Vector3i blockPos)
	{
		if (base.Complete || !this.hasOpened)
		{
			return;
		}
		Vector3 vector;
		base.OwnerQuest.GetPositionData(out vector, Quest.PositionDataTypes.TreasurePoint);
		this.containerPos = new Vector3i(vector);
		if (blockPos != this.containerPos)
		{
			return;
		}
		Chunk chunk = GameManager.Instance.World.GetChunkFromWorldPos(blockPos) as Chunk;
		if (chunk != null && chunk.IsDisplayed)
		{
			string blockName = blockNew.GetBlockName();
			if (blockName != this.containerName && blockName != this.altContainerName)
			{
				base.OwnerQuest.CloseQuest(Quest.QuestState.Failed, null);
			}
		}
	}

	// Token: 0x060047F9 RID: 18425 RVA: 0x001C4E38 File Offset: 0x001C3038
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetItemCount()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		this.expectedItem.Meta = base.OwnerQuest.QuestCode;
		return playerInventory.Backpack.GetItemCount(this.expectedItem, -1, base.OwnerQuest.QuestCode, true) + playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, base.OwnerQuest.QuestCode, true);
	}

	// Token: 0x060047FA RID: 18426 RVA: 0x001C4EBC File Offset: 0x001C30BC
	public override void Refresh()
	{
		if (base.Complete)
		{
			return;
		}
		this.currentCount = this.GetItemCount();
		if (this.currentCount > 1)
		{
			this.currentCount = 1;
		}
		this.SetupDisplay();
		if (this.currentCount != (int)base.CurrentValue)
		{
			base.CurrentValue = (byte)this.currentCount;
		}
		base.Complete = this.currentCount >= this.itemCount && base.OwnerQuest.CheckRequirements();
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
			this.RemoveHooks();
		}
	}

	// Token: 0x060047FB RID: 18427 RVA: 0x001C4F50 File Offset: 0x001C3150
	public override BaseObjective Clone()
	{
		ObjectiveFetchFromTreasure objectiveFetchFromTreasure = new ObjectiveFetchFromTreasure();
		this.CopyValues(objectiveFetchFromTreasure);
		return objectiveFetchFromTreasure;
	}

	// Token: 0x060047FC RID: 18428 RVA: 0x001C4F6B File Offset: 0x001C316B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveFetchFromTreasure objectiveFetchFromTreasure = (ObjectiveFetchFromTreasure)objective;
		objectiveFetchFromTreasure.containerName = this.containerName;
		objectiveFetchFromTreasure.hasOpened = this.hasOpened;
		objectiveFetchFromTreasure.containerName = this.containerName;
		objectiveFetchFromTreasure.altContainerName = this.altContainerName;
	}

	// Token: 0x060047FD RID: 18429 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool SetLocation(Vector3 pos, Vector3 size)
	{
		return true;
	}

	// Token: 0x060047FE RID: 18430 RVA: 0x001C4FAC File Offset: 0x001C31AC
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveFetchFromTreasure.PropQuestItemID))
		{
			this.ID = properties.Values[ObjectiveFetchFromTreasure.PropQuestItemID];
		}
		if (properties.Values.ContainsKey(ObjectiveFetchFromTreasure.PropItemCount))
		{
			this.Value = properties.Values[ObjectiveFetchFromTreasure.PropItemCount];
		}
		if (properties.Values.ContainsKey(ObjectiveFetchFromTreasure.PropBlock))
		{
			this.containerName = properties.Values[ObjectiveFetchFromTreasure.PropBlock];
		}
		if (properties.Values.ContainsKey(ObjectiveFetchFromTreasure.PropAltBlock))
		{
			this.altContainerName = properties.Values[ObjectiveFetchFromTreasure.PropAltBlock];
		}
	}

	// Token: 0x060047FF RID: 18431 RVA: 0x001C5060 File Offset: 0x001C3260
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.itemCount);
		parameters.Add("container_name", this.containerName);
		parameters.Add("container_position", this.containerPos.ToString());
	}

	// Token: 0x040039B0 RID: 14768
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x040039B1 RID: 14769
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x040039B2 RID: 14770
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x040039B3 RID: 14771
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x040039B4 RID: 14772
	[PublicizedFrom(EAccessModifier.Private)]
	public string containerName = "";

	// Token: 0x040039B5 RID: 14773
	[PublicizedFrom(EAccessModifier.Private)]
	public string altContainerName = "";

	// Token: 0x040039B6 RID: 14774
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasOpened;

	// Token: 0x040039B7 RID: 14775
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i containerPos = Vector3i.zero;

	// Token: 0x040039B8 RID: 14776
	public static string questItemClassID = "questItem";

	// Token: 0x040039B9 RID: 14777
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x040039BA RID: 14778
	public static string PropItemCount = "item_count";

	// Token: 0x040039BB RID: 14779
	public static string PropBlock = "block";

	// Token: 0x040039BC RID: 14780
	public static string PropAltBlock = "alt_block";
}
