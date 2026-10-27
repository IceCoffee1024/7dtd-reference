using System;
using System.Collections.Generic;

// Token: 0x02001203 RID: 4611
public static class GameOptionsReset
{
	// Token: 0x0600933C RID: 37692 RVA: 0x003792CC File Offset: 0x003774CC
	public static void Init()
	{
		GameOptionsReset.Audio = new GameOptionsReset.AudioGroup();
		GameOptionsReset.Graphics = new GameOptionsReset.GraphicsGroup();
		GameOptionsReset.Controls = new GameOptionsReset.ControlsGroup();
		GameOptionsReset.Controller = new GameOptionsReset.ControllerGroup();
		GameOptionsReset.Bindings = new GameOptionsReset.BindingsGroup();
		GameOptionsReset.groups = new List<GameOptionsReset.IGroup>
		{
			GameOptionsReset.Audio,
			GameOptionsReset.Graphics,
			GameOptionsReset.Controls,
			GameOptionsReset.Controller,
			GameOptionsReset.Bindings
		};
	}

	// Token: 0x0600933D RID: 37693 RVA: 0x0037934C File Offset: 0x0037754C
	public static string GetGroupId(EnumGamePrefs enumGamePref)
	{
		return GameOptionsReset.GetGroupId(enumGamePref.ToStringCached<EnumGamePrefs>());
	}

	// Token: 0x0600933E RID: 37694 RVA: 0x0037935C File Offset: 0x0037755C
	public static string GetGroupId(string prefName)
	{
		foreach (GameOptionsReset.IGroup group in GameOptionsReset.groups)
		{
			if (group.HasPref(prefName))
			{
				return group.VersionId;
			}
		}
		return GameOptionsReset.defaultGroupId;
	}

	// Token: 0x0600933F RID: 37695 RVA: 0x003793C0 File Offset: 0x003775C0
	[PublicizedFrom(EAccessModifier.Private)]
	public static PrefType EnumTypeToPrefType(GamePrefs.EnumType enumType)
	{
		switch (enumType)
		{
		case GamePrefs.EnumType.Int:
			return PrefType.Int;
		case GamePrefs.EnumType.Float:
			return PrefType.Float;
		case GamePrefs.EnumType.String:
			return PrefType.String;
		case GamePrefs.EnumType.Bool:
			return PrefType.Int;
		case GamePrefs.EnumType.Binary:
			return PrefType.String;
		default:
			throw new Exception(string.Format("Unmapped {0} for {1}: {2}", "PrefType", "EnumType", enumType));
		}
	}

	// Token: 0x06009340 RID: 37696 RVA: 0x00379414 File Offset: 0x00377614
	public static bool TryGetPrefType(EnumGamePrefs enumGamePref, out PrefType prefType)
	{
		GamePrefs.EnumType? prefType2 = GamePrefs.GetPrefType(enumGamePref);
		if (prefType2 == null)
		{
			Log.Error(string.Format("Unknown enum type for {0}", enumGamePref));
			prefType = PrefType.Float;
			return false;
		}
		prefType = GameOptionsReset.EnumTypeToPrefType(prefType2.Value);
		return true;
	}

	// Token: 0x06009341 RID: 37697 RVA: 0x0037945A File Offset: 0x0037765A
	public static bool NeedsResetGame()
	{
		return 13 != GamePrefs.GetInt(EnumGamePrefs.LastGameResetRevision);
	}

	// Token: 0x06009342 RID: 37698 RVA: 0x0037946C File Offset: 0x0037766C
	public static void ResetGame(PrefVersionStore versionedPrefs)
	{
		GamePrefs.PropertyDecl[] propertyList = GamePrefs.GetPropertyList();
		for (int i = 0; i < propertyList.Length; i++)
		{
			GameOptionsReset.ResetPref(propertyList[i].name, versionedPrefs);
		}
		GameOptionsReset.Graphics.Reset(versionedPrefs);
		GameOptionsReset.Controls.Reset(versionedPrefs);
		GameOptionsReset.Bindings.Reset(versionedPrefs);
		GamePrefs.Set(EnumGamePrefs.LastGameResetRevision, 13);
	}

