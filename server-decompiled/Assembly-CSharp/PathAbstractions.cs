using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x0200148D RID: 5261
public static class PathAbstractions
{
	// Token: 0x0600A4C4 RID: 42180 RVA: 0x003D7A2C File Offset: 0x003D5C2C
	public static void InvalidateCaches()
	{
		foreach (PathAbstractions.SearchDefinition searchDefinition in PathAbstractions.allSearchDefs)
		{
			searchDefinition.InvalidateCache();
		}
	}

	// Token: 0x17001397 RID: 5015
	// (get) Token: 0x0600A4C5 RID: 42181 RVA: 0x003D7A7C File Offset: 0x003D5C7C
	// (set) Token: 0x0600A4C6 RID: 42182 RVA: 0x003D7A83 File Offset: 0x003D5C83
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

	// Token: 0x04007B2F RID: 31535
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<PathAbstractions.SearchDefinition> allSearchDefs = new List<PathAbstractions.SearchDefinition>();

	// Token: 0x04007B30 RID: 31536
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<string> deviceLocalUserDataPath = new Func<string>(GameIO.GetDeviceLocalUserGameDataDir);

	// Token: 0x04007B31 RID: 31537
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<string> gameDataPath = new Func<string>(GameIO.GetApplicationPath);

	// Token: 0x04007B32 RID: 31538
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<UserDataStorageType> getWorldsStorageType = () => (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType);

	// Token: 0x04007B33 RID: 31539
	public static readonly PathAbstractions.SearchDefinition WorldsSearchPaths = new PathAbstractions.SearchDefinition(true, null, false, false, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathSaves("World"),
		new PathAbstractions.SearchPathUserData("GeneratedWorlds", PathAbstractions.getWorldsStorageType),
		new PathAbstractions.SearchPathMods("Worlds"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Worlds", null)
	});

	// Token: 0x04007B34 RID: 31540
	public static readonly PathAbstractions.SearchDefinition PrefabsSearchPaths = new PathAbstractions.SearchDefinition(false, ".tts", true, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalPrefabs", null),
		new PathAbstractions.SearchPathMods("Prefabs"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Prefabs", null)
	});

	// Token: 0x04007B35 RID: 31541
	public static readonly PathAbstractions.SearchDefinition PrefabImpostersSearchPaths = new PathAbstractions.SearchDefinition(false, ".mesh", false, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalPrefabs", null),
		new PathAbstractions.SearchPathMods("Prefabs"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Prefabs", null)
	});

	// Token: 0x04007B36 RID: 31542
	public static readonly PathAbstractions.SearchDefinition RwgStampsSearchPaths = new PathAbstractions.SearchDefinition(false, "", true, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalStamps", null),
		new PathAbstractions.SearchPathMods("Stamps"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Stamps", null)
	});

	// Token: 0x04007B37 RID: 31543
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool cacheEnabled;

	// Token: 0x0200148E RID: 5262
	public enum EAbstractedLocationType
	{
		// Token: 0x04007B39 RID: 31545
		HostSave,
		// Token: 0x04007B3A RID: 31546
		LocalSave,
		// Token: 0x04007B3B RID: 31547
		UserDataPath,
		// Token: 0x04007B3C RID: 31548
		Mods,
		// Token: 0x04007B3D RID: 31549
		GameData,
		// Token: 0x04007B3E RID: 31550
		None
	}

