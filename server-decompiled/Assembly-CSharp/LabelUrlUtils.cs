using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

// Token: 0x020010EB RID: 4331
public static class LabelUrlUtils
{
	// Token: 0x0600894F RID: 35151 RVA: 0x0034612F File Offset: 0x0034432F
	public static void RegisterLabelUrlHandler(string _type, LabelUrlUtils.HandleTextClickUrlDelegate _clickHandler, LabelUrlUtils.HandleTextHoverUrlDelegate _hoverHandler)
	{
		if (LabelUrlUtils.urlHandlers.ContainsKey(_type))
		{
			Log.Warning("Registering a new label URL handler for the type '" + _type + "'");
		}
		LabelUrlUtils.urlHandlers[_type] = new LabelUrlUtils.SHandlerData(_clickHandler, _hoverHandler);
	}

	// Token: 0x06008950 RID: 35152 RVA: 0x00346168 File Offset: 0x00344368
	public static void HandleLabelUrlClick(XUiView _view, UILabel _label, HashSet<string> _allowedTypes = null)
	{
		LabelUrlUtils.SHandlerData shandlerData;
		string text;
		Dictionary<string, string> dictionary;
		if (LabelUrlUtils.tryGetDataForLabelPos(_view, _label, _allowedTypes, out shandlerData, out text, out dictionary))
		{
			LabelUrlUtils.HandleTextClickUrlDelegate clickDelegate = shandlerData.ClickDelegate;
			if (clickDelegate == null)
			{
				return;
			}
			clickDelegate(_view, text, dictionary);
		}
	}

	// Token: 0x06008951 RID: 35153 RVA: 0x00346198 File Offset: 0x00344398
	public static void HandleLabelUrlHover(XUiView _view, UILabel _label, HashSet<string> _allowedTypes, out bool _isOverUrl, out string _tooltipText)
	{
		LabelUrlUtils.SHandlerData shandlerData;
		string text;
		Dictionary<string, string> dictionary;
		if (LabelUrlUtils.tryGetDataForLabelPos(_view, _label, _allowedTypes, out shandlerData, out text, out dictionary) && shandlerData.HoverDelegate != null)
		{
			_isOverUrl = true;
			shandlerData.HoverDelegate(_view, text, dictionary, out _tooltipText);
			return;
		}
		_isOverUrl = false;
		_tooltipText = null;
	}

