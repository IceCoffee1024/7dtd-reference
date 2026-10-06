using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C23 RID: 3107
public class DecoChunk
{
	// Token: 0x06005E5E RID: 24158 RVA: 0x0024E73C File Offset: 0x0024C93C
	public DecoChunk(int _x, int _z, int _drawX, int _drawZ)
	{
		this.Reset(_x, _z, _drawX, _drawZ);
	}

	// Token: 0x06005E5F RID: 24159 RVA: 0x0024E788 File Offset: 0x0024C988
	public void Reset(int _x, int _z, int _drawX, int _drawZ)
	{
		this.decoChunkX = _x;
		this.decoChunkZ = _z;
		this.drawX = _drawX;
		this.drawZ = _drawZ;
		this.decosPerSmallChunks.Clear();
		this.isDecorated = false;
		this.isModelsUpdated = false;
		this.isGameObjectUpdated = false;
	}

	// Token: 0x06005E60 RID: 24160 RVA: 0x0024E7C8 File Offset: 0x0024C9C8
	public void RestoreGeneratedDecos(Predicate<DecoObject> decoObjectValidator = null)
	{
		foreach (long num in this.decosPerSmallChunks.Keys)
		{
			this.RestoreGeneratedDecos(num, decoObjectValidator);
		}
	}

