using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F8E RID: 3982
[Preserve]
public class XUiC_LootWindowGroup : XUiController
{
	// Token: 0x06007D50 RID: 32080 RVA: 0x002F8661 File Offset: 0x002F6861
	public override void Init()
	{
		base.Init();
		this.lootWindow = base.GetChildByType<XUiC_LootWindow>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
	}

	// Token: 0x06007D51 RID: 32081 RVA: 0x002F8684 File Offset: 0x002F6884
	[PublicizedFrom(EAccessModifier.Private)]
	public void openContainer(string _lootContainerName, TEFeatureStorage _te, bool _playOpenSound, bool _firstTimeTouched)
	{
		this.te = _te;
		this.te.SetUserAccessing(true);
		this.lootWindow.SetTileEntityChest(_lootContainerName, _te, _playOpenSound);
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
		QuestEventManager.Current.OpenedContainer(_te.ToWorldPos(), _te, _firstTimeTouched);
		this.xui.playerUI.windowManager.Open("backpack", false);
		XUiC_WindowNonPagingHeader xuiC_WindowNonPagingHeader = this.nonPagingHeaderWindow;
		if (xuiC_WindowNonPagingHeader != null)
		{
			xuiC_WindowNonPagingHeader.SetHeader(Localization.Get("xuiLooting", false, null));
		}
		this.xui.RecenterWindowGroup(this.windowGroup, false);
	}

	// Token: 0x06007D52 RID: 32082 RVA: 0x002F872C File Offset: 0x002F692C
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("backpack");
		Vector3i vector3i = this.te.ToWorldPos();
		TEFeatureStorage selfOrFeature = GameManager.Instance.World.GetTileEntity(vector3i).GetSelfOrFeature<TEFeatureStorage>();
		if ((selfOrFeature == null || !selfOrFeature.IsRemoving) && selfOrFeature == this.te)
		{
			this.te.SetModified();
		}
		this.te.SetUserAccessing(false);
		LockManager.Instance.UnlockRequestLocal();
	}

	// Token: 0x06007D53 RID: 32083 RVA: 0x002F87B0 File Offset: 0x002F69B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void openTimerFinished(TimerEventData _data)
	{
		ValueTuple<string, TEFeatureStorage, bool> valueTuple = (ValueTuple<string, TEFeatureStorage, bool>)_data.Data;
		string item = valueTuple.Item1;
		TEFeatureStorage item2 = valueTuple.Item2;
		bool item3 = valueTuple.Item3;
		if (!item2.ItemGrid.PlayerOwned)
		{
			this.xui.playerUI.entityPlayer.Progression.AddLevelExp(this.xui.playerUI.entityPlayer.gameStage, "_xpFromLoot", Progression.XPTypes.Looting, true, true, -1, null);
		}
		this.openContainer(item, item2, false, item3);
	}

	// Token: 0x06007D54 RID: 32084 RVA: 0x00046AFD File Offset: 0x00044CFD
	[PublicizedFrom(EAccessModifier.Private)]
	public static void openTimerClosedManually(TimerEventData _data)
	{
		LockManager.Instance.UnlockRequestLocal();
	}

	// Token: 0x06007D55 RID: 32085 RVA: 0x002F8830 File Offset: 0x002F6A30
	public void OpenLooting(string _lootContainerName, TEFeatureStorage _te, bool _firstTimeTouched)
	{
		if (EffectManager.GetValue(PassiveEffects.DisableLoot, null, 0f, this.xui.playerUI.entityPlayer, null, _te.blockValue.Block.Tags, true, true, true, true, true, 1, true, false) > 0f)
		{
			Manager.PlayInsidePlayerHead("twitch_no_attack", -1, 0f, false, false);
			LockManager.Instance.UnlockRequestLocal();
			return;
		}
		if (!_firstTimeTouched || _te.ItemGrid.PlayerOwned)
		{
			this.openContainer(_lootContainerName, _te, true, _firstTimeTouched);
			return;
		}
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		float openTime = LootContainer.GetLootContainer(_te.lootListName, true).openTime;
		float num = EffectManager.GetValue(PassiveEffects.ScavengingTime, null, entityPlayer.IsCrouching ? (openTime * 1.5f) : openTime, entityPlayer, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) * LootContainer.LootTimerModifier;
		if (num == 0f)
		{
			num = 0.01f;
		}
		TimerEventData timerEventData = new TimerEventData();
		timerEventData.CloseEvent += XUiC_LootWindowGroup.openTimerClosedManually;
		timerEventData.FullTimeFinishEvent += this.openTimerFinished;
		timerEventData.Data = new ValueTuple<string, TEFeatureStorage, bool>(_lootContainerName, _te, _firstTimeTouched);
		timerEventData.CloseOnHit = true;
		timerEventData.CancelWithActivateButton = true;
		XUiC_Timer.OpenTimer(this.xui, num, timerEventData, -1f, Localization.Get("xuiOpeningLoot", false, null), true);
		LootContainer lootContainer = LootContainer.GetLootContainer(_te.lootListName, true);
		string text = ((lootContainer != null) ? lootContainer.soundOpen : null);
		if (!string.IsNullOrEmpty(text))
		{
			Manager.BroadcastPlayByLocalPlayer(_te.ToWorldPos().ToVector3() + Vector3.one * 0.5f, text);
		}
	}

	// Token: 0x06007D56 RID: 32086 RVA: 0x002F89D9 File Offset: 0x002F6BD9
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_LootWindowGroup GetInstance(XUi _xuiInstance = null)
	{
		if (_xuiInstance == null)
		{
			_xuiInstance = LocalPlayerUI.GetUIForPrimaryPlayer().xui;
		}
		return (XUiC_LootWindowGroup)_xuiInstance.FindWindowGroupByName(XUiC_LootWindowGroup.ID);
	}

	// Token: 0x06007D57 RID: 32087 RVA: 0x002F89FA File Offset: 0x002F6BFA
	public static Vector3i GetTeBlockPos(XUi _xuiInstance = null)
	{
		TEFeatureStorage tefeatureStorage = XUiC_LootWindowGroup.GetInstance(_xuiInstance).te;
		if (tefeatureStorage == null)
		{
			return Vector3i.zero;
		}
		return tefeatureStorage.ToWorldPos();
	}

	// Token: 0x06007D58 RID: 32088 RVA: 0x002F8A18 File Offset: 0x002F6C18
	public static void CloseIfOpenAtPos(Vector3i _blockPos, XUi _xuiInstance = null)
	{
		GUIWindowManager windowManager = XUiC_LootWindowGroup.GetInstance(_xuiInstance).xui.playerUI.windowManager;
		if (windowManager.IsWindowOpen(XUiC_LootWindowGroup.ID) && XUiC_LootWindowGroup.GetTeBlockPos(null) == _blockPos)
		{
			windowManager.Close(XUiC_LootWindowGroup.ID);
		}
	}

	// Token: 0x04005CE7 RID: 23783
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LootWindow lootWindow;

	// Token: 0x04005CE8 RID: 23784
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005CE9 RID: 23785
	[PublicizedFrom(EAccessModifier.Private)]
	public TEFeatureStorage te;

	// Token: 0x04005CEA RID: 23786
	[PublicizedFrom(EAccessModifier.Private)]
	public string lootingHeader;

	// Token: 0x04005CEB RID: 23787
	public static string ID = "looting";
}
