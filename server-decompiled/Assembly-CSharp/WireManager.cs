using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200137B RID: 4987
public class WireManager
{
	// Token: 0x17001299 RID: 4761
	// (get) Token: 0x06009D54 RID: 40276 RVA: 0x003B6B85 File Offset: 0x003B4D85
	public static WireManager Instance
	{
		get
		{
			if (WireManager.instance == null)
			{
				WireManager.instance = new WireManager();
			}
			return WireManager.instance;
		}
	}

	// Token: 0x1700129A RID: 4762
	// (get) Token: 0x06009D55 RID: 40277 RVA: 0x003B6B9D File Offset: 0x003B4D9D
	public static bool HasInstance
	{
		get
		{
			return WireManager.instance != null;
		}
	}

	// Token: 0x1700129B RID: 4763
	// (get) Token: 0x06009D56 RID: 40278 RVA: 0x003B6BA7 File Offset: 0x003B4DA7
	public Transform WireManagerRoot
	{
		get
		{
			return this.wireManagerRoot;
		}
	}

	// Token: 0x06009D57 RID: 40279 RVA: 0x003B6BB0 File Offset: 0x003B4DB0
	public void Init()
	{
		this.activeWires = new HashSet<IWireNode>();
		this.activePulseObjects = new HashSet<GameObject>();
		GameObject gameObject = GameObject.Find("WireManager");
		if (gameObject == null)
		{
			this.wireManagerRoot = new GameObject("WireManager").transform;
		}
		else
		{
			this.wireManagerRoot = gameObject.transform;
		}
		this.wirePool = this.wireManagerRoot.Find("Pool");
		if (this.wirePool == null)
		{
			this.wirePool = new GameObject("Pool").transform;
			this.wirePool.parent = this.wireManagerRoot;
		}
		Origin.Add(this.wireManagerRoot.transform, 0);
		if (this.wirePool.transform.childCount == 0)
		{
			for (int i = 0; i < 200; i++)
			{
				this.addNewNode();
			}
		}
	}

	// Token: 0x06009D58 RID: 40280 RVA: 0x003B6C90 File Offset: 0x003B4E90
	[PublicizedFrom(EAccessModifier.Private)]
	public void addNewNode()
	{
		GameObject gameObject;
		if (WireManager.USE_FAST_WIRE_NODES)
		{
			gameObject = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/WireNode2"));
		}
		else
		{
			gameObject = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/WireNode"));
		}
		global::UnityEngine.Object @object = gameObject;
		string text = "WireNode_{0}";
		int num = this.wireIndex;
		this.wireIndex = num + 1;
		@object.name = string.Format(text, num.ToString());
		gameObject.SetActive(false);
		gameObject.transform.parent = this.wirePool;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x06009D59 RID: 40281 RVA: 0x003B6D30 File Offset: 0x003B4F30
	public void ReturnToPool(IWireNode wireNode)
	{
		this.activeWires.Remove(wireNode);
		GameObject gameObject = wireNode.GetGameObject();
		gameObject.transform.parent = this.wirePool;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.SetActive(false);
		wireNode.Reset();
	}

	// Token: 0x06009D5A RID: 40282 RVA: 0x003B6D90 File Offset: 0x003B4F90
	public IWireNode GetWireNodeFromPool()
	{
		if (this.wirePool.childCount < 1)
		{
			this.addNewNode();
		}
		Transform child = this.wirePool.GetChild(this.wirePool.childCount - 1);
		child.gameObject.SetActive(true);
		child.parent = this.wireManagerRoot;
		IWireNode wireNode;
		if (WireManager.USE_FAST_WIRE_NODES)
		{
			wireNode = child.gameObject.GetComponent<FastWireNode>();
		}
		else
		{
			wireNode = child.gameObject.GetComponent<WireNode>();
		}
		this.activeWires.Add(wireNode);
		return wireNode;
	}

	// Token: 0x06009D5B RID: 40283 RVA: 0x003B6E13 File Offset: 0x003B5013
	public bool AddActiveWire(IWireNode wire)
	{
		return this.activeWires.Add(wire);
	}

	// Token: 0x06009D5C RID: 40284 RVA: 0x003B6E21 File Offset: 0x003B5021
	public bool RemoveActiveWire(IWireNode wire)
	{
		return this.activeWires.Remove(wire);
	}

	// Token: 0x06009D5D RID: 40285 RVA: 0x003B6E2F File Offset: 0x003B502F
	public bool AddPulseObject(GameObject pulseObject)
	{
		return this.activePulseObjects.Add(pulseObject);
	}

	// Token: 0x06009D5E RID: 40286 RVA: 0x003B6E3D File Offset: 0x003B503D
	public bool RemovePulseObject(GameObject pulseObject)
	{
		return this.activePulseObjects.Remove(pulseObject);
	}

	// Token: 0x06009D5F RID: 40287 RVA: 0x003B6E4C File Offset: 0x003B504C
	public void ToggleAllWirePulse(bool isPulseOn)
	{
		World world = GameManager.Instance.World;
		this.ShowPulse = isPulseOn;
		this.WiresShowing = isPulseOn;
		if (this.activeWires == null)
		{
			return;
		}
		if (this.ShowPulse)
		{
			Dictionary<Vector3, bool> dictionary = new Dictionary<Vector3, bool>(Vector3EqualityComparer.Instance);
			foreach (IWireNode wireNode in this.activeWires)
			{
				Vector3 startPosition = wireNode.GetStartPosition();
				bool flag;
				if (dictionary.ContainsKey(startPosition))
				{
					flag = dictionary[startPosition];
				}
				else
				{
					flag = world.CanPlaceBlockAt(new Vector3i(startPosition), world.gameManager.GetPersistentLocalPlayer(), false);
					dictionary[startPosition] = flag;
				}
				if (flag)
				{
					wireNode.TogglePulse(isPulseOn);
					wireNode.SetVisible(this.WiresShowing);
				}
				else
				{
					wireNode.SetVisible(false);
				}
			}
			dictionary.Clear();
			using (HashSet<GameObject>.Enumerator enumerator2 = this.activePulseObjects.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameObject gameObject = enumerator2.Current;
					Vector3i vector3i = new Vector3i(gameObject.transform.position);
					if (world.CanPlaceBlockAt(vector3i, world.gameManager.GetPersistentLocalPlayer(), false))
					{
						gameObject.SetActive(isPulseOn);
					}
					gameObject.layer = 0;
				}
				return;
			}
		}
		foreach (IWireNode wireNode2 in this.activeWires)
		{
			wireNode2.TogglePulse(false);
			wireNode2.SetVisible(false);
		}
		foreach (GameObject gameObject2 in this.activePulseObjects)
		{
			gameObject2.SetActive(isPulseOn);
			gameObject2.layer = 11;
		}
	}