	// Token: 0x0200148F RID: 5263
	public readonly struct AbstractedLocation : IEquatable<PathAbstractions.AbstractedLocation>, IComparable<PathAbstractions.AbstractedLocation>, IComparable
	{
		// Token: 0x17001398 RID: 5016
		// (get) Token: 0x0600A4C8 RID: 42184 RVA: 0x003D7C1D File Offset: 0x003D5E1D
		public string FullPath
		{
			get
			{
				return this.Folder + "/" + this.FileNameNoExtension + this.Extension;
			}
		}

		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x0600A4C9 RID: 42185 RVA: 0x003D7C3B File Offset: 0x003D5E3B
		public string FullPathNoExtension
		{
			get
			{
				return this.Folder + "/" + this.FileNameNoExtension;
			}
		}

		// Token: 0x0600A4CA RID: 42186 RVA: 0x003D7C54 File Offset: 0x003D5E54
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

		// Token: 0x0600A4CB RID: 42187 RVA: 0x003D7D20 File Offset: 0x003D5F20
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

		// Token: 0x0600A4CC RID: 42188 RVA: 0x003D7DA3 File Offset: 0x003D5FA3
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

		// Token: 0x0600A4CD RID: 42189 RVA: 0x003D7DD0 File Offset: 0x003D5FD0
		public bool Equals(PathAbstractions.AbstractedLocation _other)
		{
			return this.Type == _other.Type && (this.Type == PathAbstractions.EAbstractedLocationType.None || (this.IsFolder == _other.IsFolder && string.Equals(this.Name, _other.Name) && GameIO.PathsEquals(this.FullPath, _other.FullPath, true)));
		}

		// Token: 0x0600A4CE RID: 42190 RVA: 0x003D7E30 File Offset: 0x003D6030
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

		// Token: 0x0600A4CF RID: 42191 RVA: 0x003D7E5A File Offset: 0x003D605A
		public static bool operator ==(PathAbstractions.AbstractedLocation _a, PathAbstractions.AbstractedLocation _b)
		{
			return _a.Equals(_b);
		}

		// Token: 0x0600A4D0 RID: 42192 RVA: 0x003D7E64 File Offset: 0x003D6064
		public static bool operator !=(PathAbstractions.AbstractedLocation _a, PathAbstractions.AbstractedLocation _b)
		{
			return !(_a == _b);
		}

		// Token: 0x0600A4D1 RID: 42193 RVA: 0x003D7E70 File Offset: 0x003D6070
		public override int GetHashCode()
		{
			return (((((this.Name != null) ? this.Name.GetHashCode() : 0) * 397) ^ (int)this.Type) * 397) ^ ((this.FullPath != null) ? this.FullPath.GetHashCode() : 0);
		}

		// Token: 0x0600A4D2 RID: 42194 RVA: 0x003D7EC0 File Offset: 0x003D60C0
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

		// Token: 0x0600A4D3 RID: 42195 RVA: 0x003D7F18 File Offset: 0x003D6118
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

		// Token: 0x0600A4D4 RID: 42196 RVA: 0x003D7F98 File Offset: 0x003D6198
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

		// Token: 0x0600A4D5 RID: 42197 RVA: 0x003D8024 File Offset: 0x003D6224
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

		// Token: 0x04007B3F RID: 31551
		public static readonly PathAbstractions.AbstractedLocation None = new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.None, null, null, null, false, null);

		// Token: 0x04007B40 RID: 31552
		public readonly PathAbstractions.EAbstractedLocationType Type;

		// Token: 0x04007B41 RID: 31553
		public readonly string Name;

		// Token: 0x04007B42 RID: 31554
		public readonly string Folder;

		// Token: 0x04007B43 RID: 31555
		public readonly string RelativePath;

		// Token: 0x04007B44 RID: 31556
		public readonly string FileNameNoExtension;

		// Token: 0x04007B45 RID: 31557
		public readonly string Extension;

		// Token: 0x04007B46 RID: 31558
		public readonly bool IsFolder;

		// Token: 0x04007B47 RID: 31559
		public readonly Mod ContainingMod;

