using System;
using System.Collections;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CA5 RID: 7333
	public class VirtualKeyboard : IVirtualKeyboard
	{
		// Token: 0x0600D98E RID: 55694 RVA: 0x004E1293 File Offset: 0x004DF493
		public void Init(IPlatform _owner)
		{
			_owner.Api.ClientApiInitialized += delegate
			{
				if (GameManager.IsDedicatedServer)
				{
					return;
				}
				if (SteamUtils.IsSteamInBigPictureMode())
				{
					if (this.m_TextInputDismissed == null)
					{
						this.m_TextInputDismissed = Callback<GamepadTextInputDismissed_t>.Create(new Callback<GamepadTextInputDismissed_t>.DispatchDelegate(this.GamePadTextInputDismissed_Callback));
						return;
					}
				}
				else
				{
					Log.Out("Not running in Big Picture Mode, no on-screen keyboard available");
				}
			};
		}

		// Token: 0x0600D98F RID: 55695 RVA: 0x004E12AC File Offset: 0x004DF4AC
		public string Open(string _title, string _defaultText, Action<bool, string> _onTextReceived, UIInput.InputType _mode = UIInput.InputType.Standard, bool _multiLine = false, uint singleLineLength = 200U)
		{
			if (this.onTextReceived != null)
			{
				Log.Warning("The virtual keyboard was already opened and has not closed yet.");
				return null;
			}
			if (_onTextReceived == null)
			{
				throw new ArgumentException("The callback function must not be null");
			}
			this.textBefore = _defaultText;
			this.onTextReceived = _onTextReceived;
			if (SteamUtils.ShowGamepadTextInput((_mode == UIInput.InputType.Password) ? EGamepadTextInputMode.k_EGamepadTextInputModePassword : EGamepadTextInputMode.k_EGamepadTextInputModeNormal, _multiLine ? EGamepadTextInputLineMode.k_EGamepadTextInputLineModeMultipleLines : EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, _title, _multiLine ? 500U : singleLineLength, _defaultText))
			{
				return null;
			}
			Log.Out("Opening OnScreen keyboard failed, probably not running in Steam Big Picture Mode");
			this.onTextReceived(false, _defaultText);
			this.onTextReceived = null;
			return Localization.Get("ttSteamBPM", false, null);
		}

		// Token: 0x0600D990 RID: 55696 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600D991 RID: 55697 RVA: 0x004E133C File Offset: 0x004DF53C
		[PublicizedFrom(EAccessModifier.Private)]
		public void GamePadTextInputDismissed_Callback(GamepadTextInputDismissed_t _result)
		{
			Action<bool, string> action = this.onTextReceived;
			this.onTextReceived = null;
			string text;
			bool enteredGamepadTextInput = SteamUtils.GetEnteredGamepadTextInput(out text, 500U);
			Log.Out("OnScreen keyboard result: ok={0}, submitted={1}, text={2}", new object[] { enteredGamepadTextInput, _result.m_bSubmitted, text });
			if (action != null)
			{
				action(_result.m_bSubmitted, _result.m_bSubmitted ? text : (this.textBefore ?? ""));
			}
		}

		// Token: 0x0600D992 RID: 55698 RVA: 0x004E13B8 File Offset: 0x004DF5B8
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator debugOut()
		{
			int i = 0;
			while (i < 100)
			{
				Log.Out("POST: Enabled={3}, Is={0}, Down={1}, Up={2}", new object[]
				{
					PlatformManager.NativePlatform.Input.PrimaryPlayer.GUIActions.Cancel.IsPressed,
					PlatformManager.NativePlatform.Input.PrimaryPlayer.GUIActions.Cancel.WasPressed,
					PlatformManager.NativePlatform.Input.PrimaryPlayer.GUIActions.Cancel.WasReleased,
					PlatformManager.NativePlatform.Input.PrimaryPlayer.GUIActions.Cancel.Enabled
				});
				int num = i;
				i = num + 1;
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400A4FC RID: 42236
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GamepadTextInputDismissed_t> m_TextInputDismissed;

		// Token: 0x0400A4FD RID: 42237
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<bool, string> onTextReceived;

		// Token: 0x0400A4FE RID: 42238
		[PublicizedFrom(EAccessModifier.Private)]
		public string textBefore;
	}
}
