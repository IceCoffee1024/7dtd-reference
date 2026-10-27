using System;
using System.Collections.Generic;

// Token: 0x0200062A RID: 1578
public class LootManager
{
	// Token: 0x06003355 RID: 13141 RVA: 0x0014D5E6 File Offset: 0x0014B7E6
	public LootManager(WorldBase _world)
	{
		this.world = _world;
		this.Random = _world.GetGameRandom();
	}

	// Token: 0x06003356 RID: 13142 RVA: 0x0014D604 File Offset: 0x0014B804
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

	// Token: 0x06003357 RID: 13143 RVA: 0x0014D770 File Offset: 0x0014B970
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

	// Token: 0x040028D2 RID: 10450
	public GameRandom Random;

	// Token: 0x040028D3 RID: 10451
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBase world;

	// Token: 0x040028D4 RID: 10452
	public static float[] POITierMod;

	// Token: 0x040028D5 RID: 10453
	public static float[] POITierBonus;
}
