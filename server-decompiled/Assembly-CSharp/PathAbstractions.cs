using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x02001452 RID: 5202
public static class PathAbstractions
{
	// Token: 0x0600A31A RID: 41754 RVA: 0x003CDAAC File Offset: 0x003CBCAC
	public static void InvalidateCaches()
	{
		foreach (PathAbstractions.SearchDefinition searchDefinition in PathAbstractions.allSearchDefs)
		{
			searchDefinition.InvalidateCache();
		}
	}

	// Token: 0x17001319 RID: 4889
	// (get) Token: 0x0600A31B RID: 41755 RVA: 0x003CDAFC File Offset: 0x003CBCFC
	// (set) Token: 0x0600A31C RID: 41756 RVA: 0x003CDB03 File Offset: 0x003CBD03
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

	// Token: 0x040079DB RID: 31195
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<PathAbstractions.SearchDefinition> allSearchDefs = new List<PathAbstractions.SearchDefinition>();

	// Token: 0x040079DC RID: 31196
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<string> deviceLocalUserDataPath = new Func<string>(GameIO.GetDeviceLocalUserGameDataDir);

	// Token: 0x040079DD RID: 31197
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<string> gameDataPath = new Func<string>(GameIO.GetApplicationPath);

	// Token: 0x040079DE RID: 31198
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<UserDataStorageType> getWorldsStorageType = () => (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType);

	// Token: 0x040079DF RID: 31199
	public static readonly PathAbstractions.SearchDefinition WorldsSearchPaths = new PathAbstractions.SearchDefinition(true, null, false, false, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathSaves("World"),
		new PathAbstractions.SearchPathUserData("GeneratedWorlds", PathAbstractions.getWorldsStorageType),
		new PathAbstractions.SearchPathMods("Worlds"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Worlds", null)
	});

	// Token: 0x040079E0 RID: 31200
	public static readonly PathAbstractions.SearchDefinition PrefabsSearchPaths = new PathAbstractions.SearchDefinition(false, ".tts", true, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalPrefabs", null),
		new PathAbstractions.SearchPathMods("Prefabs"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Prefabs", null)
	});

	// Token: 0x040079E1 RID: 31201
	public static readonly PathAbstractions.SearchDefinition PrefabImpostersSearchPaths = new PathAbstractions.SearchDefinition(false, ".mesh", false, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalPrefabs", null),
		new PathAbstractions.SearchPathMods("Prefabs"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Prefabs", null)
	});

	// Token: 0x040079E2 RID: 31202
	public static readonly PathAbstractions.SearchDefinition RwgStampsSearchPaths = new PathAbstractions.SearchDefinition(false, "", true, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalStamps", null),
		new PathAbstractions.SearchPathMods("Stamps"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Stamps", null)
	});

	// Token: 0x040079E3 RID: 31203
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool cacheEnabled;

	// Token: 0x02001453 RID: 5203
	public enum EAbstractedLocationType
	{
		// Token: 0x040079E5 RID: 31205
		HostSave,
		// Token: 0x040079E6 RID: 31206
		LocalSave,
		// Token: 0x040079E7 RID: 31207
		UserDataPath,
		// Token: 0x040079E8 RID: 31208
		Mods,
		// Token: 0x040079E9 RID: 31209
		GameData,
		// Token: 0x040079EA RID: 31210
		None
	}

