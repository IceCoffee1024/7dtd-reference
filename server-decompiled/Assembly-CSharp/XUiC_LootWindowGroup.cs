using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F7F RID: 3967
[Preserve]
public class XUiC_LootWindowGroup : XUiController
{
	// Token: 0x06007D40 RID: 32064 RVA: 0x002F5579 File Offset: 0x002F3779
	public override void Init()
	{
		base.Init();
		this.lootWindow = base.GetChildByType<XUiC_LootWindow>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
	}

	// Token: 0x06007D41 RID: 32065 RVA: 0x002F559C File Offset: 0x002F379C
	[PublicizedFrom(EAccessModifier.Private)]
	public void openContainer(string _lootContainerName, ITileEntityLootable _te, bool _playOpenSound)
	{
		this.te = _te;
		this.te.SetUserAccessing(true);
		this.lootWindow.SetTileEntityChest(_lootContainerName, _te, _playOpenSound);
		this.xui.playerUI.windowManager.Open(this.windowGroup, true);
		this.xui.playerUI.windowManager.Open("backpack", false);
		XUiC_WindowNonPagingHeader xuiC_WindowNonPagingHeader = this.nonPagingHeaderWindow;
		if (xuiC_WindowNonPagingHeader != null)
		{
			xuiC_WindowNonPagingHeader.SetHeader(Localization.Get("xuiLooting", false, null));
		}
		this.xui.RecenterWindowGroup(this.windowGroup, false);
	}

	// Token: 0x06007D42 RID: 32066 RVA: 0x002F5630 File Offset: 0x002F3830
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("backpack");
		Vector3i vector3i = this.te.ToWorldPos();
		ITileEntityLootable selfOrFeature = GameManager.Instance.World.GetTileEntity(vector3i).GetSelfOrFeature<ITileEntityLootable>();
		if ((selfOrFeature == null || !selfOrFeature.IsRemoving) && selfOrFeature == this.te)
		{
			this.te.SetModified();
		}
		this.te.SetUserAccessing(false);
		LockManager.Instance.UnlockRequestLocal();
	}

	// Token: 0x06007D43 RID: 32067 RVA: 0x002F56B4 File Offset: 0x002F38B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void openTimerFinished(TimerEventData _data)
	{
		ValueTuple<string, ITileEntityLootable> valueTuple = (ValueTuple<string, ITileEntityLootable>)_data.Data;
		string item = valueTuple.Item1;
		ITileEntityLootable item2 = valueTuple.Item2;
		if (!item2.bPlayerStorage)
		{
			this.xui.playerUI.entityPlayer.Progression.AddLevelExp(this.xui.playerUI.entityPlayer.gameStage, "_xpFromLoot", Progression.XPTypes.Looting, true, true, -1, null);
		}
		this.openContainer(item, item2, false);
	}

	// Token: 0x06007D44 RID: 32068 RVA: 0x002F5724 File Offset: 0x002F3924
	[PublicizedFrom(EAccessModifier.Private)]
	public static void openTimerClosedManually(TimerEventData _data)
	{
		ITileEntityLootable item = ((ValueTuple<string, ITileEntityLootable>)_data.Data).Item2;
		if (item.bWasTouched)
		{
			LockManager.Instance.UnlockRequestLocal();
			return;
		}
		Vector3i vector3i = item.ToWorldPos();
		ITileEntityLootable selfOrFeature = GameManager.Instance.World.GetTileEntity(vector3i).GetSelfOrFeature<ITileEntityLootable>();
		if ((selfOrFeature == null || !selfOrFeature.IsRemoving) && selfOrFeature == item)
		{
			item.bTouched = false;
			item.SetModified();
		}
		LockManager.Instance.UnlockRequestLocal();
	}

	// Token: 0x06007D45 RID: 32069 RVA: 0x002F5798 File Offset: 0x002F3998
	public void OpenLooting(string _lootContainerName, ITileEntityLootable _te)
	{
		if (EffectManager.GetValue(PassiveEffects.DisableLoot, null, 0f, this.xui.playerUI.entityPlayer, null, _te.blockValue.Block.Tags, true, true, true, true, true, 1, true, false) > 0f)
		{
			Manager.PlayInsidePlayerHead("twitch_no_attack", -1, 0f, false, false);
			LockManager.Instance.UnlockRequestLocal();
			return;
		}
		if (_te.bWasTouched)
		{
			this.openContainer(_lootContainerName, _te, true);
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
		timerEventData.Data = new ValueTuple<string, ITileEntityLootable>(_lootContainerName, _te);
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

	// Token: 0x06007D46 RID: 32070 RVA: 0x002F5937 File Offset: 0x002F3B37
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_LootWindowGroup GetInstance(XUi _xuiInstance = null)
	{
		if (_xuiInstance == null)
		{
			_xuiInstance = LocalPlayerUI.GetUIForPrimaryPlayer().xui;
		}
		return (XUiC_LootWindowGroup)_xuiInstance.FindWindowGroupByName(XUiC_LootWindowGroup.ID);
	}

	// Token: 0x06007D47 RID: 32071 RVA: 0x002F5958 File Offset: 0x002F3B58
	public static Vector3i GetTeBlockPos(XUi _xuiInstance = null)
	{
		ITileEntityLootable tileEntityLootable = XUiC_LootWindowGroup.GetInstance(_xuiInstance).te;
		if (tileEntityLootable == null)
		{
			return Vector3i.zero;
		}
		return tileEntityLootable.ToWorldPos();
	}

	// Token: 0x06007D48 RID: 32072 RVA: 0x002F5974 File Offset: 0x002F3B74
	public static void CloseIfOpenAtPos(Vector3i _blockPos, XUi _xuiInstance = null)
	{
		GUIWindowManager windowManager = XUiC_LootWindowGroup.GetInstance(_xuiInstance).xui.playerUI.windowManager;
		if (windowManager.IsWindowOpen(XUiC_LootWindowGroup.ID) && XUiC_LootWindowGroup.GetTeBlockPos(null) == _blockPos)
		{
			windowManager.Close(XUiC_LootWindowGroup.ID);
		}
	}

	// Token: 0x04005CBE RID: 23742
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LootWindow lootWindow;

	// Token: 0x04005CBF RID: 23743
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005CC0 RID: 23744
	[PublicizedFrom(EAccessModifier.Private)]
	public ITileEntityLootable te;

	// Token: 0x04005CC1 RID: 23745
	[PublicizedFrom(EAccessModifier.Private)]
	public string lootingHeader;

	// Token: 0x04005CC2 RID: 23746
	public static string ID = "looting";
}
