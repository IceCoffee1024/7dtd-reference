using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200017B RID: 379
[Preserve]
public class TEFeatureLandClaim : TEFeatureAbs
{
	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000B22 RID: 2850 RVA: 0x000477FC File Offset: 0x000459FC
	// (set) Token: 0x06000B23 RID: 2851 RVA: 0x00047804 File Offset: 0x00045A04
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

	// Token: 0x06000B24 RID: 2852 RVA: 0x00047814 File Offset: 0x00045A14
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		this.lockFeature = base.Parent.GetFeature<TEFeatureLockable>();
		this.lockpickFeature = base.Parent.GetFeature<TEFeatureLockPickable>();
		this.activePrompt = Localization.Get("activeBlockPrompt", false, null);
		this.inactivePrompt = Localization.Get("inactiveBlockPrompt", false, null);
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x00047870 File Offset: 0x00045A70
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
		TEFeatureLandClaim tefeatureLandClaim;
		if (_other.TryGetSelfOrFeature<TEFeatureLandClaim>(out tefeatureLandClaim))
		{
			this.showBounds = tefeatureLandClaim.showBounds;
		}
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x00047894 File Offset: 0x00045A94
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

	// Token: 0x06000B27 RID: 2855 RVA: 0x00047984 File Offset: 0x00045B84
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("show_bounds", "frames", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
		_addCallback(new BlockActivationCommand("hide_bounds", "frames", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
		_addCallback(new BlockActivationCommand("remove", "x", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x000479F8 File Offset: 0x00045BF8
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

	// Token: 0x06000B29 RID: 2857 RVA: 0x00047AB4 File Offset: 0x00045CB4
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

	// Token: 0x06000B2A RID: 2858 RVA: 0x00047B68 File Offset: 0x00045D68
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

	// Token: 0x06000B2B RID: 2859 RVA: 0x00047BDC File Offset: 0x00045DDC
	public override void Read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.Read(_br, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeRead.Persistency)
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

	// Token: 0x06000B2C RID: 2860 RVA: 0x00047C17 File Offset: 0x00045E17
	public override void Write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == TileEntity.StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		_bw.Write(this.showBounds);
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x00047C38 File Offset: 0x00045E38
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		if (this.BoundsHelper != null)
		{
			this.BoundsHelper.localPosition = base.ToWorldPos().ToVector3() - Origin.position + new Vector3(0.5f, 0.5f, 0.5f);
		}
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x00047C98 File Offset: 0x00045E98
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

	// Token: 0x06000B2F RID: 2863 RVA: 0x00047D70 File Offset: 0x00045F70
	public override void OnUnload(World _world)
	{
		base.OnUnload(_world);
		LandClaimBoundsHelper.RemoveBoundsHelper(base.ToWorldPos());
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x00047D89 File Offset: 0x00045F89
	public override void OnRemove(World _world)
	{
		base.OnRemove(_world);
		LandClaimBoundsHelper.RemoveBoundsHelper(base.ToWorldPos());
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x00047DA4 File Offset: 0x00045FA4
	public override void OnDestroy()
	{
		base.OnDestroy();
		GameManager.Instance.persistentPlayers.RemoveLandProtectionBlock(base.ToWorldPos());
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityMapMarkerRemove>().Setup(EnumMapObjectType.LandClaim, base.ToWorldPos()), false, -1, -1, -1, null, 192, false);
		}
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x00047E0C File Offset: 0x0004600C
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

	// Token: 0x06000B33 RID: 2867 RVA: 0x00047E6E File Offset: 0x0004606E
	public bool IsPrimary()
	{
		return GameManager.Instance.persistentPlayers.GetLandProtectionBlockOwner(base.ToWorldPos()) != null;
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x00047E88 File Offset: 0x00046088
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

	// Token: 0x04000A72 RID: 2674
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 18;

	// Token: 0x04000A73 RID: 2675
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockable lockFeature;

	// Token: 0x04000A74 RID: 2676
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockPickable lockpickFeature;

	// Token: 0x04000A75 RID: 2677
	public Transform BoundsHelper;

	// Token: 0x04000A76 RID: 2678
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showBounds;

	// Token: 0x04000A77 RID: 2679
	[PublicizedFrom(EAccessModifier.Private)]
	public string activePrompt;

	// Token: 0x04000A78 RID: 2680
	[PublicizedFrom(EAccessModifier.Private)]
	public string inactivePrompt;
}
