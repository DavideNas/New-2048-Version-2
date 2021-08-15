using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTheme
{
    public static List<Color> _colorTheme = new List<Color>();

    public static List<Color> SetTheme()
    {
        switch (ControlManager.Instance.ActiveTheme)
        {
            case "basic":
                _colorTheme.Add(new Color(0.85f, 0.44f, 1f, 1f));       // 2
                _colorTheme.Add(new Color(0.76f, 0.08f, 0.79f, 1f));    // 4
                _colorTheme.Add(new Color(0.44f, 0.12f, 0.52f, 1f));    // 8
                _colorTheme.Add(new Color(0.75f, 0.12f, 0.22f, 1f));    // 16
                _colorTheme.Add(new Color(0.90f, 0.16f, 0.22f, 1f));    // 32
                _colorTheme.Add(new Color(0.98f, 0.07f, 0.22f, 1f));    // 64
                _colorTheme.Add(new Color(0.75f, 0.12f, 0.22f, 1f));    // 128
                _colorTheme.Add(new Color(1f, 0.29f, 0f, 1f));          // 256
                _colorTheme.Add(new Color(1f, 0.45f, 0.16f, 1f));       // 512
                _colorTheme.Add(new Color(1f, 0.64f, 0f, 1f));          // 1024
                _colorTheme.Add(new Color(1f, 0.81f, 0f, 1f));          // 2048
                _colorTheme.Add(new Color(0.99f, 0.90f, 0.02f, 1f));    // 4096
                _colorTheme.Add(new Color(0.51f, 0.94f, 0f, 1f));       // 8192
                _colorTheme.Add(new Color(0f, 0.74f, 0.29f, 1f));       // 16384
                _colorTheme.Add(new Color(0f, 0.57f, 0.18f, 1f));       // 32768
                _colorTheme.Add(new Color(0f, 0.33f, 0.67f, 1f));       // 65536
                _colorTheme.Add(new Color(0f, 0.47f, 0.95f, 1f));       // 131072
                _colorTheme.Add(new Color(0f, 0.66f, 1f, 1f));          // 262144
                _colorTheme.Add(new Color(0.85f, 0.44f, 1f, 1f));       // 524288
                _colorTheme.Add(new Color(0.76f, 0.08f, 0.79f, 1f));    // 1048576
                _colorTheme.Add(new Color(0.44f, 0.12f, 0.52f, 1f));    // 2097152
                _colorTheme.Add(new Color(0.75f, 0.12f, 0.22f, 1f));    // 4194304
                _colorTheme.Add(new Color(0.90f, 0.16f, 0.22f, 1f));    // 8388608
                break;
            case "coral":
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                _colorTheme.Add(new Color(1f, 0.39f, 0.4f, 1f));
                break;
            case "metallic":
                _colorTheme.Add(new Color(0.89f, 0.86f, 0.78f, 1f));    // 2
                _colorTheme.Add(new Color(0.87f, 0.82f, 0.76f, 1f));    // 4
                _colorTheme.Add(new Color(0.78f, 0.74f, 0.71f, 1f));    // 8
                _colorTheme.Add(new Color(0.76f, 0.71f, 0.65f, 1f));    // 16
                _colorTheme.Add(new Color(0.71f, 0.66f, 0.60f, 1f));    // 32
                _colorTheme.Add(new Color(0.64f, 0.58f, 0.54f, 1f));    // 64
                _colorTheme.Add(new Color(0.59f, 0.54f, 0.48f, 1f));    // 128
                _colorTheme.Add(new Color(0.59f, 0.44f, 0.49f, 1f));    // 256
                _colorTheme.Add(new Color(0.43f, 0.37f, 0.32f, 1f));    // 512
                _colorTheme.Add(new Color(0.82f, 0.84f, 0.85f, 1f));    // 1024
                _colorTheme.Add(new Color(0.76f, 0.78f, 0.81f, 1f));    // 2048
                _colorTheme.Add(new Color(0.65f, 0.68f, 0.71f, 1f));    // 4096
                _colorTheme.Add(new Color(0.92f, 0.89f, 0.85f, 1f));    // 8192
                _colorTheme.Add(new Color(0.86f, 0.85f, 0.80f, 1f));    // 16384
                _colorTheme.Add(new Color(0.82f, 0.81f, 0.76f, 1f));    // 32768
                _colorTheme.Add(new Color(0.77f, 0.76f, 0.73f, 1f));    // 65536
                _colorTheme.Add(new Color(0.73f, 0.71f, 0.69f, 1f));    // 131072
                _colorTheme.Add(new Color(0.71f, 0.70f, 0.67f, 1f));    // 262144
                _colorTheme.Add(new Color(0.64f, 0.64f, 0.62f, 1f));    // 524288
                _colorTheme.Add(new Color(0.61f, 0.60f, 0.58f, 1f));    // 1048576
                _colorTheme.Add(new Color(0.55f, 0.54f, 0.52f, 1f));    // 2097152
                _colorTheme.Add(new Color(0.47f, 0.47f, 0.45f, 1f));    // 4194304
                _colorTheme.Add(new Color(0.41f, 0.40f, 0.39f, 1f));    // 8388608
                break;
            case "asian":
                _colorTheme.Add(new Color(0.93f, 0.87f, 0.80f, 1f));    // 2
                _colorTheme.Add(new Color(0.73f, 0.60f, 0.60f, 1f));    // 4
                _colorTheme.Add(new Color(0.80f, 0.67f, 0.73f, 1f));    // 8
                _colorTheme.Add(new Color(0.47f, 0.33f, 0.40f, 1f));    // 16
                _colorTheme.Add(new Color(0.33f, 0.20f, 0.20f, 1f));    // 32
                _colorTheme.Add(new Color(0.27f, 0.13f, 0.20f, 1f));    // 64
                _colorTheme.Add(new Color(0.13f, 0.20f, 0.26f, 1f));    // 128
                _colorTheme.Add(new Color(0.47f, 0.47f, 0.47f, 1f));    // 256
                _colorTheme.Add(new Color(0.73f, 0.80f, 0.67f, 1f));    // 512
                _colorTheme.Add(new Color(0.53f, 0.60f, 0.47f, 1f));    // 1024
                _colorTheme.Add(new Color(0.27f, 0.40f, 0.33f, 1f));    // 2048
                _colorTheme.Add(new Color(0.27f, 0.33f, 0.20f, 1f));    // 4096
                _colorTheme.Add(new Color(0.53f, 0.40f, 0.27f, 1f));    // 8192
                _colorTheme.Add(new Color(0.80f, 0.67f, 0.41f, 1f));    // 16384
                _colorTheme.Add(new Color(1f, 0.87f, 0.54f, 1f));       // 32768
                _colorTheme.Add(new Color(0.80f, 0.47f, 0.20f, 1f));    // 65536
                _colorTheme.Add(new Color(0.42f, 0.23f, 0.08f, 1f));    // 131072
                _colorTheme.Add(new Color(0f, 0f, 0, 1f));              // 262144
                _colorTheme.Add(new Color(0.27f, 0.13f, 0.20f, 1f));    // 524288
                _colorTheme.Add(new Color(0.47f, 0.33f, 0.40f, 1f));    // 1048576
                _colorTheme.Add(new Color(0.80f, 0.67f, 0.73f, 1f));    // 2097152
                _colorTheme.Add(new Color(0.73f, 0.60f, 0.60f, 1f));    // 4194304
                _colorTheme.Add(new Color(0.93f, 0.87f, 0.80f, 1f));    // 8388608
                break;
            case "basic-old":
                _colorTheme.Add(new Color(0.36f, 0.98f, 0.66f, 1f));    // 2
                _colorTheme.Add(new Color(0.4f, 0.6f, 0.8f, 1f));       // 4
                _colorTheme.Add(new Color(1f, 0.6f, 1f, 1f));           // 8
                _colorTheme.Add(new Color(1f, 0.4f, 0.6f, 1f));         // 16
                _colorTheme.Add(new Color(0.4f, 1f, 0.6f, 1f));         // 32
                _colorTheme.Add(new Color(0f, 0.8f, 1f, 1f));           // 64
                _colorTheme.Add(new Color(0.8f, 0.6f, 1f, 1f));         // 128
                _colorTheme.Add(new Color(0.8f, 1f, 0.6f, 1f));         // 256
                _colorTheme.Add(new Color(0.6f, 0.6f, 0.6f, 1f));       // 512
                _colorTheme.Add(new Color(1f, 0.66f, 0.15f, 1f));       // 1024
                _colorTheme.Add(new Color(0.77f, 0.74f, 0.45f));        // 2048
                _colorTheme.Add(new Color(1f, 0.61f, 0.66f, 1f));       // 4096
                _colorTheme.Add(new Color(0.86f, 0.52f, 0.57f, 1f));    // 8192
                _colorTheme.Add(new Color(0.36f, 0.98f, 0.66f, 0.5f));  // 16384
                _colorTheme.Add(new Color(0.4f, 0.6f, 0.8f, 0.5f));     // 32768
                _colorTheme.Add(new Color(1f, 0.6f, 1f, 0.5f));         // 65536
                _colorTheme.Add(new Color(1f, 0.4f, 0.6f, 0.5f));       // 131072
                _colorTheme.Add(new Color(0.4f, 1f, 0.6f, 0.5f));       // 262144
                _colorTheme.Add(new Color(0f, 0.8f, 1f, 0.5f));         // 524288
                _colorTheme.Add(new Color(0.8f, 0.6f, 1f, 0.5f));       // 1048576
                _colorTheme.Add(new Color(0.8f, 1f, 0.6f, 0.5f));       // 2097152
                _colorTheme.Add(new Color(0.6f, 0.6f, 0.6f, 0.5f));     // 4194304
                _colorTheme.Add(new Color(1f, 0.66f, 0.15f, 0.5f));     // 8388608
                break;
            case "pastel":
                _colorTheme.Add(new Color(0.42f, 0.79f, 0.86f, 1f));    // 2
                _colorTheme.Add(new Color(0.66f, 0.96f, 0.81f, 1f));    // 4
                _colorTheme.Add(new Color(0.56f, 0.80f, 0.60f, 1f));    // 8
                _colorTheme.Add(new Color(0.98f, 0.88f, 0.52f, 1f));    // 16
                _colorTheme.Add(new Color(0.97f, 0.73f, 0.52f, 1f));    // 32
                _colorTheme.Add(new Color(0.96f, 0.60f, 0.60f, 1f));    // 64
                _colorTheme.Add(new Color(0.57f, 0.62f, 0.80f, 1f));    // 128
                _colorTheme.Add(new Color(0.84f, 0.61f, 0.54f, 1f));    // 256
                _colorTheme.Add(new Color(0.88f, 0.45f, 0.51f, 1f));    // 512
                _colorTheme.Add(new Color(0.87f, 0.52f, 0.72f, 1f));    // 1024
                _colorTheme.Add(new Color(0.45f, 0.65f, 0.86f, 1f));    // 2048
                _colorTheme.Add(new Color(0.71f, 0.56f, 0.79f, 1f));    // 4096
                _colorTheme.Add(new Color(0.45f, 0.65f, 0.86f, 1f));    // 8192
                _colorTheme.Add(new Color(0.53f, 0.47f, 0.73f, 1f));    // 16384
                _colorTheme.Add(new Color(0.48f, 0.53f, 0.77f, 1f));    // 32768
                _colorTheme.Add(new Color(0.60f, 0.45f, 0.70f, 1f));    // 65536
                _colorTheme.Add(new Color(0.51f, 0.74f, 0.39f, 1f));    // 131072
                _colorTheme.Add(new Color(0.87f, 0.46f, 0.51f, 1f));    // 262144
                _colorTheme.Add(new Color(0.88f, 0.50f, 0.45f, 1f));    // 524288
                _colorTheme.Add(new Color(0.83f, 0.74f, 0.36f, 1f));    // 1048576
                _colorTheme.Add(new Color(0.70f, 0.69f, 0.35f, 0.5f));  // 2097152
                _colorTheme.Add(new Color(0.62f, 0.46f, 0.84f, 0.5f));  // 4194304
                _colorTheme.Add(new Color(0.75f, 0.49f, 0.72f, 1f));    // 8388608
                break;
            case "2048":
                _colorTheme.Add(new Color(0.93f, 0.89f, 0.86f, 1f));    // 2
                _colorTheme.Add(new Color(0.93f, 0.88f, 0.79f, 1f));    // 4
                _colorTheme.Add(new Color(0.95f, 0.69f, 0.47f, 1f));    // 8                
                _colorTheme.Add(new Color(0.95f, 0.58f, 0.37f, 1f));    // 16
                _colorTheme.Add(new Color(0.96f, 0.49f, 0.37f, 1f));    // 32
                _colorTheme.Add(new Color(0.97f, 0.37f, 0.24f, 1f));    // 64
                _colorTheme.Add(new Color(0.93f, 0.81f, 0.45f, 1f));    // 128
                _colorTheme.Add(new Color(0.93f, 0.80f, 0.38f, 1f));    // 256
                _colorTheme.Add(new Color(0.92f, 0.78f, 0.31f, 1f));    // 512
                _colorTheme.Add(new Color(0.93f, 0.76f, 0.28f, 1f));    // 1024
                _colorTheme.Add(new Color(0.93f, 0.76f, 0.18f, 1f));    // 2048
                _colorTheme.Add(new Color(0.95f, 0.40f, 0.43f, 1f));    // 4096
                _colorTheme.Add(new Color(0.93f, 0.30f, 0.36f, 1f));    // 8192
                _colorTheme.Add(new Color(0.96f, 0.25f, 0.25f, 1f));    // 16384
                _colorTheme.Add(new Color(0.44f, 0.71f, 0.82f, 1f));    // 32768
                _colorTheme.Add(new Color(0.37f, 0.62f, 0.87f, 1f));    // 65536
                _colorTheme.Add(new Color(0.09f, 0.51f, 0.80f, 1f));    // 131072
                _colorTheme.Add(new Color(0.13f, 0.73f, 0.39f, 1f));    // 262144
                _colorTheme.Add(new Color(0.15f, 0.55f, 0.32f, 1f));    // 524288
                _colorTheme.Add(new Color(0.17f, 0.47f, 0.27f, 1f));    // 1048576
                _colorTheme.Add(new Color(0.24f, 0.23f, 0.20f, 0.5f));  // 2097152
                _colorTheme.Add(new Color(0.24f, 0.23f, 0.20f, 0.5f));  // 4194304
                _colorTheme.Add(new Color(0.24f, 0.23f, 0.20f, 1f));    // 8388608
                break;
        }

        return _colorTheme;
    }
}