	// Token: 0x02001454 RID: 5204
	public readonly struct AbstractedLocation : IEquatable<PathAbstractions.AbstractedLocation>, IComparable<PathAbstractions.AbstractedLocation>, IComparable
	{
		// Token: 0x1700131A RID: 4890
		// (get) Token: 0x0600A31E RID: 41758 RVA: 0x003CDC9D File Offset: 0x003CBE9D
		public string FullPath
		{
			get
			{
				return this.Folder + "/" + this.FileNameNoExtension + this.Extension;
			}
		}

		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x0600A31F RID: 41759 RVA: 0x003CDCBB File Offset: 0x003CBEBB
		public string FullPathNoExtension
		{
			get
			{
				return this.Folder + "/" + this.FileNameNoExtension;
			}
		}

		// Token: 0x0600A320 RID: 41760 RVA: 0x003CDCD4 File Offset: 0x003CBED4
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

		// Token: 0x0600A321 RID: 41761 RVA: 0x003CDDA0 File Offset: 0x003CBFA0
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

		// Token: 0x0600A322 RID: 41762 RVA: 0x003CDE23 File Offset: 0x003CC023
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

		// Token: 0x0600A323 RID: 41763 RVA: 0x003CDE50 File Offset: 0x003CC050
		public bool Equals(PathAbstractions.AbstractedLocation _other)
		{
			return this.Type == _other.Type && (this.Type == PathAbstractions.EAbstractedLocationType.None || (this.IsFolder == _other.IsFolder && string.Equals(this.Name, _other.Name) && GameIO.PathsEquals(this.FullPath, _other.FullPath, true)));
		}

		// Token: 0x0600A324 RID: 41764 RVA: 0x003CDEB0 File Offset: 0x003CC0B0
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

		// Token: 0x0600A325 RID: 41765 RVA: 0x003CDEDA File Offset: 0x003CC0DA
		public static bool operator ==(PathAbstractions.AbstractedLocation _a, PathAbstractions.AbstractedLocation _b)
		{
			return _a.Equals(_b);
		}

		// Token: 0x0600A326 RID: 41766 RVA: 0x003CDEE4 File Offset: 0x003CC0E4
		public static bool operator !=(PathAbstractions.AbstractedLocation _a, PathAbstractions.AbstractedLocation _b)
		{
			return !(_a == _b);
		}

		// Token: 0x0600A327 RID: 41767 RVA: 0x003CDEF0 File Offset: 0x003CC0F0
		public override int GetHashCode()
		{
			return (((((this.Name != null) ? this.Name.GetHashCode() : 0) * 397) ^ (int)this.Type) * 397) ^ ((this.FullPath != null) ? this.FullPath.GetHashCode() : 0);
		}

		// Token: 0x0600A328 RID: 41768 RVA: 0x003CDF40 File Offset: 0x003CC140
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

		// Token: 0x0600A329 RID: 41769 RVA: 0x003CDF98 File Offset: 0x003CC198
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

		// Token: 0x0600A32A RID: 41770 RVA: 0x003CE018 File Offset: 0x003CC218
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

		// Token: 0x0600A32B RID: 41771 RVA: 0x003CE0A4 File Offset: 0x003CC2A4
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

		// Token: 0x040079EB RID: 31211
		public static readonly PathAbstractions.AbstractedLocation None = new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.None, null, null, null, false, null);

		// Token: 0x040079EC RID: 31212
		public readonly PathAbstractions.EAbstractedLocationType Type;

		// Token: 0x040079ED RID: 31213
		public readonly string Name;

		// Token: 0x040079EE RID: 31214
		public readonly string Folder;

		// Token: 0x040079EF RID: 31215
		public readonly string RelativePath;

		// Token: 0x040079F0 RID: 31216
		public readonly string FileNameNoExtension;

		// Token: 0x040079F1 RID: 31217
		public readonly string Extension;

		// Token: 0x040079F2 RID: 31218
		public readonly bool IsFolder;

		// Token: 0x040079F3 RID: 31219
		public readonly Mod ContainingMod;

