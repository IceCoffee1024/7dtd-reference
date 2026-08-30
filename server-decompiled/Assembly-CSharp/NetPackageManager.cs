using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Profiling;

// Token: 0x0200079F RID: 1951
public static class NetPackageManager
{
	// Token: 0x06003A0F RID: 14863 RVA: 0x0017AE34 File Offset: 0x00179034
	[PublicizedFrom(EAccessModifier.Private)]
	static NetPackageManager()
	{
		Log.Out("NetPackageManager Init");
		ReflectionHelpers.FindTypesImplementingBase(typeof(NetPackage), delegate(Type _type)
		{
			NetPackageManager.knownPackageTypes.Add(_type.Name, _type);
		}, false);
	}

	// Token: 0x06003A10 RID: 14864 RVA: 0x0017AE84 File Offset: 0x00179084
	public static void ResetMappings()
	{
		NetPackageManager.packageIdToClass = null;
		NetPackageManager.packageIdToPackageInformation = null;
		NetPackageManager.packageClassToPackageId = null;
	}

	// Token: 0x06003A11 RID: 14865 RVA: 0x0017AE98 File Offset: 0x00179098
	[PublicizedFrom(EAccessModifier.Private)]
	public static void AddPackageMapping(int _id, Type _type)
	{
		NetPackageManager.packageIdToClass[_id] = _type;
		NetPackageManager.packageClassToPackageId[_type] = _id;
		NetPackageManager.IPackageInformation packageInformation = (NetPackageManager.IPackageInformation)typeof(NetPackageManager.NetPackageInformation<>).MakeGenericType(new Type[] { _type }).GetProperty("Instance").GetValue(null, null);
		NetPackageManager.packageIdToPackageInformation[_id] = packageInformation;
	}

	// Token: 0x06003A12 RID: 14866 RVA: 0x0017AEF1 File Offset: 0x001790F1
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetupBaseMapping()
	{
		NetPackageManager.packageIdToClass = new Type[NetPackageManager.KnownPackageCount];
		NetPackageManager.packageIdToPackageInformation = new NetPackageManager.IPackageInformation[NetPackageManager.KnownPackageCount];
		NetPackageManager.packageClassToPackageId = new Dictionary<Type, int>();
		NetPackageManager.AddPackageMapping(0, NetPackageManager.packageIdsType);
	}

	// Token: 0x06003A13 RID: 14867 RVA: 0x0017AF28 File Offset: 0x00179128
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

