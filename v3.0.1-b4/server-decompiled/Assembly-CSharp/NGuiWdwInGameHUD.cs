using System;
using UnityEngine;

// Token: 0x02001289 RID: 4745
public class NGuiWdwInGameHUD : MonoBehaviour
{
	// Token: 0x170011BF RID: 4543
	// (get) Token: 0x0600966C RID: 38508 RVA: 0x0038B8B5 File Offset: 0x00389AB5
	public GameManager gameManager
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance;
		}
	}

	// Token: 0x0600966D RID: 38509 RVA: 0x0038B8BC File Offset: 0x00389ABC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.playerUI = base.GetComponentInParent<LocalPlayerUI>();
	}

	// Token: 0x0600966E RID: 38510 RVA: 0x0038B8CC File Offset: 0x00389ACC
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

	// Token: 0x0600966F RID: 38511 RVA: 0x0038B9D0 File Offset: 0x00389BD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.playerEntity = this.playerUI.entityPlayer;
		this.playerUI.OnEntityPlayerLocalAssigned += this.HandleEntityPlayerLocalAssigned;
	}

	// Token: 0x06009670 RID: 38512 RVA: 0x0038B9FA File Offset: 0x00389BFA
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.playerUI.OnEntityPlayerLocalAssigned -= this.HandleEntityPlayerLocalAssigned;
	}

	// Token: 0x06009671 RID: 38513 RVA: 0x0038BA13 File Offset: 0x00389C13
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleEntityPlayerLocalAssigned(EntityPlayerLocal _entity)
	{
		this.playerEntity = _entity;
	}

	// Token: 0x06009672 RID: 38514 RVA: 0x0038BA1C File Offset: 0x00389C1C
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

	// Token: 0x04007090 RID: 28816
	public Texture2D[] overlayDamageTextures = new Texture2D[8];

	// Token: 0x04007091 RID: 28817
	public Texture2D[] overlayDamageBloodDrops = new Texture2D[3];

	// Token: 0x04007092 RID: 28818
	public Texture2D CrosshairTexture;

	// Token: 0x04007093 RID: 28819
	public Texture2D CrosshairDamage;

	// Token: 0x04007094 RID: 28820
	public Texture2D CrosshairUpgrade;

	// Token: 0x04007095 RID: 28821
	public Texture2D CrosshairRepair;

	// Token: 0x04007096 RID: 28822
	public Texture2D CrosshairAiming;

	// Token: 0x04007097 RID: 28823
	public Texture2D CrosshairPowerSource;

	// Token: 0x04007098 RID: 28824
	public Texture2D CrosshairPowerItem;

	// Token: 0x04007099 RID: 28825
	public Texture2D[] StealthIcons = new Texture2D[5];

	// Token: 0x0400709A RID: 28826
	public Texture2D[] StealthOverlays = new Texture2D[2];

	// Token: 0x0400709B RID: 28827
	public Texture2D CrosshairBlocked;

	// Token: 0x0400709C RID: 28828
	public Texture2D CrosshairDot;

	// Token: 0x0400709D RID: 28829
	public Texture2D CrosshairSquare;

	// Token: 0x0400709E RID: 28830
	public Transform FocusCube;

	// Token: 0x0400709F RID: 28831
	public float crosshairAlpha = 1f;

	// Token: 0x040070A0 RID: 28832
	public bool showCrosshair = true;

	// Token: 0x040070A1 RID: 28833
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string wdwOpenedOnGameStatsShowWindow;

	// Token: 0x040070A2 RID: 28834
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal playerEntity;

	// Token: 0x040070A3 RID: 28835
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;
}
