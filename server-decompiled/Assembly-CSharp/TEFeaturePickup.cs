using System;
using UnityEngine.Scripting;

// Token: 0x02000180 RID: 384
[Preserve]
public class TEFeaturePickup : TEFeatureAbs, IFeatureSavedInPrefab
{
	// Token: 0x06000B71 RID: 2929 RVA: 0x00048DA2 File Offset: 0x00046FA2
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		_featureData.Props.ParseFloat(TEFeaturePickup.PropTakeDelay, ref this.TakeDelay);
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x00048695 File Offset: 0x00046895
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x00048DC2 File Offset: 0x00046FC2
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("Take", "hand", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x00048DEC File Offset: 0x00046FEC
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		return base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing) && (!this.Equals(_module) || (_world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer()) && base.Parent.PlayerPlaced && this.TakeDelay > 0f));
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x00048E48 File Offset: 0x00047048
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

	// Token: 0x06000B77 RID: 2935 RVA: 0x00048E84 File Offset: 0x00047084
	public override void Read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		int num;
		if (_eStreamMode != TileEntity.StreamModeRead.Persistency)
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

	// Token: 0x06000B78 RID: 2936 RVA: 0x00048EB2 File Offset: 0x000470B2
	public override void Write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(2);
		}
	}

	// Token: 0x04000A8F RID: 2703
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 2;

	// Token: 0x04000A90 RID: 2704
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropTakeDelay = "TakeDelay";

	// Token: 0x04000A91 RID: 2705
	[PublicizedFrom(EAccessModifier.Protected)]
	public float TakeDelay = 2f;
}
