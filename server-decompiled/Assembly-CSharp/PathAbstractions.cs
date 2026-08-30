using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Platform;

// Token: 0x0200146D RID: 5229
public static class PathAbstractions
{
	// Token: 0x0600A417 RID: 42007 RVA: 0x003D2DF0 File Offset: 0x003D0FF0
	public static void InvalidateCaches()
	{
		foreach (PathAbstractions.SearchDefinition searchDefinition in PathAbstractions.allSearchDefs)
		{
			searchDefinition.InvalidateCache();
		}
	}

	// Token: 0x17001352 RID: 4946
	// (get) Token: 0x0600A418 RID: 42008 RVA: 0x003D2E40 File Offset: 0x003D1040
	// (set) Token: 0x0600A419 RID: 42009 RVA: 0x003D2E47 File Offset: 0x003D1047
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

	// Token: 0x04007AB4 RID: 31412
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly List<PathAbstractions.SearchDefinition> allSearchDefs = new List<PathAbstractions.SearchDefinition>();

	// Token: 0x04007AB5 RID: 31413
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<string> deviceLocalUserDataPath = new Func<string>(GameIO.GetDeviceLocalUserGameDataDir);

	// Token: 0x04007AB6 RID: 31414
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<string> gameDataPath = new Func<string>(GameIO.GetApplicationPath);

	// Token: 0x04007AB7 RID: 31415
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Func<UserDataStorageType> getWorldsStorageType = () => (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType);

	// Token: 0x04007AB8 RID: 31416
	public static readonly PathAbstractions.SearchDefinition WorldsSearchPaths = new PathAbstractions.SearchDefinition(true, null, false, false, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathSaves("World"),
		new PathAbstractions.SearchPathUserData("GeneratedWorlds", PathAbstractions.getWorldsStorageType),
		new PathAbstractions.SearchPathMods("Worlds"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Worlds", null)
	});

	// Token: 0x04007AB9 RID: 31417
	public static readonly PathAbstractions.SearchDefinition PrefabsSearchPaths = new PathAbstractions.SearchDefinition(false, ".tts", true, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalPrefabs", null),
		new PathAbstractions.SearchPathMods("Prefabs"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Prefabs", null)
	});

	// Token: 0x04007ABA RID: 31418
	public static readonly PathAbstractions.SearchDefinition PrefabImpostersSearchPaths = new PathAbstractions.SearchDefinition(false, ".mesh", false, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalPrefabs", null),
		new PathAbstractions.SearchPathMods("Prefabs"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Prefabs", null)
	});

