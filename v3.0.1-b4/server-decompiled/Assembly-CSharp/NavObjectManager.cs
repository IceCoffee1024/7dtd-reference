using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000751 RID: 1873
public class NavObjectManager
{
	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x060037A0 RID: 14240 RVA: 0x0016DD7A File Offset: 0x0016BF7A
	public static NavObjectManager Instance
	{
		get
		{
			if (NavObjectManager.instance == null)
			{
				NavObjectManager.instance = new NavObjectManager();
			}
			return NavObjectManager.instance;
		}
	}

	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x060037A1 RID: 14241 RVA: 0x0016DD92 File Offset: 0x0016BF92
	public static bool HasInstance
	{
		get
		{
			return NavObjectManager.instance != null;
		}
	}

	// Token: 0x060037A2 RID: 14242 RVA: 0x0016DD9C File Offset: 0x0016BF9C
	[PublicizedFrom(EAccessModifier.Private)]
	public NavObjectManager()
	{
		NavObjectManager.instance = this;
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x0016DDD6 File Offset: 0x0016BFD6
	public void Cleanup()
	{
		NavObjectManager.instance = null;
		this.NavObjectList.Clear();
		this.removedNavObjectPool.Clear();
		this.tagList.Clear();
	}

	// Token: 0x1400003E RID: 62
	// (add) Token: 0x060037A4 RID: 14244 RVA: 0x0016DE00 File Offset: 0x0016C000
	// (remove) Token: 0x060037A5 RID: 14245 RVA: 0x0016DE38 File Offset: 0x0016C038
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectAdded;

	// Token: 0x1400003F RID: 63
	// (add) Token: 0x060037A6 RID: 14246 RVA: 0x0016DE70 File Offset: 0x0016C070
	// (remove) Token: 0x060037A7 RID: 14247 RVA: 0x0016DEA8 File Offset: 0x0016C0A8
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectRemoved;

	// Token: 0x14000040 RID: 64
	// (add) Token: 0x060037A8 RID: 14248 RVA: 0x0016DEE0 File Offset: 0x0016C0E0
	// (remove) Token: 0x060037A9 RID: 14249 RVA: 0x0016DF18 File Offset: 0x0016C118
	public event NavObjectManager.NavObjectChangedDelegate OnNavObjectRefreshed;

	// Token: 0x060037AA RID: 14250 RVA: 0x0016DF50 File Offset: 0x0016C150
	public NavObject RegisterNavObject(string className, Transform trackedTransform, string overrideSprite = "", bool hiddenOnCompass = false)
	{
		for (int i = 0; i < this.NavObjectList.Count; i++)
		{
			if (this.NavObjectList[i].IsTrackedTransform(trackedTransform))
			{
				return this.NavObjectList[i];
			}
		}
		NavObject navObject;
		if (this.removedNavObjectPool.Count > 0)
		{
			navObject = this.removedNavObjectPool[0];
			this.removedNavObjectPool.RemoveAt(0);
			navObject.IsActive = true;
			navObject.Reset(className);
		}
		else
		{
			navObject = new NavObject(className);
		}
		navObject.hiddenOnCompass = hiddenOnCompass;
		navObject.TrackedTransform = trackedTransform;
		navObject.OverrideSpriteName = overrideSprite;
		this.AddNavObjectTag(navObject);
		if (this.OnNavObjectAdded != null)
		{
			this.OnNavObjectAdded(navObject);
		}
		this.NavObjectList.Add(navObject);
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			navObject.HandleActiveNavClass(primaryPlayer);
		}
		return navObject;
	}

