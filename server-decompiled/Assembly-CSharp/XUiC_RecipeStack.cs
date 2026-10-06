using System;
using System.Collections;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200101E RID: 4126
[Preserve]
public class XUiC_RecipeStack : XUiController
{
	// Token: 0x17000F46 RID: 3910
	// (get) Token: 0x060081F0 RID: 33264 RVA: 0x00317BA4 File Offset: 0x00315DA4
	// (set) Token: 0x060081F1 RID: 33265 RVA: 0x00317BAC File Offset: 0x00315DAC
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

	// Token: 0x17000F47 RID: 3911
	// (get) Token: 0x060081F2 RID: 33266 RVA: 0x00317BB5 File Offset: 0x00315DB5
	// (set) Token: 0x060081F3 RID: 33267 RVA: 0x00317BBD File Offset: 0x00315DBD
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

	// Token: 0x17000F48 RID: 3912
	// (get) Token: 0x060081F4 RID: 33268 RVA: 0x00317BC6 File Offset: 0x00315DC6
	// (set) Token: 0x060081F5 RID: 33269 RVA: 0x00317BCE File Offset: 0x00315DCE
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

	// Token: 0x17000F49 RID: 3913
	// (get) Token: 0x060081F6 RID: 33270 RVA: 0x00317BD7 File Offset: 0x00315DD7
	// (set) Token: 0x060081F7 RID: 33271 RVA: 0x00317BDF File Offset: 0x00315DDF
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

	// Token: 0x17000F4A RID: 3914
	// (get) Token: 0x060081F8 RID: 33272 RVA: 0x00317BE8 File Offset: 0x00315DE8
	// (set) Token: 0x060081F9 RID: 33273 RVA: 0x00317BF0 File Offset: 0x00315DF0
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

	// Token: 0x17000F4B RID: 3915
	// (get) Token: 0x060081FA RID: 33274 RVA: 0x00317BF9 File Offset: 0x00315DF9
	// (set) Token: 0x060081FB RID: 33275 RVA: 0x00317C1E File Offset: 0x00315E1E
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

	// Token: 0x060081FC RID: 33276 RVA: 0x00317C40 File Offset: 0x00315E40
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

	// Token: 0x060081FD RID: 33277 RVA: 0x00317CE0 File Offset: 0x00315EE0
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

	// Token: 0x060081FE RID: 33278 RVA: 0x00317DB2 File Offset: 0x00315FB2
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnHover(XUiController _sender, bool _isOver)
	{
		this.isOver = _isOver;
	}

	// Token: 0x060081FF RID: 33279 RVA: 0x00317DBB File Offset: 0x00315FBB
	public void ForceCancel()
	{
		this.HandleOnPress(null, -1);
	}

	// Token: 0x06008200 RID: 33280 RVA: 0x00317DC8 File Offset: 0x00315FC8
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

	// Token: 0x06008201 RID: 33281 RVA: 0x003180E8 File Offset: 0x003162E8
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

	// Token: 0x06008202 RID: 33282 RVA: 0x00318500 File Offset: 0x00316700
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

	// Token: 0x06008203 RID: 33283 RVA: 0x00318B68 File Offset: 0x00316D68
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

	// Token: 0x06008204 RID: 33284 RVA: 0x00318C4C File Offset: 0x00316E4C
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

	// Token: 0x06008205 RID: 33285 RVA: 0x00318D16 File Offset: 0x00316F16
	public bool HasRecipe()
	{
		return this.recipe != null;
	}

	// Token: 0x06008206 RID: 33286 RVA: 0x00318D21 File Offset: 0x00316F21
	public Recipe GetRecipe()
	{
		return this.recipe;
	}

	// Token: 0x06008207 RID: 33287 RVA: 0x00318D29 File Offset: 0x00316F29
	public void ClearRecipe()
	{
		this.SetRecipe(null, 0, 0f, true, -1, -1, -1f);
	}

	// Token: 0x06008208 RID: 33288 RVA: 0x00318D41 File Offset: 0x00316F41
	public int GetRecipeCount()
	{
		return this.recipeCount;
	}

	// Token: 0x06008209 RID: 33289 RVA: 0x00318D49 File Offset: 0x00316F49
	public float GetRecipeCraftingTimeLeft()
	{
		return this.craftingTimeLeft;
	}

	// Token: 0x0600820A RID: 33290 RVA: 0x00318D51 File Offset: 0x00316F51
	public float GetTotalRecipeCraftingTimeLeft()
	{
		return this.totalCraftTimeLeft;
	}

	// Token: 0x0600820B RID: 33291 RVA: 0x00318D59 File Offset: 0x00316F59
	public float GetOneItemCraftTime()
	{
		return this.oneItemCraftTime;
	}

	// Token: 0x0600820C RID: 33292 RVA: 0x00318D64 File Offset: 0x00316F64
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

	// Token: 0x0600820D RID: 33293 RVA: 0x00318EF4 File Offset: 0x003170F4
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

	// Token: 0x0600820E RID: 33294 RVA: 0x00319054 File Offset: 0x00317254
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

	// Token: 0x0600820F RID: 33295 RVA: 0x00319118 File Offset: 0x00317318
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

	// Token: 0x06008210 RID: 33296 RVA: 0x003193C8 File Offset: 0x003175C8
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

