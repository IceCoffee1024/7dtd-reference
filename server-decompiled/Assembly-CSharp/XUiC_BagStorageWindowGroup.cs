using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000EE2 RID: 3810
[Preserve]
public class XUiC_BagStorageWindowGroup : XUiController
{
	// Token: 0x17000E11 RID: 3601
	// (get) Token: 0x0600767E RID: 30334 RVA: 0x002CAF61 File Offset: 0x002C9161
	// (set) Token: 0x0600767F RID: 30335 RVA: 0x002CAF69 File Offset: 0x002C9169
	public Bag Bag
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06007680 RID: 30336 RVA: 0x002CAF74 File Offset: 0x002C9174
	public override void Init()
	{
		base.Init();
		XUiC_BagStorageWindowGroup.ID = this.windowGroup.Id;
		this.containerWindow = base.GetChildByType<XUiC_BagContainer>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.timerWindow = this.xui.GetChildByType<XUiC_Timer>();
	}

	// Token: 0x06007681 RID: 30337 RVA: 0x002CAFC0 File Offset: 0x002C91C0
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

	// Token: 0x06007682 RID: 30338 RVA: 0x002A21E4 File Offset: 0x002A03E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenStorageWindow()
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
	}

	// Token: 0x06007683 RID: 30339 RVA: 0x002CB19E File Offset: 0x002C939E
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

	// Token: 0x06007684 RID: 30340 RVA: 0x002CB1B7 File Offset: 0x002C93B7
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.checkInteraction != null && !this.checkInteraction())
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x06007685 RID: 30341 RVA: 0x002CB1F4 File Offset: 0x002C93F4
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

	// Token: 0x06007686 RID: 30342 RVA: 0x002CB35C File Offset: 0x002C955C
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

	// Token: 0x06007687 RID: 30343 RVA: 0x002CB3DC File Offset: 0x002C95DC
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

	// Token: 0x040057C8 RID: 22472
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_BagContainer containerWindow;

	// Token: 0x040057C9 RID: 22473
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x040057CA RID: 22474
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Timer timerWindow;

	// Token: 0x040057CB RID: 22475
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playOnOpenSound;

	// Token: 0x040057CC RID: 22476
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ignoreCloseSound;

	// Token: 0x040057CD RID: 22477
	public static string ID;

	// Token: 0x040057CE RID: 22478
	public Entity Entity;

	// Token: 0x040057D0 RID: 22480
	[PublicizedFrom(EAccessModifier.Private)]
	public LootContainer lootContainer;

	// Token: 0x040057D1 RID: 22481
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseCallback;

	// Token: 0x040057D2 RID: 22482
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> checkInteraction;
}
