using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntArrayEvent : UnityEvent<int[]> { }

[CreateAssetMenu(menuName = "Derby/Lives List")]
public class LivesList : PlayerRef<int, IntArrayEvent> { }