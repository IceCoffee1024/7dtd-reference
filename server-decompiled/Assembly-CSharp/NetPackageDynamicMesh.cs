using System;
using System.Threading;
using UnityEngine.Scripting;

// Token: 0x020003C5 RID: 965
[Preserve]
public class NetPackageDynamicMesh : DynamicMeshServerData, IMemoryPoolableObject
{
	// Token: 0x06001CCC RID: 7372 RVA: 0x000AE338 File Offset: 0x000AC538
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

	// Token: 0x06001CCD RID: 7373 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Prechecks()
	{
		return true;
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x06001CCE RID: 7374 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool FlushQueue
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x000AE39C File Offset: 0x000AC59C
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

	// Token: 0x06001CD0 RID: 7376 RVA: 0x000AE54C File Offset: 0x000AC74C
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

	// Token: 0x17000362 RID: 866
	// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06001CD2 RID: 7378 RVA: 0x000AE77C File Offset: 0x000AC97C
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

	// Token: 0x06001CD3 RID: 7379 RVA: 0x000AE7CF File Offset: 0x000AC9CF
	public void Reset()
	{
		DynamicMeshServer.SyncRelease(this.Item);
		this.Item = null;
		this.Attempts = 0;
		this.bytes = null;
		this.PresumedLength = 0;
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x000AE7F8 File Offset: 0x000AC9F8
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x17000363 RID: 867
	// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x00020119 File Offset: 0x0001E319
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000364 RID: 868
	// (get) Token: 0x06001CD6 RID: 7382 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool Compress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x040012C8 RID: 4808
	public static byte[] DelayedMessageBytes = new byte[1];

	// Token: 0x040012C9 RID: 4809
	public static int MaxMessageSize = 2097152;

	// Token: 0x040012CA RID: 4810
	public static int MaxLength = 0;

	// Token: 0x040012CB RID: 4811
	public static int LastLength;

	// Token: 0x040012CC RID: 4812
	public static int LastZ;

	// Token: 0x040012CD RID: 4813
	public static int LastX;

	// Token: 0x040012CE RID: 4814
	public static int Count = 0;

	// Token: 0x040012CF RID: 4815
	[PublicizedFrom(EAccessModifier.Private)]
	public DynamicMeshItem Item;

	// Token: 0x040012D0 RID: 4816
	public int Attempts;

	// Token: 0x040012D1 RID: 4817
	public int PresumedLength;

	// Token: 0x040012D2 RID: 4818
	public int UpdateTime;

	// Token: 0x040012D3 RID: 4819
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] bytes;

	// Token: 0x040012D4 RID: 4820
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsValidUpdate;
}
