using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Platform;
using Services.Analytics;
using UnityEngine;

// Token: 0x02000729 RID: 1833
[JsonObject(MemberSerialization.OptIn)]
public class Mod
{
	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x06003792 RID: 14226 RVA: 0x0016DFF9 File Offset: 0x0016C1F9
	// (set) Token: 0x06003793 RID: 14227 RVA: 0x0016E001 File Offset: 0x0016C201
	public Mod.EModLoadState LoadState
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06003794 RID: 14228 RVA: 0x0016E00A File Offset: 0x0016C20A
	// (set) Token: 0x06003795 RID: 14229 RVA: 0x0016E012 File Offset: 0x0016C212
	public string Path
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x06003796 RID: 14230 RVA: 0x0016E01B File Offset: 0x0016C21B
	// (set) Token: 0x06003797 RID: 14231 RVA: 0x0016E023 File Offset: 0x0016C223
	public string FolderName
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x06003798 RID: 14232 RVA: 0x0016E02C File Offset: 0x0016C22C
	// (set) Token: 0x06003799 RID: 14233 RVA: 0x0016E034 File Offset: 0x0016C234
	[JsonProperty]
	[JsonConverter(typeof(TruncateStringSerializerConverter))]
	public string Name
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x0600379A RID: 14234 RVA: 0x0016E03D File Offset: 0x0016C23D
	// (set) Token: 0x0600379B RID: 14235 RVA: 0x0016E045 File Offset: 0x0016C245
	[JsonProperty]
	[JsonConverter(typeof(TruncateStringSerializerConverter))]
	public string DisplayName
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x0600379C RID: 14236 RVA: 0x0016E04E File Offset: 0x0016C24E
	// (set) Token: 0x0600379D RID: 14237 RVA: 0x0016E056 File Offset: 0x0016C256
	[DefaultValue("")]
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
	[JsonConverter(typeof(TruncateStringSerializerConverter))]
	public string Description
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x0600379E RID: 14238 RVA: 0x0016E05F File Offset: 0x0016C25F
	// (set) Token: 0x0600379F RID: 14239 RVA: 0x0016E067 File Offset: 0x0016C267
	public string Author
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x060037A0 RID: 14240 RVA: 0x0016E070 File Offset: 0x0016C270
	// (set) Token: 0x060037A1 RID: 14241 RVA: 0x0016E078 File Offset: 0x0016C278
	public Version Version
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x060037A2 RID: 14242 RVA: 0x0016E081 File Offset: 0x0016C281
	// (set) Token: 0x060037A3 RID: 14243 RVA: 0x0016E089 File Offset: 0x0016C289
	public string VersionString
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x060037A4 RID: 14244 RVA: 0x0016E092 File Offset: 0x0016C292
	// (set) Token: 0x060037A5 RID: 14245 RVA: 0x0016E09A File Offset: 0x0016C29A
	public string Website
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x060037A6 RID: 14246 RVA: 0x0016E0A3 File Offset: 0x0016C2A3
	// (set) Token: 0x060037A7 RID: 14247 RVA: 0x0016E0AB File Offset: 0x0016C2AB
	public bool SkipLoadingWithAntiCheat
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x060037A8 RID: 14248 RVA: 0x0016E0B4 File Offset: 0x0016C2B4
	// (set) Token: 0x060037A9 RID: 14249 RVA: 0x0016E0BC File Offset: 0x0016C2BC
	public bool AntiCheatCompatible
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x060037AA RID: 14250 RVA: 0x0016E0C5 File Offset: 0x0016C2C5
	// (set) Token: 0x060037AB RID: 14251 RVA: 0x0016E0CD File Offset: 0x0016C2CD
	public bool GameConfigMod
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060037AC RID: 14252 RVA: 0x0016E0D6 File Offset: 0x0016C2D6
	[PublicizedFrom(EAccessModifier.Private)]
	public Mod()
	{
		this.AllAssemblies = new ReadOnlyCollection<Assembly>(this.allAssemblies);
	}

