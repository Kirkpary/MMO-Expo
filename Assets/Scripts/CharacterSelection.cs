using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    // All the characters options goes into this list
    private List<GameObject> characters = new List<GameObject>();
    private int selectedCharacter = 0;
    private string gender = "Men";

    private void Start()
    {
        var temp = Resources.LoadAll(gender, typeof(GameObject)).Cast<GameObject>();
        foreach (GameObject character in temp)
        {
            print(character);
            GameObject newPlayer = Instantiate(character, gameObject.transform);
            newPlayer.name = character.name;
            newPlayer.tag = "Loaded";
            newPlayer.SetActive(false);
            characters.Add(newPlayer);
        }
        characters[selectedCharacter].SetActive(true);
    }

    //void DestroyWithTag(string destroyTag)
    //{
        //GameObject[] destroyObject = GameObject.FindGameObjectsWithTag(destroyTag);
        //Debug.LogFormat("Count of destroyObject: {0}", destroyObject.Length);

        //foreach (GameObject oneObject in destroyObject)
        //{
        //    Debug.LogFormat("Removing gameObject: {0}", oneObject);
        //    characters.Remove(oneObject);
        //    Destroy(oneObject);
        //}

        //foreach (GameObject oneObject in characters)
        //{
        //    if (oneObject.tag == destroyTag)
        //    {
        //        characters.Remove(oneObject);
        //        Destroy(oneObject);
        //    }
        //}
    //}

    // This function should be callen only when the gender button is clicked.
    // It will unload the previously loaded prefabs
    // Then load the selected gender prefabs
    private void ChangeGender()
    {

        // DestroyWithTag("Loaded");

        GameObject[] destroyObject = GameObject.FindGameObjectsWithTag("Loaded");
        Debug.LogFormat("Count of destroyObject: {0}", destroyObject.Length);

        selectedCharacter = characters.Count;

        var temp = Resources.LoadAll(gender, typeof(GameObject)).Cast<GameObject>();
        foreach (GameObject character in temp)
        {
            print(character);
            GameObject newPlayer = Instantiate(character, gameObject.transform);
            newPlayer.name = character.name;
            newPlayer.tag = "Loaded";
            newPlayer.SetActive(false);
            characters.Add(newPlayer);
        }
        Debug.LogFormat("The selectedChar is: {0} and characters.Count is: {1}", selectedCharacter, characters.Count);
        characters[selectedCharacter].SetActive(true);

        foreach (GameObject oneObject in destroyObject)
        {
            Debug.LogFormat("Removing gameObject: {0}", oneObject);
            characters.Remove(oneObject);
            Destroy(oneObject);
        }
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Count;
        Debug.LogFormat("The selectedChar is: {0} and characters.Count is: {1}", selectedCharacter, characters.Count);
        characters[selectedCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Count;
        }
        Debug.LogFormat("The selectedChar is: {0} and characters.Count is: {1}", selectedCharacter, characters.Count);
        characters[selectedCharacter].SetActive(true);
    }

    public void MenCharacter()
    {
        string tmpGender = gender;
        gender = "Men";
        if (tmpGender != gender)
        {
            ChangeGender();
        }
    }

    public void WomenCharacter()
    {
        string tmpGender = gender;
        gender = "Women";
        if (tmpGender != gender)
        {
            ChangeGender();
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetString("selectedCharacter", characters[selectedCharacter].name);
        Debug.LogFormat("The selectedCharacter int is {0}", PlayerPrefs.GetString("selectedCharacter"));
    }
}
