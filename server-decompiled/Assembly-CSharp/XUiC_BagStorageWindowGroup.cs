using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000EC8 RID: 3784
[Preserve]
public class XUiC_BagStorageWindowGroup : XUiController
{
	// Token: 0x17000DF0 RID: 3568
	// (get) Token: 0x0600765E RID: 30302 RVA: 0x002C66CD File Offset: 0x002C48CD
	// (set) Token: 0x0600765F RID: 30303 RVA: 0x002C66D5 File Offset: 0x002C48D5
	public Bag Bag
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007660 RID: 30304 RVA: 0x002C66E0 File Offset: 0x002C48E0
	public override void Init()
	{
		base.Init();
		XUiC_BagStorageWindowGroup.ID = this.windowGroup.Id;
		this.containerWindow = base.GetChildByType<XUiC_BagContainer>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.timerWindow = this.xui.GetChildByType<XUiC_Timer>();
	}

	// Token: 0x06007661 RID: 30305 RVA: 0x002C672C File Offset: 0x002C492C
	public static void Open(XUi _xui, Entity _entity, Bag _bag, LootContainer _lootContainer, string _containerName = null, Action _onModified = null, Action _onClose = null, Func<bool> _interactionCheck = null, bool _runOpenTimer = false)
	{
		if (_containerName == null)
		{
			_containerName = Localization.Get("xuiLoot", false, null);
		}
		XUiC_BagStorageWindowGroup ctrl = _xui.FindWindowGroupByName(XUiC_BagStorageWindowGroup.ID).GetChildByType<XUiC_BagStorageWindowGroup>();
		ctrl.Entity = _entity;
		ctrl.Bag = _bag;
		ctrl.lootContainer = _lootContainer;
		ctrl.onCloseCallback = _onClose;
		ctrl.checkInteraction = _interactionCheck;
		ctrl.playOnOpenSound = !_runOpenTimer;
		ctrl.containerWindow.SetBag(_bag, _lootContainer, _containerName, _onModified);
		if (!_runOpenTimer || _lootContainer == null)
		{
			ctrl.OpenStorageWindow();
			return;
		}
		EntityPlayer entityPlayer = _xui.playerUI.entityPlayer;
		float num = EffectManager.GetValue(PassiveEffects.ScavengingTime, null, (entityPlayer != null && entityPlayer.IsCrouching) ? (_lootContainer.openTime * 1.5f) : _lootContainer.openTime, entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) * LootContainer.LootTimerModifier;
		if (num == 0f)
		{
			num = 0.01f;
		}
		if (num <= 0f)
		{
			ctrl.OpenStorageWindow();
			return;
		}
		_xui.playerUI.windowManager.Close("backpack");
		_xui.playerUI.windowManager.Open("CalloutGroup", false);
		TimerEventData timerEventData = new TimerEventData();
		timerEventData.FullTimeFinishEvent += delegate(TimerEventData _)
		{
			ctrl.OpenStorageWindow();
		};
		timerEventData.CloseEvent += delegate(TimerEventData _)
		{
			ctrl.CancelOpen();
		};
		timerEventData.CloseOnHit = true;
		timerEventData.CancelWithActivateButton = true;
		XUiC_Timer.OpenTimer(_xui, num, timerEventData, -1f, Localization.Get("xuiOpeningLoot", false, null), true);
		if (!string.IsNullOrEmpty(ctrl.lootContainer.soundOpen))
		{
			Manager.BroadcastPlayByLocalPlayer(XUiM_Player.GetPlayer().position, ctrl.lootContainer.soundOpen);
		}
	}

