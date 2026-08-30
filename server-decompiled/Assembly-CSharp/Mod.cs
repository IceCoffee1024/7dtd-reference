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

// Token: 0x02000727 RID: 1831
[JsonObject(MemberSerialization.OptIn)]
public class Mod
{
	// Token: 0x1700056F RID: 1391
	// (get) Token: 0x0600374F RID: 14159 RVA: 0x0016D695 File Offset: 0x0016B895
	// (set) Token: 0x06003750 RID: 14160 RVA: 0x0016D69D File Offset: 0x0016B89D
	public Mod.EModLoadState LoadState
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000570 RID: 1392
	// (get) Token: 0x06003751 RID: 14161 RVA: 0x0016D6A6 File Offset: 0x0016B8A6
	// (set) Token: 0x06003752 RID: 14162 RVA: 0x0016D6AE File Offset: 0x0016B8AE
	public string Path
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000571 RID: 1393
	// (get) Token: 0x06003753 RID: 14163 RVA: 0x0016D6B7 File Offset: 0x0016B8B7
	// (set) Token: 0x06003754 RID: 14164 RVA: 0x0016D6BF File Offset: 0x0016B8BF
	public string FolderName
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06003755 RID: 14165 RVA: 0x0016D6C8 File Offset: 0x0016B8C8
	// (set) Token: 0x06003756 RID: 14166 RVA: 0x0016D6D0 File Offset: 0x0016B8D0
	[JsonProperty]
	[JsonConverter(typeof(TruncateStringSerializerConverter))]
	public string Name
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000573 RID: 1395
	// (get) Token: 0x06003757 RID: 14167 RVA: 0x0016D6D9 File Offset: 0x0016B8D9
	// (set) Token: 0x06003758 RID: 14168 RVA: 0x0016D6E1 File Offset: 0x0016B8E1
	[JsonProperty]
	[JsonConverter(typeof(TruncateStringSerializerConverter))]
	public string DisplayName
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000574 RID: 1396
	// (get) Token: 0x06003759 RID: 14169 RVA: 0x0016D6EA File Offset: 0x0016B8EA
	// (set) Token: 0x0600375A RID: 14170 RVA: 0x0016D6F2 File Offset: 0x0016B8F2
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

	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x0600375B RID: 14171 RVA: 0x0016D6FB File Offset: 0x0016B8FB
	// (set) Token: 0x0600375C RID: 14172 RVA: 0x0016D703 File Offset: 0x0016B903
	public string Author
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x0600375D RID: 14173 RVA: 0x0016D70C File Offset: 0x0016B90C
	// (set) Token: 0x0600375E RID: 14174 RVA: 0x0016D714 File Offset: 0x0016B914
	public Version Version
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x0600375F RID: 14175 RVA: 0x0016D71D File Offset: 0x0016B91D
	// (set) Token: 0x06003760 RID: 14176 RVA: 0x0016D725 File Offset: 0x0016B925
	public string VersionString
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06003761 RID: 14177 RVA: 0x0016D72E File Offset: 0x0016B92E
	// (set) Token: 0x06003762 RID: 14178 RVA: 0x0016D736 File Offset: 0x0016B936
	public string Website
	{
		[UsedImplicitly]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06003763 RID: 14179 RVA: 0x0016D73F File Offset: 0x0016B93F
	// (set) Token: 0x06003764 RID: 14180 RVA: 0x0016D747 File Offset: 0x0016B947
	public bool SkipLoadingWithAntiCheat
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06003765 RID: 14181 RVA: 0x0016D750 File Offset: 0x0016B950
	// (set) Token: 0x06003766 RID: 14182 RVA: 0x0016D758 File Offset: 0x0016B958
	public bool AntiCheatCompatible
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x06003767 RID: 14183 RVA: 0x0016D761 File Offset: 0x0016B961
	// (set) Token: 0x06003768 RID: 14184 RVA: 0x0016D769 File Offset: 0x0016B969
	public bool GameConfigMod
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003769 RID: 14185 RVA: 0x0016D772 File Offset: 0x0016B972
	[PublicizedFrom(EAccessModifier.Private)]
	public Mod()
	{
		this.AllAssemblies = new ReadOnlyCollection<Assembly>(this.allAssemblies);
	}

	// Token: 0x0600376A RID: 14186 RVA: 0x0016D798 File Offset: 0x0016B998
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

	// Token: 0x0600376B RID: 14187 RVA: 0x0016D84C File Offset: 0x0016BA4C
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

	// Token: 0x0600376C RID: 14188 RVA: 0x0016D93C File Offset: 0x0016BB3C
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

	// Token: 0x0600376D RID: 14189 RVA: 0x0016D978 File Offset: 0x0016BB78
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

	// Token: 0x0600376E RID: 14190 RVA: 0x0016DB50 File Offset: 0x0016BD50
	public bool ContainsAssembly(Assembly _assembly)
	{
		return this.allAssemblies.Contains(_assembly);
	}

	// Token: 0x0600376F RID: 14191 RVA: 0x0016DB60 File Offset: 0x0016BD60
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

	// Token: 0x06003770 RID: 14192 RVA: 0x0016DBD4 File Offset: 0x0016BDD4
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

	// Token: 0x06003771 RID: 14193 RVA: 0x0016DC8C File Offset: 0x0016BE8C
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

	// Token: 0x06003772 RID: 14194 RVA: 0x0016DE1C File Offset: 0x0016C01C
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

	// Token: 0x06003773 RID: 14195 RVA: 0x0016DECB File Offset: 0x0016C0CB
	[PublicizedFrom(EAccessModifier.Private)]
	public static Mod parseModInfoV1(string _modPath, string _folderName, string _modInfoFilename, XmlFile _xml)
	{
		Log.Error("[MODS]     " + _folderName + "/ModInfo.xml in legacy format. V2 required to load mod");
		return null;
	}

	// Token: 0x06003774 RID: 14196 RVA: 0x0016DEE3 File Offset: 0x0016C0E3
	public override string ToString()
	{
		return this.DisplayName;
	}

	// Token: 0x04002D51 RID: 11601
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Assembly> allAssemblies = new List<Assembly>();

	// Token: 0x04002D52 RID: 11602
	public readonly ReadOnlyCollection<Assembly> AllAssemblies;

	// Token: 0x04002D54 RID: 11604
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex nameValidationRegex = new Regex("^[0-9a-zA-Z_\\-]+$", RegexOptions.Compiled);

	// Token: 0x02000728 RID: 1832
	public enum EModLoadState
	{
		// Token: 0x04002D56 RID: 11606
		LoadNotRequested,
		// Token: 0x04002D57 RID: 11607
		Success,
		// Token: 0x04002D58 RID: 11608
		NotAntiCheatCompatible,
		// Token: 0x04002D59 RID: 11609
		SkippedDueToAntiCheat,
		// Token: 0x04002D5A RID: 11610
		DuplicateModName,
		// Token: 0x04002D5B RID: 11611
		FailedLoadingAssembly,
		// Token: 0x04002D5C RID: 11612
		Failed
	}
}
