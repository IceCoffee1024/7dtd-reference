using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CB RID: 6603
	[Preserve]
	public class ActionShuffleContainers : ActionBaseContainersAction
	{
		// Token: 0x0600C8FF RID: 51455 RVA: 0x0049D844 File Offset: 0x0049BA44
		public override bool CheckValidTileEntity(TileEntity te, out bool isEmpty)
		{
			isEmpty = false;
			TileEntityType tileEntityType = te.GetTileEntityType();
			if (tileEntityType != TileEntityType.Workstation)
			{
				ITileEntityLootable tileEntityLootable;
				if (tileEntityType == TileEntityType.Composite && te.TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable))
				{
					isEmpty = tileEntityLootable.IsEmpty();
					return true;
				}
			}
			else if (this.includeOutputs)
			{
				TileEntityWorkstation tileEntityWorkstation = te as TileEntityWorkstation;
				if (tileEntityWorkstation != null)
				{
					isEmpty = tileEntityWorkstation.OutputEmpty();
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C900 RID: 51456 RVA: 0x0049D898 File Offset: 0x0049BA98
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleContainerAction(List<TileEntity> tileEntityList)
		{
			List<ItemStack> list = new List<ItemStack>();
			List<TileEntity> list2 = new List<TileEntity>();
			bool flag = false;
			for (int i = 0; i < tileEntityList.Count; i++)
			{
				TileEntityType tileEntityType = tileEntityList[i].GetTileEntityType();
				if (tileEntityType != TileEntityType.Workstation)
				{
					ITileEntityLootable tileEntityLootable;
					if (tileEntityType == TileEntityType.Composite && tileEntityList[i].TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable))
					{
						list2.Add(tileEntityList[i]);
						list.AddRange(tileEntityLootable.items);
						if (!tileEntityLootable.IsEmpty())
						{
							flag = true;
						}
					}
				}
				else if (this.includeOutputs)
				{
					TileEntityWorkstation tileEntityWorkstation = tileEntityList[i] as TileEntityWorkstation;
					if (tileEntityWorkstation != null)
					{
						list2.Add(tileEntityWorkstation);
						list.AddRange(tileEntityWorkstation.Output);
						if (!tileEntityWorkstation.OutputEmpty())
						{
							flag = true;
						}
					}
				}
			}
			if (flag && this.changeName && base.Owner.Target != null)
			{
				PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(base.Owner.Target.entityId);
				for (int j = 0; j < tileEntityList.Count; j++)
				{
					ITileEntitySignable tileEntitySignable;
					if (tileEntityList[j].TryGetSelfOrFeature<ITileEntitySignable>(out tileEntitySignable))
					{
						tileEntitySignable.SetText(base.ModifiedName, true, (playerDataFromEntityID != null) ? playerDataFromEntityID.PrimaryId : null);
					}
				}
			}
			GameRandom random = GameEventManager.Current.Random;
			if (list2.Count > 0)
			{
				for (int k = 0; k < list.Count * 2; k++)
				{
					int num = random.RandomRange(list.Count);
					int num2 = random.RandomRange(list.Count);
					ItemStack itemStack = list[num];
					list[num] = list[num2];
					list[num2] = itemStack;
				}
				for (int l = 0; l < list2.Count; l++)
				{
					TileEntityType tileEntityType2 = list2[l].GetTileEntityType();
					if (tileEntityType2 != TileEntityType.Workstation)
					{
						ITileEntityLootable tileEntityLootable2;
						if (tileEntityType2 == TileEntityType.Composite && list2[l].TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable2))
						{
							for (int m = 0; m < tileEntityLootable2.items.Length; m++)
							{
								tileEntityLootable2.items[m] = list[0];
								list.RemoveAt(0);
							}
						}
					}
					else if (this.includeOutputs)
					{
						TileEntityWorkstation tileEntityWorkstation2 = list2[l] as TileEntityWorkstation;
						if (tileEntityWorkstation2 != null)
						{
							ItemStack[] output = tileEntityWorkstation2.Output;
							for (int n = 0; n < output.Length; n++)
							{
								output[n] = list[0];
								list.RemoveAt(0);
							}
							tileEntityWorkstation2.Output = output;
						}
					}
					list2[l].SetModified();
				}
			}
			return flag;
		}

		// Token: 0x0600C901 RID: 51457 RVA: 0x0049DB2C File Offset: 0x0049BD2C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionShuffleContainers.PropIncludeOutputs, ref this.includeOutputs);
		}

		// Token: 0x0600C902 RID: 51458 RVA: 0x0049DB48 File Offset: 0x0049BD48
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionShuffleContainers
			{
				TargetingType = this.TargetingType,
				maxDistance = this.maxDistance,
				newName = this.newName,
				changeName = this.changeName,
				includeOutputs = this.includeOutputs,
				tileEntityList = this.tileEntityList
			};
		}

		// Token: 0x0400989A RID: 39066
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool includeOutputs;

		// Token: 0x0400989B RID: 39067
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIncludeOutputs = "include_outputs";
	}
}
