using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

// Token: 0x02000CF7 RID: 3319
public class BlocksFromXml
{
	// Token: 0x06006582 RID: 25986 RVA: 0x0027A680 File Offset: 0x00278880
	public static IEnumerator CreateBlocks(XmlFile _xmlFile, bool _fillLookupTable, bool _bEditMode = false)
	{
		XElement root = _xmlFile.XmlDoc.Root;
		if (!root.HasElements)
		{
			throw new Exception("No element <blocks> found!");
		}
		if (root.HasAttribute("defaultDescriptionKey"))
		{
			Block.defaultBlockDescriptionKey = root.GetAttribute("defaultDescriptionKey");
		}
		MicroStopwatch msw = new MicroStopwatch(true);
		int i = 0;
		int totalBlocks = root.Elements("block").Count<XElement>();
		LocalPlayerUI ui = LocalPlayerUI.primaryUI;
		bool progressWindowOpen = ui && ui.windowManager.IsWindowOpen(XUiC_ProgressWindow.ID);
		foreach (XElement xelement in root.Elements("block"))
		{
			int num = i;
			i = num + 1;
			if (xelement.HasAttribute(XNames.shapes))
			{
				yield return ShapesFromXml.CreateShapeVariants(_bEditMode, xelement);
			}
			else
			{
				BlocksFromXml.ParseBlock(_bEditMode, xelement);
			}
			if (msw.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
			{
				if (progressWindowOpen)
				{
					XUiC_ProgressWindow.SetText(ui, string.Format(Localization.Get("uiLoadLoadingXmlBlocks", false, null), Math.Min(100.0, 105.0 * (double)i / (double)totalBlocks).ToString("0")), true);
				}
				yield return null;
				msw.ResetAndRestart();
			}
		}
		IEnumerator<XElement> enumerator = null;
		if (progressWindowOpen)
		{
			XUiC_ProgressWindow.SetText(ui, string.Format(Localization.Get("uiLoadLoadingXmlBlocks", false, null), "100"), true);
			yield return null;
			XUiC_ProgressWindow.SetText(ui, Localization.Get("uiLoadLoadingXml", false, null), true);
		}
		if (Application.isPlaying)
		{
			Resources.UnloadUnusedAssets();
		}
		ShapesFromXml.Cleanup();
		yield break;
		yield break;
	}

	// Token: 0x06006583 RID: 25987 RVA: 0x0027A698 File Offset: 0x00278898
	public static void ParseBlock(bool _bEditMode, XElement elementBlock)
	{
		DynamicProperties dynamicProperties = BlocksFromXml.ParseProperties(elementBlock);
		string attribute = elementBlock.GetAttribute(XNames.name);
		Block block = BlocksFromXml.CreateBlock(_bEditMode, attribute, dynamicProperties);
		bool flag;
		BlocksFromXml.ParseItemDrops(block, elementBlock, out flag);
		if (!flag)
		{
			BlocksFromXml.LoadExtendedItemDrops(block);
		}
		BlocksFromXml.InitBlock(block);
	}

	// Token: 0x06006584 RID: 25988 RVA: 0x0027A6DC File Offset: 0x002788DC
	public static void ParseExtendedBlock(XElement elementBlock, out string extendedBlockName, out string excludedPropertiesList)
	{
		IEnumerable<XElement> enumerable = from e in elementBlock.Elements(XNames.property)
			where e.GetAttribute(XNames.name) == "Extends"
			select e;
		XElement xelement = ((enumerable != null) ? enumerable.FirstOrDefault<XElement>() : null);
		if (xelement != null)
		{
			extendedBlockName = xelement.GetAttribute(XNames.value);
			excludedPropertiesList = xelement.GetAttribute(XNames.param1);
			return;
		}
		extendedBlockName = null;
		excludedPropertiesList = null;
	}

	// Token: 0x06006585 RID: 25989 RVA: 0x0027A74C File Offset: 0x0027894C
	public static DynamicProperties ParseProperties(XElement elementBlock)
	{
		string text;
		string text2;
		BlocksFromXml.ParseExtendedBlock(elementBlock, out text, out text2);
		DynamicProperties dynamicProperties = BlocksFromXml.CreateProperties(text, text2);
		BlocksFromXml.LoadProperties(dynamicProperties, elementBlock);
		return dynamicProperties;
	}

	// Token: 0x06006586 RID: 25990 RVA: 0x0027A774 File Offset: 0x00278974
	public static DynamicProperties CreateProperties(string extendedBlockName = null, string excludedPropertiesList = null)
	{
		DynamicProperties dynamicProperties = new DynamicProperties();
		if (extendedBlockName != null)
		{
			Block blockByName = Block.GetBlockByName(extendedBlockName, false);
			if (blockByName == null)
			{
				throw new Exception(string.Format("Could not find Extends block {0}", extendedBlockName));
			}
			HashSet<string> hashSet = new HashSet<string> { Block.PropCreativeMode };
			if (!string.IsNullOrEmpty(excludedPropertiesList))
			{
				foreach (string text in excludedPropertiesList.Split(',', StringSplitOptions.None))
				{
					hashSet.Add(text.Trim());
				}
			}
			dynamicProperties.CopyFrom(blockByName.Properties, hashSet);
		}
		return dynamicProperties;
	}

	// Token: 0x06006587 RID: 25991 RVA: 0x0027A800 File Offset: 0x00278A00
	public static void LoadProperties(DynamicProperties properties, XElement elementBlock)
	{
		foreach (XElement xelement in elementBlock.Elements(XNames.property))
		{
			properties.Add(xelement, false);
		}
		foreach (XElement xelement2 in elementBlock.Elements(XNames.array))
		{
			properties.AddArray(xelement2);
		}
	}

	// Token: 0x06006588 RID: 25992 RVA: 0x0027A894 File Offset: 0x00278A94
	public static Block CreateBlock(bool _bEditMode, string blockName, DynamicProperties properties)
	{
		Block block;
		if (properties.Values.ContainsKey("Class"))
		{
			string text = properties.Values["Class"];
			Type typeWithPrefix = ReflectionHelpers.GetTypeWithPrefix("Block", text);
			if (typeWithPrefix == null || (block = Activator.CreateInstance(typeWithPrefix) as Block) == null)
			{
				throw new Exception(string.Concat(new string[] { "Class '", text, "' not found on block ", blockName, "!" }));
			}
		}
		else
		{
			block = new Block();
		}
		block.Properties = properties;
		block.SetBlockName(blockName);
		block.ResourceScale = 1f;
		properties.ParseFloat(Block.PropResourceScale, ref block.ResourceScale);
		BlockPlacement blockPlacement = BlockPlacement.None;
		if (properties.Values.ContainsKey("Place"))
		{
			string text2 = properties.Values["Place"];
			try
			{
				blockPlacement = (BlockPlacement)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("BlockPlacement", text2));
			}
			catch (Exception)
			{
				throw new Exception(string.Concat(new string[] { "No block placement class '", text2, "' found on block ", blockName, "!" }));
			}
		}
		block.BlockPlacementHelper = blockPlacement;
		string text3 = properties.Values["Material"];
		block.blockMaterial = MaterialBlock.fromString(text3);
		if (text3 == null || text3.Length == 0)
		{
			throw new Exception("Block with name=" + blockName + " has no material defined");
		}
		if (block.blockMaterial == null)
		{
			throw new Exception(string.Concat(new string[] { "Block with name=", blockName, " references a not existing material '", text3, "'" }));
		}
		BlockShape blockShape;
		if (properties.Values.ContainsKey("Shape"))
		{
			string text4 = properties.Values["Shape"];
			try
			{
				blockShape = (BlockShape)Activator.CreateInstance(ReflectionHelpers.GetTypeWithPrefix("BlockShape", text4));
				goto IL_0218;
			}
			catch (Exception)
			{
				throw new Exception("Shape class '" + text4 + "' not found for block " + blockName);
			}
		}
		blockShape = new BlockShapeNew();
		block.Properties.Values["Model"] = "@:Shapes/Cube.fbx";
		IL_0218:
		block.shape = blockShape;
		if (properties.Values.ContainsKey("ShapeMinBB"))
		{
			Vector3 vector = StringParsers.ParseVector3(properties.Values["ShapeMinBB"], 0, -1);
			block.shape.SetMinAABB(vector);
		}
		if (properties.Values.ContainsKey("Mesh"))
		{
			block.MeshIndex = byte.MaxValue;
			string text5 = properties.Values["Mesh"];
			for (int i = 0; i < MeshDescription.meshes.Length; i++)
			{
				if (text5.Equals(MeshDescription.meshes[i].Name))
				{
					block.MeshIndex = (byte)i;
					break;
				}
			}
			if (block.MeshIndex == 255)
			{
				throw new Exception("Unknown mesh attribute '" + text5 + "' on block " + blockName);
			}
		}
		if (!_bEditMode && properties.Values.ContainsKey("Stacknumber"))
		{
			block.Stacknumber = int.Parse(properties.Values["Stacknumber"]);
		}
		else
		{
			block.Stacknumber = 500;
		}
		if (properties.Values.ContainsKey("Light"))
		{
			block.SetLightValue(StringParsers.ParseFloat(properties.Values["Light"], 0, -1, NumberStyles.Any));
		}
		if (properties.Values.ContainsKey("MovementFactor"))
		{
			block.MovementFactor = StringParsers.ParseFloat(properties.Values["MovementFactor"], 0, -1, NumberStyles.Any);
		}
		else
		{
			block.MovementFactor = block.blockMaterial.MovementFactor;
		}
		if (block.MovementFactor <= 0f)
		{
			block.MovementFactor = 1f;
		}
		block.IsCheckCollideWithEntity |= block.MovementFactor != 1f;
		if (properties.Values.ContainsKey("EconomicValue"))
		{
			block.EconomicValue = StringParsers.ParseFloat(properties.Values["EconomicValue"], 0, -1, NumberStyles.Any);
		}
		if (properties.Values.ContainsKey("Collide"))
		{
			string text6 = properties.Values["Collide"];
			block.BlockingType = 0;
			if (text6.ContainsCaseInsensitive("sight"))
			{
				block.BlockingType |= 1;
			}
			if (text6.ContainsCaseInsensitive("movement"))
			{
				block.BlockingType |= 2;
			}
			if (text6.ContainsCaseInsensitive("bullet"))
			{
				block.BlockingType |= 4;
			}
			if (text6.ContainsCaseInsensitive("rocket"))
			{
				block.BlockingType |= 8;
			}
			if (text6.ContainsCaseInsensitive("arrow"))
			{
				block.BlockingType |= 32;
			}
			if (text6.ContainsCaseInsensitive("melee"))
			{
				block.BlockingType |= 16;
			}
		}
		else
		{
			block.BlockingType = (block.blockMaterial.IsCollidable ? 255 : 0);
		}
		string text7;
		if (properties.TryGetValue(Block.PropDisableCover, out text7))
		{
			block.DisableCover = StringParsers.ParseBool(text7, 0, -1, true);
		}
		else
		{
			block.DisableCover = false;
		}
		string text8;
		if (properties.Values.TryGetValue("Path", out text8))
		{
			if (text8.EqualsCaseInsensitive("solid"))
			{
				block.PathType = 1;
			}
			if (text8.EqualsCaseInsensitive("scan"))
			{
				block.PathType = -1;
			}
		}
		else if (properties.Values.TryGetValue("Model", out text8) && (text8.EqualsCaseInsensitive("@:Shapes/cube.fbx") || text8.EqualsCaseInsensitive("@:Shapes/cube_glass.fbx") || text8.EqualsCaseInsensitive("@:Shapes/cube_frame.fbx")))
		{
			block.PathType = 1;
		}
		string text9;
		if (properties.Values.TryGetValue("WaterFlow", out text9))
		{
			block.WaterFlowMask = StringParsers.ParseWaterFlowMask(text9);
		}
		string text10;
		if (properties.Values.TryGetValue("WaterClipPlane", out text10))
		{
			block.WaterClipPlane = StringParsers.ParsePlane(text10);
			block.WaterClipEnabled = true;
		}
		else
		{
			block.WaterClipEnabled = false;
		}
		for (int j = 0; j < 1; j++)
		{
			string texture = ShapesFromXml.TextureLabelsByChannel[j].Texture;
			string @string = properties.GetString(texture);
			if (@string.Length > 0)
			{
				try
				{
					if (@string.Contains(","))
					{
						string[] array = @string.Split(',', StringSplitOptions.None);
						block.SetSideTextureId(array, j);
					}
					else
					{
						int num = int.Parse(@string);
						block.SetSideTextureId(num, j);
					}
				}
				catch (Exception)
				{
					throw new Exception(string.Concat(new string[] { "Error parsing \"", texture, "\" texture id '", @string, "' in block with name=", blockName }));
				}
			}
		}
		properties.ParseInt("TerrainIndex", ref block.TerrainTAIndex);
		if (properties.Values.ContainsKey("BlockTag"))
		{
			block.BlockTag = EnumUtils.Parse<BlockTags>(properties.Values["BlockTag"], false);
		}
		if (properties.Values.ContainsKey("StabilitySupport"))
		{
			block.StabilitySupport = properties.GetBool("StabilitySupport");
		}
		else
		{
			block.StabilitySupport = block.blockMaterial.StabilitySupport;
		}
		if (properties.Values.ContainsKey("StabilityFull"))
		{
			block.StabilityFull = properties.GetBool("StabilityFull");
		}
		if (properties.Values.ContainsKey("StabilityIgnore"))
		{
			block.StabilityIgnore = properties.GetBool("StabilityIgnore");
		}
		if (properties.Values.ContainsKey("Density"))
		{
			block.Density = (sbyte)(properties.GetFloat("Density") * (float)(block.shape.IsTerrain() ? MarchingCubes.DensityTerrain : MarchingCubes.DensityAir));
		}
		else
		{
			block.Density = (block.shape.IsTerrain() ? MarchingCubes.DensityTerrain : MarchingCubes.DensityAir);
		}
		DynamicProperties @class = properties.GetClass("RepairItems");
		if (@class != null)
		{
			block.RepairItems = new List<Block.SItemNameCount>();
			foreach (KeyValuePair<string, string> keyValuePair in @class.Values)
			{
				Block.SItemNameCount sitemNameCount = default(Block.SItemNameCount);
				sitemNameCount.ItemName = keyValuePair.Key;
				sitemNameCount.Count = int.Parse(@class.Values[keyValuePair.Key]);
				block.RepairItems.Add(sitemNameCount);
			}
		}
		DynamicProperties class2 = properties.GetClass("RepairItemsMeshDamage");
		if (class2 != null)
		{
			block.RepairItemsMeshDamage = new List<Block.SItemNameCount>();
			foreach (KeyValuePair<string, string> keyValuePair2 in class2.Values)
			{
				Block.SItemNameCount sitemNameCount2;
				sitemNameCount2.ItemName = keyValuePair2.Key;
				sitemNameCount2.Count = int.Parse(class2.Values[keyValuePair2.Key]);
				block.RepairItemsMeshDamage.Add(sitemNameCount2);
			}
		}
		if (properties.Values.ContainsKey("RestrictSubmergedPlacement"))
		{
			block.bRestrictSubmergedPlacement = properties.GetBool("RestrictSubmergedPlacement");
		}
		return block;
	}

