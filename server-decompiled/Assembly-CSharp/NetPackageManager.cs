using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Profiling;

// Token: 0x020007A2 RID: 1954
public static class NetPackageManager
{
	// Token: 0x06003A51 RID: 14929 RVA: 0x0017B5EC File Offset: 0x001797EC
	[PublicizedFrom(EAccessModifier.Private)]
	static NetPackageManager()
	{
		Log.Out("NetPackageManager Init");
		ReflectionHelpers.FindTypesImplementingBase(typeof(NetPackage), delegate(Type _type)
		{
			NetPackageManager.knownPackageTypes.Add(_type.Name, _type);
		}, false);
	}

	// Token: 0x06003A52 RID: 14930 RVA: 0x0017B63C File Offset: 0x0017983C
	public static void ResetMappings()
	{
		NetPackageManager.packageIdToClass = null;
		NetPackageManager.packageIdToPackageInformation = null;
		NetPackageManager.packageClassToPackageId = null;
	}

	// Token: 0x06003A53 RID: 14931 RVA: 0x0017B650 File Offset: 0x00179850
	[PublicizedFrom(EAccessModifier.Private)]
	public static void AddPackageMapping(int _id, Type _type)
	{
		NetPackageManager.packageIdToClass[_id] = _type;
		NetPackageManager.packageClassToPackageId[_type] = _id;
		NetPackageManager.IPackageInformation packageInformation = (NetPackageManager.IPackageInformation)typeof(NetPackageManager.NetPackageInformation<>).MakeGenericType(new Type[] { _type }).GetProperty("Instance").GetValue(null, null);
		NetPackageManager.packageIdToPackageInformation[_id] = packageInformation;
	}

	// Token: 0x06003A54 RID: 14932 RVA: 0x0017B6A9 File Offset: 0x001798A9
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetupBaseMapping()
	{
		NetPackageManager.packageIdToClass = new Type[NetPackageManager.KnownPackageCount];
		NetPackageManager.packageIdToPackageInformation = new NetPackageManager.IPackageInformation[NetPackageManager.KnownPackageCount];
		NetPackageManager.packageClassToPackageId = new Dictionary<Type, int>();
		NetPackageManager.AddPackageMapping(0, NetPackageManager.packageIdsType);
	}

	// Token: 0x06003A55 RID: 14933 RVA: 0x0017B6E0 File Offset: 0x001798E0
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

	// Token: 0x06003A56 RID: 14934 RVA: 0x0017B75C File Offset: 0x0017995C
	public static void StartClient()
	{
		NetPackageManager.ResetMappings();
		NetPackageManager.SetupBaseMapping();
	}

	// Token: 0x06003A57 RID: 14935 RVA: 0x0017B768 File Offset: 0x00179968
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

	// Token: 0x06003A58 RID: 14936 RVA: 0x0017B7EA File Offset: 0x001799EA
	public static NetPackage ParsePackage(PooledBinaryReader _reader, ClientInfo _sender)
	{
		NetPackage rawPackage = NetPackageManager.getPackageInfoByType((int)_reader.ReadUInt16()).GetRawPackage();
		rawPackage.Sender = _sender;
		rawPackage.read(_reader);
		return rawPackage;
	}

	// Token: 0x06003A59 RID: 14937 RVA: 0x0017B80A File Offset: 0x00179A0A
	public static void FreePackage(NetPackage _package)
	{
		NetPackageManager.getPackageInfoByType(_package.PackageId).FreePackage(_package);
	}

	// Token: 0x06003A5A RID: 14938 RVA: 0x0017B81D File Offset: 0x00179A1D
	[PublicizedFrom(EAccessModifier.Private)]
	public static NetPackageManager.IPackageInformation getPackageInfoByType(int _packageTypeId)
	{
		if (_packageTypeId >= NetPackageManager.packageIdToPackageInformation.Length || NetPackageManager.packageIdToPackageInformation[_packageTypeId] == null)
		{
			throw new NetPackageManager.UnknownNetPackageException(_packageTypeId);
		}
		return NetPackageManager.packageIdToPackageInformation[_packageTypeId];
	}

