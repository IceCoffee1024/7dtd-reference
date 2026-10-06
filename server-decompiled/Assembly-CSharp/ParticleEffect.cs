using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Audio;
using UnityEngine;

// Token: 0x020008CA RID: 2250
public class ParticleEffect
{
	// Token: 0x06004092 RID: 16530 RVA: 0x0019618A File Offset: 0x0019438A
	public static void Init()
	{
		ParticleEffect.RootT = GameObject.Find("/Particles").transform;
		Origin.Add(ParticleEffect.RootT, 0);
		ParticleEffect.entityParticles.Clear();
	}

	// Token: 0x06004093 RID: 16531 RVA: 0x001961B5 File Offset: 0x001943B5
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

	// Token: 0x06004094 RID: 16532 RVA: 0x001961C0 File Offset: 0x001943C0
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

	// Token: 0x06004095 RID: 16533 RVA: 0x0019621D File Offset: 0x0019441D
	public ParticleEffect()
	{
	}

	// Token: 0x06004096 RID: 16534 RVA: 0x0019622C File Offset: 0x0019442C
	public ParticleEffect(ParticleType _type, Vector3 _pos, float _lightValue, Color _color)
	{
		this.type = _type;
		this.pos = _pos;
		this.lightValue = _lightValue;
		this.color = _color;
	}

	// Token: 0x06004097 RID: 16535 RVA: 0x00196258 File Offset: 0x00194458
	public ParticleEffect(ParticleType _type, Vector3 _pos, float _lightValue, Color _color, Transform _parentTransform)
		: this(_type, _pos, _lightValue, _color)
	{
		this.SetParent(_parentTransform);
	}

	// Token: 0x06004098 RID: 16536 RVA: 0x0019626D File Offset: 0x0019446D
	public ParticleEffect(ParticleType _type, Vector3 _pos, float _lightValue, Color _color, string _soundName, Transform _parentTransform)
		: this(_type, _pos, _lightValue, _color)
	{
		this.soundName = _soundName;
		this.SetParent(_parentTransform);
	}

	// Token: 0x06004099 RID: 16537 RVA: 0x0019628C File Offset: 0x0019448C
	public ParticleEffect(string _name, Vector3 _pos, Quaternion _rot, float _lightValue, Color _color)
		: this(_name, _pos, _rot, _lightValue, _color, null, null, 1f, "")
	{
	}

	// Token: 0x0600409A RID: 16538 RVA: 0x001962B4 File Offset: 0x001944B4
	public ParticleEffect(string _name, Vector3 _pos, Quaternion _rot, float _lightValue, Color _color, string _soundName, Transform _parentTransform, float _volumeScale = 1f, string _additionalHitSound = "")
		: this(_name, _pos, _lightValue, _color, _soundName, _parentTransform, false, 1f, "")
	{
		this.rot = _rot;
		this.volumeScale = _volumeScale;
		this.additionalHitSoundName = _additionalHitSound;
	}

	// Token: 0x0600409B RID: 16539 RVA: 0x001962F4 File Offset: 0x001944F4
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

	// Token: 0x0600409C RID: 16540 RVA: 0x00196345 File Offset: 0x00194545
	public ParticleEffect(string _name, Vector3 _pos, float _lightValue, Color _color, string _soundName, int _parentEntityId, ParticleEffect.Attachment _attachment)
		: this(ParticleType.Dynamic, _pos, _lightValue, _color)
	{
		this.ParticleId = ((_name != null) ? ParticleEffect.ToId(_name) : 0);
		this.debugName = _name;
		this.soundName = _soundName;
		this.SetParent(_parentEntityId, _attachment);
	}

	// Token: 0x0600409D RID: 16541 RVA: 0x000DB224 File Offset: 0x000D9424
	public static int ToId(string _name)
	{
		return _name.GetHashCode();
	}

	// Token: 0x0600409E RID: 16542 RVA: 0x00196380 File Offset: 0x00194580
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

