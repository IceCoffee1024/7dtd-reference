using System;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F66 RID: 3942
[Preserve]
public class XUiC_LootWindowGroup : XUiController
{
	// Token: 0x06007C4E RID: 31822 RVA: 0x002F0DA5 File Offset: 0x002EEFA5
	public override void Init()
	{
		base.Init();
		this.lootWindow = base.GetChildByType<XUiC_LootWindow>();
		this.nonPagingHeaderWindow = base.GetChildByType<XUiC_WindowNonPagingHeader>();
	}

	// Token: 0x06007C4F RID: 31823 RVA: 0x002F0DC8 File Offset: 0x002EEFC8
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

	// Token: 0x06007C50 RID: 31824 RVA: 0x002F0E5C File Offset: 0x002EF05C
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

	// Token: 0x06007C51 RID: 31825 RVA: 0x002F0EE0 File Offset: 0x002EF0E0
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

	// Token: 0x06007C52 RID: 31826 RVA: 0x002F0F50 File Offset: 0x002EF150
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

	// Token: 0x06007C53 RID: 31827 RVA: 0x002F0FC4 File Offset: 0x002EF1C4
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

	// Token: 0x06007C54 RID: 31828 RVA: 0x002F1163 File Offset: 0x002EF363
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_LootWindowGroup GetInstance(XUi _xuiInstance = null)
	{
		if (_xuiInstance == null)
		{
			_xuiInstance = LocalPlayerUI.GetUIForPrimaryPlayer().xui;
		}
		return (XUiC_LootWindowGroup)_xuiInstance.FindWindowGroupByName(XUiC_LootWindowGroup.ID);
	}

	// Token: 0x06007C55 RID: 31829 RVA: 0x002F1184 File Offset: 0x002EF384
	public static Vector3i GetTeBlockPos(XUi _xuiInstance = null)
	{
		ITileEntityLootable tileEntityLootable = XUiC_LootWindowGroup.GetInstance(_xuiInstance).te;
		if (tileEntityLootable == null)
		{
			return Vector3i.zero;
		}
		return tileEntityLootable.ToWorldPos();
	}

	// Token: 0x06007C56 RID: 31830 RVA: 0x002F11A0 File Offset: 0x002EF3A0
	public static void CloseIfOpenAtPos(Vector3i _blockPos, XUi _xuiInstance = null)
	{
		GUIWindowManager windowManager = XUiC_LootWindowGroup.GetInstance(_xuiInstance).xui.playerUI.windowManager;
		if (windowManager.IsWindowOpen(XUiC_LootWindowGroup.ID) && XUiC_LootWindowGroup.GetTeBlockPos(null) == _blockPos)
		{
			windowManager.Close(XUiC_LootWindowGroup.ID);
		}
	}

	// Token: 0x04005BF7 RID: 23543
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LootWindow lootWindow;

	// Token: 0x04005BF8 RID: 23544
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_WindowNonPagingHeader nonPagingHeaderWindow;

	// Token: 0x04005BF9 RID: 23545
	[PublicizedFrom(EAccessModifier.Private)]
	public ITileEntityLootable te;

	// Token: 0x04005BFA RID: 23546
	[PublicizedFrom(EAccessModifier.Private)]
	public string lootingHeader;

	// Token: 0x04005BFB RID: 23547
	public static string ID = "looting";
}
