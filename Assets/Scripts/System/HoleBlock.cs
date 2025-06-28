using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBlock
{
    public List<Hole> holes = new();
    public List<HolePositionData> positionDatas =new();

    public void InitializeHoles()
    {
        holes.Add(new(3f, -3f));
        holes.Add(new(7f, 1f));
        holes.Add(new(2f, 4f));
    }

    public Hole PickRandomPosition()
    {
        Hole[] result = RandomManager.Instance.RandomInCollection(holes, 1, false);
        return (result != null && result.Length > 0) ? result[0] : null;
    }

    public Hole[] PickRandomPositions(int amount, bool allowDuplicate)
    {
        return RandomManager.Instance.RandomInCollection(holes, amount, allowDuplicate);
    }
}
