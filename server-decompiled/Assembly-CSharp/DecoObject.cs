using System;
using System.IO;
using UnityEngine;

// Token: 0x02000C37 RID: 3127
public class DecoObject : IEquatable<DecoObject>
{
	// Token: 0x06005ED4 RID: 24276 RVA: 0x00252C2A File Offset: 0x00250E2A
	public void Init(Vector3i _pos, float _realYPos, BlockValue _bv, DecoState _state)
	{
		this.pos = _pos;
		this.realYPos = _realYPos;
		this.bv = _bv;
		this.state = _state;
		this.asyncItem = null;
		this.go = null;
	}

	// Token: 0x06005ED5 RID: 24277 RVA: 0x00252C58 File Offset: 0x00250E58
	public string GetModelName()
	{
		Block block = this.bv.Block;
		if (block == null)
		{
			Log.Error(string.Format("DecoObject '{0}', no block!", this.bv));
			return null;
		}
		string value = block.Properties.GetValue("Model");
		if (string.IsNullOrEmpty(value))
		{
			Log.Error("DecoObject block '" + block.GetBlockName() + "', no model!");
			return null;
		}
		return GameIO.GetFilenameFromPathWithoutExtension(value);
	}

	// Token: 0x06005ED6 RID: 24278 RVA: 0x00252CCC File Offset: 0x00250ECC
	public void CreateGameObject(DecoChunk _decoChunk, Transform _parent)
	{
		string modelName = this.GetModelName();
		if (modelName != null)
		{
			GameObject objectForType = GameObjectPool.Instance.GetObjectForType(modelName);
			this.CreateGameObjectCallback(objectForType, _parent, false);
		}
	}

	// Token: 0x06005ED7 RID: 24279 RVA: 0x00252CF8 File Offset: 0x00250EF8
	public void CreateGameObjectCallback(GameObject _obj, Transform _parent, bool _isAsync)
	{
		this.go = _obj;
		if (_isAsync && this.asyncItem == null)
		{
			this.Destroy();
			return;
		}
		this.asyncItem = null;
		Block block = this.bv.Block;
		BlockShapeDistantDeco blockShapeDistantDeco = block.shape as BlockShapeDistantDeco;
		if (blockShapeDistantDeco == null)
		{
			Log.Error("Block '{0}' needs a deco shape assigned but has not!", new object[] { block.GetBlockName() });
			return;
		}
		Transform transform = this.go.transform;
		transform.SetParent(_parent, false);
		float y = blockShapeDistantDeco.modelOffset.y;
		transform.position = new Vector3((float)this.pos.x + DecoManager.cDecoMiddleOffset.x, this.realYPos + y, (float)this.pos.z + DecoManager.cDecoMiddleOffset.z) - Origin.position;
		int num = (int)this.bv.rotation;
		if (!blockShapeDistantDeco.Has45DegreeRotations)
		{
			num &= 3;
		}
		transform.localRotation = BlockShapeNew.GetRotationStatic(num);
		this.go.SetActive(true);
		BlockEntityData blockEntityData = new BlockEntityData();
		blockEntityData.transform = transform;
		blockShapeDistantDeco.OnBlockEntityTransformAfterActivated(null, this.pos, this.bv, blockEntityData);
	}

	// Token: 0x06005ED8 RID: 24280 RVA: 0x00252E1D File Offset: 0x0025101D
	public void Destroy()
	{
		this.asyncItem = null;
		if (this.go)
		{
			GameObjectPool.Instance.PoolObjectAsync(this.go);
			this.go = null;
		}
	}

	// Token: 0x06005ED9 RID: 24281 RVA: 0x00252E4C File Offset: 0x0025104C
	public void Write(BinaryWriter _bw, NameIdMapping _blockMap = null)
	{
		_bw.Write(GameUtils.Vector3iToUInt64(this.pos));
		_bw.Write(this.realYPos);
		_bw.Write(this.bv.rawData);
		_bw.Write((byte)this.state);
		Block block = this.bv.Block;
		if (block == null)
		{
			Log.Error(string.Format("Writing DecoObject '{0}', no block!", this.bv));
			return;
		}
		if (_blockMap != null)
		{
			_blockMap.AddMapping(block.blockID, block.GetBlockName(), false);
		}
	}

	// Token: 0x06005EDA RID: 24282 RVA: 0x00252ED5 File Offset: 0x002510D5
	public void Read(BinaryReader _br)
	{
		this.pos = GameUtils.UInt64ToVector3i(_br.ReadUInt64());
		this.realYPos = _br.ReadSingle();
		this.bv = new BlockValue(_br.ReadUInt32());
		this.state = (DecoState)_br.ReadByte();
	}

	// Token: 0x06005EDB RID: 24283 RVA: 0x00252F11 File Offset: 0x00251111
	public override int GetHashCode()
	{
		return HashCode.Combine<int, DecoState, int>(this.pos.GetHashCode(), this.state, this.bv.GetHashCode());
	}

	// Token: 0x06005EDC RID: 24284 RVA: 0x00252F40 File Offset: 0x00251140
	public override bool Equals(object _obj)
	{
		if (_obj == null)
		{
			return false;
		}
		if (this == _obj)
		{
			return true;
		}
		DecoObject decoObject = _obj as DecoObject;
		return decoObject != null && this.Equals(decoObject);
	}

	// Token: 0x06005EDD RID: 24285 RVA: 0x00252F6C File Offset: 0x0025116C
	public bool Equals(DecoObject _other)
	{
		return _other != null && (this == _other || (_other.pos.Equals(this.pos) && _other.state == this.state && _other.bv.Equals(this.bv)));
	}

	// Token: 0x040049EB RID: 18923
	public Vector3i pos;

	// Token: 0x040049EC RID: 18924
	public float realYPos;

	// Token: 0x040049ED RID: 18925
	public BlockValue bv;

	// Token: 0x040049EE RID: 18926
	public DecoState state;

	// Token: 0x040049EF RID: 18927
	public GameObjectPool.AsyncItem asyncItem;

	// Token: 0x040049F0 RID: 18928
	public GameObject go;
}
