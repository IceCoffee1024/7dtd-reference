using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x0200149F RID: 5279
public static class PathAbstractions
{
	// Token: 0x0600A51C RID: 42268 RVA: 0x003D9E30 File Offset: 0x003D8030
	public static void InvalidateCaches()
	{
		foreach (PathAbstractions.SearchDefinition searchDefinition in PathAbstractions.allSearchDefs)
		{
			searchDefinition.InvalidateCache();
		}
	}

	// Token: 0x170013A3 RID: 5027
	// (get) Token: 0x0600A51D RID: 42269 RVA: 0x003D9E80 File Offset: 0x003D8080
	// (set) Token: 0x0600A51E RID: 42270 RVA: 0x003D9E87 File Offset: 0x003D8087
	public static bool CacheEnabled
	{
		get
		{
			return PathAbstractions.cacheEnabled;
		}
		set
		{
			PathAbstractions.cacheEnabled = value;
			if (!value)
			{
				PathAbstractions.InvalidateCaches();
			}
		}
	}

	// Token: 0x04007B70 RID: 31600
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<PathAbstractions.SearchDefinition> allSearchDefs = new List<PathAbstractions.SearchDefinition>();

	// Token: 0x04007B71 RID: 31601
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<string> deviceLocalUserDataPath = new Func<string>(GameIO.GetDeviceLocalUserGameDataDir);

	// Token: 0x04007B72 RID: 31602
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<string> gameDataPath = new Func<string>(GameIO.GetApplicationPath);

	// Token: 0x04007B73 RID: 31603
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<UserDataStorageType> getWorldsStorageType = () => (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType);

	// Token: 0x04007B74 RID: 31604
	public static readonly PathAbstractions.SearchDefinition WorldsSearchPaths = new PathAbstractions.SearchDefinition(true, null, false, false, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathSaves("World"),
		new PathAbstractions.SearchPathUserData("GeneratedWorlds", PathAbstractions.getWorldsStorageType),
		new PathAbstractions.SearchPathMods("Worlds"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Worlds", null)
	});

	// Token: 0x04007B75 RID: 31605
	public static readonly PathAbstractions.SearchDefinition PrefabsSearchPaths = new PathAbstractions.SearchDefinition(false, ".tts", true, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalPrefabs", null),
		new PathAbstractions.SearchPathMods("Prefabs"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Prefabs", null)
	});

	// Token: 0x04007B76 RID: 31606
	public static readonly PathAbstractions.SearchDefinition PrefabImpostersSearchPaths = new PathAbstractions.SearchDefinition(false, ".mesh", false, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalPrefabs", null),
		new PathAbstractions.SearchPathMods("Prefabs"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Prefabs", null)
	});

	// Token: 0x04007B77 RID: 31607
	public static readonly PathAbstractions.SearchDefinition RwgStampsSearchPaths = new PathAbstractions.SearchDefinition(false, "", true, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalStamps", null),
		new PathAbstractions.SearchPathMods("Stamps"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Stamps", null)
	});

	// Token: 0x04007B78 RID: 31608
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool cacheEnabled;

	// Token: 0x020014A0 RID: 5280
	public enum EAbstractedLocationType
	{
		// Token: 0x04007B7A RID: 31610
		HostSave,
		// Token: 0x04007B7B RID: 31611
		LocalSave,
		// Token: 0x04007B7C RID: 31612
		UserDataPath,
		// Token: 0x04007B7D RID: 31613
		Mods,
		// Token: 0x04007B7E RID: 31614
		GameData,
		// Token: 0x04007B7F RID: 31615
		None
	}