	// Token: 0x06009343 RID: 37699 RVA: 0x003794CC File Offset: 0x003776CC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ResetPref(EnumGamePrefs enumGamePref, PrefVersionStore versionedPrefs)
	{
		object @default;
		if (versionedPrefs == null || !versionedPrefs.TryGetGamePref(enumGamePref, out @default))
		{
			@default = GamePrefs.GetDefault(enumGamePref);
		}
		GamePrefs.SetObject(enumGamePref, @default);
	}

	// Token: 0x04006C9E RID: 27806
	public static GameOptionsReset.AudioGroup Audio;

	// Token: 0x04006C9F RID: 27807
	public static GameOptionsReset.GraphicsGroup Graphics;

	// Token: 0x04006CA0 RID: 27808
	public static GameOptionsReset.ControlsGroup Controls;

	// Token: 0x04006CA1 RID: 27809
	public static GameOptionsReset.ControllerGroup Controller;

	// Token: 0x04006CA2 RID: 27810
	public static GameOptionsReset.BindingsGroup Bindings;

	// Token: 0x04006CA3 RID: 27811
	public static List<GameOptionsReset.IGroup> groups;

	// Token: 0x04006CA4 RID: 27812
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string defaultGroupId = string.Format("DefaultGroup_{0}", 13);

	// Token: 0x02001204 RID: 4612
	public interface IGroup
	{
		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x06009345 RID: 37701
		string VersionId { get; }

		// Token: 0x06009346 RID: 37702
		bool HasPref(string prefName);

		// Token: 0x06009347 RID: 37703
		bool NeedsReset();

		// Token: 0x06009348 RID: 37704
		void Reset(PrefVersionStore versionedPrefs);
	}

	// Token: 0x02001205 RID: 4613
	public abstract class EnumGamePrefGroup : GameOptionsReset.IGroup
	{
		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x06009349 RID: 37705
		public abstract string VersionId { get; }

		// Token: 0x0600934A RID: 37706 RVA: 0x00379510 File Offset: 0x00377710
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGamePrefGroup(params EnumGamePrefs[] enumGamePrefs)
		{
			foreach (EnumGamePrefs enumGamePrefs2 in enumGamePrefs)
			{
				this.AddEnumGamePref(enumGamePrefs2);
			}
		}

		// Token: 0x0600934B RID: 37707 RVA: 0x00379554 File Offset: 0x00377754
		[PublicizedFrom(EAccessModifier.Protected)]
		public void AddEnumGamePref(EnumGamePrefs enumGamePref)
		{
			if (!this.EnumPrefs.Add(enumGamePref))
			{
				return;
			}
			this.PrefNames.Add(enumGamePref.ToStringCached<EnumGamePrefs>());
		}

		// Token: 0x0600934C RID: 37708 RVA: 0x00379577 File Offset: 0x00377777
		public virtual bool HasPref(string prefName)
		{
			return this.PrefNames.Contains(prefName);
		}

		// Token: 0x0600934D RID: 37709 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool NeedsReset()
		{
			return false;
		}

		// Token: 0x0600934E RID: 37710 RVA: 0x00379588 File Offset: 0x00377788
		public virtual void Reset(PrefVersionStore versionedPrefs)
		{
			foreach (EnumGamePrefs enumGamePrefs in this.EnumPrefs)
			{
				GameOptionsReset.ResetPref(enumGamePrefs, versionedPrefs);
			}
		}

		// Token: 0x04006CA5 RID: 27813
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<EnumGamePrefs> EnumPrefs = new HashSet<EnumGamePrefs>();

		// Token: 0x04006CA6 RID: 27814
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<string> PrefNames = new HashSet<string>();
	}

	// Token: 0x02001206 RID: 4614
	public class AudioGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x0600934F RID: 37711 RVA: 0x003795DC File Offset: 0x003777DC
		public override string VersionId
		{
			get
			{
				return string.Format("Audio_{0}_0", 13);
			}
		}

		// Token: 0x06009350 RID: 37712 RVA: 0x003795EF File Offset: 0x003777EF
		public AudioGroup()
			: base(new EnumGamePrefs[]
			{
				EnumGamePrefs.OptionsAmbientVolumeLevel,
				EnumGamePrefs.OptionsMusicVolumeLevel,
				EnumGamePrefs.OptionsMenuMusicVolumeLevel,
				EnumGamePrefs.OptionsDynamicMusicEnabled,
				EnumGamePrefs.OptionsDynamicMusicDailyTime,
				EnumGamePrefs.OptionsMicVolumeLevel,
				EnumGamePrefs.OptionsVoiceVolumeLevel,
				EnumGamePrefs.OptionsOverallAudioVolumeLevel,
				EnumGamePrefs.OptionsVoiceChatEnabled,
				EnumGamePrefs.OptionsAudioOcclusion,
				EnumGamePrefs.OptionsSubtitlesEnabled
			})
		{
		}

