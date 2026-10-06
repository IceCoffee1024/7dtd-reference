using System;
using System.Collections.Generic;

// Token: 0x020011F3 RID: 4595
public static class GameOptionsReset
{
	// Token: 0x060092E6 RID: 37606 RVA: 0x00377158 File Offset: 0x00375358
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

	// Token: 0x060092E7 RID: 37607 RVA: 0x003771D8 File Offset: 0x003753D8
	public static string GetGroupId(EnumGamePrefs enumGamePref)
	{
		return GameOptionsReset.GetGroupId(enumGamePref.ToStringCached<EnumGamePrefs>());
	}

	// Token: 0x060092E8 RID: 37608 RVA: 0x003771E8 File Offset: 0x003753E8
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

	// Token: 0x060092E9 RID: 37609 RVA: 0x0037724C File Offset: 0x0037544C
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

	// Token: 0x060092EA RID: 37610 RVA: 0x003772A0 File Offset: 0x003754A0
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

	// Token: 0x060092EB RID: 37611 RVA: 0x003772E6 File Offset: 0x003754E6
	public static bool NeedsResetGame()
	{
		return 13 != GamePrefs.GetInt(EnumGamePrefs.LastGameResetRevision);
	}

	// Token: 0x060092EC RID: 37612 RVA: 0x003772F8 File Offset: 0x003754F8
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

	// Token: 0x060092ED RID: 37613 RVA: 0x00377358 File Offset: 0x00375558
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

	// Token: 0x04006C86 RID: 27782
	public static GameOptionsReset.AudioGroup Audio;

	// Token: 0x04006C87 RID: 27783
	public static GameOptionsReset.GraphicsGroup Graphics;

	// Token: 0x04006C88 RID: 27784
	public static GameOptionsReset.ControlsGroup Controls;

	// Token: 0x04006C89 RID: 27785
	public static GameOptionsReset.ControllerGroup Controller;

	// Token: 0x04006C8A RID: 27786
	public static GameOptionsReset.BindingsGroup Bindings;

	// Token: 0x04006C8B RID: 27787
	public static List<GameOptionsReset.IGroup> groups;

	// Token: 0x04006C8C RID: 27788
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string defaultGroupId = string.Format("DefaultGroup_{0}", 13);

	// Token: 0x020011F4 RID: 4596
	public interface IGroup
	{
		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x060092EF RID: 37615
		string VersionId { get; }

		// Token: 0x060092F0 RID: 37616
		bool HasPref(string prefName);

		// Token: 0x060092F1 RID: 37617
		bool NeedsReset();

		// Token: 0x060092F2 RID: 37618
		void Reset(PrefVersionStore versionedPrefs);
	}

	// Token: 0x020011F5 RID: 4597
	public abstract class EnumGamePrefGroup : GameOptionsReset.IGroup
	{
		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x060092F3 RID: 37619
		public abstract string VersionId { get; }

		// Token: 0x060092F4 RID: 37620 RVA: 0x0037739C File Offset: 0x0037559C
		[PublicizedFrom(EAccessModifier.Protected)]
		public EnumGamePrefGroup(params EnumGamePrefs[] enumGamePrefs)
		{
			foreach (EnumGamePrefs enumGamePrefs2 in enumGamePrefs)
			{
				this.AddEnumGamePref(enumGamePrefs2);
			}
		}

		// Token: 0x060092F5 RID: 37621 RVA: 0x003773E0 File Offset: 0x003755E0
		[PublicizedFrom(EAccessModifier.Protected)]
		public void AddEnumGamePref(EnumGamePrefs enumGamePref)
		{
			if (!this.EnumPrefs.Add(enumGamePref))
			{
				return;
			}
			this.PrefNames.Add(enumGamePref.ToStringCached<EnumGamePrefs>());
		}

		// Token: 0x060092F6 RID: 37622 RVA: 0x00377403 File Offset: 0x00375603
		public virtual bool HasPref(string prefName)
		{
			return this.PrefNames.Contains(prefName);
		}

		// Token: 0x060092F7 RID: 37623 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool NeedsReset()
		{
			return false;
		}

		// Token: 0x060092F8 RID: 37624 RVA: 0x00377414 File Offset: 0x00375614
		public virtual void Reset(PrefVersionStore versionedPrefs)
		{
			foreach (EnumGamePrefs enumGamePrefs in this.EnumPrefs)
			{
				GameOptionsReset.ResetPref(enumGamePrefs, versionedPrefs);
			}
		}

