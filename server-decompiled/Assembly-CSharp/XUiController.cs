using System;
using System.Collections.Generic;
using Platform;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000ECA RID: 3786
[Preserve]
public class XUiController : IXUiElement
{
	// Token: 0x17000DF1 RID: 3569
	// (get) Token: 0x06007578 RID: 30072 RVA: 0x002C5618 File Offset: 0x002C3818
	// (set) Token: 0x06007579 RID: 30073 RVA: 0x002C5620 File Offset: 0x002C3820
	public XUiView ViewComponent
	{
		get
		{
			return this.viewComponent;
		}
		set
		{
			this.viewComponent = value;
		}
	}

	// Token: 0x17000DF2 RID: 3570
	// (get) Token: 0x0600757A RID: 30074 RVA: 0x002C5629 File Offset: 0x002C3829
	// (set) Token: 0x0600757B RID: 30075 RVA: 0x002C5634 File Offset: 0x002C3834
	public XUiController Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			if (this.parent == value)
			{
				return;
			}
			XUiController xuiController = this.parent;
			if (xuiController != null)
			{
				xuiController.children.Remove(this);
			}
			this.parent = value;
			XUiController xuiController2 = this.parent;
			if (xuiController2 == null)
			{
				return;
			}
			xuiController2.children.Add(this);
		}
	}

	// Token: 0x17000DF3 RID: 3571
	// (get) Token: 0x0600757C RID: 30076 RVA: 0x002C5680 File Offset: 0x002C3880
	public List<XUiController> Children
	{
		get
		{
			return this.children;
		}
	}

	// Token: 0x17000DF4 RID: 3572
	// (get) Token: 0x0600757D RID: 30077 RVA: 0x002C5688 File Offset: 0x002C3888
	// (set) Token: 0x0600757E RID: 30078 RVA: 0x002C5690 File Offset: 0x002C3890
	public XUiWindowGroup WindowGroup
	{
		get
		{
			return this.windowGroup;
		}
		set
		{
			this.windowGroup = value;
		}
	}

	// Token: 0x17000DF5 RID: 3573
	// (get) Token: 0x0600757F RID: 30079 RVA: 0x002C5699 File Offset: 0x002C3899
	public bool IsOpen
	{
		get
		{
			return this.windowGroup.isShowing;
		}
	}

	// Token: 0x06007580 RID: 30080 RVA: 0x002C56A6 File Offset: 0x002C38A6
	public void AutoBindComponents()
	{
		AutoBindCache.Instance.BindComponents(this);
	}

	// Token: 0x06007581 RID: 30081 RVA: 0x002C56B3 File Offset: 0x002C38B3
	public void AutoBindEvents()
	{
		AutoBindCache.Instance.BindEvents(this);
	}

	// Token: 0x06007582 RID: 30082 RVA: 0x002C56C0 File Offset: 0x002C38C0
	public virtual void Init()
	{
		if (this.viewComponent != null)
		{
			this.viewComponent.InitView();
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].AutoBindComponents();
			this.children[i].Init();
			this.children[i].AutoBindEvents();
		}
		this.curInputStyle = PlatformManager.NativePlatform.Input.CurrentInputStyle;
	}

	// Token: 0x06007583 RID: 30083 RVA: 0x002C5740 File Offset: 0x002C3940
	public virtual void Update(float _dt)
	{
		XUiView xuiView = this.viewComponent;
		bool isShowing = this.windowGroup.isShowing;
		if (this.forceInputStyleChange || this.curInputStyle != this.lastInputStyle)
		{
			this.forceInputStyleChange = false;
			PlayerInputManager.InputStyle inputStyle = this.lastInputStyle;
			this.lastInputStyle = this.curInputStyle;
			this.RefreshBindings();
			this.inputStyleChanged(inputStyle, this.lastInputStyle);
		}
		if (isShowing && xuiView != null && xuiView.IsVisible)
		{
			using (this.pmVcUpdate.Auto())
			{
				using (this.pmVcNamedUpdate.Auto())
				{
					xuiView.Update(_dt);
				}
			}
		}
		List<XUiController> list = this.children;
		for (int i = 0; i < list.Count; i++)
		{
			XUiController xuiController = list[i];
			if (isShowing || xuiController.AlwaysUpdate)
			{
				xuiController.Update(_dt);
			}
		}
	}

	// Token: 0x06007584 RID: 30084 RVA: 0x002C5848 File Offset: 0x002C3A48
	public bool TryGetChildController(Type _controllerType, ReadOnlyMemory<char> _id, out XUiController _child)
	{
		if ((_id.IsEmpty || (this.viewComponent != null && _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase))) && _controllerType.IsInstanceOfType(this))
		{
			_child = this;
			return true;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i].TryGetChildController(_controllerType, _id, out _child))
			{
				return true;
			}
		}
		_child = null;
		return false;
	}

	// Token: 0x06007585 RID: 30085 RVA: 0x002C58C8 File Offset: 0x002C3AC8
	public bool TryGetChildController<T>(ReadOnlyMemory<char> _id, out T _child) where T : XUiController
	{
		if (_id.IsEmpty || (this.viewComponent != null && _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase)))
		{
			T t = this as T;
			if (t != null)
			{
				_child = t;
				return true;
			}
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i].TryGetChildController<T>(_id, out _child))
			{
				return true;
			}
		}
		_child = default(T);
		return false;
	}

	// Token: 0x06007586 RID: 30086 RVA: 0x002C5957 File Offset: 0x002C3B57
	public bool TryGetChildController<T>(string _id, out T _child) where T : XUiController
	{
		return this.TryGetChildController<T>(_id.AsMemory(), out _child);
	}

	// Token: 0x06007587 RID: 30087 RVA: 0x002C5968 File Offset: 0x002C3B68
	public XUiController[] GetChildControllers(Type _controllerType, ReadOnlyMemory<char> _id, List<XUiController> _list = null)
	{
		List<XUiController> list = _list ?? new List<XUiController>();
		if ((_id.IsEmpty || (this.viewComponent != null && _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase))) && _controllerType.IsInstanceOfType(this))
		{
			list.Add(this);
		}
		else
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].GetChildControllers(_controllerType, _id, list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x06007588 RID: 30088 RVA: 0x002C59F8 File Offset: 0x002C3BF8
	public T[] GetChildControllers<T>(ReadOnlyMemory<char> _id, List<T> _list = null) where T : XUiController
	{
		List<T> list = _list ?? new List<T>();
		if (_id.IsEmpty || (this.viewComponent != null && _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase)))
		{
			T t = this as T;
			if (t != null)
			{
				list.Add(t);
				goto IL_0082;
			}
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].GetChildControllers<T>(_id, list);
		}
		IL_0082:
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x06007589 RID: 30089 RVA: 0x002C5A92 File Offset: 0x002C3C92
	public T[] GetChildControllers<T>(string _id, List<T> _list = null) where T : XUiController
	{
		return this.GetChildControllers<T>(_id.AsMemory(), _list);
	}

	// Token: 0x0600758A RID: 30090 RVA: 0x002C5AA4 File Offset: 0x002C3CA4
	public bool TryGetChildView(Type _viewType, ReadOnlyMemory<char> _id, out XUiView _child)
	{
		if (this.viewComponent != null && (_id.IsEmpty || _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase)) && _viewType.IsInstanceOfType(this.viewComponent))
		{
			_child = this.viewComponent;
			return true;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i].TryGetChildView(_viewType, _id, out _child))
			{
				return true;
			}
		}
		_child = null;
		return false;
	}

	// Token: 0x0600758B RID: 30091 RVA: 0x002C5B2C File Offset: 0x002C3D2C
	public bool TryGetChildView<T>(ReadOnlyMemory<char> _id, out T _child) where T : XUiView
	{
		T t = this.viewComponent as T;
		if (t != null && (_id.IsEmpty || _id.Span.Equals(t.ID, StringComparison.OrdinalIgnoreCase)))
		{
			_child = t;
			return true;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i].TryGetChildView<T>(_id, out _child))
			{
				return true;
			}
		}
		_child = default(T);
		return false;
	}

	// Token: 0x0600758C RID: 30092 RVA: 0x002C5BB8 File Offset: 0x002C3DB8
	public bool TryGetChildView<T>(string _id, out T _child) where T : XUiView
	{
		return this.TryGetChildView<T>(_id.AsMemory(), out _child);
	}

	// Token: 0x0600758D RID: 30093 RVA: 0x002C5BC8 File Offset: 0x002C3DC8
	public XUiView[] GetChildViews(Type _viewType, ReadOnlyMemory<char> _id, List<XUiView> _list = null)
	{
		List<XUiView> list = _list ?? new List<XUiView>();
		if (this.viewComponent != null && (_id.IsEmpty || _id.Span.Equals(this.viewComponent.ID, StringComparison.OrdinalIgnoreCase)) && _viewType.IsInstanceOfType(this.viewComponent))
		{
			list.Add(this.viewComponent);
		}
		else
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].GetChildViews(_viewType, _id, list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x0600758E RID: 30094 RVA: 0x002C5C64 File Offset: 0x002C3E64
	public T[] GetChildViews<T>(ReadOnlyMemory<char> _id, List<T> _list = null) where T : XUiView
	{
		List<T> list = _list ?? new List<T>();
		T t = this.viewComponent as T;
		if (t != null && (_id.IsEmpty || _id.Span.Equals(t.ID, StringComparison.OrdinalIgnoreCase)))
		{
			list.Add(t);
		}
		else
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].GetChildViews<T>(_id, list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x0600758F RID: 30095 RVA: 0x002C5CFB File Offset: 0x002C3EFB
	public T[] GetChildViews<T>(string _id, List<T> _list = null) where T : XUiView
	{
		return this.GetChildViews<T>(_id.AsMemory(), _list);
	}

	// Token: 0x06007590 RID: 30096 RVA: 0x002C5D0C File Offset: 0x002C3F0C
	public bool TryGetChildByIdAndType<T>(string _id, out T _child) where T : XUiController
	{
		if (this.viewComponent != null && string.Equals(this.viewComponent.ID, _id, StringComparison.OrdinalIgnoreCase))
		{
			T t = this as T;
			if (t != null)
			{
				_child = t;
				return true;
			}
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			if (this.children[i].TryGetChildByIdAndType<T>(_id, out _child))
			{
				return true;
			}
		}
		_child = default(T);
		return false;
	}

	// Token: 0x06007591 RID: 30097 RVA: 0x002C5D88 File Offset: 0x002C3F88
	public XUiController GetChildById(string _id)
	{
		if (this.viewComponent != null && string.Equals(this.viewComponent.ID, _id, StringComparison.OrdinalIgnoreCase))
		{
			return this;
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			XUiController childById = this.children[i].GetChildById(_id);
			if (childById != null)
			{
				return childById;
			}
		}
		return null;
	}

	// Token: 0x06007592 RID: 30098 RVA: 0x002C5DE4 File Offset: 0x002C3FE4
	public XUiController[] GetChildrenById(string _id, List<XUiController> _list = null)
	{
		List<XUiController> list = _list ?? new List<XUiController>();
		if (this.viewComponent != null && string.Equals(this.viewComponent.ID, _id, StringComparison.OrdinalIgnoreCase))
		{
			list.Add(this);
		}
		else
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].GetChildrenById(_id, list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x06007593 RID: 30099 RVA: 0x002C5E58 File Offset: 0x002C4058
	public T GetChildByType<T>() where T : XUiController
	{
		T t = this as T;
		if (t != null)
		{
			return t;
		}
		foreach (XUiController xuiController in this.children)
		{
			t = xuiController.GetChildByType<T>();
			if (t != null)
			{
				return t;
			}
		}
		return default(T);
	}

	// Token: 0x06007594 RID: 30100 RVA: 0x002C5ED8 File Offset: 0x002C40D8
	public T[] GetChildrenByType<T>(List<T> _list = null) where T : XUiController
	{
		List<T> list = _list ?? new List<T>();
		T t = this as T;
		if (t != null)
		{
			list.Add(t);
		}
		else
		{
			foreach (XUiController xuiController in this.children)
			{
				xuiController.GetChildrenByType<T>(list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x06007595 RID: 30101 RVA: 0x002C5F60 File Offset: 0x002C4160
	public T[] GetChildrenByViewType<T>(List<T> _list = null) where T : XUiView
	{
		List<T> list = _list ?? new List<T>();
		T t = this.viewComponent as T;
		if (t != null)
		{
			list.Add(t);
		}
		else
		{
			foreach (XUiController xuiController in this.children)
			{
				xuiController.GetChildrenByViewType<T>(list);
			}
		}
		if (_list != null)
		{
			return null;
		}
		return list.ToArray();
	}

	// Token: 0x06007596 RID: 30102 RVA: 0x002C5FEC File Offset: 0x002C41EC
	public T GetParentByType<T>() where T : XUiController
	{
		T t = this as T;
		if (t != null)
		{
			return t;
		}
		XUiController xuiController = this.Parent;
		if (xuiController == null)
		{
			return default(T);
		}
		return xuiController.GetParentByType<T>();
	}

	// Token: 0x06007597 RID: 30103 RVA: 0x002C6028 File Offset: 0x002C4228
	public bool TryGetParentController(Type _controllerType, out XUiController _parent)
	{
		if (_controllerType.IsInstanceOfType(this))
		{
			_parent = this;
			return true;
		}
		if (this.Parent != null)
		{
			return this.Parent.TryGetParentController(_controllerType, out _parent);
		}
		_parent = null;
		return false;
	}

	// Token: 0x06007598 RID: 30104 RVA: 0x002C6054 File Offset: 0x002C4254
	public bool TryGetParentController<T>(out T _parent) where T : XUiController
	{
		T t = this as T;
		if (t != null)
		{
			_parent = t;
			return true;
		}
		if (this.Parent != null)
		{
			return this.Parent.TryGetParentController<T>(out _parent);
		}
		_parent = default(T);
		return false;
	}

	// Token: 0x06007599 RID: 30105 RVA: 0x002C609B File Offset: 0x002C429B
	public bool TryGetParentView(Type _viewType, out XUiView _parent)
	{
		if (_viewType.IsInstanceOfType(this.viewComponent))
		{
			_parent = this.viewComponent;
			return true;
		}
		if (this.Parent != null)
		{
			return this.Parent.TryGetParentView(_viewType, out _parent);
		}
		_parent = null;
		return false;
	}

	// Token: 0x0600759A RID: 30106 RVA: 0x002C60D0 File Offset: 0x002C42D0
	public bool TryGetParentView<T>(out T _parent) where T : XUiView
	{
		T t = this.viewComponent as T;
		if (t != null)
		{
			_parent = t;
			return true;
		}
		if (this.Parent != null)
		{
			return this.Parent.TryGetParentView<T>(out _parent);
		}
		_parent = default(T);
		return false;
	}

	// Token: 0x0600759B RID: 30107 RVA: 0x002C611C File Offset: 0x002C431C
	public bool IsChildOf(XUiController _controller)
	{
		return this.Parent != null && (this.Parent == _controller || this.Parent.IsChildOf(_controller));
	}

	// Token: 0x0600759C RID: 30108 RVA: 0x002C613F File Offset: 0x002C433F
	public bool IsSelfOrChildOf(XUiController _controller)
	{
		return this == _controller || this.IsChildOf(_controller);
	}

	// Token: 0x0600759D RID: 30109 RVA: 0x002C6150 File Offset: 0x002C4350
	public XUiV_Window GetParentWindow()
	{
		XUiV_Window xuiV_Window = this.ViewComponent as XUiV_Window;
		if (xuiV_Window != null)
		{
			return xuiV_Window;
		}
		XUiController xuiController = this.Parent;
		if (xuiController == null)
		{
			return null;
		}
		return xuiController.GetParentWindow();
	}

	// Token: 0x140000BF RID: 191
	// (add) Token: 0x0600759E RID: 30110 RVA: 0x002C6180 File Offset: 0x002C4380
	// (remove) Token: 0x0600759F RID: 30111 RVA: 0x002C61B8 File Offset: 0x002C43B8
	public event XUiEvent_OnMouseUpDownEventHandler OnMouseUpDown;

	// Token: 0x140000C0 RID: 192
	// (add) Token: 0x060075A0 RID: 30112 RVA: 0x002C61F0 File Offset: 0x002C43F0
	// (remove) Token: 0x060075A1 RID: 30113 RVA: 0x002C6228 File Offset: 0x002C4428
	public event XUiEvent_OnPressEventHandler OnPress;

	// Token: 0x140000C1 RID: 193
	// (add) Token: 0x060075A2 RID: 30114 RVA: 0x002C6260 File Offset: 0x002C4460
	// (remove) Token: 0x060075A3 RID: 30115 RVA: 0x002C6298 File Offset: 0x002C4498
	public event XUiEvent_OnPressEventHandler OnDoubleClick;

	// Token: 0x140000C2 RID: 194
	// (add) Token: 0x060075A4 RID: 30116 RVA: 0x002C62D0 File Offset: 0x002C44D0
	// (remove) Token: 0x060075A5 RID: 30117 RVA: 0x002C6308 File Offset: 0x002C4508
	public event XUiEvent_OnPressEventHandler OnRightPress;

	// Token: 0x140000C3 RID: 195
	// (add) Token: 0x060075A6 RID: 30118 RVA: 0x002C6340 File Offset: 0x002C4540
	// (remove) Token: 0x060075A7 RID: 30119 RVA: 0x002C6378 File Offset: 0x002C4578
	public event XUiEvent_OnHoverEventHandler OnHover;

	// Token: 0x140000C4 RID: 196
	// (add) Token: 0x060075A8 RID: 30120 RVA: 0x002C63B0 File Offset: 0x002C45B0
	// (remove) Token: 0x060075A9 RID: 30121 RVA: 0x002C63E8 File Offset: 0x002C45E8
	public event XUiEvent_OnDragEventHandler OnDrag;

	// Token: 0x140000C5 RID: 197
	// (add) Token: 0x060075AA RID: 30122 RVA: 0x002C6420 File Offset: 0x002C4620
	// (remove) Token: 0x060075AB RID: 30123 RVA: 0x002C6458 File Offset: 0x002C4658
	public event XUiEvent_OnHeldHandler OnHold;

	// Token: 0x140000C6 RID: 198
	// (add) Token: 0x060075AC RID: 30124 RVA: 0x002C6490 File Offset: 0x002C4690
	// (remove) Token: 0x060075AD RID: 30125 RVA: 0x002C64C8 File Offset: 0x002C46C8
	public event XUiEvent_OnScrollEventHandler OnScroll;

	// Token: 0x140000C7 RID: 199
	// (add) Token: 0x060075AE RID: 30126 RVA: 0x002C6500 File Offset: 0x002C4700
	// (remove) Token: 0x060075AF RID: 30127 RVA: 0x002C6538 File Offset: 0x002C4738
	public event XUiEvent_OnSelectEventHandler OnSelect;

	// Token: 0x140000C8 RID: 200
	// (add) Token: 0x060075B0 RID: 30128 RVA: 0x002C6570 File Offset: 0x002C4770
	// (remove) Token: 0x060075B1 RID: 30129 RVA: 0x002C65A8 File Offset: 0x002C47A8
	public event XUiEvent_OnVisibilityChanged OnVisiblity;

	// Token: 0x140000C9 RID: 201
	// (add) Token: 0x060075B2 RID: 30130 RVA: 0x002C65E0 File Offset: 0x002C47E0
	// (remove) Token: 0x060075B3 RID: 30131 RVA: 0x002C6618 File Offset: 0x002C4818
	public event XUiEvent_OnEnabledChanged OnEnabled;

	// Token: 0x140000CA RID: 202
	// (add) Token: 0x060075B4 RID: 30132 RVA: 0x002C6650 File Offset: 0x002C4850
	// (remove) Token: 0x060075B5 RID: 30133 RVA: 0x002C6688 File Offset: 0x002C4888
	public event XUiEvent_OnInteraction OnInteraction;

	// Token: 0x060075B6 RID: 30134 RVA: 0x002C66BD File Offset: 0x002C48BD
	public void MouseUpDown(bool _pressed)
	{
		XUiEvent_OnMouseUpDownEventHandler onMouseUpDown = this.OnMouseUpDown;
		if (onMouseUpDown != null)
		{
			onMouseUpDown(this, _pressed);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.MouseUpDown);
	}

	// Token: 0x060075B7 RID: 30135 RVA: 0x002C66E8 File Offset: 0x002C48E8
	public virtual void Pressed(int _mouseButton)
	{
		if (_mouseButton != -1)
		{
			if (_mouseButton == -2)
			{
				XUiEvent_OnPressEventHandler onRightPress = this.OnRightPress;
				if (onRightPress != null)
				{
					onRightPress(this, _mouseButton);
				}
				XUiEvent_OnInteraction onInteraction = this.OnInteraction;
				if (onInteraction == null)
				{
					return;
				}
				onInteraction(this, EXUiControllerInteractionType.RightClick);
			}
			return;
		}
		XUiEvent_OnPressEventHandler onPress = this.OnPress;
		if (onPress != null)
		{
			onPress(this, _mouseButton);
		}
		XUiEvent_OnInteraction onInteraction2 = this.OnInteraction;
		if (onInteraction2 == null)
		{
			return;
		}
		onInteraction2(this, EXUiControllerInteractionType.LeftClick);
	}

	// Token: 0x060075B8 RID: 30136 RVA: 0x002C6749 File Offset: 0x002C4949
	public void DoubleClicked(int _mouseButton)
	{
		XUiEvent_OnPressEventHandler onDoubleClick = this.OnDoubleClick;
		if (onDoubleClick != null)
		{
			onDoubleClick(this, _mouseButton);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.DoubleClick);
	}

	// Token: 0x060075B9 RID: 30137 RVA: 0x002C6770 File Offset: 0x002C4970
	public void Hovered(bool _isOver)
	{
		XUiEvent_OnHoverEventHandler onHover = this.OnHover;
		if (onHover != null)
		{
			onHover(this, _isOver);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction != null)
		{
			onInteraction(this, EXUiControllerInteractionType.Hover);
		}
		this.OnHovered(_isOver);
	}

	// Token: 0x060075BA RID: 30138 RVA: 0x002C679F File Offset: 0x002C499F
	public void Scrolled(float _delta)
	{
		XUiEvent_OnScrollEventHandler onScroll = this.OnScroll;
		if (onScroll != null)
		{
			onScroll(this, _delta);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.Scroll);
	}

	// Token: 0x060075BB RID: 30139 RVA: 0x002C67C6 File Offset: 0x002C49C6
	public void Selected(bool _selected)
	{
		XUiEvent_OnSelectEventHandler onSelect = this.OnSelect;
		if (onSelect != null)
		{
			onSelect(this, _selected);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.Select);
	}

	// Token: 0x060075BC RID: 30140 RVA: 0x002C67F0 File Offset: 0x002C49F0
	public void Dragged(Vector2 _mouseDelta, EDragType _dragType)
	{
		if (this.viewComponent.ID == "btn2")
		{
			Log.Warning(string.Format("[XUi] F={0} ButtonC received Dragged={1} event", Time.frameCount, _dragType));
		}
		XUiEvent_OnDragEventHandler onDrag = this.OnDrag;
		if (onDrag != null)
		{
			onDrag(this, _dragType, _mouseDelta);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.Drag);
	}

	// Token: 0x060075BD RID: 30141 RVA: 0x002C6859 File Offset: 0x002C4A59
	public void Held(EHoldType _event, float _holdDuration, float _deltaSinceLastTimedEvent = -1f)
	{
		XUiEvent_OnHeldHandler onHold = this.OnHold;
		if (onHold != null)
		{
			onHold(this, _event, _holdDuration, _deltaSinceLastTimedEvent);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.Held);
	}

	// Token: 0x060075BE RID: 30142 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnHovered(bool _isOver)
	{
	}

	// Token: 0x060075BF RID: 30143 RVA: 0x002C6884 File Offset: 0x002C4A84
	public virtual void OnOpen()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnOpen();
		}
		XUiView xuiView = this.ViewComponent;
		if (xuiView != null)
		{
			xuiView.OnOpen();
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction != null)
		{
			onInteraction(this, EXUiControllerInteractionType.Open);
		}
		this.RefreshBindings();
	}

	// Token: 0x060075C0 RID: 30144 RVA: 0x002C68E4 File Offset: 0x002C4AE4
	public virtual void OnClose()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnClose();
		}
		XUiView xuiView = this.ViewComponent;
		if (xuiView != null)
		{
			xuiView.OnClose();
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.Close);
	}

	// Token: 0x060075C1 RID: 30145 RVA: 0x002C693C File Offset: 0x002C4B3C
	public void OnVisibilityChanged(bool _parentVisibleInScene)
	{
		bool isVisible = this.viewComponent.IsVisible;
		bool flag = isVisible && _parentVisibleInScene;
		XUiEvent_OnVisibilityChanged onVisiblity = this.OnVisiblity;
		if (onVisiblity != null)
		{
			onVisiblity(this, isVisible, flag);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction != null)
		{
			onInteraction(this, EXUiControllerInteractionType.VisibilityChanged);
		}
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].OnVisibilityChanged(flag);
			this.children[i].viewComponent.OnVisibilityChanged(flag);
		}
	}

	// Token: 0x060075C2 RID: 30146 RVA: 0x002C69BF File Offset: 0x002C4BBF
	public void OnEnabledChanged(bool _enabled)
	{
		XUiEvent_OnEnabledChanged onEnabled = this.OnEnabled;
		if (onEnabled != null)
		{
			onEnabled(this, _enabled);
		}
		XUiEvent_OnInteraction onInteraction = this.OnInteraction;
		if (onInteraction == null)
		{
			return;
		}
		onInteraction(this, EXUiControllerInteractionType.EnabledChanged);
	}

	// Token: 0x060075C3 RID: 30147 RVA: 0x002C69E8 File Offset: 0x002C4BE8
	public virtual void OnCursorSelected(bool _isActualElement)
	{
		XUiV_ScrollView xuiV_ScrollView;
		if (_isActualElement && this.TryGetParentView<XUiV_ScrollView>(out xuiV_ScrollView))
		{
			xuiV_ScrollView.MakeVisible(this.ViewComponent);
		}
		XUiController xuiController = this.parent;
		if (xuiController == null)
		{
			return;
		}
		xuiController.OnCursorSelected(false);
	}

	// Token: 0x060075C4 RID: 30148 RVA: 0x002C6A1F File Offset: 0x002C4C1F
	public virtual void OnCursorUnSelected()
	{
		XUiController xuiController = this.parent;
		if (xuiController == null)
		{
			return;
		}
		xuiController.OnCursorUnSelected();
	}

	// Token: 0x060075C5 RID: 30149 RVA: 0x002C6A31 File Offset: 0x002C4C31
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool handleDirtyUpdateDefault()
	{
		if (!this.IsDirty)
		{
			return false;
		}
		this.IsDirty = false;
		this.RefreshBindings();
		return true;
	}

	// Token: 0x060075C6 RID: 30150 RVA: 0x002C6A4B File Offset: 0x002C4C4B
	public virtual bool ParseAttribute(string _name, string _value)
	{
		if (_name == "always_update")
		{
			StringParsers.TryParseBool(_value, out this.AlwaysUpdate);
			return true;
		}
		return false;
	}

	// Token: 0x060075C7 RID: 30151 RVA: 0x002C6A6C File Offset: 0x002C4C6C
	public bool GetBindingValue(ref string _value, string _bindingName)
	{
		bool bindingValueInternal;
		try
		{
			bindingValueInternal = this.GetBindingValueInternal(ref _value, _bindingName);
		}
		catch
		{
			Log.Error("Unhandled exception in GetBindingValue. Binding name: " + _bindingName + ", hierarchy: " + this.GetXuiHierarchy());
			throw;
		}
		return bindingValueInternal;
	}

	// Token: 0x17000DF6 RID: 3574
	// (get) Token: 0x060075C8 RID: 30152 RVA: 0x002C6AB4 File Offset: 0x002C4CB4
	[XuiXmlBinding("is_online")]
	public bool IsOnline
	{
		get
		{
			return PlatformManager.MultiPlatform.User.UserStatus == EUserStatus.LoggedIn;
		}
	}

	// Token: 0x17000DF7 RID: 3575
	// (get) Token: 0x060075C9 RID: 30153 RVA: 0x002C6AC8 File Offset: 0x002C4CC8
	[XuiXmlBinding("system_time")]
	public DateTime SystemTime
	{
		get
		{
			return DateTime.Now;
		}
	}

	// Token: 0x17000DF8 RID: 3576
	// (get) Token: 0x060075CA RID: 30154 RVA: 0x002C6ACF File Offset: 0x002C4CCF
	[XuiXmlBinding("gamelanguage")]
	public string Language
	{
		get
		{
			return Localization.ActiveLanguage;
		}
	}

	// Token: 0x060075CB RID: 30155 RVA: 0x002C6AD8 File Offset: 0x002C4CD8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool GetBindingValueInternal(ref string _value, string _bindingName)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(_bindingName);
		if (num <= 2577987401U)
		{
			if (num <= 820250566U)
			{
				if (num <= 364418046U)
				{
					if (num != 23126121U)
					{
						if (num == 364418046U)
						{
							if (_bindingName == "is_map_enabled")
							{
								_value = World.MapEnabled.ToString();
								return true;
							}
						}
					}
					else if (_bindingName == "is_challenge_enabled")
					{
						_value = (ChallengeJournal.AllowChallenges || World.BiomeProgressionEnabled).ToString();
						return true;
					}
				}
				else if (num != 470361894U)
				{
					if (num == 820250566U)
					{
						if (_bindingName == "is_unityeditor")
						{
							_value = "false";
							return true;
						}
					}
				}
				else if (_bindingName == "version_long")
				{
					_value = Constants.cVersionInformation.LongString;
					return true;
				}
			}
			else if (num <= 1573359332U)
			{
				if (num != 1383400110U)
				{
					if (num == 1573359332U)
					{
						if (_bindingName == "is_playtesting")
						{
							_value = GameUtils.IsPlaytesting().ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "is_world_loaded")
				{
					_value = (GameManager.Instance.World != null).ToString();
					return true;
				}
			}
			else if (num != 1899437583U)
			{
				if (num == 2577987401U)
				{
					if (_bindingName == "is_editmode")
					{
						_value = GameManager.Instance.IsEditMode().ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "is_modal")
			{
				_value = this.WindowGroup.isModal.ToString();
				return true;
			}
		}
		else if (num <= 2934648263U)
		{
			if (num <= 2759425756U)
			{
				if (num != 2745931225U)
				{
					if (num == 2759425756U)
					{
						if (_bindingName == "is_prefab_editor")
						{
							_value = PrefabEditModeManager.Instance.IsActive().ToString();
							return true;
						}
					}
				}
				else if (_bindingName == "is_server")
				{
					_value = SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer.ToString();
					return true;
				}
			}
			else if (num != 2799769034U)
			{
				if (num == 2934648263U)
				{
					if (_bindingName == "is_console")
					{
						_value = (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent().ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "is_ps5")
			{
				_value = DeviceFlag.PS5.IsCurrent().ToString();
				return true;
			}
		}
		else if (num <= 3445481009U)
		{
			if (num != 2963904327U)
			{
				if (num == 3445481009U)
				{
					if (_bindingName == "is_controller_input")
					{
						this.registerForInputStyleChanges();
						_value = (this.lastInputStyle != PlayerInputManager.InputStyle.Keyboard).ToString();
						return true;
					}
				}
			}
			else if (_bindingName == "is_release")
			{
				_value = "true";
				return true;
			}
		}
		else if (num != 3556752319U)
		{
			if (num == 4180283068U)
			{
				if (_bindingName == "inputstyle")
				{
					this.registerForInputStyleChanges();
					_value = this.lastInputStyle.ToStringCached<PlayerInputManager.InputStyle>();
					return true;
				}
			}
		}
		else if (_bindingName == "is_creative")
		{
			_value = (GameStats.GetBool(EnumGameStats.IsCreativeMenuEnabled) || GamePrefs.GetBool(EnumGamePrefs.CreativeMenuEnabled)).ToString();
			return true;
		}
		return false;
	}

	// Token: 0x060075CC RID: 30156 RVA: 0x002C6E8C File Offset: 0x002C508C
	public void RefreshBindingsSelfAndChildren()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].RefreshBindingsSelfAndChildren();
		}
		this.Bindings.RefreshBindings();
	}

	// Token: 0x060075CD RID: 30157 RVA: 0x002C6ECB File Offset: 0x002C50CB
	public void RefreshBindings()
	{
		this.Bindings.RefreshBindings();
	}

	// Token: 0x060075CE RID: 30158 RVA: 0x002C6ED8 File Offset: 0x002C50D8
	public virtual void Cleanup()
	{
		foreach (XUiController xuiController in this.children)
		{
			xuiController.Cleanup();
		}
		if (this.registeredForInputStyleChanges)
		{
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			if (((nativePlatform != null) ? nativePlatform.Input : null) != null)
			{
				PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
			}
		}
		XUiView xuiView = this.ViewComponent;
		if (xuiView == null)
		{
			return;
		}
		xuiView.Cleanup();
	}

	// Token: 0x060075CF RID: 30159 RVA: 0x002C6F70 File Offset: 0x002C5170
	public void FindNavigatableChildren(List<XUiView> _views)
	{
		foreach (XUiController xuiController in this.children)
		{
			if (xuiController.viewComponent.IsNavigatable)
			{
				_views.Add(xuiController.viewComponent);
			}
			xuiController.FindNavigatableChildren(_views);
		}
	}

	// Token: 0x060075D0 RID: 30160 RVA: 0x002C6FDC File Offset: 0x002C51DC
	public bool TryFindFirstNavigableView(out XUiView _foundView)
	{
		_foundView = null;
		bool isActiveInHierarchy = this.ViewComponent.IsActiveInHierarchy;
		if (this.ViewComponent.IsNavigatable && this.ViewComponent.IsVisible && isActiveInHierarchy)
		{
			_foundView = this.ViewComponent;
			return true;
		}
		if (!isActiveInHierarchy)
		{
			return false;
		}
		using (List<XUiController>.Enumerator enumerator = this.children.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.TryFindFirstNavigableView(out _foundView))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060075D1 RID: 30161 RVA: 0x002C7074 File Offset: 0x002C5274
	public bool SelectCursorElement(bool _withDelay = false, bool _overrideCursorMode = false)
	{
		if (this.ViewComponent == null)
		{
			return false;
		}
		CursorControllerAbs cursorController = this.xui.playerUI.CursorController;
		if (cursorController.CursorModeActive && !_overrideCursorMode)
		{
			return false;
		}
		XUiView xuiView;
		this.TryFindFirstNavigableView(out xuiView);
		if (xuiView == null)
		{
			return false;
		}
		if (_withDelay)
		{
			cursorController.SetNavigationTargetLater(xuiView);
		}
		else
		{
			cursorController.SetNavigationTarget(xuiView);
		}
		return true;
	}

	// Token: 0x060075D2 RID: 30162 RVA: 0x002C70CC File Offset: 0x002C52CC
	public void SetAllChildrenDirty()
	{
		for (int i = 0; i < this.children.Count; i++)
		{
			this.children[i].SetAllChildrenDirty();
		}
		XUiView xuiView = this.viewComponent;
		if (xuiView != null)
		{
			xuiView.SetDirty();
		}
		this.IsDirty = true;
	}

	// Token: 0x17000DF9 RID: 3577
	// (get) Token: 0x060075D3 RID: 30163 RVA: 0x002C7118 File Offset: 0x002C5318
	public PlayerInputManager.InputStyle CurrentInputStyle
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.lastInputStyle;
		}
	}

	// Token: 0x060075D4 RID: 30164 RVA: 0x002C7120 File Offset: 0x002C5320
	[PublicizedFrom(EAccessModifier.Protected)]
	public void registerForInputStyleChanges()
	{
		if (this.registeredForInputStyleChanges)
		{
			return;
		}
		this.registeredForInputStyleChanges = true;
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged += this.OnLastInputStyleChanged;
	}

	// Token: 0x060075D5 RID: 30165 RVA: 0x002C714D File Offset: 0x002C534D
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		this.curInputStyle = _style;
		this.forceInputStyleChange = true;
	}

	// Token: 0x060075D6 RID: 30166 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void inputStyleChanged(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
	}

	// Token: 0x060075D7 RID: 30167 RVA: 0x002C7160 File Offset: 0x002C5360
	public void ForceInputStyleChange(PlayerInputManager.InputStyle _oldStyle, PlayerInputManager.InputStyle _newStyle)
	{
		if (this.registeredForInputStyleChanges)
		{
			this.inputStyleChanged(_oldStyle, _newStyle);
		}
		foreach (XUiController xuiController in this.children)
		{
			xuiController.ForceInputStyleChange(_oldStyle, _newStyle);
		}
	}

	// Token: 0x17000DFA RID: 3578
	// (get) Token: 0x060075D8 RID: 30168 RVA: 0x0012AC85 File Offset: 0x00128E85
	public XUiController Controller
	{
		get
		{
			return this;
		}
	}

	// Token: 0x060075D9 RID: 30169 RVA: 0x002C71C4 File Offset: 0x002C53C4
	public string GetXuiHierarchy()
	{
		return XUiUtils.GetXuiHierarchy(this);
	}

	// Token: 0x04005742 RID: 22338
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiView viewComponent;

	// Token: 0x04005743 RID: 22339
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiController parent;

	// Token: 0x04005744 RID: 22340
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly List<XUiController> children = new List<XUiController>();

	// Token: 0x04005745 RID: 22341
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiWindowGroup windowGroup;

	// Token: 0x04005746 RID: 22342
	public bool AlwaysUpdate;

	// Token: 0x04005747 RID: 22343
	public bool IsDirty;

	// Token: 0x04005748 RID: 22344
	public object CustomData;

	// Token: 0x04005749 RID: 22345
	public readonly ObservableDictionary<string, object> CustomAttributes = new ObservableDictionary<string, object>();

	// Token: 0x0400574A RID: 22346
	public XUi xui;

	// Token: 0x0400574B RID: 22347
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmControllerUpdate = new ProfilerMarker("XC.Update");

	// Token: 0x0400574C RID: 22348
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmVcUpdate = new ProfilerMarker("VC");

	// Token: 0x0400574D RID: 22349
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmVcNamedUpdate;

	// Token: 0x0400575A RID: 22362
	public readonly BindingsManager Bindings = new BindingsManager();

	// Token: 0x0400575B RID: 22363
	[PublicizedFrom(EAccessModifier.Private)]
	public bool forceInputStyleChange;

	// Token: 0x0400575C RID: 22364
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x0400575D RID: 22365
	[PublicizedFrom(EAccessModifier.Private)]
	public PlayerInputManager.InputStyle curInputStyle;

	// Token: 0x0400575E RID: 22366
	[PublicizedFrom(EAccessModifier.Private)]
	public bool registeredForInputStyleChanges;
}
