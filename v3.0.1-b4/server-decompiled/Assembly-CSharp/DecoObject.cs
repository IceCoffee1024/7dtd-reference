using System;
using System.IO;
using UnityEngine;

// Token: 0x02000C08 RID: 3080
public class DecoObject : IEquatable<DecoObject>
{
	// Token: 0x06005E0C RID: 24076 RVA: 0x0024B556 File Offset: 0x00249756
	public void Init(Vector3i _pos, float _realYPos, BlockValue _bv, DecoState _state)
	{
		this.pos = _pos;
		this.realYPos = _realYPos;
		this.bv = _bv;
		this.state = _state;
		this.asyncItem = null;
		this.go = null;
	}

	// Token: 0x06005E0D RID: 24077 RVA: 0x0024B584 File Offset: 0x00249784
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

	// Token: 0x06005E0E RID: 24078 RVA: 0x0024B5F8 File Offset: 0x002497F8
	public void CreateGameObject(DecoChunk _decoChunk, Transform _parent)
	{
		string modelName = this.GetModelName();
		if (modelName != null)
		{
			GameObject objectForType = GameObjectPool.Instance.GetObjectForType(modelName);
			this.CreateGameObjectCallback(objectForType, _parent, false);
		}
	}

	// Token: 0x06005E0F RID: 24079 RVA: 0x0024B624 File Offset: 0x00249824
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

	// Token: 0x06005E10 RID: 24080 RVA: 0x0024B749 File Offset: 0x00249949
	public void Destroy()
	{
		this.asyncItem = null;
		if (this.go)
		{
			GameObjectPool.Instance.PoolObjectAsync(this.go);
			this.go = null;
		}
	}

	// Token: 0x06005E11 RID: 24081 RVA: 0x0024B778 File Offset: 0x00249978
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

	// Token: 0x06005E12 RID: 24082 RVA: 0x0024B801 File Offset: 0x00249A01
	public void Read(BinaryReader _br)
	{
		this.pos = GameUtils.UInt64ToVector3i(_br.ReadUInt64());
		this.realYPos = _br.ReadSingle();
		this.bv = new BlockValue(_br.ReadUInt32());
		this.state = (DecoState)_br.ReadByte();
	}

	// Token: 0x06005E13 RID: 24083 RVA: 0x0024B83D File Offset: 0x00249A3D
	public override int GetHashCode()
	{
		return HashCode.Combine<int, DecoState, int>(this.pos.GetHashCode(), this.state, this.bv.GetHashCode());
	}

	// Token: 0x06005E14 RID: 24084 RVA: 0x0024B86C File Offset: 0x00249A6C
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

	// Token: 0x06005E15 RID: 24085 RVA: 0x0024B898 File Offset: 0x00249A98
	public bool Equals(DecoObject _other)
	{
		return _other != null && (this == _other || (_other.pos.Equals(this.pos) && _other.state == this.state && _other.bv.Equals(this.bv)));
	}

	// Token: 0x040048EA RID: 18666
	public Vector3i pos;

	// Token: 0x040048EB RID: 18667
	public float realYPos;

	// Token: 0x040048EC RID: 18668
	public BlockValue bv;

	// Token: 0x040048ED RID: 18669
	public DecoState state;

	// Token: 0x040048EE RID: 18670
	public GameObjectPool.AsyncItem asyncItem;

	// Token: 0x040048EF RID: 18671
	public GameObject go;
}
