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

// Token: 0x020012EC RID: 4844
public static class WorldStaticData
{
	// Token: 0x06009926 RID: 39206 RVA: 0x003A04E4 File Offset: 0x0039E6E4
	public static void InitSync(bool _bForce, bool _bDediServer, bool _cleanup = true, string[] _onlyXmls = null)
	{
		if (_cleanup)
		{
			WorldStaticData.Cleanup();
		}
		LoadManager.InitSync();
		ThreadManager.RunCoroutineSync(WorldStaticData.Init(_bForce, _bDediServer, null, _onlyXmls));
	}

	// Token: 0x06009927 RID: 39207 RVA: 0x003A0501 File Offset: 0x0039E701
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

	// Token: 0x06009928 RID: 39208 RVA: 0x003A0525 File Offset: 0x0039E725
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

	// Token: 0x06009929 RID: 39209 RVA: 0x003A052D File Offset: 0x0039E72D
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadMaterials(XmlFile _xmlFile)
	{
		AIDirectorData.InitStatic();
		yield return MaterialsFromXml.CreateMaterials(_xmlFile);
		yield break;
	}

	// Token: 0x0600992A RID: 39210 RVA: 0x003A053C File Offset: 0x0039E73C
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadBlocks(XmlFile _xmlFile)
	{
		Block.InitStatic();
		yield return BlocksFromXml.CreateBlocks(_xmlFile, false, GameManager.Instance != null && GameManager.Instance.IsEditMode());
		Block.AssignIds();
		yield break;
	}

	// Token: 0x0600992B RID: 39211 RVA: 0x003A054B File Offset: 0x0039E74B
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadMisc(XmlFile _xmlFile)
	{
		AnimationDelayData.InitStatic();
		AnimationGunjointOffsetData.InitStatic();
		yield return MiscFromXml.Create(_xmlFile);
		yield break;
	}

	// Token: 0x0600992C RID: 39212 RVA: 0x003A055A File Offset: 0x0039E75A
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadMisc(XmlFile _xmlFile)
	{
		AnimationDelayData.InitStatic();
		AnimationGunjointOffsetData.InitStatic();
		ThreadManager.RunCoroutineSync(MiscFromXml.Create(_xmlFile));
	}

