using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A6 RID: 6566
	[Preserve]
	public class ActionRemoveVehicles : ActionRemoveEntities
	{
		// Token: 0x0600C917 RID: 51479 RVA: 0x0049CC74 File Offset: 0x0049AE74
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

		// Token: 0x0600C918 RID: 51480 RVA: 0x0049D112 File Offset: 0x0049B312
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionRemoveVehicles.PropReturnItems, ref this.returnItems);
			properties.ParseBool(ActionRemoveVehicles.PropReturnFuel, ref this.returnFuel);
			properties.ParseEnum<ActionRemoveVehicles.ReturnVehicleTypes>(ActionRemoveVehicles.PropIncludeVehicle, ref this.includeVehicle);
		}

		// Token: 0x0600C919 RID: 51481 RVA: 0x0049D14E File Offset: 0x0049B34E
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

		// Token: 0x04009852 RID: 38994
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool returnItems = true;

		// Token: 0x04009853 RID: 38995
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool returnFuel;

		// Token: 0x04009854 RID: 38996
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionRemoveVehicles.ReturnVehicleTypes includeVehicle;

		// Token: 0x04009855 RID: 38997
		public static string PropReturnItems = "return_items";

		// Token: 0x04009856 RID: 38998
		public static string PropReturnFuel = "return_fuel";

		// Token: 0x04009857 RID: 38999
		public static string PropIncludeVehicle = "return_vehicle_type";

		// Token: 0x020019A7 RID: 6567
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ReturnVehicleTypes
		{
			// Token: 0x04009859 RID: 39001
			None,
			// Token: 0x0400985A RID: 39002
			Vehicle,
			// Token: 0x0400985B RID: 39003
			Parts
		}
	}
}
