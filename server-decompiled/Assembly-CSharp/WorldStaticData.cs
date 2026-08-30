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

// Token: 0x02001306 RID: 4870
public static class WorldStaticData
{
	// Token: 0x06009A21 RID: 39457 RVA: 0x003A577C File Offset: 0x003A397C
	public static void InitSync(bool _bForce, bool _bDediServer, bool _cleanup = true, string[] _onlyXmls = null)
	{
		if (_cleanup)
		{
			WorldStaticData.Cleanup();
		}
		LoadManager.InitSync();
		ThreadManager.RunCoroutineSync(WorldStaticData.Init(_bForce, _bDediServer, null, _onlyXmls));
	}

	// Token: 0x06009A22 RID: 39458 RVA: 0x003A5799 File Offset: 0x003A3999
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

	// Token: 0x06009A23 RID: 39459 RVA: 0x003A57BD File Offset: 0x003A39BD
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

	// Token: 0x06009A24 RID: 39460 RVA: 0x003A57C5 File Offset: 0x003A39C5
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadMaterials(XmlFile _xmlFile)
	{
		AIDirectorData.InitStatic();
		yield return MaterialsFromXml.CreateMaterials(_xmlFile);
		yield break;
	}

	// Token: 0x06009A25 RID: 39461 RVA: 0x003A57D4 File Offset: 0x003A39D4
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadBlocks(XmlFile _xmlFile)
	{
		Block.InitStatic();
		yield return BlocksFromXml.CreateBlocks(_xmlFile, false, GameManager.Instance != null && GameManager.Instance.IsEditMode());
		Block.AssignIds();
		yield break;
	}

	// Token: 0x06009A26 RID: 39462 RVA: 0x003A57E3 File Offset: 0x003A39E3
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadMisc(XmlFile _xmlFile)
	{
		AnimationDelayData.InitStatic();
		AnimationGunjointOffsetData.InitStatic();
		yield return MiscFromXml.Create(_xmlFile);
		yield break;
	}

	// Token: 0x06009A27 RID: 39463 RVA: 0x003A57F2 File Offset: 0x003A39F2
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadMisc(XmlFile _xmlFile)
	{
		AnimationDelayData.InitStatic();
		AnimationGunjointOffsetData.InitStatic();
		ThreadManager.RunCoroutineSync(MiscFromXml.Create(_xmlFile));
	}

