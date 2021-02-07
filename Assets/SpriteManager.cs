using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    public List<RectTransform> imagesRT;
    List<Image> images;

    private void Awake()
    {
        images = new List<Image>();
        for (int i = 0; i < imagesRT.Count; i++)
        {
            images.Add(imagesRT[i].GetComponent<Image>());
        }
    }

    public void Set(Sprite s, int id)
    {
        images[id].gameObject.SetActive(true);
        images[id].sprite = s;
    }

    public void Hide(int id)
    {
        images[id].gameObject.SetActive(false);
    }
}
