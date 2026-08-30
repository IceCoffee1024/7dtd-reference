using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200118C RID: 4492
public interface IGameManager
{
	// Token: 0x06008F36 RID: 36662
	void SetBlocksRPC(List<BlockChangeInfo> _changes, PlatformUserIdentifierAbs _persistentPlayerId = null);

	// Token: 0x06008F37 RID: 36663
	void SetPropsRPC(List<PropChangeInfo> _changes, PlatformUserIdentifierAbs _persistentPlayerId = null);

	// Token: 0x06008F38 RID: 36664
	void SimpleRPC(int _entityId, SimpleRPCType _rpcType, bool _bExeLocal, bool _bOnlyLocal);

	// Token: 0x06008F39 RID: 36665
	void SpawnBlockParticleEffect(Vector3i _blockPos, ParticleEffect _pe);

	// Token: 0x06008F3A RID: 36666
	bool HasBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x06008F3B RID: 36667
	Transform GetBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x06008F3C RID: 36668
	void RemoveBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x06008F3D RID: 36669
	ChunkManager.ChunkObserver AddChunkObserver(Vector3 _initialPosition, bool _bBuildVisualMeshAround, int _viewDim, int _entityIdToSendChunksTo);

	// Token: 0x06008F3E RID: 36670
	void RemoveChunkObserver(ChunkManager.ChunkObserver _observer);

	// Token: 0x06008F3F RID: 36671
	PersistentPlayerList GetPersistentPlayerList();

	// Token: 0x06008F40 RID: 36672
	PersistentPlayerData GetPersistentLocalPlayer();

	// Token: 0x06008F41 RID: 36673
	void HandlePersistentPlayerDisconnected(int _entityId);

	// Token: 0x06008F42 RID: 36674
	void ItemActionEffectsServer(int _entityId, int _slotIdx, int _itemActionIdx, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0);

	// Token: 0x06008F43 RID: 36675
	void ItemActionEffectsClient(int _entityId, int _slotIdx, int _itemActionIdx, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0);

	// Token: 0x06008F44 RID: 36676
	void SpawnParticleEffectServer(ParticleEffect _pe, int _entityIdThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false);

	// Token: 0x06008F45 RID: 36677
	void SpawnParticleEffectClient(ParticleEffect _pe, int _entityIdThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false);

	// Token: 0x06008F46 RID: 36678
	Transform SpawnParticleEffectClientForceCreation(ParticleEffect _pe, int _entityIdThatCausedIt, bool worldSpawn);

	// Token: 0x06008F47 RID: 36679
	void ExplosionServer(Vector3 _worldPos, Vector3i _blockPos, Quaternion _rotation, ExplosionData _explosionData, int _playerIdx, float _delay, bool _bRemoveBlockAtExplPosition, ItemValue _itemValueExplosive = null);

	// Token: 0x06008F48 RID: 36680
	GameObject ExplosionClient(Vector3 _center, Quaternion _rotation, int _index, int _blastPower, float _blastRadius, float _blockDamage, int _entityId, List<BlockChangeInfo> _explosionChanges);

	// Token: 0x06008F49 RID: 36681
	void ItemReloadServer(int _entityId);

	// Token: 0x06008F4A RID: 36682
	void ItemReloadClient(int _entityId);

	// Token: 0x06008F4B RID: 36683
	void ItemDropServer(ItemStack _itemStack, Vector3 _dropPos, Vector3 _randomPosAdd, int _entityId = -1, float _lifetime = 60f, bool _bDropPosIsRelativeToHead = false);

	// Token: 0x06008F4C RID: 36684
	void ItemDropServer(ItemStack _itemStack, Vector3 _dropPos, Vector3 _randomPosAdd, Vector3 _initialMotion, int _entityId = -1, float _lifetime = 60f, bool _bDropPosIsRelativeToHead = false, int _clientInstanceId = 0);

	// Token: 0x06008F4D RID: 36685
	void RequestToSpawnEntityServer(EntityCreationData _ecd);

	// Token: 0x06008F4E RID: 36686
	void DropContentOfLootContainerServer(BlockValue _bvOld, Vector3i _worldPos, ITileEntityLootable _teOld = null);

	// Token: 0x06008F4F RID: 36687
	void DropContentInLootContainerServer(int _droppedByID, string _containerEntity, Vector3 _pos, ItemStack[] items, bool _skipIfEmpty = false, Vector3? increment = null);

	// Token: 0x06008F50 RID: 36688
	void PickupBlockServer(Vector3i _blockPos, BlockValue _blockValue, int _playerId, PlatformUserIdentifierAbs persistentPlayerId = null);

	// Token: 0x06008F51 RID: 36689
	void PickupBlockClient(Vector3i _blockPos, BlockValue _blockValue, int _playerId);

	// Token: 0x06008F52 RID: 36690
	void PlaySoundAtPositionServer(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, float _volumeScale = 1f);

	// Token: 0x06008F53 RID: 36691
	void PlaySoundAtPositionServer(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, int _entityId, float _volumeScale = 1f);

	// Token: 0x06008F54 RID: 36692
	void PlaySoundAtPositionClient(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, float _volumeScale);

	// Token: 0x06008F55 RID: 36693
	void SetWorldTime(ulong _worldTime);

	// Token: 0x06008F56 RID: 36694
	void AddVelocityToEntityServer(int _entityId, Vector3 _velToAdd);

	// Token: 0x06008F57 RID: 36695
	void AddExpServer(int _entityId, string _skill, int _experience);

	// Token: 0x06008F58 RID: 36696
	void AddScoreServer(int _entityId, int _zombieKills, int _playerKills, int _otherTeamnumber, int _conditions);
}
