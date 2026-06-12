using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplaySongsUI : MonoBehaviour
{

    [SerializeField] private GameObject songPrefab;
    [SerializeField] private Transform songcontentPanel;
    [SerializeField] private TMP_Text songCount;

    public void PopulateSongs(Artist artist, bool selectionAvailable)
    {
        foreach (Song song in artist.Songs)
        {
            GameObject songUI = Instantiate(songPrefab, songcontentPanel);

            songUI.transform.Find("SongImage").GetComponent<Image>().sprite = song.SongImage;

            songUI.transform.Find("SongName").GetComponent<TMP_Text>().text = song.SongName;

            songUI.transform.Find("SongGenre").GetComponent<TMP_Text>().text = song.SongGenre.ToString();

            if (selectionAvailable)
            {
                AddSongSelectionHandler(songUI, song);
            }
        }
    }
    
    private void AddSongSelectionHandler(GameObject songUI, Song song)
    {
        Button btn = songUI.GetComponent<Button>();

        if (btn == null)
        {
            btn = songUI.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
        }

        btn.onClick.AddListener(() => 
        {
            if (DataCarrier.Instance.SelectedSongs.Contains(song))
            {
                ToggleSongSelection(btn, song);
                return;
            }

            if (DataCarrier.Instance.SelectedSongs.Count >= 6)
            {
                Debug.Log("Maximum songs (6) selected");
                return;
            }

            ToggleSongSelection(btn, song);
        });
    }

    private void ToggleSongSelection(Button btn, Song song)
    {
        bool wasSelected = DataCarrier.Instance.SelectedSongs.Contains(song);

        if (wasSelected)
        {
            DataCarrier.Instance.SelectedSongs.Remove(song);
        }
        else
        {
            DataCarrier.Instance.SelectedSongs.Add(song);
            Debug.Log(song.SongName);
        }

        songCount.text = $"({DataCarrier.Instance.SelectedSongs.Count}/6)";

        bool isSelected = DataCarrier.Instance.SelectedSongs.Contains(song);
        btn.image.color = isSelected ? new Color(0.851f, 0.788f, 0.788f) : Color.white;
    } 
}