	// Token: 0x06003A5B RID: 14939 RVA: 0x0017B840 File Offset: 0x00179A40
	public static TPackage GetPackage<TPackage>() where TPackage : NetPackage
	{
		return NetPackageManager.NetPackageInformation<TPackage>.Instance.GetPackage();
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x0017B84C File Offset: 0x00179A4C
	public static int GetPackageId(Type _type)
	{
		return NetPackageManager.packageClassToPackageId[_type];
	}

	// Token: 0x06003A5D RID: 14941 RVA: 0x0017B859 File Offset: 0x00179A59
	public static string GetPackageName(int _id)
	{
		return NetPackageManager.packageIdToClass[_id].ToString();
	}

	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x06003A5E RID: 14942 RVA: 0x0017B867 File Offset: 0x00179A67
	public static int KnownPackageCount
	{
		get
		{
			return NetPackageManager.knownPackageTypes.Count;
		}
	}

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x06003A5F RID: 14943 RVA: 0x0017B873 File Offset: 0x00179A73
	public static Type[] PackageMappings
	{
		get
		{
			return NetPackageManager.packageIdToClass;
		}
	}

	// Token: 0x06003A60 RID: 14944 RVA: 0x0017B87C File Offset: 0x00179A7C
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

	// Token: 0x04002FA4 RID: 12196
	[PublicizedFrom(EAccessModifier.Private)]
	public static Type[] packageIdToClass;

	// Token: 0x04002FA5 RID: 12197
	[PublicizedFrom(EAccessModifier.Private)]
	public static NetPackageManager.IPackageInformation[] packageIdToPackageInformation;

	// Token: 0x04002FA6 RID: 12198
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<Type, int> packageClassToPackageId;

	// Token: 0x04002FA7 RID: 12199
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, Type> knownPackageTypes = new CaseInsensitiveStringDictionary<Type>();

	// Token: 0x04002FA8 RID: 12200
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Type packageIdsType = typeof(NetPackagePackageIds);

	// Token: 0x020007A3 RID: 1955
	[PublicizedFrom(EAccessModifier.Private)]
	public interface IPackageInformation
	{
		// Token: 0x06003A61 RID: 14945
		NetPackage GetRawPackage();

		// Token: 0x06003A62 RID: 14946
		void FreePackage(NetPackage _package);

		// Token: 0x06003A63 RID: 14947
		void GetStats(out int _packages, out int _totalSize);
	}

	// Token: 0x020007A4 RID: 1956
	[PublicizedFrom(EAccessModifier.Private)]
	public class NetPackageInformation<TPackage> : NetPackageManager.IPackageInformation where TPackage : NetPackage
	{
		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06003A64 RID: 14948 RVA: 0x0017B917 File Offset: 0x00179B17
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

		// Token: 0x06003A65 RID: 14949 RVA: 0x0017B930 File Offset: 0x00179B30
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

		// Token: 0x06003A66 RID: 14950 RVA: 0x0017BA68 File Offset: 0x00179C68
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

		// Token: 0x06003A67 RID: 14951 RVA: 0x0017BB04 File Offset: 0x00179D04
		public NetPackage GetRawPackage()
		{
			return this.GetPackage();
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x0017BB14 File Offset: 0x00179D14
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

		// Token: 0x06003A69 RID: 14953 RVA: 0x0017BBA0 File Offset: 0x00179DA0
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

		// Token: 0x06003A6A RID: 14954 RVA: 0x0017BBF4 File Offset: 0x00179DF4
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

		// Token: 0x04002FA9 RID: 12201
		[PublicizedFrom(EAccessModifier.Private)]
		public static NetPackageManager.NetPackageInformation<TPackage> instance;

		// Token: 0x04002FAA RID: 12202
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly TPackage[] pool;

		// Token: 0x04002FAB RID: 12203
		[PublicizedFrom(EAccessModifier.Private)]
		public int poolSize;

		// Token: 0x04002FAC RID: 12204
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int capacity;

		// Token: 0x04002FAD RID: 12205
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ConstructorInfo ctor;

		// Token: 0x04002FAE RID: 12206
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSampler = CustomSampler.Create("NPM.PI.GetPackage", false);

		// Token: 0x04002FAF RID: 12207
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerPool = CustomSampler.Create("Pooled", false);

		// Token: 0x04002FB0 RID: 12208
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerNew = CustomSampler.Create("New", false);

		// Token: 0x04002FB1 RID: 12209
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerType = CustomSampler.Create(typeof(TPackage).Name, false);

		// Token: 0x04002FB2 RID: 12210
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSampler = CustomSampler.Create("NPM.PI.FreePackage", false);

		// Token: 0x04002FB3 RID: 12211
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSamplerPool = CustomSampler.Create("ToPool", false);

		// Token: 0x04002FB4 RID: 12212
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSamplerCleanup = CustomSampler.Create("Cleanup", false);
	}

	// Token: 0x020007A5 RID: 1957
	public class UnknownNetPackageException : Exception
	{
		// Token: 0x06003A6B RID: 14955 RVA: 0x0017BC64 File Offset: 0x00179E64
		public UnknownNetPackageException(int _packageId)
			: base("Unknown NetPackage ID: " + _packageId.ToString())
		{
		}
	}
}