		// Token: 0x06009351 RID: 37713 RVA: 0x00379609 File Offset: 0x00377809
		public override void Reset(PrefVersionStore versionedPrefs)
		{
			Log.Out("Resetting game options AudioGroup");
			base.Reset(versionedPrefs);
		}
	}

	// Token: 0x02001207 RID: 4615
	public class GraphicsGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x06009352 RID: 37714 RVA: 0x0037961C File Offset: 0x0037781C
		public override string VersionId
		{
			get
			{
				return string.Format("Graphics_{0}_{1}", 13, 4);
			}
		}

		// Token: 0x06009353 RID: 37715 RVA: 0x00379638 File Offset: 0x00377838
		public GraphicsGroup()
			: base(new EnumGamePrefs[]
			{
				EnumGamePrefs.OptionsGfxResolution,
				EnumGamePrefs.OptionsGfxVsync,
				EnumGamePrefs.OptionsGfxDynamicMode,
				EnumGamePrefs.OptionsGfxUpscalerMode,
				EnumGamePrefs.OptionsGfxFSRPreset,
				EnumGamePrefs.OptionsGfxDynamicScale,
				EnumGamePrefs.OptionsGfxBrightness,
				EnumGamePrefs.OptionsGfxSignQuality,
				EnumGamePrefs.DynamicMeshEnabled,
				EnumGamePrefs.DynamicMeshDistance,
				EnumGamePrefs.NoGraphicsMode
			})
		{
			base.AddEnumGamePref(EnumGamePrefs.OptionsGfxQualityPreset);
			foreach (EnumGamePrefs enumGamePrefs in GameOptionsManager.QualityPresets.Keys)
			{
				base.AddEnumGamePref(enumGamePrefs);
			}
		}

		// Token: 0x06009354 RID: 37716 RVA: 0x003796B4 File Offset: 0x003778B4
		public override bool NeedsReset()
		{
			return 4 != GamePrefs.GetInt(EnumGamePrefs.OptionsGfxResetRevision);
		}

		// Token: 0x06009355 RID: 37717 RVA: 0x003796C4 File Offset: 0x003778C4
		public override void Reset(PrefVersionStore versionedPrefs)
		{
			Log.Out("Resetting game options GraphicsGroup");
			base.Reset(versionedPrefs);
			GameOptionsPlatforms.GfxPreset gfxPreset = GameOptionsPlatforms.CalcDefaultGfxPreset();
			GamePrefs.Set(EnumGamePrefs.OptionsGfxQualityPreset, (int)gfxPreset);
			GameOptionsManager.SetGraphicsQuality();
			GamePrefs.Set(EnumGamePrefs.OptionsGfxResetRevision, 4);
		}
	}

	// Token: 0x02001208 RID: 4616
	public class ControlsGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x06009356 RID: 37718 RVA: 0x00379700 File Offset: 0x00377900
		public override string VersionId
		{
			get
			{
				return string.Format("Controls_{0}_{1}", 13, 7);
			}
		}

		// Token: 0x06009357 RID: 37719 RVA: 0x00379719 File Offset: 0x00377919
		public ControlsGroup()
			: base(new EnumGamePrefs[]
			{
				EnumGamePrefs.OptionsLookSensitivity,
				EnumGamePrefs.OptionsZoomSensitivity,
				EnumGamePrefs.OptionsZoomAccel,
				EnumGamePrefs.OptionsInvertMouse,
				EnumGamePrefs.OptionsVehicleLookSensitivity,
				EnumGamePrefs.OptionsControlsSprintLock
			})
		{
		}

		// Token: 0x06009358 RID: 37720 RVA: 0x00379732 File Offset: 0x00377932
		public override bool NeedsReset()
		{
			return 7 != GamePrefs.GetInt(EnumGamePrefs.OptionsControlsResetRevision);
		}

		// Token: 0x06009359 RID: 37721 RVA: 0x00379744 File Offset: 0x00377944
		public override void Reset(PrefVersionStore versionedPrefs)
		{
			Log.Out("Resetting game options ControlsGroup");
			base.Reset(versionedPrefs);
			GamePrefs.Set(EnumGamePrefs.OptionsControlsResetRevision, 7);
		}
	}

	// Token: 0x02001209 RID: 4617
	public class ControllerGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x0600935A RID: 37722 RVA: 0x00379762 File Offset: 0x00377962
		public override string VersionId
		{
			get
			{
				return string.Format("Controller_{0}_0", 13);
			}
		}

		// Token: 0x0600935B RID: 37723 RVA: 0x00379775 File Offset: 0x00377975
		public ControllerGroup()
			: base(new EnumGamePrefs[]
			{
				EnumGamePrefs.OptionsAllowController,
				EnumGamePrefs.OptionsControllerVibration,
				EnumGamePrefs.OptionsInterfaceSensitivity,
				EnumGamePrefs.OptionsControllerSensitivityX,
				EnumGamePrefs.OptionsControllerSensitivityY,
				EnumGamePrefs.OptionsControllerLookInvert,
				EnumGamePrefs.OptionsControllerJoystickLayout,
				EnumGamePrefs.OptionsControllerLookAcceleration,
				EnumGamePrefs.OptionsControllerZoomSensitivity,
				EnumGamePrefs.OptionsControllerLookAxisDeadzone,
				EnumGamePrefs.OptionsControllerMoveAxisDeadzone,
				EnumGamePrefs.OptionsControllerCursorSnap,
				EnumGamePrefs.OptionsControllerCursorHoverSensitivity,
				EnumGamePrefs.OptionsControllerVehicleSensitivity,
				EnumGamePrefs.OptionsControllerAimAssists,
				EnumGamePrefs.OptionsControllerWeaponAiming,
				EnumGamePrefs.OptionsControlsSprintLock,
				EnumGamePrefs.OptionsControllerTriggerEffects,
				EnumGamePrefs.OptionsControllerIconStyle
			})
		{
		}

		// Token: 0x0600935C RID: 37724 RVA: 0x0037978F File Offset: 0x0037798F
		public override void Reset(PrefVersionStore versionedPrefs)
		{
			Log.Out("Resetting game options ControllerGroup");
			base.Reset(versionedPrefs);
		}
	}

	// Token: 0x0200120A RID: 4618
	public class BindingsGroup : GameOptionsReset.IGroup
	{
		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x0600935D RID: 37725 RVA: 0x003797A2 File Offset: 0x003779A2
		public string VersionId
		{
			get
			{
				return string.Format("Bindings_{0}_{1}", 13, 1);
			}
		}

		// Token: 0x0600935E RID: 37726 RVA: 0x003797BC File Offset: 0x003779BC
		public bool HasPref(string prefName)
		{
			if (prefName.Equals("Controls"))
			{
				return true;
			}
			if (prefName.Equals("ActionSetsSaved"))
			{
				return true;
			}
			if (!prefName.StartsWith("ActionSet_"))
			{
				return false;
			}
			foreach (ValueTuple<PlayerActionsBase, string> valueTuple in GameOptionsControls.ActionSetPrefs)
			{
				string item = valueTuple.Item2;
				if (prefName.Equals(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600935F RID: 37727 RVA: 0x00379844 File Offset: 0x00377A44
		public bool NeedsReset()
		{
			return 1 != GamePrefs.GetInt(EnumGamePrefs.OptionsBindingsResetRevision);
		}

		// Token: 0x06009360 RID: 37728 RVA: 0x00379858 File Offset: 0x00377A58
		public void Reset(PrefVersionStore versionedPrefs)
		{
			Log.Out("Resetting game options BindingsGroup");
			foreach (ValueTuple<PlayerActionsBase, string> valueTuple in GameOptionsControls.ActionSetPrefs)
			{
				PlayerActionsBase item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string text;
				if (versionedPrefs != null && versionedPrefs.TryGetString(item2, out text) && !string.IsNullOrEmpty(text))
				{
					item.Load(text);
				}
				else
				{
					item.Reset();
				}
			}
			GameOptionsControls.Save();
			GamePrefs.Set(EnumGamePrefs.OptionsBindingsResetRevision, 1);
		}
	}
}
