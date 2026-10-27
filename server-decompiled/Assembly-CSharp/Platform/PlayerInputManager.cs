using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InControl;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001C52 RID: 7250
	public class PlayerInputManager
	{
		// Token: 0x0600D72B RID: 55083 RVA: 0x004D3754 File Offset: 0x004D1954
		public PlayerInputManager()
		{
			Log.Out("Starting PlayerInputManager...");
			MouseBindingSource.ScaleX = (MouseBindingSource.ScaleY = (MouseBindingSource.ScaleZ = 0.2f));
			GameObject gameObject = GameObject.Find("Input");
			if (gameObject == null)
			{
				gameObject = new GameObject("Input");
				global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			InControlManager inControlManager = gameObject.GetComponent<InControlManager>();
			if (inControlManager != null)
			{
				Log.Error("InControl already instantiated");
				return;
			}
			bool flag = GameUtils.GetLaunchArgument("noxinput") == null;
			bool flag2 = GameUtils.GetLaunchArgument("disablenativeinput") == null;
			if (GameManager.IsDedicatedServer)
			{
				flag = false;
				flag2 = false;
			}
			gameObject.SetActive(false);
			inControlManager = gameObject.AddComponent<InControlManager>();
			inControlManager.logDebugInfo = false;
			inControlManager.suspendInBackground = true;
			inControlManager.nativeInputPreventSleep = true;
			inControlManager.enableNativeInput = flag2;
			inControlManager.enableXInput = flag;
			inControlManager.nativeInputEnableXInput = flag;
			InputManager.AddCustomDeviceManagers += delegate(ref bool enableUnityInput)
			{
			};
			global::InControl.Logger.OnLogMessage += delegate(LogMessage _message)
			{
				switch (_message.Type)
				{
				case LogMessageType.Info:
					Log.Out(_message.Text);
					return;
				case LogMessageType.Warning:
					Log.Warning(_message.Text);
					return;
				case LogMessageType.Error:
					Log.Error(_message.Text);
					return;
				default:
					return;
				}
			};
			global::InControl.Logger.LogInfo(string.Concat(new string[]
			{
				"InControl (version ",
				InputManager.Version.ToString(),
				", native module = ",
				inControlManager.enableNativeInput.ToString(),
				", XInput = ",
				inControlManager.enableXInput.ToString(),
				")"
			}));
			gameObject.SetActive(true);
			PlayerActionsGlobal.Init();
			if (!Submission.Enabled)
			{
				this.actionSets.Add(PlayerActionsGlobal.Instance);
			}
			this.PrimaryPlayer = new PlayerActionsLocal();
			this.actionSets.Add(this.PrimaryPlayer);
			this.actionSets.Add(this.PrimaryPlayer.VehicleActions);
			this.actionSets.Add(this.PrimaryPlayer.GUIActions);
			this.actionSets.Add(this.PrimaryPlayer.PermanentActions);
			this.ActionSets = new ReadOnlyCollection<PlayerActionsBase>(this.actionSets);
			for (int i = 0; i < this.actionSets.Count; i++)
			{
				PlayerActionSet actionSet = this.actionSets[i];
				actionSet.OnLastInputTypeChanged += delegate(BindingSourceType _type)
				{
					if (_type == BindingSourceType.DeviceBindingSource)
					{
						this.newInputDevice = actionSet.Device ?? InputManager.ActiveDevice;
						return;
					}
					this.newInputDevice = InputDevice.Null;
				};
			}
			if (!GameManager.IsDedicatedServer)
			{
				this.ActionSetManager.Push(this.PrimaryPlayer);
			}
			this.CurrentInputStyle = this.defaultInputStyle;
		}

		// Token: 0x0600D72C RID: 55084 RVA: 0x004D3A10 File Offset: 0x004D1C10
		public void Update()
		{
			BindingSourceType bindingSourceType;
			this.newInputDevice = this.LastActiveInputDevice(out bindingSourceType);
			if (!this.firstInputDetected && bindingSourceType == BindingSourceType.None)
			{
				return;
			}
			if (this.lastInputDevice == this.newInputDevice)
			{
				if (bindingSourceType == this.lastBindingSource)
				{
					return;
				}
				if (bindingSourceType == BindingSourceType.KeyBindingSource && this.lastBindingSource == BindingSourceType.MouseBindingSource)
				{
					return;
				}
				if (bindingSourceType == BindingSourceType.MouseBindingSource && this.lastBindingSource == BindingSourceType.KeyBindingSource)
				{
					return;
				}
			}
			if (!this.firstInputDetected)
			{
				this.firstInputDetected = true;
			}
			this.lastInputDevice = this.newInputDevice;
			this.lastBindingSource = bindingSourceType;
			PlayerInputManager.InputStyle currentInputStyle = this.CurrentInputStyle;
			if (bindingSourceType == BindingSourceType.KeyBindingSource || bindingSourceType == BindingSourceType.MouseBindingSource)
			{
				this.lastInputDeviceName = null;
				this.CurrentInputStyle = PlayerInputManager.InputStyle.Keyboard;
			}
			else if (this.lastInputDevice.Name == "None" || bindingSourceType == BindingSourceType.None)
			{
				this.lastInputDeviceName = null;
				this.CurrentInputStyle = this.defaultInputStyle;
			}
			else
			{
				string name = this.lastInputDevice.Name;
				if (name != this.lastInputDeviceName)
				{
					this.lastInputDeviceName = name;
					this.CurrentInputStyle = ((this.lastInputDevice.DeviceStyle == InputDeviceStyle.PlayStation2 || this.lastInputDevice.DeviceStyle == InputDeviceStyle.PlayStation3 || this.lastInputDevice.DeviceStyle == InputDeviceStyle.PlayStation4 || this.lastInputDevice.DeviceStyle == InputDeviceStyle.PlayStation5) ? PlayerInputManager.InputStyle.PS4 : PlayerInputManager.InputStyle.XB1);
				}
			}
			if (currentInputStyle != this.CurrentInputStyle && currentInputStyle != PlayerInputManager.InputStyle.Undefined)
			{
				float unscaledTime = Time.unscaledTime;
				this.inputStylesUsedMinutes[(int)currentInputStyle] += (unscaledTime - this.lastInputStyleSwitchTime) / 60f;
				this.lastInputStyleSwitchTime = unscaledTime;
			}
			Action<PlayerInputManager.InputStyle> onLastInputStyleChanged = this.OnLastInputStyleChanged;
			if (onLastInputStyleChanged == null)
			{
				return;
			}
			onLastInputStyleChanged(this.CurrentInputStyle);
		}

		// Token: 0x0600D72D RID: 55085 RVA: 0x004D3B8E File Offset: 0x004D1D8E
		public void ForceInputStyleChange()
		{
			Action<PlayerInputManager.InputStyle> onLastInputStyleChanged = this.OnLastInputStyleChanged;
			if (onLastInputStyleChanged == null)
			{
				return;
			}
			onLastInputStyleChanged(this.CurrentInputStyle);
		}

		// Token: 0x14000129 RID: 297
		// (add) Token: 0x0600D72E RID: 55086 RVA: 0x004D3BA8 File Offset: 0x004D1DA8
		// (remove) Token: 0x0600D72F RID: 55087 RVA: 0x004D3BE0 File Offset: 0x004D1DE0
		public event Action<PlayerInputManager.InputStyle> OnLastInputStyleChanged;

		// Token: 0x17001B51 RID: 6993
		// (get) Token: 0x0600D730 RID: 55088 RVA: 0x004D3C15 File Offset: 0x004D1E15
		// (set) Token: 0x0600D731 RID: 55089 RVA: 0x004D3C1D File Offset: 0x004D1E1D
		public PlayerInputManager.InputStyle CurrentInputStyle
		{
			get
			{
				return this._currentInputStyle;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				if (value == PlayerInputManager.InputStyle.PS4 || value == PlayerInputManager.InputStyle.XB1)
				{
					this.CurrentControllerInputStyle = value;
				}
				this._currentInputStyle = value;
			}
		}

		// Token: 0x17001B52 RID: 6994
		// (get) Token: 0x0600D732 RID: 55090 RVA: 0x004D3C35 File Offset: 0x004D1E35
		// (set) Token: 0x0600D733 RID: 55091 RVA: 0x004D3C53 File Offset: 0x004D1E53
		public PlayerInputManager.InputStyle CurrentControllerInputStyle
		{
			get
			{
				if (DeviceFlag.PS5.IsCurrent())
				{
					return PlayerInputManager.InputStyle.PS4;
				}
				if ((DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX).IsCurrent())
				{
					return PlayerInputManager.InputStyle.XB1;
				}
				return this._currentControllerInputStyle;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				this._currentControllerInputStyle = value;
			}
		}

		// Token: 0x17001B53 RID: 6995
		// (get) Token: 0x0600D734 RID: 55092 RVA: 0x004D3C5C File Offset: 0x004D1E5C
		public ReadOnlyCollection<PlayerActionsBase> ActionSets { get; }

		// Token: 0x0600D735 RID: 55093 RVA: 0x004D3C64 File Offset: 0x004D1E64
		[PublicizedFrom(EAccessModifier.Private)]
		public InputDevice LastActiveInputDevice(out BindingSourceType lastBindingSource)
		{
			ulong num = 0UL;
			PlayerActionSet playerActionSet = null;
			lastBindingSource = BindingSourceType.None;
			for (int i = 0; i < this.ActionSets.Count; i++)
			{
				PlayerActionSet playerActionSet2 = this.ActionSets[i];
				if (playerActionSet2.Enabled && playerActionSet2.LastInputTypeChangedTick > num)
				{
					playerActionSet = playerActionSet2;
					num = playerActionSet2.LastInputTypeChangedTick;
				}
			}
			if (playerActionSet != null)
			{
				lastBindingSource = playerActionSet.LastInputType;
				if (playerActionSet.LastInputType == BindingSourceType.DeviceBindingSource)
				{
					return playerActionSet.Device ?? InputManager.ActiveDevice;
				}
			}
			return InputDevice.Null;
		}

		// Token: 0x0600D736 RID: 55094 RVA: 0x004D3CE0 File Offset: 0x004D1EE0
		public void ResetInputStyleUsage()
		{
			for (int i = 0; i < this.inputStylesUsedMinutes.Length; i++)
			{
				this.inputStylesUsedMinutes[i] = 0f;
				this.lastInputStyleSwitchTime = Time.unscaledTime;
			}
		}

		// Token: 0x0600D737 RID: 55095 RVA: 0x004D3D18 File Offset: 0x004D1F18
		public PlayerInputManager.InputStyle MostUsedInputStyle()
		{
			if (this.CurrentInputStyle != PlayerInputManager.InputStyle.Undefined)
			{
				float unscaledTime = Time.unscaledTime;
				this.inputStylesUsedMinutes[(int)this.CurrentInputStyle] += (unscaledTime - this.lastInputStyleSwitchTime) / 60f;
				this.lastInputStyleSwitchTime = unscaledTime;
			}
			PlayerInputManager.InputStyle inputStyle = PlayerInputManager.InputStyle.Count;
			float num = -1f;
			for (int i = 0; i < this.inputStylesUsedMinutes.Length; i++)
			{
				if (this.inputStylesUsedMinutes[i] > num)
				{
					num = this.inputStylesUsedMinutes[i];
					inputStyle = (PlayerInputManager.InputStyle)i;
				}
			}
			return inputStyle;
		}

		// Token: 0x0600D738 RID: 55096 RVA: 0x004D3D90 File Offset: 0x004D1F90
		public PlayerActionsBase GetActionSetForName(string _name)
		{
			foreach (PlayerActionsBase playerActionsBase in this.ActionSets)
			{
				if (playerActionsBase.Name.EqualsCaseInsensitive(_name))
				{
					return playerActionsBase;
				}
			}
			return null;
		}

		// Token: 0x17001B54 RID: 6996
		// (get) Token: 0x0600D739 RID: 55097 RVA: 0x004D3DEC File Offset: 0x004D1FEC
		public PlayerActionsLocal PrimaryPlayer { get; }

		// Token: 0x0600D73A RID: 55098 RVA: 0x004D3DF4 File Offset: 0x004D1FF4
		public void LoadActionSetsFromStrings(IList<string> actionSets)
		{
			if (this.ActionSets.Count != actionSets.Count)
			{
				Log.Warning(string.Format("Loading ActionSets from string array with incorrect length. Expected: {0}. Actual: {1}.", this.ActionSets.Count, (actionSets != null) ? new int?(actionSets.Count) : null));
				return;
			}
			for (int i = 0; i < this.ActionSets.Count; i++)
			{
				this.ActionSets[i].Load(actionSets[i]);
			}
		}

		// Token: 0x0600D73B RID: 55099 RVA: 0x004D3E80 File Offset: 0x004D2080
		public static PlayerInputManager.InputStyle InputStyleFromSelectedIconStyle()
		{
			PlayerInputManager.ControllerIconStyle @int = (PlayerInputManager.ControllerIconStyle)GamePrefs.GetInt(EnumGamePrefs.OptionsControllerIconStyle);
			if (@int == PlayerInputManager.ControllerIconStyle.Xbox)
			{
				return PlayerInputManager.InputStyle.XB1;
			}
			if (@int == PlayerInputManager.ControllerIconStyle.Playstation)
			{
				return PlayerInputManager.InputStyle.PS4;
			}
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			PlayerInputManager.InputStyle? inputStyle;
			if (nativePlatform == null)
			{
				inputStyle = null;
			}
			else
			{
				PlayerInputManager input = nativePlatform.Input;
				inputStyle = ((input != null) ? new PlayerInputManager.InputStyle?(input.CurrentControllerInputStyle) : null);
			}
			PlayerInputManager.InputStyle? inputStyle2 = inputStyle;
			if (inputStyle2 == null)
			{
				return PlayerInputManager.InputStyle.XB1;
			}
			return inputStyle2.GetValueOrDefault();
		}

		// Token: 0x0400A370 RID: 41840
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerInputManager.InputStyle defaultInputStyle = PlayerInputManager.InputStyle.Keyboard;

		// Token: 0x0400A371 RID: 41841
		[PublicizedFrom(EAccessModifier.Private)]
		public bool firstInputDetected;

		// Token: 0x0400A372 RID: 41842
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float[] inputStylesUsedMinutes = new float[4];

		// Token: 0x0400A373 RID: 41843
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastInputStyleSwitchTime;

		// Token: 0x0400A375 RID: 41845
		[PublicizedFrom(EAccessModifier.Private)]
		public string lastInputDeviceName;

		// Token: 0x0400A376 RID: 41846
		[PublicizedFrom(EAccessModifier.Private)]
		public InputDevice lastInputDevice;

		// Token: 0x0400A377 RID: 41847
		[PublicizedFrom(EAccessModifier.Private)]
		public InputDevice newInputDevice;

		// Token: 0x0400A378 RID: 41848
		[PublicizedFrom(EAccessModifier.Private)]
		public BindingSourceType lastBindingSource;

		// Token: 0x0400A379 RID: 41849
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerInputManager.InputStyle _currentInputStyle;

		// Token: 0x0400A37A RID: 41850
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerInputManager.InputStyle _currentControllerInputStyle = PlayerInputManager.InputStyle.XB1;

		// Token: 0x0400A37C RID: 41852
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<PlayerActionsBase> actionSets = new List<PlayerActionsBase>();

		// Token: 0x0400A37E RID: 41854
		public readonly ActionSetManager ActionSetManager = new ActionSetManager();

		// Token: 0x02001C53 RID: 7251
		public enum InputStyle
		{
			// Token: 0x0400A380 RID: 41856
			Undefined,
			// Token: 0x0400A381 RID: 41857
			Keyboard,
			// Token: 0x0400A382 RID: 41858
			PS4,
			// Token: 0x0400A383 RID: 41859
			XB1,
			// Token: 0x0400A384 RID: 41860
			Count
		}

		// Token: 0x02001C54 RID: 7252
		public enum ControllerIconStyle
		{
			// Token: 0x0400A386 RID: 41862
			Automatic,
			// Token: 0x0400A387 RID: 41863
			Xbox,
			// Token: 0x0400A388 RID: 41864
			Playstation
		}
	}
}
