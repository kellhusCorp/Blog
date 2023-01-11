namespace MyBlogOnCore.Models;

public class NavbarListItemAdminViewModel
{
    public NavbarListItemAdminViewModel(string actionName, string controllerName, bool isActive, string text)
    {
        ActionName = actionName;
        ControllerName = controllerName;
        IsActive = isActive;
        Text = text;
    }

    public string ActionName { get; }
    
    public string ControllerName { get; }
    
    public bool IsActive { get; }
    
    public string Text { get; }
}