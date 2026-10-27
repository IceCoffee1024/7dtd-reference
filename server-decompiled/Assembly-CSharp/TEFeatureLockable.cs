using System;
using System.Collections.Generic;
using Audio;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200017F RID: 383
[Preserve]
public class TEFeatureLockable : TEFeatureAbs, ILockable, IFeatureTriggerCapability
{
	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00020119 File Offset: 0x0001E319
	public IFeatureTriggerCapability.ETriggerRole TriggerRole
	{
		get
		{
			return IFeatureTriggerCapability.ETriggerRole.TriggeredBy;
		}
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x00046788 File Offset: 0x00044988
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x00048500 File Offset: 0x00046700
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
		TEFeatureLockable tefeatureLockable;
		if (_other.TryGetSelfOrFeature<TEFeatureLockable>(out tefeatureLockable))
		{
			this.locked = tefeatureLockable.IsLocked();
			this.allowedUserIds.AddRange(tefeatureLockable.GetUsers());
			this.passwordHash = tefeatureLockable.GetPasswordHash();
		}
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x00048540 File Offset: 0x00046740
	public override void UpgradeDowngradeFrom(TileEntityComposite _other)
	{
		base.UpgradeDowngradeFrom(_other);
		TEFeatureLockable feature = _other.GetFeature<TEFeatureLockable>();
		if (feature != null)
		{
			this.locked = feature.IsLocked();
			this.allowedUserIds.AddRange(feature.GetUsers());
			this.passwordHash = feature.GetPasswordHash();
		}
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x00048587 File Offset: 0x00046787
	public bool IsLocked()
	{
		return this.locked;
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x0004858F File Offset: 0x0004678F
	public void SetLocked(bool _isLocked)
	{
		this.locked = _isLocked;
		base.SetModified();
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x0004859E File Offset: 0x0004679E
	public void OnBlockTriggered(EntityPlayer _player, Vector3i _blockPos, BlockValue _blockValue, List<BlockChangeInfo> _blockChanges, BlockTrigger _triggeredBy)
	{
		if (_triggeredBy == null)
		{
			return;
		}
		if (_triggeredBy.Unlock)
		{
			this.SetLocked(false);
		}
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x000485B5 File Offset: 0x000467B5
	public PlatformUserIdentifierAbs GetOwner()
	{
		return base.Parent.Owner;
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x000485C2 File Offset: 0x000467C2
	public void SetOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		base.Parent.SetOwner(_userIdentifier);
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x000485D0 File Offset: 0x000467D0
	public bool IsUserAllowed(PlatformUserIdentifierAbs _userIdentifier)
	{
		TEFeatureDoor tefeatureDoor;
		return this.IsOwner(_userIdentifier) || this.allowedUserIds.Contains(_userIdentifier) || (_userIdentifier.Equals(PlatformManager.InternalLocalUserIdentifier) && (!base.Parent.TryGetSelfOrFeature<TEFeatureDoor>(out tefeatureDoor) && GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled)));
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x00048620 File Offset: 0x00046820
	public List<PlatformUserIdentifierAbs> GetUsers()
	{
		return this.allowedUserIds;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00048628 File Offset: 0x00046828
	public bool LocalPlayerIsOwner()
	{
		return this.IsOwner(PlatformManager.InternalLocalUserIdentifier);
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00048635 File Offset: 0x00046835
	public bool IsOwner(PlatformUserIdentifierAbs _userIdentifier)
	{
		return _userIdentifier != null && _userIdentifier.Equals(base.Parent.Owner);
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x0004864D File Offset: 0x0004684D
	public bool HasPassword()
	{
		return !string.IsNullOrEmpty(this.passwordHash);
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x0004865D File Offset: 0x0004685D
	public bool SetPasswordHash(string _passwordHash, PlatformUserIdentifierAbs _userIdentifier)
	{
		if (this.IsOwner(_userIdentifier) && _passwordHash != this.passwordHash)
		{
			this.passwordHash = _passwordHash;
			this.allowedUserIds.Clear();
			base.SetModified();
			return true;
		}
		return false;
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x00048691 File Offset: 0x00046891
	public bool CheckPasswordHash(string _passwordHash, PlatformUserIdentifierAbs _userIdentifier)
	{
		if (this.IsOwner(_userIdentifier) || !this.HasPassword())
		{
			return true;
		}
		if (_passwordHash == this.passwordHash)
		{
			this.allowedUserIds.Add(_userIdentifier);
			base.SetModified();
			return true;
		}
		return false;
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x000486C9 File Offset: 0x000468C9
	public string GetPasswordHash()
	{
		return this.passwordHash;
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x000486D4 File Offset: 0x000468D4
	public override string GetActivationText(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing, string _activateHotkeyMarkup, string _focusedTileEntityName)
	{
		base.GetActivationText(_world, _blockPos, _blockValue, _entityFocusing, _activateHotkeyMarkup, _focusedTileEntityName);
		if (!this.IsLocked())
		{
			return string.Format(Localization.Get("tooltipUnlocked", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		if (!this.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier))
		{
			return string.Format(Localization.Get("tooltipJammed", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
		}
		return string.Format(Localization.Get("tooltipLocked", false, null), _activateHotkeyMarkup, _focusedTileEntityName);
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00048748 File Offset: 0x00046948
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("lock", "lock", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
		_addCallback(new BlockActivationCommand("unlock", "unlock", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
		_addCallback(new BlockActivationCommand("keypad", "keypad", false, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x000487BC File Offset: 0x000469BC
	public override bool AllowBlockActivationCommand(ITileEntityFeature _module, ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _entityFocusing)
	{
		if (!base.AllowBlockActivationCommand(_module, _commandName, _world, _blockPos, _blockValue, _entityFocusing))
		{
			return false;
		}
		if (!this.Equals(_module))
		{
			return true;
		}
		PlatformUserIdentifierAbs internalLocalUserIdentifier = PlatformManager.InternalLocalUserIdentifier;
		bool flag = !this.LocalPlayerIsOwner() && GameManager.Instance.persistentPlayers.Allies.IsAlly(base.Parent.Owner, internalLocalUserIdentifier);
		if (base.CommandIs(_commandName, "lock"))
		{
			return !this.IsLocked() && (this.LocalPlayerIsOwner() || flag || _world.IsEditor());
		}
		if (base.CommandIs(_commandName, "unlock"))
		{
			return this.IsLocked() && (this.LocalPlayerIsOwner() || _world.IsEditor());
		}
		return !base.CommandIs(_commandName, "keypad") || (!this.IsUserAllowed(internalLocalUserIdentifier) && this.HasPassword() && this.IsLocked()) || this.LocalPlayerIsOwner();
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x000488A0 File Offset: 0x00046AA0
	public override bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		base.OnBlockActivated(_commandName, _world, _blockPos, _blockValue, _player);
		if (base.CommandIs(_commandName, "lock"))
		{
			this.SetLocked(true);
			Manager.BroadcastPlayByLocalPlayer(_blockPos.ToVector3() + Vector3.one * 0.5f, "Misc/locking");
			GameManager.ShowTooltip(_player, "containerLocked", false, false, 0f);
			return true;
		}
		if (base.CommandIs(_commandName, "unlock"))
		{
			this.SetLocked(false);
			Manager.BroadcastPlayByLocalPlayer(_blockPos.ToVector3() + Vector3.one * 0.5f, "Misc/unlocking");
			GameManager.ShowTooltip(_player, "containerUnlocked", false, false, 0f);
			return true;
		}
		if (base.CommandIs(_commandName, "keypad"))
		{
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(_player);
			if (uiforPlayer != null)
			{
				XUiC_KeypadWindow.Open(uiforPlayer, this, null, null);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x00048984 File Offset: 0x00046B84
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
		this.locked = _br.ReadBoolean();
		int num = _br.ReadInt32();
		this.allowedUserIds.Clear();
		for (int i = 0; i < num; i++)
		{
			this.allowedUserIds.Add(PlatformUserIdentifierAbs.FromStream(_br, false, false));
		}
		this.passwordHash = _br.ReadString();
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x00048A08 File Offset: 0x00046C08
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		_bw.Write(this.locked);
		_bw.Write(this.allowedUserIds.Count);
		for (int i = 0; i < this.allowedUserIds.Count; i++)
		{
			this.allowedUserIds[i].ToStream(_bw, false);
		}
		_bw.Write(this.passwordHash);
	}

	// Token: 0x04000A85 RID: 2693
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 18;

	// Token: 0x04000A86 RID: 2694
	[PublicizedFrom(EAccessModifier.Private)]
	public bool locked;

	// Token: 0x04000A87 RID: 2695
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<PlatformUserIdentifierAbs> allowedUserIds = new List<PlatformUserIdentifierAbs>();

	// Token: 0x04000A88 RID: 2696
	[PublicizedFrom(EAccessModifier.Private)]
	public string passwordHash = "";
}
