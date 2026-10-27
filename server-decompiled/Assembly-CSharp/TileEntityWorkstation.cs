using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C9A RID: 3226
public class TileEntityWorkstation : TileEntity
{
	// Token: 0x14000088 RID: 136
	// (add) Token: 0x0600622F RID: 25135 RVA: 0x0026EB18 File Offset: 0x0026CD18
	// (remove) Token: 0x06006230 RID: 25136 RVA: 0x0026EB50 File Offset: 0x0026CD50
	public event XUiEvent_InputStackChanged InputChanged;

	// Token: 0x14000089 RID: 137
	// (add) Token: 0x06006231 RID: 25137 RVA: 0x0026EB88 File Offset: 0x0026CD88
	// (remove) Token: 0x06006232 RID: 25138 RVA: 0x0026EBC0 File Offset: 0x0026CDC0
	public event XUiEvent_FuelStackChanged FuelChanged;

	// Token: 0x17000A29 RID: 2601
	// (get) Token: 0x06006233 RID: 25139 RVA: 0x0026EBF5 File Offset: 0x0026CDF5
	public string[] MaterialNames
	{
		get
		{
			return this.materialNames;
		}
	}

	// Token: 0x06006234 RID: 25140 RVA: 0x0026EC00 File Offset: 0x0026CE00
	public bool AcceptsMaterial(MaterialBlock material)
	{
		foreach (string text in this.materialNames)
		{
			if (material.ForgeCategory != null && material.ForgeCategory.EqualsCaseInsensitive(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000A2A RID: 2602
	// (get) Token: 0x06006235 RID: 25141 RVA: 0x0026EC3F File Offset: 0x0026CE3F
	// (set) Token: 0x06006236 RID: 25142 RVA: 0x0026EC47 File Offset: 0x0026CE47
	public ItemStack[] Tools
	{
		get
		{
			return this.tools;
		}
		set
		{
			if (!this.IsToolsSame(value))
			{
				this.tools = ItemStack.Clone(value);
				this.visibleChanged = true;
				this.UpdateVisible();
				this.setModified();
			}
		}
	}

	// Token: 0x06006237 RID: 25143 RVA: 0x0026EC74 File Offset: 0x0026CE74
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsToolsSame(ItemStack[] _tools)
	{
		if (_tools == null || _tools.Length != this.tools.Length)
		{
			return false;
		}
		for (int i = 0; i < _tools.Length; i++)
		{
			if (!_tools[i].Equals(this.tools[i]))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x17000A2B RID: 2603
	// (get) Token: 0x06006238 RID: 25144 RVA: 0x0026ECB6 File Offset: 0x0026CEB6
	// (set) Token: 0x06006239 RID: 25145 RVA: 0x0026ECBE File Offset: 0x0026CEBE
	public ItemStack[] Fuel
	{
		get
		{
			return this.fuel;
		}
		set
		{
			this.fuel = ItemStack.Clone(value);
			this.setModified();
		}
	}

	// Token: 0x17000A2C RID: 2604
	// (get) Token: 0x0600623A RID: 25146 RVA: 0x0026ECD2 File Offset: 0x0026CED2
	// (set) Token: 0x0600623B RID: 25147 RVA: 0x0026ECDA File Offset: 0x0026CEDA
	public ItemStack[] Input
	{
		get
		{
			return this.input;
		}
		set
		{
			this.input = ItemStack.Clone(value);
			this.setModified();
		}
	}

	// Token: 0x17000A2D RID: 2605
	// (get) Token: 0x0600623C RID: 25148 RVA: 0x0026ECEE File Offset: 0x0026CEEE
	// (set) Token: 0x0600623D RID: 25149 RVA: 0x0026ECF6 File Offset: 0x0026CEF6
	public ItemStack[] Output
	{
		get
		{
			return this.output;
		}
		set
		{
			this.output = ItemStack.Clone(value);
			this.setModified();
		}
	}

	// Token: 0x17000A2E RID: 2606
	// (get) Token: 0x0600623E RID: 25150 RVA: 0x0026ED0A File Offset: 0x0026CF0A
	// (set) Token: 0x0600623F RID: 25151 RVA: 0x0026ED12 File Offset: 0x0026CF12
	public RecipeQueueItem[] Queue
	{
		get
		{
			return this.queue;
		}
		set
		{
			this.queue = value;
			this.setModified();
		}
	}

	// Token: 0x17000A2F RID: 2607
	// (get) Token: 0x06006240 RID: 25152 RVA: 0x0026ED21 File Offset: 0x0026CF21
	// (set) Token: 0x06006241 RID: 25153 RVA: 0x0026ED29 File Offset: 0x0026CF29
	public bool IsBurning
	{
		get
		{
			return this.isBurning;
		}
		set
		{
			this.isBurning = value;
			this.setModified();
		}
	}

	// Token: 0x17000A30 RID: 2608
	// (get) Token: 0x06006242 RID: 25154 RVA: 0x0026ED38 File Offset: 0x0026CF38
	public bool IsCrafting
	{
		get
		{
			return this.hasRecipeInQueue() && (!this.isModuleUsed[3] || this.isBurning);
		}
	}

	// Token: 0x17000A31 RID: 2609
	// (get) Token: 0x06006243 RID: 25155 RVA: 0x0026ED56 File Offset: 0x0026CF56
	// (set) Token: 0x06006244 RID: 25156 RVA: 0x0026ED5E File Offset: 0x0026CF5E
	public bool IsPlayerPlaced
	{
		get
		{
			return this.isPlayerPlaced;
		}
		set
		{
			this.isPlayerPlaced = value;
			this.setModified();
		}
	}

	// Token: 0x17000A32 RID: 2610
	// (get) Token: 0x06006245 RID: 25157 RVA: 0x0026ED6D File Offset: 0x0026CF6D
	public bool IsBesideWater
	{
		get
		{
			return this.isBesideWater;
		}
	}

	// Token: 0x17000A33 RID: 2611
	// (get) Token: 0x06006246 RID: 25158 RVA: 0x0026ED75 File Offset: 0x0026CF75
	public float BurnTimeLeft
	{
		get
		{
			return this.currentBurnTimeLeft;
		}
	}

	// Token: 0x17000A34 RID: 2612
	// (get) Token: 0x06006247 RID: 25159 RVA: 0x0026ED7D File Offset: 0x0026CF7D
	public float BurnTotalTimeLeft
	{
		get
		{
			return this.getTotalFuelSeconds() + this.currentBurnTimeLeft;
		}
	}

	// Token: 0x17000A35 RID: 2613
	// (get) Token: 0x06006248 RID: 25160 RVA: 0x00047076 File Offset: 0x00045276
	public int InputSlotCount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x06006249 RID: 25161 RVA: 0x0026ED8C File Offset: 0x0026CF8C
	public TileEntityWorkstation(Chunk _chunk)
		: base(_chunk)
	{
		this.fuel = ItemStack.CreateArray(3);
		this.tools = ItemStack.CreateArray(3);
		this.output = ItemStack.CreateArray(6);
		this.input = ItemStack.CreateArray(3);
		this.lastInput = ItemStack.CreateArray(3);
		this.queue = new RecipeQueueItem[4];
		this.materialNames = new string[0];
		this.isModuleUsed = new bool[5];
		this.currentMeltTimesLeft = new float[this.input.Length];
	}

	// Token: 0x0600624A RID: 25162 RVA: 0x0026EE13 File Offset: 0x0026D013
	public void ResetTickTime()
	{
		this.lastTickTime = GameTimer.Instance.ticks;
	}

	// Token: 0x0600624B RID: 25163 RVA: 0x0026EE28 File Offset: 0x0026D028
	public override void OnSetLocalChunkPosition()
	{
		if (base.localChunkPos == Vector3i.zero)
		{
			return;
		}
		Block block = this.chunk.GetBlock(World.toBlockXZ(base.localChunkPos.x), base.localChunkPos.y, World.toBlockXZ(base.localChunkPos.z)).Block;
		if (block.Properties.Contains("Workstation", "InputMaterials"))
		{
			string @string = block.Properties.GetString("Workstation", "InputMaterials");
			if (@string.Contains(","))
			{
				this.materialNames = @string.Replace(" ", "").Split(',', StringSplitOptions.None);
			}
			else
			{
				this.materialNames = new string[] { @string };
			}
			if (this.input.Length != 3 + this.materialNames.Length)
			{
				ItemStack[] array = new ItemStack[3 + this.materialNames.Length];
				for (int i = 0; i < this.input.Length; i++)
				{
					array[i] = this.input[i].Clone();
				}
				this.input = array;
				for (int j = 0; j < this.materialNames.Length; j++)
				{
					ItemClass itemClass = ItemClass.GetItemClass("unit_" + this.materialNames[j], false);
					if (itemClass != null)
					{
						int num = j + 3;
						this.input[num] = new ItemStack(new ItemValue(itemClass.Id, false), 0);
					}
				}
			}
		}
		if (block.Properties.Contains("Workstation", "Modules"))
		{
			string string2 = block.Properties.GetString("Workstation", "Modules");
			string[] array2;
			if (string2.Contains(","))
			{
				array2 = string2.Replace(" ", "").Split(',', StringSplitOptions.None);
			}
			else
			{
				array2 = new string[] { string2 };
			}
			for (int k = 0; k < array2.Length; k++)
			{
				TileEntityWorkstation.Module module = EnumUtils.Parse<TileEntityWorkstation.Module>(array2[k], true);
				this.isModuleUsed[(int)module] = true;
			}
			if (this.isModuleUsed[4])
			{
				this.isModuleUsed[1] = true;
			}
		}
	}

	// Token: 0x0600624C RID: 25164 RVA: 0x0026F04C File Offset: 0x0026D24C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateLightState(World world, BlockValue blockValue)
	{
		bool flag = this.CanOperate(GameTimer.Instance.ticks);
		if (!flag && blockValue.meta != 0)
		{
			blockValue.meta = 0;
			world.SetBlockRPC(base.ToWorldPos(), blockValue);
			return;
		}
		if (flag && blockValue.meta != 15)
		{
			blockValue.meta = 15;
			world.SetBlockRPC(base.ToWorldPos(), blockValue);
		}
	}

	// Token: 0x0600624D RID: 25165 RVA: 0x0026ED21 File Offset: 0x0026CF21
	public bool CanOperate(ulong _worldTimeInTicks)
	{
		return this.isBurning;
	}

	// Token: 0x0600624E RID: 25166 RVA: 0x0026F0BA File Offset: 0x0026D2BA
	public override bool IsActive(World world)
	{
		return this.IsBurning;
	}

	// Token: 0x0600624F RID: 25167 RVA: 0x0026F0C4 File Offset: 0x0026D2C4
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		bool flag = (!this.isModuleUsed[3] && this.hasRecipeInQueue()) || this.isBurning;
		float num = (GameTimer.Instance.ticks - this.lastTickTime) / 20f;
		float num2 = Mathf.Min(num, this.BurnTotalTimeLeft);
		float num3 = (this.isModuleUsed[3] ? num2 : num);
		this.isBesideWater = base.IsByWater(world, base.ToWorldPos());
		this.isBurning &= !this.isBesideWater;
		BlockValue block = world.GetBlock(base.ToWorldPos());
		this.UpdateLightState(world, block);
		if (this.isModuleUsed[3])
		{
			this.HandleFuel(world, num3);
		}
		else if (block.Block.HeatMapStrength > 0f && this.IsCrafting)
		{
			base.emitHeatMapEvent(world, EnumAIDirectorChunkEvent.Campfire);
		}
		this.HandleRecipeQueue(num3);
		this.HandleMaterialInput(num3);
		if (this.isModuleUsed[3])
		{
			this.isBurning &= this.BurnTotalTimeLeft > 0f;
		}
		this.lastTickTime = GameTimer.Instance.ticks;
		if ((!this.isModuleUsed[3] && this.hasRecipeInQueue()) || this.isBurning || flag)
		{
			this.setModified();
		}
		this.UpdateVisible();
	}

	// Token: 0x06006250 RID: 25168 RVA: 0x0026F211 File Offset: 0x0026D411
	public void SetVisibleChanged()
	{
		this.visibleChanged = true;
	}

	// Token: 0x06006251 RID: 25169 RVA: 0x0026F21C File Offset: 0x0026D41C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateVisible()
	{
		bool isCrafting = this.IsCrafting;
		if (isCrafting != this.visibleCrafting)
		{
			this.visibleCrafting = isCrafting;
			this.visibleChanged = true;
		}
		bool flag = (!this.isModuleUsed[3] && this.hasRecipeInQueue()) || this.isBurning;
		if (flag != this.visibleWorking)
		{
			this.visibleWorking = flag;
			this.visibleChanged = true;
		}
		if (this.visibleChanged)
		{
			this.visibleChanged = false;
			BlockWorkstation blockWorkstation = GameManager.Instance.World.GetBlock(base.ToWorldPos()).Block as BlockWorkstation;
			if (blockWorkstation != null)
			{
				blockWorkstation.UpdateVisible(this);
			}
		}
	}

	// Token: 0x06006252 RID: 25170 RVA: 0x0026F2B6 File Offset: 0x0026D4B6
	public float GetTimerForSlot(int inputSlot)
	{
		if (inputSlot >= this.currentMeltTimesLeft.Length)
		{
			return 0f;
		}
		return this.currentMeltTimesLeft[inputSlot];
	}

	// Token: 0x06006253 RID: 25171 RVA: 0x0026F2D4 File Offset: 0x0026D4D4
	public void ClearSlotTimersForInputs()
	{
		for (int i = 0; i < this.currentMeltTimesLeft.Length; i++)
		{
			this.currentMeltTimesLeft[i] = 0f;
		}
	}

	// Token: 0x06006254 RID: 25172 RVA: 0x0026F304 File Offset: 0x0026D504
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleMaterialInput(float timePassed)
	{
		if (XUiM_Recipes.DisableSmelter)
		{
			return;
		}
		if (this.isModuleUsed[4] && (this.isBurning || !this.isModuleUsed[3]))
		{
			for (int i = 0; i < this.input.Length - this.materialNames.Length; i++)
			{
				if (this.input[i].IsEmpty())
				{
					this.input[i].Clear();
					this.currentMeltTimesLeft[i] = -2.1474836E+09f;
					if (this.InputChanged != null)
					{
						this.InputChanged();
					}
				}
				else
				{
					ItemClass forId = ItemClass.GetForId(this.input[i].itemValue.type);
					if (forId != null)
					{
						if (this.currentMeltTimesLeft[i] >= 0f && this.input[i].count > 0)
						{
							if (this.lastInput[i].itemValue.type != this.input[i].itemValue.type)
							{
								this.currentMeltTimesLeft[i] = -2.1474836E+09f;
							}
							else
							{
								this.currentMeltTimesLeft[i] -= timePassed;
							}
						}
						if (this.currentMeltTimesLeft[i] == -2.1474836E+09f && this.input[i].count > 0)
						{
							for (int j = 0; j < this.materialNames.Length; j++)
							{
								if (forId.MadeOfMaterial.ForgeCategory != null && forId.MadeOfMaterial.ForgeCategory.EqualsCaseInsensitive(this.materialNames[j]))
								{
									ItemClass itemClass = ItemClass.GetItemClass("unit_" + this.materialNames[j], false);
									if (itemClass != null && itemClass.MadeOfMaterial.ForgeCategory != null)
									{
										float num = (float)forId.GetWeight() * ((forId.MeltTimePerUnit > 0f) ? forId.MeltTimePerUnit : 1f);
										if (this.isModuleUsed[0])
										{
											for (int k = 0; k < this.tools.Length; k++)
											{
												float num2 = 1f;
												this.tools[k].itemValue.ModifyValue(null, null, PassiveEffects.CraftingSmeltTime, ref num, ref num2, FastTags<TagGroup.Global>.Parse(forId.Name), true, false);
												num *= num2;
											}
										}
										if (num > 0f && this.currentMeltTimesLeft[i] == -2.1474836E+09f)
										{
											this.currentMeltTimesLeft[i] = num;
										}
										else
										{
											this.currentMeltTimesLeft[i] += num;
										}
									}
								}
							}
							this.lastInput[i] = this.input[i].Clone();
						}
						if (this.currentMeltTimesLeft[i] != -2.1474836E+09f)
						{
							int num3 = 0;
							int num4 = 3;
							while ((num4 < this.input.Length) & (num3 < this.materialNames.Length))
							{
								if (forId.MadeOfMaterial.ForgeCategory != null && forId.MadeOfMaterial.ForgeCategory.EqualsCaseInsensitive(this.materialNames[num3]))
								{
									ItemClass itemClass2 = ItemClass.GetItemClass("unit_" + this.materialNames[num3], false);
									if (itemClass2 != null && itemClass2.MadeOfMaterial.ForgeCategory != null)
									{
										if (this.input[num4].itemValue.type == 0)
										{
											this.input[num4] = new ItemStack(new ItemValue(itemClass2.Id, false), this.input[num4].count);
										}
										bool flag = false;
										while (this.currentMeltTimesLeft[i] < 0f && this.currentMeltTimesLeft[i] != -2.1474836E+09f)
										{
											if (this.input[i].count <= 0)
											{
												this.input[i].Clear();
												this.currentMeltTimesLeft[i] = 0f;
												flag = true;
												if (this.InputChanged != null)
												{
													this.InputChanged();
													break;
												}
												break;
											}
											else
											{
												if (this.input[num4].count + forId.GetWeight() > itemClass2.MaxCount)
												{
													this.currentMeltTimesLeft[i] = -2.1474836E+09f;
													break;
												}
												this.input[num4].count += forId.GetWeight();
												ItemStack itemStack = this.input[i];
												int count = itemStack.count;
												itemStack.count = count - 1;
												float num5 = (float)forId.GetWeight() * ((forId.MeltTimePerUnit > 0f) ? forId.MeltTimePerUnit : 1f);
												if (this.isModuleUsed[0])
												{
													for (int l = 0; l < this.tools.Length; l++)
													{
														if (!this.tools[l].IsEmpty())
														{
															float num6 = 1f;
															this.tools[l].itemValue.ModifyValue(null, null, PassiveEffects.CraftingSmeltTime, ref num5, ref num6, FastTags<TagGroup.Global>.Parse(itemClass2.Name), true, false);
															num5 *= num6;
														}
													}
												}
												this.currentMeltTimesLeft[i] += num5;
												if (this.input[i].count <= 0)
												{
													this.input[i].Clear();
													this.currentMeltTimesLeft[i] = -2.1474836E+09f;
													flag = true;
													if (this.InputChanged != null)
													{
														this.InputChanged();
														break;
													}
													break;
												}
												else
												{
													if (this.InputChanged != null)
													{
														this.InputChanged();
													}
													flag = true;
												}
											}
										}
										if (flag && this.currentMeltTimesLeft[i] < 0f && this.currentMeltTimesLeft[i] != -2.1474836E+09f)
										{
											this.currentMeltTimesLeft[i] = -2.1474836E+09f;
											break;
										}
										break;
									}
								}
								num3++;
								num4++;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06006255 RID: 25173 RVA: 0x0026F868 File Offset: 0x0026DA68
	[PublicizedFrom(EAccessModifier.Private)]
	public void cycleRecipeQueue()
	{
		RecipeQueueItem recipeQueueItem = null;
		if (this.queue[this.queue.Length - 1] != null && this.queue[this.queue.Length - 1].Multiplier > 0)
		{
			return;
		}
		for (int i = 0; i < this.queue.Length; i++)
		{
			recipeQueueItem = this.queue[this.queue.Length - 1];
			if (recipeQueueItem != null && recipeQueueItem.Multiplier != 0)
			{
				break;
			}
			for (int j = this.queue.Length - 1; j >= 0; j--)
			{
				RecipeQueueItem recipeQueueItem2 = this.queue[j];
				if (j != 0)
				{
					RecipeQueueItem recipeQueueItem3 = this.queue[j - 1];
					if (recipeQueueItem3.Multiplier < 0)
					{
						recipeQueueItem3.Multiplier = 0;
					}
					recipeQueueItem2.Recipe = recipeQueueItem3.Recipe;
					recipeQueueItem2.Multiplier = recipeQueueItem3.Multiplier;
					recipeQueueItem2.CraftingTimeLeft = recipeQueueItem3.CraftingTimeLeft;
					recipeQueueItem2.IsCrafting = recipeQueueItem3.IsCrafting;
					recipeQueueItem2.Quality = recipeQueueItem3.Quality;
					recipeQueueItem2.OneItemCraftTime = recipeQueueItem3.OneItemCraftTime;
					recipeQueueItem2.StartingEntityId = recipeQueueItem3.StartingEntityId;
					this.queue[j] = recipeQueueItem2;
					recipeQueueItem3 = new RecipeQueueItem();
					recipeQueueItem3.Recipe = null;
					recipeQueueItem3.Multiplier = 0;
					recipeQueueItem3.CraftingTimeLeft = 0f;
					recipeQueueItem3.OneItemCraftTime = 0f;
					recipeQueueItem3.IsCrafting = false;
					recipeQueueItem3.Quality = 0;
					recipeQueueItem3.StartingEntityId = -1;
					this.queue[j - 1] = recipeQueueItem3;
				}
			}
		}
		if (recipeQueueItem != null && recipeQueueItem.Recipe != null && !recipeQueueItem.IsCrafting && recipeQueueItem.Multiplier != 0)
		{
			recipeQueueItem.IsCrafting = true;
		}
	}

	// Token: 0x06006256 RID: 25174 RVA: 0x0026FA00 File Offset: 0x0026DC00
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleRecipeQueue(float _timePassed)
	{
		if (this.bUserAccessing)
		{
			return;
		}
		if (this.queue.Length == 0)
		{
			return;
		}
		if (this.isModuleUsed[3] && !this.isBurning)
		{
			return;
		}
		RecipeQueueItem recipeQueueItem = this.queue[this.queue.Length - 1];
		if (recipeQueueItem == null)
		{
			return;
		}
		if (recipeQueueItem.CraftingTimeLeft >= 0f)
		{
			recipeQueueItem.CraftingTimeLeft -= _timePassed;
		}
		while (recipeQueueItem.CraftingTimeLeft < 0f && this.hasRecipeInQueue())
		{
			if (recipeQueueItem.Multiplier > 0)
			{
				ItemValue itemValue = new ItemValue(recipeQueueItem.Recipe.itemValueType, false);
				if (ItemClass.list[recipeQueueItem.Recipe.itemValueType] != null && ItemClass.list[recipeQueueItem.Recipe.itemValueType].HasQuality)
				{
					itemValue = new ItemValue(recipeQueueItem.Recipe.itemValueType, (int)recipeQueueItem.Quality, (int)recipeQueueItem.Quality, false, null, 1f);
				}
				if (ItemStack.AddToItemStackArray(this.output, new ItemStack(itemValue, recipeQueueItem.Recipe.count), -1) == -1)
				{
					return;
				}
				this.AddCraftComplete(recipeQueueItem.StartingEntityId, itemValue, recipeQueueItem.Recipe.GetName(), recipeQueueItem.Recipe.IsScrap ? recipeQueueItem.Recipe.ingredients[0].itemValue.ItemClass.GetItemName() : "", recipeQueueItem.Recipe.craftExpGain, recipeQueueItem.Recipe.count);
				GameSparksCollector.IncrementCounter(GameSparksCollector.GSDataKey.CraftedItems, itemValue.ItemClass.Name, recipeQueueItem.Recipe.count, true, GameSparksCollector.GSDataCollection.SessionUpdates);
				RecipeQueueItem recipeQueueItem2 = recipeQueueItem;
				recipeQueueItem2.Multiplier -= 1;
				recipeQueueItem.CraftingTimeLeft += recipeQueueItem.OneItemCraftTime;
			}
			if (recipeQueueItem.Multiplier <= 0)
			{
				float craftingTimeLeft = recipeQueueItem.CraftingTimeLeft;
				this.cycleRecipeQueue();
				recipeQueueItem = this.queue[this.queue.Length - 1];
				recipeQueueItem.CraftingTimeLeft += ((craftingTimeLeft < 0f) ? craftingTimeLeft : 0f);
			}
		}
	}

	// Token: 0x06006257 RID: 25175 RVA: 0x0026FBFC File Offset: 0x0026DDFC
	[PublicizedFrom(EAccessModifier.Private)]
	public bool HandleFuel(World _world, float _timePassed)
	{
		if (!this.isBurning)
		{
			return false;
		}
		base.emitHeatMapEvent(_world, EnumAIDirectorChunkEvent.Campfire);
		bool flag = false;
		if (this.currentBurnTimeLeft > 0f || (this.currentBurnTimeLeft == 0f && this.getTotalFuelSeconds() > 0f))
		{
			this.currentBurnTimeLeft -= _timePassed;
			this.currentBurnTimeLeft = (float)Mathf.FloorToInt(this.currentBurnTimeLeft * 100f) / 100f;
			flag = true;
		}
		while (this.currentBurnTimeLeft < 0f && this.getTotalFuelSeconds() > 0f)
		{
			if (this.fuel[0].count > 0)
			{
				ItemStack itemStack = this.fuel[0];
				int count = itemStack.count;
				itemStack.count = count - 1;
				this.currentBurnTimeLeft += this.GetFuelTime(this.fuel[0]);
				flag = true;
				if (this.FuelChanged != null)
				{
					this.FuelChanged();
				}
			}
			else
			{
				this.cycleFuelStacks();
				flag = true;
			}
		}
		if (this.getTotalFuelSeconds() == 0f && this.currentBurnTimeLeft < 0f)
		{
			this.currentBurnTimeLeft = 0f;
			flag = true;
		}
		return flag;
	}

	// Token: 0x06006258 RID: 25176 RVA: 0x0026FD20 File Offset: 0x0026DF20
	[PublicizedFrom(EAccessModifier.Private)]
	public float getFuelTime(ItemStack _itemStack)
	{
		if (_itemStack == null)
		{
			return 0f;
		}
		ItemClass forId = ItemClass.GetForId(_itemStack.itemValue.type);
		float num = 0f;
		if (forId == null)
		{
			return num;
		}
		if (!forId.IsBlock())
		{
			if (forId.FuelValue != null)
			{
				num = (float)forId.FuelValue.Value;
			}
		}
		else if (forId.Id < Block.list.Length)
		{
			Block block = Block.list[forId.Id];
			if (block != null)
			{
				num = (float)block.FuelValue;
			}
		}
		return num;
	}

	// Token: 0x06006259 RID: 25177 RVA: 0x0026FD9C File Offset: 0x0026DF9C
	[PublicizedFrom(EAccessModifier.Private)]
	public void cycleFuelStacks()
	{
		if (this.fuel.Length < 2)
		{
			return;
		}
		for (int i = 0; i < this.fuel.Length - 1; i++)
		{
			for (int j = 0; j < this.fuel.Length; j++)
			{
				ItemStack itemStack = this.fuel[j];
				if (itemStack.count <= 0 && j + 1 < this.fuel.Length)
				{
					ItemStack itemStack2 = this.fuel[j + 1];
					itemStack = itemStack2.Clone();
					this.fuel[j] = itemStack;
					itemStack2 = ItemStack.Empty;
					this.fuel[j + 1] = itemStack2;
				}
			}
			if (this.fuel[0].count > 0)
			{
				break;
			}
		}
	}

	// Token: 0x0600625A RID: 25178 RVA: 0x0026FE38 File Offset: 0x0026E038
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setModified()
	{
		base.setModified();
	}

	// Token: 0x0600625B RID: 25179 RVA: 0x0026FE40 File Offset: 0x0026E040
	[PublicizedFrom(EAccessModifier.Private)]
	public float getTotalFuelSeconds()
	{
		float num = 0f;
		for (int i = 0; i < this.fuel.Length; i++)
		{
			if (!this.fuel[i].IsEmpty())
			{
				num += (float)ItemClass.GetFuelValue(this.fuel[i].itemValue) * (float)this.fuel[i].count;
			}
		}
		return num;
	}

	// Token: 0x0600625C RID: 25180 RVA: 0x0026FE9C File Offset: 0x0026E09C
	[PublicizedFrom(EAccessModifier.Private)]
	public float GetFuelTime(ItemStack _fuel)
	{
		if (_fuel.itemValue.type == 0)
		{
			return 0f;
		}
		return (float)ItemClass.GetFuelValue(_fuel.itemValue);
	}

	// Token: 0x17000A36 RID: 2614
	// (get) Token: 0x0600625D RID: 25181 RVA: 0x0026FEC0 File Offset: 0x0026E0C0
	public bool IsEmpty
	{
		get
		{
			return !this.hasRecipeInQueue() && this.isEmpty(this.fuel) && this.isEmpty(this.tools) && this.isEmpty(this.output) && this.InputIsEmpty();
		}
	}

	// Token: 0x0600625E RID: 25182 RVA: 0x0026FF14 File Offset: 0x0026E114
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isEmpty(ItemStack[] items)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (!items[i].IsEmpty())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600625F RID: 25183 RVA: 0x0026FF40 File Offset: 0x0026E140
	public bool InputIsEmpty()
	{
		int num = this.input.Length - this.materialNames.Length;
		int i;
		for (i = 0; i < num; i++)
		{
			if (!this.input[i].IsEmpty())
			{
				return false;
			}
		}
		while (i < this.input.Length)
		{
			ItemStack itemStack = this.input[i];
			if (itemStack.itemValue.type > 0 && itemStack.count >= 10)
			{
				return false;
			}
			i++;
		}
		return true;
	}

	// Token: 0x06006260 RID: 25184 RVA: 0x0026FFB4 File Offset: 0x0026E1B4
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasRecipeInQueue()
	{
		for (int i = 0; i < this.queue.Length; i++)
		{
			RecipeQueueItem recipeQueueItem = this.queue[i];
			if (recipeQueueItem != null && recipeQueueItem.Multiplier > 0 && recipeQueueItem.Recipe != null)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06006261 RID: 25185 RVA: 0x0026FFF4 File Offset: 0x0026E1F4
	public bool OutputEmpty()
	{
		for (int i = 0; i < this.output.Length; i++)
		{
			if (!this.output[i].IsEmpty())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06006262 RID: 25186 RVA: 0x00270028 File Offset: 0x0026E228
	public void ResetCraftingQueue()
	{
		for (int i = 0; i < this.queue.Length; i++)
		{
			RecipeQueueItem recipeQueueItem = new RecipeQueueItem();
			recipeQueueItem.Recipe = null;
			recipeQueueItem.Multiplier = 0;
			recipeQueueItem.CraftingTimeLeft = 0f;
			recipeQueueItem.OneItemCraftTime = 0f;
			recipeQueueItem.IsCrafting = false;
			recipeQueueItem.Quality = 0;
			recipeQueueItem.StartingEntityId = -1;
			this.queue[i] = recipeQueueItem;
		}
	}

	// Token: 0x06006263 RID: 25187 RVA: 0x00270090 File Offset: 0x0026E290
	public override void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		int num = (int)_br.ReadByte();
		if (_eStreamMode == StreamModeRead.Persistency)
		{
			this.lastTickTime = _br.ReadUInt64();
			this.readItemStackArray(_br, ref this.fuel);
			this.readItemStackArray(_br, ref this.input);
			this.readItemStackArray(_br, ref this.toolsNet);
			this.readItemStackArray(_br, ref this.output);
			this.readRecipeStackArray(_br, num, ref this.queue);
			this.readCraftCompleteData(_br, num);
			if (!this.bUserAccessing)
			{
				this.isBurning = _br.ReadBoolean();
				this.currentBurnTimeLeft = _br.ReadSingle();
				int num2 = (int)_br.ReadByte();
				for (int i = 0; i < num2; i++)
				{
					this.currentMeltTimesLeft[i] = _br.ReadSingle();
				}
			}
			else
			{
				_br.ReadBoolean();
				_br.ReadSingle();
				int num3 = (int)_br.ReadByte();
				for (int j = 0; j < num3; j++)
				{
					_br.ReadSingle();
				}
			}
			this.isPlayerPlaced = _br.ReadBoolean();
			this.readItemStackArray(_br, ref this.lastInput);
		}
		else if (_eStreamMode == StreamModeRead.FromClient)
		{
			this.readItemStackArray(_br, ref this.fuel);
			this.readItemStackArray(_br, ref this.input);
			this.readItemStackArray(_br, ref this.toolsNet);
			this.readItemStackArray(_br, ref this.output);
			this.readRecipeStackArray(_br, num, ref this.queue);
			this.readCraftCompleteData(_br, num);
			this.isBurning = _br.ReadBoolean();
			this.currentBurnTimeLeft = _br.ReadSingle();
			int num4 = (int)_br.ReadByte();
			for (int k = 0; k < num4; k++)
			{
				this.currentMeltTimesLeft[k] = _br.ReadSingle();
			}
			this.isPlayerPlaced = _br.ReadBoolean();
			ulong num5 = _br.ReadUInt64();
			this.lastTickTime = GameTimer.Instance.ticks - num5;
			this.readItemStackArray(_br, ref this.lastInput);
		}
		else if (_eStreamMode == StreamModeRead.FromServer)
		{
			this.readItemStackArray(_br, ref this.fuel);
			this.readItemStackArray(_br, ref this.input);
			this.readItemStackArray(_br, ref this.toolsNet);
			this.readItemStackArray(_br, ref this.output);
			this.readRecipeStackArray(_br, num, ref this.queue);
			this.readCraftCompleteData(_br, num);
			if (!this.bUserAccessing)
			{
				this.isBurning = _br.ReadBoolean();
				this.currentBurnTimeLeft = _br.ReadSingle();
				int num6 = (int)_br.ReadByte();
				for (int l = 0; l < num6; l++)
				{
					this.currentMeltTimesLeft[l] = _br.ReadSingle();
				}
			}
			else
			{
				_br.ReadBoolean();
				_br.ReadSingle();
				int num7 = (int)_br.ReadByte();
				for (int m = 0; m < num7; m++)
				{
					_br.ReadSingle();
				}
			}
			this.isPlayerPlaced = _br.ReadBoolean();
			ulong num8 = _br.ReadUInt64();
			if (!this.bUserAccessing)
			{
				this.lastTickTime = GameTimer.Instance.ticks - num8;
			}
			this.readItemStackArray(_br, ref this.lastInput);
		}
		this.OnSetLocalChunkPosition();
		this.SetDataFromNet();
	}

	// Token: 0x06006264 RID: 25188 RVA: 0x00270370 File Offset: 0x0026E570
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDataFromNet()
	{
		if (this.bUserAccessing)
		{
			return;
		}
		if (!this.IsToolsSame(this.toolsNet))
		{
			this.tools = ItemStack.Clone(this.toolsNet);
			this.visibleChanged = true;
		}
		this.UpdateVisible();
	}

	// Token: 0x06006265 RID: 25189 RVA: 0x002703A8 File Offset: 0x0026E5A8
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		_bw.Write(50);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(this.lastTickTime);
			this.writeItemStackArray(_bw, this.fuel);
			this.writeItemStackArray(_bw, this.input);
			this.writeItemStackArray(_bw, this.tools);
			this.writeItemStackArray(_bw, this.output);
			this.writeRecipeStackArray(_bw, 50);
			this.writeCraftCompleteData(_bw, 50);
			_bw.Write(this.isBurning);
			_bw.Write(this.currentBurnTimeLeft);
			int num = this.currentMeltTimesLeft.Length;
			_bw.Write((byte)num);
			for (int i = 0; i < num; i++)
			{
				_bw.Write(this.currentMeltTimesLeft[i]);
			}
			_bw.Write(this.isPlayerPlaced);
			this.writeItemStackArray(_bw, this.lastInput);
			return;
		}
		if (_eStreamMode == StreamModeWrite.ToServer)
		{
			this.writeItemStackArray(_bw, this.fuel);
			this.writeItemStackArray(_bw, this.input);
			this.writeItemStackArray(_bw, this.tools);
			this.writeItemStackArray(_bw, this.output);
			this.writeRecipeStackArray(_bw, 50);
			this.writeCraftCompleteData(_bw, 50);
			_bw.Write(this.isBurning);
			_bw.Write(this.currentBurnTimeLeft);
			int num2 = this.currentMeltTimesLeft.Length;
			_bw.Write((byte)num2);
			for (int j = 0; j < num2; j++)
			{
				_bw.Write(this.currentMeltTimesLeft[j]);
			}
			_bw.Write(this.isPlayerPlaced);
			_bw.Write(GameTimer.Instance.ticks - this.lastTickTime);
			this.writeItemStackArray(_bw, this.lastInput);
			return;
		}
		if (_eStreamMode == StreamModeWrite.ToClient)
		{
			this.writeItemStackArray(_bw, this.fuel);
			this.writeItemStackArray(_bw, this.input);
			this.writeItemStackArray(_bw, this.tools);
			this.writeItemStackArray(_bw, this.output);
			this.writeRecipeStackArray(_bw, 50);
			this.writeCraftCompleteData(_bw, 50);
			_bw.Write(this.isBurning);
			_bw.Write(this.currentBurnTimeLeft);
			int num3 = this.currentMeltTimesLeft.Length;
			_bw.Write((byte)num3);
			for (int k = 0; k < num3; k++)
			{
				_bw.Write(this.currentMeltTimesLeft[k]);
			}
			_bw.Write(this.isPlayerPlaced);
			_bw.Write(GameTimer.Instance.ticks - this.lastTickTime);
			this.writeItemStackArray(_bw, this.lastInput);
		}
	}

	// Token: 0x06006266 RID: 25190 RVA: 0x00270604 File Offset: 0x0026E804
	[PublicizedFrom(EAccessModifier.Private)]
	public void readItemStackArray(PooledBinaryReader _br, ref ItemStack[] stack, ref ItemStack[] lastStack)
	{
		int num = (int)_br.ReadByte();
		if (stack == null || stack.Length != num)
		{
			stack = ItemStack.CreateArray(num);
		}
		if (!base.bWaitingForServerResponse)
		{
			for (int i = 0; i < num; i++)
			{
				stack[i].Read(_br);
			}
			lastStack = ItemStack.Clone(stack);
			return;
		}
		ItemStack empty = ItemStack.Empty;
		for (int j = 0; j < num; j++)
		{
			empty.Read(_br);
		}
	}

	// Token: 0x06006267 RID: 25191 RVA: 0x00270670 File Offset: 0x0026E870
	[PublicizedFrom(EAccessModifier.Private)]
	public void readItemStackArray(PooledBinaryReader _br, ref ItemStack[] stack)
	{
		int num = (int)_br.ReadByte();
		if (stack == null || stack.Length != num)
		{
			stack = ItemStack.CreateArray(num);
		}
		if (!this.bUserAccessing)
		{
			for (int i = 0; i < num; i++)
			{
				stack[i].Read(_br);
			}
			return;
		}
		ItemStack empty = ItemStack.Empty;
		for (int j = 0; j < num; j++)
		{
			empty.Read(_br);
		}
	}

	// Token: 0x06006268 RID: 25192 RVA: 0x002706D4 File Offset: 0x0026E8D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void writeItemStackArray(PooledBinaryWriter bw, ItemStack[] stack)
	{
		byte b = ((stack != null) ? ((byte)stack.Length) : 0);
		bw.Write(b);
		if (stack == null)
		{
			return;
		}
		for (int i = 0; i < stack.Length; i++)
		{
			stack[i].Write(bw);
		}
	}

	// Token: 0x06006269 RID: 25193 RVA: 0x00270710 File Offset: 0x0026E910
	[PublicizedFrom(EAccessModifier.Private)]
	public void readCraftCompleteData(PooledBinaryReader _br, int version)
	{
		if (version <= 45)
		{
			return;
		}
		int num = (int)_br.ReadInt16();
		if (this.CraftCompleteList == null)
		{
			this.CraftCompleteList = new List<CraftCompleteData>();
		}
		this.CraftCompleteList.Clear();
		for (int i = 0; i < num; i++)
		{
			CraftCompleteData craftCompleteData = new CraftCompleteData();
			if (version >= 50)
			{
				craftCompleteData.Read(_br);
			}
			else
			{
				craftCompleteData.ReadLegacy(_br);
			}
			this.CraftCompleteList.Add(craftCompleteData);
		}
	}

	// Token: 0x0600626A RID: 25194 RVA: 0x0027077C File Offset: 0x0026E97C
	public void writeCraftCompleteData(PooledBinaryWriter _bw, int version)
	{
		short num = ((this.CraftCompleteList != null) ? ((short)this.CraftCompleteList.Count) : 0);
		_bw.Write(num);
		if (this.CraftCompleteList != null)
		{
			for (int i = 0; i < this.CraftCompleteList.Count; i++)
			{
				this.CraftCompleteList[i].Write(_bw);
			}
		}
	}

	// Token: 0x0600626B RID: 25195 RVA: 0x002707D8 File Offset: 0x0026E9D8
	[PublicizedFrom(EAccessModifier.Private)]
	public void readRecipeStackArray(PooledBinaryReader _br, int version, ref RecipeQueueItem[] queueStack)
	{
		int num = (int)_br.ReadByte();
		if (queueStack == null || queueStack.Length != num)
		{
			queueStack = new RecipeQueueItem[num];
		}
		if (!base.bWaitingForServerResponse)
		{
			for (int i = 0; i < num; i++)
			{
				if (queueStack[i] == null)
				{
					queueStack[i] = new RecipeQueueItem();
				}
				if (version >= 50)
				{
					queueStack[i].Read(_br);
				}
				else
				{
					queueStack[i].ReadLegacy(_br);
				}
			}
			return;
		}
		RecipeQueueItem recipeQueueItem = new RecipeQueueItem();
		for (int j = 0; j < num; j++)
		{
			if (version >= 50)
			{
				recipeQueueItem.Read(_br);
			}
			else
			{
				recipeQueueItem.ReadLegacy(_br);
			}
		}
	}

	// Token: 0x0600626C RID: 25196 RVA: 0x00270868 File Offset: 0x0026EA68
	public void writeRecipeStackArray(PooledBinaryWriter _bw, int version)
	{
		byte b = ((this.queue != null) ? ((byte)this.queue.Length) : 0);
		_bw.Write(b);
		if (this.queue == null)
		{
			return;
		}
		for (int i = 0; i < this.queue.Length; i++)
		{
			if (this.queue[i] != null)
			{
				this.queue[i].Write(_bw);
			}
			else
			{
				RecipeQueueItem recipeQueueItem = new RecipeQueueItem();
				recipeQueueItem.Multiplier = 0;
				recipeQueueItem.Recipe = null;
				recipeQueueItem.IsCrafting = false;
				this.queue[i] = recipeQueueItem;
				this.queue[i].Write(_bw);
			}
		}
	}

	// Token: 0x0600626D RID: 25197 RVA: 0x002708FC File Offset: 0x0026EAFC
	public void AddCraftComplete(int crafterEntityID, ItemValue itemCrafted, string recipeName, string itemScrapped, int craftExpGain, int craftedCount)
	{
		if (this.CraftCompleteList == null)
		{
			this.CraftCompleteList = new List<CraftCompleteData>();
		}
		for (int i = 0; i < this.CraftCompleteList.Count; i++)
		{
			if (this.CraftCompleteList[i].CraftedItemStack.itemValue.GetItemId() == itemCrafted.GetItemId() && this.CraftCompleteList[i].ItemScrapped == itemScrapped)
			{
				this.CraftCompleteList[i].CraftedItemStack.count += craftedCount;
				this.setModified();
				return;
			}
		}
		this.CraftCompleteList.Add(new CraftCompleteData(crafterEntityID, new ItemStack(itemCrafted, craftedCount), recipeName, itemScrapped, craftExpGain, 1));
		this.setModified();
	}

	// Token: 0x0600626E RID: 25198 RVA: 0x002709BC File Offset: 0x0026EBBC
	public void CheckForCraftComplete(EntityPlayerLocal player)
	{
		if (this.CraftCompleteList == null)
		{
			return;
		}
		bool flag = false;
		for (int i = this.CraftCompleteList.Count - 1; i >= 0; i--)
		{
			if (this.CraftCompleteList[i].CrafterEntityID == player.entityId)
			{
				player.equipment.UnlockCosmeticItem(ItemClass.GetItemClass(this.CraftCompleteList[i].ItemScrapped, false));
				player.GiveExp(this.CraftCompleteList[i]);
				this.CraftCompleteList.RemoveAt(i);
				flag = true;
			}
		}
		if (flag)
		{
			this.setModified();
		}
	}

	// Token: 0x0600626F RID: 25199 RVA: 0x00270A50 File Offset: 0x0026EC50
	public override void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
		base.ReplacedBy(_bvOld, _bvNew, _teNew);
		TileEntityWorkstation tileEntityWorkstation;
		if (_teNew.TryGetSelfOrFeature<TileEntityWorkstation>(out tileEntityWorkstation))
		{
			return;
		}
		List<ItemStack> list = new List<ItemStack>();
		if (this.fuel != null)
		{
			list.AddRange(this.fuel);
		}
		if (this.input != null)
		{
			for (int i = 0; i < 3; i++)
			{
				if (!this.input[i].IsEmpty())
				{
					list.Add(this.input[i]);
				}
			}
			List<Recipe> allRecipes = CraftingManager.GetAllRecipes();
			for (int j = 0; j < this.materialNames.Length; j++)
			{
				int num = j + 3;
				ItemClass itemClass = ItemClass.GetItemClass("unit_" + this.materialNames[j], false);
				if (itemClass != null && itemClass.MadeOfMaterial.ForgeCategory != null)
				{
					ItemStack itemStack = this.input[num];
					if (itemStack.itemValue.type == 0)
					{
						this.input[num] = new ItemStack(new ItemValue(itemClass.Id, false), itemStack.count);
					}
					Recipe recipe = null;
					foreach (Recipe recipe2 in allRecipes)
					{
						if (recipe2.ingredients.Count == 1 && recipe2.ingredients[0].itemValue.type == itemClass.Id && (!recipe2.UseIngredientModifier || recipe == null))
						{
							recipe = recipe2;
						}
					}
					if (recipe == null)
					{
						Log.Warning("No craft out recipe found for workstation input " + itemClass.GetItemName());
					}
					else
					{
						int k = itemStack.count / recipe.ingredients[0].count;
						ItemValue itemValue = new ItemValue(recipe.itemValueType, false);
						int maxCount = itemValue.ItemClass.MaxCount;
						while (k > 0)
						{
							int num2 = Mathf.Min(k, maxCount);
							list.Add(new ItemStack(itemValue, num2));
							k -= num2;
						}
					}
				}
			}
		}
		if (this.tools != null)
		{
			list.AddRange(this.tools);
		}
		if (this.output != null)
		{
			list.AddRange(this.output);
		}
		Vector3 vector = base.ToWorldCenterPos();
		vector.y += 0.9f;
		GameManager.Instance.DropContentInLootContainerServer(-1, "DroppedLootContainer", vector, list.ToArray(), true, null);
	}

	// Token: 0x06006270 RID: 25200 RVA: 0x00270CBC File Offset: 0x0026EEBC
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Workstation;
	}

	// Token: 0x06006271 RID: 25201 RVA: 0x00270CC0 File Offset: 0x0026EEC0
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return false;
		}
		string blockName = GameManager.Instance.World.GetBlock(base.ToWorldPos()).Block.GetBlockName();
		WorkstationData workstationData = CraftingManager.GetWorkstationData(blockName);
		if (workstationData == null)
		{
			return false;
		}
		string text = ((workstationData.WorkstationWindow != "") ? workstationData.WorkstationWindow : string.Format("workstation_{0}", blockName));
		if (blockName == "forge" && XUiM_Recipes.DisableSmelter)
		{
			text += "_nosmelting";
		}
		GUIWindow guiwindow;
		if (uiforPrimaryPlayer.windowManager.TryGetWindow(text, out guiwindow))
		{
			((XUiC_WorkstationWindowGroup)((XUiWindowGroup)guiwindow).Controller).SetTileEntity(this);
			uiforPrimaryPlayer.windowManager.Open(text, true);
			return true;
		}
		Log.Warning("Window '{0}' not found in XUI!", new object[] { text });
		return false;
	}

	// Token: 0x04004CC8 RID: 19656
	public const int ChangedFuel = 1;

	// Token: 0x04004CC9 RID: 19657
	public const int ChangedInput = 2;

	// Token: 0x04004CCA RID: 19658
	public const int OutputItemAdded = 4;

	// Token: 0x04004CCB RID: 19659
	public new const int Version = 50;

	// Token: 0x04004CCE RID: 19662
	public const int cInputSlotCount = 3;

	// Token: 0x04004CCF RID: 19663
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMinInternalMatCount = 10;

	// Token: 0x04004CD0 RID: 19664
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] fuel;

	// Token: 0x04004CD1 RID: 19665
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] input;

	// Token: 0x04004CD2 RID: 19666
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] tools;

	// Token: 0x04004CD3 RID: 19667
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] toolsNet;

	// Token: 0x04004CD4 RID: 19668
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] output;

	// Token: 0x04004CD5 RID: 19669
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeQueueItem[] queue;

	// Token: 0x04004CD6 RID: 19670
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastTickTime;

	// Token: 0x04004CD7 RID: 19671
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentBurnTimeLeft;

	// Token: 0x04004CD8 RID: 19672
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] currentMeltTimesLeft;

	// Token: 0x04004CD9 RID: 19673
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] lastInput;

	// Token: 0x04004CDA RID: 19674
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBurning;

	// Token: 0x04004CDB RID: 19675
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBesideWater;

	// Token: 0x04004CDC RID: 19676
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPlayerPlaced;

	// Token: 0x04004CDD RID: 19677
	[PublicizedFrom(EAccessModifier.Private)]
	public bool visibleChanged;

	// Token: 0x04004CDE RID: 19678
	[PublicizedFrom(EAccessModifier.Private)]
	public bool visibleCrafting;

	// Token: 0x04004CDF RID: 19679
	[PublicizedFrom(EAccessModifier.Private)]
	public bool visibleWorking;

	// Token: 0x04004CE0 RID: 19680
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] materialNames;

	// Token: 0x04004CE1 RID: 19681
	[PublicizedFrom(EAccessModifier.Private)]
	public List<CraftCompleteData> CraftCompleteList;

	// Token: 0x04004CE2 RID: 19682
	[PublicizedFrom(EAccessModifier.Private)]
	public bool[] isModuleUsed;

	// Token: 0x02000C9B RID: 3227
	[PublicizedFrom(EAccessModifier.Private)]
	public enum Module
	{
		// Token: 0x04004CE4 RID: 19684
		Tools,
		// Token: 0x04004CE5 RID: 19685
		Input,
		// Token: 0x04004CE6 RID: 19686
		Output,
		// Token: 0x04004CE7 RID: 19687
		Fuel,
		// Token: 0x04004CE8 RID: 19688
		Material_Input,
		// Token: 0x04004CE9 RID: 19689
		Count
	}
}
