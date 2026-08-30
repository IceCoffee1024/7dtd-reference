using System;
using System.Globalization;

// Token: 0x020014C1 RID: 5313
public static class StringUtils
{
	// Token: 0x0600A6F4 RID: 42740 RVA: 0x003E1266 File Offset: 0x003DF466
	[PublicizedFrom(EAccessModifier.Private)]
	static StringUtils()
	{
		Localization.LanguageSelected += StringUtils.OnLanguageSelected;
		StringUtils.OnLanguageSelected(Localization.RequestedLanguage);
	}

	// Token: 0x0600A6F5 RID: 42741 RVA: 0x003E1294 File Offset: 0x003DF494
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnLanguageSelected(string _lang)
	{
		string text = Localization.Get("cultureInfoName", false, null);
		if (string.IsNullOrEmpty(text))
		{
			Log.Warning("No culture info name given for selected language: " + _lang);
			return;
		}
		TextInfo textInfo;
		try
		{
			textInfo = CultureInfo.GetCultureInfo(text).TextInfo;
		}
		catch (Exception)
		{
			Log.Warning(string.Concat(new string[] { "No culture info found for given name: ", text, " (language: ", _lang, ")" }));
			return;
		}
		if (textInfo.CultureName != StringUtils.textInfo.CultureName)
		{
			StringUtils.textInfo = textInfo;
			Log.Out("Updated culture for display texts");
		}
	}

	// Token: 0x0600A6F6 RID: 42742 RVA: 0x003E1340 File Offset: 0x003DF540
	public static string ToLowerWithUserLocale(this string _value)
	{
		return StringUtils.textInfo.ToLower(_value);
	}

	// Token: 0x0600A6F7 RID: 42743 RVA: 0x003E134D File Offset: 0x003DF54D
	public static string ToUpperWithUserLocale(this string _value)
	{
		return StringUtils.textInfo.ToUpper(_value);
	}

	// Token: 0x04007BDC RID: 31708
	[PublicizedFrom(EAccessModifier.Private)]
	public static TextInfo textInfo = Utils.StandardCulture.TextInfo;
}
