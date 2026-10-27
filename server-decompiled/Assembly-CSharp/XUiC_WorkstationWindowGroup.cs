using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010F8 RID: 4344
[Preserve]
public class XUiC_WorkstationWindowGroup : XUiC_CraftingWindowGroup
{
	// Token: 0x060089F1 RID: 35313 RVA: 0x0034B5CF File Offset: 0x003497CF
	public void SetTileEntity(TileEntityWorkstation _te)
	{
		this.WorkstationData = new XUiM_Workstation(_te);
		this.workstationBlock = _te.GetChunk().GetBlock(_te.localChunkPos);
		base.Workstation = this.workstationBlock.Block.GetBlockName();
	}

	// Token: 0x060089F2 RID: 35314 RVA: 0x0034B60C File Offset: 0x0034980C
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

	// Token: 0x060089F3 RID: 35315 RVA: 0x0034B65E File Offset: 0x0034985E
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_ToolChanged()
	{
		if (this.toolWindow != null)
		{
			this.toolWindow.SetSlots(this.WorkstationData.GetToolStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x060089F4 RID: 35316 RVA: 0x0034B684 File Offset: 0x00349884
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_OutputChanged()
	{
		if (this.outputWindow != null)
		{
			this.outputWindow.SetSlots(this.WorkstationData.GetOutputStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x060089F5 RID: 35317 RVA: 0x0034B6AA File Offset: 0x003498AA
	public void TileEntity_InputChanged()
	{
		if (this.inputWindow != null)
		{
			this.inputWindow.SetSlots(this.WorkstationData.GetInputStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x060089F6 RID: 35318 RVA: 0x0034B6D0 File Offset: 0x003498D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_FuelChanged()
	{
		if (this.fuelWindow != null)
		{
			this.fuelWindow.SetSlots(this.WorkstationData.GetFuelStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x060089F7 RID: 35319 RVA: 0x0034B6F8 File Offset: 0x003498F8
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

	// Token: 0x060089F8 RID: 35320 RVA: 0x0034B7BC File Offset: 0x003499BC
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

	// Token: 0x060089F9 RID: 35321 RVA: 0x0034B828 File Offset: 0x00349A28
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

	// Token: 0x060089FA RID: 35322 RVA: 0x0034B8B8 File Offset: 0x00349AB8
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

	// Token: 0x060089FB RID: 35323 RVA: 0x0034BB00 File Offset: 0x00349D00
	public override bool AddItemToQueue(Recipe _recipe)
	{
		if (this.fuelWindow != null)
		{
			this.fuelWindow.TurnOn();
		}
		return base.AddItemToQueue(_recipe);
	}

	// Token: 0x060089FC RID: 35324 RVA: 0x0034BB1C File Offset: 0x00349D1C
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

	// Token: 0x060089FD RID: 35325 RVA: 0x0034BC7C File Offset: 0x00349E7C
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

	// Token: 0x060089FE RID: 35326 RVA: 0x0034BEE4 File Offset: 0x0034A0E4
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

	// Token: 0x060089FF RID: 35327 RVA: 0x0034BFDC File Offset: 0x0034A1DC
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

	// Token: 0x04006693 RID: 26259
	public XUiM_Workstation WorkstationData;

	// Token: 0x04006694 RID: 26260
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue workstationBlock;

	// Token: 0x04006695 RID: 26261
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationToolGrid toolWindow;

	// Token: 0x04006696 RID: 26262
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationInputGrid inputWindow;

	// Token: 0x04006697 RID: 26263
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationOutputGrid outputWindow;

	// Token: 0x04006698 RID: 26264
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationFuelGrid fuelWindow;

	// Token: 0x04006699 RID: 26265
	[PublicizedFrom(EAccessModifier.Private)]
	public new XUiC_CraftingQueue craftingQueue;

	// Token: 0x0400669A RID: 26266
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label burnTimeLeft;

	// Token: 0x0400669B RID: 26267
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x0400669C RID: 26268
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasCrafting;

	// Token: 0x0400669D RID: 26269
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x0400669E RID: 26270
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x0400669F RID: 26271
	[PublicizedFrom(EAccessModifier.Private)]
	public float openTEUpdateTime;

	// Token: 0x040066A0 RID: 26272
	[PublicizedFrom(EAccessModifier.Private)]
	public const float openTEUpdateTimeMax = 0.5f;
}
