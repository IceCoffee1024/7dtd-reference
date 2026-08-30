using System;

// Token: 0x0200016A RID: 362
public interface ITileEntityFeature
{
	// Token: 0x170000CE RID: 206
	// (get) Token: 0x06000A64 RID: 2660
	TileEntityFeatureData FeatureData { get; }

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06000A65 RID: 2661
	TileEntityComposite Parent { get; }

	// Token: 0x06000A66 RID: 2662
	void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData);

	// Token: 0x06000A67 RID: 2663
	void CopyFrom(TileEntityComposite _other);

	// Token: 0x06000A68 RID: 2664
	void OnRemove(World _world);

	// Token: 0x06000A69 RID: 2665
	void OnLoad();

	// Token: 0x06000A6A RID: 2666
	void OnUnload(World _world);

	// Token: 0x06000A6B RID: 2667
	void OnDestroy();

	// Token: 0x06000A6C RID: 2668
	void OnAdded(Vector3i _blockPos, BlockValue _blockValue);

	// Token: 0x06000A6D RID: 2669
	void OnBlockValueChanged(Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue);

	// Token: 0x06000A6E RID: 2670
	void OnBlockReset(Vector3i _blockPos, BlockValue _blockValue);

	// Token: 0x06000A6F RID: 2671
	void OnBlockStartsToFall(Vector3i _blockPos, BlockValue _blockValue);

	// Token: 0x06000A70 RID: 2672
	Block.DestroyedResult OnBlockDestroyedBy(Vector3i _blockPos, BlockValue _blockValue, int _entityId, bool _bUseHarvestTool);

	// Token: 0x06000A71 RID: 2673
	Block.DestroyedResult OnBlockDestroyedByExplosion(Vector3i _blockPos, BlockValue _blockValue, int _playerThatStartedExpl);

	// Token: 0x06000A72 RID: 2674
	void SetBlockEntityData(BlockEntityData _blockEntityData);

	// Token: 0x06000A73 RID: 2675
	void UpgradeDowngradeFrom(TileEntityComposite _other);

	// Token: 0x06000A74 RID: 2676
	void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew);

	// Token: 0x06000A75 RID: 2677
	void Reset(FastTags<TagGroup.Global> _questTags);

	// Token: 0x06000A76 RID: 2678
	void UpdateTick(World _world);

	// Token: 0x06000A77 RID: 2679
	string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName);

	// Token: 0x06000A78 RID: 2680
	void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback);

	// Token: 0x06000A79 RID: 2681
	bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing);

	// Token: 0x06000A7A RID: 2682
	bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player);

	// Token: 0x06000A7B RID: 2683
	void Read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode);

	// Token: 0x06000A7C RID: 2684
	void Write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode);
}
