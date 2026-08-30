using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Profiling;

// Token: 0x02000795 RID: 1941
public static class NetPackageManager
{
	// Token: 0x060039B7 RID: 14775 RVA: 0x00178BB4 File Offset: 0x00176DB4
	[PublicizedFrom(EAccessModifier.Private)]
	static NetPackageManager()
	{
		Log.Out("NetPackageManager Init");
		ReflectionHelpers.FindTypesImplementingBase(typeof(NetPackage), delegate(Type _type)
		{
			NetPackageManager.knownPackageTypes.Add(_type.Name, _type);
		}, false);
	}

	// Token: 0x060039B8 RID: 14776 RVA: 0x00178C04 File Offset: 0x00176E04
	public static void ResetMappings()
	{
		NetPackageManager.packageIdToClass = null;
		NetPackageManager.packageIdToPackageInformation = null;
		NetPackageManager.packageClassToPackageId = null;
	}

	// Token: 0x060039B9 RID: 14777 RVA: 0x00178C18 File Offset: 0x00176E18
	[PublicizedFrom(EAccessModifier.Private)]
	public static void AddPackageMapping(int _id, Type _type)
	{
		NetPackageManager.packageIdToClass[_id] = _type;
		NetPackageManager.packageClassToPackageId[_type] = _id;
		NetPackageManager.IPackageInformation packageInformation = (NetPackageManager.IPackageInformation)typeof(NetPackageManager.NetPackageInformation<>).MakeGenericType(new Type[] { _type }).GetProperty("Instance").GetValue(null, null);
		NetPackageManager.packageIdToPackageInformation[_id] = packageInformation;
	}

	// Token: 0x060039BA RID: 14778 RVA: 0x00178C71 File Offset: 0x00176E71
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetupBaseMapping()
	{
		NetPackageManager.packageIdToClass = new Type[NetPackageManager.KnownPackageCount];
		NetPackageManager.packageIdToPackageInformation = new NetPackageManager.IPackageInformation[NetPackageManager.KnownPackageCount];
		NetPackageManager.packageClassToPackageId = new Dictionary<Type, int>();
		NetPackageManager.AddPackageMapping(0, NetPackageManager.packageIdsType);
	}

	// Token: 0x060039BB RID: 14779 RVA: 0x00178CA8 File Offset: 0x00176EA8
	public static void StartServer()
	{
		NetPackageManager.ResetMappings();
		NetPackageManager.SetupBaseMapping();
		int num = 1;
		foreach (KeyValuePair<string, Type> keyValuePair in NetPackageManager.knownPackageTypes)
		{
			if (!(keyValuePair.Value == NetPackageManager.packageIdsType))
			{
				NetPackageManager.AddPackageMapping(num, keyValuePair.Value);
				num++;
			}
		}
	}

	// Token: 0x060039BC RID: 14780 RVA: 0x00178D24 File Offset: 0x00176F24
	public static void StartClient()
	{
		NetPackageManager.ResetMappings();
		NetPackageManager.SetupBaseMapping();
	}

	// Token: 0x060039BD RID: 14781 RVA: 0x00178D30 File Offset: 0x00176F30
	public static void IdMappingsReceived(string[] _mappings)
	{
		for (int i = 0; i < _mappings.Length; i++)
		{
			Type type;
			if (!NetPackageManager.knownPackageTypes.TryGetValue(_mappings[i], out type))
			{
				Log.Error("[NET] Unknown package type " + _mappings[i] + ", can not proceed connecting to server");
				SingletonMonoBehaviour<ConnectionManager>.Instance.Disconnect();
				GameManager.Instance.ShowMessagePlayerDenied(new GameUtils.KickPlayerData(GameUtils.EKickReason.UnknownNetPackage, 0, default(DateTime), ""));
				return;
			}
			if (!(type == NetPackageManager.packageIdsType))
			{
				NetPackageManager.AddPackageMapping(i, type);
			}
		}
	}

	// Token: 0x060039BE RID: 14782 RVA: 0x00178DB2 File Offset: 0x00176FB2
	public static NetPackage ParsePackage(PooledBinaryReader _reader, ClientInfo _sender)
	{
		NetPackage rawPackage = NetPackageManager.getPackageInfoByType((int)_reader.ReadUInt16()).GetRawPackage();
		rawPackage.Sender = _sender;
		rawPackage.read(_reader);
		return rawPackage;
	}

	// Token: 0x060039BF RID: 14783 RVA: 0x00178DD2 File Offset: 0x00176FD2
	public static void FreePackage(NetPackage _package)
	{
		NetPackageManager.getPackageInfoByType(_package.PackageId).FreePackage(_package);
	}

