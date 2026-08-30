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
	// Token: 0x02001C3D RID: 7229
	public static class Migrator
	{
		// Token: 0x0600D5A1 RID: 54689 RVA: 0x004D0B8C File Offset: 0x004CED8C
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

		// Token: 0x0600D5A2 RID: 54690 RVA: 0x004D0CB0 File Offset: 0x004CEEB0
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

		// Token: 0x0600D5A3 RID: 54691 RVA: 0x004D0D6C File Offset: 0x004CEF6C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateContainerData(Platform.XBL.Save.MasterFileTable.V01.ContainerData oldContainerData, Platform.XBL.Save.MasterFileTable.V02.ContainerData newContainerData)
		{
			Migrator.MigrateNode(oldContainerData.RootNode, newContainerData.RootNode);
		}

		// Token: 0x0600D5A4 RID: 54692 RVA: 0x004D0D80 File Offset: 0x004CEF80
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

		// Token: 0x0600D5A5 RID: 54693 RVA: 0x004D0DE0 File Offset: 0x004CEFE0
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

		// Token: 0x0600D5A6 RID: 54694 RVA: 0x004D0E7C File Offset: 0x004CF07C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateContainerData(Platform.XBL.Save.MasterFileTable.V02.ContainerData oldContainerData, Platform.XBL.Save.MasterFileTable.V03.ContainerData newContainerData)
		{
			Migrator.MigrateNode(oldContainerData.RootNode, newContainerData.RootNode);
		}

		// Token: 0x0600D5A7 RID: 54695 RVA: 0x004D0E90 File Offset: 0x004CF090
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

		// Token: 0x0600D5A8 RID: 54696 RVA: 0x004D0EF0 File Offset: 0x004CF0F0
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateNodeAttributes(Platform.XBL.Save.MasterFileTable.V03.NodeAttributes oldAttributes, out Platform.XBL.Save.MasterFileTable.V04.NodeAttributes newAttributes)
		{
			newAttributes = Platform.XBL.Save.MasterFileTable.V04.NodeAttributes.None;
			if (oldAttributes.HasFlag(Platform.XBL.Save.MasterFileTable.V03.NodeAttributes.Directory))
			{
				newAttributes |= Platform.XBL.Save.MasterFileTable.V04.NodeAttributes.Directory;
			}
		}

		// Token: 0x0600D5A9 RID: 54697 RVA: 0x004D0F0E File Offset: 0x004CF10E
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateBlobRef(ulong oldBlobRef, out Platform.XBL.Save.MasterFileTable.V04.BlobRef newBlobRef)
		{
			newBlobRef = new Platform.XBL.Save.MasterFileTable.V04.BlobRef(oldBlobRef, 0U);
		}

		// Token: 0x0600D5AA RID: 54698 RVA: 0x004D0F1C File Offset: 0x004CF11C
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

		// Token: 0x0600D5AB RID: 54699 RVA: 0x004D0FE8 File Offset: 0x004CF1E8
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateContainerData(Platform.XBL.Save.MasterFileTable.V03.ContainerData oldContainerData, Platform.XBL.Save.MasterFileTable.V04.ContainerData newContainerData)
		{
			Migrator.MigrateNode(oldContainerData.RootNode, newContainerData.RootNode);
		}

		// Token: 0x0600D5AC RID: 54700 RVA: 0x004D0FFC File Offset: 0x004CF1FC
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

		// Token: 0x0600D5AD RID: 54701 RVA: 0x004D105C File Offset: 0x004CF25C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateNodeAttributes(Platform.XBL.Save.MasterFileTable.V04.NodeAttributes oldAttributes, out Platform.XBL.Save.MasterFileTable.V05.NodeAttributes newAttributes)
		{
			newAttributes = Platform.XBL.Save.MasterFileTable.V05.NodeAttributes.None;
			if (oldAttributes.HasFlag(Platform.XBL.Save.MasterFileTable.V04.NodeAttributes.Directory))
			{
				newAttributes |= Platform.XBL.Save.MasterFileTable.V05.NodeAttributes.Directory;
			}
		}

		// Token: 0x0600D5AE RID: 54702 RVA: 0x004D107A File Offset: 0x004CF27A
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

		// Token: 0x0600D5AF RID: 54703 RVA: 0x004D10A8 File Offset: 0x004CF2A8
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

		// Token: 0x0600D5B0 RID: 54704 RVA: 0x004D1174 File Offset: 0x004CF374
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateContainerData(Platform.XBL.Save.MasterFileTable.V04.ContainerData oldContainerData, Platform.XBL.Save.MasterFileTable.V05.ContainerData newContainerData)
		{
			Migrator.MigrateNode(oldContainerData.RootNode, newContainerData.RootNode);
		}

		// Token: 0x0600D5B1 RID: 54705 RVA: 0x004D1188 File Offset: 0x004CF388
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

		// Token: 0x0600D5B2 RID: 54706 RVA: 0x004D11E8 File Offset: 0x004CF3E8
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateNodeAttributes(Platform.XBL.Save.MasterFileTable.V05.NodeAttributes oldAttributes, out Platform.XBL.Save.MasterFileTable.Latest.NodeAttributes newAttributes)
		{
			newAttributes = Platform.XBL.Save.MasterFileTable.Latest.NodeAttributes.None;
			if (oldAttributes.HasFlag(Platform.XBL.Save.MasterFileTable.V05.NodeAttributes.Directory))
			{
				newAttributes |= Platform.XBL.Save.MasterFileTable.Latest.NodeAttributes.Directory;
			}
		}

		// Token: 0x0600D5B3 RID: 54707 RVA: 0x004D1206 File Offset: 0x004CF406
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateBlobRef(Platform.XBL.Save.MasterFileTable.V05.BlobRef oldBlobRef, Platform.XBL.Save.MasterFileTable.Latest.BlobRef newBlobRef)
		{
			newBlobRef.Id = oldBlobRef.Id;
			newBlobRef.Length = oldBlobRef.Length;
			newBlobRef.Hash = oldBlobRef.Hash;
		}

		// Token: 0x0600D5B4 RID: 54708 RVA: 0x004D122C File Offset: 0x004CF42C
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

		// Token: 0x0600D5B5 RID: 54709 RVA: 0x004D1308 File Offset: 0x004CF508
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateContainerData(Platform.XBL.Save.MasterFileTable.V05.ContainerData oldContainerData, Platform.XBL.Save.MasterFileTable.Latest.ContainerData newContainerData)
		{
			Migrator.MigrateNode(oldContainerData.RootNode, newContainerData.RootNode);
		}

		// Token: 0x0600D5B6 RID: 54710 RVA: 0x004D131C File Offset: 0x004CF51C
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

		// Token: 0x0600D5B7 RID: 54711 RVA: 0x004D137C File Offset: 0x004CF57C
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

		// Token: 0x0600D5B8 RID: 54712 RVA: 0x004D13DC File Offset: 0x004CF5DC
		[PublicizedFrom(EAccessModifier.Private)]
		public static void MigrateFromV5BlobRef(PooledBinaryReader reader, PooledBinaryWriter writer)
		{
			Platform.XBL.Save.MasterFileTable.V05.BlobRef blobRef = new Platform.XBL.Save.MasterFileTable.V05.BlobRef();
			Platform.XBL.Save.MasterFileTable.Latest.BlobRef blobRef2 = new Platform.XBL.Save.MasterFileTable.Latest.BlobRef();
			blobRef.Read(reader);
			Migrator.MigrateBlobRef(blobRef, blobRef2);
			blobRef2.Write(writer);
		}

		// Token: 0x0400A2A6 RID: 41638
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

		// Token: 0x0400A2A7 RID: 41639
		public static readonly Dictionary<ushort, Action<PooledBinaryReader, PooledBinaryWriter>> s_nodeMigrators = new Dictionary<ushort, Action<PooledBinaryReader, PooledBinaryWriter>> { 
		{
			5,
			new Action<PooledBinaryReader, PooledBinaryWriter>(Migrator.MigrateFromV5Node)
		} };

		// Token: 0x0400A2A8 RID: 41640
		public static readonly Dictionary<ushort, Action<PooledBinaryReader, PooledBinaryWriter>> s_blobRefMigrators = new Dictionary<ushort, Action<PooledBinaryReader, PooledBinaryWriter>> { 
		{
			5,
			new Action<PooledBinaryReader, PooledBinaryWriter>(Migrator.MigrateFromV5BlobRef)
		} };
	}
}