	// Token: 0x06009D60 RID: 40288 RVA: 0x003B7048 File Offset: 0x003B5248
	public void SetWirePulse(IWireNode node)
	{
		node.TogglePulse(this.ShowPulse);
	}

	// Token: 0x06009D61 RID: 40289 RVA: 0x003B7058 File Offset: 0x003B5258
	public void RefreshPulseObjects()
	{
		foreach (GameObject gameObject in this.activePulseObjects)
		{
			gameObject.SetActive(this.ShowPulse);
			gameObject.layer = (this.ShowPulse ? 0 : 11);
		}
	}

	// Token: 0x06009D62 RID: 40290 RVA: 0x003B70C4 File Offset: 0x003B52C4
	public void Cleanup()
	{
		foreach (IWireNode wireNode in this.activeWires)
		{
			global::UnityEngine.Object.Destroy(wireNode.GetGameObject());
		}
		this.activeWires.Clear();
		for (int i = this.wirePool.childCount - 1; i >= 0; i--)
		{
			global::UnityEngine.Object.Destroy(this.wirePool.GetChild(i).gameObject);
		}
		global::UnityEngine.Object.Destroy(this.wirePool.gameObject);
		Origin.Remove(this.wireManagerRoot);
		global::UnityEngine.Object.Destroy(this.wireManagerRoot.gameObject);
		WireManager.instance = null;
	}

	// Token: 0x04007744 RID: 30532
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool USE_FAST_WIRE_NODES = true;

	// Token: 0x04007745 RID: 30533
	[PublicizedFrom(EAccessModifier.Private)]
	public static WireManager instance = null;

	// Token: 0x04007746 RID: 30534
	[PublicizedFrom(EAccessModifier.Private)]
	public int wireIndex;

	// Token: 0x04007747 RID: 30535
	public bool ShowPulse;

	// Token: 0x04007748 RID: 30536
	[PublicizedFrom(EAccessModifier.Private)]
	public Color standardPulseColor = new Color32(0, 97, byte.MaxValue, byte.MaxValue);

	// Token: 0x04007749 RID: 30537
	[PublicizedFrom(EAccessModifier.Private)]
	public Color tripWirePulseColor = Color.magenta;

	// Token: 0x0400774A RID: 30538
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<IWireNode> activeWires;

	// Token: 0x0400774B RID: 30539
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<GameObject> activePulseObjects;

	// Token: 0x0400774C RID: 30540
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform wireManagerRoot;

	// Token: 0x0400774D RID: 30541
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform wirePool;

	// Token: 0x0400774E RID: 30542
	public bool WiresShowing;
}
