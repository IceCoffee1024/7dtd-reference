using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Platform;
using UnityEngine;

// Token: 0x0200071D RID: 1821
[JsonObject(MemberSerialization.OptIn)]
public class Mod
{
	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x060036F9 RID: 14073 RVA: 0x0016B479 File Offset: 0x00169679
	// (set) Token: 0x060036FA RID: 14074 RVA: 0x0016B481 File Offset: 0x00169681
	public Mod.EModLoadState LoadState
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x060036FB RID: 14075 RVA: 0x0016B48A File Offset: 0x0016968A
	// (set) Token: 0x060036FC RID: 14076 RVA: 0x0016B492 File Offset: 0x00169692
	public string Path
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x060036FD RID: 14077 RVA: 0x0016B49B File Offset: 0x0016969B
	// (set) Token: 0x060036FE RID: 14078 RVA: 0x0016B4A3 File Offset: 0x001696A3
	public string FolderName
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x060036FF RID: 14079 RVA: 0x0016B4AC File Offset: 0x001696AC
	// (set) Token: 0x06003700 RID: 14080 RVA: 0x0016B4B4 File Offset: 0x001696B4
	[JsonProperty]
	public string Name
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700056A RID: 1386
	// (get) Token: 0x06003701 RID: 14081 RVA: 0x0016B4BD File Offset: 0x001696BD
	// (set) Token: 0x06003702 RID: 14082 RVA: 0x0016B4C5 File Offset: 0x001696C5
	[JsonProperty]
	public string DisplayName
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700056B RID: 1387
	// (get) Token: 0x06003703 RID: 14083 RVA: 0x0016B4CE File Offset: 0x001696CE
	// (set) Token: 0x06003704 RID: 14084 RVA: 0x0016B4D6 File Offset: 0x001696D6
	[JsonProperty]
	public string Description
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x06003705 RID: 14085 RVA: 0x0016B4DF File Offset: 0x001696DF
	// (set) Token: 0x06003706 RID: 14086 RVA: 0x0016B4E7 File Offset: 0x001696E7
	[JsonProperty]
	public string Author
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x06003707 RID: 14087 RVA: 0x0016B4F0 File Offset: 0x001696F0
	// (set) Token: 0x06003708 RID: 14088 RVA: 0x0016B4F8 File Offset: 0x001696F8
	public Version Version
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06003709 RID: 14089 RVA: 0x0016B501 File Offset: 0x00169701
	// (set) Token: 0x0600370A RID: 14090 RVA: 0x0016B509 File Offset: 0x00169709
	[JsonProperty]
	public string VersionString
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x0600370B RID: 14091 RVA: 0x0016B512 File Offset: 0x00169712
	// (set) Token: 0x0600370C RID: 14092 RVA: 0x0016B51A File Offset: 0x0016971A
	[JsonProperty]
	public string Website
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x0600370D RID: 14093 RVA: 0x0016B523 File Offset: 0x00169723
	// (set) Token: 0x0600370E RID: 14094 RVA: 0x0016B52B File Offset: 0x0016972B
	[JsonProperty]
	public bool SkipLoadingWithAntiCheat
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x0600370F RID: 14095 RVA: 0x0016B534 File Offset: 0x00169734
	// (set) Token: 0x06003710 RID: 14096 RVA: 0x0016B53C File Offset: 0x0016973C
	[JsonProperty]
	public bool AntiCheatCompatible
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06003711 RID: 14097 RVA: 0x0016B545 File Offset: 0x00169745
	// (set) Token: 0x06003712 RID: 14098 RVA: 0x0016B54D File Offset: 0x0016974D
	[JsonProperty]
	public bool GameConfigMod
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003713 RID: 14099 RVA: 0x0016B556 File Offset: 0x00169756
	[PublicizedFrom(EAccessModifier.Private)]
	public Mod()
	{
		this.AllAssemblies = new ReadOnlyCollection<Assembly>(this.allAssemblies);
	}

	// Token: 0x06003714 RID: 14100 RVA: 0x0016B57C File Offset: 0x0016977C
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

	// Token: 0x06003715 RID: 14101 RVA: 0x0016B630 File Offset: 0x00169830
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

	// Token: 0x06003716 RID: 14102 RVA: 0x0016B720 File Offset: 0x00169920
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

	// Token: 0x06003717 RID: 14103 RVA: 0x0016B75C File Offset: 0x0016995C
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

	// Token: 0x06003718 RID: 14104 RVA: 0x0016B934 File Offset: 0x00169B34
	public bool ContainsAssembly(Assembly _assembly)
	{
		return this.allAssemblies.Contains(_assembly);
	}

	// Token: 0x06003719 RID: 14105 RVA: 0x0016B944 File Offset: 0x00169B44
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

	// Token: 0x0600371A RID: 14106 RVA: 0x0016B9B8 File Offset: 0x00169BB8
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

	// Token: 0x0600371B RID: 14107 RVA: 0x0016BA70 File Offset: 0x00169C70
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

	// Token: 0x0600371C RID: 14108 RVA: 0x0016BC00 File Offset: 0x00169E00
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

	// Token: 0x0600371D RID: 14109 RVA: 0x0016BCAF File Offset: 0x00169EAF
	[PublicizedFrom(EAccessModifier.Private)]
	public static Mod parseModInfoV1(string _modPath, string _folderName, string _modInfoFilename, XmlFile _xml)
	{
		Log.Error("[MODS]     " + _folderName + "/ModInfo.xml in legacy format. V2 required to load mod");
		return null;
	}

	// Token: 0x0600371E RID: 14110 RVA: 0x0016BCC7 File Offset: 0x00169EC7
	public override string ToString()
	{
		return this.DisplayName;
	}

	// Token: 0x04002CD9 RID: 11481
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Assembly> allAssemblies = new List<Assembly>();

	// Token: 0x04002CDA RID: 11482
	public readonly ReadOnlyCollection<Assembly> AllAssemblies;

	// Token: 0x04002CDC RID: 11484
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex nameValidationRegex = new Regex("^[0-9a-zA-Z_\\-]+$", RegexOptions.Compiled);

	// Token: 0x0200071E RID: 1822
	public enum EModLoadState
	{
		// Token: 0x04002CDE RID: 11486
		LoadNotRequested,
		// Token: 0x04002CDF RID: 11487
		Success,
		// Token: 0x04002CE0 RID: 11488
		NotAntiCheatCompatible,
		// Token: 0x04002CE1 RID: 11489
		SkippedDueToAntiCheat,
		// Token: 0x04002CE2 RID: 11490
		DuplicateModName,
		// Token: 0x04002CE3 RID: 11491
		FailedLoadingAssembly,
		// Token: 0x04002CE4 RID: 11492
		Failed
	}
}
