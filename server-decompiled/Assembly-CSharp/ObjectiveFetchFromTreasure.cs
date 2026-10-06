using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200099D RID: 2461
[Preserve]
public class ObjectiveFetchFromTreasure : BaseObjective
{
	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x060047C6 RID: 18374 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x060047C7 RID: 18375 RVA: 0x001C3828 File Offset: 0x001C1A28
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

	// Token: 0x060047C8 RID: 18376 RVA: 0x001C38BB File Offset: 0x001C1ABB
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			this.SetupExpectedItem();
		}
	}

	// Token: 0x060047C9 RID: 18377 RVA: 0x001C38E0 File Offset: 0x001C1AE0
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

	// Token: 0x060047CA RID: 18378 RVA: 0x001C3973 File Offset: 0x001C1B73
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
	}

	// Token: 0x060047CB RID: 18379 RVA: 0x001C399C File Offset: 0x001C1B9C
	public override void HandleCompleted()
	{
		base.HandleCompleted();
		this.RemoveFetchItems();
	}

	// Token: 0x060047CC RID: 18380 RVA: 0x001C39AA File Offset: 0x001C1BAA
	public override void HandlePhaseCompleted()
	{
		base.HandlePhaseCompleted();
	}

	// Token: 0x060047CD RID: 18381 RVA: 0x001C39B2 File Offset: 0x001C1BB2
	public override void HandleFailed()
	{
		base.HandleFailed();
		this.RemoveFetchItems();
	}

	// Token: 0x060047CE RID: 18382 RVA: 0x001C39C0 File Offset: 0x001C1BC0
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

	// Token: 0x060047CF RID: 18383 RVA: 0x001C3A78 File Offset: 0x001C1C78
	public override void RemoveObjectives()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
		QuestEventManager.Current.BlockChange -= this.Current_BlockChange;
	}

	// Token: 0x060047D0 RID: 18384 RVA: 0x001C3AC8 File Offset: 0x001C1CC8
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

	// Token: 0x060047D1 RID: 18385 RVA: 0x001C3B34 File Offset: 0x001C1D34
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

	// Token: 0x060047D2 RID: 18386 RVA: 0x001C3B74 File Offset: 0x001C1D74
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

	// Token: 0x060047D3 RID: 18387 RVA: 0x001C3BB4 File Offset: 0x001C1DB4
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

	// Token: 0x060047D4 RID: 18388 RVA: 0x001C3C54 File Offset: 0x001C1E54
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

	// Token: 0x060047D5 RID: 18389 RVA: 0x001C3D18 File Offset: 0x001C1F18
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

	// Token: 0x060047D6 RID: 18390 RVA: 0x001C3DB4 File Offset: 0x001C1FB4
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetItemCount()
	{
		XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(base.OwnerQuest.OwnerJournal.OwnerPlayer).xui.PlayerInventory;
		this.expectedItem.Meta = base.OwnerQuest.QuestCode;
		return playerInventory.Backpack.GetItemCount(this.expectedItem, -1, base.OwnerQuest.QuestCode, true) + playerInventory.Toolbelt.GetItemCount(this.expectedItem, false, -1, base.OwnerQuest.QuestCode, true);
	}

	// Token: 0x060047D7 RID: 18391 RVA: 0x001C3E38 File Offset: 0x001C2038
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

	// Token: 0x060047D8 RID: 18392 RVA: 0x001C3ECC File Offset: 0x001C20CC
	public override BaseObjective Clone()
	{
		ObjectiveFetchFromTreasure objectiveFetchFromTreasure = new ObjectiveFetchFromTreasure();
		this.CopyValues(objectiveFetchFromTreasure);
		return objectiveFetchFromTreasure;
	}

	// Token: 0x060047D9 RID: 18393 RVA: 0x001C3EE7 File Offset: 0x001C20E7
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

	// Token: 0x060047DA RID: 18394 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool SetLocation(Vector3 pos, Vector3 size)
	{
		return true;
	}

	// Token: 0x060047DB RID: 18395 RVA: 0x001C3F28 File Offset: 0x001C2128
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

	// Token: 0x060047DC RID: 18396 RVA: 0x001C3FDC File Offset: 0x001C21DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("expected_item_id", this.expectedItem.ItemClass.Name);
		parameters.Add("count", this.itemCount);
		parameters.Add("container_name", this.containerName);
		parameters.Add("container_position", this.containerPos.ToString());
	}

	// Token: 0x04003990 RID: 14736
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue expectedItem = ItemValue.None;

	// Token: 0x04003991 RID: 14737
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemClass expectedItemClass;

	// Token: 0x04003992 RID: 14738
	[PublicizedFrom(EAccessModifier.Private)]
	public int itemCount;

	// Token: 0x04003993 RID: 14739
	[PublicizedFrom(EAccessModifier.Private)]
	public int currentCount;

	// Token: 0x04003994 RID: 14740
	[PublicizedFrom(EAccessModifier.Private)]
	public string containerName = "";

	// Token: 0x04003995 RID: 14741
	[PublicizedFrom(EAccessModifier.Private)]
	public string altContainerName = "";

	// Token: 0x04003996 RID: 14742
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasOpened;

	// Token: 0x04003997 RID: 14743
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i containerPos = Vector3i.zero;

	// Token: 0x04003998 RID: 14744
	public static string questItemClassID = "questItem";

	// Token: 0x04003999 RID: 14745
	public static string PropQuestItemID = "quest_item_ID";

	// Token: 0x0400399A RID: 14746
	public static string PropItemCount = "item_count";

	// Token: 0x0400399B RID: 14747
	public static string PropBlock = "block";

	// Token: 0x0400399C RID: 14748
	public static string PropAltBlock = "alt_block";
}
