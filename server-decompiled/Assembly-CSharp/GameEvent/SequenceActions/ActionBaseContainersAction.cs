using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019EC RID: 6636
	[Preserve]
	public class ActionBaseContainersAction : BaseAction
	{
		// Token: 0x170018B9 RID: 6329
		// (get) Token: 0x0600CA6D RID: 51821 RVA: 0x004A2F3E File Offset: 0x004A113E
		public string ModifiedName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return base.GetTextWithElements(this.newName);
			}
		}

		// Token: 0x0600CA6E RID: 51822 RVA: 0x004A2F4C File Offset: 0x004A114C
		public override bool CanPerform(Entity target)
		{
			return base.CanPerform(target) && this.GetTileEntityList(target);
		}

		// Token: 0x0600CA6F RID: 51823 RVA: 0x004A2F60 File Offset: 0x004A1160
		public virtual bool CheckValidTileEntity(TileEntity te, out bool isEmpty)
		{
			isEmpty = true;
			return true;
		}

		// Token: 0x0600CA70 RID: 51824 RVA: 0x004A2F68 File Offset: 0x004A1168
		[PublicizedFrom(EAccessModifier.Private)]
		public bool GetTileEntityList(Entity target)
		{
			World world = GameManager.Instance.World;
			Vector3i blockPosition = target.GetBlockPosition();
			int num = World.toChunkXZ(blockPosition.x);
			int num2 = World.toChunkXZ(blockPosition.z);
			int @int = GameStats.GetInt(EnumGameStats.LandClaimSize);
			int num3 = @int / 16 + 1;
			int num4 = @int / 16 + 1;
			this.tileEntityList.Clear();
			bool flag = false;
			for (int i = -num4; i <= num4; i++)
			{
				for (int j = -num3; j <= num3; j++)
				{
					Chunk chunk = (Chunk)world.GetChunkSync(num + j, num2 + i);
					if (chunk != null)
					{
						DictionaryList<Vector3i, TileEntity> tileEntities = chunk.GetTileEntities();
						for (int k = 0; k < tileEntities.list.Count; k++)
						{
							TileEntity tileEntity = tileEntities.list[k];
							if (tileEntity != null)
							{
								bool flag2 = false;
								ActionBaseContainersAction.TargetingTypes targetingType = this.TargetingType;
								if (targetingType != ActionBaseContainersAction.TargetingTypes.SafeZone)
								{
									if (targetingType == ActionBaseContainersAction.TargetingTypes.Distance)
									{
										if (target.GetDistanceSq(tileEntity.ToWorldPos().ToVector3()) < this.maxDistance)
										{
											ITileEntityLootable tileEntityLootable;
											if (tileEntity.TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable) && !tileEntityLootable.bPlayerStorage)
											{
												goto IL_016D;
											}
											flag2 = true;
										}
									}
								}
								else
								{
									ITileEntityLootable tileEntityLootable2;
									if (tileEntity.TryGetSelfOrFeature<ITileEntityLootable>(out tileEntityLootable2) && !tileEntityLootable2.bPlayerStorage)
									{
										goto IL_016D;
									}
									flag2 = world.IsMyLandProtectedBlock(tileEntity.ToWorldPos(), world.gameManager.GetPersistentPlayerList().GetPlayerDataFromEntityID(target.entityId));
								}
								if (flag2)
								{
									bool flag3 = false;
									if (this.CheckValidTileEntity(tileEntity, out flag3))
									{
										this.tileEntityList.Add(tileEntity);
										if (!flag3)
										{
											flag = true;
										}
										if (LockManager.Instance.IsLockedServer(tileEntity, 0))
										{
											LockManager.Instance.ForceUnlockLockTarget(tileEntity);
										}
									}
								}
							}
							IL_016D:;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600CA71 RID: 51825 RVA: 0x004A311C File Offset: 0x004A131C
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			World world = GameManager.Instance.World;
			for (int i = 0; i < this.tileEntityList.Count; i++)
			{
				TileEntity tileEntity = this.tileEntityList[i];
				if (LockManager.Instance.IsLockedServer(tileEntity, 0))
				{
					LockManager.Instance.ForceUnlockLockTarget(tileEntity);
				}
			}
			if (!this.HandleContainerAction(this.tileEntityList))
			{
				return BaseAction.ActionCompleteStates.InCompleteRefund;
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CA72 RID: 51826 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleContainerAction(List<TileEntity> tileEntityList)
		{
			return false;
		}

		// Token: 0x0600CA73 RID: 51827 RVA: 0x004A3184 File Offset: 0x004A1384
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string ParseTextElement(string element)
		{
			if (!(element == "viewer"))
			{
				return element;
			}
			if (base.Owner.ExtraData.Length <= 12)
			{
				return base.Owner.ExtraData;
			}
			return base.Owner.ExtraData.Insert(12, "\n");
		}

		// Token: 0x0600CA74 RID: 51828 RVA: 0x004A31D8 File Offset: 0x004A13D8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Contains(ActionBaseContainersAction.PropNewName))
			{
				this.changeName = true;
				properties.ParseString(ActionBaseContainersAction.PropNewName, ref this.newName);
			}
			properties.ParseEnum<ActionBaseContainersAction.TargetingTypes>(ActionBaseContainersAction.PropTargetingType, ref this.TargetingType);
			properties.ParseFloat(ActionBaseContainersAction.PropMaxDistance, ref this.maxDistance);
			this.maxDistance *= this.maxDistance;
		}

		// Token: 0x0600CA75 RID: 51829 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return null;
		}

		// Token: 0x040099AD RID: 39341
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 5f;

		// Token: 0x040099AE RID: 39342
		[PublicizedFrom(EAccessModifier.Protected)]
		public string newName = "";

		// Token: 0x040099AF RID: 39343
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool changeName;

		// Token: 0x040099B0 RID: 39344
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<TileEntity> tileEntityList = new List<TileEntity>();

		// Token: 0x040099B1 RID: 39345
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseContainersAction.ContainerActionStates ActionState;

		// Token: 0x040099B2 RID: 39346
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseContainersAction.TargetingTypes TargetingType;

		// Token: 0x040099B3 RID: 39347
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x040099B4 RID: 39348
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewName = "new_name";

		// Token: 0x040099B5 RID: 39349
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetingType = "targeting_type";

		// Token: 0x020019ED RID: 6637
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ContainerActionStates
		{
			// Token: 0x040099B7 RID: 39351
			FindContainers,
			// Token: 0x040099B8 RID: 39352
			Action
		}

		// Token: 0x020019EE RID: 6638
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetingTypes
		{
			// Token: 0x040099BA RID: 39354
			SafeZone,
			// Token: 0x040099BB RID: 39355
			Distance
		}
	}
}
