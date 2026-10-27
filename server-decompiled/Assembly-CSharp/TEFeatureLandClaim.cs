using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200017E RID: 382
[Preserve]
public class TEFeatureLandClaim : TEFeatureAbs
{
	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00047DB0 File Offset: 0x00045FB0
	// (set) Token: 0x06000B37 RID: 2871 RVA: 0x00047DB8 File Offset: 0x00045FB8
	public bool ShowBounds
	{
		get
		{
			return this.showBounds;
		}
		set
		{
			this.showBounds = value;
			base.SetModified();
		}
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x00047DC8 File Offset: 0x00045FC8
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		this.lockFeature = base.Parent.GetFeature<TEFeatureLockable>();
		this.lockpickFeature = base.Parent.GetFeature<TEFeatureLockPickable>();
		this.activePrompt = Localization.Get("activeBlockPrompt", false, null);
		this.inactivePrompt = Localization.Get("inactiveBlockPrompt", false, null);
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x00047E24 File Offset: 0x00046024
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
		TEFeatureLandClaim tefeatureLandClaim;
		if (_other.TryGetSelfOrFeature<TEFeatureLandClaim>(out tefeatureLandClaim))
		{
			this.showBounds = tefeatureLandClaim.showBounds;
		}
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x00047E48 File Offset: 0x00046048
	public override string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		base.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
		if (this.lockpickFeature != null && this.lockpickFeature.NeedsLockpicking())
		{
			return string.Format(Localization.Get("tooltipLocked", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		if (this.lockFeature != null && this.lockFeature.IsLocked() && !this.lockFeature.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier))
		{
			return string.Format(Localization.Get("tooltipJammed", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		if (this.IsPrimary())
		{
			string text = (base.Parent.LocalPlayerIsOwner ? ("\n" + Localization.Get("useWorkstation", false, null)) : "");
			return string.Format(this.activePrompt, _blockValue.Block.GetLocalizedBlockName()) + text;
		}
		return string.Format(this.inactivePrompt, _blockValue.Block.GetLocalizedBlockName());
	}

	// Token: 0x06000B3B RID: 2875 RVA: 0x00047F38 File Offset: 0x00046138
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("show_bounds", "frames", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
		_addCallback(new BlockActivationCommand("hide_bounds", "frames", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
		_addCallback(new BlockActivationCommand("remove", "x", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x00047FAC File Offset: 0x000461AC
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		if (!base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing))
		{
			return false;
		}
		bool flag = base.Parent.LocalPlayerIsOwner && this.IsPrimary();
		if (!this.Equals(_module))
		{
			return true;
		}
		if (this.lockFeature != null && !GameManager.Instance.IsEditMode() && this.lockFeature.IsLocked() && !this.lockFeature.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier))
		{
			return false;
		}
		if (base.CommandIs(_commandName, "show_bounds"))
		{
			return flag && !this.ShowBounds;
		}
		if (base.CommandIs(_commandName, "hide_bounds"))
		{
			return flag && this.ShowBounds;
		}
		base.CommandIs(_commandName, "remove");
		return flag;
	}

	// Token: 0x06000B3D RID: 2877 RVA: 0x00048068 File Offset: 0x00046268
	public override bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		base.OnBlockActivated(_commandName, _world, _blockPos, _blockValue, _player);
		if (base.CommandIs(_commandName, "show_bounds") || base.CommandIs(_commandName, "hide_bounds"))
		{
			this.ShowBounds = !this.ShowBounds;
			Transform boundsHelper = LandClaimBoundsHelper.GetBoundsHelper(base.ToWorldPos());
			if (boundsHelper != null)
			{
				this.BoundsHelper = boundsHelper;
				boundsHelper.gameObject.SetActive(this.ShowBounds);
			}
			return true;
		}
		if (base.CommandIs(_commandName, "remove"))
		{
			_world.SetBlockRPC(_blockPos, BlockValue.Air);
			_player.PlayOneShot("keystone_destroyed", false, false, false, null, 1f);
			return true;
		}
		return false;
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x0004811C File Offset: 0x0004631C
	public override void OnAdded(Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnAdded(_blockPos, _blockValue);
		GameManager.Instance.persistentPlayers.PlaceLandProtectionBlock(base.ToWorldPos(), base.Parent.Owner);
		if (base.Parent.LocalPlayerIsOwner)
		{
			Transform boundsHelper = LandClaimBoundsHelper.GetBoundsHelper(base.ToWorldPos());
			if (boundsHelper != null)
			{
				this.BoundsHelper = boundsHelper;
				boundsHelper.gameObject.SetActive(this.ShowBounds);
			}
		}
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x00048190 File Offset: 0x00046390
	public override void Read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		if (_eStreamMode == StreamModeRead.Persistency)
		{
			if (base.Parent.UseLocalVersioning())
			{
				_br.ReadUInt16();
			}
			else
			{
				base.Parent.GetLegacyForkVersion();
			}
		}
		this.showBounds = _br.ReadBoolean();
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x000481CB File Offset: 0x000463CB
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		_bw.Write(this.showBounds);
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x000481EC File Offset: 0x000463EC
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		if (this.BoundsHelper != null)
		{
			this.BoundsHelper.localPosition = base.ToWorldPos().ToVector3() - Origin.position + new Vector3(0.5f, 0.5f, 0.5f);
		}
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x0004824C File Offset: 0x0004644C
	public override void OnLoad()
	{
		base.OnLoad();
		Vector3i vector3i = base.ToWorldPos();
		if (!this.IsPrimary())
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				TEFeatureLandClaim.HandleDeactivateLandClaim(vector3i);
			}
			NavObjectManager.Instance.UnRegisterNavObjectByPosition(vector3i, "land_claim");
			if (GameManager.Instance.persistentPlayers.m_lpBlockMap.ContainsKey(vector3i))
			{
				PersistentPlayerData persistentPlayerData = GameManager.Instance.persistentPlayers.m_lpBlockMap[vector3i];
				GameManager.Instance.persistentPlayers.m_lpBlockMap.Remove(vector3i);
				persistentPlayerData.LPBlocks.Remove(vector3i);
			}
			this.ShowBounds = false;
		}
		if (base.Parent.LocalPlayerIsOwner)
		{
			Transform boundsHelper = LandClaimBoundsHelper.GetBoundsHelper(vector3i);
			if (boundsHelper != null)
			{
				this.BoundsHelper = boundsHelper;
				boundsHelper.gameObject.SetActive(this.ShowBounds);
			}
		}
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x00048324 File Offset: 0x00046524
	public override void OnUnload(World _world)
	{
		base.OnUnload(_world);
		LandClaimBoundsHelper.RemoveBoundsHelper(base.ToWorldPos());
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x0004833D File Offset: 0x0004653D
	public override void OnRemove(World _world)
	{
		base.OnRemove(_world);
		LandClaimBoundsHelper.RemoveBoundsHelper(base.ToWorldPos());
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x00048358 File Offset: 0x00046558
	public override void OnDestroy()
	{
		base.OnDestroy();
		GameManager.Instance.persistentPlayers.RemoveLandProtectionBlock(base.ToWorldPos());
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityMapMarkerRemove>().Setup(EnumMapObjectType.LandClaim, base.ToWorldPos()), false, -1, -1, -1, null, 192, false);
		}
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x000483C0 File Offset: 0x000465C0
	public override void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
		base.ReplacedBy(_bvOld, _bvNew, _teNew);
		if (!this.IsPrimary())
		{
			this.ShowBounds = false;
			Transform boundsHelper = LandClaimBoundsHelper.GetBoundsHelper(base.ToWorldPos());
			if (boundsHelper != null)
			{
				this.BoundsHelper = null;
				boundsHelper.gameObject.SetActive(false);
				LandClaimBoundsHelper.RemoveBoundsHelper(base.ToWorldPos());
			}
		}
	}

	// Token: 0x06000B47 RID: 2887 RVA: 0x00048422 File Offset: 0x00046622
	public bool IsPrimary()
	{
		return GameManager.Instance.persistentPlayers.GetLandProtectionBlockOwner(base.ToWorldPos()) != null;
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x0004843C File Offset: 0x0004663C
	public static void HandleDeactivateLandClaim(Vector3i _blockPos)
	{
		World world = GameManager.Instance.World;
		BlockValue block = world.GetBlock(_blockPos);
		if (!block.isair)
		{
			GameManager.Instance.persistentPlayers.m_lpBlockMap.Remove(_blockPos);
			NavObjectManager.Instance.UnRegisterNavObjectByPosition(_blockPos.ToVector3(), "land_claim");
			LandClaimBoundsHelper.RemoveBoundsHelper(_blockPos.ToVector3());
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				block.damage = block.Block.MaxDamage - 1;
				world.SetBlockRPC(_blockPos, block);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityMapMarkerRemove>().Setup(EnumMapObjectType.LandClaim, _blockPos), false, -1, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x04000A7E RID: 2686
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 18;

	// Token: 0x04000A7F RID: 2687
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockable lockFeature;

	// Token: 0x04000A80 RID: 2688
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockPickable lockpickFeature;

	// Token: 0x04000A81 RID: 2689
	public Transform BoundsHelper;

	// Token: 0x04000A82 RID: 2690
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showBounds;

	// Token: 0x04000A83 RID: 2691
	[PublicizedFrom(EAccessModifier.Private)]
	public string activePrompt;

	// Token: 0x04000A84 RID: 2692
	[PublicizedFrom(EAccessModifier.Private)]
	public string inactivePrompt;
}
