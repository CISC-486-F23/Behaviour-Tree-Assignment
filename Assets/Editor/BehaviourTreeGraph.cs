using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviours;
using Behaviours.Action;
using Behaviours.Conditional;
using Behaviours.Control;
using Behaviours.Decorator;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Behaviour = Behaviours.Behaviour;

public class BehaviourTreeGraph : GraphView
{
  public new class UxmlFactory : UxmlFactory<BehaviourTreeGraph, GraphView.UxmlTraits> { }

  public System.Action SaveCallback;
  
  public BehaviourTree _tree;
  private GameObject _prefab;
  private Transform _compositesTransform;
  private GridBackground _background;

  public enum MenuContext
  {
    Root,
    Controls,
    Decorators,
    Conditionals,
    Actions,
    Composites
  }

  private MenuContext _context;

  public GameObject Prefab
  {
    set { _prefab = value; }
  }

  public Transform Composites
  {
    set { _compositesTransform = value; }
  }
  
  public BehaviourTreeGraph() : base()
  {
    var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
    styleSheets.Add(styleSheet);
    
    _background = new GridBackground();
    Insert(0, _background);
    
    this.AddManipulator(new ContentZoomer());
    this.AddManipulator(new ContentDragger());
    this.AddManipulator(new SelectionDragger());
    this.AddManipulator(new RectangleSelector());
    
    this.graphViewChanged = OnGraphChanged;
  }

  public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
  {
    base.BuildContextualMenu(evt);
    evt.menu.AppendSeparator();
    
    if (_context == MenuContext.Root)
    {
      evt.menu.AppendAction("Controls", action => _context = MenuContext.Controls);
      evt.menu.AppendAction("Decorators", action => _context = MenuContext.Decorators);
      evt.menu.AppendAction("Conditionals", action => _context = MenuContext.Conditionals);
      evt.menu.AppendAction("Actions", action => _context = MenuContext.Actions);
      evt.menu.AppendAction("Composites", action => _context = MenuContext.Composites);
    }
    else
    {
      evt.menu.AppendAction("Back", action => _context = MenuContext.Root);
      evt.menu.AppendSeparator();
    }
    
    if (_context == MenuContext.Controls) AppendBehaviourActions(evt, _tree.Controls);
    if (_context == MenuContext.Decorators) AppendBehaviourActions(evt, _tree.Decorators);
    if (_context == MenuContext.Conditionals) AppendBehaviourActions(evt, _tree.Conditionals);
    if (_context == MenuContext.Actions) AppendBehaviourActions(evt, _tree.Actions);
    if (_context == MenuContext.Composites) AppendBehaviourActions(evt, _tree.Composites);
  }

  protected void AppendBehaviourActions<T>(ContextualMenuPopulateEvent evt, List<T> behaviours) where T : Behaviour
  {
    Behaviour open = _prefab.GetComponent<Behaviour>();
    
    foreach (T behaviour in behaviours)
    {
      evt.menu.AppendAction(behaviour.name, a =>
      {
        Behaviour toCreate = behaviour;

        if (behaviour is Composite && !behaviour.Contains(open))
        {
          Composite composite = GameObject.Instantiate(_tree.CompositePrefab, _prefab.transform).GetComponent<Composite>();
          composite.Root = behaviour;
          composite.name = behaviour.name;
          toCreate = composite as T;
        }
        
        CreateNode(toCreate.name, _prefab, toCreate, false, Vector2.Scale((Vector2)contentViewContainer.transform.position, Vector2.left + Vector2.down));
      });
    }
  }

  public void Update()
  {
    foreach(Node node in nodes) (node as BehaviourNode).UpdateBehaviour();
  }

  public BehaviourNode CreateNode<T>(string name, GameObject parent, T t, bool root = false, Vector2? position = null) where T : Behaviours.Behaviour
  {
    Behaviours.Behaviour behaviour = GameObject.Instantiate(t.gameObject, parent.transform).GetComponent<Behaviours.Behaviour>();
    if (position.HasValue)
    {
      behaviour.Position = position.Value;
    }
    behaviour.gameObject.name = behaviour.gameObject.name.Replace("(Clone)", "");
    return CreateNode(name, behaviour, root, position);
  }

  public BehaviourNode CreateNode<T>(string name, T behaviour, bool root, Vector2? position) where T : Behaviours.Behaviour
  {
    BehaviourNode node = null;

    if (behaviour is Composite) node = new CompositeBehaviourNode(name, behaviour as Composite, root);
    if (behaviour is Control) node = new ControlBehaviourNode(name, behaviour as Control);
    if (behaviour is Decorator) node = new DecoratorBehaviourNode(name, behaviour as Decorator);
    if (behaviour is Conditional) node = new ConditionalBehaviourNode(name, behaviour as Conditional);
    if (behaviour is Action) node = new ActionBehaviourNode(name, behaviour as Action);

    if(position.HasValue) node.SetPosition(new Rect(position.Value, node.contentRect.size));
    AddElement(node);
    
    return node;
  }

  public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
  {
    return ports.ToList().Where(endPort => endPort.direction != startPort.direction).ToList();
  }
  
  protected GraphViewChange OnGraphChanged(GraphViewChange change)
  {
    if (change.edgesToCreate != null)
    {
      foreach (Edge e in change.edgesToCreate)
      {
        int index = (e.output.node as BehaviourNode).Behaviour.AddChild((e.input.node as BehaviourNode).Behaviour);
        (e.input.node as BehaviourNode).Index = index;
        NameChildren(e.output.node as BehaviourNode);
      }
    }

    if (change.elementsToRemove != null)
    {
      foreach (VisualElement ve in change.elementsToRemove)
      {
        Edge e = ve as Edge;
        if (e != null)
        {
          (e.output.node as BehaviourNode).Behaviour.RemoveChild((e.input.node as BehaviourNode).Behaviour, _prefab.transform);
          NameChildren(e.output.node as BehaviourNode);
        }
        
        BehaviourNode n = ve as BehaviourNode;
        if (n != null)
        {
          if(n.input != null)
            foreach (Edge ie in n.input.connections)
            {
              (ie.output.node as BehaviourNode).Behaviour.RemoveChild(n.Behaviour, _prefab.transform);
              NameChildren(ie.output.node as BehaviourNode);
            }
          if(n.output != null) foreach(Edge oe in n.output.connections) n.Behaviour.RemoveChild((oe.input.node as BehaviourNode).Behaviour, _prefab.transform);
          
          NameChildren(n);
        }
      }
      
      foreach (VisualElement ve in change.elementsToRemove)
      {
        BehaviourNode n = ve as BehaviourNode;
        if(n != null) GameObject.DestroyImmediate(n.Behaviour.gameObject);
      }
    }

    Update();
    SaveCallback();
    
    return change;
  }

  protected void NameChildren(BehaviourNode node)
  {
    if (node.output != null)
    {
      List<Edge> children = node.output.connections.ToList();
      for (int i = 0; i < children.Count; i++) (children[i].input.node as BehaviourNode).Index = (children[i].output.node as BehaviourNode).Behaviour.Children.IndexOf((children[i].input.node as BehaviourNode).Behaviour) + 1;
    }
  }
}
