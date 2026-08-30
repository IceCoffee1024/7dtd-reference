using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.Latest
{
	// Token: 0x02001C4F RID: 7247
	public sealed class Node : IDisposable, IMigratable
	{
		// Token: 0x17001A59 RID: 6745
		// (get) Token: 0x0600D67B RID: 54907 RVA: 0x004D4AFB File Offset: 0x004D2CFB
		// (set) Token: 0x0600D67C RID: 54908 RVA: 0x004D4B03 File Offset: 0x004D2D03
		public ushort Version
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = 6;

		// Token: 0x17001A5A RID: 6746
		// (get) Token: 0x0600D67D RID: 54909 RVA: 0x004D4B0C File Offset: 0x004D2D0C
		// (set) Token: 0x0600D67E RID: 54910 RVA: 0x004D4B14 File Offset: 0x004D2D14
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A5B RID: 6747
		// (get) Token: 0x0600D67F RID: 54911 RVA: 0x004D4B1D File Offset: 0x004D2D1D
		public IReadOnlyList<BlobRef> BlobRefs
		{
			get
			{
				return this.m_blobRefs;
			}
		}

		// Token: 0x17001A5C RID: 6748
		// (get) Token: 0x0600D680 RID: 54912 RVA: 0x004D4B25 File Offset: 0x004D2D25
		// (set) Token: 0x0600D681 RID: 54913 RVA: 0x004D4B2D File Offset: 0x004D2D2D
		public byte[] FutureData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = Array.Empty<byte>();

		// Token: 0x17001A5D RID: 6749
		// (get) Token: 0x0600D682 RID: 54914 RVA: 0x004D4B36 File Offset: 0x004D2D36
		public IReadOnlyDictionary<string, Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x0600D683 RID: 54915 RVA: 0x004D4B40 File Offset: 0x004D2D40
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

		// Token: 0x0600D684 RID: 54916 RVA: 0x004D4BC8 File Offset: 0x004D2DC8
		public void SetBlobRefs(BlobRef[] blobRefs)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_blobRefs = blobRefs;
			}
		}

		// Token: 0x0600D685 RID: 54917 RVA: 0x004D4C0C File Offset: 0x004D2E0C
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

		// Token: 0x0600D686 RID: 54918 RVA: 0x004D4CF0 File Offset: 0x004D2EF0
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

		// Token: 0x0600D687 RID: 54919 RVA: 0x004D4E80 File Offset: 0x004D3080
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

		// Token: 0x0600D688 RID: 54920 RVA: 0x004D501C File Offset: 0x004D321C
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

		// Token: 0x0600D689 RID: 54921 RVA: 0x004D50BC File Offset: 0x004D32BC
		public bool IsDirectory()
		{
			return this.Attributes.HasFlag(NodeAttributes.Directory);
		}

		// Token: 0x0600D68A RID: 54922 RVA: 0x004D50D4 File Offset: 0x004D32D4
		public bool IsFile()
		{
			return !this.IsDirectory();
		}

		// Token: 0x0600D68B RID: 54923 RVA: 0x004D50E0 File Offset: 0x004D32E0
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

		// Token: 0x0600D68C RID: 54924 RVA: 0x004D5134 File Offset: 0x004D3334
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory)
		{
			bool flag;
			return this.GetOrCreateChildNode(name, createDirectory, out flag);
		}

		// Token: 0x0600D68D RID: 54925 RVA: 0x004D514C File Offset: 0x004D334C
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

		// Token: 0x0600D68E RID: 54926 RVA: 0x004D520C File Offset: 0x004D340C
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

		// Token: 0x0600D68F RID: 54927 RVA: 0x004D5278 File Offset: 0x004D3478
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

		// Token: 0x0600D690 RID: 54928 RVA: 0x004D5314 File Offset: 0x004D3514
		public void MoveChild(Node child)
		{
			this.MoveChild(child, child.Name);
		}

		// Token: 0x0600D691 RID: 54929 RVA: 0x004D5328 File Offset: 0x004D3528
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

		// Token: 0x0600D693 RID: 54931 RVA: 0x004D53F0 File Offset: 0x004D35F0
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

		// Token: 0x0600D694 RID: 54932 RVA: 0x004D5470 File Offset: 0x004D3670
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

		// Token: 0x0400A306 RID: 41734
		public const ushort VERSION = 6;

		// Token: 0x0400A307 RID: 41735
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A30A RID: 41738
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A30B RID: 41739
		public NodeAttributes Attributes;

		// Token: 0x0400A30C RID: 41740
		[PublicizedFrom(EAccessModifier.Private)]
		public BlobRef[] m_blobRefs;

		// Token: 0x0400A30D RID: 41741
		public DateTime CreationTimeUtc;

		// Token: 0x0400A30F RID: 41743
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringSpanDictionary<Node> m_children;

		// Token: 0x0400A310 RID: 41744
		public readonly object m_blobLock;

		// Token: 0x0400A311 RID: 41745
		public readonly object m_lock;

		// Token: 0x0400A312 RID: 41746
		public Node Parent;
	}
}
