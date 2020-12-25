/* Sets the dropdown component's onValueChanged method to the selected method of the GameObject's SelectObjectsScriptFunction */
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SetDropdownOnValueChangedFunction : MonoBehaviour {

    Dropdown dropdown;
    SelectObjectsScriptFunction selectObjectsScriptFunction;

    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        selectObjectsScriptFunction = GetComponent<SelectObjectsScriptFunction>();
        dropdown.onValueChanged.AddListener(delegate {
            MonoBehaviour monoBehaviour = selectObjectsScriptFunction.SelectedScript();
            MethodInfo method = selectObjectsScriptFunction.SelectedMethod();
            method.Invoke(monoBehaviour, new object[]{dropdown.value});
        });
    }

}