	// Token: 0x06006589 RID: 25993 RVA: 0x0027B1D8 File Offset: 0x002793D8
	public static void ParseItemDrops(Block block, XElement elementBlock, out bool dropExtendsOff)
	{
		dropExtendsOff = false;
		foreach (XElement xelement in elementBlock.Elements())
		{
			if (xelement.Name == XNames.dropextendsoff)
			{
				dropExtendsOff = true;
			}
			else if (xelement.Name == XNames.drop)
			{
				XElement xelement2 = xelement;
				string attribute = xelement2.GetAttribute(XNames.name);
				int num = 1;
				int num2 = 1;
				if (xelement2.HasAttribute(XNames.count))
				{
					StringParsers.ParseMinMaxCount(xelement2.GetAttribute(XNames.count), out num, out num2);
				}
				float num3 = 1f;
				DynamicProperties.ParseFloat(xelement2, "prob", ref num3);
				num3 *= block.ResourceScale;
				EnumDropEvent enumDropEvent = EnumDropEvent.Destroy;
				if (xelement2.HasAttribute(XNames.event_))
				{
					enumDropEvent = EnumUtils.Parse<EnumDropEvent>(xelement2.GetAttribute(XNames.event_), false);
				}
				float num4 = 0f;
				DynamicProperties.ParseFloat(xelement2, "stick_chance", ref num4);
				string text = null;
				if (xelement2.HasAttribute(XNames.tool_category))
				{
					text = xelement2.GetAttribute(XNames.tool_category);
				}
				string attribute2 = xelement2.GetAttribute(XNames.tag);
				block.AddDroppedId(enumDropEvent, attribute, num, num2, num3, block.ResourceScale, num4, text, attribute2);
			}
		}
	}

