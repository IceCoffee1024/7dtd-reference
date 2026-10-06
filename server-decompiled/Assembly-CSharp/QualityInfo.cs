using System;
using UnityEngine;

// Token: 0x0200058B RID: 1419
public class QualityInfo
{
	// Token: 0x06002ECA RID: 11978 RVA: 0x00128235 File Offset: 0x00126435
	public static void Cleanup()
	{
		QualityInfo.qualityColors = new Color[7];
		QualityInfo.hexColors = new string[7];
	}

	// Token: 0x06002ECB RID: 11979 RVA: 0x0012824D File Offset: 0x0012644D
	public static void Add(int _key, string _hexColor)
	{
		QualityInfo.qualityColors[_key] = QualityInfo.HexToRGB(_hexColor);
		QualityInfo.hexColors[_key] = _hexColor;
	}

	// Token: 0x06002ECC RID: 11980 RVA: 0x00128268 File Offset: 0x00126468
	public static Color GetQualityColor(int _quality)
	{
		return QualityInfo.GetTierColor(_quality);
	}

	// Token: 0x06002ECD RID: 11981 RVA: 0x00128270 File Offset: 0x00126470
	public static Color GetTierColor(int _tier)
	{
		if (_tier > QualityInfo.qualityColors.Length - 1)
		{
			_tier = QualityInfo.qualityColors.Length - 1;
		}
		return QualityInfo.qualityColors[_tier];
	}

	// Token: 0x06002ECE RID: 11982 RVA: 0x00128294 File Offset: 0x00126494
	public static string GetQualityColorHex(int _quality)
	{
		if (_quality > QualityInfo.qualityColors.Length - 1)
		{
			_quality = QualityInfo.qualityColors.Length - 1;
		}
		return QualityInfo.hexColors[_quality];
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x001282B4 File Offset: 0x001264B4
	public static string GetQualityLevelName(int _quality, bool _useQualityColor = false)
	{
		if (_quality == 0)
		{
			return Localization.Get("lblQualityBroken", false, null);
		}
		string text = "";
		_quality /= 1;
		switch (_quality)
		{
		case 0:
			text = Localization.Get("lblQualityDamaged", false, null);
			break;
		case 1:
			text = Localization.Get("lblQualityPoor", false, null);
			break;
		case 2:
			text = Localization.Get("lblQualityAverage", false, null);
			break;
		case 3:
			text = Localization.Get("lblQualityGreat", false, null);
			break;
		case 4:
			text = Localization.Get("lblQualityFlawless", false, null);
			break;
		case 5:
			text = Localization.Get("lblQualityLegendary", false, null);
			break;
		case 6:
			text = Localization.Get("lblQualityLegendary", false, null);
			break;
		}
		if (_useQualityColor)
		{
			text = string.Format("[{0}]{1}[-]", QualityInfo.GetQualityColorHex(_quality), text);
		}
		return text;
	}

	// Token: 0x06002ED0 RID: 11984 RVA: 0x00128380 File Offset: 0x00126580
	[PublicizedFrom(EAccessModifier.Private)]
	public static int HexToInt(char hexChar)
	{
		switch (hexChar)
		{
		case '0':
			return 0;
		case '1':
			return 1;
		case '2':
			return 2;
		case '3':
			return 3;
		case '4':
			return 4;
		case '5':
			return 5;
		case '6':
			return 6;
		case '7':
			return 7;
		case '8':
			return 8;
		case '9':
			return 9;
		case 'A':
			return 10;
		case 'B':
			return 11;
		case 'C':
			return 12;
		case 'D':
			return 13;
		case 'E':
			return 14;
		case 'F':
			return 15;
		}
		return -1;
	}

	// Token: 0x06002ED1 RID: 11985 RVA: 0x0012841C File Offset: 0x0012661C
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color HexToRGB(string color)
	{
		color.Replace("#", "");
		float num = ((float)QualityInfo.HexToInt(color[1]) + (float)QualityInfo.HexToInt(color[0]) * 16f) / 255f;
		float num2 = ((float)QualityInfo.HexToInt(color[3]) + (float)QualityInfo.HexToInt(color[2]) * 16f) / 255f;
		float num3 = ((float)QualityInfo.HexToInt(color[5]) + (float)QualityInfo.HexToInt(color[4]) * 16f) / 255f;
		return new Color
		{
			r = num,
			g = num2,
			b = num3,
			a = 1f
		};
	}

	// Token: 0x040023FC RID: 9212
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color[] qualityColors = new Color[7];

	// Token: 0x040023FD RID: 9213
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] hexColors = new string[7];
}
