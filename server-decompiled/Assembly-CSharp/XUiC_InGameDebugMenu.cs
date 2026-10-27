using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F61 RID: 3937
[Preserve]
public class XUiC_InGameDebugMenu : XUiController
{
	// Token: 0x17000E76 RID: 3702
	// (get) Token: 0x06007AE0 RID: 31456 RVA: 0x002E8BED File Offset: 0x002E6DED
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

	// Token: 0x17000E77 RID: 3703
	// (get) Token: 0x06007AE1 RID: 31457 RVA: 0x002E8C1B File Offset: 0x002E6E1B
	public bool HasWorld
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return GameManager.Instance.World != null;
		}
	}

	// Token: 0x06007AE2 RID: 31458 RVA: 0x002E8C2C File Offset: 0x002E6E2C
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

	// Token: 0x06007AE3 RID: 31459 RVA: 0x002E8CAC File Offset: 0x002E6EAC
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

	// Token: 0x06007AE4 RID: 31460 RVA: 0x002E913C File Offset: 0x002E733C
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

	// Token: 0x06007AE5 RID: 31461 RVA: 0x002E91AC File Offset: 0x002E73AC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void TeleportX_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (!int.TryParse(_text, out XUiC_InGameDebugMenu.lastTeleportX))
		{
			XUiC_InGameDebugMenu.lastTeleportX = int.MinValue;
		}
	}

	// Token: 0x06007AE6 RID: 31462 RVA: 0x002E91C5 File Offset: 0x002E73C5
	[PublicizedFrom(EAccessModifier.Private)]
	public static void TeleportZ_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (!int.TryParse(_text, out XUiC_InGameDebugMenu.lastTeleportZ))
		{
			XUiC_InGameDebugMenu.lastTeleportZ = int.MinValue;
		}
	}

	// Token: 0x06007AE7 RID: 31463 RVA: 0x002E91DE File Offset: 0x002E73DE
	[PublicizedFrom(EAccessModifier.Private)]
	public void Teleport_OnSubmitHandler(XUiController _sender, string _text)
	{
		this.BtnTeleport_Controller_OnPress(_sender, -1);
	}

	// Token: 0x06007AE8 RID: 31464 RVA: 0x002E91E8 File Offset: 0x002E73E8
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

	// Token: 0x06007AE9 RID: 31465 RVA: 0x002E9274 File Offset: 0x002E7474
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

	// Token: 0x06007AEA RID: 31466 RVA: 0x002E9444 File Offset: 0x002E7644
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

	// Token: 0x06007AEB RID: 31467 RVA: 0x002E9550 File Offset: 0x002E7750
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

	// Token: 0x06007AEC RID: 31468 RVA: 0x002E95AE File Offset: 0x002E77AE
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleFlyMode_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		if (this.xui.playerUI.entityPlayer != null)
		{
			this.xui.playerUI.entityPlayer.IsFlyMode.Value = _newValue;
		}
	}

	// Token: 0x06007AED RID: 31469 RVA: 0x002E95E4 File Offset: 0x002E77E4
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

	// Token: 0x06007AEE RID: 31470 RVA: 0x002E9626 File Offset: 0x002E7826
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleNoCollisionMode_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		if (this.xui.playerUI.entityPlayer != null)
		{
			this.xui.playerUI.entityPlayer.IsNoCollisionMode.Value = _newValue;
		}
	}

	// Token: 0x06007AEF RID: 31471 RVA: 0x002E965B File Offset: 0x002E785B
	[PublicizedFrom(EAccessModifier.Private)]
	public void toggleInvisibleModeOnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		if (this.xui.playerUI.entityPlayer != null)
		{
			this.xui.playerUI.entityPlayer.IsSpectator = _newValue;
		}
	}

	// Token: 0x06007AF0 RID: 31472 RVA: 0x002E968B File Offset: 0x002E788B
	[PublicizedFrom(EAccessModifier.Private)]
	public void TogglePhysics_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		GameManager.bPhysicsActive = _newValue;
	}

	// Token: 0x06007AF1 RID: 31473 RVA: 0x002E9693 File Offset: 0x002E7893
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleWaterSim_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		WaterSimulationNative.Instance.SetPaused(!_newValue);
	}

	// Token: 0x06007AF2 RID: 31474 RVA: 0x002E96A3 File Offset: 0x002E78A3
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleDebugShaders_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		MeshDescription.SetDebugStabilityShader(_newValue);
	}

	// Token: 0x06007AF3 RID: 31475 RVA: 0x002E96AB File Offset: 0x002E78AB
	[PublicizedFrom(EAccessModifier.Private)]
	public void ToggleLightPerformance_OnValueChanged(XUiC_ToggleButton _sender, bool _newValue)
	{
		LightViewer.SetEnabled(_newValue);
	}

	// Token: 0x06007AF4 RID: 31476 RVA: 0x002E96B4 File Offset: 0x002E78B4
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

	// Token: 0x06007AF5 RID: 31477 RVA: 0x002E9702 File Offset: 0x002E7902
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

	// Token: 0x06007AF6 RID: 31478 RVA: 0x002E973C File Offset: 0x002E793C
	[PublicizedFrom(EAccessModifier.Private)]
	public void startPlaytest(XUiC_SaveDirtyPrefab.ESelectedAction _action)
	{
		if (_action == XUiC_SaveDirtyPrefab.ESelectedAction.Cancel)
		{
			return;
		}
		GameUtils.StartPlaytesting(GameUtils.IsPlaytesting() ? GamePrefs.GetString(EnumGamePrefs.GameName) : GamePrefs.GetString(EnumGamePrefs.LastLoadedPrefab));
	}

	// Token: 0x06007AF7 RID: 31479 RVA: 0x002E9762 File Offset: 0x002E7962
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnBackToEditorOnPressed(XUiController _sender, int _mouseButton)
	{
		GameUtils.StartSinglePrefabEditing(null);
	}

	// Token: 0x06007AF8 RID: 31480 RVA: 0x002E976C File Offset: 0x002E796C
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

	// Token: 0x04005B47 RID: 23367
	[PublicizedFrom(EAccessModifier.Private)]
	public static int lastTeleportX;

	// Token: 0x04005B48 RID: 23368
	[PublicizedFrom(EAccessModifier.Private)]
	public static int lastTeleportZ;

	// Token: 0x04005B49 RID: 23369
	public static string ID = "";

	// Token: 0x04005B4A RID: 23370
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton togglePhysics;

	// Token: 0x04005B4B RID: 23371
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleWaterSim;

	// Token: 0x04005B4C RID: 23372
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleDebugShaders;

	// Token: 0x04005B4D RID: 23373
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleLightPerformance;

	// Token: 0x04005B4E RID: 23374
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleStabilityGlue;

	// Token: 0x04005B4F RID: 23375
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleFlyMode;

	// Token: 0x04005B50 RID: 23376
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleGodMode;

	// Token: 0x04005B51 RID: 23377
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleNoCollisionMode;

	// Token: 0x04005B52 RID: 23378
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ToggleButton toggleInvisibleMode;

	// Token: 0x04005B53 RID: 23379
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showCamPositions;
}
