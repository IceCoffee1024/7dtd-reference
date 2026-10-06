using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V04
{
	// Token: 0x02001C9D RID: 7325
	public sealed class Node : IDisposable
	{
		// Token: 0x17001B5D RID: 7005
		// (get) Token: 0x0600D90E RID: 55566 RVA: 0x004DD88C File Offset: 0x004DBA8C
		// (set) Token: 0x0600D90F RID: 55567 RVA: 0x004DD894 File Offset: 0x004DBA94
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001B5E RID: 7006
		// (get) Token: 0x0600D910 RID: 55568 RVA: 0x004DD89D File Offset: 0x004DBA9D
		public IReadOnlyList<BlobRef> BlobRefs
		{
			get
			{
				return this.m_blobRefs;
			}
		}

		// Token: 0x17001B5F RID: 7007
		// (get) Token: 0x0600D911 RID: 55569 RVA: 0x004DD8A5 File Offset: 0x004DBAA5
		public IReadOnlyDictionary<string, Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x0600D912 RID: 55570 RVA: 0x004DD8B0 File Offset: 0x004DBAB0
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

		// Token: 0x0600D913 RID: 55571 RVA: 0x004DD918 File Offset: 0x004DBB18
		public void SetBlobRefs(BlobRef[] blobRefs)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_blobRefs = blobRefs;
			}
		}

		// Token: 0x0600D914 RID: 55572 RVA: 0x004DD95C File Offset: 0x004DBB5C
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

		// Token: 0x0600D915 RID: 55573 RVA: 0x004DDA28 File Offset: 0x004DBC28
		public void Write(PooledBinaryWriter writer)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				writer.Write(this.Name);
				writer.Write(this.LastWriteTimeUtc.Ticks);
				writer.Write(Convert.ToUInt32(this.Attributes));
				BlobRef[] blobRefs = this.m_blobRefs;
				ushort num = (ushort)blobRefs.Length;
				writer.Write(num);
				BlobRef[] array = blobRefs;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Write(writer);
				}
				ushort num2 = (ushort)this.Children.Count;
				writer.Write(num2);
				foreach (Node node in this.Children.Values)
				{
					node.Write(writer);
				}
			}
		}

		// Token: 0x0600D916 RID: 55574 RVA: 0x004DDB1C File Offset: 0x004DBD1C
		public void Read(PooledBinaryReader reader)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.Name = reader.ReadString();
				this.LastWriteTimeUtc = new DateTime(reader.ReadInt64(), DateTimeKind.Utc);
				this.Attributes = (NodeAttributes)reader.ReadUInt32();
				ushort num = reader.ReadUInt16();
				BlobRef[] array = new BlobRef[(int)num];
				for (int i = 0; i < (int)num; i++)
				{
					array[i] = BlobRef.Read(reader);
				}
				this.m_blobRefs = array;
				ushort num2 = reader.ReadUInt16();
				for (int j = 0; j < (int)num2; j++)
				{
					Node node = new Node();
					node.Read(reader);
					this.m_children.Add(node.Name, node);
					node.Parent = this;
				}
			}
		}

		// Token: 0x0600D917 RID: 55575 RVA: 0x004DDBF4 File Offset: 0x004DBDF4
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

		// Token: 0x0600D918 RID: 55576 RVA: 0x004DDC94 File Offset: 0x004DBE94
		public bool IsDirectory()
		{
			return this.Attributes.HasFlag(NodeAttributes.Directory);
		}

		// Token: 0x0600D919 RID: 55577 RVA: 0x004DDCAC File Offset: 0x004DBEAC
		public bool IsFile()
		{
			return !this.IsDirectory();
		}

		// Token: 0x0600D91A RID: 55578 RVA: 0x004DDCB8 File Offset: 0x004DBEB8
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

		// Token: 0x0600D91B RID: 55579 RVA: 0x004DDD0C File Offset: 0x004DBF0C
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory)
		{
			bool flag;
			return this.GetOrCreateChildNode(name, createDirectory, out flag);
		}

		// Token: 0x0600D91C RID: 55580 RVA: 0x004DDD24 File Offset: 0x004DBF24
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

		// Token: 0x0600D91D RID: 55581 RVA: 0x004DDDE4 File Offset: 0x004DBFE4
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

		// Token: 0x0600D91E RID: 55582 RVA: 0x004DDE50 File Offset: 0x004DC050
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
				Node.<Enumerate>g__AddNodeChildrenToStack|28_0(stack, this);
			}
			while (stack.Count > 0)
			{
				Node node = stack.Pop();
				list.Add(node);
				Node.<Enumerate>g__AddNodeChildrenToStack|28_0(stack, node);
			}
			return list;
		}

		// Token: 0x0600D91F RID: 55583 RVA: 0x004DDEEC File Offset: 0x004DC0EC
		public void MoveChild(Node child)
		{
			this.MoveChild(child, child.Name);
		}

		// Token: 0x0600D920 RID: 55584 RVA: 0x004DDF00 File Offset: 0x004DC100
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
						Node.<MoveChild>g__MoveChildInternal|30_0(parent, this, child, newName);
					}
					else
					{
						object lock3 = parent.m_lock;
						lock (lock3)
						{
							Node.<MoveChild>g__MoveChildInternal|30_0(parent, this, child, newName);
						}
					}
				}
			}
		}

		// Token: 0x0600D922 RID: 55586 RVA: 0x004DDFC8 File Offset: 0x004DC1C8
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <Enumerate>g__AddNodeChildrenToStack|28_0(Stack<Node> stack, Node currentNode)
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

		// Token: 0x0600D923 RID: 55587 RVA: 0x004DE048 File Offset: 0x004DC248
		[CompilerGenerated]
		[PublicizedFrom(EAccessModifier.Internal)]
		public static void <MoveChild>g__MoveChildInternal|30_0(Node oldParent, Node newParent, Node child, StringSpan newName)
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

		// Token: 0x0400A4D5 RID: 42197
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A4D7 RID: 42199
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A4D8 RID: 42200
		public NodeAttributes Attributes;

		// Token: 0x0400A4D9 RID: 42201
		[PublicizedFrom(EAccessModifier.Private)]
		public BlobRef[] m_blobRefs;

		// Token: 0x0400A4DA RID: 42202
		public readonly object m_blobLock;

		// Token: 0x0400A4DB RID: 42203
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringSpanDictionary<Node> m_children;

		// Token: 0x0400A4DC RID: 42204
		public readonly object m_lock;

		// Token: 0x0400A4DD RID: 42205
		public Node Parent;
	}
}
