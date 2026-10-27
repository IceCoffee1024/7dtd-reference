using System;
using System.Collections.Generic;
using InControl;
using Platform;
using TriggerEffects;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

// Token: 0x0200150F RID: 5391
[global::UnityEngine.Scripting.Preserve]
public class TriggerEffectManager : IDisposable
{
	// Token: 0x170013F8 RID: 5112
	// (get) Token: 0x0600A8BD RID: 43197 RVA: 0x003ED0A7 File Offset: 0x003EB2A7
	// (set) Token: 0x0600A8BE RID: 43198 RVA: 0x003ED0AF File Offset: 0x003EB2AF
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

	// Token: 0x0600A8BF RID: 43199 RVA: 0x003ED0C0 File Offset: 0x003EB2C0
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

	// Token: 0x0600A8C0 RID: 43200 RVA: 0x003ED12F File Offset: 0x003EB32F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnControllerConnected(InputDevice device)
	{
		this.Shutdown();
		this.PollSetting();
		this.Init();
		this._stateChanged = true;
		GameManager.Instance.triggerEffectManager.EnableVibration();
	}

	// Token: 0x0600A8C1 RID: 43201 RVA: 0x003ED159 File Offset: 0x003EB359
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

	// Token: 0x0600A8C2 RID: 43202 RVA: 0x003ED180 File Offset: 0x003EB380
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

	// Token: 0x0600A8C3 RID: 43203 RVA: 0x003ED20C File Offset: 0x003EB40C
	public void EnableVibration()
	{
		TriggerEffectDualsensePC.EnableVibration();
		TriggerEffectDualsense.EnableVibration();
	}

	// Token: 0x0600A8C4 RID: 43204 RVA: 0x003ED218 File Offset: 0x003EB418
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitializeLightbarGradient()
	{
		TriggerEffectManager.lightbarGradients = Resources.Load<LightbarGradients>("Data/LightBarGradients");
	}

	// Token: 0x0600A8C5 RID: 43205 RVA: 0x003ED229 File Offset: 0x003EB429
	public static void SetEnabled(bool _enabled)
	{
		GameManager.Instance.triggerEffectManager.Enabled = _enabled;
	}

	// Token: 0x0600A8C6 RID: 43206 RVA: 0x003ED23B File Offset: 0x003EB43B
	[PublicizedFrom(EAccessModifier.Private)]
	public byte FloatToByte(float value)
	{
		return (byte)(Mathf.Clamp01(value) * 255f);
	}

	// Token: 0x0600A8C7 RID: 43207 RVA: 0x003ED24C File Offset: 0x003EB44C
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

	// Token: 0x0600A8C8 RID: 43208 RVA: 0x003ED5AD File Offset: 0x003EB7AD
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

	// Token: 0x0600A8C9 RID: 43209 RVA: 0x003ED5EB File Offset: 0x003EB7EB
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

	// Token: 0x0600A8CA RID: 43210 RVA: 0x003ED615 File Offset: 0x003EB815
	public void ResetControllerIdentification()
	{
		TriggerEffectDualsensePC.ResetControllerIdentification();
		TriggerEffectDualsense.ResetControllerIdentification();
		this._stateChanged = true;
	}

	// Token: 0x0600A8CB RID: 43211 RVA: 0x003ED628 File Offset: 0x003EB828
	public void SetControllerIdentification()
	{
		TriggerEffectDualsensePC.SetControllerIdentification();
		TriggerEffectDualsense.SetControllerIdentification();
		this._stateChanged = true;
	}

	// Token: 0x0600A8CC RID: 43212 RVA: 0x003ED63B File Offset: 0x003EB83B
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

	// Token: 0x0600A8CD RID: 43213 RVA: 0x003ED665 File Offset: 0x003EB865
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

	// Token: 0x0600A8CE RID: 43214 RVA: 0x003ED68C File Offset: 0x003EB88C
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

	// Token: 0x0600A8CF RID: 43215 RVA: 0x003ED6F9 File Offset: 0x003EB8F9
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LibShutdown()
	{
		TriggerEffectDualsensePC.LibShutdown();
	}

	// Token: 0x0600A8D0 RID: 43216 RVA: 0x003ED700 File Offset: 0x003EB900
	public void Dispose()
	{
		this.Shutdown();
		InputManager.OnDeviceAttached -= this.OnControllerConnected;
	}

	// Token: 0x0600A8D1 RID: 43217 RVA: 0x003ED719 File Offset: 0x003EB919
	public void PollSetting()
	{
		this.Enabled = GamePrefs.GetBool(EnumGamePrefs.OptionsControllerTriggerEffects);
	}

	// Token: 0x170013F9 RID: 5113
	// (get) Token: 0x0600A8D2 RID: 43218 RVA: 0x003ED72B File Offset: 0x003EB92B
	// (set) Token: 0x0600A8D3 RID: 43219 RVA: 0x003ED733 File Offset: 0x003EB933
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

	// Token: 0x0600A8D4 RID: 43220 RVA: 0x003ED74C File Offset: 0x003EB94C
	public void SetGamepadVibration(float strength)
	{
		this._currentControllerVibrationStrength = strength;
		this._stateChanged = true;
	}

