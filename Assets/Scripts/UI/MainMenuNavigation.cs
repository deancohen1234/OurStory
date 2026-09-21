using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuNavigation : MonoBehaviour
{

    public CanvasGroup MainMenuGroup;
    public CanvasGroup FileSelectGroup;
    public CanvasGroup CreditsGroup;

    public GameObject DefaultSelectedObject;

    private MenuSection CurrentSelection;

    public enum MenuSection 
    { 
        MainMenu = 0,
        FileSelect = 1,
        Credits = 2
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
        CurrentSelection = section;

        DisableAllGroups();

        switch (section)
        {
            case MenuSection.MainMenu:
                EnableGroup(MainMenuGroup);
                break;
            case MenuSection.FileSelect:
                EnableGroup(FileSelectGroup);
                break;
            case MenuSection.Credits:
                EnableGroup(CreditsGroup);
                break;
        }

        SetSelection();
    }

    private void DisableAllGroups()
    {
        DisableGroup(MainMenuGroup);
        DisableGroup(FileSelectGroup);
        DisableGroup(CreditsGroup);
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
            case MenuSection.Credits:
                EnableGroup(CreditsGroup);
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

    private void SetSelection()
    {
        switch (CurrentSelection)
        {
            case MenuSection.MainMenu:
                EventSystem.current.SetSelectedGameObject(DefaultSelectedObject);
                break;

            case MenuSection.FileSelect:
                SaveSlot slot = gameObject.GetComponentInChildren<SaveSlot>();
                EventSystem.current.SetSelectedGameObject(slot.gameObject);
                break;
        }
    }
}
