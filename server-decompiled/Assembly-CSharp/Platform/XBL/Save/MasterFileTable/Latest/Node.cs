using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.Latest
{
	// Token: 0x02001CA8 RID: 7336
	public sealed class Node : IDisposable, IMigratable
	{
		// Token: 0x17001B71 RID: 7025
		// (get) Token: 0x0600D984 RID: 55684 RVA: 0x004DFA5B File Offset: 0x004DDC5B
		// (set) Token: 0x0600D985 RID: 55685 RVA: 0x004DFA63 File Offset: 0x004DDC63
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 6;

		// Token: 0x17001B72 RID: 7026
		// (get) Token: 0x0600D986 RID: 55686 RVA: 0x004DFA6C File Offset: 0x004DDC6C
		// (set) Token: 0x0600D987 RID: 55687 RVA: 0x004DFA74 File Offset: 0x004DDC74
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B73 RID: 7027
		// (get) Token: 0x0600D988 RID: 55688 RVA: 0x004DFA7D File Offset: 0x004DDC7D
		public IReadOnlyList<BlobRef> BlobRefs
		{
			get
			{
				return this.m_blobRefs;
			}
		}

		// Token: 0x17001B74 RID: 7028
		// (get) Token: 0x0600D989 RID: 55689 RVA: 0x004DFA85 File Offset: 0x004DDC85
		// (set) Token: 0x0600D98A RID: 55690 RVA: 0x004DFA8D File Offset: 0x004DDC8D
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001B75 RID: 7029
		// (get) Token: 0x0600D98B RID: 55691 RVA: 0x004DFA96 File Offset: 0x004DDC96
		public IReadOnlyDictionary<string, Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x0600D98C RID: 55692 RVA: 0x004DFAA0 File Offset: 0x004DDCA0
		public Node()
		{
			this.Name = string.Empty;
			this.CreationTimeUtc = DateTime.UtcNow;
			this.LastWriteTimeUtc = Node.DefaultLastWriteTime;
			this.Attributes = NodeAttributes.None;
			this.m_blobRefs = Array.Empty<BlobRef>();
			this.m_blobLock = new object();
			this.m_children = new StringSpanDictionary<Node>(new SortedDictionary<string, Node>());
			this.m_lock = new object();
			this.Parent = null;
		}

		// Token: 0x0600D98D RID: 55693 RVA: 0x004DFB28 File Offset: 0x004DDD28
		public void SetBlobRefs(BlobRef[] blobRefs)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_blobRefs = blobRefs;
			}
		}

		// Token: 0x0600D98E RID: 55694 RVA: 0x004DFB6C File Offset: 0x004DDD6C
		public override string ToString()
		{
			object @lock = this.m_lock;
			string text;
			lock (@lock)
			{
				text = string.Format("{0}[{1}=\"{2}\", {3}=\"{4}\", {5}=\"{6}\", {7}=\"{8}\", {9}=[{10}], #{11}={12}]", new object[]
				{
					"Node",
					"Name",
					this.Name,
					"CreationTimeUtc",
					this.CreationTimeUtc,
					"LastWriteTimeUtc",
					this.LastWriteTimeUtc,
					"Attributes",
					this.Attributes,
					"BlobRefs",
					string.Join<BlobRef>(", ", this.BlobRefs),
					"Children",
					this.Children.Count
				});
			}
			return text;
		}

		// Token: 0x0600D98F RID: 55695 RVA: 0x004DFC50 File Offset: 0x004DDE50
		public void Write(PooledBinaryWriter writer)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				writer.Write(this.Version);
				int num = 0;
				long position = writer.BaseStream.Position;
				writer.Write(num);
				long position2 = writer.BaseStream.Position;
				writer.Write(this.Name);
				writer.Write(this.LastWriteTimeUtc.Ticks);
				writer.Write(Convert.ToUInt32(this.Attributes));
				BlobRef[] blobRefs = this.m_blobRefs;
				ushort num2 = (ushort)blobRefs.Length;
				writer.Write(num2);
				BlobRef[] array = blobRefs;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Write(writer);
				}
				writer.Write(this.CreationTimeUtc.Ticks);
				writer.Write(this.FutureData);
				long position3 = writer.BaseStream.Position;
				num = (int)(position3 - position2);
				writer.BaseStream.Position = position;
				writer.Write(num);
				writer.BaseStream.Position = position3;
				ushort num3 = (ushort)this.Children.Count;
				writer.Write(num3);
				foreach (Node node in this.Children.Values)
				{
					node.Write(writer);
				}
			}
		}

		// Token: 0x0600D990 RID: 55696 RVA: 0x004DFDE0 File Offset: 0x004DDFE0
		public void Read(PooledBinaryReader reader)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.Version = reader.ReadUInt16();
				int num = reader.ReadInt32();
				long position = reader.BaseStream.Position;
				this.Name = reader.ReadString();
				this.LastWriteTimeUtc = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
				this.Attributes = (NodeAttributes)reader.ReadUInt32();
				ushort num2 = reader.ReadUInt16();
				BlobRef[] array = new BlobRef[(int)num2];
				for (int i = 0; i < (int)num2; i++)
				{
					BlobRef blobRef = new BlobRef();
					Migrator.ReadMigrate(reader.BaseStream, blobRef, "BlobRef", Migrator.s_blobRefMigrators);
					array[i] = blobRef;
				}
				this.m_blobRefs = array;
				this.CreationTimeUtc = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
				long position2 = reader.BaseStream.Position;
				this.FutureData = new byte[(int)((long)num - (position2 - position))];
				int num3;
				if (!reader.TryReadAllBytes(this.FutureData, out num3))
				{
					throw new IOException(string.Format("Expected {0} bytes to be read for future data but only got {1} bytes.", this.FutureData.Length, num3));
				}
				ushort num4 = reader.ReadUInt16();
				for (int j = 0; j < (int)num4; j++)
				{
					Node node = new Node();
					Migrator.ReadMigrate(reader.BaseStream, node, "Node", Migrator.s_nodeMigrators);
					this.m_children.Add(node.Name, node);
					node.Parent = this;
				}
			}
		}

		// Token: 0x0600D991 RID: 55697 RVA: 0x004DFF7C File Offset: 0x004DE17C
		public void Dispose()
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				foreach (Node node in this.Children.Values)
				{
					node.Dispose();
				}
				this.m_children.Clear();
				this.Parent = null;
				this.m_blobRefs = Array.Empty<BlobRef>();
				this.Name = string.Empty;
			}
		}

		// Token: 0x0600D992 RID: 55698 RVA: 0x004E001C File Offset: 0x004DE21C
		public bool IsDirectory()
		{
			return this.Attributes.HasFlag(NodeAttributes.Directory);
		}

		// Token: 0x0600D993 RID: 55699 RVA: 0x004E0034 File Offset: 0x004DE234
		public bool IsFile()
		{
			return !this.IsDirectory();
		}

		// Token: 0x0600D994 RID: 55700 RVA: 0x004E0040 File Offset: 0x004DE240
		public Node GetChildNode(StringSpan name)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
			{
				Node node;
				if (!this.m_children.TryGetValue(name, out node))
				{
					node2 = null;
				}
				else
				{
					node2 = node;
				}
			}
			return node2;
		}

		// Token: 0x0600D995 RID: 55701 RVA: 0x004E0094 File Offset: 0x004DE294
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory)
		{
			bool flag;
			return this.GetOrCreateChildNode(name, createDirectory, out flag);
		}

		// Token: 0x0600D996 RID: 55702 RVA: 0x004E00AC File Offset: 0x004DE2AC
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory, out bool wasCreated)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
			{
				Node node;
				if (this.m_children.TryGetValue(name, out node))
				{
					wasCreated = false;
					node2 = node;
				}
				else
				{
					string text = name.ToString();
					node = new Node
					{
						Name = text
					};
					node.Parent = this;
					this.m_children.Add(text, node);
					this.LastWriteTimeUtc = (node.LastWriteTimeUtc = DateTime.UtcNow);
					this.Attributes |= NodeAttributes.Directory;
					if (createDirectory)
					{
						node.Attributes |= NodeAttributes.Directory;
					}
					wasCreated = true;
					node2 = node;
				}
			}
			return node2;
		}

		// Token: 0x0600D997 RID: 55703 RVA: 0x004E016C File Offset: 0x004DE36C
		public Node DeleteChildNode(StringSpan name)
		{
			object @lock = this.m_lock;
			Node node2;
			lock (@lock)
			{
				Node node;
				if (!this.m_children.TryGetValue(name, out node) || !this.m_children.Remove(name))
				{
					node2 = null;
				}
				else
				{
					this.LastWriteTimeUtc = DateTime.UtcNow;
					node2 = node;
				}
			}
			return node2;
		}

		// Token: 0x0600D998 RID: 55704 RVA: 0x004E01D8 File Offset: 0x004DE3D8
		public IEnumerable<Node> Enumerate(bool includeSelf, bool recursive)
		{
			List<Node> list = new List<Node>();
			if (!recursive)
			{
				if (includeSelf)
				{
					list.Add(this);
				}
				object @lock = this.m_lock;
				lock (@lock)
				{
					list.AddRange(this.Children.Values);
					return list;
				}
			}
			Stack<Node> stack = new Stack<Node>();
			if (includeSelf)
			{
				stack.Push(this);
			}
			else
			{
				Node.<Enumerate>g__AddNodeChildrenToStack|38_0(stack, this);
			}
			while (stack.Count > 0)
			{
				Node node = stack.Pop();
				list.Add(node);
				Node.<Enumerate>g__AddNodeChildrenToStack|38_0(stack, node);
			}
			return list;
		}

		// Token: 0x0600D999 RID: 55705 RVA: 0x004E0274 File Offset: 0x004DE474
		public void MoveChild(Node child)
		{
			this.MoveChild(child, child.Name);
		}

		// Token: 0x0600D99A RID: 55706 RVA: 0x004E0288 File Offset: 0x004DE488
		public void MoveChild(Node child, StringSpan newName)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				object lock2 = child.m_lock;
				lock (lock2)
				{
					Node parent = child.Parent;
					if (parent == null || parent == this)
					{
						Node.<MoveChild>g__MoveChildInternal|40_0(parent, this, child, newName);
					}
					else
					{
						object lock3 = parent.m_lock;
						lock (lock3)
						{
							Node.<MoveChild>g__MoveChildInternal|40_0(parent, this, child, newName);
						}
					}
				}
			}
		}

		// Token: 0x0600D99C RID: 55708 RVA: 0x004E0350 File Offset: 0x004DE550
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <Enumerate>g__AddNodeChildrenToStack|38_0(Stack<Node> stack, Node currentNode)
		{
			object @lock = currentNode.m_lock;
			lock (@lock)
			{
				foreach (Node node in currentNode.Children.Values.Reverse<Node>())
				{
					stack.Push(node);
				}
			}
		}

		// Token: 0x0600D99D RID: 55709 RVA: 0x004E03D0 File Offset: 0x004DE5D0
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <MoveChild>g__MoveChildInternal|40_0(Node oldParent, Node newParent, Node child, StringSpan newName)
		{
			if (oldParent == newParent && child.Name == newName)
			{
				return;
			}
			DateTime utcNow = DateTime.UtcNow;
			if (oldParent != null)
			{
				oldParent.m_children.Remove(child.Name);
				oldParent.LastWriteTimeUtc = utcNow;
			}
			if (child.Name != newName)
			{
				child.Name = newName.ToString();
			}
			child.Parent = newParent;
			Node node;
			newParent.m_children.Remove(child.Name, out node);
			if (node != null && node != child)
			{
				node.Dispose();
			}
			newParent.m_children.Add(child.Name, child);
			newParent.LastWriteTimeUtc = utcNow;
		}

		// Token: 0x0400A50C RID: 42252
		public const ushort VERSION = 6;

		// Token: 0x0400A50D RID: 42253
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A510 RID: 42256
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A511 RID: 42257
		public NodeAttributes Attributes;

		// Token: 0x0400A512 RID: 42258
		[PublicizedFrom(EAccessModifier.Private)]
		public BlobRef[] m_blobRefs;

		// Token: 0x0400A513 RID: 42259
		public DateTime CreationTimeUtc;

		// Token: 0x0400A515 RID: 42261
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringSpanDictionary<Node> m_children;

		// Token: 0x0400A516 RID: 42262
		public readonly object m_blobLock;

		// Token: 0x0400A517 RID: 42263
		public readonly object m_lock;

		// Token: 0x0400A518 RID: 42264
		public Node Parent;
	}
}
