using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorize : MonoBehaviour
{
    public static Colorize Instance { get; private set; }

    public Tile _tile;

    private List<Color> _colorActive;        // list of color themes

    // Start is called before the first frame update
    void Start()
    {
        if ( null == Instance )
            Instance = this;
        //_colorActive = ColorTheme.SetTheme ();
        //GetComponent<SpriteRenderer>().color = GetColorForTile(_tile.tilePoints);
    }

    public  void SetColor()
    {
        _colorActive = ColorTheme.SetTheme();
    }

    public Color GetColorForTile(float tilePoint)
    {
        /*Check if Rainbow power is active*/
        /*then */

        //Color newColor = colors[(Random.Range(0, 5))];

        float _alpha = 1f;

        // default
        switch (tilePoint)
        {
            case 2f:        // level 1
                return new Color(_colorActive[0].r, _colorActive[0].g, _colorActive[0].b, _alpha);
            case 4f:        // level 2
                return new Color(_colorActive[1].r, _colorActive[1].g, _colorActive[1].b, _alpha);
            case 8f:        // level 3
                return new Color(_colorActive[2].r, _colorActive[2].g, _colorActive[2].b, _alpha);
            case 16f:       // level 4
                return new Color(_colorActive[3].r, _colorActive[3].g, _colorActive[3].b, _alpha);
            case 32f:       // level 5
                return new Color(_colorActive[4].r, _colorActive[4].g, _colorActive[4].b, _alpha);
            case 64f:       // level 6
                return new Color(_colorActive[5].r, _colorActive[5].g, _colorActive[5].b, _alpha);
            case 128f:      // level 7
                return new Color(_colorActive[6].r, _colorActive[6].g, _colorActive[6].b, _alpha);
            case 256f:      // level 8
                return new Color(_colorActive[7].r, _colorActive[7].g, _colorActive[7].b, _alpha);
            case 512f:      // level 9
                return new Color(_colorActive[8].r, _colorActive[8].g, _colorActive[8].b, _alpha);
            case 1024f:     // level 10
                return new Color(_colorActive[9].r, _colorActive[9].g, _colorActive[9].b, _alpha);
            case 2048f:     // level 11
                return new Color(_colorActive[10].r, _colorActive[10].g, _colorActive[10].b, _alpha);
            case 4096f:     // level 12
                return new Color(_colorActive[11].r, _colorActive[11].g, _colorActive[11].b, _alpha);
            case 8192f:     // level 13
                return new Color(_colorActive[12].r, _colorActive[12].g, _colorActive[12].b, _alpha);
            case 16384:     // level 14
                return new Color(_colorActive[13].r, _colorActive[13].g, _colorActive[13].b, _alpha);
            case 32768:     // level 15
                return new Color(_colorActive[14].r, _colorActive[14].g, _colorActive[14].b, _alpha);
            case 65536:     // level 16
                return new Color(_colorActive[15].r, _colorActive[15].g, _colorActive[15].b, _alpha);
            case 131072:    // level 17
                return new Color(_colorActive[16].r, _colorActive[16].g, _colorActive[16].b, _alpha);
            case 262144:    // level 18
                return new Color(_colorActive[17].r, _colorActive[17].g, _colorActive[17].b, _alpha);
            case 524288:    // level 19
                return new Color(_colorActive[18].r, _colorActive[18].g, _colorActive[18].b, _alpha);
            case 1048576:   // level 20
                return new Color(_colorActive[19].r, _colorActive[19].g, _colorActive[19].b, _alpha);
            case 2097152:   // level 21
                return new Color(_colorActive[20].r, _colorActive[20].g, _colorActive[20].b, _alpha);
            case 4194304:   // level 22
                return new Color(_colorActive[20].r, _colorActive[20].g, _colorActive[20].b, _alpha);
            case 8388608:   // level 23
                return new Color(_colorActive[20].r, _colorActive[20].g, _colorActive[20].b, _alpha);
            default:
                return Color.clear;
        }
    }
}
