using System;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

// Token: 0x02000D43 RID: 3395
public class WeatherSurvivalParametersFromXml
{
	// Token: 0x06006755 RID: 26453 RVA: 0x0028D928 File Offset: 0x0028BB28
	public static void Load(XmlFile _xmlFile)
	{
		XElement root = _xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new Exception("No element <weathersurvival> found!");
		}
		DynamicProperties dynamicProperties = new DynamicProperties();
		foreach (XElement xelement in root.Elements("property"))
		{
			dynamicProperties.Add(xelement, false);
		}
		WeatherManager.ClearTemperatureOffSetHeights();
		foreach (XElement xelement2 in root.Descendants("TemperatureHeight"))
		{
			float num = StringParsers.ParseFloat(xelement2.GetAttribute("height"), 0, -1, NumberStyles.Any);
			float num2 = StringParsers.ParseFloat(xelement2.GetAttribute("addDegrees"), 0, -1, NumberStyles.Any);
			WeatherManager.AddTemperatureOffSetHeight(num, num2);
		}
		foreach (FieldInfo fieldInfo in typeof(WeatherParams).GetFields(BindingFlags.Static | BindingFlags.Public))
		{
			if (fieldInfo.DeclaringType == typeof(WeatherParams) && fieldInfo.FieldType == typeof(float) && dynamicProperties.Contains(fieldInfo.Name))
			{
				fieldInfo.SetValue(null, dynamicProperties.GetFloat(fieldInfo.Name));
			}
		}
	}
}
