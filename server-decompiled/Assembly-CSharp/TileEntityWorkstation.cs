using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000C6F RID: 3183
public class TileEntityWorkstation : TileEntity
{
	// Token: 0x14000088 RID: 136
	// (add) Token: 0x0600618B RID: 24971 RVA: 0x002672D8 File Offset: 0x002654D8
	// (remove) Token: 0x0600618C RID: 24972 RVA: 0x00267310 File Offset: 0x00265510
	public event XUiEvent_InputStackChanged InputChanged;

	// Token: 0x14000089 RID: 137
	// (add) Token: 0x0600618D RID: 24973 RVA: 0x00267348 File Offset: 0x00265548
	// (remove) Token: 0x0600618E RID: 24974 RVA: 0x00267380 File Offset: 0x00265580
	public event XUiEvent_FuelStackChanged FuelChanged;

	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x0600618F RID: 24975 RVA: 0x002673B5 File Offset: 0x002655B5
	public string[] MaterialNames
	{
		get
		{
			return this.materialNames;
		}
	}

	// Token: 0x06006190 RID: 24976 RVA: 0x002673C0 File Offset: 0x002655C0
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

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x06006191 RID: 24977 RVA: 0x002673FF File Offset: 0x002655FF
	// (set) Token: 0x06006192 RID: 24978 RVA: 0x00267407 File Offset: 0x00265607
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

	// Token: 0x06006193 RID: 24979 RVA: 0x00267434 File Offset: 0x00265634
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

	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x06006194 RID: 24980 RVA: 0x00267476 File Offset: 0x00265676
	// (set) Token: 0x06006195 RID: 24981 RVA: 0x0026747E File Offset: 0x0026567E
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

	// Token: 0x170009FD RID: 2557
	// (get) Token: 0x06006196 RID: 24982 RVA: 0x00267492 File Offset: 0x00265692
	// (set) Token: 0x06006197 RID: 24983 RVA: 0x0026749A File Offset: 0x0026569A
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

	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x06006198 RID: 24984 RVA: 0x002674AE File Offset: 0x002656AE
	// (set) Token: 0x06006199 RID: 24985 RVA: 0x002674B6 File Offset: 0x002656B6
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

	// Token: 0x170009FF RID: 2559
	// (get) Token: 0x0600619A RID: 24986 RVA: 0x002674CA File Offset: 0x002656CA
	// (set) Token: 0x0600619B RID: 24987 RVA: 0x002674D2 File Offset: 0x002656D2
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

	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x0600619C RID: 24988 RVA: 0x002674E1 File Offset: 0x002656E1
	// (set) Token: 0x0600619D RID: 24989 RVA: 0x002674E9 File Offset: 0x002656E9
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

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x0600619E RID: 24990 RVA: 0x002674F8 File Offset: 0x002656F8
	public bool IsCrafting
	{
		get
		{
			return this.hasRecipeInQueue() && (!this.isModuleUsed[3] || this.isBurning);
		}
	}

	// Token: 0x17000A02 RID: 2562
	// (get) Token: 0x0600619F RID: 24991 RVA: 0x00267516 File Offset: 0x00265716
	// (set) Token: 0x060061A0 RID: 24992 RVA: 0x0026751E File Offset: 0x0026571E
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

	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x060061A1 RID: 24993 RVA: 0x0026752D File Offset: 0x0026572D
	public bool IsBesideWater
	{
		get
		{
			return this.isBesideWater;
		}
	}

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x060061A2 RID: 24994 RVA: 0x00267535 File Offset: 0x00265735
	public float BurnTimeLeft
	{
		get
		{
			return this.currentBurnTimeLeft;
		}
	}

	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x060061A3 RID: 24995 RVA: 0x0026753D File Offset: 0x0026573D
	public float BurnTotalTimeLeft
	{
		get
		{
			return this.getTotalFuelSeconds() + this.currentBurnTimeLeft;
		}
	}

	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x060061A4 RID: 24996 RVA: 0x00046C7A File Offset: 0x00044E7A
	public int InputSlotCount
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x060061A5 RID: 24997 RVA: 0x0026754C File Offset: 0x0026574C
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

	// Token: 0x060061A6 RID: 24998 RVA: 0x002675D3 File Offset: 0x002657D3
	public void ResetTickTime()
	{
		this.lastTickTime = GameTimer.Instance.ticks;
	}

	// Token: 0x060061A7 RID: 24999 RVA: 0x002675E8 File Offset: 0x002657E8
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

	// Token: 0x060061A8 RID: 25000 RVA: 0x0026780C File Offset: 0x00265A0C
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

	// Token: 0x060061A9 RID: 25001 RVA: 0x002674E1 File Offset: 0x002656E1
	public bool CanOperate(ulong _worldTimeInTicks)
	{
		return this.isBurning;
	}

	// Token: 0x060061AA RID: 25002 RVA: 0x0026787A File Offset: 0x00265A7A
	public override bool IsActive(World world)
	{
		return this.IsBurning;
	}

	// Token: 0x060061AB RID: 25003 RVA: 0x00267884 File Offset: 0x00265A84
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

