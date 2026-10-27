using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Profiling;

// Token: 0x020007AA RID: 1962
public static class NetPackageManager
{
	// Token: 0x06003A68 RID: 14952 RVA: 0x0017C584 File Offset: 0x0017A784
	[PublicizedFrom(EAccessModifier.Private)]
	static NetPackageManager()
	{
		Log.Out("NetPackageManager Init");
		ReflectionHelpers.FindTypesImplementingBase(typeof(NetPackage), delegate(Type _type)
		{
			NetPackageManager.knownPackageTypes.Add(_type.Name, _type);
		}, false);
	}

	// Token: 0x06003A69 RID: 14953 RVA: 0x0017C5D4 File Offset: 0x0017A7D4
	public static void ResetMappings()
	{
		NetPackageManager.packageIdToClass = null;
		NetPackageManager.packageIdToPackageInformation = null;
		NetPackageManager.packageClassToPackageId = null;
	}

	// Token: 0x06003A6A RID: 14954 RVA: 0x0017C5E8 File Offset: 0x0017A7E8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void AddPackageMapping(int _id, Type _type)
	{
		NetPackageManager.packageIdToClass[_id] = _type;
		NetPackageManager.packageClassToPackageId[_type] = _id;
		NetPackageManager.IPackageInformation packageInformation = (NetPackageManager.IPackageInformation)typeof(NetPackageManager.NetPackageInformation<>).MakeGenericType(new Type[] { _type }).GetProperty("Instance").GetValue(null, null);
		NetPackageManager.packageIdToPackageInformation[_id] = packageInformation;
	}

	// Token: 0x06003A6B RID: 14955 RVA: 0x0017C641 File Offset: 0x0017A841
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetupBaseMapping()
	{
		NetPackageManager.packageIdToClass = new Type[NetPackageManager.KnownPackageCount];
		NetPackageManager.packageIdToPackageInformation = new NetPackageManager.IPackageInformation[NetPackageManager.KnownPackageCount];
		NetPackageManager.packageClassToPackageId = new Dictionary<Type, int>();
		NetPackageManager.AddPackageMapping(0, NetPackageManager.packageIdsType);
	}

	// Token: 0x06003A6C RID: 14956 RVA: 0x0017C678 File Offset: 0x0017A878
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

	// Token: 0x06003A6D RID: 14957 RVA: 0x0017C6F4 File Offset: 0x0017A8F4
	public static void StartClient()
	{
		NetPackageManager.ResetMappings();
		NetPackageManager.SetupBaseMapping();
	}

	// Token: 0x06003A6E RID: 14958 RVA: 0x0017C700 File Offset: 0x0017A900
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

	// Token: 0x06003A6F RID: 14959 RVA: 0x0017C782 File Offset: 0x0017A982
	public static NetPackage ParsePackage(PooledBinaryReader _reader, ClientInfo _sender)
	{
		NetPackage rawPackage = NetPackageManager.getPackageInfoByType((int)_reader.ReadUInt16()).GetRawPackage();
		rawPackage.Sender = _sender;
		rawPackage.read(_reader);
		return rawPackage;
	}

	// Token: 0x06003A70 RID: 14960 RVA: 0x0017C7A2 File Offset: 0x0017A9A2
	public static void FreePackage(NetPackage _package)
	{
		NetPackageManager.getPackageInfoByType(_package.PackageId).FreePackage(_package);
	}

	// Token: 0x06003A71 RID: 14961 RVA: 0x0017C7B5 File Offset: 0x0017A9B5
	[PublicizedFrom(EAccessModifier.Private)]
	public static NetPackageManager.IPackageInformation getPackageInfoByType(int _packageTypeId)
	{
		if (_packageTypeId >= NetPackageManager.packageIdToPackageInformation.Length || NetPackageManager.packageIdToPackageInformation[_packageTypeId] == null)
		{
			throw new NetPackageManager.UnknownNetPackageException(_packageTypeId);
		}
		return NetPackageManager.packageIdToPackageInformation[_packageTypeId];
	}

	// Token: 0x06003A72 RID: 14962 RVA: 0x0017C7D8 File Offset: 0x0017A9D8
	public static TPackage GetPackage<TPackage>() where TPackage : NetPackage
	{
		return NetPackageManager.NetPackageInformation<TPackage>.Instance.GetPackage();
	}

	// Token: 0x06003A73 RID: 14963 RVA: 0x0017C7E4 File Offset: 0x0017A9E4
	public static int GetPackageId(Type _type)
	{
		return NetPackageManager.packageClassToPackageId[_type];
	}

