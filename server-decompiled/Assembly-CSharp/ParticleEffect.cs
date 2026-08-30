using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Audio;
using UnityEngine;

// Token: 0x020008B9 RID: 2233
public class ParticleEffect
{
	// Token: 0x06004097 RID: 16535 RVA: 0x00193FAE File Offset: 0x001921AE
	public static void Init()
	{
		ParticleEffect.RootT = GameObject.Find("/Particles").transform;
		Origin.Add(ParticleEffect.RootT, 0);
		ParticleEffect.entityParticles.Clear();
	}

	// Token: 0x06004098 RID: 16536 RVA: 0x00193FD9 File Offset: 0x001921D9
	public static IEnumerator LoadResources()
	{
		ParticleEffect.loadedTs.Clear();
		LoadManager.AddressableAssetsRequestTask<GameObject> loadTask = LoadManager.LoadAssetsFromAddressables<GameObject>("particleeffects", delegate(string address)
		{
			if (!address.EndsWith(".prefab"))
			{
				return false;
			}
			StringSpan.CharSplitEnumerator splitEnumerator = address.GetSplitEnumerator('/', StringSplitOptions.None);
			if (!splitEnumerator.MoveNext())
			{
				Log.Error("Particle effect at " + address + " did not have expected folder name");
				return false;
			}
			if (!splitEnumerator.MoveNext())
			{
				Log.Error("Particle effect at " + address + " did not have expected name format");
				return false;
			}
			StringSpan stringSpan = splitEnumerator.Current;
			StringSpan stringSpan2 = ParticleEffect.prefix;
			return stringSpan.IndexOf(stringSpan2) == 0;
		}, null, false, false);
		while (!loadTask.IsDone)
		{
			yield return null;
		}
		List<GameObject> list = new List<GameObject>();
		loadTask.CollectResults(list);
		foreach (GameObject gameObject in list)
		{
			string text = gameObject.name;
			text = text.Substring(ParticleEffect.prefix.Length);
			int num = ParticleEffect.ToId(text);
			if (ParticleEffect.loadedTs.ContainsKey(num))
			{
				Log.Error("Particle Effect " + text + " already exists! Skipping it!");
			}
			else
			{
				ParticleEffect.loadedTs.Add(num, gameObject.transform);
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06004099 RID: 16537 RVA: 0x00193FE4 File Offset: 0x001921E4
	public static void LoadAsset(string _path)
	{
		DataLoader.DataPathIdentifier dataPathIdentifier = DataLoader.ParseDataPathIdentifier(_path);
		if (!dataPathIdentifier.IsBundle)
		{
			return;
		}
		int num = ParticleEffect.ToId(_path);
		if (ParticleEffect.loadedTs.ContainsKey(num))
		{
			Log.Warning("Particle Effect {0} already exists! Skipping it!", new object[] { _path });
			return;
		}
		Transform transform = DataLoader.LoadAsset<Transform>(dataPathIdentifier, false);
		ParticleEffect.loadedTs.Add(num, transform);
	}

	// Token: 0x0600409A RID: 16538 RVA: 0x00194041 File Offset: 0x00192241
	public ParticleEffect()
	{
	}

	// Token: 0x0600409B RID: 16539 RVA: 0x00194050 File Offset: 0x00192250
	public ParticleEffect(ParticleType _type, Vector3 _pos, float _lightValue, Color _color)
	{
		this.type = _type;
		this.pos = _pos;
		this.lightValue = _lightValue;
		this.color = _color;
	}

	// Token: 0x0600409C RID: 16540 RVA: 0x0019407C File Offset: 0x0019227C
	public ParticleEffect(ParticleType _type, Vector3 _pos, float _lightValue, Color _color, Transform _parentTransform)
		: this(_type, _pos, _lightValue, _color)
	{
		this.SetParent(_parentTransform);
	}

	// Token: 0x0600409D RID: 16541 RVA: 0x00194091 File Offset: 0x00192291
	public ParticleEffect(ParticleType _type, Vector3 _pos, float _lightValue, Color _color, string _soundName, Transform _parentTransform)
		: this(_type, _pos, _lightValue, _color)
	{
		this.soundName = _soundName;
		this.SetParent(_parentTransform);
	}

	// Token: 0x0600409E RID: 16542 RVA: 0x001940B0 File Offset: 0x001922B0
	public ParticleEffect(string _name, Vector3 _pos, Quaternion _rot, float _lightValue, Color _color)
		: this(_name, _pos, _rot, _lightValue, _color, null, null, 1f, "")
	{
	}

	// Token: 0x0600409F RID: 16543 RVA: 0x001940D8 File Offset: 0x001922D8
	public ParticleEffect(string _name, Vector3 _pos, Quaternion _rot, float _lightValue, Color _color, string _soundName, Transform _parentTransform, float _volumeScale = 1f, string _additionalHitSound = "")
		: this(_name, _pos, _lightValue, _color, _soundName, _parentTransform, false, 1f, "")
	{
		this.rot = _rot;
		this.volumeScale = _volumeScale;
		this.additionalHitSoundName = _additionalHitSound;
	}

	// Token: 0x060040A0 RID: 16544 RVA: 0x00194118 File Offset: 0x00192318
	public ParticleEffect(string _name, Vector3 _pos, float _lightValue, Color _color, string _soundName, Transform _parentTransform, bool _OLDCreateColliders, float _volumeScale = 1f, string _additionalHitSound = "")
		: this(ParticleType.Dynamic, _pos, _lightValue, _color)
	{
		this.ParticleId = ((_name != null) ? ParticleEffect.ToId(_name) : 0);
		this.debugName = _name;
		this.soundName = _soundName;
		this.volumeScale = _volumeScale;
		this.additionalHitSoundName = _additionalHitSound;
		this.SetParent(_parentTransform);
	}

	// Token: 0x060040A1 RID: 16545 RVA: 0x00194169 File Offset: 0x00192369
	public ParticleEffect(string _name, Vector3 _pos, float _lightValue, Color _color, string _soundName, int _parentEntityId, ParticleEffect.Attachment _attachment)
		: this(ParticleType.Dynamic, _pos, _lightValue, _color)
	{
		this.ParticleId = ((_name != null) ? ParticleEffect.ToId(_name) : 0);
		this.debugName = _name;
		this.soundName = _soundName;
		this.SetParent(_parentEntityId, _attachment);
	}

	// Token: 0x060040A2 RID: 16546 RVA: 0x000DA7A4 File Offset: 0x000D89A4
	public static int ToId(string _name)
	{
		return _name.GetHashCode();
	}

	// Token: 0x060040A3 RID: 16547 RVA: 0x001941A4 File Offset: 0x001923A4
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform GetParentTransform()
	{
		if (!this.parentTransform && this.parentEntityId != -1)
		{
			Entity entity;
			GameManager.Instance.World.Entities.dict.TryGetValue(this.parentEntityId, out entity);
			if (entity)
			{
				this.parentTransform = entity.transform;
				if (this.attachment != ParticleEffect.Attachment.None)
				{
					ParticleEffect.Attachment attachment = this.attachment;
					Transform transform;
					if (attachment != ParticleEffect.Attachment.Head)
					{
						if (attachment != ParticleEffect.Attachment.Pelvis)
						{
							transform = null;
						}
						else
						{
							transform = entity.emodel.GetPelvisTransform();
						}
					}
					else
					{
						transform = entity.emodel.GetHeadTransform();
					}
					if (transform)
					{
						this.parentTransform = transform;
					}
				}
			}
		}
		return this.parentTransform;
	}

	// Token: 0x060040A4 RID: 16548 RVA: 0x0019424C File Offset: 0x0019244C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetParent(Transform _parentT)
	{
		this.parentTransform = null;
		this.attachment = ParticleEffect.Attachment.None;
		if (_parentT)
		{
			Entity component = _parentT.GetComponent<Entity>();
			this.parentEntityId = (component ? component.entityId : (-1));
		}
	}

	// Token: 0x060040A5 RID: 16549 RVA: 0x0019428D File Offset: 0x0019248D
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetParent(int _entityId, ParticleEffect.Attachment _attachment = ParticleEffect.Attachment.None)
	{
		this.parentTransform = null;
		this.attachment = _attachment;
		this.parentEntityId = _entityId;
	}

	// Token: 0x060040A6 RID: 16550 RVA: 0x001942A4 File Offset: 0x001924A4
	public static Transform GetDynamicTransform(int _particleId)
	{
		Transform transform;
		if (ParticleEffect.loadedTs.TryGetValue(_particleId, out transform))
		{
			return transform;
		}
		Log.Error(string.Format("Unknown particle effect: {0}", _particleId));
		return null;
	}

	// Token: 0x060040A7 RID: 16551 RVA: 0x001942D8 File Offset: 0x001924D8
	public static bool IsAvailable(string _name)
	{
		return ParticleEffect.loadedTs.ContainsKey(ParticleEffect.ToId(_name));
	}

	// Token: 0x060040A8 RID: 16552 RVA: 0x001942EC File Offset: 0x001924EC
	public void Read(BinaryReader _br)
	{
		this.ParticleId = _br.ReadInt32();
		this.pos = StreamUtils.ReadVector3(_br);
		this.rot = StreamUtils.ReadQuaterion(_br);
		this.color = StreamUtils.ReadColor32(_br);
		this.soundName = _br.ReadString();
		if (this.soundName == string.Empty)
		{
			this.soundName = null;
		}
		this.additionalHitSoundName = _br.ReadString();
		if (this.additionalHitSoundName == string.Empty)
		{
			this.additionalHitSoundName = null;
		}
		this.volumeScale = _br.ReadSingle();
		this.parentEntityId = _br.ReadInt32();
		this.attachment = (ParticleEffect.Attachment)_br.ReadByte();
	}

	// Token: 0x060040A9 RID: 16553 RVA: 0x00194398 File Offset: 0x00192598
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(this.ParticleId);
		StreamUtils.Write(_bw, this.pos);
		StreamUtils.Write(_bw, this.rot);
		StreamUtils.WriteColor32(_bw, this.color);
		_bw.Write((this.soundName != null) ? this.soundName : string.Empty);
		_bw.Write((this.additionalHitSoundName != null) ? this.additionalHitSoundName : string.Empty);
		_bw.Write(this.volumeScale);
		_bw.Write(this.parentEntityId);
		_bw.Write((byte)this.attachment);
	}

	// Token: 0x060040AA RID: 16554 RVA: 0x00194430 File Offset: 0x00192630
	public static Transform SpawnParticleEffect(ParticleEffect _pe, int _entityThatCausedIt, bool _forceCreation = false, bool _isWorldPos = false)
	{
		ParticleEffect.PlaySoundInServer(_entityThatCausedIt, _pe.pos, _pe.soundName, _pe.volumeScale);
		ParticleEffect.PlaySoundInServer(_entityThatCausedIt, _pe.pos, _pe.additionalHitSoundName, _pe.volumeScale);
		if (GameManager.IsDedicatedServer)
		{
			return null;
		}
		ParticleEffect.PlaySoundInClient(_entityThatCausedIt, _pe.pos, _pe.soundName, _pe.volumeScale);
		ParticleEffect.PlaySoundInClient(_entityThatCausedIt, _pe.pos, _pe.additionalHitSoundName, _pe.volumeScale);
		List<ParticleEffect.EntityData> list = null;
		Transform transform;
		if (_entityThatCausedIt != -1 && !_forceCreation)
		{
			int num = -1;
			if (ParticleEffect.entityParticles.TryGetValue(_entityThatCausedIt, out list))
			{
				int num2 = 0;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					if (!list[i].t)
					{
						list.RemoveAt(i);
						num--;
					}
					else if (list[i].id == _pe.ParticleId && ++num2 >= 3)
					{
						num = i;
					}
				}
			}
			else
			{
				list = new List<ParticleEffect.EntityData>();
				ParticleEffect.entityParticles[_entityThatCausedIt] = list;
			}
			if (num >= 0 && _pe.attachment == ParticleEffect.Attachment.None)
			{
				ParticleEffect.EntityData entityData = list[num];
				list.RemoveAt(num);
				list.Add(entityData);
				transform = entityData.t;
				transform.position = _pe.pos - Origin.position;
				transform.rotation = _pe.rot;
				foreach (ParticleSystem particleSystem in transform.GetComponentsInChildren<ParticleSystem>())
				{
					particleSystem.Clear();
					particleSystem.Play();
				}
				TemporaryObject[] componentsInChildren2 = transform.GetComponentsInChildren<TemporaryObject>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					componentsInChildren2[j].Restart();
				}
				return null;
			}
		}
		Transform dynamicTransform = ParticleEffect.GetDynamicTransform(_pe.ParticleId);
		if (!dynamicTransform)
		{
			return null;
		}
		if (_isWorldPos)
		{
			transform = global::UnityEngine.Object.Instantiate<Transform>(dynamicTransform, _pe.pos - Origin.position, _pe.rot);
		}
		else
		{
			transform = global::UnityEngine.Object.Instantiate<Transform>(dynamicTransform, _pe.pos, _pe.rot);
		}
		if (!transform)
		{
			return null;
		}
		if (list != null)
		{
			ParticleEffect.EntityData entityData2;
			entityData2.id = _pe.ParticleId;
			entityData2.t = transform;
			list.Add(entityData2);
		}
		foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
		{
			if (renderer.GetComponent<ParticleSystem>() == null)
			{
				renderer.material.SetColor("_Color", _pe.color);
			}
		}
		if (_pe.opqueTextureId != 0)
		{
			Material material = transform.GetComponent<ParticleSystem>().GetComponent<Renderer>().material;
			TextureAtlas textureAtlas = MeshDescription.meshes[0].textureAtlas;
			material.SetTexture("_MainTex", textureAtlas.diffuseTexture);
			material.SetTexture("_BumpMap", textureAtlas.normalTexture);
			material.SetFloat("_TexI", (float)textureAtlas.uvMapping[_pe.opqueTextureId].index);
			if (material.HasProperty("_OffsetUV"))
			{
				Rect uv = textureAtlas.uvMapping[_pe.opqueTextureId].uv;
				material.SetVector("_OffsetUV", new Vector4(uv.x, uv.y, uv.width, uv.height));
			}
		}
		Transform transform2 = _pe.GetParentTransform();
		if (transform2)
		{
			transform.SetParent(transform2, false);
			if (_pe.attachment != ParticleEffect.Attachment.None)
			{
				transform.localPosition = _pe.pos;
			}
			else
			{
				transform.localPosition = Vector3.zero;
			}
			transform.localRotation = Quaternion.identity;
		}
		else
		{
			transform.SetParent(ParticleEffect.RootT, false);
		}
		return transform;
	}

