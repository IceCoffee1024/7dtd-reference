using System;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000184 RID: 388
[Preserve]
public class TEFeatureSignable : TEFeatureAbs, ITileEntitySignable, ITileEntity, ILockTarget, IHasWorldAddress, IFeatureSavedInPrefab
{
	// Token: 0x06000B8D RID: 2957 RVA: 0x000494C5 File Offset: 0x000476C5
	public TEFeatureSignable()
	{
		PlatformUserManager.BlockedStateChanged += this.UserBlockedStateChanged;
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x000494E9 File Offset: 0x000476E9
	public override void OnUnload(World _world)
	{
		PlatformUserManager.BlockedStateChanged -= this.UserBlockedStateChanged;
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x000494E9 File Offset: 0x000476E9
	public override void OnDestroy()
	{
		PlatformUserManager.BlockedStateChanged -= this.UserBlockedStateChanged;
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x000494FC File Offset: 0x000476FC
	public override void Init(TileEntityComposite _parent, TileEntityFeatureData _featureData)
	{
		base.Init(_parent, _featureData);
		this.lockFeature = base.Parent.GetFeature<TEFeatureLockable>();
		this.lockpickFeature = base.Parent.GetFeature<TEFeatureLockPickable>();
		DynamicProperties props = _featureData.Props;
		props.ParseInt("LineCount", ref this.lineCount);
		props.ParseFloat("LineWidth", ref this.lineWidth);
		props.ParseInt("FontSize", ref this.fontSize);
		props.ParseFloat("LineSpacing", ref this.lineSpacing);
		if (this.lineWidth <= 0f || this.lineCount <= 0 || this.fontSize <= 0 || this.lineSpacing <= 0f)
		{
			Log.Error(string.Format("Block '{0}' has TEFeatureSignable with missing or invalid properties: LineWidth={1}, LineCount={2}, FontSize={3}, LineSpacing={4}", new object[]
			{
				base.Parent.TeData.Block.GetBlockName(),
				this.lineWidth,
				this.lineCount,
				this.fontSize,
				this.lineSpacing
			}));
		}
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x00049610 File Offset: 0x00047810
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyFromInternal(TileEntityComposite _other)
	{
		TEFeatureSignable tefeatureSignable;
		if (_other.TryGetSelfOrFeature<TEFeatureSignable>(out tefeatureSignable))
		{
			this.signText = tefeatureSignable.signText.Clone();
		}
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x00049638 File Offset: 0x00047838
	public override void SetBlockEntityData(BlockEntityData _blockEntityData)
	{
		if (_blockEntityData == null || !_blockEntityData.bHasTransform)
		{
			return;
		}
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		if (this.lineWidth <= 0f || this.lineCount <= 0 || this.fontSize <= 0 || this.lineSpacing <= 0f)
		{
			return;
		}
		float num = this.lineWidth;
		TextMesh[] componentsInChildren = _blockEntityData.transform.GetComponentsInChildren<TextMesh>();
		if (componentsInChildren == null || componentsInChildren.Length == 0)
		{
			return;
		}
		this.smartTextMesh = new SmartTextMesh[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].fontSize = this.fontSize;
			componentsInChildren[i].characterSize = 0.01f;
			componentsInChildren[i].lineSpacing = this.lineSpacing;
			this.smartTextMesh[i] = componentsInChildren[i].gameObject.AddComponent<SmartTextMesh>();
			this.smartTextMesh[i].MaxWidthReal = num;
			this.smartTextMesh[i].MaxLines = this.lineCount;
			this.smartTextMesh[i].ConvertNewLines = true;
		}
		string text;
		if ((text = this.displayText) == null)
		{
			AuthoredText authoredText = this.signText;
			text = ((authoredText != null) ? authoredText.Text : null);
		}
		this.RefreshTextMesh(text);
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x0004974C File Offset: 0x0004794C
	[PublicizedFrom(EAccessModifier.Private)]
	public void UserBlockedStateChanged(IPlatformUserData _userData, EBlockType _blockType, EUserBlockState _blockState)
	{
		if (!_userData.PrimaryId.Equals(this.signText.Author) || _blockType != EBlockType.TextChat)
		{
			return;
		}
		GeneratedTextManager.GetDisplayText(this.signText, new Action<string>(this.RefreshTextMesh), true, true, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.NotSupported);
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x00049788 File Offset: 0x00047988
	[PublicizedFrom(EAccessModifier.Private)]
	public void RefreshTextMesh(string _text)
	{
		this.displayText = _text;
		if (GameManager.IsDedicatedServer || this.smartTextMesh == null || this.displayText == this.smartTextMesh[0].UnwrappedText)
		{
			return;
		}
		for (int i = 0; i < this.smartTextMesh.Length; i++)
		{
			this.smartTextMesh[i].UnwrappedText = this.displayText;
		}
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x000497EC File Offset: 0x000479EC
	public override void UpgradeDowngradeFrom(TileEntityComposite _other)
	{
		base.UpgradeDowngradeFrom(_other);
		ITileEntitySignable feature = _other.GetFeature<ITileEntitySignable>();
		if (feature != null)
		{
			this.signText = feature.GetAuthoredText();
			if (this.signText != null)
			{
				GeneratedTextManager.GetDisplayText(this.signText, new Action<string>(this.RefreshTextMesh), true, true, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.NotSupported);
			}
		}
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x00049839 File Offset: 0x00047A39
	public virtual void SetText(AuthoredText _authoredText, bool _syncData = true)
	{
		this.SetText((_authoredText != null) ? _authoredText.Text : null, _syncData, (_authoredText != null) ? _authoredText.Author : null);
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x0004985C File Offset: 0x00047A5C
	public virtual void SetText(string _text, bool _syncData = true, PlatformUserIdentifierAbs _signingPlayer = null)
	{
		if (_signingPlayer == null)
		{
			_signingPlayer = PlatformManager.MultiPlatform.User.PlatformUserId;
		}
		PersistentPlayerList persistentPlayers = GameManager.Instance.persistentPlayers;
		if (((persistentPlayers != null) ? persistentPlayers.GetPlayerData(_signingPlayer) : null) == null)
		{
			_signingPlayer = null;
		}
		if (_text == this.signText.Text)
		{
			return;
		}
		this.signText.Update(_text, _signingPlayer);
		GeneratedTextManager.GetDisplayText(this.signText, new Action<string>(this.RefreshTextMesh), true, true, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.NotSupported);
		if (_syncData)
		{
			base.SetModified();
		}
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x000498DE File Offset: 0x00047ADE
	public virtual AuthoredText GetAuthoredText()
	{
		return this.signText;
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x000498E6 File Offset: 0x00047AE6
	public virtual bool CanRenderString(string _text)
	{
		return this.smartTextMesh != null && this.smartTextMesh.Length != 0 && this.smartTextMesh[0].CanRenderString(_text);
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x0004990C File Offset: 0x00047B0C
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
		return Localization.Get("useWorkstation", false, null);
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x000499A0 File Offset: 0x00047BA0
	public override void InitBlockActivationCommands(Action<BlockActivationCommand, TileEntityComposite.EBlockCommandOrder, TileEntityFeatureData> _addCallback)
	{
		base.InitBlockActivationCommands(_addCallback);
		_addCallback(new BlockActivationCommand("edit", "pen", true, false, null), TileEntityComposite.EBlockCommandOrder.Normal, base.FeatureData);
		_addCallback(new BlockActivationCommand("report", "report", true, false, null), TileEntityComposite.EBlockCommandOrder.Last, base.FeatureData);
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x000499F4 File Offset: 0x00047BF4
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
		if (base.CommandIs(_commandName, "edit"))
		{
			return this.lockFeature == null || GameManager.Instance.IsEditMode() || !this.lockFeature.IsLocked() || this.lockFeature.IsUserAllowed(PlatformManager.InternalLocalUserIdentifier);
		}
		if (base.CommandIs(_commandName, "report"))
		{
			PlatformUserIdentifierAbs internalLocalUserIdentifier = PlatformManager.InternalLocalUserIdentifier;
			bool flag = PlatformManager.MultiPlatform.PlayerReporting != null && !string.IsNullOrEmpty(this.signText.Text) && !internalLocalUserIdentifier.Equals(this.signText.Author);
			PersistentPlayerData playerData = GameManager.Instance.persistentPlayers.GetPlayerData(this.signText.Author);
			bool flag2 = playerData != null && playerData.PlatformData.Blocked[EBlockType.TextChat].IsBlocked();
			return flag && !flag2;
		}
		return true;
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x00049AEC File Offset: 0x00047CEC
	public override bool OnBlockActivated(ReadOnlySpan<char> _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		base.OnBlockActivated(_commandName, _world, _blockPos, _blockValue, _player);
		if (base.CommandIs(_commandName, "edit"))
		{
			_player.AimingGun = false;
			LockManager.Instance.LockRequestLocal(this, 0);
			return true;
		}
		if (base.CommandIs(_commandName, "report"))
		{
			GeneratedTextManager.GetDisplayText(this.signText, delegate(string _filtered)
			{
				ThreadManager.AddSingleTaskMainThread("OpenReportWindow", delegate(object _)
				{
					PersistentPlayerData playerData = GameManager.Instance.persistentPlayers.GetPlayerData(this.signText.Author);
					XUiC_ReportPlayer.Open((playerData != null) ? playerData.PlayerData : null, EnumReportCategory.VerbalAbuse, string.Format(Localization.Get("xuiReportOffensiveTextMessage", false, null), _filtered));
				}, null);
			}, true, false, GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
			return true;
		}
		return false;
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x00049B57 File Offset: 0x00047D57
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
		this.SetText(AuthoredText.FromStream(_br), false);
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x00049B94 File Offset: 0x00047D94
	public override void Write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
	{
		base.Write(_bw, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			_bw.Write(18);
		}
		if (GameManager.Instance.IsEditMode())
		{
			AuthoredText.ToStream(new AuthoredText(this.signText.Text, null), _bw);
			return;
		}
		AuthoredText.ToStream(this.signText, _bw);
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x00049BE4 File Offset: 0x00047DE4
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		TEFeatureSignable.ShowUI(this, _success);
		return true;
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x00049BF0 File Offset: 0x00047DF0
	public static void ShowUI(ITileEntitySignable _te, bool _lockGranted)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_lockGranted)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow("signMultiline")).Controller.GetChildByType<XUiC_SignWindow>().SetTileEntitySign(_te);
		uiforPrimaryPlayer.windowManager.Open("signMultiline", true);
	}

	// Token: 0x04000A9E RID: 2718
	[PublicizedFrom(EAccessModifier.Private)]
	public const int Version = 18;

	// Token: 0x04000A9F RID: 2719
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockable lockFeature;

	// Token: 0x04000AA0 RID: 2720
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureLockPickable lockpickFeature;

	// Token: 0x04000AA1 RID: 2721
	[PublicizedFrom(EAccessModifier.Private)]
	public AuthoredText signText = new AuthoredText();

	// Token: 0x04000AA2 RID: 2722
	[PublicizedFrom(EAccessModifier.Private)]
	public int fontSize;

	// Token: 0x04000AA3 RID: 2723
	[PublicizedFrom(EAccessModifier.Private)]
	public int lineCount;

	// Token: 0x04000AA4 RID: 2724
	[PublicizedFrom(EAccessModifier.Private)]
	public float lineWidth;

	// Token: 0x04000AA5 RID: 2725
	[PublicizedFrom(EAccessModifier.Private)]
	public float lineSpacing;

	// Token: 0x04000AA6 RID: 2726
	[PublicizedFrom(EAccessModifier.Private)]
	public string displayText;

	// Token: 0x04000AA7 RID: 2727
	[PublicizedFrom(EAccessModifier.Private)]
	public SmartTextMesh[] smartTextMesh;
}
