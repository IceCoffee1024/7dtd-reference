using System;
using System.Collections.Generic;

// Token: 0x020011BF RID: 4543
public static class GameOptionsReset
{
	// Token: 0x06009154 RID: 37204 RVA: 0x0036D664 File Offset: 0x0036B864
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

	// Token: 0x06009155 RID: 37205 RVA: 0x0036D6E4 File Offset: 0x0036B8E4
	public static string GetGroupId(EnumGamePrefs enumGamePref)
	{
		return GameOptionsReset.GetGroupId(enumGamePref.ToStringCached<EnumGamePrefs>());
	}

	// Token: 0x06009156 RID: 37206 RVA: 0x0036D6F4 File Offset: 0x0036B8F4
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

	// Token: 0x06009157 RID: 37207 RVA: 0x0036D758 File Offset: 0x0036B958
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

	// Token: 0x06009158 RID: 37208 RVA: 0x0036D7AC File Offset: 0x0036B9AC
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

	// Token: 0x06009159 RID: 37209 RVA: 0x0036D7F2 File Offset: 0x0036B9F2
	public static bool NeedsResetGame()
	{
		return 13 != GamePrefs.GetInt(EnumGamePrefs.LastGameResetRevision);
	}

	// Token: 0x0600915A RID: 37210 RVA: 0x0036D804 File Offset: 0x0036BA04
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

	// Token: 0x0600915B RID: 37211 RVA: 0x0036D864 File Offset: 0x0036BA64
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

	// Token: 0x04006B44 RID: 27460
	public static GameOptionsReset.AudioGroup Audio;

	// Token: 0x04006B45 RID: 27461
	public static GameOptionsReset.GraphicsGroup Graphics;

	// Token: 0x04006B46 RID: 27462
	public static GameOptionsReset.ControlsGroup Controls;

	// Token: 0x04006B47 RID: 27463
	public static GameOptionsReset.ControllerGroup Controller;

	// Token: 0x04006B48 RID: 27464
	public static GameOptionsReset.BindingsGroup Bindings;

	// Token: 0x04006B49 RID: 27465
	public static List<GameOptionsReset.IGroup> groups;

	// Token: 0x04006B4A RID: 27466
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string defaultGroupId = string.Format("DefaultGroup_{0}", 13);

	// Token: 0x020011C0 RID: 4544
	public interface IGroup
	{
		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x0600915D RID: 37213
		string VersionId { get; }

		// Token: 0x0600915E RID: 37214
		bool HasPref(string prefName);

		// Token: 0x0600915F RID: 37215
		bool NeedsReset();

		// Token: 0x06009160 RID: 37216
		void Reset(PrefVersionStore versionedPrefs);
	}

	// Token: 0x020011C1 RID: 4545
	public abstract class EnumGamePrefGroup : GameOptionsReset.IGroup
	{
		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x06009161 RID: 37217
		public abstract string VersionId { get; }

		// Token: 0x06009162 RID: 37218 RVA: 0x0036D8A8 File Offset: 0x0036BAA8
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGamePrefGroup(params EnumGamePrefs[] enumGamePrefs)
		{
			foreach (EnumGamePrefs enumGamePrefs2 in enumGamePrefs)
			{
				this.AddEnumGamePref(enumGamePrefs2);
			}
		}

		// Token: 0x06009163 RID: 37219 RVA: 0x0036D8EC File Offset: 0x0036BAEC
		[PublicizedFrom(EAccessModifier.Protected)]
		public void AddEnumGamePref(EnumGamePrefs enumGamePref)
		{
			if (!this.EnumPrefs.Add(enumGamePref))
			{
				return;
			}
			this.PrefNames.Add(enumGamePref.ToStringCached<EnumGamePrefs>());
		}

		// Token: 0x06009164 RID: 37220 RVA: 0x0036D90F File Offset: 0x0036BB0F
		public virtual bool HasPref(string prefName)
		{
			return this.PrefNames.Contains(prefName);
		}

		// Token: 0x06009165 RID: 37221 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool NeedsReset()
		{
			return false;
		}

		// Token: 0x06009166 RID: 37222 RVA: 0x0036D920 File Offset: 0x0036BB20
		public virtual void Reset(PrefVersionStore versionedPrefs)
		{
			foreach (EnumGamePrefs enumGamePrefs in this.EnumPrefs)
			{
				GameOptionsReset.ResetPref(enumGamePrefs, versionedPrefs);
			}
		}

