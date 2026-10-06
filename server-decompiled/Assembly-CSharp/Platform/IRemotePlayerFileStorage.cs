using System;

namespace Platform
{
	// Token: 0x02001BC9 RID: 7113
	public interface IRemotePlayerFileStorage
	{
		// Token: 0x0600D480 RID: 54400
		void Init(IPlatform _owner);

		// Token: 0x0600D481 RID: 54401
		void ReadRemoteData(string _filename, bool _overwriteCache, IRemotePlayerFileStorage.FileReadCompleteCallback _callback);

		// Token: 0x0600D482 RID: 54402
		void WriteRemoteData(string _filename, byte[] _data, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback);

		// Token: 0x0600D483 RID: 54403 RVA: 0x004CBE2C File Offset: 0x004CA02C
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

		// Token: 0x0600D484 RID: 54404 RVA: 0x004CBE6D File Offset: 0x004CA06D
		void WriteRemoteObject(string _filename, IRemotePlayerStorageObject _object, bool _overwriteCache, IRemotePlayerFileStorage.FileWriteCompleteCallback _callback)
		{
			if (_callback == null)
			{
				Log.Error("[RPFS] Write failed as no callback was supplied");
				return;
			}
			this.WriteRemoteData(_filename, IRemotePlayerFileStorage.ObjectToBytes(_object), _overwriteCache, _callback);
		}

		// Token: 0x0600D485 RID: 54405 RVA: 0x004CBE90 File Offset: 0x004CA090
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

		// Token: 0x0600D486 RID: 54406 RVA: 0x004CBF60 File Offset: 0x004CA160
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

		// Token: 0x0600D487 RID: 54407 RVA: 0x004CC008 File Offset: 0x004CA208
		public static string GetCacheFolder(IUserClient _user)
		{
			if (_user == null || _user.UserStatus > EUserStatus.LoggedIn)
			{
				return null;
			}
			return GameIO.GetUserGameDataDir() + "/RfsPlayerCache/" + _user.PlatformUserId.ReadablePlatformUserIdentifier;
		}

		// Token: 0x0600D488 RID: 54408 RVA: 0x004CC037 File Offset: 0x004CA237
		public static T ReadCachedObject<T>(IUserClient _user, string _filename) where T : IRemotePlayerStorageObject, new()
		{
			return IRemotePlayerFileStorage.BytesToObject<T>(IRemotePlayerFileStorage.ReadCachedData(_user, _filename));
		}

		// Token: 0x0600D489 RID: 54409 RVA: 0x004CC048 File Offset: 0x004CA248
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

		// Token: 0x0600D48A RID: 54410 RVA: 0x004CC0BC File Offset: 0x004CA2BC
		public static bool WriteCachedObject(IUserClient _user, string _filename, IRemotePlayerStorageObject _object)
		{
			return IRemotePlayerFileStorage.WriteCachedObject(_user, _filename, IRemotePlayerFileStorage.ObjectToBytes(_object));
		}

		// Token: 0x0600D48B RID: 54411 RVA: 0x004CC0CC File Offset: 0x004CA2CC
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

		// Token: 0x0600D48C RID: 54412 RVA: 0x004CC160 File Offset: 0x004CA360
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

		// Token: 0x0400A1AA RID: 41386
		public static object LocalCacheLock = new object();

		// Token: 0x02001BCA RID: 7114
		public enum CallbackResult
		{
			// Token: 0x0400A1AC RID: 41388
			Success,
			// Token: 0x0400A1AD RID: 41389
			FileNotFound,
			// Token: 0x0400A1AE RID: 41390
			NoConnection,
			// Token: 0x0400A1AF RID: 41391
			MalformedData,
			// Token: 0x0400A1B0 RID: 41392
			Other
		}

		// Token: 0x02001BCB RID: 7115
		// (Invoke) Token: 0x0600D48F RID: 54415
		public delegate void FileReadCompleteCallback(IRemotePlayerFileStorage.CallbackResult _result, byte[] _data);

		// Token: 0x02001BCC RID: 7116
		// (Invoke) Token: 0x0600D493 RID: 54419
		public delegate void FileReadObjectCompleteCallback<T>(IRemotePlayerFileStorage.CallbackResult _result, T _object) where T : IRemotePlayerStorageObject;

		// Token: 0x02001BCD RID: 7117
		// (Invoke) Token: 0x0600D497 RID: 54423
		public delegate void FileWriteCompleteCallback(IRemotePlayerFileStorage.CallbackResult _result);
	}
}
