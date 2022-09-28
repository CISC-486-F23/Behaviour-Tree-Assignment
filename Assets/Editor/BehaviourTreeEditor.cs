using System;
using System.Collections.Generic;
using Behaviours;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;
using Behaviour = Behaviours.Behaviour;


public class BehaviourTreeEditor : EditorWindow
{
    private enum Modes
    {
        View,
        Edit
    }

    private const string _prefabPath = "Assets/Resources/Prefabs/Player/Player.prefab";
    private GameObject _prefab;

    private Modes _mode;
    private BehaviourTree _tree;
    private Composite _openInEditor;
    
    private BehaviourTreeGraph _graph;
    
    private TextField _nameText;

    private ListView _composites;
    private Button _compositesButton;

    [MenuItem("Tools/Behaviour Tree Editor")]
    public static void ShowExample()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("Behaviour Tree Editor");
    }

    public void CreateGUI()
    {
        SceneManager.sceneLoaded += (scene, mode) => SetTree(GameObject.FindObjectOfType<BehaviourTree>());
        SceneManager.sceneUnloaded += (scene) => LoadPrefab();

        VisualElement root = rootVisualElement;
        
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);
        
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        _graph = root.Q<BehaviourTreeGraph>("BehaviourTreeGraph");

        _nameText = root.Q<TextField>("NameField");
        
        _nameText.RegisterCallback<KeyDownEvent>(evt =>
        {
            if (evt.keyCode == KeyCode.Return)
            {
                _openInEditor.name = _nameText.value;
                Refresh();
            }
        });

        _composites = root.Q<ListView>("CompositesList");
        _compositesButton = root.Q<Button>("CompositesButton");

        _compositesButton.clicked += () => CreateNewComposite();
        
        LoadPrefab();
    }

    private void Update()
    {
        if(_tree == null || (_mode == Modes.View && !Application.isPlaying)) LoadPrefab();
    }
    
    private void LoadPrefab()
    {
        SetTree(null);
    }

    protected void CreateNewComposite()
    {
        if (_tree != null)
        {
            _tree.CreateNewComposite();
            RefreshComposites();
        }
    }
    
    public void OnSelectionChange()
    {
        if (!Application.isPlaying)
        {
            if (Selection.gameObjects.Length > 0)
            {
                BehaviourTree tree = Selection.gameObjects[0].GetComponent<BehaviourTree>();
                SetTree(tree);
            }
            else SetTree(null);
        }
    }

    public void SavePrefab()
    {
        PrefabUtility.SaveAsPrefabAsset(_prefab, _prefabPath);
    }
    
    protected void SetTree(BehaviourTree tree)
    {
        if (tree == null)
        {
            _mode = Modes.Edit;
            _prefab = PrefabUtility.LoadPrefabContents(_prefabPath);
            tree = _prefab.GetComponentInChildren<BehaviourTree>();
            _graph.SaveCallback = () => { SavePrefab(); };
        }
        else
        {
            _mode = Modes.View;
            _graph.SaveCallback = () => { };
        }
        
        _tree = tree;
        if (_tree != null)
        {
            _openInEditor = _tree.Root;
            _tree.DrawCallback = () => _graph.Update();
            _graph._tree = _tree;
            _nameText.SetValueWithoutNotify(_openInEditor.name);
        }
        
        Refresh();
    }

    protected void SetOpen(Composite composite)
    {
        _openInEditor = composite;
        _nameText.SetValueWithoutNotify(_openInEditor.name);
        Refresh();
    }
    
    protected void Refresh()
    {
        if (_tree != null && _openInEditor != null)
        {
            RefreshComposites();
            BuildGraph();
        }
    }
    
    protected void RefreshComposites()
    {
        _composites.itemsSource = _tree != null ? _tree.Composites : new List<Composite>();
        _composites.makeItem = () => new BehaviourTreeButton<Composite>();
        _composites.bindItem = (item, index) =>
        {
            if (index < _tree.Composites.Count)
            {
                (item as BehaviourTreeButton<Composite>).text = _tree.Composites[index].name;

                (item as BehaviourTreeButton<Composite>)._behaviour = _tree.Composites[index];
                (item as BehaviourTreeButton<Composite>).clicked += () =>
                {
                    SetOpen(_tree.Composites[index]);
                };
            }
        };
        
        _composites.Rebuild();
    }
    
    protected void BuildGraph()
    {
        foreach(Node node in _graph.nodes) _graph.RemoveElement(node);
        foreach(Edge edge in _graph.edges) _graph.RemoveElement(edge);

        if (_tree != null)
        {
            _graph.Prefab = _openInEditor.gameObject;
            _graph.Composites = _tree.CompositesTransform;
    
            BuildGraph(_openInEditor, true);
        }
    }

    protected BehaviourNode BuildGraph(Behaviours.Behaviour behaviour, bool root)
    {
        BehaviourNode node = _graph.CreateNode(behaviour.name, behaviour, root, null);

        bool compositeChild = behaviour is Composite && !root;
        
        // Do not build subgraph for composite children
        if (!compositeChild)
        {
            foreach (Behaviours.Behaviour child in behaviour.transform.GetComponentsInChildren<Behaviours.Behaviour>())
            {
                if (child != behaviour && child.transform.parent == behaviour.transform)
                {
                    BehaviourNode childNode = BuildGraph(child, false);
                    if (behaviour.Children.Contains(child))
                    {
                        Edge e = node.ConnectTo(childNode);
                        childNode.Index = behaviour.Children.IndexOf(child) + 1;
                        _graph.Add(e);
                    }
                }
            }
        }
        
        return node;
    }
}

public class BehaviourTreeButton<T> : Button where T : Behaviour
{
    public T _behaviour;
}