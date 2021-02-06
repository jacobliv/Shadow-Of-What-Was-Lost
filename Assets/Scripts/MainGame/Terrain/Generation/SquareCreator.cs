using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SquareConfiguration;

public class SquareCreator : MonoBehaviour {
    public void generate(GenerationData data) {
        Dictionary<Vector2, Square> squares = new Dictionary<Vector2, Square>();
        
        foreach (KeyValuePair<Vector2,ControlNode> node in data.ControlNodes) {
            if(!data.Bounds.inside(node.Key)) continue;
            ControlNode topLeft = node.Value;
            
            Vector2 topRightPos = new Vector2(topLeft.Position.x + GameManager.SQUARE_SIZE, topLeft.Position.y);
            if(!data.ControlNodes.ContainsKey(topRightPos) || !data.Bounds.inside(topRightPos)) continue;
            ControlNode topRight = data.ControlNodes[topRightPos];
            
            Vector2 bottomRightPos = new Vector2(topLeft.Position.x + GameManager.SQUARE_SIZE, topLeft.Position.y - GameManager.SQUARE_SIZE);
            if(!data.ControlNodes.ContainsKey(bottomRightPos) || !data.Bounds.inside(bottomRightPos)) continue;
            ControlNode bottomRight = data.ControlNodes[bottomRightPos];
            
            Vector2 bottomLeftPos = new Vector2(topLeft.Position.x, topLeft.Position.y - GameManager.SQUARE_SIZE);
            if(!data.ControlNodes.ContainsKey(bottomLeftPos) || !data.Bounds.inside(bottomLeftPos)) continue;
            ControlNode bottomLeft = data.ControlNodes[bottomLeftPos];
            
            Node centerTop = topLeft.Right;
            Node centerRight = topRight.Below;
            Node centerBottom = bottomLeft.Right;
            Node centerLeft = topLeft.Below;
            Node center = topLeft.Center;
            SquareConfiguration configuration = getConfiguration(topLeft, topRight, bottomRight, bottomLeft);
            
            Square square = new Square(topLeft,
                                       topRight,
                                       bottomRight,
                                       bottomLeft,
                                       centerTop,
                                       centerRight,
                                       centerBottom,
                                       centerLeft,
                                       center,
                                       configuration);
            squares.Add(topLeft.Position,square);
        }

        data.squares = squares;
    }

    private SquareConfiguration getConfiguration(ControlNode topLeft,
                                                 ControlNode topRight,
                                                 ControlNode bottomRight,
                                                 ControlNode bottomLeft) {
        bool tl = topLeft.material > 0;
        bool tr = topRight.material > 0;
        bool br = bottomRight.material > 0;
        bool bl = bottomLeft.material > 0;

        if (tl && tr && br && bl) {
            return FULL;
        } else if (tl && tr && br) {
            return TL_TR_BR;
        }else if (tl && tr && bl) {
            return TL_TR_BL;
        }else if (tl && br && bl) {
            return TL_BR_BL;
        }else if (tr && br && bl) {
            return TR_BR_BL;
        }else if (tl && tr) {
            return TL_TR;
        }else if (tl && br) {
            return TL_BR;
        }else if (tl && bl) {
            return TL_BL;
        }else if (tr && br) {
            return TR_BR;
        }else if (tr && bl) {
            return TR_BL;
        }else if (br && bl) {
            return BR_BL;
        }else if (tl) {
            return TL;
        }else if (tr) {
            return TR;
        }else if (br) {
            return BR;
        }else if (bl) {
            return BL;
        }

        return EMPTY;
    }
}
