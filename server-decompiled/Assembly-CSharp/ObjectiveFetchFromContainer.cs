using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009A5 RID: 2469
[Preserve]
public class ObjectiveFetchFromContainer : ObjectiveBaseFetchContainer
{
	// Token: 0x170007C1 RID: 1985
	// (get) Token: 0x060047CF RID: 18383 RVA: 0x001C413D File Offset: 0x001C233D
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

	// Token: 0x170007C2 RID: 1986
	// (get) Token: 0x060047D0 RID: 18384 RVA: 0x001C414B File Offset: 0x001C234B
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed && base.CurrentValue != 3;
		}
	}

	// Token: 0x060047D1 RID: 18385 RVA: 0x001C4164 File Offset: 0x001C2364
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? QuestEventManager.fetchTag : FastTags<TagGroup.Global>.Parse("hidden_cache"));
	}

	// Token: 0x060047D2 RID: 18386 RVA: 0x001C418C File Offset: 0x001C238C
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			base.SetupExpectedItem();
		}
		base.OwnerQuest.AddQuestTag((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? QuestEventManager.fetchTag : FastTags<TagGroup.Global>.Parse("hidden_cache"));
	}

	// Token: 0x060047D3 RID: 18387 RVA: 0x001C41DD File Offset: 0x001C23DD
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
		this.nearbyKeyword = Localization.Get("ObjectiveNearby_keyword", false, null);
	}

	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x060047D4 RID: 18388 RVA: 0x001C4218 File Offset: 0x001C2418
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

	// Token: 0x060047D5 RID: 18389 RVA: 0x001C4290 File Offset: 0x001C2490
	public override void HandleFailed()
	{
		base.HandleFailed();
		base.OwnerQuest.RemovePositionData((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		base.OwnerQuest.RemoveMapObject();
	}

	// Token: 0x060047D6 RID: 18390 RVA: 0x001C42BC File Offset: 0x001C24BC
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

	// Token: 0x060047D7 RID: 18391 RVA: 0x001C4369 File Offset: 0x001C2569
	public override void RemoveObjectives()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
	}

	// Token: 0x060047D8 RID: 18392 RVA: 0x001C4398 File Offset: 0x001C2598
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

	// Token: 0x060047D9 RID: 18393 RVA: 0x001C4404 File Offset: 0x001C2604
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

	// Token: 0x060047DA RID: 18394 RVA: 0x001C4444 File Offset: 0x001C2644
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

	// Token: 0x060047DB RID: 18395 RVA: 0x001C4483 File Offset: 0x001C2683
	public override void SetPosition(Quest.PositionDataTypes dataType, Vector3i position)
	{
		if (base.Phase == base.OwnerQuest.CurrentPhase && ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache) == dataType)
		{
			this.FinalizePoint(position);
		}
	}

	// Token: 0x060047DC RID: 18396 RVA: 0x001C44B0 File Offset: 0x001C26B0
	public override void ResetObjective()
	{
		base.ResetObjective();
		Quest.PositionDataTypes positionDataTypes = ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		base.OwnerQuest.RemovePositionData(positionDataTypes);
	}

	// Token: 0x060047DD RID: 18397 RVA: 0x001C44DC File Offset: 0x001C26DC
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

	// Token: 0x060047DE RID: 18398 RVA: 0x001C45EC File Offset: 0x001C27EC
	public void FinalizePoint(Vector3i containerPos)
	{
		Quest.PositionDataTypes positionDataTypes = ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		this.position = containerPos.ToVector3();
		base.OwnerQuest.SetPositionData(positionDataTypes, this.position);
		this.lootContainerPos = containerPos;
		base.OwnerQuest.HandleMapObject(positionDataTypes, this.NavObjectName, -1);
		base.CurrentValue = 2;
	}

	// Token: 0x060047DF RID: 18399 RVA: 0x001C4646 File Offset: 0x001C2846
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerOpened(Vector3i containerLocation, TEFeatureStorage lootTE, bool firstTimeTouched)
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

	// Token: 0x060047E0 RID: 18400 RVA: 0x001C4686 File Offset: 0x001C2886
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, TEFeatureStorage lootTE)
	{
		if (containerLocation == this.lootContainerPos && lootTE != null)
		{
			lootTE.RemoveItem(this.expectedItem);
			lootTE.SetModified();
		}
	}

	// Token: 0x060047E1 RID: 18401 RVA: 0x001C46AC File Offset: 0x001C28AC
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

	// Token: 0x060047E2 RID: 18402 RVA: 0x001C4738 File Offset: 0x001C2938
	public override BaseObjective Clone()
	{
		ObjectiveFetchFromContainer objectiveFetchFromContainer = new ObjectiveFetchFromContainer();
		this.CopyValues(objectiveFetchFromContainer);
		return objectiveFetchFromContainer;
	}

	// Token: 0x060047E3 RID: 18403 RVA: 0x001C4753 File Offset: 0x001C2953
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveFetchFromContainer objectiveFetchFromContainer = (ObjectiveFetchFromContainer)objective;
		objectiveFetchFromContainer.FetchMode = this.FetchMode;
		objectiveFetchFromContainer.defaultContainer = this.defaultContainer;
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x001C4779 File Offset: 0x001C2979
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_NeedSetup()
	{
		this.GetPosition();
	}

	// Token: 0x060047E5 RID: 18405 RVA: 0x001C4784 File Offset: 0x001C2984
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

	// Token: 0x060047E6 RID: 18406 RVA: 0x001C4854 File Offset: 0x001C2A54
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveFetchFromContainer.PropFetchMode))
		{
			this.FetchMode = EnumUtils.Parse<ObjectiveFetchFromContainer.FetchModeTypes>(properties.Values[ObjectiveFetchFromContainer.PropFetchMode], false);
		}
	}

	// Token: 0x040039A8 RID: 14760
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 position;

	// Token: 0x040039A9 RID: 14761
	public string nearbyKeyword = "";

	// Token: 0x040039AA RID: 14762
	public static string PropFetchMode = "fetch_mode";

	// Token: 0x040039AB RID: 14763
	public ObjectiveFetchFromContainer.FetchModeTypes FetchMode;

	// Token: 0x040039AC RID: 14764
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i lootContainerPos;

	// Token: 0x020009A6 RID: 2470
	public enum FetchModeTypes
	{
		// Token: 0x040039AE RID: 14766
		Standard,
		// Token: 0x040039AF RID: 14767
		Hidden
	}
}
