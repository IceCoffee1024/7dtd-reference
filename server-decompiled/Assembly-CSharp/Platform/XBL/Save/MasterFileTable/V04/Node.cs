using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Platform.XBL.Save.MasterFileTable.V04
{
	// Token: 0x02001C6B RID: 7275
	public sealed class Node : IDisposable
	{
		// Token: 0x17001AB7 RID: 6839
		// (get) Token: 0x0600D795 RID: 55189 RVA: 0x004D8D50 File Offset: 0x004D6F50
		// (set) Token: 0x0600D796 RID: 55190 RVA: 0x004D8D58 File Offset: 0x004D6F58
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001AB8 RID: 6840
		// (get) Token: 0x0600D797 RID: 55191 RVA: 0x004D8D61 File Offset: 0x004D6F61
		public IReadOnlyList<BlobRef> BlobRefs
		{
			get
			{
				return this.m_blobRefs;
			}
		}

		// Token: 0x17001AB9 RID: 6841
		// (get) Token: 0x0600D798 RID: 55192 RVA: 0x004D8D69 File Offset: 0x004D6F69
		public IReadOnlyDictionary<string, Node> Children
		{
			get
			{
				return this.m_children;
			}
		}

		// Token: 0x0600D799 RID: 55193 RVA: 0x004D8D74 File Offset: 0x004D6F74
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

		// Token: 0x0600D79A RID: 55194 RVA: 0x004D8DDC File Offset: 0x004D6FDC
		public void SetBlobRefs(BlobRef[] blobRefs)
		{
			object @lock = this.m_lock;
			lock (@lock)
			{
				this.m_blobRefs = blobRefs;
			}
		}

		// Token: 0x0600D79B RID: 55195 RVA: 0x004D8E20 File Offset: 0x004D7020
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

		// Token: 0x0600D79C RID: 55196 RVA: 0x004D8EEC File Offset: 0x004D70EC
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

		// Token: 0x0600D79D RID: 55197 RVA: 0x004D8FE0 File Offset: 0x004D71E0
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

		// Token: 0x0600D79E RID: 55198 RVA: 0x004D90B8 File Offset: 0x004D72B8
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

		// Token: 0x0600D79F RID: 55199 RVA: 0x004D9158 File Offset: 0x004D7358
		public bool IsDirectory()
		{
			return this.Attributes.HasFlag(NodeAttributes.Directory);
		}

		// Token: 0x0600D7A0 RID: 55200 RVA: 0x004D9170 File Offset: 0x004D7370
		public bool IsFile()
		{
			return !this.IsDirectory();
		}

		// Token: 0x0600D7A1 RID: 55201 RVA: 0x004D917C File Offset: 0x004D737C
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

		// Token: 0x0600D7A2 RID: 55202 RVA: 0x004D91D0 File Offset: 0x004D73D0
		public Node GetOrCreateChildNode(StringSpan name, bool createDirectory)
		{
			bool flag;
			return this.GetOrCreateChildNode(name, createDirectory, out flag);
		}

		// Token: 0x0600D7A3 RID: 55203 RVA: 0x004D91E8 File Offset: 0x004D73E8
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

		// Token: 0x0600D7A4 RID: 55204 RVA: 0x004D92A8 File Offset: 0x004D74A8
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

		// Token: 0x0600D7A5 RID: 55205 RVA: 0x004D9314 File Offset: 0x004D7514
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

		// Token: 0x0600D7A6 RID: 55206 RVA: 0x004D93B0 File Offset: 0x004D75B0
		public void MoveChild(Node child)
		{
			this.MoveChild(child, child.Name);
		}

		// Token: 0x0600D7A7 RID: 55207 RVA: 0x004D93C4 File Offset: 0x004D75C4
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

		// Token: 0x0600D7A9 RID: 55209 RVA: 0x004D948C File Offset: 0x004D768C
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

		// Token: 0x0600D7AA RID: 55210 RVA: 0x004D950C File Offset: 0x004D770C
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

		// Token: 0x0400A400 RID: 41984
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly DateTime DefaultLastWriteTime = new DateTime(0L, DateTimeKind.Utc);

		// Token: 0x0400A402 RID: 41986
		public DateTime LastWriteTimeUtc;

		// Token: 0x0400A403 RID: 41987
		public NodeAttributes Attributes;

		// Token: 0x0400A404 RID: 41988
		[PublicizedFrom(EAccessModifier.Private)]
		public BlobRef[] m_blobRefs;

		// Token: 0x0400A405 RID: 41989
		public readonly object m_blobLock;

		// Token: 0x0400A406 RID: 41990
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly StringSpanDictionary<Node> m_children;

		// Token: 0x0400A407 RID: 41991
		public readonly object m_lock;

		// Token: 0x0400A408 RID: 41992
		public Node Parent;
	}
}