	// Token: 0x060040AB RID: 16555 RVA: 0x001947C1 File Offset: 0x001929C1
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool PlaySoundInServer(int entityId, Vector3 position, string soundName, float volumeScale)
	{
		if (!string.IsNullOrEmpty(soundName) && SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			GameManager.Instance.World.aiDirector.OnSoundPlayedAtPosition(entityId, position, soundName, volumeScale);
			return true;
		}
		return false;
	}

	// Token: 0x060040AC RID: 16556 RVA: 0x001947F2 File Offset: 0x001929F2
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool PlaySoundInClient(int entityId, Vector3 position, string soundName, float volumeScale)
	{
		if (!string.IsNullOrEmpty(soundName))
		{
			Manager.Play(position, soundName, entityId, false, volumeScale);
			return true;
		}
		return false;
	}

	// Token: 0x0400344B RID: 13387
	[PublicizedFrom(EAccessModifier.Private)]
	public static string prefix = "p_";

	// Token: 0x0400344C RID: 13388
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cEntitySameParticleMax = 3;

	// Token: 0x0400344D RID: 13389
	[PublicizedFrom(EAccessModifier.Private)]
	public ParticleType type;

	// Token: 0x0400344E RID: 13390
	[PublicizedFrom(EAccessModifier.Private)]
	public ParticleEffect.Attachment attachment;

