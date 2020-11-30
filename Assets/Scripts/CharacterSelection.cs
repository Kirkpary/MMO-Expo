using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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

    void DestroyWithTag(string destroyTag)
    {
        GameObject[] destroyObject;
        destroyObject = GameObject.FindGameObjectsWithTag(destroyTag);
        foreach (GameObject oneObject in destroyObject)
        {
            Debug.LogFormat("Removing gameObject: {0}", oneObject);
            characters.Remove(oneObject);
        }
    }

    // This function should be callen only when the gender button is clicked.
    // It will unload the previously loaded prefabs
    // Then load the selected gender prefabs
    private void ChangeGender()
    {
        List<string> charNames = new List<string>();
        // Get the prefab folder directory opposite to the selected gender
        string directory = "Assets\\Resources\\";
        string tempGender = "Men";
        
        if (gender == "Men")
        {
            tempGender = "Women";
        }

        string path = directory + tempGender;

        //Get the names of the files in the directory
        //foreach (string file in Directory.GetFiles(path, "*.prefab"))
        //{
        //    string tmpStr1;
        //    string tmpStr2;

        //    // Trim the string to be only the file name,
        //    tmpStr1 = file.Remove(0, 24);
        //    tmpStr2 = tmpStr1.Remove(tmpStr1.IndexOf("."));

        //    Debug.LogFormat("Pringint trimmed filename: {0}", tmpStr2);
        //    charNames.Add(file);
        //}

        DestroyWithTag("Loaded");

        var temp = Resources.LoadAll(gender, typeof(GameObject)).Cast<GameObject>();
        foreach (GameObject character in temp)
        {
            print(character);
            GameObject newPlayer = Instantiate(character, gameObject.transform);
            newPlayer.name = character.name;

            newPlayer.SetActive(false);
            characters.Add(newPlayer);
        }
        selectedCharacter = 0;
        Debug.LogFormat("The selectedChar is: {0} and characters.Count is: {1}", selectedCharacter, characters.Count);
        characters[selectedCharacter].SetActive(true);
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
        gender = "Men";
        ChangeGender();
    }

    public void WomenCharacter()
    {
        gender = "Women";
        ChangeGender();
    }

    public void StartGame()
    {
        PlayerPrefs.SetString("selectedCharacter", characters[selectedCharacter].name);
        Debug.LogFormat("The selectedCharacter int is {0}", PlayerPrefs.GetString("selectedCharacter"));
    }
}
