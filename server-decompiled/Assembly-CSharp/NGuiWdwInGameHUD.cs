using System;
using UnityEngine;

// Token: 0x020012A3 RID: 4771
public class NGuiWdwInGameHUD : MonoBehaviour
{
	// Token: 0x170011F8 RID: 4600
	// (get) Token: 0x06009767 RID: 38759 RVA: 0x00390A11 File Offset: 0x0038EC11
	public GameManager gameManager
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance;
		}
	}

	// Token: 0x06009768 RID: 38760 RVA: 0x00390A18 File Offset: 0x0038EC18
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Awake()
	{
		this.playerUI = base.GetComponentInParent<LocalPlayerUI>();
	}

	// Token: 0x06009769 RID: 38761 RVA: 0x00390A28 File Offset: 0x0038EC28
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

	// Token: 0x0600976A RID: 38762 RVA: 0x00390B2C File Offset: 0x0038ED2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.playerEntity = this.playerUI.entityPlayer;
		this.playerUI.OnEntityPlayerLocalAssigned += this.HandleEntityPlayerLocalAssigned;
	}

	// Token: 0x0600976B RID: 38763 RVA: 0x00390B56 File Offset: 0x0038ED56
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.playerUI.OnEntityPlayerLocalAssigned -= this.HandleEntityPlayerLocalAssigned;
	}

	// Token: 0x0600976C RID: 38764 RVA: 0x00390B6F File Offset: 0x0038ED6F
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleEntityPlayerLocalAssigned(EntityPlayerLocal _entity)
	{
		this.playerEntity = _entity;
	}

	// Token: 0x0600976D RID: 38765 RVA: 0x00390B78 File Offset: 0x0038ED78
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

	// Token: 0x04007165 RID: 29029
	public Texture2D[] overlayDamageTextures = new Texture2D[8];

	// Token: 0x04007166 RID: 29030
	public Texture2D[] overlayDamageBloodDrops = new Texture2D[3];

	// Token: 0x04007167 RID: 29031
	public Texture2D CrosshairTexture;

	// Token: 0x04007168 RID: 29032
	public Texture2D CrosshairDamage;

	// Token: 0x04007169 RID: 29033
	public Texture2D CrosshairUpgrade;

	// Token: 0x0400716A RID: 29034
	public Texture2D CrosshairRepair;

	// Token: 0x0400716B RID: 29035
	public Texture2D CrosshairAiming;

	// Token: 0x0400716C RID: 29036
	public Texture2D CrosshairPowerSource;

	// Token: 0x0400716D RID: 29037
	public Texture2D CrosshairPowerItem;

	// Token: 0x0400716E RID: 29038
	public Texture2D[] StealthIcons = new Texture2D[5];

	// Token: 0x0400716F RID: 29039
	public Texture2D[] StealthOverlays = new Texture2D[2];

	// Token: 0x04007170 RID: 29040
	public Texture2D CrosshairBlocked;

	// Token: 0x04007171 RID: 29041
	public Texture2D CrosshairDot;

	// Token: 0x04007172 RID: 29042
	public Texture2D CrosshairSquare;

	// Token: 0x04007173 RID: 29043
	public Transform FocusCube;

	// Token: 0x04007174 RID: 29044
	public float crosshairAlpha = 1f;

	// Token: 0x04007175 RID: 29045
	public bool showCrosshair = true;

	// Token: 0x04007176 RID: 29046
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string wdwOpenedOnGameStatsShowWindow;

	// Token: 0x04007177 RID: 29047
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityPlayerLocal playerEntity;

	// Token: 0x04007178 RID: 29048
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public LocalPlayerUI playerUI;
}