	// Token: 0x020014A1 RID: 5281
	public readonly struct AbstractedLocation : IEquatable<PathAbstractions.AbstractedLocation>, IComparable<PathAbstractions.AbstractedLocation>, IComparable
	{
		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x0600A520 RID: 42272 RVA: 0x003DA021 File Offset: 0x003D8221
		public string FullPath
		{
			get
			{
				return this.Folder + "/" + this.FileNameNoExtension + this.Extension;
			}
		}

		// Token: 0x170013A5 RID: 5029
		// (get) Token: 0x0600A521 RID: 42273 RVA: 0x003DA03F File Offset: 0x003D823F
		public string FullPathNoExtension
		{
			get
			{
				return this.Folder + "/" + this.FileNameNoExtension;
			}
		}

		// Token: 0x0600A522 RID: 42274 RVA: 0x003DA058 File Offset: 0x003D8258
		public AbstractedLocation(PathAbstractions.EAbstractedLocationType _type, string _name, string _fullPath, string _relativePath, bool _isFolder, Mod _containingMod = null)
		{
			_fullPath = ((_fullPath != null) ? Path.GetFullPath(_fullPath).Replace("\\", "/") : null);
			this.Type = _type;
			this.Name = Path.GetFileName(_name);
			string directoryName = Path.GetDirectoryName(_fullPath);
			this.Folder = ((directoryName != null) ? directoryName.Replace("\\", "/") : null);
			this.RelativePath = _relativePath;
			this.FileNameNoExtension = Path.GetFileNameWithoutExtension(_fullPath);
			this.Extension = Path.GetExtension(_fullPath);
			this.Extension = (string.IsNullOrEmpty(this.Extension) ? null : this.Extension);
			this.IsFolder = _isFolder;
			this.ContainingMod = _containingMod;
			this.StorageType = ((this.Folder != null && GameIO.IsRoamingUserDataPath(this.Folder)) ? UserDataStorageType.Roaming : UserDataStorageType.DeviceLocal);
		}

		// Token: 0x0600A523 RID: 42275 RVA: 0x003DA124 File Offset: 0x003D8324
		public AbstractedLocation(PathAbstractions.EAbstractedLocationType _type, string _name, string _folder, string _relativePath, string _fileNameNoExtension, string _extension, bool _isFolder, Mod _containingMod = null)
		{
			this.Type = _type;
			this.Name = _name;
			this.Folder = ((_folder != null) ? Path.GetFullPath(_folder).Replace("\\", "/") : null);
			this.RelativePath = _relativePath;
			this.FileNameNoExtension = _fileNameNoExtension;
			this.Extension = _extension;
			this.IsFolder = _isFolder;
			this.ContainingMod = _containingMod;
			this.StorageType = ((this.Folder != null && GameIO.IsRoamingUserDataPath(this.Folder)) ? UserDataStorageType.Roaming : UserDataStorageType.DeviceLocal);
		}

		// Token: 0x0600A524 RID: 42276 RVA: 0x003DA1A7 File Offset: 0x003D83A7
		public bool Exists()
		{
			if (this.Type == PathAbstractions.EAbstractedLocationType.None)
			{
				return false;
			}
			if (!this.IsFolder)
			{
				return SdFile.Exists(this.FullPath);
			}
			return SdDirectory.Exists(this.FullPath);
		}

		// Token: 0x0600A525 RID: 42277 RVA: 0x003DA1D4 File Offset: 0x003D83D4
		public bool Equals(PathAbstractions.AbstractedLocation _other)
		{
			return this.Type == _other.Type && (this.Type == PathAbstractions.EAbstractedLocationType.None || (this.IsFolder == _other.IsFolder && string.Equals(this.Name, _other.Name) && GameIO.PathsEquals(this.FullPath, _other.FullPath, true)));
		}

		// Token: 0x0600A526 RID: 42278 RVA: 0x003DA234 File Offset: 0x003D8434
		public override bool Equals(object _obj)
		{
			if (_obj == null)
			{
				return false;
			}
			if (_obj is PathAbstractions.AbstractedLocation)
			{
				PathAbstractions.AbstractedLocation abstractedLocation = (PathAbstractions.AbstractedLocation)_obj;
				return this.Equals(abstractedLocation);
			}
			return false;
		}

		// Token: 0x0600A527 RID: 42279 RVA: 0x003DA25E File Offset: 0x003D845E
		public static bool operator ==(PathAbstractions.AbstractedLocation _a, PathAbstractions.AbstractedLocation _b)
		{
			return _a.Equals(_b);
		}

		// Token: 0x0600A528 RID: 42280 RVA: 0x003DA268 File Offset: 0x003D8468
		public static bool operator !=(PathAbstractions.AbstractedLocation _a, PathAbstractions.AbstractedLocation _b)
		{
			return !(_a == _b);
		}

		// Token: 0x0600A529 RID: 42281 RVA: 0x003DA274 File Offset: 0x003D8474
		public override int GetHashCode()
		{
			return (((((this.Name != null) ? this.Name.GetHashCode() : 0) * 397) ^ (int)this.Type) * 397) ^ ((this.FullPath != null) ? this.FullPath.GetHashCode() : 0);
		}

		// Token: 0x0600A52A RID: 42282 RVA: 0x003DA2C4 File Offset: 0x003D84C4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.Name,
				" (src: ",
				this.Type.ToStringCached<PathAbstractions.EAbstractedLocationType>(),
				", ",
				this.StorageType.ToStringCached<UserDataStorageType>(),
				")"
			});
		}

		// Token: 0x0600A52B RID: 42283 RVA: 0x003DA31C File Offset: 0x003D851C
		public string GetLocationTypeDisplayString()
		{
			switch (this.Type)
			{
			case PathAbstractions.EAbstractedLocationType.UserDataPath:
				if (PlatformManager.MultiPlatform.UserDataRoaming.IsRoamingOptional)
				{
					return "User Data: " + this.StorageType.LocalizedName();
				}
				return "User Data";
			case PathAbstractions.EAbstractedLocationType.Mods:
				return "Mod: " + this.ContainingMod.Name;
			case PathAbstractions.EAbstractedLocationType.GameData:
				return "Vanilla";
			default:
				return this.Type.ToStringCached<PathAbstractions.EAbstractedLocationType>();
			}
		}

		// Token: 0x0600A52C RID: 42284 RVA: 0x003DA39C File Offset: 0x003D859C
		public int CompareTo(PathAbstractions.AbstractedLocation _other)
		{
			int num = string.Compare(this.Name, _other.Name, StringComparison.OrdinalIgnoreCase);
			if (num != 0)
			{
				return num;
			}
			int num2 = this.Type.CompareTo(_other.Type);
			if (num2 != 0)
			{
				return num2;
			}
			int num3 = string.Compare(this.FileNameNoExtension, _other.FileNameNoExtension, StringComparison.OrdinalIgnoreCase);
			if (num3 != 0)
			{
				return num3;
			}
			int num4 = string.Compare(this.Extension, _other.Extension, StringComparison.OrdinalIgnoreCase);
			if (num4 != 0)
			{
				return num4;
			}
			return string.Compare(this.Folder, _other.Folder, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600A52D RID: 42285 RVA: 0x003DA428 File Offset: 0x003D8628
		public int CompareTo(object _obj)
		{
			if (_obj == null)
			{
				return 1;
			}
			if (_obj is PathAbstractions.AbstractedLocation)
			{
				PathAbstractions.AbstractedLocation abstractedLocation = (PathAbstractions.AbstractedLocation)_obj;
				return this.CompareTo(abstractedLocation);
			}
			throw new ArgumentException("Object must be of type AbstractedLocation");
		}

		// Token: 0x04007B80 RID: 31616
		public static readonly PathAbstractions.AbstractedLocation None = new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.None, null, null, null, false, null);

		// Token: 0x04007B81 RID: 31617
		public readonly PathAbstractions.EAbstractedLocationType Type;

		// Token: 0x04007B82 RID: 31618
		public readonly string Name;

		// Token: 0x04007B83 RID: 31619
		public readonly string Folder;

		// Token: 0x04007B84 RID: 31620
		public readonly string RelativePath;

		// Token: 0x04007B85 RID: 31621
		public readonly string FileNameNoExtension;

		// Token: 0x04007B86 RID: 31622
		public readonly string Extension;

		// Token: 0x04007B87 RID: 31623
		public readonly bool IsFolder;

		// Token: 0x04007B88 RID: 31624
		public readonly Mod ContainingMod;

		// Token: 0x04007B89 RID: 31625
		public readonly UserDataStorageType StorageType;
	}

	// Token: 0x020014A2 RID: 5282
	public class SearchDefinition
	{
		// Token: 0x0600A52F RID: 42287 RVA: 0x003DA470 File Offset: 0x003D8670
		public SearchDefinition(bool _isFolder, string _extension, bool _removeExtension, bool _recursive, params PathAbstractions.SearchPath[] _paths)
		{
			this.IsFolder = _isFolder;
			this.Extension = _extension;
			this.RemoveExtension = _removeExtension;
			this.Recursive = _recursive;
			if (this.IsFolder && this.Recursive)
			{
				throw new Exception("SearchDefinition can not be set to target folders and search recursively at the same time!");
			}
			this.paths = new List<PathAbstractions.SearchPath>(_paths);
			for (int i = 0; i < _paths.Length; i++)
			{
				_paths[i].SetOwner(this);
			}
			PathAbstractions.allSearchDefs.Add(this);
		}

		// Token: 0x0600A530 RID: 42288 RVA: 0x003DA4F0 File Offset: 0x003D86F0
		public PathAbstractions.AbstractedLocation GetLocation(string _name, PathAbstractions.EAbstractedLocationType? _onlyLocationType = null, UserDataStorageType? _userDataHint = null)
		{
			foreach (PathAbstractions.SearchPath searchPath in this.paths)
			{
				if (searchPath.CanMatch && (_onlyLocationType == null || searchPath.CanHandleLocationType(_onlyLocationType.Value)))
				{
					PathAbstractions.AbstractedLocation location = searchPath.GetLocation(_name, _userDataHint);
					if (location.Type != PathAbstractions.EAbstractedLocationType.None)
					{
						return location;
					}
				}
			}
			return new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.None, _name, null, null, this.IsFolder, null);
		}

		// Token: 0x0600A531 RID: 42289 RVA: 0x003DA584 File Offset: 0x003D8784
		public List<PathAbstractions.AbstractedLocation> GetAvailablePathsList(Regex _nameMatch = null, bool _ignoreDuplicateNames = false, PathAbstractions.EAbstractedLocationType? _onlyLocationType = null)
		{
			List<PathAbstractions.AbstractedLocation> list = new List<PathAbstractions.AbstractedLocation>();
			this.GetAvailablePathsList(ref list, _nameMatch, _ignoreDuplicateNames, _onlyLocationType);
			return list;
		}

		// Token: 0x0600A532 RID: 42290 RVA: 0x003DA5A4 File Offset: 0x003D87A4
		public void GetAvailablePathsList(ref List<PathAbstractions.AbstractedLocation> _resultList, Regex _nameMatch = null, bool _ignoreDuplicateNames = false, PathAbstractions.EAbstractedLocationType? _onlyLocationType = null)
		{
			if (_resultList == null)
			{
				_resultList = new List<PathAbstractions.AbstractedLocation>();
			}
			foreach (PathAbstractions.SearchPath searchPath in this.paths)
			{
				if (searchPath.CanMatch && (_onlyLocationType == null || searchPath.CanHandleLocationType(_onlyLocationType.Value)))
				{
					searchPath.GetAvailablePathsList(_resultList, _nameMatch, _ignoreDuplicateNames);
				}
			}
		}

		// Token: 0x0600A533 RID: 42291 RVA: 0x003DA628 File Offset: 0x003D8828
		public PathAbstractions.AbstractedLocation? BuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, string _modName = null, UserDataStorageType? _userStorageHint = null)
		{
			Mod mod = null;
			if (_locationType == PathAbstractions.EAbstractedLocationType.Mods)
			{
				if (string.IsNullOrEmpty(_modName))
				{
					throw new ArgumentException("BuildLocation for LocationType==Mods requires a Mod name", "_modName");
				}
				mod = ModManager.GetMod(_modName, true);
				if (mod == null)
				{
					throw new ArgumentException("BuildLocation for LocationType==Mods requires the name of a loaded Mod", "_modName");
				}
			}
			return this.BuildLocation(_locationType, _subFolder, _elementName, mod, _userStorageHint);
		}

		// Token: 0x0600A534 RID: 42292 RVA: 0x003DA67C File Offset: 0x003D887C
		public PathAbstractions.AbstractedLocation? BuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod = null, UserDataStorageType? _userStorageHint = null)
		{
			foreach (PathAbstractions.SearchPath searchPath in this.paths)
			{
				if (searchPath.CanHandleLocationType(_locationType))
				{
					return searchPath.TryBuildLocation(_locationType, _subFolder, _elementName, _mod, _userStorageHint);
				}
			}
			return null;
		}

		// Token: 0x0600A535 RID: 42293 RVA: 0x003DA6EC File Offset: 0x003D88EC
		public string GetBasePath(PathAbstractions.EAbstractedLocationType _locationType, string _modName = null)
		{
			Mod mod = null;
			if (_locationType == PathAbstractions.EAbstractedLocationType.Mods)
			{
				if (string.IsNullOrEmpty(_modName))
				{
					throw new ArgumentException("GetBasePath for LocationType==Mods requires a Mod name", "_modName");
				}
				mod = ModManager.GetMod(_modName, true);
				if (mod == null)
				{
					throw new ArgumentException("GetBasePath for LocationType==Mods requires the name of a loaded Mod", "_modName");
				}
			}
			return this.GetBasePath(_locationType, mod);
		}

		// Token: 0x0600A536 RID: 42294 RVA: 0x003DA73C File Offset: 0x003D893C
		public string GetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod = null)
		{
			foreach (PathAbstractions.SearchPath searchPath in this.paths)
			{
				if (searchPath.CanHandleLocationType(_locationType))
				{
					return searchPath.TryGetBasePath(_locationType, _mod);
				}
			}
			return null;
		}

		// Token: 0x0600A537 RID: 42295 RVA: 0x003DA7A0 File Offset: 0x003D89A0
		public void InvalidateCache()
		{
			foreach (PathAbstractions.SearchPath searchPath in this.paths)
			{
				searchPath.InvalidateCache();
			}
		}

		// Token: 0x04007B8A RID: 31626
		public readonly bool IsFolder;

		// Token: 0x04007B8B RID: 31627
		public readonly string Extension;

		// Token: 0x04007B8C RID: 31628
		public readonly bool RemoveExtension;

		// Token: 0x04007B8D RID: 31629
		public readonly bool Recursive;

		// Token: 0x04007B8E RID: 31630
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<PathAbstractions.SearchPath> paths;
	}

	// Token: 0x020014A3 RID: 5283
	public abstract class SearchPath
	{
		// Token: 0x170013A6 RID: 5030
		// (get) Token: 0x0600A538 RID: 42296 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool CanMatch
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A539 RID: 42297 RVA: 0x003DA7F0 File Offset: 0x003D89F0
		[PublicizedFrom(EAccessModifier.Protected)]
		public SearchPath(string _relativePath)
		{
			this.RelativePath = _relativePath;
		}

		// Token: 0x0600A53A RID: 42298 RVA: 0x003DA80A File Offset: 0x003D8A0A
		public void SetOwner(PathAbstractions.SearchDefinition _owner)
		{
			this.Owner = _owner;
		}

		// Token: 0x0600A53B RID: 42299
		public abstract PathAbstractions.AbstractedLocation GetLocation(string _name, UserDataStorageType? _userDataHint);

		// Token: 0x0600A53C RID: 42300
		public abstract void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames);

		// Token: 0x0600A53D RID: 42301 RVA: 0x003DA814 File Offset: 0x003D8A14
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathAbstractions.AbstractedLocation getLocationSingleBase(PathAbstractions.EAbstractedLocationType _locationType, string _basePath, string _name, Mod _containingMod, string _subfolder = null)
		{
			this.UseCache();
			if (!SdDirectory.Exists(_basePath))
			{
				return PathAbstractions.AbstractedLocation.None;
			}
			string text = _basePath + "/" + _name + this.Owner.Extension;
			if (this.Owner.IsFolder)
			{
				if (SdDirectory.Exists(text))
				{
					return new PathAbstractions.AbstractedLocation(_locationType, _name, text, _subfolder, this.Owner.IsFolder, _containingMod);
				}
			}
			else
			{
				if (SdFile.Exists(text))
				{
					string text2 = (this.Owner.RemoveExtension ? GameIO.RemoveExtension(_name, this.Owner.Extension) : _name);
					return new PathAbstractions.AbstractedLocation(_locationType, text2, text, _subfolder, this.Owner.IsFolder, _containingMod);
				}
				if (this.Owner.Recursive)
				{
					foreach (string text3 in SdDirectory.GetDirectories(_basePath))
					{
						PathAbstractions.AbstractedLocation locationSingleBase = this.getLocationSingleBase(_locationType, text3, _name, _containingMod, Path.GetFileName(text3));
						if (!locationSingleBase.Equals(PathAbstractions.AbstractedLocation.None))
						{
							return locationSingleBase;
						}
					}
				}
			}
			return PathAbstractions.AbstractedLocation.None;
		}

		// Token: 0x0600A53E RID: 42302 RVA: 0x003DA914 File Offset: 0x003D8B14
		[PublicizedFrom(EAccessModifier.Protected)]
		public void getAvailablePathsSingleBase(List<PathAbstractions.AbstractedLocation> _targetList, PathAbstractions.EAbstractedLocationType _locationType, string _basePath, Regex _nameMatch, bool _ignoreDuplicateNames, Mod _containingMod, string _subfolder = null)
		{
			if (!SdDirectory.Exists(_basePath))
			{
				return;
			}
			SdDirectoryInfo sdDirectoryInfo = new SdDirectoryInfo(_basePath);
			SdFileSystemInfo[] array;
			SdFileSystemInfo[] array2;
			if (this.Owner.IsFolder)
			{
				array = sdDirectoryInfo.GetDirectories();
				array2 = array;
			}
			else
			{
				array = sdDirectoryInfo.GetFiles("*" + this.Owner.Extension, SearchOption.TopDirectoryOnly);
				array2 = array;
			}
			array = array2;
			for (int i = 0; i < array.Length; i++)
			{
				SdFileSystemInfo sdFileSystemInfo = array[i];
				if (this.Owner.Extension == null || sdFileSystemInfo.Name.EndsWith(this.Owner.Extension, StringComparison.Ordinal))
				{
					string filename = (this.Owner.RemoveExtension ? GameIO.RemoveExtension(sdFileSystemInfo.Name, this.Owner.Extension) : sdFileSystemInfo.Name);
					if ((_nameMatch == null || _nameMatch.IsMatch(filename)) && (!_ignoreDuplicateNames || !_targetList.Exists((PathAbstractions.AbstractedLocation _location) => _location.Name.Equals(filename))))
					{
						_targetList.Add(new PathAbstractions.AbstractedLocation(_locationType, filename, sdFileSystemInfo.FullName, _subfolder, this.Owner.IsFolder, _containingMod));
					}
				}
			}
			if (!this.Owner.IsFolder && this.Owner.Recursive)
			{
				foreach (string text in SdDirectory.GetDirectories(_basePath))
				{
					this.getAvailablePathsSingleBase(_targetList, _locationType, text, _nameMatch, _ignoreDuplicateNames, _containingMod, Path.GetFileName(text));
				}
			}
		}

		// Token: 0x0600A53F RID: 42303 RVA: 0x003DAA8C File Offset: 0x003D8C8C
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool UseCache()
		{
			bool cacheEnabled = PathAbstractions.CacheEnabled;
			if (cacheEnabled && !this.locationsCachePopulated)
			{
				this.PopulateCache();
			}
			return cacheEnabled;
		}

		// Token: 0x0600A540 RID: 42304 RVA: 0x003DAAA4 File Offset: 0x003D8CA4
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathAbstractions.AbstractedLocation GetCachedLocation(string _name, bool _ignoreName = false)
		{
			if (this.locationsCache.Count == 0)
			{
				return PathAbstractions.AbstractedLocation.None;
			}
			if (_ignoreName)
			{
				foreach (KeyValuePair<string, IList<PathAbstractions.AbstractedLocation>> keyValuePair in this.locationsCache)
				{
					if (keyValuePair.Value.Count != 0)
					{
						return keyValuePair.Value[0];
					}
				}
				return PathAbstractions.AbstractedLocation.None;
			}
			IList<PathAbstractions.AbstractedLocation> list;
			if (!this.locationsCache.TryGetValue(this.Owner.RemoveExtension ? _name : (_name + this.Owner.Extension), out list))
			{
				return PathAbstractions.AbstractedLocation.None;
			}
			if (list.Count <= 0)
			{
				return PathAbstractions.AbstractedLocation.None;
			}
			return list[0];
		}

		// Token: 0x0600A541 RID: 42305 RVA: 0x003DAB78 File Offset: 0x003D8D78
		[PublicizedFrom(EAccessModifier.Protected)]
		public void GetCachedPathList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			if (this.locationsCache.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<string, IList<PathAbstractions.AbstractedLocation>> keyValuePair in this.locationsCache)
			{
				using (IEnumerator<PathAbstractions.AbstractedLocation> enumerator2 = keyValuePair.Value.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						PathAbstractions.AbstractedLocation loc = enumerator2.Current;
						if ((_nameMatch == null || _nameMatch.IsMatch(loc.Name)) && (!_ignoreDuplicateNames || !_targetList.Exists((PathAbstractions.AbstractedLocation location) => location.Name.Equals(loc.Name))))
						{
							_targetList.Add(loc);
						}
					}
				}
			}
		}

		// Token: 0x0600A542 RID: 42306 RVA: 0x003DAC4C File Offset: 0x003D8E4C
		public void InvalidateCache()
		{
			this.locationsCache.Clear();
			this.locationsCachePopulated = false;
		}

		// Token: 0x0600A543 RID: 42307
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void PopulateCache();

		// Token: 0x0600A544 RID: 42308
		public abstract PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint);

		// Token: 0x0600A545 RID: 42309
		public abstract string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod);

		// Token: 0x0600A546 RID: 42310
		public abstract bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType);

		// Token: 0x04007B8F RID: 31631
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathAbstractions.SearchDefinition Owner;

		// Token: 0x04007B90 RID: 31632
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string RelativePath;

		// Token: 0x04007B91 RID: 31633
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Dictionary<string, IList<PathAbstractions.AbstractedLocation>> locationsCache = new CaseInsensitiveStringDictionary<IList<PathAbstractions.AbstractedLocation>>();

		// Token: 0x04007B92 RID: 31634
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool locationsCachePopulated;
	}

	// Token: 0x020014A6 RID: 5286
	public class SearchPathBasic : PathAbstractions.SearchPath
	{
		// Token: 0x0600A54B RID: 42315 RVA: 0x003DAC8B File Offset: 0x003D8E8B
		public SearchPathBasic(PathAbstractions.EAbstractedLocationType _locationType, Func<string> _basePath, string _relativePath, Func<bool> _canMatch = null)
			: base(_relativePath)
		{
			this.locationType = _locationType;
			this.basePath = _basePath;
			this.canMatch = _canMatch;
		}

		// Token: 0x170013A7 RID: 5031
		// (get) Token: 0x0600A54C RID: 42316 RVA: 0x003DACAA File Offset: 0x003D8EAA
		public override bool CanMatch
		{
			get
			{
				Func<bool> func = this.canMatch;
				if (func == null)
				{
					return base.CanMatch;
				}
				return func();
			}
		}

		// Token: 0x0600A54D RID: 42317 RVA: 0x003DACC4 File Offset: 0x003D8EC4
		public override PathAbstractions.AbstractedLocation GetLocation(string _name, UserDataStorageType? _userDataHint)
		{
			if (base.UseCache())
			{
				return base.GetCachedLocation(_name, false);
			}
			PathAbstractions.AbstractedLocation locationSingleBase = base.getLocationSingleBase(this.locationType, this.basePath() + "/" + this.RelativePath, _name, null, null);
			if (!locationSingleBase.Equals(PathAbstractions.AbstractedLocation.None))
			{
				return locationSingleBase;
			}
			return PathAbstractions.AbstractedLocation.None;
		}

		// Token: 0x0600A54E RID: 42318 RVA: 0x003DAD22 File Offset: 0x003D8F22
		public override void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			if (base.UseCache())
			{
				base.GetCachedPathList(_targetList, _nameMatch, _ignoreDuplicateNames);
				return;
			}
			base.getAvailablePathsSingleBase(_targetList, this.locationType, this.basePath() + "/" + this.RelativePath, _nameMatch, _ignoreDuplicateNames, null, null);
		}

		// Token: 0x0600A54F RID: 42319 RVA: 0x003DAD64 File Offset: 0x003D8F64
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void PopulateCache()
		{
			List<PathAbstractions.AbstractedLocation> list = new List<PathAbstractions.AbstractedLocation>();
			base.getAvailablePathsSingleBase(list, this.locationType, this.basePath() + "/" + this.RelativePath, null, false, null, null);
			this.locationsCache.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				PathAbstractions.AbstractedLocation abstractedLocation = list[i];
				IList<PathAbstractions.AbstractedLocation> list2;
				if (!this.locationsCache.TryGetValue(abstractedLocation.Name, out list2))
				{
					list2 = new List<PathAbstractions.AbstractedLocation>();
					this.locationsCache[abstractedLocation.Name] = list2;
				}
				list2.Add(abstractedLocation);
			}
			this.locationsCachePopulated = true;
		}

		// Token: 0x0600A550 RID: 42320 RVA: 0x003DAE04 File Offset: 0x003D9004
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, this.basePath() + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, null));
		}

		// Token: 0x0600A551 RID: 42321 RVA: 0x003DAE6B File Offset: 0x003D906B
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			return this.basePath() + "/" + this.RelativePath;
		}

		// Token: 0x0600A552 RID: 42322 RVA: 0x003DAE88 File Offset: 0x003D9088
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == this.locationType;
		}

		// Token: 0x04007B95 RID: 31637
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly PathAbstractions.EAbstractedLocationType locationType;

		// Token: 0x04007B96 RID: 31638
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<string> basePath;

		// Token: 0x04007B97 RID: 31639
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<bool> canMatch;
	}

	// Token: 0x020014A7 RID: 5287
	public class SearchPathUserData : PathAbstractions.SearchPath
	{
		// Token: 0x0600A553 RID: 42323 RVA: 0x003DAE93 File Offset: 0x003D9093
		public SearchPathUserData(string _relativePath, Func<UserDataStorageType> _getStorageTarget)
			: base(_relativePath)
		{
			this.getStorageTarget = _getStorageTarget;
		}

		// Token: 0x0600A554 RID: 42324 RVA: 0x003DAEA4 File Offset: 0x003D90A4
		public override PathAbstractions.AbstractedLocation GetLocation(string _name, UserDataStorageType? _userDataHint)
		{
			UserDataStorageType userDataStorageType = ((_userDataHint != null) ? _userDataHint.Value : this.getStorageTarget());
			if (base.UseCache())
			{
				return base.GetCachedLocation(PathAbstractions.SearchPathUserData.GetCacheLookupName(_name, userDataStorageType), false);
			}
			string text = Path.Combine(GameIO.GetUserGameDataDir(userDataStorageType), this.RelativePath);
			return base.getLocationSingleBase(PathAbstractions.EAbstractedLocationType.UserDataPath, text, _name, null, null);
		}

		// Token: 0x0600A555 RID: 42325 RVA: 0x003DAF04 File Offset: 0x003D9104
		[PublicizedFrom(EAccessModifier.Private)]
		public void CollectLocations(List<PathAbstractions.AbstractedLocation> _list, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			string text = Path.Combine(GameIO.GetUserGameDataDir(UserDataStorageType.DeviceLocal), this.RelativePath);
			base.getAvailablePathsSingleBase(_list, PathAbstractions.EAbstractedLocationType.UserDataPath, text, _nameMatch, _ignoreDuplicateNames, null, null);
			if (PlatformManager.MultiPlatform.UserDataRoaming.IsSupported)
			{
				string text2 = Path.Combine(GameIO.GetUserGameDataDir(UserDataStorageType.Roaming), this.RelativePath);
				base.getAvailablePathsSingleBase(_list, PathAbstractions.EAbstractedLocationType.UserDataPath, text2, _nameMatch, _ignoreDuplicateNames, null, null);
			}
		}

		// Token: 0x0600A556 RID: 42326 RVA: 0x003DAF60 File Offset: 0x003D9160
		public override void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			if (base.UseCache())
			{
				base.GetCachedPathList(_targetList, _nameMatch, _ignoreDuplicateNames);
				return;
			}
			this.CollectLocations(_targetList, _nameMatch, _ignoreDuplicateNames);
		}

		// Token: 0x0600A557 RID: 42327 RVA: 0x003DAF7D File Offset: 0x003D917D
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetCacheLookupName(string _name, UserDataStorageType _storageType)
		{
			return string.Format("{0}_{1}", _name, _storageType);
		}

		// Token: 0x0600A558 RID: 42328 RVA: 0x003DAF90 File Offset: 0x003D9190
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void PopulateCache()
		{
			List<PathAbstractions.AbstractedLocation> list = new List<PathAbstractions.AbstractedLocation>();
			this.CollectLocations(list, null, false);
			this.locationsCache.Clear();
			foreach (PathAbstractions.AbstractedLocation abstractedLocation in list)
			{
				string cacheLookupName = PathAbstractions.SearchPathUserData.GetCacheLookupName(abstractedLocation.Name, abstractedLocation.StorageType);
				IList<PathAbstractions.AbstractedLocation> list2;
				if (!this.locationsCache.TryGetValue(cacheLookupName, out list2))
				{
					list2 = new List<PathAbstractions.AbstractedLocation>();
					this.locationsCache.Add(cacheLookupName, list2);
				}
				list2.Add(abstractedLocation);
			}
			this.locationsCachePopulated = true;
		}

		// Token: 0x0600A559 RID: 42329 RVA: 0x003DB038 File Offset: 0x003D9238
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			string userGameDataDir = GameIO.GetUserGameDataDir((_userStorageHint != null) ? _userStorageHint.Value : this.getStorageTarget());
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, userGameDataDir + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, null));
		}

		// Token: 0x0600A55A RID: 42330 RVA: 0x003DB0B8 File Offset: 0x003D92B8
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			return GameIO.GetUserGameDataDir(this.getStorageTarget()) + "/" + this.RelativePath;
		}

		// Token: 0x0600A55B RID: 42331 RVA: 0x003DB0DA File Offset: 0x003D92DA
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.UserDataPath;
		}

		// Token: 0x04007B98 RID: 31640
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<UserDataStorageType> getStorageTarget;
	}

	// Token: 0x020014A8 RID: 5288
	public class SearchPathSaves : PathAbstractions.SearchPath
	{
		// Token: 0x170013A8 RID: 5032
		// (get) Token: 0x0600A55C RID: 42332 RVA: 0x003DB0E0 File Offset: 0x003D92E0
		public override bool CanMatch
		{
			get
			{
				return SingletonMonoBehaviour<ConnectionManager>.Instance != null;
			}
		}

		// Token: 0x0600A55D RID: 42333 RVA: 0x003DB0ED File Offset: 0x003D92ED
		public SearchPathSaves(string _relativePath)
			: base(_relativePath)
		{
		}

		// Token: 0x0600A55E RID: 42334 RVA: 0x003DB0F6 File Offset: 0x003D92F6
		[PublicizedFrom(EAccessModifier.Private)]
		public ValueTuple<string, PathAbstractions.EAbstractedLocationType> GetSaveFolder()
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return new ValueTuple<string, PathAbstractions.EAbstractedLocationType>(GameIO.GetSaveGameDir(), PathAbstractions.EAbstractedLocationType.HostSave);
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
			{
				return new ValueTuple<string, PathAbstractions.EAbstractedLocationType>(GameIO.GetSaveGameLocalDir(), PathAbstractions.EAbstractedLocationType.LocalSave);
			}
			return new ValueTuple<string, PathAbstractions.EAbstractedLocationType>(null, PathAbstractions.EAbstractedLocationType.None);
		}

		// Token: 0x0600A55F RID: 42335 RVA: 0x003DB130 File Offset: 0x003D9330
		public override PathAbstractions.AbstractedLocation GetLocation(string _name, UserDataStorageType? _userDataHint)
		{
			if (base.UseCache())
			{
				return base.GetCachedLocation(_name, true);
			}
			ValueTuple<string, PathAbstractions.EAbstractedLocationType> saveFolder = this.GetSaveFolder();
			string item = saveFolder.Item1;
			PathAbstractions.EAbstractedLocationType item2 = saveFolder.Item2;
			if (item == null)
			{
				return PathAbstractions.AbstractedLocation.None;
			}
			string text = item + "/" + this.RelativePath;
			if ((this.Owner.IsFolder && SdDirectory.Exists(text)) || (!this.Owner.IsFolder && SdFile.Exists(text)))
			{
				return new PathAbstractions.AbstractedLocation(item2, this.RelativePath, text, null, this.Owner.IsFolder, null);
			}
			return PathAbstractions.AbstractedLocation.None;
		}

		// Token: 0x0600A560 RID: 42336 RVA: 0x003DB1CC File Offset: 0x003D93CC
		public override void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			if (base.UseCache())
			{
				base.GetCachedPathList(_targetList, null, _ignoreDuplicateNames);
				return;
			}
			ValueTuple<string, PathAbstractions.EAbstractedLocationType> saveFolder = this.GetSaveFolder();
			string item = saveFolder.Item1;
			PathAbstractions.EAbstractedLocationType item2 = saveFolder.Item2;
			if (item == null)
			{
				return;
			}
			string text = item + "/" + this.RelativePath;
			if ((this.Owner.IsFolder && SdDirectory.Exists(text)) || (!this.Owner.IsFolder && SdFile.Exists(text)))
			{
				_targetList.Add(new PathAbstractions.AbstractedLocation(item2, this.RelativePath, text, null, this.Owner.IsFolder, null));
			}
		}

		// Token: 0x0600A561 RID: 42337 RVA: 0x003DB260 File Offset: 0x003D9460
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void PopulateCache()
		{
			ValueTuple<string, PathAbstractions.EAbstractedLocationType> saveFolder = this.GetSaveFolder();
			string item = saveFolder.Item1;
			PathAbstractions.EAbstractedLocationType item2 = saveFolder.Item2;
			this.locationsCache.Clear();
			this.locationsCachePopulated = true;
			if (item == null)
			{
				return;
			}
			string text = item + "/" + this.RelativePath;
			if ((this.Owner.IsFolder && SdDirectory.Exists(text)) || (!this.Owner.IsFolder && SdFile.Exists(text)))
			{
				PathAbstractions.AbstractedLocation abstractedLocation = new PathAbstractions.AbstractedLocation(item2, this.RelativePath, text, null, this.Owner.IsFolder, null);
				List<PathAbstractions.AbstractedLocation> list = new List<PathAbstractions.AbstractedLocation>();
				this.locationsCache[abstractedLocation.Name] = list;
				list.Add(abstractedLocation);
			}
		}

		// Token: 0x0600A562 RID: 42338 RVA: 0x003DB310 File Offset: 0x003D9510
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			ValueTuple<string, PathAbstractions.EAbstractedLocationType> saveFolder = this.GetSaveFolder();
			string item = saveFolder.Item1;
			PathAbstractions.EAbstractedLocationType item2 = saveFolder.Item2;
			if (item == null)
			{
				return null;
			}
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(item2, _elementName, item + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, null));
		}

		// Token: 0x0600A563 RID: 42339 RVA: 0x003DB390 File Offset: 0x003D9590
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			string item = this.GetSaveFolder().Item1;
			if (item == null)
			{
				return null;
			}
			return item + "/" + this.RelativePath;
		}

		// Token: 0x0600A564 RID: 42340 RVA: 0x0033D58D File Offset: 0x0033B78D
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.HostSave || _locationType == PathAbstractions.EAbstractedLocationType.LocalSave;
		}
	}

	// Token: 0x020014A9 RID: 5289
	public class SearchPathMods : PathAbstractions.SearchPath
	{
		// Token: 0x170013A9 RID: 5033
		// (get) Token: 0x0600A565 RID: 42341 RVA: 0x003DB0E0 File Offset: 0x003D92E0
		public override bool CanMatch
		{
			get
			{
				return SingletonMonoBehaviour<ConnectionManager>.Instance != null;
			}
		}

		// Token: 0x0600A566 RID: 42342 RVA: 0x003DB0ED File Offset: 0x003D92ED
		public SearchPathMods(string _relativePath)
			: base(_relativePath)
		{
		}

		// Token: 0x0600A567 RID: 42343 RVA: 0x003DB3C0 File Offset: 0x003D95C0
		public override PathAbstractions.AbstractedLocation GetLocation(string _name, UserDataStorageType? _userDataHint)
		{
			if (base.UseCache())
			{
				return base.GetCachedLocation(_name, false);
			}
			foreach (Mod mod in ModManager.GetLoadedMods())
			{
				PathAbstractions.AbstractedLocation locationSingleBase = base.getLocationSingleBase(PathAbstractions.EAbstractedLocationType.Mods, mod.Path + "/" + this.RelativePath, _name, mod, null);
				if (!locationSingleBase.Equals(PathAbstractions.AbstractedLocation.None))
				{
					return locationSingleBase;
				}
			}
			return PathAbstractions.AbstractedLocation.None;
		}

		// Token: 0x0600A568 RID: 42344 RVA: 0x003DB458 File Offset: 0x003D9658
		public override void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			if (base.UseCache())
			{
				base.GetCachedPathList(_targetList, _nameMatch, _ignoreDuplicateNames);
				return;
			}
			foreach (Mod mod in ModManager.GetLoadedMods())
			{
				base.getAvailablePathsSingleBase(_targetList, PathAbstractions.EAbstractedLocationType.Mods, mod.Path + "/" + this.RelativePath, _nameMatch, _ignoreDuplicateNames, mod, null);
			}
		}

		// Token: 0x0600A569 RID: 42345 RVA: 0x003DB4D8 File Offset: 0x003D96D8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void PopulateCache()
		{
			List<PathAbstractions.AbstractedLocation> list = new List<PathAbstractions.AbstractedLocation>();
			foreach (Mod mod in ModManager.GetLoadedMods())
			{
				base.getAvailablePathsSingleBase(list, PathAbstractions.EAbstractedLocationType.Mods, mod.Path + "/" + this.RelativePath, null, false, mod, null);
			}
			this.locationsCache.Clear();
			foreach (PathAbstractions.AbstractedLocation abstractedLocation in list)
			{
				IList<PathAbstractions.AbstractedLocation> list2;
				if (!this.locationsCache.TryGetValue(abstractedLocation.Name, out list2))
				{
					list2 = new List<PathAbstractions.AbstractedLocation>();
					this.locationsCache[abstractedLocation.Name] = list2;
				}
				list2.Add(abstractedLocation);
			}
			this.locationsCachePopulated = true;
		}

		// Token: 0x0600A56A RID: 42346 RVA: 0x003DB5D0 File Offset: 0x003D97D0
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			if (_mod == null)
			{
				throw new ArgumentException("BuildLocation for LocationType==Mods requires a loaded Mod", "_mod");
			}
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, _mod.Path + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, _mod));
		}

		// Token: 0x0600A56B RID: 42347 RVA: 0x003DB648 File Offset: 0x003D9848
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			if (_mod == null)
			{
				throw new ArgumentException("GetBasePath for LocationType==Mods requires a loaded Mod", "_mod");
			}
			return _mod.Path + "/" + this.RelativePath;
		}

		// Token: 0x0600A56C RID: 42348 RVA: 0x003DB673 File Offset: 0x003D9873
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.Mods;
		}
	}

	// Token: 0x020014AA RID: 5290
	public static class Contextual
	{
		// Token: 0x0600A56D RID: 42349 RVA: 0x003DB67C File Offset: 0x003D987C
		public static PathAbstractions.AbstractedLocation FindDownloadedRemoteWorld(string _saveDir)
		{
			string text = Path.Combine(_saveDir, "World");
			if (!SdDirectory.Exists(text))
			{
				return PathAbstractions.AbstractedLocation.None;
			}
			return new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.LocalSave, "World", text, null, true, null);
		}

		// Token: 0x0600A56E RID: 42350 RVA: 0x003DB6B4 File Offset: 0x003D98B4
		public static void FindActiveWorld(out string _name, out PathAbstractions.AbstractedLocation _location)
		{
			_name = GamePrefs.GetString(EnumGamePrefs.GameWorld);
			PathAbstractions.EAbstractedLocationType @int = (PathAbstractions.EAbstractedLocationType)GamePrefs.GetInt(EnumGamePrefs.GameWorldLocationType);
			UserDataStorageType int2 = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType);
			_location = PathAbstractions.Contextual.FindWorld(_name, @int, int2);
		}

		// Token: 0x0600A56F RID: 42351 RVA: 0x003DB6EF File Offset: 0x003D98EF
		public static PathAbstractions.AbstractedLocation FindActiveWorldLocation()
		{
			return PathAbstractions.Contextual.FindWorld(GamePrefs.GetString(EnumGamePrefs.GameWorld), (PathAbstractions.EAbstractedLocationType)GamePrefs.GetInt(EnumGamePrefs.GameWorldLocationType), (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType));
		}

		// Token: 0x0600A570 RID: 42352 RVA: 0x003DB714 File Offset: 0x003D9914
		public static bool DoesWorldExist(string _name)
		{
			UserDataStorageType? userDataStorageType;
			if (PlatformManager.MultiPlatform.UserDataRoaming.SaveRoamingEnabled)
			{
				PathAbstractions.SearchDefinition worldsSearchPaths = PathAbstractions.WorldsSearchPaths;
				userDataStorageType = new UserDataStorageType?(UserDataStorageType.Roaming);
				if (worldsSearchPaths.GetLocation(_name, null, userDataStorageType).Exists())
				{
					return true;
				}
			}
			PathAbstractions.SearchDefinition worldsSearchPaths2 = PathAbstractions.WorldsSearchPaths;
			userDataStorageType = new UserDataStorageType?(UserDataStorageType.DeviceLocal);
			return worldsSearchPaths2.GetLocation(_name, null, userDataStorageType).Exists();
		}

		// Token: 0x0600A571 RID: 42353 RVA: 0x003DB780 File Offset: 0x003D9980
		[PublicizedFrom(EAccessModifier.Private)]
		public static PathAbstractions.AbstractedLocation FindWorld(string _name, PathAbstractions.EAbstractedLocationType _locationTypeHint, UserDataStorageType _storageTypeHint)
		{
			if (_locationTypeHint == PathAbstractions.EAbstractedLocationType.None)
			{
				PathAbstractions.SearchDefinition worldsSearchPaths = PathAbstractions.WorldsSearchPaths;
				UserDataStorageType? userDataStorageType = new UserDataStorageType?(_storageTypeHint);
				return worldsSearchPaths.GetLocation(_name, null, userDataStorageType);
			}
			return PathAbstractions.WorldsSearchPaths.GetLocation(_name, new PathAbstractions.EAbstractedLocationType?(_locationTypeHint), new UserDataStorageType?(_storageTypeHint));
		}
	}
}
