using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C8 RID: 6600
	[Preserve]
	public class ActionEmptyContainers : ActionBaseContainersAction
	{
		// Token: 0x0600C8EF RID: 51439 RVA: 0x0049CE9C File Offset: 0x0049B09C
		public override bool CheckValidTileEntity(TileEntity te, out bool isEmpty)
		{
			TileEntityType tileEntityType = te.GetTileEntityType();
			isEmpty = true;
			if (tileEntityType != TileEntityType.Workstation)
			{
				ITileEntityLootable tileEntityLootable;
				if (tileEntityType == TileEntityType.Composite && te.TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable))
				{
					isEmpty = tileEntityLootable.IsEmpty();
					return true;
				}
			}
			else
			{
				TileEntityWorkstation tileEntityWorkstation = te as TileEntityWorkstation;
				if (tileEntityWorkstation != null)
				{
					if (this.includeInputs)
					{
						ItemStack[] input = tileEntityWorkstation.Input;
						for (int i = 0; i < input.Length; i++)
						{
							if (!input[i].IsEmpty())
							{
								isEmpty = false;
							}
						}
					}
					if (this.includeOutputs)
					{
						ItemStack[] output = tileEntityWorkstation.Output;
						for (int j = 0; j < output.Length; j++)
						{
							if (!output[j].IsEmpty())
							{
								isEmpty = false;
							}
						}
					}
					if (this.includeFuel)
					{
						tileEntityWorkstation.IsBurning = false;
						tileEntityWorkstation.ResetTickTime();
						ItemStack[] fuel = tileEntityWorkstation.Fuel;
						for (int k = 0; k < fuel.Length; k++)
						{
							if (!fuel[k].IsEmpty())
							{
								isEmpty = false;
							}
						}
					}
					if (this.includeTools)
					{
						ItemStack[] tools = tileEntityWorkstation.Tools;
						for (int l = 0; l < tools.Length; l++)
						{
							if (!tools[l].IsEmpty())
							{
								isEmpty = false;
							}
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C8F0 RID: 51440 RVA: 0x0049CFBC File Offset: 0x0049B1BC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleContainerAction(List<TileEntity> tileEntityList)
		{
			bool flag = false;
			for (int i = 0; i < tileEntityList.Count; i++)
			{
				TileEntityType tileEntityType = tileEntityList[i].GetTileEntityType();
				if (tileEntityType != TileEntityType.Workstation)
				{
					ITileEntityLootable tileEntityLootable;
					if (tileEntityType == TileEntityType.Composite && tileEntityList[i].TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable) && !tileEntityLootable.IsEmpty())
					{
						tileEntityLootable.SetEmpty();
						flag = true;
					}
				}
				else
				{
					TileEntityWorkstation tileEntityWorkstation = tileEntityList[i] as TileEntityWorkstation;
					if (tileEntityWorkstation != null)
					{
						if (this.includeInputs)
						{
							ItemStack[] input = tileEntityWorkstation.Input;
							for (int j = 0; j < input.Length; j++)
							{
								if (!input[j].IsEmpty())
								{
									input[j] = ItemStack.Empty;
									flag = true;
								}
							}
							tileEntityWorkstation.ClearSlotTimersForInputs();
							tileEntityWorkstation.Input = input;
						}
						if (this.includeOutputs)
						{
							ItemStack[] output = tileEntityWorkstation.Output;
							for (int k = 0; k < output.Length; k++)
							{
								if (!output[k].IsEmpty())
								{
									output[k] = ItemStack.Empty;
									flag = true;
								}
							}
							tileEntityWorkstation.Output = output;
						}
						if (this.includeFuel)
						{
							tileEntityWorkstation.IsBurning = false;
							tileEntityWorkstation.ResetTickTime();
							ItemStack[] fuel = tileEntityWorkstation.Fuel;
							for (int l = 0; l < fuel.Length; l++)
							{
								if (!fuel[l].IsEmpty())
								{
									fuel[l] = ItemStack.Empty;
									flag = true;
								}
							}
							tileEntityWorkstation.Fuel = fuel;
						}
						if (this.includeTools)
						{
							ItemStack[] tools = tileEntityWorkstation.Tools;
							for (int m = 0; m < tools.Length; m++)
							{
								if (!tools[m].IsEmpty())
								{
									tools[m] = ItemStack.Empty;
									flag = true;
								}
							}
							tileEntityWorkstation.Tools = tools;
						}
						if (this.includeTools || this.includeOutputs)
						{
							tileEntityWorkstation.ResetCraftingQueue();
						}
					}
				}
			}
			if (flag && this.changeName && base.Owner.Target != null)
			{
				PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(base.Owner.Target.entityId);
				for (int n = 0; n < tileEntityList.Count; n++)
				{
					ITileEntitySignable tileEntitySignable;
					if (tileEntityList[n].TryGetSelfOrFeature<ITileEntitySignable>(out tileEntitySignable))
					{
						tileEntitySignable.SetText(base.ModifiedName, true, (playerDataFromEntityID != null) ? playerDataFromEntityID.PrimaryId : null);
					}
				}
			}
			return flag;
		}

		// Token: 0x0600C8F1 RID: 51441 RVA: 0x0049D200 File Offset: 0x0049B400
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionEmptyContainers.PropIncludeInputs, ref this.includeInputs);
			properties.ParseBool(ActionEmptyContainers.PropIncludeOutputs, ref this.includeOutputs);
			properties.ParseBool(ActionEmptyContainers.PropIncludeFuel, ref this.includeFuel);
			properties.ParseBool(ActionEmptyContainers.PropIncludeTools, ref this.includeTools);
		}

		// Token: 0x0600C8F2 RID: 51442 RVA: 0x0049D258 File Offset: 0x0049B458
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionEmptyContainers
			{
				TargetingType = this.TargetingType,
				maxDistance = this.maxDistance,
				newName = this.newName,
				changeName = this.changeName,
				includeInputs = this.includeInputs,
				includeOutputs = this.includeOutputs,
				includeFuel = this.includeFuel,
				includeTools = this.includeTools,
				tileEntityList = this.tileEntityList
			};
		}

		// Token: 0x0400988B RID: 39051
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool includeInputs;

		// Token: 0x0400988C RID: 39052
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool includeOutputs;

		// Token: 0x0400988D RID: 39053
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool includeFuel;

		// Token: 0x0400988E RID: 39054
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool includeTools;

		// Token: 0x0400988F RID: 39055
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIncludeInputs = "include_inputs";

		// Token: 0x04009890 RID: 39056
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIncludeOutputs = "include_outputs";

		// Token: 0x04009891 RID: 39057
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIncludeFuel = "include_fuel";

		// Token: 0x04009892 RID: 39058
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIncludeTools = "include_tools";
	}
}
