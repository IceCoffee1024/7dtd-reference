using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V05
{
	// Token: 0x02001C67 RID: 7271
	public sealed class Node : IDisposable, IMigratable
	{
		// Token: 0x17001AB0 RID: 6832
		// (get) Token: 0x0600D766 RID: 55142 RVA: 0x004D7F43 File Offset: 0x004D6143
		// (set) Token: 0x0600D767 RID: 55143 RVA: 0x004D7F4B File Offset: 0x004D614B
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 5;

		// Token: 0x17001AB1 RID: 6833
		// (get) Token: 0x0600D768 RID: 55144 RVA: 0x004D7F54 File Offset: 0x004D6154
		// (set) Token: 0x0600D769 RID: 55145 RVA: 0x004D7F5C File Offset: 0x004D615C
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AB2 RID: 6834
		// (get) Token: 0x0600D76A RID: 55146 RVA: 0x004D7F65 File Offset: 0x004D6165
		public IReadOnlyList<BlobRef> BlobRefs
		{
			get
			{
				return this.m_blobRefs;
			}
		}

		// Token: 0x17001AB3 RID: 6835
		// (get) Token: 0x0600D76B RID: 55147 RVA: 0x004D7F6D File Offset: 0x004D616D
		// (set) Token: 0x0600D76C RID: 55148 RVA: 0x004D7F75 File Offset: 0x004D6175
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001AB4 RID: 6836
		// (get) Token: 0x0600D76D RID: 55149 RVA: 0x004D7F7E File Offset: 0x004D617E
		public IReadOnlyDictionary<string, Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x0600D76E RID: 55150 RVA: 0x004D7F88 File Offset: 0x004D6188
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

		// Token: 0x0600D76F RID: 55151 RVA: 0x004D8004 File Offset: 0x004D6204
		public void SetBlobRefs(BlobRef[] blobRefs)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_blobRefs = blobRefs;
			}
		}

		// Token: 0x0600D770 RID: 55152 RVA: 0x004D8048 File Offset: 0x004D6248
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

		// Token: 0x0600D771 RID: 55153 RVA: 0x004D8114 File Offset: 0x004D6314
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

		// Token: 0x0600D772 RID: 55154 RVA: 0x004D8294 File Offset: 0x004D6494
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

		// Token: 0x0600D773 RID: 55155 RVA: 0x004D8420 File Offset: 0x004D6620
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

		// Token: 0x0600D774 RID: 55156 RVA: 0x004D84C0 File Offset: 0x004D66C0
		public bool IsDirectory()
		{
			return this.Attributes.HasFlag(NodeAttributes.Directory);
		}

		// Token: 0x0600D775 RID: 55157 RVA: 0x004D84D8 File Offset: 0x004D66D8
		public bool IsFile()
		{
			return !this.IsDirectory();
		}

		// Token: 0x0600D776 RID: 55158 RVA: 0x004D84E4 File Offset: 0x004D66E4
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

		// Token: 0x0600D777 RID: 55159 RVA: 0x004D8538 File Offset: 0x004D6738
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory)
		{
			bool flag;
			return this.GetOrCreateChildNode(name, createDirectory, out flag);
		}

		// Token: 0x0600D778 RID: 55160 RVA: 0x004D8550 File Offset: 0x004D6750
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

		// Token: 0x0600D779 RID: 55161 RVA: 0x004D8610 File Offset: 0x004D6810
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

		// Token: 0x0600D77A RID: 55162 RVA: 0x004D867C File Offset: 0x004D687C
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

		// Token: 0x0600D77B RID: 55163 RVA: 0x004D8718 File Offset: 0x004D6918
		public void MoveChild(Node child)
		{
			this.MoveChild(child, child.Name);
		}

		// Token: 0x0600D77C RID: 55164 RVA: 0x004D872C File Offset: 0x004D692C
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

		// Token: 0x0600D77E RID: 55166 RVA: 0x004D87F4 File Offset: 0x004D69F4
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

		// Token: 0x0600D77F RID: 55167 RVA: 0x004D8874 File Offset: 0x004D6A74
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

		// Token: 0x0400A3E8 RID: 41960
		public const ushort VERSION = 5;

		// Token: 0x0400A3E9 RID: 41961
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A3EC RID: 41964
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A3ED RID: 41965
		public NodeAttributes Attributes;

		// Token: 0x0400A3EE RID: 41966
		[PublicizedFrom(EAccessModifier.Private)]
		public BlobRef[] m_blobRefs;

		// Token: 0x0400A3F0 RID: 41968
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringSpanDictionary<Node> m_children;

		// Token: 0x0400A3F1 RID: 41969
		public readonly object m_blobLock;

		// Token: 0x0400A3F2 RID: 41970
		public readonly object m_lock;

		// Token: 0x0400A3F3 RID: 41971
		public Node Parent;
	}
}
