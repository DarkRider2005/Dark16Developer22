using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public GameObject ConfirmationObjectEjection;

    public GameObject QuickMenu;
    public GameObject QuickMenuBackPack;
    public GameObject QuickMenuSettings;
    public GameObject QuickMenuExitConfirmation;

    public GameObject DeathPanel;
    public GameObject TaskPanel;

    public void OpenMenu(GameObject openMenu)
    {
        openMenu.SetActive(true);       
    }

    public IEnumerator CloseMenu(float delay, GameObject closeMenu, Animator animator, string trigger)
    {
        animator.SetTrigger(trigger);
        yield return new WaitForSeconds(delay);
        closeMenu.SetActive(false);
    }
}