		// Token: 0x04006C8D RID: 27789
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<EnumGamePrefs> EnumPrefs = new HashSet<EnumGamePrefs>();

		// Token: 0x04006C8E RID: 27790
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSet<string> PrefNames = new HashSet<string>();
	}

	// Token: 0x020011F6 RID: 4598
	public class AudioGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x060092F9 RID: 37625 RVA: 0x00377468 File Offset: 0x00375668
		public override string VersionId
		{
			get
			{
				return string.Format("Audio_{0}_0", 13);
			}
		}

		// Token: 0x060092FA RID: 37626 RVA: 0x0037747B File Offset: 0x0037567B
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

		// Token: 0x060092FB RID: 37627 RVA: 0x00377495 File Offset: 0x00375695
		public override void Reset(PrefVersionStore versionedPrefs)
		{
			Log.Out("Resetting game options AudioGroup");
			base.Reset(versionedPrefs);
		}
	}

	// Token: 0x020011F7 RID: 4599
	public class GraphicsGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x060092FC RID: 37628 RVA: 0x003774A8 File Offset: 0x003756A8
		public override string VersionId
		{
			get
			{
				return string.Format("Graphics_{0}_{1}", 13, 4);
			}
		}

		// Token: 0x060092FD RID: 37629 RVA: 0x003774C4 File Offset: 0x003756C4
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

		// Token: 0x060092FE RID: 37630 RVA: 0x00377540 File Offset: 0x00375740
		public override bool NeedsReset()
		{
			return 4 != GamePrefs.GetInt(EnumGamePrefs.OptionsGfxResetRevision);
		}

		// Token: 0x060092FF RID: 37631 RVA: 0x00377550 File Offset: 0x00375750
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

	// Token: 0x020011F8 RID: 4600
	public class ControlsGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x06009300 RID: 37632 RVA: 0x0037758C File Offset: 0x0037578C
		public override string VersionId
		{
			get
			{
				return string.Format("Controls_{0}_{1}", 13, 7);
			}
		}

		// Token: 0x06009301 RID: 37633 RVA: 0x003775A5 File Offset: 0x003757A5
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

		// Token: 0x06009302 RID: 37634 RVA: 0x003775BE File Offset: 0x003757BE
		public override bool NeedsReset()
		{
			return 7 != GamePrefs.GetInt(EnumGamePrefs.OptionsControlsResetRevision);
		}

		// Token: 0x06009303 RID: 37635 RVA: 0x003775D0 File Offset: 0x003757D0
		public override void Reset(PrefVersionStore versionedPrefs)
		{
			Log.Out("Resetting game options ControlsGroup");
			base.Reset(versionedPrefs);
			GamePrefs.Set(EnumGamePrefs.OptionsControlsResetRevision, 7);
		}
	}

	// Token: 0x020011F9 RID: 4601
	public class ControllerGroup : GameOptionsReset.EnumGamePrefGroup
	{
		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x06009304 RID: 37636 RVA: 0x003775EE File Offset: 0x003757EE
		public override string VersionId
		{
			get
			{
				return string.Format("Controller_{0}_0", 13);
			}
		}

		// Token: 0x06009305 RID: 37637 RVA: 0x00377601 File Offset: 0x00375801
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

		// Token: 0x06009306 RID: 37638 RVA: 0x0037761B File Offset: 0x0037581B
		public override void Reset(PrefVersionStore versionedPrefs)
		{
			Log.Out("Resetting game options ControllerGroup");
			base.Reset(versionedPrefs);
		}
	}

	// Token: 0x020011FA RID: 4602
	public class BindingsGroup : GameOptionsReset.IGroup
	{
		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x06009307 RID: 37639 RVA: 0x0037762E File Offset: 0x0037582E
		public string VersionId
		{
			get
			{
				return string.Format("Bindings_{0}_{1}", 13, 1);
			}
		}

		// Token: 0x06009308 RID: 37640 RVA: 0x00377648 File Offset: 0x00375848
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

		// Token: 0x06009309 RID: 37641 RVA: 0x003776D0 File Offset: 0x003758D0
		public bool NeedsReset()
		{
			return 1 != GamePrefs.GetInt(EnumGamePrefs.OptionsBindingsResetRevision);
		}

		// Token: 0x0600930A RID: 37642 RVA: 0x003776E4 File Offset: 0x003758E4
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
