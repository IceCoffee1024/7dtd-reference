using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A49 RID: 6729
	[Preserve]
	public class ActionBaseContainersAction : BaseAction
	{
		// Token: 0x170019A3 RID: 6563
		// (get) Token: 0x0600CCD9 RID: 52441 RVA: 0x004AAEDE File Offset: 0x004A90DE
		public string ModifiedName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return base.GetTextWithElements(this.newName);
			}
		}

		// Token: 0x0600CCDA RID: 52442 RVA: 0x004AAEEC File Offset: 0x004A90EC
		public override bool CanPerform(Entity target)
		{
			return base.CanPerform(target) && this.GetTileEntityList(target);
		}

		// Token: 0x0600CCDB RID: 52443 RVA: 0x004AAF00 File Offset: 0x004A9100
		public virtual bool CheckValidTileEntity(TileEntity te, out bool isEmpty)
		{
			isEmpty = true;
			return true;
		}

		// Token: 0x0600CCDC RID: 52444 RVA: 0x004AAF08 File Offset: 0x004A9108
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
											TEFeatureStorage tefeatureStorage;
											if (tileEntity.TryGetSelfOrFeature<TEFeatureStorage>(out tefeatureStorage) && !tefeatureStorage.ItemGrid.PlayerOwned)
											{
												goto IL_0177;
											}
											flag2 = true;
										}
									}
								}
								else
								{
									TEFeatureStorage tefeatureStorage2;
									if (tileEntity.TryGetSelfOrFeature<TEFeatureStorage>(out tefeatureStorage2) && !tefeatureStorage2.ItemGrid.PlayerOwned)
									{
										goto IL_0177;
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
							IL_0177:;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600CCDD RID: 52445 RVA: 0x004AB0C4 File Offset: 0x004A92C4
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

		// Token: 0x0600CCDE RID: 52446 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleContainerAction(List<TileEntity> tileEntityList)
		{
			return false;
		}

		// Token: 0x0600CCDF RID: 52447 RVA: 0x004AB12C File Offset: 0x004A932C
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

		// Token: 0x0600CCE0 RID: 52448 RVA: 0x004AB180 File Offset: 0x004A9380
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

		// Token: 0x0600CCE1 RID: 52449 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return null;
		}

		// Token: 0x04009B5C RID: 39772
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 5f;

		// Token: 0x04009B5D RID: 39773
		[PublicizedFrom(EAccessModifier.Protected)]
		public string newName = "";

		// Token: 0x04009B5E RID: 39774
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool changeName;

		// Token: 0x04009B5F RID: 39775
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<TileEntity> tileEntityList = new List<TileEntity>();

		// Token: 0x04009B60 RID: 39776
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseContainersAction.ContainerActionStates ActionState;

		// Token: 0x04009B61 RID: 39777
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseContainersAction.TargetingTypes TargetingType;

		// Token: 0x04009B62 RID: 39778
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009B63 RID: 39779
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewName = "new_name";

		// Token: 0x04009B64 RID: 39780
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetingType = "targeting_type";

		// Token: 0x02001A4A RID: 6730
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ContainerActionStates
		{
			// Token: 0x04009B66 RID: 39782
			FindContainers,
			// Token: 0x04009B67 RID: 39783
			Action
		}

		// Token: 0x02001A4B RID: 6731
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetingTypes
		{
			// Token: 0x04009B69 RID: 39785
			SafeZone,
			// Token: 0x04009B6A RID: 39786
			Distance
		}
	}
}
