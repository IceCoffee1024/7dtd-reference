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

// Token: 0x02000731 RID: 1841
[JsonObject(MemberSerialization.OptIn)]
public class Mod
{
	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x060037A9 RID: 14249 RVA: 0x0016EF91 File Offset: 0x0016D191
	// (set) Token: 0x060037AA RID: 14250 RVA: 0x0016EF99 File Offset: 0x0016D199
	public Mod.EModLoadState LoadState
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x060037AB RID: 14251 RVA: 0x0016EFA2 File Offset: 0x0016D1A2
	// (set) Token: 0x060037AC RID: 14252 RVA: 0x0016EFAA File Offset: 0x0016D1AA
	public string Path
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x060037AD RID: 14253 RVA: 0x0016EFB3 File Offset: 0x0016D1B3
	// (set) Token: 0x060037AE RID: 14254 RVA: 0x0016EFBB File Offset: 0x0016D1BB
	public string FolderName
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x060037AF RID: 14255 RVA: 0x0016EFC4 File Offset: 0x0016D1C4
	// (set) Token: 0x060037B0 RID: 14256 RVA: 0x0016EFCC File Offset: 0x0016D1CC
	[JsonProperty]
	[JsonConverter(typeof(TruncateStringSerializerConverter))]
	public string Name
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700059D RID: 1437
	// (get) Token: 0x060037B1 RID: 14257 RVA: 0x0016EFD5 File Offset: 0x0016D1D5
	// (set) Token: 0x060037B2 RID: 14258 RVA: 0x0016EFDD File Offset: 0x0016D1DD
	[JsonProperty]
	[JsonConverter(typeof(TruncateStringSerializerConverter))]
	public string DisplayName
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x060037B3 RID: 14259 RVA: 0x0016EFE6 File Offset: 0x0016D1E6
	// (set) Token: 0x060037B4 RID: 14260 RVA: 0x0016EFEE File Offset: 0x0016D1EE
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

	// Token: 0x1700059F RID: 1439
	// (get) Token: 0x060037B5 RID: 14261 RVA: 0x0016EFF7 File Offset: 0x0016D1F7
	// (set) Token: 0x060037B6 RID: 14262 RVA: 0x0016EFFF File Offset: 0x0016D1FF
	public string Author
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x060037B7 RID: 14263 RVA: 0x0016F008 File Offset: 0x0016D208
	// (set) Token: 0x060037B8 RID: 14264 RVA: 0x0016F010 File Offset: 0x0016D210
	public Version Version
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170005A1 RID: 1441
	// (get) Token: 0x060037B9 RID: 14265 RVA: 0x0016F019 File Offset: 0x0016D219
	// (set) Token: 0x060037BA RID: 14266 RVA: 0x0016F021 File Offset: 0x0016D221
	public string VersionString
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170005A2 RID: 1442
	// (get) Token: 0x060037BB RID: 14267 RVA: 0x0016F02A File Offset: 0x0016D22A
	// (set) Token: 0x060037BC RID: 14268 RVA: 0x0016F032 File Offset: 0x0016D232
	public string Website
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x060037BD RID: 14269 RVA: 0x0016F03B File Offset: 0x0016D23B
	// (set) Token: 0x060037BE RID: 14270 RVA: 0x0016F043 File Offset: 0x0016D243
	public bool SkipLoadingWithAntiCheat
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x060037BF RID: 14271 RVA: 0x0016F04C File Offset: 0x0016D24C
	// (set) Token: 0x060037C0 RID: 14272 RVA: 0x0016F054 File Offset: 0x0016D254
	public bool AntiCheatCompatible
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x060037C1 RID: 14273 RVA: 0x0016F05D File Offset: 0x0016D25D
	// (set) Token: 0x060037C2 RID: 14274 RVA: 0x0016F065 File Offset: 0x0016D265
	public bool GameConfigMod
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060037C3 RID: 14275 RVA: 0x0016F06E File Offset: 0x0016D26E
	[PublicizedFrom(EAccessModifier.Private)]
	public Mod()
	{
		this.AllAssemblies = new ReadOnlyCollection<Assembly>(this.allAssemblies);
	}

	// Token: 0x060037C4 RID: 14276 RVA: 0x0016F094 File Offset: 0x0016D294
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

	// Token: 0x060037C5 RID: 14277 RVA: 0x0016F148 File Offset: 0x0016D348
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

	// Token: 0x060037C6 RID: 14278 RVA: 0x0016F238 File Offset: 0x0016D438
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

	// Token: 0x060037C7 RID: 14279 RVA: 0x0016F274 File Offset: 0x0016D474
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

	// Token: 0x060037C8 RID: 14280 RVA: 0x0016F44C File Offset: 0x0016D64C
	public bool ContainsAssembly(Assembly _assembly)
	{
		return this.allAssemblies.Contains(_assembly);
	}

	// Token: 0x060037C9 RID: 14281 RVA: 0x0016F45C File Offset: 0x0016D65C
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

	// Token: 0x060037CA RID: 14282 RVA: 0x0016F4D0 File Offset: 0x0016D6D0
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

	// Token: 0x060037CB RID: 14283 RVA: 0x0016F588 File Offset: 0x0016D788
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

	// Token: 0x060037CC RID: 14284 RVA: 0x0016F718 File Offset: 0x0016D918
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

	// Token: 0x060037CD RID: 14285 RVA: 0x0016F7C7 File Offset: 0x0016D9C7
	[PublicizedFrom(EAccessModifier.Private)]
	public static Mod parseModInfoV1(string _modPath, string _folderName, string _modInfoFilename, XmlFile _xml)
	{
		Log.Error("[MODS]     " + _folderName + "/ModInfo.xml in legacy format. V2 required to load mod");
		return null;
	}

	// Token: 0x060037CE RID: 14286 RVA: 0x0016F7DF File Offset: 0x0016D9DF
	public override string ToString()
	{
		return this.DisplayName;
	}

	// Token: 0x04002D66 RID: 11622
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Assembly> allAssemblies = new List<Assembly>();

	// Token: 0x04002D67 RID: 11623
	public readonly ReadOnlyCollection<Assembly> AllAssemblies;

	// Token: 0x04002D69 RID: 11625
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex nameValidationRegex = new Regex("^[0-9a-zA-Z_\\-]+$", RegexOptions.Compiled);

	// Token: 0x02000732 RID: 1842
	public enum EModLoadState
	{
		// Token: 0x04002D6B RID: 11627
		LoadNotRequested,
		// Token: 0x04002D6C RID: 11628
		Success,
		// Token: 0x04002D6D RID: 11629
		NotAntiCheatCompatible,
		// Token: 0x04002D6E RID: 11630
		SkippedDueToAntiCheat,
		// Token: 0x04002D6F RID: 11631
		DuplicateModName,
		// Token: 0x04002D70 RID: 11632
		FailedLoadingAssembly,
		// Token: 0x04002D71 RID: 11633
		Failed
	}
}
