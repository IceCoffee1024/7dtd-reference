using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Platform.XBL
{
	// Token: 0x02001C38 RID: 7224
	public class TextCensor : ITextCensor
	{
		// Token: 0x0600D69F RID: 54943 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D6A0 RID: 54944 RVA: 0x004D1EA0 File Offset: 0x004D00A0
		public void Update()
		{
			if (this.fetchStarted)
			{
				return;
			}
			this.fetchStarted = true;
			if (PlatformManager.MultiPlatform.RemoteFileStorage != null)
			{
				ThreadManager.StartCoroutine(this.RetrieveBannedWords());
			}
		}

		// Token: 0x0600D6A1 RID: 54945 RVA: 0x004D1ECA File Offset: 0x004D00CA
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator RetrieveBannedWords()
		{
			IRemoteFileStorage storage = PlatformManager.MultiPlatform.RemoteFileStorage;
			if (storage == null)
			{
				yield break;
			}
			while (!storage.IsReady)
			{
				yield return null;
			}
			storage.GetFile("BannedWordsXBL.txt", new IRemoteFileStorage.FileDownloadCompleteCallback(this.StorageProviderCallback));
			yield break;
		}

		// Token: 0x0600D6A2 RID: 54946 RVA: 0x004D1EDC File Offset: 0x004D00DC
		[PublicizedFrom(EAccessModifier.Private)]
		public void StorageProviderCallback(IRemoteFileStorage.EFileDownloadResult _result, string _errorDetails, byte[] _data)
		{
			if (_result != IRemoteFileStorage.EFileDownloadResult.Ok)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Retrieving banned words list failed: ",
					_result.ToStringCached<IRemoteFileStorage.EFileDownloadResult>(),
					" (",
					_errorDetails,
					")"
				}));
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream(_data))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8))
				{
					string text;
					while ((text = streamReader.ReadLine()) != null)
					{
						if (Regex.IsMatch(text, "^[a-zA-Z0-9]+$"))
						{
							this.bannedPatterns.Add("(?<![a-zA-Z])" + Regex.Escape(text) + "(?![a-zA-Z])");
						}
						else
						{
							this.bannedPatterns.Add(Regex.Escape(text));
						}
					}
				}
			}
			this.fetchComplete = true;
		}

		// Token: 0x0600D6A3 RID: 54947 RVA: 0x004D1FBC File Offset: 0x004D01BC
		public void CensorProfanity(string _input, PlatformUserIdentifierAbs _author, Action<CensoredTextResult> _callback)
		{
			if (string.IsNullOrEmpty(_input) || _input.Length == 0 || !this.fetchComplete)
			{
				_callback(new CensoredTextResult(true, _input, _input));
				return;
			}
			if (_author != null)
			{
				IPlatform crossplatformPlatform = PlatformManager.CrossplatformPlatform;
				if (((crossplatformPlatform != null) ? crossplatformPlatform.User : null) != null && PlatformManager.CrossplatformPlatform.User.PlatformUserId.Equals(_author))
				{
					_callback(new CensoredTextResult(true, _input, _input));
					return;
				}
				IPlatform nativePlatform = PlatformManager.NativePlatform;
				if (((nativePlatform != null) ? nativePlatform.User : null) != null && (PlatformManager.NativePlatform.User.PlatformUserId.Equals(_author) || PlatformManager.NativePlatform.User.IsFriend(_author)))
				{
					_callback(new CensoredTextResult(true, _input, _input));
					return;
				}
			}
			Task.Run(delegate
			{
				char[] array = _input.ToCharArray();
				foreach (string text in this.bannedPatterns)
				{
					foreach (object obj in Regex.Matches(_input, text, RegexOptions.IgnoreCase))
					{
						Match match = (Match)obj;
						for (int i = match.Index; i < match.Index + match.Length; i++)
						{
							array[i] = '*';
						}
					}
				}
				_callback(new CensoredTextResult(true, _input, new string(array)));
			});
		}

		// Token: 0x0400A2E7 RID: 41703
		[PublicizedFrom(EAccessModifier.Private)]
		public const string isAlpNum = "^[a-zA-Z0-9]+$";

		// Token: 0x0400A2E8 RID: 41704
		[PublicizedFrom(EAccessModifier.Private)]
		public const string nonAlphabeticBefore = "(?<![a-zA-Z])";

		// Token: 0x0400A2E9 RID: 41705
		[PublicizedFrom(EAccessModifier.Private)]
		public const string nonAlphabeticAfter = "(?![a-zA-Z])";

		// Token: 0x0400A2EA RID: 41706
		[PublicizedFrom(EAccessModifier.Private)]
		public bool fetchStarted;

		// Token: 0x0400A2EB RID: 41707
		[PublicizedFrom(EAccessModifier.Private)]
		public bool fetchComplete;

		// Token: 0x0400A2EC RID: 41708
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<string> bannedPatterns = new HashSet<string>();
	}
}
