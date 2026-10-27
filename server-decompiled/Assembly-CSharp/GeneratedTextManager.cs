using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Platform;

// Token: 0x02000C73 RID: 3187
public static class GeneratedTextManager
{
	// Token: 0x06006060 RID: 24672 RVA: 0x00265698 File Offset: 0x00263898
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

	// Token: 0x06006061 RID: 24673 RVA: 0x0026576C File Offset: 0x0026396C
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

	// Token: 0x06006062 RID: 24674 RVA: 0x0026599C File Offset: 0x00263B9C
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

	// Token: 0x06006063 RID: 24675 RVA: 0x00265B44 File Offset: 0x00263D44
	public static void PrefilterText(AuthoredText _authoredText, GeneratedTextManager.TextFilteringMode _filteringMode = GeneratedTextManager.TextFilteringMode.Filter)
	{
		GeneratedTextManager.GetDisplayText(_authoredText, null, false, false, _filteringMode, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
	}

	// Token: 0x06006064 RID: 24676 RVA: 0x00265B54 File Offset: 0x00263D54
	public static bool IsFiltered(AuthoredText _authoredText)
	{
		GeneratedTextManager.AuthoredTextDetails authoredTextDetails;
		return _authoredText != null && (PlatformManager.MultiPlatform.TextCensor == null || string.IsNullOrEmpty(_authoredText.Text) || GameManager.IsDedicatedServer || (GeneratedTextManager.authoredTextReferences.TryGetValue(_authoredText, out authoredTextDetails) && authoredTextDetails.IsFiltered()));
	}

	// Token: 0x06006065 RID: 24677 RVA: 0x00265BA0 File Offset: 0x00263DA0
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

	// Token: 0x06006066 RID: 24678 RVA: 0x00265BEC File Offset: 0x00263DEC
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

	// Token: 0x06006067 RID: 24679 RVA: 0x00265C44 File Offset: 0x00263E44
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

	// Token: 0x06006068 RID: 24680 RVA: 0x00265C8C File Offset: 0x00263E8C
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

	// Token: 0x06006069 RID: 24681 RVA: 0x00265DFC File Offset: 0x00263FFC
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

	// Token: 0x0600606A RID: 24682 RVA: 0x00265EF4 File Offset: 0x002640F4
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

	// Token: 0x0600606B RID: 24683 RVA: 0x00265F08 File Offset: 0x00264108
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

	// Token: 0x04004BCE RID: 19406
	public const string SafeString = "{...}";

	// Token: 0x04004BCF RID: 19407
	public const string BlockedString = "";

	// Token: 0x04004BD0 RID: 19408
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObj = new object();

	// Token: 0x04004BD1 RID: 19409
	[PublicizedFrom(EAccessModifier.Private)]
	public static ConditionalWeakTable<AuthoredText, GeneratedTextManager.AuthoredTextDetails> authoredTextReferences = new ConditionalWeakTable<AuthoredText, GeneratedTextManager.AuthoredTextDetails>();

	// Token: 0x04004BD2 RID: 19410
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<AuthoredText, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>> pendingFilterCallbacks = new Dictionary<AuthoredText, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>>();

	// Token: 0x04004BD3 RID: 19411
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>> pendingFilterCallbacksStrings = new Dictionary<string, List<ValueTuple<GeneratedTextManager.BbCodeSupportMode, Action<string>>>>();

	// Token: 0x02000C74 RID: 3188
	public enum TextFilteringMode
	{
		// Token: 0x04004BD5 RID: 19413
		None,
		// Token: 0x04004BD6 RID: 19414
		Filter,
		// Token: 0x04004BD7 RID: 19415
		FilterOtherPlatforms,
		// Token: 0x04004BD8 RID: 19416
		FilterWithSafeString
	}

	// Token: 0x02000C75 RID: 3189
	public enum BbCodeSupportMode
	{
		// Token: 0x04004BDA RID: 19418
		NotSupported,
		// Token: 0x04004BDB RID: 19419
		Supported,
		// Token: 0x04004BDC RID: 19420
		SupportedAndAddEscapes
	}

	// Token: 0x02000C76 RID: 3190
	[PublicizedFrom(EAccessModifier.Private)]
	public class AuthoredTextDetails
	{
		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x0600606D RID: 24685 RVA: 0x00265FCF File Offset: 0x002641CF
		public string BaseText
		{
			get
			{
				return this.baseText;
			}
		}

		// Token: 0x0600606E RID: 24686 RVA: 0x00265FD7 File Offset: 0x002641D7
		public AuthoredTextDetails(string _baseText)
		{
			this.SetText(_baseText);
		}

		// Token: 0x0600606F RID: 24687 RVA: 0x00265FE6 File Offset: 0x002641E6
		public bool IsFiltered()
		{
			return this.filteredTextBase != null;
		}

		// Token: 0x06006070 RID: 24688 RVA: 0x00265FF1 File Offset: 0x002641F1
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

		// Token: 0x06006071 RID: 24689 RVA: 0x00266024 File Offset: 0x00264224
		public void SetFilteredText(string _filteredText)
		{
			this.filteredTextBase = _filteredText;
		}

		// Token: 0x06006072 RID: 24690 RVA: 0x00266030 File Offset: 0x00264230
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

		// Token: 0x04004BDD RID: 19421
		[PublicizedFrom(EAccessModifier.Private)]
		public string baseText;

		// Token: 0x04004BDE RID: 19422
		[PublicizedFrom(EAccessModifier.Private)]
		public string baseTextEscaped;

		// Token: 0x04004BDF RID: 19423
		[PublicizedFrom(EAccessModifier.Private)]
		public string filteredTextBase;

		// Token: 0x04004BE0 RID: 19424
		[PublicizedFrom(EAccessModifier.Private)]
		public string filteredTextBBSupported;

		// Token: 0x04004BE1 RID: 19425
		[PublicizedFrom(EAccessModifier.Private)]
		public string filteredTextBBEscaped;
	}
}
