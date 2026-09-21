using UnityEngine;

public class MainMenuNavigation : MonoBehaviour
{

    public CanvasGroup MainMenuGroup;
    public CanvasGroup FileSelectGroup;

    public enum MenuSection 
    { 
        MainMenu = 0,
        FileSelect = 1
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisableAllGroups();

        EnableGroup(MenuSection.MainMenu);
    }

    public void OnButtonSelected(int section)
    {
        OnButtonSelected((MenuSection)section);
    }

    public void OnButtonSelected(MenuSection section)
    {
        DisableAllGroups();

        switch (section)
        {
            case MenuSection.MainMenu:
                EnableGroup(MainMenuGroup);
                break;
            case MenuSection.FileSelect:
                EnableGroup(FileSelectGroup);
                break;
        }
    }

    private void DisableAllGroups()
    {
        DisableGroup(MainMenuGroup);
        DisableGroup(FileSelectGroup);
    }

    private void EnableGroup(MenuSection section)
    {
        DisableAllGroups();

        switch (section)
        {
            case MenuSection.MainMenu:
                EnableGroup(MainMenuGroup);
                break;
            case MenuSection.FileSelect:
                EnableGroup(FileSelectGroup);
                break;
        }
    }

    private void EnableGroup(CanvasGroup group)
    {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    private void DisableGroup(CanvasGroup group)
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }
}
