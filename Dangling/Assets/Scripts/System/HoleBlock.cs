using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleBlock
{
    public List<Hole> holes = new();


    public void InitializeHoles()
    {
        // �԰�ȭ �س��� ���� ��ġ�� ����
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
