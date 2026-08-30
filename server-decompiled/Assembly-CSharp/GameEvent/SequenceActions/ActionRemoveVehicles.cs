using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001980 RID: 6528
	[Preserve]
	public class ActionRemoveVehicles : ActionRemoveEntities
	{
		// Token: 0x0600C790 RID: 51088 RVA: 0x00496930 File Offset: 0x00494B30
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRemoveData(GameEventManager gm, Entity ent)
		{
			if (ent is EntityVehicle)
			{
				EntityVehicle entityVehicle = ent as EntityVehicle;
				for (int i = 0; i < entityVehicle.GetAttachMaxCount(); i++)
				{
					EntityPlayer entityPlayer = entityVehicle.GetAttached(i) as EntityPlayer;
					if (entityPlayer != null)
					{
						if (entityPlayer.isEntityRemote)
						{
							SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageCloseAllWindows>().Setup(entityPlayer.entityId), false, entityPlayer.entityId, -1, -1, null, 192, false);
						}
						else
						{
							(entityPlayer as EntityPlayerLocal).PlayerUI.windowManager.CloseAllOpenModalWindows(null, false);
						}
						entityPlayer.SendDetach();
					}
				}
				List<ItemStack> list = new List<ItemStack>();
				ActionRemoveVehicles.ReturnVehicleTypes returnVehicleTypes = this.includeVehicle;
				if (returnVehicleTypes != ActionRemoveVehicles.ReturnVehicleTypes.Vehicle)
				{
					if (returnVehicleTypes == ActionRemoveVehicles.ReturnVehicleTypes.Parts)
					{
						ItemStack[] items = entityVehicle.GetVehicle().GetItems();
						for (int j = 0; j < items[0].itemValue.CosmeticMods.Length; j++)
						{
							ItemValue itemValue = items[0].itemValue.CosmeticMods[j];
							if (itemValue != null && !itemValue.IsEmpty())
							{
								list.Add(new ItemStack(itemValue.Clone(), 1));
							}
						}
						for (int k = 0; k < items[0].itemValue.Modifications.Length; k++)
						{
							ItemValue itemValue2 = items[0].itemValue.Modifications[k];
							if (itemValue2 != null && !itemValue2.IsEmpty())
							{
								list.Add(new ItemStack(itemValue2.Clone(), 1));
							}
						}
						if (entityVehicle.GetVehicle().RecipeName != null)
						{
							Recipe recipe = CraftingManager.GetRecipe(entityVehicle.GetVehicle().RecipeName);
							if (recipe != null)
							{
								for (int l = 0; l < recipe.ingredients.Count; l++)
								{
									ItemStack itemStack = recipe.ingredients[l].Clone();
									if (itemStack.itemValue.HasQuality)
									{
										itemStack.itemValue.Quality = 1;
									}
									list.Add(itemStack);
								}
							}
						}
					}
				}
				else
				{
					list.Add(new ItemStack(entityVehicle.GetVehicle().GetUpdatedItemValue(), 1));
				}
				if (this.returnFuel)
				{
					int fuelCount = entityVehicle.GetFuelCount();
					if (fuelCount > 0)
					{
						list.Add(new ItemStack(ItemClass.GetItem(entityVehicle.vehicle.GetFuelItem(), false), fuelCount));
					}
				}
				if (this.returnItems)
				{
					ItemStack[] array = entityVehicle.inventory.GetSlots();
					for (int m = 0; m < array.Length; m++)
					{
						if (!array[m].IsEmpty())
						{
							list.Add(array[m]);
						}
					}
					array = entityVehicle.bag.GetSlots();
					for (int n = 0; n < array.Length; n++)
					{
						if (!array[n].IsEmpty())
						{
							list.Add(array[n]);
						}
					}
				}
				if (list.Count > 0)
				{
					EntityLootContainer entityLootContainer = EntityFactory.CreateEntity("DroppedLootContainerTwitch".GetHashCode(), ent.position, Vector3.zero) as EntityLootContainer;
					if (entityLootContainer != null)
					{
						entityLootContainer.SetContent(ItemStack.Clone(list));
					}
					GameManager.Instance.World.SpawnEntityInWorld(entityLootContainer);
					return;
				}
			}
			else if (ent is EntityTurret)
			{
				EntityTurret entityTurret = ent as EntityTurret;
				for (int num = 0; num < entityTurret.GetAttachMaxCount(); num++)
				{
					EntityPlayer entityPlayer2 = entityTurret.GetAttached(num) as EntityPlayer;
					if (entityPlayer2 != null)
					{
						if (entityPlayer2.isEntityRemote)
						{
							SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageCloseAllWindows>().Setup(entityPlayer2.entityId), false, entityPlayer2.entityId, -1, -1, null, 192, false);
						}
						else
						{
							(entityPlayer2 as EntityPlayerLocal).PlayerUI.windowManager.CloseAllOpenModalWindows(null, false);
						}
						entityPlayer2.SendDetach();
					}
				}
				if (this.returnItems)
				{
					List<ItemStack> list2 = new List<ItemStack>();
					ItemStack[] array2 = entityTurret.inventory.GetSlots();
					for (int num2 = 0; num2 < array2.Length; num2++)
					{
						if (!array2[num2].IsEmpty())
						{
							list2.Add(array2[num2]);
						}
					}
					array2 = entityTurret.bag.GetSlots();
					for (int num3 = 0; num3 < array2.Length; num3++)
					{
						if (!array2[num3].IsEmpty())
						{
							list2.Add(array2[num3]);
						}
					}
					if (this.includeVehicle == ActionRemoveVehicles.ReturnVehicleTypes.Vehicle)
					{
						list2.Add(new ItemStack(entityTurret.OriginalItemValue, 1));
					}
					if (list2.Count > 0)
					{
						EntityLootContainer entityLootContainer2 = EntityFactory.CreateEntity("DroppedLootContainerTwitch".GetHashCode(), ent.position, Vector3.zero) as EntityLootContainer;
						if (entityLootContainer2 != null)
						{
							entityLootContainer2.SetContent(ItemStack.Clone(list2));
						}
						GameManager.Instance.World.SpawnEntityInWorld(entityLootContainer2);
					}
				}
			}
		}

		// Token: 0x0600C791 RID: 51089 RVA: 0x00496DCE File Offset: 0x00494FCE
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionRemoveVehicles.PropReturnItems, ref this.returnItems);
			properties.ParseBool(ActionRemoveVehicles.PropReturnFuel, ref this.returnFuel);
			properties.ParseEnum<ActionRemoveVehicles.ReturnVehicleTypes>(ActionRemoveVehicles.PropIncludeVehicle, ref this.includeVehicle);
		}

		// Token: 0x0600C792 RID: 51090 RVA: 0x00496E0A File Offset: 0x0049500A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveVehicles
			{
				targetGroup = this.targetGroup,
				returnItems = this.returnItems,
				includeVehicle = this.includeVehicle,
				returnFuel = this.returnFuel
			};
		}

		// Token: 0x04009729 RID: 38697
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool returnItems = true;

		// Token: 0x0400972A RID: 38698
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool returnFuel;

		// Token: 0x0400972B RID: 38699
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionRemoveVehicles.ReturnVehicleTypes includeVehicle;

		// Token: 0x0400972C RID: 38700
		public static string PropReturnItems = "return_items";

		// Token: 0x0400972D RID: 38701
		public static string PropReturnFuel = "return_fuel";

		// Token: 0x0400972E RID: 38702
		public static string PropIncludeVehicle = "return_vehicle_type";

		// Token: 0x02001981 RID: 6529
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ReturnVehicleTypes
		{
			// Token: 0x04009730 RID: 38704
			None,
			// Token: 0x04009731 RID: 38705
			Vehicle,
			// Token: 0x04009732 RID: 38706
			Parts
		}
	}
}
