using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D2 RID: 6610
	[Preserve]
	public class ActionRemoveVehicles : ActionRemoveEntities
	{
		// Token: 0x0600CA6F RID: 51823 RVA: 0x004A1138 File Offset: 0x0049F338
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
						for (int j = 0; j < items[0].itemValue.CosmeticModCount; j++)
						{
							ItemValue cosmeticMod = items[0].itemValue.GetCosmeticMod(j);
							if (cosmeticMod != null && !cosmeticMod.IsEmpty())
							{
								list.Add(new ItemStack(cosmeticMod.Clone(), 1));
							}
						}
						for (int k = 0; k < items[0].itemValue.ModificationCount; k++)
						{
							ItemValue modification = items[0].itemValue.GetModification(k);
							if (modification != null && !modification.IsEmpty())
							{
								list.Add(new ItemStack(modification.Clone(), 1));
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
					for (int m = 0; m < entityVehicle.inventory.Length; m++)
					{
						ItemStack item = entityVehicle.inventory.GetItem(m);
						if (!item.IsEmpty())
						{
							list.Add(item.Clone());
						}
					}
					for (int n = 0; n < entityVehicle.bag.SlotCount; n++)
					{
						ItemStack itemStack2 = entityVehicle.bag.ItemGrid[n];
						if (!itemStack2.IsEmpty())
						{
							list.Add(itemStack2.Clone());
						}
					}
				}
				if (list.Count > 0)
				{
					EntityLootContainer entityLootContainer = EntityFactory.CreateEntity("DroppedLootContainerTwitch".GetHashCode(), ent.position, Vector3.zero) as EntityLootContainer;
					if (entityLootContainer != null)
					{
						entityLootContainer.SetContent(ItemStack.Clone(list), 0);
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
					for (int num2 = 0; num2 < entityTurret.inventory.Length; num2++)
					{
						ItemStack item2 = entityTurret.inventory.GetItem(num2);
						if (!item2.IsEmpty())
						{
							list2.Add(item2.Clone());
						}
					}
					for (int num3 = 0; num3 < entityTurret.bag.SlotCount; num3++)
					{
						ItemStack itemStack3 = entityTurret.bag.ItemGrid[num3];
						if (!itemStack3.IsEmpty())
						{
							list2.Add(itemStack3.Clone());
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
							entityLootContainer2.SetContent(ItemStack.Clone(list2), 0);
						}
						GameManager.Instance.World.SpawnEntityInWorld(entityLootContainer2);
					}
				}
			}
		}

		// Token: 0x0600CA70 RID: 51824 RVA: 0x004A1601 File Offset: 0x0049F801
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionRemoveVehicles.PropReturnItems, ref this.returnItems);
			properties.ParseBool(ActionRemoveVehicles.PropReturnFuel, ref this.returnFuel);
			properties.ParseEnum<ActionRemoveVehicles.ReturnVehicleTypes>(ActionRemoveVehicles.PropIncludeVehicle, ref this.includeVehicle);
		}

		// Token: 0x0600CA71 RID: 51825 RVA: 0x004A163D File Offset: 0x0049F83D
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

		// Token: 0x04009918 RID: 39192
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool returnItems = true;

		// Token: 0x04009919 RID: 39193
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool returnFuel;

		// Token: 0x0400991A RID: 39194
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionRemoveVehicles.ReturnVehicleTypes includeVehicle;

		// Token: 0x0400991B RID: 39195
		public static string PropReturnItems = "return_items";

		// Token: 0x0400991C RID: 39196
		public static string PropReturnFuel = "return_fuel";

		// Token: 0x0400991D RID: 39197
		public static string PropIncludeVehicle = "return_vehicle_type";

		// Token: 0x020019D3 RID: 6611
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ReturnVehicleTypes
		{
			// Token: 0x0400991F RID: 39199
			None,
			// Token: 0x04009920 RID: 39200
			Vehicle,
			// Token: 0x04009921 RID: 39201
			Parts
		}
	}
}
