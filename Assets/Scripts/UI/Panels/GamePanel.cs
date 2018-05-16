using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
public class GamePanel : SerializedMonoBehaviour
{

    /// <summary>
    /// The panel's animation time.
    /// </summary>
    public static float animTime { get { return 0.3f; } }
    /// <summary>
    /// The reference for the center of the screen.
    /// </summary>
    public Vector2 screenCenter;
    /// <summary>
    /// Panel to change when the Escape key is pressed. (Back key on mobile)
    /// </summary>
    public GamePanel onBackPanel;
    public List<GameObject> panelObjects = new List<GameObject>();

    public bool moving { get; internal set; }

    /// <summary>
    /// Places the panel on the center of the screen. Defined by the screenCenter variable.
    /// </summary>
    public virtual void CenterPanel()
    {
        RectTransform rect = transform as RectTransform;
        rect.localPosition = screenCenter;
    }

    /// <summary>
    /// Makes the SceneController change the current panel to this one.
    /// </summary>
    public virtual void ChangeToThisPanel()
    {
        SceneController.instance?.ChangePanel(this);
    }

    /// <summary>
    /// Returns the default offscreen position for animating the panel. 
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public virtual Vector2 DefaultStartPosition(int direction)
    {
        RectTransform rect = transform as RectTransform;
        return new Vector2(rect.rect.width * Mathf.Sign(direction) + screenCenter.x, screenCenter.y);
    }

    protected virtual void OnEnable()
    {
        SceneController.OnBack += OnBack;
    }

    protected virtual void OnDisable()
    {
        SceneController.OnBack -= OnBack;
    }

    public virtual IEnumerator Enter()
    {
        if (moving)
            yield break;

        moving = true;

        foreach (GameObject obj in panelObjects)
        {
            obj.gameObject.SetActive(false);
        }

        Vector2 start = DefaultStartPosition(1);
        Vector2 end = screenCenter;
        transform.localPosition = start;


        yield return null;
        float animTime = GamePanel.animTime / 2f;
        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime / 2f);

        if (panelObjects != null)
        {

            for (int i = 0; i < panelObjects.Count; i++)
            {

                RectTransform objRect = panelObjects[i].transform as RectTransform;

                objRect.localPosition = new Vector3(start.x, objRect.localPosition.y, objRect.localPosition.z);

                panelObjects[i].gameObject.SetActive(true);
                panelObjects[i].transform.MoveToLocal(new Vector3(end.x, objRect.localPosition.y, objRect.localPosition.z), animTime);
                yield return new WaitForSeconds(animTime / 2f);
            }
        }

        yield return new WaitForSeconds(animTime / 2f);

        moving = false;
    }

    public virtual IEnumerator Exit()
    {
        if (moving)
            yield break;

        moving = true;

        Vector2 start = screenCenter;
        Vector2 end = DefaultStartPosition(-1);
        transform.localPosition = start;
        float animTime = GamePanel.animTime / 2f;
        if (panelObjects != null)
            for (int i = panelObjects.Count - 1; i >= 0; i--)
            {

                RectTransform objRect = panelObjects[i].transform as RectTransform;

                objRect.localPosition = new Vector3(start.x, objRect.localPosition.y, objRect.localPosition.z);

                panelObjects[i].gameObject.SetActive(true);
                panelObjects[i].transform.MoveToLocal(new Vector3(end.x, objRect.localPosition.y, objRect.localPosition.z), animTime);
                yield return new WaitForSeconds(animTime / 2f);
            }



        transform.MoveToLocal(end, animTime);
        yield return new WaitForSeconds(animTime);

        gameObject.SetActive(false);
        moving = false;
    }

    public virtual void OnBack()
    {
        if (onBackPanel && !moving)
        {
            SceneController.instance.ChangePanel(onBackPanel);
        }
    }

}
