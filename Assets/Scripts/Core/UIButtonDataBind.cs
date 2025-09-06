using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonDataBind : MonoBehaviour
{
    public MonoBehaviour Target;
    public string Method;

    private void Awake()
    {
        var btn = GetComponent<Button>();
        
        if (!Target || string.IsNullOrEmpty(Method))
        {
            return;
        }
        
        var mi = Target.GetType().GetMethod(Method,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        
        if (mi == null || mi.GetParameters().Length != 0)
        {
            return;
        }
        
        btn.onClick.AddListener(() => mi.Invoke(Target, null));
    }
}
