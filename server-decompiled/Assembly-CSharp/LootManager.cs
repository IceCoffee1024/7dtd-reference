using System;
using System.Collections.Generic;

// Token: 0x02000618 RID: 1560
public class LootManager
{
	// Token: 0x060032B1 RID: 12977 RVA: 0x00149F32 File Offset: 0x00148132
	public LootManager(WorldBase _world)
	{
		this.world = _world;
		this.Random = _world.GetGameRandom();
	}

	// Token: 0x060032B2 RID: 12978 RVA: 0x00149F50 File Offset: 0x00148150
	public void LootContainerOpened(ITileEntityLootable _tileEntity, int _entityIdThatOpenedIt, FastTags<TagGroup.Global> _containerTags)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		if (this.world.IsEditor())
		{
			return;
		}
		if (_tileEntity.bTouched)
		{
			return;
		}
		_tileEntity.bTouched = true;
		_tileEntity.worldTimeTouched = this.world.GetWorldTime();
		LootContainer lootContainer = LootContainer.GetLootContainer(_tileEntity.lootListName, true);
		if (lootContainer == null)
		{
			return;
		}
		bool flag = _tileEntity.IsEmpty();
		_tileEntity.bTouched = true;
		_tileEntity.worldTimeTouched = this.world.GetWorldTime();
		if (!flag)
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
		IList<ItemStack> list = lootContainer.Spawn(this.Random, _tileEntity.items.Length, (float)num, 0f, entityPlayer, _containerTags, lootContainer.UniqueItems, lootContainer.IgnoreLootProb, false);
		for (int i = 0; i < list.Count; i++)
		{
			_tileEntity.items[i] = list[i].Clone();
		}
		entityPlayer.FireEvent(MinEventTypes.onSelfLootContainer, true);
	}

	// Token: 0x060032B3 RID: 12979 RVA: 0x0014A0E0 File Offset: 0x001482E0
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
		_bag.Touched = true;
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
		IList<ItemStack> list = lootContainer.Spawn(this.Random, _bag.GetSlots().Length, (float)num, 0f, entityPlayer, _bagOwner.EntityTags, lootContainer.UniqueItems, lootContainer.IgnoreLootProb, false);
		ItemStack[] slots = _bag.GetSlots();
		for (int i = 0; i < list.Count; i++)
		{
			slots[i] = list[i].Clone();
		}
		entityPlayer.FireEvent(MinEventTypes.onSelfLootContainer, true);
	}

	// Token: 0x0400284B RID: 10315
	public GameRandom Random;

	// Token: 0x0400284C RID: 10316
	[PublicizedFrom(EAccessModifier.Private)]
	public WorldBase world;

	// Token: 0x0400284D RID: 10317
	public static float[] POITierMod;

	// Token: 0x0400284E RID: 10318
	public static float[] POITierBonus;
}