	// Token: 0x04007ABB RID: 31419
	public static readonly PathAbstractions.SearchDefinition RwgStampsSearchPaths = new PathAbstractions.SearchDefinition(false, "", true, true, new PathAbstractions.SearchPath[]
	{
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.UserDataPath, PathAbstractions.deviceLocalUserDataPath, "LocalStamps", null),
		new PathAbstractions.SearchPathMods("Stamps"),
		new PathAbstractions.SearchPathBasic(PathAbstractions.EAbstractedLocationType.GameData, PathAbstractions.gameDataPath, "Data/Stamps", null)
	});

	// Token: 0x04007ABC RID: 31420
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool cacheEnabled;

	// Token: 0x0200146E RID: 5230
	public enum EAbstractedLocationType
	{
		// Token: 0x04007ABE RID: 31422
		HostSave,
		// Token: 0x04007ABF RID: 31423
		LocalSave,
		// Token: 0x04007AC0 RID: 31424
		UserDataPath,
		// Token: 0x04007AC1 RID: 31425
		Mods,
		// Token: 0x04007AC2 RID: 31426
		GameData,
		// Token: 0x04007AC3 RID: 31427
		None
	}

	// Token: 0x0200146F RID: 5231
	public readonly struct AbstractedLocation : IEquatable<PathAbstractions.AbstractedLocation>, IComparable<PathAbstractions.AbstractedLocation>, IComparable
	{
		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x0600A41B RID: 42011 RVA: 0x003D2FE1 File Offset: 0x003D11E1
		public string FullPath
		{
			get
			{
				return this.Folder + "/" + this.FileNameNoExtension + this.Extension;
			}
		}

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x0600A41C RID: 42012 RVA: 0x003D2FFF File Offset: 0x003D11FF
		public string FullPathNoExtension
		{
			get
			{
				return this.Folder + "/" + this.FileNameNoExtension;
			}
		}

		// Token: 0x0600A41D RID: 42013 RVA: 0x003D3018 File Offset: 0x003D1218
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

		// Token: 0x0600A41E RID: 42014 RVA: 0x003D30E4 File Offset: 0x003D12E4
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

		// Token: 0x0600A41F RID: 42015 RVA: 0x003D3167 File Offset: 0x003D1367
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

		// Token: 0x0600A420 RID: 42016 RVA: 0x003D3194 File Offset: 0x003D1394
		public bool Equals(PathAbstractions.AbstractedLocation _other)
		{
			return this.Type == _other.Type && (this.Type == PathAbstractions.EAbstractedLocationType.None || (this.IsFolder == _other.IsFolder && string.Equals(this.Name, _other.Name) && GameIO.PathsEquals(this.FullPath, _other.FullPath, true)));
		}

		// Token: 0x0600A421 RID: 42017 RVA: 0x003D31F4 File Offset: 0x003D13F4
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

		// Token: 0x0600A422 RID: 42018 RVA: 0x003D321E File Offset: 0x003D141E
		public static bool operator ==(PathAbstractions.AbstractedLocation _a, PathAbstractions.AbstractedLocation _b)
		{
			return _a.Equals(_b);
		}

		// Token: 0x0600A423 RID: 42019 RVA: 0x003D3228 File Offset: 0x003D1428
		public static bool operator !=(PathAbstractions.AbstractedLocation _a, PathAbstractions.AbstractedLocation _b)
		{
			return !(_a == _b);
		}

		// Token: 0x0600A424 RID: 42020 RVA: 0x003D3234 File Offset: 0x003D1434
		public override int GetHashCode()
		{
			return (((((this.Name != null) ? this.Name.GetHashCode() : 0) * 397) ^ (int)this.Type) * 397) ^ ((this.FullPath != null) ? this.FullPath.GetHashCode() : 0);
		}

		// Token: 0x0600A425 RID: 42021 RVA: 0x003D3284 File Offset: 0x003D1484
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

		// Token: 0x0600A426 RID: 42022 RVA: 0x003D32DC File Offset: 0x003D14DC
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

		// Token: 0x0600A427 RID: 42023 RVA: 0x003D335C File Offset: 0x003D155C
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

		// Token: 0x0600A428 RID: 42024 RVA: 0x003D33E8 File Offset: 0x003D15E8
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

		// Token: 0x04007AC4 RID: 31428
		public static readonly PathAbstractions.AbstractedLocation None = new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.None, null, null, null, false, null);

		// Token: 0x04007AC5 RID: 31429
		public readonly PathAbstractions.EAbstractedLocationType Type;

		// Token: 0x04007AC6 RID: 31430
		public readonly string Name;

		// Token: 0x04007AC7 RID: 31431
		public readonly string Folder;

		// Token: 0x04007AC8 RID: 31432
		public readonly string RelativePath;

		// Token: 0x04007AC9 RID: 31433
		public readonly string FileNameNoExtension;

		// Token: 0x04007ACA RID: 31434
		public readonly string Extension;

		// Token: 0x04007ACB RID: 31435
		public readonly bool IsFolder;

		// Token: 0x04007ACC RID: 31436
		public readonly Mod ContainingMod;

		// Token: 0x04007ACD RID: 31437
		public readonly UserDataStorageType StorageType;
	}

	// Token: 0x02001470 RID: 5232
	public class SearchDefinition
	{
		// Token: 0x0600A42A RID: 42026 RVA: 0x003D3430 File Offset: 0x003D1630
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

		// Token: 0x0600A42B RID: 42027 RVA: 0x003D34B0 File Offset: 0x003D16B0
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

		// Token: 0x0600A42C RID: 42028 RVA: 0x003D3544 File Offset: 0x003D1744
		public List<PathAbstractions.AbstractedLocation> GetAvailablePathsList(Regex _nameMatch = null, bool _ignoreDuplicateNames = false, PathAbstractions.EAbstractedLocationType? _onlyLocationType = null)
		{
			List<PathAbstractions.AbstractedLocation> list = new List<PathAbstractions.AbstractedLocation>();
			this.GetAvailablePathsList(ref list, _nameMatch, _ignoreDuplicateNames, _onlyLocationType);
			return list;
		}

		// Token: 0x0600A42D RID: 42029 RVA: 0x003D3564 File Offset: 0x003D1764
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

		// Token: 0x0600A42E RID: 42030 RVA: 0x003D35E8 File Offset: 0x003D17E8
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

		// Token: 0x0600A42F RID: 42031 RVA: 0x003D363C File Offset: 0x003D183C
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

		// Token: 0x0600A430 RID: 42032 RVA: 0x003D36AC File Offset: 0x003D18AC
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

		// Token: 0x0600A431 RID: 42033 RVA: 0x003D36FC File Offset: 0x003D18FC
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

		// Token: 0x0600A432 RID: 42034 RVA: 0x003D3760 File Offset: 0x003D1960
		public void InvalidateCache()
		{
			foreach (PathAbstractions.SearchPath searchPath in this.paths)
			{
				searchPath.InvalidateCache();
			}
		}

		// Token: 0x04007ACE RID: 31438
		public readonly bool IsFolder;

		// Token: 0x04007ACF RID: 31439
		public readonly string Extension;

		// Token: 0x04007AD0 RID: 31440
		public readonly bool RemoveExtension;

		// Token: 0x04007AD1 RID: 31441
		public readonly bool Recursive;

		// Token: 0x04007AD2 RID: 31442
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<PathAbstractions.SearchPath> paths;
	}

	// Token: 0x02001471 RID: 5233
	public abstract class SearchPath
	{
		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x0600A433 RID: 42035 RVA: 0x00020021 File Offset: 0x0001E221
		public virtual bool CanMatch
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A434 RID: 42036 RVA: 0x003D37B0 File Offset: 0x003D19B0
		[PublicizedFrom(EAccessModifier.Protected)]
		public SearchPath(string _relativePath)
		{
			this.RelativePath = _relativePath;
		}

		// Token: 0x0600A435 RID: 42037 RVA: 0x003D37CA File Offset: 0x003D19CA
		public void SetOwner(PathAbstractions.SearchDefinition _owner)
		{
			this.Owner = _owner;
		}

		// Token: 0x0600A436 RID: 42038
		public abstract PathAbstractions.AbstractedLocation GetLocation(string _name, UserDataStorageType? _userDataHint);

		// Token: 0x0600A437 RID: 42039
		public abstract void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames);

		// Token: 0x0600A438 RID: 42040 RVA: 0x003D37D4 File Offset: 0x003D19D4
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

		// Token: 0x0600A439 RID: 42041 RVA: 0x003D38D4 File Offset: 0x003D1AD4
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

		// Token: 0x0600A43A RID: 42042 RVA: 0x003D3A4C File Offset: 0x003D1C4C
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

		// Token: 0x0600A43B RID: 42043 RVA: 0x003D3A64 File Offset: 0x003D1C64
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

		// Token: 0x0600A43C RID: 42044 RVA: 0x003D3B38 File Offset: 0x003D1D38
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

		// Token: 0x0600A43D RID: 42045 RVA: 0x003D3C0C File Offset: 0x003D1E0C
		public void InvalidateCache()
		{
			this.locationsCache.Clear();
			this.locationsCachePopulated = false;
		}

		// Token: 0x0600A43E RID: 42046
		[PublicizedFrom(EAccessModifier.Protected)]
		public abstract void PopulateCache();

		// Token: 0x0600A43F RID: 42047
		public abstract PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint);

		// Token: 0x0600A440 RID: 42048
		public abstract string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod);

		// Token: 0x0600A441 RID: 42049
		public abstract bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType);

		// Token: 0x04007AD3 RID: 31443
		[PublicizedFrom(EAccessModifier.Protected)]
		public PathAbstractions.SearchDefinition Owner;

		// Token: 0x04007AD4 RID: 31444
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string RelativePath;

		// Token: 0x04007AD5 RID: 31445
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Dictionary<string, IList<PathAbstractions.AbstractedLocation>> locationsCache = new CaseInsensitiveStringDictionary<IList<PathAbstractions.AbstractedLocation>>();

		// Token: 0x04007AD6 RID: 31446
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool locationsCachePopulated;
	}

	// Token: 0x02001474 RID: 5236
	public class SearchPathBasic : PathAbstractions.SearchPath
	{
		// Token: 0x0600A446 RID: 42054 RVA: 0x003D3C4B File Offset: 0x003D1E4B
		public SearchPathBasic(PathAbstractions.EAbstractedLocationType _locationType, Func<string> _basePath, string _relativePath, Func<bool> _canMatch = null)
			: base(_relativePath)
		{
			this.locationType = _locationType;
			this.basePath = _basePath;
			this.canMatch = _canMatch;
		}

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x0600A447 RID: 42055 RVA: 0x003D3C6A File Offset: 0x003D1E6A
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

		// Token: 0x0600A448 RID: 42056 RVA: 0x003D3C84 File Offset: 0x003D1E84
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

		// Token: 0x0600A449 RID: 42057 RVA: 0x003D3CE2 File Offset: 0x003D1EE2
		public override void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			if (base.UseCache())
			{
				base.GetCachedPathList(_targetList, _nameMatch, _ignoreDuplicateNames);
				return;
			}
			base.getAvailablePathsSingleBase(_targetList, this.locationType, this.basePath() + "/" + this.RelativePath, _nameMatch, _ignoreDuplicateNames, null, null);
		}

		// Token: 0x0600A44A RID: 42058 RVA: 0x003D3D24 File Offset: 0x003D1F24
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

		// Token: 0x0600A44B RID: 42059 RVA: 0x003D3DC4 File Offset: 0x003D1FC4
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, this.basePath() + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, null));
		}

		// Token: 0x0600A44C RID: 42060 RVA: 0x003D3E2B File Offset: 0x003D202B
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			return this.basePath() + "/" + this.RelativePath;
		}

		// Token: 0x0600A44D RID: 42061 RVA: 0x003D3E48 File Offset: 0x003D2048
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == this.locationType;
		}

		// Token: 0x04007AD9 RID: 31449
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly PathAbstractions.EAbstractedLocationType locationType;

		// Token: 0x04007ADA RID: 31450
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<string> basePath;

		// Token: 0x04007ADB RID: 31451
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<bool> canMatch;
	}

	// Token: 0x02001475 RID: 5237
	public class SearchPathUserData : PathAbstractions.SearchPath
	{
		// Token: 0x0600A44E RID: 42062 RVA: 0x003D3E53 File Offset: 0x003D2053
		public SearchPathUserData(string _relativePath, Func<UserDataStorageType> _getStorageTarget)
			: base(_relativePath)
		{
			this.getStorageTarget = _getStorageTarget;
		}

		// Token: 0x0600A44F RID: 42063 RVA: 0x003D3E64 File Offset: 0x003D2064
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

		// Token: 0x0600A450 RID: 42064 RVA: 0x003D3EC4 File Offset: 0x003D20C4
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

		// Token: 0x0600A451 RID: 42065 RVA: 0x003D3F20 File Offset: 0x003D2120
		public override void GetAvailablePathsList(List<PathAbstractions.AbstractedLocation> _targetList, Regex _nameMatch, bool _ignoreDuplicateNames)
		{
			if (base.UseCache())
			{
				base.GetCachedPathList(_targetList, _nameMatch, _ignoreDuplicateNames);
				return;
			}
			this.CollectLocations(_targetList, _nameMatch, _ignoreDuplicateNames);
		}

		// Token: 0x0600A452 RID: 42066 RVA: 0x003D3F3D File Offset: 0x003D213D
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetCacheLookupName(string _name, UserDataStorageType _storageType)
		{
			return string.Format("{0}_{1}", _name, _storageType);
		}

		// Token: 0x0600A453 RID: 42067 RVA: 0x003D3F50 File Offset: 0x003D2150
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

		// Token: 0x0600A454 RID: 42068 RVA: 0x003D3FF8 File Offset: 0x003D21F8
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			string userGameDataDir = GameIO.GetUserGameDataDir((_userStorageHint != null) ? _userStorageHint.Value : this.getStorageTarget());
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, userGameDataDir + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, null));
		}

		// Token: 0x0600A455 RID: 42069 RVA: 0x003D4078 File Offset: 0x003D2278
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			return GameIO.GetUserGameDataDir(this.getStorageTarget()) + "/" + this.RelativePath;
		}

		// Token: 0x0600A456 RID: 42070 RVA: 0x003D409A File Offset: 0x003D229A
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.UserDataPath;
		}

		// Token: 0x04007ADC RID: 31452
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<UserDataStorageType> getStorageTarget;
	}

	// Token: 0x02001476 RID: 5238
	public class SearchPathSaves : PathAbstractions.SearchPath
	{
		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x0600A457 RID: 42071 RVA: 0x003D40A0 File Offset: 0x003D22A0
		public override bool CanMatch
		{
			get
			{
				return SingletonMonoBehaviour<ConnectionManager>.Instance != null;
			}
		}

		// Token: 0x0600A458 RID: 42072 RVA: 0x003D40AD File Offset: 0x003D22AD
		public SearchPathSaves(string _relativePath)
			: base(_relativePath)
		{
		}

		// Token: 0x0600A459 RID: 42073 RVA: 0x003D40B6 File Offset: 0x003D22B6
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

		// Token: 0x0600A45A RID: 42074 RVA: 0x003D40F0 File Offset: 0x003D22F0
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

		// Token: 0x0600A45B RID: 42075 RVA: 0x003D418C File Offset: 0x003D238C
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

		// Token: 0x0600A45C RID: 42076 RVA: 0x003D4220 File Offset: 0x003D2420
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

		// Token: 0x0600A45D RID: 42077 RVA: 0x003D42D0 File Offset: 0x003D24D0
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

		// Token: 0x0600A45E RID: 42078 RVA: 0x003D4350 File Offset: 0x003D2550
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			string item = this.GetSaveFolder().Item1;
			if (item == null)
			{
				return null;
			}
			return item + "/" + this.RelativePath;
		}

		// Token: 0x0600A45F RID: 42079 RVA: 0x003376F1 File Offset: 0x003358F1
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.HostSave || _locationType == PathAbstractions.EAbstractedLocationType.LocalSave;
		}
	}

	// Token: 0x02001477 RID: 5239
	public class SearchPathMods : PathAbstractions.SearchPath
	{
		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x0600A460 RID: 42080 RVA: 0x003D40A0 File Offset: 0x003D22A0
		public override bool CanMatch
		{
			get
			{
				return SingletonMonoBehaviour<ConnectionManager>.Instance != null;
			}
		}

		// Token: 0x0600A461 RID: 42081 RVA: 0x003D40AD File Offset: 0x003D22AD
		public SearchPathMods(string _relativePath)
			: base(_relativePath)
		{
		}

		// Token: 0x0600A462 RID: 42082 RVA: 0x003D4380 File Offset: 0x003D2580
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

		// Token: 0x0600A463 RID: 42083 RVA: 0x003D4418 File Offset: 0x003D2618
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

		// Token: 0x0600A464 RID: 42084 RVA: 0x003D4498 File Offset: 0x003D2698
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

		// Token: 0x0600A465 RID: 42085 RVA: 0x003D4590 File Offset: 0x003D2790
		public override PathAbstractions.AbstractedLocation? TryBuildLocation(PathAbstractions.EAbstractedLocationType _locationType, string _subFolder, string _elementName, Mod _mod, UserDataStorageType? _userStorageHint = null)
		{
			if (_mod == null)
			{
				throw new ArgumentException("BuildLocation for LocationType==Mods requires a loaded Mod", "_mod");
			}
			return new PathAbstractions.AbstractedLocation?(new PathAbstractions.AbstractedLocation(_locationType, _elementName, _mod.Path + "/" + this.RelativePath + ((!string.IsNullOrEmpty(_subFolder)) ? ("/" + _subFolder) : ""), _subFolder, _elementName, this.Owner.Extension, this.Owner.IsFolder, _mod));
		}

		// Token: 0x0600A466 RID: 42086 RVA: 0x003D4608 File Offset: 0x003D2808
		public override string TryGetBasePath(PathAbstractions.EAbstractedLocationType _locationType, Mod _mod)
		{
			if (_mod == null)
			{
				throw new ArgumentException("GetBasePath for LocationType==Mods requires a loaded Mod", "_mod");
			}
			return _mod.Path + "/" + this.RelativePath;
		}

		// Token: 0x0600A467 RID: 42087 RVA: 0x003D4633 File Offset: 0x003D2833
		public override bool CanHandleLocationType(PathAbstractions.EAbstractedLocationType _locationType)
		{
			return _locationType == PathAbstractions.EAbstractedLocationType.Mods;
		}
	}

	// Token: 0x02001478 RID: 5240
	public static class Contextual
	{
		// Token: 0x0600A468 RID: 42088 RVA: 0x003D463C File Offset: 0x003D283C
		public static PathAbstractions.AbstractedLocation FindDownloadedRemoteWorld(string _saveDir)
		{
			string text = Path.Combine(_saveDir, "World");
			if (!SdDirectory.Exists(text))
			{
				return PathAbstractions.AbstractedLocation.None;
			}
			return new PathAbstractions.AbstractedLocation(PathAbstractions.EAbstractedLocationType.LocalSave, "World", text, null, true, null);
		}

		// Token: 0x0600A469 RID: 42089 RVA: 0x003D4674 File Offset: 0x003D2874
		public static void FindActiveWorld(out string _name, out PathAbstractions.AbstractedLocation _location)
		{
			_name = GamePrefs.GetString(EnumGamePrefs.GameWorld);
			PathAbstractions.EAbstractedLocationType @int = (PathAbstractions.EAbstractedLocationType)GamePrefs.GetInt(EnumGamePrefs.GameWorldLocationType);
			UserDataStorageType int2 = (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType);
			_location = PathAbstractions.Contextual.FindWorld(_name, @int, int2);
		}

		// Token: 0x0600A46A RID: 42090 RVA: 0x003D46AF File Offset: 0x003D28AF
		public static PathAbstractions.AbstractedLocation FindActiveWorldLocation()
		{
			return PathAbstractions.Contextual.FindWorld(GamePrefs.GetString(EnumGamePrefs.GameWorld), (PathAbstractions.EAbstractedLocationType)GamePrefs.GetInt(EnumGamePrefs.GameWorldLocationType), (UserDataStorageType)GamePrefs.GetInt(EnumGamePrefs.UserWorldStorageType));
		}

		// Token: 0x0600A46B RID: 42091 RVA: 0x003D46D4 File Offset: 0x003D28D4
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

		// Token: 0x0600A46C RID: 42092 RVA: 0x003D4740 File Offset: 0x003D2940
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
