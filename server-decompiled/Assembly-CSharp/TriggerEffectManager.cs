using System;
using System.Collections.Generic;
using InControl;
using Platform;
using TriggerEffects;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

// Token: 0x020014FD RID: 5373
[global::UnityEngine.Scripting.Preserve]
public class TriggerEffectManager : IDisposable
{
	// Token: 0x170013EC RID: 5100
	// (get) Token: 0x0600A865 RID: 43109 RVA: 0x003EACA3 File Offset: 0x003E8EA3
	// (set) Token: 0x0600A866 RID: 43110 RVA: 0x003EACAB File Offset: 0x003E8EAB
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

	// Token: 0x0600A867 RID: 43111 RVA: 0x003EACBC File Offset: 0x003E8EBC
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

	// Token: 0x0600A868 RID: 43112 RVA: 0x003EAD2B File Offset: 0x003E8F2B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnControllerConnected(InputDevice device)
	{
		this.Shutdown();
		this.PollSetting();
		this.Init();
		this._stateChanged = true;
		GameManager.Instance.triggerEffectManager.EnableVibration();
	}

	// Token: 0x0600A869 RID: 43113 RVA: 0x003EAD55 File Offset: 0x003E8F55
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

	// Token: 0x0600A86A RID: 43114 RVA: 0x003EAD7C File Offset: 0x003E8F7C
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

	// Token: 0x0600A86B RID: 43115 RVA: 0x003EAE08 File Offset: 0x003E9008
	public void EnableVibration()
	{
		TriggerEffectDualsensePC.EnableVibration();
		TriggerEffectDualsense.EnableVibration();
	}

	// Token: 0x0600A86C RID: 43116 RVA: 0x003EAE14 File Offset: 0x003E9014
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitializeLightbarGradient()
	{
		TriggerEffectManager.lightbarGradients = Resources.Load<LightbarGradients>("Data/LightBarGradients");
	}

	// Token: 0x0600A86D RID: 43117 RVA: 0x003EAE25 File Offset: 0x003E9025
	public static void SetEnabled(bool _enabled)
	{
		GameManager.Instance.triggerEffectManager.Enabled = _enabled;
	}

	// Token: 0x0600A86E RID: 43118 RVA: 0x003EAE37 File Offset: 0x003E9037
	[PublicizedFrom(EAccessModifier.Private)]
	public byte FloatToByte(float value)
	{
		return (byte)(Mathf.Clamp01(value) * 255f);
	}

	// Token: 0x0600A86F RID: 43119 RVA: 0x003EAE48 File Offset: 0x003E9048
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

	// Token: 0x0600A870 RID: 43120 RVA: 0x003EB1A9 File Offset: 0x003E93A9
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

	// Token: 0x0600A871 RID: 43121 RVA: 0x003EB1E7 File Offset: 0x003E93E7
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

	// Token: 0x0600A872 RID: 43122 RVA: 0x003EB211 File Offset: 0x003E9411
	public void ResetControllerIdentification()
	{
		TriggerEffectDualsensePC.ResetControllerIdentification();
		TriggerEffectDualsense.ResetControllerIdentification();
		this._stateChanged = true;
	}

	// Token: 0x0600A873 RID: 43123 RVA: 0x003EB224 File Offset: 0x003E9424
	public void SetControllerIdentification()
	{
		TriggerEffectDualsensePC.SetControllerIdentification();
		TriggerEffectDualsense.SetControllerIdentification();
		this._stateChanged = true;
	}

	// Token: 0x0600A874 RID: 43124 RVA: 0x003EB237 File Offset: 0x003E9437
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

	// Token: 0x0600A875 RID: 43125 RVA: 0x003EB261 File Offset: 0x003E9461
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

	// Token: 0x0600A876 RID: 43126 RVA: 0x003EB288 File Offset: 0x003E9488
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

	// Token: 0x0600A877 RID: 43127 RVA: 0x003EB2F5 File Offset: 0x003E94F5
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LibShutdown()
	{
		TriggerEffectDualsensePC.LibShutdown();
	}

	// Token: 0x0600A878 RID: 43128 RVA: 0x003EB2FC File Offset: 0x003E94FC
	public void Dispose()
	{
		this.Shutdown();
		InputManager.OnDeviceAttached -= this.OnControllerConnected;
	}

	// Token: 0x0600A879 RID: 43129 RVA: 0x003EB315 File Offset: 0x003E9515
	public void PollSetting()
	{
		this.Enabled = GamePrefs.GetBool(EnumGamePrefs.OptionsControllerTriggerEffects);
	}

	// Token: 0x170013ED RID: 5101
	// (get) Token: 0x0600A87A RID: 43130 RVA: 0x003EB327 File Offset: 0x003E9527
	// (set) Token: 0x0600A87B RID: 43131 RVA: 0x003EB32F File Offset: 0x003E952F
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

	// Token: 0x0600A87C RID: 43132 RVA: 0x003EB348 File Offset: 0x003E9548
	public void SetGamepadVibration(float strength)
	{
		this._currentControllerVibrationStrength = strength;
		this._stateChanged = true;
	}

