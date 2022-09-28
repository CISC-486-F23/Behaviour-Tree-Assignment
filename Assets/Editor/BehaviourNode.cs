using System.Collections;
using System.Collections.Generic;
using Behaviours;
using Behaviours.Action;
using Behaviours.Conditional;
using Behaviours.Control;
using Behaviours.Decorator;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class BehaviourNode : UnityEditor.Experimental.GraphView.Node
{
  public Port input;
  public Port output;

  protected Behaviours.Behaviour _behaviour;
  public Behaviours.Behaviour Behaviour
  {
    get { return _behaviour; }
  }

  protected string _name;
  protected int? _index;

  protected Label _resultLabel;

  protected Color _successColor;
  protected Color _failureColor;
  protected Color _runningColor;

  public int? Index
  {
    get { return _index;}
    set
    {
      _index = value;
      title = (_index.HasValue ? _index.Value + ": " : "") + _name;
    } 
  }
  
  public BehaviourNode(string name, Behaviours.Behaviour behaviour, bool root = false)
  {
    _name = name;
    title = name;
    _behaviour = behaviour;
    
    SetPosition(new Rect(behaviour.Position, GetPosition().size));
    
    var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourNode.uxml");
    VisualElement labelFromUXML = visualTree.Instantiate();
    Add(labelFromUXML);

    Initialize(root);

    RefreshExpandedState();
    RefreshPorts();

    _resultLabel = labelFromUXML.Q<Label>("Result");

    ColorUtility.TryParseHtmlString("#80e0ff", out _successColor);
    ColorUtility.TryParseHtmlString("#ff80e0", out _failureColor);
    ColorUtility.TryParseHtmlString("#ffe080", out _runningColor);
    
    SetColor();
  }
  
  protected abstract void Initialize(bool root);

  public void UpdateBehaviour()
  {
    _behaviour.Position = this.GetPosition().position;
    
    SetColor();
  }

  protected void SetColor()
  {
    if (_behaviour.Result == Status.None)
    {
      _resultLabel.style.backgroundColor = new StyleColor(Color.white);
      _resultLabel.text = "";
    }

    if (_behaviour.Result == Status.Success)
    {
      _resultLabel.style.backgroundColor = new StyleColor(_successColor);
      _resultLabel.text = "Success";
    }

    if (_behaviour.Result == Status.Failure)
    {
      _resultLabel.style.backgroundColor = new StyleColor(_failureColor);
      _resultLabel.text = "Failure";
    }

    if (_behaviour.Result == Status.Running)
    {
      _resultLabel.style.backgroundColor = new StyleColor(_runningColor);
      _resultLabel.text = "Running";
    }
  }

  public Edge ConnectTo<T>(T child) where T : BehaviourNode
  {
    return output.ConnectTo(child.input);
  }

  protected void CreateInput()
  {
    input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
    input.portName = "Execute";
    inputContainer.Add(input);
  }

  protected void CreateOutput(Port.Capacity capacity)
  {
    if (output == null)
    {
      output = InstantiatePort(Orientation.Horizontal, Direction.Output, capacity, typeof(bool));
      output.portName = "Execute";
      outputContainer.Add(output);
    }
  }
}

public class CompositeBehaviourNode : BehaviourNode
{
  public CompositeBehaviourNode(string name, Composite behaviour, bool root) : base(name, behaviour, root)
  {
    style.color = new StyleColor(Color.blue);
  }

  protected override void Initialize(bool root)
  {
    if(root) CreateOutput(Port.Capacity.Single);
    else CreateInput();
  }
}

public class ControlBehaviourNode : BehaviourNode
{
  public ControlBehaviourNode(string name, Control behaviour) : base(name, behaviour)
  {
    
  }

  protected override void Initialize(bool root)
  {
    CreateInput();
    CreateOutput(Port.Capacity.Multi);
  }
}

public class ConditionalBehaviourNode : BehaviourNode
{
  public ConditionalBehaviourNode(string name, Conditional behaviour) : base(name, behaviour)
  {
    
  }

  protected override void Initialize(bool root)
  {
    CreateInput();
  }
}

public class DecoratorBehaviourNode : BehaviourNode
{
  public DecoratorBehaviourNode(string name, Decorator behaviour) : base(name, behaviour)
  {
    
  }

  protected override void Initialize(bool root)
  {
    CreateInput();
    CreateOutput(Port.Capacity.Single);
  }
}

public class ActionBehaviourNode : BehaviourNode
{
  public ActionBehaviourNode(string name, Action behaviour) : base(name, behaviour)
  {
    
  }

  protected override void Initialize(bool root)
  {
    CreateInput();
  }
}