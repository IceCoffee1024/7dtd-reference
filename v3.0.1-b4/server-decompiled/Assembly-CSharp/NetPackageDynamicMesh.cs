using System;
using System.Threading;
using UnityEngine.Scripting;

// Token: 0x020003BE RID: 958
[Preserve]
public class NetPackageDynamicMesh : DynamicMeshServerData, IMemoryPoolableObject
{
	// Token: 0x06001CB6 RID: 7350 RVA: 0x000AD9B8 File Offset: 0x000ABBB8
	public void Setup(DynamicMeshItem item, byte[] byteArray)
	{
		this.Item = item;
		this.bytes = byteArray;
		this.X = item.WorldPosition.x;
		this.Z = item.WorldPosition.z;
		this.UpdateTime = item.UpdateTime;
		DynamicMeshItem item2 = this.Item;
		this.PresumedLength = ((item2 != null) ? item2.PackageLength : 0);
	}

	// Token: 0x06001CB7 RID: 7351 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool Prechecks()
	{
		return true;
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001CB9 RID: 7353 RVA: 0x000ADA1C File Offset: 0x000ABC1C
	public override void read(PooledBinaryReader reader)
	{
		this.X = reader.ReadInt32();
		this.Z = reader.ReadInt32();
		this.UpdateTime = reader.ReadInt32();
		int num = reader.ReadInt32();
		NetPackageDynamicMesh.Count++;
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			string text = string.Concat(new string[]
			{
				DynamicMeshFile.MeshLocation,
				this.X.ToString(),
				",",
				this.Z.ToString(),
				".mesh"
			});
			if (DynamicMeshManager.DoLog)
			{
				Log.Out(string.Format("Reading {0},{1} len {2}", this.X, this.Z, num));
			}
			if (num == 0)
			{
				if (DynamicMeshManager.Instance != null)
				{
					DynamicMeshManager.Instance.ArrangeChunkRemoval(this.X, this.Z);
				}
				this.bytes = null;
				return;
			}
			int num2 = 0;
			this.bytes = DynamicMeshThread.ChunkDataQueue.GetFromPool(num);
			reader.Read(this.bytes, 0, num);
			if (string.IsNullOrWhiteSpace(DynamicMeshFile.MeshLocation))
			{
				this.IsValidUpdate = false;
				return;
			}
			while (num2++ < 10)
			{
				try
				{
					this.IsValidUpdate = DynamicMeshThread.ChunkDataQueue.SaveNetPackageData(this.X, this.Z, this.bytes, this.UpdateTime);
					break;
				}
				catch (Exception)
				{
					Log.Out(string.Concat(new string[]
					{
						"Failed attempt ",
						num2.ToString(),
						" to write mesh ",
						text,
						". Retrying..."
					}));
					Thread.Sleep(1000);
				}
			}
		}
	}

