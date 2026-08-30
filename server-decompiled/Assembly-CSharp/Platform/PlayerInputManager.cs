using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InControl;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001BEF RID: 7151
	public class PlayerInputManager
	{
		// Token: 0x0600D49F RID: 54431 RVA: 0x004CB144 File Offset: 0x004C9344
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

		// Token: 0x0600D4A0 RID: 54432 RVA: 0x004CB400 File Offset: 0x004C9600
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

		// Token: 0x0600D4A1 RID: 54433 RVA: 0x004CB57E File Offset: 0x004C977E
		public void ForceInputStyleChange()
		{
			Action<PlayerInputManager.InputStyle> onLastInputStyleChanged = this.OnLastInputStyleChanged;
			if (onLastInputStyleChanged == null)
			{
				return;
			}
			onLastInputStyleChanged(this.CurrentInputStyle);
		}

		// Token: 0x1400012A RID: 298
		// (add) Token: 0x0600D4A2 RID: 54434 RVA: 0x004CB598 File Offset: 0x004C9798
		// (remove) Token: 0x0600D4A3 RID: 54435 RVA: 0x004CB5D0 File Offset: 0x004C97D0
		public event Action<PlayerInputManager.InputStyle> OnLastInputStyleChanged;

		// Token: 0x17001A63 RID: 6755
		// (get) Token: 0x0600D4A4 RID: 54436 RVA: 0x004CB605 File Offset: 0x004C9805
		// (set) Token: 0x0600D4A5 RID: 54437 RVA: 0x004CB60D File Offset: 0x004C980D
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

		// Token: 0x17001A64 RID: 6756
		// (get) Token: 0x0600D4A6 RID: 54438 RVA: 0x004CB625 File Offset: 0x004C9825
		// (set) Token: 0x0600D4A7 RID: 54439 RVA: 0x004CB643 File Offset: 0x004C9843
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

		// Token: 0x17001A65 RID: 6757
		// (get) Token: 0x0600D4A8 RID: 54440 RVA: 0x004CB64C File Offset: 0x004C984C
		public ReadOnlyCollection<PlayerActionsBase> ActionSets { get; }

		// Token: 0x0600D4A9 RID: 54441 RVA: 0x004CB654 File Offset: 0x004C9854
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

		// Token: 0x0600D4AA RID: 54442 RVA: 0x004CB6D0 File Offset: 0x004C98D0
		public void ResetInputStyleUsage()
		{
			for (int i = 0; i < this.inputStylesUsedMinutes.Length; i++)
			{
				this.inputStylesUsedMinutes[i] = 0f;
				this.lastInputStyleSwitchTime = Time.unscaledTime;
			}
		}

		// Token: 0x0600D4AB RID: 54443 RVA: 0x004CB708 File Offset: 0x004C9908
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

		// Token: 0x0600D4AC RID: 54444 RVA: 0x004CB780 File Offset: 0x004C9980
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

		// Token: 0x17001A66 RID: 6758
		// (get) Token: 0x0600D4AD RID: 54445 RVA: 0x004CB7DC File Offset: 0x004C99DC
		public PlayerActionsLocal PrimaryPlayer { get; }

		// Token: 0x0600D4AE RID: 54446 RVA: 0x004CB7E4 File Offset: 0x004C99E4
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

		// Token: 0x0600D4AF RID: 54447 RVA: 0x004CB870 File Offset: 0x004C9A70
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

		// Token: 0x0400A1B2 RID: 41394
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerInputManager.InputStyle defaultInputStyle = PlayerInputManager.InputStyle.Keyboard;

		// Token: 0x0400A1B3 RID: 41395
		[PublicizedFrom(EAccessModifier.Private)]
		public bool firstInputDetected;

		// Token: 0x0400A1B4 RID: 41396
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float[] inputStylesUsedMinutes = new float[4];

		// Token: 0x0400A1B5 RID: 41397
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastInputStyleSwitchTime;

		// Token: 0x0400A1B7 RID: 41399
		[PublicizedFrom(EAccessModifier.Private)]
		public string lastInputDeviceName;

		// Token: 0x0400A1B8 RID: 41400
		[PublicizedFrom(EAccessModifier.Private)]
		public InputDevice lastInputDevice;

		// Token: 0x0400A1B9 RID: 41401
		[PublicizedFrom(EAccessModifier.Private)]
		public InputDevice newInputDevice;

		// Token: 0x0400A1BA RID: 41402
		[PublicizedFrom(EAccessModifier.Private)]
		public BindingSourceType lastBindingSource;

		// Token: 0x0400A1BB RID: 41403
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerInputManager.InputStyle _currentInputStyle;

		// Token: 0x0400A1BC RID: 41404
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerInputManager.InputStyle _currentControllerInputStyle = PlayerInputManager.InputStyle.XB1;

		// Token: 0x0400A1BE RID: 41406
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<PlayerActionsBase> actionSets = new List<PlayerActionsBase>();

		// Token: 0x0400A1C0 RID: 41408
		public readonly ActionSetManager ActionSetManager = new ActionSetManager();

		// Token: 0x02001BF0 RID: 7152
		public enum InputStyle
		{
			// Token: 0x0400A1C2 RID: 41410
			Undefined,
			// Token: 0x0400A1C3 RID: 41411
			Keyboard,
			// Token: 0x0400A1C4 RID: 41412
			PS4,
			// Token: 0x0400A1C5 RID: 41413
			XB1,
			// Token: 0x0400A1C6 RID: 41414
			Count
		}

		// Token: 0x02001BF1 RID: 7153
		public enum ControllerIconStyle
		{
			// Token: 0x0400A1C8 RID: 41416
			Automatic,
			// Token: 0x0400A1C9 RID: 41417
			Xbox,
			// Token: 0x0400A1CA RID: 41418
			Playstation
		}
	}
}
