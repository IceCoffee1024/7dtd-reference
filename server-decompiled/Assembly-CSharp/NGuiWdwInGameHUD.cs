using System;
using UnityEngine;

// Token: 0x020012CD RID: 4813
public class NGuiWdwInGameHUD : MonoBehaviour
{
	// Token: 0x17001248 RID: 4680
	// (get) Token: 0x0600984C RID: 38988 RVA: 0x00397681 File Offset: 0x00395881
	public GameManager gameManager
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance;
		}
	}

	// Token: 0x0600984D RID: 38989 RVA: 0x00397688 File Offset: 0x00395888
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.playerUI = base.GetComponentInParent<LocalPlayerUI>();
	}

	// Token: 0x0600984E RID: 38990 RVA: 0x00397698 File Offset: 0x00395898
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		NGuiAction nguiAction = new NGuiAction("DebugSpawn", PlayerActionsGlobal.Instance.DebugSpawn);
		nguiAction.SetClickActionDelegate(delegate
		{
			this.playerUI.windowManager.SwitchVisible(XUiC_SpawnMenu.ID, true);
		});
		nguiAction.SetIsEnabledDelegate(() => GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled));
		NGuiAction nguiAction2 = new NGuiAction("DebugGameEvent", PlayerActionsGlobal.Instance.DebugGameEvent);
		nguiAction2.SetClickActionDelegate(delegate
		{
			this.playerUI.windowManager.SwitchVisible(XUiC_GameEventMenu.ID, true);
		});
		nguiAction2.SetIsEnabledDelegate(() => GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled));
		NGuiAction nguiAction3 = new NGuiAction("SwitchHUD", PlayerActionsGlobal.Instance.SwitchHUD);
		nguiAction3.SetClickActionDelegate(delegate
		{
			this.playerUI.windowManager.ToggleHUDEnabled();
		});
		this.playerUI.windowManager.AddGlobalAction(nguiAction);
		this.playerUI.windowManager.AddGlobalAction(nguiAction2);
		this.playerUI.windowManager.AddGlobalAction(nguiAction3);
	}

	// Token: 0x0600984F RID: 38991 RVA: 0x0039779C File Offset: 0x0039599C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.playerEntity = this.playerUI.entityPlayer;
		this.playerUI.OnEntityPlayerLocalAssigned += this.HandleEntityPlayerLocalAssigned;
	}

	// Token: 0x06009850 RID: 38992 RVA: 0x003977C6 File Offset: 0x003959C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.playerUI.OnEntityPlayerLocalAssigned -= this.HandleEntityPlayerLocalAssigned;
	}

	// Token: 0x06009851 RID: 38993 RVA: 0x003977DF File Offset: 0x003959DF
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleEntityPlayerLocalAssigned(EntityPlayerLocal _entity)
	{
		this.playerEntity = _entity;
	}

	// Token: 0x06009852 RID: 38994 RVA: 0x003977E8 File Offset: 0x003959E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnGUI()
	{
		if (!this.gameManager.gameStateManager.IsGameStarted())
		{
			return;
		}
		string @string = GameStats.GetString(EnumGameStats.ShowWindow);
		if (!string.IsNullOrEmpty(@string))
		{
			if (!this.playerUI.windowManager.IsWindowOpen(@string))
			{
				this.playerUI.windowManager.Open(@string, false);
				this.wdwOpenedOnGameStatsShowWindow = @string;
			}
		}
		else if (this.wdwOpenedOnGameStatsShowWindow != null)
		{
			this.playerUI.windowManager.Close(this.wdwOpenedOnGameStatsShowWindow);
			this.wdwOpenedOnGameStatsShowWindow = null;
		}
		if (this.playerEntity != null)
		{
			this.playerEntity.OnHUD();
		}
		if (!this.playerUI.windowManager.IsHUDEnabled())
		{
			return;
		}
		int @int = GameStats.GetInt(EnumGameStats.GameState);
		if (@int != 1)
		{
		}
	}

	// Token: 0x04007210 RID: 29200
	public Texture2D[] overlayDamageTextures = new Texture2D[8];

	// Token: 0x04007211 RID: 29201
	public Texture2D[] overlayDamageBloodDrops = new Texture2D[3];

	// Token: 0x04007212 RID: 29202
	public Texture2D CrosshairTexture;

	// Token: 0x04007213 RID: 29203
	public Texture2D CrosshairDamage;

	// Token: 0x04007214 RID: 29204
	public Texture2D CrosshairUpgrade;

	// Token: 0x04007215 RID: 29205
	public Texture2D CrosshairRepair;

	// Token: 0x04007216 RID: 29206
	public Texture2D CrosshairAiming;

	// Token: 0x04007217 RID: 29207
	public Texture2D CrosshairPowerSource;

	// Token: 0x04007218 RID: 29208
	public Texture2D CrosshairPowerItem;

	// Token: 0x04007219 RID: 29209
	public Texture2D[] StealthIcons = new Texture2D[5];

	// Token: 0x0400721A RID: 29210
	public Texture2D[] StealthOverlays = new Texture2D[2];

	// Token: 0x0400721B RID: 29211
	public Texture2D CrosshairBlocked;

	// Token: 0x0400721C RID: 29212
	public Texture2D CrosshairDot;

	// Token: 0x0400721D RID: 29213
	public Texture2D CrosshairSquare;

	// Token: 0x0400721E RID: 29214
	public Transform FocusCube;

	// Token: 0x0400721F RID: 29215
	public float crosshairAlpha = 1f;

	// Token: 0x04007220 RID: 29216
	public bool showCrosshair = true;

	// Token: 0x04007221 RID: 29217
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string wdwOpenedOnGameStatsShowWindow;

	// Token: 0x04007222 RID: 29218
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal playerEntity;

	// Token: 0x04007223 RID: 29219
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;
}
