using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A18 RID: 6680
	[Preserve]
	public class ActionBaseContainersAction : BaseAction
	{
		// Token: 0x1700195B RID: 6491
		// (get) Token: 0x0600CBC5 RID: 52165 RVA: 0x004A73E2 File Offset: 0x004A55E2
		public string ModifiedName
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return base.GetTextWithElements(this.newName);
			}
		}

		// Token: 0x0600CBC6 RID: 52166 RVA: 0x004A73F0 File Offset: 0x004A55F0
		public override bool CanPerform(Entity target)
		{
			return base.CanPerform(target) && this.GetTileEntityList(target);
		}

		// Token: 0x0600CBC7 RID: 52167 RVA: 0x004A7404 File Offset: 0x004A5604
		public virtual bool CheckValidTileEntity(TileEntity te, out bool isEmpty)
		{
			isEmpty = true;
			return true;
		}

		// Token: 0x0600CBC8 RID: 52168 RVA: 0x004A740C File Offset: 0x004A560C
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

		// Token: 0x0600CBC9 RID: 52169 RVA: 0x004A75C8 File Offset: 0x004A57C8
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

		// Token: 0x0600CBCA RID: 52170 RVA: 0x00010E62 File Offset: 0x0000F062
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual bool HandleContainerAction(List<TileEntity> tileEntityList)
		{
			return false;
		}

		// Token: 0x0600CBCB RID: 52171 RVA: 0x004A7630 File Offset: 0x004A5830
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

		// Token: 0x0600CBCC RID: 52172 RVA: 0x004A7684 File Offset: 0x004A5884
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

		// Token: 0x0600CBCD RID: 52173 RVA: 0x000200DA File Offset: 0x0001E2DA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return null;
		}

		// Token: 0x04009A73 RID: 39539
		[PublicizedFrom(EAccessModifier.Protected)]
		public float maxDistance = 5f;

		// Token: 0x04009A74 RID: 39540
		[PublicizedFrom(EAccessModifier.Protected)]
		public string newName = "";

		// Token: 0x04009A75 RID: 39541
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool changeName;

		// Token: 0x04009A76 RID: 39542
		[PublicizedFrom(EAccessModifier.Protected)]
		public List<TileEntity> tileEntityList = new List<TileEntity>();

		// Token: 0x04009A77 RID: 39543
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseContainersAction.ContainerActionStates ActionState;

		// Token: 0x04009A78 RID: 39544
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionBaseContainersAction.TargetingTypes TargetingType;

		// Token: 0x04009A79 RID: 39545
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMaxDistance = "max_distance";

		// Token: 0x04009A7A RID: 39546
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropNewName = "new_name";

		// Token: 0x04009A7B RID: 39547
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetingType = "targeting_type";

		// Token: 0x02001A19 RID: 6681
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum ContainerActionStates
		{
			// Token: 0x04009A7D RID: 39549
			FindContainers,
			// Token: 0x04009A7E RID: 39550
			Action
		}

		// Token: 0x02001A1A RID: 6682
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TargetingTypes
		{
			// Token: 0x04009A80 RID: 39552
			SafeZone,
			// Token: 0x04009A81 RID: 39553
			Distance
		}
	}
}
