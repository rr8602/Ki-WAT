using System.Collections.Generic;
using System.Windows.Forms;
using System;
using System.Linq;

public class FormManager
{
    private static FormManager _instance;
    private static readonly object _lock = new object();
    private Dictionary<Type, Form> _forms = new Dictionary<Type, Form>();

    public static FormManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new FormManager();
                    }
                }
            }
            return _instance;
        }
    }

    public void RegisterForm<T>(Form form) where T : Form
    {
        _forms[typeof(T)] = form;
    }

    public T GetForm<T>() where T : Form
    {
        return _forms.TryGetValue(typeof(T), out var form) ? (T)form : null;
    }

    public List<Form> GetAllForms()
    {
        return _forms.Values.ToList();
    }
}