		// Token: 0x04006B4B RID: 27467
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<EnumGamePrefs> EnumPrefs = new HashSet<EnumGamePrefs>();

		// Token: 0x04006B4C RID: 27468
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<string> PrefNames = new HashSet<string>();
	}

	// Token: 0x020011C2 RID: 4546
	public class AudioGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x06009167 RID: 37223 RVA: 0x0036D974 File Offset: 0x0036BB74
		public override string VersionId
		{
			get
			{
				return string.Format("Audio_{0}_0", 13);
			}
		}

		// Token: 0x06009168 RID: 37224 RVA: 0x0036D987 File Offset: 0x0036BB87
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

		// Token: 0x06009169 RID: 37225 RVA: 0x0036D9A1 File Offset: 0x0036BBA1
		public override void Reset(PrefVersionStore versionedPrefs)
		{
			Log.Out("Resetting game options AudioGroup");
			base.Reset(versionedPrefs);
		}
	}

	// Token: 0x020011C3 RID: 4547
	public class GraphicsGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x0600916A RID: 37226 RVA: 0x0036D9B4 File Offset: 0x0036BBB4
		public override string VersionId
		{
			get
			{
				return string.Format("Graphics_{0}_{1}", 13, 4);
			}
		}

		// Token: 0x0600916B RID: 37227 RVA: 0x0036D9D0 File Offset: 0x0036BBD0
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

		// Token: 0x0600916C RID: 37228 RVA: 0x0036DA4C File Offset: 0x0036BC4C
		public override bool NeedsReset()
		{
			return 4 != GamePrefs.GetInt(EnumGamePrefs.OptionsGfxResetRevision);
		}

		// Token: 0x0600916D RID: 37229 RVA: 0x0036DA5C File Offset: 0x0036BC5C
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

	// Token: 0x020011C4 RID: 4548
	public class ControlsGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x0600916E RID: 37230 RVA: 0x0036DA98 File Offset: 0x0036BC98
		public override string VersionId
		{
			get
			{
				return string.Format("Controls_{0}_{1}", 13, 7);
			}
		}

		// Token: 0x0600916F RID: 37231 RVA: 0x0036DAB1 File Offset: 0x0036BCB1
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

		// Token: 0x06009170 RID: 37232 RVA: 0x0036DACA File Offset: 0x0036BCCA
		public override bool NeedsReset()
		{
			return 7 != GamePrefs.GetInt(EnumGamePrefs.OptionsControlsResetRevision);
		}

		// Token: 0x06009171 RID: 37233 RVA: 0x0036DADC File Offset: 0x0036BCDC
		public override void Reset(PrefVersionStore versionedPrefs)
		{
			Log.Out("Resetting game options ControlsGroup");
			base.Reset(versionedPrefs);
			GamePrefs.Set(EnumGamePrefs.OptionsControlsResetRevision, 7);
		}
	}

	// Token: 0x020011C5 RID: 4549
	public class ControllerGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x06009172 RID: 37234 RVA: 0x0036DAFA File Offset: 0x0036BCFA
		public override string VersionId
		{
			get
			{
				return string.Format("Controller_{0}_0", 13);
			}
		}

		// Token: 0x06009173 RID: 37235 RVA: 0x0036DB0D File Offset: 0x0036BD0D
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

		// Token: 0x06009174 RID: 37236 RVA: 0x0036DB27 File Offset: 0x0036BD27
		public override void Reset(PrefVersionStore versionedPrefs)
		{
			Log.Out("Resetting game options ControllerGroup");
			base.Reset(versionedPrefs);
		}
	}

	// Token: 0x020011C6 RID: 4550
	public class BindingsGroup : GameOptionsReset.IGroup
	{
		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x06009175 RID: 37237 RVA: 0x0036DB3A File Offset: 0x0036BD3A
		public string VersionId
		{
			get
			{
				return string.Format("Bindings_{0}_{1}", 13, 1);
			}
		}

		// Token: 0x06009176 RID: 37238 RVA: 0x0036DB54 File Offset: 0x0036BD54
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

		// Token: 0x06009177 RID: 37239 RVA: 0x0036DBDC File Offset: 0x0036BDDC
		public bool NeedsReset()
		{
			return 1 != GamePrefs.GetInt(EnumGamePrefs.OptionsBindingsResetRevision);
		}

		// Token: 0x06009178 RID: 37240 RVA: 0x0036DBF0 File Offset: 0x0036BDF0
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