	// Token: 0x060061AC RID: 25004 RVA: 0x002679D1 File Offset: 0x00265BD1
	public void SetVisibleChanged()
	{
		this.visibleChanged = true;
	}

	// Token: 0x060061AD RID: 25005 RVA: 0x002679DC File Offset: 0x00265BDC
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

	// Token: 0x060061AE RID: 25006 RVA: 0x00267A76 File Offset: 0x00265C76
	public float GetTimerForSlot(int inputSlot)
	{
		if (inputSlot >= this.currentMeltTimesLeft.Length)
		{
			return 0f;
		}
		return this.currentMeltTimesLeft[inputSlot];
	}

	// Token: 0x060061AF RID: 25007 RVA: 0x00267A94 File Offset: 0x00265C94
	public void ClearSlotTimersForInputs()
	{
		for (int i = 0; i < this.currentMeltTimesLeft.Length; i++)
		{
			this.currentMeltTimesLeft[i] = 0f;
		}
	}

	// Token: 0x060061B0 RID: 25008 RVA: 0x00267AC4 File Offset: 0x00265CC4
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
												if (this.input[num4].count + forId.GetWeight() > itemClass2.Stacknumber.Value)
												{
													this.currentMeltTimesLeft[i] = -2.1474836E+09f;
													break;
												}
												this.input[num4].count += forId.GetWeight();
												this.input[i].count--;
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

	// Token: 0x060061B1 RID: 25009 RVA: 0x0026802C File Offset: 0x0026622C
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

	// Token: 0x060061B2 RID: 25010 RVA: 0x002681C4 File Offset: 0x002663C4
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

	// Token: 0x060061B3 RID: 25011 RVA: 0x002683C0 File Offset: 0x002665C0
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
				this.fuel[0].count--;
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

	// Token: 0x060061B4 RID: 25012 RVA: 0x002684E0 File Offset: 0x002666E0
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

	// Token: 0x060061B5 RID: 25013 RVA: 0x0026855C File Offset: 0x0026675C
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

	// Token: 0x060061B6 RID: 25014 RVA: 0x002685F8 File Offset: 0x002667F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setModified()
	{
		base.setModified();
	}

	// Token: 0x060061B7 RID: 25015 RVA: 0x00268600 File Offset: 0x00266800
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

	// Token: 0x060061B8 RID: 25016 RVA: 0x0026865C File Offset: 0x0026685C
	[PublicizedFrom(EAccessModifier.Private)]
	public float GetFuelTime(ItemStack _fuel)
	{
		if (_fuel.itemValue.type == 0)
		{
			return 0f;
		}
		return (float)ItemClass.GetFuelValue(_fuel.itemValue);
	}

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x060061B9 RID: 25017 RVA: 0x00268680 File Offset: 0x00266880
	public bool IsEmpty
	{
		get
		{
			return !this.hasRecipeInQueue() && this.isEmpty(this.fuel) && this.isEmpty(this.tools) && this.isEmpty(this.output) && this.InputIsEmpty();
		}
	}

	// Token: 0x060061BA RID: 25018 RVA: 0x002686D4 File Offset: 0x002668D4
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

	// Token: 0x060061BB RID: 25019 RVA: 0x00268700 File Offset: 0x00266900
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

	// Token: 0x060061BC RID: 25020 RVA: 0x00268774 File Offset: 0x00266974
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

	// Token: 0x060061BD RID: 25021 RVA: 0x002687B4 File Offset: 0x002669B4
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

	// Token: 0x060061BE RID: 25022 RVA: 0x002687E8 File Offset: 0x002669E8
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

	// Token: 0x060061BF RID: 25023 RVA: 0x00268850 File Offset: 0x00266A50
	public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		int num = (int)_br.ReadByte();
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
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
		else if (_eStreamMode == TileEntity.StreamModeRead.FromClient)
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
		else if (_eStreamMode == TileEntity.StreamModeRead.FromServer)
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

	// Token: 0x060061C0 RID: 25024 RVA: 0x00268B30 File Offset: 0x00266D30
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

	// Token: 0x060061C1 RID: 25025 RVA: 0x00268B68 File Offset: 0x00266D68
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		_bw.Write(50);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
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
		if (_eStreamMode == TileEntity.StreamModeWrite.ToServer)
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
		if (_eStreamMode == TileEntity.StreamModeWrite.ToClient)
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

	// Token: 0x060061C2 RID: 25026 RVA: 0x00268DC4 File Offset: 0x00266FC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void readItemStackArray(BinaryReader _br, ref ItemStack[] stack, ref ItemStack[] lastStack)
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

	// Token: 0x060061C3 RID: 25027 RVA: 0x00268E30 File Offset: 0x00267030
	[PublicizedFrom(EAccessModifier.Private)]
	public void readItemStackArray(BinaryReader _br, ref ItemStack[] stack)
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

	// Token: 0x060061C4 RID: 25028 RVA: 0x00268E94 File Offset: 0x00267094
	[PublicizedFrom(EAccessModifier.Private)]
	public void writeItemStackArray(BinaryWriter bw, ItemStack[] stack)
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

