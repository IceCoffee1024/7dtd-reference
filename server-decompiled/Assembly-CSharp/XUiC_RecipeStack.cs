using System;
using System.Collections;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200100F RID: 4111
[Preserve]
public class XUiC_RecipeStack : XUiController
{
	// Token: 0x17000F1F RID: 3871
	// (get) Token: 0x060081E1 RID: 33249 RVA: 0x00314B74 File Offset: 0x00312D74
	// (set) Token: 0x060081E2 RID: 33250 RVA: 0x00314B7C File Offset: 0x00312D7C
	public int OutputQuality
	{
		get
		{
			return this.outputQuality;
		}
		set
		{
			this.outputQuality = value;
		}
	}

	// Token: 0x17000F20 RID: 3872
	// (get) Token: 0x060081E3 RID: 33251 RVA: 0x00314B85 File Offset: 0x00312D85
	// (set) Token: 0x060081E4 RID: 33252 RVA: 0x00314B8D File Offset: 0x00312D8D
	public int StartingEntityId
	{
		get
		{
			return this.startingEntityId;
		}
		set
		{
			this.startingEntityId = value;
		}
	}

	// Token: 0x17000F21 RID: 3873
	// (get) Token: 0x060081E5 RID: 33253 RVA: 0x00314B96 File Offset: 0x00312D96
	// (set) Token: 0x060081E6 RID: 33254 RVA: 0x00314B9E File Offset: 0x00312D9E
	public ItemValue OriginalItem
	{
		get
		{
			return this.originalItem;
		}
		set
		{
			this.originalItem = value;
		}
	}

	// Token: 0x17000F22 RID: 3874
	// (get) Token: 0x060081E7 RID: 33255 RVA: 0x00314BA7 File Offset: 0x00312DA7
	// (set) Token: 0x060081E8 RID: 33256 RVA: 0x00314BAF File Offset: 0x00312DAF
	public int AmountToRepair
	{
		get
		{
			return this.amountToRepair;
		}
		set
		{
			this.amountToRepair = value;
		}
	}

	// Token: 0x17000F23 RID: 3875
	// (get) Token: 0x060081E9 RID: 33257 RVA: 0x00314BB8 File Offset: 0x00312DB8
	// (set) Token: 0x060081EA RID: 33258 RVA: 0x00314BC0 File Offset: 0x00312DC0
	public bool IsCrafting
	{
		get
		{
			return this.isCrafting;
		}
		set
		{
			this.isCrafting = value;
		}
	}

	// Token: 0x17000F24 RID: 3876
	// (get) Token: 0x060081EB RID: 33259 RVA: 0x00314BC9 File Offset: 0x00312DC9
	// (set) Token: 0x060081EC RID: 33260 RVA: 0x00314BEE File Offset: 0x00312DEE
	public string LockIconSprite
	{
		get
		{
			if (this.lockIcon != null)
			{
				return ((XUiV_Sprite)this.lockIcon.ViewComponent).SpriteName;
			}
			return "";
		}
		set
		{
			if (this.lockIcon != null)
			{
				((XUiV_Sprite)this.lockIcon.ViewComponent).SpriteName = value;
			}
		}
	}

	// Token: 0x060081ED RID: 33261 RVA: 0x00314C10 File Offset: 0x00312E10
	public void CopyTo(XUiC_RecipeStack _recipeStack)
	{
		_recipeStack.recipe = this.recipe;
		_recipeStack.craftingTimeLeft = this.craftingTimeLeft;
		_recipeStack.totalCraftTimeLeft = this.totalCraftTimeLeft;
		_recipeStack.recipeCount = this.recipeCount;
		_recipeStack.IsCrafting = this.IsCrafting;
		_recipeStack.originalItem = this.originalItem;
		_recipeStack.amountToRepair = this.amountToRepair;
		_recipeStack.LockIconSprite = this.LockIconSprite;
		_recipeStack.outputQuality = this.outputQuality;
		_recipeStack.startingEntityId = this.startingEntityId;
		_recipeStack.outputItemValue = this.outputItemValue;
		_recipeStack.oneItemCraftTime = this.oneItemCraftTime;
	}

	// Token: 0x060081EE RID: 33262 RVA: 0x00314CB0 File Offset: 0x00312EB0
	public override void Init()
	{
		base.Init();
		this.background = base.GetChildById("background");
		this.overlay = base.GetChildById("overlay");
		this.lockIcon = base.GetChildById("lockIcon");
		this.itemIcon = base.GetChildById("itemIcon");
		this.timer = base.GetChildById("timer");
		this.count = base.GetChildById("count");
		this.cancel = base.GetChildById("cancel");
		if (this.background != null)
		{
			this.background.OnPress += this.HandleOnPress;
			this.background.OnHover += this.HandleOnHover;
		}
		this.inventoryFullDropping = Localization.Get("xuiInventoryFullDropping", false, null);
	}

