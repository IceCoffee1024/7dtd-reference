using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Challenges;
using DynamicMusic.Legacy.ObjectModel;
using Noemax.GZip;
using Twitch;
using UnityEngine;
using WorldGenerationEngineFinal;
using XMLData.Item;

// Token: 0x02001338 RID: 4920
public static class WorldStaticData
{
	// Token: 0x06009B25 RID: 39717 RVA: 0x003AC730 File Offset: 0x003AA930
	public static void InitSync(bool _bForce, bool _bDediServer, bool _cleanup = true, string[] _onlyXmls = null)
	{
		if (_cleanup)
		{
			WorldStaticData.Cleanup();
		}
		LoadManager.InitSync();
		ThreadManager.RunCoroutineSync(WorldStaticData.Init(_bForce, _bDediServer, null, _onlyXmls));
	}

	// Token: 0x06009B26 RID: 39718 RVA: 0x003AC74D File Offset: 0x003AA94D
	public static IEnumerator Init(bool _bForce, bool _bDediServer, WorldStaticData.ProgressDelegate _progressDelegate, string[] _onlyXmls = null)
	{
		WorldStaticData.isDediServer = _bDediServer;
		if (!_bForce && WorldStaticData.bInitDone)
		{
			Log.Out("WorldStaticData.Init() was already done");
			yield break;
		}
		MicroStopwatch sw = new MicroStopwatch();
		if (_progressDelegate != null)
		{
			_progressDelegate(Localization.Get("loadActionParticles", false, null), 0f);
		}
		yield return null;
		yield return null;
		yield return ParticleEffect.LoadResources();
		List<LoadManager.AssetRequestTask<GameObject>> explosionPrefabLoadTasks = new List<LoadManager.AssetRequestTask<GameObject>>();
		int num;
		for (int i = 0; i < 100; i = num + 1)
		{
			explosionPrefabLoadTasks.Add(LoadManager.LoadAssetFromAddressables<GameObject>(string.Format("Prefabs/prefabExplosion{0}.prefab", i), null, null, false, false, false));
			yield return null;
			num = i;
		}
		yield return LoadManager.WaitAll(explosionPrefabLoadTasks);
		WorldStaticData.prefabExplosions = new Transform[100];
		for (int j = 0; j < 100; j++)
		{
			LoadManager.AssetRequestTask<GameObject> assetRequestTask = explosionPrefabLoadTasks[j];
			if (assetRequestTask.Asset)
			{
				WorldStaticData.prefabExplosions[j] = assetRequestTask.Asset.transform;
			}
		}
		if (_progressDelegate != null)
		{
			_progressDelegate(Localization.Get("loadActionBlockTextures", false, null), 0f);
		}
		yield return null;
		yield return null;
		if (!WorldStaticData.meshDescCol)
		{
			WorldStaticData.<>c__DisplayClass13_0 CS$<>8__locals1 = new WorldStaticData.<>c__DisplayClass13_0();
			bool flag = !Application.isPlaying;
			LoadManager.AssetRequestTask<GameObject> request = LoadManager.LoadAssetFromAddressables<GameObject>("BlockTextureAtlases", "prefabMeshDescriptionCollection.prefab", null, null, false, flag, false);
			yield return request;
			GameObject asset = request.Asset;
			if (asset == null)
			{
				throw new Exception("Missing resource bundle BlockTextureAtlases");
			}
			WorldStaticData.meshDescCol = asset.GetComponent<MeshDescriptionCollection>();
			WorldStaticData.meshDescCol.Init();
			CS$<>8__locals1.coroutineHadException = false;
			yield return ThreadManager.CoroutineWrapperWithExceptionCallback(WorldStaticData.meshDescCol.LoadTextureArraysForQuality(false), delegate(Exception _exception)
			{
				Log.Error("WSD.Init: Initializing MDC failed");
				Log.Exception(_exception);
				CS$<>8__locals1.coroutineHadException = true;
			});
			if (CS$<>8__locals1.coroutineHadException)
			{
				yield break;
			}
			CS$<>8__locals1 = null;
			request = null;
		}
		if (GameManager.IsDedicatedServer)
		{
			for (int k = 0; k < WorldStaticData.meshDescCol.meshes.Length; k++)
			{
				WorldStaticData.meshDescCol.meshes[k].TexDiffuse = null;
				WorldStaticData.meshDescCol.meshes[k].TexNormal = null;
				WorldStaticData.meshDescCol.meshes[k].TexSpecular = null;
				WorldStaticData.meshDescCol.meshes[k].TexEmission = null;
				WorldStaticData.meshDescCol.meshes[k].TexHeight = null;
				WorldStaticData.meshDescCol.meshes[k].TexOcclusion = null;
				WorldStaticData.meshDescCol.meshes[k].TexMask = null;
				WorldStaticData.meshDescCol.meshes[k].TexMaskNormal = null;
			}
		}
		yield return WorldStaticData.LoadAllXmlsCo(!_bForce, _progressDelegate, _onlyXmls);
		WorldStaticData.bInitDone = true;
		Log.Out(string.Format("WorldStaticData.Init() needed {0:F3} s", (float)sw.ElapsedMilliseconds / 1000f));
		GameOptionsManager.TextureQualityChanged += delegate(int _obj)
		{
			ThreadManager.RunCoroutineSync(WorldStaticData.meshDescCol.LoadTextureArraysForQuality(true));
		};
		GameOptionsManager.TextureFilterChanged += delegate(int _obj)
		{
			WorldStaticData.meshDescCol.SetTextureArraysFilter();
		};
		yield break;
	}

	// Token: 0x06009B27 RID: 39719 RVA: 0x003AC771 File Offset: 0x003AA971
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadTextureAtlases()
	{
		MeshDescription[] meshes = WorldStaticData.meshDescCol.Meshes;
		MeshDescription.meshes = new MeshDescription[meshes.Length];
		int num;
		for (int i = 0; i < meshes.Length; i = num + 1)
		{
			MeshDescription.meshes[i] = meshes[i];
			string textureAtlasClass = meshes[i].TextureAtlasClass;
			Type type = Type.GetType(textureAtlasClass);
			if (type == null)
			{
				Log.Error(string.Format("Could not find type '{0}' for texture atlas on mesh layer {1}", textureAtlasClass, i));
				yield break;
			}
			TextureAtlas textureAtlas = (TextureAtlas)Activator.CreateInstance(type);
			textureAtlas.LoadTextureAtlas(i, WorldStaticData.meshDescCol, !WorldStaticData.isDediServer);
			yield return MeshDescription.meshes[i].Init(i, textureAtlas);
			yield return null;
			num = i;
		}
		MeshDescription.SetGrassQuality();
		MeshDescription.SetWaterQuality();
		yield break;
	}

