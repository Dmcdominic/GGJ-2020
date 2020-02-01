


using UnityEngine;
using UnityEngine.Events;

public class IntArrayEvent : UnityEvent<SerializedParts[]> {}

[CreateAssetMenu(menuName = "Derby/PartList")]
public class PartList : PlayerRef<SerializedParts,IntArrayEvent> {}