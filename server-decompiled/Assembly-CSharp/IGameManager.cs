using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011BE RID: 4542
public interface IGameManager
{
	// Token: 0x060090BE RID: 37054
	void SetBlocksRPC(List<BlockChangeInfo> _changes, PlatformUserIdentifierAbs _persistentPlayerId = null);

	// Token: 0x060090BF RID: 37055
	void SetPropsRPC(List<PropChangeInfo> _changes, PlatformUserIdentifierAbs _persistentPlayerId = null);

	// Token: 0x060090C0 RID: 37056
	void SimpleRPC(int _entityId, SimpleRPCType _rpcType, bool _bExeLocal, bool _bOnlyLocal);

	// Token: 0x060090C1 RID: 37057
	void SpawnBlockParticleEffect(Vector3i _blockPos, ParticleEffect _pe);

	// Token: 0x060090C2 RID: 37058
	bool HasBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x060090C3 RID: 37059
	Transform GetBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x060090C4 RID: 37060
	void RemoveBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x060090C5 RID: 37061
	ChunkManager.ChunkObserver AddChunkObserver(Vector3 _initialPosition, bool _bBuildVisualMeshAround, int _viewDim, int _entityIdToSendChunksTo);

	// Token: 0x060090C6 RID: 37062
	void RemoveChunkObserver(ChunkManager.ChunkObserver _observer);

	// Token: 0x060090C7 RID: 37063
	PersistentPlayerList GetPersistentPlayerList();

	// Token: 0x060090C8 RID: 37064
	PersistentPlayerData GetPersistentLocalPlayer();

	// Token: 0x060090C9 RID: 37065
	void HandlePersistentPlayerDisconnected(int _entityId);

	// Token: 0x060090CA RID: 37066
	void ItemActionEffectsServer(int _entityId, int _slotIdx, int _itemActionIdx, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0);

	// Token: 0x060090CB RID: 37067
	void ItemActionEffectsClient(int _entityId, int _slotIdx, int _itemActionIdx, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0);

	// Token: 0x060090CC RID: 37068
	void SpawnParticleEffectServer(ParticleEffect _pe, int _entityIdThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false);

	// Token: 0x060090CD RID: 37069
	void SpawnParticleEffectClient(ParticleEffect _pe, int _entityIdThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false);

	// Token: 0x060090CE RID: 37070
	Transform SpawnParticleEffectClientForceCreation(ParticleEffect _pe, int _entityIdThatCausedIt, bool worldSpawn);

	// Token: 0x060090CF RID: 37071
	void ExplosionServer(Vector3 _worldPos, Vector3i _blockPos, Quaternion _rotation, ExplosionData _explosionData, int _playerIdx, float _delay, bool _bRemoveBlockAtExplPosition, ItemValue _itemValueExplosive = null);

	// Token: 0x060090D0 RID: 37072
	GameObject ExplosionClient(Vector3 _center, Quaternion _rotation, int _index, int _blastPower, float _blastRadius, float _blockDamage, int _entityId, List<BlockChangeInfo> _explosionChanges);

	// Token: 0x060090D1 RID: 37073
	void ItemReloadServer(int _entityId);

	// Token: 0x060090D2 RID: 37074
	void ItemReloadClient(int _entityId);

	// Token: 0x060090D3 RID: 37075
	void ItemDropServer(ItemStack _itemStack, Vector3 _dropPos, Vector3 _randomPosAdd, int _entityId = -1, float _lifetime = 60f, bool _bDropPosIsRelativeToHead = false);

	// Token: 0x060090D4 RID: 37076
	void ItemDropServer(ItemStack _itemStack, Vector3 _dropPos, Vector3 _randomPosAdd, Vector3 _initialMotion, int _entityId = -1, float _lifetime = 60f, bool _bDropPosIsRelativeToHead = false, int _clientInstanceId = 0);

	// Token: 0x060090D5 RID: 37077
	void RequestToSpawnEntityServer(EntityCreationData _ecd);

	// Token: 0x060090D6 RID: 37078
	void DropContentOfLootContainerServer(BlockValue _bvOld, Vector3i _worldPos, TEFeatureStorage _teOld = null);

	// Token: 0x060090D7 RID: 37079
	void DropContentInLootContainerServer(int _droppedByID, string _containerEntity, Vector3 _pos, ItemStack[] items, bool _skipIfEmpty = false, Vector3? increment = null);

	// Token: 0x060090D8 RID: 37080
	void PickupBlockServer(Vector3i _blockPos, BlockValue _blockValue, int _playerId, PlatformUserIdentifierAbs persistentPlayerId = null);

	// Token: 0x060090D9 RID: 37081
	void PickupBlockClient(Vector3i _blockPos, BlockValue _blockValue, int _playerId);

	// Token: 0x060090DA RID: 37082
	void PlaySoundAtPositionServer(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, float _volumeScale = 1f);

	// Token: 0x060090DB RID: 37083
	void PlaySoundAtPositionServer(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, int _entityId, float _volumeScale = 1f);

	// Token: 0x060090DC RID: 37084
	void PlaySoundAtPositionClient(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, float _volumeScale);

	// Token: 0x060090DD RID: 37085
	void SetWorldTime(ulong _worldTime);

	// Token: 0x060090DE RID: 37086
	void AddVelocityToEntityServer(int _entityId, Vector3 _velToAdd);

	// Token: 0x060090DF RID: 37087
	void AddExpServer(int _entityId, string _skill, int _experience);

	// Token: 0x060090E0 RID: 37088
	void AddScoreServer(int _entityId, int _zombieKills, int _playerKills, int _otherTeamnumber, int _conditions);
}
