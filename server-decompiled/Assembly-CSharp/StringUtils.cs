using System;
using System.Globalization;

// Token: 0x020014F5 RID: 5365
public static class StringUtils
{
	// Token: 0x0600A7FF RID: 43007 RVA: 0x003E84BE File Offset: 0x003E66BE
	[PublicizedFrom(EAccessModifier.Private)]
	static StringUtils()
	{
		Localization.LanguageSelected += StringUtils.OnLanguageSelected;
		StringUtils.OnLanguageSelected(Localization.RequestedLanguage);
	}

	// Token: 0x0600A800 RID: 43008 RVA: 0x003E84EC File Offset: 0x003E66EC
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

	// Token: 0x0600A801 RID: 43009 RVA: 0x003E8598 File Offset: 0x003E6798
	public static string ToLowerWithUserLocale(this string _value)
	{
		return StringUtils.textInfo.ToLower(_value);
	}

	// Token: 0x0600A802 RID: 43010 RVA: 0x003E85A5 File Offset: 0x003E67A5
	public static string ToUpperWithUserLocale(this string _value)
	{
		return StringUtils.textInfo.ToUpper(_value);
	}

	// Token: 0x04007CA0 RID: 31904
	[PublicizedFrom(EAccessModifier.Private)]
	public static TextInfo textInfo = Utils.StandardCulture.TextInfo;
}
