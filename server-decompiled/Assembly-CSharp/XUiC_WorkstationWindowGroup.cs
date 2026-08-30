using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010D7 RID: 4311
[Preserve]
public class XUiC_WorkstationWindowGroup : XUiC_CraftingWindowGroup
{
	// Token: 0x06008950 RID: 35152 RVA: 0x0034579B File Offset: 0x0034399B
	public void SetTileEntity(TileEntityWorkstation _te)
	{
		this.WorkstationData = new XUiM_Workstation(_te);
		this.workstationBlock = _te.GetChunk().GetBlock(_te.localChunkPos);
		base.Workstation = this.workstationBlock.Block.GetBlockName();
	}

	// Token: 0x06008951 RID: 35153 RVA: 0x003457D8 File Offset: 0x003439D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_Destroyed(ITileEntity te)
	{
		if (this.WorkstationData == null || this.WorkstationData.TileEntity == te)
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
			return;
		}
		te.Destroyed -= this.TileEntity_Destroyed;
	}

	// Token: 0x06008952 RID: 35154 RVA: 0x0034582A File Offset: 0x00343A2A
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_ToolChanged()
	{
		if (this.toolWindow != null)
		{
			this.toolWindow.SetSlots(this.WorkstationData.GetToolStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x06008953 RID: 35155 RVA: 0x00345850 File Offset: 0x00343A50
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_OutputChanged()
	{
		if (this.outputWindow != null)
		{
			this.outputWindow.SetSlots(this.WorkstationData.GetOutputStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x06008954 RID: 35156 RVA: 0x00345876 File Offset: 0x00343A76
	public void TileEntity_InputChanged()
	{
		if (this.inputWindow != null)
		{
			this.inputWindow.SetSlots(this.WorkstationData.GetInputStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x06008955 RID: 35157 RVA: 0x0034589C File Offset: 0x00343A9C
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_FuelChanged()
	{
		if (this.fuelWindow != null)
		{
			this.fuelWindow.SetSlots(this.WorkstationData.GetFuelStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x06008956 RID: 35158 RVA: 0x003458C4 File Offset: 0x00343AC4
	public override void Init()
	{
		base.Init();
		XUiController xuiController = base.GetChildByType<XUiC_WorkstationToolGrid>();
		if (xuiController != null)
		{
			this.toolWindow = (XUiC_WorkstationToolGrid)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_WorkstationInputGrid>();
		if (xuiController != null)
		{
			this.inputWindow = (XUiC_WorkstationInputGrid)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_WorkstationOutputGrid>();
		if (xuiController != null)
		{
			this.outputWindow = (XUiC_WorkstationOutputGrid)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_WorkstationFuelGrid>();
		if (xuiController != null)
		{
			this.fuelWindow = (XUiC_WorkstationFuelGrid)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_CraftingQueue>();
		if (xuiController != null)
		{
			this.craftingQueue = (XUiC_CraftingQueue)xuiController;
		}
		xuiController = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		if (xuiController != null)
		{
			this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		}
		xuiController = base.GetChildById("burnTimeLeft");
		if (xuiController != null)
		{
			this.burnTimeLeft = (XUiV_Label)base.GetChildById("burnTimeLeft").ViewComponent;
		}
	}

	// Token: 0x06008957 RID: 35159 RVA: 0x00345988 File Offset: 0x00343B88
	public override bool CraftingRequirementsValid(Recipe _recipe)
	{
		bool flag = true;
		if (this.toolWindow != null)
		{
			flag &= this.toolWindow.HasRequirement(_recipe);
		}
		if (this.inputWindow != null)
		{
			flag &= this.inputWindow.HasRequirement(_recipe);
		}
		if (this.outputWindow != null)
		{
			flag &= this.outputWindow.HasRequirement(_recipe);
		}
		if (this.fuelWindow != null)
		{
			flag &= this.fuelWindow.HasRequirement(_recipe);
		}
		return flag;
	}

	// Token: 0x06008958 RID: 35160 RVA: 0x003459F4 File Offset: 0x00343BF4
	public override string CraftingRequirementsInvalidMessage(Recipe _recipe)
	{
		if (this.toolWindow != null && !this.toolWindow.HasRequirement(_recipe))
		{
			return Localization.Get("ttMissingCraftingTools", false, null);
		}
		if (this.inputWindow != null && !this.inputWindow.HasRequirement(_recipe))
		{
			return Localization.Get("ttMissingCraftingResources", false, null);
		}
		if (this.outputWindow != null)
		{
			this.outputWindow.HasRequirement(_recipe);
		}
		if (this.fuelWindow != null && !this.fuelWindow.HasRequirement(_recipe))
		{
			return Localization.Get("ttMissingCraftingFuel", false, null);
		}
		return "";
	}

	// Token: 0x06008959 RID: 35161 RVA: 0x00345A84 File Offset: 0x00343C84
	public override void Update(float _dt)
	{
		base.Update(_dt);
		bool flag = this.craftingQueue.IsCrafting();
		if (flag != this.wasCrafting)
		{
			this.wasCrafting = flag;
			this.syncTEfromUI();
		}
		if (this.windowGroup.isShowing)
		{
			if (this.WorkstationData != null && GameManager.Instance != null && GameManager.Instance.World != null && this.WorkstationData.TileEntity.IsBurning)
			{
				if (this.openTEUpdateTime <= 0f)
				{
					this.WorkstationData.TileEntity.UpdateTick(GameManager.Instance.World);
					this.openTEUpdateTime = 0.5f;
				}
				else
				{
					this.openTEUpdateTime -= _dt;
				}
			}
			if (!this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
			{
				this.wasReleased = true;
			}
			if (this.wasReleased)
			{
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.IsPressed)
				{
					this.activeKeyDown = true;
				}
				if (this.xui.playerUI.playerInput.PermanentActions.Activate.WasReleased && this.activeKeyDown)
				{
					this.activeKeyDown = false;
					if (!this.xui.playerUI.windowManager.IsInputActive())
					{
						this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, false);
					}
				}
			}
		}
		if (this.fuelWindow != null && this.craftingQueue != null && this.burnTimeLeft != null && this.WorkstationData != null)
		{
			float totalBurnTimeLeft = this.WorkstationData.GetTotalBurnTimeLeft();
			if ((!this.WorkstationData.GetIsBurning() || totalBurnTimeLeft == 0f) && this.craftingQueue.IsCrafting())
			{
				this.craftingQueue.HaltCrafting();
			}
			else if (this.WorkstationData.GetIsBurning() && totalBurnTimeLeft == 0f && !this.craftingQueue.IsCrafting())
			{
				this.craftingQueue.ResumeCrafting();
			}
			this.burnTimeLeft.Text = string.Format("{0}:{1}", ((int)(totalBurnTimeLeft / 60f)).ToString("00"), ((int)(totalBurnTimeLeft % 60f)).ToString("00"));
		}
	}

	// Token: 0x0600895A RID: 35162 RVA: 0x00345CCC File Offset: 0x00343ECC
	public override bool AddItemToQueue(Recipe _recipe)
	{
		if (this.fuelWindow != null)
		{
			this.fuelWindow.TurnOn();
		}
		return base.AddItemToQueue(_recipe);
	}

	// Token: 0x0600895B RID: 35163 RVA: 0x00345CE8 File Offset: 0x00343EE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void syncTEfromUI()
	{
		TileEntityWorkstation tileEntity = this.WorkstationData.TileEntity;
		tileEntity.SetDisableModifiedCheck(true);
		if (this.toolWindow != null)
		{
			this.WorkstationData.SetToolStacks(this.toolWindow.GetSlots());
		}
		if (this.inputWindow != null)
		{
			this.WorkstationData.SetInputStacks(this.inputWindow.GetSlots());
		}
		if (this.outputWindow != null)
		{
			this.WorkstationData.SetOutputStacks(this.outputWindow.GetSlots());
		}
		if (this.fuelWindow != null)
		{
			this.WorkstationData.SetFuelStacks(this.fuelWindow.GetSlots());
		}
		if (this.craftingQueue != null)
		{
			XUiC_RecipeStack[] recipesToCraft = this.craftingQueue.GetRecipesToCraft();
			RecipeQueueItem[] array = new RecipeQueueItem[recipesToCraft.Length];
			for (int i = 0; i < recipesToCraft.Length; i++)
			{
				array[i] = new RecipeQueueItem
				{
					Recipe = recipesToCraft[i].GetRecipe(),
					Multiplier = (short)recipesToCraft[i].GetRecipeCount(),
					CraftingTimeLeft = recipesToCraft[i].GetRecipeCraftingTimeLeft(),
					IsCrafting = recipesToCraft[i].IsCrafting,
					Quality = (byte)recipesToCraft[i].OutputQuality,
					StartingEntityId = recipesToCraft[i].StartingEntityId,
					OneItemCraftTime = recipesToCraft[i].GetOneItemCraftTime()
				};
			}
			this.WorkstationData.SetRecipeQueueItems(array);
		}
		tileEntity.SetDisableModifiedCheck(false);
		tileEntity.SetModified();
		tileEntity.ResetTickTime();
	}

	// Token: 0x0600895C RID: 35164 RVA: 0x00345E48 File Offset: 0x00344048
	public override void OnOpen()
	{
		this.WorkstationData.SetUserAccessing(true);
		this.WorkstationData.TileEntity.FuelChanged += this.TileEntity_FuelChanged;
		this.WorkstationData.TileEntity.InputChanged += this.TileEntity_InputChanged;
		this.WorkstationData.TileEntity.Destroyed += this.TileEntity_Destroyed;
		this.openTEUpdateTime = 0.5f;
		if (base.ViewComponent != null && !base.ViewComponent.IsVisible)
		{
			base.ViewComponent.OnOpen();
			base.ViewComponent.IsVisible = true;
		}
		if (this.recipeList != null && this.categoryList != null && this.categoryList.SetupCategoriesByWorkstation(base.Workstation))
		{
			this.recipeList.Workstation = base.Workstation;
			this.categoryList.SetCategoryToFirst();
		}
		if (this.nonPagingHeaderWindow != null)
		{
			this.nonPagingHeaderWindow.SetHeader(Localization.Get(base.Workstation, false, null));
		}
		GUIWindowManager windowManager = this.xui.playerUI.windowManager;
		this.syncUIfromTE();
		if (this.craftingQueue != null)
		{
			this.craftingQueue.ClearQueue();
			RecipeQueueItem[] recipeQueueItems = this.WorkstationData.GetRecipeQueueItems();
			for (int i = 0; i < recipeQueueItems.Length; i++)
			{
				if (recipeQueueItems[i] != null)
				{
					this.craftingQueue.AddRecipeToCraftAtIndex(i, recipeQueueItems[i].Recipe, (int)recipeQueueItems[i].Multiplier, recipeQueueItems[i].CraftingTimeLeft, recipeQueueItems[i].IsCrafting, false, (int)recipeQueueItems[i].Quality, recipeQueueItems[i].StartingEntityId, recipeQueueItems[i].OneItemCraftTime);
				}
				else
				{
					this.craftingQueue.AddRecipeToCraftAtIndex(i, null, 0, -1f, false, true, -1, -1, -1f);
				}
			}
			this.craftingQueue.IsDirty = true;
		}
		this.xui.RecenterWindowGroup(this.windowGroup, false);
		for (int j = 0; j < this.children.Count; j++)
		{
			this.children[j].OnOpen();
		}
		WorkstationData workstationData = CraftingManager.GetWorkstationData(this.workstation);
		if (workstationData != null)
		{
			Manager.BroadcastPlayByLocalPlayer(this.WorkstationData.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f, workstationData.OpenSound);
			this.WorkstationData.TileEntity.CheckForCraftComplete(this.xui.playerUI.entityPlayer);
		}
		this.IsDirty = true;
	}

	// Token: 0x0600895D RID: 35165 RVA: 0x003460B0 File Offset: 0x003442B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void syncUIfromTE()
	{
		if (this.WorkstationData != null && GameManager.Instance != null && GameManager.Instance.World != null)
		{
			this.WorkstationData.TileEntity.UpdateTick(GameManager.Instance.World);
		}
		if (this.toolWindow != null)
		{
			this.toolWindow.SetSlots(this.WorkstationData.GetToolStacks());
			this.toolWindow.IsDirty = true;
		}
		if (this.inputWindow != null)
		{
			this.inputWindow.SetSlots(this.WorkstationData.GetInputStacks());
			this.inputWindow.IsDirty = true;
		}
		if (this.outputWindow != null)
		{
			this.outputWindow.SetSlots(this.WorkstationData.GetOutputStacks());
			this.outputWindow.IsDirty = true;
		}
		if (this.fuelWindow != null)
		{
			this.fuelWindow.SetSlots(this.WorkstationData.GetFuelStacks());
			this.fuelWindow.IsDirty = true;
		}
		base.SetAllChildrenDirty();
	}

	// Token: 0x0600895E RID: 35166 RVA: 0x003461A8 File Offset: 0x003443A8
	public override void OnClose()
	{
		this.wasReleased = false;
		this.activeKeyDown = false;
		this.syncTEfromUI();
		this.WorkstationData.SetUserAccessing(false);
		this.WorkstationData.TileEntity.FuelChanged -= this.TileEntity_FuelChanged;
		this.WorkstationData.TileEntity.InputChanged -= this.TileEntity_InputChanged;
		this.WorkstationData.TileEntity.Destroyed -= this.TileEntity_Destroyed;
		base.OnClose();
		LockManager.Instance.UnlockRequestLocal();
		WorkstationData workstationData = CraftingManager.GetWorkstationData(this.workstation);
		if (workstationData != null)
		{
			Manager.BroadcastPlayByLocalPlayer(this.WorkstationData.TileEntity.ToWorldPos().ToVector3() + Vector3.one * 0.5f, workstationData.CloseSound);
		}
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.MenuShortcuts);
	}

	// Token: 0x04006612 RID: 26130
	public XUiM_Workstation WorkstationData;

	// Token: 0x04006613 RID: 26131
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue workstationBlock;

	// Token: 0x04006614 RID: 26132
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationToolGrid toolWindow;

	// Token: 0x04006615 RID: 26133
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationInputGrid inputWindow;

	// Token: 0x04006616 RID: 26134
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationOutputGrid outputWindow;

	// Token: 0x04006617 RID: 26135
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationFuelGrid fuelWindow;

	// Token: 0x04006618 RID: 26136
	[PublicizedFrom(EAccessModifier.Private)]
	public new XUiC_CraftingQueue craftingQueue;

	// Token: 0x04006619 RID: 26137
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label burnTimeLeft;

	// Token: 0x0400661A RID: 26138
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x0400661B RID: 26139
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasCrafting;

	// Token: 0x0400661C RID: 26140
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x0400661D RID: 26141
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x0400661E RID: 26142
	[PublicizedFrom(EAccessModifier.Private)]
	public float openTEUpdateTime;

	// Token: 0x0400661F RID: 26143
	[PublicizedFrom(EAccessModifier.Private)]
	public const float openTEUpdateTimeMax = 0.5f;
}
