using System.Collections;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject character;
    public float time = 3f;

    void Start()
    {
        StartCoroutine(SpawnAfterDelay());
    }

    IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(time);
        character.SetActive(true);
    }
}
