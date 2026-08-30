using System;
using System.Collections.Generic;
using System.IO;
using PrefabVolumes;
using UnityEngine;

// Token: 0x020001CA RID: 458
public class TriggerVolume
{
	// Token: 0x06000E2B RID: 3627 RVA: 0x0005D5EB File Offset: 0x0005B7EB
	public static TriggerVolume Create(PrefabTriggerVolume psv, Vector3i _boxMin, Vector3i _boxMax)
	{
		TriggerVolume triggerVolume = new TriggerVolume();
		triggerVolume.SetMinMax(_boxMin, _boxMax);
		triggerVolume.TriggersIndices = new List<byte>(psv.TriggersIndices);
		triggerVolume.AddToPrefabInstance();
		return triggerVolume;
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x0005D614 File Offset: 0x0005B814
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMinMax(Vector3i _boxMin, Vector3i _boxMax)
	{
		this.BoxMin = _boxMin;
		this.BoxMax = _boxMax;
		this.Center = ((this.BoxMin + this.BoxMax) * 0.5f).ToVector3();
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000E2D RID: 3629 RVA: 0x0005D658 File Offset: 0x0005B858
	public PrefabInstance PrefabInstance
	{
		get
		{
			return this.prefabInstance;
		}
	}

	// Token: 0x06000E2E RID: 3630 RVA: 0x0005D660 File Offset: 0x0005B860
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

	// Token: 0x06000E2F RID: 3631 RVA: 0x0005D6B5 File Offset: 0x0005B8B5
	public void Reset()
	{
		this.isTriggered = false;
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x0005D6BE File Offset: 0x0005B8BE
	public bool HasAnyTriggers()
	{
		return this.TriggersIndices.Count > 0;
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x0005D6D0 File Offset: 0x0005B8D0
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

	// Token: 0x06000E32 RID: 3634 RVA: 0x0005D77D File Offset: 0x0005B97D
	public bool Intersects(Bounds bounds)
	{
		return BoundsUtils.Intersects(bounds, this.BoxMin, this.BoxMax);
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x0005D79B File Offset: 0x0005B99B
	[PublicizedFrom(EAccessModifier.Private)]
	public void Touch(World _world, EntityPlayer _player)
	{
		this.isTriggered = true;
		_world.triggerManager.TriggerBlocks(_player, this.prefabInstance, this);
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x0005D7B8 File Offset: 0x0005B9B8
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

	// Token: 0x06000E35 RID: 3637 RVA: 0x0005D848 File Offset: 0x0005BA48
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

	// Token: 0x06000E36 RID: 3638 RVA: 0x0005D908 File Offset: 0x0005BB08
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

	// Token: 0x06000E37 RID: 3639 RVA: 0x0005DB04 File Offset: 0x0005BD04
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

	// Token: 0x04000C1B RID: 3099
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte VERSION = 2;

	// Token: 0x04000C1C RID: 3100
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cPlayerYOffset = 0.8f;

	// Token: 0x04000C1D RID: 3101
	public static Vector3i chunkPadding = new Vector3i(12, 1, 12);

	// Token: 0x04000C1E RID: 3102
	[PublicizedFrom(EAccessModifier.Private)]
	public PrefabInstance prefabInstance;

	// Token: 0x04000C1F RID: 3103
	public List<byte> TriggersIndices = new List<byte>();

	// Token: 0x04000C20 RID: 3104
	public Vector3i BoxMin;

	// Token: 0x04000C21 RID: 3105
	public Vector3i BoxMax;

	// Token: 0x04000C22 RID: 3106
	public Vector3 Center;

	// Token: 0x04000C23 RID: 3107
	public bool isTriggered;
}