		// Token: 0x040079F4 RID: 31220
		public readonly UserDataStorageType StorageType;
	}

	// Token: 0x02001455 RID: 5205
	public class SearchDefinition
	{
		// Token: 0x0600A32D RID: 41773 RVA: 0x003CE0EC File Offset: 0x003CC2EC
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

		// Token: 0x0600A32E RID: 41774 RVA: 0x003CE16C File Offset: 0x003CC36C
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

		// Token: 0x0600A32F RID: 41775 RVA: 0x003CE200 File Offset: 0x003CC400
		public List<PathAbstractions.AbstractedLocation> GetAvailablePathsList(Regex _nameMatch = null, bool _ignoreDuplicateNames = false, PathAbstractions.EAbstractedLocationType? _onlyLocationType = null)
		{
			List<PathAbstractions.AbstractedLocation> list = new List<PathAbstractions.AbstractedLocation>();
			this.GetAvailablePathsList(ref list, _nameMatch, _ignoreDuplicateNames, _onlyLocationType);
			return list;
		}

		// Token: 0x0600A330 RID: 41776 RVA: 0x003CE220 File Offset: 0x003CC420
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

		// Token: 0x0600A331 RID: 41777 RVA: 0x003CE2A4 File Offset: 0x003CC4A4
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

		// Token: 0x0600A332 RID: 41778 RVA: 0x003CE2F8 File Offset: 0x003CC4F8
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

		// Token: 0x0600A333 RID: 41779 RVA: 0x003CE368 File Offset: 0x003CC568
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

		// Token: 0x0600A334 RID: 41780 RVA: 0x003CE3B8 File Offset: 0x003CC5B8
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

		// Token: 0x0600A335 RID: 41781 RVA: 0x003CE41C File Offset: 0x003CC61C
		public void InvalidateCache()
		{
			foreach (PathAbstractions.SearchPath searchPath in this.paths)
			{
				searchPath.InvalidateCache();
			}
		}

		// Token: 0x040079F5 RID: 31221
		public readonly bool IsFolder;

		// Token: 0x040079F6 RID: 31222
		public readonly string Extension;

		// Token: 0x040079F7 RID: 31223
		public readonly bool RemoveExtension;

		// Token: 0x040079F8 RID: 31224
		public readonly bool Recursive;

		// Token: 0x040079F9 RID: 31225
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<PathAbstractions.SearchPath> paths;
	}

	// Token: 0x02001456 RID: 5206
	public abstract class SearchPath
	{
		// Token: 0x1700131C RID: 4892
		// (get) Token: 0x0600A336 RID: 41782 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public virtual bool CanMatch
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A337 RID: 41783 RVA: 0x003CE46C File Offset: 0x003CC66C
		[PublicizedFrom(EAccessModifier.Protected)]
		public SearchPath(string _relativePath)
		{
			this.RelativePath = _relativePath;
		}

		// Token: 0x0600A338 RID: 41784 RVA: 0x003CE486 File Offset: 0x003CC686
		public void SetOwner(PathAbstractions.SearchDefinition _owner)
		{
			this.Owner = _owner;
		}

		// Token: 0x0600A339 RID: 41785
		public abstract PathAbstractions.AbstractedLocation GetLocation(string _name, UserDataStorageType? _userDataHint);

		// Token: 0x0600A33A RID: 41786
		public abstract void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames);

		// Token: 0x0600A33B RID: 41787 RVA: 0x003CE490 File Offset: 0x003CC690
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

		// Token: 0x0600A33C RID: 41788 RVA: 0x003CE590 File Offset: 0x003CC790
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

		// Token: 0x0600A33D RID: 41789 RVA: 0x003CE708 File Offset: 0x003CC908
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

		// Token: 0x0600A33E RID: 41790 RVA: 0x003CE720 File Offset: 0x003CC920
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

		// Token: 0x0600A33F RID: 41791 RVA: 0x003CE7F4 File Offset: 0x003CC9F4
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

		// Token: 0x0600A340 RID: 41792 RVA: 0x003CE8C8 File Offset: 0x003CCAC8
		public void InvalidateCache()
		{
			this.locationsCache.Clear();
			this.locationsCachePopulated = false;
		}

		// Token: 0x0600A341 RID: 41793
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void PopulateCache();

		// Token: 0x0600A342 RID: 41794
		public abstract PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint);

		// Token: 0x0600A343 RID: 41795
		public abstract string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod);

		// Token: 0x0600A344 RID: 41796
		public abstract bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType);

		// Token: 0x040079FA RID: 31226
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathAbstractions.SearchDefinition Owner;

		// Token: 0x040079FB RID: 31227
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string RelativePath;

		// Token: 0x040079FC RID: 31228
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Dictionary<string, IList<PathAbstractions.AbstractedLocation>> locationsCache = new CaseInsensitiveStringDictionary<IList<PathAbstractions.AbstractedLocation>>();

		// Token: 0x040079FD RID: 31229
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool locationsCachePopulated;
	}

	// Token: 0x02001459 RID: 5209
	public class SearchPathBasic : PathAbstractions.SearchPath
	{
		// Token: 0x0600A349 RID: 41801 RVA: 0x003CE907 File Offset: 0x003CCB07
		public SearchPathBasic(PathAbstractions.EAbstractedLocationType _locationType, Func<string> _basePath, string _relativePath, Func<bool> _canMatch = null)
			: base(_relativePath)
		{
			this.locationType = _locationType;
			this.basePath = _basePath;
			this.canMatch = _canMatch;
		}

		// Token: 0x1700131D RID: 4893
		// (get) Token: 0x0600A34A RID: 41802 RVA: 0x003CE926 File Offset: 0x003CCB26
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

		// Token: 0x0600A34B RID: 41803 RVA: 0x003CE940 File Offset: 0x003CCB40
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

		// Token: 0x0600A34C RID: 41804 RVA: 0x003CE99E File Offset: 0x003CCB9E
		public override void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			if (base.UseCache())
			{
				base.GetCachedPathList(_targetList, _nameMatch, _ignoreDuplicateNames);
				return;
			}
			base.getAvailablePathsSingleBase(_targetList, this.locationType, this.basePath() + "/" + this.RelativePath, _nameMatch, _ignoreDuplicateNames, null, null);
		}

		// Token: 0x0600A34D RID: 41805 RVA: 0x003CE9E0 File Offset: 0x003CCBE0
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

		// Token: 0x0600A34E RID: 41806 RVA: 0x003CEA80 File Offset: 0x003CCC80
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, this.basePath() + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, null));
		}

		// Token: 0x0600A34F RID: 41807 RVA: 0x003CEAE7 File Offset: 0x003CCCE7
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			return this.basePath() + "/" + this.RelativePath;
		}

		// Token: 0x0600A350 RID: 41808 RVA: 0x003CEB04 File Offset: 0x003CCD04
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == this.locationType;
		}

		// Token: 0x04007A00 RID: 31232
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly PathAbstractions.EAbstractedLocationType locationType;

		// Token: 0x04007A01 RID: 31233
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<string> basePath;

		// Token: 0x04007A02 RID: 31234
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<bool> canMatch;
	}

	// Token: 0x0200145A RID: 5210
	public class SearchPathUserData : PathAbstractions.SearchPath
	{
		// Token: 0x0600A351 RID: 41809 RVA: 0x003CEB0F File Offset: 0x003CCD0F
		public SearchPathUserData(string _relativePath, Func<UserDataStorageType> _getStorageTarget)
			: base(_relativePath)
		{
			this.getStorageTarget = _getStorageTarget;
		}

		// Token: 0x0600A352 RID: 41810 RVA: 0x003CEB20 File Offset: 0x003CCD20
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

		// Token: 0x0600A353 RID: 41811 RVA: 0x003CEB80 File Offset: 0x003CCD80
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

		// Token: 0x0600A354 RID: 41812 RVA: 0x003CEBDC File Offset: 0x003CCDDC
		public override void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			if (base.UseCache())
			{
				base.GetCachedPathList(_targetList, _nameMatch, _ignoreDuplicateNames);
				return;
			}
			this.CollectLocations(_targetList, _nameMatch, _ignoreDuplicateNames);
		}

		// Token: 0x0600A355 RID: 41813 RVA: 0x003CEBF9 File Offset: 0x003CCDF9
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetCacheLookupName(string _name, UserDataStorageType _storageType)
		{
			return string.Format("{0}_{1}", _name, _storageType);
		}

		// Token: 0x0600A356 RID: 41814 RVA: 0x003CEC0C File Offset: 0x003CCE0C
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

		// Token: 0x0600A357 RID: 41815 RVA: 0x003CECB4 File Offset: 0x003CCEB4
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			string userGameDataDir = GameIO.GetUserGameDataDir((_userStorageHint != null) ? _userStorageHint.Value : this.getStorageTarget());
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, userGameDataDir + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, null));
		}

		// Token: 0x0600A358 RID: 41816 RVA: 0x003CED34 File Offset: 0x003CCF34
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			return GameIO.GetUserGameDataDir(this.getStorageTarget()) + "/" + this.RelativePath;
		}

		// Token: 0x0600A359 RID: 41817 RVA: 0x003CED56 File Offset: 0x003CCF56
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.UserDataPath;
		}

		// Token: 0x04007A03 RID: 31235
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<UserDataStorageType> getStorageTarget;
	}

	// Token: 0x0200145B RID: 5211
	public class SearchPathSaves : PathAbstractions.SearchPath
	{
		// Token: 0x1700131E RID: 4894
		// (get) Token: 0x0600A35A RID: 41818 RVA: 0x003CED5C File Offset: 0x003CCF5C
		public override bool CanMatch
		{
			get
			{
				return SingletonMonoBehaviour<ConnectionManager>.Instance != null;
			}
		}

		// Token: 0x0600A35B RID: 41819 RVA: 0x003CED69 File Offset: 0x003CCF69
		public SearchPathSaves(string _relativePath)
			: base(_relativePath)
		{
		}

		// Token: 0x0600A35C RID: 41820 RVA: 0x003CED72 File Offset: 0x003CCF72
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

		// Token: 0x0600A35D RID: 41821 RVA: 0x003CEDAC File Offset: 0x003CCFAC
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

		// Token: 0x0600A35E RID: 41822 RVA: 0x003CEE48 File Offset: 0x003CD048
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

		// Token: 0x0600A35F RID: 41823 RVA: 0x003CEEDC File Offset: 0x003CD0DC
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

		// Token: 0x0600A360 RID: 41824 RVA: 0x003CEF8C File Offset: 0x003CD18C
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

		// Token: 0x0600A361 RID: 41825 RVA: 0x003CF00C File Offset: 0x003CD20C
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			string item = this.GetSaveFolder().Item1;
			if (item == null)
			{
				return null;
			}
			return item + "/" + this.RelativePath;
		}

		// Token: 0x0600A362 RID: 41826 RVA: 0x00332EC9 File Offset: 0x003310C9
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.HostSave || _locationType == PathAbstractions.EAbstractedLocationType.LocalSave;
		}
	}

	// Token: 0x0200145C RID: 5212
	public class SearchPathMods : PathAbstractions.SearchPath
	{
		// Token: 0x1700131F RID: 4895
		// (get) Token: 0x0600A363 RID: 41827 RVA: 0x003CED5C File Offset: 0x003CCF5C
		public override bool CanMatch
		{
			get
			{
				return SingletonMonoBehaviour<ConnectionManager>.Instance != null;
			}
		}

		// Token: 0x0600A364 RID: 41828 RVA: 0x003CED69 File Offset: 0x003CCF69
		public SearchPathMods(string _relativePath)
			: base(_relativePath)
		{
		}

		// Token: 0x0600A365 RID: 41829 RVA: 0x003CF03C File Offset: 0x003CD23C
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

		// Token: 0x0600A366 RID: 41830 RVA: 0x003CF0D4 File Offset: 0x003CD2D4
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

		// Token: 0x0600A367 RID: 41831 RVA: 0x003CF154 File Offset: 0x003CD354
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

		// Token: 0x0600A368 RID: 41832 RVA: 0x003CF24C File Offset: 0x003CD44C
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			if (_mod == null)
			{
				throw new ArgumentException("BuildLocation for LocationType==Mods requires a loaded Mod", "_mod");
			}
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, _mod.Path + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, _mod));
		}

		// Token: 0x0600A369 RID: 41833 RVA: 0x003CF2C4 File Offset: 0x003CD4C4
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			if (_mod == null)
			{
				throw new ArgumentException("GetBasePath for LocationType==Mods requires a loaded Mod", "_mod");
			}
			return _mod.Path + "/" + this.RelativePath;
		}

		// Token: 0x0600A36A RID: 41834 RVA: 0x003CF2EF File Offset: 0x003CD4EF
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.Mods;
		}
	}

	// Token: 0x0200145D RID: 5213
	public static class Contextual
	{
		// Token: 0x0600A36B RID: 41835 RVA: 0x003CF2F8 File Offset: 0x003CD4F8
		public static PathAbstractions.AbstractedLocation FindDownloadedRemoteWorld(string _saveDir)
		{
			string text = Path.Combine(_saveDir, "World");
			if (!SdDirectory.Exists(text))
			{
				return PathAbstractions.AbstractedLocation.None;
			}
			return new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.LocalSave, "World", text, null, true, null);
		}

		// Token: 0x0600A36C RID: 41836 RVA: 0x003CF330 File Offset: 0x003CD530
		public static void FindActiveWorld(out string _name, out PathAbstractions.AbstractedLocation _location)
		{
			_name = GamePrefs.GetString(EnumGamePrefs.GameWorld);
			PathAbstractions.EAbstractedLocationType @int = (PathAbstractions.EAbstractedLocationType)GamePrefs.GetInt(EnumGamePrefs.GameWorldLocationType);
			UserDataStorageType int2 = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType);
			_location = PathAbstractions.Contextual.FindWorld(_name, @int, int2);
		}

		// Token: 0x0600A36D RID: 41837 RVA: 0x003CF36B File Offset: 0x003CD56B
		public static PathAbstractions.AbstractedLocation FindActiveWorldLocation()
		{
			return PathAbstractions.Contextual.FindWorld(GamePrefs.GetString(EnumGamePrefs.GameWorld), (PathAbstractions.EAbstractedLocationType)GamePrefs.GetInt(EnumGamePrefs.GameWorldLocationType), (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType));
		}

		// Token: 0x0600A36E RID: 41838 RVA: 0x003CF390 File Offset: 0x003CD590
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

		// Token: 0x0600A36F RID: 41839 RVA: 0x003CF3FC File Offset: 0x003CD5FC
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