	// Token: 0x060037AB RID: 14251 RVA: 0x0016E034 File Offset: 0x0016C234
	public NavObject RegisterNavObject(string className, Vector3 trackedPosition, string overrideSprite = "", bool hiddenOnCompass = false, int entityId = -1, Entity ownerEntity = null)
	{
		for (int i = 0; i < this.NavObjectList.Count; i++)
		{
			if (this.NavObjectList[i].NavObjectClass != null && !(className != this.NavObjectList[i].NavObjectClass.NavObjectClassName) && (!(ownerEntity != null) || !(ownerEntity != this.NavObjectList[i].OwnerEntity)))
			{
				if (entityId != -1 && this.NavObjectList[i].EntityID == entityId)
				{
					return this.NavObjectList[i];
				}
				if (this.NavObjectList[i].IsTrackedPosition(trackedPosition))
				{
					return this.NavObjectList[i];
				}
			}
		}
		NavObject navObject;
		if (this.removedNavObjectPool.Count > 0)
		{
			navObject = this.removedNavObjectPool[0];
			this.removedNavObjectPool.RemoveAt(0);
			navObject.IsActive = true;
			navObject.Reset(className);
		}
		else
		{
			navObject = new NavObject(className);
		}
		navObject.EntityID = entityId;
		navObject.OwnerEntity = ownerEntity;
		navObject.hiddenOnCompass = hiddenOnCompass;
		navObject.TrackedPosition = trackedPosition;
		navObject.OverrideSpriteName = overrideSprite;
		this.AddNavObjectTag(navObject);
		if (this.OnNavObjectAdded != null)
		{
			this.OnNavObjectAdded(navObject);
		}
		this.NavObjectList.Add(navObject);
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			navObject.HandleActiveNavClass(primaryPlayer);
		}
		return navObject;
	}

	// Token: 0x060037AC RID: 14252 RVA: 0x0016E1AC File Offset: 0x0016C3AC
	public NavObject RegisterNavObject(string className, Entity trackedEntity, string overrideSprite = "", bool hiddenOnCompass = false)
	{
		for (int i = 0; i < this.NavObjectList.Count; i++)
		{
			if (this.NavObjectList[i].IsTrackedEntity(trackedEntity) && (this.NavObjectList[i].NavObjectClass == null || this.NavObjectList[i].NavObjectClass.NavObjectClassName == className))
			{
				this.NavObjectList[i].TrackedEntity = trackedEntity;
				return this.NavObjectList[i];
			}
		}
		NavObject navObject;
		if (this.removedNavObjectPool.Count > 0)
		{
			navObject = this.removedNavObjectPool[0];
			this.removedNavObjectPool.RemoveAt(0);
			navObject.IsActive = true;
			navObject.Reset(className);
		}
		else
		{
			navObject = new NavObject(className);
		}
		navObject.OverrideSpriteName = overrideSprite;
		navObject.TrackedEntity = trackedEntity;
		navObject.hiddenOnCompass = hiddenOnCompass;
		this.AddNavObjectTag(navObject);
		if (this.OnNavObjectAdded != null)
		{
			this.OnNavObjectAdded(navObject);
		}
		this.NavObjectList.Add(navObject);
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (primaryPlayer != null)
		{
			navObject.HandleActiveNavClass(primaryPlayer);
		}
		return navObject;
	}

	// Token: 0x060037AD RID: 14253 RVA: 0x0016E2D4 File Offset: 0x0016C4D4
	public void UnRegisterNavObject(NavObject navObject)
	{
		if (navObject == null)
		{
			return;
		}
		bool flag = false;
		if (this.NavObjectList.Contains(navObject))
		{
			flag = true;
			this.NavObjectList.Remove(navObject);
		}
		if (!this.removedNavObjectPool.Contains(navObject))
		{
			this.removedNavObjectPool.Add(navObject);
		}
		if (flag)
		{
			this.RemoveNavObjectTag(navObject);
			if (this.OnNavObjectRemoved != null)
			{
				this.OnNavObjectRemoved(navObject);
			}
		}
	}

	// Token: 0x060037AE RID: 14254 RVA: 0x0016E33C File Offset: 0x0016C53C
	public void UnRegisterNavObjectByPosition(Vector3 position, string navObjectClass)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.IsTrackedPosition(position) && navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == navObjectClass, string.Format("UnRegisterNavObjectByOwnerEntity {0}", position));
	}

	// Token: 0x060037AF RID: 14255 RVA: 0x0016E384 File Offset: 0x0016C584
	public void UnRegisterNavObjectByOwnerEntity(Entity ownerEntity, string navObjectClass)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.OwnerEntity == ownerEntity && navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == navObjectClass, string.Format("\"UnRegisterNavObjectByOwnerEntity {0}", ownerEntity));
	}

	// Token: 0x060037B0 RID: 14256 RVA: 0x0016E3C8 File Offset: 0x0016C5C8
	public void UnRegisterNavObjectByEntityID(int entityId)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.EntityID == entityId, string.Format("UnRegisterNavObjectByEntityID {0}", entityId));
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x0016E40C File Offset: 0x0016C60C
	public void UnRegisterNavObjectByClass(string className)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName == className, string.Format("UnRegisterNavObjectByClass {0}", className));
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x0016E448 File Offset: 0x0016C648
	public void UnRegisterNavObjectByClass(int classId)
	{
		this.unRegisterNavObjects((NavObject navObject) => navObject.NavObjectClass != null && navObject.NavObjectClass.NavObjectClassName.GetHashCode() == classId, string.Format("UnRegisterNavObjectByClass {0}", EntityClass.GetEntityClassName(classId)));
	}

	// Token: 0x060037B3 RID: 14259 RVA: 0x0016E48C File Offset: 0x0016C68C
	[PublicizedFrom(EAccessModifier.Private)]
	public void unRegisterNavObjects(NavObjectManager.includeInList includeTest, string logStringPrefix)
	{
		foreach (NavObject navObject in this.NavObjectList)
		{
			if (includeTest(navObject))
			{
				this.removeList.Add(navObject);
			}
		}
		foreach (NavObject navObject2 in this.removeList)
		{
			this.NavObjectList.Remove(navObject2);
			this.RemoveNavObjectTag(navObject2);
			if (!this.removedNavObjectPool.Contains(navObject2))
			{
				this.removedNavObjectPool.Add(navObject2);
			}
			NavObjectManager.NavObjectChangedDelegate onNavObjectRemoved = this.OnNavObjectRemoved;
			if (onNavObjectRemoved != null)
			{
				onNavObjectRemoved(navObject2);
			}
		}
		this.removeList.Clear();
	}

	// Token: 0x060037B4 RID: 14260 RVA: 0x0016E574 File Offset: 0x0016C774
	public void AddNavObjectTag(NavObject navObject)
	{
		for (int i = 0; i < navObject.NavObjectClassList.Count; i++)
		{
			if (!string.IsNullOrEmpty(navObject.NavObjectClassList[i].Tag))
			{
				if (!this.tagList.ContainsKey(navObject.NavObjectClassList[i].Tag))
				{
					this.tagList.Add(navObject.NavObjectClassList[i].Tag, new List<NavObject>());
				}
				this.tagList[navObject.NavObjectClassList[i].Tag].Add(navObject);
			}
		}
	}

	// Token: 0x060037B5 RID: 14261 RVA: 0x0016E614 File Offset: 0x0016C814
	public void RemoveNavObjectTag(NavObject navObject)
	{
		for (int i = 0; i < navObject.NavObjectClassList.Count; i++)
		{
			if (!string.IsNullOrEmpty(navObject.NavObjectClassList[i].Tag))
			{
				if (!this.tagList.ContainsKey(navObject.NavObjectClassList[i].Tag))
				{
					this.tagList.Add(navObject.NavObjectClassList[i].Tag, new List<NavObject>());
				}
				this.tagList[navObject.NavObjectClassList[i].Tag].Remove(navObject);
			}
		}
	}

	// Token: 0x060037B6 RID: 14262 RVA: 0x0016E6B4 File Offset: 0x0016C8B4
	public bool HasNavObjectTag(string tag)
	{
		if (this.tagList.ContainsKey(tag))
		{
			for (int i = 0; i < this.tagList[tag].Count; i++)
			{
				if (this.tagList[tag][i].NavObjectClass != null && this.tagList[tag][i].NavObjectClass.Tag == tag)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060037B7 RID: 14263 RVA: 0x0016E72C File Offset: 0x0016C92C
	public NavObject GetNavObjectByEntityID(int entityId)
	{
		for (int i = this.NavObjectList.Count - 1; i >= 0; i--)
		{
			NavObject navObject = this.NavObjectList[i];
			if (navObject != null && navObject.EntityID == entityId)
			{
				return this.NavObjectList[i];
			}
		}
		return null;
	}

	// Token: 0x060037B8 RID: 14264 RVA: 0x0016E77C File Offset: 0x0016C97C
	public void RefreshNavObjects()
	{
		for (int i = 0; i < this.NavObjectList.Count; i++)
		{
			bool flag = false;
			if (this.NavObjectList[i].NavObjectClass != null)
			{
				this.NavObjectList[i].NavObjectClass = NavObjectClass.GetNavObjectClass(this.NavObjectList[i].NavObjectClass.NavObjectClassName);
				flag = true;
			}
			if (this.NavObjectList[i].NavObjectClassList != null)
			{
				for (int j = 0; j < this.NavObjectList[i].NavObjectClassList.Count; j++)
				{
					this.NavObjectList[i].NavObjectClassList[j] = NavObjectClass.GetNavObjectClass(this.NavObjectList[i].NavObjectClassList[j].NavObjectClassName);
					flag = true;
				}
			}
			if (flag && this.OnNavObjectRefreshed != null)
			{
				this.OnNavObjectRefreshed(this.NavObjectList[i]);
			}
		}
	}

	// Token: 0x060037B9 RID: 14265 RVA: 0x0016E87C File Offset: 0x0016CA7C
	public void Update()
	{
		if (GameManager.Instance.World == null)
		{
			return;
		}
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		for (int i = this.NavObjectList.Count - 1; i >= 0; i--)
		{
			NavObject navObject = this.NavObjectList[i];
			if (!navObject.IsValid())
			{
				this.UnRegisterNavObject(navObject);
			}
			else if (primaryPlayer != null)
			{
				navObject.HandleActiveNavClass(primaryPlayer);
			}
		}
	}

	// Token: 0x060037BA RID: 14266 RVA: 0x0016E8EC File Offset: 0x0016CAEC
	[Conditional("DEBUG_NAV")]
	public static void LogNav(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} Nav {1}", GameManager.frameCount, _format);
		Log.Out(_format, _args);
	}

	// Token: 0x04002DB2 RID: 11698
	[PublicizedFrom(EAccessModifier.Private)]
	public static NavObjectManager instance;

	// Token: 0x04002DB6 RID: 11702
	public List<NavObject> NavObjectList = new List<NavObject>();

	// Token: 0x04002DB7 RID: 11703
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NavObject> removedNavObjectPool = new List<NavObject>();

	// Token: 0x04002DB8 RID: 11704
	public Dictionary<string, List<NavObject>> tagList = new Dictionary<string, List<NavObject>>();

	// Token: 0x04002DB9 RID: 11705
	[PublicizedFrom(EAccessModifier.Private)]
	public List<NavObject> removeList = new List<NavObject>();

	// Token: 0x02000752 RID: 1874
	// (Invoke) Token: 0x060037BC RID: 14268
	public delegate void NavObjectChangedDelegate(NavObject newNavObject);

	// Token: 0x02000753 RID: 1875
	// (Invoke) Token: 0x060037C0 RID: 14272
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate bool includeInList(NavObject navObject);
}
