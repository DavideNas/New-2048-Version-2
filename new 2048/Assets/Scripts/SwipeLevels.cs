using UnityEngine;
using UnityEngine.UI;

public class SwipeLevels : MonoBehaviour
{
    public static SwipeLevels Instance { get; private set; }
    public static bool RightDirection = false;
    public static bool LeftDirection = false;
    public Color[] colors;
    public GameObject scrollbar, imageContent;
    float scroll_pos = 0;
    public static float[] pos;
    float distanceToMove = 0.2f;

    // Update is called once per frame
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        if (RightDirection)
        {
            // script to move next
            // read button selected from actual position
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            // add 1/numbers of tabs
            scroll_pos += distanceToMove;
            for (int i = 0; i < pos.Length - 1; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
            RightDirection = false;
        }

        if (LeftDirection)
        {
            // script to move prev
            // read button selected from actual positions
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            // sub 1/numbers of tabs
            scroll_pos -= distanceToMove;
            for (int i = 1; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
            LeftDirection = false;
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                //Debug.LogWarning("Current Selected Level " + i);
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }
}