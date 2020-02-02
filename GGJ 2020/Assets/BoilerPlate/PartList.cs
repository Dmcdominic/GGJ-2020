using UnityEngine;
using UnityEngine.Events;

public class SerializedPartArrayEvent : UnityEvent<SerializedParts[]> {}

[CreateAssetMenu(menuName = "Derby/PartList")]
public class PartList : PlayerRef<SerializedParts, SerializedPartArrayEvent> {}