	// Token: 0x06009A28 RID: 39464 RVA: 0x003A5809 File Offset: 0x003A3A09
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadItems(XmlFile _xmlFile)
	{
		ItemClass.Cleanup();
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadItems(_xmlFile));
	}

	// Token: 0x06009A29 RID: 39465 RVA: 0x003A581B File Offset: 0x003A3A1B
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadItemsAppend(XmlFile _xmlFile)
	{
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadItems(_xmlFile));
	}

	// Token: 0x06009A2A RID: 39466 RVA: 0x003A5828 File Offset: 0x003A3A28
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadItemModifiers(XmlFile _xmlFile)
	{
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadItemModifiers(_xmlFile));
		ThreadManager.RunCoroutineSync(WorldStaticData.LateInitItems());
		if (GameManager.Instance != null && GameManager.Instance.World != null && GameManager.Instance.World.GetPrimaryPlayer() != null)
		{
			GameManager.Instance.World.GetPrimaryPlayer().inventory.ForceHoldingItemUpdate();
		}
	}

	// Token: 0x06009A2B RID: 39467 RVA: 0x003A5893 File Offset: 0x003A3A93
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

	// Token: 0x06009A2C RID: 39468 RVA: 0x003A58A2 File Offset: 0x003A3AA2
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadItemsAppend(XmlFile _xmlFile)
	{
		yield return ItemClassesFromXml.CreateItems(_xmlFile);
		ItemData.Parser.idOffset = Block.ItemsStartHere;
		yield break;
	}

	// Token: 0x06009A2D RID: 39469 RVA: 0x003A58B1 File Offset: 0x003A3AB1
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LateInitItems()
	{
		ItemClass.AssignIds();
		Block.LateInitAll();
		ItemClass.LateInitAll();
		yield break;
	}

	// Token: 0x06009A2E RID: 39470 RVA: 0x003A58B9 File Offset: 0x003A3AB9
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadItemModifiers(XmlFile _xmlFile)
	{
		yield return ItemModificationsFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009A2F RID: 39471 RVA: 0x003A58C8 File Offset: 0x003A3AC8
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

	// Token: 0x06009A30 RID: 39472 RVA: 0x003A58D7 File Offset: 0x003A3AD7
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadRecipes(XmlFile _xmlFile)
	{
		CraftingManager.ClearAllRecipes();
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadRecipes(_xmlFile));
	}

	// Token: 0x06009A31 RID: 39473 RVA: 0x003A58E9 File Offset: 0x003A3AE9
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadLoot(XmlFile _xmlFile)
	{
		LootContainer.InitStatic();
		yield return LootFromXml.LoadLootContainers(_xmlFile);
		yield break;
	}

	// Token: 0x06009A32 RID: 39474 RVA: 0x003A58F8 File Offset: 0x003A3AF8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadLoot(XmlFile _xmlFile)
	{
		LootContainer.Cleanup();
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadLoot(_xmlFile));
	}

	// Token: 0x06009A33 RID: 39475 RVA: 0x003A590A File Offset: 0x003A3B0A
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadWeather(XmlFile _xmlFile)
	{
		WeatherSurvivalParametersFromXml.Load(_xmlFile);
		WorldStaticData.LinkBuffs();
		yield break;
	}

	// Token: 0x06009A34 RID: 39476 RVA: 0x003A5919 File Offset: 0x003A3B19
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadSDCSArchetypes(XmlFile _xmlFile)
	{
		SDCSArchetypesFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009A35 RID: 39477 RVA: 0x003A5928 File Offset: 0x003A3B28
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadTraders(XmlFile _xmlFile)
	{
		TraderInfo.InitStatic();
		yield return TradersFromXml.LoadTraderInfo(_xmlFile);
		yield break;
	}

	// Token: 0x06009A36 RID: 39478 RVA: 0x003A5937 File Offset: 0x003A3B37
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadNpc(XmlFile _xmlFile)
	{
		NPCInfo.InitStatic();
		yield return NPCsFromXml.LoadNPCInfo(_xmlFile);
		yield break;
	}

	// Token: 0x06009A37 RID: 39479 RVA: 0x003A5946 File Offset: 0x003A3B46
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadMusic(XmlFile _xmlFile)
	{
		MusicGroup.InitStatic();
		yield return MusicDataFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009A38 RID: 39480 RVA: 0x003A5955 File Offset: 0x003A3B55
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadUIDisplayInfo(XmlFile _xmlFile)
	{
		UIDisplayInfoManager.Reset();
		yield return UIDisplayInfoFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009A39 RID: 39481 RVA: 0x003A5964 File Offset: 0x003A3B64
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadBiomes(XmlFile _xmlFile)
	{
		new WorldBiomes(_xmlFile.XmlDoc, true);
		yield break;
	}

	// Token: 0x06009A3A RID: 39482 RVA: 0x003A5973 File Offset: 0x003A3B73
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadSpawning(XmlFile _xmlFile)
	{
		EntitySpawnerClassesFromXml.LoadEntitySpawnerClasses(_xmlFile.XmlDoc);
		yield return BiomeSpawningFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009A3B RID: 39483 RVA: 0x003A5982 File Offset: 0x003A3B82
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupBlocks()
	{
		AIDirectorData.Cleanup();
		Block.Cleanup();
		TileEntityCompositeData.Cleanup();
	}

	// Token: 0x06009A3C RID: 39484 RVA: 0x003A5993 File Offset: 0x003A3B93
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupGamestages()
	{
		GameStageDefinition.Clear();
		GameStageGroup.Clear();
	}

	// Token: 0x06009A3D RID: 39485 RVA: 0x003A599F File Offset: 0x003A3B9F
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupChallenges()
	{
		ChallengeClass.Cleanup();
	}

	// Token: 0x06009A3E RID: 39486 RVA: 0x003A59A6 File Offset: 0x003A3BA6
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupGameEvents()
	{
		if (GameEventManager.HasInstance)
		{
			GameEventManager.Current.Cleanup();
		}
	}

	// Token: 0x06009A3F RID: 39487 RVA: 0x003A59B9 File Offset: 0x003A3BB9
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupTwitch()
	{
		if (TwitchActionManager.HasInstance)
		{
			TwitchActionManager.Current.Cleanup();
		}
	}

	// Token: 0x06009A40 RID: 39488 RVA: 0x003A59CC File Offset: 0x003A3BCC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupTwitchEvents()
	{
		if (TwitchManager.HasInstance)
		{
			TwitchManager.Current.CleanupEventData();
		}
	}

	// Token: 0x06009A41 RID: 39489 RVA: 0x003A59DF File Offset: 0x003A3BDF
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupSpawning()
	{
		EntitySpawnerClass.Cleanup();
		BiomeSpawningClass.Cleanup();
	}

	// Token: 0x06009A42 RID: 39490 RVA: 0x003A59EB File Offset: 0x003A3BEB
	public static void LoadPhysicsBodies()
	{
		WorldStaticData.Reset("physicsbodies");
	}

	// Token: 0x06009A43 RID: 39491 RVA: 0x003A59F7 File Offset: 0x003A3BF7
	public static void SavePhysicsBodies()
	{
		PhysicsBodiesFromXml.Save(GameIO.GetGameDir("Data/Config") + "/physicsbodies.xml");
	}

	// Token: 0x06009A44 RID: 39492 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LinkBuffs()
	{
	}

	// Token: 0x06009A45 RID: 39493 RVA: 0x003A5A14 File Offset: 0x003A3C14
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

	// Token: 0x06009A46 RID: 39494 RVA: 0x003A5A60 File Offset: 0x003A3C60
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

	// Token: 0x06009A47 RID: 39495 RVA: 0x000027FC File Offset: 0x000009FC
	public static void QuitCleanup()
	{
	}

	// Token: 0x06009A48 RID: 39496 RVA: 0x003A5A94 File Offset: 0x003A3C94
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

	// Token: 0x06009A49 RID: 39497 RVA: 0x003A5B1D File Offset: 0x003A3D1D
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

	// Token: 0x06009A4A RID: 39498 RVA: 0x003A5B41 File Offset: 0x003A3D41
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

	// Token: 0x06009A4B RID: 39499 RVA: 0x003A5B5E File Offset: 0x003A3D5E
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

	// Token: 0x06009A4C RID: 39500 RVA: 0x003A5B7B File Offset: 0x003A3D7B
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

	// Token: 0x06009A4D RID: 39501 RVA: 0x003A5BAF File Offset: 0x003A3DAF
	[Conditional("UNITY_GAMECORE")]
	[Conditional("UNITY_XBOXONE")]
	[Conditional("UNITY_PS4")]
	[Conditional("UNITY_PS5")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CollectGarbage()
	{
		GC.Collect();
	}

	// Token: 0x06009A4E RID: 39502 RVA: 0x003A5BB6 File Offset: 0x003A3DB6
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

	// Token: 0x06009A4F RID: 39503 RVA: 0x003A5BD3 File Offset: 0x003A3DD3
	public static void ReloadAllXmlsSync()
	{
		WorldStaticData.Cleanup(null);
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadAllXmlsCo(false, null, null));
	}

	// Token: 0x06009A50 RID: 39504 RVA: 0x003A5BE8 File Offset: 0x003A3DE8
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

	// Token: 0x06009A51 RID: 39505 RVA: 0x003A5C4C File Offset: 0x003A3E4C
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

	// Token: 0x06009A52 RID: 39506 RVA: 0x003A5D4C File Offset: 0x003A3F4C
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

	// Token: 0x06009A53 RID: 39507 RVA: 0x003A5D8A File Offset: 0x003A3F8A
	public static void WaitForConfigsFromServer()
	{
		if (WorldStaticData.receivedConfigsHandlerCoroutine != null)
		{
			ThreadManager.StopCoroutine(WorldStaticData.receivedConfigsHandlerCoroutine);
		}
		WorldStaticData.receivedConfigsHandlerCoroutine = ThreadManager.StartCoroutine(WorldStaticData.handleReceivedConfigs());
	}

	// Token: 0x06009A54 RID: 39508 RVA: 0x003A5DAC File Offset: 0x003A3FAC
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

	// Token: 0x06009A55 RID: 39509 RVA: 0x003A5DB4 File Offset: 0x003A3FB4
	public static bool AllConfigsReceivedAndLoaded()
	{
		return WorldStaticData.receivedConfigsHandlerCoroutine == null;
	}

	// Token: 0x06009A56 RID: 39510 RVA: 0x003A5DC0 File Offset: 0x003A3FC0
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

	// Token: 0x06009A57 RID: 39511 RVA: 0x003A5E40 File Offset: 0x003A4040
	public static void UnloadMeshDescriptionTextureArrays(int _meshIndex)
	{
		WorldStaticData.meshDescCol.UnloadTextureArrays(_meshIndex);
	}

	// Token: 0x06009A58 RID: 39512 RVA: 0x003A5E4D File Offset: 0x003A404D
	public static void ReloadTextureArrays()
	{
		ThreadManager.RunCoroutineSync(WorldStaticData.meshDescCol.LoadTextureArrays(true));
	}

	// Token: 0x06009A59 RID: 39513 RVA: 0x003A5E60 File Offset: 0x003A4060
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

	// Token: 0x04007431 RID: 29745
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cExplosionPrefabMax = 100;

	// Token: 0x04007432 RID: 29746
	public static bool LoadAllXmlsCoComplete;

	// Token: 0x04007433 RID: 29747
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

	// Token: 0x04007434 RID: 29748
	public static Transform[] prefabExplosions;

	// Token: 0x04007435 RID: 29749
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool bInitDone;

	// Token: 0x04007436 RID: 29750
	[PublicizedFrom(EAccessModifier.Private)]
	public static MeshDescriptionCollection meshDescCol;

	// Token: 0x04007437 RID: 29751
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isDediServer;

	// Token: 0x04007438 RID: 29752
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine receivedConfigsHandlerCoroutine;

	// Token: 0x04007439 RID: 29753
	[PublicizedFrom(EAccessModifier.Private)]
	public static int highestReceivedIndex = -1;

	// Token: 0x02001307 RID: 4871
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EClientFileState
	{
		// Token: 0x0400743B RID: 29755
		None,
		// Token: 0x0400743C RID: 29756
		Received,
		// Token: 0x0400743D RID: 29757
		LoadLocal
	}

	// Token: 0x02001308 RID: 4872
	[PublicizedFrom(EAccessModifier.Private)]
	public class XmlLoadInfo
	{
		// Token: 0x06009A5B RID: 39515 RVA: 0x003A67A8 File Offset: 0x003A49A8
		public bool XmlFileExists()
		{
			return SdFile.Exists(GameIO.GetGameDir("Data/Config") + "/" + this.XmlName + ".xml");
		}

		// Token: 0x06009A5C RID: 39516 RVA: 0x003A67D0 File Offset: 0x003A49D0
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

		// Token: 0x0400743E RID: 29758
		public readonly string XmlName;

		// Token: 0x0400743F RID: 29759
		public readonly string LoadStepLocalizationKey;

		// Token: 0x04007440 RID: 29760
		public readonly bool LoadAtStartup;

		// Token: 0x04007441 RID: 29761
		public readonly bool SendToClients;

		// Token: 0x04007442 RID: 29762
		public readonly bool IgnoreMissingFile;

		// Token: 0x04007443 RID: 29763
		public readonly bool AllowReloadDuringGame;

		// Token: 0x04007444 RID: 29764
		public readonly Func<XmlFile, IEnumerator> LoadMethod;

		// Token: 0x04007445 RID: 29765
		public readonly Action CleanupMethod;

		// Token: 0x04007446 RID: 29766
		public readonly Func<IEnumerator> ExecuteAfterLoad;

		// Token: 0x04007447 RID: 29767
		public readonly Action<XmlFile> ReloadDuringGameMethod;

		// Token: 0x04007448 RID: 29768
		public byte[] CompressedXmlData;

		// Token: 0x04007449 RID: 29769
		public bool LoadClientFile;

		// Token: 0x0400744A RID: 29770
		public WorldStaticData.EClientFileState WasReceivedFromServer;
	}

	// Token: 0x02001309 RID: 4873
	// (Invoke) Token: 0x06009A5E RID: 39518
	public delegate void ProgressDelegate(string _progressText, float _percentage);
}