	// Token: 0x0600658A RID: 25994 RVA: 0x0027B334 File Offset: 0x00279534
	public static void LoadExtendedItemDrops(Block block)
	{
		if (block.Properties.Values.ContainsKey("Extends"))
		{
			Block blockByName = Block.GetBlockByName(block.Properties.Values["Extends"], false);
			block.CopyDroppedFrom(blockByName);
		}
	}

	// Token: 0x0600658B RID: 25995 RVA: 0x0027B37B File Offset: 0x0027957B
	public static void InitBlock(Block block)
	{
		block.shape.Init(block);
		block.Init();
	}

	// Token: 0x0600658C RID: 25996 RVA: 0x0027B390 File Offset: 0x00279590
	public static bool FindExternalModels(XmlFile _xmlFile, string _meshName, Dictionary<string, string> _referencedModels)
	{
		try
		{
			XElement root = _xmlFile.XmlDoc.Root;
			if (!root.HasElements)
			{
				throw new Exception("No element <blocks> found!");
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (XElement xelement in root.Elements(XNames.block))
			{
				string attribute = xelement.GetAttribute(XNames.name);
				DynamicProperties dynamicProperties = new DynamicProperties();
				foreach (XElement xelement2 in xelement.Elements(XNames.property))
				{
					dynamicProperties.Add(xelement2, false);
				}
				bool flag = false;
				if (dynamicProperties.Values.ContainsKey("Extends"))
				{
					string text = dynamicProperties.Values["Extends"];
					flag = hashSet.Contains(text);
				}
				bool flag2 = dynamicProperties.Values.ContainsKey("Shape") && dynamicProperties.Values["Shape"].StartsWith("Ext3dModel");
				bool flag3 = dynamicProperties.Values.ContainsKey("Shape") && !dynamicProperties.Values["Shape"].StartsWith("Ext3dModel");
				if ((flag && !flag3) || flag2)
				{
					string text2 = "opaque";
					if (dynamicProperties.Values.ContainsKey("Mesh"))
					{
						text2 = dynamicProperties.Values["Mesh"];
					}
					if (flag || text2.Equals(_meshName))
					{
						string value = dynamicProperties.GetValue("Model");
						if (value != null && !_referencedModels.ContainsKey(value))
						{
							_referencedModels.Add(value, dynamicProperties.GetParam1("Model"));
						}
						hashSet.Add(attribute);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(string.Concat(new string[] { "Loading and parsing '", _xmlFile.Filename, "' (", ex.Message, ")" }));
			Log.Error("Loading of blocks aborted due to errors!");
			Log.Error(ex.StackTrace);
			return false;
		}
		return true;
	}

	// Token: 0x0600658D RID: 25997 RVA: 0x0027B604 File Offset: 0x00279804
	public static HashSet<int> GetTextureIdsForMesh(XmlFile _xmlFile, string _meshName)
	{
		HashSet<int> hashSet = new HashSet<int>();
		try
		{
			XElement root = _xmlFile.XmlDoc.Root;
			if (!root.HasElements)
			{
				throw new Exception("No element <blocks> found!");
			}
			Dictionary<string, DynamicProperties> dictionary = new Dictionary<string, DynamicProperties>();
			foreach (XElement xelement in root.Elements(XNames.block))
			{
				DynamicProperties dynamicProperties = new DynamicProperties();
				foreach (XElement xelement2 in xelement.Elements(XNames.property))
				{
					dynamicProperties.Add(xelement2, false);
				}
				if (dynamicProperties.Values.ContainsKey("Extends"))
				{
					string text = dynamicProperties.Values["Extends"];
					if (!dictionary.ContainsKey(text))
					{
						Log.Error(string.Format("Extends references not existing block {0}", text));
					}
					else
					{
						DynamicProperties dynamicProperties2 = new DynamicProperties();
						dynamicProperties2.CopyFrom(dictionary[text], null);
						dynamicProperties2.CopyFrom(dynamicProperties, null);
						dynamicProperties = dynamicProperties2;
					}
				}
				string attribute = xelement.GetAttribute(XNames.name);
				try
				{
					dictionary.Add(attribute, dynamicProperties);
				}
				catch (Exception)
				{
					throw new Exception("Duplicate block with name " + attribute);
				}
			}
			foreach (XElement xelement3 in root.Elements(XNames.block))
			{
				DynamicProperties dynamicProperties3 = dictionary[xelement3.GetAttribute(XNames.name)];
				string text2 = "opaque";
				if (dynamicProperties3.Values.ContainsKey("Mesh"))
				{
					text2 = dynamicProperties3.Values["Mesh"];
				}
				if (text2.Equals(_meshName) && (!dynamicProperties3.Values.ContainsKey("Shape") || (!dynamicProperties3.Values["Shape"].Equals("ModelEntity") && !dynamicProperties3.Values["Shape"].Equals("DistantDeco"))))
				{
					for (int i = 0; i < 1; i++)
					{
						string text3;
						if (dynamicProperties3.Values.TryGetValue(ShapesFromXml.TextureLabelsByChannel[i].Texture, out text3))
						{
							try
							{
								int num;
								if (text3.Contains(","))
								{
									string[] array = text3.Split(new char[] { ',' });
									for (int j = 0; j < array.Length; j++)
									{
										hashSet.Add(int.Parse(array[j]));
									}
								}
								else if (int.TryParse(text3, out num))
								{
									hashSet.Add(num);
								}
							}
							catch (Exception)
							{
								throw new Exception(string.Concat(new string[]
								{
									"Error parsing texture id '",
									text3,
									"' on layer '",
									(i + 1).ToString(),
									"' in block ",
									xelement3.GetAttribute(XNames.name)
								}));
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(string.Concat(new string[] { "Loading and parsing '", _xmlFile.Filename, "' (", ex.Message, ")" }));
			Log.Error("Loading of blocks aborted due to errors!");
			Log.Error(ex.StackTrace);
		}
		return hashSet;
	}
}
