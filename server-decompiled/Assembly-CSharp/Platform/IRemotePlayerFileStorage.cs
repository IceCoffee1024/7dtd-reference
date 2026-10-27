using System;

namespace Platform
{
	// Token: 0x02001BFA RID: 7162
	public interface IRemotePlayerFileStorage
	{
		// Token: 0x0600D594 RID: 54676
		void Init(IPlatform _owner);

		// Token: 0x0600D595 RID: 54677
		void ReadRemoteData(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback);

		// Token: 0x0600D596 RID: 54678
		void WriteRemoteData(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback);

		// Token: 0x0600D597 RID: 54679 RVA: 0x004CF954 File Offset: 0x004CDB54
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

		// Token: 0x0600D598 RID: 54680 RVA: 0x004CF995 File Offset: 0x004CDB95
		void WriteRemoteObject(string _filename, IRemotePlayerStorageObject _object, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
		{
			if (_callback == null)
			{
				Log.Error("[RPFS] Write failed as no callback was supplied");
				return;
			}
			this.WriteRemoteData(_filename, IRemotePlayerFileStorage.ObjectToBytes(_object), _overwriteCache, _callback);
		}

		// Token: 0x0600D599 RID: 54681 RVA: 0x004CF9B8 File Offset: 0x004CDBB8
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

		// Token: 0x0600D59A RID: 54682 RVA: 0x004CFA88 File Offset: 0x004CDC88
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

		// Token: 0x0600D59B RID: 54683 RVA: 0x004CFB30 File Offset: 0x004CDD30
		public static string GetCacheFolder(IUserClient _user)
		{
			if (_user == null || _user.UserStatus > EUserStatus.LoggedIn)
			{
				return null;
			}
			return GameIO.GetUserGameDataDir() + "/RfsPlayerCache/" + _user.PlatformUserId.ReadablePlatformUserIdentifier;
		}

		// Token: 0x0600D59C RID: 54684 RVA: 0x004CFB5F File Offset: 0x004CDD5F
		public static T ReadCachedObject<T>(IUserClient _user, string _filename) where T : IRemotePlayerStorageObject, new()
		{
			return IRemotePlayerFileStorage.BytesToObject<T>(IRemotePlayerFileStorage.ReadCachedData(_user, _filename));
		}

		// Token: 0x0600D59D RID: 54685 RVA: 0x004CFB70 File Offset: 0x004CDD70
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

		// Token: 0x0600D59E RID: 54686 RVA: 0x004CFBE4 File Offset: 0x004CDDE4
		public static bool WriteCachedObject(IUserClient _user, string _filename, IRemotePlayerStorageObject _object)
		{
			return IRemotePlayerFileStorage.WriteCachedObject(_user, _filename, IRemotePlayerFileStorage.ObjectToBytes(_object));
		}

		// Token: 0x0600D59F RID: 54687 RVA: 0x004CFBF4 File Offset: 0x004CDDF4
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

		// Token: 0x0600D5A0 RID: 54688 RVA: 0x004CFC88 File Offset: 0x004CDE88
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

		// Token: 0x0400A293 RID: 41619
		public static object LocalCacheLock = new object();

		// Token: 0x02001BFB RID: 7163
		public enum CallbackResult
		{
			// Token: 0x0400A295 RID: 41621
			Success,
			// Token: 0x0400A296 RID: 41622
			FileNotFound,
			// Token: 0x0400A297 RID: 41623
			NoConnection,
			// Token: 0x0400A298 RID: 41624
			MalformedData,
			// Token: 0x0400A299 RID: 41625
			Other
		}

		// Token: 0x02001BFC RID: 7164
		// (Invoke) Token: 0x0600D5A3 RID: 54691
		public delegate void FileReadCompleteCallback(IRemotePlayerFileStorage.CallbackResult _result, byte[] _data);

		// Token: 0x02001BFD RID: 7165
		// (Invoke) Token: 0x0600D5A7 RID: 54695
		public delegate void FileReadObjectCompleteCallback<T>(IRemotePlayerFileStorage.CallbackResult _result, T _object) where T : IRemotePlayerStorageObject;

		// Token: 0x02001BFE RID: 7166
		// (Invoke) Token: 0x0600D5AB RID: 54699
		public delegate void FileWriteCompleteCallback(IRemotePlayerFileStorage.CallbackResult _result);
	}
}
