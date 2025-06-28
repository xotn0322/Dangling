using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBlock
{
    public List<Hole> holes = new();


    public void InitializeHoles()
    {
        // 규격화 해놓은 구멍 위치들 선언
        holes.Add(new(500f, 400f));
        holes.Add(new(700f, 900f));
        holes.Add(new(100f, 300f));
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
