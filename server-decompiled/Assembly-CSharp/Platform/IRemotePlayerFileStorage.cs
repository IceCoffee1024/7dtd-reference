using System;

namespace Platform
{
	// Token: 0x02001B97 RID: 7063
	public interface IRemotePlayerFileStorage
	{
		// Token: 0x0600D308 RID: 54024
		void Init(IPlatform _owner);

		// Token: 0x0600D309 RID: 54025
		void ReadRemoteData(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback);

		// Token: 0x0600D30A RID: 54026
		void WriteRemoteData(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback);

		// Token: 0x0600D30B RID: 54027 RVA: 0x004C7344 File Offset: 0x004C5544
		void ReadRemoteObject<T>(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadObjectCompleteCallback<T> _callback) where T : IRemotePlayerStorageObject, new()
		{
			if (_callback == null)
			{
				Log.Error("[RPFS] Read failed as no callback was supplied");
				return;
			}
			this.ReadRemoteData(_filename, _overwriteCache, delegate(IRemotePlayerFileStorage.CallbackResult result, byte[] data)
			{
				if (data == null)
				{
					_callback(result, default(T));
					return;
				}
				T t = IRemotePlayerFileStorage.BytesToObject<T>(data);
				if (t == null)
				{
					Log.Error(string.Format("[RPFS] Reading data into type {0} yields malformed result.", typeof(T)));
					result = IRemotePlayerFileStorage.CallbackResult.MalformedData;
					_callback(result, default(T));
					return;
				}
				_callback(result, t);
			});
		}

