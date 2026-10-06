using System;
using System.Globalization;

// Token: 0x020014E3 RID: 5347
public static class StringUtils
{
	// Token: 0x0600A7A7 RID: 42919 RVA: 0x003E60BA File Offset: 0x003E42BA
	[PublicizedFrom(EAccessModifier.Private)]
	static StringUtils()
	{
		Localization.LanguageSelected += StringUtils.OnLanguageSelected;
		StringUtils.OnLanguageSelected(Localization.RequestedLanguage);
	}

	// Token: 0x0600A7A8 RID: 42920 RVA: 0x003E60E8 File Offset: 0x003E42E8
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

	// Token: 0x0600A7A9 RID: 42921 RVA: 0x003E6194 File Offset: 0x003E4394
	public static string ToLowerWithUserLocale(this string _value)
	{
		return StringUtils.textInfo.ToLower(_value);
	}

	// Token: 0x0600A7AA RID: 42922 RVA: 0x003E61A1 File Offset: 0x003E43A1
	public static string ToUpperWithUserLocale(this string _value)
	{
		return StringUtils.textInfo.ToUpper(_value);
	}

	// Token: 0x04007C5F RID: 31839
	[PublicizedFrom(EAccessModifier.Private)]
	public static TextInfo textInfo = Utils.StandardCulture.TextInfo;
}