	// Token: 0x06009B28 RID: 39720 RVA: 0x003AC779 File Offset: 0x003AA979
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadMaterials(XmlFile _xmlFile)
	{
		AIDirectorData.InitStatic();
		yield return MaterialsFromXml.CreateMaterials(_xmlFile);
		yield break;
	}

	// Token: 0x06009B29 RID: 39721 RVA: 0x003AC788 File Offset: 0x003AA988
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadBlocks(XmlFile _xmlFile)
	{
		Block.InitStatic();
		yield return BlocksFromXml.CreateBlocks(_xmlFile, false, GameManager.Instance != null && GameManager.Instance.IsEditMode());
		Block.AssignIds();
		yield break;
	}

	// Token: 0x06009B2A RID: 39722 RVA: 0x003AC797 File Offset: 0x003AA997
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadMisc(XmlFile _xmlFile)
	{
		AnimationDelayData.InitStatic();
		AnimationGunjointOffsetData.InitStatic();
		yield return MiscFromXml.Create(_xmlFile);
		yield break;
	}

	// Token: 0x06009B2B RID: 39723 RVA: 0x003AC7A6 File Offset: 0x003AA9A6
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadMisc(XmlFile _xmlFile)
	{
		AnimationDelayData.InitStatic();
		AnimationGunjointOffsetData.InitStatic();
		ThreadManager.RunCoroutineSync(MiscFromXml.Create(_xmlFile));
	}

