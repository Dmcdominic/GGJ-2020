


using UnityEngine;
using UnityEngine.Events;

public class IntArrayEvent : UnityEvent<int[][]> {}
[CreateAssetMenu(menuName = "Derby/PartList")]
public class PartList : PlayerRef<int[],IntArrayEvent> {}