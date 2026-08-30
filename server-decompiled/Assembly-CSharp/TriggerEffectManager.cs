using System;
using System.Collections.Generic;
using InControl;
using Platform;
using TriggerEffects;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

// Token: 0x020014C0 RID: 5312
[global::UnityEngine.Scripting.Preserve]
public class TriggerEffectManager : IDisposable
{
	// Token: 0x1700136E RID: 4974
	// (get) Token: 0x0600A6B5 RID: 42677 RVA: 0x003E0B0F File Offset: 0x003DED0F
	// (set) Token: 0x0600A6B6 RID: 42678 RVA: 0x003E0B17 File Offset: 0x003DED17
	public bool Enabled
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this._enabled;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this._enabled = value;
			this._stateChanged = true;
		}
	}

	// Token: 0x0600A6B7 RID: 42679 RVA: 0x003E0B28 File Offset: 0x003DED28
	public TriggerEffectManager()
	{
		this.PollSetting();
		this._stateChanged = false;
		for (int i = 0; i < this.vibrationAudioSources.Length; i++)
		{
			this.vibrationAudioSources[i] = new AudioGamepadRumbleSource();
		}
		this.Init();
		InputManager.OnDeviceAttached += this.OnControllerConnected;
	}

	// Token: 0x0600A6B8 RID: 42680 RVA: 0x003E0B97 File Offset: 0x003DED97
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnControllerConnected(InputDevice device)
	{
		this.Shutdown();
		this.PollSetting();
		this.Init();
		this._stateChanged = true;
		GameManager.Instance.triggerEffectManager.EnableVibration();
	}

	// Token: 0x0600A6B9 RID: 42681 RVA: 0x003E0BC1 File Offset: 0x003DEDC1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.OptionsControllerTriggerEffects)
		{
			TriggerEffectManager.SetEnabled(GamePrefs.GetBool(_pref));
			return;
		}
		if (_pref != EnumGamePrefs.OptionsControllerVibrationStrength)
		{
			return;
		}
		TriggerEffectManager.UpdateControllerVibrationStrength();
	}

	// Token: 0x0600A6BA RID: 42682 RVA: 0x003E0BE8 File Offset: 0x003DEDE8
	public void Init()
	{
		try
		{
			TriggerEffectDualsensePC.InitTriggerEffectManager(ref this._controllersConnected);
		}
		catch (DllNotFoundException ex)
		{
			Log.Warning(string.Format("[TriggerEffectManager] Failed to load ControllerExt, disabling. Details: {0}", ex));
			GamePrefs.Set(EnumGamePrefs.OptionsControllerTriggerEffects, false);
			return;
		}
		TriggerEffectDualsense.InitTriggerEffectManager();
		TriggerEffectManager.UpdateControllerVibrationStrength();
		this.EnableVibration();
		this.InitializeLightbarGradient();
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged += this.OnLastInputStyleChanged;
		GamePrefs.OnGamePrefChanged += this.OnGamePrefChanged;
	}

	// Token: 0x0600A6BB RID: 42683 RVA: 0x003E0C74 File Offset: 0x003DEE74
	public void EnableVibration()
	{
		TriggerEffectDualsensePC.EnableVibration();
		TriggerEffectDualsense.EnableVibration();
	}

	// Token: 0x0600A6BC RID: 42684 RVA: 0x003E0C80 File Offset: 0x003DEE80
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitializeLightbarGradient()
	{
		TriggerEffectManager.lightbarGradients = Resources.Load<LightbarGradients>("Data/LightBarGradients");
	}

	// Token: 0x0600A6BD RID: 42685 RVA: 0x003E0C91 File Offset: 0x003DEE91
	public static void SetEnabled(bool _enabled)
	{
		GameManager.Instance.triggerEffectManager.Enabled = _enabled;
	}

	// Token: 0x0600A6BE RID: 42686 RVA: 0x003E0CA3 File Offset: 0x003DEEA3
	[PublicizedFrom(EAccessModifier.Private)]
	public byte FloatToByte(float value)
	{
		return (byte)(Mathf.Clamp01(value) * 255f);
	}

	// Token: 0x0600A6BF RID: 42687 RVA: 0x003E0CB4 File Offset: 0x003DEEB4
	public void Update()
	{
		if (GameManager.Instance.World != null && TriggerEffectManager.audioRumbleStrength > 0f && !GameManager.Instance.IsPaused() && PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			int num = 0;
			int num2 = 0;
			this.targetLeftAudioStrength = 0f;
			this.targetRightAudioStrength = 0f;
			Transform transform;
			if (GameManager.Instance.World != null)
			{
				EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
				transform = ((primaryPlayer != null) ? primaryPlayer.cameraTransform : null);
			}
			else
			{
				transform = LocalPlayerUI.primaryUI.uiCamera.transform;
			}
			foreach (AudioGamepadRumbleSource audioGamepadRumbleSource in this.vibrationAudioSources)
			{
				if (audioGamepadRumbleSource.audioSrc != null)
				{
					if (audioGamepadRumbleSource.audioSrc.isPlaying)
					{
						float num3 = audioGamepadRumbleSource.GetSample(0);
						if (audioGamepadRumbleSource.locationBased)
						{
							float num4 = 1f - Vector3.Distance(transform.position, audioGamepadRumbleSource.audioSrc.transform.position) / audioGamepadRumbleSource.audioSrc.maxDistance;
							if (num4 < 0.9f)
							{
								goto IL_0184;
							}
							num3 *= num4;
						}
						num3 *= audioGamepadRumbleSource.strengthMultiplier * audioGamepadRumbleSource.audioSrc.pitch * TriggerEffectManager.audioRumbleStrength;
						if (num3 > 0f)
						{
							num2++;
							this.targetRightAudioStrength += num3;
						}
						else if (num3 < 0f)
						{
							num++;
							this.targetLeftAudioStrength += num3;
						}
					}
					else
					{
						audioGamepadRumbleSource.Clear();
					}
				}
				IL_0184:;
			}
			if (num > 0 || num2 > 0)
			{
				if (num > 0)
				{
					this.targetLeftAudioStrength /= (float)num;
				}
				if (num2 > 0)
				{
					this.targetRightAudioStrength /= (float)num2;
				}
				this.leftAudioStrength = Mathf.Lerp(this.leftAudioStrength, this.targetLeftAudioStrength, Time.deltaTime * 15f);
				this.rightAudioStrength = Mathf.Lerp(this.rightAudioStrength, this.targetRightAudioStrength, Time.deltaTime * 15f);
				InputManager.ActiveDevice.Vibrate(-this.leftAudioStrength, this.rightAudioStrength);
				this.SetGamepadVibration(-this.leftAudioStrength + this.rightAudioStrength);
				this.SetDualSenseVibration(this.FloatToByte(-this.leftAudioStrength * 0.25f), this.FloatToByte(this.rightAudioStrength * 0.25f));
			}
			else
			{
				this.SetDualSenseVibration(0, 0);
				InputManager.ActiveDevice.StopVibration();
				this.SetGamepadVibration(0f);
			}
		}
		TriggerEffectDualsensePC.PCConnectedUpdate(ref this._controllersConnected, this._currentEffectLeft.DualsenseEffect, this._currentEffectRight.DualsenseEffect);
		TriggerEffectDualsense.ConnectedUpdate(this._currentEffectLeft.DualsenseEffect, this._currentEffectRight.DualsenseEffect);
		if (this.Enabled && !this.inUI)
		{
			TriggerEffectDualsensePC.PCTriggerUpdate(this._stateChanged, this._currentEffectLeft.DualsenseEffect, this._currentEffectRight.DualsenseEffect);
			TriggerEffectDualsense.Update(this._currentEffectLeft.DualsenseEffect, this._currentEffectRight.DualsenseEffect);
		}
		else if (this._stateChanged && (!this.Enabled || this.inUI))
		{
			this.SetGamepadTriggerEffectOff(TriggerEffectManager.GamepadTrigger.RightTrigger);
			this.SetGamepadTriggerEffectOff(TriggerEffectManager.GamepadTrigger.LeftTrigger);
			TriggerEffectDualsensePC.DualsensePCSetEffectToOff();
			TriggerEffectDualsense.SetEffectToOff();
			this._stateChanged = true;
		}
		this._triggerSetLeft = false;
		this._triggerSetRight = false;
		this._stateChanged = false;
	}

	// Token: 0x0600A6C0 RID: 42688 RVA: 0x003E1015 File Offset: 0x003DF215
	public void SetGamepadTriggerEffectOff(TriggerEffectManager.GamepadTrigger trigger)
	{
		if (trigger != TriggerEffectManager.GamepadTrigger.LeftTrigger)
		{
			if (trigger != TriggerEffectManager.GamepadTrigger.RightTrigger)
			{
				throw new ArgumentOutOfRangeException("trigger", trigger, null);
			}
			this._currentEffectRight = TriggerEffectManager.NoneEffect;
		}
		else
		{
			this._currentEffectLeft = TriggerEffectManager.NoneEffect;
		}
		this._stateChanged = true;
	}

	// Token: 0x0600A6C1 RID: 42689 RVA: 0x003E1053 File Offset: 0x003DF253
	public void SetWeaponEffect(int userID, TriggerEffectManager.GamepadTrigger trigger, byte startPosition, byte endPosition, byte strength)
	{
		if (!this.Enabled)
		{
			return;
		}
		TriggerEffectDualsensePC.SetWeaponEffect(userID, trigger, startPosition, endPosition, strength);
		TriggerEffectDualsense.SetWeaponEffect(userID, trigger, startPosition, endPosition, strength);
		this._stateChanged = true;
	}

	// Token: 0x0600A6C2 RID: 42690 RVA: 0x003E107D File Offset: 0x003DF27D
	public void ResetControllerIdentification()
	{
		TriggerEffectDualsensePC.ResetControllerIdentification();
		TriggerEffectDualsense.ResetControllerIdentification();
		this._stateChanged = true;
	}

	// Token: 0x0600A6C3 RID: 42691 RVA: 0x003E1090 File Offset: 0x003DF290
	public void SetControllerIdentification()
	{
		TriggerEffectDualsensePC.SetControllerIdentification();
		TriggerEffectDualsense.SetControllerIdentification();
		this._stateChanged = true;
	}

	// Token: 0x0600A6C4 RID: 42692 RVA: 0x003E10A3 File Offset: 0x003DF2A3
	public void SetTriggerEffectVibration(int userID, TriggerEffectManager.GamepadTrigger trigger, byte position, byte amplitude, byte frequency)
	{
		if (!this.Enabled)
		{
			return;
		}
		TriggerEffectDualsensePC.SetTriggerEffectVibration(userID, trigger, position, amplitude, frequency);
		TriggerEffectDualsense.SetTriggerEffectVibration(userID, trigger, position, amplitude, frequency);
		this._stateChanged = true;
	}

	// Token: 0x0600A6C5 RID: 42693 RVA: 0x003E10CD File Offset: 0x003DF2CD
	public void SetTriggerEffectVibrationMultiplePosition(int userID, TriggerEffectManager.GamepadTrigger trigger, byte[] amplitudes, byte frequency)
	{
		if (!this.Enabled)
		{
			return;
		}
		TriggerEffectDualsensePC.SetTriggerEffectVibrationMultiplePosition(userID, trigger, amplitudes, frequency);
		TriggerEffectDualsense.SetTriggerEffectVibrationMultiplePosition(userID, trigger, amplitudes, frequency);
		this._stateChanged = true;
	}

	// Token: 0x0600A6C6 RID: 42694 RVA: 0x003E10F4 File Offset: 0x003DF2F4
	public void Shutdown()
	{
		this.StopGamepadVibration(true);
		TriggerEffectManager.LibShutdown();
		TriggerEffectDualsense.ResetControllerIdentification();
		TriggerEffectDualsensePC.ResetControllerIdentification();
		for (int i = 1; i < 5; i++)
		{
			this._controllersConnected[i - 1] = false;
		}
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
		GamePrefs.OnGamePrefChanged -= this.OnGamePrefChanged;
		this.Enabled = false;
	}

	// Token: 0x0600A6C7 RID: 42695 RVA: 0x003E1161 File Offset: 0x003DF361
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LibShutdown()
	{
		TriggerEffectDualsensePC.LibShutdown();
	}

	// Token: 0x0600A6C8 RID: 42696 RVA: 0x003E1168 File Offset: 0x003DF368
	public void Dispose()
	{
		this.Shutdown();
		InputManager.OnDeviceAttached -= this.OnControllerConnected;
	}

	// Token: 0x0600A6C9 RID: 42697 RVA: 0x003E1181 File Offset: 0x003DF381
	public void PollSetting()
	{
		this.Enabled = GamePrefs.GetBool(EnumGamePrefs.OptionsControllerTriggerEffects);
	}

	// Token: 0x1700136F RID: 4975
	// (get) Token: 0x0600A6CA RID: 42698 RVA: 0x003E1193 File Offset: 0x003DF393
	// (set) Token: 0x0600A6CB RID: 42699 RVA: 0x003E119B File Offset: 0x003DF39B
	public bool inUI
	{
		get
		{
			return this._inUI;
		}
		set
		{
			if (this._inUI != value)
			{
				this._inUI = value;
				this._stateChanged = true;
			}
		}
	}

	// Token: 0x0600A6CC RID: 42700 RVA: 0x003E11B4 File Offset: 0x003DF3B4
	public void SetGamepadVibration(float strength)
	{
		this._currentControllerVibrationStrength = strength;
		this._stateChanged = true;
	}

	// Token: 0x0600A6CD RID: 42701 RVA: 0x003E11C4 File Offset: 0x003DF3C4
	public void StopGamepadVibration(bool clearSources = false)
	{
		if (clearSources)
		{
			AudioGamepadRumbleSource[] array = this.vibrationAudioSources;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Clear();
			}
		}
		this._currentControllerVibrationStrength = 0f;
		this._stateChanged = true;
		this.SetDualSenseVibration(0, 0);
		InputManager.ActiveDevice.StopVibration();
	}

	// Token: 0x0600A6CE RID: 42702 RVA: 0x003E1215 File Offset: 0x003DF415
	public void SetTriggerEffect(TriggerEffectManager.ControllerTriggerEffect effect)
	{
		if (!this.Enabled)
		{
			return;
		}
		this.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.LeftTrigger, effect, false);
		this.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.RightTrigger, effect, false);
	}

	// Token: 0x0600A6CF RID: 42703 RVA: 0x003E1234 File Offset: 0x003DF434
	public void SetTriggerEffect(TriggerEffectManager.GamepadTrigger trigger, TriggerEffectManager.ControllerTriggerEffect effect, bool asap = false)
	{
		if (!this.Enabled)
		{
			return;
		}
		this._stateChanged = true;
		if (trigger == TriggerEffectManager.GamepadTrigger.LeftTrigger)
		{
			this._currentEffectLeft.DualsenseEffect = effect.DualsenseEffect;
			if (asap)
			{
				TriggerEffectDualsensePC.ApplyImmediate(trigger, this._currentEffectLeft);
			}
			this._triggerSetLeft = true;
			return;
		}
		if (trigger != TriggerEffectManager.GamepadTrigger.RightTrigger)
		{
			return;
		}
		this._currentEffectRight.DualsenseEffect = effect.DualsenseEffect;
		if (asap)
		{
			TriggerEffectDualsensePC.ApplyImmediate(trigger, this._currentEffectRight);
		}
		this._triggerSetRight = true;
	}

	// Token: 0x0600A6D0 RID: 42704 RVA: 0x003E12A8 File Offset: 0x003DF4A8
	public static TriggerEffectManager.ControllerTriggerEffect GetTriggerEffect(ValueTuple<string, string> triggerEffectNames)
	{
		if (string.IsNullOrEmpty(triggerEffectNames.Item1) || string.IsNullOrEmpty(triggerEffectNames.Item2) || (triggerEffectNames.Item1.Contains("NoEffect") && triggerEffectNames.Item2.Contains("NoEffect")) || (triggerEffectNames.Item1.Contains("NoneEffect") && triggerEffectNames.Item2.Contains("NoneEffect")))
		{
			return TriggerEffectManager.NoneEffect;
		}
		TriggerEffectManager.ControllerTriggerEffect controllerTriggerEffect = new TriggerEffectManager.ControllerTriggerEffect
		{
			DualsenseEffect = TriggerEffectManager.NoneEffectDs,
			XboxTriggerEffect = TriggerEffectManager.NoneEffectXb
		};
		TriggerEffectManager.TriggerEffectDS triggerEffectDS;
		if (TriggerEffectManager.ControllerTriggerEffectsDS.TryGetValue(triggerEffectNames.Item1, out triggerEffectDS))
		{
			controllerTriggerEffect.DualsenseEffect = triggerEffectDS;
		}
		else
		{
			Log.Warning("Failed to find trigger effect DS: " + triggerEffectNames.Item1);
		}
		TriggerEffectManager.TriggerEffectXB triggerEffectXB;
		if (TriggerEffectManager.ControllerTriggerEffectsXb.TryGetValue(triggerEffectNames.Item2, out triggerEffectXB))
		{
			controllerTriggerEffect.XboxTriggerEffect = triggerEffectXB;
		}
		else
		{
			Log.Warning("Failed to find trigger effect XB: " + triggerEffectNames.Item2);
		}
		return controllerTriggerEffect;
	}

	// Token: 0x0600A6D1 RID: 42705 RVA: 0x003E13A8 File Offset: 0x003DF5A8
	public static TriggerEffectManager.ControllerTriggerEffect GetTriggerEffect(string dualsenseTrigger, string impulseTrigger)
	{
		if (string.IsNullOrEmpty(dualsenseTrigger) || string.IsNullOrEmpty(impulseTrigger) || (dualsenseTrigger.Contains("NoEffect") && impulseTrigger.Contains("NoEffect")) || (dualsenseTrigger.Contains("NoneEffect") && impulseTrigger.Contains("NoneEffect")))
		{
			return TriggerEffectManager.NoneEffect;
		}
		TriggerEffectManager.ControllerTriggerEffect controllerTriggerEffect = new TriggerEffectManager.ControllerTriggerEffect
		{
			DualsenseEffect = TriggerEffectManager.NoneEffectDs,
			XboxTriggerEffect = TriggerEffectManager.NoneEffectXb
		};
		TriggerEffectManager.TriggerEffectDS triggerEffectDS;
		if (TriggerEffectManager.ControllerTriggerEffectsDS.TryGetValue(dualsenseTrigger, out triggerEffectDS))
		{
			controllerTriggerEffect.DualsenseEffect = triggerEffectDS;
		}
		else
		{
			Log.Warning("Failed to find trigger effect DS: " + dualsenseTrigger);
		}
		TriggerEffectManager.TriggerEffectXB triggerEffectXB;
		if (TriggerEffectManager.ControllerTriggerEffectsXb.TryGetValue(impulseTrigger, out triggerEffectXB))
		{
			controllerTriggerEffect.XboxTriggerEffect = triggerEffectXB;
		}
		else
		{
			Log.Warning("Failed to find trigger effect XB: " + impulseTrigger);
		}
		return controllerTriggerEffect;
	}

	// Token: 0x0600A6D2 RID: 42706 RVA: 0x003E1474 File Offset: 0x003DF674
	public static ValueTuple<TriggerEffectManager.TriggerEffectDS, TriggerEffectManager.TriggerEffectXB> GetTriggerEffectAsTuple(ValueTuple<string, string> triggerEffectNames)
	{
		if (string.IsNullOrEmpty(triggerEffectNames.Item1) || string.IsNullOrEmpty(triggerEffectNames.Item2) || (triggerEffectNames.Item1.Contains("NoEffect") && triggerEffectNames.Item2.Contains("NoEffect")) || (triggerEffectNames.Item1.Contains("NoneEffect") && triggerEffectNames.Item2.Contains("NoneEffect")))
		{
			return new ValueTuple<TriggerEffectManager.TriggerEffectDS, TriggerEffectManager.TriggerEffectXB>(TriggerEffectManager.NoneEffect.DualsenseEffect, TriggerEffectManager.NoneEffect.XboxTriggerEffect);
		}
		ValueTuple<TriggerEffectManager.TriggerEffectDS, TriggerEffectManager.TriggerEffectXB> valueTuple = new ValueTuple<TriggerEffectManager.TriggerEffectDS, TriggerEffectManager.TriggerEffectXB>(TriggerEffectManager.NoneEffect.DualsenseEffect, TriggerEffectManager.NoneEffect.XboxTriggerEffect);
		TriggerEffectManager.TriggerEffectDS triggerEffectDS;
		if (TriggerEffectManager.ControllerTriggerEffectsDS.TryGetValue(triggerEffectNames.Item1, out triggerEffectDS))
		{
			valueTuple.Item1 = triggerEffectDS;
		}
		else
		{
			Log.Warning("Failed to find trigger effect DS: " + triggerEffectNames.Item1);
		}
		TriggerEffectManager.TriggerEffectXB triggerEffectXB;
		if (TriggerEffectManager.ControllerTriggerEffectsXb.TryGetValue(triggerEffectNames.Item2, out triggerEffectXB))
		{
			valueTuple.Item2 = triggerEffectXB;
		}
		else
		{
			Log.Warning("Failed to find trigger effect XB: " + triggerEffectNames.Item2);
		}
		return valueTuple;
	}

	// Token: 0x0600A6D3 RID: 42707 RVA: 0x003E157E File Offset: 0x003DF77E
	public static bool SettingDefaultValue()
	{
		return Application.platform == RuntimePlatform.PS5 || Application.platform == RuntimePlatform.GameCoreXboxSeries || Application.platform == RuntimePlatform.WindowsEditor;
	}

	// Token: 0x0600A6D4 RID: 42708 RVA: 0x003E159C File Offset: 0x003DF79C
	public void SetAudioRumbleSource(AudioSource _audioSource, float _strengthMultiplier, bool _locationBased)
	{
		AudioGamepadRumbleSource audioGamepadRumbleSource = null;
		float num = float.MaxValue;
		foreach (AudioGamepadRumbleSource audioGamepadRumbleSource2 in this.vibrationAudioSources)
		{
			if (!(audioGamepadRumbleSource2.audioSrc != null))
			{
				audioGamepadRumbleSource2.SetAudioSource(_audioSource, _strengthMultiplier, _locationBased);
				return;
			}
			if (audioGamepadRumbleSource2.timeAdded < num)
			{
				audioGamepadRumbleSource = audioGamepadRumbleSource2;
				num = audioGamepadRumbleSource2.timeAdded;
			}
		}
		if (audioGamepadRumbleSource != null)
		{
			audioGamepadRumbleSource.SetAudioSource(_audioSource, _strengthMultiplier, _locationBased);
		}
	}

	// Token: 0x0600A6D5 RID: 42709 RVA: 0x003E1608 File Offset: 0x003DF808
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDualSenseVibration(byte _smallMotor, byte _largeMotor)
	{
		TriggerEffectDualsense.SetDualSenseVibration(_smallMotor, _largeMotor);
		TriggerEffectDualsensePC.SetDualSenseVibration(_smallMotor, _largeMotor);
	}

	// Token: 0x0600A6D6 RID: 42710 RVA: 0x003E1618 File Offset: 0x003DF818
	public static void UpdateControllerVibrationStrength()
	{
		switch (GamePrefs.GetInt(EnumGamePrefs.OptionsControllerVibrationStrength))
		{
		case 0:
			TriggerEffectManager.audioRumbleStrength = 0f;
			return;
		case 1:
			TriggerEffectManager.audioRumbleStrength = 0.5f;
			return;
		case 2:
			TriggerEffectManager.audioRumbleStrength = 1f;
			return;
		case 3:
			TriggerEffectManager.audioRumbleStrength = 2f;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600A6D7 RID: 42711 RVA: 0x003E1672 File Offset: 0x003DF872
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		if (_style == PlayerInputManager.InputStyle.Keyboard)
		{
			this.StopGamepadVibration(false);
		}
	}

	// Token: 0x0600A6D8 RID: 42712 RVA: 0x003E1680 File Offset: 0x003DF880
	public static void UpdateDualSenseLightFromWeather(WeatherManager.BiomeWeather weather)
	{
		if (TriggerEffectManager.lightbarGradients == null || weather == null)
		{
			return;
		}
		float num = GameManager.Instance.World.GetWorldTime() % 24000UL;
		float num2 = SkyManager.GetDawnTime() * 1000f;
		float num3 = SkyManager.GetDuskTime() * 1000f;
		bool flag = num < num2 || num > num3;
		if (!flag && SkyManager.isAllTimeNight)
		{
			flag = true;
		}
		if (flag && SkyManager.isAllTimeDay)
		{
			flag = false;
		}
		float num6;
		if (flag)
		{
			float num4 = 24000f - num3;
			float num5 = num4 + num2;
			if (SkyManager.isAllTimeNight)
			{
				num6 = 0.5f;
			}
			else if (num >= num3 && num < 24000f)
			{
				num6 = (num - num3) / num5;
			}
			else
			{
				num6 = num4 / num5 + num / (num2 + num4);
			}
		}
		else if (SkyManager.isAllTimeDay)
		{
			num6 = 0.5f;
		}
		else
		{
			float num7 = num3 - num2;
			num6 = (num - num2) / num7;
		}
		Color color;
		if (SkyManager.BloodMoonVisiblePercent() == 1f)
		{
			float num8 = (1f + Mathf.Sin(Time.time)) / 2f;
			color = TriggerEffectManager.lightbarGradients.bloodmoonGradient.Evaluate(num8);
		}
		else if (weather.rainParam.value >= 0.5f || (weather.biomeDefinition != null && (weather.biomeDefinition.m_BiomeType == BiomeDefinition.BiomeType.Wasteland || weather.biomeDefinition.m_BiomeType == BiomeDefinition.BiomeType.burnt_forest)))
		{
			if (flag)
			{
				color = TriggerEffectManager.lightbarGradients.cloudNightGradient.Evaluate(num6);
			}
			else
			{
				color = TriggerEffectManager.lightbarGradients.cloudDayGradient.Evaluate(num6);
			}
		}
		else if (flag)
		{
			color = TriggerEffectManager.lightbarGradients.nightGradient.Evaluate(num6);
		}
		else
		{
			color = TriggerEffectManager.lightbarGradients.dayGradient.Evaluate(num6);
		}
		TriggerEffectManager.SetDualSenseLightbarColor(color);
	}

	// Token: 0x0600A6D9 RID: 42713 RVA: 0x003E182C File Offset: 0x003DFA2C
	public static void SetDualSenseLightbarColor(Color color)
	{
		for (int i = 1; i < 5; i++)
		{
			TriggerEffectDualsense.SetLightbar(i, (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f));
			TriggerEffectDualsensePC.SetLightbar(i, (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f));
		}
	}

	// Token: 0x0600A6DA RID: 42714 RVA: 0x003E189F File Offset: 0x003DFA9F
	public static void SetMainMenuLightbarColor()
	{
		TriggerEffectManager.SetDualSenseLightbarColor(TriggerEffectManager.lightbarGradients.mainMenuColor);
	}

	// Token: 0x04007B88 RID: 31624
	public static readonly Dictionary<string, TriggerEffectManager.TriggerEffectDS> ControllerTriggerEffectsDS = new Dictionary<string, TriggerEffectManager.TriggerEffectDS>();

	// Token: 0x04007B89 RID: 31625
	public static readonly Dictionary<string, TriggerEffectManager.TriggerEffectXB> ControllerTriggerEffectsXb = new Dictionary<string, TriggerEffectManager.TriggerEffectXB>();

	// Token: 0x04007B8A RID: 31626
	[PublicizedFrom(EAccessModifier.Internal)]
	public static readonly TriggerEffectManager.TriggerEffectDS NoneEffectDs = new TriggerEffectManager.TriggerEffectDS
	{
		Effect = TriggerEffectManager.EffectDualsense.Off,
		AmplitudeEndStrength = 0,
		Frequency = 0,
		Position = 0,
		EndPosition = 0,
		Strength = 0,
		Strengths = Array.Empty<byte>()
	};

	// Token: 0x04007B8B RID: 31627
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TriggerEffectManager.TriggerEffectXB NoneEffectXb = new TriggerEffectManager.TriggerEffectXB
	{
		Effect = TriggerEffectManager.EffectXbox.Off,
		Strength = 0f
	};

	// Token: 0x04007B8C RID: 31628
	public static readonly TriggerEffectManager.ControllerTriggerEffect NoneEffect = new TriggerEffectManager.ControllerTriggerEffect
	{
		DualsenseEffect = TriggerEffectManager.NoneEffectDs,
		XboxTriggerEffect = TriggerEffectManager.NoneEffectXb
	};

	// Token: 0x04007B8D RID: 31629
	[PublicizedFrom(EAccessModifier.Private)]
	public bool[] _controllersConnected = new bool[4];

	// Token: 0x04007B8E RID: 31630
	[PublicizedFrom(EAccessModifier.Private)]
	public float _currentControllerVibrationStrength;

	// Token: 0x04007B8F RID: 31631
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _enabled;

	// Token: 0x04007B90 RID: 31632
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _triggerSetLeft;

	// Token: 0x04007B91 RID: 31633
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _triggerSetRight;

	// Token: 0x04007B92 RID: 31634
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _stateChanged;

	// Token: 0x04007B93 RID: 31635
	[PublicizedFrom(EAccessModifier.Private)]
	public TriggerEffectManager.ControllerTriggerEffect _currentEffectLeft;

	// Token: 0x04007B94 RID: 31636
	[PublicizedFrom(EAccessModifier.Private)]
	public TriggerEffectManager.ControllerTriggerEffect _currentEffectRight;

	// Token: 0x04007B95 RID: 31637
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAudioRumbleStrengthSubtle = 0.5f;

	// Token: 0x04007B96 RID: 31638
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAudioRumbleStrengthStandard = 1f;

	// Token: 0x04007B97 RID: 31639
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAudioRumbleStrengthStrong = 2f;

	// Token: 0x04007B98 RID: 31640
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDualSenseRumbleStrengthMultiplier = 0.25f;

	// Token: 0x04007B99 RID: 31641
	[PublicizedFrom(EAccessModifier.Private)]
	public static float audioRumbleStrength = 1f;

	// Token: 0x04007B9A RID: 31642
	public AudioGamepadRumbleSource[] vibrationAudioSources = new AudioGamepadRumbleSource[5];

	// Token: 0x04007B9B RID: 31643
	[PublicizedFrom(EAccessModifier.Protected)]
	public static LightbarGradients lightbarGradients;

	// Token: 0x04007B9C RID: 31644
	[PublicizedFrom(EAccessModifier.Private)]
	public float leftAudioStrength;

	// Token: 0x04007B9D RID: 31645
	[PublicizedFrom(EAccessModifier.Private)]
	public float rightAudioStrength;

	// Token: 0x04007B9E RID: 31646
	[PublicizedFrom(EAccessModifier.Private)]
	public float targetLeftAudioStrength;

	// Token: 0x04007B9F RID: 31647
	[PublicizedFrom(EAccessModifier.Private)]
	public float targetRightAudioStrength;

	// Token: 0x04007BA0 RID: 31648
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _inUI;

	// Token: 0x020014C1 RID: 5313
	public enum GamepadTrigger
	{
		// Token: 0x04007BA2 RID: 31650
		LeftTrigger,
		// Token: 0x04007BA3 RID: 31651
		RightTrigger
	}

	// Token: 0x020014C2 RID: 5314
	public enum EffectDualsense
	{
		// Token: 0x04007BA5 RID: 31653
		Off,
		// Token: 0x04007BA6 RID: 31654
		WeaponSingle,
		// Token: 0x04007BA7 RID: 31655
		WeaponMultipoint,
		// Token: 0x04007BA8 RID: 31656
		FeedbackSingle,
		// Token: 0x04007BA9 RID: 31657
		VibrationSingle,
		// Token: 0x04007BAA RID: 31658
		FeedbackSlope,
		// Token: 0x04007BAB RID: 31659
		VibrationSlope,
		// Token: 0x04007BAC RID: 31660
		FeedbackMultipoint,
		// Token: 0x04007BAD RID: 31661
		VibrationMultipoint
	}

	// Token: 0x020014C3 RID: 5315
	public struct TriggerEffectDS
	{
		// Token: 0x04007BAE RID: 31662
		public TriggerEffectManager.EffectDualsense Effect;

		// Token: 0x04007BAF RID: 31663
		public byte Position;

		// Token: 0x04007BB0 RID: 31664
		public byte EndPosition;

		// Token: 0x04007BB1 RID: 31665
		public byte Frequency;

		// Token: 0x04007BB2 RID: 31666
		public byte AmplitudeEndStrength;

		// Token: 0x04007BB3 RID: 31667
		public byte Strength;

		// Token: 0x04007BB4 RID: 31668
		public byte[] Strengths;
	}

	// Token: 0x020014C4 RID: 5316
	public enum EffectXbox
	{
		// Token: 0x04007BB6 RID: 31670
		Off,
		// Token: 0x04007BB7 RID: 31671
		FeedbackSingle,
		// Token: 0x04007BB8 RID: 31672
		VibrationSingle,
		// Token: 0x04007BB9 RID: 31673
		FeedbackSlope,
		// Token: 0x04007BBA RID: 31674
		VibrationSlope
	}

	// Token: 0x020014C5 RID: 5317
	public struct TriggerEffectXB
	{
		// Token: 0x04007BBB RID: 31675
		public TriggerEffectManager.EffectXbox Effect;

		// Token: 0x04007BBC RID: 31676
		public float Strength;

		// Token: 0x04007BBD RID: 31677
		[FormerlySerializedAs("endStrength")]
		[FormerlySerializedAs("Amplitude")]
		public float EndStrength;

		// Token: 0x04007BBE RID: 31678
		public float StartPosition;

		// Token: 0x04007BBF RID: 31679
		public float EndPosition;
	}

	// Token: 0x020014C6 RID: 5318
	public struct ControllerTriggerEffect
	{
		// Token: 0x04007BC0 RID: 31680
		public TriggerEffectManager.TriggerEffectDS DualsenseEffect;

		// Token: 0x04007BC1 RID: 31681
		public TriggerEffectManager.TriggerEffectXB XboxTriggerEffect;
	}
}