	// Token: 0x060061C5 RID: 25029 RVA: 0x00268ED0 File Offset: 0x002670D0
	[PublicizedFrom(EAccessModifier.Private)]
	public void readCraftCompleteData(BinaryReader _br, int version)
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

	// Token: 0x060061C6 RID: 25030 RVA: 0x00268F3C File Offset: 0x0026713C
	public void writeCraftCompleteData(BinaryWriter _bw, int version)
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

	// Token: 0x060061C7 RID: 25031 RVA: 0x00268F98 File Offset: 0x00267198
	[PublicizedFrom(EAccessModifier.Private)]
	public void readRecipeStackArray(BinaryReader _br, int version, ref RecipeQueueItem[] queueStack)
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

	// Token: 0x060061C8 RID: 25032 RVA: 0x00269028 File Offset: 0x00267228
	public void writeRecipeStackArray(BinaryWriter _bw, int version)
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

	// Token: 0x060061C9 RID: 25033 RVA: 0x002690BC File Offset: 0x002672BC
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

	// Token: 0x060061CA RID: 25034 RVA: 0x0026917C File Offset: 0x0026737C
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

	// Token: 0x060061CB RID: 25035 RVA: 0x00269210 File Offset: 0x00267410
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
						int value = itemValue.ItemClass.Stacknumber.Value;
						while (k > 0)
						{
							int num2 = Mathf.Min(k, value);
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

	// Token: 0x060061CC RID: 25036 RVA: 0x0017DEA0 File Offset: 0x0017C0A0
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Workstation;
	}

	// Token: 0x060061CD RID: 25037 RVA: 0x00048695 File Offset: 0x00046895
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x060061CE RID: 25038 RVA: 0x00269480 File Offset: 0x00267680
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		string blockName = GameManager.Instance.World.GetBlock(base.ToWorldPos()).Block.GetBlockName();
		WorkstationData workstationData = CraftingManager.GetWorkstationData(blockName);
		if (workstationData == null)
		{
			return;
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
			return;
		}
		Log.Warning("Window '{0}' not found in XUI!", new object[] { text });
	}

	// Token: 0x04004BD0 RID: 19408
	public const int ChangedFuel = 1;

	// Token: 0x04004BD1 RID: 19409
	public const int ChangedInput = 2;

	// Token: 0x04004BD2 RID: 19410
	public const int OutputItemAdded = 4;

	// Token: 0x04004BD3 RID: 19411
	public new const int Version = 50;

	// Token: 0x04004BD6 RID: 19414
	public const int cInputSlotCount = 3;

	// Token: 0x04004BD7 RID: 19415
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cMinInternalMatCount = 10;

	// Token: 0x04004BD8 RID: 19416
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] fuel;

	// Token: 0x04004BD9 RID: 19417
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] input;

	// Token: 0x04004BDA RID: 19418
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] tools;

	// Token: 0x04004BDB RID: 19419
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] toolsNet;

	// Token: 0x04004BDC RID: 19420
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] output;

	// Token: 0x04004BDD RID: 19421
	[PublicizedFrom(EAccessModifier.Private)]
	public RecipeQueueItem[] queue;

	// Token: 0x04004BDE RID: 19422
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong lastTickTime;

	// Token: 0x04004BDF RID: 19423
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentBurnTimeLeft;

	// Token: 0x04004BE0 RID: 19424
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] currentMeltTimesLeft;

	// Token: 0x04004BE1 RID: 19425
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] lastInput;

	// Token: 0x04004BE2 RID: 19426
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBurning;

	// Token: 0x04004BE3 RID: 19427
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBesideWater;

	// Token: 0x04004BE4 RID: 19428
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPlayerPlaced;

	// Token: 0x04004BE5 RID: 19429
	[PublicizedFrom(EAccessModifier.Private)]
	public bool visibleChanged;

	// Token: 0x04004BE6 RID: 19430
	[PublicizedFrom(EAccessModifier.Private)]
	public bool visibleCrafting;

	// Token: 0x04004BE7 RID: 19431
	[PublicizedFrom(EAccessModifier.Private)]
	public bool visibleWorking;

	// Token: 0x04004BE8 RID: 19432
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] materialNames;

	// Token: 0x04004BE9 RID: 19433
	[PublicizedFrom(EAccessModifier.Private)]
	public List<CraftCompleteData> CraftCompleteList;

	// Token: 0x04004BEA RID: 19434
	[PublicizedFrom(EAccessModifier.Private)]
	public bool[] isModuleUsed;

	// Token: 0x02000C70 RID: 3184
	[PublicizedFrom(EAccessModifier.Private)]
	public enum Module
	{
		// Token: 0x04004BEC RID: 19436
		Tools,
		// Token: 0x04004BED RID: 19437
		Input,
		// Token: 0x04004BEE RID: 19438
		Output,
		// Token: 0x04004BEF RID: 19439
		Fuel,
		// Token: 0x04004BF0 RID: 19440
		Material_Input,
		// Token: 0x04004BF1 RID: 19441
		Count
	}
}
