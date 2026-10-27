using System;
using System.Collections.Generic;
using System.IO;
using PrefabVolumes;
using UnityEngine;

// Token: 0x020001CD RID: 461
public class TriggerVolume
{
	// Token: 0x06000E2D RID: 3629 RVA: 0x0005DA3B File Offset: 0x0005BC3B
	public static TriggerVolume Create(PrefabTriggerVolume psv, Vector3i _boxMin, Vector3i _boxMax)
	{
		TriggerVolume triggerVolume = new TriggerVolume();
		triggerVolume.SetMinMax(_boxMin, _boxMax);
		triggerVolume.TriggersIndices = new List<byte>(psv.TriggersIndices);
		triggerVolume.AddToPrefabInstance();
		return triggerVolume;
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x0005DA64 File Offset: 0x0005BC64
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMinMax(Vector3i _boxMin, Vector3i _boxMax)
	{
		this.BoxMin = _boxMin;
		this.BoxMax = _boxMax;
		this.Center = ((this.BoxMin + this.BoxMax) * 0.5f).ToVector3();
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000E2F RID: 3631 RVA: 0x0005DAA8 File Offset: 0x0005BCA8
	public PrefabInstance PrefabInstance
	{
		get
		{
			return this.prefabInstance;
		}
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0005DAB0 File Offset: 0x0005BCB0
	public bool AddToPrefabInstance()
	{
		this.prefabInstance = GameManager.Instance.World.ChunkCache.ChunkProvider.GetDynamicPrefabDecorator().FindVolumeOwner(PrefabVolumeAbs.EVolumeType.Trigger, this.BoxMin, this.BoxMax);
		if (this.prefabInstance != null)
		{
			this.prefabInstance.AddTriggerVolume(this);
			return true;
		}
		return false;
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x0005DB05 File Offset: 0x0005BD05
	public void Reset()
	{
		this.isTriggered = false;
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x0005DB0E File Offset: 0x0005BD0E
	public bool HasAnyTriggers()
	{
		return this.TriggersIndices.Count > 0;
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x0005DB20 File Offset: 0x0005BD20
	public void CheckTouching(World _world, EntityPlayer _player)
	{
		if (this.isTriggered)
		{
			return;
		}
		Vector3 position = _player.position;
		position.y += 0.8f;
		if (position.x >= (float)this.BoxMin.x && position.x < (float)this.BoxMax.x && position.y >= (float)this.BoxMin.y && position.y < (float)this.BoxMax.y && position.z >= (float)this.BoxMin.z && position.z < (float)this.BoxMax.z)
		{
			this.Touch(_world, _player);
		}
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x0005DBCD File Offset: 0x0005BDCD
	public bool Intersects(Bounds bounds)
	{
		return BoundsUtils.Intersects(bounds, this.BoxMin, this.BoxMax);
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x0005DBEB File Offset: 0x0005BDEB
	[PublicizedFrom(EAccessModifier.Private)]
	public void Touch(World _world, EntityPlayer _player)
	{
		this.isTriggered = true;
		_world.triggerManager.TriggerBlocks(_player, this.prefabInstance, this);
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x0005DC08 File Offset: 0x0005BE08
	public static TriggerVolume Read(BinaryReader _br)
	{
		TriggerVolume triggerVolume = new TriggerVolume();
		int num = (int)_br.ReadByte();
		triggerVolume.SetMinMax(new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()), new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32()));
		int num2 = (int)_br.ReadByte();
		triggerVolume.TriggersIndices.Clear();
		for (int i = 0; i < num2; i++)
		{
			triggerVolume.TriggersIndices.Add(_br.ReadByte());
		}
		if (num > 1)
		{
			triggerVolume.isTriggered = _br.ReadBoolean();
		}
		return triggerVolume;
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0005DC98 File Offset: 0x0005BE98
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(2);
		_bw.Write(this.BoxMin.x);
		_bw.Write(this.BoxMin.y);
		_bw.Write(this.BoxMin.z);
		_bw.Write(this.BoxMax.x);
		_bw.Write(this.BoxMax.y);
		_bw.Write(this.BoxMax.z);
		_bw.Write((byte)this.TriggersIndices.Count);
		for (int i = 0; i < this.TriggersIndices.Count; i++)
		{
			_bw.Write(this.TriggersIndices[i]);
		}
		_bw.Write(this.isTriggered);
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x0005DD58 File Offset: 0x0005BF58
	[PublicizedFrom(EAccessModifier.Private)]
	public void DrawVolume()
	{
		Vector3 vector = this.BoxMin.ToVector3();
		vector -= Origin.position;
		Vector3 vector2 = this.BoxMax.ToVector3();
		vector2 -= Origin.position;
		Debug.DrawLine(vector, new Vector3(vector.x, vector.y, vector2.z), Color.blue, 1f);
		Debug.DrawLine(vector, new Vector3(vector2.x, vector.y, vector.z), Color.blue, 1f);
		Debug.DrawLine(new Vector3(vector.x, vector.y, vector2.z), new Vector3(vector2.x, vector.y, vector2.z), Color.blue, 1f);
		Debug.DrawLine(new Vector3(vector2.x, vector.y, vector.z), new Vector3(vector2.x, vector.y, vector2.z), Color.blue, 1f);
		Debug.DrawLine(new Vector3(vector.x, vector2.y, vector.z), new Vector3(vector.x, vector2.y, vector2.z), Color.cyan, 1f);
		Debug.DrawLine(new Vector3(vector.x, vector2.y, vector.z), new Vector3(vector2.x, vector2.y, vector.z), Color.cyan, 1f);
		Debug.DrawLine(new Vector3(vector.x, vector2.y, vector2.z), new Vector3(vector2.x, vector2.y, vector2.z), Color.cyan, 1f);
		Debug.DrawLine(new Vector3(vector2.x, vector2.y, vector.z), new Vector3(vector2.x, vector2.y, vector2.z), Color.cyan, 1f);
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x0005DF54 File Offset: 0x0005C154
	public void DrawDebugLines(float _duration)
	{
		string text = string.Format("TriggerVolume{0},{1}", this.BoxMin, this.BoxMax);
		Color color = new Color(0.1f, 0.1f, 1f);
		if (this.isTriggered)
		{
			color = new Color(0f, 0f, 0.5f, 0.16f);
		}
		Vector3 vector = this.BoxMin.ToVector3();
		Vector3 vector2 = this.BoxMax.ToVector3();
		vector += DebugLines.InsideOffsetV * 2f;
		vector2 -= DebugLines.InsideOffsetV * 2f;
		DebugLines.Create(text, GameManager.Instance.World.GetPrimaryPlayer().RootTransform, color, color, 0.03f, 0.03f, _duration).AddCube(vector, vector2);
	}

	// Token: 0x04000C21 RID: 3105
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte VERSION = 2;

	// Token: 0x04000C22 RID: 3106
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cPlayerYOffset = 0.8f;

	// Token: 0x04000C23 RID: 3107
	public static Vector3i chunkPadding = new Vector3i(12, 1, 12);

	// Token: 0x04000C24 RID: 3108
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance prefabInstance;

	// Token: 0x04000C25 RID: 3109
	public List<byte> TriggersIndices = new List<byte>();

	// Token: 0x04000C26 RID: 3110
	public Vector3i BoxMin;

	// Token: 0x04000C27 RID: 3111
	public Vector3i BoxMax;

	// Token: 0x04000C28 RID: 3112
	public Vector3 Center;

	// Token: 0x04000C29 RID: 3113
	public bool isTriggered;
}
