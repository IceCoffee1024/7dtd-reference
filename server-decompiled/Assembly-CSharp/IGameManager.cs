using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011CE RID: 4558
public interface IGameManager
{
	// Token: 0x06009114 RID: 37140
	void SetBlocksRPC(List<BlockChangeInfo> _changes, PlatformUserIdentifierAbs _persistentPlayerId = null);

	// Token: 0x06009115 RID: 37141
	void SetPropsRPC(List<PropChangeInfo> _changes, PlatformUserIdentifierAbs _persistentPlayerId = null);

	// Token: 0x06009116 RID: 37142
	void SimpleRPC(int _entityId, SimpleRPCType _rpcType, bool _bExeLocal, bool _bOnlyLocal);

	// Token: 0x06009117 RID: 37143
	void SpawnBlockParticleEffect(Vector3i _blockPos, ParticleEffect _pe);

	// Token: 0x06009118 RID: 37144
	bool HasBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x06009119 RID: 37145
	Transform GetBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x0600911A RID: 37146
	void RemoveBlockParticleEffect(Vector3i _blockPos);

	// Token: 0x0600911B RID: 37147
	ChunkManager.ChunkObserver AddChunkObserver(Vector3 _initialPosition, bool _bBuildVisualMeshAround, int _viewDim, int _entityIdToSendChunksTo);

	// Token: 0x0600911C RID: 37148
	void RemoveChunkObserver(ChunkManager.ChunkObserver _observer);

	// Token: 0x0600911D RID: 37149
	PersistentPlayerList GetPersistentPlayerList();

	// Token: 0x0600911E RID: 37150
	PersistentPlayerData GetPersistentLocalPlayer();

	// Token: 0x0600911F RID: 37151
	void HandlePersistentPlayerDisconnected(int _entityId);

	// Token: 0x06009120 RID: 37152
	void ItemActionEffectsServer(int _entityId, int _slotIdx, int _itemActionIdx, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0);

	// Token: 0x06009121 RID: 37153
	void ItemActionEffectsClient(int _entityId, int _slotIdx, int _itemActionIdx, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0);

	// Token: 0x06009122 RID: 37154
	void SpawnParticleEffectServer(ParticleEffect _pe, int _entityIdThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false);

	// Token: 0x06009123 RID: 37155
	void SpawnParticleEffectClient(ParticleEffect _pe, int _entityIdThatCausedIt, bool _forceCreation = false, bool _worldSpawn = false);

	// Token: 0x06009124 RID: 37156
	Transform SpawnParticleEffectClientForceCreation(ParticleEffect _pe, int _entityIdThatCausedIt, bool worldSpawn);

	// Token: 0x06009125 RID: 37157
	void ExplosionServer(Vector3 _worldPos, Vector3i _blockPos, Quaternion _rotation, ExplosionData _explosionData, int _playerIdx, float _delay, bool _bRemoveBlockAtExplPosition, ItemValue _itemValueExplosive = null);

	// Token: 0x06009126 RID: 37158
	GameObject ExplosionClient(Vector3 _center, Quaternion _rotation, int _index, int _blastPower, float _blastRadius, float _blockDamage, int _entityId, List<BlockChangeInfo> _explosionChanges);

	// Token: 0x06009127 RID: 37159
	void ItemReloadServer(int _entityId);

	// Token: 0x06009128 RID: 37160
	void ItemReloadClient(int _entityId);

	// Token: 0x06009129 RID: 37161
	void ItemDropServer(ItemStack _itemStack, Vector3 _dropPos, Vector3 _randomPosAdd, int _entityId = -1, float _lifetime = 60f, bool _bDropPosIsRelativeToHead = false);

	// Token: 0x0600912A RID: 37162
	void ItemDropServer(ItemStack _itemStack, Vector3 _dropPos, Vector3 _randomPosAdd, Vector3 _initialMotion, int _entityId = -1, float _lifetime = 60f, bool _bDropPosIsRelativeToHead = false, int _clientInstanceId = 0);

	// Token: 0x0600912B RID: 37163
	void RequestToSpawnEntityServer(EntityCreationData _ecd);

	// Token: 0x0600912C RID: 37164
	void DropContentOfLootContainerServer(BlockValue _bvOld, Vector3i _worldPos, TEFeatureStorage _teOld = null);

	// Token: 0x0600912D RID: 37165
	void DropContentInLootContainerServer(int _droppedByID, string _containerEntity, Vector3 _pos, ItemStack[] items, bool _skipIfEmpty = false, Vector3? increment = null);

	// Token: 0x0600912E RID: 37166
	void PickupBlockServer(Vector3i _blockPos, BlockValue _blockValue, int _playerId, PlatformUserIdentifierAbs persistentPlayerId = null);

	// Token: 0x0600912F RID: 37167
	void PickupBlockClient(Vector3i _blockPos, BlockValue _blockValue, int _playerId);

	// Token: 0x06009130 RID: 37168
	void PlaySoundAtPositionServer(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, float _volumeScale = 1f);

	// Token: 0x06009131 RID: 37169
	void PlaySoundAtPositionServer(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, int _entityId, float _volumeScale = 1f);

	// Token: 0x06009132 RID: 37170
	void PlaySoundAtPositionClient(Vector3 _pos, string _audioClipName, AudioRolloffMode _mode, int _distance, float _volumeScale);

	// Token: 0x06009133 RID: 37171
	void SetWorldTime(ulong _worldTime);

	// Token: 0x06009134 RID: 37172
	void AddVelocityToEntityServer(int _entityId, Vector3 _velToAdd);

	// Token: 0x06009135 RID: 37173
	void AddExpServer(int _entityId, string _skill, int _experience);

	// Token: 0x06009136 RID: 37174
	void AddScoreServer(int _entityId, int _zombieKills, int _playerKills, int _otherTeamnumber, int _conditions);
}
