using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Platform.XBL
{
	// Token: 0x02001C69 RID: 7273
	public class TextCensor : ITextCensor
	{
		// Token: 0x0600D7B3 RID: 55219 RVA: 0x000027FC File Offset: 0x000009FC
		public void Init(IPlatform _owner)
		{
		}

		// Token: 0x0600D7B4 RID: 55220 RVA: 0x004D59C8 File Offset: 0x004D3BC8
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

		// Token: 0x0600D7B5 RID: 55221 RVA: 0x004D59F2 File Offset: 0x004D3BF2
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

		// Token: 0x0600D7B6 RID: 55222 RVA: 0x004D5A04 File Offset: 0x004D3C04
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

		// Token: 0x0600D7B7 RID: 55223 RVA: 0x004D5AE4 File Offset: 0x004D3CE4
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

		// Token: 0x0400A3D0 RID: 41936
		[PublicizedFrom(EAccessModifier.Private)]
		public const string isAlpNum = "^[a-zA-Z0-9]+$";

		// Token: 0x0400A3D1 RID: 41937
		[PublicizedFrom(EAccessModifier.Private)]
		public const string nonAlphabeticBefore = "(?<![a-zA-Z])";

		// Token: 0x0400A3D2 RID: 41938
		[PublicizedFrom(EAccessModifier.Private)]
		public const string nonAlphabeticAfter = "(?![a-zA-Z])";

		// Token: 0x0400A3D3 RID: 41939
		[PublicizedFrom(EAccessModifier.Private)]
		public bool fetchStarted;

		// Token: 0x0400A3D4 RID: 41940
		[PublicizedFrom(EAccessModifier.Private)]
		public bool fetchComplete;

		// Token: 0x0400A3D5 RID: 41941
		[PublicizedFrom(EAccessModifier.Private)]
		public HashSet<string> bannedPatterns = new HashSet<string>();
	}
}
