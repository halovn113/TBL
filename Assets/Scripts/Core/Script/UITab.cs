using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITab : MonoBehaviour
{
    [Serializable]
    public class TabInfo
    {
        public string name;
        public Button button;
        public GameObject highlight;
        public GameObject content;
    }

    public List<TabInfo> listTab;

    public string defaultTab;
    private TabInfo _selectedTab;

    public bool CheckName(string name)
    {
        foreach (TabInfo tab in listTab)
        {
            if (tab.name == name) return true;
        }
        return false;
    }

    void Start()
    {
        if (defaultTab == "" || CheckName(defaultTab) || defaultTab == null)
        {
            defaultTab = listTab[0].name;
        }
        _selectedTab = null;
        foreach (TabInfo tab in listTab)
        {
            tab.button.onClick.AddListener(() => ActiveContent(tab));
        }

        foreach (TabInfo tab in listTab)
        {
            if (tab.name == defaultTab)
            {
                ActiveContent(tab);
                _selectedTab = tab;
            }
            else
            {
                DeactiveContent(tab);
            }
        }
    }

    public void ActiveContent(TabInfo tab)
    {
        if (_selectedTab == tab) return;
        if (!tab.content.activeSelf)
        {
            tab.content.SetActive(true);
            tab.highlight.SetActive(true);
        }
        if (_selectedTab != null) DeactiveContent(_selectedTab);
        _selectedTab = tab;
    }

    public void DeactiveContent(TabInfo tab)
    {
        tab.content.SetActive(false);
        tab.highlight.SetActive(false);
    }

}



//[CustomEditor(typeof(UITab)]
//public class UITabMenu: Editor
//{
//    private ReorderableList list;

//    private void OnEnable()
//    {
//        list = new ReorderableList(serializedObject, serializedObject.FindProperty("listTab"), true, true, true, true);


//    }

//    protected void Draw(Rect rect, int index, bool isActive, bool isFocus)
//    {

//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        list.DoLayoutList();
//        serializedObject.ApplyModifiedProperties();
//    }
//}