	// Token: 0x06009B2C RID: 39724 RVA: 0x003AC7BD File Offset: 0x003AA9BD
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadItems(XmlFile _xmlFile)
	{
		ItemClass.Cleanup();
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadItems(_xmlFile));
	}

	// Token: 0x06009B2D RID: 39725 RVA: 0x003AC7CF File Offset: 0x003AA9CF
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadItemsAppend(XmlFile _xmlFile)
	{
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadItems(_xmlFile));
	}

	// Token: 0x06009B2E RID: 39726 RVA: 0x003AC7DC File Offset: 0x003AA9DC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadItemModifiers(XmlFile _xmlFile)
	{
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadItemModifiers(_xmlFile));
		ThreadManager.RunCoroutineSync(WorldStaticData.LateInitItems());
	}

	// Token: 0x06009B2F RID: 39727 RVA: 0x003AC7F3 File Offset: 0x003AA9F3
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadItems(XmlFile _xmlFile)
	{
		CraftingManager.ClearLockedData();
		Equipment.SetupCosmeticMapping();
		ItemClass.InitStatic();
		ItemClassesFromXml.CreateItemsFromBlocks();
		yield return null;
		yield return ItemClassesFromXml.CreateItems(_xmlFile);
		ItemData.Parser.idOffset = Block.ItemsStartHere;
		yield break;
	}

	// Token: 0x06009B30 RID: 39728 RVA: 0x003AC802 File Offset: 0x003AAA02
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadItemsAppend(XmlFile _xmlFile)
	{
		yield return ItemClassesFromXml.CreateItems(_xmlFile);
		ItemData.Parser.idOffset = Block.ItemsStartHere;
		yield break;
	}

	// Token: 0x06009B31 RID: 39729 RVA: 0x003AC811 File Offset: 0x003AAA11
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LateInitItems()
	{
		ItemClass.AssignIds();
		Block.LateInitAll();
		ItemClass.LateInitAll();
		yield break;
	}

	// Token: 0x06009B32 RID: 39730 RVA: 0x003AC819 File Offset: 0x003AAA19
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadItemModifiers(XmlFile _xmlFile)
	{
		yield return ItemModificationsFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009B33 RID: 39731 RVA: 0x003AC828 File Offset: 0x003AAA28
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadRecipes(XmlFile _xmlFile)
	{
		yield return RecipesFromXml.LoadRecipies(_xmlFile);
		List<Recipe> allRecipes = CraftingManager.GetAllRecipes();
		Dictionary<string, List<Recipe>> recipesByName = new Dictionary<string, List<Recipe>>();
		foreach (Recipe recipe in allRecipes)
		{
			string name = recipe.GetName();
			List<Recipe> list;
			if (!recipesByName.TryGetValue(name, out list))
			{
				list = new List<Recipe>();
				recipesByName[name] = list;
			}
			list.Add(recipe);
		}
		MicroStopwatch msw = new MicroStopwatch(true);
		List<string> recipeCalcStack = new List<string>();
		foreach (ItemClass itemClass in ItemClass.list)
		{
			if (itemClass != null)
			{
				recipeCalcStack.Clear();
				itemClass.AutoCalcWeight(recipesByName);
				if (itemClass.AutoCalcEcoVal(recipesByName, recipeCalcStack) < 0f)
				{
					Log.Warning("Loading recipes: Could not calculate eco value for item " + itemClass.GetItemName() + ": Only recursive recipes found");
				}
				if (recipeCalcStack.Count > 0)
				{
					Log.Warning("Loading recipes: Eco value calculation stack not empty for item " + itemClass.GetItemName() + ": " + string.Join(" > ", recipeCalcStack));
				}
				if (msw.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
				{
					yield return null;
					msw.ResetAndRestart();
				}
			}
		}
		ItemClass[] array = null;
		yield break;
	}

	// Token: 0x06009B34 RID: 39732 RVA: 0x003AC837 File Offset: 0x003AAA37
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadRecipes(XmlFile _xmlFile)
	{
		CraftingManager.ClearAllRecipes();
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadRecipes(_xmlFile));
	}

	// Token: 0x06009B35 RID: 39733 RVA: 0x003AC849 File Offset: 0x003AAA49
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadLoot(XmlFile _xmlFile)
	{
		LootContainer.InitStatic();
		yield return LootFromXml.LoadLootContainers(_xmlFile);
		yield break;
	}

	// Token: 0x06009B36 RID: 39734 RVA: 0x003AC858 File Offset: 0x003AAA58
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadLoot(XmlFile _xmlFile)
	{
		LootContainer.Cleanup();
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadLoot(_xmlFile));
	}

	// Token: 0x06009B37 RID: 39735 RVA: 0x003AC86A File Offset: 0x003AAA6A
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadWeather(XmlFile _xmlFile)
	{
		WeatherSurvivalParametersFromXml.Load(_xmlFile);
		WorldStaticData.LinkBuffs();
		yield break;
	}

	// Token: 0x06009B38 RID: 39736 RVA: 0x003AC879 File Offset: 0x003AAA79
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadSDCSArchetypes(XmlFile _xmlFile)
	{
		SDCSArchetypesFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009B39 RID: 39737 RVA: 0x003AC888 File Offset: 0x003AAA88
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadTraders(XmlFile _xmlFile)
	{
		TraderInfo.InitStatic();
		yield return TradersFromXml.LoadTraderInfo(_xmlFile);
		yield break;
	}

	// Token: 0x06009B3A RID: 39738 RVA: 0x003AC897 File Offset: 0x003AAA97
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadNpc(XmlFile _xmlFile)
	{
		NPCInfo.InitStatic();
		yield return NPCsFromXml.LoadNPCInfo(_xmlFile);
		yield break;
	}

	// Token: 0x06009B3B RID: 39739 RVA: 0x003AC8A6 File Offset: 0x003AAAA6
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadMusic(XmlFile _xmlFile)
	{
		MusicGroup.InitStatic();
		yield return MusicDataFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009B3C RID: 39740 RVA: 0x003AC8B5 File Offset: 0x003AAAB5
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadUIDisplayInfo(XmlFile _xmlFile)
	{
		UIDisplayInfoManager.Reset();
		yield return UIDisplayInfoFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009B3D RID: 39741 RVA: 0x003AC8C4 File Offset: 0x003AAAC4
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadBiomes(XmlFile _xmlFile)
	{
		new WorldBiomes(_xmlFile.XmlDoc, true);
		yield break;
	}

	// Token: 0x06009B3E RID: 39742 RVA: 0x003AC8D3 File Offset: 0x003AAAD3
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadSpawning(XmlFile _xmlFile)
	{
		EntitySpawnerClassesFromXml.LoadEntitySpawnerClasses(_xmlFile.XmlDoc);
		yield return BiomeSpawningFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009B3F RID: 39743 RVA: 0x003AC8E2 File Offset: 0x003AAAE2
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupBlocks()
	{
		AIDirectorData.Cleanup();
		Block.Cleanup();
		TileEntityCompositeData.Cleanup();
	}

	// Token: 0x06009B40 RID: 39744 RVA: 0x003AC8F3 File Offset: 0x003AAAF3
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupGamestages()
	{
		GameStageDefinition.Clear();
		GameStageGroup.Clear();
	}

	// Token: 0x06009B41 RID: 39745 RVA: 0x003AC8FF File Offset: 0x003AAAFF
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupChallenges()
	{
		ChallengeClass.Cleanup();
	}

	// Token: 0x06009B42 RID: 39746 RVA: 0x003AC906 File Offset: 0x003AAB06
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupGameEvents()
	{
		if (GameEventManager.HasInstance)
		{
			GameEventManager.Current.Cleanup();
		}
	}

	// Token: 0x06009B43 RID: 39747 RVA: 0x003AC919 File Offset: 0x003AAB19
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupTwitch()
	{
		if (TwitchActionManager.HasInstance)
		{
			TwitchActionManager.Current.Cleanup();
		}
	}

	// Token: 0x06009B44 RID: 39748 RVA: 0x003AC92C File Offset: 0x003AAB2C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupTwitchEvents()
	{
		if (TwitchManager.HasInstance)
		{
			TwitchManager.Current.CleanupEventData();
		}
	}

	// Token: 0x06009B45 RID: 39749 RVA: 0x003AC93F File Offset: 0x003AAB3F
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupSpawning()
	{
		EntitySpawnerClass.Cleanup();
		BiomeSpawningClass.Cleanup();
	}

	// Token: 0x06009B46 RID: 39750 RVA: 0x003AC94B File Offset: 0x003AAB4B
	public static void LoadPhysicsBodies()
	{
		WorldStaticData.Reset("physicsbodies");
	}

	// Token: 0x06009B47 RID: 39751 RVA: 0x003AC957 File Offset: 0x003AAB57
	public static void SavePhysicsBodies()
	{
		PhysicsBodiesFromXml.Save(GameIO.GetGameDir("Data/Config") + "/physicsbodies.xml");
	}

	// Token: 0x06009B48 RID: 39752 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LinkBuffs()
	{
	}

	// Token: 0x06009B49 RID: 39753 RVA: 0x003AC974 File Offset: 0x003AAB74
	public static void Cleanup(string _xmlNameContaining)
	{
		foreach (WorldStaticData.XmlLoadInfo xmlLoadInfo in WorldStaticData.xmlsToLoad)
		{
			if (string.IsNullOrEmpty(_xmlNameContaining) || xmlLoadInfo.XmlName.ContainsCaseInsensitive(_xmlNameContaining))
			{
				Action cleanupMethod = xmlLoadInfo.CleanupMethod;
				if (cleanupMethod != null)
				{
					cleanupMethod();
				}
			}
		}
	}

	// Token: 0x06009B4A RID: 39754 RVA: 0x003AC9C0 File Offset: 0x003AABC0
	public static void Cleanup()
	{
		WorldStaticData.Cleanup(null);
		if (WorldStaticData.meshDescCol)
		{
			WorldStaticData.meshDescCol.Cleanup();
			WorldStaticData.meshDescCol = null;
		}
		MeshDescription.Cleanup();
		AssetBundles.Cleanup();
		WorldStaticData.bInitDone = false;
	}

	// Token: 0x06009B4B RID: 39755 RVA: 0x000027FC File Offset: 0x000009FC
	public static void QuitCleanup()
	{
	}

	// Token: 0x06009B4C RID: 39756 RVA: 0x003AC9F4 File Offset: 0x003AABF4
	public static void Reset(string _xmlNameContaining)
	{
		WorldStaticData.Cleanup(_xmlNameContaining);
		MemoryStream memoryStream = new MemoryStream();
		DeflateOutputStream deflateOutputStream = new DeflateOutputStream(memoryStream, 3);
		foreach (WorldStaticData.XmlLoadInfo xmlLoadInfo in WorldStaticData.xmlsToLoad)
		{
			if (string.IsNullOrEmpty(_xmlNameContaining) || xmlLoadInfo.XmlName.ContainsCaseInsensitive(_xmlNameContaining))
			{
				if (!xmlLoadInfo.XmlFileExists())
				{
					if (!xmlLoadInfo.IgnoreMissingFile)
					{
						Log.Error("XML loader: XML is missing: " + xmlLoadInfo.XmlName);
					}
				}
				else
				{
					ThreadManager.RunCoroutineSync(WorldStaticData.loadSingleXml(xmlLoadInfo, memoryStream, deflateOutputStream));
				}
			}
		}
	}

	// Token: 0x06009B4D RID: 39757 RVA: 0x003ACA7D File Offset: 0x003AAC7D
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator cacheSingleXml(WorldStaticData.XmlLoadInfo _loadInfo, XmlFile _origXml, MemoryStream _memStream, DeflateOutputStream _zipStream)
	{
		MicroStopwatch timer = new MicroStopwatch(true);
		_memStream.SetLength(0L);
		_origXml.SerializeToStream(_zipStream, true, null);
		_zipStream.Restart();
		if (timer.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
		{
			yield return null;
			timer.ResetAndRestart();
		}
		_loadInfo.CompressedXmlData = _memStream.ToArray();
		_memStream.SetLength(0L);
		yield break;
	}

	// Token: 0x06009B4E RID: 39758 RVA: 0x003ACAA1 File Offset: 0x003AACA1
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator loadSingleXml(WorldStaticData.XmlLoadInfo _loadInfo, MemoryStream _memStream, DeflateOutputStream _zipStream)
	{
		MicroStopwatch timer = new MicroStopwatch(true);
		bool coroutineHadException = false;
		XmlFile xmlFile = null;
		yield return XmlPatcher.LoadAndPatchConfig(_loadInfo.XmlName, delegate(XmlFile _file)
		{
			xmlFile = _file;
		});
		if (xmlFile == null)
		{
			yield break;
		}
		yield return XmlPatcher.ApplyConditionalXmlBlocks(_loadInfo.XmlName, xmlFile, timer, XmlPatcher.EEvaluator.Host, delegate
		{
			coroutineHadException = true;
		});
		if (coroutineHadException)
		{
			yield break;
		}
		if (timer.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
		{
			yield return null;
			timer.ResetAndRestart();
		}
		yield return WorldStaticData.DumpPatchedXml(_loadInfo, xmlFile, timer);
		xmlFile.RemoveComments();
		yield return WorldStaticData.CachePatchedXml(_loadInfo, xmlFile, timer, _memStream, _zipStream, delegate
		{
			coroutineHadException = true;
		});
		if (coroutineHadException)
		{
			yield break;
		}
		if (timer.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
		{
			yield return null;
			timer.ResetAndRestart();
		}
		yield return XmlPatcher.ApplyConditionalXmlBlocks(_loadInfo.XmlName, xmlFile, timer, XmlPatcher.EEvaluator.Client, delegate
		{
			coroutineHadException = true;
		});
		if (coroutineHadException)
		{
			yield break;
		}
		yield return ThreadManager.CoroutineWrapperWithExceptionCallback(_loadInfo.LoadMethod(xmlFile), delegate(Exception _exception)
		{
			Log.Error("XML loader: Loading and parsing '" + xmlFile.Filename + "' failed");
			Log.Exception(_exception);
			coroutineHadException = true;
		});
		if (coroutineHadException)
		{
			yield break;
		}
		if (_loadInfo.ExecuteAfterLoad != null)
		{
			if (timer.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
			{
				yield return null;
				timer.ResetAndRestart();
			}
			yield return ThreadManager.CoroutineWrapperWithExceptionCallback(_loadInfo.ExecuteAfterLoad(), delegate(Exception _exception)
			{
				Log.Error("XML loader: Executing post load step on '" + xmlFile.Filename + "' failed");
				Log.Exception(_exception);
				coroutineHadException = true;
			});
			if (coroutineHadException)
			{
				yield break;
			}
		}
		xmlFile = null;
		Log.Out("Loaded (local): {0} in {1}", new object[]
		{
			_loadInfo.XmlName,
			((float)timer.ElapsedMicroseconds * 1E-06f).ToCultureInvariantString("f2")
		});
		yield break;
	}

	// Token: 0x06009B4F RID: 39759 RVA: 0x003ACABE File Offset: 0x003AACBE
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator DumpPatchedXml(WorldStaticData.XmlLoadInfo _loadInfo, XmlFile _xmlFile, MicroStopwatch _timer)
	{
		if (!ThreadManager.IsMainThread() || !Application.isPlaying)
		{
			yield break;
		}
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			yield break;
		}
		string outputFileName = GameIO.GetSaveGameDir() + "/ConfigsDump/" + _loadInfo.XmlName + ".xml";
		if (GameIO.IsRoamingUserDataPath(outputFileName))
		{
			yield break;
		}
		if (_timer.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
		{
			yield return null;
			_timer.ResetAndRestart();
		}
		string directoryName = global::System.IO.Path.GetDirectoryName(outputFileName);
		if (!SdDirectory.Exists(directoryName))
		{
			SdDirectory.CreateDirectory(directoryName);
		}
		_xmlFile.SerializeToFile(outputFileName, false, null);
		yield break;
	}

	// Token: 0x06009B50 RID: 39760 RVA: 0x003ACADB File Offset: 0x003AACDB
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator CachePatchedXml(WorldStaticData.XmlLoadInfo _loadInfo, XmlFile _xmlFile, MicroStopwatch _timer, MemoryStream _memStream, DeflateOutputStream _zipStream, Action _errorCallback)
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			yield break;
		}
		if (_timer.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
		{
			yield return null;
			_timer.ResetAndRestart();
		}
		bool coroutineHadException = false;
		yield return ThreadManager.CoroutineWrapperWithExceptionCallback(WorldStaticData.cacheSingleXml(_loadInfo, _xmlFile, _memStream, _zipStream), delegate(Exception _exception)
		{
			Log.Error("XML loader: Compressing XML data for '" + _xmlFile.Filename + "' failed");
			Log.Exception(_exception);
			coroutineHadException = true;
		});
		if (coroutineHadException)
		{
			if (_errorCallback != null)
			{
				_errorCallback();
			}
		}
		yield break;
	}

	// Token: 0x06009B51 RID: 39761 RVA: 0x003ACB0F File Offset: 0x003AAD0F
	[Conditional("UNITY_GAMECORE")]
	[Conditional("UNITY_XBOXONE")]
	[Conditional("UNITY_PS4")]
	[Conditional("UNITY_PS5")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CollectGarbage()
	{
		GC.Collect();
	}

	// Token: 0x06009B52 RID: 39762 RVA: 0x003ACB16 File Offset: 0x003AAD16
	public static IEnumerator LoadAllXmlsCo(bool _isStartup, WorldStaticData.ProgressDelegate _progressDelegate, string[] _onlyXmls = null)
	{
		WorldStaticData.LoadAllXmlsCoComplete = false;
		MemoryStream memStream = new MemoryStream();
		DeflateOutputStream zipStream = new DeflateOutputStream(memStream, 3);
		foreach (WorldStaticData.XmlLoadInfo xmlLoadInfo in WorldStaticData.xmlsToLoad)
		{
			if (_onlyXmls == null || _onlyXmls.ContainsCaseInsensitive(xmlLoadInfo.XmlName))
			{
				if (!xmlLoadInfo.XmlFileExists())
				{
					if (!xmlLoadInfo.IgnoreMissingFile)
					{
						Log.Error("XML loader: XML is missing: " + xmlLoadInfo.XmlName);
					}
				}
				else if (!_isStartup || xmlLoadInfo.LoadAtStartup)
				{
					if (_progressDelegate != null && xmlLoadInfo.LoadStepLocalizationKey != null)
					{
						_progressDelegate(Localization.Get(xmlLoadInfo.LoadStepLocalizationKey, false, null), 0f);
					}
					yield return WorldStaticData.loadSingleXml(xmlLoadInfo, memStream, zipStream);
				}
			}
		}
		WorldStaticData.XmlLoadInfo[] array = null;
		WorldStaticData.LoadAllXmlsCoComplete = true;
		yield break;
	}

	// Token: 0x06009B53 RID: 39763 RVA: 0x003ACB33 File Offset: 0x003AAD33
	public static void ReloadAllXmlsSync()
	{
		WorldStaticData.Cleanup(null);
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadAllXmlsCo(false, null, null));
	}

	// Token: 0x06009B54 RID: 39764 RVA: 0x003ACB48 File Offset: 0x003AAD48
	public static void SendXmlsToClient(ClientInfo _cInfo)
	{
		foreach (WorldStaticData.XmlLoadInfo xmlLoadInfo in WorldStaticData.xmlsToLoad)
		{
			if (xmlLoadInfo.SendToClients && (xmlLoadInfo.LoadClientFile || xmlLoadInfo.CompressedXmlData != null))
			{
				_cInfo.SendPackage(NetPackageManager.GetPackage<NetPackageConfigFile>().Setup(xmlLoadInfo.XmlName, xmlLoadInfo.LoadClientFile ? null : xmlLoadInfo.CompressedXmlData));
			}
		}
	}

	// Token: 0x06009B55 RID: 39765 RVA: 0x003ACBAC File Offset: 0x003AADAC
	public static void SaveXmlsToFolder(string _exportPath)
	{
		foreach (WorldStaticData.XmlLoadInfo xmlLoadInfo in WorldStaticData.xmlsToLoad)
		{
			if (xmlLoadInfo.CompressedXmlData != null)
			{
				byte[] array2;
				using (MemoryStream memoryStream = new MemoryStream(xmlLoadInfo.CompressedXmlData))
				{
					using (DeflateInputStream deflateInputStream = new DeflateInputStream(memoryStream))
					{
						using (MemoryStream memoryStream2 = new MemoryStream())
						{
							StreamUtils.StreamCopy(deflateInputStream, memoryStream2, null, true);
							array2 = memoryStream2.ToArray();
						}
					}
				}
				string text = _exportPath + "/" + xmlLoadInfo.XmlName + ".xml";
				if (xmlLoadInfo.XmlName.IndexOf('/') >= 0)
				{
					string directoryName = global::System.IO.Path.GetDirectoryName(text);
					if (!SdDirectory.Exists(directoryName))
					{
						SdDirectory.CreateDirectory(directoryName);
					}
				}
				SdFile.WriteAllBytes(text, array2);
			}
		}
	}

	// Token: 0x06009B56 RID: 39766 RVA: 0x003ACCAC File Offset: 0x003AAEAC
	[PublicizedFrom(EAccessModifier.Private)]
	public static WorldStaticData.XmlLoadInfo getLoadInfoForName(string _xmlName, out int _arrayIndex)
	{
		_arrayIndex = -1;
		for (int i = 0; i < WorldStaticData.xmlsToLoad.Length; i++)
		{
			WorldStaticData.XmlLoadInfo xmlLoadInfo = WorldStaticData.xmlsToLoad[i];
			if (xmlLoadInfo.XmlName.EqualsCaseInsensitive(_xmlName))
			{
				_arrayIndex = i;
				return xmlLoadInfo;
			}
		}
		return null;
	}

	// Token: 0x06009B57 RID: 39767 RVA: 0x003ACCEA File Offset: 0x003AAEEA
	public static void WaitForConfigsFromServer()
	{
		if (WorldStaticData.receivedConfigsHandlerCoroutine != null)
		{
			ThreadManager.StopCoroutine(WorldStaticData.receivedConfigsHandlerCoroutine);
		}
		WorldStaticData.receivedConfigsHandlerCoroutine = ThreadManager.StartCoroutine(WorldStaticData.handleReceivedConfigs());
	}

	// Token: 0x06009B58 RID: 39768 RVA: 0x003ACD0C File Offset: 0x003AAF0C
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator handleReceivedConfigs()
	{
		MicroStopwatch timer = new MicroStopwatch(true);
		WorldStaticData.highestReceivedIndex = -1;
		WorldStaticData.XmlLoadInfo[] array = WorldStaticData.xmlsToLoad;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].WasReceivedFromServer = WorldStaticData.EClientFileState.None;
		}
		while (string.IsNullOrEmpty(GamePrefs.GetString(EnumGamePrefs.GameWorld)))
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
			{
				WorldStaticData.receivedConfigsHandlerCoroutine = null;
				yield break;
			}
			yield return null;
		}
		int waitingFor = 0;
		while (waitingFor < WorldStaticData.xmlsToLoad.Length)
		{
			WorldStaticData.<>c__DisplayClass63_0 CS$<>8__locals1 = new WorldStaticData.<>c__DisplayClass63_0();
			CS$<>8__locals1.loadInfo = WorldStaticData.xmlsToLoad[waitingFor];
			if (!CS$<>8__locals1.loadInfo.SendToClients)
			{
				int i = waitingFor;
				waitingFor = i + 1;
			}
			else if (CS$<>8__locals1.loadInfo.WasReceivedFromServer == WorldStaticData.EClientFileState.None)
			{
				if (CS$<>8__locals1.loadInfo.IgnoreMissingFile && WorldStaticData.highestReceivedIndex > waitingFor)
				{
					int i = waitingFor;
					waitingFor = i + 1;
				}
				else
				{
					yield return null;
				}
			}
			else
			{
				int i = waitingFor;
				waitingFor = i + 1;
				WorldStaticData.Cleanup(CS$<>8__locals1.loadInfo.XmlName);
				if (CS$<>8__locals1.loadInfo.WasReceivedFromServer == WorldStaticData.EClientFileState.LoadLocal)
				{
					yield return WorldStaticData.loadSingleXml(CS$<>8__locals1.loadInfo, null, null);
				}
				else
				{
					XmlFile xmlFile;
					using (MemoryStream memoryStream = new MemoryStream(CS$<>8__locals1.loadInfo.CompressedXmlData))
					{
						using (DeflateInputStream deflateInputStream = new DeflateInputStream(memoryStream))
						{
							xmlFile = new XmlFile(deflateInputStream);
						}
					}
					yield return null;
					CS$<>8__locals1.coroutineHadException = false;
					yield return XmlPatcher.ApplyConditionalXmlBlocks(CS$<>8__locals1.loadInfo.XmlName, xmlFile, timer, XmlPatcher.EEvaluator.Client, delegate
					{
						CS$<>8__locals1.coroutineHadException = true;
					});
					if (CS$<>8__locals1.coroutineHadException)
					{
						continue;
					}
					yield return ThreadManager.CoroutineWrapperWithExceptionCallback(CS$<>8__locals1.loadInfo.LoadMethod(xmlFile), delegate(Exception _exception)
					{
						Log.Error("XML loader: Loading and parsing '" + CS$<>8__locals1.loadInfo.XmlName + "' failed");
						Log.Exception(_exception);
						CS$<>8__locals1.coroutineHadException = true;
					});
					if (CS$<>8__locals1.coroutineHadException)
					{
						continue;
					}
					if (CS$<>8__locals1.loadInfo.ExecuteAfterLoad != null)
					{
						yield return null;
						yield return ThreadManager.CoroutineWrapperWithExceptionCallback(CS$<>8__locals1.loadInfo.ExecuteAfterLoad(), delegate(Exception _exception)
						{
							Log.Error("XML loader: Executing post load step on '" + CS$<>8__locals1.loadInfo.XmlName + "' failed");
							Log.Exception(_exception);
							CS$<>8__locals1.coroutineHadException = true;
						});
						if (CS$<>8__locals1.coroutineHadException)
						{
							continue;
						}
					}
					Log.Out("Loaded (received): " + CS$<>8__locals1.loadInfo.XmlName);
					yield return null;
					xmlFile = null;
				}
				CS$<>8__locals1 = null;
			}
		}
		WorldStaticData.receivedConfigsHandlerCoroutine = null;
		yield break;
	}

	// Token: 0x06009B59 RID: 39769 RVA: 0x003ACD14 File Offset: 0x003AAF14
	public static bool AllConfigsReceivedAndLoaded()
	{
		return WorldStaticData.receivedConfigsHandlerCoroutine == null;
	}

	// Token: 0x06009B5A RID: 39770 RVA: 0x003ACD20 File Offset: 0x003AAF20
	public static void ReceivedConfigFile(string _name, byte[] _data)
	{
		if (_data != null)
		{
			Log.Out(string.Format("Received config file '{0}' from server. Len: {1}", _name, _data.Length));
		}
		else
		{
			Log.Out("Loading config '" + _name + "' from local files");
		}
		int num;
		WorldStaticData.XmlLoadInfo loadInfoForName = WorldStaticData.getLoadInfoForName(_name, out num);
		if (loadInfoForName == null)
		{
			Log.Warning("XML loader: Received unknown config from server: " + _name);
			return;
		}
		loadInfoForName.CompressedXmlData = _data;
		loadInfoForName.WasReceivedFromServer = ((_data != null) ? WorldStaticData.EClientFileState.Received : WorldStaticData.EClientFileState.LoadLocal);
		WorldStaticData.highestReceivedIndex = MathUtils.Max(WorldStaticData.highestReceivedIndex, num);
	}

	// Token: 0x06009B5B RID: 39771 RVA: 0x003ACDA0 File Offset: 0x003AAFA0
	public static void UnloadMeshDescriptionTextureArrays(int _meshIndex)
	{
		WorldStaticData.meshDescCol.UnloadTextureArrays(_meshIndex);
	}

	// Token: 0x06009B5C RID: 39772 RVA: 0x003ACDAD File Offset: 0x003AAFAD
	public static void ReloadTextureArrays()
	{
		ThreadManager.RunCoroutineSync(WorldStaticData.meshDescCol.LoadTextureArrays(true));
	}

	// Token: 0x06009B5D RID: 39773 RVA: 0x003ACDC0 File Offset: 0x003AAFC0
	public static void ReloadInGameXML()
	{
		MicroStopwatch microStopwatch = new MicroStopwatch(true);
		foreach (WorldStaticData.XmlLoadInfo xmlLoadInfo in WorldStaticData.xmlsToLoad)
		{
			if (xmlLoadInfo.AllowReloadDuringGame)
			{
				Log.Out("-- Reloading {0} --\n", new object[] { xmlLoadInfo.XmlName });
				XmlFile xmlFile = new XmlFile(xmlLoadInfo.XmlName);
				if (xmlLoadInfo.ReloadDuringGameMethod != null)
				{
					xmlLoadInfo.ReloadDuringGameMethod(xmlFile);
				}
				else
				{
					ThreadManager.RunCoroutineSync(xmlLoadInfo.LoadMethod(xmlFile));
				}
			}
		}
		World world = GameManager.Instance.World;
		if (world != null)
		{
			List<Entity> list = world.Entities.list;
			for (int j = 0; j < list.Count; j++)
			{
				list[j].OnXMLChanged();
			}
		}
		Log.Out("-- ReloadInGameXML {0} seconds --\n", new object[] { ((float)microStopwatch.ElapsedMicroseconds * 1E-06f).ToCultureInvariantString() });
	}

	// Token: 0x040074ED RID: 29933
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cExplosionPrefabMax = 100;

	// Token: 0x040074EE RID: 29934
	public static bool LoadAllXmlsCoComplete;

	// Token: 0x040074EF RID: 29935
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly WorldStaticData.XmlLoadInfo[] xmlsToLoad = new WorldStaticData.XmlLoadInfo[]
	{
		new WorldStaticData.XmlLoadInfo("events", true, true, new Func<XmlFile, IEnumerator>(EventsFromXml.Load), new Action(EventsFromXml.Cleanup), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("materials", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadMaterials), new Action(MaterialBlock.Cleanup), new Func<IEnumerator>(WorldStaticData.LoadTextureAtlases), false, null, false, "loadActionMaterials"),
		new WorldStaticData.XmlLoadInfo("physicsbodies", false, true, new Func<XmlFile, IEnumerator>(PhysicsBodiesFromXml.Load), new Action(PhysicsBodyLayout.Reset), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("painting", false, true, new Func<XmlFile, IEnumerator>(BlockTexturesFromXML.CreateBlockTextures), new Action(BlockTextureData.Cleanup), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("shapes", false, true, new Func<XmlFile, IEnumerator>(ShapesFromXml.LoadShapes), null, null, true, null, false, null),
		new WorldStaticData.XmlLoadInfo("blocks", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadBlocks), new Action(WorldStaticData.CleanupBlocks), null, true, null, false, "loadActionBlocks"),
		new WorldStaticData.XmlLoadInfo("progression", false, true, new Func<XmlFile, IEnumerator>(ProgressionFromXml.Load), new Action(Progression.Cleanup), null, true, null, false, null),
		new WorldStaticData.XmlLoadInfo("buffs", false, true, new Func<XmlFile, IEnumerator>(BuffsFromXml.CreateBuffs), new Action(BuffManager.Cleanup), null, true, new Action<XmlFile>(BuffsFromXml.Reload), false, null),
		new WorldStaticData.XmlLoadInfo("misc", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadMisc), new Action(AnimationDelayData.Cleanup), null, true, new Action<XmlFile>(WorldStaticData.ReloadMisc), false, null),
		new WorldStaticData.XmlLoadInfo("toolbelt", false, true, new Func<XmlFile, IEnumerator>(ToolbeltFromXml.Load), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("items", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadItems), new Action(ItemClass.Cleanup), null, true, new Action<XmlFile>(WorldStaticData.ReloadItems), false, "loadActionItems"),
		new WorldStaticData.XmlLoadInfo("item_modifiers", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadItemModifiers), null, new Func<IEnumerator>(WorldStaticData.LateInitItems), true, new Action<XmlFile>(WorldStaticData.ReloadItemModifiers), false, null),
		new WorldStaticData.XmlLoadInfo("entityclasses", false, true, new Func<XmlFile, IEnumerator>(EntityClassesFromXml.LoadMain), new Action(EntityClass.Cleanup), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("qualityinfo", false, true, new Func<XmlFile, IEnumerator>(QualityInfoFromXml.CreateQualityInfo), new Action(QualityInfo.Cleanup), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("sounds", false, true, new Func<XmlFile, IEnumerator>(SoundsFromXml.CreateSounds), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("recipes", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadRecipes), new Action(CraftingManager.ClearAllRecipes), null, true, new Action<XmlFile>(WorldStaticData.ReloadRecipes), false, null),
		new WorldStaticData.XmlLoadInfo("blockplaceholders", false, true, new Func<XmlFile, IEnumerator>(BlockPlaceholdersFromXml.Load), new Action(BlockPlaceholderMap.Cleanup), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("loot", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadLoot), new Action(LootContainer.Cleanup), null, true, new Action<XmlFile>(WorldStaticData.ReloadLoot), false, null),
		new WorldStaticData.XmlLoadInfo("entitygroups", false, true, new Func<XmlFile, IEnumerator>(EntityGroupsFromXml.LoadEntityGroups), new Action(EntityGroups.Cleanup), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("utilityai", false, true, new Func<XmlFile, IEnumerator>(UAIFromXml.Load), new Action(UAIFromXml.Cleanup), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("vehicles", false, true, new Func<XmlFile, IEnumerator>(VehiclesFromXml.Load), new Action(Vehicle.Cleanup), null, true, new Action<XmlFile>(VehiclesFromXml.Reload), false, null),
		new WorldStaticData.XmlLoadInfo("rwgmixer", true, false, new Func<XmlFile, IEnumerator>(WorldGenerationFromXml.Load), new Action(WorldGenerationFromXml.Cleanup), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("weathersurvival", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadWeather), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("archetypes", true, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadSDCSArchetypes), null, null, false, null, true, null),
		new WorldStaticData.XmlLoadInfo("challenges", false, true, new Func<XmlFile, IEnumerator>(ChallengesFromXml.CreateChallenges), new Action(WorldStaticData.CleanupChallenges), null, true, null, false, null),
		new WorldStaticData.XmlLoadInfo("quests", false, true, new Func<XmlFile, IEnumerator>(QuestsFromXml.CreateQuests), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("traders", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadTraders), new Action(TraderInfo.Cleanup), null, true, null, false, null),
		new WorldStaticData.XmlLoadInfo("npc", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadNpc), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("dialogs", false, true, new Func<XmlFile, IEnumerator>(DialogFromXml.Load), new Action(Dialog.Cleanup), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("ui_display", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadUIDisplayInfo), new Action(UIDisplayInfoManager.Reset), null, true, null, false, null),
		new WorldStaticData.XmlLoadInfo("nav_objects", false, true, new Func<XmlFile, IEnumerator>(NavObjectClassesFromXml.Load), new Action(NavObjectClass.Reset), null, true, null, false, null),
		new WorldStaticData.XmlLoadInfo("gamestages", false, false, new Func<XmlFile, IEnumerator>(GameStagesFromXml.Load), new Action(WorldStaticData.CleanupGamestages), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("gameevents", false, true, new Func<XmlFile, IEnumerator>(GameEventsFromXml.CreateGameEvents), new Action(WorldStaticData.CleanupGameEvents), null, true, null, false, null),
		new WorldStaticData.XmlLoadInfo("twitch", false, true, new Func<XmlFile, IEnumerator>(TwitchActionsFromXml.CreateTwitchActions), new Action(WorldStaticData.CleanupTwitch), null, true, null, false, null),
		new WorldStaticData.XmlLoadInfo("twitch_events", false, true, new Func<XmlFile, IEnumerator>(TwitchActionsFromXml.CreateTwitchEvents), new Action(WorldStaticData.CleanupTwitchEvents), null, true, null, false, null)
		{
			LoadClientFile = true
		},
		new WorldStaticData.XmlLoadInfo("dmscontent", false, true, new Func<XmlFile, IEnumerator>(DMSContentFromXml.Load), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("XUi_Common/styles", false, true, new Func<XmlFile, IEnumerator>(XUiFromXml.Load), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("XUi_Common/templates", false, true, new Func<XmlFile, IEnumerator>(XUiFromXml.Load), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("XUi_InGame/styles", false, true, new Func<XmlFile, IEnumerator>(XUiFromXml.Load), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("XUi_InGame/templates", false, true, new Func<XmlFile, IEnumerator>(XUiFromXml.Load), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("XUi_InGame/windows", false, true, new Func<XmlFile, IEnumerator>(XUiFromXml.Load), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("XUi_InGame/xui", false, true, new Func<XmlFile, IEnumerator>(XUiFromXml.Load), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("biomes", false, true, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadBiomes), new Action(WorldBiomes.CleanupStatic), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("worldglobal", false, true, new Func<XmlFile, IEnumerator>(WorldGlobalFromXml.Load), null, null, true, new Action<XmlFile>(WorldGlobalFromXml.Reload), false, null),
		new WorldStaticData.XmlLoadInfo("spawning", false, false, new Func<XmlFile, IEnumerator>(WorldStaticData.LoadSpawning), new Action(WorldStaticData.CleanupSpawning), null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("loadingscreen", true, false, new Func<XmlFile, IEnumerator>(XUiC_LoadingScreen.LoadXml), null, null, true, null, false, null),
		new WorldStaticData.XmlLoadInfo("subtitles", true, false, new Func<XmlFile, IEnumerator>(SoundsFromXml.LoadSubtitleXML), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("videos", true, false, new Func<XmlFile, IEnumerator>(VideoFromXML.CreateVideos), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("signs", false, false, new Func<XmlFile, IEnumerator>(SignDataManager.LoadDefaultLibrary), null, null, false, null, false, null),
		new WorldStaticData.XmlLoadInfo("sandbox_overrides", true, true, new Func<XmlFile, IEnumerator>(SandboxOverridesFromXml.CreateOverrides), null, null, false, null, false, null)
	};

	// Token: 0x040074F0 RID: 29936
	public static Transform[] prefabExplosions;

	// Token: 0x040074F1 RID: 29937
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool bInitDone;

	// Token: 0x040074F2 RID: 29938
	[PublicizedFrom(EAccessModifier.Private)]
	public static MeshDescriptionCollection meshDescCol;

	// Token: 0x040074F3 RID: 29939
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isDediServer;

	// Token: 0x040074F4 RID: 29940
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine receivedConfigsHandlerCoroutine;

	// Token: 0x040074F5 RID: 29941
	[PublicizedFrom(EAccessModifier.Private)]
	public static int highestReceivedIndex = -1;

	// Token: 0x02001339 RID: 4921
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EClientFileState
	{
		// Token: 0x040074F7 RID: 29943
		None,
		// Token: 0x040074F8 RID: 29944
		Received,
		// Token: 0x040074F9 RID: 29945
		LoadLocal
	}

	// Token: 0x0200133A RID: 4922
	[PublicizedFrom(EAccessModifier.Private)]
	public class XmlLoadInfo
	{
		// Token: 0x06009B5F RID: 39775 RVA: 0x003AD72A File Offset: 0x003AB92A
		public bool XmlFileExists()
		{
			return SdFile.Exists(GameIO.GetGameDir("Data/Config") + "/" + this.XmlName + ".xml");
		}

		// Token: 0x06009B60 RID: 39776 RVA: 0x003AD750 File Offset: 0x003AB950
		public XmlLoadInfo(string _xmlName, bool _loadAtStartup, bool _sendToClients, Func<XmlFile, IEnumerator> _loadMethod, Action _cleanupMethod, Func<IEnumerator> _executeAfterLoad = null, bool _allowReloadDuringGame = false, Action<XmlFile> _reloadDuringGameMethod = null, bool _ignoreMissingFile = false, string _loadStepLocalizationKey = null)
		{
			this.XmlName = _xmlName;
			this.LoadStepLocalizationKey = _loadStepLocalizationKey;
			this.LoadAtStartup = _loadAtStartup;
			this.SendToClients = _sendToClients;
			this.IgnoreMissingFile = _ignoreMissingFile;
			this.AllowReloadDuringGame = _allowReloadDuringGame;
			this.LoadMethod = _loadMethod;
			this.CleanupMethod = _cleanupMethod;
			this.ExecuteAfterLoad = _executeAfterLoad;
			this.ReloadDuringGameMethod = _reloadDuringGameMethod;
			if (this.LoadMethod == null)
			{
				throw new ArgumentNullException("_loadMethod");
			}
		}

		// Token: 0x040074FA RID: 29946
		public readonly string XmlName;

		// Token: 0x040074FB RID: 29947
		public readonly string LoadStepLocalizationKey;

		// Token: 0x040074FC RID: 29948
		public readonly bool LoadAtStartup;

		// Token: 0x040074FD RID: 29949
		public readonly bool SendToClients;

		// Token: 0x040074FE RID: 29950
		public readonly bool IgnoreMissingFile;

		// Token: 0x040074FF RID: 29951
		public readonly bool AllowReloadDuringGame;

		// Token: 0x04007500 RID: 29952
		public readonly Func<XmlFile, IEnumerator> LoadMethod;

		// Token: 0x04007501 RID: 29953
		public readonly Action CleanupMethod;

		// Token: 0x04007502 RID: 29954
		public readonly Func<IEnumerator> ExecuteAfterLoad;

		// Token: 0x04007503 RID: 29955
		public readonly Action<XmlFile> ReloadDuringGameMethod;

		// Token: 0x04007504 RID: 29956
		public byte[] CompressedXmlData;

		// Token: 0x04007505 RID: 29957
		public bool LoadClientFile;

		// Token: 0x04007506 RID: 29958
		public WorldStaticData.EClientFileState WasReceivedFromServer;
	}

	// Token: 0x0200133B RID: 4923
	// (Invoke) Token: 0x06009B62 RID: 39778
	public delegate void ProgressDelegate(string _progressText, float _percentage);
}
