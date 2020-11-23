using UnityEngine;
using System.Collections.Generic;

public class CharacterSelection : MonoBehaviour
{
    // All the characters options goes into this list
    private List<GameObject> characters = new List<GameObject>();
    private int selectedCharacter = 0;

    private void Start()
    {
        Object[] temp = Resources.LoadAll("Avatars", typeof(GameObject));
        Debug.LogFormat("Avatars: {0}", temp.Length);
        foreach (GameObject character in temp)
        {
            GameObject newPlayer = Instantiate(character, gameObject.transform);
            newPlayer.name = character.name;
            newPlayer.SetActive(false);
            characters.Add(newPlayer);
        }
        if (temp.Length > 0)
        {
            characters[selectedCharacter].SetActive(true);
        }
        else
        {
            Debug.LogError("<Color=red><a>Missing</a></Color> couldn't load playerPrefabs from Avatars folder. Please make sure there are prefabs in the 'Assets/Resources/Avatars' folder.", this);
        }
    }

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Count;
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
        characters[selectedCharacter].SetActive(true);
    }

    public void StartGame()
    {
        PlayerPrefs.SetString("selectedCharacter", characters[selectedCharacter].name);
        Debug.LogFormat("The selectedCharacter int is {0}", PlayerPrefs.GetString("selectedCharacter"));
    }
}
