using System;

// Token: 0x0200016D RID: 365
public interface ITileEntityFeature
{
	// Token: 0x170000CE RID: 206
	// (get) Token: 0x06000A74 RID: 2676
	TileEntityFeatureData FeatureData { get; }

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06000A75 RID: 2677
	TileEntityComposite Parent { get; }

	// Token: 0x06000A76 RID: 2678
	void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData);

	// Token: 0x06000A77 RID: 2679
	void CopyFrom(TileEntityComposite _other);

	// Token: 0x06000A78 RID: 2680
	void OnRemove(World _world);

	// Token: 0x06000A79 RID: 2681
	void OnLoad();

	// Token: 0x06000A7A RID: 2682
	void OnUnload(World _world);

	// Token: 0x06000A7B RID: 2683
	void OnDestroy();

	// Token: 0x06000A7C RID: 2684
	void OnAdded(Vector3i _blockPos, BlockValue _blockValue);

	// Token: 0x06000A7D RID: 2685
	void OnBlockValueChanged(Vector3i _blockPos, BlockValue _oldBlockValue, BlockValue _newBlockValue);

	// Token: 0x06000A7E RID: 2686
	void OnBlockReset(Vector3i _blockPos, BlockValue _blockValue);

	// Token: 0x06000A7F RID: 2687
	void OnBlockStartsToFall(Vector3i _blockPos, BlockValue _blockValue);

	// Token: 0x06000A80 RID: 2688
	Block.DestroyedResult OnBlockDestroyedBy(Vector3i _blockPos, BlockValue _blockValue, int _entityId, bool _bUseHarvestTool);

	// Token: 0x06000A81 RID: 2689
	Block.DestroyedResult OnBlockDestroyedByExplosion(Vector3i _blockPos, BlockValue _blockValue, int _playerThatStartedExpl);

	// Token: 0x06000A82 RID: 2690
	void SetBlockEntityData(BlockEntityData _blockEntityData);

	// Token: 0x06000A83 RID: 2691
	void UpgradeDowngradeFrom(TileEntityComposite _other);

	// Token: 0x06000A84 RID: 2692
	void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew);

	// Token: 0x06000A85 RID: 2693
	void Reset(FastTags<TagGroup.Global> _questTags);

	// Token: 0x06000A86 RID: 2694
	void UpdateTick(World _world);

	// Token: 0x06000A87 RID: 2695
	string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName);

	// Token: 0x06000A88 RID: 2696
	void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback);

	// Token: 0x06000A89 RID: 2697
	bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing);

	// Token: 0x06000A8A RID: 2698
	bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player);

	// Token: 0x06000A8B RID: 2699
	void Read(PooledBinaryReader _br, StreamModeRead _eStreamMode);

	// Token: 0x06000A8C RID: 2700
	void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode);
}