	// Token: 0x0600992D RID: 39213 RVA: 0x003A0571 File Offset: 0x0039E771
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadItems(XmlFile _xmlFile)
	{
		ItemClass.Cleanup();
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadItems(_xmlFile));
	}

	// Token: 0x0600992E RID: 39214 RVA: 0x003A0583 File Offset: 0x0039E783
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadItemsAppend(XmlFile _xmlFile)
	{
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadItems(_xmlFile));
	}

	// Token: 0x0600992F RID: 39215 RVA: 0x003A0590 File Offset: 0x0039E790
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

	// Token: 0x06009930 RID: 39216 RVA: 0x003A05FB File Offset: 0x0039E7FB
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

	// Token: 0x06009931 RID: 39217 RVA: 0x003A060A File Offset: 0x0039E80A
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadItemsAppend(XmlFile _xmlFile)
	{
		yield return ItemClassesFromXml.CreateItems(_xmlFile);
		ItemData.Parser.idOffset = Block.ItemsStartHere;
		yield break;
	}

	// Token: 0x06009932 RID: 39218 RVA: 0x003A0619 File Offset: 0x0039E819
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LateInitItems()
	{
		ItemClass.AssignIds();
		Block.LateInitAll();
		ItemClass.LateInitAll();
		yield break;
	}

	// Token: 0x06009933 RID: 39219 RVA: 0x003A0621 File Offset: 0x0039E821
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadItemModifiers(XmlFile _xmlFile)
	{
		yield return ItemModificationsFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009934 RID: 39220 RVA: 0x003A0630 File Offset: 0x0039E830
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

	// Token: 0x06009935 RID: 39221 RVA: 0x003A063F File Offset: 0x0039E83F
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadRecipes(XmlFile _xmlFile)
	{
		CraftingManager.ClearAllRecipes();
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadRecipes(_xmlFile));
	}

	// Token: 0x06009936 RID: 39222 RVA: 0x003A0651 File Offset: 0x0039E851
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadLoot(XmlFile _xmlFile)
	{
		LootContainer.InitStatic();
		yield return LootFromXml.LoadLootContainers(_xmlFile);
		yield break;
	}

	// Token: 0x06009937 RID: 39223 RVA: 0x003A0660 File Offset: 0x0039E860
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReloadLoot(XmlFile _xmlFile)
	{
		LootContainer.Cleanup();
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadLoot(_xmlFile));
	}

	// Token: 0x06009938 RID: 39224 RVA: 0x003A0672 File Offset: 0x0039E872
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadWeather(XmlFile _xmlFile)
	{
		WeatherSurvivalParametersFromXml.Load(_xmlFile);
		WorldStaticData.LinkBuffs();
		yield break;
	}

	// Token: 0x06009939 RID: 39225 RVA: 0x003A0681 File Offset: 0x0039E881
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadSDCSArchetypes(XmlFile _xmlFile)
	{
		SDCSArchetypesFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x0600993A RID: 39226 RVA: 0x003A0690 File Offset: 0x0039E890
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadTraders(XmlFile _xmlFile)
	{
		TraderInfo.InitStatic();
		yield return TradersFromXml.LoadTraderInfo(_xmlFile);
		yield break;
	}

	// Token: 0x0600993B RID: 39227 RVA: 0x003A069F File Offset: 0x0039E89F
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadNpc(XmlFile _xmlFile)
	{
		NPCInfo.InitStatic();
		yield return NPCsFromXml.LoadNPCInfo(_xmlFile);
		yield break;
	}

	// Token: 0x0600993C RID: 39228 RVA: 0x003A06AE File Offset: 0x0039E8AE
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadMusic(XmlFile _xmlFile)
	{
		MusicGroup.InitStatic();
		yield return MusicDataFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x0600993D RID: 39229 RVA: 0x003A06BD File Offset: 0x0039E8BD
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadUIDisplayInfo(XmlFile _xmlFile)
	{
		UIDisplayInfoManager.Reset();
		yield return UIDisplayInfoFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x0600993E RID: 39230 RVA: 0x003A06CC File Offset: 0x0039E8CC
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadBiomes(XmlFile _xmlFile)
	{
		new WorldBiomes(_xmlFile.XmlDoc, true);
		yield break;
	}

	// Token: 0x0600993F RID: 39231 RVA: 0x003A06DB File Offset: 0x0039E8DB
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator LoadSpawning(XmlFile _xmlFile)
	{
		EntitySpawnerClassesFromXml.LoadEntitySpawnerClasses(_xmlFile.XmlDoc);
		yield return BiomeSpawningFromXml.Load(_xmlFile);
		yield break;
	}

	// Token: 0x06009940 RID: 39232 RVA: 0x003A06EA File Offset: 0x0039E8EA
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupBlocks()
	{
		AIDirectorData.Cleanup();
		Block.Cleanup();
		TileEntityCompositeData.Cleanup();
	}

	// Token: 0x06009941 RID: 39233 RVA: 0x003A06FB File Offset: 0x0039E8FB
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupGamestages()
	{
		GameStageDefinition.Clear();
		GameStageGroup.Clear();
	}

	// Token: 0x06009942 RID: 39234 RVA: 0x003A0707 File Offset: 0x0039E907
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupChallenges()
	{
		ChallengeClass.Cleanup();
	}

	// Token: 0x06009943 RID: 39235 RVA: 0x003A070E File Offset: 0x0039E90E
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupGameEvents()
	{
		if (GameEventManager.HasInstance)
		{
			GameEventManager.Current.Cleanup();
		}
	}

	// Token: 0x06009944 RID: 39236 RVA: 0x003A0721 File Offset: 0x0039E921
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupTwitch()
	{
		if (TwitchActionManager.HasInstance)
		{
			TwitchActionManager.Current.Cleanup();
		}
	}

	// Token: 0x06009945 RID: 39237 RVA: 0x003A0734 File Offset: 0x0039E934
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupTwitchEvents()
	{
		if (TwitchManager.HasInstance)
		{
			TwitchManager.Current.CleanupEventData();
		}
	}

	// Token: 0x06009946 RID: 39238 RVA: 0x003A0747 File Offset: 0x0039E947
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CleanupSpawning()
	{
		EntitySpawnerClass.Cleanup();
		BiomeSpawningClass.Cleanup();
	}

	// Token: 0x06009947 RID: 39239 RVA: 0x003A0753 File Offset: 0x0039E953
	public static void LoadPhysicsBodies()
	{
		WorldStaticData.Reset("physicsbodies");
	}

	// Token: 0x06009948 RID: 39240 RVA: 0x003A075F File Offset: 0x0039E95F
	public static void SavePhysicsBodies()
	{
		PhysicsBodiesFromXml.Save(GameIO.GetGameDir("Data/Config") + "/physicsbodies.xml");
	}

	// Token: 0x06009949 RID: 39241 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LinkBuffs()
	{
	}

	// Token: 0x0600994A RID: 39242 RVA: 0x003A077C File Offset: 0x0039E97C
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

	// Token: 0x0600994B RID: 39243 RVA: 0x003A07C8 File Offset: 0x0039E9C8
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

	// Token: 0x0600994C RID: 39244 RVA: 0x000027FC File Offset: 0x000009FC
	public static void QuitCleanup()
	{
	}

	// Token: 0x0600994D RID: 39245 RVA: 0x003A07FC File Offset: 0x0039E9FC
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

	// Token: 0x0600994E RID: 39246 RVA: 0x003A0885 File Offset: 0x0039EA85
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

	// Token: 0x0600994F RID: 39247 RVA: 0x003A08A9 File Offset: 0x0039EAA9
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

	// Token: 0x06009950 RID: 39248 RVA: 0x003A08C6 File Offset: 0x0039EAC6
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

	// Token: 0x06009951 RID: 39249 RVA: 0x003A08E3 File Offset: 0x0039EAE3
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

	// Token: 0x06009952 RID: 39250 RVA: 0x003A0917 File Offset: 0x0039EB17
	[Conditional("UNITY_GAMECORE")]
	[Conditional("UNITY_XBOXONE")]
	[Conditional("UNITY_PS4")]
	[Conditional("UNITY_PS5")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CollectGarbage()
	{
		GC.Collect();
	}

	// Token: 0x06009953 RID: 39251 RVA: 0x003A091E File Offset: 0x0039EB1E
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

	// Token: 0x06009954 RID: 39252 RVA: 0x003A093B File Offset: 0x0039EB3B
	public static void ReloadAllXmlsSync()
	{
		WorldStaticData.Cleanup(null);
		ThreadManager.RunCoroutineSync(WorldStaticData.LoadAllXmlsCo(false, null, null));
	}

	// Token: 0x06009955 RID: 39253 RVA: 0x003A0950 File Offset: 0x0039EB50
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

	// Token: 0x06009956 RID: 39254 RVA: 0x003A09B4 File Offset: 0x0039EBB4
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

	// Token: 0x06009957 RID: 39255 RVA: 0x003A0AB4 File Offset: 0x0039ECB4
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

	// Token: 0x06009958 RID: 39256 RVA: 0x003A0AF2 File Offset: 0x0039ECF2
	public static void WaitForConfigsFromServer()
	{
		if (WorldStaticData.receivedConfigsHandlerCoroutine != null)
		{
			ThreadManager.StopCoroutine(WorldStaticData.receivedConfigsHandlerCoroutine);
		}
		WorldStaticData.receivedConfigsHandlerCoroutine = ThreadManager.StartCoroutine(WorldStaticData.handleReceivedConfigs());
	}

	// Token: 0x06009959 RID: 39257 RVA: 0x003A0B14 File Offset: 0x0039ED14
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

	// Token: 0x0600995A RID: 39258 RVA: 0x003A0B1C File Offset: 0x0039ED1C
	public static bool AllConfigsReceivedAndLoaded()
	{
		return WorldStaticData.receivedConfigsHandlerCoroutine == null;
	}

	// Token: 0x0600995B RID: 39259 RVA: 0x003A0B28 File Offset: 0x0039ED28
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

	// Token: 0x0600995C RID: 39260 RVA: 0x003A0BA8 File Offset: 0x0039EDA8
	public static void UnloadMeshDescriptionTextureArrays(int _meshIndex)
	{
		WorldStaticData.meshDescCol.UnloadTextureArrays(_meshIndex);
	}

	// Token: 0x0600995D RID: 39261 RVA: 0x003A0BB5 File Offset: 0x0039EDB5
	public static void ReloadTextureArrays()
	{
		ThreadManager.RunCoroutineSync(WorldStaticData.meshDescCol.LoadTextureArrays(true));
	}

	// Token: 0x0600995E RID: 39262 RVA: 0x003A0BC8 File Offset: 0x0039EDC8
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

	// Token: 0x04007359 RID: 29529
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cExplosionPrefabMax = 100;

	// Token: 0x0400735A RID: 29530
	public static bool LoadAllXmlsCoComplete;

	// Token: 0x0400735B RID: 29531
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

	// Token: 0x0400735C RID: 29532
	public static Transform[] prefabExplosions;

	// Token: 0x0400735D RID: 29533
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool bInitDone;

	// Token: 0x0400735E RID: 29534
	[PublicizedFrom(EAccessModifier.Private)]
	public static MeshDescriptionCollection meshDescCol;

	// Token: 0x0400735F RID: 29535
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isDediServer;

	// Token: 0x04007360 RID: 29536
	[PublicizedFrom(EAccessModifier.Private)]
	public static Coroutine receivedConfigsHandlerCoroutine;

	// Token: 0x04007361 RID: 29537
	[PublicizedFrom(EAccessModifier.Private)]
	public static int highestReceivedIndex = -1;

	// Token: 0x020012ED RID: 4845
	[PublicizedFrom(EAccessModifier.Private)]
	public enum EClientFileState
	{
		// Token: 0x04007363 RID: 29539
		None,
		// Token: 0x04007364 RID: 29540
		Received,
		// Token: 0x04007365 RID: 29541
		LoadLocal
	}

	// Token: 0x020012EE RID: 4846
	[PublicizedFrom(EAccessModifier.Private)]
	public class XmlLoadInfo
	{
		// Token: 0x06009960 RID: 39264 RVA: 0x003A1510 File Offset: 0x0039F710
		public bool XmlFileExists()
		{
			return SdFile.Exists(GameIO.GetGameDir("Data/Config") + "/" + this.XmlName + ".xml");
		}

		// Token: 0x06009961 RID: 39265 RVA: 0x003A1538 File Offset: 0x0039F738
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

		// Token: 0x04007366 RID: 29542
		public readonly string XmlName;

		// Token: 0x04007367 RID: 29543
		public readonly string LoadStepLocalizationKey;

		// Token: 0x04007368 RID: 29544
		public readonly bool LoadAtStartup;

		// Token: 0x04007369 RID: 29545
		public readonly bool SendToClients;

		// Token: 0x0400736A RID: 29546
		public readonly bool IgnoreMissingFile;

		// Token: 0x0400736B RID: 29547
		public readonly bool AllowReloadDuringGame;

		// Token: 0x0400736C RID: 29548
		public readonly Func<XmlFile, IEnumerator> LoadMethod;

		// Token: 0x0400736D RID: 29549
		public readonly Action CleanupMethod;

		// Token: 0x0400736E RID: 29550
		public readonly Func<IEnumerator> ExecuteAfterLoad;

		// Token: 0x0400736F RID: 29551
		public readonly Action<XmlFile> ReloadDuringGameMethod;

		// Token: 0x04007370 RID: 29552
		public byte[] CompressedXmlData;

		// Token: 0x04007371 RID: 29553
		public bool LoadClientFile;

		// Token: 0x04007372 RID: 29554
		public WorldStaticData.EClientFileState WasReceivedFromServer;
	}

	// Token: 0x020012EF RID: 4847
	// (Invoke) Token: 0x06009963 RID: 39267
	public delegate void ProgressDelegate(string _progressText, float _percentage);
}
