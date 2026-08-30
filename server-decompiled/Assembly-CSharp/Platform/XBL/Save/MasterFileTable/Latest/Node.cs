using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.Latest
{
	// Token: 0x02001C76 RID: 7286
	public sealed class Node : IDisposable, IMigratable
	{
		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x0600D80B RID: 55307 RVA: 0x004DAF1F File Offset: 0x004D911F
		// (set) Token: 0x0600D80C RID: 55308 RVA: 0x004DAF27 File Offset: 0x004D9127
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 6;

		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x0600D80D RID: 55309 RVA: 0x004DAF30 File Offset: 0x004D9130
		// (set) Token: 0x0600D80E RID: 55310 RVA: 0x004DAF38 File Offset: 0x004D9138
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x0600D80F RID: 55311 RVA: 0x004DAF41 File Offset: 0x004D9141
		public IReadOnlyList<BlobRef> BlobRefs
		{
			get
			{
				return this.m_blobRefs;
			}
		}

		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x0600D810 RID: 55312 RVA: 0x004DAF49 File Offset: 0x004D9149
		// (set) Token: 0x0600D811 RID: 55313 RVA: 0x004DAF51 File Offset: 0x004D9151
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x0600D812 RID: 55314 RVA: 0x004DAF5A File Offset: 0x004D915A
		public IReadOnlyDictionary<string, Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x0600D813 RID: 55315 RVA: 0x004DAF64 File Offset: 0x004D9164
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

		// Token: 0x0600D814 RID: 55316 RVA: 0x004DAFEC File Offset: 0x004D91EC
		public void SetBlobRefs(BlobRef[] blobRefs)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_blobRefs = blobRefs;
			}
		}

		// Token: 0x0600D815 RID: 55317 RVA: 0x004DB030 File Offset: 0x004D9230
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

		// Token: 0x0600D816 RID: 55318 RVA: 0x004DB114 File Offset: 0x004D9314
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

		// Token: 0x0600D817 RID: 55319 RVA: 0x004DB2A4 File Offset: 0x004D94A4
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

		// Token: 0x0600D818 RID: 55320 RVA: 0x004DB440 File Offset: 0x004D9640
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

		// Token: 0x0600D819 RID: 55321 RVA: 0x004DB4E0 File Offset: 0x004D96E0
		public bool IsDirectory()
		{
			return this.Attributes.HasFlag(NodeAttributes.Directory);
		}

		// Token: 0x0600D81A RID: 55322 RVA: 0x004DB4F8 File Offset: 0x004D96F8
		public bool IsFile()
		{
			return !this.IsDirectory();
		}

		// Token: 0x0600D81B RID: 55323 RVA: 0x004DB504 File Offset: 0x004D9704
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

		// Token: 0x0600D81C RID: 55324 RVA: 0x004DB558 File Offset: 0x004D9758
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory)
		{
			bool flag;
			return this.GetOrCreateChildNode(name, createDirectory, out flag);
		}

		// Token: 0x0600D81D RID: 55325 RVA: 0x004DB570 File Offset: 0x004D9770
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

		// Token: 0x0600D81E RID: 55326 RVA: 0x004DB630 File Offset: 0x004D9830
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

		// Token: 0x0600D81F RID: 55327 RVA: 0x004DB69C File Offset: 0x004D989C
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

		// Token: 0x0600D820 RID: 55328 RVA: 0x004DB738 File Offset: 0x004D9938
		public void MoveChild(Node child)
		{
			this.MoveChild(child, child.Name);
		}

		// Token: 0x0600D821 RID: 55329 RVA: 0x004DB74C File Offset: 0x004D994C
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

		// Token: 0x0600D823 RID: 55331 RVA: 0x004DB814 File Offset: 0x004D9A14
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

		// Token: 0x0600D824 RID: 55332 RVA: 0x004DB894 File Offset: 0x004D9A94
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

		// Token: 0x0400A437 RID: 42039
		public const ushort VERSION = 6;

		// Token: 0x0400A438 RID: 42040
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A43B RID: 42043
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A43C RID: 42044
		public NodeAttributes Attributes;

		// Token: 0x0400A43D RID: 42045
		[PublicizedFrom(EAccessModifier.Private)]
		public BlobRef[] m_blobRefs;

		// Token: 0x0400A43E RID: 42046
		public DateTime CreationTimeUtc;

		// Token: 0x0400A440 RID: 42048
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringSpanDictionary<Node> m_children;

		// Token: 0x0400A441 RID: 42049
		public readonly object m_blobLock;

		// Token: 0x0400A442 RID: 42050
		public readonly object m_lock;

		// Token: 0x0400A443 RID: 42051
		public Node Parent;
	}
}
