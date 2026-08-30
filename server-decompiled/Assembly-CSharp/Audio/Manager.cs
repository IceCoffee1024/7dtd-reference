using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B1C RID: 6940
	public class Manager : IDisposable
	{
		// Token: 0x17001997 RID: 6551
		// (get) Token: 0x0600D03C RID: 53308 RVA: 0x004BCA9A File Offset: 0x004BAC9A
		public static Manager Instance
		{
			get
			{
				if (Manager.instance == null)
				{
					Manager.instance = new Manager();
				}
				return Manager.instance;
			}
		}

		// Token: 0x0600D03D RID: 53309 RVA: 0x004BCAB2 File Offset: 0x004BACB2
		public Manager()
		{
			this.PositionalSoundsPlaying = new GameObject("PositionalSoundsPlaying");
			this.PositionalSoundsPlayingT = this.PositionalSoundsPlaying.transform;
			Origin.Add(this.PositionalSoundsPlayingT, 0);
		}

		// Token: 0x0600D03E RID: 53310 RVA: 0x004BCAE8 File Offset: 0x004BACE8
		public static void Init()
		{
			Manager.audioData = new Dictionary<string, XmlData>(StringComparer.OrdinalIgnoreCase);
			Manager.loopingOnEntity = new Dictionary<int, Dictionary<string, Manager.NearAndFarGO>>();
			Manager.fadingOutOnEntity = new Dictionary<int, Dictionary<string, Manager.NearAndFarGO>>();
			Manager.loopingOnPosition = new Dictionary<Vector3, Dictionary<string, Manager.NearAndFarGO>>(Vector3ToFixedEqualityComparer.Instance);
			Manager.sequenceOnEntity = new Dictionary<int, Dictionary<string, Manager.SequenceGOs>>();
			Manager.stoppedEntitySequences = new Dictionary<int, Dictionary<string, Manager.SequenceStopper>>();
			Manager.playAndCleanupSources = new DictionaryList<string, List<AudioSource>>();
			Manager.playingAudioSources = new List<AudioSource>();
			Manager.playingAudioSourceDopplers = new List<Manager.DopplerItem>();
			Manager.audioSourceDatas = new Dictionary<string, Manager.AudioSourceData>();
			Manager.playingOnEntity = new Dictionary<int, Manager.Channels>();
			Manager.random = GameRandomManager.Instance.CreateGameRandom();
		}

		// Token: 0x0600D03F RID: 53311 RVA: 0x004BCB7C File Offset: 0x004BAD7C
		[PublicizedFrom(EAccessModifier.Private)]
		public static EntityPlayerLocal LocalPlayer()
		{
			if (!Manager.localPlayer)
			{
				Manager.localPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			}
			return Manager.localPlayer;
		}

		// Token: 0x0600D040 RID: 53312 RVA: 0x004BCBA4 File Offset: 0x004BADA4
		public static void OriginChanged(Vector3 offset)
		{
			Manager.updateCurrentListener();
			Dictionary<Vector3, Dictionary<string, Manager.NearAndFarGO>> dictionary = new Dictionary<Vector3, Dictionary<string, Manager.NearAndFarGO>>(Vector3ToFixedEqualityComparer.Instance);
			foreach (KeyValuePair<Vector3, Dictionary<string, Manager.NearAndFarGO>> keyValuePair in Manager.loopingOnPosition)
			{
				Vector3 vector = keyValuePair.Key + offset;
				bool flag = false;
				for (int i = 0; i < 10; i++)
				{
					if (!dictionary.ContainsKey(vector))
					{
						dictionary.Add(vector, keyValuePair.Value);
						flag = true;
						break;
					}
					vector.x += 0.0234f;
					vector.z += 0.09f;
				}
				if (!flag)
				{
					Log.Warning("AudioManager OriginChanged key collision {0}, count {1}", new object[]
					{
						vector,
						keyValuePair.Value.Count
					});
				}
			}
			Manager.loopingOnPosition.Clear();
			Manager.loopingOnPosition = dictionary;
			Manager.DopplerCheckForMove();
		}

		// Token: 0x0600D041 RID: 53313 RVA: 0x004BCCAC File Offset: 0x004BAEAC
		[PublicizedFrom(EAccessModifier.Private)]
		public static void DopplerCheckForMove()
		{
			if (Manager.dopplerDelay > 0)
			{
				return;
			}
			if ((Manager.mainCamera.transform.position - Manager.currentListenerPosition).sqrMagnitude > 400f)
			{
				Manager.DopplerRestore();
				for (int i = Manager.playingAudioSources.Count - 1; i >= 0; i--)
				{
					AudioSource audioSource = Manager.playingAudioSources[i];
					if (audioSource)
					{
						float dopplerLevel = audioSource.dopplerLevel;
						if (dopplerLevel > 0f)
						{
							audioSource.dopplerLevel = 0f;
							Manager.DopplerItem dopplerItem;
							dopplerItem.src = audioSource;
							dopplerItem.doppler = dopplerLevel;
							Manager.playingAudioSourceDopplers.Add(dopplerItem);
						}
					}
				}
				Manager.dopplerDelay = 6;
			}
		}

		// Token: 0x0600D042 RID: 53314 RVA: 0x004BCD58 File Offset: 0x004BAF58
		[PublicizedFrom(EAccessModifier.Private)]
		public static void DopplerRestore()
		{
			int count = Manager.playingAudioSourceDopplers.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					AudioSource src = Manager.playingAudioSourceDopplers[i].src;
					if (src)
					{
						src.dopplerLevel = Manager.playingAudioSourceDopplers[i].doppler;
					}
				}
				Manager.playingAudioSourceDopplers.Clear();
			}
		}

		// Token: 0x0600D043 RID: 53315 RVA: 0x004BCDB9 File Offset: 0x004BAFB9
		public static void AddSoundToIgnoreDistanceCheckList(string soundGroupName)
		{
			if (Manager.ignoreDistanceCheckSounds == null)
			{
				Manager.ignoreDistanceCheckSounds = new CaseInsensitiveStringDictionary<bool>();
			}
			if (!Manager.ignoreDistanceCheckSounds.ContainsKey(soundGroupName))
			{
				Manager.ignoreDistanceCheckSounds.Add(soundGroupName, true);
			}
		}

		// Token: 0x0600D044 RID: 53316 RVA: 0x004BCDE5 File Offset: 0x004BAFE5
		public static bool IgnoresDistanceCheck(string soundGroupName)
		{
			Manager.StripOffDirectories(ref soundGroupName);
			if (Manager.ignoreDistanceCheckSounds == null)
			{
				Manager.ignoreDistanceCheckSounds = new CaseInsensitiveStringDictionary<bool>();
			}
			return Manager.ignoreDistanceCheckSounds.ContainsKey(soundGroupName);
		}

		// Token: 0x0600D045 RID: 53317 RVA: 0x004BCE0A File Offset: 0x004BB00A
		public static void Reset()
		{
			Manager.Init();
		}

		// Token: 0x0600D046 RID: 53318 RVA: 0x004BCE11 File Offset: 0x004BB011
		public static void CleanUp()
		{
			if (Manager.Instance.PositionalSoundsPlaying != null)
			{
				Origin.Remove(Manager.Instance.PositionalSoundsPlayingT);
				global::UnityEngine.Object.Destroy(Manager.Instance.PositionalSoundsPlaying);
			}
			Manager.instance = null;
		}

		// Token: 0x0600D047 RID: 53319 RVA: 0x004BCE4C File Offset: 0x004BB04C
		public static void AddAudioData(XmlData _data)
		{
			Manager.StripOffDirectories(ref _data.soundGroupName);
			Manager.audioData.Add(_data.soundGroupName, _data);
			if (_data.audioClipMap.Count > 0 || _data.altAudioClipMap != null)
			{
				if (_data.noiseData != null)
				{
					float volume = _data.noiseData.volume;
					float time = _data.noiseData.time;
					float crouchMuffle = _data.noiseData.crouchMuffle;
					float heatMapStrength = _data.noiseData.heatMapStrength;
					ulong num = _data.noiseData.heatMapTime * 10UL;
					AIDirectorData.AddNoisySound(_data.soundGroupName, new AIDirectorData.Noise(_data.soundGroupName, volume, time, crouchMuffle, heatMapStrength, num));
				}
				if (_data.hasProfanity)
				{
					_data.cleanClipMap = new List<ClipSourceMap>();
					for (int i = 0; i < _data.audioClipMap.Count; i++)
					{
						if (!_data.audioClipMap[i].profanity)
						{
							_data.cleanClipMap.Add(_data.audioClipMap[i]);
						}
					}
				}
			}
		}

		// Token: 0x0600D048 RID: 53320 RVA: 0x004BCF4C File Offset: 0x004BB14C
		public static void AddSubtitleData(List<SubtitleData> subtitleDatas, List<SubtitleSpeakerColor> speakerColors)
		{
			Manager.subtitleCache = new Dictionary<string, SubtitleData>();
			foreach (SubtitleData subtitleData in subtitleDatas)
			{
				Manager.subtitleCache.Add(subtitleData.name, subtitleData);
			}
			Manager.subtitleSpeakerColorCache = new Dictionary<string, string>();
			foreach (SubtitleSpeakerColor subtitleSpeakerColor in speakerColors)
			{
				Manager.subtitleSpeakerColorCache.Add(subtitleSpeakerColor.name, subtitleSpeakerColor.color);
			}
			Log.Out(string.Format("Added {0} subtitle data entries and {1} speaker colors", Manager.subtitleCache.Count, Manager.subtitleSpeakerColorCache.Count));
		}

		// Token: 0x0600D049 RID: 53321 RVA: 0x004BD034 File Offset: 0x004BB234
		public static void PauseGameplayAudio()
		{
			List<AudioSource> list = Manager.playingAudioSources;
			lock (list)
			{
				for (int i = Manager.playingAudioSources.Count - 1; i >= 0; i--)
				{
					if (Manager.playingAudioSources[i] != null)
					{
						Manager.playingAudioSources[i].Pause();
					}
					else
					{
						Manager.playingAudioSources.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x0600D04A RID: 53322 RVA: 0x004BD0B4 File Offset: 0x004BB2B4
		public static void UnPauseGameplayAudio()
		{
			List<AudioSource> list = Manager.playingAudioSources;
			lock (list)
			{
				for (int i = Manager.playingAudioSources.Count - 1; i >= 0; i--)
				{
					if (Manager.playingAudioSources[i] != null)
					{
						Manager.playingAudioSources[i].UnPause();
					}
					else
					{
						Manager.playingAudioSources.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x0600D04B RID: 53323 RVA: 0x004BD134 File Offset: 0x004BB334
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioSource LoadAudio(bool _forcedLooping, float _listenerDistance, string _clipName, string _audioSourceName)
		{
			if (_clipName == "none")
			{
				return null;
			}
			GameObject gameObject;
			if (!Manager.audioSrcObjAssetCache.TryGetValue(_audioSourceName, out gameObject))
			{
				gameObject = DataLoader.LoadAsset<GameObject>(_audioSourceName, false);
				if (gameObject)
				{
					Manager.audioSrcObjAssetCache.Add(_audioSourceName, gameObject);
				}
			}
			if (!gameObject)
			{
				Log.Warning("AudioManager LoadAudio failed to load audio source object for " + _audioSourceName);
				return null;
			}
			AudioSource audioSource = gameObject.GetComponent<AudioSource>();
			if (_listenerDistance >= audioSource.maxDistance && !audioSource.loop && !_forcedLooping)
			{
				return null;
			}
			GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
			if (!gameObject2)
			{
				Log.Warning("AudioManager LoadAudio failed to instantiate audio source object for " + _audioSourceName);
				return null;
			}
			audioSource = gameObject2.GetComponent<AudioSource>();
			if (!audioSource)
			{
				Log.Warning("AudioManager LoadAudio failed to load audio source " + _audioSourceName);
				global::UnityEngine.Object.Destroy(gameObject2);
				return null;
			}
			AudioClip audioClip;
			if (!Manager.audioClipAssetCache.TryGetValue(_clipName, out audioClip))
			{
				audioClip = DataLoader.LoadAsset<AudioClip>(_clipName, false);
				if (audioClip)
				{
					Manager.audioClipAssetCache.Add(_clipName, audioClip);
				}
			}
			if (!audioClip)
			{
				Log.Warning("AudioManager LoadAudio failed to load audio clip " + _clipName);
				return null;
			}
			audioSource.clip = audioClip;
			string text = _audioSourceName.Replace("Sounds/", "");
			if (!Manager.audioSourceDatas.ContainsKey(text))
			{
				Manager.AudioSourceData audioSourceData;
				audioSourceData.maxVolume = audioSource.volume;
				Manager.audioSourceDatas.Add(text, audioSourceData);
			}
			if (audioSource.dopplerLevel > 0f)
			{
				Rigidbody rigidbody = gameObject2.GetComponent<Rigidbody>();
				if (rigidbody == null)
				{
					rigidbody = gameObject2.AddComponent<Rigidbody>();
				}
				rigidbody.useGravity = false;
				rigidbody.velocity = Vector3.zero;
				rigidbody.isKinematic = true;
				rigidbody.gameObject.tag = "AudioRigidBody";
			}
			return audioSource;
		}

		// Token: 0x0600D04C RID: 53324 RVA: 0x004BD2D8 File Offset: 0x004BB4D8
		public static void BroadcastPlay(string soundGroupName)
		{
			if (soundGroupName == null)
			{
				return;
			}
			if (Manager.ServerAudio != null)
			{
				if (!GameManager.IsDedicatedServer)
				{
					Manager.Play(Manager.LocalPlayer(), soundGroupName, 1f, false);
				}
				Manager.ServerAudio.Play(Manager.LocalPlayer(), soundGroupName, 0f, false, 1f);
				return;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
			{
				NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(Manager.LocalPlayer().entityId, soundGroupName, 0f, true, false, 1f);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageAudio, false);
			}
		}

		// Token: 0x0600D04D RID: 53325 RVA: 0x004BD360 File Offset: 0x004BB560
		public static void BroadcastPlay(Entity entity, string soundGroupName, bool signalOnly = false, float volumeScale = 1f)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient && signalOnly)
			{
				return;
			}
			EntityPlayer entityPlayer = entity as EntityPlayer;
			if (entityPlayer != null && entityPlayer.IsSpectator)
			{
				return;
			}
			if (soundGroupName == null)
			{
				return;
			}
			if (Manager.ServerAudio != null)
			{
				if (!GameManager.IsDedicatedServer)
				{
					Manager.Play(entity, soundGroupName, volumeScale, false);
				}
				Manager.ServerAudio.Play(entity, soundGroupName, entity.CalculateAudioOcclusion(), signalOnly, 1f);
				return;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
			{
				NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(entity.entityId, soundGroupName, entity.CalculateAudioOcclusion(), true, signalOnly, 1f);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageAudio, false);
			}
		}

		// Token: 0x0600D04E RID: 53326 RVA: 0x004BD401 File Offset: 0x004BB601
		public static void BroadcastPlayByLocalPlayer(Vector3 position, string soundGroupName)
		{
			Manager.BroadcastPlay(Manager.LocalPlayer(), position, soundGroupName);
		}

		// Token: 0x0600D04F RID: 53327 RVA: 0x004BD410 File Offset: 0x004BB610
		public static void BroadcastPlay(Entity entity, Vector3 position, string soundGroupName)
		{
			if (soundGroupName == null)
			{
				return;
			}
			if (Manager.ServerAudio != null)
			{
				if (!GameManager.IsDedicatedServer)
				{
					Manager.Play(position, soundGroupName, (entity != null) ? entity.entityId : (-1), false, 1f);
				}
				Manager.ServerAudio.Play(position, soundGroupName, 0f, -1, 1f);
				return;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
			{
				NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(position, soundGroupName, 0f, true, (entity != null) ? entity.entityId : (-1), 1f);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageAudio, false);
			}
		}

		// Token: 0x0600D050 RID: 53328 RVA: 0x004BD4AC File Offset: 0x004BB6AC
		public static void BroadcastPlay(Vector3 position, string soundGroupName, float _occlusion = 0f)
		{
			if (soundGroupName == null)
			{
				return;
			}
			if (Manager.ServerAudio != null)
			{
				if (!GameManager.IsDedicatedServer)
				{
					Manager.Play(position, soundGroupName, -1, false, 1f);
				}
				Manager.ServerAudio.Play(position, soundGroupName, _occlusion, -1, 1f);
				return;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
			{
				NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(position, soundGroupName, _occlusion, true, -1, 1f);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageAudio, false);
			}
		}

		// Token: 0x0600D051 RID: 53329 RVA: 0x004BD51C File Offset: 0x004BB71C
		public static void BroadcastStop(int entityId, string soundGroupName)
		{
			if (soundGroupName == null)
			{
				return;
			}
			if (Manager.ServerAudio != null)
			{
				if (!GameManager.IsDedicatedServer)
				{
					Manager.Stop(entityId, soundGroupName);
				}
				Manager.ServerAudio.Stop(entityId, soundGroupName);
				return;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
			{
				NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(entityId, soundGroupName, 0f, false, false, 1f);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageAudio, false);
			}
		}

		// Token: 0x0600D052 RID: 53330 RVA: 0x004BD580 File Offset: 0x004BB780
		public static void BroadcastStop(Vector3 position, string soundGroupName)
		{
			if (soundGroupName == null)
			{
				return;
			}
			if (Manager.ServerAudio != null)
			{
				if (!GameManager.IsDedicatedServer)
				{
					Manager.Stop(position, soundGroupName);
				}
				Manager.ServerAudio.Stop(position, soundGroupName);
				return;
			}
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
			{
				NetPackageAudio netPackageAudio = NetPackageManager.GetPackage<NetPackageAudio>().Setup(position, soundGroupName, 0f, false, -1, 1f);
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackageAudio, false);
			}
		}

		// Token: 0x0600D053 RID: 53331 RVA: 0x004BD5E4 File Offset: 0x004BB7E4
		public static Handle Play(Vector3 position, string soundGroupName, int entityId = -1, bool wantHandle = false, float _volumeScale = 1f)
		{
			Entity entity = null;
			Manager.ConvertName(ref soundGroupName, null);
			if (entityId >= 0)
			{
				entity = GameManager.Instance.World.GetEntity(entityId);
			}
			Manager.SignalAI(entity, position, soundGroupName, _volumeScale);
			if (!Manager.CheckGlobalPlayRequirements(soundGroupName))
			{
				return null;
			}
			XmlData xmlData;
			if (!Manager.audioData.TryGetValue(soundGroupName, out xmlData))
			{
				return null;
			}
			if (!xmlData.CanAddANewVoice())
			{
				return null;
			}
			if (xmlData.stopOldestNode)
			{
				Manager.Stop(entityId, soundGroupName);
			}
			position -= Origin.position;
			bool flag = false;
			bool flag2 = true;
			if (xmlData.distantFadeStart >= 0f)
			{
				float magnitude = (position - Manager.currentListenerPosition).magnitude;
				flag = magnitude > xmlData.distantFadeStart;
				flag2 = magnitude < xmlData.distantFadeEnd;
			}
			ClipSourceMap randomClip = xmlData.GetRandomClip();
			if (randomClip == null)
			{
				return null;
			}
			GameObject gameObject = null;
			AudioSource audioSource = null;
			if (flag2)
			{
				audioSource = Manager.LoadAudio(randomClip.forceLoop, (Manager.LocalPlayer().position - Origin.position - position).magnitude, randomClip.clipName, randomClip.audioSourceName);
				if (!audioSource)
				{
					return null;
				}
				gameObject = audioSource.gameObject;
				xmlData.lastClipLenght = audioSource.clip.length;
				Transform transform = audioSource.transform;
				transform.SetParent(Manager.Instance.PositionalSoundsPlayingT, false);
				transform.position = position;
				audioSource.loop = audioSource.loop || randomClip.forceLoop;
				if (entity != null && entity is EntityAlive)
				{
					float num = audioSource.volume * _volumeScale;
					num = (((EntityAlive)entity).IsCrouching ? (num * xmlData.localCrouchVolumeScale) : num);
					audioSource.volume = (((EntityAlive)entity).MovementRunning ? (num * xmlData.runningVolumeScale) : num);
				}
			}
			GameObject gameObject2 = null;
			AudioSource audioSource2 = null;
			if (flag)
			{
				audioSource2 = Manager.LoadAudio(randomClip.forceLoop, (Manager.LocalPlayer().position - Origin.position - position).magnitude, randomClip.clipName_distant, (randomClip.audioSourceName_distant.Length > 0) ? randomClip.audioSourceName_distant : randomClip.audioSourceName);
				if (audioSource2 == null)
				{
					return null;
				}
				gameObject2 = audioSource2.gameObject;
				xmlData.lastClipLenght = audioSource2.clip.length;
				Transform transform2 = audioSource2.transform;
				transform2.SetParent(Manager.Instance.PositionalSoundsPlayingT, false);
				transform2.position = position;
				audioSource2.loop = audioSource2.loop || randomClip.forceLoop;
				if (entity != null && entity is EntityAlive)
				{
					float num2 = audioSource2.volume;
					num2 = (((EntityAlive)entity).IsCrouching ? (num2 * xmlData.localCrouchVolumeScale) : num2);
					audioSource2.volume = (((EntityAlive)entity).MovementRunning ? (num2 * xmlData.runningVolumeScale) : num2);
				}
			}
			if ((audioSource && audioSource.loop) || (audioSource2 && audioSource2.loop))
			{
				Dictionary<string, Manager.NearAndFarGO> dictionary;
				Manager.NearAndFarGO nearAndFarGO;
				if (Manager.loopingOnPosition.TryGetValue(position, out dictionary) && dictionary.TryGetValue(soundGroupName, out nearAndFarGO))
				{
					if (nearAndFarGO.near != null)
					{
						AudioSource component = nearAndFarGO.near.GetComponent<AudioSource>();
						if (component != null)
						{
							Manager.RemovePlayingAudioSource(component);
						}
						global::UnityEngine.Object.Destroy(nearAndFarGO.near);
					}
					if (nearAndFarGO.far != null)
					{
						AudioSource component2 = nearAndFarGO.far.GetComponent<AudioSource>();
						if (component2 != null)
						{
							Manager.RemovePlayingAudioSource(component2);
						}
						global::UnityEngine.Object.Destroy(nearAndFarGO.far);
					}
					dictionary.Remove(soundGroupName);
					if (dictionary.Count == 0)
					{
						Manager.loopingOnPosition.Remove(position);
					}
				}
				if (audioSource != null)
				{
					audioSource.volume *= _volumeScale;
					audioSource.volume *= 1f - ((entity == null) ? 0f : entity.CalculateAudioOcclusion());
					Manager.SetPitch(audioSource, xmlData, 0f);
					Manager.PlaySource(audioSource);
					Manager.AddPlayingAudioSource(audioSource);
				}
				if (audioSource2 != null)
				{
					audioSource2.volume *= _volumeScale;
					Manager.SetPitch(audioSource2, xmlData, 0f);
					Manager.PlaySource(audioSource2);
					Manager.AddPlayingAudioSource(audioSource2);
				}
				Manager.NearAndFarGO nearAndFarGO2 = default(Manager.NearAndFarGO);
				nearAndFarGO2.near = gameObject;
				nearAndFarGO2.far = gameObject2;
				Dictionary<string, Manager.NearAndFarGO> dictionary2;
				if (Manager.loopingOnPosition.TryGetValue(position, out dictionary2))
				{
					dictionary2.Add(soundGroupName, nearAndFarGO2);
				}
				else
				{
					Dictionary<string, Manager.NearAndFarGO> dictionary3 = new Dictionary<string, Manager.NearAndFarGO>();
					dictionary3.Add(soundGroupName, nearAndFarGO2);
					Manager.loopingOnPosition.Add(position, dictionary3);
				}
			}
			else
			{
				if (audioSource != null)
				{
					Manager.SetPitch(audioSource, xmlData, 0f);
					new PlayAndCleanup(gameObject, audioSource, (entity == null) ? 0f : entity.CalculateAudioOcclusion(), 0f, false, false, soundGroupName);
					if (xmlData.vibratesController && entity is EntityPlayerLocal)
					{
						GameManager.Instance.triggerEffectManager.SetAudioRumbleSource(audioSource, xmlData.vibrationStrengthMultiplier, true);
					}
				}
				if (audioSource2 != null)
				{
					Manager.SetPitch(audioSource2, xmlData, 0f);
					new PlayAndCleanup(gameObject2, audioSource2, (entity == null) ? 0f : entity.CalculateAudioOcclusion(), 0f, false, false, soundGroupName);
				}
			}
			if (wantHandle)
			{
				return new Handle(soundGroupName, audioSource, audioSource2);
			}
			return null;
		}

		// Token: 0x0600D054 RID: 53332 RVA: 0x004BDB40 File Offset: 0x004BBD40
		public static Handle Play(Entity _entity, string soundGroupName, float volumeScale = 1f, bool wantHandle = false)
		{
			Manager.ConvertName(ref soundGroupName, _entity);
			Manager.SignalAI(_entity, _entity.position, soundGroupName, volumeScale);
			if (!Manager.CheckGlobalPlayRequirements(soundGroupName))
			{
				return null;
			}
			XmlData xmlData;
			if (!Manager.audioData.TryGetValue(soundGroupName, out xmlData) || xmlData == null)
			{
				return null;
			}
			if (!xmlData.CanAddANewVoice())
			{
				return null;
			}
			if (xmlData.stopOldestNode)
			{
				Manager.Stop(_entity.entityId, soundGroupName);
			}
			bool flag = false;
			Manager.Channels channels;
			if (Manager.playingOnEntity.TryGetValue(_entity.entityId, out channels))
			{
				if (xmlData.channel == XmlData.Channel.Environment)
				{
					List<AudioSource> list;
					if (channels.environment != null && channels.environment.dict.TryGetValue(soundGroupName, out list))
					{
						for (int i = list.Count - 1; i >= 0; i--)
						{
							AudioSource audioSource = list[i];
							if (!audioSource || !audioSource.isPlaying)
							{
								list.RemoveAt(i);
								Manager.RemovePlayingAudioSource(audioSource);
							}
						}
						if (list.Count == 0)
						{
							channels.environment.Remove(soundGroupName);
							if (channels.environment.Count == 0 && (channels.mouth == null || channels.mouth.Count == 0))
							{
								Manager.playingOnEntity.Remove(_entity.entityId);
							}
						}
						else if (list.Count > xmlData.maxVoicesPerEntity)
						{
							flag = true;
						}
					}
				}
				else if (channels.mouth != null)
				{
					List<int> list2 = null;
					for (int j = 0; j < channels.mouth.Count; j++)
					{
						if (channels.mouth[j] == null || !channels.mouth[j].isPlaying)
						{
							if (list2 == null)
							{
								list2 = new List<int>();
							}
							list2.Add(j);
						}
					}
					if (list2 != null)
					{
						for (int k = list2.Count - 1; k >= 0; k--)
						{
							if (channels.mouth[list2[k]] != null)
							{
								Manager.RemovePlayingAudioSource(channels.mouth[list2[k]]);
							}
							channels.mouth.RemoveAt(list2[k]);
						}
					}
					if (channels.mouth.Count > 0)
					{
						if (xmlData.priority < channels.currentMouthPriority)
						{
							for (int l = 0; l < channels.mouth.Count; l++)
							{
								Manager.StopSource(channels.mouth[l]);
								Manager.RemovePlayingAudioSource(channels.mouth[l]);
							}
							channels.mouth.Clear();
						}
						else
						{
							flag = true;
						}
					}
					if (channels.mouth.Count == 0 && (channels.environment == null || channels.environment.Count == 0))
					{
						Manager.playingOnEntity.Remove(_entity.entityId);
					}
				}
				if (Manager.playingOnEntity.ContainsKey(_entity.entityId))
				{
					Manager.playingOnEntity[_entity.entityId] = channels;
				}
			}
			if (flag)
			{
				return null;
			}
			bool flag2 = false;
			bool flag3 = true;
			if (xmlData.distantFadeStart >= 0f)
			{
				float magnitude = (_entity.position - Origin.position - Manager.currentListenerPosition).magnitude;
				flag2 = magnitude > xmlData.distantFadeStart;
				flag3 = magnitude < xmlData.distantFadeEnd;
			}
			Transform transform = _entity.transform;
			Vector3 position = transform.position;
			ClipSourceMap randomClip = xmlData.GetRandomClip();
			if (randomClip == null)
			{
				return null;
			}
			GameObject gameObject = null;
			AudioSource audioSource2 = null;
			if (flag3)
			{
				audioSource2 = Manager.LoadAudio(randomClip.forceLoop, (_entity.position - Manager.LocalPlayer().position).magnitude, randomClip.clipName, randomClip.audioSourceName);
				if (!audioSource2)
				{
					return null;
				}
				gameObject = audioSource2.gameObject;
				xmlData.lastClipLenght = audioSource2.clip.length;
				Transform transform2 = audioSource2.transform;
				Transform transform3 = null;
				if (_entity.emodel)
				{
					transform3 = _entity.emodel.bipedPelvisTransform;
				}
				if (transform3)
				{
					transform2.SetParent(transform3, false);
					transform2.position = transform3.position;
				}
				else
				{
					transform2.SetParent(transform, false);
					transform2.position = transform.position;
				}
				audioSource2.loop = audioSource2.loop || randomClip.forceLoop;
			}
			GameObject gameObject2 = null;
			AudioSource audioSource3 = null;
			if (flag2)
			{
				audioSource3 = Manager.LoadAudio(randomClip.forceLoop, (_entity.position - Manager.LocalPlayer().position).magnitude, randomClip.clipName_distant, (randomClip.audioSourceName_distant.Length > 0) ? randomClip.audioSourceName_distant : randomClip.audioSourceName);
				if (audioSource3 == null)
				{
					return null;
				}
				gameObject2 = audioSource3.gameObject;
				xmlData.lastClipLenght = audioSource3.clip.length;
				Transform transform4 = audioSource3.transform;
				transform4.SetParent(transform);
				transform4.position = position;
				audioSource3.loop = audioSource3.loop || randomClip.forceLoop;
			}
			EntityAlive entityAlive = _entity as EntityAlive;
			if (entityAlive)
			{
				float num = ((_entity is EntityPlayerLocal) ? 0.05f : entityAlive.OverridePitch);
				if (audioSource2 != null)
				{
					float num2 = audioSource2.volume * volumeScale;
					if (entityAlive.IsCrouching)
					{
						num2 *= xmlData.localCrouchVolumeScale;
					}
					if (entityAlive.MovementRunning)
					{
						num2 *= xmlData.runningVolumeScale;
					}
					audioSource2.volume = num2;
					Manager.SetPitch(audioSource2, xmlData, num);
					if (xmlData.vibratesController)
					{
						if (_entity is EntityPlayerLocal)
						{
							GameManager.Instance.triggerEffectManager.SetAudioRumbleSource(audioSource2, xmlData.vibrationStrengthMultiplier, false);
						}
						else if (entityAlive.GetAttachedPlayerLocal() != null)
						{
							GameManager.Instance.triggerEffectManager.SetAudioRumbleSource(audioSource2, xmlData.vibrationStrengthMultiplier, false);
						}
					}
				}
				if (audioSource3 != null)
				{
					float num3 = audioSource3.volume * volumeScale;
					if (entityAlive.IsCrouching)
					{
						num3 *= xmlData.localCrouchVolumeScale;
					}
					if (entityAlive.MovementRunning)
					{
						num3 *= xmlData.runningVolumeScale;
					}
					audioSource3.volume = num3;
					Manager.SetPitch(audioSource3, xmlData, num);
				}
			}
			if ((audioSource2 && audioSource2.loop) || (audioSource3 && audioSource3.loop))
			{
				Dictionary<string, Manager.NearAndFarGO> dictionary;
				if (Manager.loopingOnEntity.TryGetValue(_entity.entityId, out dictionary))
				{
					Manager.StopGroupLoop(dictionary, soundGroupName);
				}
				if (audioSource2 != null)
				{
					audioSource2.volume *= 1f - ((_entity == null) ? 0f : _entity.CalculateAudioOcclusion());
					Manager.PlaySource(audioSource2);
					Manager.AddPlayingAudioSource(audioSource2);
				}
				if (audioSource3 != null)
				{
					Manager.PlaySource(audioSource3);
					Manager.AddPlayingAudioSource(audioSource3);
				}
				if (dictionary == null)
				{
					dictionary = new Dictionary<string, Manager.NearAndFarGO>();
					Manager.loopingOnEntity.Add(_entity.entityId, dictionary);
				}
				Manager.NearAndFarGO nearAndFarGO = new Manager.NearAndFarGO
				{
					near = gameObject,
					far = gameObject2
				};
				dictionary.Add(soundGroupName, nearAndFarGO);
			}
			else
			{
				Manager.Channels channels2;
				if (!Manager.playingOnEntity.TryGetValue(_entity.entityId, out channels2))
				{
					channels2 = default(Manager.Channels);
					Manager.playingOnEntity.Add(_entity.entityId, channels2);
				}
				if (xmlData.channel == XmlData.Channel.Environment)
				{
					if (channels2.environment == null)
					{
						channels2.environment = new DictionaryList<string, List<AudioSource>>();
					}
					List<AudioSource> list3;
					if (!channels2.environment.dict.TryGetValue(soundGroupName, out list3))
					{
						list3 = new List<AudioSource>();
						channels2.environment.Add(soundGroupName, list3);
					}
					if (audioSource2 != null)
					{
						list3.Add(audioSource2);
					}
					if (audioSource3 != null)
					{
						list3.Add(audioSource3);
					}
				}
				else
				{
					if (channels2.mouth == null)
					{
						channels2.mouth = new List<AudioSource>();
					}
					channels2.currentMouthPriority = xmlData.priority;
					if (audioSource2 != null)
					{
						channels2.mouth.Add(audioSource2);
					}
					if (audioSource3 != null)
					{
						channels2.mouth.Add(audioSource3);
					}
				}
				Manager.playingOnEntity[_entity.entityId] = channels2;
				if (audioSource2 != null)
				{
					new PlayAndCleanup(gameObject, audioSource2, _entity.CalculateAudioOcclusion(), 0f, false, false, soundGroupName);
				}
				if (audioSource3 != null)
				{
					new PlayAndCleanup(gameObject2, audioSource3, _entity.CalculateAudioOcclusion(), 0f, false, false, soundGroupName);
				}
			}
			Handle handle = null;
			if (wantHandle)
			{
				handle = new Handle(soundGroupName, audioSource2, audioSource3);
			}
			if (GamePrefs.GetBool(EnumGamePrefs.OptionsSubtitlesEnabled) && randomClip.hasSubtitle)
			{
				GameManager.ShowSubtitle(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Manager.GetFormattedSubtitleSpeaker(randomClip.subtitleID), Manager.GetFormattedSubtitle(randomClip.subtitleID), audioSource2.clip.length, false);
			}
			return handle;
		}

		// Token: 0x0600D055 RID: 53333 RVA: 0x004BE3EC File Offset: 0x004BC5EC
		[PublicizedFrom(EAccessModifier.Private)]
		public static void SetPitch(AudioSource _src, XmlData _data, float _shift)
		{
			float num = Manager.random.RandomRange(_data.lowestPitch, _data.highestPitch);
			num += _shift;
			if (num < 0.85f)
			{
				num = 0.85f;
			}
			_src.pitch = num;
		}

		// Token: 0x0600D056 RID: 53334 RVA: 0x004BE42C File Offset: 0x004BC62C
		public static void StopAllLocal()
		{
			if (Manager.playingAudioSources != null)
			{
				for (int i = 0; i < Manager.playingAudioSources.Count; i++)
				{
					if (Manager.playingAudioSources[i] != null)
					{
						Manager.StopSource(Manager.playingAudioSources[i]);
					}
				}
			}
		}

		// Token: 0x0600D057 RID: 53335 RVA: 0x004BE478 File Offset: 0x004BC678
		public static void AddPlayingAudioSource(AudioSource _src)
		{
			List<AudioSource> list = Manager.playingAudioSources;
			lock (list)
			{
				Manager.playingAudioSources.Add(_src);
			}
		}

		// Token: 0x0600D058 RID: 53336 RVA: 0x004BE4BC File Offset: 0x004BC6BC
		public static void AddPlayAndCleanupAudioSource(string soundGroupName, AudioSource _src)
		{
			DictionaryList<string, List<AudioSource>> dictionaryList = Manager.playAndCleanupSources;
			lock (dictionaryList)
			{
				List<AudioSource> list = Manager.playAndCleanupSources.Get(soundGroupName);
				if (list == null)
				{
					list = new List<AudioSource> { _src };
				}
				else
				{
					list.Add(_src);
				}
				Manager.playAndCleanupSources.Set(soundGroupName, list);
			}
		}

		// Token: 0x0600D059 RID: 53337 RVA: 0x004BE528 File Offset: 0x004BC728
		public static void RemovePlayingAudioSource(AudioSource _src)
		{
			List<AudioSource> list = Manager.playingAudioSources;
			lock (list)
			{
				Manager.playingAudioSources.Remove(_src);
			}
		}

		// Token: 0x0600D05A RID: 53338 RVA: 0x004BE570 File Offset: 0x004BC770
		public static void RemovePlayAndCleanupAudioSource(string soundGroupName, AudioSource _src)
		{
			DictionaryList<string, List<AudioSource>> dictionaryList = Manager.playAndCleanupSources;
			lock (dictionaryList)
			{
				List<AudioSource> list = Manager.playAndCleanupSources.Get(soundGroupName);
				if (list != null)
				{
					list.Remove(_src);
					if (list.Count == 0)
					{
						Manager.playAndCleanupSources.Remove(soundGroupName);
					}
					else
					{
						Manager.playAndCleanupSources.Set(soundGroupName, list);
					}
				}
			}
		}

		// Token: 0x0600D05B RID: 53339 RVA: 0x004BE5E4 File Offset: 0x004BC7E4
		public static void FadeOut(int entityId, string soundGroupName)
		{
			if (!Manager.CheckGlobalPlayRequirements(soundGroupName))
			{
				return;
			}
			Manager.ConvertName(ref soundGroupName, null);
			Dictionary<string, Manager.NearAndFarGO> dictionary;
			if (!Manager.loopingOnEntity.TryGetValue(entityId, out dictionary))
			{
				return;
			}
			Manager.NearAndFarGO nearAndFarGO;
			if (!dictionary.TryGetValue(soundGroupName, out nearAndFarGO))
			{
				return;
			}
			if (Manager.fadingOutOnEntity.TryGetValue(entityId, out dictionary))
			{
				if (dictionary.ContainsKey(soundGroupName))
				{
					return;
				}
			}
			else
			{
				dictionary = new Dictionary<string, Manager.NearAndFarGO>();
				Manager.fadingOutOnEntity.Add(entityId, dictionary);
			}
			dictionary.Add(soundGroupName, nearAndFarGO);
		}

		// Token: 0x0600D05C RID: 53340 RVA: 0x004BE654 File Offset: 0x004BC854
		public static void Stop(int entityId, string soundGroupName)
		{
			if (!Manager.CheckGlobalPlayRequirements(soundGroupName))
			{
				return;
			}
			Manager.ConvertName(ref soundGroupName, null);
			Dictionary<string, Manager.NearAndFarGO> dictionary;
			if (Manager.loopingOnEntity.TryGetValue(entityId, out dictionary))
			{
				Manager.StopGroupLoop(dictionary, soundGroupName);
				if (dictionary.Count == 0)
				{
					Manager.loopingOnEntity.Remove(entityId);
				}
			}
			Manager.Channels channels;
			List<AudioSource> list;
			if (Manager.playingOnEntity.TryGetValue(entityId, out channels) && channels.environment != null && channels.environment.dict.TryGetValue(soundGroupName, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					AudioSource audioSource = list[i];
					if (audioSource)
					{
						Manager.StopSource(audioSource);
						Manager.RemovePlayingAudioSource(audioSource);
						global::UnityEngine.Object.Destroy(audioSource.gameObject);
					}
				}
				channels.environment.Remove(soundGroupName);
				if (channels.environment.Count == 0)
				{
					channels.environment = null;
					if (channels.mouth == null || channels.mouth.Count == 0)
					{
						Manager.playingOnEntity.Remove(entityId);
					}
				}
			}
			Manager.StopPlayAndCleanupSource(soundGroupName);
		}

		// Token: 0x0600D05D RID: 53341 RVA: 0x004BE758 File Offset: 0x004BC958
		[PublicizedFrom(EAccessModifier.Private)]
		public static void StopGroupLoop(Dictionary<string, Manager.NearAndFarGO> soundGroup, string soundGroupName)
		{
			Manager.NearAndFarGO nearAndFarGO;
			if (soundGroup.TryGetValue(soundGroupName, out nearAndFarGO))
			{
				if (nearAndFarGO.near != null)
				{
					AudioSource component = nearAndFarGO.near.GetComponent<AudioSource>();
					if (component != null)
					{
						Manager.StopSource(component);
						Manager.RemovePlayingAudioSource(component);
					}
					global::UnityEngine.Object.Destroy(nearAndFarGO.near);
				}
				if (nearAndFarGO.far != null)
				{
					AudioSource component2 = nearAndFarGO.far.GetComponent<AudioSource>();
					if (component2 != null)
					{
						Manager.StopSource(component2);
						Manager.RemovePlayingAudioSource(component2);
					}
					global::UnityEngine.Object.Destroy(nearAndFarGO.far);
				}
				soundGroup.Remove(soundGroupName);
			}
		}

		// Token: 0x0600D05E RID: 53342 RVA: 0x004BE7EC File Offset: 0x004BC9EC
		public static void Stop(Vector3 position, string soundGroupName)
		{
			if (!Manager.CheckGlobalPlayRequirements(soundGroupName))
			{
				return;
			}
			Manager.ConvertName(ref soundGroupName, null);
			position -= Origin.position;
			Dictionary<string, Manager.NearAndFarGO> dictionary;
			Manager.NearAndFarGO nearAndFarGO;
			if (Manager.loopingOnPosition.TryGetValue(position, out dictionary) && dictionary.TryGetValue(soundGroupName, out nearAndFarGO))
			{
				if (nearAndFarGO.near != null)
				{
					AudioSource component = nearAndFarGO.near.GetComponent<AudioSource>();
					if (component != null)
					{
						Manager.StopSource(component);
						Manager.RemovePlayingAudioSource(component);
					}
					global::UnityEngine.Object.Destroy(nearAndFarGO.near);
				}
				if (nearAndFarGO.far != null)
				{
					AudioSource component2 = nearAndFarGO.far.GetComponent<AudioSource>();
					if (component2 != null)
					{
						Manager.StopSource(component2);
						Manager.RemovePlayingAudioSource(component2);
					}
					global::UnityEngine.Object.Destroy(nearAndFarGO.far);
				}
				dictionary.Remove(soundGroupName);
				if (dictionary.Count < 1)
				{
					Manager.loopingOnPosition.Remove(position);
				}
			}
		}

		// Token: 0x0600D05F RID: 53343 RVA: 0x004BE8C8 File Offset: 0x004BCAC8
		[PublicizedFrom(EAccessModifier.Private)]
		public static void StopPlayAndCleanupSource(string soundGroupName)
		{
			DictionaryList<string, List<AudioSource>> dictionaryList = Manager.playAndCleanupSources;
			lock (dictionaryList)
			{
				List<AudioSource> list = Manager.playAndCleanupSources.Get(soundGroupName);
				if (list != null && list.Count > 0)
				{
					AudioSource audioSource = list[0];
					if (audioSource != null)
					{
						Manager.StopSource(audioSource);
						Manager.RemovePlayAndCleanupAudioSource(soundGroupName, audioSource);
						if (audioSource.gameObject != null)
						{
							global::UnityEngine.Object.Destroy(audioSource.gameObject);
						}
					}
				}
			}
		}

		// Token: 0x0600D060 RID: 53344 RVA: 0x004BE950 File Offset: 0x004BCB50
		public static void PlayInsidePlayerHead(string soundGroupNameBegin, int entityID)
		{
			string text = soundGroupNameBegin + "_lp";
			if (!Manager.CheckGlobalPlayRequirements(soundGroupNameBegin))
			{
				return;
			}
			if (!Manager.CheckGlobalPlayRequirements(text))
			{
				return;
			}
			Manager.ConvertName(ref soundGroupNameBegin, null);
			Manager.ConvertName(ref text, null);
			XmlData xmlData;
			if (!Manager.audioData.TryGetValue(soundGroupNameBegin, out xmlData))
			{
				return;
			}
			if (!xmlData.CanAddANewVoice())
			{
				return;
			}
			if (xmlData.stopOldestNode)
			{
				Manager.Stop(entityID, soundGroupNameBegin);
			}
			XmlData xmlData2;
			if (!Manager.audioData.TryGetValue(text, out xmlData2))
			{
				return;
			}
			if (!xmlData2.CanAddANewVoice())
			{
				return;
			}
			if (xmlData2.stopOldestNode)
			{
				Manager.Stop(entityID, text);
			}
			ClipSourceMap randomClip = xmlData.GetRandomClip();
			if (randomClip == null)
			{
				return;
			}
			AudioSource audioSource = Manager.LoadAudio(randomClip.forceLoop, 0f, randomClip.clipName, randomClip.audioSourceName);
			if (audioSource == null)
			{
				return;
			}
			ClipSourceMap randomClip2 = xmlData2.GetRandomClip();
			if (randomClip2 == null)
			{
				return;
			}
			AudioSource audioSource2 = Manager.LoadAudio(randomClip2.forceLoop, 0f, randomClip2.clipName, randomClip2.audioSourceName);
			if (audioSource2 == null)
			{
				return;
			}
			audioSource2.loop = true;
			Transform transform = Manager.LocalPlayer().transform;
			Vector3 position = transform.position;
			Transform transform2 = audioSource.transform;
			transform2.position = position;
			transform2.SetParent(transform);
			Transform transform3 = audioSource2.transform;
			transform3.position = position;
			transform3.SetParent(transform);
			GameObject gameObject = audioSource.gameObject;
			GameObject gameObject2 = audioSource2.gameObject;
			LoopingPair loopingPair = default(LoopingPair);
			loopingPair.sgoBegin.go = gameObject;
			loopingPair.sgoBegin.src = audioSource;
			loopingPair.sgoLoop.go = gameObject2;
			loopingPair.sgoLoop.src = audioSource2;
			Manager.NearAndFarGO nearAndFarGO = default(Manager.NearAndFarGO);
			nearAndFarGO.near = gameObject2;
			if (!Manager.loopingOnEntity.ContainsKey(entityID))
			{
				Manager.loopingOnEntity.Add(entityID, new Dictionary<string, Manager.NearAndFarGO>());
			}
			Manager.loopingOnEntity[entityID].Add(text, nearAndFarGO);
			new PlayAndCleanup(loopingPair, text);
		}

		// Token: 0x0600D061 RID: 53345 RVA: 0x004BEB30 File Offset: 0x004BCD30
		public static void PlayInsidePlayerHead(string soundGroupName, int entityID = -1, float delay = 0f, bool isLooping = false, bool isUnique = false)
		{
			if (!Manager.CheckGlobalPlayRequirements(soundGroupName))
			{
				return;
			}
			Entity entity = ((entityID >= 0) ? GameManager.Instance.World.GetEntity(entityID) : null);
			Manager.ConvertName(ref soundGroupName, entity);
			XmlData xmlData;
			if (!Manager.audioData.TryGetValue(soundGroupName, out xmlData))
			{
				return;
			}
			if (!xmlData.CanAddANewVoice())
			{
				return;
			}
			if (xmlData.stopOldestNode)
			{
				Manager.Stop(entityID, soundGroupName);
			}
			ClipSourceMap randomClip = xmlData.GetRandomClip();
			if (randomClip == null)
			{
				return;
			}
			AudioSource audioSource = Manager.LoadAudio(randomClip.forceLoop, 0f, randomClip.clipName, randomClip.audioSourceName);
			if (audioSource == null)
			{
				return;
			}
			GameObject gameObject = audioSource.gameObject;
			Transform transform = audioSource.transform;
			EntityAlive entityAlive = Manager.LocalPlayer();
			Transform transform2 = entityAlive.transform;
			transform.SetParent(transform2, false);
			transform.position = transform2.position;
			if (entityAlive.IsCrouching)
			{
				audioSource.volume *= xmlData.localCrouchVolumeScale;
			}
			Manager.SetPitch(audioSource, xmlData, 0f);
			if (xmlData.vibratesController)
			{
				GameManager.Instance.triggerEffectManager.SetAudioRumbleSource(audioSource, xmlData.vibrationStrengthMultiplier, false);
			}
			if (isLooping)
			{
				audioSource.loop = true;
				Manager.NearAndFarGO nearAndFarGO = default(Manager.NearAndFarGO);
				nearAndFarGO.near = gameObject;
				if (entityID != -1)
				{
					if (Manager.loopingOnEntity.ContainsKey(entityID))
					{
						Manager.loopingOnEntity[entityID].Add(soundGroupName, nearAndFarGO);
					}
					else
					{
						Manager.loopingOnEntity.Add(entityID, new Dictionary<string, Manager.NearAndFarGO>());
						Manager.loopingOnEntity[entityID].Add(soundGroupName, nearAndFarGO);
					}
				}
				new PlayAndCleanup(gameObject, audioSource, 0f, delay, true, false, soundGroupName);
			}
			else
			{
				if (isUnique)
				{
					if (Manager.uniqueSrc != null)
					{
						Manager.StopSource(Manager.uniqueSrc);
					}
					Manager.uniqueSrc = audioSource;
				}
				new PlayAndCleanup(gameObject, audioSource, 0f, delay, false, false, soundGroupName);
			}
			if (GamePrefs.GetBool(EnumGamePrefs.OptionsSubtitlesEnabled) && randomClip.hasSubtitle)
			{
				GameManager.ShowSubtitle(LocalPlayerUI.GetUIForPrimaryPlayer().xui, Manager.GetFormattedSubtitleSpeaker(randomClip.subtitleID), Manager.GetFormattedSubtitle(randomClip.subtitleID), audioSource.clip.length, false);
			}
		}

		// Token: 0x0600D062 RID: 53346 RVA: 0x004BED2C File Offset: 0x004BCF2C
		public static void StopLoopInsidePlayerHead(string soundGroupName, int entityID = -1, bool stopImmediately = false)
		{
			if (!Manager.CheckGlobalPlayRequirements(soundGroupName))
			{
				return;
			}
			Manager.ConvertName(ref soundGroupName, null);
			Manager.NearAndFarGO nearAndFarGO;
			if (entityID != -1 && Manager.loopingOnEntity.ContainsKey(entityID) && Manager.loopingOnEntity[entityID].TryGetValue(soundGroupName, out nearAndFarGO))
			{
				AudioSource component = nearAndFarGO.near.GetComponent<AudioSource>();
				if (component != null)
				{
					if (stopImmediately)
					{
						component.loop = false;
					}
					Manager.StopSource(component);
					global::UnityEngine.Object.Destroy(nearAndFarGO.near);
					Manager.loopingOnEntity[entityID].Remove(soundGroupName);
				}
			}
		}

		// Token: 0x0600D063 RID: 53347 RVA: 0x004BEDB4 File Offset: 0x004BCFB4
		public static void ConvertName(ref string soundGroupName, Entity _entity = null)
		{
			if (soundGroupName == null)
			{
				return;
			}
			EntityPlayer entityPlayer = _entity as EntityPlayer;
			if (entityPlayer != null)
			{
				int num = soundGroupName.IndexOfAny(Manager.convertChars);
				if (num >= 0)
				{
					if (soundGroupName[num] == '*')
					{
						string text = (entityPlayer.IsMale ? "Male" : "Female");
						soundGroupName = soundGroupName.Replace("*", text);
					}
					else
					{
						string text2 = (entityPlayer.IsMale ? "1" : "2");
						soundGroupName = soundGroupName.Replace("#", text2);
					}
				}
			}
			Manager.StripOffDirectories(ref soundGroupName);
		}

		// Token: 0x0600D064 RID: 53348 RVA: 0x004BEE40 File Offset: 0x004BD040
		public static float CalculateOcclusion(Vector3 positionOfSound, Vector3 positionOfEars)
		{
			if (!Manager.occlusionsOn)
			{
				return 1f;
			}
			if (Manager.mainCamera == null)
			{
				return 1f;
			}
			Vector3 vector = positionOfSound - positionOfEars;
			float magnitude = vector.magnitude;
			if (magnitude < 1f)
			{
				return 1f;
			}
			Vector3 normalized = vector.normalized;
			RaycastHit raycastHit;
			RaycastHit raycastHit2;
			if (Physics.Raycast(new Ray(positionOfEars - Origin.position, normalized), out raycastHit, float.PositiveInfinity, 65537) && magnitude > raycastHit.distance + 0.5f && raycastHit.distance < float.PositiveInfinity && Physics.Raycast(new Ray(positionOfSound - Origin.position, (positionOfEars - positionOfSound).normalized), out raycastHit2, float.PositiveInfinity, 65537))
			{
				float num = magnitude - raycastHit2.distance - raycastHit.distance;
				return 1f - Mathf.Pow(Mathf.Clamp01(num / 13f), 0.75f) * 0.9f;
			}
			return 1f;
		}

		// Token: 0x0600D065 RID: 53349 RVA: 0x004BEF48 File Offset: 0x004BD148
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool CheckGlobalPlayRequirements(string soundGroupName)
		{
			if (GameManager.IsDedicatedServer)
			{
				return false;
			}
			if (!GameManager.Instance)
			{
				return false;
			}
			World world = GameManager.Instance.World;
			return world != null && world.GetPrimaryPlayer() && soundGroupName != null;
		}

		// Token: 0x0600D066 RID: 53350 RVA: 0x004BEF90 File Offset: 0x004BD190
		public static void PlaySequence(Entity entity, string soundGroupName)
		{
			Manager.ConvertName(ref soundGroupName, entity);
			Manager.SignalAI(entity, entity.position, soundGroupName, 1f);
			if (!Manager.CheckGlobalPlayRequirements(soundGroupName))
			{
				return;
			}
			if (entity == null)
			{
				return;
			}
			XmlData xmlData;
			if (!Manager.audioData.TryGetValue(soundGroupName, out xmlData))
			{
				return;
			}
			if (!xmlData.CanAddANewVoice())
			{
				return;
			}
			if (xmlData.stopOldestNode)
			{
				Manager.Stop(entity.entityId, soundGroupName);
			}
			Dictionary<string, Manager.SequenceGOs> dictionary;
			if (!Manager.sequenceOnEntity.TryGetValue(entity.entityId, out dictionary))
			{
				dictionary = new Dictionary<string, Manager.SequenceGOs>();
				Manager.sequenceOnEntity.Add(entity.entityId, dictionary);
			}
			if (!dictionary.ContainsKey(soundGroupName))
			{
				Manager.SequenceGOs sequenceGOs = default(Manager.SequenceGOs);
				sequenceGOs.longestClipLength = 0f;
				int num = 0;
				double num2 = 0.0;
				double num3 = 0.0;
				bool flag = false;
				bool flag2 = true;
				float num4 = 1f;
				Transform transform = entity.transform;
				Vector3 position = transform.position;
				if (xmlData.distantFadeStart >= 0f)
				{
					float magnitude = (position - Manager.currentListenerPosition).magnitude;
					flag = magnitude > xmlData.distantFadeStart;
					flag2 = magnitude < xmlData.distantFadeEnd;
					num4 = (flag ? (1f - (magnitude - xmlData.distantFadeStart) / (xmlData.distantFadeEnd - xmlData.distantFadeStart)) : 1f);
				}
				List<ClipSourceMap> clipList = xmlData.GetClipList();
				for (int i = 0; i < clipList.Count; i++)
				{
					ClipSourceMap clipSourceMap = clipList[i];
					if (flag2 || clipSourceMap.clipName_distant.Length == 0)
					{
						AudioSource audioSource = Manager.LoadAudio(false, 0f, clipSourceMap.clipName, clipSourceMap.audioSourceName);
						if (audioSource != null)
						{
							GameObject gameObject = audioSource.gameObject;
							Transform transform2 = audioSource.transform;
							transform2.SetParent(transform);
							transform2.position = position;
							if (audioSource.clip.length > sequenceGOs.longestClipLength)
							{
								sequenceGOs.longestClipLength = audioSource.clip.length;
							}
							if (num == 0)
							{
								audioSource.volume *= Manager.CalculateOcclusion(position, Manager.currentListenerPosition);
								audioSource.volume *= num4;
								audioSource.loop = false;
								Manager.PlaySource(audioSource);
								Manager.AddPlayingAudioSource(audioSource);
								double dspTime = AudioSettings.dspTime;
								double num5 = (double)(audioSource.clip.samples / 2);
								num5 /= (double)audioSource.clip.frequency;
								num2 += dspTime + num5;
								sequenceGOs.nearStart = gameObject;
							}
							else if (num == 1)
							{
								if (xmlData.playImmediate)
								{
									audioSource.loop = false;
									audioSource.volume *= Manager.CalculateOcclusion(position, Manager.currentListenerPosition);
									audioSource.volume *= num4;
									Manager.PlaySource(audioSource);
									Manager.AddPlayingAudioSource(audioSource);
								}
								else
								{
									audioSource.loop = true;
									audioSource.volume *= Manager.CalculateOcclusion(position, Manager.currentListenerPosition);
									audioSource.volume *= num4;
									audioSource.PlayScheduled(num2);
									Manager.AddPlayingAudioSource(audioSource);
								}
								sequenceGOs.nearLoop = gameObject;
							}
							else if (xmlData.playImmediate && num == 2)
							{
								audioSource.volume *= Manager.CalculateOcclusion(position, Manager.currentListenerPosition);
								audioSource.volume *= num4;
								Manager.PlaySource(audioSource);
								Manager.AddPlayingAudioSource(audioSource);
								sequenceGOs.nearEnd = gameObject;
							}
							else
							{
								sequenceGOs.nearEnd = gameObject;
							}
						}
					}
					if (flag && clipSourceMap.clipName_distant.Length > 0)
					{
						AudioSource audioSource2 = Manager.LoadAudio(false, 0f, clipSourceMap.clipName_distant, (clipSourceMap.audioSourceName_distant.Length > 0) ? clipSourceMap.audioSourceName_distant : clipSourceMap.audioSourceName);
						if (audioSource2 != null)
						{
							GameObject gameObject2 = audioSource2.gameObject;
							Transform transform3 = audioSource2.transform;
							transform3.SetParent(transform);
							transform3.position = position;
							if (audioSource2.clip.length > sequenceGOs.longestClipLength)
							{
								sequenceGOs.longestClipLength = audioSource2.clip.length;
							}
							if (num == 0)
							{
								audioSource2.volume *= Manager.CalculateOcclusion(position, Manager.currentListenerPosition);
								audioSource2.volume *= 1f - num4;
								audioSource2.loop = false;
								Manager.PlaySource(audioSource2);
								Manager.AddPlayingAudioSource(audioSource2);
								double dspTime2 = AudioSettings.dspTime;
								double num6 = (double)(audioSource2.clip.samples / 2);
								num6 /= (double)audioSource2.clip.frequency;
								num3 += dspTime2 + num6;
								sequenceGOs.farStart = gameObject2;
							}
							else if (num == 1)
							{
								if (xmlData.playImmediate)
								{
									audioSource2.loop = false;
									audioSource2.volume *= Manager.CalculateOcclusion(position, Manager.currentListenerPosition);
									audioSource2.volume *= 1f - num4;
									Manager.PlaySource(audioSource2);
									Manager.AddPlayingAudioSource(audioSource2);
								}
								else
								{
									audioSource2.loop = true;
									audioSource2.volume *= Manager.CalculateOcclusion(position, Manager.currentListenerPosition);
									audioSource2.volume *= 1f - num4;
									audioSource2.PlayScheduled(num3);
									Manager.AddPlayingAudioSource(audioSource2);
								}
								sequenceGOs.farLoop = gameObject2;
							}
							else if (xmlData.playImmediate && num == 2)
							{
								audioSource2.volume *= Manager.CalculateOcclusion(position, Manager.currentListenerPosition);
								audioSource2.volume *= 1f - num4;
								Manager.PlaySource(audioSource2);
								Manager.AddPlayingAudioSource(audioSource2);
								sequenceGOs.farEnd = gameObject2;
							}
							else
							{
								sequenceGOs.farEnd = gameObject2;
							}
						}
					}
					num++;
				}
				dictionary.Add(soundGroupName, sequenceGOs);
			}
		}

		// Token: 0x0600D067 RID: 53351 RVA: 0x004BF550 File Offset: 0x004BD750
		public static bool IsASequence(Entity entity, string soundGroupName)
		{
			if (soundGroupName == null)
			{
				return false;
			}
			Manager.ConvertName(ref soundGroupName, entity);
			XmlData xmlData;
			return Manager.audioData.TryGetValue(soundGroupName, out xmlData) && xmlData.sequence;
		}

		// Token: 0x0600D068 RID: 53352 RVA: 0x004BF584 File Offset: 0x004BD784
		public static void RestartSequence(Entity entity, string soundGroupName)
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			Manager.ConvertName(ref soundGroupName, entity);
			Dictionary<string, Manager.SequenceStopper> dictionary;
			Manager.SequenceStopper sequenceStopper;
			if (Manager.stoppedEntitySequences.TryGetValue(entity.entityId, out dictionary) && dictionary.TryGetValue(soundGroupName, out sequenceStopper))
			{
				for (int i = 0; i < sequenceStopper.sequenceObjs.Count; i++)
				{
					GameObject gameObject = sequenceStopper.sequenceObjs[i];
					if (gameObject != null)
					{
						AudioSource component = gameObject.GetComponent<AudioSource>();
						if (component != null)
						{
							Manager.StopSource(component);
							Manager.RemovePlayingAudioSource(component);
						}
						global::UnityEngine.Object.Destroy(gameObject);
					}
				}
				dictionary.Remove(soundGroupName);
				if (dictionary.Count < 1)
				{
					Manager.stoppedEntitySequences.Remove(entity.entityId);
				}
			}
			Dictionary<string, Manager.SequenceGOs> dictionary2;
			Manager.SequenceGOs sequenceGOs;
			if (Manager.sequenceOnEntity.TryGetValue(entity.entityId, out dictionary2) && dictionary2.TryGetValue(soundGroupName, out sequenceGOs))
			{
				if (sequenceGOs.nearStart != null)
				{
					AudioSource component2 = sequenceGOs.nearStart.GetComponent<AudioSource>();
					if (component2 != null)
					{
						Manager.StopSource(component2);
						Manager.RemovePlayingAudioSource(component2);
					}
					global::UnityEngine.Object.Destroy(sequenceGOs.nearStart);
				}
				if (sequenceGOs.nearLoop != null)
				{
					AudioSource component3 = sequenceGOs.nearLoop.GetComponent<AudioSource>();
					if (component3 != null)
					{
						Manager.StopSource(component3);
						Manager.RemovePlayingAudioSource(component3);
					}
					global::UnityEngine.Object.Destroy(sequenceGOs.nearLoop);
				}
				if (sequenceGOs.nearEnd != null)
				{
					AudioSource component4 = sequenceGOs.nearEnd.GetComponent<AudioSource>();
					if (component4 != null)
					{
						Manager.StopSource(component4);
						Manager.RemovePlayingAudioSource(component4);
					}
					global::UnityEngine.Object.Destroy(sequenceGOs.nearEnd);
				}
				if (sequenceGOs.farStart != null)
				{
					AudioSource component5 = sequenceGOs.farStart.GetComponent<AudioSource>();
					if (component5 != null)
					{
						Manager.StopSource(component5);
						Manager.RemovePlayingAudioSource(component5);
					}
					global::UnityEngine.Object.Destroy(sequenceGOs.farStart);
				}
				if (sequenceGOs.farLoop != null)
				{
					AudioSource component6 = sequenceGOs.farLoop.GetComponent<AudioSource>();
					if (component6 != null)
					{
						Manager.StopSource(component6);
						Manager.RemovePlayingAudioSource(component6);
					}
					global::UnityEngine.Object.Destroy(sequenceGOs.farLoop);
				}
				if (sequenceGOs.farEnd != null)
				{
					AudioSource component7 = sequenceGOs.farEnd.GetComponent<AudioSource>();
					if (component7 != null)
					{
						Manager.StopSource(component7);
						Manager.RemovePlayingAudioSource(component7);
					}
					global::UnityEngine.Object.Destroy(sequenceGOs.farEnd);
				}
				dictionary2.Remove(soundGroupName);
				if (dictionary2.Count < 1)
				{
					Manager.sequenceOnEntity.Remove(entity.entityId);
				}
			}
			Manager.PlaySequence(entity, soundGroupName);
		}

		// Token: 0x0600D069 RID: 53353 RVA: 0x004BF810 File Offset: 0x004BDA10
		public static void StopAllSequencesOnEntity(Entity entity)
		{
			if (GameManager.IsDedicatedServer)
			{
				return;
			}
			Dictionary<string, Manager.SequenceGOs> dictionary;
			if (Manager.sequenceOnEntity.TryGetValue(entity.entityId, out dictionary))
			{
				foreach (KeyValuePair<string, Manager.SequenceGOs> keyValuePair in dictionary)
				{
					Manager.SequenceGOs value = keyValuePair.Value;
					if (value.nearLoop != null)
					{
						AudioSource component = value.nearLoop.GetComponent<AudioSource>();
						if (component != null)
						{
							Manager.StopSource(component);
							Manager.RemovePlayingAudioSource(component);
						}
					}
					if (value.farLoop != null)
					{
						AudioSource component2 = value.farLoop.GetComponent<AudioSource>();
						if (component2 != null)
						{
							Manager.StopSource(component2);
							Manager.RemovePlayingAudioSource(component2);
						}
					}
					if (value.nearEnd != null || value.farEnd != null)
					{
						AudioSource audioSource = (value.nearEnd ? value.nearEnd.GetComponent<AudioSource>() : null);
						AudioSource audioSource2 = (value.farEnd ? value.farEnd.GetComponent<AudioSource>() : null);
						if (audioSource != null || audioSource2 != null)
						{
							XmlData xmlData;
							if (Manager.audioData.TryGetValue(keyValuePair.Key, out xmlData) && !xmlData.playImmediate)
							{
								if (audioSource != null)
								{
									audioSource.volume *= Manager.CalculateOcclusion(entity.position - Origin.position, Manager.currentListenerPosition);
									Manager.PlaySource(audioSource);
									Manager.AddPlayingAudioSource(audioSource);
								}
								if (audioSource2 != null)
								{
									audioSource2.volume *= Manager.CalculateOcclusion(entity.position - Origin.position, Manager.currentListenerPosition);
									Manager.PlaySource(audioSource2);
									Manager.AddPlayingAudioSource(audioSource2);
								}
							}
							Dictionary<string, Manager.SequenceStopper> dictionary2;
							Manager.SequenceStopper sequenceStopper;
							if (Manager.stoppedEntitySequences.TryGetValue(entity.entityId, out dictionary2))
							{
								if (dictionary2.TryGetValue(keyValuePair.Key, out sequenceStopper))
								{
									for (int i = 0; i < sequenceStopper.sequenceObjs.Count; i++)
									{
										global::UnityEngine.Object.Destroy(sequenceStopper.sequenceObjs[i]);
									}
									dictionary2.Remove(keyValuePair.Key);
								}
							}
							else
							{
								dictionary2 = new Dictionary<string, Manager.SequenceStopper>();
								Manager.stoppedEntitySequences.Add(entity.entityId, dictionary2);
							}
							if (!dictionary2.TryGetValue(keyValuePair.Key, out sequenceStopper))
							{
								List<GameObject> list = new List<GameObject>();
								if (value.nearStart != null)
								{
									list.Add(value.nearStart);
								}
								if (value.nearLoop != null)
								{
									list.Add(value.nearLoop);
								}
								if (value.nearEnd != null)
								{
									list.Add(value.nearEnd);
								}
								if (value.farStart != null)
								{
									list.Add(value.farStart);
								}
								if (value.farLoop != null)
								{
									list.Add(value.farLoop);
								}
								if (value.farEnd != null)
								{
									list.Add(value.farEnd);
								}
								sequenceStopper = new Manager.SequenceStopper(list, Time.time + value.longestClipLength);
								dictionary2.Add(keyValuePair.Key, sequenceStopper);
							}
						}
					}
				}
				dictionary.Clear();
			}
		}

		// Token: 0x0600D06A RID: 53354 RVA: 0x004BFB74 File Offset: 0x004BDD74
		public static void StopSequence(Entity entity, string soundGroupName)
		{
			if (GameManager.IsDedicatedServer || soundGroupName == null)
			{
				return;
			}
			Manager.ConvertName(ref soundGroupName, entity);
			Dictionary<string, Manager.SequenceGOs> dictionary;
			Manager.SequenceGOs sequenceGOs;
			if (Manager.sequenceOnEntity.TryGetValue(entity.entityId, out dictionary) && dictionary.TryGetValue(soundGroupName, out sequenceGOs))
			{
				if (sequenceGOs.nearLoop != null)
				{
					AudioSource component = sequenceGOs.nearLoop.GetComponent<AudioSource>();
					if (component != null)
					{
						Manager.StopSource(component);
						Manager.RemovePlayingAudioSource(component);
					}
				}
				if (sequenceGOs.farLoop != null)
				{
					AudioSource component2 = sequenceGOs.farLoop.GetComponent<AudioSource>();
					if (component2 != null)
					{
						Manager.StopSource(component2);
						Manager.RemovePlayingAudioSource(component2);
					}
				}
				if (sequenceGOs.nearEnd != null || sequenceGOs.farEnd != null)
				{
					AudioSource audioSource = (sequenceGOs.nearEnd ? sequenceGOs.nearEnd.GetComponent<AudioSource>() : null);
					AudioSource audioSource2 = (sequenceGOs.farEnd ? sequenceGOs.farEnd.GetComponent<AudioSource>() : null);
					if (audioSource != null || audioSource2 != null)
					{
						XmlData xmlData;
						if (Manager.audioData.TryGetValue(soundGroupName, out xmlData) && !xmlData.playImmediate)
						{
							if (audioSource != null)
							{
								audioSource.volume *= Manager.CalculateOcclusion(entity.position - Origin.position, Manager.currentListenerPosition);
								Manager.PlaySource(audioSource);
								Manager.AddPlayingAudioSource(audioSource);
							}
							if (audioSource2 != null)
							{
								audioSource2.volume *= Manager.CalculateOcclusion(entity.position - Origin.position, Manager.currentListenerPosition);
								Manager.PlaySource(audioSource2);
								Manager.AddPlayingAudioSource(audioSource2);
							}
						}
						Dictionary<string, Manager.SequenceStopper> dictionary2;
						Manager.SequenceStopper sequenceStopper;
						if (Manager.stoppedEntitySequences.TryGetValue(entity.entityId, out dictionary2))
						{
							if (dictionary2.TryGetValue(soundGroupName, out sequenceStopper))
							{
								for (int i = 0; i < sequenceStopper.sequenceObjs.Count; i++)
								{
									global::UnityEngine.Object.Destroy(sequenceStopper.sequenceObjs[i]);
								}
								dictionary2.Remove(soundGroupName);
							}
						}
						else
						{
							dictionary2 = new Dictionary<string, Manager.SequenceStopper>();
							Manager.stoppedEntitySequences.Add(entity.entityId, dictionary2);
						}
						if (!dictionary2.TryGetValue(soundGroupName, out sequenceStopper))
						{
							List<GameObject> list = new List<GameObject>();
							if (sequenceGOs.nearStart != null)
							{
								list.Add(sequenceGOs.nearStart);
							}
							if (sequenceGOs.nearLoop != null)
							{
								list.Add(sequenceGOs.nearLoop);
							}
							if (sequenceGOs.nearEnd != null)
							{
								list.Add(sequenceGOs.nearEnd);
							}
							if (sequenceGOs.farStart != null)
							{
								list.Add(sequenceGOs.farStart);
							}
							if (sequenceGOs.farLoop != null)
							{
								list.Add(sequenceGOs.farLoop);
							}
							if (sequenceGOs.farEnd != null)
							{
								list.Add(sequenceGOs.farEnd);
							}
							sequenceStopper = new Manager.SequenceStopper(list, Time.time + sequenceGOs.longestClipLength);
							dictionary2.Add(soundGroupName, sequenceStopper);
						}
					}
				}
				dictionary.Remove(soundGroupName);
			}
		}

		// Token: 0x0600D06B RID: 53355 RVA: 0x004BFE78 File Offset: 0x004BE078
		public static void DestroySoundsForEntity(int entityId)
		{
			Manager.Channels channels;
			if (Manager.playingOnEntity.TryGetValue(entityId, out channels))
			{
				if (channels.environment != null)
				{
					for (int i = 0; i < channels.environment.list.Count; i++)
					{
						for (int j = 0; j < channels.environment.list[i].Count; j++)
						{
							if (channels.environment.list[i][j] != null)
							{
								Manager.StopSource(channels.environment.list[i][j]);
								Manager.RemovePlayingAudioSource(channels.environment.list[i][j]);
							}
						}
					}
				}
				if (channels.mouth != null)
				{
					for (int k = 0; k < channels.mouth.Count; k++)
					{
						if (channels.mouth[k] != null)
						{
							if (k < channels.mouth.Count - 1)
							{
								Manager.StopSource(channels.mouth[k]);
							}
							Manager.RemovePlayingAudioSource(channels.mouth[k]);
						}
					}
				}
			}
			Manager.playingOnEntity.Remove(entityId);
			Dictionary<string, Manager.SequenceGOs> dictionary;
			if (Manager.sequenceOnEntity.TryGetValue(entityId, out dictionary))
			{
				foreach (KeyValuePair<string, Manager.SequenceGOs> keyValuePair in dictionary)
				{
					global::UnityEngine.Object.Destroy(keyValuePair.Value.nearStart);
					global::UnityEngine.Object.Destroy(keyValuePair.Value.nearLoop);
					global::UnityEngine.Object.Destroy(keyValuePair.Value.nearEnd);
					global::UnityEngine.Object.Destroy(keyValuePair.Value.farStart);
					global::UnityEngine.Object.Destroy(keyValuePair.Value.farLoop);
					global::UnityEngine.Object.Destroy(keyValuePair.Value.farEnd);
				}
				Manager.sequenceOnEntity.Remove(entityId);
			}
			Dictionary<string, Manager.NearAndFarGO> dictionary2;
			if (Manager.loopingOnEntity.TryGetValue(entityId, out dictionary2))
			{
				foreach (KeyValuePair<string, Manager.NearAndFarGO> keyValuePair2 in dictionary2)
				{
					if (keyValuePair2.Value.near != null)
					{
						AudioSource component = keyValuePair2.Value.near.GetComponent<AudioSource>();
						if (component != null)
						{
							Manager.RemovePlayingAudioSource(component);
						}
						global::UnityEngine.Object.Destroy(keyValuePair2.Value.near);
					}
					if (keyValuePair2.Value.far != null)
					{
						AudioSource component2 = keyValuePair2.Value.far.GetComponent<AudioSource>();
						if (component2 != null)
						{
							Manager.RemovePlayingAudioSource(component2);
						}
						global::UnityEngine.Object.Destroy(keyValuePair2.Value.far);
					}
				}
				Manager.loopingOnEntity.Remove(entityId);
			}
		}

		// Token: 0x0600D06C RID: 53356 RVA: 0x004C0168 File Offset: 0x004BE368
		public static void SignalAI(Entity _entity, Vector3 _position, string _soundName, float volumeScale)
		{
			if (!(_entity is EntityPlayer))
			{
				return;
			}
			if (GameManager.Instance != null)
			{
				World world = GameManager.Instance.World;
				if (world != null && world.aiDirector != null)
				{
					world.aiDirector.OnSoundPlayedAtPosition(_entity.entityId, _position, _soundName, volumeScale);
				}
			}
		}

		// Token: 0x0600D06D RID: 53357 RVA: 0x004C01B5 File Offset: 0x004BE3B5
		[PublicizedFrom(EAccessModifier.Private)]
		public static void StripOffDirectories(ref string _name)
		{
			_name = Path.GetFileName(_name).ToLower();
		}

		// Token: 0x0600D06E RID: 53358 RVA: 0x004C01C5 File Offset: 0x004BE3C5
		public static void CameraChanged()
		{
			if (Manager.mainCamera)
			{
				Manager.DopplerCheckForMove();
			}
		}

		// Token: 0x0600D06F RID: 53359 RVA: 0x000027FC File Offset: 0x000009FC
		public static void FrameUpdate()
		{
		}

		// Token: 0x0600D070 RID: 53360 RVA: 0x004C01D8 File Offset: 0x004BE3D8
		public static void PlayButtonClick()
		{
			if (GameManager.Instance != null && GameManager.Instance.World != null)
			{
				Manager.PlayInsidePlayerHead("Sounds/Misc/buttonclick", -1, 0f, false, false);
			}
		}

		// Token: 0x0600D071 RID: 53361 RVA: 0x004C0208 File Offset: 0x004BE408
		public static void PlayXUiSound(AudioClip _sound, float _volume)
		{
			if (GameManager.Instance.UIAudioSource == null)
			{
				GameManager.Instance.UIAudioSource = GameManager.Instance.transform.gameObject.AddComponent<AudioSource>();
			}
			if (_sound != null && GameManager.Instance.UIAudioSource != null)
			{
				if (GameManager.Instance.World != null && Manager.LocalPlayer() != null)
				{
					GameManager.Instance.UIAudioSource.minDistance = 1f;
					GameManager.Instance.UIAudioSource.transform.position = Manager.LocalPlayer().transform.position;
				}
				GameManager.Instance.UIAudioSource.PlayOneShot(_sound, _volume);
			}
		}

		// Token: 0x0600D072 RID: 53362 RVA: 0x004C02C1 File Offset: 0x004BE4C1
		public static void PlaySource(AudioSource src)
		{
			if (src)
			{
				src.Play();
			}
		}

		// Token: 0x0600D073 RID: 53363 RVA: 0x004C02D1 File Offset: 0x004BE4D1
		public static void StopSource(AudioSource src)
		{
			if (src)
			{
				src.Stop();
			}
		}

		// Token: 0x0600D074 RID: 53364 RVA: 0x004C02E4 File Offset: 0x004BE4E4
		public void StopDistantLoopingPositionalSounds(Vector3 localPlayerPosition)
		{
			foreach (AudioSource audioSource in this.PositionalSoundsPlaying.GetComponentsInChildren<AudioSource>())
			{
				if (audioSource.loop && (audioSource.transform.position - localPlayerPosition).magnitude > audioSource.maxDistance)
				{
					Manager.StopSource(audioSource);
				}
			}
		}

		// Token: 0x0600D075 RID: 53365 RVA: 0x004C0340 File Offset: 0x004BE540
		public void RestartNearbyLoopingPositionalSounds(Vector3 localPlayerPosition)
		{
			foreach (AudioSource audioSource in this.PositionalSoundsPlaying.GetComponentsInChildren<AudioSource>())
			{
				if (audioSource.loop && !audioSource.isPlaying && (audioSource.transform.position - localPlayerPosition).magnitude <= audioSource.maxDistance)
				{
					Manager.PlaySource(audioSource);
				}
			}
		}

		// Token: 0x0600D076 RID: 53366 RVA: 0x004C03A2 File Offset: 0x004BE5A2
		public void Dispose()
		{
			if (Manager.ServerAudio != null)
			{
				Manager.ServerAudio.Dispose();
				Manager.ServerAudio = null;
			}
		}

		// Token: 0x0600D077 RID: 53367 RVA: 0x004C03BB File Offset: 0x004BE5BB
		public void AttachLocalPlayer(EntityPlayerLocal localPlayer, World world)
		{
			if (Manager.ServerAudio != null)
			{
				Manager.ServerAudio.AttachLocalPlayer(localPlayer);
			}
		}

		// Token: 0x0600D078 RID: 53368 RVA: 0x004C03CF File Offset: 0x004BE5CF
		public void EntityAddedToWorld(Entity entity, World world)
		{
			if (Manager.ServerAudio != null)
			{
				Manager.ServerAudio.EntityAddedToWorld(entity, world);
			}
		}

		// Token: 0x0600D079 RID: 53369 RVA: 0x004C03E4 File Offset: 0x004BE5E4
		public void EntityRemovedFromWorld(Entity entity, World world)
		{
			if (entity == null)
			{
				return;
			}
			if (Manager.ServerAudio != null)
			{
				Manager.ServerAudio.EntityRemovedFromWorld(entity, world);
			}
		}

		// Token: 0x0600D07A RID: 53370 RVA: 0x004C0403 File Offset: 0x004BE603
		public static void CreateServer()
		{
			if (Manager.ServerAudio == null && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				Manager.ServerAudio = new Server();
			}
		}

		// Token: 0x0600D07B RID: 53371 RVA: 0x004C0424 File Offset: 0x004BE624
		public static string GetFormattedSubtitleSpeaker(string subtitleId)
		{
			string text = "";
			SubtitleData subtitleData = Manager.GetSubtitleData(subtitleId);
			if (subtitleData != null && !string.IsNullOrEmpty(subtitleData.speakerLocId))
			{
				text = Localization.Get(subtitleData.speakerLocId, false, null);
				if (!string.IsNullOrEmpty(subtitleData.speakerColorId))
				{
					string subtitleSpeakerColor = Manager.GetSubtitleSpeakerColor(subtitleData.speakerColorId);
					text = string.Format("[{0}]{1}:[-]", subtitleSpeakerColor, text);
				}
			}
			return text;
		}

		// Token: 0x0600D07C RID: 53372 RVA: 0x004C0484 File Offset: 0x004BE684
		public static string GetFormattedSubtitle(string subtitleId)
		{
			string text = "";
			SubtitleData subtitleData = Manager.GetSubtitleData(subtitleId);
			if (subtitleData != null)
			{
				text = Localization.Get(subtitleData.contentLocId, false, null);
			}
			return text;
		}

		// Token: 0x0600D07D RID: 53373 RVA: 0x004C04B0 File Offset: 0x004BE6B0
		[PublicizedFrom(EAccessModifier.Private)]
		public static SubtitleData GetSubtitleData(string subtitleId)
		{
			SubtitleData subtitleData;
			if (Manager.subtitleCache.TryGetValue(subtitleId, out subtitleData))
			{
				return subtitleData;
			}
			Log.Error("Could not retrieve subtitle data for ID " + subtitleId);
			return null;
		}

		// Token: 0x0600D07E RID: 53374 RVA: 0x004C04E0 File Offset: 0x004BE6E0
		[PublicizedFrom(EAccessModifier.Private)]
		public static string GetSubtitleSpeakerColor(string speaker)
		{
			string text;
			if (Manager.subtitleSpeakerColorCache.TryGetValue(speaker, out text))
			{
				return text;
			}
			Log.Error("Could not retrieve subtitle speaker color for ID " + speaker);
			return "#FFFFFF";
		}

		// Token: 0x0600D07F RID: 53375 RVA: 0x004C0513 File Offset: 0x004BE713
		[PublicizedFrom(EAccessModifier.Private)]
		public static void updateCurrentListener()
		{
			Manager.currentListenerPosition = Manager.mainCamera.transform.position;
		}

		// Token: 0x04009EA7 RID: 40615
		public static Dictionary<string, AudioClip> audioClipAssetCache = new Dictionary<string, AudioClip>();

		// Token: 0x04009EA8 RID: 40616
		public static Dictionary<string, GameObject> audioSrcObjAssetCache = new Dictionary<string, GameObject>();

		// Token: 0x04009EA9 RID: 40617
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<string, bool> ignoreDistanceCheckSounds = new Dictionary<string, bool>();

		// Token: 0x04009EAA RID: 40618
		public static Dictionary<string, SubtitleData> subtitleCache = new Dictionary<string, SubtitleData>();

		// Token: 0x04009EAB RID: 40619
		public static Dictionary<string, string> subtitleSpeakerColorCache = new Dictionary<string, string>();

		// Token: 0x04009EAC RID: 40620
		public static GameRandom random;

		// Token: 0x04009EAD RID: 40621
		public static bool occlusionsOn;

		// Token: 0x04009EAE RID: 40622
		public const float OccludedVolumeMultiplier = 0.5f;

		// Token: 0x04009EAF RID: 40623
		public static Server ServerAudio;

		// Token: 0x04009EB0 RID: 40624
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool bCameraWasUnderwater;

		// Token: 0x04009EB1 RID: 40625
		[PublicizedFrom(EAccessModifier.Private)]
		public static int underwaterSoundID = -1;

		// Token: 0x04009EB2 RID: 40626
		public static Vector3 currentListenerPosition;

		// Token: 0x04009EB3 RID: 40627
		[PublicizedFrom(EAccessModifier.Private)]
		public static float fadeOutUpdateTime;

		// Token: 0x04009EB4 RID: 40628
		[PublicizedFrom(EAccessModifier.Private)]
		public GameObject PositionalSoundsPlaying;

		// Token: 0x04009EB5 RID: 40629
		[PublicizedFrom(EAccessModifier.Private)]
		public Transform PositionalSoundsPlayingT;

		// Token: 0x04009EB6 RID: 40630
		public bool bUseAltSounds;

		// Token: 0x04009EB7 RID: 40631
		[PublicizedFrom(EAccessModifier.Private)]
		public static Manager instance;

		// Token: 0x04009EB8 RID: 40632
		public static Dictionary<string, XmlData> audioData;

		// Token: 0x04009EB9 RID: 40633
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<int, Dictionary<string, Manager.NearAndFarGO>> loopingOnEntity;

		// Token: 0x04009EBA RID: 40634
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<int, Dictionary<string, Manager.NearAndFarGO>> fadingOutOnEntity;

		// Token: 0x04009EBB RID: 40635
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<Vector3, Dictionary<string, Manager.NearAndFarGO>> loopingOnPosition;

		// Token: 0x04009EBC RID: 40636
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<int, Dictionary<string, Manager.SequenceGOs>> sequenceOnEntity;

		// Token: 0x04009EBD RID: 40637
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<int, Dictionary<string, Manager.SequenceStopper>> stoppedEntitySequences;

		// Token: 0x04009EBE RID: 40638
		public static DictionaryList<string, List<AudioSource>> playAndCleanupSources;

		// Token: 0x04009EBF RID: 40639
		public static List<AudioSource> playingAudioSources;

		// Token: 0x04009EC0 RID: 40640
		[PublicizedFrom(EAccessModifier.Private)]
		public static Dictionary<int, Manager.Channels> playingOnEntity;

		// Token: 0x04009EC1 RID: 40641
		[PublicizedFrom(EAccessModifier.Private)]
		public static int dopplerDelay;

		// Token: 0x04009EC2 RID: 40642
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<Manager.DopplerItem> playingAudioSourceDopplers;

		// Token: 0x04009EC3 RID: 40643
		public static Dictionary<string, Manager.AudioSourceData> audioSourceDatas;

		// Token: 0x04009EC4 RID: 40644
		[PublicizedFrom(EAccessModifier.Private)]
		public static EntityPlayerLocal localPlayer;

		// Token: 0x04009EC5 RID: 40645
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cDopplerDist = 20f;

		// Token: 0x04009EC6 RID: 40646
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cPlayerPitchShift = 0.05f;

		// Token: 0x04009EC7 RID: 40647
		[PublicizedFrom(EAccessModifier.Private)]
		public static AudioSource uniqueSrc;

		// Token: 0x04009EC8 RID: 40648
		[PublicizedFrom(EAccessModifier.Private)]
		public static char[] convertChars = new char[] { '*', '#' };

		// Token: 0x04009EC9 RID: 40649
		[PublicizedFrom(EAccessModifier.Private)]
		public const int raycastMask = 65537;

		// Token: 0x04009ECA RID: 40650
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<int> listIntRemove = new List<int>();

		// Token: 0x04009ECB RID: 40651
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<string> listStringRemove = new List<string>();

		// Token: 0x04009ECC RID: 40652
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<string> removeSequenceStopper = new List<string>();

		// Token: 0x04009ECD RID: 40653
		[PublicizedFrom(EAccessModifier.Private)]
		public static Camera mainCamera;

		// Token: 0x04009ECE RID: 40654
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 cameraPos;

		// Token: 0x02001B1D RID: 6941
		[PublicizedFrom(EAccessModifier.Private)]
		public struct SequenceGOs
		{
			// Token: 0x04009ECF RID: 40655
			public GameObject nearStart;

			// Token: 0x04009ED0 RID: 40656
			public GameObject nearLoop;

			// Token: 0x04009ED1 RID: 40657
			public GameObject nearEnd;

			// Token: 0x04009ED2 RID: 40658
			public GameObject farStart;

			// Token: 0x04009ED3 RID: 40659
			public GameObject farLoop;

			// Token: 0x04009ED4 RID: 40660
			public GameObject farEnd;

			// Token: 0x04009ED5 RID: 40661
			public float longestClipLength;
		}

		// Token: 0x02001B1E RID: 6942
		[PublicizedFrom(EAccessModifier.Private)]
		public struct NearAndFarGO
		{
			// Token: 0x04009ED6 RID: 40662
			public GameObject near;

			// Token: 0x04009ED7 RID: 40663
			public GameObject far;
		}

		// Token: 0x02001B1F RID: 6943
		[PublicizedFrom(EAccessModifier.Private)]
		public struct Channels
		{
			// Token: 0x04009ED8 RID: 40664
			public int currentMouthPriority;

			// Token: 0x04009ED9 RID: 40665
			public List<AudioSource> mouth;

			// Token: 0x04009EDA RID: 40666
			public DictionaryList<string, List<AudioSource>> environment;
		}

		// Token: 0x02001B20 RID: 6944
		[PublicizedFrom(EAccessModifier.Private)]
		public struct DopplerItem
		{
			// Token: 0x04009EDB RID: 40667
			public AudioSource src;

			// Token: 0x04009EDC RID: 40668
			public float doppler;
		}

		// Token: 0x02001B21 RID: 6945
		public struct AudioSourceData
		{
			// Token: 0x04009EDD RID: 40669
			public float maxVolume;
		}

		// Token: 0x02001B22 RID: 6946
		public class SequenceStopper
		{
			// Token: 0x0600D081 RID: 53377 RVA: 0x004C05A4 File Offset: 0x004BE7A4
			public SequenceStopper(List<GameObject> audioSourceObjs, float _stopTime)
			{
				this.sequenceObjs = audioSourceObjs;
				this.stopTime = _stopTime;
			}

			// Token: 0x04009EDE RID: 40670
			public List<GameObject> sequenceObjs;

			// Token: 0x04009EDF RID: 40671
			public float stopTime;
		}
	}
}
