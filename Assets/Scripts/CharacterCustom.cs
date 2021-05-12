using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustom : MonoBehaviour
{
    private GameObject characterBody;
    private GameObject characterHead;
    public Texture[] skins;
    private int selectedSkin = 0;
    // Start is called before the first frame update
    void Start()
    {
        characterBody = this.gameObject;
        characterHead = GameObject.Find("human_head_mesh");
        Debug.Log("Current skins size is:" + skins.Length);
    }

    public void nextSkinTexture()
    {
        Debug.Log("Next skin was pressed");
        selectedSkin = (selectedSkin + 1) % skins.Length;
        SkinnedMeshRenderer bodyRenderer = characterBody.GetComponentInChildren<SkinnedMeshRenderer>();
        SkinnedMeshRenderer headRenderer = characterHead.GetComponentInChildren<SkinnedMeshRenderer>();
        bodyRenderer.materials[0].mainTexture = skins[selectedSkin];
        headRenderer.materials[0].mainTexture = skins[selectedSkin];
    }

    public void prevSkinTexture()
    {
        Debug.Log("Prev skin was pressed");
        selectedSkin--;
        if (selectedSkin < 0)
        {
            selectedSkin += skins.Length;
        }
        //Debug.Log("Current selectedSkin is:" + selectedSkin);
        SkinnedMeshRenderer bodyRenderer = characterBody.GetComponentInChildren<SkinnedMeshRenderer>();
        SkinnedMeshRenderer headRenderer = characterHead.GetComponentInChildren<SkinnedMeshRenderer>();
        bodyRenderer.materials[0].mainTexture = skins[selectedSkin];
        headRenderer.materials[0].mainTexture = skins[selectedSkin];
    }
}
