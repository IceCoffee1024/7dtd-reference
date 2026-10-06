using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V05
{
	// Token: 0x02001C99 RID: 7321
	public sealed class Node : IDisposable, IMigratable
	{
		// Token: 0x17001B56 RID: 6998
		// (get) Token: 0x0600D8DF RID: 55519 RVA: 0x004DCA7F File Offset: 0x004DAC7F
		// (set) Token: 0x0600D8E0 RID: 55520 RVA: 0x004DCA87 File Offset: 0x004DAC87
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 5;

		// Token: 0x17001B57 RID: 6999
		// (get) Token: 0x0600D8E1 RID: 55521 RVA: 0x004DCA90 File Offset: 0x004DAC90
		// (set) Token: 0x0600D8E2 RID: 55522 RVA: 0x004DCA98 File Offset: 0x004DAC98
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B58 RID: 7000
		// (get) Token: 0x0600D8E3 RID: 55523 RVA: 0x004DCAA1 File Offset: 0x004DACA1
		public IReadOnlyList<BlobRef> BlobRefs
		{
			get
			{
				return this.m_blobRefs;
			}
		}

		// Token: 0x17001B59 RID: 7001
		// (get) Token: 0x0600D8E4 RID: 55524 RVA: 0x004DCAA9 File Offset: 0x004DACA9
		// (set) Token: 0x0600D8E5 RID: 55525 RVA: 0x004DCAB1 File Offset: 0x004DACB1
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001B5A RID: 7002
		// (get) Token: 0x0600D8E6 RID: 55526 RVA: 0x004DCABA File Offset: 0x004DACBA
		public IReadOnlyDictionary<string, Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x0600D8E7 RID: 55527 RVA: 0x004DCAC4 File Offset: 0x004DACC4
		public Node()
		{
			this.Name = string.Empty;
			this.LastWriteTimeUtc = Node.DefaultLastWriteTime;
			this.Attributes = NodeAttributes.None;
			this.m_blobRefs = Array.Empty<BlobRef>();
			this.m_blobLock = new object();
			this.m_children = new StringSpanDictionary<Node>(new SortedDictionary<string, Node>());
			this.m_lock = new object();
			this.Parent = null;
		}

		// Token: 0x0600D8E8 RID: 55528 RVA: 0x004DCB40 File Offset: 0x004DAD40
		public void SetBlobRefs(BlobRef[] blobRefs)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_blobRefs = blobRefs;
			}
		}

		// Token: 0x0600D8E9 RID: 55529 RVA: 0x004DCB84 File Offset: 0x004DAD84
		public override string ToString()
		{
			object @lock = this.m_lock;
			string text;
			lock (@lock)
			{
				text = string.Format("{0}[{1}=\"{2}\", {3}=\"{4}\", {5}=\"{6}\", {7}=[{8}], #{9}={10}]", new object[]
				{
					"Node",
					"Name",
					this.Name,
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

		// Token: 0x0600D8EA RID: 55530 RVA: 0x004DCC50 File Offset: 0x004DAE50
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

		// Token: 0x0600D8EB RID: 55531 RVA: 0x004DCDD0 File Offset: 0x004DAFD0
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

		// Token: 0x0600D8EC RID: 55532 RVA: 0x004DCF5C File Offset: 0x004DB15C
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

		// Token: 0x0600D8ED RID: 55533 RVA: 0x004DCFFC File Offset: 0x004DB1FC
		public bool IsDirectory()
		{
			return this.Attributes.HasFlag(NodeAttributes.Directory);
		}

		// Token: 0x0600D8EE RID: 55534 RVA: 0x004DD014 File Offset: 0x004DB214
		public bool IsFile()
		{
			return !this.IsDirectory();
		}

		// Token: 0x0600D8EF RID: 55535 RVA: 0x004DD020 File Offset: 0x004DB220
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

		// Token: 0x0600D8F0 RID: 55536 RVA: 0x004DD074 File Offset: 0x004DB274
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory)
		{
			bool flag;
			return this.GetOrCreateChildNode(name, createDirectory, out flag);
		}

		// Token: 0x0600D8F1 RID: 55537 RVA: 0x004DD08C File Offset: 0x004DB28C
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

		// Token: 0x0600D8F2 RID: 55538 RVA: 0x004DD14C File Offset: 0x004DB34C
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

		// Token: 0x0600D8F3 RID: 55539 RVA: 0x004DD1B8 File Offset: 0x004DB3B8
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
				Node.<Enumerate>g__AddNodeChildrenToStack|37_0(stack, this);
			}
			while (stack.Count > 0)
			{
				Node node = stack.Pop();
				list.Add(node);
				Node.<Enumerate>g__AddNodeChildrenToStack|37_0(stack, node);
			}
			return list;
		}

		// Token: 0x0600D8F4 RID: 55540 RVA: 0x004DD254 File Offset: 0x004DB454
		public void MoveChild(Node child)
		{
			this.MoveChild(child, child.Name);
		}

		// Token: 0x0600D8F5 RID: 55541 RVA: 0x004DD268 File Offset: 0x004DB468
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
						Node.<MoveChild>g__MoveChildInternal|39_0(parent, this, child, newName);
					}
					else
					{
						object lock3 = parent.m_lock;
						lock (lock3)
						{
							Node.<MoveChild>g__MoveChildInternal|39_0(parent, this, child, newName);
						}
					}
				}
			}
		}

		// Token: 0x0600D8F7 RID: 55543 RVA: 0x004DD330 File Offset: 0x004DB530
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <Enumerate>g__AddNodeChildrenToStack|37_0(Stack<Node> stack, Node currentNode)
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

		// Token: 0x0600D8F8 RID: 55544 RVA: 0x004DD3B0 File Offset: 0x004DB5B0
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <MoveChild>g__MoveChildInternal|39_0(Node oldParent, Node newParent, Node child, StringSpan newName)
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

		// Token: 0x0400A4BD RID: 42173
		public const ushort VERSION = 5;

		// Token: 0x0400A4BE RID: 42174
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A4C1 RID: 42177
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A4C2 RID: 42178
		public NodeAttributes Attributes;

		// Token: 0x0400A4C3 RID: 42179
		[PublicizedFrom(EAccessModifier.Private)]
		public BlobRef[] m_blobRefs;

		// Token: 0x0400A4C5 RID: 42181
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringSpanDictionary<Node> m_children;

		// Token: 0x0400A4C6 RID: 42182
		public readonly object m_blobLock;

		// Token: 0x0400A4C7 RID: 42183
		public readonly object m_lock;

		// Token: 0x0400A4C8 RID: 42184
		public Node Parent;
	}
}
