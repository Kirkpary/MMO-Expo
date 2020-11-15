using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FpsCounter : MonoBehaviour
{
    public Canvas canvas;
    [Tooltip("Set the maximum frames per second. Set to -1 for unlimited.")]
    public int maxFPS = -1;
    string label = "";
    float count;
    int width = 100;
    int height = 25;

    IEnumerator Start()
    {
        Application.targetFrameRate = maxFPS;
        GUI.depth = 2;
        while (true)
        {
            if (Time.timeScale == 1)
            {
                yield return new WaitForSeconds(0.1f);
                count = (1 / Time.deltaTime);
                label = "FPS: " + (Mathf.Round(count));
            }
            else
            {
                label = "Pause";
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, width, height), label);
    }
}