	// Token: 0x0600409F RID: 16543 RVA: 0x00196428 File Offset: 0x00194628
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

	// Token: 0x060040A0 RID: 16544 RVA: 0x00196469 File Offset: 0x00194669
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetParent(int _entityId, ParticleEffect.Attachment _attachment = ParticleEffect.Attachment.None)
	{
		this.parentTransform = null;
		this.attachment = _attachment;
		this.parentEntityId = _entityId;
	}

	// Token: 0x060040A1 RID: 16545 RVA: 0x00196480 File Offset: 0x00194680
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

	// Token: 0x060040A2 RID: 16546 RVA: 0x001964B4 File Offset: 0x001946B4
	public static bool IsAvailable(string _name)
	{
		return ParticleEffect.loadedTs.ContainsKey(ParticleEffect.ToId(_name));
	}

	// Token: 0x060040A3 RID: 16547 RVA: 0x001964C8 File Offset: 0x001946C8
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

	// Token: 0x060040A4 RID: 16548 RVA: 0x00196574 File Offset: 0x00194774
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

	// Token: 0x060040A5 RID: 16549 RVA: 0x0019660C File Offset: 0x0019480C
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

	// Token: 0x060040A6 RID: 16550 RVA: 0x0019699D File Offset: 0x00194B9D
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

	// Token: 0x060040A7 RID: 16551 RVA: 0x001969CE File Offset: 0x00194BCE
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

	// Token: 0x040034C0 RID: 13504
	[PublicizedFrom(EAccessModifier.Private)]
	public static string prefix = "p_";

	// Token: 0x040034C1 RID: 13505
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cEntitySameParticleMax = 3;

	// Token: 0x040034C2 RID: 13506
	[PublicizedFrom(EAccessModifier.Private)]
	public ParticleType type;

	// Token: 0x040034C3 RID: 13507
	[PublicizedFrom(EAccessModifier.Private)]
	public ParticleEffect.Attachment attachment;

	// Token: 0x040034C4 RID: 13508
	public Vector3 pos;

	// Token: 0x040034C5 RID: 13509
	public Quaternion rot;

	// Token: 0x040034C6 RID: 13510
	public Color color;

	// Token: 0x040034C7 RID: 13511
	public float lightValue;

	// Token: 0x040034C8 RID: 13512
	public int ParticleId;

	// Token: 0x040034C9 RID: 13513
	public string soundName;

	// Token: 0x040034CA RID: 13514
	public float volumeScale;

	// Token: 0x040034CB RID: 13515
	public string additionalHitSoundName;

	// Token: 0x040034CC RID: 13516
	public int opqueTextureId;

	// Token: 0x040034CD RID: 13517
	[PublicizedFrom(EAccessModifier.Private)]
	public int parentEntityId = -1;

	// Token: 0x040034CE RID: 13518
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform parentTransform;

	// Token: 0x040034CF RID: 13519
	public string debugName;

	// Token: 0x040034D0 RID: 13520
	[PublicizedFrom(EAccessModifier.Private)]
	public static Transform RootT;

	// Token: 0x040034D1 RID: 13521
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, Transform> loadedTs = new Dictionary<int, Transform>();

	// Token: 0x040034D2 RID: 13522
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, List<ParticleEffect.EntityData>> entityParticles = new Dictionary<int, List<ParticleEffect.EntityData>>();

	// Token: 0x020008CB RID: 2251
	public enum Attachment : byte
	{
		// Token: 0x040034D4 RID: 13524
		None,
		// Token: 0x040034D5 RID: 13525
		Head,
		// Token: 0x040034D6 RID: 13526
		Pelvis
	}

	// Token: 0x020008CC RID: 2252
	public struct EntityData
	{
		// Token: 0x040034D7 RID: 13527
		public int id;

		// Token: 0x040034D8 RID: 13528
		public Transform t;
	}
}
