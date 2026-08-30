using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

// Token: 0x02000A82 RID: 2690
public class SignDataManager
{
	// Token: 0x170008CC RID: 2252
	// (get) Token: 0x06005131 RID: 20785 RVA: 0x001EF993 File Offset: 0x001EDB93
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

	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x06005132 RID: 20786 RVA: 0x001EF9AB File Offset: 0x001EDBAB
	public static bool HasInstance
	{
		get
		{
			return SignDataManager.m_Instance != null;
		}
	}

	// Token: 0x170008CE RID: 2254
	// (get) Token: 0x06005133 RID: 20787 RVA: 0x001EF9B5 File Offset: 0x001EDBB5
	public IEnumerable<string> FontNames
	{
		get
		{
			return this.fontDataByName.Keys;
		}
	}

	// Token: 0x06005134 RID: 20788 RVA: 0x001EF9C4 File Offset: 0x001EDBC4
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
					goto IL_063C;
				}
			}
			Log.Error("Texture arrays are not supported on this platform/GPU; sign fonts will not be rendered correctly.");
		}
		IL_063C:
		this.intermediateDataWrappers = new SignDataManager.IIntermediateDataWrapper[]
		{
			this.layerDescriptors, this.circleLayers, this.polygonLayers, this.charLayers, this.noiseLayers, this.transformWarps, this.skewWarps, this.bulgeWarps, this.twirlWarps, this.kaleidoWarps,
			this.perspectiveWarps, this.arcWarps, this.stretchWarps, this.gridWarps
		};
	}

	// Token: 0x06005135 RID: 20789 RVA: 0x001F00BC File Offset: 0x001EE2BC
	public static string GetLibraryNiceName(GlobalSignId signId)
	{
		if (!signId.IsValid)
		{
			return "-";
		}
		string libraryId = signId.libraryId;
		string text;
		if (!(libraryId == "[D]"))
		{
			if (!(libraryId == "[U]"))
			{
				text = signId.libraryId;
			}
			else
			{
				text = Localization.Get("lblSignLibraryUser", false, null);
			}
		}
		else
		{
			text = Localization.Get("lblSignLibraryDefault", false, null);
		}
		return text;
	}

	// Token: 0x170008CF RID: 2255
	// (get) Token: 0x06005136 RID: 20790 RVA: 0x001F0121 File Offset: 0x001EE321
	// (set) Token: 0x06005137 RID: 20791 RVA: 0x001F0128 File Offset: 0x001EE328
	public static GlobalSignId ErrorSignId
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06005138 RID: 20792 RVA: 0x001F0130 File Offset: 0x001EE330
	public static IEnumerator LoadDefaultLibrary(XmlFile file)
	{
		SignLibrary signLibrary = new SignLibrary();
		signLibrary.ReadXml(file);
		SignDataManager.Instance.TryRegisterLibrary("[D]", signLibrary, false);
		SignDataManager.ErrorSignId = SignDataManager.Instance.AddSignToLibrary("[I]", SignData.GetErrorSignData());
		yield break;
	}

	// Token: 0x06005139 RID: 20793 RVA: 0x001F0140 File Offset: 0x001EE340
	public void Cleanup()
	{
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

	// Token: 0x0600513A RID: 20794 RVA: 0x001F01C0 File Offset: 0x001EE3C0
	public void RegisterListener(GlobalSignId id, ISignRenderingDataUpdateListener listener, bool exclusive = true)
	{
		if (exclusive)
		{
			this.updateListenerMap.DeregisterListener(listener);
		}
		this.updateListenerMap.RegisterListener(id, listener);
	}

	// Token: 0x0600513B RID: 20795 RVA: 0x001F01DE File Offset: 0x001EE3DE
	public void DeregisterListener(ISignRenderingDataUpdateListener listener)
	{
		this.updateListenerMap.DeregisterListener(listener);
	}

	// Token: 0x0600513C RID: 20796 RVA: 0x001F01EC File Offset: 0x001EE3EC
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

	// Token: 0x0600513D RID: 20797 RVA: 0x001F0260 File Offset: 0x001EE460
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

	// Token: 0x0600513E RID: 20798 RVA: 0x001F02D4 File Offset: 0x001EE4D4
	public bool MoveSignToLibrary(GlobalSignId signId, string libraryName, out GlobalSignId newId)
	{
		SignData signData;
		if (this.TryGetSignData(signId, out signData))
		{
			SignData signData2 = SignData.Duplicate(signData, "");
			newId = this.AddSignToLibrary(libraryName, signData2);
			return true;
		}
		newId = GlobalSignId.InvalidId;
		return false;
	}

	// Token: 0x0600513F RID: 20799 RVA: 0x001F0314 File Offset: 0x001EE514
	public GlobalSignId AddSignToLibrary(string libraryId, SignData sign)
	{
		SignLibrary signLibrary;
		if (!this.signLibraries.TryGetValue(libraryId, out signLibrary))
		{
			signLibrary = new SignLibrary();
			this.signLibraries[libraryId] = signLibrary;
		}
		signLibrary.signs[sign.guid] = sign;
		return new GlobalSignId(libraryId, sign.guid);
	}

	// Token: 0x06005140 RID: 20800 RVA: 0x001F0364 File Offset: 0x001EE564
	public bool TryDeleteSign(GlobalSignId signId)
	{
		this.TryClearRenderingData(signId);
		SignLibrary signLibrary;
		return this.signLibraries.TryGetValue(signId.libraryId, out signLibrary) && signLibrary.signs.Remove(signId.signGuid);
	}

	// Token: 0x06005141 RID: 20801 RVA: 0x001F03A4 File Offset: 0x001EE5A4
	public bool MarkSignDirty(GlobalSignId signId)
	{
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

	// Token: 0x06005142 RID: 20802 RVA: 0x001F0420 File Offset: 0x001EE620
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

	// Token: 0x06005143 RID: 20803 RVA: 0x001F0454 File Offset: 0x001EE654
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

	// Token: 0x06005144 RID: 20804 RVA: 0x001F04F8 File Offset: 0x001EE6F8
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

	// Token: 0x06005145 RID: 20805 RVA: 0x001F0508 File Offset: 0x001EE708
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

	// Token: 0x06005146 RID: 20806 RVA: 0x001F06E0 File Offset: 0x001EE8E0
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
							SignData signData = SignData.Read(pooledBinaryReader);
							this.AddSignToLibrary(text, signData);
						}
					}
				}
			}
		}
		catch
		{
			Log.Error("[SignDataManager] Failed to apply sign data batch from server");
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

	// Token: 0x06005147 RID: 20807 RVA: 0x001F07E8 File Offset: 0x001EE9E8
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerable<KeyValuePair<string, SignLibrary>> EnumeratePublicSignLibraries()
	{
		foreach (KeyValuePair<string, SignLibrary> keyValuePair in this.signLibraries)
		{
			if (!keyValuePair.Key.Equals("[I]") || !keyValuePair.Key.Equals("[U]"))
			{
				yield return keyValuePair;
			}
		}
		Dictionary<string, SignLibrary>.Enumerator enumerator = default(Dictionary<string, SignLibrary>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x06005148 RID: 20808 RVA: 0x001F07F8 File Offset: 0x001EE9F8
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

	// Token: 0x06005149 RID: 20809 RVA: 0x001F087C File Offset: 0x001EEA7C
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

	// Token: 0x0600514A RID: 20810 RVA: 0x001F0960 File Offset: 0x001EEB60
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

	// Token: 0x0600514B RID: 20811 RVA: 0x001F0A6C File Offset: 0x001EEC6C
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

	// Token: 0x0600514C RID: 20812 RVA: 0x001F0AB0 File Offset: 0x001EECB0
	[PublicizedFrom(EAccessModifier.Private)]
	public SignDataManager.SignRenderingData UpdateRenderingData(SignData signData, GlobalSignId id)
	{
		SignDataManager.<>c__DisplayClass91_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.signData = signData;
		CS$<>8__locals1.id = id;
		this.<UpdateRenderingData>g__ClearIntermediateLists|91_0(ref CS$<>8__locals1);
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
		CS$<>8__locals1.signRenderingData.stackInfo = this.<UpdateRenderingData>g__ProcessLayers|91_1(CS$<>8__locals1.signData.layers, SignData.SignTransform.Identity, this.colorOpStack, default(SignDataManager.GroupOffsets), ref CS$<>8__locals1);
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
		this.<UpdateRenderingData>g__ClearIntermediateLists|91_0(ref CS$<>8__locals1);
		return CS$<>8__locals1.signRenderingData;
	}

	// Token: 0x0600514D RID: 20813 RVA: 0x001F0C14 File Offset: 0x001EEE14
	public void AddFilteredSignIdsToList(string libraryId, string nameFilter, List<GlobalSignId> signIds)
	{
		SignLibrary signLibrary;
		if (!this.signLibraries.TryGetValue(libraryId, out signLibrary))
		{
			Log.Error("Failed to retrieve sign IDs. Library '" + libraryId + "' not found.");
			return;
		}
		foreach (SignData signData in signLibrary.signs.Values)
		{
			if (string.IsNullOrEmpty(nameFilter) || signData.name.ContainsCaseInsensitive(nameFilter))
			{
				GlobalSignId globalSignId = new GlobalSignId(libraryId, signData.guid);
				signIds.Add(globalSignId);
			}
		}
	}

	// Token: 0x0600514E RID: 20814 RVA: 0x001F0CB8 File Offset: 0x001EEEB8
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

	// Token: 0x0600514F RID: 20815 RVA: 0x001F0CF1 File Offset: 0x001EEEF1
	public bool HasSignLibrary(string libraryId)
	{
		return this.signLibraries.ContainsKey(libraryId);
	}

	// Token: 0x06005150 RID: 20816 RVA: 0x001F0CFF File Offset: 0x001EEEFF
	public bool TryGetSignLibrary(string libraryId, out SignLibrary library)
	{
		return this.signLibraries.TryGetValue(libraryId, out library);
	}

	// Token: 0x06005151 RID: 20817 RVA: 0x001F0D10 File Offset: 0x001EEF10
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__ClearIntermediateLists|91_0(ref SignDataManager.<>c__DisplayClass91_0 A_1)
	{
		this.colorOpStack.Clear();
		SignDataManager.IIntermediateDataWrapper[] array = this.intermediateDataWrappers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ClearStagingList();
		}
	}

	// Token: 0x06005152 RID: 20818 RVA: 0x001F0D48 File Offset: 0x001EEF48
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public LayerStackInfo <UpdateRenderingData>g__ProcessLayers|91_1(List<SignData.SignLayer> layers, SignData.SignTransform parentTransform, List<SignDataManager.ColorOperation> colorOps, SignDataManager.GroupOffsets parentOffsets = default(SignDataManager.GroupOffsets), ref SignDataManager.<>c__DisplayClass91_0 A_5)
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
								this.<UpdateRenderingData>g__PushWarps|91_5(parentTransform, signLayer, ref A_5);
								layerComplexityInfo.MaxUVStackIndex = A_5.uvStackIndex;
								this.<UpdateRenderingData>g__ProcessNoiseLayer|91_3(noiseSignLayer, colorOps, parentOffsets, ref A_5);
								this.<UpdateRenderingData>g__PopWarps|91_6(ref A_5);
							}
							else
							{
								this.<UpdateRenderingData>g__ProcessNoiseLayer|91_3(noiseSignLayer, colorOps, parentOffsets, ref A_5);
							}
						}
					}
					else if (signLayer.HasTransformOrWarps || parentTransform != SignData.SignTransform.Identity)
					{
						this.<UpdateRenderingData>g__PushWarps|91_5(parentTransform, signLayer, ref A_5);
						layerComplexityInfo.MaxUVStackIndex = A_5.uvStackIndex;
						this.<UpdateRenderingData>g__ProcessPolygonLayer|91_2(polygonSignLayer, colorOps, parentOffsets, ref A_5);
						this.<UpdateRenderingData>g__PopWarps|91_6(ref A_5);
					}
					else
					{
						this.<UpdateRenderingData>g__ProcessPolygonLayer|91_2(polygonSignLayer, colorOps, parentOffsets, ref A_5);
					}
				}
				else if (signLayer.HasTransformOrWarps || parentTransform != SignData.SignTransform.Identity)
				{
					this.<UpdateRenderingData>g__PushWarps|91_5(parentTransform, signLayer, ref A_5);
					layerComplexityInfo.MaxUVStackIndex = A_5.uvStackIndex;
					this.<UpdateRenderingData>g__ProcessTextLayer|91_4(textSignLayer, colorOps, parentOffsets, ref A_5);
					this.<UpdateRenderingData>g__PopWarps|91_6(ref A_5);
				}
				else
				{
					this.<UpdateRenderingData>g__ProcessTextLayer|91_4(textSignLayer, colorOps, parentOffsets, ref A_5);
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
					this.<UpdateRenderingData>g__PushWarps|91_5(parentTransform, signLayer, ref A_5);
					LayerStackInfo layerStackInfo2 = this.<UpdateRenderingData>g__ProcessLayers|91_1(groupSignLayer.layers, SignData.SignTransform.Identity, colorOps, groupOffsets, ref A_5);
					layerComplexityInfo.MaxCompStackIndex = layerStackInfo2.MaxCompStackIndex;
					layerComplexityInfo.MaxUVStackIndex = layerStackInfo2.MaxUVStackIndex;
					layerComplexityInfo.GroupDepth = layerStackInfo2.GroupDepth + 1;
					this.<UpdateRenderingData>g__PopWarps|91_6(ref A_5);
				}
				else
				{
					LayerStackInfo layerStackInfo3 = this.<UpdateRenderingData>g__ProcessLayers|91_1(groupSignLayer.layers, parentTransform * groupSignLayer.transform, colorOps, groupOffsets, ref A_5);
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

	// Token: 0x06005153 RID: 20819 RVA: 0x001F1374 File Offset: 0x001EF574
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__PushWarps|91_5(SignData.SignTransform parentTransform, SignData.SignLayer layer, ref SignDataManager.<>c__DisplayClass91_0 A_3)
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

	// Token: 0x06005154 RID: 20820 RVA: 0x001F1B08 File Offset: 0x001EFD08
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__PopWarps|91_6(ref SignDataManager.<>c__DisplayClass91_0 A_1)
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

	// Token: 0x06005155 RID: 20821 RVA: 0x001F1BC4 File Offset: 0x001EFDC4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__ProcessPolygonLayer|91_2(SignData.PolygonSignLayer polygonLayer, List<SignDataManager.ColorOperation> colorOps, SignDataManager.GroupOffsets offsets, ref SignDataManager.<>c__DisplayClass91_0 A_4)
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

	// Token: 0x06005156 RID: 20822 RVA: 0x001F1DC4 File Offset: 0x001EFFC4
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__ProcessNoiseLayer|91_3(SignData.NoiseSignLayer noiseLayer, List<SignDataManager.ColorOperation> colorOps, SignDataManager.GroupOffsets offsets, ref SignDataManager.<>c__DisplayClass91_0 A_4)
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

	// Token: 0x06005157 RID: 20823 RVA: 0x001F1EE8 File Offset: 0x001F00E8
	[CompilerGenerated]
	[PublicizedFrom(EAccessModifier.Private)]
	public void <UpdateRenderingData>g__ProcessTextLayer|91_4(SignData.TextSignLayer textLayer, List<SignDataManager.ColorOperation> colorOps, SignDataManager.GroupOffsets offsets, ref SignDataManager.<>c__DisplayClass91_0 A_4)
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

	// Token: 0x04003E66 RID: 15974
	[PublicizedFrom(EAccessModifier.Private)]
	public const string fontConfigLoc = "@:Fonts/SignFontConfig.asset";

	// Token: 0x04003E67 RID: 15975
	[PublicizedFrom(EAccessModifier.Private)]
	public const float baseDescriptorComplexity = 0.5f;

	// Token: 0x04003E68 RID: 15976
	public const string defaultFontName = "LiberationSans";

	// Token: 0x04003E69 RID: 15977
	public const string defaultLibraryId = "[D]";

	// Token: 0x04003E6A RID: 15978
	public const string internalLibraryId = "[I]";

	// Token: 0x04003E6B RID: 15979
	public const string userLibraryId = "[U]";

	// Token: 0x04003E6C RID: 15980
	public const float cMaxComplexity = 600f;

	// Token: 0x04003E6D RID: 15981
	public const int cMaxCompStackIndex = 7;

	// Token: 0x04003E6E RID: 15982
	public const int cMaxUVStackIndex = 7;

	// Token: 0x04003E6F RID: 15983
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, SignDataManager.SignFontData> fontDataByName = new Dictionary<string, SignDataManager.SignFontData>();

	// Token: 0x04003E70 RID: 15984
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, SignLibrary> signLibraries = new Dictionary<string, SignLibrary>(100);

	// Token: 0x04003E71 RID: 15985
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<GlobalSignId, SignDataManager.SignRenderingData> renderingDataByID = new Dictionary<GlobalSignId, SignDataManager.SignRenderingData>(100);

	// Token: 0x04003E72 RID: 15986
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.UpdateListenerMap updateListenerMap = new SignDataManager.UpdateListenerMap(100);

	// Token: 0x04003E73 RID: 15987
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.LayerDescriptor> layerDescriptors = new SignDataManager.IntermediateDataWrapper<SignDataManager.LayerDescriptor>(SignShaderIDs._LayerDescriptors, 100, (SignDataManager.SignRenderingData srd) => srd.descriptorBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.descriptorBuffer = b;
	});

	// Token: 0x04003E74 RID: 15988
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.CircleLayer> circleLayers = new SignDataManager.IntermediateDataWrapper<SignDataManager.CircleLayer>(SignShaderIDs._CircleLayers, 100, (SignDataManager.SignRenderingData srd) => srd.circleBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.circleBuffer = b;
	});

	// Token: 0x04003E75 RID: 15989
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.PolygonLayer> polygonLayers = new SignDataManager.IntermediateDataWrapper<SignDataManager.PolygonLayer>(SignShaderIDs._PolygonLayers, 100, (SignDataManager.SignRenderingData srd) => srd.polygonBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.polygonBuffer = b;
	});

	// Token: 0x04003E76 RID: 15990
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.CharLayer> charLayers = new SignDataManager.IntermediateDataWrapper<SignDataManager.CharLayer>(SignShaderIDs._CharLayers, 100, (SignDataManager.SignRenderingData srd) => srd.charBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.charBuffer = b;
	});

	// Token: 0x04003E77 RID: 15991
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.NoiseLayer> noiseLayers = new SignDataManager.IntermediateDataWrapper<SignDataManager.NoiseLayer>(SignShaderIDs._NoiseLayers, 100, (SignDataManager.SignRenderingData srd) => srd.noiseBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.noiseBuffer = b;
	});

	// Token: 0x04003E78 RID: 15992
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.TransformWarp> transformWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.TransformWarp>(SignShaderIDs._TransformWarps, 100, (SignDataManager.SignRenderingData srd) => srd.transformBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.transformBuffer = b;
	});

	// Token: 0x04003E79 RID: 15993
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.SkewWarp> skewWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.SkewWarp>(SignShaderIDs._SkewWarps, 100, (SignDataManager.SignRenderingData srd) => srd.skewBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.skewBuffer = b;
	});

	// Token: 0x04003E7A RID: 15994
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.BulgeWarp> bulgeWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.BulgeWarp>(SignShaderIDs._BulgeWarps, 100, (SignDataManager.SignRenderingData srd) => srd.bulgeBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.bulgeBuffer = b;
	});

	// Token: 0x04003E7B RID: 15995
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.TwirlWarp> twirlWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.TwirlWarp>(SignShaderIDs._TwirlWarps, 100, (SignDataManager.SignRenderingData srd) => srd.twirlBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.twirlBuffer = b;
	});

	// Token: 0x04003E7C RID: 15996
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.KaleidoWarp> kaleidoWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.KaleidoWarp>(SignShaderIDs._KaleidoWarps, 100, (SignDataManager.SignRenderingData srd) => srd.kaleidoBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.kaleidoBuffer = b;
	});

	// Token: 0x04003E7D RID: 15997
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.PerspectiveWarp> perspectiveWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.PerspectiveWarp>(SignShaderIDs._PerspectiveWarps, 100, (SignDataManager.SignRenderingData srd) => srd.perspectiveBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.perspectiveBuffer = b;
	});

	// Token: 0x04003E7E RID: 15998
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.ArcWarp> arcWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.ArcWarp>(SignShaderIDs._ArcWarps, 100, (SignDataManager.SignRenderingData srd) => srd.arcBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.arcBuffer = b;
	});

	// Token: 0x04003E7F RID: 15999
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.StretchWarp> stretchWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.StretchWarp>(SignShaderIDs._StretchWarps, 100, (SignDataManager.SignRenderingData srd) => srd.stretchBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.stretchBuffer = b;
	});

	// Token: 0x04003E80 RID: 16000
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IntermediateDataWrapper<SignDataManager.GridWarp> gridWarps = new SignDataManager.IntermediateDataWrapper<SignDataManager.GridWarp>(SignShaderIDs._GridWarps, 100, (SignDataManager.SignRenderingData srd) => srd.gridBuffer, delegate(SignDataManager.SignRenderingData srd, ComputeBuffer b)
	{
		srd.gridBuffer = b;
	});

	// Token: 0x04003E81 RID: 16001
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly SignDataManager.IIntermediateDataWrapper[] intermediateDataWrappers;

	// Token: 0x04003E82 RID: 16002
	[PublicizedFrom(EAccessModifier.Private)]
	public List<SignDataManager.ColorOperation> colorOpStack = new List<SignDataManager.ColorOperation>();

	// Token: 0x04003E83 RID: 16003
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2DArray fontAtlasTextureArray;

	// Token: 0x04003E84 RID: 16004
	[PublicizedFrom(EAccessModifier.Private)]
	public static SignDataManager m_Instance;

	// Token: 0x04003E86 RID: 16006
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSignSyncBatchBytes = 1048576;

	// Token: 0x04003E87 RID: 16007
	[PublicizedFrom(EAccessModifier.Private)]
	public bool signDataDownloadInProgress;

	// Token: 0x04003E88 RID: 16008
	[PublicizedFrom(EAccessModifier.Private)]
	public bool signDataDownloadComplete;

	// Token: 0x02000A83 RID: 2691
	[PublicizedFrom(EAccessModifier.Private)]
	public struct RuntimeRenderingInfo
	{
		// Token: 0x06005158 RID: 20824 RVA: 0x001F22C3 File Offset: 0x001F04C3
		public RuntimeRenderingInfo(int baseDescriptorIndex, int descriptorCount)
		{
			this.BaseDescriptorIndex = baseDescriptorIndex;
			this.DescriptorCount = descriptorCount;
		}

		// Token: 0x04003E89 RID: 16009
		public readonly int BaseDescriptorIndex;

		// Token: 0x04003E8A RID: 16010
		public readonly int DescriptorCount;
	}

	// Token: 0x02000A84 RID: 2692
	[PublicizedFrom(EAccessModifier.Private)]
	public class SignRenderingData : IDisposable
	{
		// Token: 0x06005159 RID: 20825 RVA: 0x001F22D3 File Offset: 0x001F04D3
		public SignRenderingData()
		{
			this.materialPropertyBlock = new MaterialPropertyBlock();
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x001F22FC File Offset: 0x001F04FC
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

		// Token: 0x0600515B RID: 20827 RVA: 0x001F2350 File Offset: 0x001F0550
		[PublicizedFrom(EAccessModifier.Protected)]
		public ~SignRenderingData()
		{
			this.Dispose(false);
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x001F2380 File Offset: 0x001F0580
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x001F2390 File Offset: 0x001F0590
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

		// Token: 0x0600515E RID: 20830 RVA: 0x001F23F8 File Offset: 0x001F05F8
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

		// Token: 0x04003E8B RID: 16011
		public static ComputeBuffer DummyBuffer = new ComputeBuffer(1, 4);

		// Token: 0x04003E8C RID: 16012
		public MaterialPropertyBlock materialPropertyBlock;

		// Token: 0x04003E8D RID: 16013
		public int layerCount;

		// Token: 0x04003E8E RID: 16014
		public ComputeBuffer descriptorBuffer;

		// Token: 0x04003E8F RID: 16015
		public ComputeBuffer circleBuffer;

		// Token: 0x04003E90 RID: 16016
		public ComputeBuffer polygonBuffer;

		// Token: 0x04003E91 RID: 16017
		public ComputeBuffer charBuffer;

		// Token: 0x04003E92 RID: 16018
		public ComputeBuffer noiseBuffer;

		// Token: 0x04003E93 RID: 16019
		public ComputeBuffer transformBuffer;

		// Token: 0x04003E94 RID: 16020
		public ComputeBuffer skewBuffer;

		// Token: 0x04003E95 RID: 16021
		public ComputeBuffer bulgeBuffer;

		// Token: 0x04003E96 RID: 16022
		public ComputeBuffer twirlBuffer;

		// Token: 0x04003E97 RID: 16023
		public ComputeBuffer kaleidoBuffer;

		// Token: 0x04003E98 RID: 16024
		public ComputeBuffer perspectiveBuffer;

		// Token: 0x04003E99 RID: 16025
		public ComputeBuffer arcBuffer;

		// Token: 0x04003E9A RID: 16026
		public ComputeBuffer stretchBuffer;

		// Token: 0x04003E9B RID: 16027
		public ComputeBuffer gridBuffer;

		// Token: 0x04003E9C RID: 16028
		public readonly Dictionary<SignData.SignLayer, SignDataManager.RuntimeRenderingInfo> renderingInfoByLayer = new Dictionary<SignData.SignLayer, SignDataManager.RuntimeRenderingInfo>();

		// Token: 0x04003E9D RID: 16029
		public readonly Dictionary<SignData.SignLayer, LayerComplexityInfo> complexityByLayer = new Dictionary<SignData.SignLayer, LayerComplexityInfo>();

		// Token: 0x04003E9E RID: 16030
		public float totalComplexity;

		// Token: 0x04003E9F RID: 16031
		public int totalWarps;

		// Token: 0x04003EA0 RID: 16032
		public int totalDraws;

		// Token: 0x04003EA1 RID: 16033
		public LayerStackInfo stackInfo;

		// Token: 0x04003EA2 RID: 16034
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposedValue;
	}

	// Token: 0x02000A85 RID: 2693
	[PublicizedFrom(EAccessModifier.Private)]
	public class UpdateListenerMap
	{
		// Token: 0x06005160 RID: 20832 RVA: 0x001F2563 File Offset: 0x001F0763
		public UpdateListenerMap(int capacity)
		{
			this._idsByListener = new Dictionary<ISignRenderingDataUpdateListener, HashSet<GlobalSignId>>(capacity);
			this._listenersById = new Dictionary<GlobalSignId, HashSet<ISignRenderingDataUpdateListener>>(capacity);
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x001F2584 File Offset: 0x001F0784
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

		// Token: 0x06005162 RID: 20834 RVA: 0x001F25EC File Offset: 0x001F07EC
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

		// Token: 0x06005163 RID: 20835 RVA: 0x001F2680 File Offset: 0x001F0880
		public IEnumerable<ISignRenderingDataUpdateListener> GetListeners(GlobalSignId id)
		{
			HashSet<ISignRenderingDataUpdateListener> hashSet;
			if (!this._listenersById.TryGetValue(id, out hashSet))
			{
				return Array.Empty<ISignRenderingDataUpdateListener>();
			}
			return hashSet;
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x001F26A8 File Offset: 0x001F08A8
		public IEnumerable<GlobalSignId> GetIds(ISignRenderingDataUpdateListener listener)
		{
			HashSet<GlobalSignId> hashSet;
			if (!this._idsByListener.TryGetValue(listener, out hashSet))
			{
				return Array.Empty<GlobalSignId>();
			}
			return hashSet;
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x001F26D0 File Offset: 0x001F08D0
		public void Clear()
		{
			this._listenersById.Clear();
			this._idsByListener.Clear();
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x001F26E8 File Offset: 0x001F08E8
		public bool HasListeners(GlobalSignId signId)
		{
			return this._listenersById.ContainsKey(signId);
		}

		// Token: 0x04003EA3 RID: 16035
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<GlobalSignId, HashSet<ISignRenderingDataUpdateListener>> _listenersById;

		// Token: 0x04003EA4 RID: 16036
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<ISignRenderingDataUpdateListener, HashSet<GlobalSignId>> _idsByListener;
	}

	// Token: 0x02000A86 RID: 2694
	[PublicizedFrom(EAccessModifier.Private)]
	public enum LayerTypeID
	{
		// Token: 0x04003EA6 RID: 16038
		UvClear,
		// Token: 0x04003EA7 RID: 16039
		UvPop,
		// Token: 0x04003EA8 RID: 16040
		UvPush,
		// Token: 0x04003EA9 RID: 16041
		MaskBegin,
		// Token: 0x04003EAA RID: 16042
		MaskFlipPhase,
		// Token: 0x04003EAB RID: 16043
		MaskEnd,
		// Token: 0x04003EAC RID: 16044
		WarpTransform = 10,
		// Token: 0x04003EAD RID: 16045
		WarpSkew,
		// Token: 0x04003EAE RID: 16046
		WarpBulge,
		// Token: 0x04003EAF RID: 16047
		WarpTwirl,
		// Token: 0x04003EB0 RID: 16048
		WarpKaleido,
		// Token: 0x04003EB1 RID: 16049
		WarpPerspective,
		// Token: 0x04003EB2 RID: 16050
		WarpArc,
		// Token: 0x04003EB3 RID: 16051
		WarpStretch,
		// Token: 0x04003EB4 RID: 16052
		WarpGrid,
		// Token: 0x04003EB5 RID: 16053
		Polygon = 100,
		// Token: 0x04003EB6 RID: 16054
		Char,
		// Token: 0x04003EB7 RID: 16055
		Circle,
		// Token: 0x04003EB8 RID: 16056
		Noise
	}

	// Token: 0x02000A87 RID: 2695
	[PublicizedFrom(EAccessModifier.Private)]
	public struct LayerDescriptor
	{
		// Token: 0x04003EB9 RID: 16057
		public int type;

		// Token: 0x04003EBA RID: 16058
		public int index;
	}

	// Token: 0x02000A88 RID: 2696
	[PublicizedFrom(EAccessModifier.Private)]
	public struct CircleLayer
	{
		// Token: 0x04003EBB RID: 16059
		public Vector4 color;

		// Token: 0x04003EBC RID: 16060
		public float softness;

		// Token: 0x04003EBD RID: 16061
		public float dilate;

		// Token: 0x04003EBE RID: 16062
		public float frequency;

		// Token: 0x04003EBF RID: 16063
		public int mode;
	}

	// Token: 0x02000A89 RID: 2697
	[PublicizedFrom(EAccessModifier.Private)]
	public struct PolygonLayer
	{
		// Token: 0x04003EC0 RID: 16064
		public Vector4 color;

		// Token: 0x04003EC1 RID: 16065
		public float sides;

		// Token: 0x04003EC2 RID: 16066
		public float smoothness;

		// Token: 0x04003EC3 RID: 16067
		public float starify;

		// Token: 0x04003EC4 RID: 16068
		public float softness;

		// Token: 0x04003EC5 RID: 16069
		public float dilate;

		// Token: 0x04003EC6 RID: 16070
		public float frequency;

		// Token: 0x04003EC7 RID: 16071
		public int mode;
	}

	// Token: 0x02000A8A RID: 2698
	[PublicizedFrom(EAccessModifier.Private)]
	public struct CharLayer
	{
		// Token: 0x04003EC8 RID: 16072
		public Vector2 position;

		// Token: 0x04003EC9 RID: 16073
		public Vector2 scale;

		// Token: 0x04003ECA RID: 16074
		public Vector4 color;

		// Token: 0x04003ECB RID: 16075
		public int atlasIndex;

		// Token: 0x04003ECC RID: 16076
		public Vector4 offsetSize;

		// Token: 0x04003ECD RID: 16077
		public float softness;

		// Token: 0x04003ECE RID: 16078
		public float dilate;
	}

	// Token: 0x02000A8B RID: 2699
	[PublicizedFrom(EAccessModifier.Private)]
	public struct NoiseLayer
	{
		// Token: 0x04003ECF RID: 16079
		public Vector4 color;

		// Token: 0x04003ED0 RID: 16080
		public float offset;

		// Token: 0x04003ED1 RID: 16081
		public int detail;

		// Token: 0x04003ED2 RID: 16082
		public float softness;

		// Token: 0x04003ED3 RID: 16083
		public float dilate;

		// Token: 0x04003ED4 RID: 16084
		public float fade;
	}

	// Token: 0x02000A8C RID: 2700
	[PublicizedFrom(EAccessModifier.Private)]
	public struct TransformWarp
	{
		// Token: 0x04003ED5 RID: 16085
		public Vector2 position;

		// Token: 0x04003ED6 RID: 16086
		public float rotation;

		// Token: 0x04003ED7 RID: 16087
		public Vector2 scale;
	}

	// Token: 0x02000A8D RID: 2701
	[PublicizedFrom(EAccessModifier.Private)]
	public struct SkewWarp
	{
		// Token: 0x04003ED8 RID: 16088
		public float rotation;

		// Token: 0x04003ED9 RID: 16089
		public Vector2 amount;
	}

	// Token: 0x02000A8E RID: 2702
	[PublicizedFrom(EAccessModifier.Private)]
	public struct BulgeWarp
	{
		// Token: 0x04003EDA RID: 16090
		public Vector2 position;

		// Token: 0x04003EDB RID: 16091
		public float amount;
	}

	// Token: 0x02000A8F RID: 2703
	[PublicizedFrom(EAccessModifier.Private)]
	public struct TwirlWarp
	{
		// Token: 0x04003EDC RID: 16092
		public Vector2 position;

		// Token: 0x04003EDD RID: 16093
		public float amount;

		// Token: 0x04003EDE RID: 16094
		public float frequency;
	}

	// Token: 0x02000A90 RID: 2704
	[PublicizedFrom(EAccessModifier.Private)]
	public struct KaleidoWarp
	{
		// Token: 0x04003EDF RID: 16095
		public Vector2 position;

		// Token: 0x04003EE0 RID: 16096
		public float angle;

		// Token: 0x04003EE1 RID: 16097
		public float offset;
	}

	// Token: 0x02000A91 RID: 2705
	[PublicizedFrom(EAccessModifier.Private)]
	public struct PerspectiveWarp
	{
		// Token: 0x04003EE2 RID: 16098
		public Vector3 rotation;

		// Token: 0x04003EE3 RID: 16099
		public float strength;
	}

	// Token: 0x02000A92 RID: 2706
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ArcWarp
	{
		// Token: 0x04003EE4 RID: 16100
		public float rotation;

		// Token: 0x04003EE5 RID: 16101
		public float radius;

		// Token: 0x04003EE6 RID: 16102
		public float width;
	}

	// Token: 0x02000A93 RID: 2707
	[PublicizedFrom(EAccessModifier.Private)]
	public struct StretchWarp
	{
		// Token: 0x04003EE7 RID: 16103
		public Vector2 position;

		// Token: 0x04003EE8 RID: 16104
		public float rotation;

		// Token: 0x04003EE9 RID: 16105
		public float distance;

		// Token: 0x04003EEA RID: 16106
		public float width;

		// Token: 0x04003EEB RID: 16107
		public float exponent;
	}

	// Token: 0x02000A94 RID: 2708
	[PublicizedFrom(EAccessModifier.Private)]
	public struct GridWarp
	{
		// Token: 0x04003EEC RID: 16108
		public Vector2 offset;

		// Token: 0x04003EED RID: 16109
		public float rotation;

		// Token: 0x04003EEE RID: 16110
		public float scale;

		// Token: 0x04003EEF RID: 16111
		public int mode;

		// Token: 0x04003EF0 RID: 16112
		public float aspect;

		// Token: 0x04003EF1 RID: 16113
		public float shift;
	}

	// Token: 0x02000A95 RID: 2709
	[PublicizedFrom(EAccessModifier.Private)]
	public class CharAtlasData
	{
		// Token: 0x06005167 RID: 20839 RVA: 0x001F26F6 File Offset: 0x001F08F6
		public CharAtlasData(Vector2 quadOffset, Vector2 quadSize, Vector4 uvOffsetSize, float charWidth)
		{
			this.quadOffset = quadOffset;
			this.quadSize = quadSize;
			this.uvOffsetSize = uvOffsetSize;
			this.charWidth = charWidth;
		}

		// Token: 0x04003EF2 RID: 16114
		public Vector2 quadOffset;

		// Token: 0x04003EF3 RID: 16115
		public Vector2 quadSize;

		// Token: 0x04003EF4 RID: 16116
		public Vector4 uvOffsetSize;

		// Token: 0x04003EF5 RID: 16117
		public float charWidth;
	}

	// Token: 0x02000A96 RID: 2710
	[PublicizedFrom(EAccessModifier.Private)]
	public class SignFontData
	{
		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06005168 RID: 20840 RVA: 0x001F271B File Offset: 0x001F091B
		// (set) Token: 0x06005169 RID: 20841 RVA: 0x001F2723 File Offset: 0x001F0923
		public int AtlasIndex
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x0600516A RID: 20842 RVA: 0x001F272C File Offset: 0x001F092C
		// (set) Token: 0x0600516B RID: 20843 RVA: 0x001F2734 File Offset: 0x001F0934
		public Texture2D AtlasTexture
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x0600516C RID: 20844 RVA: 0x001F273D File Offset: 0x001F093D
		// (set) Token: 0x0600516D RID: 20845 RVA: 0x001F2745 File Offset: 0x001F0945
		public float LineHeight
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x001F2750 File Offset: 0x001F0950
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

		// Token: 0x0600516F RID: 20847 RVA: 0x001F2938 File Offset: 0x001F0B38
		public SignFontData(int atlasIndex, TMP_FontAsset fontAsset)
		{
			this.AtlasIndex = atlasIndex;
			this.AtlasTexture = fontAsset.atlasTexture;
			this.ConstructAtlasDataMap(fontAsset);
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x001F2965 File Offset: 0x001F0B65
		public bool TryGetCharAtlasData(char character, out SignDataManager.CharAtlasData atlasData)
		{
			return this.charToAtlasDataMap.TryGetValue((uint)character, out atlasData);
		}

		// Token: 0x04003EF9 RID: 16121
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<uint, SignDataManager.CharAtlasData> charToAtlasDataMap = new Dictionary<uint, SignDataManager.CharAtlasData>();
	}

	// Token: 0x02000A97 RID: 2711
	public struct ColorOperation
	{
		// Token: 0x06005171 RID: 20849 RVA: 0x001F2974 File Offset: 0x001F0B74
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

		// Token: 0x04003EFA RID: 16122
		public Color color;

		// Token: 0x04003EFB RID: 16123
		public SignData.GroupSignLayer.ColorMode mode;
	}

	// Token: 0x02000A98 RID: 2712
	public struct GroupOffsets
	{
		// Token: 0x06005172 RID: 20850 RVA: 0x001F2A38 File Offset: 0x001F0C38
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

		// Token: 0x04003EFC RID: 16124
		public float shapeSoftnessOffset;

		// Token: 0x04003EFD RID: 16125
		public float shapeDilateOffset;

		// Token: 0x04003EFE RID: 16126
		public float textSoftnessOffset;

		// Token: 0x04003EFF RID: 16127
		public float textDilateOffset;

		// Token: 0x04003F00 RID: 16128
		public float noiseSoftnessOffset;

		// Token: 0x04003F01 RID: 16129
		public float noiseDilateOffset;
	}

	// Token: 0x02000A99 RID: 2713
	[PublicizedFrom(EAccessModifier.Private)]
	public interface IIntermediateDataWrapper
	{
		// Token: 0x06005173 RID: 20851
		void UpdateBuffer(SignDataManager.SignRenderingData signRenderingData);

		// Token: 0x06005174 RID: 20852
		void ApplyToMaterial(SignDataManager.SignRenderingData signRenderingData, Material material);

		// Token: 0x06005175 RID: 20853
		void ClearStagingList();
	}

	// Token: 0x02000A9A RID: 2714
	[PublicizedFrom(EAccessModifier.Private)]
	public class IntermediateDataWrapper<T> : SignDataManager.IIntermediateDataWrapper where T : struct
	{
		// Token: 0x06005176 RID: 20854 RVA: 0x001F2AFE File Offset: 0x001F0CFE
		public IntermediateDataWrapper(int shaderPropertyId, int capacity, Func<SignDataManager.SignRenderingData, ComputeBuffer> getter, Action<SignDataManager.SignRenderingData, ComputeBuffer> setter)
		{
			this._shaderPropertyId = shaderPropertyId;
			this.List = new List<T>(capacity);
			this._get = getter;
			this._set = setter;
		}

		// Token: 0x06005177 RID: 20855 RVA: 0x001F2B28 File Offset: 0x001F0D28
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

		// Token: 0x06005178 RID: 20856 RVA: 0x001F2C02 File Offset: 0x001F0E02
		public void ApplyToMaterial(SignDataManager.SignRenderingData signRenderingData, Material material)
		{
			material.SetBuffer(this._shaderPropertyId, this._get(signRenderingData) ?? SignDataManager.SignRenderingData.DummyBuffer);
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x001F2C25 File Offset: 0x001F0E25
		public void ClearStagingList()
		{
			this.List.Clear();
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x001F2C32 File Offset: 0x001F0E32
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

		// Token: 0x04003F02 RID: 16130
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly int _stride = Marshal.SizeOf(typeof(T));

		// Token: 0x04003F03 RID: 16131
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int _shaderPropertyId;

		// Token: 0x04003F04 RID: 16132
		public readonly List<T> List;

		// Token: 0x04003F05 RID: 16133
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Func<SignDataManager.SignRenderingData, ComputeBuffer> _get;

		// Token: 0x04003F06 RID: 16134
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Action<SignDataManager.SignRenderingData, ComputeBuffer> _set;
	}

	// Token: 0x02000A9B RID: 2715
	// (Invoke) Token: 0x0600517D RID: 20861
	public delegate void RenderingDataPatcher(MaterialPropertyBlock mpb);
}