	// Token: 0x06001CBA RID: 7354 RVA: 0x000ADBCC File Offset: 0x000ABDCC
	public override void write(PooledBinaryWriter writer)
	{
		base.write(writer);
		writer.Write(this.X);
		writer.Write(this.Z);
		writer.Write(this.UpdateTime);
		int num = this.PresumedLength;
		long position = writer.BaseStream.Position;
		string text = "start";
		try
		{
			text = "len";
			if (DynamicMeshManager.DoLog)
			{
				Log.Out(string.Format("Sending {0},{1} len {2}", this.X, this.Z, num));
			}
			text = "lencheck";
			if (NetPackageDynamicMesh.MaxLength < num)
			{
				NetPackageDynamicMesh.MaxLength = num;
				if (DynamicMeshManager.DoLog)
				{
					Log.Out("New dyMesh maxLen: " + NetPackageDynamicMesh.MaxLength.ToString());
				}
			}
			NetPackageDynamicMesh.LastLength = num;
			NetPackageDynamicMesh.LastX = this.X;
			NetPackageDynamicMesh.LastZ = this.Z;
			text = "writelen";
			writer.Write(num);
			if (this.bytes != null)
			{
				text = "writebytes";
				if (this.bytes.Length < num)
				{
					text = "writebytecheck";
					Log.Warning("Dymesh byte length was lower than expected. Len was " + num.ToString() + " and bytes were " + this.bytes.Length.ToString());
					num = (this.PresumedLength = this.bytes.Length);
				}
				text = "writenow";
				writer.Write(this.bytes, 0, num);
			}
		}
		catch (Exception ex)
		{
			string text2 = " ERROR MESH EXCEPTION\r\ndyMesh netWrite error: {0}\r\n({1},{2})\r\nLength: {3}\r\nLen: {4}\r\nbytes: {5}\r\nMaxLength: {6}\r\nwriterStartPosition: {7}\r\nwriterLength: {8}\r\nwriterPos: {9}\r\nstep: {10}\r\n";
			object[] array = new object[11];
			array[0] = ex.Message;
			array[1] = this.X;
			array[2] = this.Z;
			array[3] = num;
			array[4] = num;
			int num2 = 5;
			byte[] array2 = this.bytes;
			array[num2] = ((array2 != null) ? array2.Length.ToString() : null) ?? "null";
			array[6] = NetPackageDynamicMesh.MaxLength;
			array[7] = position;
			array[8] = writer.BaseStream.Length;
			array[9] = writer.BaseStream.Position;
			array[10] = text;
			Log.Error(string.Format(text2, array));
			throw ex;
		}
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06001CBB RID: 7355 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06001CBC RID: 7356 RVA: 0x000ADDFC File Offset: 0x000ABFFC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (!DynamicMeshManager.CONTENT_ENABLED)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			DynamicMeshServer.ClientReadyForNextMesh(this);
			return;
		}
		if (this.IsValidUpdate)
		{
			DynamicMeshManager.AddDataFromServer(this.X, this.Z);
		}
		NetPackageDynamicMesh package = NetPackageManager.GetPackage<NetPackageDynamicMesh>();
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(package, false);
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x000ADE4F File Offset: 0x000AC04F
	public override int GetLength()
	{
		if (this.PresumedLength > 0)
		{
			return Math.Min(this.PresumedLength, NetPackageDynamicMesh.MaxMessageSize);
		}
		if (this.bytes != null)
		{
			return 16 + this.bytes.Length;
		}
		return 16;
	}

	// Token: 0x06001CBE RID: 7358 RVA: 0x000ADE81 File Offset: 0x000AC081
	public void Reset()
	{
		DynamicMeshServer.SyncRelease(this.Item);
		this.Item = null;
		this.Attempts = 0;
		this.bytes = null;
		this.PresumedLength = 0;
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x000ADEAA File Offset: 0x000AC0AA
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x06001CC0 RID: 7360 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x040012BA RID: 4794
	public static byte[] DelayedMessageBytes = new byte[1];

	// Token: 0x040012BB RID: 4795
	public static int MaxMessageSize = 2097152;

	// Token: 0x040012BC RID: 4796
	public static int MaxLength = 0;

	// Token: 0x040012BD RID: 4797
	public static int LastLength;

	// Token: 0x040012BE RID: 4798
	public static int LastZ;

	// Token: 0x040012BF RID: 4799
	public static int LastX;

	// Token: 0x040012C0 RID: 4800
	public static int Count = 0;

	// Token: 0x040012C1 RID: 4801
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMeshItem Item;

	// Token: 0x040012C2 RID: 4802
	public int Attempts;

	// Token: 0x040012C3 RID: 4803
	public int PresumedLength;

	// Token: 0x040012C4 RID: 4804
	public int UpdateTime;

	// Token: 0x040012C5 RID: 4805
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] bytes;

	// Token: 0x040012C6 RID: 4806
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsValidUpdate;
}
