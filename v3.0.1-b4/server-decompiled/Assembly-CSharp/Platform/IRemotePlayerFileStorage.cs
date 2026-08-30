using System;

namespace Platform
{
	// Token: 0x02001B70 RID: 7024
	public interface IRemotePlayerFileStorage
	{
		// Token: 0x0600D17C RID: 53628
		void Init(IPlatform _owner);

		// Token: 0x0600D17D RID: 53629
		void ReadRemoteData(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback);

		// Token: 0x0600D17E RID: 53630
		void WriteRemoteData(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback);

		// Token: 0x0600D17F RID: 53631 RVA: 0x004C0F50 File Offset: 0x004BF150
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

		// Token: 0x0600D180 RID: 53632 RVA: 0x004C0F91 File Offset: 0x004BF191
		void WriteRemoteObject(string _filename, IRemotePlayerStorageObject _object, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
		{
			if (_callback == null)
			{
				Log.Error("[RPFS] Write failed as no callback was supplied");
				return;
			}
			this.WriteRemoteData(_filename, IRemotePlayerFileStorage.ObjectToBytes(_object), _overwriteCache, _callback);
		}

		// Token: 0x0600D181 RID: 53633 RVA: 0x004C0FB4 File Offset: 0x004BF1B4
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

		// Token: 0x0600D182 RID: 53634 RVA: 0x004C1084 File Offset: 0x004BF284
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

		// Token: 0x0600D183 RID: 53635 RVA: 0x004C112C File Offset: 0x004BF32C
		public static string GetCacheFolder(IUserClient _user)
		{
			if (_user == null || _user.UserStatus > EUserStatus.LoggedIn)
			{
				return null;
			}
			return GameIO.GetUserGameDataDir() + "/RfsPlayerCache/" + _user.PlatformUserId.ReadablePlatformUserIdentifier;
		}

		// Token: 0x0600D184 RID: 53636 RVA: 0x004C115B File Offset: 0x004BF35B
		public static T ReadCachedObject<T>(IUserClient _user, string _filename) where T : IRemotePlayerStorageObject, new()
		{
			return IRemotePlayerFileStorage.BytesToObject<T>(IRemotePlayerFileStorage.ReadCachedData(_user, _filename));
		}

		// Token: 0x0600D185 RID: 53637 RVA: 0x004C116C File Offset: 0x004BF36C
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

		// Token: 0x0600D186 RID: 53638 RVA: 0x004C11E0 File Offset: 0x004BF3E0
		public static bool WriteCachedObject(IUserClient _user, string _filename, IRemotePlayerStorageObject _object)
		{
			return IRemotePlayerFileStorage.WriteCachedObject(_user, _filename, IRemotePlayerFileStorage.ObjectToBytes(_object));
		}

		// Token: 0x0600D187 RID: 53639 RVA: 0x004C11F0 File Offset: 0x004BF3F0
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

		// Token: 0x0600D188 RID: 53640 RVA: 0x004C1284 File Offset: 0x004BF484
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

		// Token: 0x04009FA4 RID: 40868
		public static object LocalCacheLock = new object();

		// Token: 0x02001B71 RID: 7025
		public enum CallbackResult
		{
			// Token: 0x04009FA6 RID: 40870
			Success,
			// Token: 0x04009FA7 RID: 40871
			FileNotFound,
			// Token: 0x04009FA8 RID: 40872
			NoConnection,
			// Token: 0x04009FA9 RID: 40873
			MalformedData,
			// Token: 0x04009FAA RID: 40874
			Other
		}

		// Token: 0x02001B72 RID: 7026
		// (Invoke) Token: 0x0600D18B RID: 53643
		public delegate void FileReadCompleteCallback(IRemotePlayerFileStorage.CallbackResult _result, byte[] _data);

		// Token: 0x02001B73 RID: 7027
		// (Invoke) Token: 0x0600D18F RID: 53647
		public delegate void FileReadObjectCompleteCallback<T>(IRemotePlayerFileStorage.CallbackResult _result, T _object) where T : IRemotePlayerStorageObject;

		// Token: 0x02001B74 RID: 7028
		// (Invoke) Token: 0x0600D193 RID: 53651
		public delegate void FileWriteCompleteCallback(IRemotePlayerFileStorage.CallbackResult _result);
	}
}
