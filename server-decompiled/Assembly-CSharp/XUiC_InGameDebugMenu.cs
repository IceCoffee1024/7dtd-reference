using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F2C RID: 3884
[Preserve]
public class XUiC_InGameDebugMenu : XUiController
{
	// Token: 0x17000E34 RID: 3636
	// (get) Token: 0x060079E7 RID: 31207 RVA: 0x002DFFBD File Offset: 0x002DE1BD
	public ChunkCluster ChunkCluster0
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (GameManager.Instance == null)
			{
				return null;
			}
			if (GameManager.Instance.World != null)
			{
				return GameManager.Instance.World.ChunkCache;
			}
			return null;
		}
	}

	// Token: 0x17000E35 RID: 3637
	// (get) Token: 0x060079E8 RID: 31208 RVA: 0x002DFFEB File Offset: 0x002DE1EB
	public bool HasWorld
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance.World != null;
		}
	}

	// Token: 0x060079E9 RID: 31209 RVA: 0x002DFFFC File Offset: 0x002DE1FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		if (_bindingName == "decorationsState")
		{
			if (this.HasWorld)
			{
				_value = ((this.ChunkCluster0 != null && this.ChunkCluster0.ChunkProvider.IsDecorationsEnabled()) ? "On" : "Off");
			}
			else
			{
				_value = "N/A";
			}
			return true;
		}
		if (!(_bindingName == "cam_positions_open"))
		{
			return base.GetBindingValueInternal(ref _value, _bindingName);
		}
		_value = this.showCamPositions.ToString();
		return true;
	}

	// Token: 0x060079EA RID: 31210 RVA: 0x002E007C File Offset: 0x002DE27C
	public override void Init()
	{
		base.Init();
		XUiC_InGameDebugMenu.ID = base.WindowGroup.Id;
		XUiC_SimpleButton xuiC_SimpleButton = base.GetChildById("btnSuicide") as XUiC_SimpleButton;
		if (xuiC_SimpleButton != null)
		{
			xuiC_SimpleButton.OnPressed += this.BtnSuicide_Controller_OnPress;
		}
		XUiC_TextInput xuiC_TextInput = base.GetChildById("teleportX") as XUiC_TextInput;
		XUiC_TextInput xuiC_TextInput2 = base.GetChildById("teleportZ") as XUiC_TextInput;
		if (xuiC_TextInput != null)
		{
			xuiC_TextInput.OnChangeHandler += XUiC_InGameDebugMenu.TeleportX_OnChangeHandler;
			xuiC_TextInput.OnSubmitHandler += this.Teleport_OnSubmitHandler;
			xuiC_TextInput.SelectOnTab = xuiC_TextInput2;
			xuiC_TextInput.Text = XUiC_InGameDebugMenu.lastTeleportX.ToString();
		}
		if (xuiC_TextInput2 != null)
		{
			xuiC_TextInput2.OnChangeHandler += XUiC_InGameDebugMenu.TeleportZ_OnChangeHandler;
			xuiC_TextInput2.OnSubmitHandler += this.Teleport_OnSubmitHandler;
			xuiC_TextInput2.SelectOnTab = xuiC_TextInput;
			xuiC_TextInput2.Text = XUiC_InGameDebugMenu.lastTeleportZ.ToString();
		}
		XUiC_SimpleButton xuiC_SimpleButton2 = base.GetChildById("btnTeleport") as XUiC_SimpleButton;
		if (xuiC_SimpleButton2 != null)
		{
			xuiC_SimpleButton2.OnPressed += this.BtnTeleport_Controller_OnPress;
		}
		XUiC_SimpleButton xuiC_SimpleButton3 = base.GetChildById("btnRecalcLight") as XUiC_SimpleButton;
		if (xuiC_SimpleButton3 != null)
		{
			xuiC_SimpleButton3.OnPressed += this.BtnRecalcLight_Controller_OnPress;
		}
		XUiC_SimpleButton xuiC_SimpleButton4 = base.GetChildById("btnRecalcStability") as XUiC_SimpleButton;
		if (xuiC_SimpleButton4 != null)
		{
			xuiC_SimpleButton4.OnPressed += this.BtnRecalcStability_Controller_OnPress;
		}
		XUiC_SimpleButton xuiC_SimpleButton5 = base.GetChildById("btnReloadChunks") as XUiC_SimpleButton;
		if (xuiC_SimpleButton5 != null)
		{
			xuiC_SimpleButton5.OnPressed += this.BtnReloadChunks_Controller_OnPress;
		}
		XUiController childById = base.GetChildById("toggleFlyMode");
		this.toggleFlyMode = ((childById != null) ? childById.GetChildByType<XUiC_ToggleButton>() : null);
		if (this.toggleFlyMode != null)
		{
			this.toggleFlyMode.OnValueChanged += this.ToggleFlyMode_OnValueChanged;
		}
		XUiController childById2 = base.GetChildById("toggleGodMode");
		this.toggleGodMode = ((childById2 != null) ? childById2.GetChildByType<XUiC_ToggleButton>() : null);
		if (this.toggleGodMode != null)
		{
			this.toggleGodMode.OnValueChanged += this.ToggleGodMode_OnValueChanged;
		}
		XUiController childById3 = base.GetChildById("toggleNoCollisionMode");
		this.toggleNoCollisionMode = ((childById3 != null) ? childById3.GetChildByType<XUiC_ToggleButton>() : null);
		if (this.toggleNoCollisionMode != null)
		{
			this.toggleNoCollisionMode.OnValueChanged += this.ToggleNoCollisionMode_OnValueChanged;
		}
		XUiController childById4 = base.GetChildById("toggleInvisibleMode");
		this.toggleInvisibleMode = ((childById4 != null) ? childById4.GetChildByType<XUiC_ToggleButton>() : null);
		if (this.toggleInvisibleMode != null)
		{
			this.toggleInvisibleMode.OnValueChanged += this.toggleInvisibleModeOnValueChanged;
		}
		XUiController childById5 = base.GetChildById("togglePhysics");
		this.togglePhysics = ((childById5 != null) ? childById5.GetChildByType<XUiC_ToggleButton>() : null);
		if (this.togglePhysics != null)
		{
			this.togglePhysics.OnValueChanged += this.TogglePhysics_OnValueChanged;
		}
		XUiController childById6 = base.GetChildById("toggleWaterSim");
		this.toggleWaterSim = ((childById6 != null) ? childById6.GetChildByType<XUiC_ToggleButton>() : null);
		if (this.toggleWaterSim != null)
		{
			this.toggleWaterSim.OnValueChanged += this.ToggleWaterSim_OnValueChanged;
		}
		XUiController childById7 = base.GetChildById("toggleDebugShaders");
		this.toggleDebugShaders = ((childById7 != null) ? childById7.GetChildByType<XUiC_ToggleButton>() : null);
		if (this.toggleDebugShaders != null)
		{
			this.toggleDebugShaders.OnValueChanged += this.ToggleDebugShaders_OnValueChanged;
		}
		XUiController childById8 = base.GetChildById("toggleLightPerformance");
		this.toggleLightPerformance = ((childById8 != null) ? childById8.GetChildByType<XUiC_ToggleButton>() : null);
		if (this.toggleLightPerformance != null)
		{
			this.toggleLightPerformance.OnValueChanged += this.ToggleLightPerformance_OnValueChanged;
		}
		XUiController childById9 = base.GetChildById("toggleStabilityGlue");
		this.toggleStabilityGlue = ((childById9 != null) ? childById9.GetChildByType<XUiC_ToggleButton>() : null);
		if (this.toggleStabilityGlue != null)
		{
			this.toggleStabilityGlue.OnValueChanged += this.ToggleStabilityGlue_OnValueChanged;
		}
		XUiC_SimpleButton xuiC_SimpleButton6 = base.GetChildById("btnPlaytest") as XUiC_SimpleButton;
		if (xuiC_SimpleButton6 != null)
		{
			xuiC_SimpleButton6.OnPressed += this.BtnPlaytestOnPressed;
		}
		XUiC_SimpleButton xuiC_SimpleButton7 = base.GetChildById("btnBackToEditor") as XUiC_SimpleButton;
		if (xuiC_SimpleButton7 != null)
		{
			xuiC_SimpleButton7.OnPressed += this.BtnBackToEditorOnPressed;
		}
		XUiController childById10 = base.GetChildById("toggleCamPositions");
		XUiC_ToggleButton xuiC_ToggleButton = ((childById10 != null) ? childById10.GetChildByType<XUiC_ToggleButton>() : null);
		if (xuiC_ToggleButton != null)
		{
			xuiC_ToggleButton.OnValueChanged += delegate(XUiC_ToggleButton _, bool _value)
			{
				this.showCamPositions = _value;
				base.RefreshBindings();
			};
		}
		XUiC_ComboBoxList<BiomeDefinition.BiomeType> xuiC_ComboBoxList = base.GetChildById("cbxPlaytestBiome") as XUiC_ComboBoxList<BiomeDefinition.BiomeType>;
		if (xuiC_ComboBoxList != null)
		{
			xuiC_ComboBoxList.Elements.AddRange(new BiomeDefinition.BiomeType[]
			{
				BiomeDefinition.BiomeType.Snow,
				BiomeDefinition.BiomeType.PineForest,
				BiomeDefinition.BiomeType.Desert,
				BiomeDefinition.BiomeType.Wasteland,
				BiomeDefinition.BiomeType.burnt_forest
			});
			xuiC_ComboBoxList.Value = (BiomeDefinition.BiomeType)GamePrefs.GetInt(EnumGamePrefs.PlaytestBiome);
			xuiC_ComboBoxList.OnValueChanged += delegate(XUiController _, BiomeDefinition.BiomeType _, BiomeDefinition.BiomeType _newValue)
			{
				GamePrefs.Set(EnumGamePrefs.PlaytestBiome, (int)_newValue);
			};
		}
	}

	// Token: 0x060079EB RID: 31211 RVA: 0x002E050C File Offset: 0x002DE70C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSuicide_Controller_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.xui.playerUI.entityPlayer == null)
		{
			return;
		}
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		this.xui.playerUI.entityPlayer.Kill(DamageResponse.New(new DamageSource(EnumDamageSource.Internal, EnumDamageTypes.Suicide), true));
		GameManager.Instance.Pause(false);
	}

	// Token: 0x060079EC RID: 31212 RVA: 0x002E057C File Offset: 0x002DE77C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void TeleportX_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (!int.TryParse(_text, out XUiC_InGameDebugMenu.lastTeleportX))
		{
			XUiC_InGameDebugMenu.lastTeleportX = int.MinValue;
		}
	}

	// Token: 0x060079ED RID: 31213 RVA: 0x002E0595 File Offset: 0x002DE795
	[PublicizedFrom(EAccessModifier.Private)]
	public static void TeleportZ_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (!int.TryParse(_text, out XUiC_InGameDebugMenu.lastTeleportZ))
		{
			XUiC_InGameDebugMenu.lastTeleportZ = int.MinValue;
		}
	}

	// Token: 0x060079EE RID: 31214 RVA: 0x002E05AE File Offset: 0x002DE7AE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Teleport_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnTeleport_Controller_OnPress(_sender, -1);
	}

	// Token: 0x060079EF RID: 31215 RVA: 0x002E05B8 File Offset: 0x002DE7B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnTeleport_Controller_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.xui.playerUI.entityPlayer == null)
		{
			return;
		}
		if (XUiC_InGameDebugMenu.lastTeleportX == -2147483648 || XUiC_InGameDebugMenu.lastTeleportZ == -2147483648)
		{
			return;
		}
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
		this.xui.playerUI.entityPlayer.Teleport(new Vector3((float)XUiC_InGameDebugMenu.lastTeleportX, 240f, (float)XUiC_InGameDebugMenu.lastTeleportZ), float.MinValue);
	}

	// Token: 0x060079F0 RID: 31216 RVA: 0x002E0644 File Offset: 0x002DE844
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRecalcLight_Controller_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.ChunkCluster0 == null)
		{
			return;
		}
		LightProcessor lightProcessor = new LightProcessor(GameManager.Instance.World);
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		List<Chunk> chunkArrayCopySync = this.ChunkCluster0.GetChunkArrayCopySync();
		foreach (Chunk chunk in chunkArrayCopySync)
		{
			chunk.ResetLights(0);
			chunk.RefreshSunlight();
		}
		foreach (Chunk chunk2 in chunkArrayCopySync)
		{
			lightProcessor.GenerateSunlight(chunk2, false);
		}
		foreach (Chunk chunk3 in chunkArrayCopySync)
		{
			lightProcessor.GenerateSunlight(chunk3, true);
		}
		foreach (Chunk chunk4 in chunkArrayCopySync)
		{
			lightProcessor.LightChunk(chunk4);
		}
		stopwatch.Stop();
		foreach (Chunk chunk5 in chunkArrayCopySync)
		{
			chunk5.NeedsRegeneration = true;
		}
		Log.Out(string.Concat(new string[]
		{
			"#",
			chunkArrayCopySync.Count.ToString(),
			" chunks needed ",
			stopwatch.ElapsedMilliseconds.ToString(),
			"ms"
		}));
	}

	// Token: 0x060079F1 RID: 31217 RVA: 0x002E0814 File Offset: 0x002DEA14
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRecalcStability_Controller_OnPress(XUiController _sender, int _mouseButton)
	{
		ChunkCluster chunkCluster = this.ChunkCluster0;
		if (chunkCluster == null)
		{
			return;
		}
		StabilityInitializer stabilityInitializer = new StabilityInitializer(GameManager.Instance.World);
		MicroStopwatch microStopwatch = new MicroStopwatch();
		foreach (Chunk chunk in chunkCluster.GetChunkArray())
		{
			chunk.ResetStabilityToBottomMost();
		}
		Log.Out("RecalcStability reset in {0}ms", new object[] { microStopwatch.ElapsedMilliseconds });
		foreach (Chunk chunk2 in chunkCluster.GetChunkArray())
		{
			stabilityInitializer.DistributeStability(chunk2);
			chunk2.NeedsRegeneration = true;
		}
		Log.Out("RecalcStability #{0} in {1}ms", new object[]
		{
			chunkCluster.GetChunkArray().Count,
			microStopwatch.ElapsedMilliseconds
		});
	}

	// Token: 0x060079F2 RID: 31218 RVA: 0x002E0920 File Offset: 0x002DEB20
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnReloadChunks_Controller_OnPress(XUiController _sender, int _mouseButton)
	{
		if (this.ChunkCluster0 == null)
		{
			return;
		}
		GameManager.Instance.World.m_ChunkManager.ReloadAllChunks();
		this.ChunkCluster0.ChunkProvider.ReloadAllChunks();
		GameManager.Instance.World.UnloadEntities(GameManager.Instance.World.Entities.list, false);
	}

	// Token: 0x060079F3 RID: 31219 RVA: 0x002E097E File Offset: 0x002DEB7E
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleFlyMode_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		if (this.xui.playerUI.entityPlayer != null)
		{
			this.xui.playerUI.entityPlayer.IsFlyMode.Value = _newValue;
		}
	}

	// Token: 0x060079F4 RID: 31220 RVA: 0x002E09B4 File Offset: 0x002DEBB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleGodMode_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (entityPlayer == null)
		{
			return;
		}
		PlayerMoveController moveController = entityPlayer.MoveController;
		if (moveController == null)
		{
			return;
		}
		Action action = moveController.toggleGodMode;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x060079F5 RID: 31221 RVA: 0x002E09F6 File Offset: 0x002DEBF6
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleNoCollisionMode_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		if (this.xui.playerUI.entityPlayer != null)
		{
			this.xui.playerUI.entityPlayer.IsNoCollisionMode.Value = _newValue;
		}
	}

	// Token: 0x060079F6 RID: 31222 RVA: 0x002E0A2B File Offset: 0x002DEC2B
	[PublicizedFrom(EAccessModifier.Private)]
	public void toggleInvisibleModeOnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		if (this.xui.playerUI.entityPlayer != null)
		{
			this.xui.playerUI.entityPlayer.IsSpectator = _newValue;
		}
	}

	// Token: 0x060079F7 RID: 31223 RVA: 0x002E0A5B File Offset: 0x002DEC5B
	[PublicizedFrom(EAccessModifier.Private)]
	public void TogglePhysics_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		GameManager.bPhysicsActive = _newValue;
	}

	// Token: 0x060079F8 RID: 31224 RVA: 0x002E0A63 File Offset: 0x002DEC63
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleWaterSim_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		WaterSimulationNative.Instance.SetPaused(!_newValue);
	}

	// Token: 0x060079F9 RID: 31225 RVA: 0x002E0A73 File Offset: 0x002DEC73
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleDebugShaders_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		MeshDescription.SetDebugStabilityShader(_newValue);
	}

	// Token: 0x060079FA RID: 31226 RVA: 0x002E0A7B File Offset: 0x002DEC7B
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleLightPerformance_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		LightViewer.SetEnabled(_newValue);
	}

	// Token: 0x060079FB RID: 31227 RVA: 0x002E0A84 File Offset: 0x002DEC84
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleStabilityGlue_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		if (_newValue)
		{
			if (GameManager.Instance.stabilityViewer == null)
			{
				GameManager.Instance.CreateStabilityViewer();
				GameManager.Instance.stabilityViewer.StartSearch(100);
				return;
			}
		}
		else if (GameManager.Instance.stabilityViewer != null)
		{
			GameManager.Instance.ClearStabilityViewer();
		}
	}

	// Token: 0x060079FC RID: 31228 RVA: 0x002E0AD2 File Offset: 0x002DECD2
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnPlaytestOnPressed(XUiController _sender, int _mouseButton)
	{
		if (PrefabEditModeManager.Instance.IsActive() && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			XUiC_SaveDirtyPrefab.Show(this.xui, new Action<XUiC_SaveDirtyPrefab.ESelectedAction>(this.startPlaytest), XUiC_SaveDirtyPrefab.EMode.AskSaveIfDirty);
			return;
		}
		this.startPlaytest(XUiC_SaveDirtyPrefab.ESelectedAction.DontSave);
	}

	// Token: 0x060079FD RID: 31229 RVA: 0x002E0B0C File Offset: 0x002DED0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void startPlaytest(XUiC_SaveDirtyPrefab.ESelectedAction _action)
	{
		if (_action == XUiC_SaveDirtyPrefab.ESelectedAction.Cancel)
		{
			return;
		}
		GameUtils.StartPlaytesting(GameUtils.IsPlaytesting() ? GamePrefs.GetString(EnumGamePrefs.GameName) : GamePrefs.GetString(EnumGamePrefs.LastLoadedPrefab));
	}

	// Token: 0x060079FE RID: 31230 RVA: 0x002E0B32 File Offset: 0x002DED32
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBackToEditorOnPressed(XUiController _sender, int _mouseButton)
	{
		GameUtils.StartSinglePrefabEditing(null);
	}

	// Token: 0x060079FF RID: 31231 RVA: 0x002E0B3C File Offset: 0x002DED3C
	public override void Update(float _dt)
	{
		base.Update(_dt);
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (this.toggleFlyMode != null)
		{
			this.toggleFlyMode.Value = entityPlayer != null && entityPlayer.IsFlyMode.Value;
		}
		if (this.toggleGodMode != null)
		{
			this.toggleGodMode.Value = entityPlayer != null && entityPlayer.IsGodMode.Value;
		}
		if (this.toggleNoCollisionMode != null)
		{
			this.toggleNoCollisionMode.Value = entityPlayer != null && entityPlayer.IsNoCollisionMode.Value;
		}
		if (this.toggleInvisibleMode != null)
		{
			this.toggleInvisibleMode.Value = entityPlayer != null && entityPlayer.IsSpectator;
		}
		if (this.togglePhysics != null)
		{
			this.togglePhysics.Value = GameManager.bPhysicsActive;
		}
		if (this.toggleWaterSim != null)
		{
			this.toggleWaterSim.Value = !WaterSimulationNative.Instance.IsPaused;
		}
		if (this.toggleDebugShaders != null)
		{
			this.toggleDebugShaders.Value = MeshDescription.bDebugStability;
		}
		if (this.toggleLightPerformance != null)
		{
			this.toggleLightPerformance.Value = LightViewer.IsEnabled;
		}
		base.RefreshBindings();
	}

	// Token: 0x04005A29 RID: 23081
	[PublicizedFrom(EAccessModifier.Private)]
	public static int lastTeleportX;

	// Token: 0x04005A2A RID: 23082
	[PublicizedFrom(EAccessModifier.Private)]
	public static int lastTeleportZ;

	// Token: 0x04005A2B RID: 23083
	public static string ID = "";

	// Token: 0x04005A2C RID: 23084
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton togglePhysics;

	// Token: 0x04005A2D RID: 23085
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleWaterSim;

	// Token: 0x04005A2E RID: 23086
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleDebugShaders;

	// Token: 0x04005A2F RID: 23087
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleLightPerformance;

	// Token: 0x04005A30 RID: 23088
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleStabilityGlue;

	// Token: 0x04005A31 RID: 23089
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleFlyMode;

	// Token: 0x04005A32 RID: 23090
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleGodMode;

	// Token: 0x04005A33 RID: 23091
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleNoCollisionMode;

	// Token: 0x04005A34 RID: 23092
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleInvisibleMode;

	// Token: 0x04005A35 RID: 23093
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showCamPositions;
}