		// Token: 0x0600D30C RID: 54028 RVA: 0x004C7385 File Offset: 0x004C5585
		void WriteRemoteObject(string _filename, IRemotePlayerStorageObject _object, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
		{
			if (_callback == null)
			{
				Log.Error("[RPFS] Write failed as no callback was supplied");
				return;
			}
			this.WriteRemoteData(_filename, IRemotePlayerFileStorage.ObjectToBytes(_object), _overwriteCache, _callback);
		}

		// Token: 0x0600D30D RID: 54029 RVA: 0x004C73A8 File Offset: 0x004C55A8
		public static T BytesToObject<T>(byte[] _data) where T : IRemotePlayerStorageObject, new()
		{
			T t;
			if (_data == null || _data.Length == 0)
			{
				Log.Warning("[RPFS] Byte data was empty or null.");
				t = default(T);
				return t;
			}
			try
			{
				using (PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
				{
					pooledExpandableMemoryStream.Write(_data, 0, _data.Length);
					pooledExpandableMemoryStream.Position = 0L;
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(true))
					{
						pooledBinaryReader.SetBaseStream(pooledExpandableMemoryStream);
						T t2 = new T();
						t2.ReadInto(pooledBinaryReader);
						t = t2;
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Format("[RPFS] Error while reading object from byte data. Error: {0}.", ex));
				t = default(T);
			}
			return t;
		}

		// Token: 0x0600D30E RID: 54030 RVA: 0x004C7478 File Offset: 0x004C5678
		public static byte[] ObjectToBytes(IRemotePlayerStorageObject _obj)
		{
			if (_obj == null)
			{
				Log.Warning("[RPFS] Object was null.");
				return null;
			}
			byte[] array;
			try
			{
				using (PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
				{
					using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(true))
					{
						pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
						_obj.WriteFrom(pooledBinaryWriter);
						pooledBinaryWriter.Flush();
					}
					array = pooledExpandableMemoryStream.ToArray();
				}
			}
			catch (Exception ex)
			{
				Log.Error(string.Format("[RPFS] Error while writing object into byte data. Error: {0}.", ex));
				array = null;
			}
			return array;
		}

		// Token: 0x0600D30F RID: 54031 RVA: 0x004C7520 File Offset: 0x004C5720
		public static string GetCacheFolder(IUserClient _user)
		{
			if (_user == null || _user.UserStatus > EUserStatus.LoggedIn)
			{
				return null;
			}
			return GameIO.GetUserGameDataDir() + "/RfsPlayerCache/" + _user.PlatformUserId.ReadablePlatformUserIdentifier;
		}

		// Token: 0x0600D310 RID: 54032 RVA: 0x004C754F File Offset: 0x004C574F
		public static T ReadCachedObject<T>(IUserClient _user, string _filename) where T : IRemotePlayerStorageObject, new()
		{
			return IRemotePlayerFileStorage.BytesToObject<T>(IRemotePlayerFileStorage.ReadCachedData(_user, _filename));
		}

		// Token: 0x0600D311 RID: 54033 RVA: 0x004C7560 File Offset: 0x004C5760
		public static byte[] ReadCachedData(IUserClient _user, string _filename)
		{
			object localCacheLock = IRemotePlayerFileStorage.LocalCacheLock;
			byte[] array;
			lock (localCacheLock)
			{
				string text = IRemotePlayerFileStorage.GetCacheFolder(_user) + "/" + _filename;
				if (string.IsNullOrEmpty(_filename) || !SdFile.Exists(text))
				{
					Log.Warning("[RPFS] File path was not found.");
					array = null;
				}
				else
				{
					array = SdFile.ReadAllBytes(text);
				}
			}
			return array;
		}

		// Token: 0x0600D312 RID: 54034 RVA: 0x004C75D4 File Offset: 0x004C57D4
		public static bool WriteCachedObject(IUserClient _user, string _filename, IRemotePlayerStorageObject _object)
		{
			return IRemotePlayerFileStorage.WriteCachedObject(_user, _filename, IRemotePlayerFileStorage.ObjectToBytes(_object));
		}

		// Token: 0x0600D313 RID: 54035 RVA: 0x004C75E4 File Offset: 0x004C57E4
		public static bool WriteCachedObject(IUserClient _user, string _filename, byte[] _data)
		{
			object localCacheLock = IRemotePlayerFileStorage.LocalCacheLock;
			bool flag2;
			lock (localCacheLock)
			{
				try
				{
					if (_data == null)
					{
						Log.Warning("[RPFS] Error while converting object to bytes.");
						flag2 = false;
					}
					else
					{
						SdDirectory.CreateDirectory(IRemotePlayerFileStorage.GetCacheFolder(_user));
						SdFile.WriteAllBytes(IRemotePlayerFileStorage.GetCacheFolder(_user) + "/" + _filename, _data);
						flag2 = true;
					}
				}
				catch (Exception ex)
				{
					Log.Warning(string.Format("[RPFS] Error while writing object to cache. Error: {0}.", ex));
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x0600D314 RID: 54036 RVA: 0x004C7678 File Offset: 0x004C5878
		public static void ClearCache(IUserClient _user)
		{
			object localCacheLock = IRemotePlayerFileStorage.LocalCacheLock;
			lock (localCacheLock)
			{
				if (SdDirectory.Exists(IRemotePlayerFileStorage.GetCacheFolder(_user)))
				{
					string[] files = SdDirectory.GetFiles(IRemotePlayerFileStorage.GetCacheFolder(_user));
					for (int i = 0; i < files.Length; i++)
					{
						SdFile.Delete(files[i]);
					}
				}
			}
		}

		// Token: 0x0400A0D5 RID: 41173
		public static object LocalCacheLock = new object();

		// Token: 0x02001B98 RID: 7064
		public enum CallbackResult
		{
			// Token: 0x0400A0D7 RID: 41175
			Success,
			// Token: 0x0400A0D8 RID: 41176
			FileNotFound,
			// Token: 0x0400A0D9 RID: 41177
			NoConnection,
			// Token: 0x0400A0DA RID: 41178
			MalformedData,
			// Token: 0x0400A0DB RID: 41179
			Other
		}

		// Token: 0x02001B99 RID: 7065
		// (Invoke) Token: 0x0600D317 RID: 54039
		public delegate void FileReadCompleteCallback(IRemotePlayerFileStorage.CallbackResult _result, byte[] _data);

		// Token: 0x02001B9A RID: 7066
		// (Invoke) Token: 0x0600D31B RID: 54043
		public delegate void FileReadObjectCompleteCallback<T>(IRemotePlayerFileStorage.CallbackResult _result, T _object) where T : IRemotePlayerStorageObject;

		// Token: 0x02001B9B RID: 7067
		// (Invoke) Token: 0x0600D31F RID: 54047
		public delegate void FileWriteCompleteCallback(IRemotePlayerFileStorage.CallbackResult _result);
	}
}
