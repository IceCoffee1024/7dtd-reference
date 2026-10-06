using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200099B RID: 2459
[Preserve]
public class ObjectiveFetchFromContainer : ObjectiveBaseFetchContainer
{
	// Token: 0x170007BB RID: 1979
	// (get) Token: 0x060047AC RID: 18348 RVA: 0x001C30B9 File Offset: 0x001C12B9
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

	// Token: 0x170007BC RID: 1980
	// (get) Token: 0x060047AD RID: 18349 RVA: 0x001C30C7 File Offset: 0x001C12C7
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed && base.CurrentValue != 3;
		}
	}

	// Token: 0x060047AE RID: 18350 RVA: 0x001C30E0 File Offset: 0x001C12E0
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? QuestEventManager.fetchTag : FastTags<TagGroup.Global>.Parse("hidden_cache"));
	}

	// Token: 0x060047AF RID: 18351 RVA: 0x001C3108 File Offset: 0x001C1308
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			base.SetupExpectedItem();
		}
		base.OwnerQuest.AddQuestTag((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? QuestEventManager.fetchTag : FastTags<TagGroup.Global>.Parse("hidden_cache"));
	}

	// Token: 0x060047B0 RID: 18352 RVA: 0x001C3159 File Offset: 0x001C1359
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
		this.nearbyKeyword = Localization.Get("ObjectiveNearby_keyword", false, null);
	}

	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x060047B1 RID: 18353 RVA: 0x001C3194 File Offset: 0x001C1394
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

	// Token: 0x060047B2 RID: 18354 RVA: 0x001C320C File Offset: 0x001C140C
	public override void HandleFailed()
	{
		base.HandleFailed();
		base.OwnerQuest.RemovePositionData((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		base.OwnerQuest.RemoveMapObject();
	}

	// Token: 0x060047B3 RID: 18355 RVA: 0x001C3238 File Offset: 0x001C1438
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

	// Token: 0x060047B4 RID: 18356 RVA: 0x001C32E5 File Offset: 0x001C14E5
	public override void RemoveObjectives()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
	}

	// Token: 0x060047B5 RID: 18357 RVA: 0x001C3314 File Offset: 0x001C1514
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

	// Token: 0x060047B6 RID: 18358 RVA: 0x001C3380 File Offset: 0x001C1580
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

	// Token: 0x060047B7 RID: 18359 RVA: 0x001C33C0 File Offset: 0x001C15C0
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

	// Token: 0x060047B8 RID: 18360 RVA: 0x001C33FF File Offset: 0x001C15FF
	public override void SetPosition(Quest.PositionDataTypes dataType, Vector3i position)
	{
		if (base.Phase == base.OwnerQuest.CurrentPhase && ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache) == dataType)
		{
			this.FinalizePoint(position);
		}
	}

	// Token: 0x060047B9 RID: 18361 RVA: 0x001C342C File Offset: 0x001C162C
	public override void ResetObjective()
	{
		base.ResetObjective();
		Quest.PositionDataTypes positionDataTypes = ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		base.OwnerQuest.RemovePositionData(positionDataTypes);
	}

	// Token: 0x060047BA RID: 18362 RVA: 0x001C3458 File Offset: 0x001C1658
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

	// Token: 0x060047BB RID: 18363 RVA: 0x001C3568 File Offset: 0x001C1768
	public void FinalizePoint(Vector3i containerPos)
	{
		Quest.PositionDataTypes positionDataTypes = ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		this.position = containerPos.ToVector3();
		base.OwnerQuest.SetPositionData(positionDataTypes, this.position);
		this.lootContainerPos = containerPos;
		base.OwnerQuest.HandleMapObject(positionDataTypes, this.NavObjectName, -1);
		base.CurrentValue = 2;
	}

	// Token: 0x060047BC RID: 18364 RVA: 0x001C35C2 File Offset: 0x001C17C2
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

	// Token: 0x060047BD RID: 18365 RVA: 0x001C3602 File Offset: 0x001C1802
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, TEFeatureStorage lootTE)
	{
		if (containerLocation == this.lootContainerPos && lootTE != null)
		{
			lootTE.RemoveItem(this.expectedItem);
			lootTE.SetModified();
		}
	}

	// Token: 0x060047BE RID: 18366 RVA: 0x001C3628 File Offset: 0x001C1828
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

	// Token: 0x060047BF RID: 18367 RVA: 0x001C36B4 File Offset: 0x001C18B4
	public override BaseObjective Clone()
	{
		ObjectiveFetchFromContainer objectiveFetchFromContainer = new ObjectiveFetchFromContainer();
		this.CopyValues(objectiveFetchFromContainer);
		return objectiveFetchFromContainer;
	}

	// Token: 0x060047C0 RID: 18368 RVA: 0x001C36CF File Offset: 0x001C18CF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveFetchFromContainer objectiveFetchFromContainer = (ObjectiveFetchFromContainer)objective;
		objectiveFetchFromContainer.FetchMode = this.FetchMode;
		objectiveFetchFromContainer.defaultContainer = this.defaultContainer;
	}

	// Token: 0x060047C1 RID: 18369 RVA: 0x001C36F5 File Offset: 0x001C18F5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_NeedSetup()
	{
		this.GetPosition();
	}

	// Token: 0x060047C2 RID: 18370 RVA: 0x001C3700 File Offset: 0x001C1900
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

	// Token: 0x060047C3 RID: 18371 RVA: 0x001C37D0 File Offset: 0x001C19D0
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveFetchFromContainer.PropFetchMode))
		{
			this.FetchMode = EnumUtils.Parse<ObjectiveFetchFromContainer.FetchModeTypes>(properties.Values[ObjectiveFetchFromContainer.PropFetchMode], false);
		}
	}

	// Token: 0x04003988 RID: 14728
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 position;

	// Token: 0x04003989 RID: 14729
	public string nearbyKeyword = "";

	// Token: 0x0400398A RID: 14730
	public static string PropFetchMode = "fetch_mode";

	// Token: 0x0400398B RID: 14731
	public ObjectiveFetchFromContainer.FetchModeTypes FetchMode;

	// Token: 0x0400398C RID: 14732
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i lootContainerPos;

	// Token: 0x0200099C RID: 2460
	public enum FetchModeTypes
	{
		// Token: 0x0400398E RID: 14734
		Standard,
		// Token: 0x0400398F RID: 14735
		Hidden
	}
}