	// Token: 0x06005E61 RID: 24161 RVA: 0x0024E824 File Offset: 0x0024CA24
	public void RestoreGeneratedDecos(long smallChunkKey, Predicate<DecoObject> decoObjectValidator = null)
	{
		List<DecoObject> list;
		if (this.decosPerSmallChunks.TryGetValue(smallChunkKey, out list))
		{
			for (int i = list.Count - 1; i >= 0; i--)
			{
				DecoObject decoObject = list[i];
				if (decoObjectValidator == null || decoObjectValidator(decoObject))
				{
					switch (decoObject.state)
					{
					case DecoState.GeneratedInactive:
						decoObject.state = DecoState.GeneratedActive;
						this.isModelsUpdated = false;
						break;
					case DecoState.Dynamic:
						this.RemoveDecoObject(decoObject);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06005E62 RID: 24162 RVA: 0x002334FC File Offset: 0x002316FC
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int MakeKey16(int _x, int _z)
	{
		return (_x << 16) | (_z & 65535);
	}

	// Token: 0x06005E63 RID: 24163 RVA: 0x0024E89A File Offset: 0x0024CA9A
	public static int ToDecoChunkPos(float _worldPos)
	{
		return Utils.Fastfloor(_worldPos / 128f);
	}

	// Token: 0x06005E64 RID: 24164 RVA: 0x0024E8A8 File Offset: 0x0024CAA8
	public static int ToDecoChunkPos(int _worldPos)
	{
		if (_worldPos >= 0)
		{
			return _worldPos / 128;
		}
		return (_worldPos - 128 + 1) / 128;
	}

	// Token: 0x06005E65 RID: 24165 RVA: 0x0024E8C8 File Offset: 0x0024CAC8
	public void UpdateGameObject()
	{
		if (!this.rootObj)
		{
			this.rootObj = new GameObject();
		}
		this.SetVisible(true);
		this.rootObj.name = "DecoC_" + this.decoChunkX.ToString() + "_" + this.decoChunkZ.ToString();
		this.rootObj.transform.position = new Vector3((float)(this.drawX * 128), 0f, (float)(this.drawZ * 128)) - Origin.position;
		this.isGameObjectUpdated = true;
	}

	// Token: 0x06005E66 RID: 24166 RVA: 0x0024E969 File Offset: 0x0024CB69
	public IEnumerator UpdateModels(World _world, MicroStopwatch ms)
	{
		this.SetVisible(true);
		foreach (KeyValuePair<long, List<DecoObject>> keyValuePair in this.decosPerSmallChunks)
		{
			List<DecoObject> value = keyValuePair.Value;
			for (int i = 0; i < value.Count; i++)
			{
				DecoObject decoObject = value[i];
				if (decoObject.state != DecoState.GeneratedInactive && !decoObject.go && decoObject.asyncItem == null)
				{
					string modelName = decoObject.GetModelName();
					List<DecoObject> list;
					if (!this.models.TryGetValue(modelName, out list))
					{
						list = new List<DecoObject>();
						this.models.Add(modelName, list);
					}
					list.Add(decoObject);
				}
			}
		}
		foreach (KeyValuePair<string, List<DecoObject>> keyValuePair2 in this.models)
		{
			List<DecoObject> value2 = keyValuePair2.Value;
			GameObjectPool.AsyncItem objectsForTypeAsync = GameObjectPool.Instance.GetObjectsForTypeAsync(keyValuePair2.Key, value2.Count, new GameObjectPool.CreateAsyncCallback(this.CreateGameObjectCallback), value2);
			if (objectsForTypeAsync != null)
			{
				this.asyncItems.Add(objectsForTypeAsync);
				for (int j = 0; j < value2.Count; j++)
				{
					value2[j].asyncItem = objectsForTypeAsync;
				}
			}
			if (ms.ElapsedMicroseconds > 900L)
			{
				yield return null;
				ms.ResetAndRestart();
			}
		}
		Dictionary<string, List<DecoObject>>.Enumerator enumerator2 = default(Dictionary<string, List<DecoObject>>.Enumerator);
		this.models.Clear();
		this.isModelsUpdated = true;
		yield break;
		yield break;
	}

	// Token: 0x06005E67 RID: 24167 RVA: 0x0024E980 File Offset: 0x0024CB80
	public void CreateGameObjectCallback(object _userData, global::UnityEngine.Object[] _objs, int _objsCount, bool _isAsync)
	{
		List<DecoObject> list = (List<DecoObject>)_userData;
		Transform transform = this.rootObj.transform;
		for (int i = 0; i < _objsCount; i++)
		{
			GameObject gameObject = (GameObject)_objs[i];
			list[i].CreateGameObjectCallback(gameObject, transform, _isAsync);
			this.occlusionTs.Add(gameObject.transform);
		}
		if (this.occlusionTs.Count > 0)
		{
			if (OcclusionManager.Instance.cullDecorations)
			{
				OcclusionManager.Instance.AddDeco(this, this.occlusionTs);
			}
			this.occlusionTs.Clear();
		}
	}

	// Token: 0x06005E68 RID: 24168 RVA: 0x0024EA0C File Offset: 0x0024CC0C
	public void AddDecoObject(DecoObject _decoObject, bool _tryInstantiate = false)
	{
		long num = WorldChunkCache.MakeChunkKey(World.toChunkXZ(_decoObject.pos.x), World.toChunkXZ(_decoObject.pos.z));
		List<DecoObject> list;
		if (!this.decosPerSmallChunks.TryGetValue(num, out list))
		{
			list = new List<DecoObject>(64);
			this.decosPerSmallChunks.Add(num, list);
		}
		list.Add(_decoObject);
		if (_tryInstantiate)
		{
			if (ThreadManager.IsMainThread() && this.rootObj)
			{
				_decoObject.CreateGameObject(this, this.rootObj.transform);
				if (OcclusionManager.Instance.cullDecorations && _decoObject.go)
				{
					this.occlusionTs.Add(_decoObject.go.transform);
					OcclusionManager.Instance.AddDeco(this, this.occlusionTs);
					this.occlusionTs.Clear();
					return;
				}
			}
			else
			{
				this.isModelsUpdated = false;
			}
		}
	}

	// Token: 0x06005E69 RID: 24169 RVA: 0x0024EAE8 File Offset: 0x0024CCE8
	public DecoObject GetDecoObjectAt(Vector3i _worldBlockPos)
	{
		long num = WorldChunkCache.MakeChunkKey(World.toChunkXZ(_worldBlockPos.x), World.toChunkXZ(_worldBlockPos.z));
		List<DecoObject> list;
		if (!this.decosPerSmallChunks.TryGetValue(num, out list))
		{
			return null;
		}
		foreach (DecoObject decoObject in list)
		{
			if (decoObject.pos.x == _worldBlockPos.x && decoObject.pos.z == _worldBlockPos.z && decoObject.state != DecoState.GeneratedInactive)
			{
				return decoObject;
			}
		}
		return null;
	}

	// Token: 0x06005E6A RID: 24170 RVA: 0x0024EB98 File Offset: 0x0024CD98
	public bool RemoveDecoObject(Vector3i _worldBlockPos)
	{
		DecoObject decoObjectAt = this.GetDecoObjectAt(_worldBlockPos);
		if (decoObjectAt == null)
		{
			return false;
		}
		this.RemoveDecoObject(decoObjectAt);
		return true;
	}

	// Token: 0x06005E6B RID: 24171 RVA: 0x0024EBBC File Offset: 0x0024CDBC
	public void RemoveDecoObject(DecoObject deco)
	{
		if (deco.state == DecoState.Dynamic)
		{
			long num = WorldChunkCache.MakeChunkKey(World.toChunkXZ(deco.pos.x), World.toChunkXZ(deco.pos.z));
			List<DecoObject> list;
			if (this.decosPerSmallChunks.TryGetValue(num, out list))
			{
				list.Remove(deco);
			}
		}
		else
		{
			deco.state = DecoState.GeneratedInactive;
		}
		if (OcclusionManager.Instance.cullDecorations && deco.go)
		{
			OcclusionManager.Instance.RemoveDeco(this, deco.go.transform);
		}
		deco.Destroy();
	}

	// Token: 0x06005E6C RID: 24172 RVA: 0x0024EC50 File Offset: 0x0024CE50
	public void ClearDecoObjectsInArea(Vector3i _boundsPosition, Vector3i _boundsSize)
	{
		Vector3i vector3i = _boundsPosition + _boundsSize;
		int num = World.toChunkXZ(_boundsPosition.x);
		int num2 = World.toChunkXZ(_boundsPosition.z);
		int num3 = World.toChunkXZ(vector3i.x);
		int num4 = World.toChunkXZ(vector3i.z);
		for (int i = num; i <= num3; i++)
		{
			for (int j = num2; j <= num4; j++)
			{
				long num5 = WorldChunkCache.MakeChunkKey(i, j);
				List<DecoObject> list;
				if (this.decosPerSmallChunks.TryGetValue(num5, out list))
				{
					for (int k = list.Count - 1; k >= 0; k--)
					{
						DecoObject decoObject = list[k];
						Vector3i pos = decoObject.pos;
						if (pos.x >= _boundsPosition.x && pos.x < vector3i.x && pos.y >= _boundsPosition.y && pos.y < vector3i.y && pos.z >= _boundsPosition.z && pos.z < vector3i.z)
						{
							if (OcclusionManager.Instance.cullDecorations && decoObject.go)
							{
								OcclusionManager.Instance.RemoveDeco(this, decoObject.go.transform);
							}
							decoObject.Destroy();
							list.RemoveAt(k);
						}
					}
					if (list.Count == 0)
					{
						this.decosPerSmallChunks.Remove(num5);
					}
				}
			}
		}
	}

	// Token: 0x06005E6D RID: 24173 RVA: 0x0024EDCC File Offset: 0x0024CFCC
	public void Destroy()
	{
		if (OcclusionManager.Instance.cullDecorations)
		{
			OcclusionManager.Instance.RemoveDecoChunk(this);
		}
		foreach (KeyValuePair<long, List<DecoObject>> keyValuePair in this.decosPerSmallChunks)
		{
			List<DecoObject> value = keyValuePair.Value;
			for (int i = 0; i < value.Count; i++)
			{
				value[i].Destroy();
			}
		}
		for (int j = 0; j < this.asyncItems.Count; j++)
		{
			GameObjectPool.Instance.CancelAsync(this.asyncItems[j]);
		}
		this.asyncItems.Clear();
		this.isModelsUpdated = false;
		this.isGameObjectUpdated = false;
		global::UnityEngine.Object.Destroy(this.rootObj);
	}

	// Token: 0x06005E6E RID: 24174 RVA: 0x0024EEAC File Offset: 0x0024D0AC
	public void SetVisible(bool _bVisible)
	{
		if (this.rootObj && this.rootObj.activeSelf != _bVisible)
		{
			this.rootObj.SetActive(_bVisible);
		}
	}

	// Token: 0x06005E6F RID: 24175 RVA: 0x0024EED5 File Offset: 0x0024D0D5
	public override string ToString()
	{
		return string.Format("DecoChunk {0},{1}", this.decoChunkX, this.decoChunkZ);
	}

	// Token: 0x04004967 RID: 18791
	public int decoChunkX;

	// Token: 0x04004968 RID: 18792
	public int decoChunkZ;

	// Token: 0x04004969 RID: 18793
	public int drawX;

	// Token: 0x0400496A RID: 18794
	public int drawZ;

	// Token: 0x0400496B RID: 18795
	public bool isDecorated;

	// Token: 0x0400496C RID: 18796
	public bool isModelsUpdated;

	// Token: 0x0400496D RID: 18797
	public bool isGameObjectUpdated;

	// Token: 0x0400496E RID: 18798
	public GameObject rootObj;

	// Token: 0x0400496F RID: 18799
	public Dictionary<long, List<DecoObject>> decosPerSmallChunks = new Dictionary<long, List<DecoObject>>(64);

	// Token: 0x04004970 RID: 18800
	public OcclusionManager.OccludeeZone occludeeZone;

	// Token: 0x04004971 RID: 18801
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<Transform> occlusionTs = new List<Transform>();

	// Token: 0x04004972 RID: 18802
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<GameObjectPool.AsyncItem> asyncItems = new List<GameObjectPool.AsyncItem>();

	// Token: 0x04004973 RID: 18803
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<string, List<DecoObject>> models = new Dictionary<string, List<DecoObject>>();
}
