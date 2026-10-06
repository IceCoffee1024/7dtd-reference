using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020010E8 RID: 4328
[Preserve]
public class XUiC_WorkstationWindowGroup : XUiC_CraftingWindowGroup
{
	// Token: 0x0600899E RID: 35230 RVA: 0x0034954B File Offset: 0x0034774B
	public void SetTileEntity(TileEntityWorkstation _te)
	{
		this.WorkstationData = new XUiM_Workstation(_te);
		this.workstationBlock = _te.GetChunk().GetBlock(_te.localChunkPos);
		base.Workstation = this.workstationBlock.Block.GetBlockName();
	}

	// Token: 0x0600899F RID: 35231 RVA: 0x00349588 File Offset: 0x00347788
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

	// Token: 0x060089A0 RID: 35232 RVA: 0x003495DA File Offset: 0x003477DA
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_ToolChanged()
	{
		if (this.toolWindow != null)
		{
			this.toolWindow.SetSlots(this.WorkstationData.GetToolStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x060089A1 RID: 35233 RVA: 0x00349600 File Offset: 0x00347800
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_OutputChanged()
	{
		if (this.outputWindow != null)
		{
			this.outputWindow.SetSlots(this.WorkstationData.GetOutputStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x060089A2 RID: 35234 RVA: 0x00349626 File Offset: 0x00347826
	public void TileEntity_InputChanged()
	{
		if (this.inputWindow != null)
		{
			this.inputWindow.SetSlots(this.WorkstationData.GetInputStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x060089A3 RID: 35235 RVA: 0x0034964C File Offset: 0x0034784C
	[PublicizedFrom(EAccessModifier.Private)]
	public void TileEntity_FuelChanged()
	{
		if (this.fuelWindow != null)
		{
			this.fuelWindow.SetSlots(this.WorkstationData.GetFuelStacks());
			base.SetAllChildrenDirty();
		}
	}

	// Token: 0x060089A4 RID: 35236 RVA: 0x00349674 File Offset: 0x00347874
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

	// Token: 0x060089A5 RID: 35237 RVA: 0x00349738 File Offset: 0x00347938
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

	// Token: 0x060089A6 RID: 35238 RVA: 0x003497A4 File Offset: 0x003479A4
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

	// Token: 0x060089A7 RID: 35239 RVA: 0x00349834 File Offset: 0x00347A34
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

	// Token: 0x060089A8 RID: 35240 RVA: 0x00349A7C File Offset: 0x00347C7C
	public override bool AddItemToQueue(Recipe _recipe)
	{
		if (this.fuelWindow != null)
		{
			this.fuelWindow.TurnOn();
		}
		return base.AddItemToQueue(_recipe);
	}

	// Token: 0x060089A9 RID: 35241 RVA: 0x00349A98 File Offset: 0x00347C98
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

	// Token: 0x060089AA RID: 35242 RVA: 0x00349BF8 File Offset: 0x00347DF8
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

	// Token: 0x060089AB RID: 35243 RVA: 0x00349E60 File Offset: 0x00348060
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

	// Token: 0x060089AC RID: 35244 RVA: 0x00349F58 File Offset: 0x00348158
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

	// Token: 0x04006654 RID: 26196
	public XUiM_Workstation WorkstationData;

	// Token: 0x04006655 RID: 26197
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockValue workstationBlock;

	// Token: 0x04006656 RID: 26198
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationToolGrid toolWindow;

	// Token: 0x04006657 RID: 26199
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationInputGrid inputWindow;

	// Token: 0x04006658 RID: 26200
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationOutputGrid outputWindow;

	// Token: 0x04006659 RID: 26201
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WorkstationFuelGrid fuelWindow;

	// Token: 0x0400665A RID: 26202
	[PublicizedFrom(EAccessModifier.Private)]
	public new XUiC_CraftingQueue craftingQueue;

	// Token: 0x0400665B RID: 26203
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label burnTimeLeft;

	// Token: 0x0400665C RID: 26204
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x0400665D RID: 26205
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasCrafting;

	// Token: 0x0400665E RID: 26206
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activeKeyDown;

	// Token: 0x0400665F RID: 26207
	[PublicizedFrom(EAccessModifier.Private)]
	public bool wasReleased;

	// Token: 0x04006660 RID: 26208
	[PublicizedFrom(EAccessModifier.Private)]
	public float openTEUpdateTime;

	// Token: 0x04006661 RID: 26209
	[PublicizedFrom(EAccessModifier.Private)]
	public const float openTEUpdateTimeMax = 0.5f;
}