	// Token: 0x06003A74 RID: 14964 RVA: 0x0017C7F1 File Offset: 0x0017A9F1
	public static string GetPackageName(int _id)
	{
		return NetPackageManager.packageIdToClass[_id].ToString();
	}

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x06003A75 RID: 14965 RVA: 0x0017C7FF File Offset: 0x0017A9FF
	public static int KnownPackageCount
	{
		get
		{
			return NetPackageManager.knownPackageTypes.Count;
		}
	}

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x06003A76 RID: 14966 RVA: 0x0017C80B File Offset: 0x0017AA0B
	public static Type[] PackageMappings
	{
		get
		{
			return NetPackageManager.packageIdToClass;
		}
	}

	// Token: 0x06003A77 RID: 14967 RVA: 0x0017C814 File Offset: 0x0017AA14
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

	// Token: 0x04002FC0 RID: 12224
	[PublicizedFrom(EAccessModifier.Private)]
	public static Type[] packageIdToClass;

	// Token: 0x04002FC1 RID: 12225
	[PublicizedFrom(EAccessModifier.Private)]
	public static NetPackageManager.IPackageInformation[] packageIdToPackageInformation;

	// Token: 0x04002FC2 RID: 12226
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<Type, int> packageClassToPackageId;

	// Token: 0x04002FC3 RID: 12227
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, Type> knownPackageTypes = new CaseInsensitiveStringDictionary<Type>();

	// Token: 0x04002FC4 RID: 12228
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Type packageIdsType = typeof(NetPackagePackageIds);

	// Token: 0x020007AB RID: 1963
	[PublicizedFrom(EAccessModifier.Private)]
	public interface IPackageInformation
	{
		// Token: 0x06003A78 RID: 14968
		NetPackage GetRawPackage();

		// Token: 0x06003A79 RID: 14969
		void FreePackage(NetPackage _package);

		// Token: 0x06003A7A RID: 14970
		void GetStats(out int _packages, out int _totalSize);
	}

	// Token: 0x020007AC RID: 1964
	[PublicizedFrom(EAccessModifier.Private)]
	public class NetPackageInformation<TPackage> : NetPackageManager.IPackageInformation where TPackage : NetPackage
	{
		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06003A7B RID: 14971 RVA: 0x0017C8AF File Offset: 0x0017AAAF
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

		// Token: 0x06003A7C RID: 14972 RVA: 0x0017C8C8 File Offset: 0x0017AAC8
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

		// Token: 0x06003A7D RID: 14973 RVA: 0x0017CA00 File Offset: 0x0017AC00
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

		// Token: 0x06003A7E RID: 14974 RVA: 0x0017CA9C File Offset: 0x0017AC9C
		public NetPackage GetRawPackage()
		{
			return this.GetPackage();
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x0017CAAC File Offset: 0x0017ACAC
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

		// Token: 0x06003A80 RID: 14976 RVA: 0x0017CB38 File Offset: 0x0017AD38
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

		// Token: 0x06003A81 RID: 14977 RVA: 0x0017CB8C File Offset: 0x0017AD8C
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

		// Token: 0x04002FC5 RID: 12229
		[PublicizedFrom(EAccessModifier.Private)]
		public static NetPackageManager.NetPackageInformation<TPackage> instance;

		// Token: 0x04002FC6 RID: 12230
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly TPackage[] pool;

		// Token: 0x04002FC7 RID: 12231
		[PublicizedFrom(EAccessModifier.Private)]
		public int poolSize;

		// Token: 0x04002FC8 RID: 12232
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int capacity;

		// Token: 0x04002FC9 RID: 12233
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ConstructorInfo ctor;

		// Token: 0x04002FCA RID: 12234
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSampler = CustomSampler.Create("NPM.PI.GetPackage", false);

		// Token: 0x04002FCB RID: 12235
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerPool = CustomSampler.Create("Pooled", false);

		// Token: 0x04002FCC RID: 12236
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerNew = CustomSampler.Create("New", false);

		// Token: 0x04002FCD RID: 12237
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerType = CustomSampler.Create(typeof(TPackage).Name, false);

		// Token: 0x04002FCE RID: 12238
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSampler = CustomSampler.Create("NPM.PI.FreePackage", false);

		// Token: 0x04002FCF RID: 12239
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSamplerPool = CustomSampler.Create("ToPool", false);

		// Token: 0x04002FD0 RID: 12240
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSamplerCleanup = CustomSampler.Create("Cleanup", false);
	}

	// Token: 0x020007AD RID: 1965
	public class UnknownNetPackageException : Exception
	{
		// Token: 0x06003A82 RID: 14978 RVA: 0x0017CBFC File Offset: 0x0017ADFC
		public UnknownNetPackageException(int _packageId)
			: base("Unknown NetPackage ID: " + _packageId.ToString())
		{
		}
	}
}