	// Token: 0x060039C0 RID: 14784 RVA: 0x00178DE5 File Offset: 0x00176FE5
	[PublicizedFrom(EAccessModifier.Private)]
	public static NetPackageManager.IPackageInformation getPackageInfoByType(int _packageTypeId)
	{
		if (_packageTypeId >= NetPackageManager.packageIdToPackageInformation.Length || NetPackageManager.packageIdToPackageInformation[_packageTypeId] == null)
		{
			throw new NetPackageManager.UnknownNetPackageException(_packageTypeId);
		}
		return NetPackageManager.packageIdToPackageInformation[_packageTypeId];
	}

	// Token: 0x060039C1 RID: 14785 RVA: 0x00178E08 File Offset: 0x00177008
	public static TPackage GetPackage<TPackage>() where TPackage : NetPackage
	{
		return NetPackageManager.NetPackageInformation<TPackage>.Instance.GetPackage();
	}

	// Token: 0x060039C2 RID: 14786 RVA: 0x00178E14 File Offset: 0x00177014
	public static int GetPackageId(Type _type)
	{
		return NetPackageManager.packageClassToPackageId[_type];
	}

	// Token: 0x060039C3 RID: 14787 RVA: 0x00178E21 File Offset: 0x00177021
	public static string GetPackageName(int _id)
	{
		return NetPackageManager.packageIdToClass[_id].ToString();
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x060039C4 RID: 14788 RVA: 0x00178E2F File Offset: 0x0017702F
	public static int KnownPackageCount
	{
		get
		{
			return NetPackageManager.knownPackageTypes.Count;
		}
	}

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x060039C5 RID: 14789 RVA: 0x00178E3B File Offset: 0x0017703B
	public static Type[] PackageMappings
	{
		get
		{
			return NetPackageManager.packageIdToClass;
		}
	}

	// Token: 0x060039C6 RID: 14790 RVA: 0x00178E44 File Offset: 0x00177044
	public static void LogStats()
	{
		Log.Out("NetPackage pool stats:");
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < NetPackageManager.packageIdToPackageInformation.Length; i++)
		{
			NetPackageManager.IPackageInformation packageInformation = NetPackageManager.packageIdToPackageInformation[i];
			if (packageInformation != null)
			{
				int num3;
				int num4;
				packageInformation.GetStats(out num3, out num4);
				Log.Out("    {0}: {1} packages, {2} Bytes", new object[]
				{
					NetPackageManager.GetPackageName(i),
					num3,
					num4
				});
				num += num3;
				num2 += num4;
			}
		}
		Log.Out("  Total: {0} packages, {1} Bytes", new object[] { num, num2 });
	}

	// Token: 0x04002F31 RID: 12081
	[PublicizedFrom(EAccessModifier.Private)]
	public static Type[] packageIdToClass;

	// Token: 0x04002F32 RID: 12082
	[PublicizedFrom(EAccessModifier.Private)]
	public static NetPackageManager.IPackageInformation[] packageIdToPackageInformation;

	// Token: 0x04002F33 RID: 12083
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<Type, int> packageClassToPackageId;

	// Token: 0x04002F34 RID: 12084
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, Type> knownPackageTypes = new CaseInsensitiveStringDictionary<Type>();

	// Token: 0x04002F35 RID: 12085
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Type packageIdsType = typeof(NetPackagePackageIds);

	// Token: 0x02000796 RID: 1942
	[PublicizedFrom(EAccessModifier.Private)]
	public interface IPackageInformation
	{
		// Token: 0x060039C7 RID: 14791
		NetPackage GetRawPackage();

		// Token: 0x060039C8 RID: 14792
		void FreePackage(NetPackage _package);

		// Token: 0x060039C9 RID: 14793
		void GetStats(out int _packages, out int _totalSize);
	}

