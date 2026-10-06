using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000ED7 RID: 3799
[Preserve]
public class XUiC_BagStorageWindowGroup : XUiController
{
	// Token: 0x17000E0D RID: 3597
	// (get) Token: 0x06007659 RID: 30297 RVA: 0x002C9AA5 File Offset: 0x002C7CA5
	// (set) Token: 0x0600765A RID: 30298 RVA: 0x002C9AAD File Offset: 0x002C7CAD
	public Bag Bag
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600765B RID: 30299 RVA: 0x002C9AB8 File Offset: 0x002C7CB8
	public override void Init()
	{
		base.Init();
		XUiC_BagStorageWindowGroup.ID = this.windowGroup.Id;
		this.containerWindow = base.GetChildByType<XUiC_BagContainer>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
		this.timerWindow = this.xui.GetChildByType<XUiC_Timer>();
	}

	// Token: 0x0600765C RID: 30300 RVA: 0x002C9B04 File Offset: 0x002C7D04
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

	// Token: 0x0600765D RID: 30301 RVA: 0x002A0CBC File Offset: 0x0029EEBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenStorageWindow()
	{
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
	}

	// Token: 0x0600765E RID: 30302 RVA: 0x002C9CE2 File Offset: 0x002C7EE2
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

	// Token: 0x0600765F RID: 30303 RVA: 0x002C9CFB File Offset: 0x002C7EFB
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (this.checkInteraction != null && !this.checkInteraction())
		{
			this.xui.playerUI.windowManager.Close(this.windowGroup, false);
		}
	}

	// Token: 0x06007660 RID: 30304 RVA: 0x002C9D38 File Offset: 0x002C7F38
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

	// Token: 0x06007661 RID: 30305 RVA: 0x002C9EA0 File Offset: 0x002C80A0
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

	// Token: 0x06007662 RID: 30306 RVA: 0x002C9F20 File Offset: 0x002C8120
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

	// Token: 0x040057A3 RID: 22435
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_BagContainer containerWindow;

	// Token: 0x040057A4 RID: 22436
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x040057A5 RID: 22437
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_Timer timerWindow;

	// Token: 0x040057A6 RID: 22438
	[PublicizedFrom(EAccessModifier.Private)]
	public bool playOnOpenSound;

	// Token: 0x040057A7 RID: 22439
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ignoreCloseSound;

	// Token: 0x040057A8 RID: 22440
	public static string ID;

	// Token: 0x040057A9 RID: 22441
	public Entity Entity;

	// Token: 0x040057AB RID: 22443
	[PublicizedFrom(EAccessModifier.Private)]
	public LootContainer lootContainer;

	// Token: 0x040057AC RID: 22444
	[PublicizedFrom(EAccessModifier.Private)]
	public Action onCloseCallback;

	// Token: 0x040057AD RID: 22445
	[PublicizedFrom(EAccessModifier.Private)]
	public Func<bool> checkInteraction;
}