	// Token: 0x06003A14 RID: 14868 RVA: 0x0017AFA4 File Offset: 0x001791A4
	public static void StartClient()
	{
		NetPackageManager.ResetMappings();
		NetPackageManager.SetupBaseMapping();
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x0017AFB0 File Offset: 0x001791B0
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

	// Token: 0x06003A16 RID: 14870 RVA: 0x0017B032 File Offset: 0x00179232
	public static NetPackage ParsePackage(PooledBinaryReader _reader, ClientInfo _sender)
	{
		NetPackage rawPackage = NetPackageManager.getPackageInfoByType((int)_reader.ReadUInt16()).GetRawPackage();
		rawPackage.Sender = _sender;
		rawPackage.read(_reader);
		return rawPackage;
	}

	// Token: 0x06003A17 RID: 14871 RVA: 0x0017B052 File Offset: 0x00179252
	public static void FreePackage(NetPackage _package)
	{
		NetPackageManager.getPackageInfoByType(_package.PackageId).FreePackage(_package);
	}

	// Token: 0x06003A18 RID: 14872 RVA: 0x0017B065 File Offset: 0x00179265
	[PublicizedFrom(EAccessModifier.Private)]
	public static NetPackageManager.IPackageInformation getPackageInfoByType(int _packageTypeId)
	{
		if (_packageTypeId >= NetPackageManager.packageIdToPackageInformation.Length || NetPackageManager.packageIdToPackageInformation[_packageTypeId] == null)
		{
			throw new NetPackageManager.UnknownNetPackageException(_packageTypeId);
		}
		return NetPackageManager.packageIdToPackageInformation[_packageTypeId];
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x0017B088 File Offset: 0x00179288
	public static TPackage GetPackage<TPackage>() where TPackage : NetPackage
	{
		return NetPackageManager.NetPackageInformation<TPackage>.Instance.GetPackage();
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x0017B094 File Offset: 0x00179294
	public static int GetPackageId(Type _type)
	{
		return NetPackageManager.packageClassToPackageId[_type];
	}

	// Token: 0x06003A1B RID: 14875 RVA: 0x0017B0A1 File Offset: 0x001792A1
	public static string GetPackageName(int _id)
	{
		return NetPackageManager.packageIdToClass[_id].ToString();
	}

	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x06003A1C RID: 14876 RVA: 0x0017B0AF File Offset: 0x001792AF
	public static int KnownPackageCount
	{
		get
		{
			return NetPackageManager.knownPackageTypes.Count;
		}
	}

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x06003A1D RID: 14877 RVA: 0x0017B0BB File Offset: 0x001792BB
	public static Type[] PackageMappings
	{
		get
		{
			return NetPackageManager.packageIdToClass;
		}
	}

	// Token: 0x06003A1E RID: 14878 RVA: 0x0017B0C4 File Offset: 0x001792C4
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

	// Token: 0x04002FAA RID: 12202
	[PublicizedFrom(EAccessModifier.Private)]
	public static Type[] packageIdToClass;

	// Token: 0x04002FAB RID: 12203
	[PublicizedFrom(EAccessModifier.Private)]
	public static NetPackageManager.IPackageInformation[] packageIdToPackageInformation;

	// Token: 0x04002FAC RID: 12204
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<Type, int> packageClassToPackageId;

	// Token: 0x04002FAD RID: 12205
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<string, Type> knownPackageTypes = new CaseInsensitiveStringDictionary<Type>();

	// Token: 0x04002FAE RID: 12206
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Type packageIdsType = typeof(NetPackagePackageIds);

	// Token: 0x020007A0 RID: 1952
	[PublicizedFrom(EAccessModifier.Private)]
	public interface IPackageInformation
	{
		// Token: 0x06003A1F RID: 14879
		NetPackage GetRawPackage();

		// Token: 0x06003A20 RID: 14880
		void FreePackage(NetPackage _package);

		// Token: 0x06003A21 RID: 14881
		void GetStats(out int _packages, out int _totalSize);
	}

	// Token: 0x020007A1 RID: 1953
	[PublicizedFrom(EAccessModifier.Private)]
	public class NetPackageInformation<TPackage> : NetPackageManager.IPackageInformation where TPackage : NetPackage
	{
		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06003A22 RID: 14882 RVA: 0x0017B15F File Offset: 0x0017935F
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

		// Token: 0x06003A23 RID: 14883 RVA: 0x0017B178 File Offset: 0x00179378
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

		// Token: 0x06003A24 RID: 14884 RVA: 0x0017B2B0 File Offset: 0x001794B0
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

		// Token: 0x06003A25 RID: 14885 RVA: 0x0017B34C File Offset: 0x0017954C
		public NetPackage GetRawPackage()
		{
			return this.GetPackage();
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x0017B35C File Offset: 0x0017955C
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

		// Token: 0x06003A27 RID: 14887 RVA: 0x0017B3E8 File Offset: 0x001795E8
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

		// Token: 0x06003A28 RID: 14888 RVA: 0x0017B43C File Offset: 0x0017963C
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

		// Token: 0x04002FAF RID: 12207
		[PublicizedFrom(EAccessModifier.Private)]
		public static NetPackageManager.NetPackageInformation<TPackage> instance;

		// Token: 0x04002FB0 RID: 12208
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly TPackage[] pool;

		// Token: 0x04002FB1 RID: 12209
		[PublicizedFrom(EAccessModifier.Private)]
		public int poolSize;

		// Token: 0x04002FB2 RID: 12210
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int capacity;

		// Token: 0x04002FB3 RID: 12211
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ConstructorInfo ctor;

		// Token: 0x04002FB4 RID: 12212
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSampler = CustomSampler.Create("NPM.PI.GetPackage", false);

		// Token: 0x04002FB5 RID: 12213
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerPool = CustomSampler.Create("Pooled", false);

		// Token: 0x04002FB6 RID: 12214
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerNew = CustomSampler.Create("New", false);

		// Token: 0x04002FB7 RID: 12215
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler getSamplerType = CustomSampler.Create(typeof(TPackage).Name, false);

		// Token: 0x04002FB8 RID: 12216
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSampler = CustomSampler.Create("NPM.PI.FreePackage", false);

		// Token: 0x04002FB9 RID: 12217
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSamplerPool = CustomSampler.Create("ToPool", false);

		// Token: 0x04002FBA RID: 12218
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly CustomSampler freeSamplerCleanup = CustomSampler.Create("Cleanup", false);
	}

	// Token: 0x020007A2 RID: 1954
	public class UnknownNetPackageException : Exception
	{
		// Token: 0x06003A29 RID: 14889 RVA: 0x0017B4AC File Offset: 0x001796AC
		public UnknownNetPackageException(int _packageId)
			: base("Unknown NetPackage ID: " + _packageId.ToString())
		{
		}
	}
}
