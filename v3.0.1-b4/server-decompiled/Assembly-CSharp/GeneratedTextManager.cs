using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Platform;

// Token: 0x02000C44 RID: 3140
public static class GeneratedTextManager
{
	// Token: 0x06005F98 RID: 24472 RVA: 0x0025DFD4 File Offset: 0x0025C1D4
	public static string GetDisplayTextImmediately(AuthoredText _authoredText, bool _checkBlockState, GeneratedTextManager.TextFilteringMode _filteringMode = GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode _bbSupportMode = GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes)
	{
		GeneratedTextManager.AuthoredTextDetails orCreateFilterDetails = GeneratedTextManager.GetOrCreateFilterDetails(_authoredText);
		if (string.IsNullOrEmpty((orCreateFilterDetails != null) ? orCreateFilterDetails.BaseText : null) || GameManager.IsDedicatedServer)
		{
			if (orCreateFilterDetails == null)
			{
				return null;
			}
			return orCreateFilterDetails.GetDisplayText(false, _bbSupportMode);
		}
		else
		{
			if (_checkBlockState && _authoredText.Author != null && !PlatformManager.MultiPlatform.User.PlatformUserId.Equals(_authoredText.Author))
			{
				PersistentPlayerData playerData = GameManager.Instance.persistentPlayers.GetPlayerData(_authoredText.Author);
				if (playerData != null && playerData.PlatformData.Blocked[EBlockType.TextChat].IsBlocked())
				{
					return "";
				}
			}
			if (PlatformManager.MultiPlatform.TextCensor == null || GeneratedTextManager.ShouldSkipFiltering(_authoredText.Author, _filteringMode))
			{
				return orCreateFilterDetails.GetDisplayText(false, _bbSupportMode);
			}
			if (orCreateFilterDetails.IsFiltered())
			{
				return orCreateFilterDetails.GetDisplayText(true, _bbSupportMode);
			}
			return "{...}";
		}
	}

	// Token: 0x06005F99 RID: 24473 RVA: 0x0025E0A8 File Offset: 0x0025C2A8
	public static void GetDisplayText(AuthoredText _authoredText, Action<string> _textReadyCallback, bool _runCallbackIfReadyNow, bool _checkBlockState, GeneratedTextManager.TextFilteringMode _filteringMode = GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode _bbSupportMode = GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes)
	{
		GeneratedTextManager.AuthoredTextDetails orCreateFilterDetails = GeneratedTextManager.GetOrCreateFilterDetails(_authoredText);
		if (string.IsNullOrEmpty((orCreateFilterDetails != null) ? orCreateFilterDetails.BaseText : null) || GameManager.IsDedicatedServer)
		{
			if (_runCallbackIfReadyNow && _textReadyCallback != null)
			{
				_textReadyCallback((orCreateFilterDetails != null) ? orCreateFilterDetails.GetDisplayText(false, _bbSupportMode) : null);
			}
			return;
		}
		if (_checkBlockState && _authoredText.Author != null && !PlatformManager.MultiPlatform.User.PlatformUserId.Equals(_authoredText.Author))
		{
			PersistentPlayerData playerData = GameManager.Instance.persistentPlayers.GetPlayerData(_authoredText.Author);
			if (playerData != null && playerData.PlatformData.Blocked[EBlockType.TextChat].IsBlocked())
			{
				if (_runCallbackIfReadyNow && _textReadyCallback != null)
				{
					_textReadyCallback("");
				}
				return;
			}
		}
		if (PlatformManager.MultiPlatform.TextCensor == null || GeneratedTextManager.ShouldSkipFiltering(_authoredText.Author, _filteringMode))
		{
			if (_runCallbackIfReadyNow && _textReadyCallback != null)
			{
				_textReadyCallback(orCreateFilterDetails.GetDisplayText(false, _bbSupportMode));
			}
			return;
		}
		if (orCreateFilterDetails.IsFiltered())
		{
			if (_runCallbackIfReadyNow && _textReadyCallback != null)
			{
				_textReadyCallback(orCreateFilterDetails.GetDisplayText(true, _bbSupportMode));
			}
			return;
		}
		if (_filteringMode == GeneratedTextManager.TextFilteringMode.FilterWithSafeString && _textReadyCallback != null)
		{
			_textReadyCallback("{...}");
		}
		object obj = GeneratedTextManager.lockObj;
		bool flag2;
		lock (obj)
		{
			flag2 = GeneratedTextManager.pendingFilterCallbacks.ContainsKey(_authoredText);
			if (!flag2)
			{
				GeneratedTextManager.pendingFilterCallbacks.Add(_authoredText, null);
			}
			if (_textReadyCallback != null)
			{
				if (GeneratedTextManager.pendingFilterCallbacks[_authoredText] == null)
				{
					GeneratedTextManager.pendingFilterCallbacks[_authoredText] = new List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>();
				}
				GeneratedTextManager.pendingFilterCallbacks[_authoredText].Add(new ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>(_bbSupportMode, _textReadyCallback));
			}
		}
		if (!flag2)
		{
			string textToFilter = GeneratedTextManager.GetTextToFilter(orCreateFilterDetails.BaseText, _bbSupportMode);
			PlatformManager.MultiPlatform.TextCensor.CensorProfanity(textToFilter, _authoredText.Author, delegate(CensoredTextResult _censorResult)
			{
				GeneratedTextManager.FilterTextCallback(_authoredText, _censorResult, _bbSupportMode);
			});
		}
	}

