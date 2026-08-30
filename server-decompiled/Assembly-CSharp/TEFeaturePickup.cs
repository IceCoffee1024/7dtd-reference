using System;
using UnityEngine.Scripting;

// Token: 0x02000182 RID: 386
[Preserve]
public class TEFeaturePickup : TEFeatureAbs, IFeatureSavedInPrefab
{
	// Token: 0x06000B7C RID: 2940 RVA: 0x000491BA File Offset: 0x000473BA
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		_featureData.Props.ParseFloat(TEFeaturePickup.PropTakeDelay, ref this.TakeDelay);
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x00048AAD File Offset: 0x00046CAD
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x000491DA File Offset: 0x000473DA
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("Take", "hand", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x00049204 File Offset: 0x00047404
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		return base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing) && (!this.Equals(_module) || (_world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer()) && base.Parent.PlayerPlaced && this.TakeDelay > 0f));
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x00049260 File Offset: 0x00047460
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

	// Token: 0x06000B82 RID: 2946 RVA: 0x0004929C File Offset: 0x0004749C
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

	// Token: 0x06000B83 RID: 2947 RVA: 0x000492CA File Offset: 0x000474CA
	public override void Write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(2);
		}
	}

	// Token: 0x04000A9C RID: 2716
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 2;

	// Token: 0x04000A9D RID: 2717
	[PublicizedFrom(EAccessModifier.Private)]
	public static string PropTakeDelay = "TakeDelay";

	// Token: 0x04000A9E RID: 2718
	[PublicizedFrom(EAccessModifier.Protected)]
	public float TakeDelay = 2f;
}
