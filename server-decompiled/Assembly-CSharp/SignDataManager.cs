using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Platform;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.TextCore;

// Token: 0x02000AAC RID: 2732
public class SignDataManager
{
	// Token: 0x17000906 RID: 2310
	// (get) Token: 0x060051D0 RID: 20944 RVA: 0x001F5533 File Offset: 0x001F3733
	public static SignDataManager Instance
	{
		get
		{
			if (SignDataManager.m_Instance == null)
			{
				SignDataManager.m_Instance = new SignDataManager();
			}
			return SignDataManager.m_Instance;
		}
	}

	// Token: 0x17000907 RID: 2311
	// (get) Token: 0x060051D1 RID: 20945 RVA: 0x001F554B File Offset: 0x001F374B
	public static bool HasInstance
	{
		get
		{
			return SignDataManager.m_Instance != null;
		}
	}

	// Token: 0x17000908 RID: 2312
	// (get) Token: 0x060051D2 RID: 20946 RVA: 0x001F5555 File Offset: 0x001F3755
	public IEnumerable<string> FontNames
	{
		get
		{
			return this.fontDataByName.Keys;
		}
	}

	// Token: 0x060051D3 RID: 20947 RVA: 0x001F5564 File Offset: 0x001F3764
	[PublicizedFrom(EAccessModifier.Private)]
	public SignDataManager()
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		SignFontConfiguration signFontConfiguration = DataLoader.LoadAsset<SignFontConfiguration>("@:Fonts/SignFontConfig.asset", true);
		if (signFontConfiguration == null)
		{
			Log.Error("Failed to load font configuration asset from path: @:Fonts/SignFontConfig.asset");
		}
		else
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (SignFontConfiguration.FontInfo fontInfo in signFontConfiguration.fontInfos)
			{
				TMP_FontAsset fontAsset = fontInfo.fontAsset;
				if (((fontAsset != null) ? fontAsset.atlasTexture : null) != null)
				{
					if (num2 == 0 || num3 == 0)
					{
						num2 = fontInfo.fontAsset.atlasTexture.width;
						num3 = fontInfo.fontAsset.atlasTexture.height;
					}
					else if (num2 != fontInfo.fontAsset.atlasTexture.width || num3 != fontInfo.fontAsset.atlasTexture.height)
					{
						Log.Error("SignFontConfiguration asset references fonts with differing font atlas resolutions. This is not supported. Configuration asset prefabLocation: `@:Fonts/SignFontConfig.asset`.");
						continue;
					}
					this.fontDataByName[fontInfo.name] = new SignDataManager.SignFontData(num, fontInfo.fontAsset);
					num++;
				}
			}
			if (SystemInfo.supports2DArrayTextures)
			{
				this.fontAtlasTextureArray = new Texture2DArray(num2, num3, this.fontDataByName.Count, TextureFormat.Alpha8, false);
				this.fontAtlasTextureArray.name = "Sign Font Atlas";
				using (Dictionary<string, SignDataManager.SignFontData>.Enumerator enumerator2 = this.fontDataByName.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<string, SignDataManager.SignFontData> keyValuePair = enumerator2.Current;
						SignDataManager.SignFontData value = keyValuePair.Value;
						Graphics.CopyTexture(value.AtlasTexture, 0, this.fontAtlasTextureArray, value.AtlasIndex);
					}
					goto IL_0673;
				}
			}
			Log.Error("Texture arrays are not supported on this platform/GPU; sign fonts will not be rendered correctly.");
		}
		IL_0673:
		this.intermediateDataWrappers = new SignDataManager.IIntermediateDataWrapper[]
		{
			this.layerDescriptors, this.circleLayers, this.polygonLayers, this.charLayers, this.noiseLayers, this.transformWarps, this.skewWarps, this.bulgeWarps, this.twirlWarps, this.kaleidoWarps,
			this.perspectiveWarps, this.arcWarps, this.stretchWarps, this.gridWarps
		};
	}

	// Token: 0x060051D4 RID: 20948 RVA: 0x001F5C90 File Offset: 0x001F3E90
	public static string GetLibraryNiceName(GlobalSignId signId)
	{
		if (!signId.IsValid)
		{
			return "-";
		}
		string libraryId = signId.libraryId;
		if (libraryId == "[D]")
		{
			return Localization.Get("lblSignLibraryDefault", false, null);
		}
		if (libraryId == "[U]")
		{
			return Localization.Get("lblSignLibraryUser", false, null);
		}
		if (SignDataManager.IsPlayerLibraryId(signId.libraryId))
		{
			return SignDataManager.GetPlayerLibraryOwnerName(signId.libraryId);
		}
		return Localization.Get(signId.libraryId, false, null);
	}

	// Token: 0x060051D5 RID: 20949 RVA: 0x001F5D10 File Offset: 0x001F3F10
	[PublicizedFrom(EAccessModifier.Private)]
	public static string GetPlayerLibraryOwnerName(string playerLibraryId)
	{
		PlatformUserIdentifierAbs platformUserIdentifierAbs;
		if (PlatformUserIdentifierAbs.TryFromCombinedString(playerLibraryId, out platformUserIdentifierAbs))
		{
			GameManager instance = GameManager.Instance;
			PersistentPlayerData persistentPlayerData;
			if (instance == null)
			{
				persistentPlayerData = null;
			}
			else
			{
				PersistentPlayerList persistentPlayers = instance.persistentPlayers;
				persistentPlayerData = ((persistentPlayers != null) ? persistentPlayers.GetPlayerData(platformUserIdentifierAbs) : null);
			}
			PersistentPlayerData persistentPlayerData2 = persistentPlayerData;
			if (persistentPlayerData2 != null)
			{
				return persistentPlayerData2.PlayerName.DisplayName;
			}
		}
		return Localization.Get("lblSignFontUnknown", false, null);
	}

	// Token: 0x17000909 RID: 2313
	// (get) Token: 0x060051D6 RID: 20950 RVA: 0x001F5D61 File Offset: 0x001F3F61
	// (set) Token: 0x060051D7 RID: 20951 RVA: 0x001F5D68 File Offset: 0x001F3F68
	public static GlobalSignId ErrorSignId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060051D8 RID: 20952 RVA: 0x001F5D70 File Offset: 0x001F3F70
	public static IEnumerator LoadDefaultLibrary(XmlFile file)
	{
		SignLibrary signLibrary = new SignLibrary();
		signLibrary.ReadXml(file);
		SignDataManager.Instance.TryRegisterLibrary("[D]", signLibrary, false);
		SignDataManager.ErrorSignId = SignDataManager.Instance.AddSignToLibraryLocal("[I]", SignData.GetErrorSignData());
		yield break;
	}

	// Token: 0x060051D9 RID: 20953 RVA: 0x001F5D80 File Offset: 0x001F3F80
	public void Cleanup()
	{
		this.FlushPendingPlayerSignWrites();
		this.playerSignStates.Clear();
		DeletedSignStore deletedSignStore = this.deletedSignStore;
		if (deletedSignStore != null)
		{
			deletedSignStore.Flush();
		}
		this.deletedSignStore = null;
		this.sessionDeletedSignIds.Clear();
		foreach (KeyValuePair<GlobalSignId, SignDataManager.SignRenderingData> keyValuePair in this.renderingDataByID)
		{
			SignDataManager.SignRenderingData value = keyValuePair.Value;
			if (value != null)
			{
				value.Dispose();
			}
		}
		this.renderingDataByID.Clear();
		this.signLibraries.Clear();
		this.updateListenerMap.Clear();
	}

	// Token: 0x060051DA RID: 20954 RVA: 0x001F5E34 File Offset: 0x001F4034
	public void RegisterListener(GlobalSignId id, ISignRenderingDataUpdateListener listener, bool exclusive = true)
	{
		if (exclusive)
		{
			this.updateListenerMap.DeregisterListener(listener);
		}
		this.updateListenerMap.RegisterListener(id, listener);
	}

	// Token: 0x060051DB RID: 20955 RVA: 0x001F5E52 File Offset: 0x001F4052
	public void DeregisterListener(ISignRenderingDataUpdateListener listener)
	{
		this.updateListenerMap.DeregisterListener(listener);
	}

	// Token: 0x060051DC RID: 20956 RVA: 0x001F5E60 File Offset: 0x001F4060
	[PublicizedFrom(EAccessModifier.Private)]
	public bool LibraryContainsName(string libraryName, string signName)
	{
		SignLibrary signLibrary;
		if (this.signLibraries.TryGetValue(libraryName, out signLibrary))
		{
			using (Dictionary<Guid, SignData>.ValueCollection.Enumerator enumerator = signLibrary.signs.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.name == signName)
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x060051DD RID: 20957 RVA: 0x001F5ED4 File Offset: 0x001F40D4
	public string GetDuplicateName(string libraryName, string signName)
	{
		string text = signName;
		Match match = Regex.Match(signName, "^(.+) \\(\\d+\\)$");
		if (match.Success)
		{
			text = match.Groups[1].Value;
		}
		if (!this.LibraryContainsName(libraryName, text))
		{
			return text;
		}
		int num = 1;
		while (this.LibraryContainsName(libraryName, string.Format("{0} ({1})", text, num)))
		{
			num++;
		}
		return string.Format("{0} ({1})", text, num);
	}

	// Token: 0x060051DE RID: 20958 RVA: 0x001F5F48 File Offset: 0x001F4148
	public bool CopySignToLibrary(GlobalSignId signId, string libraryName, out GlobalSignId newId)
	{
		SignData signData;
		if (this.TryGetSignData(signId, out signData))
		{
			this.CopySignToLibrary(signData, libraryName, out newId);
			return true;
		}
		newId = GlobalSignId.InvalidId;
		return false;
	}

	// Token: 0x060051DF RID: 20959 RVA: 0x001F5F78 File Offset: 0x001F4178
	public void CopySignToLibrary(SignData signData, string libraryName, out GlobalSignId newId)
	{
		SignData signData2 = SignData.Duplicate(signData, this.GetDuplicateName(libraryName, signData.name) ?? "");
		newId = this.AddSignToLibrary(libraryName, signData2);
	}

	// Token: 0x060051E0 RID: 20960 RVA: 0x001F5FB0 File Offset: 0x001F41B0
	public GlobalSignId AddSignToLibrary(string libraryId, SignData sign)
	{
		GlobalSignId globalSignId = this.AddSignToLibraryLocal(libraryId, sign);
		if (!SignDataManager.localLibraryIds.Contains(libraryId))
		{
			this.SyncSignData(globalSignId, sign);
		}
		return globalSignId;
	}

	// Token: 0x060051E1 RID: 20961 RVA: 0x001F5FDC File Offset: 0x001F41DC
	[PublicizedFrom(EAccessModifier.Private)]
	public GlobalSignId AddSignToLibraryLocal(string libraryId, SignData sign)
	{
		SignLibrary signLibrary;
		if (!this.signLibraries.TryGetValue(libraryId, out signLibrary))
		{
			signLibrary = new SignLibrary();
			this.signLibraries[libraryId] = signLibrary;
		}
		signLibrary.signs[sign.guid] = sign;
		GlobalSignId globalSignId = new GlobalSignId(libraryId, sign.guid);
		this.sessionDeletedSignIds.Remove(globalSignId);
		DeletedSignStore deletedSignStore = this.deletedSignStore;
		if (deletedSignStore != null)
		{
			deletedSignStore.Remove(globalSignId);
		}
		return globalSignId;
	}

	// Token: 0x060051E2 RID: 20962 RVA: 0x001F604C File Offset: 0x001F424C
	public bool TryDeleteSign(GlobalSignId signId)
	{
		bool flag = this.TryDeleteSignLocal(signId);
		if (flag && !SignDataManager.localLibraryIds.Contains(signId.libraryId))
		{
			this.SyncSignDelete(signId);
		}
		return flag;
	}

	// Token: 0x060051E3 RID: 20963 RVA: 0x001F6074 File Offset: 0x001F4274
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryDeleteSignLocal(GlobalSignId signId)
	{
		this.TryClearRenderingData(signId);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.RemovePlayerSignBlob(signId);
		}
		SignLibrary signLibrary;
		bool flag = this.signLibraries.TryGetValue(signId.libraryId, out signLibrary) && signLibrary.signs.Remove(signId.signGuid);
		if (SignDataManager.IsPlayerLibraryId(signId.libraryId))
		{
			this.HandlePlayerSignDeleted(signId);
		}
		return flag;
	}

	// Token: 0x060051E4 RID: 20964 RVA: 0x001F60DC File Offset: 0x001F42DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandlePlayerSignDeleted(GlobalSignId signId)
	{
		this.sessionDeletedSignIds.Add(signId);
		DeletedSignStore deletedSignStore = this.deletedSignStore;
		if (deletedSignStore != null)
		{
			deletedSignStore.Add(signId);
		}
		if (this.updateListenerMap.HasListeners(signId))
		{
			this.deletedSignListenerScratch.Clear();
			this.deletedSignListenerScratch.AddRange(this.updateListenerMap.GetListeners(signId));
			foreach (ISignRenderingDataUpdateListener signRenderingDataUpdateListener in this.deletedSignListenerScratch)
			{
				signRenderingDataUpdateListener.HandleSignDataRemoved();
			}
			this.deletedSignListenerScratch.Clear();
		}
		SignTextureManager.Instance.Invalidate(signId);
		this.ReplaceLoadedSignReferences(signId);
	}

	// Token: 0x060051E5 RID: 20965 RVA: 0x001F6198 File Offset: 0x001F4398
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReplaceLoadedSignReferences(GlobalSignId signId)
	{
		using (SignDataManager.s_SignDataManagerReplaceLoadedSignReferences.Auto())
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				World world = ((GameManager.Instance != null) ? GameManager.Instance.World : null);
				if (world != null && world.ChunkCache != null)
				{
					List<Chunk> chunkArrayCopySync = world.ChunkCache.GetChunkArrayCopySync();
					for (int i = 0; i < chunkArrayCopySync.Count; i++)
					{
						Chunk chunk = chunkArrayCopySync[i];
						if (chunk != null)
						{
							List<TileEntity> list = chunk.GetTileEntities().list;
							for (int j = 0; j < list.Count; j++)
							{
								TileEntity tileEntity = list[j];
								TEFeatureCanvas tefeatureCanvas;
								if (tileEntity != null && tileEntity.TryGetSelfOrFeature<TEFeatureCanvas>(out tefeatureCanvas) && tefeatureCanvas.TryReplaceSignReference(signId, SignData.defaultSignDataID))
								{
									tefeatureCanvas.SetModified();
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060051E6 RID: 20966 RVA: 0x001F6294 File Offset: 0x001F4494
	public bool IsKnownDeleted(GlobalSignId signId)
	{
		return this.sessionDeletedSignIds.Contains(signId) || (this.deletedSignStore != null && this.deletedSignStore.IsDeleted(signId));
	}

	// Token: 0x060051E7 RID: 20967 RVA: 0x001F62BC File Offset: 0x001F44BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void SyncSignDelete(GlobalSignId signId)
	{
		NetPackageSignDelete netPackageSignDelete = new NetPackageSignDelete().Setup(signId);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageSignDelete, false, -1, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageSignDelete, false);
	}

	// Token: 0x060051E8 RID: 20968 RVA: 0x001F630C File Offset: 0x001F450C
	public bool MarkSignDirty(GlobalSignId signId)
	{
		SignData signData;
		if (!SignDataManager.localLibraryIds.Contains(signId.libraryId) && this.TryGetSignData(signId, out signData))
		{
			this.SyncSignData(signId, signData);
		}
		return this.MarkSignDirtyLocal(signId);
	}

	// Token: 0x060051E9 RID: 20969 RVA: 0x001F6348 File Offset: 0x001F4548
	[PublicizedFrom(EAccessModifier.Private)]
	public void SyncSignData(GlobalSignId signId, SignData sign)
	{
		byte[] array = SignDataManager.SerializeSign(sign);
		NetPackageSingleSignData netPackageSingleSignData = new NetPackageSingleSignData().Setup(signId.libraryId, array);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(netPackageSingleSignData, false, -1, -1, -1, null, 192, false);
			this.StorePlayerSignBlob(signId.libraryId, sign.guid, array);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageSingleSignData, false);
	}

	// Token: 0x060051EA RID: 20970 RVA: 0x001F63B8 File Offset: 0x001F45B8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool MarkSignDirtyLocal(GlobalSignId signId)
	{
		SignTextureManager.Instance.Invalidate(signId);
		SignData signData;
		if (this.updateListenerMap.HasListeners(signId) && this.TryGetSignData(signId, out signData))
		{
			this.UpdateRenderingData(signData, signId);
			foreach (ISignRenderingDataUpdateListener signRenderingDataUpdateListener in this.updateListenerMap.GetListeners(signId))
			{
				signRenderingDataUpdateListener.HandleRenderingDataUpdate();
			}
			return true;
		}
		return this.TryClearRenderingData(signId);
	}

	// Token: 0x060051EB RID: 20971 RVA: 0x001F6440 File Offset: 0x001F4640
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryClearRenderingData(GlobalSignId signId)
	{
		SignDataManager.SignRenderingData signRenderingData;
		if (this.renderingDataByID.TryGetValue(signId, out signRenderingData))
		{
			signRenderingData.Dispose();
			this.renderingDataByID.Remove(signId);
			return true;
		}
		return false;
	}

	// Token: 0x060051EC RID: 20972 RVA: 0x001F6474 File Offset: 0x001F4674
	public bool TryRegisterLibrary(string libraryId, SignLibrary library, bool replaceExisting = false)
	{
		SignLibrary signLibrary;
		if (this.signLibraries.TryGetValue(libraryId, out signLibrary))
		{
			if (!replaceExisting)
			{
				return false;
			}
			foreach (Guid guid in signLibrary.signs.Keys)
			{
				GlobalSignId globalSignId = new GlobalSignId(libraryId, guid);
				SignDataManager.SignRenderingData signRenderingData;
				if (this.renderingDataByID.TryGetValue(globalSignId, out signRenderingData))
				{
					signRenderingData.Dispose();
					this.renderingDataByID.Remove(globalSignId);
				}
			}
		}
		this.signLibraries[libraryId] = library;
		return true;
	}

	// Token: 0x060051ED RID: 20973 RVA: 0x001F6518 File Offset: 0x001F4718
	public IEnumerator RequestWorldSignDataFromServer()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Error("[SignDataManager] Only client can request to download Sign Data.");
			yield break;
		}
		if (this.signDataDownloadInProgress)
		{
			Log.Error("[SignDataManager] Sign Data download is already in progress.");
			yield break;
		}
		this.signDataDownloadInProgress = true;
		this.signDataDownloadComplete = false;
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageSignDataRequest>(), false);
		while (this.signDataDownloadInProgress)
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
			{
				this.signDataDownloadInProgress = false;
				this.signDataDownloadComplete = false;
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060051EE RID: 20974 RVA: 0x001F6528 File Offset: 0x001F4728
	public void SendSignDataToClient(ClientInfo client)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
		{
			Log.Error("[SignDataManager] Only server can send Sign Data to clients.");
			return;
		}
		List<KeyValuePair<string, SignData>> list = new List<KeyValuePair<string, SignData>>();
		foreach (KeyValuePair<string, SignLibrary> keyValuePair in this.EnumeratePublicSignLibraries())
		{
			foreach (SignData signData in keyValuePair.Value.signs.Values)
			{
				list.Add(new KeyValuePair<string, SignData>(keyValuePair.Key, signData));
			}
		}
		if (list.Count == 0)
		{
			client.SendPackage(NetPackageManager.GetPackage<NetPackageSignDataResponse>().Setup(null, true));
			return;
		}
		using (PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
		{
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
				int i = 0;
				while (i < list.Count)
				{
					pooledExpandableMemoryStream.SetLength(0L);
					PooledBinaryWriter.StreamWriteSizeMarker streamWriteSizeMarker = pooledBinaryWriter.ReserveSizeMarker(PooledBinaryWriter.EMarkerSize.UInt32);
					while (i < list.Count && pooledExpandableMemoryStream.Length < 1048576L)
					{
						pooledBinaryWriter.Write(list[i].Key);
						list[i].Value.Write(pooledBinaryWriter);
						i++;
					}
					pooledBinaryWriter.FinalizeSizeMarker(ref streamWriteSizeMarker);
					byte[] array = pooledExpandableMemoryStream.ToArray();
					client.SendPackage(NetPackageManager.GetPackage<NetPackageSignDataResponse>().Setup(array, i >= list.Count));
				}
			}
		}
	}

	// Token: 0x060051EF RID: 20975 RVA: 0x001F6700 File Offset: 0x001F4900
	public void ProcessSignDataBatchReceived(byte[] data, bool isLastBatch)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Error("[SignDataManager] Only client can download Sign Data.");
			return;
		}
		try
		{
			if (data != null)
			{
				using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
				{
					using (MemoryStream memoryStream = new MemoryStream(data))
					{
						pooledBinaryReader.SetBaseStream(memoryStream);
						PooledBinaryReader.StreamReadSizeMarker streamReadSizeMarker = pooledBinaryReader.ReadSizeMarker(PooledBinaryWriter.EMarkerSize.UInt32);
						long num = streamReadSizeMarker.Position + (long)((ulong)streamReadSizeMarker.ExpectedSize);
						while (memoryStream.Position < num)
						{
							string text = pooledBinaryReader.ReadString();
							int num2;
							SignData signData = SignData.Read(pooledBinaryReader, out num2);
							this.AddSignToLibraryLocal(text, signData);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("[SignDataManager] Failed to apply sign data batch from server: {0}", ex));
		}
		finally
		{
			if (isLastBatch)
			{
				this.signDataDownloadInProgress = false;
				this.signDataDownloadComplete = true;
			}
		}
		SignDataManager.ErrorSignId = SignDataManager.Instance.AddSignToLibrary("[I]", SignData.GetErrorSignData());
	}

	// Token: 0x060051F0 RID: 20976 RVA: 0x001F6810 File Offset: 0x001F4A10
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<KeyValuePair<string, SignLibrary>> EnumeratePublicSignLibraries()
	{
		foreach (KeyValuePair<string, SignLibrary> keyValuePair in this.signLibraries)
		{
			if (!SignDataManager.localLibraryIds.Contains(keyValuePair.Key))
			{
				yield return keyValuePair;
			}
		}
		Dictionary<string, SignLibrary>.Enumerator enumerator = default(Dictionary<string, SignLibrary>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x060051F1 RID: 20977 RVA: 0x001F6820 File Offset: 0x001F4A20
	[PublicizedFrom(EAccessModifier.Private)]
	public bool TryGetRenderingData(GlobalSignId signId, out SignDataManager.SignRenderingData signRenderingData)
	{
		if (!signId.IsValid)
		{
			Log.Error("Failed to retrieve rendering data due to invalid sign ID: " + signId.ToString() + ".");
			signRenderingData = null;
			return false;
		}
		if (!this.renderingDataByID.TryGetValue(signId, out signRenderingData))
		{
			SignLibrary signLibrary;
			SignData signData;
			if (!this.signLibraries.TryGetValue(signId.libraryId, out signLibrary) || !signLibrary.signs.TryGetValue(signId.signGuid, out signData))
			{
				return false;
			}
			signRenderingData = this.UpdateRenderingData(signData, signId);
		}
		return true;
	}

	// Token: 0x060051F2 RID: 20978 RVA: 0x001F68A4 File Offset: 0x001F4AA4
	public bool TryApplyRenderingData(GlobalSignId signId, SignDataManager.RenderingDataPatcher patcher, List<SignRenderer> signRenderers, SignCanvas.SignBlendMode blendMode = SignCanvas.SignBlendMode.Cutout, Camera targetCamera = null)
	{
		SignDataManager.SignRenderingData signRenderingData;
		if (!this.TryGetRenderingData(signId, out signRenderingData))
		{
			return false;
		}
		Vector4 vector = new Vector4(2f, 2f, -1f, -1f);
		signRenderingData.materialPropertyBlock.SetVector(SignShaderIDs._AtlasArray_ST, vector);
		if (patcher != null)
		{
			patcher(signRenderingData.materialPropertyBlock);
		}
		if (signRenderingData.materialPropertyBlock == null)
		{
			Log.Error("Unexpected case in Sign Data Manager: signRenderingData.materialPropertyBlock is null.");
		}
		foreach (SignRenderer signRenderer in signRenderers)
		{
			if (signRenderer == null)
			{
				Log.Warning("Unexpected case in Sign Data Manager: signRenderer is null");
			}
			else if (signRenderer.Renderer == null)
			{
				Log.Warning("Unexpected case in Sign Data Manager: signRenderer.Renderer is null");
			}
			else
			{
				signRenderer.SetRenderParameters(signRenderingData.materialPropertyBlock, blendMode, targetCamera);
			}
		}
		return true;
	}

	// Token: 0x060051F3 RID: 20979 RVA: 0x001F6988 File Offset: 0x001F4B88
	public bool TryApplyRenderingData(GlobalSignId signId, float canvasAspect, Material material, SignData.SignLayer isolateLayer = null, SignUIStyle style = SignUIStyle.Standard)
	{
		SignDataManager.SignRenderingData signRenderingData;
		if (!this.TryGetRenderingData(signId, out signRenderingData))
		{
			return false;
		}
		float num = 1f / canvasAspect;
		Mathf.Max(canvasAspect, num);
		if (this.fontAtlasTextureArray != null)
		{
			material.SetTexture(SignShaderIDs._AtlasArray, this.fontAtlasTextureArray);
		}
		SignDataManager.IIntermediateDataWrapper[] array = this.intermediateDataWrappers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ApplyToMaterial(signRenderingData, material);
		}
		Vector4 vector = new Vector4(2f, 2f, -1f, -1f);
		material.SetVector(SignShaderIDs._AtlasArray_ST, vector);
		SignDataManager.RuntimeRenderingInfo runtimeRenderingInfo;
		if (isolateLayer != null && signRenderingData.renderingInfoByLayer.TryGetValue(isolateLayer, out runtimeRenderingInfo))
		{
			material.SetInteger(SignShaderIDs._BaseLayer, runtimeRenderingInfo.BaseDescriptorIndex);
			material.SetInteger(SignShaderIDs._LayerCount, runtimeRenderingInfo.DescriptorCount);
		}
		else
		{
			material.SetInteger(SignShaderIDs._BaseLayer, 0);
			material.SetInteger(SignShaderIDs._LayerCount, signRenderingData.layerCount);
		}
		material.SetInteger(SignShaderIDs._UIStyle, (int)style);
		material.SetFloat(SignShaderIDs._CanvasRotation, 0f);
		return true;
	}

	// Token: 0x060051F4 RID: 20980 RVA: 0x001F6A94 File Offset: 0x001F4C94
	public bool TryGetSignComplexityInfo(GlobalSignId signId, out SignComplexityInfo complexityInfo)
	{
		SignDataManager.SignRenderingData signRenderingData;
		if (this.TryGetRenderingData(signId, out signRenderingData))
		{
			complexityInfo = new SignComplexityInfo(signRenderingData.totalComplexity, signRenderingData.complexityByLayer, signRenderingData.stackInfo);
			return true;
		}
		complexityInfo = SignComplexityInfo.Invalid;
		return false;
	}

	// Token: 0x060051F5 RID: 20981 RVA: 0x001F6AD8 File Offset: 0x001F4CD8
	[PublicizedFrom(EAccessModifier.Private)]
	public SignDataManager.SignRenderingData UpdateRenderingData(SignData signData, GlobalSignId id)
	{
		SignDataManager.<>c__DisplayClass106_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.signData = signData;
		CS$<>8__locals1.id = id;
		this.<UpdateRenderingData>g__ClearIntermediateLists|106_0(ref CS$<>8__locals1);
		if (this.renderingDataByID.TryGetValue(CS$<>8__locals1.id, out CS$<>8__locals1.signRenderingData))
		{
			CS$<>8__locals1.signRenderingData.Reset();
		}
		else
		{
			CS$<>8__locals1.signRenderingData = new SignDataManager.SignRenderingData();
			this.renderingDataByID[CS$<>8__locals1.id] = CS$<>8__locals1.signRenderingData;
		}
		CS$<>8__locals1.compStackIndex = 0;
		CS$<>8__locals1.uvStackIndex = 0;
		CS$<>8__locals1.signRenderingData.stackInfo = this.<UpdateRenderingData>g__ProcessLayers|106_1(CS$<>8__locals1.signData.layers, SignData.SignTransform.Identity, this.colorOpStack, default(SignDataManager.GroupOffsets), ref CS$<>8__locals1);
		CS$<>8__locals1.signRenderingData.layerCount = this.layerDescriptors.List.Count;
		if (this.fontAtlasTextureArray != null)
		{
			CS$<>8__locals1.signRenderingData.materialPropertyBlock.SetTexture(SignShaderIDs._AtlasArray, this.fontAtlasTextureArray);
		}
		CS$<>8__locals1.signRenderingData.materialPropertyBlock.SetInteger(SignShaderIDs._BaseLayer, 0);
		CS$<>8__locals1.signRenderingData.materialPropertyBlock.SetInteger(SignShaderIDs._LayerCount, CS$<>8__locals1.signRenderingData.layerCount);
		SignDataManager.IIntermediateDataWrapper[] array = this.intermediateDataWrappers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateBuffer(CS$<>8__locals1.signRenderingData);
		}
		this.<UpdateRenderingData>g__ClearIntermediateLists|106_0(ref CS$<>8__locals1);
		return CS$<>8__locals1.signRenderingData;
	}

	// Token: 0x060051F6 RID: 20982 RVA: 0x001F6C3C File Offset: 0x001F4E3C
	public void AddFilteredSignIdsToList(string libraryId, string nameFilter, List<GlobalSignId> signIds)
	{
		SignLibrary signLibrary;
		if (!this.signLibraries.TryGetValue(libraryId, out signLibrary))
		{
			Log.Error("Failed to retrieve sign IDs. Library '" + libraryId + "' not found.");
			return;
		}
		bool flag;
		if (SignDataManager.IsPlayerLibraryId(libraryId))
		{
			PlatformUserIdentifierAbs internalLocalUserIdentifier = PlatformManager.InternalLocalUserIdentifier;
			flag = !string.Equals(libraryId, (internalLocalUserIdentifier != null) ? internalLocalUserIdentifier.CombinedString : null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		foreach (SignData signData in signLibrary.signs.Values)
		{
			if ((!flag2 || signData.sharing != SignData.SignSharing.Private) && (string.IsNullOrEmpty(nameFilter) || signData.name.ContainsCaseInsensitive(nameFilter)))
			{
				GlobalSignId globalSignId = new GlobalSignId(libraryId, signData.guid);
				signIds.Add(globalSignId);
			}
		}
	}

	// Token: 0x060051F7 RID: 20983 RVA: 0x001F6D10 File Offset: 0x001F4F10
	public SignData.SignSharing GetEffectiveSharing(GlobalSignId signId)
	{
		SignData signData;
		if (!this.TryGetSignData(signId, out signData))
		{
			return SignData.SignSharing.Private;
		}
		return signData.sharing;
	}

	// Token: 0x060051F8 RID: 20984 RVA: 0x001F6D30 File Offset: 0x001F4F30
	public static bool IsLocalPlayerAlliedWith(string playerLibraryId)
	{
		PlatformUserIdentifierAbs platformUserIdentifierAbs;
		if (!PlatformUserIdentifierAbs.TryFromCombinedString(playerLibraryId, out platformUserIdentifierAbs))
		{
			return false;
		}
		PersistentPlayerData persistentLocalPlayer = GameManager.Instance.persistentLocalPlayer;
		return persistentLocalPlayer != null && persistentLocalPlayer.IsAlly(platformUserIdentifierAbs);
	}

	// Token: 0x060051F9 RID: 20985 RVA: 0x001F6D60 File Offset: 0x001F4F60
	public bool TryGetSignData(GlobalSignId signId, out SignData signData)
	{
		SignLibrary signLibrary;
		if (!this.signLibraries.TryGetValue(signId.libraryId, out signLibrary))
		{
			signData = null;
			return false;
		}
		return signLibrary.signs.TryGetValue(signId.signGuid, out signData);
	}

	// Token: 0x060051FA RID: 20986 RVA: 0x001F6D99 File Offset: 0x001F4F99
	public bool HasSignLibrary(string libraryId)
	{
		return this.signLibraries.ContainsKey(libraryId);
	}

	// Token: 0x060051FB RID: 20987 RVA: 0x001F6DA7 File Offset: 0x001F4FA7
	public bool TryGetSignLibrary(string libraryId, out SignLibrary library)
	{
		return this.signLibraries.TryGetValue(libraryId, out library);
	}

	// Token: 0x060051FC RID: 20988 RVA: 0x001F6DB8 File Offset: 0x001F4FB8
	public void ProcessSignReceived(string libraryId, byte[] signBlob)
	{
		SignData signData;
		try
		{
			int num;
			signData = SignDataManager.DeserializeSign(signBlob, out num);
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("[SignDataManager] Failed to deserialise a received sign for library '{0}': {1}", libraryId, ex));
			return;
		}
		GlobalSignId globalSignId = this.AddSignToLibraryLocal(libraryId, signData);
		this.MarkSignDirtyLocal(globalSignId);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.StorePlayerSignBlob(libraryId, signData.guid, signBlob);
		}
	}

	// Token: 0x060051FD RID: 20989 RVA: 0x001F6E20 File Offset: 0x001F5020
	public void ProcessSignDeleteReceived(GlobalSignId signId)
	{
		this.TryDeleteSignLocal(signId);
	}

	// Token: 0x060051FE RID: 20990 RVA: 0x001F6E2C File Offset: 0x001F502C
	public static bool IsPlayerLibraryId(string libraryId)
	{
		PlatformUserIdentifierAbs platformUserIdentifierAbs;
		return !string.IsNullOrEmpty(libraryId) && !SignDataManager.localLibraryIds.Contains(libraryId) && libraryId != "[D]" && PlatformUserIdentifierAbs.TryFromCombinedString(libraryId, out platformUserIdentifierAbs);
	}

	// Token: 0x060051FF RID: 20991 RVA: 0x001F6E68 File Offset: 0x001F5068
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] SerializeSign(SignData sign)
	{
		byte[] array;
		using (PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
		{
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
				pooledExpandableMemoryStream.SetLength(0L);
				sign.Write(pooledBinaryWriter);
				array = pooledExpandableMemoryStream.ToArray();
			}
		}
		return array;
	}

	// Token: 0x06005200 RID: 20992 RVA: 0x001F6EE0 File Offset: 0x001F50E0
	[PublicizedFrom(EAccessModifier.Private)]
	public static SignData DeserializeSign(byte[] bytes, out int binaryVersion)
	{
		SignData signData2;
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				pooledBinaryReader.SetBaseStream(memoryStream);
				SignData signData = SignData.Read(pooledBinaryReader, out binaryVersion);
				if (memoryStream.Position != memoryStream.Length)
				{
					throw new InvalidDataException(string.Format("Sign blob has {0} trailing byte(s) after deserialisation; SignData.Read and SignData.Write are out of sync.", memoryStream.Length - memoryStream.Position));
				}
				signData2 = signData;
			}
		}
		return signData2;
	}

	// Token: 0x06005201 RID: 20993 RVA: 0x001F6F74 File Offset: 0x001F5174
	[PublicizedFrom(EAccessModifier.Private)]
	public void StorePlayerSignBlob(string libraryId, Guid signGuid, byte[] signBlob)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || !SignDataManager.IsPlayerLibraryId(libraryId) || signBlob == null)
		{
			return;
		}
		SignDataManager.PlayerSignLibraryState orCreatePlayerSignState = this.GetOrCreatePlayerSignState(libraryId);
		orCreatePlayerSignState.blobsByGuid[signGuid] = signBlob;
		SignDataManager.MarkPlayerSignsDirty(orCreatePlayerSignState);
	}

	// Token: 0x06005202 RID: 20994 RVA: 0x001F6FA8 File Offset: 0x001F51A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void RemovePlayerSignBlob(GlobalSignId signId)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || !SignDataManager.IsPlayerLibraryId(signId.libraryId))
		{
			return;
		}
		SignDataManager.PlayerSignLibraryState playerSignLibraryState;
		if (!this.playerSignStates.TryGetValue(signId.libraryId, out playerSignLibraryState))
		{
			return;
		}
		if (playerSignLibraryState.blobsByGuid.Remove(signId.signGuid))
		{
			SignDataManager.MarkPlayerSignsDirty(playerSignLibraryState);
		}
	}

	// Token: 0x06005203 RID: 20995 RVA: 0x001F7000 File Offset: 0x001F5200
	[PublicizedFrom(EAccessModifier.Private)]
	public SignDataManager.PlayerSignLibraryState GetOrCreatePlayerSignState(string libraryId)
	{
		SignDataManager.PlayerSignLibraryState playerSignLibraryState;
		if (!this.playerSignStates.TryGetValue(libraryId, out playerSignLibraryState))
		{
			playerSignLibraryState = new SignDataManager.PlayerSignLibraryState();
			this.playerSignStates[libraryId] = playerSignLibraryState;
		}
		return playerSignLibraryState;
	}

	// Token: 0x06005204 RID: 20996 RVA: 0x001F7031 File Offset: 0x001F5231
	[PublicizedFrom(EAccessModifier.Private)]
	public static void MarkPlayerSignsDirty(SignDataManager.PlayerSignLibraryState state)
	{
		state.dirty = true;
	}

	// Token: 0x06005205 RID: 20997 RVA: 0x001F703C File Offset: 0x001F523C
	public void Update()
	{
		DeletedSignStore deletedSignStore = this.deletedSignStore;
		if (deletedSignStore != null)
		{
			deletedSignStore.Update();
		}
		if (this.playerSignStates.Count == 0)
		{
			return;
		}
		float unscaledTime = Time.unscaledTime;
		this.playerSignWriteScratch.Clear();
		foreach (KeyValuePair<string, SignDataManager.PlayerSignLibraryState> keyValuePair in this.playerSignStates)
		{
			SignDataManager.PlayerSignLibraryState value = keyValuePair.Value;
			SignDataManager.<Update>g__CheckCompletedPlayerSignWrite|126_0(value);
			if (value.dirty && !value.writesBlocked && value.writeTask == null && unscaledTime - value.lastWriteTime >= 60f)
			{
				this.playerSignWriteScratch.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < this.playerSignWriteScratch.Count; i++)
		{
			string text = this.playerSignWriteScratch[i];
			this.BeginPlayerSignWrite(text, this.playerSignStates[text], false);
		}
		this.playerSignWriteScratch.Clear();
	}

	// Token: 0x06005206 RID: 20998 RVA: 0x001F714C File Offset: 0x001F534C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BeginPlayerSignWrite(string libraryId, SignDataManager.PlayerSignLibraryState state, bool waitForCompletion)
	{
		state.dirty = false;
		state.lastWriteTime = Time.unscaledTime;
		PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
		try
		{
			this.playerSignBlobScratch.Clear();
			foreach (byte[] array in state.blobsByGuid.Values)
			{
				this.playerSignBlobScratch.Add(array);
			}
			PlayerSignStore.BuildImage(this.playerSignBlobScratch, pooledExpandableMemoryStream);
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("[SignDataManager] Failed to build sign store image for '{0}': {1}", libraryId, ex));
			MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
			return;
		}
		finally
		{
			this.playerSignBlobScratch.Clear();
		}
		SignDataManager.PlayerSignWriteRequest playerSignWriteRequest = new SignDataManager.PlayerSignWriteRequest
		{
			libraryId = libraryId,
			image = pooledExpandableMemoryStream
		};
		if (waitForCompletion)
		{
			SignDataManager.WritePlayerSignImage(playerSignWriteRequest);
			return;
		}
		state.writeTask = ThreadManager.AddSingleTask(new ThreadManager.TaskFunctionDelegate(SignDataManager.PlayerSignWriteTask), playerSignWriteRequest, null, true);
	}

	// Token: 0x06005207 RID: 20999 RVA: 0x001F7260 File Offset: 0x001F5460
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PlayerSignWriteTask(ThreadManager.TaskInfo _taskInfo)
	{
		SignDataManager.WritePlayerSignImage((SignDataManager.PlayerSignWriteRequest)_taskInfo.parameter);
	}

	// Token: 0x06005208 RID: 21000 RVA: 0x001F7274 File Offset: 0x001F5474
	[PublicizedFrom(EAccessModifier.Private)]
	public static void WritePlayerSignImage(SignDataManager.PlayerSignWriteRequest request)
	{
		try
		{
			PlayerSignStore.Write(request.libraryId, request.image);
		}
		catch (Exception ex)
		{
			Log.Error(string.Format("[SignDataManager] Failed to write sign store for '{0}': {1}", request.libraryId, ex));
		}
		finally
		{
			MemoryPools.poolMemoryStream.FreeSync(request.image);
			request.image = null;
		}
	}

	// Token: 0x06005209 RID: 21001 RVA: 0x001F72E4 File Offset: 0x001F54E4
	public void FlushPendingPlayerSignWrites()
	{
		foreach (KeyValuePair<string, SignDataManager.PlayerSignLibraryState> keyValuePair in this.playerSignStates)
		{
			SignDataManager.PlayerSignLibraryState value = keyValuePair.Value;
			if (value.writeTask != null)
			{
				value.writeTask.WaitForEnd();
				ThreadManager.ReleaseTaskInfo(value.writeTask);
				value.writeTask = null;
			}
			if (value.dirty && !value.writesBlocked)
			{
				this.BeginPlayerSignWrite(keyValuePair.Key, value, true);
			}
		}
	}

	// Token: 0x0600520A RID: 21002 RVA: 0x001F737C File Offset: 0x001F557C
	public void LoadPlayerSignLibraries()
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			return;
		}
		this.deletedSignStore = new DeletedSignStore();
		this.deletedSignStore.Load();
		HashSet<string> hashSet = new HashSet<string>();
		PlayerSignStore.GetStoredLibraryIds(hashSet);
		List<byte[]> list = new List<byte[]>();
		foreach (string text in hashSet)
		{
			if (!SignDataManager.IsPlayerLibraryId(text))
			{
				Log.Warning("[SignDataManager] Skipping sign store with unrecognised player id: " + text);
			}
			else
			{
				list.Clear();
				int num;
				if (!PlayerSignStore.TryLoad(text, list, out num))
				{
					this.GetOrCreatePlayerSignState(text).writesBlocked = true;
					Log.Error("[SignDataManager] No readable sign store for player '" + text + "'; their libray appears on disk but signs could not be loaded. Writes for this library are disabled for the session to avoid overwriting it.");
				}
				else
				{
					bool flag = num != 3;
					if (flag)
					{
						Log.Out(string.Format("[SignDataManager] Migrating sign store for player '{0}' from container version {1} to {2}.", text, num, 3));
					}
					SignLibrary signLibrary = new SignLibrary();
					SignDataManager.PlayerSignLibraryState orCreatePlayerSignState = this.GetOrCreatePlayerSignState(text);
					orCreatePlayerSignState.blobsByGuid.Clear();
					orCreatePlayerSignState.dirty = false;
					int num2 = 0;
					int num3 = 0;
					for (int i = 0; i < list.Count; i++)
					{
						byte[] array = list[i];
						try
						{
							int num4;
							SignData signData = SignDataManager.DeserializeSign(array, out num4);
							signLibrary.signs[signData.guid] = signData;
							bool flag2 = num4 != 3;
							orCreatePlayerSignState.blobsByGuid[signData.guid] = (flag2 ? SignDataManager.SerializeSign(signData) : array);
							if (flag2)
							{
								num3++;
							}
						}
						catch (Exception ex)
						{
							num2++;
							Log.Error(string.Format("[SignDataManager] Failed to deserialise sign {0} of {1} for player '{2}'; skipping it: {3}", new object[] { i, list.Count, text, ex }));
						}
					}
					if (num3 > 0)
					{
						Log.Out(string.Format("[SignDataManager] Migrated {0} of {1} sign(s) for player '{2}' to blob version {3}.", new object[] { num3, list.Count, text, 3 }));
					}
					if (num2 > 0)
					{
						PlayerSignStore.PreserveUnreadable(text);
					}
					if (num2 > 0 && num2 == list.Count)
					{
						orCreatePlayerSignState.writesBlocked = true;
						Log.Error(string.Format("[SignDataManager] Every one of the {0} sign record(s) for player '{1}' failed to load. ", list.Count, text) + "This suggests a SignData serialisation problem rather than a damaged store, so writes for this library are disabled for the session to avoid overwriting it. The store has been preserved for investigation.");
					}
					else if (num2 > 0 || num3 > 0 || flag)
					{
						SignDataManager.MarkPlayerSignsDirty(orCreatePlayerSignState);
					}
					this.TryRegisterLibrary(text, signLibrary, true);
				}
			}
		}
		this.deletedSignStore.PruneExistingSigns(this);
	}

	// Token: 0x0600520C RID: 21004 RVA: 0x001F7674 File Offset: 0x001F5874
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__ClearIntermediateLists|106_0(ref SignDataManager.<>c__DisplayClass106_0 A_1)
	{
		this.colorOpStack.Clear();
		SignDataManager.IIntermediateDataWrapper[] array = this.intermediateDataWrappers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ClearStagingList();
		}
	}

	// Token: 0x0600520D RID: 21005 RVA: 0x001F76AC File Offset: 0x001F58AC
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public LayerStackInfo <UpdateRenderingData>g__ProcessLayers|106_1(List<SignData.SignLayer> layers, SignData.SignTransform parentTransform, List<SignDataManager.ColorOperation> colorOps, SignDataManager.GroupOffsets parentOffsets = default(SignDataManager.GroupOffsets), ref SignDataManager.<>c__DisplayClass106_0 A_5)
	{
		int num = 0;
		LayerStackInfo layerStackInfo = new LayerStackInfo(A_5.compStackIndex, A_5.uvStackIndex);
		foreach (SignData.SignLayer signLayer in layers)
		{
			LayerComplexityInfo layerComplexityInfo = default(LayerComplexityInfo);
			layerComplexityInfo.BaseOffset = A_5.signRenderingData.totalComplexity;
			layerComplexityInfo.StartCompStackIndex = A_5.compStackIndex;
			layerComplexityInfo.MaxCompStackIndex = A_5.compStackIndex;
			layerComplexityInfo.StartUVStackIndex = A_5.uvStackIndex;
			layerComplexityInfo.MaxUVStackIndex = A_5.uvStackIndex;
			layerComplexityInfo.GroupDepth = 0;
			int count = this.layerDescriptors.List.Count;
			int totalDraws = A_5.signRenderingData.totalDraws;
			int totalWarps = A_5.signRenderingData.totalWarps;
			bool flag = false;
			SignData.SignRenderSettings.Mode mode = signLayer.renderSettings.mode;
			if (mode != SignData.SignRenderSettings.Mode.ColorOnly && mode - SignData.SignRenderSettings.Mode.ColorAndMask <= 2)
			{
				this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
				{
					type = 3,
					index = (int)signLayer.renderSettings.mode
				});
				flag = true;
				A_5.signRenderingData.totalComplexity += 0.5f;
				int compStackIndex = A_5.compStackIndex;
				A_5.compStackIndex = compStackIndex + 1;
				layerComplexityInfo.MaxCompStackIndex = A_5.compStackIndex;
			}
			SignData.GroupSignLayer groupSignLayer = signLayer as SignData.GroupSignLayer;
			if (groupSignLayer == null)
			{
				SignData.TextSignLayer textSignLayer = signLayer as SignData.TextSignLayer;
				if (textSignLayer == null)
				{
					SignData.PolygonSignLayer polygonSignLayer = signLayer as SignData.PolygonSignLayer;
					if (polygonSignLayer == null)
					{
						SignData.NoiseSignLayer noiseSignLayer = signLayer as SignData.NoiseSignLayer;
						if (noiseSignLayer != null)
						{
							if (signLayer.HasTransformOrWarps || parentTransform != SignData.SignTransform.Identity)
							{
								this.<UpdateRenderingData>g__PushWarps|106_5(parentTransform, signLayer, ref A_5);
								layerComplexityInfo.MaxUVStackIndex = A_5.uvStackIndex;
								this.<UpdateRenderingData>g__ProcessNoiseLayer|106_3(noiseSignLayer, colorOps, parentOffsets, ref A_5);
								this.<UpdateRenderingData>g__PopWarps|106_6(ref A_5);
							}
							else
							{
								this.<UpdateRenderingData>g__ProcessNoiseLayer|106_3(noiseSignLayer, colorOps, parentOffsets, ref A_5);
							}
						}
					}
					else if (signLayer.HasTransformOrWarps || parentTransform != SignData.SignTransform.Identity)
					{
						this.<UpdateRenderingData>g__PushWarps|106_5(parentTransform, signLayer, ref A_5);
						layerComplexityInfo.MaxUVStackIndex = A_5.uvStackIndex;
						this.<UpdateRenderingData>g__ProcessPolygonLayer|106_2(polygonSignLayer, colorOps, parentOffsets, ref A_5);
						this.<UpdateRenderingData>g__PopWarps|106_6(ref A_5);
					}
					else
					{
						this.<UpdateRenderingData>g__ProcessPolygonLayer|106_2(polygonSignLayer, colorOps, parentOffsets, ref A_5);
					}
				}
				else if (signLayer.HasTransformOrWarps || parentTransform != SignData.SignTransform.Identity)
				{
					this.<UpdateRenderingData>g__PushWarps|106_5(parentTransform, signLayer, ref A_5);
					layerComplexityInfo.MaxUVStackIndex = A_5.uvStackIndex;
					this.<UpdateRenderingData>g__ProcessTextLayer|106_4(textSignLayer, colorOps, parentOffsets, ref A_5);
					this.<UpdateRenderingData>g__PopWarps|106_6(ref A_5);
				}
				else
				{
					this.<UpdateRenderingData>g__ProcessTextLayer|106_4(textSignLayer, colorOps, parentOffsets, ref A_5);
				}
			}
			else
			{
				SignDataManager.GroupOffsets groupOffsets = parentOffsets.WithOffsets(groupSignLayer.softnessOffset, groupSignLayer.dilateOffset, groupSignLayer.offsetTarget);
				Color color = groupSignLayer.renderSettings.color;
				bool flag2 = (groupSignLayer.colorMode == SignData.GroupSignLayer.ColorMode.Multiply && color == Color.white) || (groupSignLayer.colorMode == SignData.GroupSignLayer.ColorMode.Blend && color.a == 0f);
				if (!flag2)
				{
					colorOps.Add(new SignDataManager.ColorOperation
					{
						color = color,
						mode = groupSignLayer.colorMode
					});
				}
				if (groupSignLayer.HasWarps || !groupSignLayer.transform.HasUniformScale)
				{
					this.<UpdateRenderingData>g__PushWarps|106_5(parentTransform, signLayer, ref A_5);
					LayerStackInfo layerStackInfo2 = this.<UpdateRenderingData>g__ProcessLayers|106_1(groupSignLayer.layers, SignData.SignTransform.Identity, colorOps, groupOffsets, ref A_5);
					layerComplexityInfo.MaxCompStackIndex = layerStackInfo2.MaxCompStackIndex;
					layerComplexityInfo.MaxUVStackIndex = layerStackInfo2.MaxUVStackIndex;
					layerComplexityInfo.GroupDepth = layerStackInfo2.GroupDepth + 1;
					this.<UpdateRenderingData>g__PopWarps|106_6(ref A_5);
				}
				else
				{
					LayerStackInfo layerStackInfo3 = this.<UpdateRenderingData>g__ProcessLayers|106_1(groupSignLayer.layers, parentTransform * groupSignLayer.transform, colorOps, groupOffsets, ref A_5);
					layerComplexityInfo.MaxCompStackIndex = layerStackInfo3.MaxCompStackIndex;
					layerComplexityInfo.MaxUVStackIndex = layerStackInfo3.MaxUVStackIndex;
					layerComplexityInfo.GroupDepth = layerStackInfo3.GroupDepth + 1;
				}
				if (!flag2)
				{
					colorOps.RemoveAt(colorOps.Count - 1);
				}
			}
			if (flag)
			{
				this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
				{
					type = 4,
					index = -1
				});
				num++;
				A_5.signRenderingData.totalComplexity += 0.55f;
			}
			else if (num > 0)
			{
				this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
				{
					type = 5,
					index = num
				});
				A_5.signRenderingData.totalComplexity += 0.5f + 0.05f * (float)num;
				A_5.compStackIndex -= num;
				num = 0;
			}
			int num2 = this.layerDescriptors.List.Count - count;
			layerComplexityInfo.EndCompStackIndex = A_5.compStackIndex;
			layerComplexityInfo.EndUVStackIndex = A_5.uvStackIndex;
			layerComplexityInfo.Complexity = A_5.signRenderingData.totalComplexity - layerComplexityInfo.BaseOffset;
			layerComplexityInfo.WarpCount = A_5.signRenderingData.totalWarps - totalWarps;
			layerComplexityInfo.DrawCount = A_5.signRenderingData.totalDraws - totalDraws;
			layerComplexityInfo.DescriptorCount = num2;
			A_5.signRenderingData.renderingInfoByLayer[signLayer] = new SignDataManager.RuntimeRenderingInfo(count, num2);
			A_5.signRenderingData.complexityByLayer[signLayer] = layerComplexityInfo;
			layerStackInfo.MaxCompStackIndex = Mathf.Max(layerStackInfo.MaxCompStackIndex, layerComplexityInfo.MaxCompStackIndex);
			layerStackInfo.MaxUVStackIndex = Mathf.Max(layerStackInfo.MaxUVStackIndex, layerComplexityInfo.MaxUVStackIndex);
			layerStackInfo.GroupDepth = Mathf.Max(layerStackInfo.GroupDepth, layerComplexityInfo.GroupDepth);
		}
		if (num > 0)
		{
			this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
			{
				type = 5,
				index = num
			});
			A_5.signRenderingData.totalComplexity += 0.5f + 0.05f * (float)num;
			A_5.compStackIndex -= num;
			num = 0;
		}
		return layerStackInfo;
	}

	// Token: 0x0600520E RID: 21006 RVA: 0x001F7CD8 File Offset: 0x001F5ED8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__PushWarps|106_5(SignData.SignTransform parentTransform, SignData.SignLayer layer, ref SignDataManager.<>c__DisplayClass106_0 A_3)
	{
		this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
		{
			type = 2,
			index = -1
		});
		A_3.signRenderingData.totalComplexity += 0.5f;
		int uvStackIndex = A_3.uvStackIndex;
		A_3.uvStackIndex = uvStackIndex + 1;
		this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
		{
			type = 10,
			index = this.transformWarps.List.Count
		});
		SignData.SignTransform signTransform = parentTransform * layer.transform;
		this.transformWarps.List.Add(new SignDataManager.TransformWarp
		{
			position = signTransform.position,
			rotation = signTransform.rotation * 0.017453292f,
			scale = signTransform.scale
		});
		A_3.signRenderingData.totalComplexity += 1.3f;
		A_3.signRenderingData.totalWarps++;
		for (int i = layer.warps.Count - 1; i >= 0; i--)
		{
			SignData.SignWarp signWarp = layer.warps[i];
			SignData.SkewWarp skewWarp = signWarp as SignData.SkewWarp;
			if (skewWarp == null)
			{
				SignData.BulgeWarp bulgeWarp = signWarp as SignData.BulgeWarp;
				if (bulgeWarp == null)
				{
					SignData.TwirlWarp twirlWarp = signWarp as SignData.TwirlWarp;
					if (twirlWarp == null)
					{
						SignData.KaleidoWarp kaleidoWarp = signWarp as SignData.KaleidoWarp;
						if (kaleidoWarp == null)
						{
							SignData.PerspectiveWarp perspectiveWarp = signWarp as SignData.PerspectiveWarp;
							if (perspectiveWarp == null)
							{
								SignData.ArcWarp arcWarp = signWarp as SignData.ArcWarp;
								if (arcWarp == null)
								{
									SignData.StretchWarp stretchWarp = signWarp as SignData.StretchWarp;
									if (stretchWarp == null)
									{
										SignData.GridWarp gridWarp = signWarp as SignData.GridWarp;
										if (gridWarp == null)
										{
											Log.Error(string.Format("Unsupported layer warp type at index {0}.", i));
										}
										else
										{
											this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
											{
												type = 18,
												index = this.gridWarps.List.Count
											});
											this.gridWarps.List.Add(new SignDataManager.GridWarp
											{
												offset = gridWarp.offset,
												rotation = 0.017453292f * gridWarp.rotation,
												scale = gridWarp.scale,
												mode = (int)gridWarp.mode,
												aspect = gridWarp.aspect,
												shift = gridWarp.shift
											});
											float num;
											switch (gridWarp.mode)
											{
											case SignData.GridWarp.Mode.Column:
												num = 1.26f;
												break;
											case SignData.GridWarp.Mode.Rectangle:
												num = 1.46f;
												break;
											case SignData.GridWarp.Mode.Hex:
												num = 1.66f;
												break;
											default:
												num = 1.46f;
												break;
											}
											float num2 = num;
											A_3.signRenderingData.totalComplexity += 0.5f + num2;
										}
									}
									else
									{
										this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
										{
											type = 17,
											index = this.stretchWarps.List.Count
										});
										this.stretchWarps.List.Add(new SignDataManager.StretchWarp
										{
											position = stretchWarp.offset,
											rotation = 0.017453292f * stretchWarp.rotation,
											distance = stretchWarp.distance,
											width = stretchWarp.distance * stretchWarp.width,
											exponent = stretchWarp.exponent
										});
										A_3.signRenderingData.totalComplexity += 2f;
									}
								}
								else
								{
									this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
									{
										type = 16,
										index = this.arcWarps.List.Count
									});
									this.arcWarps.List.Add(new SignDataManager.ArcWarp
									{
										rotation = 0.017453292f * arcWarp.rotation,
										radius = arcWarp.radius,
										width = arcWarp.width
									});
									A_3.signRenderingData.totalComplexity += 1.95f;
								}
							}
							else
							{
								this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
								{
									type = 15,
									index = this.perspectiveWarps.List.Count
								});
								this.perspectiveWarps.List.Add(new SignDataManager.PerspectiveWarp
								{
									rotation = 0.017453292f * perspectiveWarp.rotation,
									strength = Mathf.Lerp(0.01f, 5f, Mathf.Pow(1f - perspectiveWarp.strength, 2f))
								});
								A_3.signRenderingData.totalComplexity += 1.96f;
							}
						}
						else
						{
							this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
							{
								type = 14,
								index = this.kaleidoWarps.List.Count
							});
							this.kaleidoWarps.List.Add(new SignDataManager.KaleidoWarp
							{
								position = kaleidoWarp.offset * kaleidoWarp.offsetScale,
								angle = 0.017453292f * (360f / (float)kaleidoWarp.sides),
								offset = 0.017453292f * (kaleidoWarp.rotation + 360f)
							});
							A_3.signRenderingData.totalComplexity += 1.76f;
						}
					}
					else
					{
						this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
						{
							type = 13,
							index = this.twirlWarps.List.Count
						});
						this.twirlWarps.List.Add(new SignDataManager.TwirlWarp
						{
							position = twirlWarp.offset,
							amount = 20f * twirlWarp.amount,
							frequency = 100f * twirlWarp.frequency
						});
						A_3.signRenderingData.totalComplexity += 1.45f;
					}
				}
				else
				{
					this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
					{
						type = 12,
						index = this.bulgeWarps.List.Count
					});
					this.bulgeWarps.List.Add(new SignDataManager.BulgeWarp
					{
						position = bulgeWarp.offset,
						amount = bulgeWarp.amount
					});
					A_3.signRenderingData.totalComplexity += 1.78f;
				}
			}
			else
			{
				this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
				{
					type = 11,
					index = this.skewWarps.List.Count
				});
				this.skewWarps.List.Add(new SignDataManager.SkewWarp
				{
					rotation = 0.017453292f * skewWarp.rotation,
					amount = skewWarp.amount
				});
				A_3.signRenderingData.totalComplexity += 1.475f;
			}
			A_3.signRenderingData.totalWarps++;
		}
	}

	// Token: 0x0600520F RID: 21007 RVA: 0x001F846C File Offset: 0x001F666C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__PopWarps|106_6(ref SignDataManager.<>c__DisplayClass106_0 A_1)
	{
		SignDataManager.LayerDescriptor layerDescriptor = this.layerDescriptors.List[this.layerDescriptors.List.Count - 1];
		if (layerDescriptor.type == 1)
		{
			layerDescriptor.index--;
			this.layerDescriptors.List[this.layerDescriptors.List.Count - 1] = layerDescriptor;
		}
		else
		{
			this.layerDescriptors.List.Add(new SignDataManager.LayerDescriptor
			{
				type = 1,
				index = -1
			});
			A_1.signRenderingData.totalComplexity += 0.5f;
		}
		int uvStackIndex = A_1.uvStackIndex;
		A_1.uvStackIndex = uvStackIndex - 1;
	}

	// Token: 0x06005210 RID: 21008 RVA: 0x001F8528 File Offset: 0x001F6728
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__ProcessPolygonLayer|106_2(SignData.PolygonSignLayer polygonLayer, List<SignDataManager.ColorOperation> colorOps, SignDataManager.GroupOffsets offsets, ref SignDataManager.<>c__DisplayClass106_0 A_4)
	{
		float num = Mathf.Max(0f, polygonLayer.softness + offsets.shapeSoftnessOffset);
		float num2 = polygonLayer.dilate + offsets.shapeDilateOffset;
		Color color = SignDataManager.ColorOperation.EvaluateColor(polygonLayer.renderSettings.color, colorOps).GammaToLinearSpace(true);
		if (polygonLayer.smoothness > 0.99f)
		{
			SignDataManager.LayerDescriptor layerDescriptor = new SignDataManager.LayerDescriptor
			{
				type = 102,
				index = this.circleLayers.List.Count
			};
			SignDataManager.CircleLayer circleLayer = new SignDataManager.CircleLayer
			{
				color = color,
				softness = num,
				dilate = num2,
				frequency = polygonLayer.frequency,
				mode = (int)polygonLayer.shapeMode
			};
			this.layerDescriptors.List.Add(layerDescriptor);
			this.circleLayers.List.Add(circleLayer);
			A_4.signRenderingData.totalComplexity += 1.5f;
			A_4.signRenderingData.totalDraws++;
			return;
		}
		SignDataManager.LayerDescriptor layerDescriptor2 = new SignDataManager.LayerDescriptor
		{
			type = 100,
			index = this.polygonLayers.List.Count
		};
		SignDataManager.PolygonLayer polygonLayer2 = new SignDataManager.PolygonLayer
		{
			color = color,
			sides = (float)polygonLayer.sides,
			smoothness = polygonLayer.smoothness,
			starify = polygonLayer.starify,
			softness = num,
			dilate = num2,
			frequency = polygonLayer.frequency,
			mode = (int)polygonLayer.shapeMode
		};
		this.layerDescriptors.List.Add(layerDescriptor2);
		this.polygonLayers.List.Add(polygonLayer2);
		A_4.signRenderingData.totalComplexity += 2.875f;
		A_4.signRenderingData.totalDraws++;
	}

	// Token: 0x06005211 RID: 21009 RVA: 0x001F8728 File Offset: 0x001F6928
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__ProcessNoiseLayer|106_3(SignData.NoiseSignLayer noiseLayer, List<SignDataManager.ColorOperation> colorOps, SignDataManager.GroupOffsets offsets, ref SignDataManager.<>c__DisplayClass106_0 A_4)
	{
		SignDataManager.LayerDescriptor layerDescriptor = new SignDataManager.LayerDescriptor
		{
			type = 103,
			index = this.noiseLayers.List.Count
		};
		float num = Mathf.Max(0f, noiseLayer.softness + offsets.noiseSoftnessOffset);
		float num2 = noiseLayer.dilate + offsets.noiseDilateOffset;
		SignDataManager.NoiseLayer noiseLayer2 = new SignDataManager.NoiseLayer
		{
			color = SignDataManager.ColorOperation.EvaluateColor(noiseLayer.renderSettings.color, colorOps).GammaToLinearSpace(true),
			offset = (float)(noiseLayer.seed * 10),
			detail = noiseLayer.detail,
			softness = num,
			dilate = num2,
			fade = noiseLayer.fade
		};
		this.layerDescriptors.List.Add(layerDescriptor);
		this.noiseLayers.List.Add(noiseLayer2);
		A_4.signRenderingData.totalComplexity += 1.5f + 1.1f * (float)noiseLayer.detail;
		A_4.signRenderingData.totalDraws++;
	}

	// Token: 0x06005212 RID: 21010 RVA: 0x001F884C File Offset: 0x001F6A4C
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__ProcessTextLayer|106_4(SignData.TextSignLayer textLayer, List<SignDataManager.ColorOperation> colorOps, SignDataManager.GroupOffsets offsets, ref SignDataManager.<>c__DisplayClass106_0 A_4)
	{
		SignDataManager.SignFontData signFontData;
		if (textLayer.font == null || !this.fontDataByName.TryGetValue(textLayer.font, out signFontData))
		{
			Log.Error("Failed to find font data for font `" + textLayer.font + "`.");
			if (!this.fontDataByName.TryGetValue("LiberationSans", out signFontData))
			{
				Log.Error("Failed to load default fallback font `LiberationSans`.");
				return;
			}
		}
		int count = this.charLayers.List.Count;
		Color color = SignDataManager.ColorOperation.EvaluateColor(textLayer.renderSettings.color, colorOps).GammaToLinearSpace(true);
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		float num = 1f - Mathf.Abs(90f - (textLayer.direction + 360f) % 180f) / 90f;
		float num2 = textLayer.direction * 0.017453292f;
		Vector2 vector3 = new Vector2(Mathf.Cos(num2), -Mathf.Sin(num2));
		float num3 = Mathf.Max(0f, textLayer.spacing - 1f) * signFontData.LineHeight;
		float num4 = Mathf.Clamp01(textLayer.spacing);
		float num5 = Mathf.Clamp01(textLayer.softness + offsets.textSoftnessOffset);
		float num6 = Mathf.Clamp(textLayer.dilate + offsets.textDilateOffset, -1f, 1f);
		float num7 = num5 * (1f - Mathf.Abs(num6));
		float num8 = num6 * (1f - 0.5f * num5);
		foreach (char c in textLayer.text)
		{
			SignDataManager.CharAtlasData charAtlasData;
			if (!signFontData.TryGetCharAtlasData(c, out charAtlasData))
			{
				Log.Warning(string.Concat(new string[]
				{
					string.Format("Could not retrieve atlas data for character \"{0}\" in string \"{1}\",", c, textLayer.text),
					" found in sign \"",
					A_4.signData.name,
					"\" (library:\"",
					A_4.id.libraryId,
					"\"). \nPlease check \"TextSignLayer\" elements in the relevant _signs.xml for invalid characters, as they may not be visible in the Sign Editor tool."
				}));
			}
			else
			{
				float num9 = Mathf.Lerp(charAtlasData.charWidth, signFontData.LineHeight, num);
				float num10 = num4 * (num9 + num3);
				Vector2 quadOffset = charAtlasData.quadOffset;
				quadOffset.x *= vector3.x;
				vector2 = vector + vector3 * Mathf.Lerp(num9, 0f, num);
				if (c == ' ')
				{
					vector += vector3 * num10;
				}
				else
				{
					SignDataManager.LayerDescriptor layerDescriptor = new SignDataManager.LayerDescriptor
					{
						type = 101,
						index = this.charLayers.List.Count
					};
					SignDataManager.CharLayer charLayer = new SignDataManager.CharLayer
					{
						position = vector + quadOffset,
						scale = charAtlasData.quadSize,
						color = color,
						atlasIndex = signFontData.AtlasIndex,
						offsetSize = charAtlasData.uvOffsetSize,
						softness = num7,
						dilate = num8
					};
					this.layerDescriptors.List.Add(layerDescriptor);
					this.charLayers.List.Add(charLayer);
					A_4.signRenderingData.totalComplexity += 2.275f;
					A_4.signRenderingData.totalDraws++;
					vector += vector3 * num10;
				}
			}
		}
		Vector2 vector4 = -0.5f * vector2;
		for (int j = count; j < this.charLayers.List.Count; j++)
		{
			SignDataManager.CharLayer charLayer2 = this.charLayers.List[j];
			charLayer2.position += vector4;
			this.charLayers.List[j] = charLayer2;
		}
	}

	// Token: 0x06005213 RID: 21011 RVA: 0x001F8C27 File Offset: 0x001F6E27
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Internal)]
	public static void <Update>g__CheckCompletedPlayerSignWrite|126_0(SignDataManager.PlayerSignLibraryState state)
	{
		if (state.writeTask == null)
		{
			return;
		}
		if (state.writeTask.evStopped == null || state.writeTask.evStopped.WaitOne(0))
		{
			ThreadManager.ReleaseTaskInfo(state.writeTask);
			state.writeTask = null;
		}
	}

	// Token: 0x04003F43 RID: 16195
	[PublicizedFrom(EAccessModifier.Private)]
	public const string fontConfigLoc = "@:Fonts/SignFontConfig.asset";

	// Token: 0x04003F44 RID: 16196
	[PublicizedFrom(EAccessModifier.Private)]
	public const float baseDescriptorComplexity = 0.5f;

	// Token: 0x04003F45 RID: 16197
	public const string defaultFontName = "LiberationSans";

	// Token: 0x04003F46 RID: 16198
	public static readonly ProfilerMarker s_SignDataManagerReplaceLoadedSignReferences = new ProfilerMarker("SignDataManager.ReplaceLoadedSignReferences");

	// Token: 0x04003F47 RID: 16199
	public const string defaultLibraryId = "[D]";

	// Token: 0x04003F48 RID: 16200
	public const string internalLibraryId = "[I]";

	// Token: 0x04003F49 RID: 16201
	public const string userLibraryId = "[U]";

	// Token: 0x04003F4A RID: 16202
	public static readonly HashSet<string> localLibraryIds = new HashSet<string> { "[I]", "[U]" };

	// Token: 0x04003F4B RID: 16203
	public const float cMaxComplexity = 600f;

	// Token: 0x04003F4C RID: 16204
	public const int cMaxCompStackIndex = 7;

	// Token: 0x04003F4D RID: 16205
	public const int cMaxUVStackIndex = 7;

	// Token: 0x04003F4E RID: 16206
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, SignDataManager.SignFontData> fontDataByName = new Dictionary<string, SignDataManager.SignFontData>();

	// Token: 0x04003F4F RID: 16207
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, SignLibrary> signLibraries = new Dictionary<string, SignLibrary>(100);

	// Token: 0x04003F50 RID: 16208
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<GlobalSignId, SignDataManager.SignRenderingData> renderingDataByID = new Dictionary<GlobalSignId, SignDataManager.SignRenderingData>(100);

	// Token: 0x04003F51 RID: 16209
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.UpdateListenerMap updateListenerMap = new SignDataManager.UpdateListenerMap(100);

	// Token: 0x04003F52 RID: 16210
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly HashSet<GlobalSignId> sessionDeletedSignIds = new HashSet<GlobalSignId>();

	// Token: 0x04003F53 RID: 16211
	[PublicizedFrom(EAccessModifier.Private)]
	public DeletedSignStore deletedSignStore;

	// Token: 0x04003F54 RID: 16212
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.LayerDescriptor> layerDescriptors = new SignDataManager.IntermediateDataWrapper<SignDataManager.LayerDescriptor>(SignShaderIDs._LayerDescriptors, 100, (SignDataManager.SignRenderingData srd) => srd.descriptorBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.descriptorBuffer = b;
	});

	// Token: 0x04003F55 RID: 16213
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.CircleLayer> circleLayers = new SignDataManager.IntermediateDataWrapper<SignDataManager.CircleLayer>(SignShaderIDs._CircleLayers, 100, (SignDataManager.SignRenderingData srd) => srd.circleBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.circleBuffer = b;
	});

	// Token: 0x04003F56 RID: 16214
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.PolygonLayer> polygonLayers = new SignDataManager.IntermediateDataWrapper<SignDataManager.PolygonLayer>(SignShaderIDs._PolygonLayers, 100, (SignDataManager.SignRenderingData srd) => srd.polygonBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.polygonBuffer = b;
	});

	// Token: 0x04003F57 RID: 16215
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.CharLayer> charLayers = new SignDataManager.IntermediateDataWrapper<SignDataManager.CharLayer>(SignShaderIDs._CharLayers, 100, (SignDataManager.SignRenderingData srd) => srd.charBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.charBuffer = b;
	});

	// Token: 0x04003F58 RID: 16216
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.NoiseLayer> noiseLayers = new SignDataManager.IntermediateDataWrapper<SignDataManager.NoiseLayer>(SignShaderIDs._NoiseLayers, 100, (SignDataManager.SignRenderingData srd) => srd.noiseBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.noiseBuffer = b;
	});

	// Token: 0x04003F59 RID: 16217
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.TransformWarp> transformWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.TransformWarp>(SignShaderIDs._TransformWarps, 100, (SignDataManager.SignRenderingData srd) => srd.transformBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.transformBuffer = b;
	});

	// Token: 0x04003F5A RID: 16218
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.SkewWarp> skewWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.SkewWarp>(SignShaderIDs._SkewWarps, 100, (SignDataManager.SignRenderingData srd) => srd.skewBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.skewBuffer = b;
	});

	// Token: 0x04003F5B RID: 16219
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.BulgeWarp> bulgeWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.BulgeWarp>(SignShaderIDs._BulgeWarps, 100, (SignDataManager.SignRenderingData srd) => srd.bulgeBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.bulgeBuffer = b;
	});

	// Token: 0x04003F5C RID: 16220
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.TwirlWarp> twirlWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.TwirlWarp>(SignShaderIDs._TwirlWarps, 100, (SignDataManager.SignRenderingData srd) => srd.twirlBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.twirlBuffer = b;
	});

	// Token: 0x04003F5D RID: 16221
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.KaleidoWarp> kaleidoWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.KaleidoWarp>(SignShaderIDs._KaleidoWarps, 100, (SignDataManager.SignRenderingData srd) => srd.kaleidoBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.kaleidoBuffer = b;
	});

	// Token: 0x04003F5E RID: 16222
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.PerspectiveWarp> perspectiveWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.PerspectiveWarp>(SignShaderIDs._PerspectiveWarps, 100, (SignDataManager.SignRenderingData srd) => srd.perspectiveBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.perspectiveBuffer = b;
	});

	// Token: 0x04003F5F RID: 16223
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.ArcWarp> arcWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.ArcWarp>(SignShaderIDs._ArcWarps, 100, (SignDataManager.SignRenderingData srd) => srd.arcBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.arcBuffer = b;
	});

	// Token: 0x04003F60 RID: 16224
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.StretchWarp> stretchWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.StretchWarp>(SignShaderIDs._StretchWarps, 100, (SignDataManager.SignRenderingData srd) => srd.stretchBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.stretchBuffer = b;
	});

	// Token: 0x04003F61 RID: 16225
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.GridWarp> gridWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.GridWarp>(SignShaderIDs._GridWarps, 100, (SignDataManager.SignRenderingData srd) => srd.gridBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.gridBuffer = b;
	});

	// Token: 0x04003F62 RID: 16226
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IIntermediateDataWrapper[] intermediateDataWrappers;

	// Token: 0x04003F63 RID: 16227
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SignDataManager.ColorOperation> colorOpStack = new List<SignDataManager.ColorOperation>();

	// Token: 0x04003F64 RID: 16228
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2DArray fontAtlasTextureArray;

	// Token: 0x04003F65 RID: 16229
	[PublicizedFrom(EAccessModifier.Private)]
	public static SignDataManager m_Instance;

	// Token: 0x04003F67 RID: 16231
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ISignRenderingDataUpdateListener> deletedSignListenerScratch = new List<ISignRenderingDataUpdateListener>();

	// Token: 0x04003F68 RID: 16232
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSignSyncBatchBytes = 1048576;

	// Token: 0x04003F69 RID: 16233
	[PublicizedFrom(EAccessModifier.Private)]
	public bool signDataDownloadInProgress;

	// Token: 0x04003F6A RID: 16234
	[PublicizedFrom(EAccessModifier.Private)]
	public bool signDataDownloadComplete;

	// Token: 0x04003F6B RID: 16235
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cPlayerSignWriteCooldownSeconds = 60f;

	// Token: 0x04003F6C RID: 16236
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, SignDataManager.PlayerSignLibraryState> playerSignStates = new Dictionary<string, SignDataManager.PlayerSignLibraryState>();

	// Token: 0x04003F6D RID: 16237
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<string> playerSignWriteScratch = new List<string>();

	// Token: 0x04003F6E RID: 16238
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<byte[]> playerSignBlobScratch = new List<byte[]>();

	// Token: 0x02000AAD RID: 2733
	[PublicizedFrom(EAccessModifier.Private)]
	public struct RuntimeRenderingInfo
	{
		// Token: 0x06005214 RID: 21012 RVA: 0x001F8C64 File Offset: 0x001F6E64
		public RuntimeRenderingInfo(int baseDescriptorIndex, int descriptorCount)
		{
			this.BaseDescriptorIndex = baseDescriptorIndex;
			this.DescriptorCount = descriptorCount;
		}

		// Token: 0x04003F6F RID: 16239
		public readonly int BaseDescriptorIndex;

		// Token: 0x04003F70 RID: 16240
		public readonly int DescriptorCount;
	}

	// Token: 0x02000AAE RID: 2734
	[PublicizedFrom(EAccessModifier.Private)]
	public class SignRenderingData : IDisposable
	{
		// Token: 0x06005215 RID: 21013 RVA: 0x001F8C74 File Offset: 0x001F6E74
		public SignRenderingData()
		{
			this.materialPropertyBlock = new MaterialPropertyBlock();
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x001F8CA0 File Offset: 0x001F6EA0
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					MaterialPropertyBlock materialPropertyBlock = this.materialPropertyBlock;
					if (materialPropertyBlock != null)
					{
						materialPropertyBlock.Clear();
					}
					this.materialPropertyBlock = null;
					this.renderingInfoByLayer.Clear();
					this.complexityByLayer.Clear();
				}
				this.DisposeBuffers();
				this.disposedValue = true;
			}
		}

		// Token: 0x06005217 RID: 21015 RVA: 0x001F8CF4 File Offset: 0x001F6EF4
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~SignRenderingData()
		{
			this.Dispose(false);
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x001F8D24 File Offset: 0x001F6F24
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x001F8D34 File Offset: 0x001F6F34
		public void Reset()
		{
			this.layerCount = 0;
			this.totalComplexity = 0f;
			this.totalWarps = 0;
			this.totalDraws = 0;
			this.stackInfo = default(LayerStackInfo);
			MaterialPropertyBlock materialPropertyBlock = this.materialPropertyBlock;
			if (materialPropertyBlock != null)
			{
				materialPropertyBlock.Clear();
			}
			this.renderingInfoByLayer.Clear();
			this.complexityByLayer.Clear();
			this.DisposeBuffers();
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x001F8D9C File Offset: 0x001F6F9C
		public void DisposeBuffers()
		{
			ComputeBuffer computeBuffer = this.descriptorBuffer;
			if (computeBuffer != null)
			{
				computeBuffer.Dispose();
			}
			this.descriptorBuffer = null;
			ComputeBuffer computeBuffer2 = this.circleBuffer;
			if (computeBuffer2 != null)
			{
				computeBuffer2.Dispose();
			}
			this.circleBuffer = null;
			ComputeBuffer computeBuffer3 = this.polygonBuffer;
			if (computeBuffer3 != null)
			{
				computeBuffer3.Dispose();
			}
			this.polygonBuffer = null;
			ComputeBuffer computeBuffer4 = this.charBuffer;
			if (computeBuffer4 != null)
			{
				computeBuffer4.Dispose();
			}
			this.charBuffer = null;
			ComputeBuffer computeBuffer5 = this.noiseBuffer;
			if (computeBuffer5 != null)
			{
				computeBuffer5.Dispose();
			}
			this.noiseBuffer = null;
			ComputeBuffer computeBuffer6 = this.transformBuffer;
			if (computeBuffer6 != null)
			{
				computeBuffer6.Dispose();
			}
			this.transformBuffer = null;
			ComputeBuffer computeBuffer7 = this.skewBuffer;
			if (computeBuffer7 != null)
			{
				computeBuffer7.Dispose();
			}
			this.skewBuffer = null;
			ComputeBuffer computeBuffer8 = this.bulgeBuffer;
			if (computeBuffer8 != null)
			{
				computeBuffer8.Dispose();
			}
			this.bulgeBuffer = null;
			ComputeBuffer computeBuffer9 = this.twirlBuffer;
			if (computeBuffer9 != null)
			{
				computeBuffer9.Dispose();
			}
			this.twirlBuffer = null;
			ComputeBuffer computeBuffer10 = this.kaleidoBuffer;
			if (computeBuffer10 != null)
			{
				computeBuffer10.Dispose();
			}
			this.kaleidoBuffer = null;
			ComputeBuffer computeBuffer11 = this.perspectiveBuffer;
			if (computeBuffer11 != null)
			{
				computeBuffer11.Dispose();
			}
			this.perspectiveBuffer = null;
			ComputeBuffer computeBuffer12 = this.arcBuffer;
			if (computeBuffer12 != null)
			{
				computeBuffer12.Dispose();
			}
			this.arcBuffer = null;
			ComputeBuffer computeBuffer13 = this.stretchBuffer;
			if (computeBuffer13 != null)
			{
				computeBuffer13.Dispose();
			}
			this.stretchBuffer = null;
			ComputeBuffer computeBuffer14 = this.gridBuffer;
			if (computeBuffer14 != null)
			{
				computeBuffer14.Dispose();
			}
			this.gridBuffer = null;
		}

		// Token: 0x04003F71 RID: 16241
		public static ComputeBuffer DummyBuffer = new ComputeBuffer(1, 4);

		// Token: 0x04003F72 RID: 16242
		public MaterialPropertyBlock materialPropertyBlock;

		// Token: 0x04003F73 RID: 16243
		public int layerCount;

		// Token: 0x04003F74 RID: 16244
		public ComputeBuffer descriptorBuffer;

		// Token: 0x04003F75 RID: 16245
		public ComputeBuffer circleBuffer;

		// Token: 0x04003F76 RID: 16246
		public ComputeBuffer polygonBuffer;

		// Token: 0x04003F77 RID: 16247
		public ComputeBuffer charBuffer;

		// Token: 0x04003F78 RID: 16248
		public ComputeBuffer noiseBuffer;

		// Token: 0x04003F79 RID: 16249
		public ComputeBuffer transformBuffer;

		// Token: 0x04003F7A RID: 16250
		public ComputeBuffer skewBuffer;

		// Token: 0x04003F7B RID: 16251
		public ComputeBuffer bulgeBuffer;

		// Token: 0x04003F7C RID: 16252
		public ComputeBuffer twirlBuffer;

		// Token: 0x04003F7D RID: 16253
		public ComputeBuffer kaleidoBuffer;

		// Token: 0x04003F7E RID: 16254
		public ComputeBuffer perspectiveBuffer;

		// Token: 0x04003F7F RID: 16255
		public ComputeBuffer arcBuffer;

		// Token: 0x04003F80 RID: 16256
		public ComputeBuffer stretchBuffer;

		// Token: 0x04003F81 RID: 16257
		public ComputeBuffer gridBuffer;

		// Token: 0x04003F82 RID: 16258
		public readonly Dictionary<SignData.SignLayer, SignDataManager.RuntimeRenderingInfo> renderingInfoByLayer = new Dictionary<SignData.SignLayer, SignDataManager.RuntimeRenderingInfo>();

		// Token: 0x04003F83 RID: 16259
		public readonly Dictionary<SignData.SignLayer, LayerComplexityInfo> complexityByLayer = new Dictionary<SignData.SignLayer, LayerComplexityInfo>();

		// Token: 0x04003F84 RID: 16260
		public float totalComplexity;

		// Token: 0x04003F85 RID: 16261
		public int totalWarps;

		// Token: 0x04003F86 RID: 16262
		public int totalDraws;

		// Token: 0x04003F87 RID: 16263
		public LayerStackInfo stackInfo;

		// Token: 0x04003F88 RID: 16264
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposedValue;
	}

	// Token: 0x02000AAF RID: 2735
	[PublicizedFrom(EAccessModifier.Private)]
	public class UpdateListenerMap
	{
		// Token: 0x0600521C RID: 21020 RVA: 0x001F8F07 File Offset: 0x001F7107
		public UpdateListenerMap(int capacity)
		{
			this._idsByListener = new Dictionary<ISignRenderingDataUpdateListener, HashSet<GlobalSignId>>(capacity);
			this._listenersById = new Dictionary<GlobalSignId, HashSet<ISignRenderingDataUpdateListener>>(capacity);
		}

		// Token: 0x0600521D RID: 21021 RVA: 0x001F8F28 File Offset: 0x001F7128
		public void RegisterListener(GlobalSignId id, ISignRenderingDataUpdateListener listener)
		{
			HashSet<ISignRenderingDataUpdateListener> hashSet;
			if (!this._listenersById.TryGetValue(id, out hashSet))
			{
				hashSet = new HashSet<ISignRenderingDataUpdateListener>(5);
				this._listenersById[id] = hashSet;
			}
			hashSet.Add(listener);
			HashSet<GlobalSignId> hashSet2;
			if (!this._idsByListener.TryGetValue(listener, out hashSet2))
			{
				hashSet2 = new HashSet<GlobalSignId>(1);
				this._idsByListener[listener] = hashSet2;
			}
			hashSet2.Add(id);
		}

		// Token: 0x0600521E RID: 21022 RVA: 0x001F8F90 File Offset: 0x001F7190
		public void DeregisterListener(ISignRenderingDataUpdateListener listener)
		{
			HashSet<GlobalSignId> hashSet;
			if (this._idsByListener.TryGetValue(listener, out hashSet))
			{
				foreach (GlobalSignId globalSignId in hashSet)
				{
					HashSet<ISignRenderingDataUpdateListener> hashSet2;
					if (this._listenersById.TryGetValue(globalSignId, out hashSet2))
					{
						hashSet2.Remove(listener);
						if (hashSet2.Count == 0)
						{
							this._listenersById.Remove(globalSignId);
						}
					}
				}
				this._idsByListener.Remove(listener);
			}
		}

		// Token: 0x0600521F RID: 21023 RVA: 0x001F9024 File Offset: 0x001F7224
		public IEnumerable<ISignRenderingDataUpdateListener> GetListeners(GlobalSignId id)
		{
			HashSet<ISignRenderingDataUpdateListener> hashSet;
			if (!this._listenersById.TryGetValue(id, out hashSet))
			{
				return Array.Empty<ISignRenderingDataUpdateListener>();
			}
			return hashSet;
		}

		// Token: 0x06005220 RID: 21024 RVA: 0x001F904C File Offset: 0x001F724C
		public IEnumerable<GlobalSignId> GetIds(ISignRenderingDataUpdateListener listener)
		{
			HashSet<GlobalSignId> hashSet;
			if (!this._idsByListener.TryGetValue(listener, out hashSet))
			{
				return Array.Empty<GlobalSignId>();
			}
			return hashSet;
		}

		// Token: 0x06005221 RID: 21025 RVA: 0x001F9074 File Offset: 0x001F7274
		public void Clear()
		{
			this._listenersById.Clear();
			this._idsByListener.Clear();
		}

		// Token: 0x06005222 RID: 21026 RVA: 0x001F908C File Offset: 0x001F728C
		public bool HasListeners(GlobalSignId signId)
		{
			return this._listenersById.ContainsKey(signId);
		}

		// Token: 0x04003F89 RID: 16265
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<GlobalSignId, HashSet<ISignRenderingDataUpdateListener>> _listenersById;

		// Token: 0x04003F8A RID: 16266
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<ISignRenderingDataUpdateListener, HashSet<GlobalSignId>> _idsByListener;
	}

	// Token: 0x02000AB0 RID: 2736
	[PublicizedFrom(EAccessModifier.Private)]
	public enum LayerTypeID
	{
		// Token: 0x04003F8C RID: 16268
		UvClear,
		// Token: 0x04003F8D RID: 16269
		UvPop,
		// Token: 0x04003F8E RID: 16270
		UvPush,
		// Token: 0x04003F8F RID: 16271
		MaskBegin,
		// Token: 0x04003F90 RID: 16272
		MaskFlipPhase,
		// Token: 0x04003F91 RID: 16273
		MaskEnd,
		// Token: 0x04003F92 RID: 16274
		WarpTransform = 10,
		// Token: 0x04003F93 RID: 16275
		WarpSkew,
		// Token: 0x04003F94 RID: 16276
		WarpBulge,
		// Token: 0x04003F95 RID: 16277
		WarpTwirl,
		// Token: 0x04003F96 RID: 16278
		WarpKaleido,
		// Token: 0x04003F97 RID: 16279
		WarpPerspective,
		// Token: 0x04003F98 RID: 16280
		WarpArc,
		// Token: 0x04003F99 RID: 16281
		WarpStretch,
		// Token: 0x04003F9A RID: 16282
		WarpGrid,
		// Token: 0x04003F9B RID: 16283
		Polygon = 100,
		// Token: 0x04003F9C RID: 16284
		Char,
		// Token: 0x04003F9D RID: 16285
		Circle,
		// Token: 0x04003F9E RID: 16286
		Noise
	}

	// Token: 0x02000AB1 RID: 2737
	[PublicizedFrom(EAccessModifier.Private)]
	public struct LayerDescriptor
	{
		// Token: 0x04003F9F RID: 16287
		public int type;

		// Token: 0x04003FA0 RID: 16288
		public int index;
	}

	// Token: 0x02000AB2 RID: 2738
	[PublicizedFrom(EAccessModifier.Private)]
	public struct CircleLayer
	{
		// Token: 0x04003FA1 RID: 16289
		public Vector4 color;

		// Token: 0x04003FA2 RID: 16290
		public float softness;

		// Token: 0x04003FA3 RID: 16291
		public float dilate;

		// Token: 0x04003FA4 RID: 16292
		public float frequency;

		// Token: 0x04003FA5 RID: 16293
		public int mode;
	}

	// Token: 0x02000AB3 RID: 2739
	[PublicizedFrom(EAccessModifier.Private)]
	public struct PolygonLayer
	{
		// Token: 0x04003FA6 RID: 16294
		public Vector4 color;

		// Token: 0x04003FA7 RID: 16295
		public float sides;

		// Token: 0x04003FA8 RID: 16296
		public float smoothness;

		// Token: 0x04003FA9 RID: 16297
		public float starify;

		// Token: 0x04003FAA RID: 16298
		public float softness;

		// Token: 0x04003FAB RID: 16299
		public float dilate;

		// Token: 0x04003FAC RID: 16300
		public float frequency;

		// Token: 0x04003FAD RID: 16301
		public int mode;
	}

	// Token: 0x02000AB4 RID: 2740
	[PublicizedFrom(EAccessModifier.Private)]
	public struct CharLayer
	{
		// Token: 0x04003FAE RID: 16302
		public Vector2 position;

		// Token: 0x04003FAF RID: 16303
		public Vector2 scale;

		// Token: 0x04003FB0 RID: 16304
		public Vector4 color;

		// Token: 0x04003FB1 RID: 16305
		public int atlasIndex;

		// Token: 0x04003FB2 RID: 16306
		public Vector4 offsetSize;

		// Token: 0x04003FB3 RID: 16307
		public float softness;

		// Token: 0x04003FB4 RID: 16308
		public float dilate;
	}

	// Token: 0x02000AB5 RID: 2741
	[PublicizedFrom(EAccessModifier.Private)]
	public struct NoiseLayer
	{
		// Token: 0x04003FB5 RID: 16309
		public Vector4 color;

		// Token: 0x04003FB6 RID: 16310
		public float offset;

		// Token: 0x04003FB7 RID: 16311
		public int detail;

		// Token: 0x04003FB8 RID: 16312
		public float softness;

		// Token: 0x04003FB9 RID: 16313
		public float dilate;

		// Token: 0x04003FBA RID: 16314
		public float fade;
	}

	// Token: 0x02000AB6 RID: 2742
	[PublicizedFrom(EAccessModifier.Private)]
	public struct TransformWarp
	{
		// Token: 0x04003FBB RID: 16315
		public Vector2 position;

		// Token: 0x04003FBC RID: 16316
		public float rotation;

		// Token: 0x04003FBD RID: 16317
		public Vector2 scale;
	}

	// Token: 0x02000AB7 RID: 2743
	[PublicizedFrom(EAccessModifier.Private)]
	public struct SkewWarp
	{
		// Token: 0x04003FBE RID: 16318
		public float rotation;

		// Token: 0x04003FBF RID: 16319
		public Vector2 amount;
	}

	// Token: 0x02000AB8 RID: 2744
	[PublicizedFrom(EAccessModifier.Private)]
	public struct BulgeWarp
	{
		// Token: 0x04003FC0 RID: 16320
		public Vector2 position;

		// Token: 0x04003FC1 RID: 16321
		public float amount;
	}

	// Token: 0x02000AB9 RID: 2745
	[PublicizedFrom(EAccessModifier.Private)]
	public struct TwirlWarp
	{
		// Token: 0x04003FC2 RID: 16322
		public Vector2 position;

		// Token: 0x04003FC3 RID: 16323
		public float amount;

		// Token: 0x04003FC4 RID: 16324
		public float frequency;
	}

	// Token: 0x02000ABA RID: 2746
	[PublicizedFrom(EAccessModifier.Private)]
	public struct KaleidoWarp
	{
		// Token: 0x04003FC5 RID: 16325
		public Vector2 position;

		// Token: 0x04003FC6 RID: 16326
		public float angle;

		// Token: 0x04003FC7 RID: 16327
		public float offset;
	}

	// Token: 0x02000ABB RID: 2747
	[PublicizedFrom(EAccessModifier.Private)]
	public struct PerspectiveWarp
	{
		// Token: 0x04003FC8 RID: 16328
		public Vector3 rotation;

		// Token: 0x04003FC9 RID: 16329
		public float strength;
	}

	// Token: 0x02000ABC RID: 2748
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ArcWarp
	{
		// Token: 0x04003FCA RID: 16330
		public float rotation;

		// Token: 0x04003FCB RID: 16331
		public float radius;

		// Token: 0x04003FCC RID: 16332
		public float width;
	}

	// Token: 0x02000ABD RID: 2749
	[PublicizedFrom(EAccessModifier.Private)]
	public struct StretchWarp
	{
		// Token: 0x04003FCD RID: 16333
		public Vector2 position;

		// Token: 0x04003FCE RID: 16334
		public float rotation;

		// Token: 0x04003FCF RID: 16335
		public float distance;

		// Token: 0x04003FD0 RID: 16336
		public float width;

		// Token: 0x04003FD1 RID: 16337
		public float exponent;
	}

	// Token: 0x02000ABE RID: 2750
	[PublicizedFrom(EAccessModifier.Private)]
	public struct GridWarp
	{
		// Token: 0x04003FD2 RID: 16338
		public Vector2 offset;

		// Token: 0x04003FD3 RID: 16339
		public float rotation;

		// Token: 0x04003FD4 RID: 16340
		public float scale;

		// Token: 0x04003FD5 RID: 16341
		public int mode;

		// Token: 0x04003FD6 RID: 16342
		public float aspect;

		// Token: 0x04003FD7 RID: 16343
		public float shift;
	}

	// Token: 0x02000ABF RID: 2751
	[PublicizedFrom(EAccessModifier.Private)]
	public class CharAtlasData
	{
		// Token: 0x06005223 RID: 21027 RVA: 0x001F909A File Offset: 0x001F729A
		public CharAtlasData(Vector2 quadOffset, Vector2 quadSize, Vector4 uvOffsetSize, float charWidth)
		{
			this.quadOffset = quadOffset;
			this.quadSize = quadSize;
			this.uvOffsetSize = uvOffsetSize;
			this.charWidth = charWidth;
		}

		// Token: 0x04003FD8 RID: 16344
		public Vector2 quadOffset;

		// Token: 0x04003FD9 RID: 16345
		public Vector2 quadSize;

		// Token: 0x04003FDA RID: 16346
		public Vector4 uvOffsetSize;

		// Token: 0x04003FDB RID: 16347
		public float charWidth;
	}

	// Token: 0x02000AC0 RID: 2752
	[PublicizedFrom(EAccessModifier.Private)]
	public class SignFontData
	{
		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06005224 RID: 21028 RVA: 0x001F90BF File Offset: 0x001F72BF
		// (set) Token: 0x06005225 RID: 21029 RVA: 0x001F90C7 File Offset: 0x001F72C7
		public int AtlasIndex
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06005226 RID: 21030 RVA: 0x001F90D0 File Offset: 0x001F72D0
		// (set) Token: 0x06005227 RID: 21031 RVA: 0x001F90D8 File Offset: 0x001F72D8
		public Texture2D AtlasTexture
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06005228 RID: 21032 RVA: 0x001F90E1 File Offset: 0x001F72E1
		// (set) Token: 0x06005229 RID: 21033 RVA: 0x001F90E9 File Offset: 0x001F72E9
		public float LineHeight
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x001F90F4 File Offset: 0x001F72F4
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConstructAtlasDataMap(TMP_FontAsset fontAsset)
		{
			float num = (float)fontAsset.atlasPadding;
			float num2 = 1f / (float)fontAsset.atlasWidth;
			float num3 = 1f / (float)fontAsset.atlasHeight;
			float num4 = (float)(fontAsset.atlasWidth / fontAsset.atlasHeight);
			float num5 = 1f / (num3 * (float)fontAsset.creationSettings.pointSize);
			this.LineHeight = fontAsset.faceInfo.lineHeight * num4 * num2 * num5;
			foreach (KeyValuePair<uint, TMP_Character> keyValuePair in fontAsset.characterLookupTable)
			{
				GlyphRect glyphRect = keyValuePair.Value.glyph.glyphRect;
				Vector4 vector = default(Vector4);
				vector.x = num2 * ((float)glyphRect.x - num);
				vector.y = num3 * ((float)glyphRect.y - num);
				vector.z = num2 * ((float)glyphRect.width + 2f * num);
				vector.w = num3 * ((float)glyphRect.height + 2f * num);
				float num6 = keyValuePair.Value.glyph.metrics.horizontalAdvance * num4 * num2 * num5;
				Vector2 vector2 = new Vector2(0.5f * num6, (-0.5f * ((float)glyphRect.height + fontAsset.faceInfo.capLine) + keyValuePair.Value.glyph.metrics.horizontalBearingY) * num3 * num5);
				Vector2 vector3 = new Vector2(vector.z * num4, vector.w) * num5;
				SignDataManager.CharAtlasData charAtlasData = new SignDataManager.CharAtlasData(vector2, vector3, vector, num6);
				this.charToAtlasDataMap[keyValuePair.Key] = charAtlasData;
			}
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x001F92DC File Offset: 0x001F74DC
		public SignFontData(int atlasIndex, TMP_FontAsset fontAsset)
		{
			this.AtlasIndex = atlasIndex;
			this.AtlasTexture = fontAsset.atlasTexture;
			this.ConstructAtlasDataMap(fontAsset);
		}

		// Token: 0x0600522C RID: 21036 RVA: 0x001F9309 File Offset: 0x001F7509
		public bool TryGetCharAtlasData(char character, out SignDataManager.CharAtlasData atlasData)
		{
			return this.charToAtlasDataMap.TryGetValue((uint)character, out atlasData);
		}

		// Token: 0x04003FDF RID: 16351
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<uint, SignDataManager.CharAtlasData> charToAtlasDataMap = new Dictionary<uint, SignDataManager.CharAtlasData>();
	}

	// Token: 0x02000AC1 RID: 2753
	public struct ColorOperation
	{
		// Token: 0x0600522D RID: 21037 RVA: 0x001F9318 File Offset: 0x001F7518
		public static Color EvaluateColor(Color leafColor, List<SignDataManager.ColorOperation> colorOps)
		{
			Color color = leafColor;
			for (int i = colorOps.Count - 1; i >= 0; i--)
			{
				SignDataManager.ColorOperation colorOperation = colorOps[i];
				switch (colorOperation.mode)
				{
				case SignData.GroupSignLayer.ColorMode.Multiply:
					color *= colorOperation.color;
					break;
				case SignData.GroupSignLayer.ColorMode.Blend:
				{
					float a = colorOperation.color.a;
					color = new Color(Mathf.Lerp(color.r, colorOperation.color.r, a), Mathf.Lerp(color.g, colorOperation.color.g, a), Mathf.Lerp(color.b, colorOperation.color.b, a), color.a);
					break;
				}
				case SignData.GroupSignLayer.ColorMode.Override:
					color = colorOperation.color;
					break;
				}
			}
			return color;
		}

		// Token: 0x04003FE0 RID: 16352
		public Color color;

		// Token: 0x04003FE1 RID: 16353
		public SignData.GroupSignLayer.ColorMode mode;
	}

	// Token: 0x02000AC2 RID: 2754
	public struct GroupOffsets
	{
		// Token: 0x0600522E RID: 21038 RVA: 0x001F93DC File Offset: 0x001F75DC
		public SignDataManager.GroupOffsets WithOffsets(float softness, float dilate, SignData.GroupSignLayer.OffsetTarget target)
		{
			SignDataManager.GroupOffsets groupOffsets = this;
			switch (target)
			{
			case SignData.GroupSignLayer.OffsetTarget.All:
				groupOffsets.shapeSoftnessOffset += softness;
				groupOffsets.shapeDilateOffset += dilate;
				groupOffsets.textSoftnessOffset += softness;
				groupOffsets.textDilateOffset += dilate;
				groupOffsets.noiseSoftnessOffset += softness;
				groupOffsets.noiseDilateOffset += dilate;
				break;
			case SignData.GroupSignLayer.OffsetTarget.Shapes:
				groupOffsets.shapeSoftnessOffset += softness;
				groupOffsets.shapeDilateOffset += dilate;
				break;
			case SignData.GroupSignLayer.OffsetTarget.Text:
				groupOffsets.textSoftnessOffset += softness;
				groupOffsets.textDilateOffset += dilate;
				break;
			case SignData.GroupSignLayer.OffsetTarget.Noise:
				groupOffsets.noiseSoftnessOffset += softness;
				groupOffsets.noiseDilateOffset += dilate;
				break;
			}
			return groupOffsets;
		}

		// Token: 0x04003FE2 RID: 16354
		public float shapeSoftnessOffset;

		// Token: 0x04003FE3 RID: 16355
		public float shapeDilateOffset;

		// Token: 0x04003FE4 RID: 16356
		public float textSoftnessOffset;

		// Token: 0x04003FE5 RID: 16357
		public float textDilateOffset;

		// Token: 0x04003FE6 RID: 16358
		public float noiseSoftnessOffset;

		// Token: 0x04003FE7 RID: 16359
		public float noiseDilateOffset;
	}

	// Token: 0x02000AC3 RID: 2755
	[PublicizedFrom(EAccessModifier.Private)]
	public interface IIntermediateDataWrapper
	{
		// Token: 0x0600522F RID: 21039
		void UpdateBuffer(SignDataManager.SignRenderingData signRenderingData);

		// Token: 0x06005230 RID: 21040
		void ApplyToMaterial(SignDataManager.SignRenderingData signRenderingData, Material material);

		// Token: 0x06005231 RID: 21041
		void ClearStagingList();
	}

	// Token: 0x02000AC4 RID: 2756
	[PublicizedFrom(EAccessModifier.Private)]
	public class IntermediateDataWrapper<T> : SignDataManager.IIntermediateDataWrapper where T : struct
	{
		// Token: 0x06005232 RID: 21042 RVA: 0x001F94A2 File Offset: 0x001F76A2
		public IntermediateDataWrapper(int shaderPropertyId, int capacity, Func<SignDataManager.SignRenderingData, ComputeBuffer> getter, Action<SignDataManager.SignRenderingData, ComputeBuffer> setter)
		{
			this._shaderPropertyId = shaderPropertyId;
			this.List = new List<T>(capacity);
			this._get = getter;
			this._set = setter;
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x001F94CC File Offset: 0x001F76CC
		public void UpdateBuffer(SignDataManager.SignRenderingData signRenderingData)
		{
			ComputeBuffer computeBuffer = this._get(signRenderingData);
			if (this.List.Count > 0)
			{
				int num = SignDataManager.IntermediateDataWrapper<T>.NextPow2(this.List.Count);
				if (computeBuffer == null || computeBuffer.count != num)
				{
					if (computeBuffer != null)
					{
						computeBuffer.Dispose();
					}
					computeBuffer = new ComputeBuffer(num, SignDataManager.IntermediateDataWrapper<T>._stride)
					{
						name = string.Format("{0}:{1}", typeof(T).Name, num)
					};
					this._set(signRenderingData, computeBuffer);
				}
				computeBuffer.SetData<T>(this.List, 0, 0, this.List.Count);
			}
			else if (computeBuffer != null)
			{
				computeBuffer.Dispose();
				computeBuffer = null;
				this._set(signRenderingData, null);
			}
			signRenderingData.materialPropertyBlock.SetBuffer(this._shaderPropertyId, computeBuffer ?? SignDataManager.SignRenderingData.DummyBuffer);
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x001F95A6 File Offset: 0x001F77A6
		public void ApplyToMaterial(SignDataManager.SignRenderingData signRenderingData, Material material)
		{
			material.SetBuffer(this._shaderPropertyId, this._get(signRenderingData) ?? SignDataManager.SignRenderingData.DummyBuffer);
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x001F95C9 File Offset: 0x001F77C9
		public void ClearStagingList()
		{
			this.List.Clear();
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x001F95D6 File Offset: 0x001F77D6
		[PublicizedFrom(EAccessModifier.Private)]
		public static int NextPow2(int v)
		{
			if (v <= 4)
			{
				return 4;
			}
			v--;
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;
			return v + 1;
		}

		// Token: 0x04003FE8 RID: 16360
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly int _stride = Marshal.SizeOf(typeof(T));

		// Token: 0x04003FE9 RID: 16361
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int _shaderPropertyId;

		// Token: 0x04003FEA RID: 16362
		public readonly List<T> List;

		// Token: 0x04003FEB RID: 16363
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<SignDataManager.SignRenderingData, ComputeBuffer> _get;

		// Token: 0x04003FEC RID: 16364
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Action<SignDataManager.SignRenderingData, ComputeBuffer> _set;
	}

	// Token: 0x02000AC5 RID: 2757
	// (Invoke) Token: 0x06005239 RID: 21049
	public delegate void RenderingDataPatcher(MaterialPropertyBlock mpb);

	// Token: 0x02000AC6 RID: 2758
	[PublicizedFrom(EAccessModifier.Private)]
	public class PlayerSignLibraryState
	{
		// Token: 0x04003FED RID: 16365
		public readonly Dictionary<Guid, byte[]> blobsByGuid = new Dictionary<Guid, byte[]>();

		// Token: 0x04003FEE RID: 16366
		public bool dirty;

		// Token: 0x04003FEF RID: 16367
		public float lastWriteTime = float.NegativeInfinity;

		// Token: 0x04003FF0 RID: 16368
		public ThreadManager.TaskInfo writeTask;

		// Token: 0x04003FF1 RID: 16369
		public bool writesBlocked;
	}

	// Token: 0x02000AC7 RID: 2759
	[PublicizedFrom(EAccessModifier.Private)]
	public class PlayerSignWriteRequest
	{
		// Token: 0x04003FF2 RID: 16370
		public string libraryId;

		// Token: 0x04003FF3 RID: 16371
		public PooledExpandableMemoryStream image;
	}
}
