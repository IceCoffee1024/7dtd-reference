using System;
using System.Collections;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C7E RID: 7294
	public class VirtualKeyboard : IVirtualKeyboard
	{
		// Token: 0x0600D7F8 RID: 55288 RVA: 0x004DACB7 File Offset: 0x004D8EB7
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

		// Token: 0x0600D7F9 RID: 55289 RVA: 0x004DACD0 File Offset: 0x004D8ED0
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

		// Token: 0x0600D7FA RID: 55290 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600D7FB RID: 55291 RVA: 0x004DAD60 File Offset: 0x004D8F60
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

		// Token: 0x0600D7FC RID: 55292 RVA: 0x004DADDC File Offset: 0x004D8FDC
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

		// Token: 0x0400A3C9 RID: 41929
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GamepadTextInputDismissed_t> m_TextInputDismissed;

		// Token: 0x0400A3CA RID: 41930
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<bool, string> onTextReceived;

		// Token: 0x0400A3CB RID: 41931
		[PublicizedFrom(EAccessModifier.Private)]
		public string textBefore;
	}
}
