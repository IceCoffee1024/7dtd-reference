using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A1B RID: 6683
	[Preserve]
	public class ActionEmptyContainers : ActionBaseContainersAction
	{
		// Token: 0x0600CBD0 RID: 52176 RVA: 0x004A773C File Offset: 0x004A593C
		public override bool CheckValidTileEntity(TileEntity te, out bool isEmpty)
		{
			TileEntityType tileEntityType = te.GetTileEntityType();
			isEmpty = true;
			if (tileEntityType != TileEntityType.Workstation)
			{
				TEFeatureStorage tefeatureStorage;
				if (tileEntityType == TileEntityType.Composite && te.TryGetSelfOrFeature<TEFeatureStorage>(out tefeatureStorage))
				{
					isEmpty = tefeatureStorage.IsEmpty();
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

		// Token: 0x0600CBD1 RID: 52177 RVA: 0x004A785C File Offset: 0x004A5A5C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleContainerAction(List<TileEntity> tileEntityList)
		{
			bool flag = false;
			for (int i = 0; i < tileEntityList.Count; i++)
			{
				TileEntityType tileEntityType = tileEntityList[i].GetTileEntityType();
				if (tileEntityType != TileEntityType.Workstation)
				{
					TEFeatureStorage tefeatureStorage;
					if (tileEntityType == TileEntityType.Composite && tileEntityList[i].TryGetSelfOrFeature<TEFeatureStorage>(out tefeatureStorage) && !tefeatureStorage.IsEmpty())
					{
						tefeatureStorage.SetEmpty();
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

		// Token: 0x0600CBD2 RID: 52178 RVA: 0x004A7AA0 File Offset: 0x004A5CA0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionEmptyContainers.PropIncludeInputs, ref this.includeInputs);
			properties.ParseBool(ActionEmptyContainers.PropIncludeOutputs, ref this.includeOutputs);
			properties.ParseBool(ActionEmptyContainers.PropIncludeFuel, ref this.includeFuel);
			properties.ParseBool(ActionEmptyContainers.PropIncludeTools, ref this.includeTools);
		}

		// Token: 0x0600CBD3 RID: 52179 RVA: 0x004A7AF8 File Offset: 0x004A5CF8
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

		// Token: 0x04009A82 RID: 39554
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool includeInputs;

		// Token: 0x04009A83 RID: 39555
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool includeOutputs;

		// Token: 0x04009A84 RID: 39556
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool includeFuel;

		// Token: 0x04009A85 RID: 39557
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool includeTools;

		// Token: 0x04009A86 RID: 39558
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIncludeInputs = "include_inputs";

		// Token: 0x04009A87 RID: 39559
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIncludeOutputs = "include_outputs";

		// Token: 0x04009A88 RID: 39560
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIncludeFuel = "include_fuel";

		// Token: 0x04009A89 RID: 39561
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIncludeTools = "include_tools";
	}
}