	// Token: 0x060081EF RID: 33263 RVA: 0x00314D82 File Offset: 0x00312F82
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnHover(XUiController _sender, bool _isOver)
	{
		this.isOver = _isOver;
	}

	// Token: 0x060081F0 RID: 33264 RVA: 0x00314D8B File Offset: 0x00312F8B
	public void ForceCancel()
	{
		this.HandleOnPress(null, -1);
	}

	// Token: 0x060081F1 RID: 33265 RVA: 0x00314D98 File Offset: 0x00312F98
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnPress(XUiController _sender, int _mouseButton)
	{
		if (this.recipe == null)
		{
			return;
		}
		XUiC_WorkstationMaterialInputGrid childByType = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationMaterialInputGrid>();
		XUiC_WorkstationInputGrid childByType2 = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationInputGrid>();
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (this.amountToRepair <= 0 && childByType != null)
		{
			for (int i = 0; i < this.recipe.ingredients.Count; i++)
			{
				childByType.SetWeight(this.recipe.ingredients[i].itemValue.Clone(), this.recipe.ingredients[i].count * this.recipeCount);
			}
		}
		else
		{
			if (this.originalItem != null && !this.originalItem.IsEmpty())
			{
				ItemStack itemStack = new ItemStack(this.originalItem.Clone(), 1);
				if (!this.xui.PlayerInventory.AddItem(itemStack))
				{
					GameManager.ShowTooltip(entityPlayer, this.inventoryFullDropping, false, false, 0f);
					GameManager.Instance.ItemDropServer(new ItemStack(this.originalItem.Clone(), 1), entityPlayer.position, Vector3.zero, entityPlayer.entityId, 120f, false);
				}
				this.originalItem = ItemValue.None;
			}
			int[] array = new int[this.recipe.ingredients.Count];
			for (int j = 0; j < this.recipe.ingredients.Count; j++)
			{
				array[j] = this.recipe.ingredients[j].count * this.recipeCount;
				if (array[j] != 0)
				{
					ItemStack itemStack2 = new ItemStack(this.recipe.ingredients[j].itemValue.Clone(), array[j]);
					bool flag;
					if (this.amountToRepair <= 0 && childByType2 != null)
					{
						flag = childByType2.AddToItemStackArray(itemStack2) != -1;
					}
					else
					{
						if (itemStack2.itemValue.ItemClass.IsResourceUnit)
						{
							string replaceResourceUnit = itemStack2.itemValue.ItemClass.ReplaceResourceUnit;
							if (replaceResourceUnit != "")
							{
								itemStack2.itemValue = ItemClass.GetItem(replaceResourceUnit, false);
							}
						}
						flag = this.xui.PlayerInventory.AddItem(itemStack2, true);
					}
					if (flag)
					{
						array[j] = 0;
					}
					else
					{
						array[j] = itemStack2.count;
					}
				}
			}
			bool flag2 = false;
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k] > 0)
				{
					flag2 = true;
					GameManager.Instance.ItemDropServer(new ItemStack(this.recipe.ingredients[k].itemValue.Clone(), array[k]), entityPlayer.position, Vector3.zero, entityPlayer.entityId, 120f, false);
				}
			}
			if (flag2)
			{
				GameManager.ShowTooltip(this.xui.playerUI.entityPlayer, this.inventoryFullDropping, false, false, 0f);
			}
		}
		this.isCrafting = false;
		this.ClearRecipe();
		XUiC_CraftingQueue owner = this.Owner;
		if (owner != null)
		{
			owner.RefreshQueue();
		}
		this.windowGroup.Controller.SetAllChildrenDirty();
	}

	// Token: 0x060081F2 RID: 33266 RVA: 0x003150B8 File Offset: 0x003132B8
	public override void Update(float _dt)
	{
		if (this.isInventoryFull)
		{
			if (this.recipe != null && this.outputItemValue != null)
			{
				XUiC_WorkstationOutputGrid childByType = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationOutputGrid>();
				bool flag = false;
				ItemStack[] array = new ItemStack[0];
				if (childByType != null)
				{
					array = childByType.GetSlots();
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].CanStackWith(new ItemStack(this.outputItemValue, this.recipe.count), false))
						{
							array[i].count += this.recipe.count;
							flag = true;
							break;
						}
						if (array[i].IsEmpty())
						{
							array[i] = new ItemStack(this.outputItemValue, this.recipe.count);
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					childByType.SetSlots(array);
					childByType.UpdateData(array);
					childByType.IsDirty = true;
					this.isInventoryFull = false;
					this.recipeCount--;
					if (this.recipeCount <= 0)
					{
						this.isCrafting = false;
						if (this.recipe != null || this.craftingTimeLeft != 0f)
						{
							this.ClearRecipe();
						}
					}
					else
					{
						this.craftingTimeLeft += this.oneItemCraftTime;
					}
					base.Update(_dt);
					return;
				}
				if (!this.xui.DragAndDropWindow.CurrentStack.IsEmpty() && this.xui.DragAndDropWindow.CurrentStack.itemValue.ItemClass is ItemClassQuest)
				{
					base.Update(_dt);
					return;
				}
				ItemStack itemStack = new ItemStack(this.outputItemValue, this.recipe.count);
				if (!this.xui.PlayerInventory.AddItemNoPartial(itemStack, false))
				{
					this.updateRecipeData();
					if (itemStack.count != this.recipe.count)
					{
						this.xui.PlayerInventory.DropItem(itemStack);
						QuestEventManager.Current.CraftedItem(itemStack);
						this.isInventoryFull = false;
						this.recipeCount--;
						if (this.recipeCount <= 0)
						{
							this.isCrafting = false;
							if (this.recipe != null || this.craftingTimeLeft != 0f)
							{
								this.ClearRecipe();
							}
						}
						else
						{
							this.craftingTimeLeft += this.oneItemCraftTime;
						}
					}
					base.Update(_dt);
					return;
				}
				QuestEventManager.Current.CraftedItem(new ItemStack(this.outputItemValue, this.recipe.count));
				this.isInventoryFull = false;
				this.recipeCount--;
				if (this.recipeCount <= 0)
				{
					this.isCrafting = false;
					if (this.recipe != null || this.craftingTimeLeft != 0f)
					{
						this.ClearRecipe();
					}
				}
				else
				{
					this.craftingTimeLeft += this.oneItemCraftTime;
				}
				base.Update(_dt);
				return;
			}
			else
			{
				this.isInventoryFull = false;
				this.isCrafting = false;
			}
		}
		if (this.recipe == null)
		{
			this.isCrafting = false;
		}
		if (this.recipeCount > 0)
		{
			if (this.isCrafting && this.craftingTimeLeft <= 0f && this.recipe != null && this.outputStack())
			{
				this.recipeCount--;
				if (this.recipeCount <= 0)
				{
					this.isCrafting = false;
					if (this.recipe != null || this.craftingTimeLeft != 0f)
					{
						this.ClearRecipe();
					}
				}
				else
				{
					this.craftingTimeLeft += this.oneItemCraftTime;
				}
			}
		}
		else
		{
			this.isCrafting = false;
			if (this.recipe != null && (this.recipe != null || this.craftingTimeLeft != 0f))
			{
				this.ClearRecipe();
			}
		}
		if (base.ViewComponent.IsVisible)
		{
			this.updateRecipeData();
		}
		if (this.recipeCount > 0 && this.isCrafting)
		{
			this.craftingTimeLeft -= _dt;
			this.totalCraftTimeLeft = this.oneItemCraftTime * ((float)this.recipeCount - 1f) + this.craftingTimeLeft;
		}
		else
		{
			if (this.craftingTimeLeft < 0f)
			{
				this.craftingTimeLeft = 0f;
			}
			if (this.totalCraftTimeLeft < 0f)
			{
				this.totalCraftTimeLeft = 0f;
			}
		}
		base.Update(_dt);
	}

	// Token: 0x060081F3 RID: 33267 RVA: 0x003154D0 File Offset: 0x003136D0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool outputStack()
	{
		if (this.recipe == null)
		{
			return false;
		}
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (entityPlayer == null)
		{
			return false;
		}
		if (this.originalItem == null || this.originalItem.IsEmpty())
		{
			this.outputItemValue = new ItemValue(this.recipe.itemValueType, this.outputQuality, this.outputQuality, false, null, 1f);
			ItemClass itemClass = this.outputItemValue.ItemClass;
			if (this.outputItemValue == null)
			{
				return false;
			}
			if (itemClass == null)
			{
				return false;
			}
			if (entityPlayer.entityId == this.startingEntityId)
			{
				this.giveExp(this.outputItemValue, itemClass);
			}
			else
			{
				XUiC_WorkstationWindowGroup xuiC_WorkstationWindowGroup = this.windowGroup.Controller as XUiC_WorkstationWindowGroup;
				if (xuiC_WorkstationWindowGroup != null)
				{
					xuiC_WorkstationWindowGroup.WorkstationData.TileEntity.AddCraftComplete(this.startingEntityId, this.outputItemValue, this.recipe.GetName(), this.recipe.IsScrap ? this.recipe.ingredients[0].itemValue.ItemClass.GetItemName() : "", this.recipe.craftExpGain, this.recipe.count);
				}
			}
			if (this.recipe.GetName().Equals("meleeToolRepairT0StoneAxe"))
			{
				IAchievementManager achievementManager = PlatformManager.NativePlatform.AchievementManager;
				if (achievementManager != null)
				{
					achievementManager.SetAchievementStat(EnumAchievementDataStat.StoneAxeCrafted, 1);
				}
			}
			else if (this.recipe.GetName().Equals("frameShapes:VariantHelper"))
			{
				IAchievementManager achievementManager2 = PlatformManager.NativePlatform.AchievementManager;
				if (achievementManager2 != null)
				{
					achievementManager2.SetAchievementStat(EnumAchievementDataStat.WoodFrameCrafted, 1);
				}
			}
		}
		else if (this.amountToRepair > 0)
		{
			ItemValue itemValue = this.originalItem.Clone();
			ItemAction.HandleDegradation(itemValue, this.xui.playerUI.entityPlayer);
			itemValue.UseTimes -= (float)this.amountToRepair;
			ItemClass itemClass2 = itemValue.ItemClass;
			if (itemValue.UseTimes < 0f)
			{
				itemValue.UseTimes = 0f;
			}
			this.outputItemValue = itemValue.Clone();
			QuestEventManager.Current.RepairedItem(this.outputItemValue);
			this.amountToRepair = 0;
		}
		if (this.outputItemValue != null)
		{
			GameSparksCollector.IncrementCounter(GameSparksCollector.GSDataKey.CraftedItems, this.outputItemValue.ItemClass.Name, this.recipe.count, true, GameSparksCollector.GSDataCollection.SessionUpdates);
		}
		else
		{
			this.outputItemValue = this.originalItem;
		}
		XUiC_WorkstationOutputGrid childByType = this.windowGroup.Controller.GetChildByType<XUiC_WorkstationOutputGrid>();
		if (childByType != null && (this.originalItem == null || this.originalItem.IsEmpty()))
		{
			ItemStack itemStack = new ItemStack(this.outputItemValue, this.recipe.count);
			ItemStack[] slots = childByType.GetSlots();
			bool flag = false;
			for (int i = 0; i < slots.Length; i++)
			{
				if (slots[i].CanStackWith(itemStack, false))
				{
					slots[i].count += this.recipe.count;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				for (int j = 0; j < slots.Length; j++)
				{
					if (slots[j].IsEmpty())
					{
						slots[j] = itemStack;
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				childByType.SetSlots(slots);
				childByType.UpdateData(slots);
				childByType.IsDirty = true;
				if (this.recipe.IsScrap)
				{
					QuestEventManager.Current.ScrappedItem(this.recipe.ingredients[0]);
					entityPlayer.equipment.UnlockCosmeticItem(this.recipe.ingredients[0].itemValue.ItemClass);
				}
				else
				{
					QuestEventManager.Current.CraftedItem(itemStack);
				}
				if (this.playSound)
				{
					if (this.recipe.craftingArea != null)
					{
						WorkstationData workstationData = CraftingManager.GetWorkstationData(this.recipe.craftingArea);
						if (workstationData != null)
						{
							Manager.PlayInsidePlayerHead(workstationData.CraftCompleteSound, -1, 0f, false, false);
						}
					}
					else
					{
						Manager.PlayInsidePlayerHead("craft_complete_item", -1, 0f, false, false);
					}
				}
			}
			else if (!this.AddItemToInventory())
			{
				this.isInventoryFull = true;
				string text = "No room in workstation output, crafting has been halted until space is cleared.";
				if (Localization.Exists("wrnWorkstationOutputFull", false))
				{
					text = Localization.Get("wrnWorkstationOutputFull", false, null);
				}
				GameManager.ShowTooltip(entityPlayer, text, false, false, 0f);
				Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
				return false;
			}
		}
		else
		{
			if (!this.xui.DragAndDropWindow.CurrentStack.IsEmpty() && this.xui.DragAndDropWindow.CurrentStack.itemValue.ItemClass is ItemClassQuest)
			{
				return false;
			}
			ItemStack itemStack2 = new ItemStack(this.outputItemValue, this.recipe.count);
			if (!this.xui.PlayerInventory.AddItemNoPartial(itemStack2, false))
			{
				if (itemStack2.count != this.recipe.count)
				{
					this.xui.PlayerInventory.DropItem(itemStack2);
					QuestEventManager.Current.CraftedItem(itemStack2);
					return true;
				}
				this.isInventoryFull = true;
				string text2 = "No room in inventory, crafting has been halted until space is cleared.";
				if (Localization.Exists("wrnInventoryFull", false))
				{
					text2 = Localization.Get("wrnInventoryFull", false, null);
				}
				GameManager.ShowTooltip(entityPlayer, text2, false, false, 0f);
				Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
				return false;
			}
			else
			{
				if (this.originalItem != null && !this.originalItem.IsEmpty())
				{
					if (this.recipe.ingredients.Count > 0 && this.recipe.IsScrap)
					{
						QuestEventManager.Current.ScrappedItem(this.recipe.ingredients[0]);
						entityPlayer.equipment.UnlockCosmeticItem(this.recipe.ingredients[0].itemValue.ItemClass);
					}
				}
				else
				{
					itemStack2.count = this.recipe.count - itemStack2.count;
					if (this.recipe.IsScrap)
					{
						QuestEventManager.Current.ScrappedItem(this.recipe.ingredients[0]);
						entityPlayer.equipment.UnlockCosmeticItem(this.recipe.ingredients[0].itemValue.ItemClass);
					}
					else
					{
						QuestEventManager.Current.CraftedItem(itemStack2);
					}
				}
				if (this.playSound)
				{
					Manager.PlayInsidePlayerHead("craft_complete_item", -1, 0f, false, false);
				}
			}
		}
		if (!this.isInventoryFull)
		{
			this.originalItem = ItemValue.None;
		}
		return true;
	}

	// Token: 0x060081F4 RID: 33268 RVA: 0x00315B38 File Offset: 0x00313D38
	[PublicizedFrom(EAccessModifier.Private)]
	public bool AddItemToInventory()
	{
		ItemStack itemStack = new ItemStack(this.outputItemValue, this.recipe.count);
		if (!this.xui.PlayerInventory.AddItemNoPartial(itemStack, false))
		{
			this.updateRecipeData();
			return false;
		}
		QuestEventManager.Current.CraftedItem(new ItemStack(this.outputItemValue, this.recipe.count));
		this.isInventoryFull = false;
		if (this.playSound)
		{
			if (this.recipe.craftingArea != null)
			{
				WorkstationData workstationData = CraftingManager.GetWorkstationData(this.recipe.craftingArea);
				if (workstationData != null)
				{
					Manager.PlayInsidePlayerHead(workstationData.CraftCompleteSound, -1, 0f, false, false);
				}
			}
			else
			{
				Manager.PlayInsidePlayerHead("craft_complete_item", -1, 0f, false, false);
			}
		}
		if (this.recipeCount <= 0)
		{
			this.isCrafting = false;
			if (this.recipe != null || this.craftingTimeLeft != 0f)
			{
				this.ClearRecipe();
			}
		}
		return true;
	}

	// Token: 0x060081F5 RID: 33269 RVA: 0x00315C1C File Offset: 0x00313E1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void giveExp(ItemValue _iv, ItemClass _ic)
	{
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		int num = (int)this.xui.playerUI.entityPlayer.Buffs.GetCustomVar("_craftCount_" + this.recipe.GetName());
		this.xui.playerUI.entityPlayer.Buffs.SetCustomVar("_craftCount_" + this.recipe.GetName(), (float)(num + 1), true, CVarOperation.set, false);
		this.xui.playerUI.entityPlayer.Progression.AddLevelExp(this.recipe.craftExpGain / (num + 1), "_xpFromCrafting", Progression.XPTypes.Crafting, true, true, -1, null);
		entityPlayer.totalItemsCrafted += 1U;
		XUiC_RecipeStack.itemCraftedAchievementUpdate();
	}

	// Token: 0x060081F6 RID: 33270 RVA: 0x00315CE6 File Offset: 0x00313EE6
	public bool HasRecipe()
	{
		return this.recipe != null;
	}

	// Token: 0x060081F7 RID: 33271 RVA: 0x00315CF1 File Offset: 0x00313EF1
	public Recipe GetRecipe()
	{
		return this.recipe;
	}

	// Token: 0x060081F8 RID: 33272 RVA: 0x00315CF9 File Offset: 0x00313EF9
	public void ClearRecipe()
	{
		this.SetRecipe(null, 0, 0f, true, -1, -1, -1f);
	}

	// Token: 0x060081F9 RID: 33273 RVA: 0x00315D11 File Offset: 0x00313F11
	public int GetRecipeCount()
	{
		return this.recipeCount;
	}

	// Token: 0x060081FA RID: 33274 RVA: 0x00315D19 File Offset: 0x00313F19
	public float GetRecipeCraftingTimeLeft()
	{
		return this.craftingTimeLeft;
	}

	// Token: 0x060081FB RID: 33275 RVA: 0x00315D21 File Offset: 0x00313F21
	public float GetTotalRecipeCraftingTimeLeft()
	{
		return this.totalCraftTimeLeft;
	}

	// Token: 0x060081FC RID: 33276 RVA: 0x00315D29 File Offset: 0x00313F29
	public float GetOneItemCraftTime()
	{
		return this.oneItemCraftTime;
	}

	// Token: 0x060081FD RID: 33277 RVA: 0x00315D34 File Offset: 0x00313F34
	public bool SetRepairRecipe(float _repairTimeLeft, ItemValue _itemToRepair, int _amountToRepair)
	{
		if (this.isCrafting || (this.originalItem != null && this.originalItem.type != 0))
		{
			return false;
		}
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		this.recipeCount = 1;
		this.craftingTimeLeft = _repairTimeLeft;
		this.originalItem = _itemToRepair.Clone();
		this.amountToRepair = _amountToRepair;
		this.totalCraftTimeLeft = _repairTimeLeft;
		this.oneItemCraftTime = _repairTimeLeft;
		if (this.lockIcon != null && _itemToRepair.type != 0)
		{
			((XUiV_Sprite)this.lockIcon.ViewComponent).SpriteName = "ui_game_symbol_wrench";
		}
		this.outputQuality = (int)this.originalItem.Quality;
		this.StartingEntityId = entityPlayer.entityId;
		this.recipe = new Recipe();
		this.recipe.craftingTime = _repairTimeLeft;
		this.recipe.count = 1;
		this.recipe.itemValueType = this.originalItem.type;
		this.recipe.craftExpGain = Mathf.Clamp(this.amountToRepair, 0, 200);
		ItemClass itemClass = this.originalItem.ItemClass;
		if (itemClass.RepairTools != null && itemClass.RepairTools.Length > 0)
		{
			ItemClass itemClass2 = ItemClass.GetItemClass(itemClass.RepairTools[0].Value, false);
			if (itemClass2 != null)
			{
				int num = Mathf.CeilToInt((float)_amountToRepair / (float)itemClass2.RepairAmount.Value);
				this.recipe.ingredients.Add(new ItemStack(ItemClass.GetItem(itemClass.RepairTools[0].Value, false), num));
			}
		}
		this.updateRecipeData();
		return true;
	}

	// Token: 0x060081FE RID: 33278 RVA: 0x00315EC4 File Offset: 0x003140C4
	public bool SetRecipe(Recipe _recipe, int _count = 1, float craftTime = -1f, bool recipeModification = false, int startingEntityId = -1, int _outputQuality = -1, float _oneItemCraftTime = -1f)
	{
		if ((this.isCrafting || (this.recipe != null && _recipe != null)) && !recipeModification)
		{
			return false;
		}
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (startingEntityId == -1)
		{
			startingEntityId = entityPlayer.entityId;
		}
		this.StartingEntityId = startingEntityId;
		this.recipe = _recipe;
		this.recipeCount = _count;
		this.craftingTimeLeft = ((craftTime == -1f) ? ((_recipe != null) ? _recipe.craftingTime : 0f) : craftTime);
		if (this.originalItem != null && !this.originalItem.IsEmpty())
		{
			this.originalItem = ItemValue.None;
		}
		this.amountToRepair = 0;
		this.oneItemCraftTime = ((_oneItemCraftTime == -1f) ? ((_recipe != null) ? _recipe.craftingTime : 0f) : _oneItemCraftTime);
		this.totalCraftTimeLeft = this.oneItemCraftTime * ((float)_count - 1f) + this.craftingTimeLeft;
		if (this.lockIcon != null && this.recipe != null)
		{
			WorkstationData workstationData = CraftingManager.GetWorkstationData(this.recipe.craftingArea);
			if (workstationData != null)
			{
				((XUiV_Sprite)this.lockIcon.ViewComponent).SpriteName = workstationData.CraftIcon;
			}
		}
		if (_outputQuality == -1)
		{
			if (this.recipe != null)
			{
				this.outputQuality = this.recipe.craftingTier;
			}
			else
			{
				this.outputQuality = 1;
			}
		}
		else
		{
			this.outputQuality = _outputQuality;
		}
		this.ClearDisplayFromLastRecipe();
		this.updateRecipeData();
		return true;
	}

	// Token: 0x060081FF RID: 33279 RVA: 0x00316024 File Offset: 0x00314224
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearDisplayFromLastRecipe()
	{
		if (this.timer != null)
		{
			((XUiV_Label)this.timer.ViewComponent).SetTextImmediately("");
			this.timer.ViewComponent.IsVisible = true;
		}
		if (this.count != null)
		{
			this.count.ViewComponent.IsVisible = true;
			((XUiV_Label)this.count.ViewComponent).SetTextImmediately("");
		}
		if (this.cancel != null)
		{
			Color color = ((XUiV_Sprite)this.cancel.ViewComponent).Color;
			((XUiV_Sprite)this.cancel.ViewComponent).SetColorImmediately(new Color(color.r, color.g, color.b, 0f));
		}
	}

	// Token: 0x06008200 RID: 33280 RVA: 0x003160E8 File Offset: 0x003142E8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateRecipeData()
	{
		if (this.recipe == null && (this.originalItem == null || this.originalItem.type == 0))
		{
			if (this.lockIcon != null)
			{
				this.lockIcon.ViewComponent.IsVisible = false;
			}
			if (this.overlay != null)
			{
				this.overlay.ViewComponent.IsVisible = false;
			}
			if (this.itemIcon != null)
			{
				this.itemIcon.ViewComponent.IsVisible = false;
			}
			if (this.timer != null)
			{
				this.timer.ViewComponent.IsVisible = false;
			}
			if (this.count != null)
			{
				this.count.ViewComponent.IsVisible = false;
			}
			if (this.cancel != null)
			{
				this.cancel.ViewComponent.IsVisible = false;
				return;
			}
		}
		else
		{
			if (this.lockIcon != null)
			{
				this.lockIcon.ViewComponent.IsVisible = true;
			}
			if (this.overlay != null)
			{
				this.overlay.ViewComponent.IsVisible = true;
			}
			if (this.itemIcon != null)
			{
				ItemClass itemClass = ((this.recipe != null) ? ItemClass.GetForId(this.recipe.itemValueType) : this.originalItem.ItemClass);
				if (itemClass != null)
				{
					((XUiV_Sprite)this.itemIcon.ViewComponent).SetSpriteImmediately(itemClass.GetIconName());
					this.itemIcon.ViewComponent.IsVisible = true;
					((XUiV_Sprite)this.itemIcon.ViewComponent).Color = itemClass.GetIconTint(null);
				}
			}
			if (this.timer != null)
			{
				((XUiV_Label)this.timer.ViewComponent).SetTextImmediately(this.craftingTimeToString(this.totalCraftTimeLeft + 0.5f));
				this.timer.ViewComponent.IsVisible = true;
			}
			if (this.count != null)
			{
				this.count.ViewComponent.IsVisible = true;
				((XUiV_Label)this.count.ViewComponent).SetTextImmediately((this.recipeCount * this.recipe.count).ToString());
			}
			if (this.cancel != null)
			{
				Color color = ((XUiV_Sprite)this.cancel.ViewComponent).Color;
				if (this.isOver && this.background.ViewComponent.UiTransformIsHovered)
				{
					((XUiV_Sprite)this.cancel.ViewComponent).Color = new Color(color.r, color.g, color.b, 0.75f);
				}
				else
				{
					((XUiV_Sprite)this.cancel.ViewComponent).Color = new Color(color.r, color.g, color.b, 0f);
				}
				this.cancel.ViewComponent.IsVisible = true;
			}
		}
	}

	// Token: 0x06008201 RID: 33281 RVA: 0x00316398 File Offset: 0x00314598
	public override void OnOpen()
	{
		base.OnOpen();
		this.isInventoryFull = false;
		if (this.cancel != null)
		{
			this.isOver = false;
			Color color = ((XUiV_Sprite)this.cancel.ViewComponent).Color;
			((XUiV_Sprite)this.cancel.ViewComponent).Color = new Color(color.r, color.g, color.b, 0f);
		}
		this.playSound = true;
	}

	// Token: 0x06008202 RID: 33282 RVA: 0x0031640F File Offset: 0x0031460F
	public override void OnClose()
	{
		base.OnClose();
		this.playSound = false;
	}

	// Token: 0x06008203 RID: 33283 RVA: 0x00316420 File Offset: 0x00314620
	[PublicizedFrom(EAccessModifier.Private)]
	public string craftingTimeToString(float time)
	{
		return string.Format("{0}:{1}", ((int)(time / 60f)).ToString("0").PadLeft(2, '0'), ((int)(time % 60f)).ToString("0").PadLeft(2, '0'));
	}

	// Token: 0x06008204 RID: 33284 RVA: 0x00316471 File Offset: 0x00314671
	public override void Cleanup()
	{
		base.Cleanup();
		this.stopAchievementUpdateCoroutine();
	}

	// Token: 0x06008205 RID: 33285 RVA: 0x0031647F File Offset: 0x0031467F
	public static void HandleCraftXPGained()
	{
		XUiC_RecipeStack.itemCraftedAchievementUpdate();
	}

	// Token: 0x06008206 RID: 33286 RVA: 0x00316486 File Offset: 0x00314686
	[PublicizedFrom(EAccessModifier.Private)]
	public static void itemCraftedAchievementUpdate()
	{
		XUiC_RecipeStack.itemsCraftedSinceLastAchievementUpdate++;
		XUiC_RecipeStack.lastItemCraftedTime = Time.unscaledTime;
		XUiC_RecipeStack.startAchievementUpdateCoroutine();
	}

	// Token: 0x06008207 RID: 33287 RVA: 0x003164A3 File Offset: 0x003146A3
	[PublicizedFrom(EAccessModifier.Private)]
	public static void startAchievementUpdateCoroutine()
	{
		if (XUiC_RecipeStack.sendCraftedItemsForAchievementsCoroutine == null)
		{
			XUiC_RecipeStack.sendCraftedItemsForAchievementsCoroutine = ThreadManager.StartCoroutine(XUiC_RecipeStack.sendCraftedItemsForAchievementsCo());
		}
	}

	// Token: 0x06008208 RID: 33288 RVA: 0x003164BB File Offset: 0x003146BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopAchievementUpdateCoroutine()
	{
		if (XUiC_RecipeStack.sendCraftedItemsForAchievementsCoroutine != null)
		{
			ThreadManager.StopCoroutine(XUiC_RecipeStack.sendCraftedItemsForAchievementsCoroutine);
			XUiC_RecipeStack.sendCraftedItemsForAchievementsCoroutine = null;
			XUiC_RecipeStack.doSendCraftingStatsForAchievements();
		}
	}

	// Token: 0x06008209 RID: 33289 RVA: 0x003164D9 File Offset: 0x003146D9
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator sendCraftedItemsForAchievementsCo()
	{
		if (XUiC_RecipeStack.sendCraftedItemsForAchievementsInterval == null)
		{
			XUiC_RecipeStack.sendCraftedItemsForAchievementsInterval = new WaitForSeconds(30f);
		}
		for (;;)
		{
			yield return XUiC_RecipeStack.sendCraftedItemsForAchievementsInterval;
			XUiC_RecipeStack.doSendCraftingStatsForAchievements();
		}
		yield break;
	}

	// Token: 0x0600820A RID: 33290 RVA: 0x003164E4 File Offset: 0x003146E4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void doSendCraftingStatsForAchievements()
	{
		if (XUiC_RecipeStack.itemsCraftedSinceLastAchievementUpdate == 0)
		{
			return;
		}
		if (XUiC_RecipeStack.itemsCraftedSinceLastAchievementUpdate >= 20 || Time.unscaledTime > XUiC_RecipeStack.lastItemCraftedTime + 15f || XUiC_RecipeStack.sendCraftedItemsForAchievementsCoroutine == null)
		{
			IAchievementManager achievementManager = PlatformManager.NativePlatform.AchievementManager;
			if (achievementManager != null)
			{
				achievementManager.SetAchievementStat(EnumAchievementDataStat.ItemsCrafted, XUiC_RecipeStack.itemsCraftedSinceLastAchievementUpdate);
			}
			XUiC_RecipeStack.itemsCraftedSinceLastAchievementUpdate = 0;
		}
	}

	// Token: 0x04006034 RID: 24628
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x04006035 RID: 24629
	[PublicizedFrom(EAccessModifier.Private)]
	public int recipeCount;

	// Token: 0x04006036 RID: 24630
	[PublicizedFrom(EAccessModifier.Private)]
	public int recipeTier;

	// Token: 0x04006037 RID: 24631
	[PublicizedFrom(EAccessModifier.Private)]
	public float craftingTimeLeft;

	// Token: 0x04006038 RID: 24632
	[PublicizedFrom(EAccessModifier.Private)]
	public float totalCraftTimeLeft;

	// Token: 0x04006039 RID: 24633
	[PublicizedFrom(EAccessModifier.Private)]
	public float oneItemCraftTime = -1f;

	// Token: 0x0400603A RID: 24634
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isCrafting;

	// Token: 0x0400603B RID: 24635
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInventoryFull;

	// Token: 0x0400603C RID: 24636
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue originalItem;

	// Token: 0x0400603D RID: 24637
	[PublicizedFrom(EAccessModifier.Private)]
	public int amountToRepair;

	// Token: 0x0400603E RID: 24638
	[PublicizedFrom(EAccessModifier.Private)]
	public int outputQuality;

	// Token: 0x0400603F RID: 24639
	[PublicizedFrom(EAccessModifier.Private)]
	public int startingEntityId = -1;

	// Token: 0x04006040 RID: 24640
	public XUiC_CraftingQueue Owner;

	// Token: 0x04006041 RID: 24641
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playSound;

	// Token: 0x04006042 RID: 24642
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController timer;

	// Token: 0x04006043 RID: 24643
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController count;

	// Token: 0x04006044 RID: 24644
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController itemIcon;

	// Token: 0x04006045 RID: 24645
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController lockIcon;

	// Token: 0x04006046 RID: 24646
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController overlay;

	// Token: 0x04006047 RID: 24647
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController background;

	// Token: 0x04006048 RID: 24648
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController cancel;

	// Token: 0x04006049 RID: 24649
	[PublicizedFrom(EAccessModifier.Private)]
	public string inventoryFullDropping;

	// Token: 0x0400604A RID: 24650
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x0400604B RID: 24651
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue outputItemValue;

	// Token: 0x0400604C RID: 24652
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine sendCraftedItemsForAchievementsCoroutine;

	// Token: 0x0400604D RID: 24653
	[PublicizedFrom(EAccessModifier.Private)]
	public static float lastItemCraftedTime;

	// Token: 0x0400604E RID: 24654
	[PublicizedFrom(EAccessModifier.Private)]
	public static int itemsCraftedSinceLastAchievementUpdate;

	// Token: 0x0400604F RID: 24655
	[PublicizedFrom(EAccessModifier.Private)]
	public static WaitForSeconds sendCraftedItemsForAchievementsInterval;
}
