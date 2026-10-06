using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InControl;
using UnityEngine;

namespace Platform
{
	// Token: 0x02001C21 RID: 7201
	public class PlayerInputManager
	{
		// Token: 0x0600D617 RID: 54807 RVA: 0x004CFC2C File Offset: 0x004CDE2C
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

		// Token: 0x0600D618 RID: 54808 RVA: 0x004CFEE8 File Offset: 0x004CE0E8
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

		// Token: 0x0600D619 RID: 54809 RVA: 0x004D0066 File Offset: 0x004CE266
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
		// (add) Token: 0x0600D61A RID: 54810 RVA: 0x004D0080 File Offset: 0x004CE280
		// (remove) Token: 0x0600D61B RID: 54811 RVA: 0x004D00B8 File Offset: 0x004CE2B8
		public event Action<PlayerInputManager.InputStyle> OnLastInputStyleChanged;

		// Token: 0x17001B09 RID: 6921
		// (get) Token: 0x0600D61C RID: 54812 RVA: 0x004D00ED File Offset: 0x004CE2ED
		// (set) Token: 0x0600D61D RID: 54813 RVA: 0x004D00F5 File Offset: 0x004CE2F5
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

		// Token: 0x17001B0A RID: 6922
		// (get) Token: 0x0600D61E RID: 54814 RVA: 0x004D010D File Offset: 0x004CE30D
		// (set) Token: 0x0600D61F RID: 54815 RVA: 0x004D012B File Offset: 0x004CE32B
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

		// Token: 0x17001B0B RID: 6923
		// (get) Token: 0x0600D620 RID: 54816 RVA: 0x004D0134 File Offset: 0x004CE334
		public ReadOnlyCollection<PlayerActionsBase> ActionSets { get; }

		// Token: 0x0600D621 RID: 54817 RVA: 0x004D013C File Offset: 0x004CE33C
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

		// Token: 0x0600D622 RID: 54818 RVA: 0x004D01B8 File Offset: 0x004CE3B8
		public void ResetInputStyleUsage()
		{
			for (int i = 0; i < this.inputStylesUsedMinutes.Length; i++)
			{
				this.inputStylesUsedMinutes[i] = 0f;
				this.lastInputStyleSwitchTime = Time.unscaledTime;
			}
		}

		// Token: 0x0600D623 RID: 54819 RVA: 0x004D01F0 File Offset: 0x004CE3F0
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

		// Token: 0x0600D624 RID: 54820 RVA: 0x004D0268 File Offset: 0x004CE468
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

		// Token: 0x17001B0C RID: 6924
		// (get) Token: 0x0600D625 RID: 54821 RVA: 0x004D02C4 File Offset: 0x004CE4C4
		public PlayerActionsLocal PrimaryPlayer { get; }

		// Token: 0x0600D626 RID: 54822 RVA: 0x004D02CC File Offset: 0x004CE4CC
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

		// Token: 0x0600D627 RID: 54823 RVA: 0x004D0358 File Offset: 0x004CE558
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

		// Token: 0x0400A287 RID: 41607
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerInputManager.InputStyle defaultInputStyle = PlayerInputManager.InputStyle.Keyboard;

		// Token: 0x0400A288 RID: 41608
		[PublicizedFrom(EAccessModifier.Private)]
		public bool firstInputDetected;

		// Token: 0x0400A289 RID: 41609
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly float[] inputStylesUsedMinutes = new float[4];

		// Token: 0x0400A28A RID: 41610
		[PublicizedFrom(EAccessModifier.Private)]
		public float lastInputStyleSwitchTime;

		// Token: 0x0400A28C RID: 41612
		[PublicizedFrom(EAccessModifier.Private)]
		public string lastInputDeviceName;

		// Token: 0x0400A28D RID: 41613
		[PublicizedFrom(EAccessModifier.Private)]
		public InputDevice lastInputDevice;

		// Token: 0x0400A28E RID: 41614
		[PublicizedFrom(EAccessModifier.Private)]
		public InputDevice newInputDevice;

		// Token: 0x0400A28F RID: 41615
		[PublicizedFrom(EAccessModifier.Private)]
		public BindingSourceType lastBindingSource;

		// Token: 0x0400A290 RID: 41616
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerInputManager.InputStyle _currentInputStyle;

		// Token: 0x0400A291 RID: 41617
		[PublicizedFrom(EAccessModifier.Private)]
		public PlayerInputManager.InputStyle _currentControllerInputStyle = PlayerInputManager.InputStyle.XB1;

		// Token: 0x0400A293 RID: 41619
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<PlayerActionsBase> actionSets = new List<PlayerActionsBase>();

		// Token: 0x0400A295 RID: 41621
		public readonly ActionSetManager ActionSetManager = new ActionSetManager();

		// Token: 0x02001C22 RID: 7202
		public enum InputStyle
		{
			// Token: 0x0400A297 RID: 41623
			Undefined,
			// Token: 0x0400A298 RID: 41624
			Keyboard,
			// Token: 0x0400A299 RID: 41625
			PS4,
			// Token: 0x0400A29A RID: 41626
			XB1,
			// Token: 0x0400A29B RID: 41627
			Count
		}

		// Token: 0x02001C23 RID: 7203
		public enum ControllerIconStyle
		{
			// Token: 0x0400A29D RID: 41629
			Automatic,
			// Token: 0x0400A29E RID: 41630
			Xbox,
			// Token: 0x0400A29F RID: 41631
			Playstation
		}
	}
}
