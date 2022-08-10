using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SafeQuest : MonoBehaviour
{
    public Button[] buttons;
    public GameObject lockWin;
    public GameObject unlockWin;

    private void Start()
    {
        foreach (var button in buttons)
            button.onClick.AddListener(() => OnButtonClick(button));
    }
    public void OnButtonClick(Button button)
    {
        button.interactable = false;
        if (buttons.All(x => x.interactable == false))
        {
            Unlock();
        }
    }
    public void Unlock()
    {
        lockWin.SetActive(false);
        unlockWin.SetActive(true);
    }
    public void OnFailedCode()
    {
        foreach (var button in buttons)
            button.interactable = true;
    }
}
