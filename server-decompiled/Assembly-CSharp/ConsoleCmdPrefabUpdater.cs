using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine.Scripting;

// Token: 0x0200026F RID: 623
[Preserve]
public class ConsoleCmdPrefabUpdater : ConsoleCmdAbstract
{
	// Token: 0x06001256 RID: 4694 RVA: 0x00073092 File Offset: 0x00071292
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "prefabupdater" };
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06001257 RID: 4695 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x000321FF File Offset: 0x000303FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "";
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x000730A2 File Offset: 0x000712A2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Update prefabs for newer game builds.\r\nUsage:\r\n   1. prefabupdater loadxml <xmlfile>\r\n   2. prefabupdater clearxml\r\n   3. prefabupdater createmapping <prefabname>\r\n   4. prefabupdater loadtable [nametablefile]\r\n   5. prefabupdater unloadtables\r\n   6. prefabupdater updateblocks\r\n\r\n1. Load a blocks.xml that has the information about the prefabs to be\r\n   updated. If you have a modded XML first load that modded XML and\r\n   afterwards load the XML provided with the game for legacy prefabs.\r\n   The xmlfile-parameter can either be relative to the game's base\r\n   directory or an absolute path (for pre-Alpha 17 prefabs).\r\n2. Unload the data loaded with loadxml.\r\n3. Create a block mapping file for the given prefab(s). Accepts '*' as\r\n   wildcard (for pre-Alpha 17 prefabs).\r\n4. Load a block name mapping file. File path is relative to the game\r\n   directory if not specified as absolute path. If no file is given the\r\n   default file supplied with the game is loaded (BlockUpdates.csv). \r\n5. Unload the data loaded with loadtable.\r\n6. Update the block mappings in prefabs with the block name mapping table loaded by 4.";
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x000730AC File Offset: 0x000712AC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count < 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command requires parameters");
			return;
		}
		if (_params[0].EqualsCaseInsensitive("loadxml"))
		{
			if (_params.Count < 2)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("'loadxml' requires 1 argument");
				return;
			}
			if (!SdFile.Exists(_params[1]))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Specified XML file does not exist");
				return;
			}
			this.loadLegacyBlocksXml(_params[1]);
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Loaded block XML data from " + _params[1]);
			return;
		}
		else
		{
			if (_params[0].EqualsCaseInsensitive("clearxml"))
			{
				this.idMapping = new NameIdMapping(null, Block.MAX_BLOCKS);
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Cleared block XML data");
				return;
			}
			if (_params[0].EqualsCaseInsensitive("createmapping"))
			{
				if (_params.Count < 2)
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("'createmapping' requires 1 argument");
					return;
				}
				MicroStopwatch microStopwatch = new MicroStopwatch(true);
				int num = 0;
				foreach (PathAbstractions.AbstractedLocation abstractedLocation in PathAbstractions.PrefabsSearchPaths.GetAvailablePathsList(null, false, null))
				{
					if (this.createMapping(abstractedLocation))
					{
						num++;
					}
				}
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[]
				{
					"Creating ",
					num.ToString(),
					" block mappings took ",
					microStopwatch.ElapsedMilliseconds.ToString(),
					" ms"
				}));
				return;
			}
			else if (_params[0].EqualsCaseInsensitive("loadtable"))
			{
				string text;
				if (_params.Count < 2)
				{
					text = GameIO.GetGameDir("Data/Config") + "/BlockUpdates.csv";
				}
				else
				{
					text = _params[1];
				}
				if (!SdFile.Exists(text))
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Specified file '" + text + "' does not exist");
					return;
				}
				this.loadMappingTable(text);
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Loaded block update table from " + text);
				return;
			}
			else
			{
				if (_params[0].EqualsCaseInsensitive("unloadtables"))
				{
					this.mappingTable.Clear();
					return;
				}
				if (_params[0].EqualsCaseInsensitive("updateblocks"))
				{
					this.updateMappings();
					return;
				}
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unknown subcommand '" + _params[0] + "'");
				return;
			}
		}
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x00073334 File Offset: 0x00071534
	[PublicizedFrom(EAccessModifier.Private)]
	public bool createMapping(PathAbstractions.AbstractedLocation _location)
	{
		string text = _location.FullPathNoExtension + ".blocks.nim";
		if (SdFile.Exists(text))
		{
			Log.Warning("Mapping for " + _location.Name + " already exists, skipping");
			return false;
		}
		Log.Out("Creating block mapping for " + _location.Name);
		Prefab prefab = new Prefab();
		if (!prefab.Load(_location, false, false, false, false))
		{
			Log.Error("Failed loading prefab '" + _location.Name + "'");
			return false;
		}
		NameIdMapping nameIdMapping = new NameIdMapping(text, Block.MAX_BLOCKS);
		int blockCount = prefab.GetBlockCount();
		for (int i = 0; i < blockCount; i++)
		{
			BlockValue blockNoDamage = prefab.GetBlockNoDamage(i);
			string nameForId = this.idMapping.GetNameForId(blockNoDamage.type);
			if (nameForId == null)
			{
				Log.Error("Creating block mapping for prefab failed: Block " + blockNoDamage.type.ToString() + " used in prefab not found in loaded XMLs.");
				return false;
			}
			nameIdMapping.AddMapping(blockNoDamage.type, nameForId, false);
		}
		nameIdMapping.WriteToFile();
		return true;
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x00073440 File Offset: 0x00071640
	[PublicizedFrom(EAccessModifier.Private)]
	public void loadLegacyBlocksXml(string _filename)
	{
		try
		{
			if (!SdFile.Exists(_filename))
			{
				Log.Error("Specified XML file does not exist (" + _filename + ")");
			}
			else
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.XmlResolver = null;
				xmlDocument.SdLoad(_filename);
				XmlElement documentElement = xmlDocument.DocumentElement;
				if (documentElement == null || documentElement.ChildNodes.Count == 0)
				{
					throw new Exception("No element <blocks> found!");
				}
				foreach (object obj in documentElement.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.NodeType == XmlNodeType.Element && xmlNode.Name.Equals("block"))
					{
						XmlElement xmlElement = (XmlElement)xmlNode;
						string attribute = xmlElement.GetAttribute("name");
						int num = int.Parse(xmlElement.GetAttribute("id"));
						this.idMapping.AddMapping(num, attribute, true);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(string.Concat(new string[] { "Loading and parsing '", _filename, "' (", ex.Message, ")" }));
			Log.Error("Loading of legacy blocks.xml aborted due to errors!");
			Log.Error(ex.StackTrace);
		}
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x00073598 File Offset: 0x00071798
	[PublicizedFrom(EAccessModifier.Private)]
	public void loadMappingTable(string _file)
	{
		ByteReader byteReader = new ByteReader(SdFile.ReadAllBytes(_file));
		BetterList<string> betterList = byteReader.ReadCSV();
		if (betterList.size < 4)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Block update table file does not match the expected format.");
			return;
		}
		if (betterList.buffer[0].IndexOf("old", StringComparison.OrdinalIgnoreCase) < 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid block update table file. The first column header is expected to contain 'old'.");
			return;
		}
		int num = 0;
		BetterList<string> betterList2;
		while ((betterList2 = byteReader.ReadCSV()) != null)
		{
			if (betterList2.size >= 4)
			{
				string text = betterList2.buffer[0];
				string text2 = betterList2.buffer[1];
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
				{
					this.mappingTable.Add(new ValueTuple<string, string>(text, text2));
					num++;
				}
			}
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Loaded {0} block mappings.", num));
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x00073668 File Offset: 0x00071868
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateMappings()
	{
		MicroStopwatch microStopwatch = new MicroStopwatch(true);
		int num = 0;
		int num2 = 0;
		foreach (PathAbstractions.AbstractedLocation abstractedLocation in PathAbstractions.PrefabsSearchPaths.GetAvailablePathsList(null, false, null))
		{
			string text = abstractedLocation.FullPathNoExtension + ".blocks.nim";
			if (!SdFile.Exists(text))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Loading block mapping file for prefab \"" + abstractedLocation.Name + "\" failed: Block name to ID mapping file missing.");
			}
			else
			{
				using (NameIdMapping nameIdMapping = MemoryPools.poolNameIdMapping.AllocSync(true))
				{
					nameIdMapping.InitMapping(text, Block.MAX_BLOCKS);
					if (!nameIdMapping.LoadFromFile())
					{
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Loading block mapping file for prefab \"" + abstractedLocation.Name + "\" failed.");
						continue;
					}
					int num3 = nameIdMapping.ReplaceNames(this.mappingTable);
					if (num3 > 0)
					{
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Updated {0} names for prefab \"{1}\"", num3, abstractedLocation.Name));
					}
					nameIdMapping.SaveIfDirty(false);
					num2 += num3;
				}
				num++;
			}
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Updating {0} block mappings took {1} ms. Replaced a total of {2} entries.", num, microStopwatch.ElapsedMilliseconds, num2));
	}

	// Token: 0x04000D30 RID: 3376
	[PublicizedFrom(EAccessModifier.Private)]
	public NameIdMapping idMapping = new NameIdMapping(null, Block.MAX_BLOCKS);

	// Token: 0x04000D31 RID: 3377
	[TupleElementNames(new string[] { "oldName", "newName" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ValueTuple<string, string>> mappingTable = new List<ValueTuple<string, string>>();

	// Token: 0x04000D32 RID: 3378
	[PublicizedFrom(EAccessModifier.Private)]
	public const string DefaultMappingFile = "BlockUpdates.csv";
}
