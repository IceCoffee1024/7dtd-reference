using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000995 RID: 2453
[Preserve]
public class ObjectiveFetchFromContainer : ObjectiveBaseFetchContainer
{
	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x060047FD RID: 18429 RVA: 0x001C290D File Offset: 0x001C0B0D
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			if (base.CurrentValue != 3)
			{
				return BaseObjective.ObjectiveValueTypes.Distance;
			}
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x060047FE RID: 18430 RVA: 0x001C291B File Offset: 0x001C0B1B
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed && base.CurrentValue != 3;
		}
	}

	// Token: 0x060047FF RID: 18431 RVA: 0x001C2934 File Offset: 0x001C0B34
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? QuestEventManager.fetchTag : FastTags<TagGroup.Global>.Parse("hidden_cache"));
	}

	// Token: 0x06004800 RID: 18432 RVA: 0x001C295C File Offset: 0x001C0B5C
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			base.SetupExpectedItem();
		}
		base.OwnerQuest.AddQuestTag((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? QuestEventManager.fetchTag : FastTags<TagGroup.Global>.Parse("hidden_cache"));
	}

	// Token: 0x06004801 RID: 18433 RVA: 0x001C29AD File Offset: 0x001C0BAD
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
		this.nearbyKeyword = Localization.Get("ObjectiveNearby_keyword", false, null);
	}

	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x06004802 RID: 18434 RVA: 0x001C29E8 File Offset: 0x001C0BE8
	public override string StatusText
	{
		get
		{
			if (base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
			{
				if (this.FetchMode != ObjectiveFetchFromContainer.FetchModeTypes.Standard && this.distance < 10f)
				{
					return this.nearbyKeyword;
				}
				return ValueDisplayFormatters.Distance(this.distance);
			}
			else
			{
				if (base.OwnerQuest.CurrentState == Quest.QuestState.NotStarted)
				{
					return "";
				}
				if (base.ObjectiveState == BaseObjective.ObjectiveStates.Failed)
				{
					return Localization.Get("failed", false, null);
				}
				return Localization.Get("completed", false, null);
			}
		}
	}

	// Token: 0x06004803 RID: 18435 RVA: 0x001C2A60 File Offset: 0x001C0C60
	public override void HandleFailed()
	{
		base.HandleFailed();
		base.OwnerQuest.RemovePositionData((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		base.OwnerQuest.RemoveMapObject();
	}

	// Token: 0x06004804 RID: 18436 RVA: 0x001C2A8C File Offset: 0x001C0C8C
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

	// Token: 0x06004805 RID: 18437 RVA: 0x001C2B39 File Offset: 0x001C0D39
	public override void RemoveObjectives()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
	}

	// Token: 0x06004806 RID: 18438 RVA: 0x001C2B68 File Offset: 0x001C0D68
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

	// Token: 0x06004807 RID: 18439 RVA: 0x001C2BD4 File Offset: 0x001C0DD4
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

	// Token: 0x06004808 RID: 18440 RVA: 0x001C2C14 File Offset: 0x001C0E14
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

	// Token: 0x06004809 RID: 18441 RVA: 0x001C2C53 File Offset: 0x001C0E53
	public override void SetPosition(Quest.PositionDataTypes dataType, Vector3i position)
	{
		if (base.Phase == base.OwnerQuest.CurrentPhase && ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache) == dataType)
		{
			this.FinalizePoint(position);
		}
	}

	// Token: 0x0600480A RID: 18442 RVA: 0x001C2C80 File Offset: 0x001C0E80
	public override void ResetObjective()
	{
		base.ResetObjective();
		Quest.PositionDataTypes positionDataTypes = ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		base.OwnerQuest.RemovePositionData(positionDataTypes);
	}

	// Token: 0x0600480B RID: 18443 RVA: 0x001C2CAC File Offset: 0x001C0EAC
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 GetPosition()
	{
		Quest.PositionDataTypes positionDataTypes = ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		if (base.OwnerQuest.GetPositionData(out this.position, positionDataTypes))
		{
			base.OwnerQuest.HandleMapObject(positionDataTypes, this.NavObjectName, -1);
			base.CurrentValue = 2;
			return this.position;
		}
		Vector3 zero = Vector3.zero;
		base.OwnerQuest.GetPositionData(out zero, Quest.PositionDataTypes.POIPosition);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (base.OwnerQuest.SharedOwnerID == -1)
			{
				QuestEventManager.Current.SetupFetchForMP(base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, zero, this.FetchMode, base.OwnerQuest.GetSharedWithIDList());
			}
			base.CurrentValue = 2;
		}
		else
		{
			if (base.OwnerQuest.SharedOwnerID == -1)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageQuestEvent>().Setup(NetPackageQuestEvent.QuestEventTypes.SetupFetch, base.OwnerQuest.OwnerJournal.OwnerPlayer.entityId, zero, this.FetchMode, base.OwnerQuest.GetSharedWithIDList()), false);
			}
			base.CurrentValue = 1;
		}
		return Vector3.zero;
	}

	// Token: 0x0600480C RID: 18444 RVA: 0x001C2DBC File Offset: 0x001C0FBC
	public void FinalizePoint(Vector3i containerPos)
	{
		Quest.PositionDataTypes positionDataTypes = ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		this.position = containerPos.ToVector3();
		base.OwnerQuest.SetPositionData(positionDataTypes, this.position);
		this.lootContainerPos = containerPos;
		base.OwnerQuest.HandleMapObject(positionDataTypes, this.NavObjectName, -1);
		base.CurrentValue = 2;
	}

	// Token: 0x0600480D RID: 18445 RVA: 0x001C2E16 File Offset: 0x001C1016
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerOpened(Vector3i containerLocation, ITileEntityLootable lootTE)
	{
		if (base.GetItemCount(-2) >= 1)
		{
			return;
		}
		if (containerLocation == this.lootContainerPos && lootTE != null && !lootTE.HasItem(this.expectedItem))
		{
			lootTE.AddItem(new ItemStack(this.expectedItem, 1));
		}
	}

	// Token: 0x0600480E RID: 18446 RVA: 0x001C2E56 File Offset: 0x001C1056
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, ITileEntityLootable lootTE)
	{
		if (containerLocation == this.lootContainerPos && lootTE != null)
		{
			lootTE.RemoveItem(this.expectedItem);
			lootTE.SetModified();
		}
	}

	// Token: 0x0600480F RID: 18447 RVA: 0x001C2E7C File Offset: 0x001C107C
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
			base.OwnerQuest.RemovePositionData((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
			base.OwnerQuest.RemoveMapObject();
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
			this.RemoveHooks();
		}
	}

	// Token: 0x06004810 RID: 18448 RVA: 0x001C2F08 File Offset: 0x001C1108
	public override BaseObjective Clone()
	{
		ObjectiveFetchFromContainer objectiveFetchFromContainer = new ObjectiveFetchFromContainer();
		this.CopyValues(objectiveFetchFromContainer);
		return objectiveFetchFromContainer;
	}

	// Token: 0x06004811 RID: 18449 RVA: 0x001C2F23 File Offset: 0x001C1123
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveFetchFromContainer objectiveFetchFromContainer = (ObjectiveFetchFromContainer)objective;
		objectiveFetchFromContainer.FetchMode = this.FetchMode;
		objectiveFetchFromContainer.defaultContainer = this.defaultContainer;
	}

	// Token: 0x06004812 RID: 18450 RVA: 0x001C2F49 File Offset: 0x001C1149
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_NeedSetup()
	{
		this.GetPosition();
	}

	// Token: 0x06004813 RID: 18451 RVA: 0x001C2F54 File Offset: 0x001C1154
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_Update()
	{
		Entity ownerPlayer = base.OwnerQuest.OwnerJournal.OwnerPlayer;
		this.position.y = 0f;
		Vector3 vector = ownerPlayer.position;
		vector.y = 0f;
		this.distance = Vector3.Distance(vector, this.position);
		if (this.world == null)
		{
			this.world = GameManager.Instance.World;
		}
		if (this.distance < 5f)
		{
			BlockValue block = this.world.GetBlock(this.lootContainerPos);
			if (block.Block.IndexName == null || !block.Block.IndexName.EqualsCaseInsensitive("fetchcontainer"))
			{
				this.world.SetBlockRPC(this.lootContainerPos, Block.GetBlockValue(this.defaultContainer, false));
			}
		}
	}

	// Token: 0x06004814 RID: 18452 RVA: 0x001C3024 File Offset: 0x001C1224
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveFetchFromContainer.PropFetchMode))
		{
			this.FetchMode = EnumUtils.Parse<ObjectiveFetchFromContainer.FetchModeTypes>(properties.Values[ObjectiveFetchFromContainer.PropFetchMode], false);
		}
	}

	// Token: 0x0400398C RID: 14732
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 position;

	// Token: 0x0400398D RID: 14733
	public string nearbyKeyword = "";

	// Token: 0x0400398E RID: 14734
	public static string PropFetchMode = "fetch_mode";

	// Token: 0x0400398F RID: 14735
	public ObjectiveFetchFromContainer.FetchModeTypes FetchMode;

	// Token: 0x04003990 RID: 14736
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i lootContainerPos;

	// Token: 0x02000996 RID: 2454
	public enum FetchModeTypes
	{
		// Token: 0x04003992 RID: 14738
		Standard,
		// Token: 0x04003993 RID: 14739
		Hidden
	}
}
