using UnityEngine;
using UnityEngine.Playables;

public class BackgroundScroll : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float scrollSpeed = 0.5f;
    public Material mat;
    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset += new Vector2(offset, 0);
    }
}
 