	// Token: 0x06005F9A RID: 24474 RVA: 0x0025E2D8 File Offset: 0x0025C4D8
	public static void GetDisplayText(string _text, PlatformUserIdentifierAbs _author, Action<string> _textReadyCallback, bool _checkBlockState, GeneratedTextManager.TextFilteringMode _filteringMode = GeneratedTextManager.TextFilteringMode.Filter, GeneratedTextManager.BbCodeSupportMode _bbSupportMode = GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes)
	{
		if (_textReadyCallback == null)
		{
			Log.Warning("Could not get display text \"" + _text + "\", no callback action provided");
		}
		if (string.IsNullOrEmpty(_text) || GameManager.IsDedicatedServer)
		{
			_textReadyCallback((_bbSupportMode == GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes) ? Utils.EscapeBbCodes(_text, false, false) : _text);
			return;
		}
		if (_checkBlockState && _author != null && !PlatformManager.MultiPlatform.User.PlatformUserId.Equals(_author))
		{
			PersistentPlayerData playerData = GameManager.Instance.persistentPlayers.GetPlayerData(_author);
			if (playerData != null && playerData.PlatformData.Blocked[EBlockType.TextChat].IsBlocked())
			{
				if (_textReadyCallback != null)
				{
					_textReadyCallback("");
				}
				return;
			}
		}
		if (PlatformManager.MultiPlatform.TextCensor == null || GeneratedTextManager.ShouldSkipFiltering(_author, _filteringMode))
		{
			_textReadyCallback((_bbSupportMode == GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes) ? Utils.EscapeBbCodes(_text, false, false) : _text);
			return;
		}
		if (_filteringMode == GeneratedTextManager.TextFilteringMode.FilterWithSafeString)
		{
			_textReadyCallback("{...}");
		}
		object obj = GeneratedTextManager.lockObj;
		lock (obj)
		{
			if (!GeneratedTextManager.pendingFilterCallbacksStrings.ContainsKey(_text))
			{
				GeneratedTextManager.pendingFilterCallbacksStrings.Add(_text, new List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>());
			}
			GeneratedTextManager.pendingFilterCallbacksStrings[_text].Add(new ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>(_bbSupportMode, _textReadyCallback));
		}
		string textToFilter = GeneratedTextManager.GetTextToFilter(_text, _bbSupportMode);
		PlatformManager.MultiPlatform.TextCensor.CensorProfanity(textToFilter, _author, delegate(CensoredTextResult _censorResult)
		{
			GeneratedTextManager.FilterTextCallbackStrings(_text, _censorResult);
		});
	}

