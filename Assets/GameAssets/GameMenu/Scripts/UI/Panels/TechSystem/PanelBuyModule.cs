using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBuyModule : MonoBehaviour
{

    [SerializeField]
    private List<ItemTechModule> itemTechModules = new();
    [SerializeField] private int columns = 5;
    [SerializeField] private float delayBetweenItems = 0.05f;

    private void Start()
    {
        StartCoroutine(ScaleItemsDiagonal());
    }

    private IEnumerator ScaleItemsDiagonal()
    {
        int rowCount = Mathf.CeilToInt((float)itemTechModules.Count / columns);

        foreach (var item in itemTechModules)
        {
            item.transform.localScale = Vector3.zero;
        }

        for (int diagonal = 0; diagonal < rowCount + columns - 1; diagonal++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                int col = diagonal - row;
                if (col >= 0 && col < columns)
                {
                    int index = row * columns + col;
                    if (index >= 0 && index < itemTechModules.Count)
                    {
                        Transform t = itemTechModules[index].transform;
                        StartCoroutine(ScaleToOne(t));
                    }
                }
            }
            yield return new WaitForSeconds(delayBetweenItems);
        }
    }

    private IEnumerator ScaleToOne(Transform t)
    {
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;
        float duration = 0.3f;
        float time = 0f;

        while (time < duration)
        {
            t.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        t.localScale = endScale;
    }
}
