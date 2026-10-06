using System;
using System.Collections.Generic;

// Token: 0x02000623 RID: 1571
public class LootManager
{
	// Token: 0x06003347 RID: 13127 RVA: 0x0014C9F2 File Offset: 0x0014ABF2
	public LootManager(WorldBase _world)
	{
		this.world = _world;
		this.Random = _world.GetGameRandom();
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x0014CA10 File Offset: 0x0014AC10
	public void LootContainerOpened(TEFeatureStorage _tileEntity, int _entityIdThatOpenedIt, FastTags<TagGroup.Global> _containerTags)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		if (this.world.IsEditor())
		{
			return;
		}
		if (_tileEntity.ItemGrid.Touched)
		{
			return;
		}
		LootContainer lootContainer = LootContainer.GetLootContainer(_tileEntity.lootListName, true);
		if (lootContainer == null)
		{
			return;
		}
		if (!_tileEntity.IsEmpty())
		{
			return;
		}
		EntityPlayer entityPlayer = (EntityPlayer)this.world.GetEntity(_entityIdThatOpenedIt);
		if (entityPlayer == null)
		{
			return;
		}
		entityPlayer.MinEventContext.TileEntity = _tileEntity;
		entityPlayer.MinEventContext.BlockValue = _tileEntity.blockValue;
		if (entityPlayer.isEntityRemote)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageMinEventFire>().Setup(entityPlayer.entityId, -1, MinEventTypes.onSelfOpenLootContainer, _tileEntity.blockValue), false, entityPlayer.entityId, -1, -1, null, 192, false);
		}
		else
		{
			entityPlayer.FireEvent(MinEventTypes.onSelfOpenLootContainer, true);
		}
		float lootStageMod = _tileEntity.LootStageMod;
		float lootStageBonus = _tileEntity.LootStageBonus;
		int num = (lootContainer.useUnmodifiedLootstage ? entityPlayer.unModifiedGameStage : entityPlayer.GetHighestPartyLootStage(lootStageMod, lootStageBonus));
		IList<ItemStack> list = lootContainer.Spawn(this.Random, _tileEntity.ItemGrid.Length, (float)num, 0f, entityPlayer, _containerTags, lootContainer.UniqueItems, lootContainer.IgnoreLootProb, false);
		for (int i = 0; i < list.Count; i++)
		{
			_tileEntity.ItemGrid[i] = list[i].Clone();
		}
		entityPlayer.FireEvent(MinEventTypes.onSelfLootContainer, true);
	}

	// Token: 0x06003349 RID: 13129 RVA: 0x0014CB7C File Offset: 0x0014AD7C
	public void LootBagOpened(Bag _bag, Entity _bagOwner, int _entityIdThatOpenedIt)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		if (this.world.IsEditor())
		{
			return;
		}
		if (_bag == null || _bagOwner == null)
		{
			return;
		}
		if (_bag.Touched)
		{
			return;
		}
		LootContainer lootContainer = LootContainer.GetLootContainer(_bagOwner.GetLootList(), true);
		if (lootContainer == null)
		{
			return;
		}
		if (!_bag.IsEmpty())
		{
			return;
		}
		EntityPlayer entityPlayer = (EntityPlayer)this.world.GetEntity(_entityIdThatOpenedIt);
		if (entityPlayer == null)
		{
			return;
		}
		if (entityPlayer.isEntityRemote)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageMinEventFire>().Setup(entityPlayer.entityId, -1, MinEventTypes.onSelfOpenLootContainer, BlockValue.Air), false, entityPlayer.entityId, -1, -1, null, 192, false);
		}
		else
		{
			entityPlayer.FireEvent(MinEventTypes.onSelfOpenLootContainer, true);
		}
		int num = (lootContainer.useUnmodifiedLootstage ? entityPlayer.unModifiedGameStage : entityPlayer.GetHighestPartyLootStage(0f, 0f));
		IList<ItemStack> list = lootContainer.Spawn(this.Random, _bag.SlotCount, (float)num, 0f, entityPlayer, _bagOwner.EntityTags, lootContainer.UniqueItems, lootContainer.IgnoreLootProb, false);
		for (int i = 0; i < list.Count; i++)
		{
			_bag.ItemGrid[i] = list[i];
		}
		entityPlayer.FireEvent(MinEventTypes.onSelfLootContainer, true);
	}

	// Token: 0x040028B7 RID: 10423
	public GameRandom Random;

	// Token: 0x040028B8 RID: 10424
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBase world;

	// Token: 0x040028B9 RID: 10425
	public static float[] POITierMod;

	// Token: 0x040028BA RID: 10426
	public static float[] POITierBonus;
}
