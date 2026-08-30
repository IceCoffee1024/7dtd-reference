using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C5 RID: 6597
	[Preserve]
	public class ActionBaseContainersAction : BaseAction
	{
		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x0600C8E4 RID: 51428 RVA: 0x0049CB4A File Offset: 0x0049AD4A
		public string ModifiedName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return base.GetTextWithElements(this.newName);
			}
		}

		// Token: 0x0600C8E5 RID: 51429 RVA: 0x0049CB58 File Offset: 0x0049AD58
		public override bool CanPerform(Entity target)
		{
			return base.CanPerform(target) && this.GetTileEntityList(target);
		}

		// Token: 0x0600C8E6 RID: 51430 RVA: 0x0049CB6C File Offset: 0x0049AD6C
		public virtual bool CheckValidTileEntity(TileEntity te, out bool isEmpty)
		{
			isEmpty = true;
			return true;
		}

		// Token: 0x0600C8E7 RID: 51431 RVA: 0x0049CB74 File Offset: 0x0049AD74
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

		// Token: 0x0600C8E8 RID: 51432 RVA: 0x0049CD28 File Offset: 0x0049AF28
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

		// Token: 0x0600C8E9 RID: 51433 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleContainerAction(List<TileEntity> tileEntityList)
		{
			return false;
		}

		// Token: 0x0600C8EA RID: 51434 RVA: 0x0049CD90 File Offset: 0x0049AF90
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

		// Token: 0x0600C8EB RID: 51435 RVA: 0x0049CDE4 File Offset: 0x0049AFE4
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

		// Token: 0x0600C8EC RID: 51436 RVA: 0x0001FF1E File Offset: 0x0001E11E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return null;
		}

		// Token: 0x0400987C RID: 39036
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 5f;

		// Token: 0x0400987D RID: 39037
		[PublicizedFrom(EAccessModifier.Protected)]
		public string newName = "";

		// Token: 0x0400987E RID: 39038
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool changeName;

		// Token: 0x0400987F RID: 39039
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<TileEntity> tileEntityList = new List<TileEntity>();

		// Token: 0x04009880 RID: 39040
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseContainersAction.ContainerActionStates ActionState;

		// Token: 0x04009881 RID: 39041
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseContainersAction.TargetingTypes TargetingType;

		// Token: 0x04009882 RID: 39042
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009883 RID: 39043
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewName = "new_name";

		// Token: 0x04009884 RID: 39044
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetingType = "targeting_type";

		// Token: 0x020019C6 RID: 6598
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ContainerActionStates
		{
			// Token: 0x04009886 RID: 39046
			FindContainers,
			// Token: 0x04009887 RID: 39047
			Action
		}

		// Token: 0x020019C7 RID: 6599
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetingTypes
		{
			// Token: 0x04009889 RID: 39049
			SafeZone,
			// Token: 0x0400988A RID: 39050
			Distance
		}
	}
}
