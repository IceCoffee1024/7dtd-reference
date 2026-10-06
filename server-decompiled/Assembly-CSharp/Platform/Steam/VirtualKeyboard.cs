using System;
using System.Collections;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001CD7 RID: 7383
	public class VirtualKeyboard : IVirtualKeyboard
	{
		// Token: 0x0600DB08 RID: 56072 RVA: 0x004E5DDB File Offset: 0x004E3FDB
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

		// Token: 0x0600DB09 RID: 56073 RVA: 0x004E5DF4 File Offset: 0x004E3FF4
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

		// Token: 0x0600DB0A RID: 56074 RVA: 0x000027FC File Offset: 0x000009FC
		public void Destroy()
		{
		}

		// Token: 0x0600DB0B RID: 56075 RVA: 0x004E5E84 File Offset: 0x004E4084
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

		// Token: 0x0600DB0C RID: 56076 RVA: 0x004E5F00 File Offset: 0x004E4100
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

		// Token: 0x0400A5D1 RID: 42449
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GamepadTextInputDismissed_t> m_TextInputDismissed;

		// Token: 0x0400A5D2 RID: 42450
		[PublicizedFrom(EAccessModifier.Private)]
		public Action<bool, string> onTextReceived;

		// Token: 0x0400A5D3 RID: 42451
		[PublicizedFrom(EAccessModifier.Private)]
		public string textBefore;
	}
}