	// Token: 0x0600A87D RID: 43133 RVA: 0x003EB358 File Offset: 0x003E9558
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

	// Token: 0x0600A87E RID: 43134 RVA: 0x003EB3A9 File Offset: 0x003E95A9
	public void SetTriggerEffect(TriggerEffectManager.ControllerTriggerEffect effect)
	{
		if (!this.Enabled)
		{
			return;
		}
		this.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.LeftTrigger, effect, false);
		this.SetTriggerEffect(TriggerEffectManager.GamepadTrigger.RightTrigger, effect, false);
	}

	// Token: 0x0600A87F RID: 43135 RVA: 0x003EB3C8 File Offset: 0x003E95C8
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

	// Token: 0x0600A880 RID: 43136 RVA: 0x003EB43C File Offset: 0x003E963C
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

	// Token: 0x0600A881 RID: 43137 RVA: 0x003EB53C File Offset: 0x003E973C
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

	// Token: 0x0600A882 RID: 43138 RVA: 0x003EB608 File Offset: 0x003E9808
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

	// Token: 0x0600A883 RID: 43139 RVA: 0x003EB712 File Offset: 0x003E9912
	public static bool SettingDefaultValue()
	{
		return Application.platform == RuntimePlatform.PS5 || Application.platform == RuntimePlatform.GameCoreXboxSeries || Application.platform == RuntimePlatform.WindowsEditor;
	}

	// Token: 0x0600A884 RID: 43140 RVA: 0x003EB730 File Offset: 0x003E9930
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

	// Token: 0x0600A885 RID: 43141 RVA: 0x003EB79C File Offset: 0x003E999C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDualSenseVibration(byte _smallMotor, byte _largeMotor)
	{
		TriggerEffectDualsense.SetDualSenseVibration(_smallMotor, _largeMotor);
		TriggerEffectDualsensePC.SetDualSenseVibration(_smallMotor, _largeMotor);
	}

	// Token: 0x0600A886 RID: 43142 RVA: 0x003EB7AC File Offset: 0x003E99AC
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

	// Token: 0x0600A887 RID: 43143 RVA: 0x003EB806 File Offset: 0x003E9A06
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		if (_style == PlayerInputManager.InputStyle.Keyboard)
		{
			this.StopGamepadVibration(false);
		}
	}

	// Token: 0x0600A888 RID: 43144 RVA: 0x003EB814 File Offset: 0x003E9A14
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

	// Token: 0x0600A889 RID: 43145 RVA: 0x003EB9C0 File Offset: 0x003E9BC0
	public static void SetDualSenseLightbarColor(Color color)
	{
		for (int i = 1; i < 5; i++)
		{
			TriggerEffectDualsense.SetLightbar(i, (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f));
			TriggerEffectDualsensePC.SetLightbar(i, (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f));
		}
	}

	// Token: 0x0600A88A RID: 43146 RVA: 0x003EBA33 File Offset: 0x003E9C33
	public static void SetMainMenuLightbarColor()
	{
		TriggerEffectManager.SetDualSenseLightbarColor(TriggerEffectManager.lightbarGradients.mainMenuColor);
	}

	// Token: 0x04007CE4 RID: 31972
	public static readonly Dictionary<string, TriggerEffectManager.TriggerEffectDS> ControllerTriggerEffectsDS = new Dictionary<string, TriggerEffectManager.TriggerEffectDS>();

	// Token: 0x04007CE5 RID: 31973
	public static readonly Dictionary<string, TriggerEffectManager.TriggerEffectXB> ControllerTriggerEffectsXb = new Dictionary<string, TriggerEffectManager.TriggerEffectXB>();

	// Token: 0x04007CE6 RID: 31974
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

	// Token: 0x04007CE7 RID: 31975
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly TriggerEffectManager.TriggerEffectXB NoneEffectXb = new TriggerEffectManager.TriggerEffectXB
	{
		Effect = TriggerEffectManager.EffectXbox.Off,
		Strength = 0f
	};

	// Token: 0x04007CE8 RID: 31976
	public static readonly TriggerEffectManager.ControllerTriggerEffect NoneEffect = new TriggerEffectManager.ControllerTriggerEffect
	{
		DualsenseEffect = TriggerEffectManager.NoneEffectDs,
		XboxTriggerEffect = TriggerEffectManager.NoneEffectXb
	};

	// Token: 0x04007CE9 RID: 31977
	[PublicizedFrom(EAccessModifier.Private)]
	public bool[] _controllersConnected = new bool[4];

	// Token: 0x04007CEA RID: 31978
	[PublicizedFrom(EAccessModifier.Private)]
	public float _currentControllerVibrationStrength;

	// Token: 0x04007CEB RID: 31979
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _enabled;

	// Token: 0x04007CEC RID: 31980
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _triggerSetLeft;

	// Token: 0x04007CED RID: 31981
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _triggerSetRight;

	// Token: 0x04007CEE RID: 31982
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _stateChanged;

	// Token: 0x04007CEF RID: 31983
	[PublicizedFrom(EAccessModifier.Private)]
	public TriggerEffectManager.ControllerTriggerEffect _currentEffectLeft;

	// Token: 0x04007CF0 RID: 31984
	[PublicizedFrom(EAccessModifier.Private)]
	public TriggerEffectManager.ControllerTriggerEffect _currentEffectRight;

	// Token: 0x04007CF1 RID: 31985
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAudioRumbleStrengthSubtle = 0.5f;

	// Token: 0x04007CF2 RID: 31986
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAudioRumbleStrengthStandard = 1f;

	// Token: 0x04007CF3 RID: 31987
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cAudioRumbleStrengthStrong = 2f;

	// Token: 0x04007CF4 RID: 31988
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cDualSenseRumbleStrengthMultiplier = 0.25f;

	// Token: 0x04007CF5 RID: 31989
	[PublicizedFrom(EAccessModifier.Private)]
	public static float audioRumbleStrength = 1f;

	// Token: 0x04007CF6 RID: 31990
	public AudioGamepadRumbleSource[] vibrationAudioSources = new AudioGamepadRumbleSource[5];

	// Token: 0x04007CF7 RID: 31991
	[PublicizedFrom(EAccessModifier.Protected)]
	public static LightbarGradients lightbarGradients;

	// Token: 0x04007CF8 RID: 31992
	[PublicizedFrom(EAccessModifier.Private)]
	public float leftAudioStrength;

	// Token: 0x04007CF9 RID: 31993
	[PublicizedFrom(EAccessModifier.Private)]
	public float rightAudioStrength;

	// Token: 0x04007CFA RID: 31994
	[PublicizedFrom(EAccessModifier.Private)]
	public float targetLeftAudioStrength;

	// Token: 0x04007CFB RID: 31995
	[PublicizedFrom(EAccessModifier.Private)]
	public float targetRightAudioStrength;

	// Token: 0x04007CFC RID: 31996
	[PublicizedFrom(EAccessModifier.Private)]
	public bool _inUI;

	// Token: 0x020014FE RID: 5374
	public enum GamepadTrigger
	{
		// Token: 0x04007CFE RID: 31998
		LeftTrigger,
		// Token: 0x04007CFF RID: 31999
		RightTrigger
	}

	// Token: 0x020014FF RID: 5375
	public enum EffectDualsense
	{
		// Token: 0x04007D01 RID: 32001
		Off,
		// Token: 0x04007D02 RID: 32002
		WeaponSingle,
		// Token: 0x04007D03 RID: 32003
		WeaponMultipoint,
		// Token: 0x04007D04 RID: 32004
		FeedbackSingle,
		// Token: 0x04007D05 RID: 32005
		VibrationSingle,
		// Token: 0x04007D06 RID: 32006
		FeedbackSlope,
		// Token: 0x04007D07 RID: 32007
		VibrationSlope,
		// Token: 0x04007D08 RID: 32008
		FeedbackMultipoint,
		// Token: 0x04007D09 RID: 32009
		VibrationMultipoint
	}

	// Token: 0x02001500 RID: 5376
	public struct TriggerEffectDS
	{
		// Token: 0x04007D0A RID: 32010
		public TriggerEffectManager.EffectDualsense Effect;

		// Token: 0x04007D0B RID: 32011
		public byte Position;

		// Token: 0x04007D0C RID: 32012
		public byte EndPosition;

		// Token: 0x04007D0D RID: 32013
		public byte Frequency;

		// Token: 0x04007D0E RID: 32014
		public byte AmplitudeEndStrength;

		// Token: 0x04007D0F RID: 32015
		public byte Strength;

		// Token: 0x04007D10 RID: 32016
		public byte[] Strengths;
	}

	// Token: 0x02001501 RID: 5377
	public enum EffectXbox
	{
		// Token: 0x04007D12 RID: 32018
		Off,
		// Token: 0x04007D13 RID: 32019
		FeedbackSingle,
		// Token: 0x04007D14 RID: 32020
		VibrationSingle,
		// Token: 0x04007D15 RID: 32021
		FeedbackSlope,
		// Token: 0x04007D16 RID: 32022
		VibrationSlope
	}

	// Token: 0x02001502 RID: 5378
	public struct TriggerEffectXB
	{
		// Token: 0x04007D17 RID: 32023
		public TriggerEffectManager.EffectXbox Effect;

		// Token: 0x04007D18 RID: 32024
		public float Strength;

		// Token: 0x04007D19 RID: 32025
		[FormerlySerializedAs("endStrength")]
		[FormerlySerializedAs("Amplitude")]
		public float EndStrength;

		// Token: 0x04007D1A RID: 32026
		public float StartPosition;

		// Token: 0x04007D1B RID: 32027
		public float EndPosition;
	}

	// Token: 0x02001503 RID: 5379
	public struct ControllerTriggerEffect
	{
		// Token: 0x04007D1C RID: 32028
		public TriggerEffectManager.TriggerEffectDS DualsenseEffect;

		// Token: 0x04007D1D RID: 32029
		public TriggerEffectManager.TriggerEffectXB XboxTriggerEffect;
	}
}