	// Token: 0x0400344F RID: 13391
	public Vector3 pos;

	// Token: 0x04003450 RID: 13392
	public Quaternion rot;

	// Token: 0x04003451 RID: 13393
	public Color color;

	// Token: 0x04003452 RID: 13394
	public float lightValue;

	// Token: 0x04003453 RID: 13395
	public int ParticleId;

	// Token: 0x04003454 RID: 13396
	public string soundName;

	// Token: 0x04003455 RID: 13397
	public float volumeScale;

	// Token: 0x04003456 RID: 13398
	public string additionalHitSoundName;

	// Token: 0x04003457 RID: 13399
	public int opqueTextureId;

	// Token: 0x04003458 RID: 13400
	[PublicizedFrom(EAccessModifier.Private)]
	public int parentEntityId = -1;

	// Token: 0x04003459 RID: 13401
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform parentTransform;

	// Token: 0x0400345A RID: 13402
	public string debugName;

	// Token: 0x0400345B RID: 13403
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform RootT;

	// Token: 0x0400345C RID: 13404
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, Transform> loadedTs = new Dictionary<int, Transform>();

	// Token: 0x0400345D RID: 13405
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, List<ParticleEffect.EntityData>> entityParticles = new Dictionary<int, List<ParticleEffect.EntityData>>();

	// Token: 0x020008BA RID: 2234
	public enum Attachment : byte
	{
		// Token: 0x0400345F RID: 13407
		None,
		// Token: 0x04003460 RID: 13408
		Head,
		// Token: 0x04003461 RID: 13409
		Pelvis
	}

	// Token: 0x020008BB RID: 2235
	public struct EntityData
	{
		// Token: 0x04003462 RID: 13410
		public int id;

		// Token: 0x04003463 RID: 13411
		public Transform t;
	}
}
