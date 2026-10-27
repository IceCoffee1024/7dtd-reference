using System;
using UnityEngine.Scripting;

// Token: 0x02000183 RID: 387
[Preserve]
public class TEFeaturePickup : TEFeatureAbs, IFeatureSavedInPrefab
{
	// Token: 0x06000B84 RID: 2948 RVA: 0x00049342 File Offset: 0x00047542
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		_featureData.Props.ParseFloat(TEFeaturePickup.PropTakeDelay, ref this.TakeDelay);
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00049362 File Offset: 0x00047562
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("Take", "hand", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0004938C File Offset: 0x0004758C
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		return base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing) && (!this.Equals(_module) || (_world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer()) && base.Parent.PlayerPlaced && this.TakeDelay > 0f));
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x000493E8 File Offset: 0x000475E8
	public override bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		base.OnBlockActivated(_commandName, _world, _blockPos, _blockValue, _player);
		if (base.CommandIs(_commandName, "Take"))
		{
			_player.AimingGun = false;
			Block.TakeItemWithTimer(_blockPos, _blockValue, _player, this.TakeDelay, null);
			return true;
		}
		return false;
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x00049424 File Offset: 0x00047624
	public override void Read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		int num;
		if (_eStreamMode != StreamModeRead.Persistency)
		{
			num = 2;
		}
		else
		{
			num = (int)_br.ReadUInt16();
		}
		if (num < 2)
		{
			_br.ReadBoolean();
		}
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x00049452 File Offset: 0x00047652
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(2);
		}
	}

	// Token: 0x04000A9B RID: 2715
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 2;

	// Token: 0x04000A9C RID: 2716
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropTakeDelay = "TakeDelay";

	// Token: 0x04000A9D RID: 2717
	[PublicizedFrom(EAccessModifier.Protected)]
	public float TakeDelay = 2f;
}
