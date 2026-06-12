using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ArtistUI : MonoBehaviour
{
    [SerializeField] private GameObject artistPrefab;
    [SerializeField] private Transform contentPanel;

    public void PopulateArtists(List<Artist> unlockedArtists)
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (Artist artist in unlockedArtists)
        {
            GameObject artistUI = Instantiate(artistPrefab, contentPanel);

            artistUI.transform.Find("ArtistImage").GetComponent<Image>().sprite = artist.ArtistImage;
            artistUI.transform.Find("ArtistName").GetComponent<TMP_Text>().text = artist.ArtistName;
            artistUI.transform.Find("Fame").GetComponent<TMP_Text>().text = $"Fame: {artist.Fame}";
            artistUI.transform.Find("Energy").GetComponent<TMP_Text>().text = $"Energy: {artist.Energy.ToString("F2")}";

            Button artistButton = artistUI.transform.Find("ArtistDetail").GetComponent<Button>();
            artistButton.onClick.AddListener(() => OnArtistSelected(artist));
        }
    }
    
    private void OnArtistSelected(Artist selectedArtist)
    {
        DataCarrier.Instance.SetSelectedArtist(selectedArtist);
    }
}