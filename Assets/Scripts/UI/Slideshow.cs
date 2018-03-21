using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slideshow : MonoBehaviour
{

    public List<GameObject> panels;

    List<GameObject> panelIndicator;
    public GameObject current;
    protected int index = 0;
    public ImageInfo activeSprite;
    public ImageInfo inactiveSprite;
    public bool cycle = false;
    public Vector2 panelCenter;
    public Vector2 panelIndicatorCenter;
    [Range(0f, 10f)]
    public float indicatorSpacing = 0.5f;


    public virtual void Init()
    {
        if (panels != null)
        {
            RectTransform rect;
            foreach (GameObject g in panels)
            {
                rect = g.transform as RectTransform;
                rect.localPosition = panelCenter;
                g.gameObject.SetActive(false);

            }
        }
        if (current)
        {
            current.gameObject.SetActive(true);
            index = panels.IndexOf(current);
        }
        CreateIndicators();
        UpdateIndicators();
    }



#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(panelCenter - new Vector2(0, 0.5f), panelCenter + new Vector2(0f, 0.5f));
        Gizmos.DrawLine(panelCenter - new Vector2(0.5f, 0), panelCenter + new Vector2(0.5f, 0));
        if (panels != null ? panels.Count > 0 ? panels[0] != null : false : false)
        {
            RectTransform rect = panels[0].transform as RectTransform;
            Gizmos.DrawWireCube(panelCenter, new Vector3(rect.rect.width * 0.0078125f, rect.rect.height * 0.0078125f, 1));
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(panelIndicatorCenter - new Vector2(0, 0.5f), panelIndicatorCenter + new Vector2(0f, 0.5f));
        Gizmos.DrawLine(panelIndicatorCenter - new Vector2(0.5f, 0), panelIndicatorCenter + new Vector2(0.5f, 0));
    }
#endif

    public virtual void ChangeSlide(int direction)
    {
        if (panels == null ? true : panels.Count == 0)
            return;

        int dir = MathOperations.Sign(direction);
        if (!cycle)
        {
            if (index + dir < panels.Count && index + dir >= 0)
            {
                current.SetActive(false);
                current = panels[index + dir];
                RectTransform rect = current.transform as RectTransform;
                rect.localPosition = panelCenter;
                current.SetActive(true);
                index += dir;
            }
        }
        else
        {
            if (index + dir >= panels.Count)
                index = 0;
            else if (index + dir < 0)
                index = panels.Count - 1;
            else
                index += dir;

            current.SetActive(false);
            current = panels[index];
            RectTransform rect = current.transform as RectTransform;
            rect.localPosition = panelCenter;
            current.SetActive(true);
        }
        UpdateIndicators();

    }

    protected virtual void CreateIndicators()
    {
        if (panels != null ? panels.Count <= 1 : true)
            return;
        if (panelIndicator != null)
        {
            for (int i = panelIndicator.Count - 1; i >= 0; i--)
            {
                Destroy(panelIndicator[i]);
            }
        }
        panelIndicator = new List<GameObject>();
        float spaceCoverage = (panels.Count + 1) * indicatorSpacing;
        Vector2 startPos = new Vector2(panelIndicatorCenter.x - spaceCoverage / 2, panelIndicatorCenter.y);
        for (int i = 0; i < panels.Count; i++)
        {
            GameObject temp = new GameObject("PanelIndicator");
            Image img = temp.AddComponent<Image>();
            img.sprite = inactiveSprite.img;
            img.color = inactiveSprite.imgColor;
            RectTransform rect = temp.transform as RectTransform;
            rect.sizeDelta = new Vector2(inactiveSprite.width, inactiveSprite.height);
            rect.localPosition = new Vector3(startPos.x + indicatorSpacing * (i + 1), panelIndicatorCenter.y, 0);

            temp.transform.SetParent(transform);
            rect.localScale = Vector3.one;
            panelIndicator.Add(temp);

        }

    }

    protected virtual void UpdateIndicators()
    {
        if (panels == null)
            return;

        if (panelIndicator == null ? true : panelIndicator.Count == 0)
        {
            CreateIndicators();
        }

        if (panels.Count != panelIndicator.Count)
            CreateIndicators();

        Image img;
        RectTransform rect;
        for (int i = 0; i < panelIndicator.Count; i++)
        {
            img = panelIndicator[i].GetComponent<Image>();
            rect = panelIndicator[i].transform as RectTransform;
            if (i == index)
            {
                if (img)
                {
                    img.sprite = activeSprite.img;
                    img.color = activeSprite.imgColor;
                }
                rect.sizeDelta = new Vector2(activeSprite.width, activeSprite.height);
            }
            else
            {
                if (img)
                {
                    img.sprite = inactiveSprite.img;
                    img.color = inactiveSprite.imgColor;
                }
                rect.sizeDelta = new Vector2(inactiveSprite.width, inactiveSprite.height);
            }
        }
    }
}
