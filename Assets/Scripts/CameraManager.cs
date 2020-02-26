using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SplitScreen
{
    Full,
    Vertical,
    Horizontal,
    Quarters
};

public class CameraManager : MonoBehaviour
{
    private class CameraPlayer
    {
        public CameraController cameraController;
        public Car car;

        public CameraPlayer(CameraController cc, Car c)
        {
            cameraController = cc;
            car = c;
        }
    }

    public SplitScreen screenType;

    [SerializeField] private List<CameraPlayer> cameraPlayers = new List<CameraPlayer>();

    private void Awake()
    {
        var camControllers = FindObjectsOfType<CameraController>();

        foreach(CameraController cc in camControllers)
        {
            if (cc.gameObject.activeInHierarchy)
            {
                bool added = false;

                var car = cc.GetComponent<Car>();
                int playerNum = car.controls.playerNum;
                for (int i = 0; i < cameraPlayers.Count; i++)
                {
                    if (playerNum < cameraPlayers[i].car.controls.playerNum)
                    {
                        cameraPlayers.Insert(i, new CameraPlayer(cc, car));
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    cameraPlayers.Add(new CameraPlayer(cc, car));
                }
            }
        }

        UpdateCamera();
    }

    void UpdateCamera()
    {
        switch (screenType)
        {
            case SplitScreen.Full:
                {
                    cameraPlayers[0].cameraController.camera.rect = new Rect(0, 0, 1, 1);
                    break;
                }
            case SplitScreen.Vertical:
                {
                    int camCount = cameraPlayers.Count;
                    for (int i = 0; i < camCount; i++)
                    {
                        cameraPlayers[i].cameraController.camera.rect = new Rect((float)i / camCount, 0, 1f / camCount, 1);
                    }
                    break;
                }
            case SplitScreen.Horizontal:
                {
                    int camCount = cameraPlayers.Count;
                    for (int i = 0; i < camCount; i++)
                    {
                        cameraPlayers[i].cameraController.camera.rect = new Rect(0, (float)(camCount - i - 1) / camCount, 1, 1f / camCount);
                    }
                    break;
                }
            case SplitScreen.Quarters:
                {
                    for (int i = 0; i < cameraPlayers.Count; i++)
                    {
                        cameraPlayers[i].cameraController.camera.rect = i == 0 ? new Rect(0, .5f, .5f, .5f) : i == 1 ? new Rect(.5f, .5f, .5f, .5f) : i == 2 ? new Rect(0, 0, .5f, .5f) : i == 3 ? new Rect(.5f, 0, .5f, .5f) : new Rect(0, 0, 0, 0);
                    }
                    break;
                }
                /*
            case View.HalfBottom: camera.rect = new Rect(0, 0, 1, .5f); break;
            case View.HalfLeft: camera.rect = new Rect(0, 0, .5f, 1); break;
            case View.HalfRight: camera.rect = new Rect(.5f, 0, .5f, 1); break;

            case View.QuarterTopLeft: camera.rect = new Rect(0, .5f, .5f, .5f); break;
            case View.QuarterTopRight: camera.rect = new Rect(.5f, .5f, .5f, .5f); break;
            case View.QuarterBottomLeft: camera.rect = new Rect(0, 0, .5f, .5f); break;
            case View.QuarterBottomRight: camera.rect = new Rect(.5f, 0, .5f, .5f); break;
            */
        }

    }
}
