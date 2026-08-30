using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000984 RID: 2436
[Preserve]
public class ObjectiveFetchFromContainer : ObjectiveBaseFetchContainer
{
	// Token: 0x17000789 RID: 1929
	// (get) Token: 0x06004777 RID: 18295 RVA: 0x001C0119 File Offset: 0x001BE319
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

	// Token: 0x1700078A RID: 1930
	// (get) Token: 0x06004778 RID: 18296 RVA: 0x001C0127 File Offset: 0x001BE327
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed && base.CurrentValue != 3;
		}
	}

	// Token: 0x06004779 RID: 18297 RVA: 0x001C0140 File Offset: 0x001BE340
	public override void SetupQuestTag()
	{
		base.OwnerQuest.AddQuestTag((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? QuestEventManager.fetchTag : FastTags<TagGroup.Global>.Parse("hidden_cache"));
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x001C0168 File Offset: 0x001BE368
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveFetchContainer_keyword", false, null);
		if (this.expectedItemClass == null)
		{
			base.SetupExpectedItem();
		}
		base.OwnerQuest.AddQuestTag((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? QuestEventManager.fetchTag : FastTags<TagGroup.Global>.Parse("hidden_cache"));
	}

	// Token: 0x0600477B RID: 18299 RVA: 0x001C01B9 File Offset: 0x001BE3B9
	public override void SetupDisplay()
	{
		base.Description = string.Format(this.keyword, this.expectedItemClass.GetLocalizedItemName());
		this.StatusText = "";
		this.nearbyKeyword = Localization.Get("ObjectiveNearby_keyword", false, null);
	}

	// Token: 0x1700078B RID: 1931
	// (get) Token: 0x0600477C RID: 18300 RVA: 0x001C01F4 File Offset: 0x001BE3F4
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

	// Token: 0x0600477D RID: 18301 RVA: 0x001C026C File Offset: 0x001BE46C
	public override void HandleFailed()
	{
		base.HandleFailed();
		base.OwnerQuest.RemovePositionData((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		base.OwnerQuest.RemoveMapObject();
	}

	// Token: 0x0600477E RID: 18302 RVA: 0x001C0298 File Offset: 0x001BE498
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

	// Token: 0x0600477F RID: 18303 RVA: 0x001C0345 File Offset: 0x001BE545
	public override void RemoveObjectives()
	{
		QuestEventManager.Current.ContainerOpened -= this.Current_ContainerOpened;
		QuestEventManager.Current.ContainerClosed -= this.Current_ContainerClosed;
	}

	// Token: 0x06004780 RID: 18304 RVA: 0x001C0374 File Offset: 0x001BE574
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

	// Token: 0x06004781 RID: 18305 RVA: 0x001C03E0 File Offset: 0x001BE5E0
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

	// Token: 0x06004782 RID: 18306 RVA: 0x001C0420 File Offset: 0x001BE620
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

	// Token: 0x06004783 RID: 18307 RVA: 0x001C045F File Offset: 0x001BE65F
	public override void SetPosition(Quest.PositionDataTypes dataType, Vector3i position)
	{
		if (base.Phase == base.OwnerQuest.CurrentPhase && ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache) == dataType)
		{
			this.FinalizePoint(position);
		}
	}

	// Token: 0x06004784 RID: 18308 RVA: 0x001C048C File Offset: 0x001BE68C
	public override void ResetObjective()
	{
		base.ResetObjective();
		Quest.PositionDataTypes positionDataTypes = ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		base.OwnerQuest.RemovePositionData(positionDataTypes);
	}

	// Token: 0x06004785 RID: 18309 RVA: 0x001C04B8 File Offset: 0x001BE6B8
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

	// Token: 0x06004786 RID: 18310 RVA: 0x001C05C8 File Offset: 0x001BE7C8
	public void FinalizePoint(Vector3i containerPos)
	{
		Quest.PositionDataTypes positionDataTypes = ((this.FetchMode == ObjectiveFetchFromContainer.FetchModeTypes.Standard) ? Quest.PositionDataTypes.FetchContainer : Quest.PositionDataTypes.HiddenCache);
		this.position = containerPos.ToVector3();
		base.OwnerQuest.SetPositionData(positionDataTypes, this.position);
		this.lootContainerPos = containerPos;
		base.OwnerQuest.HandleMapObject(positionDataTypes, this.NavObjectName, -1);
		base.CurrentValue = 2;
	}

	// Token: 0x06004787 RID: 18311 RVA: 0x001C0622 File Offset: 0x001BE822
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

	// Token: 0x06004788 RID: 18312 RVA: 0x001C0662 File Offset: 0x001BE862
	[PublicizedFrom(EAccessModifier.Private)]
	public void Current_ContainerClosed(Vector3i containerLocation, ITileEntityLootable lootTE)
	{
		if (containerLocation == this.lootContainerPos && lootTE != null)
		{
			lootTE.RemoveItem(this.expectedItem);
			lootTE.SetModified();
		}
	}

	// Token: 0x06004789 RID: 18313 RVA: 0x001C0688 File Offset: 0x001BE888
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

	// Token: 0x0600478A RID: 18314 RVA: 0x001C0714 File Offset: 0x001BE914
	public override BaseObjective Clone()
	{
		ObjectiveFetchFromContainer objectiveFetchFromContainer = new ObjectiveFetchFromContainer();
		this.CopyValues(objectiveFetchFromContainer);
		return objectiveFetchFromContainer;
	}

	// Token: 0x0600478B RID: 18315 RVA: 0x001C072F File Offset: 0x001BE92F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveFetchFromContainer objectiveFetchFromContainer = (ObjectiveFetchFromContainer)objective;
		objectiveFetchFromContainer.FetchMode = this.FetchMode;
		objectiveFetchFromContainer.defaultContainer = this.defaultContainer;
	}

	// Token: 0x0600478C RID: 18316 RVA: 0x001C0755 File Offset: 0x001BE955
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_NeedSetup()
	{
		this.GetPosition();
	}

	// Token: 0x0600478D RID: 18317 RVA: 0x001C0760 File Offset: 0x001BE960
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

	// Token: 0x0600478E RID: 18318 RVA: 0x001C0830 File Offset: 0x001BEA30
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveFetchFromContainer.PropFetchMode))
		{
			this.FetchMode = EnumUtils.Parse<ObjectiveFetchFromContainer.FetchModeTypes>(properties.Values[ObjectiveFetchFromContainer.PropFetchMode], false);
		}
	}

	// Token: 0x040038FB RID: 14587
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3 position;

	// Token: 0x040038FC RID: 14588
	public string nearbyKeyword = "";

	// Token: 0x040038FD RID: 14589
	public static string PropFetchMode = "fetch_mode";

	// Token: 0x040038FE RID: 14590
	public ObjectiveFetchFromContainer.FetchModeTypes FetchMode;

	// Token: 0x040038FF RID: 14591
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i lootContainerPos;

	// Token: 0x02000985 RID: 2437
	public enum FetchModeTypes
	{
		// Token: 0x04003901 RID: 14593
		Standard,
		// Token: 0x04003902 RID: 14594
		Hidden
	}
}
