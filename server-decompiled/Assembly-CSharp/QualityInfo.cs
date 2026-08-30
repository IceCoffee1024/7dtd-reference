using System;
using UnityEngine;

// Token: 0x02000584 RID: 1412
public class QualityInfo
{
	// Token: 0x06002E42 RID: 11842 RVA: 0x0012602D File Offset: 0x0012422D
	public static void Cleanup()
	{
		QualityInfo.qualityColors = new Color[7];
		QualityInfo.hexColors = new string[7];
	}

	// Token: 0x06002E43 RID: 11843 RVA: 0x00126045 File Offset: 0x00124245
	public static void Add(int _key, string _hexColor)
	{
		QualityInfo.qualityColors[_key] = QualityInfo.HexToRGB(_hexColor);
		QualityInfo.hexColors[_key] = _hexColor;
	}

	// Token: 0x06002E44 RID: 11844 RVA: 0x00126060 File Offset: 0x00124260
	public static Color GetQualityColor(int _quality)
	{
		return QualityInfo.GetTierColor(_quality);
	}

	// Token: 0x06002E45 RID: 11845 RVA: 0x00126068 File Offset: 0x00124268
	public static Color GetTierColor(int _tier)
	{
		if (_tier > QualityInfo.qualityColors.Length - 1)
		{
			_tier = QualityInfo.qualityColors.Length - 1;
		}
		return QualityInfo.qualityColors[_tier];
	}

	// Token: 0x06002E46 RID: 11846 RVA: 0x0012608C File Offset: 0x0012428C
	public static string GetQualityColorHex(int _quality)
	{
		if (_quality > QualityInfo.qualityColors.Length - 1)
		{
			_quality = QualityInfo.qualityColors.Length - 1;
		}
		return QualityInfo.hexColors[_quality];
	}

	// Token: 0x06002E47 RID: 11847 RVA: 0x001260AC File Offset: 0x001242AC
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

	// Token: 0x06002E48 RID: 11848 RVA: 0x00126178 File Offset: 0x00124378
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

	// Token: 0x06002E49 RID: 11849 RVA: 0x00126214 File Offset: 0x00124414
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

	// Token: 0x040023CA RID: 9162
	[PublicizedFrom(EAccessModifier.Private)]
	public static Color[] qualityColors = new Color[7];

	// Token: 0x040023CB RID: 9163
	[PublicizedFrom(EAccessModifier.Private)]
	public static string[] hexColors = new string[7];
}