	// Token: 0x0600A8D5 RID: 43221 RVA: 0x003ED75C File Offset: 0x003EB95C
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

	// Token: 0x0600A8D6 RID: 43222 RVA: 0x003ED7AD File Offset: 0x003EB9AD
	public void SetTriggerEffect(TriggerEffectManager.ControllerTriggerEffect effect)
	{
		if (!this.Enabled)
		{
			return;
		}
		this.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.LeftTrigger, effect, false);
		this.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.RightTrigger, effect, false);
	}

	// Token: 0x0600A8D7 RID: 43223 RVA: 0x003ED7CC File Offset: 0x003EB9CC
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

	// Token: 0x0600A8D8 RID: 43224 RVA: 0x003ED840 File Offset: 0x003EBA40
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

	// Token: 0x0600A8D9 RID: 43225 RVA: 0x003ED940 File Offset: 0x003EBB40
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

	// Token: 0x0600A8DA RID: 43226 RVA: 0x003EDA0C File Offset: 0x003EBC0C
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

	// Token: 0x0600A8DB RID: 43227 RVA: 0x003EDB16 File Offset: 0x003EBD16
	public static bool SettingDefaultValue()
	{
		return Application.platform == RuntimePlatform.PS5 || Application.platform == RuntimePlatform.GameCoreXboxSeries || Application.platform == RuntimePlatform.WindowsEditor;
	}

	// Token: 0x0600A8DC RID: 43228 RVA: 0x003EDB34 File Offset: 0x003EBD34
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

	// Token: 0x0600A8DD RID: 43229 RVA: 0x003EDBA0 File Offset: 0x003EBDA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDualSenseVibration(byte _smallMotor, byte _largeMotor)
	{
		TriggerEffectDualsense.SetDualSenseVibration(_smallMotor, _largeMotor);
		TriggerEffectDualsensePC.SetDualSenseVibration(_smallMotor, _largeMotor);
	}

	// Token: 0x0600A8DE RID: 43230 RVA: 0x003EDBB0 File Offset: 0x003EBDB0
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

	// Token: 0x0600A8DF RID: 43231 RVA: 0x003EDC0A File Offset: 0x003EBE0A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		if (_style == PlayerInputManager.InputStyle.Keyboard)
		{
			this.StopGamepadVibration(false);
		}
	}

	// Token: 0x0600A8E0 RID: 43232 RVA: 0x003EDC18 File Offset: 0x003EBE18
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

	// Token: 0x0600A8E1 RID: 43233 RVA: 0x003EDDC4 File Offset: 0x003EBFC4
	public static void SetDualSenseLightbarColor(Color color)
	{
		for (int i = 1; i < 5; i++)
		{
			TriggerEffectDualsense.SetLightbar(i, (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f));
			TriggerEffectDualsensePC.SetLightbar(i, (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f));
		}
	}

	// Token: 0x0600A8E2 RID: 43234 RVA: 0x003EDE37 File Offset: 0x003EC037
	public static void SetMainMenuLightbarColor()
	{
		TriggerEffectManager.SetDualSenseLightbarColor(TriggerEffectManager.lightbarGradients.mainMenuColor);
	}

	// Token: 0x04007D25 RID: 32037
	public static readonly Dictionary<string, TriggerEffectManager.TriggerEffectDS> ControllerTriggerEffectsDS = new Dictionary<string, TriggerEffectManager.TriggerEffectDS>();

	// Token: 0x04007D26 RID: 32038
	public static readonly Dictionary<string, TriggerEffectManager.TriggerEffectXB> ControllerTriggerEffectsXb = new Dictionary<string, TriggerEffectManager.TriggerEffectXB>();

	// Token: 0x04007D27 RID: 32039
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

	// Token: 0x04007D28 RID: 32040
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TriggerEffectManager.TriggerEffectXB NoneEffectXb = new TriggerEffectManager.TriggerEffectXB
	{
		Effect = TriggerEffectManager.EffectXbox.Off,
		Strength = 0f
	};

	// Token: 0x04007D29 RID: 32041
	public static readonly TriggerEffectManager.ControllerTriggerEffect NoneEffect = new TriggerEffectManager.ControllerTriggerEffect
	{
		DualsenseEffect = TriggerEffectManager.NoneEffectDs,
		XboxTriggerEffect = TriggerEffectManager.NoneEffectXb
	};

	// Token: 0x04007D2A RID: 32042
	[PublicizedFrom(EAccessModifier.Private)]
	public bool[] _controllersConnected = new bool[4];

	// Token: 0x04007D2B RID: 32043
	[PublicizedFrom(EAccessModifier.Private)]
	public float _currentControllerVibrationStrength;

	// Token: 0x04007D2C RID: 32044
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _enabled;

	// Token: 0x04007D2D RID: 32045
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _triggerSetLeft;

	// Token: 0x04007D2E RID: 32046
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _triggerSetRight;

	// Token: 0x04007D2F RID: 32047
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _stateChanged;

	// Token: 0x04007D30 RID: 32048
	[PublicizedFrom(EAccessModifier.Private)]
	public TriggerEffectManager.ControllerTriggerEffect _currentEffectLeft;

	// Token: 0x04007D31 RID: 32049
	[PublicizedFrom(EAccessModifier.Private)]
	public TriggerEffectManager.ControllerTriggerEffect _currentEffectRight;

	// Token: 0x04007D32 RID: 32050
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAudioRumbleStrengthSubtle = 0.5f;

	// Token: 0x04007D33 RID: 32051
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAudioRumbleStrengthStandard = 1f;

	// Token: 0x04007D34 RID: 32052
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAudioRumbleStrengthStrong = 2f;

	// Token: 0x04007D35 RID: 32053
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDualSenseRumbleStrengthMultiplier = 0.25f;

	// Token: 0x04007D36 RID: 32054
	[PublicizedFrom(EAccessModifier.Private)]
	public static float audioRumbleStrength = 1f;

	// Token: 0x04007D37 RID: 32055
	public AudioGamepadRumbleSource[] vibrationAudioSources = new AudioGamepadRumbleSource[5];

	// Token: 0x04007D38 RID: 32056
	[PublicizedFrom(EAccessModifier.Protected)]
	public static LightbarGradients lightbarGradients;

	// Token: 0x04007D39 RID: 32057
	[PublicizedFrom(EAccessModifier.Private)]
	public float leftAudioStrength;

	// Token: 0x04007D3A RID: 32058
	[PublicizedFrom(EAccessModifier.Private)]
	public float rightAudioStrength;

	// Token: 0x04007D3B RID: 32059
	[PublicizedFrom(EAccessModifier.Private)]
	public float targetLeftAudioStrength;

	// Token: 0x04007D3C RID: 32060
	[PublicizedFrom(EAccessModifier.Private)]
	public float targetRightAudioStrength;

	// Token: 0x04007D3D RID: 32061
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _inUI;

	// Token: 0x02001510 RID: 5392
	public enum GamepadTrigger
	{
		// Token: 0x04007D3F RID: 32063
		LeftTrigger,
		// Token: 0x04007D40 RID: 32064
		RightTrigger
	}

	// Token: 0x02001511 RID: 5393
	public enum EffectDualsense
	{
		// Token: 0x04007D42 RID: 32066
		Off,
		// Token: 0x04007D43 RID: 32067
		WeaponSingle,
		// Token: 0x04007D44 RID: 32068
		WeaponMultipoint,
		// Token: 0x04007D45 RID: 32069
		FeedbackSingle,
		// Token: 0x04007D46 RID: 32070
		VibrationSingle,
		// Token: 0x04007D47 RID: 32071
		FeedbackSlope,
		// Token: 0x04007D48 RID: 32072
		VibrationSlope,
		// Token: 0x04007D49 RID: 32073
		FeedbackMultipoint,
		// Token: 0x04007D4A RID: 32074
		VibrationMultipoint
	}

	// Token: 0x02001512 RID: 5394
	public struct TriggerEffectDS
	{
		// Token: 0x04007D4B RID: 32075
		public TriggerEffectManager.EffectDualsense Effect;

		// Token: 0x04007D4C RID: 32076
		public byte Position;

		// Token: 0x04007D4D RID: 32077
		public byte EndPosition;

		// Token: 0x04007D4E RID: 32078
		public byte Frequency;

		// Token: 0x04007D4F RID: 32079
		public byte AmplitudeEndStrength;

		// Token: 0x04007D50 RID: 32080
		public byte Strength;

		// Token: 0x04007D51 RID: 32081
		public byte[] Strengths;
	}

	// Token: 0x02001513 RID: 5395
	public enum EffectXbox
	{
		// Token: 0x04007D53 RID: 32083
		Off,
		// Token: 0x04007D54 RID: 32084
		FeedbackSingle,
		// Token: 0x04007D55 RID: 32085
		VibrationSingle,
		// Token: 0x04007D56 RID: 32086
		FeedbackSlope,
		// Token: 0x04007D57 RID: 32087
		VibrationSlope
	}

	// Token: 0x02001514 RID: 5396
	public struct TriggerEffectXB
	{
		// Token: 0x04007D58 RID: 32088
		public TriggerEffectManager.EffectXbox Effect;

		// Token: 0x04007D59 RID: 32089
		public float Strength;

		// Token: 0x04007D5A RID: 32090
		[FormerlySerializedAs("endStrength")]
		[FormerlySerializedAs("Amplitude")]
		public float EndStrength;

		// Token: 0x04007D5B RID: 32091
		public float StartPosition;

		// Token: 0x04007D5C RID: 32092
		public float EndPosition;
	}

	// Token: 0x02001515 RID: 5397
	public struct ControllerTriggerEffect
	{
		// Token: 0x04007D5D RID: 32093
		public TriggerEffectManager.TriggerEffectDS DualsenseEffect;

		// Token: 0x04007D5E RID: 32094
		public TriggerEffectManager.TriggerEffectXB XboxTriggerEffect;
	}
}
