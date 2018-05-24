using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class DotsAndBoxesNode : Node
{
    public Box box;

    public DotsAndBoxesNode(Position pos, Box box)
    {
        this.pos = pos;
        this.box = new Box(box);
    }
    public DotsAndBoxesNode(DotsAndBoxesNode other) : this(other.pos, other.box)
    {

    }

    public DotsAndBoxesNode()
    {
    }

    public DotsAndBoxesNode(Position pos)
    {
        this.pos = pos;
    }
}