	// Token: 0x060037AD RID: 14253 RVA: 0x0016E0FC File Offset: 0x0016C2FC
	public bool LoadMod()
	{
		this.LoadState = Mod.EModLoadState.Failed;
		if (ModManager.ModLoaded(this.Name))
		{
			Log.Warning("[MODS]     Mod with same name (" + this.Name + ") already loaded, ignoring");
			this.LoadState = Mod.EModLoadState.DuplicateModName;
			return false;
		}
		Mod.EModLoadState emodLoadState = this.LoadAssemblies();
		if (emodLoadState != Mod.EModLoadState.Success)
		{
			this.LoadState = emodLoadState;
			return false;
		}
		this.DetectContents();
		Log.Out(string.Concat(new string[]
		{
			"[MODS]     Loaded Mod: ",
			this.Name,
			" (",
			this.VersionString ?? "<unknown version>",
			")"
		}));
		this.LoadState = Mod.EModLoadState.Success;
		return this.LoadState == Mod.EModLoadState.Success;
	}

	// Token: 0x060037AE RID: 14254 RVA: 0x0016E1B0 File Offset: 0x0016C3B0
	[PublicizedFrom(EAccessModifier.Private)]
	public Mod.EModLoadState LoadAssemblies()
	{
		string[] files = SdDirectory.GetFiles(this.Path);
		if (files.Length == 0)
		{
			return Mod.EModLoadState.Success;
		}
		foreach (string text in files)
		{
			if (text.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
			{
				if (!GameManager.IsDedicatedServer)
				{
					IAntiCheatClient antiCheatClient = PlatformManager.MultiPlatform.AntiCheatClient;
					if (antiCheatClient != null && antiCheatClient.ClientAntiCheatEnabled())
					{
						if (this.SkipLoadingWithAntiCheat)
						{
							Log.Out("[MODS]     AntiCheat enabled, mod skipped because it is set not to load");
							return Mod.EModLoadState.SkippedDueToAntiCheat;
						}
						if (!this.AntiCheatCompatible)
						{
							Log.Warning("[MODS]     Mod contains custom code, AntiCheat needs to be disabled to load it!");
							return Mod.EModLoadState.NotAntiCheatCompatible;
						}
					}
				}
				try
				{
					Assembly assembly = Mod.loadAssembly(text);
					this.allAssemblies.Add(assembly);
					Log.Out("[MODS]     Loaded assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, text));
				}
				catch (Exception ex)
				{
					Log.Error("[MODS]     Failed loading DLL " + text);
					Log.Exception(ex);
					return Mod.EModLoadState.FailedLoadingAssembly;
				}
			}
		}
		return Mod.EModLoadState.Success;
	}

	// Token: 0x060037AF RID: 14255 RVA: 0x0016E2A0 File Offset: 0x0016C4A0
	[PublicizedFrom(EAccessModifier.Private)]
	public static Assembly loadAssembly(string _path)
	{
		if (GameManager.IsDedicatedServer)
		{
			RuntimePlatform platform = Application.platform;
			if (platform == RuntimePlatform.WindowsServer || platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.WindowsEditor)
			{
				return Assembly.Load(SdFile.ReadAllBytes(_path));
			}
		}
		return Assembly.LoadFrom(_path);
	}

	// Token: 0x060037B0 RID: 14256 RVA: 0x0016E2DC File Offset: 0x0016C4DC
	public bool InitModCode()
	{
		if (this.allAssemblies.Count > 0)
		{
			Log.Out("[MODS]   Initializing mod " + this.Name);
			bool flag = false;
			Type typeFromHandle = typeof(IModApi);
			foreach (Assembly assembly in this.allAssemblies)
			{
				try
				{
					foreach (Type type in assembly.GetTypes())
					{
						if (typeFromHandle.IsAssignableFrom(type))
						{
							Log.Out("[MODS]     Found ModAPI in assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, null) + ", creating instance");
							IModApi modApi = (IModApi)Activator.CreateInstance(type);
							try
							{
								modApi.InitMod(this);
								Log.Out("[MODS]     Initialized code in mod '" + this.Name + "' in assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, null));
							}
							catch (Exception ex)
							{
								Log.Error("[MODS]     Failed initializing ModAPI instance on mod '" + this.Name + "' in assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, null));
								Log.Exception(ex);
							}
							flag = true;
						}
					}
				}
				catch (ReflectionTypeLoadException)
				{
					Log.Warning("[MODS]     Failed iterating types in assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, null));
				}
				catch (Exception ex2)
				{
					Log.Error("[MODS]     Failed creating ModAPI instance from assembly " + ReflectionHelpers.GetAssemblyNameWithLocation(assembly, null));
					Log.Exception(ex2);
					return false;
				}
			}
			if (!flag)
			{
				Log.Out("[MODS]     No ModAPI found in mod DLLs");
				return true;
			}
			return true;
		}
		return true;
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x0016E4B4 File Offset: 0x0016C6B4
	public bool ContainsAssembly(Assembly _assembly)
	{
		return this.allAssemblies.Contains(_assembly);
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x0016E4C4 File Offset: 0x0016C6C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void DetectContents()
	{
		string text = this.Path + "/Config";
		if (!SdDirectory.Exists(text))
		{
			return;
		}
		string[] fileSystemEntries = SdDirectory.GetFileSystemEntries(text);
		for (int i = 0; i < fileSystemEntries.Length; i++)
		{
			string fileName = global::System.IO.Path.GetFileName(fileSystemEntries[i]);
			if (!fileName.EqualsCaseInsensitive("XUi_Menu") && !fileName.EqualsCaseInsensitive("loadingscreen.xml") && !fileName.EqualsCaseInsensitive("Localization.csv"))
			{
				this.GameConfigMod = true;
				return;
			}
		}
	}

	// Token: 0x060037B3 RID: 14259 RVA: 0x0016E538 File Offset: 0x0016C738
	public static Mod LoadDefinitionFromFolder(string _path)
	{
		string text = _path + "/ModInfo.xml";
		string fileName = global::System.IO.Path.GetFileName(_path);
		if (!SdFile.Exists(text))
		{
			Log.Warning("[MODS]     Folder " + fileName + " does not contain a ModInfo.xml, ignoring");
			return null;
		}
		XmlFile xmlFile = new XmlFile(_path, "ModInfo.xml", false, false);
		XElement root = xmlFile.XmlDoc.Root;
		if (root == null)
		{
			Log.Error("[MODS]     " + fileName + "/ModInfo.xml does not have a root element, ignoring");
			return null;
		}
		Mod mod = ((root.Element("ModInfo") != null) ? Mod.parseModInfoV1(_path, fileName, text, xmlFile) : Mod.parseModInfoV2(_path, fileName, root));
		if (mod == null)
		{
			Log.Error("[MODS]     Could not parse " + fileName + "/ModInfo.xml, ignoring");
			return null;
		}
		return mod;
	}

	// Token: 0x060037B4 RID: 14260 RVA: 0x0016E5F0 File Offset: 0x0016C7F0
	[PublicizedFrom(EAccessModifier.Private)]
	public static Mod parseModInfoV2(string _modPath, string _folderName, XElement _xmlRoot)
	{
		string elementAttributeValue = Mod.getElementAttributeValue(_folderName, _xmlRoot, "Name", true);
		if (elementAttributeValue == null)
		{
			return null;
		}
		if (elementAttributeValue.Length == 0)
		{
			Log.Error("[MODS]     " + _folderName + "/ModInfo.xml does not specify a non-empty Name, ignoring");
			return null;
		}
		if (!Mod.nameValidationRegex.IsMatch(elementAttributeValue))
		{
			Log.Error(string.Format("[MODS]     {0}/ModInfo.xml does not define a valid non-empty Name ({1}), ignoring", _folderName, Mod.nameValidationRegex));
			return null;
		}
		Version version = null;
		string text = Mod.getElementAttributeValue(_folderName, _xmlRoot, "Version", true);
		if (text != null)
		{
			if (text.Length == 0)
			{
				text = null;
			}
			else
			{
				Version.TryParse(text, out version);
			}
		}
		if (version == null)
		{
			Log.Warning("[MODS]     " + _folderName + "/ModInfo.xml does not define a valid Version. Please consider updating it for future compatibility.");
		}
		string elementAttributeValue2 = Mod.getElementAttributeValue(_folderName, _xmlRoot, "DisplayName", false);
		if (string.IsNullOrEmpty(elementAttributeValue2))
		{
			Log.Error("[MODS]     " + _folderName + "/ModInfo.xml does not define a non-empty DisplayName, ignoring");
			return null;
		}
		string elementAttributeValue3 = Mod.getElementAttributeValue(_folderName, _xmlRoot, "Description", false);
		string elementAttributeValue4 = Mod.getElementAttributeValue(_folderName, _xmlRoot, "Author", false);
		string elementAttributeValue5 = Mod.getElementAttributeValue(_folderName, _xmlRoot, "Website", false);
		string elementAttributeValue6 = Mod.getElementAttributeValue(_folderName, _xmlRoot, "SkipWithAntiCheat", false);
		bool flag = false;
		if (!string.IsNullOrEmpty(elementAttributeValue6) && !StringParsers.TryParseBool(elementAttributeValue6, out flag))
		{
			Log.Warning("[MODS]     " + _folderName + "/ModInfo.xml does have a SkipWithAntiCheat, but its value is not a valid boolean. Assuming 'false'");
			flag = false;
		}
		return new Mod
		{
			Path = _modPath,
			FolderName = _folderName,
			Name = elementAttributeValue,
			DisplayName = elementAttributeValue2,
			Description = elementAttributeValue3,
			Author = elementAttributeValue4,
			Version = version,
			VersionString = text,
			Website = elementAttributeValue5,
			SkipLoadingWithAntiCheat = flag
		};
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x0016E780 File Offset: 0x0016C980
	[PublicizedFrom(EAccessModifier.Private)]
	public static string getElementAttributeValue(string _folderName, XElement _xmlParent, string _elementName, bool _logNonExisting = true)
	{
		List<XElement> list = _xmlParent.Elements(_elementName).ToList<XElement>();
		if (list.Count != 1)
		{
			if (_logNonExisting)
			{
				Log.Error(string.Concat(new string[] { "[MODS] ", _folderName, "/ModInfo.xml does not have exactly one '", _elementName, "' element, ignoring" }));
			}
			return null;
		}
		XAttribute xattribute = list[0].Attribute("value");
		if (xattribute == null)
		{
			Log.Error(string.Concat(new string[] { "[MODS] ", _folderName, "/ModInfo.xml '", _elementName, "' element does not have a 'value' attribute, ignoring" }));
			return null;
		}
		return xattribute.Value;
	}

	// Token: 0x060037B6 RID: 14262 RVA: 0x0016E82F File Offset: 0x0016CA2F
	[PublicizedFrom(EAccessModifier.Private)]
	public static Mod parseModInfoV1(string _modPath, string _folderName, string _modInfoFilename, XmlFile _xml)
	{
		Log.Error("[MODS]     " + _folderName + "/ModInfo.xml in legacy format. V2 required to load mod");
		return null;
	}

	// Token: 0x060037B7 RID: 14263 RVA: 0x0016E847 File Offset: 0x0016CA47
	public override string ToString()
	{
		return this.DisplayName;
	}

	// Token: 0x04002D4A RID: 11594
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Assembly> allAssemblies = new List<Assembly>();

	// Token: 0x04002D4B RID: 11595
	public readonly ReadOnlyCollection<Assembly> AllAssemblies;

	// Token: 0x04002D4D RID: 11597
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex nameValidationRegex = new Regex("^[0-9a-zA-Z_\\-]+$", RegexOptions.Compiled);

	// Token: 0x0200072A RID: 1834
	public enum EModLoadState
	{
		// Token: 0x04002D4F RID: 11599
		LoadNotRequested,
		// Token: 0x04002D50 RID: 11600
		Success,
		// Token: 0x04002D51 RID: 11601
		NotAntiCheatCompatible,
		// Token: 0x04002D52 RID: 11602
		SkippedDueToAntiCheat,
		// Token: 0x04002D53 RID: 11603
		DuplicateModName,
		// Token: 0x04002D54 RID: 11604
		FailedLoadingAssembly,
		// Token: 0x04002D55 RID: 11605
		Failed
	}
}