	// Token: 0x02000797 RID: 1943
	[PublicizedFrom(EAccessModifier.Private)]
	public class NetPackageInformation<TPackage> : NetPackageManager.IPackageInformation where TPackage : NetPackage
	{
		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x060039CA RID: 14794 RVA: 0x00178EDF File Offset: 0x001770DF
		public static NetPackageManager.NetPackageInformation<TPackage> Instance
		{
			get
			{
				if (NetPackageManager.NetPackageInformation<TPackage>.instance == null)
				{
					NetPackageManager.NetPackageInformation<TPackage>.instance = new NetPackageManager.NetPackageInformation<TPackage>();
				}
				return NetPackageManager.NetPackageInformation<TPackage>.instance;
			}
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x00178EF8 File Offset: 0x001770F8
		[PublicizedFrom(EAccessModifier.Private)]
		public NetPackageInformation()
		{
			Type typeFromHandle = typeof(TPackage);
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			this.ctor = typeFromHandle.GetConstructor(bindingFlags, null, CallingConventions.Any, Type.EmptyTypes, null);
			if (typeof(IMemoryPoolableObject).IsAssignableFrom(typeFromHandle))
			{
				this.capacity = 10;
				MethodInfo method = typeFromHandle.GetMethod("GetPoolSize", BindingFlags.Static | BindingFlags.Public, null, Type.EmptyTypes, null);
				if (method != null)
				{
					if (method.ReturnType == typeof(int))
					{
						this.capacity = (int)method.Invoke(null, null);
					}
					else
					{
						Log.Warning("Poolable NetPackage has GetPoolSize method with wrong return type");
					}
				}
				this.pool = new TPackage[this.capacity];
			}
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x00179030 File Offset: 0x00177230
		public TPackage GetPackage()
		{
			if (this.pool != null)
			{
				TPackage[] array = this.pool;
				lock (array)
				{
					if (this.poolSize > 0)
					{
						this.poolSize--;
						TPackage tpackage = this.pool[this.poolSize];
						this.pool[this.poolSize] = default(TPackage);
						return tpackage;
					}
				}
			}
			return (TPackage)((object)this.ctor.Invoke(null));
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x001790CC File Offset: 0x001772CC
		public NetPackage GetRawPackage()
		{
			return this.GetPackage();
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x001790DC File Offset: 0x001772DC
		public void FreePackage(NetPackage _package)
		{
			if (this.pool == null)
			{
				return;
			}
			IMemoryPoolableObject memoryPoolableObject = (IMemoryPoolableObject)_package;
			TPackage[] array = this.pool;
			lock (array)
			{
				if (this.poolSize < this.capacity)
				{
					memoryPoolableObject.Reset();
					this.pool[this.poolSize] = (TPackage)((object)_package);
					this.poolSize++;
				}
				else
				{
					memoryPoolableObject.Cleanup();
				}
			}
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x00179168 File Offset: 0x00177368
		public void GetStats(out int _packages, out int _totalSize)
		{
			_packages = 0;
			_totalSize = 0;
			if (this.pool == null)
			{
				return;
			}
			TPackage[] array = this.pool;
			lock (array)
			{
				_packages = this.poolSize;
			}
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x001791BC File Offset: 0x001773BC
		public void Cleanup()
		{
			if (this.pool == null)
			{
				return;
			}
			TPackage[] array = this.pool;
			lock (array)
			{
				for (int i = 0; i < this.poolSize; i++)
				{
					((IMemoryPoolableObject)((object)this.pool[i])).Cleanup();
				}
			}
		}

		// Token: 0x04002F36 RID: 12086
		[PublicizedFrom(EAccessModifier.Private)]
		public static NetPackageManager.NetPackageInformation<TPackage> instance;

		// Token: 0x04002F37 RID: 12087
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly TPackage[] pool;

		// Token: 0x04002F38 RID: 12088
		[PublicizedFrom(EAccessModifier.Private)]
		public int poolSize;

		// Token: 0x04002F39 RID: 12089
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int capacity;

		// Token: 0x04002F3A RID: 12090
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ConstructorInfo ctor;

		// Token: 0x04002F3B RID: 12091
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSampler = CustomSampler.Create("NPM.PI.GetPackage", false);

		// Token: 0x04002F3C RID: 12092
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerPool = CustomSampler.Create("Pooled", false);

		// Token: 0x04002F3D RID: 12093
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerNew = CustomSampler.Create("New", false);

		// Token: 0x04002F3E RID: 12094
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerType = CustomSampler.Create(typeof(TPackage).Name, false);

		// Token: 0x04002F3F RID: 12095
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSampler = CustomSampler.Create("NPM.PI.FreePackage", false);

		// Token: 0x04002F40 RID: 12096
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSamplerPool = CustomSampler.Create("ToPool", false);

		// Token: 0x04002F41 RID: 12097
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSamplerCleanup = CustomSampler.Create("Cleanup", false);
	}

	// Token: 0x02000798 RID: 1944
	public class UnknownNetPackageException : Exception
	{
		// Token: 0x060039D1 RID: 14801 RVA: 0x0017922C File Offset: 0x0017742C
		public UnknownNetPackageException(int _packageId)
			: base("Unknown NetPackage ID: " + _packageId.ToString())
		{
		}
	}
}
