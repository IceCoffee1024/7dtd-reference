using System;
using UnityEngine;

// Token: 0x020012BB RID: 4795
public class NGuiWdwInGameHUD : MonoBehaviour
{
	// Token: 0x1700123C RID: 4668
	// (get) Token: 0x060097F4 RID: 38900 RVA: 0x00395279 File Offset: 0x00393479
	public GameManager gameManager
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance;
		}
	}

	// Token: 0x060097F5 RID: 38901 RVA: 0x00395280 File Offset: 0x00393480
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.playerUI = base.GetComponentInParent<LocalPlayerUI>();
	}

	// Token: 0x060097F6 RID: 38902 RVA: 0x00395290 File Offset: 0x00393490
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

	// Token: 0x060097F7 RID: 38903 RVA: 0x00395394 File Offset: 0x00393594
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.playerEntity = this.playerUI.entityPlayer;
		this.playerUI.OnEntityPlayerLocalAssigned += this.HandleEntityPlayerLocalAssigned;
	}

	// Token: 0x060097F8 RID: 38904 RVA: 0x003953BE File Offset: 0x003935BE
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.playerUI.OnEntityPlayerLocalAssigned -= this.HandleEntityPlayerLocalAssigned;
	}

	// Token: 0x060097F9 RID: 38905 RVA: 0x003953D7 File Offset: 0x003935D7
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleEntityPlayerLocalAssigned(EntityPlayerLocal _entity)
	{
		this.playerEntity = _entity;
	}

	// Token: 0x060097FA RID: 38906 RVA: 0x003953E0 File Offset: 0x003935E0
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

	// Token: 0x040071CF RID: 29135
	public Texture2D[] overlayDamageTextures = new Texture2D[8];

	// Token: 0x040071D0 RID: 29136
	public Texture2D[] overlayDamageBloodDrops = new Texture2D[3];

	// Token: 0x040071D1 RID: 29137
	public Texture2D CrosshairTexture;

	// Token: 0x040071D2 RID: 29138
	public Texture2D CrosshairDamage;

	// Token: 0x040071D3 RID: 29139
	public Texture2D CrosshairUpgrade;

	// Token: 0x040071D4 RID: 29140
	public Texture2D CrosshairRepair;

	// Token: 0x040071D5 RID: 29141
	public Texture2D CrosshairAiming;

	// Token: 0x040071D6 RID: 29142
	public Texture2D CrosshairPowerSource;

	// Token: 0x040071D7 RID: 29143
	public Texture2D CrosshairPowerItem;

	// Token: 0x040071D8 RID: 29144
	public Texture2D[] StealthIcons = new Texture2D[5];

	// Token: 0x040071D9 RID: 29145
	public Texture2D[] StealthOverlays = new Texture2D[2];

	// Token: 0x040071DA RID: 29146
	public Texture2D CrosshairBlocked;

	// Token: 0x040071DB RID: 29147
	public Texture2D CrosshairDot;

	// Token: 0x040071DC RID: 29148
	public Texture2D CrosshairSquare;

	// Token: 0x040071DD RID: 29149
	public Transform FocusCube;

	// Token: 0x040071DE RID: 29150
	public float crosshairAlpha = 1f;

	// Token: 0x040071DF RID: 29151
	public bool showCrosshair = true;

	// Token: 0x040071E0 RID: 29152
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string wdwOpenedOnGameStatsShowWindow;

	// Token: 0x040071E1 RID: 29153
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal playerEntity;

	// Token: 0x040071E2 RID: 29154
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;
}