	// Token: 0x06005F9B RID: 24475 RVA: 0x0025E480 File Offset: 0x0025C680
	public static void PrefilterText(AuthoredText _authoredText, GeneratedTextManager.TextFilteringMode _filteringMode = GeneratedTextManager.TextFilteringMode.Filter)
	{
		GeneratedTextManager.GetDisplayText(_authoredText, null, false, false, _filteringMode, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
	}

	// Token: 0x06005F9C RID: 24476 RVA: 0x0025E490 File Offset: 0x0025C690
	public static bool IsFiltered(AuthoredText _authoredText)
	{
		GeneratedTextManager.AuthoredTextDetails authoredTextDetails;
		return _authoredText != null && (PlatformManager.MultiPlatform.TextCensor == null || string.IsNullOrEmpty(_authoredText.Text) || GameManager.IsDedicatedServer || (GeneratedTextManager.authoredTextReferences.TryGetValue(_authoredText, out authoredTextDetails) && authoredTextDetails.IsFiltered()));
	}

	// Token: 0x06005F9D RID: 24477 RVA: 0x0025E4DC File Offset: 0x0025C6DC
	public static bool IsFiltering(AuthoredText _authoredText)
	{
		object obj = GeneratedTextManager.lockObj;
		bool flag2;
		lock (obj)
		{
			flag2 = _authoredText != null && GeneratedTextManager.pendingFilterCallbacks.ContainsKey(_authoredText);
		}
		return flag2;
	}

	// Token: 0x06005F9E RID: 24478 RVA: 0x0025E528 File Offset: 0x0025C728
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool ShouldSkipFiltering(PlatformUserIdentifierAbs _author, GeneratedTextManager.TextFilteringMode _mode)
	{
		switch (_mode)
		{
		case GeneratedTextManager.TextFilteringMode.None:
			return true;
		case GeneratedTextManager.TextFilteringMode.Filter:
		case GeneratedTextManager.TextFilteringMode.FilterWithSafeString:
			return false;
		case GeneratedTextManager.TextFilteringMode.FilterOtherPlatforms:
		{
			EPlatformIdentifier eplatformIdentifier;
			return PlatformUserManager.TryGetNativePlatform(_author, out eplatformIdentifier) && eplatformIdentifier == PlatformManager.NativePlatform.PlatformIdentifier;
		}
		default:
			throw new NotImplementedException(string.Format("Cannot determine if filtering should be skipped for filtering mode {0}", _mode));
		}
	}

	// Token: 0x06005F9F RID: 24479 RVA: 0x0025E580 File Offset: 0x0025C780
	[PublicizedFrom(EAccessModifier.Private)]
	public static GeneratedTextManager.AuthoredTextDetails GetOrCreateFilterDetails(AuthoredText _authoredText)
	{
		if (_authoredText == null)
		{
			return null;
		}
		GeneratedTextManager.AuthoredTextDetails authoredTextDetails;
		if (GeneratedTextManager.authoredTextReferences.TryGetValue(_authoredText, out authoredTextDetails))
		{
			authoredTextDetails.SetText(_authoredText.Text);
			return authoredTextDetails;
		}
		authoredTextDetails = new GeneratedTextManager.AuthoredTextDetails(_authoredText.Text);
		GeneratedTextManager.authoredTextReferences.Add(_authoredText, authoredTextDetails);
		return authoredTextDetails;
	}

	// Token: 0x06005FA0 RID: 24480 RVA: 0x0025E5C8 File Offset: 0x0025C7C8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void FilterTextCallback(AuthoredText _authoredText, CensoredTextResult _censorResult, GeneratedTextManager.BbCodeSupportMode _originalBBSupport)
	{
		object obj = GeneratedTextManager.lockObj;
		List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>> list;
		lock (obj)
		{
			if (!GeneratedTextManager.pendingFilterCallbacks.TryGetValue(_authoredText, out list))
			{
				Log.Error("Invalid callback information during text filtering.");
				return;
			}
		}
		GeneratedTextManager.AuthoredTextDetails authoredTextDetails;
		if (!GeneratedTextManager.authoredTextReferences.TryGetValue(_authoredText, out authoredTextDetails))
		{
			Log.Error("Authored Text filter details not found.");
			return;
		}
		if (GeneratedTextManager.GetTextToFilter(authoredTextDetails.BaseText, _originalBBSupport) != _censorResult.OriginalText)
		{
			Log.Warning("Text has changed during filtering process, displayed texts may be outdated.");
		}
		if (_censorResult.Success)
		{
			authoredTextDetails.SetFilteredText(_censorResult.CensoredText);
		}
		else if (_authoredText.Author.Equals(PlatformManager.MultiPlatform.User.PlatformUserId))
		{
			authoredTextDetails.SetFilteredText(authoredTextDetails.BaseText);
		}
		else
		{
			authoredTextDetails.SetFilteredText("{...}");
		}
		obj = GeneratedTextManager.lockObj;
		lock (obj)
		{
			GeneratedTextManager.pendingFilterCallbacks.Remove(_authoredText);
		}
		if (list != null)
		{
			foreach (ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>> valueTuple in list)
			{
				GeneratedTextManager.BbCodeSupportMode item = valueTuple.Item1;
				Action<string> item2 = valueTuple.Item2;
				if (item2 != null)
				{
					item2(authoredTextDetails.GetDisplayText(true, item));
				}
			}
		}
	}

	// Token: 0x06005FA1 RID: 24481 RVA: 0x0025E738 File Offset: 0x0025C938
	[PublicizedFrom(EAccessModifier.Private)]
	public static void FilterTextCallbackStrings(string _text, CensoredTextResult _censorResult)
	{
		object obj = GeneratedTextManager.lockObj;
		List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>> list;
		lock (obj)
		{
			if (!GeneratedTextManager.pendingFilterCallbacksStrings.TryGetValue(_text, out list))
			{
				Log.Error("Invalid callback information during text filtering.");
				return;
			}
			GeneratedTextManager.pendingFilterCallbacksStrings.Remove(_text);
		}
		if (list != null)
		{
			foreach (ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>> valueTuple in list)
			{
				GeneratedTextManager.BbCodeSupportMode item = valueTuple.Item1;
				Action<string> item2 = valueTuple.Item2;
				string text;
				if (item != GeneratedTextManager.BbCodeSupportMode.Supported)
				{
					if (item != GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes)
					{
						text = _censorResult.CensoredText;
					}
					else
					{
						text = Utils.EscapeBbCodes(_censorResult.CensoredText, false, false);
					}
				}
				else
				{
					text = GeneratedTextManager.ReconstructFilteredTextWithBbCodes(_text, _censorResult.CensoredText);
				}
				string text2 = text;
				if (item2 != null)
				{
					item2(text2);
				}
			}
		}
	}

	// Token: 0x06005FA2 RID: 24482 RVA: 0x0025E830 File Offset: 0x0025CA30
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetTextToFilter(string _baseText, GeneratedTextManager.BbCodeSupportMode _bbSupport)
	{
		if (_baseText == null)
		{
			return null;
		}
		if (_bbSupport != GeneratedTextManager.BbCodeSupportMode.Supported)
		{
			return _baseText;
		}
		return Utils.GetVisibileTextWithBbCodes(_baseText);
	}

	// Token: 0x06005FA3 RID: 24483 RVA: 0x0025E844 File Offset: 0x0025CA44
	[PublicizedFrom(EAccessModifier.Private)]
	public static string ReconstructFilteredTextWithBbCodes(string originalText, string filteredText)
	{
		int num = 0;
		int num2 = 0;
		StringBuilder stringBuilder = new StringBuilder();
		while (num < originalText.Length && num2 < filteredText.Length)
		{
			ValueTuple<int, int, bool> valueTuple = Utils.FindNextBbCode(originalText, num, false);
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			bool item3 = valueTuple.Item3;
			if (item == -1)
			{
				break;
			}
			int num3 = item - num;
			stringBuilder.Append(filteredText, num2, num3);
			num2 += num3;
			stringBuilder.Append(originalText, item, item2);
			num = item + item2;
			if (item3)
			{
				num2 += item2 - 4;
			}
		}
		if (num2 < filteredText.Length)
		{
			stringBuilder.Append(filteredText, num2, filteredText.Length - num2);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x04004ACD RID: 19149
	public const string SafeString = "{...}";

	// Token: 0x04004ACE RID: 19150
	public const string BlockedString = "";

	// Token: 0x04004ACF RID: 19151
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObj = new object();

	// Token: 0x04004AD0 RID: 19152
	[PublicizedFrom(EAccessModifier.Private)]
	public static ConditionalWeakTable<AuthoredText, GeneratedTextManager.AuthoredTextDetails> authoredTextReferences = new ConditionalWeakTable<AuthoredText, GeneratedTextManager.AuthoredTextDetails>();

	// Token: 0x04004AD1 RID: 19153
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<AuthoredText, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>> pendingFilterCallbacks = new Dictionary<AuthoredText, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>>();

	// Token: 0x04004AD2 RID: 19154
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>> pendingFilterCallbacksStrings = new Dictionary<string, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>>();

	// Token: 0x02000C45 RID: 3141
	public enum TextFilteringMode
	{
		// Token: 0x04004AD4 RID: 19156
		None,
		// Token: 0x04004AD5 RID: 19157
		Filter,
		// Token: 0x04004AD6 RID: 19158
		FilterOtherPlatforms,
		// Token: 0x04004AD7 RID: 19159
		FilterWithSafeString
	}

	// Token: 0x02000C46 RID: 3142
	public enum BbCodeSupportMode
	{
		// Token: 0x04004AD9 RID: 19161
		NotSupported,
		// Token: 0x04004ADA RID: 19162
		Supported,
		// Token: 0x04004ADB RID: 19163
		SupportedAndAddEscapes
	}

	// Token: 0x02000C47 RID: 3143
	[PublicizedFrom(EAccessModifier.Private)]
	public class AuthoredTextDetails
	{
		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x06005FA5 RID: 24485 RVA: 0x0025E90B File Offset: 0x0025CB0B
		public string BaseText
		{
			get
			{
				return this.baseText;
			}
		}

		// Token: 0x06005FA6 RID: 24486 RVA: 0x0025E913 File Offset: 0x0025CB13
		public AuthoredTextDetails(string _baseText)
		{
			this.SetText(_baseText);
		}

		// Token: 0x06005FA7 RID: 24487 RVA: 0x0025E922 File Offset: 0x0025CB22
		public bool IsFiltered()
		{
			return this.filteredTextBase != null;
		}

		// Token: 0x06005FA8 RID: 24488 RVA: 0x0025E92D File Offset: 0x0025CB2D
		public void SetText(string _baseText)
		{
			if (_baseText != this.baseText)
			{
				this.baseText = _baseText;
				this.baseTextEscaped = null;
				this.filteredTextBase = null;
				this.filteredTextBBSupported = null;
				this.filteredTextBBEscaped = null;
			}
		}

		// Token: 0x06005FA9 RID: 24489 RVA: 0x0025E960 File Offset: 0x0025CB60
		public void SetFilteredText(string _filteredText)
		{
			this.filteredTextBase = _filteredText;
		}

		// Token: 0x06005FAA RID: 24490 RVA: 0x0025E96C File Offset: 0x0025CB6C
		public string GetDisplayText(bool _filtered, GeneratedTextManager.BbCodeSupportMode _bbSupportMode)
		{
			if (_filtered)
			{
				switch (_bbSupportMode)
				{
				case GeneratedTextManager.BbCodeSupportMode.NotSupported:
					return this.filteredTextBase;
				case GeneratedTextManager.BbCodeSupportMode.Supported:
					if (this.filteredTextBBSupported == null && this.filteredTextBase != null)
					{
						this.filteredTextBBSupported = GeneratedTextManager.ReconstructFilteredTextWithBbCodes(this.baseText, this.filteredTextBase);
					}
					return this.filteredTextBBSupported;
				case GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes:
					if (this.filteredTextBBEscaped == null && this.filteredTextBase != null)
					{
						this.filteredTextBBEscaped = Utils.EscapeBbCodes(this.filteredTextBase, false, false);
					}
					return this.filteredTextBBEscaped;
				default:
					return null;
				}
			}
			else
			{
				if (_bbSupportMode <= GeneratedTextManager.BbCodeSupportMode.Supported)
				{
					return this.baseText;
				}
				if (_bbSupportMode != GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes)
				{
					return this.baseText;
				}
				if (this.baseTextEscaped == null && this.baseText != null)
				{
					this.baseTextEscaped = Utils.EscapeBbCodes(this.baseText, false, false);
				}
				return this.baseTextEscaped;
			}
		}

		// Token: 0x04004ADC RID: 19164
		[PublicizedFrom(EAccessModifier.Private)]
		public string baseText;

		// Token: 0x04004ADD RID: 19165
		[PublicizedFrom(EAccessModifier.Private)]
		public string baseTextEscaped;

		// Token: 0x04004ADE RID: 19166
		[PublicizedFrom(EAccessModifier.Private)]
		public string filteredTextBase;

		// Token: 0x04004ADF RID: 19167
		[PublicizedFrom(EAccessModifier.Private)]
		public string filteredTextBBSupported;

		// Token: 0x04004AE0 RID: 19168
		[PublicizedFrom(EAccessModifier.Private)]
		public string filteredTextBBEscaped;
	}
}
