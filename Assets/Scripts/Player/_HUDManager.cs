using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class _HUDManager : MonoBehaviour
{
    Dictionary<string, Button> _buttons = new Dictionary<string, Button>();
    Button _currentButton;
    float _currentCooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < transform.childCount; i ++)
        {
            _buttons.Add(transform.GetChild(i).name, transform.GetChild(i).GetComponent<Button>());
            Debug.Log(transform.GetChild(i).name, transform.GetChild(i).GetComponent<Button>());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerButton(string name, float time)
    {
        Cooldown(time, _buttons[name]);
    }

    public void Cooldown(float time, Button button)
    {
        button.interactable = false;
        StartCoroutine(WaitCooldown(time, button));

    }

    IEnumerator WaitCooldown(float time, Button button)
    {
        yield return new WaitForSeconds(time);
        button.interactable = true;
    }
}