	// Token: 0x06008952 RID: 35154 RVA: 0x003461D8 File Offset: 0x003443D8
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool tryGetDataForLabelPos(XUiView _view, UILabel _label, HashSet<string> _allowedTypes, out LabelUrlUtils.SHandlerData _handler, out string _fullUrlString, out Dictionary<string, string> _urlElements)
	{
		_handler = default(LabelUrlUtils.SHandlerData);
		_urlElements = null;
		_fullUrlString = _label.GetUrlAtPosition(UICamera.lastWorldPosition);
		if (string.IsNullOrEmpty(_fullUrlString))
		{
			return false;
		}
		if (_fullUrlString.StartsWith("http://") || _fullUrlString.StartsWith("https://"))
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["Type"] = "HTTP";
			dictionary["Target"] = _fullUrlString;
			_urlElements = dictionary;
		}
		else
		{
			MatchCollection matchCollection = LabelUrlUtils.urlMatcher.Matches(_fullUrlString);
			if (matchCollection.Count == 0)
			{
				Log.Warning("Text URL ('" + _fullUrlString + "'): Invalid URL");
				return false;
			}
			_urlElements = new Dictionary<string, string>();
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				_urlElements[match.Groups[1].Value] = match.Groups[2].Value;
			}
		}
		string text;
		if (!_urlElements.TryGetValue("Type", out text))
		{
			Log.Warning(string.Concat(new string[] { "Text URL ('", _fullUrlString, "'): No explicit or implicit type defined in '", _fullUrlString, "'" }));
			return false;
		}
		if (!LabelUrlUtils.urlHandlers.TryGetValue(text, out _handler))
		{
			Log.Warning(string.Concat(new string[] { "Text URL ('", _fullUrlString, "'): No handler for type '", text, "'" }));
			return false;
		}
		if (_allowedTypes != null && !_allowedTypes.Contains(text))
		{
			Log.Warning(string.Concat(new string[] { "Text URL ('", _fullUrlString, "'): URL type '", text, "' not allowed on this label" }));
			return false;
		}
		return true;
	}

	// Token: 0x06008953 RID: 35155 RVA: 0x003463C4 File Offset: 0x003445C4
	public static string BuildUrlFunctionString(string _type)
	{
		return "[url=Type>" + _type + "]";
	}

	// Token: 0x06008954 RID: 35156 RVA: 0x003463D8 File Offset: 0x003445D8
	public static string BuildUrlFunctionString(string _type, [TupleElementNames(new string[] { "key", "value" })] ValueTuple<string, string> _param1)
	{
		return string.Concat(new string[] { "[url=Type>", _type, "|", _param1.Item1, ">", _param1.Item2, "]" });
	}

	// Token: 0x06008955 RID: 35157 RVA: 0x00346428 File Offset: 0x00344628
	public static string BuildUrlFunctionString(string _type, [TupleElementNames(new string[] { "key", "value" })] ValueTuple<string, string> _param1, [TupleElementNames(new string[] { "key", "value" })] ValueTuple<string, string> _param2)
	{
		return string.Concat(new string[]
		{
			"[url=Type>", _type, "|", _param1.Item1, ">", _param1.Item2, "|", _param2.Item1, ">", _param2.Item2,
			"]"
		});
	}

	// Token: 0x06008956 RID: 35158 RVA: 0x0034649C File Offset: 0x0034469C
	public static string BuildUrlFunctionString(string _type, [TupleElementNames(new string[] { "key", "value" })] ValueTuple<string, string> _param1, [TupleElementNames(new string[] { "key", "value" })] ValueTuple<string, string> _param2, [TupleElementNames(new string[] { "key", "value" })] ValueTuple<string, string> _param3)
	{
		return string.Concat(new string[]
		{
			"[url=Type>", _type, "|", _param1.Item1, ">", _param1.Item2, "|", _param2.Item1, ">", _param2.Item2,
			"|", _param3.Item1, ">", _param3.Item2, "]"
		});
	}

	// Token: 0x06008957 RID: 35159 RVA: 0x00346538 File Offset: 0x00344738
	[PublicizedFrom(EAccessModifier.Private)]
	static LabelUrlUtils()
	{
		LabelUrlUtils.RegisterLabelUrlHandler("Chat", new LabelUrlUtils.HandleTextClickUrlDelegate(LabelUrlUtils.handleChatTargetUrlClick), new LabelUrlUtils.HandleTextHoverUrlDelegate(LabelUrlUtils.handleChatTargetUrlHover));
		LabelUrlUtils.RegisterLabelUrlHandler("HTTP", new LabelUrlUtils.HandleTextClickUrlDelegate(LabelUrlUtils.handleHttpUrl), new LabelUrlUtils.HandleTextHoverUrlDelegate(LabelUrlUtils.handleHttpUrlHover));
	}

	// Token: 0x06008958 RID: 35160 RVA: 0x003465A4 File Offset: 0x003447A4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void handleChatTargetUrlHover(XUiView _sender, string _sourceUrl, Dictionary<string, string> _urlElements, out string _tooltipText)
	{
		_tooltipText = null;
		string text;
		if (!_urlElements.TryGetValue("ChatType", out text))
		{
			Log.Warning("Chat URL ('" + _sourceUrl + "'): No ChatType defined");
			return;
		}
		string text2;
		_urlElements.TryGetValue("Sender", out text2);
		EChatType echatType;
		if (!EnumUtils.TryParse<EChatType>(text, out echatType, false))
		{
			Log.Warning(string.Concat(new string[] { "Chat URL ('", _sourceUrl, "'): Invalid chat type value '", text, "'" }));
			return;
		}
		string text3;
		if (XUiC_Chat.GetChatTargetTooltip(_sender.xui, echatType, text2, out text3))
		{
			_tooltipText = "Chat with: " + text3;
		}
	}

	// Token: 0x06008959 RID: 35161 RVA: 0x00346644 File Offset: 0x00344844
	[PublicizedFrom(EAccessModifier.Private)]
	public static void handleChatTargetUrlClick(XUiView _sender, string _sourceUrl, Dictionary<string, string> _urlElements)
	{
		string text;
		if (!_urlElements.TryGetValue("ChatType", out text))
		{
			Log.Warning("Chat URL ('" + _sourceUrl + "'): No ChatType defined");
			return;
		}
		string text2;
		_urlElements.TryGetValue("Sender", out text2);
		EChatType echatType;
		if (!EnumUtils.TryParse<EChatType>(text, out echatType, false))
		{
			Log.Warning(string.Concat(new string[] { "Chat URL ('", _sourceUrl, "'): Invalid chat type value '", text, "'" }));
			return;
		}
		XUiC_Chat.SetChatTarget(_sender.xui, echatType, text2);
	}

	// Token: 0x0600895A RID: 35162 RVA: 0x003466D0 File Offset: 0x003448D0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void handleHttpUrlHover(XUiView _sender, string _sourceUrl, Dictionary<string, string> _urlElements, out string _tooltipText)
	{
		_tooltipText = null;
		string text;
		if (!_urlElements.TryGetValue("Target", out text))
		{
			Log.Warning("Web URL (" + _sourceUrl + "): No Target defined");
			return;
		}
		_tooltipText = text;
	}

	// Token: 0x0600895B RID: 35163 RVA: 0x00346708 File Offset: 0x00344908
	[PublicizedFrom(EAccessModifier.Private)]
	public static void handleHttpUrl(XUiView _sender, string _sourceUrl, Dictionary<string, string> _urlElements)
	{
		string text;
		if (!_urlElements.TryGetValue("Target", out text))
		{
			Log.Warning("Web URL (" + _sourceUrl + "): No Target defined");
			return;
		}
		XUiC_MessageBoxWindowGroup.ShowUrlConfirmationDialog(_sender.xui, text, false, null, null, null, "", null, false);
	}

	// Token: 0x040065F2 RID: 26098
	public const string LabelUrlTypeFieldName = "Type";

	// Token: 0x040065F3 RID: 26099
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, LabelUrlUtils.SHandlerData> urlHandlers = new Dictionary<string, LabelUrlUtils.SHandlerData>();

	// Token: 0x040065F4 RID: 26100
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex urlMatcher = new Regex("([^|]+)>([^|]+)", RegexOptions.Compiled | RegexOptions.Singleline);

	// Token: 0x020010EC RID: 4332
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly struct SHandlerData
	{
		// Token: 0x0600895C RID: 35164 RVA: 0x00346751 File Offset: 0x00344951
		public SHandlerData(LabelUrlUtils.HandleTextClickUrlDelegate _clickDelegate, LabelUrlUtils.HandleTextHoverUrlDelegate _hoverDelegate)
		{
			this.ClickDelegate = _clickDelegate;
			this.HoverDelegate = _hoverDelegate;
		}

		// Token: 0x040065F5 RID: 26101
		public readonly LabelUrlUtils.HandleTextClickUrlDelegate ClickDelegate;

		// Token: 0x040065F6 RID: 26102
		public readonly LabelUrlUtils.HandleTextHoverUrlDelegate HoverDelegate;
	}

	// Token: 0x020010ED RID: 4333
	// (Invoke) Token: 0x0600895E RID: 35166
	public delegate void HandleTextClickUrlDelegate(XUiView _sender, string _sourceUrl, Dictionary<string, string> _urlElements);

	// Token: 0x020010EE RID: 4334
	// (Invoke) Token: 0x06008962 RID: 35170
	public delegate void HandleTextHoverUrlDelegate(XUiView _sender, string _sourceUrl, Dictionary<string, string> _urlElements, out string _tooltipText);
}
