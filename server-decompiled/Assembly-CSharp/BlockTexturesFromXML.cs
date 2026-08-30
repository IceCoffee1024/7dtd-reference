using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

// Token: 0x02000CE8 RID: 3304
public class BlockTexturesFromXML
{
	// Token: 0x0600650D RID: 25869 RVA: 0x00278E4C File Offset: 0x0027704C
	public static IEnumerator CreateBlockTextures(XmlFile _xmlFile)
	{
		BlockTextureData.InitStatic();
		XElement root = _xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new Exception("No element <block_textures> found!");
		}
		using (IEnumerator<XElement> enumerator = root.Elements("paint").GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				XElement xelement = enumerator.Current;
				DynamicProperties dynamicProperties = new DynamicProperties();
				foreach (XElement xelement2 in xelement.Elements("property"))
				{
					dynamicProperties.Add(xelement2, false);
				}
				BlockTextureData blockTextureData = new BlockTextureData();
				blockTextureData.Name = xelement.GetAttribute("name");
				blockTextureData.LocalizedName = Localization.Get(blockTextureData.Name, false, null);
				blockTextureData.ID = int.Parse(xelement.GetAttribute("id"));
				if (dynamicProperties.Values.ContainsKey("Group"))
				{
					blockTextureData.Group = dynamicProperties.Values["Group"];
				}
				if (dynamicProperties.Values.ContainsKey("PaintCost"))
				{
					blockTextureData.PaintCost = Convert.ToUInt16(dynamicProperties.Values["PaintCost"]);
				}
				else
				{
					blockTextureData.PaintCost = 1;
				}
				if (dynamicProperties.Values.ContainsKey("TextureId"))
				{
					blockTextureData.TextureID = Convert.ToUInt16(dynamicProperties.Values["TextureId"]);
				}
				if (dynamicProperties.Values.ContainsKey("Hidden"))
				{
					blockTextureData.Hidden = Convert.ToBoolean(dynamicProperties.Values["Hidden"]);
				}
				if (dynamicProperties.Values.ContainsKey("SortIndex"))
				{
					blockTextureData.SortIndex = Convert.ToByte(dynamicProperties.Values["SortIndex"]);
				}
				blockTextureData.Init();
			}
			yield break;
		}
		yield break;
	}
}
