using System;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000D36 RID: 3382
[PublicizedFrom(EAccessModifier.Internal)]
public static class TriggerEffectXboxParsers
{
	// Token: 0x06006679 RID: 26233 RVA: 0x0028898C File Offset: 0x00286B8C
	public static bool ParseStrength(string effectType, XElement elementTriggerEffect, string name, out float strength)
	{
		string text;
		if (!elementTriggerEffect.TryGetAttribute("strength", out text))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, "): strength is missing" }));
			strength = 0f;
			return false;
		}
		if (!StringParsers.TryParseFloat(text, out strength))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, ") strength failed to parse as a float" }));
			return false;
		}
		if (strength > 1f || strength < 0f)
		{
			Debug.LogError(string.Format("Trigger effect {0}({1}) strength is invalid, correct values are 0 to 1 inclusive, as a floating point number. actual: {2}", name, effectType, strength));
			return false;
		}
		return true;
	}

	// Token: 0x0600667A RID: 26234 RVA: 0x00288A54 File Offset: 0x00286C54
	public static bool ParseStartEndPosition(string effectType, XElement elementTriggerEffect, string name, out float startPosition, out float endPosition)
	{
		string text;
		if (!elementTriggerEffect.TryGetAttribute("startPosition", out text))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, "): startPosition is missing" }));
			startPosition = 0f;
			endPosition = 0f;
			return false;
		}
		if (!StringParsers.TryParseFloat(text, out startPosition))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, ") endPosition failed to parse" }));
			endPosition = 0f;
			return false;
		}
		if (startPosition > 1f || startPosition < 0f)
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, ") startPosition is invalid, correct values are 0 to 1 inclusive, and must be less than EndPosition" }));
			endPosition = 0f;
			return false;
		}
		string text2;
		if (!elementTriggerEffect.TryGetAttribute("endPosition", out text2))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, "): endPosition is missing" }));
			startPosition = 0f;
			endPosition = 0f;
			return false;
		}
		if (!StringParsers.TryParseFloat(text2, out endPosition))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, ") endPosition failed to parse" }));
			return false;
		}
		if (endPosition > 1f || endPosition < 0f || startPosition >= endPosition)
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, ") endPosition is invalid, correct array values are 1 to 10 inclusive, and must be greater than StartPosition" }));
			return false;
		}
		return true;
	}

	// Token: 0x0600667B RID: 26235 RVA: 0x00288C30 File Offset: 0x00286E30
	public static bool ParseStartEndStrength(string effectType, XElement elementTriggerEffect, string name, out float startStrength, out float endStrength)
	{
		string text;
		if (!elementTriggerEffect.TryGetAttribute("startStrength", out text))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, "): startStrength is missing" }));
			startStrength = 0f;
			endStrength = 0f;
			return false;
		}
		if (!StringParsers.TryParseFloat(text, out startStrength))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, ") endStrength failed to parse" }));
			endStrength = 0f;
			return false;
		}
		if (startStrength > 1f || startStrength < 0f)
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, ") startStrength is invalid, correct values are 0 to 1 inclusive, and must be less than EndStrength" }));
			endStrength = 0f;
			return false;
		}
		string text2;
		if (!elementTriggerEffect.TryGetAttribute("endStrength", out text2))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, "): endStrength is missing" }));
			startStrength = 0f;
			endStrength = 0f;
			return false;
		}
		if (!StringParsers.TryParseFloat(text2, out endStrength))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, ") endStrength failed to parse" }));
			return false;
		}
		if (endStrength > 1f || endStrength < 0f || startStrength >= endStrength)
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, ") endStrength is invalid, correct array values are 1 to 10 inclusive, and must be greater than StartStrength" }));
			return false;
		}
		return true;
	}

	// Token: 0x0600667C RID: 26236 RVA: 0x00288E0C File Offset: 0x0028700C
	public static bool ParseAmplitude(string effectType, XElement elementTriggerEffect, string name, out float amplitude)
	{
		string text;
		if (!elementTriggerEffect.TryGetAttribute("amplitude", out text))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, "): amplitude is missing" }));
			amplitude = 0f;
			return false;
		}
		if (!StringParsers.TryParseFloat(text, out amplitude))
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, ") amplitude failed to parse as float" }));
			return false;
		}
		if (amplitude < 8f)
		{
			Debug.LogError(string.Concat(new string[] { "Trigger effect ", name, "(", effectType, ") amplitude is invalid, correct values are 0 to 1 inclusive" }));
			return false;
		}
		return true;
	}
}