	// Token: 0x06007662 RID: 30306 RVA: 0x0029D92C File Offset: 0x0029BB2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenStorageWindow()
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
	}

	// Token: 0x06007663 RID: 30307 RVA: 0x002C690A File Offset: 0x002C4B0A
	[PublicizedFrom(EAccessModifier.Private)]
	public void CancelOpen()
	{
		Action action = this.onCloseCallback;
		if (action != null)
		{
			action();
		}
		this.ClearPendingState();
	}

	// Token: 0x06007664 RID: 30308 RVA: 0x002C6923 File Offset: 0x002C4B23
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.checkInteraction != null && !this.checkInteraction())
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x06007665 RID: 30309 RVA: 0x002C6960 File Offset: 0x002C4B60
	public override void OnOpen()
	{
		base.OnOpen();
		this.ignoreCloseSound = false;
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		if (EffectManager.GetValue(PassiveEffects.DisableLoot, null, 0f, entityPlayer, null, this.Entity.EntityClass.Tags, true, true, true, true, true, 1, true, false) > 0f)
		{
			Manager.PlayInsidePlayerHead("twitch_no_attack", -1, 0f, false, false);
			GUIWindowManager windowManager = this.xui.playerUI.windowManager;
			this.ignoreCloseSound = true;
			windowManager.Close("timer");
			windowManager.Close(this.windowGroup, false);
			return;
		}
		if (this.nonPagingHeaderWindow != null)
		{
			this.nonPagingHeaderWindow.SetHeader(Localization.Get("xuiStorage", false, null));
		}
		if (this.lootContainer == null)
		{
			return;
		}
		this.xui.playerUI.windowManager.Open("backpack", false);
		XUiC_BagContainer xuiC_BagContainer = this.containerWindow;
		if (((xuiC_BagContainer != null) ? xuiC_BagContainer.ViewComponent : null) != null)
		{
			this.containerWindow.ViewComponent.UiTransform.gameObject.SetActive(true);
			this.containerWindow.ViewComponent.IsVisible = true;
		}
		this.xui.RecenterWindowGroup(this.windowGroup, false);
		if (this.playOnOpenSound && !string.IsNullOrEmpty(this.lootContainer.soundOpen))
		{
			Manager.BroadcastPlayByLocalPlayer(XUiM_Player.GetPlayer().position, this.lootContainer.soundOpen);
		}
	}

	// Token: 0x06007666 RID: 30310 RVA: 0x002C6AC8 File Offset: 0x002C4CC8
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("backpack");
		if (!this.ignoreCloseSound && this.lootContainer != null && !string.IsNullOrEmpty(this.lootContainer.soundClose))
		{
			Manager.BroadcastPlayByLocalPlayer(XUiM_Player.GetPlayer().position, this.lootContainer.soundClose);
		}
		Action action = this.onCloseCallback;
		if (action != null)
		{
			action();
		}
		this.ClearPendingState();
	}

	// Token: 0x06007667 RID: 30311 RVA: 0x002C6B48 File Offset: 0x002C4D48
	[PublicizedFrom(EAccessModifier.Private)]
	public void ClearPendingState()
	{
		this.ignoreCloseSound = false;
		this.playOnOpenSound = false;
		XUiC_BagContainer xuiC_BagContainer = this.containerWindow;
		if (((xuiC_BagContainer != null) ? xuiC_BagContainer.ViewComponent : null) != null)
		{
			this.containerWindow.ViewComponent.UiTransform.gameObject.SetActive(true);
			this.containerWindow.ViewComponent.IsVisible = true;
		}
		this.Bag = null;
		this.lootContainer = null;
		this.onCloseCallback = null;
		this.checkInteraction = null;
	}

	// Token: 0x0400576B RID: 22379
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_BagContainer containerWindow;

	// Token: 0x0400576C RID: 22380
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x0400576D RID: 22381
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Timer timerWindow;

	// Token: 0x0400576E RID: 22382
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playOnOpenSound;

	// Token: 0x0400576F RID: 22383
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ignoreCloseSound;

	// Token: 0x04005770 RID: 22384
	public static string ID;

	// Token: 0x04005771 RID: 22385
	public Entity Entity;

	// Token: 0x04005773 RID: 22387
	[PublicizedFrom(EAccessModifier.Private)]
	public LootContainer lootContainer;

	// Token: 0x04005774 RID: 22388
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseCallback;

	// Token: 0x04005775 RID: 22389
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> checkInteraction;
}
