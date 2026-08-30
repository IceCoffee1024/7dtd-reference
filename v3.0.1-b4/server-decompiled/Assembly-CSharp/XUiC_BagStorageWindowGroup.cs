using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000EB3 RID: 3763
[Preserve]
public class XUiC_BagStorageWindowGroup : XUiController
{
	// Token: 0x17000DD6 RID: 3542
	// (get) Token: 0x060075BD RID: 30141 RVA: 0x002C31A9 File Offset: 0x002C13A9
	// (set) Token: 0x060075BE RID: 30142 RVA: 0x002C31B1 File Offset: 0x002C13B1
	public Bag Bag
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060075BF RID: 30143 RVA: 0x002C31BC File Offset: 0x002C13BC
	public override void Init()
	{
		base.Init();
		XUiC_BagStorageWindowGroup.ID = this.windowGroup.Id;
		this.containerWindow = base.GetChildByType<XUiC_BagContainer>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.timerWindow = this.xui.GetChildByType<XUiC_Timer>();
	}

	// Token: 0x060075C0 RID: 30144 RVA: 0x002C3208 File Offset: 0x002C1408
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

	// Token: 0x060075C1 RID: 30145 RVA: 0x0029A7CC File Offset: 0x002989CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenStorageWindow()
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
	}

	// Token: 0x060075C2 RID: 30146 RVA: 0x002C33E6 File Offset: 0x002C15E6
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

	// Token: 0x060075C3 RID: 30147 RVA: 0x002C33FF File Offset: 0x002C15FF
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.checkInteraction != null && !this.checkInteraction())
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x060075C4 RID: 30148 RVA: 0x002C343C File Offset: 0x002C163C
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

	// Token: 0x060075C5 RID: 30149 RVA: 0x002C35A4 File Offset: 0x002C17A4
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

	// Token: 0x060075C6 RID: 30150 RVA: 0x002C3624 File Offset: 0x002C1824
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

	// Token: 0x040056C6 RID: 22214
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_BagContainer containerWindow;

	// Token: 0x040056C7 RID: 22215
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x040056C8 RID: 22216
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Timer timerWindow;

	// Token: 0x040056C9 RID: 22217
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playOnOpenSound;

	// Token: 0x040056CA RID: 22218
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ignoreCloseSound;

	// Token: 0x040056CB RID: 22219
	public static string ID;

	// Token: 0x040056CC RID: 22220
	public Entity Entity;

	// Token: 0x040056CE RID: 22222
	[PublicizedFrom(EAccessModifier.Private)]
	public LootContainer lootContainer;

	// Token: 0x040056CF RID: 22223
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseCallback;

	// Token: 0x040056D0 RID: 22224
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> checkInteraction;
}