		// Token: 0x04007B48 RID: 31560
		public readonly UserDataStorageType StorageType;
	}

	// Token: 0x02001490 RID: 5264
	public class SearchDefinition
	{
		// Token: 0x0600A4D7 RID: 42199 RVA: 0x003D806C File Offset: 0x003D626C
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

		// Token: 0x0600A4D8 RID: 42200 RVA: 0x003D80EC File Offset: 0x003D62EC
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

		// Token: 0x0600A4D9 RID: 42201 RVA: 0x003D8180 File Offset: 0x003D6380
		public List<PathAbstractions.AbstractedLocation> GetAvailablePathsList(Regex _nameMatch = null, bool _ignoreDuplicateNames = false, PathAbstractions.EAbstractedLocationType? _onlyLocationType = null)
		{
			List<PathAbstractions.AbstractedLocation> list = new List<PathAbstractions.AbstractedLocation>();
			this.GetAvailablePathsList(ref list, _nameMatch, _ignoreDuplicateNames, _onlyLocationType);
			return list;
		}

		// Token: 0x0600A4DA RID: 42202 RVA: 0x003D81A0 File Offset: 0x003D63A0
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

		// Token: 0x0600A4DB RID: 42203 RVA: 0x003D8224 File Offset: 0x003D6424
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

		// Token: 0x0600A4DC RID: 42204 RVA: 0x003D8278 File Offset: 0x003D6478
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

		// Token: 0x0600A4DD RID: 42205 RVA: 0x003D82E8 File Offset: 0x003D64E8
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

		// Token: 0x0600A4DE RID: 42206 RVA: 0x003D8338 File Offset: 0x003D6538
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

		// Token: 0x0600A4DF RID: 42207 RVA: 0x003D839C File Offset: 0x003D659C
		public void InvalidateCache()
		{
			foreach (PathAbstractions.SearchPath searchPath in this.paths)
			{
				searchPath.InvalidateCache();
			}
		}

		// Token: 0x04007B49 RID: 31561
		public readonly bool IsFolder;

		// Token: 0x04007B4A RID: 31562
		public readonly string Extension;

		// Token: 0x04007B4B RID: 31563
		public readonly bool RemoveExtension;

		// Token: 0x04007B4C RID: 31564
		public readonly bool Recursive;

		// Token: 0x04007B4D RID: 31565
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<PathAbstractions.SearchPath> paths;
	}

	// Token: 0x02001491 RID: 5265
	public abstract class SearchPath
	{
		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x0600A4E0 RID: 42208 RVA: 0x00020119 File Offset: 0x0001E319
		public virtual bool CanMatch
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A4E1 RID: 42209 RVA: 0x003D83EC File Offset: 0x003D65EC
		[PublicizedFrom(EAccessModifier.Protected)]
		public SearchPath(string _relativePath)
		{
			this.RelativePath = _relativePath;
		}

		// Token: 0x0600A4E2 RID: 42210 RVA: 0x003D8406 File Offset: 0x003D6606
		public void SetOwner(PathAbstractions.SearchDefinition _owner)
		{
			this.Owner = _owner;
		}

		// Token: 0x0600A4E3 RID: 42211
		public abstract PathAbstractions.AbstractedLocation GetLocation(string _name, UserDataStorageType? _userDataHint);

		// Token: 0x0600A4E4 RID: 42212
		public abstract void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames);

		// Token: 0x0600A4E5 RID: 42213 RVA: 0x003D8410 File Offset: 0x003D6610
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

		// Token: 0x0600A4E6 RID: 42214 RVA: 0x003D8510 File Offset: 0x003D6710
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

		// Token: 0x0600A4E7 RID: 42215 RVA: 0x003D8688 File Offset: 0x003D6888
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

		// Token: 0x0600A4E8 RID: 42216 RVA: 0x003D86A0 File Offset: 0x003D68A0
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

		// Token: 0x0600A4E9 RID: 42217 RVA: 0x003D8774 File Offset: 0x003D6974
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

		// Token: 0x0600A4EA RID: 42218 RVA: 0x003D8848 File Offset: 0x003D6A48
		public void InvalidateCache()
		{
			this.locationsCache.Clear();
			this.locationsCachePopulated = false;
		}

		// Token: 0x0600A4EB RID: 42219
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void PopulateCache();

		// Token: 0x0600A4EC RID: 42220
		public abstract PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint);

		// Token: 0x0600A4ED RID: 42221
		public abstract string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod);

		// Token: 0x0600A4EE RID: 42222
		public abstract bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType);

		// Token: 0x04007B4E RID: 31566
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathAbstractions.SearchDefinition Owner;

		// Token: 0x04007B4F RID: 31567
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string RelativePath;

		// Token: 0x04007B50 RID: 31568
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Dictionary<string, IList<PathAbstractions.AbstractedLocation>> locationsCache = new CaseInsensitiveStringDictionary<IList<PathAbstractions.AbstractedLocation>>();

		// Token: 0x04007B51 RID: 31569
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool locationsCachePopulated;
	}

	// Token: 0x02001494 RID: 5268
	public class SearchPathBasic : PathAbstractions.SearchPath
	{
		// Token: 0x0600A4F3 RID: 42227 RVA: 0x003D8887 File Offset: 0x003D6A87
		public SearchPathBasic(PathAbstractions.EAbstractedLocationType _locationType, Func<string> _basePath, string _relativePath, Func<bool> _canMatch = null)
			: base(_relativePath)
		{
			this.locationType = _locationType;
			this.basePath = _basePath;
			this.canMatch = _canMatch;
		}

		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x0600A4F4 RID: 42228 RVA: 0x003D88A6 File Offset: 0x003D6AA6
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

		// Token: 0x0600A4F5 RID: 42229 RVA: 0x003D88C0 File Offset: 0x003D6AC0
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

		// Token: 0x0600A4F6 RID: 42230 RVA: 0x003D891E File Offset: 0x003D6B1E
		public override void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			if (base.UseCache())
			{
				base.GetCachedPathList(_targetList, _nameMatch, _ignoreDuplicateNames);
				return;
			}
			base.getAvailablePathsSingleBase(_targetList, this.locationType, this.basePath() + "/" + this.RelativePath, _nameMatch, _ignoreDuplicateNames, null, null);
		}

		// Token: 0x0600A4F7 RID: 42231 RVA: 0x003D8960 File Offset: 0x003D6B60
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

		// Token: 0x0600A4F8 RID: 42232 RVA: 0x003D8A00 File Offset: 0x003D6C00
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, this.basePath() + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, null));
		}

		// Token: 0x0600A4F9 RID: 42233 RVA: 0x003D8A67 File Offset: 0x003D6C67
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			return this.basePath() + "/" + this.RelativePath;
		}

		// Token: 0x0600A4FA RID: 42234 RVA: 0x003D8A84 File Offset: 0x003D6C84
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == this.locationType;
		}

		// Token: 0x04007B54 RID: 31572
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly PathAbstractions.EAbstractedLocationType locationType;

		// Token: 0x04007B55 RID: 31573
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<string> basePath;

		// Token: 0x04007B56 RID: 31574
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<bool> canMatch;
	}

	// Token: 0x02001495 RID: 5269
	public class SearchPathUserData : PathAbstractions.SearchPath
	{
		// Token: 0x0600A4FB RID: 42235 RVA: 0x003D8A8F File Offset: 0x003D6C8F
		public SearchPathUserData(string _relativePath, Func<UserDataStorageType> _getStorageTarget)
			: base(_relativePath)
		{
			this.getStorageTarget = _getStorageTarget;
		}

		// Token: 0x0600A4FC RID: 42236 RVA: 0x003D8AA0 File Offset: 0x003D6CA0
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

		// Token: 0x0600A4FD RID: 42237 RVA: 0x003D8B00 File Offset: 0x003D6D00
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

		// Token: 0x0600A4FE RID: 42238 RVA: 0x003D8B5C File Offset: 0x003D6D5C
		public override void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			if (base.UseCache())
			{
				base.GetCachedPathList(_targetList, _nameMatch, _ignoreDuplicateNames);
				return;
			}
			this.CollectLocations(_targetList, _nameMatch, _ignoreDuplicateNames);
		}

		// Token: 0x0600A4FF RID: 42239 RVA: 0x003D8B79 File Offset: 0x003D6D79
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetCacheLookupName(string _name, UserDataStorageType _storageType)
		{
			return string.Format("{0}_{1}", _name, _storageType);
		}

		// Token: 0x0600A500 RID: 42240 RVA: 0x003D8B8C File Offset: 0x003D6D8C
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

		// Token: 0x0600A501 RID: 42241 RVA: 0x003D8C34 File Offset: 0x003D6E34
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			string userGameDataDir = GameIO.GetUserGameDataDir((_userStorageHint != null) ? _userStorageHint.Value : this.getStorageTarget());
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, userGameDataDir + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, null));
		}

		// Token: 0x0600A502 RID: 42242 RVA: 0x003D8CB4 File Offset: 0x003D6EB4
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			return GameIO.GetUserGameDataDir(this.getStorageTarget()) + "/" + this.RelativePath;
		}

		// Token: 0x0600A503 RID: 42243 RVA: 0x003D8CD6 File Offset: 0x003D6ED6
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.UserDataPath;
		}

		// Token: 0x04007B57 RID: 31575
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<UserDataStorageType> getStorageTarget;
	}

	// Token: 0x02001496 RID: 5270
	public class SearchPathSaves : PathAbstractions.SearchPath
	{
		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x0600A504 RID: 42244 RVA: 0x003D8CDC File Offset: 0x003D6EDC
		public override bool CanMatch
		{
			get
			{
				return SingletonMonoBehaviour<ConnectionManager>.Instance != null;
			}
		}

		// Token: 0x0600A505 RID: 42245 RVA: 0x003D8CE9 File Offset: 0x003D6EE9
		public SearchPathSaves(string _relativePath)
			: base(_relativePath)
		{
		}

		// Token: 0x0600A506 RID: 42246 RVA: 0x003D8CF2 File Offset: 0x003D6EF2
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

		// Token: 0x0600A507 RID: 42247 RVA: 0x003D8D2C File Offset: 0x003D6F2C
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

		// Token: 0x0600A508 RID: 42248 RVA: 0x003D8DC8 File Offset: 0x003D6FC8
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

		// Token: 0x0600A509 RID: 42249 RVA: 0x003D8E5C File Offset: 0x003D705C
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

		// Token: 0x0600A50A RID: 42250 RVA: 0x003D8F0C File Offset: 0x003D710C
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

		// Token: 0x0600A50B RID: 42251 RVA: 0x003D8F8C File Offset: 0x003D718C
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			string item = this.GetSaveFolder().Item1;
			if (item == null)
			{
				return null;
			}
			return item + "/" + this.RelativePath;
		}

		// Token: 0x0600A50C RID: 42252 RVA: 0x0033B509 File Offset: 0x00339709
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.HostSave || _locationType == PathAbstractions.EAbstractedLocationType.LocalSave;
		}
	}

	// Token: 0x02001497 RID: 5271
	public class SearchPathMods : PathAbstractions.SearchPath
	{
		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x0600A50D RID: 42253 RVA: 0x003D8CDC File Offset: 0x003D6EDC
		public override bool CanMatch
		{
			get
			{
				return SingletonMonoBehaviour<ConnectionManager>.Instance != null;
			}
		}

		// Token: 0x0600A50E RID: 42254 RVA: 0x003D8CE9 File Offset: 0x003D6EE9
		public SearchPathMods(string _relativePath)
			: base(_relativePath)
		{
		}

		// Token: 0x0600A50F RID: 42255 RVA: 0x003D8FBC File Offset: 0x003D71BC
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

		// Token: 0x0600A510 RID: 42256 RVA: 0x003D9054 File Offset: 0x003D7254
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

		// Token: 0x0600A511 RID: 42257 RVA: 0x003D90D4 File Offset: 0x003D72D4
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

		// Token: 0x0600A512 RID: 42258 RVA: 0x003D91CC File Offset: 0x003D73CC
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			if (_mod == null)
			{
				throw new ArgumentException("BuildLocation for LocationType==Mods requires a loaded Mod", "_mod");
			}
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, _mod.Path + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, _mod));
		}

		// Token: 0x0600A513 RID: 42259 RVA: 0x003D9244 File Offset: 0x003D7444
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			if (_mod == null)
			{
				throw new ArgumentException("GetBasePath for LocationType==Mods requires a loaded Mod", "_mod");
			}
			return _mod.Path + "/" + this.RelativePath;
		}

		// Token: 0x0600A514 RID: 42260 RVA: 0x003D926F File Offset: 0x003D746F
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.Mods;
		}
	}

	// Token: 0x02001498 RID: 5272
	public static class Contextual
	{
		// Token: 0x0600A515 RID: 42261 RVA: 0x003D9278 File Offset: 0x003D7478
		public static PathAbstractions.AbstractedLocation FindDownloadedRemoteWorld(string _saveDir)
		{
			string text = Path.Combine(_saveDir, "World");
			if (!SdDirectory.Exists(text))
			{
				return PathAbstractions.AbstractedLocation.None;
			}
			return new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.LocalSave, "World", text, null, true, null);
		}

		// Token: 0x0600A516 RID: 42262 RVA: 0x003D92B0 File Offset: 0x003D74B0
		public static void FindActiveWorld(out string _name, out PathAbstractions.AbstractedLocation _location)
		{
			_name = GamePrefs.GetString(EnumGamePrefs.GameWorld);
			PathAbstractions.EAbstractedLocationType @int = (PathAbstractions.EAbstractedLocationType)GamePrefs.GetInt(EnumGamePrefs.GameWorldLocationType);
			UserDataStorageType int2 = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType);
			_location = PathAbstractions.Contextual.FindWorld(_name, @int, int2);
		}

		// Token: 0x0600A517 RID: 42263 RVA: 0x003D92EB File Offset: 0x003D74EB
		public static PathAbstractions.AbstractedLocation FindActiveWorldLocation()
		{
			return PathAbstractions.Contextual.FindWorld(GamePrefs.GetString(EnumGamePrefs.GameWorld), (PathAbstractions.EAbstractedLocationType)GamePrefs.GetInt(EnumGamePrefs.GameWorldLocationType), (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType));
		}

		// Token: 0x0600A518 RID: 42264 RVA: 0x003D9310 File Offset: 0x003D7510
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

		// Token: 0x0600A519 RID: 42265 RVA: 0x003D937C File Offset: 0x003D757C
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
