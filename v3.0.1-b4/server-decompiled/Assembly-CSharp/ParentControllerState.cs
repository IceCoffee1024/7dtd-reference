using System;

// Token: 0x020010EF RID: 4335
public class ParentControllerState
{
	// Token: 0x06008965 RID: 35173 RVA: 0x00346764 File Offset: 0x00344964
	public ParentControllerState(XUiController parentController)
	{
		if (parentController == null)
		{
			return;
		}
		if (parentController.ViewComponent == null)
		{
			this.m_parentControllers = new XUiController[parentController.Children.Count];
			for (int i = 0; i < parentController.Children.Count; i++)
			{
				this.m_parentControllers[i] = parentController.Children[i];
			}
		}
		else
		{
			this.m_parentControllers = new XUiController[] { parentController };
		}
		this.m_isVisible = new bool[this.m_parentControllers.Length];
		this.m_isEscClosable = new bool[this.m_parentControllers.Length];
		for (int j = 0; j < this.m_parentControllers.Length; j++)
		{
			this.m_isVisible[j] = this.m_parentControllers[j].ViewComponent.IsVisible;
			this.m_isEscClosable[j] = this.m_parentControllers[j].WindowGroup.isEscClosable;
		}
	}

	// Token: 0x06008966 RID: 35174 RVA: 0x00346844 File Offset: 0x00344A44
	public void Hide()
	{
		if (this.m_parentControllers == null)
		{
			return;
		}
		for (int i = 0; i < this.m_parentControllers.Length; i++)
		{
			this.m_parentControllers[i].ViewComponent.IsVisible = false;
			this.m_parentControllers[i].WindowGroup.isEscClosable = false;
		}
	}

	// Token: 0x06008967 RID: 35175 RVA: 0x00346894 File Offset: 0x00344A94
	public void Restore()
	{
		if (this.m_parentControllers == null)
		{
			return;
		}
		for (int i = 0; i < this.m_parentControllers.Length; i++)
		{
			this.m_parentControllers[i].ViewComponent.IsVisible = this.m_isVisible[i];
			this.m_parentControllers[i].WindowGroup.isEscClosable = this.m_isEscClosable[i];
		}
	}

	// Token: 0x040065F7 RID: 26103
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly XUiController[] m_parentControllers;

	// Token: 0x040065F8 RID: 26104
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool[] m_isVisible;

	// Token: 0x040065F9 RID: 26105
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly bool[] m_isEscClosable;
}
