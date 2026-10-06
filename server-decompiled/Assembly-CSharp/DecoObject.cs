using System;
using System.IO;
using UnityEngine;

// Token: 0x02000C2C RID: 3116
public class DecoObject : IEquatable<DecoObject>
{
	// Token: 0x06005EAF RID: 24239 RVA: 0x00251702 File Offset: 0x0024F902
	public void Init(Vector3i _pos, float _realYPos, BlockValue _bv, DecoState _state)
	{
		this.pos = _pos;
		this.realYPos = _realYPos;
		this.bv = _bv;
		this.state = _state;
		this.asyncItem = null;
		this.go = null;
	}

	// Token: 0x06005EB0 RID: 24240 RVA: 0x00251730 File Offset: 0x0024F930
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

	// Token: 0x06005EB1 RID: 24241 RVA: 0x002517A4 File Offset: 0x0024F9A4
	public void CreateGameObject(DecoChunk _decoChunk, Transform _parent)
	{
		string modelName = this.GetModelName();
		if (modelName != null)
		{
			GameObject objectForType = GameObjectPool.Instance.GetObjectForType(modelName);
			this.CreateGameObjectCallback(objectForType, _parent, false);
		}
	}

	// Token: 0x06005EB2 RID: 24242 RVA: 0x002517D0 File Offset: 0x0024F9D0
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

	// Token: 0x06005EB3 RID: 24243 RVA: 0x002518F5 File Offset: 0x0024FAF5
	public void Destroy()
	{
		this.asyncItem = null;
		if (this.go)
		{
			GameObjectPool.Instance.PoolObjectAsync(this.go);
			this.go = null;
		}
	}

	// Token: 0x06005EB4 RID: 24244 RVA: 0x00251924 File Offset: 0x0024FB24
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

	// Token: 0x06005EB5 RID: 24245 RVA: 0x002519AD File Offset: 0x0024FBAD
	public void Read(BinaryReader _br)
	{
		this.pos = GameUtils.UInt64ToVector3i(_br.ReadUInt64());
		this.realYPos = _br.ReadSingle();
		this.bv = new BlockValue(_br.ReadUInt32());
		this.state = (DecoState)_br.ReadByte();
	}

	// Token: 0x06005EB6 RID: 24246 RVA: 0x002519E9 File Offset: 0x0024FBE9
	public override int GetHashCode()
	{
		return HashCode.Combine<int, DecoState, int>(this.pos.GetHashCode(), this.state, this.bv.GetHashCode());
	}

	// Token: 0x06005EB7 RID: 24247 RVA: 0x00251A18 File Offset: 0x0024FC18
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

	// Token: 0x06005EB8 RID: 24248 RVA: 0x00251A44 File Offset: 0x0024FC44
	public bool Equals(DecoObject _other)
	{
		return _other != null && (this == _other || (_other.pos.Equals(this.pos) && _other.state == this.state && _other.bv.Equals(this.bv)));
	}

	// Token: 0x040049C6 RID: 18886
	public Vector3i pos;

	// Token: 0x040049C7 RID: 18887
	public float realYPos;

	// Token: 0x040049C8 RID: 18888
	public BlockValue bv;

	// Token: 0x040049C9 RID: 18889
	public DecoState state;

	// Token: 0x040049CA RID: 18890
	public GameObjectPool.AsyncItem asyncItem;

	// Token: 0x040049CB RID: 18891
	public GameObject go;
}
