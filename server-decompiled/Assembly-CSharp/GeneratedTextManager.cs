using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Platform;

// Token: 0x02000C68 RID: 3176
public static class GeneratedTextManager
{
	// Token: 0x0600603B RID: 24635 RVA: 0x00264170 File Offset: 0x00262370
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

	// Token: 0x0600603C RID: 24636 RVA: 0x00264244 File Offset: 0x00262444
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

	// Token: 0x0600603D RID: 24637 RVA: 0x00264474 File Offset: 0x00262674
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

	// Token: 0x0600603E RID: 24638 RVA: 0x0026461C File Offset: 0x0026281C
	public static void PrefilterText(AuthoredText _authoredText, GeneratedTextManager.TextFilteringMode _filteringMode = GeneratedTextManager.TextFilteringMode.Filter)
	{
		GeneratedTextManager.GetDisplayText(_authoredText, null, false, false, _filteringMode, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
	}

	// Token: 0x0600603F RID: 24639 RVA: 0x0026462C File Offset: 0x0026282C
	public static bool IsFiltered(AuthoredText _authoredText)
	{
		GeneratedTextManager.AuthoredTextDetails authoredTextDetails;
		return _authoredText != null && (PlatformManager.MultiPlatform.TextCensor == null || string.IsNullOrEmpty(_authoredText.Text) || GameManager.IsDedicatedServer || (GeneratedTextManager.authoredTextReferences.TryGetValue(_authoredText, out authoredTextDetails) && authoredTextDetails.IsFiltered()));
	}

	// Token: 0x06006040 RID: 24640 RVA: 0x00264678 File Offset: 0x00262878
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

	// Token: 0x06006041 RID: 24641 RVA: 0x002646C4 File Offset: 0x002628C4
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

	// Token: 0x06006042 RID: 24642 RVA: 0x0026471C File Offset: 0x0026291C
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

	// Token: 0x06006043 RID: 24643 RVA: 0x00264764 File Offset: 0x00262964
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

	// Token: 0x06006044 RID: 24644 RVA: 0x002648D4 File Offset: 0x00262AD4
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

	// Token: 0x06006045 RID: 24645 RVA: 0x002649CC File Offset: 0x00262BCC
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

	// Token: 0x06006046 RID: 24646 RVA: 0x002649E0 File Offset: 0x00262BE0
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

	// Token: 0x04004BA9 RID: 19369
	public const string SafeString = "{...}";

	// Token: 0x04004BAA RID: 19370
	public const string BlockedString = "";

	// Token: 0x04004BAB RID: 19371
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObj = new object();

	// Token: 0x04004BAC RID: 19372
	[PublicizedFrom(EAccessModifier.Private)]
	public static ConditionalWeakTable<AuthoredText, GeneratedTextManager.AuthoredTextDetails> authoredTextReferences = new ConditionalWeakTable<AuthoredText, GeneratedTextManager.AuthoredTextDetails>();

	// Token: 0x04004BAD RID: 19373
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<AuthoredText, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>> pendingFilterCallbacks = new Dictionary<AuthoredText, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>>();

	// Token: 0x04004BAE RID: 19374
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>> pendingFilterCallbacksStrings = new Dictionary<string, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>>();

	// Token: 0x02000C69 RID: 3177
	public enum TextFilteringMode
	{
		// Token: 0x04004BB0 RID: 19376
		None,
		// Token: 0x04004BB1 RID: 19377
		Filter,
		// Token: 0x04004BB2 RID: 19378
		FilterOtherPlatforms,
		// Token: 0x04004BB3 RID: 19379
		FilterWithSafeString
	}

	// Token: 0x02000C6A RID: 3178
	public enum BbCodeSupportMode
	{
		// Token: 0x04004BB5 RID: 19381
		NotSupported,
		// Token: 0x04004BB6 RID: 19382
		Supported,
		// Token: 0x04004BB7 RID: 19383
		SupportedAndAddEscapes
	}

	// Token: 0x02000C6B RID: 3179
	[PublicizedFrom(EAccessModifier.Private)]
	public class AuthoredTextDetails
	{
		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06006048 RID: 24648 RVA: 0x00264AA7 File Offset: 0x00262CA7
		public string BaseText
		{
			get
			{
				return this.baseText;
			}
		}

		// Token: 0x06006049 RID: 24649 RVA: 0x00264AAF File Offset: 0x00262CAF
		public AuthoredTextDetails(string _baseText)
		{
			this.SetText(_baseText);
		}

		// Token: 0x0600604A RID: 24650 RVA: 0x00264ABE File Offset: 0x00262CBE
		public bool IsFiltered()
		{
			return this.filteredTextBase != null;
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x00264AC9 File Offset: 0x00262CC9
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

		// Token: 0x0600604C RID: 24652 RVA: 0x00264AFC File Offset: 0x00262CFC
		public void SetFilteredText(string _filteredText)
		{
			this.filteredTextBase = _filteredText;
		}

		// Token: 0x0600604D RID: 24653 RVA: 0x00264B08 File Offset: 0x00262D08
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

		// Token: 0x04004BB8 RID: 19384
		[PublicizedFrom(EAccessModifier.Private)]
		public string baseText;

		// Token: 0x04004BB9 RID: 19385
		[PublicizedFrom(EAccessModifier.Private)]
		public string baseTextEscaped;

		// Token: 0x04004BBA RID: 19386
		[PublicizedFrom(EAccessModifier.Private)]
		public string filteredTextBase;

		// Token: 0x04004BBB RID: 19387
		[PublicizedFrom(EAccessModifier.Private)]
		public string filteredTextBBSupported;

		// Token: 0x04004BBC RID: 19388
		[PublicizedFrom(EAccessModifier.Private)]
		public string filteredTextBBEscaped;
	}
}