	// Token: 0x06008211 RID: 33297 RVA: 0x0031943F File Offset: 0x0031763F
	public override void OnClose()
	{
		base.OnClose();
		this.playSound = false;
	}

	// Token: 0x06008212 RID: 33298 RVA: 0x00319450 File Offset: 0x00317650
	[PublicizedFrom(EAccessModifier.Private)]
	public string craftingTimeToString(float time)
	{
		return string.Format("{0}:{1}", ((int)(time / 60f)).ToString("0").PadLeft(2, '0'), ((int)(time % 60f)).ToString("0").PadLeft(2, '0'));
	}

	// Token: 0x06008213 RID: 33299 RVA: 0x003194A1 File Offset: 0x003176A1
	public override void Cleanup()
	{
		base.Cleanup();
		this.stopAchievementUpdateCoroutine();
	}

	// Token: 0x06008214 RID: 33300 RVA: 0x003194AF File Offset: 0x003176AF
	public static void HandleCraftXPGained()
	{
		XUiC_RecipeStack.itemCraftedAchievementUpdate();
	}

	// Token: 0x06008215 RID: 33301 RVA: 0x003194B6 File Offset: 0x003176B6
	[PublicizedFrom(EAccessModifier.Private)]
	public static void itemCraftedAchievementUpdate()
	{
		XUiC_RecipeStack.itemsCraftedSinceLastAchievementUpdate++;
		XUiC_RecipeStack.lastItemCraftedTime = Time.unscaledTime;
		XUiC_RecipeStack.startAchievementUpdateCoroutine();
	}

	// Token: 0x06008216 RID: 33302 RVA: 0x003194D3 File Offset: 0x003176D3
	[PublicizedFrom(EAccessModifier.Private)]
	public static void startAchievementUpdateCoroutine()
	{
		if (XUiC_RecipeStack.sendCraftedItemsForAchievementsCoroutine == null)
		{
			XUiC_RecipeStack.sendCraftedItemsForAchievementsCoroutine = ThreadManager.StartCoroutine(XUiC_RecipeStack.sendCraftedItemsForAchievementsCo());
		}
	}

	// Token: 0x06008217 RID: 33303 RVA: 0x003194EB File Offset: 0x003176EB
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

	// Token: 0x06008218 RID: 33304 RVA: 0x00319509 File Offset: 0x00317709
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

	// Token: 0x06008219 RID: 33305 RVA: 0x00319514 File Offset: 0x00317714
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

	// Token: 0x0400605D RID: 24669
	[PublicizedFrom(EAccessModifier.Private)]
	public Recipe recipe;

	// Token: 0x0400605E RID: 24670
	[PublicizedFrom(EAccessModifier.Private)]
	public int recipeCount;

	// Token: 0x0400605F RID: 24671
	[PublicizedFrom(EAccessModifier.Private)]
	public int recipeTier;

	// Token: 0x04006060 RID: 24672
	[PublicizedFrom(EAccessModifier.Private)]
	public float craftingTimeLeft;

	// Token: 0x04006061 RID: 24673
	[PublicizedFrom(EAccessModifier.Private)]
	public float totalCraftTimeLeft;

	// Token: 0x04006062 RID: 24674
	[PublicizedFrom(EAccessModifier.Private)]
	public float oneItemCraftTime = -1f;

	// Token: 0x04006063 RID: 24675
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isCrafting;

	// Token: 0x04006064 RID: 24676
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInventoryFull;

	// Token: 0x04006065 RID: 24677
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue originalItem;

	// Token: 0x04006066 RID: 24678
	[PublicizedFrom(EAccessModifier.Private)]
	public int amountToRepair;

	// Token: 0x04006067 RID: 24679
	[PublicizedFrom(EAccessModifier.Private)]
	public int outputQuality;

	// Token: 0x04006068 RID: 24680
	[PublicizedFrom(EAccessModifier.Private)]
	public int startingEntityId = -1;

	// Token: 0x04006069 RID: 24681
	public XUiC_CraftingQueue Owner;

	// Token: 0x0400606A RID: 24682
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playSound;

	// Token: 0x0400606B RID: 24683
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController timer;

	// Token: 0x0400606C RID: 24684
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController count;

	// Token: 0x0400606D RID: 24685
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController itemIcon;

	// Token: 0x0400606E RID: 24686
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController lockIcon;

	// Token: 0x0400606F RID: 24687
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController overlay;

	// Token: 0x04006070 RID: 24688
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController background;

	// Token: 0x04006071 RID: 24689
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController cancel;

	// Token: 0x04006072 RID: 24690
	[PublicizedFrom(EAccessModifier.Private)]
	public string inventoryFullDropping;

	// Token: 0x04006073 RID: 24691
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOver;

	// Token: 0x04006074 RID: 24692
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemValue outputItemValue;

	// Token: 0x04006075 RID: 24693
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine sendCraftedItemsForAchievementsCoroutine;

	// Token: 0x04006076 RID: 24694
	[PublicizedFrom(EAccessModifier.Private)]
	public static float lastItemCraftedTime;

	// Token: 0x04006077 RID: 24695
	[PublicizedFrom(EAccessModifier.Private)]
	public static int itemsCraftedSinceLastAchievementUpdate;

	// Token: 0x04006078 RID: 24696
	[PublicizedFrom(EAccessModifier.Private)]
	public static WaitForSeconds sendCraftedItemsForAchievementsInterval;
}
