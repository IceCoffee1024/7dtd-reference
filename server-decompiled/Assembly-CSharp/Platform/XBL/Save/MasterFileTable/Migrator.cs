using System;
using System.Collections.Generic;
using System.IO;
using Platform.XBL.Save.MasterFileTable.Latest;
using Platform.XBL.Save.MasterFileTable.V01;
using Platform.XBL.Save.MasterFileTable.V02;
using Platform.XBL.Save.MasterFileTable.V03;
using Platform.XBL.Save.MasterFileTable.V04;
using Platform.XBL.Save.MasterFileTable.V05;

namespace Platform.XBL.Save.MasterFileTable
{
	// Token: 0x02001C96 RID: 7318
	public static class Migrator
	{
		// Token: 0x0600D8AA RID: 55466 RVA: 0x004DBAEC File Offset: 0x004D9CEC
		public static void ReadMigrate(Stream inputStream, IMigratable newMigratable, string identifier, Dictionary<ushort, Action<PooledBinaryReader, PooledBinaryWriter>> migrators)
		{
			ushort version = newMigratable.Version;
			MemoryStream memoryStream = null;
			MemoryStream memoryStream2 = null;
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(true))
			{
				pooledBinaryReader.SetBaseStream(inputStream);
				using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(true))
				{
					for (;;)
					{
						long position = pooledBinaryReader.BaseStream.Position;
						ushort num = pooledBinaryReader.ReadUInt16();
						pooledBinaryReader.BaseStream.Position = position;
						if (num > version)
						{
							Log.Out(string.Format("[{0}] Newer {1} is being loaded. V{2} > V{3}", new object[] { "Migrator", identifier, num, version }));
						}
						if (num >= version)
						{
							break;
						}
						if (memoryStream2 == null)
						{
							memoryStream2 = new MemoryStream();
						}
						pooledBinaryWriter.SetBaseStream(memoryStream2);
						pooledBinaryWriter.BaseStream.Position = 0L;
						migrators[num](pooledBinaryReader, pooledBinaryWriter);
						MemoryStream memoryStream3 = memoryStream2;
						MemoryStream memoryStream4 = memoryStream;
						memoryStream = memoryStream3;
						memoryStream2 = memoryStream4;
						pooledBinaryReader.SetBaseStream(memoryStream);
						pooledBinaryReader.BaseStream.Position = 0L;
					}
					newMigratable.Read(pooledBinaryReader);
				}
			}
		}

		// Token: 0x0600D8AB RID: 55467 RVA: 0x004DBC10 File Offset: 0x004D9E10
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateNode(Platform.XBL.Save.MasterFileTable.V01.Node oldNode, Platform.XBL.Save.MasterFileTable.V02.Node newNode)
		{
			newNode.LastWriteTimeUtc = Platform.XBL.Save.MasterFileTable.V02.Node.DefaultLastWriteTime;
			foreach (ulong num in oldNode.BlobIds)
			{
				newNode.BlobIds.Add(num);
			}
			foreach (Platform.XBL.Save.MasterFileTable.V01.Node node in oldNode.Children.Values)
			{
				Platform.XBL.Save.MasterFileTable.V02.Node orCreateChildNode = newNode.GetOrCreateChildNode(node.Name);
				Migrator.MigrateNode(node, orCreateChildNode);
			}
		}

		// Token: 0x0600D8AC RID: 55468 RVA: 0x004DBCCC File Offset: 0x004D9ECC
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateContainerData(Platform.XBL.Save.MasterFileTable.V01.ContainerData oldContainerData, Platform.XBL.Save.MasterFileTable.V02.ContainerData newContainerData)
		{
			Migrator.MigrateNode(oldContainerData.RootNode, newContainerData.RootNode);
		}

		// Token: 0x0600D8AD RID: 55469 RVA: 0x004DBCE0 File Offset: 0x004D9EE0
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void MigrateFromV1(PooledBinaryReader reader, PooledBinaryWriter writer)
		{
			using (Platform.XBL.Save.MasterFileTable.V01.ContainerData containerData = new Platform.XBL.Save.MasterFileTable.V01.ContainerData())
			{
				using (Platform.XBL.Save.MasterFileTable.V02.ContainerData containerData2 = new Platform.XBL.Save.MasterFileTable.V02.ContainerData())
				{
					containerData.Read(reader);
					Migrator.MigrateContainerData(containerData, containerData2);
					containerData2.Write(writer);
				}
			}
		}

		// Token: 0x0600D8AE RID: 55470 RVA: 0x004DBD40 File Offset: 0x004D9F40
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateNode(Platform.XBL.Save.MasterFileTable.V02.Node oldNode, Platform.XBL.Save.MasterFileTable.V03.Node newNode)
		{
			newNode.Attributes = (oldNode.IsDirectory() ? Platform.XBL.Save.MasterFileTable.V03.NodeAttributes.Directory : Platform.XBL.Save.MasterFileTable.V03.NodeAttributes.None);
			newNode.SetBlobIds(oldNode.BlobIds.ToArray());
			foreach (Platform.XBL.Save.MasterFileTable.V02.Node node in oldNode.Children.Values)
			{
				Platform.XBL.Save.MasterFileTable.V03.Node orCreateChildNode = newNode.GetOrCreateChildNode(node.Name, false);
				Migrator.MigrateNode(node, orCreateChildNode);
			}
			newNode.LastWriteTimeUtc = oldNode.LastWriteTimeUtc;
		}

		// Token: 0x0600D8AF RID: 55471 RVA: 0x004DBDDC File Offset: 0x004D9FDC
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateContainerData(Platform.XBL.Save.MasterFileTable.V02.ContainerData oldContainerData, Platform.XBL.Save.MasterFileTable.V03.ContainerData newContainerData)
		{
			Migrator.MigrateNode(oldContainerData.RootNode, newContainerData.RootNode);
		}

		// Token: 0x0600D8B0 RID: 55472 RVA: 0x004DBDF0 File Offset: 0x004D9FF0
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void MigrateFromV2(PooledBinaryReader reader, PooledBinaryWriter writer)
		{
			using (Platform.XBL.Save.MasterFileTable.V02.ContainerData containerData = new Platform.XBL.Save.MasterFileTable.V02.ContainerData())
			{
				using (Platform.XBL.Save.MasterFileTable.V03.ContainerData containerData2 = new Platform.XBL.Save.MasterFileTable.V03.ContainerData())
				{
					containerData.Read(reader);
					Migrator.MigrateContainerData(containerData, containerData2);
					containerData2.Write(writer);
				}
			}
		}

		// Token: 0x0600D8B1 RID: 55473 RVA: 0x004DBE50 File Offset: 0x004DA050
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateNodeAttributes(Platform.XBL.Save.MasterFileTable.V03.NodeAttributes oldAttributes, out Platform.XBL.Save.MasterFileTable.V04.NodeAttributes newAttributes)
		{
			newAttributes = Platform.XBL.Save.MasterFileTable.V04.NodeAttributes.None;
			if (oldAttributes.HasFlag(Platform.XBL.Save.MasterFileTable.V03.NodeAttributes.Directory))
			{
				newAttributes |= Platform.XBL.Save.MasterFileTable.V04.NodeAttributes.Directory;
			}
		}

		// Token: 0x0600D8B2 RID: 55474 RVA: 0x004DBE6E File Offset: 0x004DA06E
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateBlobRef(ulong oldBlobRef, out Platform.XBL.Save.MasterFileTable.V04.BlobRef newBlobRef)
		{
			newBlobRef = new Platform.XBL.Save.MasterFileTable.V04.BlobRef(oldBlobRef, 0U);
		}

		// Token: 0x0600D8B3 RID: 55475 RVA: 0x004DBE7C File Offset: 0x004DA07C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateNode(Platform.XBL.Save.MasterFileTable.V03.Node oldNode, Platform.XBL.Save.MasterFileTable.V04.Node newNode)
		{
			Migrator.MigrateNodeAttributes(oldNode.Attributes, out newNode.Attributes);
			IReadOnlyList<ulong> blobIds = oldNode.BlobIds;
			Platform.XBL.Save.MasterFileTable.V04.BlobRef[] array = new Platform.XBL.Save.MasterFileTable.V04.BlobRef[blobIds.Count];
			for (int i = 0; i < blobIds.Count; i++)
			{
				Migrator.MigrateBlobRef(blobIds[i], out array[i]);
			}
			newNode.SetBlobRefs(array);
			foreach (Platform.XBL.Save.MasterFileTable.V03.Node node in oldNode.Children.Values)
			{
				Platform.XBL.Save.MasterFileTable.V04.Node orCreateChildNode = newNode.GetOrCreateChildNode(node.Name, node.IsDirectory());
				Migrator.MigrateNode(node, orCreateChildNode);
			}
			newNode.LastWriteTimeUtc = oldNode.LastWriteTimeUtc;
		}

		// Token: 0x0600D8B4 RID: 55476 RVA: 0x004DBF48 File Offset: 0x004DA148
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateContainerData(Platform.XBL.Save.MasterFileTable.V03.ContainerData oldContainerData, Platform.XBL.Save.MasterFileTable.V04.ContainerData newContainerData)
		{
			Migrator.MigrateNode(oldContainerData.RootNode, newContainerData.RootNode);
		}

		// Token: 0x0600D8B5 RID: 55477 RVA: 0x004DBF5C File Offset: 0x004DA15C
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void MigrateFromV3(PooledBinaryReader reader, PooledBinaryWriter writer)
		{
			using (Platform.XBL.Save.MasterFileTable.V03.ContainerData containerData = new Platform.XBL.Save.MasterFileTable.V03.ContainerData())
			{
				using (Platform.XBL.Save.MasterFileTable.V04.ContainerData containerData2 = new Platform.XBL.Save.MasterFileTable.V04.ContainerData())
				{
					containerData.Read(reader);
					Migrator.MigrateContainerData(containerData, containerData2);
					containerData2.Write(writer);
				}
			}
		}

		// Token: 0x0600D8B6 RID: 55478 RVA: 0x004DBFBC File Offset: 0x004DA1BC
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateNodeAttributes(Platform.XBL.Save.MasterFileTable.V04.NodeAttributes oldAttributes, out Platform.XBL.Save.MasterFileTable.V05.NodeAttributes newAttributes)
		{
			newAttributes = Platform.XBL.Save.MasterFileTable.V05.NodeAttributes.None;
			if (oldAttributes.HasFlag(Platform.XBL.Save.MasterFileTable.V04.NodeAttributes.Directory))
			{
				newAttributes |= Platform.XBL.Save.MasterFileTable.V05.NodeAttributes.Directory;
			}
		}

		// Token: 0x0600D8B7 RID: 55479 RVA: 0x004DBFDA File Offset: 0x004DA1DA
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateBlobRef(Platform.XBL.Save.MasterFileTable.V04.BlobRef oldBlobRef, out Platform.XBL.Save.MasterFileTable.V05.BlobRef newBlobRef)
		{
			newBlobRef = new Platform.XBL.Save.MasterFileTable.V05.BlobRef
			{
				Id = oldBlobRef.Id,
				Length = oldBlobRef.Length,
				Hash = oldBlobRef.Hash
			};
		}

		// Token: 0x0600D8B8 RID: 55480 RVA: 0x004DC008 File Offset: 0x004DA208
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateNode(Platform.XBL.Save.MasterFileTable.V04.Node oldNode, Platform.XBL.Save.MasterFileTable.V05.Node newNode)
		{
			Migrator.MigrateNodeAttributes(oldNode.Attributes, out newNode.Attributes);
			IReadOnlyList<Platform.XBL.Save.MasterFileTable.V04.BlobRef> blobRefs = oldNode.BlobRefs;
			Platform.XBL.Save.MasterFileTable.V05.BlobRef[] array = new Platform.XBL.Save.MasterFileTable.V05.BlobRef[blobRefs.Count];
			for (int i = 0; i < blobRefs.Count; i++)
			{
				Migrator.MigrateBlobRef(blobRefs[i], out array[i]);
			}
			newNode.SetBlobRefs(array);
			foreach (Platform.XBL.Save.MasterFileTable.V04.Node node in oldNode.Children.Values)
			{
				Platform.XBL.Save.MasterFileTable.V05.Node orCreateChildNode = newNode.GetOrCreateChildNode(node.Name, node.IsDirectory());
				Migrator.MigrateNode(node, orCreateChildNode);
			}
			newNode.LastWriteTimeUtc = oldNode.LastWriteTimeUtc;
		}

		// Token: 0x0600D8B9 RID: 55481 RVA: 0x004DC0D4 File Offset: 0x004DA2D4
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateContainerData(Platform.XBL.Save.MasterFileTable.V04.ContainerData oldContainerData, Platform.XBL.Save.MasterFileTable.V05.ContainerData newContainerData)
		{
			Migrator.MigrateNode(oldContainerData.RootNode, newContainerData.RootNode);
		}

		// Token: 0x0600D8BA RID: 55482 RVA: 0x004DC0E8 File Offset: 0x004DA2E8
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void MigrateFromV4(PooledBinaryReader reader, PooledBinaryWriter writer)
		{
			using (Platform.XBL.Save.MasterFileTable.V04.ContainerData containerData = new Platform.XBL.Save.MasterFileTable.V04.ContainerData())
			{
				using (Platform.XBL.Save.MasterFileTable.V05.ContainerData containerData2 = new Platform.XBL.Save.MasterFileTable.V05.ContainerData())
				{
					containerData.Read(reader);
					Migrator.MigrateContainerData(containerData, containerData2);
					containerData2.Write(writer);
				}
			}
		}

		// Token: 0x0600D8BB RID: 55483 RVA: 0x004DC148 File Offset: 0x004DA348
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateNodeAttributes(Platform.XBL.Save.MasterFileTable.V05.NodeAttributes oldAttributes, out Platform.XBL.Save.MasterFileTable.Latest.NodeAttributes newAttributes)
		{
			newAttributes = Platform.XBL.Save.MasterFileTable.Latest.NodeAttributes.None;
			if (oldAttributes.HasFlag(Platform.XBL.Save.MasterFileTable.V05.NodeAttributes.Directory))
			{
				newAttributes |= Platform.XBL.Save.MasterFileTable.Latest.NodeAttributes.Directory;
			}
		}

		// Token: 0x0600D8BC RID: 55484 RVA: 0x004DC166 File Offset: 0x004DA366
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateBlobRef(Platform.XBL.Save.MasterFileTable.V05.BlobRef oldBlobRef, Platform.XBL.Save.MasterFileTable.Latest.BlobRef newBlobRef)
		{
			newBlobRef.Id = oldBlobRef.Id;
			newBlobRef.Length = oldBlobRef.Length;
			newBlobRef.Hash = oldBlobRef.Hash;
		}

		// Token: 0x0600D8BD RID: 55485 RVA: 0x004DC18C File Offset: 0x004DA38C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateNode(Platform.XBL.Save.MasterFileTable.V05.Node oldNode, Platform.XBL.Save.MasterFileTable.Latest.Node newNode)
		{
			Migrator.MigrateNodeAttributes(oldNode.Attributes, out newNode.Attributes);
			IReadOnlyList<Platform.XBL.Save.MasterFileTable.V05.BlobRef> blobRefs = oldNode.BlobRefs;
			Platform.XBL.Save.MasterFileTable.Latest.BlobRef[] array = new Platform.XBL.Save.MasterFileTable.Latest.BlobRef[blobRefs.Count];
			for (int i = 0; i < blobRefs.Count; i++)
			{
				array[i] = new Platform.XBL.Save.MasterFileTable.Latest.BlobRef();
				Migrator.MigrateBlobRef(blobRefs[i], array[i]);
			}
			newNode.SetBlobRefs(array);
			foreach (Platform.XBL.Save.MasterFileTable.V05.Node node in oldNode.Children.Values)
			{
				Platform.XBL.Save.MasterFileTable.Latest.Node orCreateChildNode = newNode.GetOrCreateChildNode(node.Name, node.IsDirectory());
				Migrator.MigrateNode(node, orCreateChildNode);
			}
			newNode.CreationTimeUtc = oldNode.LastWriteTimeUtc;
			newNode.LastWriteTimeUtc = oldNode.LastWriteTimeUtc;
		}

		// Token: 0x0600D8BE RID: 55486 RVA: 0x004DC268 File Offset: 0x004DA468
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateContainerData(Platform.XBL.Save.MasterFileTable.V05.ContainerData oldContainerData, Platform.XBL.Save.MasterFileTable.Latest.ContainerData newContainerData)
		{
			Migrator.MigrateNode(oldContainerData.RootNode, newContainerData.RootNode);
		}

		// Token: 0x0600D8BF RID: 55487 RVA: 0x004DC27C File Offset: 0x004DA47C
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void MigrateFromV5ContainerData(PooledBinaryReader reader, PooledBinaryWriter writer)
		{
			using (Platform.XBL.Save.MasterFileTable.V05.ContainerData containerData = new Platform.XBL.Save.MasterFileTable.V05.ContainerData())
			{
				using (Platform.XBL.Save.MasterFileTable.Latest.ContainerData containerData2 = new Platform.XBL.Save.MasterFileTable.Latest.ContainerData())
				{
					containerData.Read(reader);
					Migrator.MigrateContainerData(containerData, containerData2);
					containerData2.Write(writer);
				}
			}
		}

		// Token: 0x0600D8C0 RID: 55488 RVA: 0x004DC2DC File Offset: 0x004DA4DC
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateFromV5Node(PooledBinaryReader reader, PooledBinaryWriter writer)
		{
			using (Platform.XBL.Save.MasterFileTable.V05.Node node = new Platform.XBL.Save.MasterFileTable.V05.Node())
			{
				using (Platform.XBL.Save.MasterFileTable.Latest.Node node2 = new Platform.XBL.Save.MasterFileTable.Latest.Node())
				{
					node.Read(reader);
					Migrator.MigrateNode(node, node2);
					node2.Write(writer);
				}
			}
		}

		// Token: 0x0600D8C1 RID: 55489 RVA: 0x004DC33C File Offset: 0x004DA53C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateFromV5BlobRef(PooledBinaryReader reader, PooledBinaryWriter writer)
		{
			Platform.XBL.Save.MasterFileTable.V05.BlobRef blobRef = new Platform.XBL.Save.MasterFileTable.V05.BlobRef();
			Platform.XBL.Save.MasterFileTable.Latest.BlobRef blobRef2 = new Platform.XBL.Save.MasterFileTable.Latest.BlobRef();
			blobRef.Read(reader);
			Migrator.MigrateBlobRef(blobRef, blobRef2);
			blobRef2.Write(writer);
		}

		// Token: 0x0400A4AC RID: 42156
		public static readonly Dictionary<ushort, Action<PooledBinaryReader, PooledBinaryWriter>> s_containerDataMigrators = new Dictionary<ushort, Action<PooledBinaryReader, PooledBinaryWriter>>
		{
			{
				1,
				new Action<PooledBinaryReader, PooledBinaryWriter>(Migrator.MigrateFromV1)
			},
			{
				2,
				new Action<PooledBinaryReader, PooledBinaryWriter>(Migrator.MigrateFromV2)
			},
			{
				3,
				new Action<PooledBinaryReader, PooledBinaryWriter>(Migrator.MigrateFromV3)
			},
			{
				4,
				new Action<PooledBinaryReader, PooledBinaryWriter>(Migrator.MigrateFromV4)
			},
			{
				5,
				new Action<PooledBinaryReader, PooledBinaryWriter>(Migrator.MigrateFromV5ContainerData)
			}
		};

		// Token: 0x0400A4AD RID: 42157
		public static readonly Dictionary<ushort, Action<PooledBinaryReader, PooledBinaryWriter>> s_nodeMigrators = new Dictionary<ushort, Action<PooledBinaryReader, PooledBinaryWriter>> { 
		{
			5,
			new Action<PooledBinaryReader, PooledBinaryWriter>(Migrator.MigrateFromV5Node)
		} };

		// Token: 0x0400A4AE RID: 42158
		public static readonly Dictionary<ushort, Action<PooledBinaryReader, PooledBinaryWriter>> s_blobRefMigrators = new Dictionary<ushort, Action<PooledBinaryReader, PooledBinaryWriter>> { 
		{
			5,
			new Action<PooledBinaryReader, PooledBinaryWriter>(Migrator.MigrateFromV5BlobRef)
		} };
	}
}
