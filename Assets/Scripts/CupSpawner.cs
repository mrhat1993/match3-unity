using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupSpawner : MonoBehaviour
{
    [SerializeField] private Cup cupPrefab;
    public Cup CupPrefab => cupPrefab;
}
