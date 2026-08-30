using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011A6 RID: 4518
public interface IGameManager
{
	// Token: 0x06009030 RID: 36912
	void SetBlocksRPC(List<BlockChangeInfo> _changes, PlatformUserIdentifierAbs _persistentPlayerId = null);

	// Token: 0x06009031 RID: 36913
	void SetPropsRPC(List<PropChangeInfo> _changes, PlatformUserIdentifierAbs _persistentPlayerId = null);

	// Token: 0x06009032 RID: 36914
	void SimpleRPC(int _entityId, SimpleRPCType _rpcType, bool _bExeLocal, bool _bOnlyLocal);

	// Token: 0x06009033 RID: 36915
	void SpawnBlockParticleEffect(Vector3i _blockPos, ParticleEffect _pe);

	// Token: 0x06009034 RID: 36916
	bool HasBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x06009035 RID: 36917
	Transform GetBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x06009036 RID: 36918
	void RemoveBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x06009037 RID: 36919
	ChunkManager.ChunkObserver AddChunkObserver(Vector3 _initialPosition, bool _bBuildVisualMeshAround, int _viewDim, int _entityIdToSendChunksTo);

	// Token: 0x06009038 RID: 36920
	void RemoveChunkObserver(ChunkManager.ChunkObserver _observer);

	// Token: 0x06009039 RID: 36921
	PersistentPlayerList GetPersistentPlayerList();

	// Token: 0x0600903A RID: 36922
	PersistentPlayerData GetPersistentLocalPlayer();

	// Token: 0x0600903B RID: 36923
	void HandlePersistentPlayerDisconnected(int _entityId);

	// Token: 0x0600903C RID: 36924
	void ItemActionEffectsServer(int _entityId, int _slotIdx, int _itemActionIdx, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0);

	// Token: 0x0600903D RID: 36925
	void ItemActionEffectsClient(int _entityId, int _slotIdx, int _itemActionIdx, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0);

	// Token: 0x0600903E RID: 36926
	void SpawnParticleEffectServer(ParticleEffect _pe, int _entityIdThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false);

	// Token: 0x0600903F RID: 36927
	void SpawnParticleEffectClient(ParticleEffect _pe, int _entityIdThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false);

	// Token: 0x06009040 RID: 36928
	Transform SpawnParticleEffectClientForceCreation(ParticleEffect _pe, int _entityIdThatCausedIt, bool worldSpawn);

	// Token: 0x06009041 RID: 36929
	void ExplosionServer(Vector3 _worldPos, Vector3i _blockPos, Quaternion _rotation, ExplosionData _explosionData, int _playerIdx, float _delay, bool _bRemoveBlockAtExplPosition, ItemValue _itemValueExplosive = null);

	// Token: 0x06009042 RID: 36930
	GameObject ExplosionClient(Vector3 _center, Quaternion _rotation, int _index, int _blastPower, float _blastRadius, float _blockDamage, int _entityId, List<BlockChangeInfo> _explosionChanges);

	// Token: 0x06009043 RID: 36931
	void ItemReloadServer(int _entityId);

	// Token: 0x06009044 RID: 36932
	void ItemReloadClient(int _entityId);

	// Token: 0x06009045 RID: 36933
	void ItemDropServer(ItemStack _itemStack, Vector3 _dropPos, Vector3 _randomPosAdd, int _entityId = -1, float _lifetime = 60f, bool _bDropPosIsRelativeToHead = false);

	// Token: 0x06009046 RID: 36934
	void ItemDropServer(ItemStack _itemStack, Vector3 _dropPos, Vector3 _randomPosAdd, Vector3 _initialMotion, int _entityId = -1, float _lifetime = 60f, bool _bDropPosIsRelativeToHead = false, int _clientInstanceId = 0);

	// Token: 0x06009047 RID: 36935
	void RequestToSpawnEntityServer(EntityCreationData _ecd);

	// Token: 0x06009048 RID: 36936
	void DropContentOfLootContainerServer(BlockValue _bvOld, Vector3i _worldPos, ITileEntityLootable _teOld = null);

	// Token: 0x06009049 RID: 36937
	void DropContentInLootContainerServer(int _droppedByID, string _containerEntity, Vector3 _pos, ItemStack[] items, bool _skipIfEmpty = false, Vector3? increment = null);

	// Token: 0x0600904A RID: 36938
	void PickupBlockServer(Vector3i _blockPos, BlockValue _blockValue, int _playerId, PlatformUserIdentifierAbs persistentPlayerId = null);

	// Token: 0x0600904B RID: 36939
	void PickupBlockClient(Vector3i _blockPos, BlockValue _blockValue, int _playerId);

	// Token: 0x0600904C RID: 36940
	void PlaySoundAtPositionServer(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, float _volumeScale = 1f);

	// Token: 0x0600904D RID: 36941
	void PlaySoundAtPositionServer(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, int _entityId, float _volumeScale = 1f);

	// Token: 0x0600904E RID: 36942
	void PlaySoundAtPositionClient(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, float _volumeScale);

	// Token: 0x0600904F RID: 36943
	void SetWorldTime(ulong _worldTime);

	// Token: 0x06009050 RID: 36944
	void AddVelocityToEntityServer(int _entityId, Vector3 _velToAdd);

	// Token: 0x06009051 RID: 36945
	void AddExpServer(int _entityId, string _skill, int _experience);

	// Token: 0x06009052 RID: 36946
	void AddScoreServer(int _entityId, int _zombieKills, int _playerKills, int _otherTeamnumber, int _conditions);
}
