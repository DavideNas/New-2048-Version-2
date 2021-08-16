using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public float tilePoints = -1f;
    public Vector3 targetPosition;
    public float speed = 0;

    public bool fixedTile = false;

    public GameObject getCoin;

    public bool coinTile = false;

    public GameObject cube, cube2, cube3, cube4, cube5, basePlane;

    public TextMeshPro tileValue;

    private float velocity = GameControlManager.Instance.velocity;

    private Vector3 initTileScale = new Vector3(0.8f,0.8f,1f);
    private Vector3 targetScale = new Vector3(1f,1f,1f);
    private Vector3 evolutionScale = new Vector3(1.1f, 1.1f, 1f);

    private bool isMoving = false;
    //private bool isScaling = false;
    public bool isAnimate = false;

    public float speedScale = 55f;
    private float startTime;
    public bool evolvedTile = false;

    void Start()
    {
        startTime = Time.time;

        if (!evolvedTile)
        {
            transform.localScale = initTileScale;
        }
        else
        {
            transform.localScale = evolutionScale;
        }

        if (GameControlManager.Instance.LuckyTile())
        {
            coinTile = true;
            getCoin.SetActive(true);
        }
        else
        {
            coinTile = false;
            getCoin.SetActive(false);
        }
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speedScale;
        if (transform.localScale.x != targetScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, distCovered);
            //isScaling = true;
        }
        else
        {
            //isScaling = false;
        }

        if ((transform.position != targetPosition) && (!fixedTile))
        {
            float step = (velocity * Time.deltaTime) * speed;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)// || isScaling)
        {
            isAnimate = true;
        }
        else
        {
            isAnimate = false;
        }
    }

    private void OnDestroy()
    {
        if(coinTile)
        {
            //Utils.coins ++;
        }
    }

    public void SetColor(float value)
    {
        Color tileColor;
        tileColor = Colorize.Instance.GetColorForTile(value);
        var cubeRenderer = cube.GetComponent<Renderer>();
        cubeRenderer.material.SetColor("_Color", tileColor);

        if ("neon" == ControlManager.Instance.TileSelect)
        {
            var cubeRenderer2 = cube2.GetComponent<Renderer>();
            cubeRenderer2.material.SetColor("_Color", tileColor);

            var cubeRenderer3 = cube3.GetComponent<Renderer>();
            cubeRenderer3.material.SetColor("_Color", tileColor);

            var cubeRenderer4 = cube4.GetComponent<Renderer>();
            cubeRenderer4.material.SetColor("_Color", tileColor);

            var cubeRenderer5 = cube5.GetComponent<Renderer>();
            cubeRenderer5.material.SetColor("_Color", tileColor);

            var planeRenderer = basePlane.GetComponent<Renderer>();
            planeRenderer.material.SetColor("_Color", tileColor);
        }
    }
}